
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuizMaster.Controls
{
    public class GoldDataGrid : Control
    {
        // ==========================================
        // COLORS
        // ==========================================

        private readonly Color gold = Color.FromArgb(255, 185, 0);
        private readonly Color gridLine = Color.FromArgb(100, 210, 210, 210);
        private readonly Color selectedGold = Color.FromArgb(185, 170, 110, 0);

        // ==========================================
        // TABLE SETTINGS
        // ==========================================

        private const int BorderSize = 5;
        private const int HeaderHeight = 38;
        private const int RowHeight = 37;
        private const int ScrollbarWidth = 23;

        private int selectedRow = -1;
        private int firstVisibleRow = 0;

        private string[] headers =
        {
            "ID",
            "CATEGORY",
            "QUESTION TEXT",
            "IMAGE PATH",
            "CORRECT ANSWER",
            "WRONG ANSWER 1",
            "WRONG ANSWER 2",
            "WRONG ANSWER 3"
        };

        private float[] columnWeights =
        {
            45, 75, 225, 155, 115, 115, 115, 115
        };

        // Each row contains eight values.
        private readonly List<string[]> rows = new();

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public GoldDataGrid()
        {
            Size = new Size(990, 350);

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);

            BackColor = Color.Transparent;

            Font = new Font("Arial Narrow", 11);
            ForeColor = Color.White;
        }


        // ==========================================
        // CONFIGURE TABLE COLUMNS
        // ==========================================

        public void SetColumns(string[] titles, float[] widths)
        {
            if (titles == null || widths == null ||
                titles.Length == 0 ||
                titles.Length != widths.Length)
            {
                throw new ArgumentException(
                    "Column titles and widths must match.");
            }

            foreach (float width in widths)
            {
                if (width <= 0)
                    throw new ArgumentException(
                        "Column widths must be positive.");
            }

            headers = (string[])titles.Clone();
            columnWeights = (float[])widths.Clone();

            ClearRows();
            Invalidate();
        }

        // ==========================================
        // DATA MANAGEMENT
        // ==========================================

        public void AddRow(params string[] values)
        {
            if (values.Length != headers.Length)
            {
                throw new ArgumentException(
                    $"Each row must contain exactly {headers.Length} values.");
            }

            rows.Add(values);
            Invalidate();
        }

        public void ClearRows()
        {
            rows.Clear();
            selectedRow = -1;
            firstVisibleRow = 0;

            SelectionChanged?.Invoke(this, EventArgs.Empty);

            Invalidate();
        }

        // Notify other controls when the selected row changes.
        public event EventHandler? SelectionChanged;
        public int SelectedRowIndex => selectedRow;

        public string[]? SelectedRow =>
            selectedRow >= 0 && selectedRow < rows.Count
                ? rows[selectedRow]
                : null;

        // ==========================================
        // TABLE GEOMETRY
        // ==========================================

        private Rectangle TableBounds =>
            new Rectangle(
                BorderSize + 2,
                BorderSize + 2,
                Math.Max(0, Width - BorderSize * 2 - 4),
                Math.Max(0, Height - BorderSize * 2 - 4));

        private int VisibleRowCount =>
            Math.Max(1,
                (TableBounds.Height - HeaderHeight) / RowHeight);

        private int MaximumScroll =>
            Math.Max(0, rows.Count - VisibleRowCount);

        private int[] GetColumnWidths(int availableWidth)
        {
            int[] widths = new int[headers.Length];

            float totalWeight = 0;

            foreach (float weight in columnWeights)
                totalWeight += weight;

            int usedWidth = 0;

            for (int i = 0; i < widths.Length - 1; i++)
            {
                widths[i] = (int)(
                    availableWidth * columnWeights[i] / totalWeight);

                usedWidth += widths[i];
            }

            widths[widths.Length - 1] =
                Math.Max(0, availableWidth - usedWidth);

            return widths;
        }

        // ==========================================
        // PAINT TABLE
        // ==========================================

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle table = TableBounds;

            if (table.Width <= 0 || table.Height <= 0)
                return;

            // Clip all content to rounded corners.
            using GraphicsPath clipPath =
                CreateRoundedPath(table, 11);

            GraphicsState previousState = graphics.Save();
            graphics.SetClip(clipPath);

            DrawHeaders(graphics, table);
            DrawRows(graphics, table);
            DrawScrollbar(graphics, table);

            graphics.Restore(previousState);

            // Draw border last, above all content.
            Rectangle borderBounds = new Rectangle(
                2, 2, Width - 5, Height - 5);

            using GraphicsPath borderPath =
                CreateRoundedPath(borderBounds, 15);

            using Pen borderPen = new Pen(gold, 3);

            graphics.DrawPath(borderPen, borderPath);
        }

        // ==========================================
        // DRAW HEADERS
        // ==========================================

        private void DrawHeaders(Graphics graphics, Rectangle table)
        {
            int contentWidth = table.Width - ScrollbarWidth;

            Rectangle headerBounds = new Rectangle(
                table.X,
                table.Y,
                contentWidth,
                HeaderHeight);

            // Slightly transparent glossy header.
            using LinearGradientBrush headerBrush =
                new LinearGradientBrush(
                    headerBounds,
                    Color.FromArgb(210, 55, 55, 55),
                    Color.FromArgb(210, 12, 12, 12),
                    LinearGradientMode.Vertical);

            graphics.FillRectangle(headerBrush, headerBounds);

            int[] widths = GetColumnWidths(contentWidth);
            int x = table.X;

            using Pen linePen = new Pen(gridLine, 1);

            for (int i = 0; i < headers.Length; i++)
            {
                Rectangle cell = new Rectangle(
                    x, table.Y, widths[i], HeaderHeight);

                TextRenderer.DrawText(
                    graphics,
                    headers[i],
                    new Font("Arial Narrow", 9, FontStyle.Bold),
                    Rectangle.Inflate(cell, -4, 0),
                    Color.White,
                    TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis);

                x += widths[i];

                graphics.DrawLine(
                    linePen,
                    x,
                    table.Y,
                    x,
                    table.Bottom);
            }

            graphics.DrawLine(
                linePen,
                table.X,
                table.Y + HeaderHeight,
                table.X + contentWidth,
                table.Y + HeaderHeight);
        }

        // ==========================================
        // DRAW ROWS
        // ==========================================

        private void DrawRows(Graphics graphics, Rectangle table)
        {
            int contentWidth = table.Width - ScrollbarWidth;
            int[] widths = GetColumnWidths(contentWidth);

            using Pen linePen = new Pen(gridLine, 1);

            Rectangle body = new Rectangle(
                table.X,
                table.Y + HeaderHeight,
                contentWidth,
                table.Height - HeaderHeight);

            GraphicsState previousState = graphics.Save();
            graphics.SetClip(body);

            for (int visibleIndex = 0;
                 visibleIndex < VisibleRowCount;
                 visibleIndex++)
            {
                int rowIndex = firstVisibleRow + visibleIndex;

                if (rowIndex >= rows.Count)
                    break;

                int y = body.Y + visibleIndex * RowHeight;

                Rectangle rowBounds = new Rectangle(
                    table.X, y, contentWidth, RowHeight);

                // Golden transparent selection highlight.
                if (rowIndex == selectedRow)
                {
                    using SolidBrush selectionBrush =
                        new SolidBrush(selectedGold);

                    graphics.FillRectangle(
                        selectionBrush, rowBounds);
                }

                int x = table.X;

                for (int columnIndex = 0;
                     columnIndex < headers.Length;
                     columnIndex++)
                {
                    Rectangle cell = new Rectangle(
                        x, y, widths[columnIndex], RowHeight);

                    Rectangle textBounds =
                        Rectangle.Inflate(cell, -8, 0);

                    TextRenderer.DrawText(
                        graphics,
                        rows[rowIndex][columnIndex],
                        Font,
                        textBounds,
                        Color.White,
                        TextFormatFlags.Left |
                        TextFormatFlags.VerticalCenter |
                        TextFormatFlags.EndEllipsis);

                    x += widths[columnIndex];
                }

                graphics.DrawLine(
                    linePen,
                    table.X,
                    y + RowHeight,
                    table.X + contentWidth,
                    y + RowHeight);
            }

            graphics.Restore(previousState);
        }

        // ==========================================
        // GOLDEN SCROLLBAR
        // ==========================================

        private void DrawScrollbar(Graphics graphics, Rectangle table)
        {
            int x = table.Right - ScrollbarWidth;

            Rectangle track = new Rectangle(
                x, table.Y, ScrollbarWidth, table.Height);

            using SolidBrush trackBrush =
                new SolidBrush(Color.FromArgb(160, 0, 0, 0));

            graphics.FillRectangle(trackBrush, track);

            // Scrollbar separator.
            using Pen separator = new Pen(gold, 1);

            graphics.DrawLine(
                separator,
                x,
                table.Y,
                x,
                table.Bottom);

            // Top and bottom arrow buttons.
            DrawArrow(
                graphics,
                x + ScrollbarWidth / 2,
                table.Y + 12,
                true);

            DrawArrow(
                graphics,
                x + ScrollbarWidth / 2,
                table.Bottom - 12,
                false);

            if (rows.Count <= VisibleRowCount)
                return;

            int trackTop = table.Y + 27;
            int trackHeight = table.Height - 54;

            int thumbHeight = Math.Max(
                35,
                trackHeight * VisibleRowCount / rows.Count);

            thumbHeight = Math.Min(thumbHeight, trackHeight);

            int thumbY = trackTop;

            if (MaximumScroll > 0)
            {
                thumbY +=
                    (trackHeight - thumbHeight) *
                    firstVisibleRow / MaximumScroll;
            }

            Rectangle thumb = new Rectangle(
                x + 5,
                thumbY,
                ScrollbarWidth - 10,
                thumbHeight);

            using GraphicsPath thumbPath =
                CreateRoundedPath(thumb, 6);

            using LinearGradientBrush thumbBrush =
                new LinearGradientBrush(
                    thumb,
                    Color.FromArgb(255, 225, 75),
                    Color.FromArgb(235, 145, 0),
                    LinearGradientMode.Horizontal);

            graphics.FillPath(thumbBrush, thumbPath);
        }

        private void DrawArrow(
            Graphics graphics,
            int centerX,
            int centerY,
            bool upward)
        {
            int direction = upward ? -1 : 1;

            Point[] points =
            {
                new Point(centerX, centerY + direction * 5),
                new Point(centerX - 7, centerY - direction * 4),
                new Point(centerX + 7, centerY - direction * 4)
            };

            using SolidBrush brush = new SolidBrush(gold);

            graphics.FillPolygon(brush, points);
        }

        // ==========================================
        // MOUSE INTERACTION
        // ==========================================

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            Rectangle table = TableBounds;
            int scrollbarX = table.Right - ScrollbarWidth;

            if (e.X >= scrollbarX)
            {
                if (e.Y < table.Y + 27)
                    ScrollBy(-1);
                else if (e.Y > table.Bottom - 27)
                    ScrollBy(1);
                else
                {
                    int trackTop = table.Y + 27;
                    int trackHeight = table.Height - 54;

                    if (trackHeight > 0 && MaximumScroll > 0)
                    {
                        int position = Math.Max(
                            0,
                            Math.Min(
                                trackHeight,
                                e.Y - trackTop));

                        firstVisibleRow =
                            position * MaximumScroll / trackHeight;

                        Invalidate();
                    }
                }

                return;
            }

            int bodyY = table.Y + HeaderHeight;

            if (e.Y < bodyY || e.Y >= table.Bottom)
                return;

            int rowIndex =
                firstVisibleRow + (e.Y - bodyY) / RowHeight;

            if (rowIndex >= 0 && rowIndex < rows.Count)
            {
                if (selectedRow != rowIndex)
                {
                    selectedRow = rowIndex;
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                }

                Invalidate();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            ScrollBy(e.Delta > 0 ? -1 : 1);
        }

        private void ScrollBy(int amount)
        {
            firstVisibleRow = Math.Max(
                0,
                Math.Min(MaximumScroll, firstVisibleRow + amount));

            Invalidate();
        }

        // ==========================================
        // ROUNDED RECTANGLE HELPER
        // ==========================================

        private GraphicsPath CreateRoundedPath(
            Rectangle rectangle,
            int radius)
        {
            GraphicsPath path = new GraphicsPath();

            int diameter = radius * 2;

            path.AddArc(
                rectangle.X,
                rectangle.Y,
                diameter,
                diameter,
                180,
                90);

            path.AddArc(
                rectangle.Right - diameter,
                rectangle.Y,
                diameter,
                diameter,
                270,
                90);

            path.AddArc(
                rectangle.Right - diameter,
                rectangle.Bottom - diameter,
                diameter,
                diameter,
                0,
                90);

            path.AddArc(
                rectangle.X,
                rectangle.Bottom - diameter,
                diameter,
                diameter,
                90,
                90);

            path.CloseFigure();

            return path;
        }
    }
}
