using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ec.edu.monster.vista.ui
{
    public class GradientPanel : Panel
    {
        public Color TopLeft { get; set; } = Color.FromArgb(117, 91, 242);   // morado
        public Color BottomRight { get; set; } = Color.FromArgb(227, 118, 232); // rosa

        public GradientPanel()
        {
            Dock = DockStyle.Fill;
            DoubleBuffered = true;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using var lg = new LinearGradientBrush(ClientRectangle, TopLeft, BottomRight, 45f);
            e.Graphics.FillRectangle(lg, ClientRectangle);
        }
    }
}
