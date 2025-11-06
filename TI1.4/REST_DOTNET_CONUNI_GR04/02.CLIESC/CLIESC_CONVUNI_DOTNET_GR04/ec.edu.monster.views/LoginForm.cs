using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CLIESC_CONVUNI_DOTNET_GR04.Views
{
    public partial class LoginForm : Form
    {
        private PictureBox pbAvatar;

        private const string AvatarPath =
            @"C:\Users\Marcelo Echeverría Y\Desktop\TI1.1\GR04_ARQUITECTURA\TI1.4\REST_DOTNET_CONUNI_GR04\02.CLIESC\CLIESC_CONVUNI_DOTNET_GR04\img\sullivan.jpg";

        public LoginForm()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            SetCueBanner(txtUsuario, "Usuario");
            SetCueBanner(txtPassword, "Contraseña");
            this.AcceptButton = btnLogin;

            // ---------- Avatar sin duplicados ----------
            pbAvatar = this.Controls
                .OfType<PictureBox>()
                .FirstOrDefault(p => p.Name == "pbAvatar" || Equals(p.Tag, "avatar"));

            if (pbAvatar == null)
            {
                pbAvatar = new PictureBox
                {
                    Name = "pbAvatar",
                    Tag = "avatar",
                    Size = new Size(120, 120),
                    BackColor = Color.Transparent,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.None
                };
                this.Controls.Add(pbAvatar); // si tienes un panel/card, agrégalo allí
            }

            foreach (var extra in this.Controls
                         .OfType<PictureBox>()
                         .Where(p => p != pbAvatar && (p.Name == "pbAvatar" || Equals(p.Tag, "avatar")))
                         .ToList())
            {
                this.Controls.Remove(extra);
                extra.Dispose();
            }

            pbAvatar.BringToFront();
            UITheme.MakeCircleAvatar(pbAvatar); // aro + sombra + recorte circular

            void CenterAvatar()
            {
                var host = pbAvatar.Parent ?? this;
                pbAvatar.Left = (host.ClientSize.Width - pbAvatar.Width) / 2;
                pbAvatar.Top = 42;
            }
            this.Resize += (_, __) => CenterAvatar();
            CenterAvatar();

            // Cargar imagen con "zoom crop" para ocultar el aro impreso en el JPG
            _ = SetAvatarFromPath(AvatarPath, zoom: 1.10f); // 1.08–1.15 suele ir bien
        }

        // =======================
        //  Eventos
        // =======================
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, completa ambos campos.", "Campos vacíos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtUsuario.Text == "MONSTER" && txtPassword.Text == "MONSTER9")
            {
                var main = new MainForm(txtUsuario.Text);
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =======================
        //  Utilidades
        // =======================

        /// <summary>
        /// Carga la imagen de avatar y aplica un zoom recortando los bordes,
        /// para ocultar marcos/aros impresos dentro del JPG original.
        /// </summary>
        public bool SetAvatarFromPath(string path, float zoom = 1.10f)
        {
            try
            {
                if (!File.Exists(path)) return false;
                using var tmp = new Bitmap(path);
                pbAvatar.Image = ZoomCrop(tmp, zoom);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Crea un nuevo bitmap escalado: amplía la imagen 'zoom' veces y recorta los bordes.
        /// </summary>
        private static Bitmap ZoomCrop(Image src, float zoom)
        {
            if (zoom < 1.0f) zoom = 1.0f;
            int W = src.Width, H = src.Height;

            // Tamaño de la ventana fuente (más pequeña) para "acercar"
            int sw = (int)(W / zoom);
            int sh = (int)(H / zoom);
            int sx = (W - sw) / 2;
            int sy = (H - sh) / 2;

            var dest = new Bitmap(W, H);
            using (var g = Graphics.FromImage(dest))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.Clear(Color.Transparent);
                g.DrawImage(src,
                    new Rectangle(0, 0, W, H),
                    new Rectangle(sx, sy, sw, sh),
                    GraphicsUnit.Pixel);
            }
            return dest;
        }

        /// <summary>
        /// Placeholder nativo (cue banner) para TextBox.
        /// </summary>
        private static void SetCueBanner(TextBox tb, string text)
        {
            if (tb == null || tb.IsDisposed) return;
            SendMessage(tb.Handle, EM_SETCUEBANNER, (IntPtr)1, text);
        }

        private const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
    }
}
