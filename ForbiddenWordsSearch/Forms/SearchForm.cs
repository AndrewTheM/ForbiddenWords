using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ForbiddenWordsSearch
{
    public partial class SearchForm : Form
    {
        private readonly Dictionary<string, string> states =
            new Dictionary<string, string>()
            {
                ["Init"] = "Initializing the search...",
                ["Search"] = "Searching through the files...",
                ["Copy"] = "Copying the files...",
                ["Censor"] = "Censoring the files...",
                ["Report"] = "Generating the report..."
            };

        private readonly Form parent;

        private readonly string path;

        private readonly Thread thread;

        private readonly List<ForbiddenWord> words;

        private readonly ManualResetEvent resetEvent;

        private bool isThreadPaused;

        public SearchForm(Form parent, ICollection<string> words, string path)
        {
            InitializeComponent();
            PrgBar.Style = ProgressBarStyle.Marquee;
            PrgBar.MarqueeAnimationSpeed = 2;

            this.parent = parent;
            this.words = words.Select(w => new ForbiddenWord { Word = w }).ToList();
            this.path = path;

            resetEvent = new ManualResetEvent(true);
            thread = new Thread(new ThreadStart(PerformSearch)) { IsBackground = true };
            thread.Start();
        }

        private delegate void ChangeStateDelegate(string key);

        private void ChangeState(string value)
        {
            if (this.InvokeRequired || LblState.InvokeRequired)
                Invoke(new ChangeStateDelegate(ChangeState), value);
            else
                LblState.Text = value;
        }

        private IEnumerable<string> GetDirectoryFiles(string path, string pattern, ICollection<string> filesCollector = null)
        {
            try
            {
                filesCollector ??= new LinkedList<string>();

                resetEvent.WaitOne();
                foreach (var file in Directory.GetFiles(path, pattern))
                    filesCollector.Add(file);

                foreach (var subdir in Directory.EnumerateDirectories(path))
                {
                    resetEvent.WaitOne();
                    GetDirectoryFiles(subdir, pattern, filesCollector);
                }

                return filesCollector;
            }
            catch (Exception)
            {
                return Enumerable.Empty<string>();
            }
        }

        private void PerformSearch()
        {
            string copiesPath = $"{path}/Forbidden Words/Copies",
                    censorPath = $"{path}/Forbidden Words/Censored";
            Directory.CreateDirectory(copiesPath);
            Directory.CreateDirectory(censorPath);

            var filePaths = ScanDrivesForTextFiles();
            var filesToCensor = SearchForForbiddenWords(filePaths);
            var filesInfo = CopyFilesToFolder(filesToCensor, copiesPath);
            CensorFiles(filesInfo, censorPath);
            GenerateReport(filesInfo, filePaths.Count);

            MessageBox.Show("The search is completed.\nThe report has been generated in the working folder.");

            Invoke(new Action(() => this.Close()));
        }

        private ICollection<string> ScanDrivesForTextFiles()
        {
            var filePaths = new List<string>();
            foreach (var drive in DriveInfo.GetDrives())
            {
                try
                {
                    ChangeState($"{states["Init"]} (Drive {drive.Name})");

                    var filesOnDrive = GetDirectoryFiles(drive.Name, "*.txt").ToList();
                    filePaths.AddRange(filesOnDrive);
                }
                catch (Exception) { }
            }
            return filePaths;
        }

        private ICollection<string> SearchForForbiddenWords(ICollection<string> filePaths)
        {
            int processedCount = 0, fileCount = filePaths.Count();
            var filesToCensor = new List<string>();

            Invoke(new Action(() =>
            {
                PrgBar.Style = ProgressBarStyle.Continuous;
                PrgBar.Maximum = fileCount;
            }));

            foreach (var file in filePaths)
            {
                try
                {
                    ChangeState($"{states["Search"]} ({processedCount}/{fileCount})");
                    resetEvent.WaitOne();

                    using var sr = new StreamReader(file, Encoding.Default);
                    var fileContent = sr.ReadToEnd();

                    foreach (var word in words)
                    {
                        resetEvent.WaitOne();
                        if (fileContent.Contains(word.Word))
                        {
                            filesToCensor.Add(file);
                            break;
                        }
                    }

                    Invoke(new Action(() => PrgBar.Value = ++processedCount));
                }
                catch (Exception)
                {
                    Invoke(new Action(() => PrgBar.Maximum = --fileCount));
                }
            }

            return filesToCensor;
        }

        private ICollection<CensoredFileInfo> CopyFilesToFolder(ICollection<string> filesToCensor, string folderPath)
        {
            int copiedCount = 0, pathCount = filesToCensor.Count();
            var filesInfo = new List<CensoredFileInfo>();

            Invoke(new Action(() =>
            {
                PrgBar.Maximum = pathCount;
                PrgBar.Value = 0;
            }));

            foreach (var filePath in filesToCensor)
            {
                try
                {
                    ChangeState($"{states["Copy"]} ({copiedCount}/{pathCount})");
                    resetEvent.WaitOne();

                    string filename = Path.GetFileName(filePath);
                    string copyName = $"{folderPath}/{filename}";

                    string name = Path.GetFileNameWithoutExtension(filename);
                    string ext = Path.GetExtension(filename);

                    int i = 0;
                    while (File.Exists(copyName))
                        copyName = $"{folderPath}/{name}{++i}{ext}";

                    File.Copy(filePath, copyName, true);

                    var info = new FileInfo(copyName);
                    var censorInfo = new CensoredFileInfo
                    {
                        Name = info.Name,
                        OriginalPath = filePath,
                        CopyPath = copyName,
                        Size = info.Length
                    };
                    filesInfo.Add(censorInfo);

                    Invoke(new Action(() => PrgBar.Value = ++copiedCount));
                }
                catch (Exception)
                {
                    Invoke(new Action(() => PrgBar.Value = --pathCount));
                }
            }

            return filesInfo;
        }

        private void CensorFiles(ICollection<CensoredFileInfo> filesInfo, string path)
        {
            int censoredCount = 0, copyCount = filesInfo.Count();

            Invoke(new Action(() =>
            {
                PrgBar.Maximum = copyCount;
                PrgBar.Value = 0;
            }));

            foreach (var info in filesInfo)
            {
                ChangeState($"{states["Censor"]} ({censoredCount}/{copyCount})");
                resetEvent.WaitOne();

                string filename = Path.GetFileName(info.CopyPath);
                string censorName = $"{path}/{filename}";

                string content;
                using (var sr = new StreamReader(info.CopyPath, Encoding.Default))
                {
                    content = sr.ReadToEnd();
                }

                foreach (var fw in words)
                {
                    resetEvent.WaitOne();

                    var matches = Regex.Matches(content, fw.Word);
                    var count = matches.Count;

                    if (count > 0)
                    {
                        info.CensorCount += count;
                        fw.Popularity += count;
                        content = Regex.Replace(content, fw.Word, "*******");
                    }
                }

                File.WriteAllText(censorName, content, Encoding.Default);

                Invoke(new Action(() => PrgBar.Value = ++censoredCount));
            }
        }

        private void GenerateReport(ICollection<CensoredFileInfo> filesInfo, int processedCount)
        {
            ChangeState(states["Report"]);

            using var sw = new StreamWriter($"{path}/Forbidden Words/Report.txt", false, Encoding.Default);

            sw.WriteLine("Forbidden Words Search Report\n");
            sw.WriteLine($"Total files processed: {processedCount}");
            sw.WriteLine($"Files censored: {filesInfo.Count}\n");
            sw.WriteLine("Top 10 Most Popular Forbidden Words:");

            var popWords = words.OrderByDescending(fw => fw.Popularity).ToArray();
            for (int i = 0; i < 10 && i < popWords.Length; i++)
                sw.WriteLine($"{i + 1}. {popWords[i].Word} - {popWords[i].Popularity} times");
            sw.WriteLine("\n");

            foreach (var info in filesInfo)
            {
                resetEvent.WaitOne();
                sw.WriteLine(info.Name);
                sw.WriteLine(info.OriginalPath);
                sw.WriteLine($"{info.Size} bytes");
                sw.WriteLine($"{info.CensorCount} words censored\n");
            }
        }

        private void SearchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thread.IsAlive)
            {
                resetEvent.Set();
                thread.Abort();
            }
            resetEvent.Dispose();
            parent.Show();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (thread.IsAlive)
            {
                resetEvent.Reset();
                PrgBar.MarqueeAnimationSpeed = 0;

                var result = MessageBox.Show("Are you sure to cancel the search?", "Warning", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    var workingPath = $"{path}/Forbidden Words";
                    if (Directory.Exists(workingPath))
                        Directory.Delete(workingPath, true);

                    this.Close();
                }
                else
                {
                    resetEvent.Set();
                    PrgBar.MarqueeAnimationSpeed = 2;
                }
            }
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            if (thread.IsAlive)
                if (!isThreadPaused)
                {
                    isThreadPaused = true;
                    BtnPause.Text = "Resume";
                    PrgBar.MarqueeAnimationSpeed = 0;
                    resetEvent.Reset();
                }
                else
                {
                    isThreadPaused = false;
                    BtnPause.Text = "Pause";
                    PrgBar.MarqueeAnimationSpeed = 2;
                    resetEvent.Set();
                }
        }
    }
}
