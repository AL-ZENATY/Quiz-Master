namespace QuizMaster
{
    partial class GuestOverlay
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
            picGuestPanel = new PictureBox();
            picContinueButton = new PictureBox();
            picCancelButton = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picGuestPanel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picContinueButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCancelButton).BeginInit();
            SuspendLayout();
            // 
            // picGuestPanel
            // 
            picGuestPanel.BackColor = Color.Transparent;
            picGuestPanel.BackgroundImage = Properties.Resources.overlay_gues;
            picGuestPanel.BackgroundImageLayout = ImageLayout.Stretch;
            picGuestPanel.Location = new Point(661, 94);
            picGuestPanel.Name = "picGuestPanel";
            picGuestPanel.Size = new Size(682, 843);
            picGuestPanel.SizeMode = PictureBoxSizeMode.Zoom;
            picGuestPanel.TabIndex = 0;
            picGuestPanel.TabStop = false;
            // 
            // picContinueButton
            // 
            picContinueButton.BackColor = Color.Transparent;
            picContinueButton.BackgroundImage = Properties.Resources.button_continue;
            picContinueButton.BackgroundImageLayout = ImageLayout.Stretch;
            picContinueButton.Cursor = Cursors.Hand;
            picContinueButton.Location = new Point(818, 659);
            picContinueButton.Name = "picContinueButton";
            picContinueButton.Size = new Size(384, 83);
            picContinueButton.SizeMode = PictureBoxSizeMode.StretchImage;
            picContinueButton.TabIndex = 2;
            picContinueButton.TabStop = false;
            picContinueButton.Click += picContinueButton_Click;
            picContinueButton.MouseEnter += picContinueButton_MouseEnter;
            picContinueButton.MouseLeave += picContinueButton_MouseLeave;
            // 
            // picCancelButton
            // 
            picCancelButton.BackColor = Color.Transparent;
            picCancelButton.BackgroundImage = Properties.Resources.button_cancel;
            picCancelButton.BackgroundImageLayout = ImageLayout.Stretch;
            picCancelButton.Cursor = Cursors.Hand;
            picCancelButton.Location = new Point(856, 777);
            picCancelButton.Name = "picCancelButton";
            picCancelButton.Size = new Size(307, 75);
            picCancelButton.SizeMode = PictureBoxSizeMode.StretchImage;
            picCancelButton.TabIndex = 3;
            picCancelButton.TabStop = false;
            picCancelButton.Click += picCancelButton_Click;
            picCancelButton.MouseEnter += picCancelButton_MouseEnter;
            picCancelButton.MouseLeave += picCancelButton_MouseLeave;
            // 
            // GuestOverlay
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Black;
            Controls.Add(picCancelButton);
            Controls.Add(picContinueButton);
            Controls.Add(picGuestPanel);
            Name = "GuestOverlay";
            Size = new Size(1920, 1080);
            ((System.ComponentModel.ISupportInitialize)picGuestPanel).EndInit();
            ((System.ComponentModel.ISupportInitialize)picContinueButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCancelButton).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox picGuestPanel;
        private PictureBox picContinueButton;
        private PictureBox picCancelButton;
    }
}
