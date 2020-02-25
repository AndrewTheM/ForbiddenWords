using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace ForbiddenWordsSearch
{
    public partial class SearchForm : Form
    {
        private readonly Form parent;

        private readonly List<string> words;

        private readonly Thread thread;

        private readonly Dictionary<string, string> states =
            new Dictionary<string, string>()
            {
                ["Init"] = "Initializing the search...",
                ["Search"] = "Searching through the files...",
                ["Copy"] = "Copying the files...",
                ["Report"] = "Generating the report..."
            };

        private readonly List<string> paths = new List<string>();

        private bool paused;

        public SearchForm(Form parent, List<string> words, string path)
        {
            InitializeComponent();
            this.parent = parent;
            this.words = words;

            thread = new Thread(new ParameterizedThreadStart(PerformSearch)) { IsBackground = true };
            thread.Start(path);
        }

        private delegate void ChangeStateDelegate(string key);

        private void ChangeState(string value)
        {
            if (this.InvokeRequired || LblState.InvokeRequired)
                Invoke(new ChangeStateDelegate(ChangeState), value);
            else
                LblState.Text = value;
        }

        private void PerformSearch(object path)
        {
            List<string> files = new List<string>();
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                try
                {
                    ChangeState($"{states["Init"]} (Drive {drive.VolumeLabel})");

                    var filesOnDrive = Directory.EnumerateFiles(drive.Name, "*.txt", SearchOption.AllDirectories);
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
                    foreach (var word in words)
                        if (content.Contains(word))
                            paths.Add(file);

                    Invoke(new Action(() => PrgBar.Value = ++processed));
                }
                catch (Exception) { }
            }

            int copied = 0, pathCount = paths.Count();
            string dirPath = (string)path;

            Invoke(new Action(() =>
            {
                PrgBar.Maximum = pathCount;
                PrgBar.Value = 0;
            }));

            foreach (var filePath in paths)
            {
                ChangeState($"{states["Copy"]} ({copied}/{pathCount})");

                string filename = Path.GetFileName(filePath);
                string dirName = @$"{dirPath}/{filename}";

                if (!File.Exists(dirName))
                {
                    int i = 0;
                    string name = Path.GetFileNameWithoutExtension(dirName);
                    string ext = Path.GetExtension(dirName);
                    while (File.Exists($"{dirPath}/{name}{++i}{ext}"));
                    dirName = $"{dirPath}/{name}{i}{ext}";
                }

                // TODO: fix duplicates

                File.Copy(filePath, dirName);

                Invoke(new Action(() => PrgBar.Value = ++copied));
            }
        }

        private void SearchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thread.IsAlive)
            {
                if (paused)
                    thread.Resume();
                thread.Abort();
            }
            parent.Show();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (thread.IsAlive)
            {
                thread.Suspend();

                var result = MessageBox.Show("Are you sure to cancel the search?", "Warning", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    thread.Resume();

                    // TODO: clear the directory

                    thread.Abort();
                    this.Close();
                }
                else
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
