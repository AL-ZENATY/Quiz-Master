
using QuizMaster.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuizMaster
{
    public partial class RapidFireResults : UserControl
    {
        // ==========================================
        // RESULTS DATA
        // ==========================================

        private readonly List<QuestionResult> results = new();
        private readonly List<PictureBox> questionBoxes = new();

        private const int VisibleQuestionCount = 10;

        private int firstVisibleQuestion;
        private int selectedQuestion = -1;
        private int goldenQuestionIndex = -1;

        // ==========================================
        // QUESTION STATUS COLORS
        // ==========================================

        private static readonly Color CorrectColor = Color.LimeGreen;
        private static readonly Color WrongColor = Color.Red;
        private static readonly Color TimeoutColor = Color.DarkOrange;
        private static readonly Color SkippedColor = Color.Gold;

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public RapidFireResults()
        {
            InitializeComponent();

            SetupQuestionNavigation();
            SetupAnswerTextOutlines();
            SetupButtons();

            // Make the ranking label transparent over its PictureBox.
            Point labelPosition = picTop10Panle.PointToClient(
                lblTop10Position.PointToScreen(Point.Empty));

            lblTop10Position.Parent = picTop10Panle;
            lblTop10Position.Location = labelPosition;
            lblTop10Position.BackColor = Color.Transparent;
            lblTop10Position.BringToFront();

            ClearResults();
        }

        // ==========================================
        // LOAD RESULTS
        // ==========================================

        public void LoadResults(
            List<QuestionResult> questionResults,
            double stopwatchSeconds,
            int timeAdjustmentSeconds,
            int wrongAnswers,
            int timedOutQuestions,
            bool skipUsed,
            bool fiftyFiftyUsed,
            double finalTimeSeconds,
            int goldenIndex = -1,
            bool newWorldRecord = false)
        {
            ArgumentNullException.ThrowIfNull(questionResults);

            results.Clear();
            results.AddRange(questionResults);

            goldenQuestionIndex = goldenIndex;
            firstVisibleQuestion = 0;
            selectedQuestion = results.Count > 0 ? 0 : -1;

            lblStopwatch.Text = FormatTime(stopwatchSeconds);

            lblTimeAdjustment.Text =
                timeAdjustmentSeconds > 0
                    ? $"+{timeAdjustmentSeconds}s"
                    : $"{timeAdjustmentSeconds}s";

            lblTimeAdjustment.ForeColor =
                timeAdjustmentSeconds <= 0
                    ? Color.LimeGreen
                    : Color.Red;

            lblTotalQuestions.Text = results.Count.ToString();

            lblWrongAnswers.Text = wrongAnswers.ToString();
            lblWrongAnswers.ForeColor =
                wrongAnswers == 0 ? Color.LimeGreen : Color.Red;

            lblTimedOut.Text = timedOutQuestions.ToString();
            lblTimedOut.ForeColor =
                timedOutQuestions == 0
                    ? Color.LimeGreen
                    : Color.DarkOrange;

            lblSkipUsed.Text = skipUsed ? "Used" : "Not Used";
            lblFiftyFiftyUsed.Text =
                fiftyFiftyUsed ? "Used" : "Not Used";

            lblFinalTime.Text = FormatTime(finalTimeSeconds);

            // Display the player's actual Rapid Fire ranking.
            if (UserSession.UserId is int userId)
            {
                int? position =
                    QuizDatabase.GetRapidFireTop10Position(userId);

                lblTop10Position.Text = newWorldRecord
                    ? "#1 BEST RECORD!"
                    : position.HasValue
                        ? $"#{position.Value}"
                        : "Not in Top 10";
            }
            else
            {
                lblTop10Position.Text = "Not in Top 10";
            }

            UpdateQuestionNavigation();

            if (selectedQuestion >= 0)
                DisplayQuestion(selectedQuestion);
            else
                ClearQuestionDisplay();
        }

        // ==========================================
        // QUESTION NAVIGATION
        // ==========================================

        private void SetupQuestionNavigation()
        {
            questionBoxes.AddRange(new[]
            {
                picQuestion01,
                picQuestion02,
                picQuestion03,
                picQuestion04,
                picQuestion05,
                picQuestion06,
                picQuestion07,
                picQuestion08,
                picQuestion09,
                picQuestion10
            });

            for (int i = 0; i < questionBoxes.Count; i++)
            {
                PictureBox box = questionBoxes[i];

                box.Tag = i;
                box.Cursor = Cursors.Hand;

                box.Paint += QuestionPictureBox_Paint;
                box.Click += QuestionPictureBox_Click;
            }

            picArrowLeft.Cursor = Cursors.Hand;
            picArrowRight.Cursor = Cursors.Hand;

            picArrowLeft.Click += PicArrowLeft_Click;
            picArrowRight.Click += PicArrowRight_Click;
        }

        private void UpdateQuestionNavigation()
        {
            for (int i = 0; i < questionBoxes.Count; i++)
            {
                int index = firstVisibleQuestion + i;

                PictureBox box = questionBoxes[i];

                box.Visible = index < results.Count;

                if (!box.Visible)
                    continue;

                box.Image = GoldenQuestion.GetResultBackground(
                    GoldenQuestion.IsGolden(
                        index,
                        goldenQuestionIndex));

                box.Invalidate();
            }

            lblMoreLeft.Visible = firstVisibleQuestion > 0;

            lblMoreRight.Visible =
                firstVisibleQuestion + VisibleQuestionCount
                < results.Count;

            picArrowLeft.Enabled = firstVisibleQuestion > 0;

            picArrowRight.Enabled =
                firstVisibleQuestion + VisibleQuestionCount
                < results.Count;
        }

        // ==========================================
        // QUESTION NUMBER DRAWING
        // ==========================================

        private void QuestionPictureBox_Paint(
            object? sender,
            PaintEventArgs e)
        {
            if (sender is not PictureBox box ||
                box.Tag is not int slotIndex)
                return;

            int index = firstVisibleQuestion + slotIndex;

            if (index >= results.Count)
                return;

            string number = (index + 1).ToString();

            Color color = GetQuestionColor(
                results[index].Status);

            using Font font = new Font(
                "Cooper Black",
                20.25f,
                FontStyle.Italic,
                GraphicsUnit.Point);

            Rectangle bounds = box.ClientRectangle;

            TextFormatFlags flags =
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine |
                TextFormatFlags.NoPadding;

            Rectangle shadow = bounds;
            shadow.Offset(1, 1);

            TextRenderer.DrawText(
                e.Graphics,
                number,
                font,
                shadow,
                Color.Black,
                flags);

            TextRenderer.DrawText(
                e.Graphics,
                number,
                font,
                bounds,
                color,
                flags);

            if (index == selectedQuestion)
            {
                e.Graphics.SmoothingMode =
                    SmoothingMode.AntiAlias;

                using Pen pen = new Pen(
                    Color.FromArgb(0, 145, 85),
                    3f)
                {
                    Alignment = PenAlignment.Inset
                };

                Rectangle selection = box.ClientRectangle;
                selection.Inflate(-2, -2);

                using GraphicsPath border = new();

                int diameter = 24;

                border.AddArc(
                    selection.Left,
                    selection.Top,
                    diameter,
                    diameter,
                    180,
                    90);

                border.AddArc(
                    selection.Right - diameter,
                    selection.Top,
                    diameter,
                    diameter,
                    270,
                    90);

                border.AddArc(
                    selection.Right - diameter,
                    selection.Bottom - diameter,
                    diameter,
                    diameter,
                    0,
                    90);

                border.AddArc(
                    selection.Left,
                    selection.Bottom - diameter,
                    diameter,
                    diameter,
                    90,
                    90);

                border.CloseFigure();

                e.Graphics.DrawPath(pen, border);
            }
        }

        // ==========================================
        // QUESTION STATUS COLORS
        // ==========================================

        private static Color GetQuestionColor(string status)
        {
            return status.Trim().ToLowerInvariant() switch
            {
                "correct" => CorrectColor,
                "wrong" => WrongColor,
                "timed out" or "timeout" => TimeoutColor,
                "skipped" => SkippedColor,
                _ => Color.LightGray
            };
        }

        // ==========================================
        // QUESTION SELECTION
        // ==========================================

        private void QuestionPictureBox_Click(
            object? sender,
            EventArgs e)
        {
            if (sender is not PictureBox box ||
                box.Tag is not int slotIndex)
                return;

            int index = firstVisibleQuestion + slotIndex;

            if (index >= results.Count)
                return;

            selectedQuestion = index;

            DisplayQuestion(index);

            foreach (PictureBox questionBox in questionBoxes)
                questionBox.Invalidate();
        }

        // ==========================================
        // PAGE NAVIGATION
        // ==========================================

        private void PicArrowLeft_Click(
            object? sender,
            EventArgs e)
        {
            if (firstVisibleQuestion <= 0)
                return;

            firstVisibleQuestion--;

            UpdateQuestionNavigation();
        }

        private void PicArrowRight_Click(
            object? sender,
            EventArgs e)
        {
            int maximumStart = Math.Max(
                0,
                results.Count - VisibleQuestionCount);

            if (firstVisibleQuestion >= maximumStart)
                return;

            firstVisibleQuestion++;

            UpdateQuestionNavigation();
        }


        // ==========================================
        // DISPLAY SELECTED QUESTION
        // ==========================================

        private void DisplayQuestion(int index)
        {
            QuestionResult result = results[index];
            QuizQuestion question = result.Question;

            lblQuestionText.Text = question.QuestionText;
            lblCorrectAnswer.Text = question.CorrectAnswer;

            lblYourAnswer.Text =
                result.Status.Trim().ToLowerInvariant() switch
                {
                    "skipped" => "Skipped",
                    "timed out" or "timeout" => "Timed out",
                    _ => result.UserAnswer ?? "No answer"
                };

            lblYourAnswer.ForeColor =
                GetQuestionColor(result.Status);

            lblCorrectAnswer.ForeColor = CorrectColor;

            DisplayQuestionImage(question);

            lblCorrectAnswer.Invalidate();
            lblYourAnswer.Invalidate();
        }

        private void DisplayQuestionImage(QuizQuestion question)
        {
            picQuestionImage.Image = null;
            picQuestionImage.SizeMode = PictureBoxSizeMode.Zoom;

            if (string.IsNullOrWhiteSpace(question.ImagePath))
                return;

            string resourceName = Path.GetFileNameWithoutExtension(
                question.ImagePath.Replace('\\', '/'));

            picQuestionImage.Image =
                Properties.Resources.ResourceManager
                    .GetObject(resourceName) as Image;
        }

        // ==========================================
        // BLACK ANSWER TEXT OUTLINES
        // ==========================================

        private void SetupAnswerTextOutlines()
        {
            lblCorrectAnswer.Paint += AnswerLabel_Paint;
            lblYourAnswer.Paint += AnswerLabel_Paint;
        }

        private void AnswerLabel_Paint(
            object? sender,
            PaintEventArgs e)
        {
            if (sender is not Label label ||
                string.IsNullOrEmpty(label.Text))
                return;

            e.Graphics.SmoothingMode =
                SmoothingMode.AntiAlias;

            e.Graphics.TextRenderingHint =
                System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            using GraphicsPath path = new();

            using StringFormat format = new()
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };

            path.AddString(
                label.Text,
                label.Font.FontFamily,
                (int)label.Font.Style,
                label.Font.SizeInPoints * e.Graphics.DpiY / 72f,
                label.ClientRectangle,
                format);

            using Pen outline = new Pen(Color.Black, 2f)
            {
                LineJoin = LineJoin.Round
            };

            using SolidBrush fill =
                new SolidBrush(label.ForeColor);

            e.Graphics.DrawPath(outline, path);
            e.Graphics.FillPath(fill, path);
        }

        // ==========================================
        // BUTTON SETUP
        // ==========================================

        private void SetupButtons()
        {
            picExit.BackgroundImage = null;
            picLeaderboard.BackgroundImage = null;

            picExit.Image =
                Properties.Resources.result_exit_button;

            picLeaderboard.Image =
                Properties.Resources.result_leaderboard_button;

            picExit.Cursor = Cursors.Hand;
            picLeaderboard.Cursor = Cursors.Hand;

            picExit.Click += PicExit_Click;
            picLeaderboard.Click += PicLeaderboard_Click;

            picExit.MouseEnter += PicExit_MouseEnter;
            picExit.MouseLeave += PicExit_MouseLeave;

            picLeaderboard.MouseEnter += PicLeaderboard_MouseEnter;
            picLeaderboard.MouseLeave += PicLeaderboard_MouseLeave;
        }

        private void PicExit_MouseEnter(
            object? sender,
            EventArgs e)
        {
            picExit.Image =
                Properties.Resources.result_exit_button_hover;
        }

        private void PicExit_MouseLeave(
            object? sender,
            EventArgs e)
        {
            picExit.Image =
                Properties.Resources.result_exit_button;
        }

        private void PicLeaderboard_MouseEnter(
            object? sender,
            EventArgs e)
        {
            picLeaderboard.Image =
                Properties.Resources.result_leaderboard_button_hover;
        }

        private void PicLeaderboard_MouseLeave(
            object? sender,
            EventArgs e)
        {
            picLeaderboard.Image =
                Properties.Resources.result_leaderboard_button;
        }

        // ==========================================
        // EXIT BUTTON
        // ==========================================

        private void PicExit_Click(
            object? sender,
            EventArgs e)
        {
            Control? container = Parent;

            if (container == null)
                return;

            CategorySelection categories = new()
            {
                Dock = DockStyle.Fill
            };

            container.Controls.Add(categories);
            categories.BringToFront();

            container.Controls.Remove(this);
            Dispose();
        }

        // ==========================================
        // LEADERBOARD BUTTON
        // ==========================================

        private void PicLeaderboard_Click(
            object? sender,
            EventArgs e)
        {
            Control? container = Parent;

            if (container == null)
                return;

            Leaderboard leaderboard = new()
            {
                Dock = DockStyle.Fill
            };

            container.Controls.Add(leaderboard);
            leaderboard.BringToFront();

            container.Controls.Remove(this);
            Dispose();
        }

        // ==========================================
        // TIME FORMATTING
        // ==========================================

        private static string FormatTime(double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(
                Math.Max(0, seconds));

            return $"{(int)time.TotalMinutes:00}:" +
                   $"{time.Seconds:00}." +
                   $"{time.Milliseconds / 10:00}";
        }

        // ==========================================
        // CLEAR RESULTS
        // ==========================================

        private void ClearResults()
        {
            results.Clear();

            firstVisibleQuestion = 0;
            selectedQuestion = -1;
            goldenQuestionIndex = -1;

            lblStopwatch.Text = "00:00.00";
            lblTimeAdjustment.Text = "0s";
            lblTotalQuestions.Text = "0";
            lblWrongAnswers.Text = "0";
            lblTimedOut.Text = "0";
            lblSkipUsed.Text = "Not Used";
            lblFiftyFiftyUsed.Text = "Not Used";
            lblFinalTime.Text = "00:00.00";
            lblTop10Position.Text = "Not in Top 10";

            lblTimeAdjustment.ForeColor = Color.LimeGreen;
            lblWrongAnswers.ForeColor = Color.LimeGreen;
            lblTimedOut.ForeColor = Color.LimeGreen;

            ClearQuestionDisplay();
            UpdateQuestionNavigation();
        }

        private void ClearQuestionDisplay()
        {
            lblQuestionText.Text = "";
            lblCorrectAnswer.Text = "";
            lblYourAnswer.Text = "";
            picQuestionImage.Image = null;
        }
    }
}
