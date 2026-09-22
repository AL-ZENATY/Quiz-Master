namespace QuizMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            startPage1 = new StartPage();
            SuspendLayout();
            // 
            // startPage1
            // 
            startPage1.BackgroundImage = (Image)resources.GetObject("startPage1.BackgroundImage");
            startPage1.BackgroundImageLayout = ImageLayout.Stretch;
            startPage1.Dock = DockStyle.Fill;
            startPage1.Location = new Point(0, 0);
            startPage1.Name = "startPage1";
            startPage1.Size = new Size(1920, 1080);
            startPage1.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1920, 1080);
            Controls.Add(startPage1);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "QuizMaster";
            WindowState = FormWindowState.Maximized;
            ResumeLayout(false);
        }

        #endregion

        private StartPage startPage1;
    }
}
