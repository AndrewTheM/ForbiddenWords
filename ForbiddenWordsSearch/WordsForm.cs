using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ForbiddenWordsSearch
{
    public partial class WordsForm : Form
    {
        public WordsForm()
        {
            InitializeComponent();
            LstWords.View = View.Details;
            LstWords.Scrollable = true;
            LstWords.Columns.Add(new ColumnHeader());
            LstWords.HeaderStyle = ColumnHeaderStyle.None;
            LstWords.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            LstWords.MultiSelect = false;
            LstWords.FullRowSelect = true;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var words = GetWordsFromListView();
            if (!string.IsNullOrWhiteSpace(TxtWord.Text) && !words.Contains(TxtWord.Text))
            {
                LstWords.Items.Add(new ListViewItem(TxtWord.Text));
                TxtWord.Text = null;
            }
        }

        private void BtnClear_Click(object sender, EventArgs e) => LstWords.Items.Clear();

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            var indices = new int[LstWords.SelectedIndices.Count];
            LstWords.SelectedIndices.CopyTo(indices, 0);
            foreach (var index in indices)
                LstWords.Items.RemoveAt(index);
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            var result = DlgOpen.ShowDialog();
            if (result == DialogResult.OK && File.Exists(DlgOpen.FileName))
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    using var sr = new StreamReader(DlgOpen.FileName, Encoding.Default);
                    var content = sr.ReadToEnd();
                    var newWords = content.Split(' ', '\t', '\n');
                    var words = GetWordsFromListView();
                    var items = newWords.Except(words).Select(w => new ListViewItem(w)).ToArray();
                    AddItemsToListView(items);
                    MessageBox.Show($"Successfully added {items.Length} new words from file.");
                });
        }

        private delegate List<string> GetWordsFromListViewCallback();

        private List<string> GetWordsFromListView()
        {
            if (LstWords.InvokeRequired)
                return Invoke(new GetWordsFromListViewCallback(GetWordsFromListView)) as List<string>;
            else
            {
                var items = new ListViewItem[LstWords.Items.Count];
                LstWords.Items.CopyTo(items, 0);
                return items.Select(i => i.Text).ToList();
            }
        }

        private delegate void AddItemsToListViewCallback(ListViewItem[] items);

        private void AddItemsToListView(ListViewItem[] items)
        {
            if (LstWords.InvokeRequired)
                Invoke(new AddItemsToListViewCallback(AddItemsToListView), new object[] { items });
            else
                foreach (var item in items)
                {
                    var words = GetWordsFromListView();
                    if (!string.IsNullOrWhiteSpace(item.Text) && !words.Contains(item.Text))
                        LstWords.Items.Add(item);
                }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            var words = GetWordsFromListView();
            if (words.Count > 0)
            {
                var setupForm = new SetupForm(this, words);
                this.Hide();
                setupForm.Show();
            }
            else
                MessageBox.Show("Add at least one word to the list.");
        }
    }
}
