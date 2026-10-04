namespace QuizMaster
{
    partial class InformationOverlay
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
            picHowToPlay = new PictureBox();
            picRules = new PictureBox();
            picCategories = new PictureBox();
            picCloseButton = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picHowToPlay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picRules).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCategories).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCloseButton).BeginInit();
            SuspendLayout();
            // 
            // picHowToPlay
            // 
            picHowToPlay.BackColor = Color.Transparent;
            picHowToPlay.BackgroundImage = Properties.Resources.play_info;
            picHowToPlay.BackgroundImageLayout = ImageLayout.Stretch;
            picHowToPlay.Location = new Point(-9, 91);
            picHowToPlay.Name = "picHowToPlay";
            picHowToPlay.Size = new Size(628, 856);
            picHowToPlay.TabIndex = 0;
            picHowToPlay.TabStop = false;
            // 
            // picRules
            // 
            picRules.BackColor = Color.Transparent;
            picRules.BackgroundImage = Properties.Resources.score_info;
            picRules.BackgroundImageLayout = ImageLayout.Stretch;
            picRules.Location = new Point(652, 91);
            picRules.Name = "picRules";
            picRules.Size = new Size(624, 856);
            picRules.TabIndex = 1;
            picRules.TabStop = false;
            // 
            // picCategories
            // 
            picCategories.BackColor = Color.Transparent;
            picCategories.BackgroundImage = Properties.Resources.category_info;
            picCategories.BackgroundImageLayout = ImageLayout.Stretch;
            picCategories.Location = new Point(1309, 91);
            picCategories.Name = "picCategories";
            picCategories.Size = new Size(599, 856);
            picCategories.TabIndex = 2;
            picCategories.TabStop = false;
            // 
            // picCloseButton
            // 
            picCloseButton.BackColor = Color.Transparent;
            picCloseButton.BackgroundImage = Properties.Resources.button_back;
            picCloseButton.BackgroundImageLayout = ImageLayout.Stretch;
            picCloseButton.Cursor = Cursors.Hand;
            picCloseButton.Location = new Point(69, 936);
            picCloseButton.Name = "picCloseButton";
            picCloseButton.Size = new Size(365, 164);
            picCloseButton.SizeMode = PictureBoxSizeMode.Zoom;
            picCloseButton.TabIndex = 3;
            picCloseButton.TabStop = false;
            picCloseButton.Click += pnlClose_Click;
            picCloseButton.MouseEnter += picCloseButton_MouseEnter;
            picCloseButton.MouseLeave += picCloseButton_MouseLeave;
            // 
            // InformationOverlay
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            Controls.Add(picCloseButton);
            Controls.Add(picCategories);
            Controls.Add(picRules);
            Controls.Add(picHowToPlay);
            Name = "InformationOverlay";
            Size = new Size(1920, 1080);
            ((System.ComponentModel.ISupportInitialize)picHowToPlay).EndInit();
            ((System.ComponentModel.ISupportInitialize)picRules).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCategories).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCloseButton).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox picHowToPlay;
        private PictureBox picRules;
        private PictureBox picCategories;
        private PictureBox picCloseButton;
    }
}
