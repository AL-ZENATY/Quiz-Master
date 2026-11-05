
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuizMaster.Controls
{
    public class GoldDropdown : UserControl
    {
        // ==========================================
        // COLORS
        // ==========================================

        private readonly Color gold = Color.FromArgb(255, 191, 0);
        private readonly Color darkGold = Color.FromArgb(170, 105, 0);
        private readonly Color darkBackground = Color.FromArgb(18, 18, 18);

        // ==========================================
        // COMPONENTS
        // ==========================================

        private readonly ToolStripDropDown dropdown;
        private readonly ToolStripControlHost dropdownHost;
        private readonly ListBox optionsList;

        // ==========================================
        // PROPERTIES
        // ==========================================

        [Browsable(false)]
        public string SelectedCategory { get; private set; } = "Flags";

        public event EventHandler? SelectedCategoryChanged;

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public GoldDropdown()
        {
            Size = new Size(240, 48);

            Font = new Font("Arial", 14, FontStyle.Bold);
            ForeColor = Color.White;
            BackColor = darkBackground;

            // Reduce flickering when drawing.
            DoubleBuffered = true;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            Cursor = Cursors.Hand;

            // Create the list of categories.
            optionsList = new ListBox
            {
                BorderStyle = BorderStyle.None,
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 42,
                Font = Font,
                BackColor = darkBackground,
                ForeColor = Color.White,
                IntegralHeight = false,
                Size = new Size(232, 84)
            };

            optionsList.Items.AddRange(
                new object[] { "Flags", "Cars" });

            optionsList.SelectedIndex = 0;

            optionsList.DrawItem += DrawOption;
            optionsList.Click += SelectOption;

            // Host the list inside a popup.
            dropdownHost = new ToolStripControlHost(optionsList)
            {
                Padding = Padding.Empty,
                Margin = Padding.Empty,
                AutoSize = false,
                Size = optionsList.Size
            };

            dropdown = new ToolStripDropDown
            {
                Padding = new Padding(3),
                AutoSize = false,
                Size = new Size(240, 90),
                BackColor = gold
            };

            dropdown.Items.Add(dropdownHost);

            Click += ToggleDropdown;
        }

        // ==========================================
        // DRAW MAIN DROPDOWN
        // ==========================================

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle bounds = new Rectangle(
                2, 2, Width - 5, Height - 5);

            using GraphicsPath shape = CreateRoundedPath(bounds, 12);

            // Dark glossy gradient.
            using LinearGradientBrush background =
                new LinearGradientBrush(
                    bounds,
                    Color.FromArgb(65, 65, 65),
                    Color.FromArgb(8, 8, 8),
                    LinearGradientMode.Vertical);

            graphics.FillPath(background, shape);

            // Golden outer border.
            using Pen border = new Pen(gold, 3);
            graphics.DrawPath(border, shape);

            // Subtle inner highlight.
            Rectangle innerBounds = Rectangle.Inflate(bounds, -4, -4);

            using GraphicsPath innerShape =
                CreateRoundedPath(innerBounds, 9);

            using Pen innerBorder =
                new Pen(Color.FromArgb(110, 255, 220, 100), 1);

            graphics.DrawPath(innerBorder, innerShape);

            // Selected category text.
            Rectangle textBounds = new Rectangle(
                18, 0, Width - 65, Height);

            TextRenderer.DrawText(
                graphics,
                SelectedCategory,
                Font,
                textBounds,
                Color.White,
                TextFormatFlags.Left |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis);

            // Golden arrow.
            Point[] arrow =
            {
                new Point(Width - 42, Height / 2 - 4),
                new Point(Width - 20, Height / 2 - 4),
                new Point(Width - 31, Height / 2 + 7)
            };

            using SolidBrush arrowBrush = new SolidBrush(gold);
            graphics.FillPolygon(arrowBrush, arrow);
        }

        // ==========================================
        // DRAW DROPDOWN OPTIONS
        // ==========================================

        private void DrawOption(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            bool selected =
                (e.State & DrawItemState.Selected) != 0;

            Color background = selected
                ? darkGold
                : darkBackground;

            using SolidBrush backgroundBrush =
                new SolidBrush(background);

            e.Graphics.FillRectangle(
                backgroundBrush,
                e.Bounds);

            Rectangle textBounds = new Rectangle(
                e.Bounds.X + 15,
                e.Bounds.Y,
                e.Bounds.Width - 20,
                e.Bounds.Height);

            TextRenderer.DrawText(
                e.Graphics,
                optionsList.Items[e.Index]!.ToString(),
                optionsList.Font,
                textBounds,
                Color.White,
                TextFormatFlags.Left |
                TextFormatFlags.VerticalCenter);
        }

        // ==========================================
        // OPEN / CLOSE DROPDOWN
        // ==========================================

        private void ToggleDropdown(object? sender, EventArgs e)
        {
            if (dropdown.Visible)
            {
                dropdown.Close();
                return;
            }

            dropdown.Show(this, new Point(0, Height));
        }

        // ==========================================
        // SELECT CATEGORY
        // ==========================================

        private void SelectOption(object? sender, EventArgs e)
        {
            if (optionsList.SelectedItem == null)
                return;

            string selected =
                optionsList.SelectedItem.ToString()!;

            bool changed = SelectedCategory != selected;

            SelectedCategory = selected;

            dropdown.Close();
            Invalidate();

            if (changed)
                SelectedCategoryChanged?.Invoke(this, EventArgs.Empty);
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

        // ==========================================
        // CLEANUP
        // ==========================================

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dropdown.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
