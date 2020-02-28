namespace ForbiddenWordsSearch
{
    partial class WordsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LstWords = new System.Windows.Forms.ListView();
            this.TxtWord = new System.Windows.Forms.TextBox();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.BtnRemove = new System.Windows.Forms.Button();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.DlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // LstWords
            // 
            this.LstWords.HideSelection = false;
            this.LstWords.Location = new System.Drawing.Point(12, 12);
            this.LstWords.Name = "LstWords";
            this.LstWords.Size = new System.Drawing.Size(205, 319);
            this.LstWords.TabIndex = 0;
            this.LstWords.UseCompatibleStateImageBehavior = false;
            // 
            // TxtWord
            // 
            this.TxtWord.Location = new System.Drawing.Point(232, 12);
            this.TxtWord.Name = "TxtWord";
            this.TxtWord.Size = new System.Drawing.Size(117, 20);
            this.TxtWord.TabIndex = 1;
            this.TxtWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtWord_KeyDown);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(232, 38);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(117, 23);
            this.BtnAdd.TabIndex = 2;
            this.BtnAdd.Text = "Add";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnRemove
            // 
            this.BtnRemove.Location = new System.Drawing.Point(232, 67);
            this.BtnRemove.Name = "BtnRemove";
            this.BtnRemove.Size = new System.Drawing.Size(117, 23);
            this.BtnRemove.TabIndex = 2;
            this.BtnRemove.Text = "Remove";
            this.BtnRemove.UseVisualStyleBackColor = true;
            this.BtnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new System.Drawing.Point(232, 306);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(117, 23);
            this.BtnLoad.TabIndex = 2;
            this.BtnLoad.Text = "Load From...";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(232, 277);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(117, 23);
            this.BtnClear.TabIndex = 2;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(12, 351);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(337, 32);
            this.BtnSearch.TabIndex = 2;
            this.BtnSearch.Text = "Search";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // DlgOpen
            // 
            this.DlgOpen.Filter = "Text files|*.txt";
            // 
            // WordsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(363, 395);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.BtnSearch);
            this.Controls.Add(this.BtnLoad);
            this.Controls.Add(this.BtnRemove);
            this.Controls.Add(this.BtnAdd);
            this.Controls.Add(this.TxtWord);
            this.Controls.Add(this.LstWords);
            this.MaximizeBox = false;
            this.Name = "WordsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView LstWords;
        private System.Windows.Forms.TextBox TxtWord;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Button BtnRemove;
        private System.Windows.Forms.Button BtnLoad;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.OpenFileDialog DlgOpen;
    }
}

