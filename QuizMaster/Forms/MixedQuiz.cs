
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuizMaster.Forms
{
    public partial class MixedQuiz : UserControl
    {
        // ==========================================
        // SHARED QUIZ COMPONENTS
        // ==========================================

        private readonly QuizEngine engine;
        private readonly QuizTimer quizTimer;

        private char correctAnswerLetter = 'A';

        private bool answerLocked = true;
        private bool quizFinished;
        private bool quizPaused;
        private bool showingFeedback;
        private bool startingCountdown = true;
        private bool endingCountdown;
        private bool resultsOpened;
        private bool lifelineUsed;

        // ==========================================
        // GOLDEN QUESTION STYLING
        // ==========================================

        private readonly Color normalQuestionColor;

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public MixedQuiz()
        {
            InitializeComponent();

            normalQuestionColor = lblQuestion.ForeColor;

            SetupAnswerLabels();
            AttachLabel(lblQuestionTimer, picQuestionTimer);

            // Select 20 Flags and 20 Cars questions.
            // Shuffle complete question objects so their
            // images and correct answers stay together.

            List<QuizQuestion> flags = QuizDatabase
                .GetQuestions("Flags")
                .OrderBy(_ => Random.Shared.Next())
                .Take(20)
                .ToList();

            List<QuizQuestion> cars = QuizDatabase
                .GetQuestions("Cars")
                .OrderBy(_ => Random.Shared.Next())
                .Take(20)
                .ToList();

            List<QuizQuestion> mixedQuestions = flags
                .Concat(cars)
                .OrderBy(_ => Random.Shared.Next())
                .ToList();

            engine = new QuizEngine(mixedQuestions);

            quizTimer = new QuizTimer();

            quizTimer.CountdownUpdated += UpdateCountdownDisplay;
            quizTimer.StartingFinished += StartGameplay;
            quizTimer.QuestionExpired += QuestionTimedOut;
            quizTimer.FeedbackFinished += AdvanceQuestion;
            quizTimer.GameExpired += GameTimedOut;
            quizTimer.EndingFinished += OpenResultsScreen;

            Disposed += MixedQuiz_Disposed;

            if (flags.Count == 20 && cars.Count == 20)
            {
                PrepareStartingCountdown();
                quizTimer.Start();
            }
            else
            {
                startingCountdown = false;
                answerLocked = true;

                lblQuestion.Text =
                    $"Not enough questions: {flags.Count}/20 Flags, {cars.Count}/20 Cars.";

                ClearQuestionImages();
                ClearAnswerTexts();
                UpdateQuizLabels();
                UpdateTimerLabels();
            }
        }

        // ==========================================
        // CLEANUP
        // ==========================================

        private void MixedQuiz_Disposed(object? sender, EventArgs e)
        {
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

        private void AttachLabel(Label label, PictureBox picture)
        {
            Point position = picture.PointToClient(
                label.PointToScreen(Point.Empty));

            label.Parent = picture;
            label.Location = position;
            label.BackColor = Color.Transparent;
            label.BringToFront();
        }

        // ==========================================
        // CLEAR ANSWER TEXTS
        // ==========================================

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
                    (int)Math.Ceiling(quizTimer.StartTimeRemaining)
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

            // Apply Golden Question styling.
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

            // Shuffle and display answers.
            string[] answers = engine.ShuffleAnswers();

            lblAnswerA.Text = answers[0];
            lblAnswerB.Text = answers[1];
            lblAnswerC.Text = answers[2];
            lblAnswerD.Text = answers[3];

            // Restore normal cursors for every new question.
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

            quizTimer.StartQuestion();

            ResetAnswerImages();
            UpdateQuizLabels();
            UpdateTimerLabels();
        }

        // ==========================================
        // QUESTION IMAGE
        // ==========================================


        private void DisplayQuestionImage(QuizQuestion question)
        {
            picMixed.Image = null;

            if (string.IsNullOrWhiteSpace(question.ImagePath))
                return;

            string resourceName =
                Path.GetFileNameWithoutExtension(
                    question.ImagePath.Replace('\\', '/'));

            if (string.IsNullOrWhiteSpace(resourceName))
                return;

            // First, try the existing embedded resources.
            object? resource =
                Properties.Resources.ResourceManager.GetObject(
                    resourceName);

            if (resource is Image embeddedImage)
            {
                picMixed.Image = embeddedImage;
            }
            else
            {
                // Otherwise, load images imported by the Admin Panel.
                string imagePath = question.ImagePath;

                if (!Path.IsPathRooted(imagePath))
                {
                    imagePath = Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),
                        "QuizMaster",
                        "QuestionImages",
                        Path.GetFileName(imagePath));
                }

                if (File.Exists(imagePath))
                {
                    using var loadedImage = Image.FromFile(imagePath);
                    picMixed.Image = new Bitmap(loadedImage);
                }
            }

            picMixed.SizeMode = PictureBoxSizeMode.Zoom;
        }


        private void ClearQuestionImages()
        {
            picMixed.Image = null;
            ResetAnswerImages();
        }

        // ==========================================
        // ANSWER BUTTON IMAGES
        // ==========================================

        private void ResetAnswerImages()
        {
            picAnswerA.Image = Properties.Resources.mixed_answer_a;
            picAnswerB.Image = Properties.Resources.mixed_answer_b;
            picAnswerC.Image = Properties.Resources.mixed_answer_c;
            picAnswerD.Image = Properties.Resources.mixed_answer_d;
        }

        private void SetAnswerHover(
            PictureBox button,
            bool hovered)
        {
            if (button == picAnswerA)
            {
                button.Image = hovered
                    ? Properties.Resources.mixed_answer_a_hover
                    : Properties.Resources.mixed_answer_a;
            }
            else if (button == picAnswerB)
            {
                button.Image = hovered
                    ? Properties.Resources.mixed_answer_b_hover
                    : Properties.Resources.mixed_answer_b;
            }
            else if (button == picAnswerC)
            {
                button.Image = hovered
                    ? Properties.Resources.mixed_answer_c_hover
                    : Properties.Resources.mixed_answer_c;
            }
            else if (button == picAnswerD)
            {
                button.Image = hovered
                    ? Properties.Resources.mixed_answer_d_hover
                    : Properties.Resources.mixed_answer_d;
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

        private void Answer_MouseEnter(object sender, EventArgs e)
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

        private void Answer_MouseLeave(object sender, EventArgs e)
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

            engine.SubmitAnswer(userAnswer);

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
            if (quizFinished)
                return;

            answerLocked = true;

            engine.TimeoutQuestion();

            UpdateQuizLabels();
            AdvanceQuestion();
        }

        // ==========================================
        // NEXT QUESTION
        // ==========================================

        private void AdvanceQuestion()
        {
            if (quizFinished || startingCountdown || endingCountdown)
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

            bool blinkRed =
                ((Environment.TickCount64 / 250) % 2) == 0;

            lblGameTimer.ForeColor = gameSeconds <= 10
                ? (blinkRed ? Color.Red : Color.Black)
                : gameSeconds <= 20
                    ? Color.DarkOrange
                    : Color.Black;

            // Mixed Quiz uses violet timer text.
            lblQuestionTimer.ForeColor =
                questionSeconds <= 3 && !showingFeedback
                    ? (blinkRed ? Color.Red : Color.Violet)
                    : Color.Violet;
        }

        // ==========================================
        // QUIZ LABELS
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

            if (completedAllQuestions)
            {
                engine.ApplySpeedBonus(
                    quizTimer.GameTimeRemaining);
            }

            if (UserSession.UserId is int userId)
            {
                QuizDatabase.SaveGameResult(
                    userId,
                    "Mixed",
                    (double)engine.Score);
            }

            UpdateQuizLabels();
            UpdateTimerLabels();

            // Restore normal question styling.
            lblQuestion.IsGolden = false;
            lblQuestion.ForeColor = normalQuestionColor;
            lblQuestion.Text = "Taking you to the results...";
            lblQuestion.Invalidate();

            picMixed.Image = null;

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
        // STOP BUTTON HOVER
        // ==========================================

        private void picStopButton_MouseEnter(
            object sender,
            EventArgs e)
        {
            picStopButton.Image =
                Properties.Resources.stop_button_hover;
        }

        private void picStopButton_MouseLeave(
            object sender,
            EventArgs e)
        {
            picStopButton.Image =
                Properties.Resources.stop_button;
        }

        // ==========================================
        // STOP BUTTON CLICK
        // ==========================================

        private void picStopButton_Click(
            object sender,
            EventArgs e)
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

            using (Graphics graphics = Graphics.FromImage(blurred))
            {
                graphics.InterpolationMode =
                    InterpolationMode.HighQualityBilinear;

                graphics.DrawImage(
                    small,
                    new Rectangle(0, 0, width, height));

                using SolidBrush darkBrush = new SolidBrush(
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
                    BackgroundImageLayout = ImageLayout.Stretch
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

            if (quizFinished || IsDisposed)
                return;

            quizPaused = false;

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

            // Remove the correct answer from the list.
            wrongAnswers.Remove(correctLabel);

            // Randomly select two incorrect answers.
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

            // Restore normal artwork.
            ResetAnswerImages();

            // Lock the lifeline after one use.
            lifelineUsed = true;

            picLifelineButton.Image =
                Properties.Resources.lifeline_button_locked;

            picLifelineButton.Cursor = Cursors.Default;
        }



        // ==========================================
        // SKIP BUTTON
        // ==========================================

        private void picSkipButton_MouseEnter(
            object sender,
            EventArgs e)
        {
            if (!CanInteract() || engine.SkipUsed)
                return;

            picSkipButton.Image =
                Properties.Resources.skip_button_hover;
        }

        private void picSkipButton_MouseLeave(
            object sender,
            EventArgs e)
        {
            if (!CanInteract() || engine.SkipUsed)
                return;

            picSkipButton.Image =
                Properties.Resources.skip_button;
        }

        private void picSkipButton_Click(
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
