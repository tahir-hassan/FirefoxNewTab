namespace FirefoxNewTab
{
    partial class FirefoxContainersForm
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
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.ContainersListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // filterTextBox
            // 
            this.filterTextBox.Location = new System.Drawing.Point(29, 28);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(347, 23);
            this.filterTextBox.TabIndex = 1;
            // 
            // ContainersListBox
            // 
            this.ContainersListBox.FormattingEnabled = true;
            this.ContainersListBox.ItemHeight = 15;
            this.ContainersListBox.Location = new System.Drawing.Point(29, 57);
            this.ContainersListBox.Name = "ContainersListBox";
            this.ContainersListBox.Size = new System.Drawing.Size(347, 199);
            this.ContainersListBox.TabIndex = 2;
            // 
            // FirefoxContainersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 281);
            this.Controls.Add(this.ContainersListBox);
            this.Controls.Add(this.filterTextBox);
            this.Name = "FirefoxContainersForm";
            this.Text = "Choose Firefox Container";
            this.Load += new System.EventHandler(this.FirefoxContainersForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox filterTextBox;
        private System.Windows.Forms.ListBox ContainersListBox;
    }
}