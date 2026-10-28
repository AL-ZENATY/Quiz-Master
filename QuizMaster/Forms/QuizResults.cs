
using QuizMaster.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace QuizMaster
{
    public partial class QuizResults : UserControl
    {
        // ==========================================
        // RESULTS DATA
        // ==========================================

        private readonly List<QuestionResult> results = new();
        private readonly List<PictureBox> questionBoxes = new();

        private const int VisibleQuestionCount = 10;

        private int firstVisibleQuestion;
        private int selectedQuestion = -1;

        // ==========================================
        // SCORE DATA
        // ==========================================

        private double finalScore;
        private double timeBonus;
        private double unreachedPenalty;
        private bool lifelineUsed;

        // ==========================================
        // GOLDEN QUESTION TRACKING
        // ==========================================

        private int goldenQuestionIndex = -1;

        // ==========================================
        // NAVIGATION EVENTS
        // ==========================================

        public event EventHandler? ExitRequested;
        public event EventHandler? LeaderboardRequested;

        // ==========================================
        // QUESTION STATUS COLORS
        // ==========================================

        private static readonly Color CorrectColor = Color.LimeGreen;
        private static readonly Color WrongColor = Color.Red;
        private static readonly Color TimeoutColor = Color.Orange;
        private static readonly Color SkippedColor = Color.Yellow;
        private static readonly Color UnreachedColor = Color.LightGray;

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public QuizResults()
        {
            InitializeComponent();

            SetupQuestionNavigation();
            SetupButtons();
            SetupAnswerTextOutlines();

            // Make the label use the Top 10 panel as its background.
            Point labelPosition = picTop10Panel.PointToClient(
                lblTop10Position.PointToScreen(Point.Empty));

            lblTop10Position.Parent = picTop10Panel;
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
            double score,
            double bonus,
            double remainingQuestionPenalty,
            int goldenIndex = -1,
            bool usedLifeline = false)
        {
            ArgumentNullException.ThrowIfNull(questionResults);

            results.Clear();
            results.AddRange(questionResults);

            finalScore = score;
            timeBonus = bonus;
            unreachedPenalty = remainingQuestionPenalty;
            goldenQuestionIndex = goldenIndex;
            lifelineUsed = usedLifeline;

            firstVisibleQuestion = 0;
            selectedQuestion = results.Count > 0 ? 0 : -1;

            UpdateQuestionNavigation();
            UpdateScoreBreakdown();

            // Display the player's actual Top 10 position.
            if (UserSession.UserId is int userId)
            {
                string category = results.Count > 0
                    ? results[0].Question.Category
                    : "";

                if (category == "Flags" ||
                    category == "Cars" ||
                    category == "Mixed")
                {
                    int? position = QuizDatabase.GetPlayerTop10Position(
                        userId,
                        category);

                    lblTop10Position.Text = position.HasValue
                        ? $"#{position.Value}"
                        : "Not in Top 10";
                }
                else
                {
                    lblTop10Position.Text = "Not in Top 10";
                }
            }
            else
            {
                lblTop10Position.Text = "Not in Top 10";
            }

            if (selectedQuestion >= 0)
            {
                DisplayQuestion(selectedQuestion);
            }
            else
            {
                ClearQuestionDisplay();
            }
        }

        // ==========================================
        // QUESTION NAVIGATION SETUP
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

            lblMoreLeft.Text = "...";
            lblMoreRight.Text = "...";
        }

        // ==========================================
        // QUESTION NAVIGATION DISPLAY
        // ==========================================

        private void UpdateQuestionNavigation()
        {
            int maximumStart = Math.Max(
                0,
                results.Count - VisibleQuestionCount);

            firstVisibleQuestion = Math.Clamp(
                firstVisibleQuestion,
                0,
                maximumStart);

            for (int i = 0; i < questionBoxes.Count; i++)
            {
                int index = firstVisibleQuestion + i;

                questionBoxes[i].Visible = index < results.Count;

                // Let GoldenQuestion select the correct artwork.
                questionBoxes[i].Image = GoldenQuestion.GetResultBackground(
                    GoldenQuestion.IsGolden(index, goldenQuestionIndex));

                questionBoxes[i].Invalidate();
            }

            lblMoreLeft.Visible = firstVisibleQuestion > 0;

            lblMoreRight.Visible =
                firstVisibleQuestion + VisibleQuestionCount < results.Count;

            picArrowLeft.Enabled = firstVisibleQuestion > 0;
            picArrowRight.Enabled = firstVisibleQuestion < maximumStart;
        }

        // ==========================================
        // DRAW QUESTION NUMBERS
        // ==========================================

        private void QuestionPictureBox_Paint(
            object? sender,
            PaintEventArgs e)
        {
            if (sender is not PictureBox box ||
                box.Tag is not int slotIndex)
            {
                return;
            }

            int index = firstVisibleQuestion + slotIndex;

            if (index < 0 || index >= results.Count)
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

            // Draw a small black shadow behind the number.
            Rectangle outlineBounds = bounds;
            outlineBounds.Offset(1, 1);

            TextRenderer.DrawText(
                e.Graphics,
                number,
                font,
                outlineBounds,
                Color.Black,
                flags);

            // Draw the status-colored number.
            TextRenderer.DrawText(
                e.Graphics,
                number,
                font,
                bounds,
                color,
                flags);

            // Highlight the currently selected question.
            if (index == selectedQuestion)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using Pen pen = new Pen(
                    Color.FromArgb(0, 145, 85),
                    3f)
                {
                    Alignment = PenAlignment.Inset
                };

                Rectangle selection = box.ClientRectangle;
                selection.Inflate(-2, -2);

                using GraphicsPath border = new GraphicsPath();

                int radius = 12;
                int diameter = radius * 2;

                border.AddArc(
                    selection.Left,
                    selection.Top,
                    diameter,
                    diameter,
                    190,
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

                "timed out" or "timeout" or "game ended"
                    => TimeoutColor,

                "skipped" => SkippedColor,

                _ => UnreachedColor
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
            {
                return;
            }

            int index = firstVisibleQuestion + slotIndex;

            if (index < 0 || index >= results.Count)
                return;

            selectedQuestion = index;

            DisplayQuestion(index);

            foreach (PictureBox questionBox in questionBoxes)
            {
                questionBox.Invalidate();
            }
        }

        // ==========================================
        // QUESTION NAVIGATION ARROWS
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
            if (index < 0 || index >= results.Count)
                return;

            QuestionResult result = results[index];
            QuizQuestion question = result.Question;

            lblQuestionText.Text = question.QuestionText;
            lblCorrectAnswer.Text = question.CorrectAnswer;

            lblYourAnswer.Text =
                result.Status.Trim().ToLowerInvariant() switch
                {
                    "skipped" => "Skipped",
                    "timed out" or "timeout" => "Timed out",
                    "game ended" => "Time's up!",
                    "not reached" => "Not reached",

                    _ => result.UserAnswer ?? "No answer"
                };

            lblYourAnswer.ForeColor =
                GetQuestionColor(result.Status);

            lblCorrectAnswer.ForeColor = CorrectColor;

            DisplayQuestionImage(question);
        }

        // ==========================================
        // QUESTION IMAGE DISPLAY
        // ==========================================

        private void DisplayQuestionImage(QuizQuestion question)
        {
            picQuestionImage.Image = null;
            picQuestionImage.SizeMode = PictureBoxSizeMode.Zoom;

            if (string.IsNullOrWhiteSpace(question.ImagePath))
                return;

            string resourceName =
                System.IO.Path.GetFileNameWithoutExtension(
                    question.ImagePath);

            if (string.IsNullOrWhiteSpace(resourceName))
                return;

            object? image = Properties.Resources.ResourceManager
                .GetObject(resourceName);

            picQuestionImage.Image = image as Image;
        }

        // ==========================================
        // SCORE BREAKDOWN
        // ==========================================

        private void UpdateScoreBreakdown()
        {
            double correctPoints = 0;
            double wrongPoints = 0;
            double timeoutPoints = 0;

            int skippedCount = CountStatus("Skipped");

            for (int i = 0; i < results.Count; i++)
            {
                string status = results[i].Status
                    .Trim()
                    .ToLowerInvariant();

                bool isGolden = GoldenQuestion.IsGolden(
                    i,
                    goldenQuestionIndex);

                switch (status)
                {
                    case "correct":
                        correctPoints += isGolden
                            ? (double)GoldenQuestion.GetCorrectPoints()
                            : 5;
                        break;

                    case "wrong":
                        wrongPoints += isGolden
                            ? (double)GoldenQuestion.GetWrongPenalty()
                            : -2;
                        break;

                    case "timed out":
                    case "timeout":
                        timeoutPoints += isGolden
                            ? (double)GoldenQuestion.GetTimeoutPenalty()
                            : -1;
                        break;
                }
            }

            lblCorrectCount.Text = FormatScore(correctPoints);
            lblWrongCount.Text = FormatScore(wrongPoints);
            lblTimeoutPenalty.Text = FormatScore(timeoutPoints);

            lblSkipCount.Text = skippedCount.ToString();

            lblUnreachedPenalty.Text = FormatScore(unreachedPenalty);
            lblTimeBonus.Text = FormatScore(timeBonus);

            lblFinalScore.Text = finalScore.ToString(
                "0.##",
                CultureInfo.InvariantCulture);

            lblFiftyFiftyUsed.Text = lifelineUsed
            ? "Used"
            : "Not Used";
        }

        // ==========================================
        // COUNT QUESTION STATUS
        // ==========================================

        private int CountStatus(string status)
        {
            return results.Count(result =>
                string.Equals(
                    result.Status,
                    status,
                    StringComparison.OrdinalIgnoreCase));
        }

        // ==========================================
        // SCORE FORMATTING
        // ==========================================

        private static string FormatScore(double value)
        {
            return value.ToString(
                "+0.##;-0.##;0",
                CultureInfo.InvariantCulture);
        }

        // ==========================================
        // ANSWER TEXT OUTLINES
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
            {
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.TextRenderingHint =
                System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            using GraphicsPath path = new GraphicsPath();

            using StringFormat format = new StringFormat
            {
                Alignment = label.TextAlign switch
                {
                    ContentAlignment.TopCenter or
                    ContentAlignment.MiddleCenter or
                    ContentAlignment.BottomCenter
                        => StringAlignment.Center,

                    ContentAlignment.TopRight or
                    ContentAlignment.MiddleRight or
                    ContentAlignment.BottomRight
                        => StringAlignment.Far,

                    _ => StringAlignment.Near
                },

                LineAlignment = label.TextAlign switch
                {
                    ContentAlignment.MiddleLeft or
                    ContentAlignment.MiddleCenter or
                    ContentAlignment.MiddleRight
                        => StringAlignment.Center,

                    ContentAlignment.BottomLeft or
                    ContentAlignment.BottomCenter or
                    ContentAlignment.BottomRight
                        => StringAlignment.Far,

                    _ => StringAlignment.Near
                }
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

            using SolidBrush fill = new SolidBrush(label.ForeColor);

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

            picExit.SizeMode = PictureBoxSizeMode.Zoom;
            picLeaderboard.SizeMode = PictureBoxSizeMode.Zoom;

            picExit.Cursor = Cursors.Hand;
            picLeaderboard.Cursor = Cursors.Hand;
        }

        // ==========================================
        // BUTTON HOVER EVENTS
        // ==========================================

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
        // NAVIGATION BUTTON EVENTS
        // ==========================================

        private void PicExit_Click(
            object? sender,
            EventArgs e)
        {
            Control? container = Parent;

            if (container == null)
                return;

            CategorySelection categories = new CategorySelection
            {
                Dock = DockStyle.Fill
            };

            container.Controls.Add(categories);
            categories.BringToFront();

            container.Controls.Remove(this);
            Dispose();
        }

        private void PicLeaderboard_Click(
            object? sender,
            EventArgs e)
        {
            Control? container = Parent;

            if (container == null)
                return;

            Leaderboard leaderboard = new Leaderboard
            {
                Dock = DockStyle.Fill
            };

            container.Controls.Add(leaderboard);
            leaderboard.BringToFront();

            container.Controls.Remove(this);
            Dispose();
        }

        // ==========================================
        // CLEAR RESULTS
        // ==========================================

        private void ClearResults()
        {
            results.Clear();

            firstVisibleQuestion = 0;
            selectedQuestion = -1;

            finalScore = 0;
            timeBonus = 0;
            unreachedPenalty = 0;

            goldenQuestionIndex = -1;
            lifelineUsed = false;

            lblTop10Position.Text = "Not in Top 10";

            ClearQuestionDisplay();
            UpdateQuestionNavigation();
            UpdateScoreBreakdown();
        }

        // ==========================================
        // CLEAR QUESTION DISPLAY
        // ==========================================

        private void ClearQuestionDisplay()
        {
            lblQuestionText.Text = "";
            lblCorrectAnswer.Text = "";
            lblYourAnswer.Text = "";

            picQuestionImage.Image = null;
        }
    }
}
