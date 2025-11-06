using System.Drawing;
using System.Windows.Forms;

namespace CLIESC_CONVUNI_DOTNET_GR04.Views
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        private UITheme.GradientPanel bg;
        private UITheme.CardPanel card;
        private PictureBox avatar;
        private Label lblBien, lblSub, lblHint;
        private UITheme.PlaceholderTextBox txtUsuario;
        private UITheme.PlaceholderTextBox txtPassword;
        private UITheme.GradientButton btnLogin;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ====== FORM ======
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(900, 560);
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "ConUni • Acceso";
            this.DoubleBuffered = true;

            // ====== FONDO ======
            bg = new UITheme.GradientPanel { Dock = DockStyle.Fill };
            this.Controls.Add(bg);

            // ====== TARJETA ======
            card = new UITheme.CardPanel { Size = new Size(420, 500) };
            bg.Controls.Add(card);
            bg.Resize += (s, e) => CenterCard();
            CenterCard();

            // ====== AVATAR ======
            avatar = new PictureBox
            {
                Size = new Size(116, 116),
                Location = new Point((card.Width - 116) / 2, 26),
                BackColor = Color.Transparent
            };
            UITheme.MakeCircleAvatar(avatar, drawRing: false);
            card.Controls.Add(avatar);

            // ====== TITULOS ======
            lblBien = new Label
            {
                Text = "¡Bienvenido!",
                AutoSize = false,
                Width = card.Width,
                Height = 42,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 22f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(0, 160)
            };
            card.Controls.Add(lblBien);

            lblSub = new Label
            {
                Text = "Conversor de Unidades",
                AutoSize = false,
                Width = card.Width,
                Height = 24,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(230, 235, 248),
                BackColor = Color.Transparent,
                Location = new Point(0, 198)
            };
            card.Controls.Add(lblSub);

            // ====== CAMPOS ======
            txtUsuario = new UITheme.PlaceholderTextBox
            {
                Placeholder = "Usuario",
                BorderStyle = BorderStyle.None,
                Size = new Size(320, 44)
            };
            var hostUser = MakeFieldHost(txtUsuario);
            // 240 es el gap que visualmente calza con la maqueta
            hostUser.Location = new Point((card.Width - 320) / 2, 240);
            card.Controls.Add(hostUser);

            txtPassword = new UITheme.PlaceholderTextBox
            {
                Placeholder = "Contraseña",
                BorderStyle = BorderStyle.None,
                Size = new Size(320, 44),
                UseSystemPasswordChar = true
            };
            var hostPass = MakeFieldHost(txtPassword);
            hostPass.Location = new Point((card.Width - 320) / 2, 294);
            card.Controls.Add(hostPass);

            // ====== BOTON ======
            btnLogin = new UITheme.GradientButton
            {
                Text = "INICIAR SESIÓN",
                Size = new Size(320, 44),
                Location = new Point((card.Width - 320) / 2, 350)
            };
            btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            card.Controls.Add(btnLogin);

            // ====== HINT ======
            lblHint = new Label
            {
                Text = "Por favor, ingrese sus credenciales",
                AutoSize = false,
                Width = card.Width,
                Height = 18,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(235, 235, 245),
                BackColor = Color.Transparent,
                Location = new Point(0, 402)
            };
            card.Controls.Add(lblHint);
        }

        // Wrapper “pill” con sombra y borde
        private Panel MakeFieldHost(TextBox inner)
        {
            var host = new Panel { Size = new Size(320, 44), BackColor = Color.Transparent };
            inner.Location = new Point(14, 12);
            inner.Width = host.Width - 28;
            host.Controls.Add(inner);

            host.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var r = host.ClientRectangle; r.Inflate(-1, -1);
                int rad = 12;

                using var path = Rounded(r, rad);
                // sombra sutil
                using var sh = new SolidBrush(Color.FromArgb(28, 0, 0, 0));
                var m = new System.Drawing.Drawing2D.Matrix(); m.Translate(0, 3);
                var pSh = (System.Drawing.Drawing2D.GraphicsPath)path.Clone(); pSh.Transform(m);
                g.FillPath(sh, pSh);

                // fondo blanco
                using var fill = new SolidBrush(Color.White);
                g.FillPath(fill, path);

                // borde suave
                using var pen = new Pen(Color.FromArgb(210, 220, 235), 2f);
                g.DrawPath(pen, path);
            };
            return host;

            static System.Drawing.Drawing2D.GraphicsPath Rounded(Rectangle r, int rad)
            {
                int d = rad * 2;
                var p = new System.Drawing.Drawing2D.GraphicsPath();
                p.AddArc(r.X, r.Y, d, d, 180, 90);
                p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                p.CloseFigure();
                return p;
            }
        }

        private void CenterCard()
        {
            if (bg == null || card == null) return;
            card.Location = new Point((bg.Width - card.Width) / 2, (bg.Height - card.Height) / 2);
        }
    }
}
