using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ec.edu.monster.vista.ui
{
    public class PillButton : Button
    {
        public Color C1 { get; set; } = Color.FromArgb(242, 90, 90);
        public Color C2 { get; set; } = Color.FromArgb(247, 117, 96);

        public PillButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            ForeColor = Color.White;
            Font = new Font(Font, FontStyle.Bold);
            Height = 34;
            Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var r = ClientRectangle;
            using var br = new LinearGradientBrush(r, C1, C2, 0f);
            e.Graphics.FillRoundedRect(br, r, r.Height);
            e.Graphics.DrawString(Text, Font, Brushes.White, r, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
    }
}
