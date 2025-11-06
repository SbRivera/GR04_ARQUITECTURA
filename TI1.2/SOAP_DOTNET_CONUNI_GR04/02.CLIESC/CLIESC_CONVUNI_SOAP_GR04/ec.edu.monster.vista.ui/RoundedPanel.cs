using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ec.edu.monster.vista.ui
{
    public class RoundedPanel : Panel
    {
        public int Radius { get; set; } = 18;

        public RoundedPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            BackColor = Color.FromArgb(245, 240, 255);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // sombra
            using (var sb = new SolidBrush(Color.FromArgb(40, 60, 20, 130)))
                e.Graphics.FillRoundedRect(sb, new Rectangle(6, 6, Width - 12, Height - 6), Radius);

            // tarjeta
            using (var sb2 = new SolidBrush(Color.FromArgb(245, 240, 255)))
                e.Graphics.FillRoundedRect(sb2, new Rectangle(0, 0, Width - 12, Height - 12), Radius);

            using var p = new Pen(Color.FromArgb(230, 220, 250));
            e.Graphics.DrawRoundedRect(p, new Rectangle(0, 0, Width - 12, Height - 12), Radius);
        }
    }

    static class GfxExt
    {
        public static void FillRoundedRect(this Graphics g, Brush b, Rectangle r, int radius)
        {
            using var gp = Rounded(r, radius);
            g.FillPath(b, gp);
        }
        public static void DrawRoundedRect(this Graphics g, Pen p, Rectangle r, int radius)
        {
            using var gp = Rounded(r, radius);
            g.DrawPath(p, gp);
        }
        static System.Drawing.Drawing2D.GraphicsPath Rounded(Rectangle r, int rad)
        {
            var gp = new System.Drawing.Drawing2D.GraphicsPath();
            int d = rad * 2;
            gp.AddArc(r.Left, r.Top, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Top, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }
    }
}
