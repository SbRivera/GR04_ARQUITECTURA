using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ec.edu.monster.vista.ui
{
    public class CardPanel : Panel
    {
        public int Radius { get; set; } = 26;
        public Color Grad1 { get; set; } = Color.FromArgb(154, 108, 255); // morado
        public Color Grad2 { get; set; } = Color.FromArgb(207, 128, 245); // morado-rosa

        public CardPanel()
        {
            // No uses Transparent para evitar excepciones en algunos controles
            BackColor = Color.White;
            Padding = new Padding(34, 44, 34, 34);
            DoubleBuffered = true;
        }

        protected override void OnSizeChanged(System.EventArgs e)
        {
            base.OnSizeChanged(e);
            // Clip redondeado real para que no se vea nada fuera de las esquinas
            Region = new Region(Rounded(ClientRectangle, Radius));
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = ClientRectangle;

            // Sombra externa suave
            using (var gp = new GraphicsPath())
            {
                var rs = new Rectangle(r.X + 8, r.Y + 12, r.Width - 16, r.Height - 12);
                gp.AddPath(Rounded(rs, Radius + 4), false);
                using var pth = new PathGradientBrush(gp)
                {
                    CenterColor = Color.FromArgb(65, Color.Black),
                    SurroundColors = new[] { Color.Transparent }
                };
                g.FillPath(pth, gp);
            }

            // Tarjeta: morado degradado
            using var lg = new LinearGradientBrush(r, Grad1, Grad2, 90f);
            using var card = Rounded(r, Radius);
            g.FillPath(lg, card);

            // Brillo superior (sutil)
            var top = new Rectangle(r.X + 1, r.Y + 1, r.Width - 2, r.Height / 2);
            using var shine = new LinearGradientBrush(top,
                Color.FromArgb(85, Color.White), Color.FromArgb(0, Color.White), 90f);
            using var shinePath = Rounded(top, Radius - 1);
            g.FillPath(shine, shinePath);

            // Borde blanco translúcido
            using var pen = new Pen(Color.FromArgb(60, 255, 255, 255), 2f);
            g.DrawPath(pen, card);
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
