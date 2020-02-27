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
        private readonly Form parent;

        private readonly List<ForbiddenWord> words;

        private readonly string path;

        private readonly Thread thread;

        private bool paused;

        private readonly Dictionary<string, string> states =
            new Dictionary<string, string>()
            {
                ["Init"] = "Initializing the search...",
                ["Search"] = "Searching through the files...",
                ["Copy"] = "Copying the files...",
                ["Censor"] = "Censoring the files...",
                ["Report"] = "Generating the report..."
            };

        public SearchForm(Form parent, List<string> words, string path)
        {
            InitializeComponent();
            this.parent = parent;
            this.words = words.Select(w => new ForbiddenWord { Word = w }).ToList();
            this.path = path;

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

        private IEnumerable<string> GetDirectoryFiles(string path)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path, "*.txt");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        yield return files[i];
                    }
                }
            }
        }

        private void PerformSearch()
        {
            var files = new List<string>();
            var paths = new List<string>();
            var filesInfo = new List<CensoredFileInfo>();

            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                try
                {
                    ChangeState($"{states["Init"]} (Drive {drive.Name})");

                    var filesOnDrive = GetDirectoryFiles(drive.Name).ToList();
                    files.AddRange(filesOnDrive);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, "Error");
                }
            }

            int processed = 0, fileCount = files.Count();

            Invoke(new Action(() => PrgBar.Maximum = fileCount));

            foreach (var file in files)
            {
                try
                {
                    ChangeState($"{states["Search"]} ({processed}/{fileCount})");

                    using var sr = new StreamReader(file, Encoding.Default);
                    var content = sr.ReadToEnd();

                    foreach (var fw in words)
                        if (content.Contains(fw.Word))
                        {
                            paths.Add(file);
                            break;
                        }

                    Invoke(new Action(() => PrgBar.Value = ++processed));
                }
                catch (Exception)
                {
                    Invoke(new Action(() => PrgBar.Maximum = --fileCount));
                }
            }

            int copied = 0, pathCount = paths.Count();

            Invoke(new Action(() =>
            {
                PrgBar.Maximum = pathCount;
                PrgBar.Value = 0;
            }));

            string copiesPath = $"{path}/Forbidden Words/Copies";
            Directory.CreateDirectory(copiesPath);

            foreach (var filePath in paths)
            {
                ChangeState($"{states["Copy"]} ({copied}/{pathCount})");

                string filename = Path.GetFileName(filePath);
                string copyName = $"{copiesPath}/{filename}";

                string name = Path.GetFileNameWithoutExtension(filename);
                string ext = Path.GetExtension(filename);

                int i = 0;
                while (File.Exists(copyName))
                    copyName = $"{copiesPath}/{name}{++i}{ext}";

                File.Copy(filePath, copyName);

                var info = new FileInfo(copyName);
                var censorInfo = new CensoredFileInfo
                {
                    Name = info.Name,
                    OriginalPath = filePath,
                    CopyPath = copyName,
                    Size = info.Length
                };
                filesInfo.Add(censorInfo);

                Invoke(new Action(() => PrgBar.Value = ++copied));
            }


            var copies = Directory.EnumerateFiles(copiesPath).ToList();

            int censored = 0, copyCount = copies.Count();

            Invoke(new Action(() =>
            {
                PrgBar.Maximum = pathCount;
                PrgBar.Value = 0;
            }));

            string censorPath = $"{path}/Forbidden Words/Censored";
            Directory.CreateDirectory(censorPath);

            foreach (var info in filesInfo)
            {
                ChangeState($"{states["Censor"]} ({censored}/{copyCount})");

                string filename = Path.GetFileName(info.CopyPath);
                string censorName = $"{censorPath}/{filename}";

                string content;
                using (var sr = new StreamReader(info.CopyPath, Encoding.Default))
                {
                    content = sr.ReadToEnd();
                }

                foreach (var fw in words)
                {
                    var matches = Regex.Matches(content, fw.Word);
                    var count = matches.Count;
                    if (count > 0)
                    {
                        info.CensorCount += count;
                        fw.Popularity += count;
                        content = Regex.Replace(content, fw.Word, "*******");
                    }
                }

                File.WriteAllText(censorName, content);

                Invoke(new Action(() => PrgBar.Value = ++censored));
            }

            ChangeState(states["Report"]);

            GenerateReport(filesInfo, files.Count);
            
            Invoke(new Action(() => this.Close()));
        }

        private void GenerateReport(List<CensoredFileInfo> filesInfo, int processedCount)
        {
            using var sw = new StreamWriter($"{path}/Forbidden Words/Report.txt", false, Encoding.Default);
            sw.WriteLine("Forbidden Words Search Report");
            sw.WriteLine();
            sw.WriteLine($"Total files processed: {processedCount}");
            sw.WriteLine($"Files censored: {filesInfo.Count}");
            sw.WriteLine();
            sw.WriteLine("Top 10 Most Popular Forbidden Words:");

            var popWords = words.OrderByDescending(fw => fw.Popularity).ToArray();
            for (int i = 0; i < 10 && i < popWords.Length; i++)
                sw.WriteLine($"{i + 1}. {popWords[i].Word} - {popWords[i].Popularity} times");
            sw.WriteLine();
            sw.WriteLine();

            foreach (var info in filesInfo)
            {
                sw.WriteLine(info.Name);
                sw.WriteLine(info.OriginalPath);
                sw.WriteLine($"{info.Size} bytes");
                sw.WriteLine($"{info.CensorCount} words censored");
                sw.WriteLine();
            }

            MessageBox.Show("The search is completed.\nThe report has been generated in the working folder.");
        }

        private void SearchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thread.IsAlive)
            {
                try
                {
                    thread.Resume();
                }
                catch (Exception) { }
                finally
                {
                    thread.Abort();
                }
            }
            parent.Show();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (thread.IsAlive)
            {
                if (!paused)
                    thread.Suspend();

                var result = MessageBox.Show("Are you sure to cancel the search?", "Warning", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    var workingPath = $"{path}/Forbidden Words";
                    if (Directory.Exists(workingPath))
                        Directory.Delete(workingPath, true);

                    this.Close();
                }
                else
                    if (!paused)
                        thread.Resume();
            }
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            if (thread.IsAlive)
                if (!paused)
                {
                    paused = true;
                    BtnPause.Text = "Resume";
                    thread.Suspend();
                }
                else
                {
                    paused = false;
                    BtnPause.Text = "Pause";
                    thread.Resume();
                }
        }
    }
}
