namespace ForbiddenWordsSearch
{
    partial class SetupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupForm));
            this.BtnOpen = new System.Windows.Forms.Button();
            this.TxtFolder = new System.Windows.Forms.TextBox();
            this.DlgFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.BtnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnOpen
            // 
            this.BtnOpen.Location = new System.Drawing.Point(249, 12);
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.Size = new System.Drawing.Size(75, 20);
            this.BtnOpen.TabIndex = 0;
            this.BtnOpen.Text = "Browse...";
            this.BtnOpen.UseVisualStyleBackColor = true;
            this.BtnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // TxtFolder
            // 
            this.TxtFolder.Location = new System.Drawing.Point(12, 12);
            this.TxtFolder.Name = "TxtFolder";
            this.TxtFolder.Size = new System.Drawing.Size(217, 20);
            this.TxtFolder.TabIndex = 1;
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(12, 49);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(312, 32);
            this.BtnStart.TabIndex = 3;
            this.BtnStart.Text = "Start";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(337, 93);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.TxtFolder);
            this.Controls.Add(this.BtnOpen);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose Folder";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SetupForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOpen;
        private System.Windows.Forms.TextBox TxtFolder;
        private System.Windows.Forms.FolderBrowserDialog DlgFolder;
        private System.Windows.Forms.Button BtnStart;
    }
}