using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace ec.edu.monster.vista
{
    public class LoginForm : Form
    {
        private TextBox txtUser;
        private TextBox txtPass;
        private Button btnLogin;

        public LoginForm()
        {
            try
            {
                InitializeComponents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}\n\n{ex.StackTrace}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponents()
        {
            // Configuración de la ventana
            Text = "Conversor de Unidades - Login";
            Size = new Size(900, 600);
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            BackColor = Color.FromArgb(150, 130, 230);

            // Panel de fondo con degradado
            var bg = new Panel { Dock = DockStyle.Fill };
            bg.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var lgb = new LinearGradientBrush(
                    bg.ClientRectangle,
                    Color.FromArgb(120, 130, 230),
                    Color.FromArgb(180, 130, 230),
                    135f);
                g.FillRectangle(lgb, bg.ClientRectangle);
            };
            Controls.Add(bg);

            // Tarjeta central con glassmorphism
            var card = new Panel
            {
                Width = 380,
                Height = 540,
                BackColor = Color.FromArgb(160, 140, 120, 230)
            };
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                
                // Fondo translúcido con degradado
                using var gp = GetRoundedRect(new Rectangle(0, 0, card.Width, card.Height), 30);
                using var lgb = new LinearGradientBrush(
                    card.ClientRectangle,
                    Color.FromArgb(180, 150, 130, 240),
                    Color.FromArgb(160, 160, 130, 240),
                    90f);
                g.FillPath(lgb, gp);
                
                // Borde sutil
                using var pen = new Pen(Color.FromArgb(100, 255, 255, 255), 2);
                g.DrawPath(pen, gp);
            };
            bg.Controls.Add(card);

            // Centrar tarjeta
            void CenterCard(object? sender, EventArgs e)
            {
                card.Left = (bg.ClientSize.Width - card.Width) / 2;
                card.Top = (bg.ClientSize.Height - card.Height) / 2;
            }
            bg.Resize += CenterCard;
            Load += CenterCard;

            int yPos = 35;

            // Avatar circular con borde - NUEVO DISEÑO
            var avatarContainer = new Panel
            {
                Size = new Size(130, 130),
                BackColor = Color.Transparent,
                Left = (card.Width - 130) / 2,
                Top = yPos
            };
            avatarContainer.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                
                // Borde blanco
                using var borderPath = new GraphicsPath();
                borderPath.AddEllipse(0, 0, 130, 130);
                using var whiteBrush = new SolidBrush(Color.White);
                g.FillPath(whiteBrush, borderPath);
                
                // Imagen circular recortada
                var imageRect = new Rectangle(5, 5, 120, 120);
                using var imagePath = new GraphicsPath();
                imagePath.AddEllipse(imageRect);
                
                g.SetClip(imagePath);
                
                // Cargar y dibujar imagen
                try
                {
                    string imagePath2 = Path.Combine(Application.StartupPath, "..", "..", "..", "..", "img", "sullivan.jpg");
                    string fullPath = Path.GetFullPath(imagePath2);
                    if (File.Exists(fullPath))
                    {
                        using var img = Image.FromFile(fullPath);
                        g.DrawImage(img, imageRect);
                    }
                    else
                    {
                        // Fondo celeste si no hay imagen
                        using var fallbackBrush = new SolidBrush(Color.FromArgb(100, 150, 200));
                        g.FillEllipse(fallbackBrush, imageRect);
                    }
                }
                catch
                {
                    // Fondo celeste si hay error
                    using var fallbackBrush = new SolidBrush(Color.FromArgb(100, 150, 200));
                    g.FillEllipse(fallbackBrush, imageRect);
                }
                
                g.ResetClip();
            };
            card.Controls.Add(avatarContainer);
            yPos += 145;

            // Título: ¡Bienvenido!
            var lblTitle = new Label
            {
                Text = "¡Bienvenido!",
                Font = new Font("Segoe UI", 26f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true
            };
            card.Controls.Add(lblTitle);
            lblTitle.Left = (card.Width - lblTitle.Width) / 2;
            lblTitle.Top = yPos;
            yPos += 40;

            // Subtítulo
            var lblSub = new Label
            {
                Text = "Conversor de Unidades",
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.FromArgb(240, 240, 250),
                BackColor = Color.Transparent,
                AutoSize = true
            };
            card.Controls.Add(lblSub);
            lblSub.Left = (card.Width - lblSub.Width) / 2;
            lblSub.Top = yPos;
            yPos += 40;

            // Input Usuario con wrapper
            var userWrapper = CreateGlassInputWrapper("Usuario", out txtUser);
            userWrapper.Width = 300;
            userWrapper.Left = (card.Width - userWrapper.Width) / 2;
            userWrapper.Top = yPos;
            card.Controls.Add(userWrapper);
            yPos += 60;

            // Input Contraseña con wrapper
            var passWrapper = CreateGlassInputWrapper("Contraseña", out txtPass);
            txtPass.UseSystemPasswordChar = true;
            passWrapper.Width = 300;
            passWrapper.Left = (card.Width - passWrapper.Width) / 2;
            passWrapper.Top = yPos;
            card.Controls.Add(passWrapper);
            yPos += 60;

            // Botón de login
            btnLogin = new Button
            {
                Width = 300,
                Height = 45,
                FlatStyle = FlatStyle.Flat,
                Text = "INICIAR SESIÓN",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                
                using var gp = GetRoundedRect(btnLogin.ClientRectangle, 22);
                using var lgb = new LinearGradientBrush(
                    btnLogin.ClientRectangle,
                    Color.FromArgb(100, 120, 240),
                    Color.FromArgb(120, 140, 250),
                    0f);
                g.FillPath(lgb, gp);
                
                // Texto centrado
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(btnLogin.Text, btnLogin.Font, Brushes.White, btnLogin.ClientRectangle, sf);
            };
            btnLogin.Left = (card.Width - btnLogin.Width) / 2;
            btnLogin.Top = yPos;
            card.Controls.Add(btnLogin);
            yPos += 55;

            // Hint inferior
            var hint = new Label
            {
                AutoSize = false,
                Width = 300,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Por favor, ingrese sus credenciales",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(230, 230, 240),
                BackColor = Color.Transparent
            };
            hint.Left = (card.Width - hint.Width) / 2;
            hint.Top = yPos;
            card.Controls.Add(hint);

            // Enter para enviar
            AcceptButton = btnLogin;

            // Evento de login
            btnLogin.Click += (_, __) =>
            {
                var u = txtUser.Text.Trim();
                var p = txtPass.Text;
                
                // Verificar si son placeholders
                if (u == "Usuario") u = "";
                if (p == "Contraseña") p = "";

                if (string.IsNullOrWhiteSpace(u) || string.IsNullOrWhiteSpace(p))
                {
                    MessageBox.Show("Por favor ingrese usuario y contraseña.", "Campos requeridos",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.Equals(u, "MONSTER", StringComparison.OrdinalIgnoreCase) && p == "MONSTER9")
                {
                    AbrirMain(u);
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Acceso denegado",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPass.Text = "Contraseña";
                    txtPass.ForeColor = Color.FromArgb(140, 150, 170);
                    txtPass.UseSystemPasswordChar = false;
                    txtPass.Focus();
                }
            };
        }

        // Crear input con efecto glass
        private Panel CreateGlassInputWrapper(string placeholder, out TextBox textBox)
        {
            var txt = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10.5f),
                ForeColor = Color.White,
                BackColor = Color.White,
                Text = placeholder,
                Tag = placeholder
            };

            // Placeholder behavior
            txt.GotFocus += (s, e) =>
            {
                if (txt.Text == txt.Tag?.ToString())
                {
                    txt.Text = "";
                    txt.ForeColor = Color.FromArgb(60, 70, 90);
                }
            };

            txt.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = txt.Tag?.ToString();
                    txt.ForeColor = Color.FromArgb(140, 150, 170);
                }
            };
            
            // Color inicial del placeholder
            txt.ForeColor = Color.FromArgb(140, 150, 170);

            // Panel wrapper con borde redondeado
            var wrapper = new Panel
            {
                Height = 45,
                BackColor = Color.White,
                Padding = new Padding(15, 12, 15, 12)
            };
            
            wrapper.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                
                using var gp = GetRoundedRect(new Rectangle(0, 0, wrapper.Width - 1, wrapper.Height - 1), 22);
                using var br = new SolidBrush(Color.FromArgb(250, 252, 255, 255));
                g.FillPath(br, gp);
                
                using var pen = new Pen(Color.FromArgb(200, 210, 230), 1);
                g.DrawPath(pen, gp);
            };

            txt.Dock = DockStyle.Fill;
            wrapper.Controls.Add(txt);
            
            textBox = txt;
            return wrapper;
        }

        // Método para crear rectángulo redondeado
        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            var gp = new GraphicsPath();
            int d = radius * 2;
            gp.AddArc(rect.X, rect.Y, d, d, 180, 90);
            gp.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            gp.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            gp.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        private void AbrirMain(string usuario)
        {
            // Si tu MainForm tiene un constructor (string usuario)
            var main = new MainForm(usuario)
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            // Cierra toda la app al cerrar el principal
            main.FormClosed += (_, __) => Close();

            // Oculta login y muestra el principal
            Hide();
            main.Show();
        }
    }
}
