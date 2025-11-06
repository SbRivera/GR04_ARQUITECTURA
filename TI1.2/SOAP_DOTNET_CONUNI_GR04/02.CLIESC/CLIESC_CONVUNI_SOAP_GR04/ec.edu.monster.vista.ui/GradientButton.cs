using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ec.edu.monster.vista.ui
{
    public class GradientButton : Button
    {
        public Color G1 { get; set; } = Color.FromArgb(109, 149, 255);
        public Color G2 { get; set; } = Color.FromArgb(143, 105, 255);
        public int Radius { get; set; } = 18;

        public GradientButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            ForeColor = Color.White;
            Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            Height = 46;
            Cursor = Cursors.Hand;
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            Region = new Region(Rounded(ClientRectangle, Radius));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // sombra suave debajo
            var s = ClientRectangle; s.Offset(0, 2);
            using (var gp = Rounded(s, Radius))
            using (var pth = new PathGradientBrush(gp))
            {
                pth.CenterColor = Color.FromArgb(40, Color.Black);
                pth.SurroundColors = new[] { Color.Transparent };
                g.FillPath(pth, gp);
            }

            // fondo degradado
            using var lg = new LinearGradientBrush(ClientRectangle, G1, G2, 0f);
            using var path = Rounded(ClientRectangle, Radius);
            g.FillPath(lg, path);

            // texto
            TextRenderer.DrawText(g, Text, Font, ClientRectangle, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private static GraphicsPath Rounded(Rectangle r, int radius)
        {
            int d = radius * 2;
            var gp = new GraphicsPath();
            gp.AddArc(r.Left, r.Top, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Top, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }
    }
}
