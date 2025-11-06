using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ec.edu.monster.vista.ui
{
    public class CardHeader : Control
    {
        public static readonly int HEADER_HEIGHT = 64;
        public Color C1 { get; set; } = Color.BlueViolet;
        public Color C2 { get; set; } = Color.MediumOrchid;
        public Color Badge { get; set; } = Color.FromArgb(255, 255, 255, 35);

        public CardHeader()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            Height = HEADER_HEIGHT;
            ForeColor = Color.White;
            Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold);
            Text = "Título";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, Width, Height);
            using var br = new LinearGradientBrush(r, C1, C2, 0f);
            e.Graphics.FillRoundedRect(br, r, 18);

            int d = 22; int x = 16; int y = (Height - d) / 2;
            using var sb = new SolidBrush(Color.FromArgb(80, 255, 255, 255));
            e.Graphics.FillEllipse(sb, x, y, d, d);
            e.Graphics.DrawString("i", new Font(Font.FontFamily, 14, FontStyle.Bold), Brushes.White, x + 7, y + 4);

            using var f = new Font(Font.FontFamily, 14, FontStyle.Bold);
            e.Graphics.DrawString(Text, f, Brushes.White, x + d + 12, y + 4);
        }
    }
}
