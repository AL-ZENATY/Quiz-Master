namespace QuizMaster.Forms
{
    partial class RapidFire
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
            lblQuestionTimer = new Label();
            lblQuestionCounter = new Label();
            lblStopWatch = new Label();
            lblTimeAdjustment = new Label();
            picSkipButton = new PictureBox();
            picStopButton = new PictureBox();
            lblAnswerD = new Label();
            lblAnswerB = new Label();
            lblAnswerC = new Label();
            lblAnswerA = new Label();
            picAnswerD = new PictureBox();
            picAnswerB = new PictureBox();
            picAnswerC = new PictureBox();
            picAnswerA = new PictureBox();
            lblQuestion = new GoldenQuestionLabel();
            picQuestionImage = new PictureBox();
            picQuestionTimer = new PictureBox();
            picLifelineButton = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picSkipButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picStopButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerD).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerC).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picQuestionImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picQuestionTimer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLifelineButton).BeginInit();
            SuspendLayout();
            // 
            // lblQuestionTimer
            // 
            lblQuestionTimer.BackColor = Color.Transparent;
            lblQuestionTimer.Font = new Font("Cooper Black", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblQuestionTimer.ForeColor = Color.Black;
            lblQuestionTimer.Location = new Point(944, 53);
            lblQuestionTimer.Name = "lblQuestionTimer";
            lblQuestionTimer.Size = new Size(53, 46);
            lblQuestionTimer.TabIndex = 49;
            lblQuestionTimer.Text = "5";
            lblQuestionTimer.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblQuestionCounter
            // 
            lblQuestionCounter.BackColor = Color.Transparent;
            lblQuestionCounter.Font = new Font("Cooper Black", 26.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblQuestionCounter.ForeColor = Color.White;
            lblQuestionCounter.Location = new Point(226, 275);
            lblQuestionCounter.Name = "lblQuestionCounter";
            lblQuestionCounter.Size = new Size(216, 69);
            lblQuestionCounter.TabIndex = 48;
            lblQuestionCounter.Text = "0 / 10";
            lblQuestionCounter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStopWatch
            // 
            lblStopWatch.BackColor = Color.Transparent;
            lblStopWatch.Font = new Font("Cooper Black", 26.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblStopWatch.Location = new Point(1627, 160);
            lblStopWatch.Name = "lblStopWatch";
            lblStopWatch.Size = new Size(216, 69);
            lblStopWatch.TabIndex = 47;
            lblStopWatch.Text = "00 : 00 : 00";
            lblStopWatch.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTimeAdjustment
            // 
            lblTimeAdjustment.BackColor = Color.Transparent;
            lblTimeAdjustment.Font = new Font("Cooper Black", 26.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTimeAdjustment.Location = new Point(1625, 339);
            lblTimeAdjustment.Name = "lblTimeAdjustment";
            lblTimeAdjustment.Size = new Size(216, 69);
            lblTimeAdjustment.TabIndex = 46;
            lblTimeAdjustment.Text = "0";
            lblTimeAdjustment.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picSkipButton
            // 
            picSkipButton.BackColor = Color.Transparent;
            picSkipButton.BackgroundImageLayout = ImageLayout.Stretch;
            picSkipButton.Cursor = Cursors.Hand;
            picSkipButton.Image = Properties.Resources.skip_button;
            picSkipButton.Location = new Point(1013, 972);
            picSkipButton.Name = "picSkipButton";
            picSkipButton.Size = new Size(284, 107);
            picSkipButton.SizeMode = PictureBoxSizeMode.Zoom;
            picSkipButton.TabIndex = 45;
            picSkipButton.TabStop = false;
            picSkipButton.Click += SkipButton_Click;
            picSkipButton.MouseEnter += SkipButton_MouseEnter;
            picSkipButton.MouseLeave += SkipButton_MouseLeave;
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
            picStopButton.TabIndex = 44;
            picStopButton.TabStop = false;
            picStopButton.Click += StopButton_Click;
            picStopButton.MouseEnter += StopButton_MouseEnter;
            picStopButton.MouseLeave += StopButton_MouseLeave;
            // 
            // lblAnswerD
            // 
            lblAnswerD.BackColor = Color.Transparent;
            lblAnswerD.Font = new Font("Cooper Black", 38.25F, FontStyle.Italic);
            lblAnswerD.Location = new Point(1256, 843);
            lblAnswerD.Name = "lblAnswerD";
            lblAnswerD.Size = new Size(522, 89);
            lblAnswerD.TabIndex = 43;
            lblAnswerD.Text = "Awnser D";
            lblAnswerD.TextAlign = ContentAlignment.MiddleCenter;
            lblAnswerD.Click += Answer_Click;
            lblAnswerD.MouseEnter += AnswerLabel_MouseEnter;
            lblAnswerD.MouseLeave += AnswerLabel_MouseLeave;
            // 
            // lblAnswerB
            // 
            lblAnswerB.BackColor = Color.Transparent;
            lblAnswerB.Font = new Font("Cooper Black", 38.25F, FontStyle.Italic);
            lblAnswerB.Location = new Point(1256, 689);
            lblAnswerB.Name = "lblAnswerB";
            lblAnswerB.Size = new Size(522, 89);
            lblAnswerB.TabIndex = 42;
            lblAnswerB.Text = "Awnser B";
            lblAnswerB.TextAlign = ContentAlignment.MiddleCenter;
            lblAnswerB.Click += Answer_Click;
            lblAnswerB.MouseEnter += AnswerLabel_MouseEnter;
            lblAnswerB.MouseLeave += AnswerLabel_MouseLeave;
            // 
            // lblAnswerC
            // 
            lblAnswerC.BackColor = Color.Transparent;
            lblAnswerC.Font = new Font("Cooper Black", 38.25F, FontStyle.Italic);
            lblAnswerC.Location = new Point(257, 843);
            lblAnswerC.Name = "lblAnswerC";
            lblAnswerC.Size = new Size(522, 89);
            lblAnswerC.TabIndex = 41;
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
            lblAnswerA.Location = new Point(257, 689);
            lblAnswerA.Name = "lblAnswerA";
            lblAnswerA.Size = new Size(522, 89);
            lblAnswerA.TabIndex = 40;
            lblAnswerA.Text = "Awnser A";
            lblAnswerA.TextAlign = ContentAlignment.MiddleCenter;
            lblAnswerA.Click += Answer_Click;
            lblAnswerA.MouseEnter += AnswerLabel_MouseEnter;
            lblAnswerA.MouseLeave += AnswerLabel_MouseLeave;
            // 
            // picAnswerD
            // 
            picAnswerD.BackColor = Color.Transparent;
            picAnswerD.BackgroundImageLayout = ImageLayout.Stretch;
            picAnswerD.Cursor = Cursors.Hand;
            picAnswerD.Image = Properties.Resources.rapidfire_answer_d;
            picAnswerD.Location = new Point(1135, 821);
            picAnswerD.Name = "picAnswerD";
            picAnswerD.Size = new Size(643, 127);
            picAnswerD.SizeMode = PictureBoxSizeMode.Zoom;
            picAnswerD.TabIndex = 39;
            picAnswerD.TabStop = false;
            picAnswerD.Click += Answer_Click;
            picAnswerD.MouseEnter += Answer_MouseEnter;
            picAnswerD.MouseLeave += Answer_MouseLeave;
            // 
            // picAnswerB
            // 
            picAnswerB.BackColor = Color.Transparent;
            picAnswerB.BackgroundImageLayout = ImageLayout.Stretch;
            picAnswerB.Cursor = Cursors.Hand;
            picAnswerB.Image = Properties.Resources.rapidfire_answer_b;
            picAnswerB.Location = new Point(1135, 669);
            picAnswerB.Name = "picAnswerB";
            picAnswerB.Size = new Size(643, 127);
            picAnswerB.SizeMode = PictureBoxSizeMode.Zoom;
            picAnswerB.TabIndex = 38;
            picAnswerB.TabStop = false;
            picAnswerB.Click += Answer_Click;
            picAnswerB.MouseEnter += Answer_MouseEnter;
            picAnswerB.MouseLeave += Answer_MouseLeave;
            // 
            // picAnswerC
            // 
            picAnswerC.BackColor = Color.Transparent;
            picAnswerC.BackgroundImageLayout = ImageLayout.Stretch;
            picAnswerC.Cursor = Cursors.Hand;
            picAnswerC.Image = Properties.Resources.rapidfire_answer_c;
            picAnswerC.Location = new Point(136, 821);
            picAnswerC.Name = "picAnswerC";
            picAnswerC.Size = new Size(643, 127);
            picAnswerC.SizeMode = PictureBoxSizeMode.Zoom;
            picAnswerC.TabIndex = 37;
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
            picAnswerA.ErrorImage = Properties.Resources.cars_answer_a;
            picAnswerA.Image = Properties.Resources.rapidfire_answer_a;
            picAnswerA.Location = new Point(136, 669);
            picAnswerA.Name = "picAnswerA";
            picAnswerA.Size = new Size(643, 127);
            picAnswerA.SizeMode = PictureBoxSizeMode.Zoom;
            picAnswerA.TabIndex = 36;
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
            lblQuestion.Location = new Point(175, 519);
            lblQuestion.Name = "lblQuestion";
            lblQuestion.Size = new Size(1642, 110);
            lblQuestion.TabIndex = 35;
            lblQuestion.Text = "Which car is this?";
            lblQuestion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picQuestionImage
            // 
            picQuestionImage.BackColor = Color.Transparent;
            picQuestionImage.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestionImage.Location = new Point(595, 133);
            picQuestionImage.Name = "picQuestionImage";
            picQuestionImage.Size = new Size(751, 313);
            picQuestionImage.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestionImage.TabIndex = 34;
            picQuestionImage.TabStop = false;
            // 
            // picQuestionTimer
            // 
            picQuestionTimer.BackColor = Color.Transparent;
            picQuestionTimer.BackgroundImage = Properties.Resources.rapidfire_question_time;
            picQuestionTimer.BackgroundImageLayout = ImageLayout.Stretch;
            picQuestionTimer.Cursor = Cursors.Hand;
            picQuestionTimer.Location = new Point(843, -2);
            picQuestionTimer.Name = "picQuestionTimer";
            picQuestionTimer.Size = new Size(261, 111);
            picQuestionTimer.SizeMode = PictureBoxSizeMode.Zoom;
            picQuestionTimer.TabIndex = 50;
            picQuestionTimer.TabStop = false;
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
            picLifelineButton.TabIndex = 51;
            picLifelineButton.TabStop = false;
            picLifelineButton.Click += picLifelineButton_Click;
            picLifelineButton.MouseEnter += picLifelineButton_MouseEnter;
            picLifelineButton.MouseLeave += picLifelineButton_MouseLeave;
            // 
            // RapidFire
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            BackgroundImage = Properties.Resources.rapidfire_quiz_background;
            BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(picLifelineButton);
            Controls.Add(lblQuestionTimer);
            Controls.Add(lblQuestionCounter);
            Controls.Add(lblStopWatch);
            Controls.Add(lblTimeAdjustment);
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
            Controls.Add(picQuestionImage);
            Controls.Add(picQuestionTimer);
            Name = "RapidFire";
            Size = new Size(1920, 1080);
            ((System.ComponentModel.ISupportInitialize)picSkipButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)picStopButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerD).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerB).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerC).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAnswerA).EndInit();
            ((System.ComponentModel.ISupportInitialize)picQuestionImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)picQuestionTimer).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLifelineButton).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblQuestionTimer;
        private Label lblQuestionCounter;
        private Label lblStopWatch;
        private Label lblTimeAdjustment;
        private PictureBox picSkipButton;
        private PictureBox picStopButton;
        private Label lblAnswerD;
        private Label lblAnswerB;
        private Label lblAnswerC;
        private Label lblAnswerA;
        private PictureBox picAnswerD;
        private PictureBox picAnswerB;
        private PictureBox picAnswerC;
        private PictureBox picAnswerA;
        private GoldenQuestionLabel lblQuestion;
        private PictureBox picQuestionImage;
        private PictureBox picQuestionTimer;
        private PictureBox picLifelineButton;
    }
}
