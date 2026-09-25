using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuizMaster
{
    public class OutlinedLabel : Label
    {
        public float OutlineWidth { get; set; } = 2f;
        public Color OutlineColor { get; set; } = Color.Black;

        public OutlinedLabel()
        {
            AutoSize = false;
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    format.Trimming = StringTrimming.Word;

                    using (FontFamily family = new FontFamily(Font.FontFamily.Name))
                    {
                        float fontSize = e.Graphics.DpiY * Font.SizeInPoints / 72f;

                        path.AddString(
                            Text,
                            family,
                            (int)Font.Style,
                            fontSize,
                            ClientRectangle,
                            format);
                    }
                }

                using (Pen outline = new Pen(OutlineColor, OutlineWidth))
                {
                    outline.LineJoin = LineJoin.Round;
                    e.Graphics.DrawPath(outline, path);
                }

                using (SolidBrush fill = new SolidBrush(ForeColor))
                {
                    e.Graphics.FillPath(fill, path);
                }
            }
        }
    }
}