using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ec.edu.monster.vista.ui
{
    /// Caja con fondo redondeado y TextBox interno sin bordes (soporta placeholder).
    public class SoftTextBox : UserControl
    {
        private readonly TextBox _edit = new TextBox();

        [Browsable(true)]
        public override string Text
        {
            get => _edit.Text;
            set { _edit.Text = value; Invalidate(); }
        }

        private string _placeholder = "";
        [Browsable(true)]
        public string Placeholder
        {
            get => _placeholder;
            set { _placeholder = value; Invalidate(); }
        }

        public bool UseSystemPasswordChar
        {
            get => _edit.UseSystemPasswordChar;
            set => _edit.UseSystemPasswordChar = value;
        }

        public SoftTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            Height = 44;
            BackColor = Color.Transparent;

            _edit.BorderStyle = BorderStyle.None;
            _edit.Font = new Font("Segoe UI", 10.5f);
            _edit.Location = new Point(14, 12);
            _edit.Width = Width - 28;
            _edit.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            _edit.TextChanged += (_, __) => Invalidate();
            Controls.Add(_edit);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            _edit.Width = Width - 28;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = ClientRectangle;
            rect.Inflate(-1, -1);

            // Fondo
            using (var lg = new LinearGradientBrush(rect,
                Color.FromArgb(255, 255, 255), Color.FromArgb(245, 245, 255), 90f))
                e.Graphics.FillPath(lg, Rounded(rect, 14));

            // Borde (viola azulado)
            using var pen = new Pen(Color.FromArgb(160, 150, 180, 255), 1.6f);
            e.Graphics.DrawPath(pen, Rounded(rect, 14));

            // Placeholder
            if (string.IsNullOrEmpty(_edit.Text) && !Focused && !_edit.Focused && !string.IsNullOrEmpty(_placeholder))
            {
                var r = new Rectangle(_edit.Left, _edit.Top + 1, _edit.Width, _edit.Height);
                using var br = new SolidBrush(Color.FromArgb(160, 150, 150, 170));
                e.Graphics.DrawString(_placeholder, _edit.Font, br, r);
            }
        }

        private static GraphicsPath Rounded(Rectangle r, int radius)
        {
            int d = radius * 2;
            var gp = new GraphicsPath();
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }
    }
}
