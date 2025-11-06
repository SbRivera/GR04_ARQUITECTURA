using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ec.edu.monster.vista.ui
{
    public class FabButton : Button
    {
        public FabButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(56, 56);
            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;
            Text = string.Empty;
            TabStop = false;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var r = ClientRectangle;
            using var br = new LinearGradientBrush(r, Color.FromArgb(255, 143, 0), Color.FromArgb(237, 76, 92), 30f);
            e.Graphics.FillEllipse(br, r);

            // icono tacho
            using var p = new Pen(Color.White, 2.2f) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round };
            int cx = r.Width / 2, cy = r.Height / 2;
            e.Graphics.DrawLine(p, cx - 10, cy - 10, cx + 10, cy - 10);
            e.Graphics.DrawLine(p, cx - 6, cy - 12, cx + 6, cy - 12);
            e.Graphics.DrawArc(p, cx - 9, cy - 9, 18, 20, 0, 360);
            e.Graphics.DrawLine(p, cx - 3, cy - 4, cx - 3, cy + 7);
            e.Graphics.DrawLine(p, cx, cy - 4, cx, cy + 7);
            e.Graphics.DrawLine(p, cx + 3, cy - 4, cx + 3, cy + 7);
        }
    }
}
