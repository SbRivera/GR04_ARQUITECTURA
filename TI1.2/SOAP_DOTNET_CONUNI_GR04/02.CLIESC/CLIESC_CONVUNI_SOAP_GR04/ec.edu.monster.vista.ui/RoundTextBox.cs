using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ec.edu.monster.vista.ui
{
    public class RoundTextBox : UserControl
    {
        readonly TextBox tb = new TextBox();
        public string Placeholder { get; set; } = "";

        [Browsable(true)]
        public override string Text { get => tb.Text; set => tb.Text = value; }

        public bool UseSystemPasswordChar
        {
            get => tb.UseSystemPasswordChar;
            set => tb.UseSystemPasswordChar = value;
        }

        public RoundTextBox()
        {
            DoubleBuffered = true;
            Height = 44;
            BackColor = Color.Transparent;

            tb.BorderStyle = BorderStyle.None;
            tb.Font = new Font("Segoe UI", 10f);
            tb.Location = new Point(14, 12);
            tb.Width = Width - 28;
            tb.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            tb.ForeColor = Color.Black;

            tb.GotFocus += (s, e) => Invalidate();
            tb.LostFocus += (s, e) => Invalidate();
            tb.TextChanged += (s, e) => Invalidate();

            Controls.Add(tb);
            Resize += (s, e) => tb.Width = Width - 28;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, Width - 1, Height - 1);

            using (var bg = new LinearGradientBrush(r,
                Color.FromArgb(255, 250, 252, 255),
                Color.FromArgb(255, 240, 243, 255), 90f))
                e.Graphics.FillPath(bg, Rounded(r, 12));

            using var pen = new Pen(Color.FromArgb(140, 160, 170, 220), 1.6f);
            e.Graphics.DrawPath(pen, Rounded(r, 12));

            // placeholder dibujado cuando está vacío y sin foco
            if (!tb.Focused && string.IsNullOrEmpty(tb.Text) && !string.IsNullOrEmpty(Placeholder))
            {
                var fmt = TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                var pr = new Rectangle(14, 0, Width - 28, Height);
                TextRenderer.DrawText(e.Graphics, Placeholder, tb.Font,
                    pr, Color.FromArgb(150, 120, 130, 160), fmt);
            }

            base.OnPaint(e);
        }

        static GraphicsPath Rounded(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
