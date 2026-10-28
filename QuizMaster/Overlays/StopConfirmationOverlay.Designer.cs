namespace QuizMaster
{
    partial class StopConfirmationOverlay
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
            picCancelButton = new PictureBox();
            picExitAnywayButton = new PictureBox();
            picStopPanel = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picCancelButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picExitAnywayButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picStopPanel).BeginInit();
            SuspendLayout();
            // 
            // picCancelButton
            // 
            picCancelButton.BackColor = Color.Transparent;
            picCancelButton.BackgroundImage = Properties.Resources.button_cancel_exit_quiz;
            picCancelButton.BackgroundImageLayout = ImageLayout.Stretch;
            picCancelButton.Cursor = Cursors.Hand;
            picCancelButton.Location = new Point(821, 707);
            picCancelButton.Name = "picCancelButton";
            picCancelButton.Size = new Size(301, 94);
            picCancelButton.SizeMode = PictureBoxSizeMode.StretchImage;
            picCancelButton.TabIndex = 15;
            picCancelButton.TabStop = false;
            picCancelButton.Click += picCancelButton_Click;
            picCancelButton.MouseEnter += picCancelButton_MouseEnter;
            picCancelButton.MouseLeave += picCancelButton_MouseLeave;
            // 
            // picExitAnywayButton
            // 
            picExitAnywayButton.BackColor = Color.Transparent;
            picExitAnywayButton.BackgroundImage = Properties.Resources.button_exit_anyway_exit_quiz;
            picExitAnywayButton.BackgroundImageLayout = ImageLayout.Stretch;
            picExitAnywayButton.Cursor = Cursors.Hand;
            picExitAnywayButton.Location = new Point(765, 807);
            picExitAnywayButton.Name = "picExitAnywayButton";
            picExitAnywayButton.Size = new Size(393, 120);
            picExitAnywayButton.SizeMode = PictureBoxSizeMode.StretchImage;
            picExitAnywayButton.TabIndex = 14;
            picExitAnywayButton.TabStop = false;
            picExitAnywayButton.Click += picExitAnywayButton_Click;
            picExitAnywayButton.MouseEnter += picExitAnywayButton_MouseEnter;
            picExitAnywayButton.MouseLeave += picExitAnywayButton_MouseLeave;
            // 
            // picStopPanel
            // 
            picStopPanel.BackColor = Color.Transparent;
            picStopPanel.BackgroundImage = Properties.Resources.overley_stop_game;
            picStopPanel.BackgroundImageLayout = ImageLayout.Stretch;
            picStopPanel.Location = new Point(662, 119);
            picStopPanel.Name = "picStopPanel";
            picStopPanel.Size = new Size(597, 843);
            picStopPanel.SizeMode = PictureBoxSizeMode.Zoom;
            picStopPanel.TabIndex = 13;
            picStopPanel.TabStop = false;
            // 
            // StopConfirmationOverlay
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(picCancelButton);
            Controls.Add(picExitAnywayButton);
            Controls.Add(picStopPanel);
            Name = "StopConfirmationOverlay";
            Size = new Size(1920, 1080);
            ((System.ComponentModel.ISupportInitialize)picCancelButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)picExitAnywayButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)picStopPanel).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox picCancelButton;
        private PictureBox picExitAnywayButton;
        private PictureBox picStopPanel;
    }
}
