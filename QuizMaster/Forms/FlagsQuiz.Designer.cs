namespace QuizMaster
{
    partial class FlagsQuiz
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            picQuestionTimer = new PictureBox();
            lblQuestionTimer = new Label();
            lblQuestionCounter = new Label();
            lblGameTimer = new Label();
            lblScore = new Label();
            picSkipButton = new PictureBox();
            picStopButton = new PictureBox();
            lblAnswerB = new Label();
            lblAnswerD = new Label();
            lblAnswerC = new Label();
            lblAnswerA = new Label();
            picAnswerB = new PictureBox();
            picAnswerD = new PictureBox();
            picAnswerC = new PictureBox();
            picAnswerA = new PictureBox();
            lblQuestion = new GoldenQuestionLabel();
            picFlag = new PictureBox();
            picLifelineButton = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picQuestionTimer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSkipButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picStopButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerD).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerC).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picFlag).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLifelineButton).BeginInit();
            SuspendLayout();
            // 
            // picQuestionTimer
            // 
            picQuestionTimer.BackColor = Color.Transparent;
            picQuestionTimer.BackgroundImage = Properties.Resources.flags_question_time;
            picQuestionTimer.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestionTimer.Cursor = Cursors.Hand;
            picQuestionTimer.Location = new Point(845, -1);
            picQuestionTimer.Name = "picQuestionTimer";
            picQuestionTimer.Size = new Size(261, 111);
            picQuestionTimer.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestionTimer.TabIndex = 16;
            picQuestionTimer.TabStop = false;
            // 
            // lblQuestionTimer
            // 
            lblQuestionTimer.BackColor = Color.Transparent;
            lblQuestionTimer.Font = new Font("Cooper Black", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblQuestionTimer.Location = new Point(945, 54);
            lblQuestionTimer.Name = "lblQuestionTimer";
            lblQuestionTimer.Size = new Size(53, 46);
            lblQuestionTimer.TabIndex = 15;
            lblQuestionTimer.Text = "5";
            lblQuestionTimer.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblQuestionCounter
            // 
            lblQuestionCounter.BackColor = Color.Transparent;
            lblQuestionCounter.Font = new Font("Cooper Black", 26.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblQuestionCounter.ForeColor = Color.White;
            lblQuestionCounter.Location = new Point(228, 275);
            lblQuestionCounter.Name = "lblQuestionCounter";
            lblQuestionCounter.Size = new Size(216, 69);
            lblQuestionCounter.TabIndex = 14;
            lblQuestionCounter.Text = "1 - 00";
            lblQuestionCounter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblGameTimer
            // 
            lblGameTimer.BackColor = Color.Transparent;
            lblGameTimer.Font = new Font("Cooper Black", 26.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblGameTimer.Location = new Point(1624, 161);
            lblGameTimer.Name = "lblGameTimer";
            lblGameTimer.Size = new Size(216, 69);
            lblGameTimer.TabIndex = 13;
            lblGameTimer.Text = "60 Sec";
            lblGameTimer.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblScore
            // 
            lblScore.BackColor = Color.Transparent;
            lblScore.Font = new Font("Cooper Black", 26.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblScore.Location = new Point(1624, 337);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(216, 69);
            lblScore.TabIndex = 12;
            lblScore.Text = "Score";
            lblScore.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picSkipButton
            // 
            picSkipButton.BackColor = Color.Transparent;
            picSkipButton.BackgroundImageLayout = ImageLayout.Stretch;
            picSkipButton.Cursor = Cursors.Hand;
            picSkipButton.Image = Properties.Resources.skip_button;
            picSkipButton.Location = new Point(1015, 973);
            picSkipButton.Name = "picSkipButton";
            picSkipButton.Size = new Size(284, 107);
            picSkipButton.SizeMode = PictureBoxSizeMode.Zoom;
            picSkipButton.TabIndex = 11;
            picSkipButton.TabStop = false;
            picSkipButton.Click += picSkipButton_Click;
            picSkipButton.MouseEnter += picSkipButton_MouseEnter;
            picSkipButton.MouseLeave += picSkipButton_MouseLeave;
            // 
            // picStopButton
            // 
            picStopButton.BackColor = Color.Transparent;
            picStopButton.BackgroundImageLayout = ImageLayout.Stretch;
            picStopButton.Cursor = Cursors.Hand;
            picStopButton.Image = Properties.Resources.stop_button;
            picStopButton.Location = new Point(21, 970);
            picStopButton.Name = "picStopButton";
            picStopButton.Size = new Size(284, 107);
            picStopButton.SizeMode = PictureBoxSizeMode.Zoom;
            picStopButton.TabIndex = 10;
            picStopButton.TabStop = false;
            picStopButton.Click += picStopButton_Click;
            picStopButton.MouseEnter += picStopButton_MouseEnter;
            picStopButton.MouseLeave += picStopButton_MouseLeave;
            // 
            // lblAnswerB
            // 
            lblAnswerB.BackColor = Color.Transparent;
            lblAnswerB.Font = new Font("Cooper Black", 38.25F, FontStyle.Italic);
            lblAnswerB.Location = new Point(1258, 690);
            lblAnswerB.Name = "lblAnswerB";
            lblAnswerB.Size = new Size(522, 89);
            lblAnswerB.TabIndex = 8;
            lblAnswerB.Text = "Awnser B";
            lblAnswerB.TextAlign = ContentAlignment.MiddleCenter;
            lblAnswerB.Click += Answer_Click;
            lblAnswerB.MouseEnter += AnswerLabel_MouseEnter;
            lblAnswerB.MouseLeave += AnswerLabel_MouseLeave;
            // 
            // lblAnswerD
            // 
            lblAnswerD.BackColor = Color.Transparent;
            lblAnswerD.Font = new Font("Cooper Black", 38.25F, FontStyle.Italic);
            lblAnswerD.Location = new Point(1258, 843);
            lblAnswerD.Name = "lblAnswerD";
            lblAnswerD.Size = new Size(522, 89);
            lblAnswerD.TabIndex = 9;
            lblAnswerD.Text = "Awnser D";
            lblAnswerD.TextAlign = ContentAlignment.MiddleCenter;
            lblAnswerD.Click += Answer_Click;
            lblAnswerD.MouseEnter += AnswerLabel_MouseEnter;
            lblAnswerD.MouseLeave += AnswerLabel_MouseLeave;
            // 
            // lblAnswerC
            // 
            lblAnswerC.BackColor = Color.Transparent;
            lblAnswerC.Font = new Font("Cooper Black", 38.25F, FontStyle.Italic);
            lblAnswerC.Location = new Point(259, 843);
            lblAnswerC.Name = "lblAnswerC";
            lblAnswerC.Size = new Size(522, 89);
            lblAnswerC.TabIndex = 7;
            lblAnswerC.Text = "Awnser C";
            lblAnswerC.TextAlign = ContentAlignment.MiddleCenter;
            lblAnswerC.Click += Answer_Click;
            lblAnswerC.MouseEnter += AnswerLabel_MouseEnter;
            lblAnswerC.MouseLeave += AnswerLabel_MouseLeave;
            // 
            // lblAnswerA
            // 
            lblAnswerA.BackColor = Color.Transparent;
            lblAnswerA.Font = new Font("Cooper Black", 38.25F, FontStyle.Italic);
            lblAnswerA.Location = new Point(259, 690);
            lblAnswerA.Name = "lblAnswerA";
            lblAnswerA.Size = new Size(522, 89);
            lblAnswerA.TabIndex = 6;
            lblAnswerA.Text = "Awnser A";
            lblAnswerA.TextAlign = ContentAlignment.MiddleCenter;
            lblAnswerA.Click += Answer_Click;
            lblAnswerA.MouseEnter += AnswerLabel_MouseEnter;
            lblAnswerA.MouseLeave += AnswerLabel_MouseLeave;
            // 
            // picAnswerB
            // 
            picAnswerB.BackColor = Color.Transparent;
            picAnswerB.BackgroundImageLayout = ImageLayout.Stretch;
            picAnswerB.Cursor = Cursors.Hand;
            picAnswerB.Image = Properties.Resources.flags_answer_b;
            picAnswerB.Location = new Point(1137, 670);
            picAnswerB.Name = "picAnswerB";
            picAnswerB.Size = new Size(643, 127);
            picAnswerB.SizeMode = PictureBoxSizeMode.Zoom;
            picAnswerB.TabIndex = 4;
            picAnswerB.TabStop = false;
            picAnswerB.Click += Answer_Click;
            picAnswerB.MouseEnter += Answer_MouseEnter;
            picAnswerB.MouseLeave += Answer_MouseLeave;
            // 
            // picAnswerD
            // 
            picAnswerD.BackColor = Color.Transparent;
            picAnswerD.BackgroundImageLayout = ImageLayout.Stretch;
            picAnswerD.Cursor = Cursors.Hand;
            picAnswerD.Image = Properties.Resources.flags_answer_d;
            picAnswerD.Location = new Point(1137, 822);
            picAnswerD.Name = "picAnswerD";
            picAnswerD.Size = new Size(643, 127);
            picAnswerD.SizeMode = PictureBoxSizeMode.Zoom;
            picAnswerD.TabIndex = 5;
            picAnswerD.TabStop = false;
            picAnswerD.Click += Answer_Click;
            picAnswerD.MouseEnter += Answer_MouseEnter;
            picAnswerD.MouseLeave += Answer_MouseLeave;
            // 
            // picAnswerC
            // 
            picAnswerC.BackColor = Color.Transparent;
            picAnswerC.BackgroundImageLayout = ImageLayout.Stretch;
            picAnswerC.Cursor = Cursors.Hand;
            picAnswerC.Image = Properties.Resources.flags_answer_c;
            picAnswerC.Location = new Point(138, 822);
            picAnswerC.Name = "picAnswerC";
            picAnswerC.Size = new Size(643, 127);
            picAnswerC.SizeMode = PictureBoxSizeMode.Zoom;
            picAnswerC.TabIndex = 3;
            picAnswerC.TabStop = false;
            picAnswerC.Click += Answer_Click;
            picAnswerC.MouseEnter += Answer_MouseEnter;
            picAnswerC.MouseLeave += Answer_MouseLeave;
            // 
            // picAnswerA
            // 
            picAnswerA.BackColor = Color.Transparent;
            picAnswerA.BackgroundImageLayout = ImageLayout.Stretch;
            picAnswerA.Cursor = Cursors.Hand;
            picAnswerA.Image = Properties.Resources.flags_answer_a;
            picAnswerA.Location = new Point(138, 670);
            picAnswerA.Name = "picAnswerA";
            picAnswerA.Size = new Size(643, 127);
            picAnswerA.SizeMode = PictureBoxSizeMode.Zoom;
            picAnswerA.TabIndex = 2;
            picAnswerA.TabStop = false;
            picAnswerA.Click += Answer_Click;
            picAnswerA.MouseEnter += Answer_MouseEnter;
            picAnswerA.MouseLeave += Answer_MouseLeave;
            // 
            // lblQuestion
            // 
            lblQuestion.BackColor = Color.Transparent;
            lblQuestion.Font = new Font("Cooper Black", 54.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblQuestion.IsGolden = false;
            lblQuestion.Location = new Point(177, 520);
            lblQuestion.Name = "lblQuestion";
            lblQuestion.Size = new Size(1642, 110);
            lblQuestion.TabIndex = 1;
            lblQuestion.Text = "    Which country does this flag belong to?";
            lblQuestion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picFlag
            // 
            picFlag.BackColor = Color.Transparent;
            picFlag.BackgroundImageLayout = ImageLayout.Stretch;
            picFlag.Location = new Point(597, 134);
            picFlag.Name = "picFlag";
            picFlag.Size = new Size(751, 313);
            picFlag.SizeMode = PictureBoxSizeMode.Zoom;
            picFlag.TabIndex = 0;
            picFlag.TabStop = false;
            // 
            // picLifelineButton
            // 
            picLifelineButton.BackColor = Color.Transparent;
            picLifelineButton.BackgroundImageLayout = ImageLayout.Stretch;
            picLifelineButton.Cursor = Cursors.Hand;
            picLifelineButton.Image = Properties.Resources.lifeline_button;
            picLifelineButton.Location = new Point(622, 970);
            picLifelineButton.Name = "picLifelineButton";
            picLifelineButton.Size = new Size(284, 107);
            picLifelineButton.SizeMode = PictureBoxSizeMode.Zoom;
            picLifelineButton.TabIndex = 17;
            picLifelineButton.TabStop = false;
            picLifelineButton.Click += picLifelineButton_Click;
            picLifelineButton.MouseEnter += picLifelineButton_MouseEnter;
            picLifelineButton.MouseLeave += picLifelineButton_MouseLeave;
            // 
            // FlagsQuiz
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.flags_quiz_background;
            BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(picLifelineButton);
            Controls.Add(lblQuestionTimer);
            Controls.Add(lblQuestionCounter);
            Controls.Add(lblGameTimer);
            Controls.Add(lblScore);
            Controls.Add(picSkipButton);
            Controls.Add(picStopButton);
            Controls.Add(lblAnswerD);
            Controls.Add(lblAnswerB);
            Controls.Add(lblAnswerC);
            Controls.Add(lblAnswerA);
            Controls.Add(picAnswerD);
            Controls.Add(picAnswerB);
            Controls.Add(picAnswerC);
            Controls.Add(picAnswerA);
            Controls.Add(lblQuestion);
            Controls.Add(picFlag);
            Controls.Add(picQuestionTimer);
            Name = "FlagsQuiz";
            Size = new Size(1920, 1080);
            Load += FlagsQuiz_Load;
            ((System.ComponentModel.ISupportInitialize)picQuestionTimer).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSkipButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)picStopButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerB).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerD).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerC).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerA).EndInit();
            ((System.ComponentModel.ISupportInitialize)picFlag).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLifelineButton).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox picQuestionTimer;
        private Label lblQuestionTimer;
        private Label lblQuestionCounter;
        private Label lblGameTimer;
        private Label lblScore;
        private PictureBox picSkipButton;
        private PictureBox picStopButton;
        private Label lblAnswerB;
        private Label lblAnswerD;
        private Label lblAnswerC;
        private Label lblAnswerA;
        private PictureBox picAnswerB;
        private PictureBox picAnswerD;
        private PictureBox picAnswerC;
        private PictureBox picAnswerA;
        private GoldenQuestionLabel lblQuestion;
        private PictureBox picFlag;
        private PictureBox picLifelineButton;
    }
}