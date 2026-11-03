
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuizMaster
{
    // ==========================================
    // GOLDEN QUESTION FEATURE
    // ==========================================

    public static class GoldenQuestion
    {
        // ==========================================
        // QUESTION SELECTION
        // ==========================================

        public static int SelectIndex(
            int questionCount,
            bool isSpeedQuiz)
        {
            if (isSpeedQuiz)
            {
                return questionCount > 10
                    ? Random.Shared.Next(
                        10, Math.Min(20, questionCount))
                    : -1;
            }

            return questionCount > 0
                ? Random.Shared.Next(
                    Math.Min(20, questionCount))
                : -1;
        }

        // ==========================================
        // GOLDEN QUESTION DETECTION
        // ==========================================

        public static bool IsGolden(
            int currentIndex,
            int goldenIndex)
        {
            return goldenIndex >= 0 &&
                   currentIndex == goldenIndex;
        }

        // ==========================================
        // GOLDEN QUESTION SCORING
        // ==========================================

        public static decimal GetCorrectPoints()
        {
            return 10m;
        }

        public static decimal GetWrongPenalty()
        {
            return -1m;
        }

        public static decimal GetTimeoutPenalty()
        {
            return -0.5m;
        }

        // ==========================================
        // RESULTS SCREEN ACCENT
        // ==========================================

        public static Image GetResultBackground(
            bool isGolden)
        {
            return isGolden
                ? Properties.Resources.result_number_golden_background
                : Properties.Resources.result_number_background;
        }
    }

    // ==========================================
    // GOLDEN QUESTION TEXT LABEL
    // ==========================================

    public class GoldenQuestionLabel : Label
    {
        private bool isGolden;

        public bool IsGolden
        {
            get => isGolden;

            set
            {
                if (isGolden == value)
                    return;

                isGolden = value;
                Invalidate();
            }
        }

        // ==========================================
        // CUSTOM GOLDEN TEXT RENDERING
        // ==========================================

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!IsGolden)
            {
                base.OnPaint(e);
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using GraphicsPath path = new GraphicsPath();

            using StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            path.AddString(
                Text,
                Font.FontFamily,
                (int)Font.Style,
                Font.SizeInPoints * e.Graphics.DpiY / 72f,
                ClientRectangle,
                format);

            using Pen outline = new Pen(Color.Black, 3f)
            {
                LineJoin = LineJoin.Round
            };

            using SolidBrush gold = new SolidBrush(Color.Gold);

            e.Graphics.DrawPath(outline, path);
            e.Graphics.FillPath(gold, path);
        }
    }
}
