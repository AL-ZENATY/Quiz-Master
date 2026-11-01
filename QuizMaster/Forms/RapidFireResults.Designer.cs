
namespace QuizMaster
{
    partial class RapidFireResults
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
        /// Required method for Designer support.
        /// </summary>
        private void InitializeComponent()
        {
            picQuestionImage = new PictureBox();
            lblQuestionText = new Label();
            lblCorrectAnswer = new Label();
            lblYourAnswer = new Label();
            pnlQuestionNavigation = new Panel();
            lblMoreLeft = new Label();
            lblMoreRight = new Label();
            picQuestion01 = new PictureBox();
            picQuestion02 = new PictureBox();
            picQuestion03 = new PictureBox();
            picQuestion04 = new PictureBox();
            picQuestion05 = new PictureBox();
            picQuestion06 = new PictureBox();
            picQuestion07 = new PictureBox();
            picQuestion08 = new PictureBox();
            picQuestion09 = new PictureBox();
            picQuestion10 = new PictureBox();
            picArrowLeft = new PictureBox();
            picArrowRight = new PictureBox();
            lblStopwatch = new Label();
            lblTimeAdjustment = new Label();
            lblTotalQuestions = new Label();
            lblWrongAnswers = new Label();
            lblSkipUsed = new Label();
            lblFiftyFiftyUsed = new Label();
            lblFinalTime = new Label();
            picExit = new PictureBox();
            picLeaderboard = new PictureBox();
            lblTimedOut = new Label();
            lblTop10Position = new Label();
            picTop10Panle = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picQuestionImage).BeginInit();
            pnlQuestionNavigation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picQuestion01).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion02).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion03).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion04).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion05).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion06).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion07).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion08).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion09).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picArrowLeft).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picArrowRight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picExit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLeaderboard).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picTop10Panle).BeginInit();
            SuspendLayout();
            // 
            // picQuestionImage
            // 
            picQuestionImage.BackColor = Color.Transparent;
            picQuestionImage.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestionImage.Location = new Point(96, 464);
            picQuestionImage.Name = "picQuestionImage";
            picQuestionImage.Size = new Size(479, 367);
            picQuestionImage.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestionImage.TabIndex = 1;
            picQuestionImage.TabStop = false;
            // 
            // lblQuestionText
            // 
            lblQuestionText.BackColor = Color.Transparent;
            lblQuestionText.Font = new Font("Cooper Black", 30F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblQuestionText.Location = new Point(658, 488);
            lblQuestionText.Name = "lblQuestionText";
            lblQuestionText.Size = new Size(635, 182);
            lblQuestionText.TabIndex = 2;
            lblQuestionText.Text = "Displays the question text";
            lblQuestionText.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCorrectAnswer
            // 
            lblCorrectAnswer.BackColor = Color.Transparent;
            lblCorrectAnswer.Font = new Font("Cooper Black", 20.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblCorrectAnswer.ForeColor = Color.LimeGreen;
            lblCorrectAnswer.Location = new Point(924, 706);
            lblCorrectAnswer.Name = "lblCorrectAnswer";
            lblCorrectAnswer.Size = new Size(371, 50);
            lblCorrectAnswer.TabIndex = 3;
            lblCorrectAnswer.Text = "Correct Answer";
            lblCorrectAnswer.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblYourAnswer
            // 
            lblYourAnswer.BackColor = Color.Transparent;
            lblYourAnswer.Font = new Font("Cooper Black", 20.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblYourAnswer.ForeColor = Color.Red;
            lblYourAnswer.Location = new Point(924, 795);
            lblYourAnswer.Name = "lblYourAnswer";
            lblYourAnswer.Size = new Size(369, 50);
            lblYourAnswer.TabIndex = 4;
            lblYourAnswer.Text = "Your Answer";
            lblYourAnswer.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlQuestionNavigation
            // 
            pnlQuestionNavigation.Controls.Add(lblMoreLeft);
            pnlQuestionNavigation.Controls.Add(lblMoreRight);
            pnlQuestionNavigation.Controls.Add(picQuestion01);
            pnlQuestionNavigation.Controls.Add(picQuestion02);
            pnlQuestionNavigation.Controls.Add(picQuestion03);
            pnlQuestionNavigation.Controls.Add(picQuestion04);
            pnlQuestionNavigation.Controls.Add(picQuestion05);
            pnlQuestionNavigation.Controls.Add(picQuestion06);
            pnlQuestionNavigation.Controls.Add(picQuestion07);
            pnlQuestionNavigation.Controls.Add(picQuestion08);
            pnlQuestionNavigation.Controls.Add(picQuestion09);
            pnlQuestionNavigation.Controls.Add(picQuestion10);
            pnlQuestionNavigation.Location = new Point(149, 322);
            pnlQuestionNavigation.Name = "pnlQuestionNavigation";
            pnlQuestionNavigation.Size = new Size(1084, 87);
            pnlQuestionNavigation.TabIndex = 5;
            // 
            // lblMoreLeft
            // 
            lblMoreLeft.BackColor = Color.Transparent;
            lblMoreLeft.Font = new Font("Cooper Black", 24.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblMoreLeft.Location = new Point(13, 23);
            lblMoreLeft.Name = "lblMoreLeft";
            lblMoreLeft.Size = new Size(53, 46);
            lblMoreLeft.TabIndex = 26;
            lblMoreLeft.Text = "...";
            lblMoreLeft.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblMoreRight
            // 
            lblMoreRight.BackColor = Color.Transparent;
            lblMoreRight.Font = new Font("Cooper Black", 24.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblMoreRight.Location = new Point(1021, 23);
            lblMoreRight.Name = "lblMoreRight";
            lblMoreRight.Size = new Size(53, 46);
            lblMoreRight.TabIndex = 17;
            lblMoreRight.Text = "...";
            lblMoreRight.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picQuestion01
            // 
            picQuestion01.BackColor = Color.Transparent;
            picQuestion01.BackgroundImage = Properties.Resources.result_number_background;
            picQuestion01.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestion01.Location = new Point(84, 3);
            picQuestion01.Name = "picQuestion01";
            picQuestion01.Size = new Size(87, 81);
            picQuestion01.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestion01.TabIndex = 16;
            picQuestion01.TabStop = false;
            // 
            // picQuestion02
            // 
            picQuestion02.BackColor = Color.Transparent;
            picQuestion02.BackgroundImage = Properties.Resources.result_number_background;
            picQuestion02.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestion02.Location = new Point(177, 3);
            picQuestion02.Name = "picQuestion02";
            picQuestion02.Size = new Size(87, 81);
            picQuestion02.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestion02.TabIndex = 17;
            picQuestion02.TabStop = false;
            // 
            // picQuestion03
            // 
            picQuestion03.BackColor = Color.Transparent;
            picQuestion03.BackgroundImage = Properties.Resources.result_number_background;
            picQuestion03.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestion03.Location = new Point(270, 3);
            picQuestion03.Name = "picQuestion03";
            picQuestion03.Size = new Size(87, 81);
            picQuestion03.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestion03.TabIndex = 18;
            picQuestion03.TabStop = false;
            // 
            // picQuestion04
            // 
            picQuestion04.BackColor = Color.Transparent;
            picQuestion04.BackgroundImage = Properties.Resources.result_number_background;
            picQuestion04.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestion04.Location = new Point(363, 3);
            picQuestion04.Name = "picQuestion04";
            picQuestion04.Size = new Size(87, 81);
            picQuestion04.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestion04.TabIndex = 19;
            picQuestion04.TabStop = false;
            // 
            // picQuestion05
            // 
            picQuestion05.BackColor = Color.Transparent;
            picQuestion05.BackgroundImage = Properties.Resources.result_number_background;
            picQuestion05.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestion05.Location = new Point(456, 3);
            picQuestion05.Name = "picQuestion05";
            picQuestion05.Size = new Size(87, 81);
            picQuestion05.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestion05.TabIndex = 20;
            picQuestion05.TabStop = false;
            // 
            // picQuestion06
            // 
            picQuestion06.BackColor = Color.Transparent;
            picQuestion06.BackgroundImage = Properties.Resources.result_number_background;
            picQuestion06.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestion06.Location = new Point(549, 3);
            picQuestion06.Name = "picQuestion06";
            picQuestion06.Size = new Size(87, 81);
            picQuestion06.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestion06.TabIndex = 21;
            picQuestion06.TabStop = false;
            // 
            // picQuestion07
            // 
            picQuestion07.BackColor = Color.Transparent;
            picQuestion07.BackgroundImage = Properties.Resources.result_number_background;
            picQuestion07.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestion07.Location = new Point(642, 3);
            picQuestion07.Name = "picQuestion07";
            picQuestion07.Size = new Size(87, 81);
            picQuestion07.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestion07.TabIndex = 22;
            picQuestion07.TabStop = false;
            // 
            // picQuestion08
            // 
            picQuestion08.BackColor = Color.Transparent;
            picQuestion08.BackgroundImage = Properties.Resources.result_number_background;
            picQuestion08.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestion08.Location = new Point(735, 3);
            picQuestion08.Name = "picQuestion08";
            picQuestion08.Size = new Size(87, 81);
            picQuestion08.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestion08.TabIndex = 23;
            picQuestion08.TabStop = false;
            // 
            // picQuestion09
            // 
            picQuestion09.BackColor = Color.Transparent;
            picQuestion09.BackgroundImage = Properties.Resources.result_number_background;
            picQuestion09.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestion09.Location = new Point(828, 3);
            picQuestion09.Name = "picQuestion09";
            picQuestion09.Size = new Size(87, 81);
            picQuestion09.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestion09.TabIndex = 24;
            picQuestion09.TabStop = false;
            // 
            // picQuestion10
            // 
            picQuestion10.BackColor = Color.Transparent;
            picQuestion10.BackgroundImage = Properties.Resources.result_number_background;
            picQuestion10.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestion10.Location = new Point(921, 3);
            picQuestion10.Name = "picQuestion10";
            picQuestion10.Size = new Size(87, 81);
            picQuestion10.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestion10.TabIndex = 25;
            picQuestion10.TabStop = false;
            // 
            // picArrowLeft
            // 
            picArrowLeft.BackColor = Color.Transparent;
            picArrowLeft.BackgroundImageLayout = ImageLayout.Stretch;
            picArrowLeft.Location = new Point(59, 320);
            picArrowLeft.Name = "picArrowLeft";
            picArrowLeft.Size = new Size(82, 87);
            picArrowLeft.SizeMode = PictureBoxSizeMode.Zoom;
            picArrowLeft.TabIndex = 7;
            picArrowLeft.TabStop = false;
            // 
            // picArrowRight
            // 
            picArrowRight.BackColor = Color.Transparent;
            picArrowRight.BackgroundImageLayout = ImageLayout.Stretch;
            picArrowRight.Location = new Point(1239, 322);
            picArrowRight.Name = "picArrowRight";
            picArrowRight.Size = new Size(82, 87);
            picArrowRight.SizeMode = PictureBoxSizeMode.Zoom;
            picArrowRight.TabIndex = 6;
            picArrowRight.TabStop = false;
            // 
            // lblStopwatch
            // 
            lblStopwatch.BackColor = Color.Transparent;
            lblStopwatch.Font = new Font("Cooper Black", 20.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblStopwatch.ForeColor = Color.White;
            lblStopwatch.Location = new Point(1704, 251);
            lblStopwatch.Name = "lblStopwatch";
            lblStopwatch.Size = new Size(142, 43);
            lblStopwatch.TabIndex = 8;
            lblStopwatch.Text = "00:00.00";
            lblStopwatch.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTimeAdjustment
            // 
            lblTimeAdjustment.BackColor = Color.Transparent;
            lblTimeAdjustment.Font = new Font("Cooper Black", 20.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTimeAdjustment.ForeColor = Color.Red;
            lblTimeAdjustment.Location = new Point(1704, 315);
            lblTimeAdjustment.Name = "lblTimeAdjustment";
            lblTimeAdjustment.Size = new Size(142, 43);
            lblTimeAdjustment.TabIndex = 9;
            lblTimeAdjustment.Text = "+0s";
            lblTimeAdjustment.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalQuestions
            // 
            lblTotalQuestions.BackColor = Color.Transparent;
            lblTotalQuestions.Font = new Font("Cooper Black", 20.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTotalQuestions.ForeColor = Color.White;
            lblTotalQuestions.Location = new Point(1704, 384);
            lblTotalQuestions.Name = "lblTotalQuestions";
            lblTotalQuestions.Size = new Size(142, 43);
            lblTotalQuestions.TabIndex = 10;
            lblTotalQuestions.Text = "0";
            lblTotalQuestions.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblWrongAnswers
            // 
            lblWrongAnswers.BackColor = Color.Transparent;
            lblWrongAnswers.Font = new Font("Cooper Black", 20.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblWrongAnswers.ForeColor = Color.Red;
            lblWrongAnswers.Location = new Point(1704, 453);
            lblWrongAnswers.Name = "lblWrongAnswers";
            lblWrongAnswers.Size = new Size(142, 43);
            lblWrongAnswers.TabIndex = 11;
            lblWrongAnswers.Text = "0";
            lblWrongAnswers.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSkipUsed
            // 
            lblSkipUsed.BackColor = Color.Transparent;
            lblSkipUsed.Font = new Font("Cooper Black", 20.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblSkipUsed.ForeColor = Color.Gold;
            lblSkipUsed.Location = new Point(1704, 588);
            lblSkipUsed.Name = "lblSkipUsed";
            lblSkipUsed.Size = new Size(142, 48);
            lblSkipUsed.TabIndex = 12;
            lblSkipUsed.Text = "Not Used";
            lblSkipUsed.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFiftyFiftyUsed
            // 
            lblFiftyFiftyUsed.BackColor = Color.Transparent;
            lblFiftyFiftyUsed.Font = new Font("Cooper Black", 20.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblFiftyFiftyUsed.ForeColor = Color.Gold;
            lblFiftyFiftyUsed.Location = new Point(1704, 659);
            lblFiftyFiftyUsed.Name = "lblFiftyFiftyUsed";
            lblFiftyFiftyUsed.Size = new Size(142, 43);
            lblFiftyFiftyUsed.TabIndex = 13;
            lblFiftyFiftyUsed.Text = "Not Used";
            lblFiftyFiftyUsed.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFinalTime
            // 
            lblFinalTime.BackColor = Color.Transparent;
            lblFinalTime.Font = new Font("Cooper Black", 44F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblFinalTime.ForeColor = Color.White;
            lblFinalTime.Location = new Point(1459, 818);
            lblFinalTime.Name = "lblFinalTime";
            lblFinalTime.Size = new Size(319, 142);
            lblFinalTime.TabIndex = 14;
            lblFinalTime.Text = "00:00.00";
            lblFinalTime.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picExit
            // 
            picExit.BackColor = Color.Transparent;
            picExit.BackgroundImage = Properties.Resources.result_exit_button;
            picExit.BackgroundImageLayout = ImageLayout.Stretch;
            picExit.Location = new Point(59, 919);
            picExit.Name = "picExit";
            picExit.Size = new Size(366, 161);
            picExit.SizeMode = PictureBoxSizeMode.Zoom;
            picExit.TabIndex = 15;
            picExit.TabStop = false;
            // 
            // picLeaderboard
            // 
            picLeaderboard.BackColor = Color.Transparent;
            picLeaderboard.BackgroundImage = Properties.Resources.result_leaderboard_button;
            picLeaderboard.BackgroundImageLayout = ImageLayout.Stretch;
            picLeaderboard.Location = new Point(745, 919);
            picLeaderboard.Name = "picLeaderboard";
            picLeaderboard.Size = new Size(458, 161);
            picLeaderboard.SizeMode = PictureBoxSizeMode.Zoom;
            picLeaderboard.TabIndex = 16;
            picLeaderboard.TabStop = false;
            // 
            // lblTimedOut
            // 
            lblTimedOut.BackColor = Color.Transparent;
            lblTimedOut.Font = new Font("Cooper Black", 20.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTimedOut.ForeColor = Color.DarkOrange;
            lblTimedOut.Location = new Point(1704, 521);
            lblTimedOut.Name = "lblTimedOut";
            lblTimedOut.Size = new Size(142, 43);
            lblTimedOut.TabIndex = 17;
            lblTimedOut.Text = "0";
            lblTimedOut.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTop10Position
            // 
            lblTop10Position.BackColor = Color.Transparent;
            lblTop10Position.Font = new Font("Cooper Black", 30F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTop10Position.ForeColor = Color.FromArgb(0, 239, 160);
            lblTop10Position.Location = new Point(1029, 122);
            lblTop10Position.Name = "lblTop10Position";
            lblTop10Position.Size = new Size(297, 104);
            lblTop10Position.TabIndex = 21;
            lblTop10Position.Text = "#1 BEST RECORDS!";
            lblTop10Position.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picTop10Panle
            // 
            picTop10Panle.BackColor = Color.Transparent;
            picTop10Panle.BackgroundImage = Properties.Resources.top_10_panel;
            picTop10Panle.BackgroundImageLayout = ImageLayout.Stretch;
            picTop10Panle.Location = new Point(605, 101);
            picTop10Panle.Name = "picTop10Panle";
            picTop10Panle.Size = new Size(747, 149);
            picTop10Panle.SizeMode = PictureBoxSizeMode.Zoom;
            picTop10Panle.TabIndex = 20;
            picTop10Panle.TabStop = false;
            // 
            // RapidFireResults
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Transparent;
            BackgroundImage = Properties.Resources.rapidfire_result_background;
            BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(lblTop10Position);
            Controls.Add(picTop10Panle);
            Controls.Add(lblTimedOut);
            Controls.Add(picLeaderboard);
            Controls.Add(picExit);
            Controls.Add(lblFinalTime);
            Controls.Add(lblFiftyFiftyUsed);
            Controls.Add(lblSkipUsed);
            Controls.Add(lblWrongAnswers);
            Controls.Add(lblTotalQuestions);
            Controls.Add(lblTimeAdjustment);
            Controls.Add(lblStopwatch);
            Controls.Add(picArrowLeft);
            Controls.Add(picArrowRight);
            Controls.Add(pnlQuestionNavigation);
            Controls.Add(lblYourAnswer);
            Controls.Add(lblCorrectAnswer);
            Controls.Add(lblQuestionText);
            Controls.Add(picQuestionImage);
            Name = "RapidFireResults";
            Size = new Size(1920, 1080);
            ((System.ComponentModel.ISupportInitialize)picQuestionImage).EndInit();
            pnlQuestionNavigation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picQuestion01).EndInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion02).EndInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion03).EndInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion04).EndInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion05).EndInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion06).EndInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion07).EndInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion08).EndInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion09).EndInit();
            ((System.ComponentModel.ISupportInitialize)picQuestion10).EndInit();
            ((System.ComponentModel.ISupportInitialize)picArrowLeft).EndInit();
            ((System.ComponentModel.ISupportInitialize)picArrowRight).EndInit();
            ((System.ComponentModel.ISupportInitialize)picExit).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLeaderboard).EndInit();
            ((System.ComponentModel.ISupportInitialize)picTop10Panle).EndInit();
            ResumeLayout(false);
        }

        #endregion

        // ==========================================
        // QUESTION DISPLAY
        // ==========================================

        private PictureBox picQuestionImage;
        private Label lblQuestionText;
        private Label lblCorrectAnswer;
        private Label lblYourAnswer;

        // ==========================================
        // QUESTION NAVIGATION
        // ==========================================

        private Panel pnlQuestionNavigation;

        private Label lblMoreLeft;
        private Label lblMoreRight;

        private PictureBox picQuestion01;
        private PictureBox picQuestion02;
        private PictureBox picQuestion03;
        private PictureBox picQuestion04;
        private PictureBox picQuestion05;
        private PictureBox picQuestion06;
        private PictureBox picQuestion07;
        private PictureBox picQuestion08;
        private PictureBox picQuestion09;
        private PictureBox picQuestion10;

        private PictureBox picArrowLeft;
        private PictureBox picArrowRight;

        // ==========================================
        // RAPID FIRE STATISTICS
        // ==========================================

        private Label lblStopwatch;
        private Label lblTimeAdjustment;
        private Label lblTotalQuestions;
        private Label lblWrongAnswers;
        private Label lblSkipUsed;
        private Label lblFiftyFiftyUsed;
        private Label lblFinalTime;

        // ==========================================
        // NAVIGATION BUTTONS
        // ==========================================

        private PictureBox picExit;
        private PictureBox picLeaderboard;
        private Label lblTimedOut;
        private Label lblTop10Position;
        private PictureBox picTop10Panle;
    }
}
