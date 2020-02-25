using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ForbiddenWordsSearch
{
    public partial class SetupForm : Form
    {
        private readonly Form parent;
        private readonly List<string> words;

        public SetupForm(Form parent, List<string> words)
        {
            InitializeComponent();
            this.parent = parent;
            this.words = words;
        }

        private void SetupForm_FormClosed(object sender, FormClosedEventArgs e) => parent.Show();

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            var result = DlgFolder.ShowDialog();
            if (result == DialogResult.OK)
                TxtFolder.Text = DlgFolder.SelectedPath;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            string path = DlgFolder.SelectedPath;
            if (Directory.Exists(DlgFolder.SelectedPath))
            {
                var searchForm = new SearchForm(parent, words, path);
                this.Hide();
                searchForm.Show();
            }
        }
    }
}
