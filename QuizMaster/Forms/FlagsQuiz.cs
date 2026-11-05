
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuizMaster
{
    public partial class FlagsQuiz : UserControl
    {
        // ==========================================
        // SHARED QUIZ COMPONENTS
        // ==========================================

        private readonly QuizEngine engine;
        private readonly QuizTimer quizTimer;

        private char correctAnswerLetter = 'A';
        private Color normalQuestionColor;

        // ==========================================
        // QUIZ STATE
        // ==========================================

        private bool answerLocked = true;
        private bool quizFinished;
        private bool quizPaused;
        private bool showingFeedback;
        private bool startingCountdown = true;
        private bool endingCountdown;
        private bool resultsOpened;
        private bool lifelineUsed;

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public FlagsQuiz()
        {
            InitializeComponent();

            // Remember the question label's original color.
            normalQuestionColor = lblQuestion.ForeColor;

            SetupAnswerLabels();
            AttachLabel(lblQuestionTimer, picQuestionTimer);

            engine = new QuizEngine(
                QuizDatabase.GetQuestions("Flags"));

            quizTimer = new QuizTimer();

            // Connect timer events to quiz actions.
            quizTimer.CountdownUpdated += UpdateCountdownDisplay;
            quizTimer.StartingFinished += StartGameplay;
            quizTimer.QuestionExpired += QuestionTimedOut;
            quizTimer.FeedbackFinished += AdvanceQuestion;
            quizTimer.GameExpired += GameTimedOut;
            quizTimer.EndingFinished += OpenResultsScreen;

            Disposed += FlagsQuiz_Disposed;

            if (engine.HasQuestions)
            {
                PrepareStartingCountdown();
                quizTimer.Start();
            }
            else
            {
                startingCountdown = false;
                lblQuestion.Text = "No questions available.";

                ClearQuestionImages();
                ClearAnswerTexts();
                UpdateQuizLabels();
                UpdateTimerLabels();
            }
        }

        // ==========================================
        // CLEANUP
        // ==========================================

        private void FlagsQuiz_Disposed(
            object? sender, EventArgs e)
        {
            quizTimer.Dispose();
        }

        // ==========================================
        // LABEL SETUP
        // ==========================================

        private void SetupAnswerLabels()
        {
            AttachLabel(lblAnswerA, picAnswerA);
            AttachLabel(lblAnswerB, picAnswerB);
            AttachLabel(lblAnswerC, picAnswerC);
            AttachLabel(lblAnswerD, picAnswerD);
        }

        private void AttachLabel(Label label, PictureBox picture)
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

            ClearQuestionImages();
            ClearAnswerTexts();

            // Countdown text uses the normal label style.
            lblQuestion.IsGolden = false;
            lblQuestion.ForeColor = normalQuestionColor;
            lblQuestion.Text = "3";

            lblGameTimer.Text = "60 Sec";
            lblQuestionTimer.Text = "5";

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
                return;

            QuizQuestion question = engine.CurrentQuestion;

            lblQuestion.Text = question.QuestionText;

            // Golden questions use gold text with a black outline.
            // Normal questions keep their original appearance.
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

            DisplayFlagImage(question);

            // Shuffle the four possible answers.
            string[] answers = engine.ShuffleAnswers();

            lblAnswerA.Text = answers[0];
            lblAnswerB.Text = answers[1];
            lblAnswerC.Text = answers[2];
            lblAnswerD.Text = answers[3];

            // Restore normal cursors for the new question.
            foreach (Label label in new[]
            {
                lblAnswerA, lblAnswerB, lblAnswerC, lblAnswerD
            })

            {
                label.Cursor = Cursors.Hand;

                if (label.Parent is PictureBox button)
                    button.Cursor = Cursors.Hand;
            }

            // Remember which answer button is correct.
            int correctIndex = Array.IndexOf(
                answers, question.CorrectAnswer);

            correctAnswerLetter = (char)('A' + correctIndex);

            answerLocked = false;
            showingFeedback = false;

            quizTimer.StartQuestion();

            ResetAnswerImages();
            UpdateQuizLabels();
            UpdateTimerLabels();
        }

        // ==========================================
        // CLEAR QUESTION IMAGES
        // ==========================================

        private void ClearQuestionImages()
        {
            picFlag.Image = null;
            ResetAnswerImages();
        }

        // ==========================================
        // ANSWER ARTWORK
        // ==========================================

        private void ResetAnswerImages()
        {
            picAnswerA.Image = Properties.Resources.flags_answer_a;
            picAnswerB.Image = Properties.Resources.flags_answer_b;
            picAnswerC.Image = Properties.Resources.flags_answer_c;
            picAnswerD.Image = Properties.Resources.flags_answer_d;
        }

        // ==========================================
        // ANSWER INTERACTION HELPERS
        // ==========================================

        private bool CanInteract()
        {
            return !answerLocked &&
                   !quizPaused &&
                   !quizFinished &&
                   !startingCountdown &&
                   !endingCountdown;
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
        // ANSWER HOVER ARTWORK
        // ==========================================

        private void SetAnswerHover(
            PictureBox button, bool hovered)
        {
            if (button == picAnswerA)
            {
                button.Image = hovered
                    ? Properties.Resources.flags_answer_a_hover
                    : Properties.Resources.flags_answer_a;
            }
            else if (button == picAnswerB)
            {
                button.Image = hovered
                    ? Properties.Resources.flags_answer_b_hover
                    : Properties.Resources.flags_answer_b;
            }
            else if (button == picAnswerC)
            {
                button.Image = hovered
                    ? Properties.Resources.flags_answer_c_hover
                    : Properties.Resources.flags_answer_c;
            }
            else if (button == picAnswerD)
            {
                button.Image = hovered
                    ? Properties.Resources.flags_answer_d_hover
                    : Properties.Resources.flags_answer_d;
            }
        }

        // ==========================================
        // ANSWER MOUSE EVENTS
        // ==========================================

        private void Answer_MouseEnter(object sender, EventArgs e)
        {
            if (!CanInteract())
                return;

            PictureBox? button = GetAnswerButton(sender);

            if (button == null)
                return;

            char answerLetter = GetAnswerLetter(button);

            Label? answerLabel = answerLetter switch
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

        private void Answer_MouseLeave(object sender, EventArgs e)
        {
            if (!CanInteract())
                return;

            PictureBox? button = GetAnswerButton(sender);

            if (button != null)
                SetAnswerHover(button, false);
        }

        private void AnswerLabel_MouseEnter(
            object sender, EventArgs e)
        {
            Answer_MouseEnter(sender, e);
        }

        private void AnswerLabel_MouseLeave(
            object sender, EventArgs e)
        {
            Answer_MouseLeave(sender, e);
        }

        // ==========================================
        // ANSWER CLICK EVENT
        // ==========================================

        private void Answer_Click(object sender, EventArgs e)
        {
            if (!CanInteract() || !engine.HasQuestions)
                return;

            PictureBox? button = GetAnswerButton(sender);

            if (button == null)
                return;

            char selectedAnswer = GetAnswerLetter(button);

            if (selectedAnswer == '\0')
                return;

            // Empty answer labels cannot be selected.
            Label selectedLabel = selectedAnswer switch
            {
                'A' => lblAnswerA,
                'B' => lblAnswerB,
                'C' => lblAnswerC,
                'D' => lblAnswerD,
                _ => lblAnswerA
            };

            if (string.IsNullOrWhiteSpace(selectedLabel.Text))
                return;

            ShowAnswerFeedback(selectedAnswer);
        }

        // ==========================================
        // ANSWER FEEDBACK
        // ==========================================


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

            // Collect the three incorrect answer labels.
            List<Label> wrongAnswers = new()
            {
                lblAnswerA,
                lblAnswerB,
                lblAnswerC,
                lblAnswerD
            };

            Label correctLabel = correctAnswerLetter switch
            {
                'A' => lblAnswerA,
                'B' => lblAnswerB,
                'C' => lblAnswerC,
                'D' => lblAnswerD,
                _ => lblAnswerA
            };

            wrongAnswers.Remove(correctLabel);

            // Randomly remove two incorrect answers.
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

            // Restore normal answer artwork after elimination.
            ResetAnswerImages();

            // Lock the lifeline for the rest of the quiz.
            lifelineUsed = true;

            picLifelineButton.Image =
                Properties.Resources.lifeline_button_locked;

            picLifelineButton.Cursor = Cursors.Default;
        }


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

            // Submit the answer and update the score.
            engine.SubmitAnswer(userAnswer);

            quizTimer.StartFeedback();

            UpdateQuizLabels();
            ResetAnswerImages();

            // Highlight the correct answer.
            SetCorrectAnswerImage(correctAnswerLetter);

            // Highlight the player's wrong answer if necessary.
            if (selectedAnswer != correctAnswerLetter)
                SetWrongAnswerImage(selectedAnswer);
        }

        // ==========================================
        // CORRECT ANSWER ARTWORK
        // ==========================================

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

        // ==========================================
        // WRONG ANSWER ARTWORK
        // ==========================================

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
        // QUESTION TIMEOUT
        // ==========================================

        private void QuestionTimedOut()
        {
            if (quizFinished)
                return;

            answerLocked = true;

            engine.TimeoutQuestion();
            UpdateQuizLabels();

            AdvanceQuestion();
        }

        // ==========================================
        // QUESTION PROGRESSION
        // ==========================================

        private void AdvanceQuestion()
        {
            if (quizFinished || startingCountdown ||
                endingCountdown)
                return;

            engine.AdvanceQuestion();

            if (!engine.HasMoreQuestions)
            {
                FinishQuiz(true);
                return;
            }

            DisplayCurrentQuestion();
        }

        // ==========================================
        // GAME TIMEOUT
        // ==========================================

        private void GameTimedOut()
        {
            if (quizFinished)
                return;

            engine.GameTimedOut(!answerLocked);
            FinishQuiz(false);
        }

        // ==========================================
        // TIMER LABELS
        // ==========================================

        private void UpdateTimerLabels()
        {
            int gameSeconds = (int)Math.Ceiling(
                Math.Max(0, quizTimer.GameTimeRemaining));

            int questionSeconds = (int)Math.Ceiling(
                Math.Max(0, quizTimer.QuestionTimeRemaining));

            lblGameTimer.Text = $"{gameSeconds} Sec";
            lblQuestionTimer.Text = questionSeconds.ToString();

            // Blink the game timer when time is running low.
            bool blinkRed =
                ((Environment.TickCount64 / 250) % 2) == 0;

            lblGameTimer.ForeColor = gameSeconds <= 10
                ? (blinkRed ? Color.Red : Color.Black)
                : gameSeconds <= 20
                    ? Color.DarkOrange
                    : Color.Black;

            // Blink the question timer during its final seconds.
            lblQuestionTimer.ForeColor =
                questionSeconds <= 3 && !showingFeedback
                    ? (blinkRed ? Color.Red : Color.Black)
                    : Color.Black;
        }

        // ==========================================
        // QUESTION COUNTER AND SCORE LABELS
        // ==========================================

        private void UpdateQuizLabels()
        {
            lblQuestionCounter.Text = engine.HasQuestions
                ? $"{Math.Min(engine.CurrentQuestionIndex + 1, engine.Questions.Count)}/{engine.Questions.Count}"
                : "0/0";

            lblScore.Text = engine.Score.ToString("0.##");
        }

        // ==========================================
        // FINISH QUIZ
        // ==========================================

        private void FinishQuiz(bool completedAllQuestions)
        {
            if (quizFinished)
                return;

            quizFinished = true;
            answerLocked = true;
            showingFeedback = false;
            endingCountdown = true;

            // Award a speed bonus only when all questions are completed.
            if (completedAllQuestions)
            {
                engine.ApplySpeedBonus(
                    quizTimer.GameTimeRemaining);
            }

            // Save the final score for the logged-in player.
            if (UserSession.UserId is int userId)
            {
                QuizDatabase.SaveGameResult(
                    userId,
                    "Flags",
                    (double)engine.Score);
            }

            UpdateQuizLabels();
            UpdateTimerLabels();

            // Restore normal text for the ending message.
            lblQuestion.IsGolden = false;
            lblQuestion.ForeColor = normalQuestionColor;
            lblQuestion.Text = "Taking you to the results...";
            lblQuestion.Invalidate();

            picFlag.Image = null;
            ClearAnswerTexts();

            quizTimer.StartEndingCountdown();
        }

        // ==========================================
        // OPEN RESULTS SCREEN
        // ==========================================

        private void OpenResultsScreen()
        {
            if (resultsOpened || IsDisposed || Parent == null)
                return;

            resultsOpened = true;

            Control container = Parent;

            QuizResults results = new QuizResults
            {
                Dock = DockStyle.Fill
            };

            // Pass the golden question index to the results screen.
            results.LoadResults(
                engine.Results,
                (double)engine.Score,
                (double)engine.SpeedBonus,
                (double)engine.UnreachedPenalty,
                engine.GoldenQuestionIndex,
                lifelineUsed);

            container.Controls.Add(results);
            results.BringToFront();

            container.Controls.Remove(this);
            Dispose();
        }

        // ==========================================
        // STOP BUTTON HOVER EVENTS
        // ==========================================

        private void picStopButton_MouseEnter(
            object sender, EventArgs e)
        {
            picStopButton.Image =
                Properties.Resources.stop_button_hover;
        }

        private void picStopButton_MouseLeave(
            object sender, EventArgs e)
        {
            picStopButton.Image =
                Properties.Resources.stop_button;
        }

        // ==========================================
        // STOP BUTTON CLICK EVENT
        // ==========================================

        private void picStopButton_Click(object sender, EventArgs e)
        {
            if (quizPaused || quizFinished ||
                endingCountdown || resultsOpened)
                return;

            quizTimer.Pause();

            if (quizFinished || IsDisposed)
                return;

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

            using Bitmap screenshot = new Bitmap(width, height);

            DrawToBitmap(
                screenshot,
                new Rectangle(0, 0, width, height));

            // Create a smaller screenshot for the blur effect.
            int smallWidth = Math.Max(1, width / 12);
            int smallHeight = Math.Max(1, height / 12);

            using Bitmap small = new Bitmap(
                screenshot,
                new Size(smallWidth, smallHeight));

            Bitmap blurred = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(blurred))
            {
                graphics.InterpolationMode =
                    InterpolationMode.HighQualityBilinear;

                graphics.DrawImage(
                    small,
                    new Rectangle(0, 0, width, height));

                // Darken the blurred background.
                using SolidBrush darkBrush =
                    new SolidBrush(Color.FromArgb(170, Color.Black));

                graphics.FillRectangle(
                    darkBrush,
                    0, 0, width, height);
            }

            StopConfirmationOverlay overlay =
                new StopConfirmationOverlay
                {
                    Dock = DockStyle.Fill,
                    BackgroundImage = blurred,
                    BackgroundImageLayout = ImageLayout.Stretch
                };

            overlay.CancelRequested += StopOverlay_CancelRequested;
            overlay.ExitRequested += StopOverlay_ExitRequested;

            Controls.Add(overlay);
            overlay.BringToFront();
        }

        // ==========================================
        // CANCEL STOP CONFIRMATION
        // ==========================================

        private void StopOverlay_CancelRequested(
            object? sender, EventArgs e)
        {
            if (sender is not StopConfirmationOverlay overlay)
                return;

            Image? background = overlay.BackgroundImage;
            overlay.BackgroundImage = null;

            Controls.Remove(overlay);
            overlay.Dispose();
            background?.Dispose();

            if (quizFinished || IsDisposed)
                return;

            quizPaused = false;
            quizTimer.Resume();
        }

        // ==========================================
        // CONFIRM EXIT
        // ==========================================

        private void StopOverlay_ExitRequested(
            object? sender, EventArgs e)
        {
            quizFinished = true;
            answerLocked = true;

            quizTimer.Pause();

            Control? container = Parent;

            if (container == null)
                return;

            container.Controls.Remove(this);
            Dispose();
        }

        // ==========================================
        // SKIP BUTTON HOVER EVENTS
        // ==========================================

        private void picSkipButton_MouseEnter(
            object sender, EventArgs e)
        {
            if (!CanInteract() || engine.SkipUsed)
                return;

            picSkipButton.Image =
                Properties.Resources.skip_button_hover;
        }

        private void picSkipButton_MouseLeave(
            object sender, EventArgs e)
        {
            if (!CanInteract() || engine.SkipUsed)
                return;

            picSkipButton.Image =
                Properties.Resources.skip_button;
        }

        // ==========================================
        // SKIP BUTTON CLICK EVENT
        // ==========================================

        private void picSkipButton_Click(object sender, EventArgs e)
        {
            if (!CanInteract() || engine.SkipUsed)
                return;

            if (!engine.SkipQuestion())
                return;

            // Disable the skip button after its first use.
            picSkipButton.Image =
                Properties.Resources.skip_button_locked;

            picSkipButton.Cursor = Cursors.Default;

            AdvanceQuestion();
        }

        // ==========================================
        // FLAG IMAGE DISPLAY
        // ==========================================


        private void DisplayFlagImage(QuizQuestion question)
        {
            picFlag.Image = null;

            if (string.IsNullOrWhiteSpace(question.ImagePath))
                return;

            string resourceName =
                System.IO.Path.GetFileNameWithoutExtension(
                    question.ImagePath);

            // First, look for an existing embedded resource.
            object? resource =
                Properties.Resources.ResourceManager.GetObject(
                    resourceName);

            if (resource is Image embeddedImage)
            {
                picFlag.Image = embeddedImage;
            }
            else
            {
                // Otherwise, load an image imported by the Admin Panel.
                string imagePath = question.ImagePath;

                if (!System.IO.Path.IsPathRooted(imagePath))
                {
                    imagePath = System.IO.Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),
                        "QuizMaster",
                        "QuestionImages",
                        System.IO.Path.GetFileName(imagePath));
                }

                if (System.IO.File.Exists(imagePath))
                {
                    using var loadedImage = Image.FromFile(imagePath);
                    picFlag.Image = new Bitmap(loadedImage);
                }
            }

            picFlag.SizeMode = PictureBoxSizeMode.Zoom;
        }


        // ==========================================
        // DESIGNER LOAD EVENT
        // ==========================================

        private void FlagsQuiz_Load(object sender, EventArgs e)
        {

        }
    }
}
