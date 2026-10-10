namespace QuizMaster
{
    partial class picForgotPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlClose = new Panel();
            SuspendLayout();
            // 
            // pnlClose
            // 
            pnlClose.Cursor = Cursors.Hand;
            pnlClose.Location = new Point(611, 78);
            pnlClose.Name = "pnlClose";
            pnlClose.Size = new Size(88, 83);
            pnlClose.TabIndex = 0;
            pnlClose.Click += pnlClose_Click;
            // 
            // picForgotPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            BackgroundImage = Properties.Resources.overlay_forgot_password;
            BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(pnlClose);
            DoubleBuffered = true;
            Location = new Point(660, 240);
            Name = "picForgotPanel";
            Size = new Size(705, 824);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlClose;
    }
}
