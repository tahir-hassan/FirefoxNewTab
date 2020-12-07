namespace FirefoxNewTab
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.minimizeToTrayButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // minimizeToTrayButton
            // 
            this.minimizeToTrayButton.Location = new System.Drawing.Point(13, 13);
            this.minimizeToTrayButton.Name = "minimizeToTrayButton";
            this.minimizeToTrayButton.Size = new System.Drawing.Size(143, 23);
            this.minimizeToTrayButton.TabIndex = 0;
            this.minimizeToTrayButton.Text = "Minimize to tray";
            this.minimizeToTrayButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 50);
            this.Controls.Add(this.minimizeToTrayButton);
            this.Name = "MainForm";
            this.Text = "Firefox New Tab";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button minimizeToTrayButton;
    }
}

