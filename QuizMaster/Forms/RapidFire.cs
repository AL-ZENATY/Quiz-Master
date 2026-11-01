
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuizMaster.Forms
{
    public partial class RapidFire : UserControl
    {
        // ==========================================
        // RAPID FIRE COMPONENTS
        // ==========================================

        private readonly RapidFireEngine engine;
        private readonly RapidFireTimer quizTimer;
        private readonly System.Windows.Forms.Timer adjustmentTimer =
        new System.Windows.Forms.Timer();

        // ==========================================
        // QUIZ STATE
        // ==========================================

        private char correctAnswerLetter = 'A';

        private bool answerLocked = true;
        private bool quizFinished;
        private bool quizPaused;
        private bool showingFeedback;
        private bool startingCountdown = true;
        private bool endingCountdown;
        private bool lifelineUsed;

        private bool completedSuccessfully;
        private bool newWorldRecord;
        private double finalTimeSeconds;

        // ==========================================
        // STYLING
        // ==========================================

        private readonly Color normalQuestionColor;

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public RapidFire()
        {
            InitializeComponent();

            normalQuestionColor = lblQuestion.ForeColor;

            SetupAnswerLabels();
            AttachLabel(lblQuestionTimer, picQuestionTimer);

            // Time adjustment feedback lasts 3 seconds.
            adjustmentTimer.Interval = 3000;
            adjustmentTimer.Tick += AdjustmentTimer_Tick;

            // Select 20 Flags questions.
            List<QuizQuestion> flags = QuizDatabase
                .GetQuestions("Flags")
                .OrderBy(_ => Random.Shared.Next())
                .Take(20)
                .ToList();

            // Select 20 Cars questions.
            List<QuizQuestion> cars = QuizDatabase
                .GetQuestions("Cars")
                .OrderBy(_ => Random.Shared.Next())
                .Take(20)
                .ToList();

            // Shuffle both categories together.
            List<QuizQuestion> questions = flags
                .Concat(cars)
                .OrderBy(_ => Random.Shared.Next())
                .ToList();

            engine = new RapidFireEngine(questions);
            quizTimer = new RapidFireTimer();

            // Connect timer events.
            quizTimer.CountdownUpdated += UpdateCountdownDisplay;
            quizTimer.StartingFinished += StartGameplay;
            quizTimer.QuestionExpired += QuestionTimedOut;
            quizTimer.FeedbackFinished += AdvanceQuestion;
            quizTimer.EndingFinished += EndingFinished;

            Disposed += RapidFire_Disposed;

            if (flags.Count == 20 && cars.Count == 20)
            {
                PrepareStartingCountdown();
                quizTimer.Start();
            }
            else
            {
                startingCountdown = false;

                lblQuestion.Text =
                    $"Not enough questions: " +
                    $"{flags.Count}/20 Flags, " +
                    $"{cars.Count}/20 Cars.";

                ClearQuestionImages();
                ClearAnswerTexts();
                UpdateQuizLabels();
                UpdateTimerLabels();
            }
        }

        // ==========================================
        // CLEANUP
        // ==========================================

        private void RapidFire_Disposed(
            object? sender,
            EventArgs e)
        {
            adjustmentTimer.Stop();
            adjustmentTimer.Dispose();

            quizTimer.Dispose();
        }

        // ==========================================
        // ANSWER LABEL SETUP
        // ==========================================

        private void SetupAnswerLabels()
        {
            AttachLabel(lblAnswerA, picAnswerA);
            AttachLabel(lblAnswerB, picAnswerB);
            AttachLabel(lblAnswerC, picAnswerC);
            AttachLabel(lblAnswerD, picAnswerD);
        }

        private void AttachLabel(
            Label label,
            PictureBox picture)
        {
            Point position = picture.PointToClient(
                label.PointToScreen(Point.Empty));

            label.Parent = picture;
            label.Location = position;
            label.BackColor = Color.Transparent;
            label.BringToFront();
        }

        private void ClearAnswerTexts()
        {
            lblAnswerA.Text = "";
            lblAnswerB.Text = "";
            lblAnswerC.Text = "";
            lblAnswerD.Text = "";
        }

        // ==========================================
        // STARTING COUNTDOWN
        // ==========================================

        private void PrepareStartingCountdown()
        {
            startingCountdown = true;
            answerLocked = true;
            showingFeedback = false;

            lblQuestion.IsGolden = false;
            lblQuestion.ForeColor = normalQuestionColor;

            ClearQuestionImages();
            ClearAnswerTexts();

            lblQuestion.Text = "3";
            lblQuestionTimer.Text = "5";

            lblStopWatch.Text = "00:00.00";

            adjustmentTimer.Stop();
            ShowAdjustmentTotal();

            UpdateQuizLabels();
        }

        private void StartGameplay()
        {
            if (quizFinished || IsDisposed)
                return;

            startingCountdown = false;

            DisplayCurrentQuestion();
        }

        private void UpdateCountdownDisplay()
        {
            if (IsDisposed)
                return;

            if (startingCountdown)
            {
                lblQuestion.Text = Math.Max(
                    1,
                    (int)Math.Ceiling(
                        quizTimer.StartTimeRemaining)
                ).ToString();

                return;
            }

            if (!endingCountdown)
                UpdateTimerLabels();
        }

        // ==========================================
        // DISPLAY CURRENT QUESTION
        // ==========================================

        private void DisplayCurrentQuestion()
        {
            if (!engine.HasMoreQuestions)
            {
                FinishQuiz(false);
                return;
            }

            QuizQuestion question = engine.CurrentQuestion;

            lblQuestion.Text = question.QuestionText;

            lblQuestion.IsGolden = engine.IsGoldenQuestion;

            // Play the sound when a Golden Question appears.
            if (engine.IsGoldenQuestion)
            {
                using System.Media.SoundPlayer player = new(
                    Properties.Resources.golden_question_sound);

                player.Play();
            }

            lblQuestion.ForeColor = engine.IsGoldenQuestion
                ? Color.Gold
                : normalQuestionColor;

            lblQuestion.Invalidate();

            DisplayQuestionImage(question);

            string[] answers = engine.ShuffleAnswers();

            lblAnswerA.Text = answers[0];
            lblAnswerB.Text = answers[1];
            lblAnswerC.Text = answers[2];
            lblAnswerD.Text = answers[3];

            // Restore normal cursors for each new question.
            foreach (Label label in new[]
            {
                lblAnswerA, lblAnswerB, lblAnswerC, lblAnswerD
            })
            {
                label.Cursor = Cursors.Hand;

                if (label.Parent is PictureBox button)
                    button.Cursor = Cursors.Hand;
            }

            int correctIndex = Array.IndexOf(
                answers,
                question.CorrectAnswer);

            correctAnswerLetter = (char)('A' + correctIndex);

            answerLocked = false;
            showingFeedback = false;

            ResetAnswerImages();

            quizTimer.StartQuestion();

            UpdateQuizLabels();
            UpdateTimerLabels();
        }

        // ==========================================
        // QUESTION IMAGE
        // ==========================================

        private void DisplayQuestionImage(
            QuizQuestion question)
        {
            picQuestionImage.Image = null;

            if (string.IsNullOrWhiteSpace(question.ImagePath))
                return;

            string resourceName =
                Path.GetFileNameWithoutExtension(
                    question.ImagePath.Replace('\\', '/'));

            if (string.IsNullOrWhiteSpace(resourceName))
                return;

            picQuestionImage.Image =
                Properties.Resources.ResourceManager.GetObject(
                    resourceName) as Image;

            picQuestionImage.SizeMode =
                PictureBoxSizeMode.Zoom;
        }

        private void ClearQuestionImages()
        {
            picQuestionImage.Image = null;
            ResetAnswerImages();
        }

        // ==========================================
        // ANSWER BUTTON IMAGES
        // ==========================================

        private void ResetAnswerImages()
        {
            picAnswerA.Image =
                Properties.Resources.rapidfire_answer_a;

            picAnswerB.Image =
                Properties.Resources.rapidfire_answer_b;

            picAnswerC.Image =
                Properties.Resources.rapidfire_answer_c;

            picAnswerD.Image =
                Properties.Resources.rapidfire_answer_d;
        }

        private void SetAnswerHover(
            PictureBox button,
            bool hovered)
        {
            if (button == picAnswerA)
            {
                button.Image = hovered
                    ? Properties.Resources.rapidfire_answer_a_hover
                    : Properties.Resources.rapidfire_answer_a;
            }
            else if (button == picAnswerB)
            {
                button.Image = hovered
                    ? Properties.Resources.rapidfire_answer_b_hover
                    : Properties.Resources.rapidfire_answer_b;
            }
            else if (button == picAnswerC)
            {
                button.Image = hovered
                    ? Properties.Resources.rapidfire_answer_c_hover
                    : Properties.Resources.rapidfire_answer_c;
            }
            else if (button == picAnswerD)
            {
                button.Image = hovered
                    ? Properties.Resources.rapidfire_answer_d_hover
                    : Properties.Resources.rapidfire_answer_d;
            }
        }

        private void SetCorrectAnswerImage(char answer)
        {
            switch (answer)
            {
                case 'A':
                    picAnswerA.Image =
                        Properties.Resources.answer_a_correct;
                    break;

                case 'B':
                    picAnswerB.Image =
                        Properties.Resources.answer_b_correct;
                    break;

                case 'C':
                    picAnswerC.Image =
                        Properties.Resources.answer_c_correct;
                    break;

                case 'D':
                    picAnswerD.Image =
                        Properties.Resources.answer_d_correct;
                    break;
            }
        }

        private void SetWrongAnswerImage(char answer)
        {
            switch (answer)
            {
                case 'A':
                    picAnswerA.Image =
                        Properties.Resources.answer_a_wrong;
                    break;

                case 'B':
                    picAnswerB.Image =
                        Properties.Resources.answer_b_wrong;
                    break;

                case 'C':
                    picAnswerC.Image =
                        Properties.Resources.answer_c_wrong;
                    break;

                case 'D':
                    picAnswerD.Image =
                        Properties.Resources.answer_d_wrong;
                    break;
            }
        }

        // ==========================================
        // ANSWER INTERACTION
        // ==========================================

        private bool CanInteract()
        {
            return !answerLocked
                && !quizPaused
                && !quizFinished
                && !startingCountdown
                && !endingCountdown;
        }

        private PictureBox? GetAnswerButton(object sender)
        {
            if (sender is PictureBox picture)
                return picture;

            if (sender is Label label)
                return label.Parent as PictureBox;

            return null;
        }

        private char GetAnswerLetter(PictureBox button)
        {
            if (button == picAnswerA) return 'A';
            if (button == picAnswerB) return 'B';
            if (button == picAnswerC) return 'C';
            if (button == picAnswerD) return 'D';

            return '\0';
        }

        // ==========================================
        // ANSWER HOVER EFFECTS
        // ==========================================

        private void Answer_MouseEnter(
            object sender,
            EventArgs e)
        {
            if (!CanInteract())
                return;

            PictureBox? button = GetAnswerButton(sender);

            if (button == null)
                return;

            Label? answerLabel = GetAnswerLetter(button) switch
            {
                'A' => lblAnswerA,
                'B' => lblAnswerB,
                'C' => lblAnswerC,
                'D' => lblAnswerD,
                _ => null
            };

            // Eliminated answers cannot show hover effects.
            if (answerLabel == null ||
                string.IsNullOrWhiteSpace(answerLabel.Text))
                return;

            SetAnswerHover(button, true);
        }

        private void Answer_MouseLeave(
            object sender,
            EventArgs e)
        {
            if (!CanInteract())
                return;

            PictureBox? button = GetAnswerButton(sender);

            if (button != null)
                SetAnswerHover(button, false);
        }

        private void AnswerLabel_MouseEnter(
            object sender,
            EventArgs e)
        {
            Answer_MouseEnter(sender, e);
        }

        private void AnswerLabel_MouseLeave(
            object sender,
            EventArgs e)
        {
            Answer_MouseLeave(sender, e);
        }

        // ==========================================
        // ANSWER CLICK
        // ==========================================

        private void Answer_Click(
            object sender,
            EventArgs e)
        {
            if (!CanInteract() || !engine.HasMoreQuestions)
                return;

            PictureBox? button = GetAnswerButton(sender);

            if (button == null)
                return;

            char selectedAnswer = GetAnswerLetter(button);

            if (selectedAnswer == '\0')
                return;

            Label selectedLabel = selectedAnswer switch
            {
                'A' => lblAnswerA,
                'B' => lblAnswerB,
                'C' => lblAnswerC,
                'D' => lblAnswerD,
                _ => lblAnswerA
            };

            // Ignore clicks on eliminated answers.
            if (string.IsNullOrWhiteSpace(selectedLabel.Text))
                return;

            ShowAnswerFeedback(selectedAnswer);
        }

        // ==========================================
        // ANSWER FEEDBACK
        // ==========================================

        private void ShowAnswerFeedback(char selectedAnswer)
        {
            answerLocked = true;
            showingFeedback = true;

            string userAnswer = selectedAnswer switch
            {
                'A' => lblAnswerA.Text,
                'B' => lblAnswerB.Text,
                'C' => lblAnswerC.Text,
                'D' => lblAnswerD.Text,
                _ => ""
            };

            // Record the adjustment before answering.
            int previousAdjustment =
                engine.TimeAdjustmentSeconds;

            // Submit the answer and apply its adjustment.
            engine.SubmitAnswer(userAnswer);

            // Display the penalty or bonus immediately.
            ShowTimeAdjustment(previousAdjustment);

            quizTimer.StartFeedback();

            UpdateQuizLabels();
            ResetAnswerImages();

            SetCorrectAnswerImage(correctAnswerLetter);

            if (selectedAnswer != correctAnswerLetter)
                SetWrongAnswerImage(selectedAnswer);
        }

        // ==========================================
        // QUESTION TIMEOUT
        // ==========================================

        private void QuestionTimedOut()
        {
            if (quizFinished || quizPaused)
                return;

            answerLocked = true;

            // Remember the adjustment before the timeout.
            int previousAdjustment = engine.TimeAdjustmentSeconds;

            // Apply +2 seconds, or +1 for a Golden Question.
            engine.TimeoutQuestion();

            // Show the penalty in red for 3 seconds.
            ShowTimeAdjustment(previousAdjustment);

            AdvanceQuestion();
        }

        // ==========================================
        // NEXT QUESTION
        // ==========================================

        private void AdvanceQuestion()
        {
            if (quizFinished ||
                startingCountdown ||
                endingCountdown)
                return;

            // Finish after 10 correct answers.
            if (engine.IsCompleted)
            {
                FinishQuiz(true);
                return;
            }

            engine.AdvanceQuestion();

            if (!engine.HasMoreQuestions)
            {
                FinishQuiz(false);
                return;
            }

            DisplayCurrentQuestion();
        }

        // ==========================================
        // TIMER LABELS
        // ==========================================

        private void UpdateTimerLabels()
        {
            lblStopWatch.Text =
                FormatTime(quizTimer.StopwatchSeconds);

            int questionSeconds = (int)Math.Ceiling(
                Math.Max(
                    0,
                    quizTimer.QuestionTimeRemaining));

            lblQuestionTimer.Text =
                questionSeconds.ToString();

            bool blinkRed =
                ((Environment.TickCount64 / 250) % 2) == 0;

            lblQuestionTimer.ForeColor =
                questionSeconds <= 3 && !showingFeedback
                    ? (blinkRed ? Color.Red : Color.Black)
                    : Color.Black;
        }

        private string FormatTime(double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(
                Math.Max(0, seconds));

            return $"{(int)time.TotalMinutes:00}:" +
                   $"{time.Seconds:00}." +
                   $"{time.Milliseconds / 10:00}";
        }

        // ==========================================
        // TIME ADJUSTMENT DISPLAY
        // ==========================================

        private void ShowTimeAdjustment(int previousAdjustment)
        {
            int change =
                engine.TimeAdjustmentSeconds - previousAdjustment;

            if (change == 0)
                return;

            adjustmentTimer.Stop();

            lblTimeAdjustment.Text = change > 0
                ? $"+{change}"
                : change.ToString();

            lblTimeAdjustment.ForeColor = change > 0
                ? Color.Red
                : Color.Green;

            adjustmentTimer.Start();
        }

        private void AdjustmentTimer_Tick(
            object? sender,
            EventArgs e)
        {
            adjustmentTimer.Stop();

            ShowAdjustmentTotal();
        }

        private void ShowAdjustmentTotal()
        {
            lblTimeAdjustment.Text =
                engine.TimeAdjustmentSeconds.ToString();

            lblTimeAdjustment.ForeColor = Color.Black;
        }

        // ==========================================
        // QUIZ LABELS
        // ==========================================

        private void UpdateQuizLabels()
        {
            lblQuestionCounter.Text =
                $"{engine.CorrectAnswers} / " +
                $"{RapidFireEngine.RequiredCorrectAnswers}";
        }

        // ==========================================
        // FINISH QUIZ
        // ==========================================

        private void FinishQuiz(bool completed)
        {
            if (quizFinished)
                return;

            quizFinished = true;
            answerLocked = true;
            showingFeedback = false;
            endingCountdown = true;

            completedSuccessfully = completed;

            // Capture the stopwatch and calculate final time.
            double stopwatchSeconds =
                quizTimer.StopwatchSeconds;

            finalTimeSeconds =
                engine.CalculateFinalTime(stopwatchSeconds);

            // Check the previous world record before saving.
            if (completed && UserSession.UserId is int userId)
            {
                var previousRankings =
                    QuizDatabase.GetRapidFireLeaderboard(true);

                // The first entry contains the fastest recorded time.
                double? previousBestTime = previousRankings.Count > 0
                    ? previousRankings[0].Time
                    : null;

                // A first-ever completion also establishes a record.
                newWorldRecord = !previousBestTime.HasValue ||
                    finalTimeSeconds < previousBestTime.Value;

                QuizDatabase.SaveRapidFireResult(
                    userId,
                    finalTimeSeconds);
            }

            // Stop adjustment feedback and show the total.
            adjustmentTimer.Stop();
            ShowAdjustmentTotal();

            lblStopWatch.Text =
                FormatTime(stopwatchSeconds);

            lblQuestion.IsGolden = false;
            lblQuestion.ForeColor = normalQuestionColor;

            lblQuestion.Text = "Taking you to the results...";

            lblQuestion.Invalidate();

            picQuestionImage.Image = null;

            ClearAnswerTexts();

            quizTimer.StartEndingCountdown();
        }

        // ==========================================
        // ENDING COUNTDOWN FINISHED
        // ==========================================


        private void EndingFinished()
        {
            if (IsDisposed)
                return;

            endingCountdown = false;

            Control? container = Parent;

            if (container == null)
                return;

            // Count questions that ran out of time.
            int timedOutQuestions = engine.Results.Count(
                result => result.Status == "Timed out");

            // Create the Rapid Fire Results screen.
            RapidFireResults resultsScreen = new()
            {
                Dock = DockStyle.Fill
            };

            // Pass all recorded gameplay statistics.
            resultsScreen.LoadResults(
                engine.Results,
                quizTimer.StopwatchSeconds,
                engine.TimeAdjustmentSeconds,
                engine.WrongAnswers,
                timedOutQuestions,
                engine.SkipUsed,
                lifelineUsed,
                finalTimeSeconds,
                engine.GoldenQuestionIndex,
                newWorldRecord
            );

            // Display the Results screen.
            container.Controls.Add(resultsScreen);
            resultsScreen.BringToFront();

            // Remove the finished quiz.
            container.Controls.Remove(this);
            Dispose();
        }


        // ==========================================
        // STOP BUTTON HOVER
        // ==========================================

        private void StopButton_MouseEnter(
            object sender,
            EventArgs e)
        {
            picStopButton.Image =
                Properties.Resources.stop_button_hover;
        }

        private void StopButton_MouseLeave(
            object sender,
            EventArgs e)
        {
            picStopButton.Image =
                Properties.Resources.stop_button;
        }

        // ==========================================
        // STOP BUTTON CLICK
        // ==========================================

        private void StopButton_Click(
            object sender,
            EventArgs e)
        {
            if (quizPaused || IsDisposed)
                return;

            // Only pause the timer while gameplay is active.
            if (!quizFinished)
                quizTimer.Pause();

            quizPaused = true;

            ShowStopConfirmation();
        }

        // ==========================================
        // STOP CONFIRMATION OVERLAY
        // ==========================================

        private void ShowStopConfirmation()
        {
            int width = Math.Max(1, Width);
            int height = Math.Max(1, Height);

            using Bitmap screenshot =
                new Bitmap(width, height);

            DrawToBitmap(
                screenshot,
                new Rectangle(0, 0, width, height));

            int smallWidth = Math.Max(1, width / 12);
            int smallHeight = Math.Max(1, height / 12);

            using Bitmap small = new Bitmap(
                screenshot,
                new Size(smallWidth, smallHeight));

            Bitmap blurred = new Bitmap(width, height);

            using (Graphics graphics =
                Graphics.FromImage(blurred))
            {
                graphics.InterpolationMode =
                    InterpolationMode.HighQualityBilinear;

                graphics.DrawImage(
                    small,
                    new Rectangle(0, 0, width, height));

                using SolidBrush darkBrush =
                    new SolidBrush(
                        Color.FromArgb(170, Color.Black));

                graphics.FillRectangle(
                    darkBrush,
                    0,
                    0,
                    width,
                    height);
            }

            StopConfirmationOverlay overlay =
                new StopConfirmationOverlay
                {
                    Dock = DockStyle.Fill,
                    BackgroundImage = blurred,
                    BackgroundImageLayout =
                        ImageLayout.Stretch
                };

            overlay.CancelRequested +=
                StopOverlay_CancelRequested;

            overlay.ExitRequested +=
                StopOverlay_ExitRequested;

            Controls.Add(overlay);
            overlay.BringToFront();
        }

        // ==========================================
        // CANCEL STOP CONFIRMATION
        // ==========================================

        private void StopOverlay_CancelRequested(
            object? sender,
            EventArgs e)
        {
            if (sender is not StopConfirmationOverlay overlay)
                return;

            Image? background = overlay.BackgroundImage;

            overlay.BackgroundImage = null;

            Controls.Remove(overlay);
            overlay.Dispose();

            background?.Dispose();

            if (IsDisposed)
                return;

            quizPaused = false;

            // Resume only if the quiz is still running.
            if (!quizFinished)
                quizTimer.Resume();
        }

        // ==========================================
        // CONFIRM STOP
        // ==========================================

        private void StopOverlay_ExitRequested(
            object? sender,
            EventArgs e)
        {
            quizFinished = true;
            answerLocked = true;

            adjustmentTimer.Stop();
            quizTimer.Pause();

            Control? container = Parent;

            if (container == null)
                return;

            container.Controls.Remove(this);
            Dispose();
        }


        // ==========================================
        // 50/50 LIFELINE HOVER EVENTS
        // ==========================================

        private void picLifelineButton_MouseEnter(
            object sender, EventArgs e)
        {
            if (!CanInteract() || lifelineUsed)
                return;

            picLifelineButton.Image =
                Properties.Resources.lifeline_button_hover;
        }

        private void picLifelineButton_MouseLeave(
            object sender, EventArgs e)
        {
            if (lifelineUsed)
                return;

            picLifelineButton.Image =
                Properties.Resources.lifeline_button;
        }

        // ==========================================
        // 50/50 LIFELINE CLICK EVENT
        // ==========================================

        private void picLifelineButton_Click(
            object sender, EventArgs e)
        {
            if (!CanInteract() ||
                lifelineUsed ||
                !engine.HasMoreQuestions)
                return;

            // Collect all four answers.
            List<Label> wrongAnswers = new()
            {
                lblAnswerA,
                lblAnswerB,
                lblAnswerC,
                lblAnswerD
            };

            // Identify the correct answer.
            Label correctLabel = correctAnswerLetter switch
            {
                'A' => lblAnswerA,
                'B' => lblAnswerB,
                'C' => lblAnswerC,
                'D' => lblAnswerD,
                _ => lblAnswerA
            };

            // Keep the correct answer.
            wrongAnswers.Remove(correctLabel);

            // Randomly eliminate two incorrect answers.
            Random.Shared.Shuffle(
                System.Runtime.InteropServices.CollectionsMarshal
                    .AsSpan(wrongAnswers));

            wrongAnswers[0].Text = "";
            wrongAnswers[1].Text = "";

            // Show the NO cursor on eliminated answers.
            foreach (Label label in wrongAnswers.GetRange(0, 2))
            {
                label.Cursor = Cursors.No;

                if (label.Parent is PictureBox button)
                    button.Cursor = Cursors.No;
            }

            // Restore normal answer artwork.
            ResetAnswerImages();

            // Lock the lifeline after its first use.
            lifelineUsed = true;

            picLifelineButton.Image =
                Properties.Resources.lifeline_button_locked;

            picLifelineButton.Cursor = Cursors.Default;
        }



        // ==========================================
        // SKIP BUTTON HOVER
        // ==========================================

        private void SkipButton_MouseEnter(
            object sender,
            EventArgs e)
        {
            if (!CanInteract() || engine.SkipUsed)
                return;

            picSkipButton.Image =
                Properties.Resources.skip_button_hover;
        }

        private void SkipButton_MouseLeave(
            object sender,
            EventArgs e)
        {
            if (engine.SkipUsed)
                return;

            picSkipButton.Image =
                Properties.Resources.skip_button;
        }

        // ==========================================
        // SKIP BUTTON CLICK
        // ==========================================

        private void SkipButton_Click(
            object sender,
            EventArgs e)
        {
            if (!CanInteract() || engine.SkipUsed)
                return;

            if (!engine.SkipQuestion())
                return;

            picSkipButton.Image =
                Properties.Resources.skip_button_locked;

            picSkipButton.Cursor = Cursors.Default;

            AdvanceQuestion();
        }
    }
}
