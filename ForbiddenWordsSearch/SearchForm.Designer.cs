namespace ForbiddenWordsSearch
{
    partial class SearchForm
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
            this.PrgBar = new System.Windows.Forms.ProgressBar();
            this.BtnPause = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.LblState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PrgBar
            // 
            this.PrgBar.Location = new System.Drawing.Point(12, 12);
            this.PrgBar.Name = "PrgBar";
            this.PrgBar.Size = new System.Drawing.Size(409, 23);
            this.PrgBar.TabIndex = 0;
            // 
            // BtnPause
            // 
            this.BtnPause.Location = new System.Drawing.Point(346, 51);
            this.BtnPause.Name = "BtnPause";
            this.BtnPause.Size = new System.Drawing.Size(75, 23);
            this.BtnPause.TabIndex = 1;
            this.BtnPause.Text = "Pause";
            this.BtnPause.UseVisualStyleBackColor = true;
            this.BtnPause.Click += new System.EventHandler(this.BtnPause_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(346, 80);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 23);
            this.BtnStop.TabIndex = 1;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // LblState
            // 
            this.LblState.AutoSize = true;
            this.LblState.Location = new System.Drawing.Point(9, 51);
            this.LblState.Name = "LblState";
            this.LblState.Size = new System.Drawing.Size(32, 13);
            this.LblState.TabIndex = 2;
            this.LblState.Text = "State";
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(433, 119);
            this.Controls.Add(this.LblState);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnPause);
            this.Controls.Add(this.PrgBar);
            this.MaximizeBox = false;
            this.Name = "SearchForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SearchForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar PrgBar;
        private System.Windows.Forms.Button BtnPause;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Label LblState;
    }
}