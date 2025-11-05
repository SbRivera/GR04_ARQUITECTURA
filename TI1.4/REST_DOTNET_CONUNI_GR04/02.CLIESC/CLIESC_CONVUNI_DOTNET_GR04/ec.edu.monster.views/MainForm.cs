using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using CLIESC_CONVUNI_DOTNET_GR04.Controllers;
using CLIESC_CONVUNI_DOTNET_GR04.Models;

namespace CLIESC_CONVUNI_DOTNET_GR04.Views
{
    public partial class MainForm : Form
    {
        private readonly ConversionController _controller = new ConversionController();

        private Label lblEstadoConexion;
        private Label lblSesion;
        private Label lblFooter;
        private Button btnLogout;

        // Títulos (encima de los campos)
        private Label lblTipoTitulo;
        private Label lblValorTitulo;

        private System.Windows.Forms.Timer glowTimer;
        private System.Windows.Forms.Timer colorTimer;
        private int glowIntensity = 0;
        private bool glowIncreasing = true;
        private int colorPhase = 0;

        private readonly string usuarioActivo;

        // Recibe el usuario desde el login
        public MainForm(string usuario)
        {
            usuarioActivo = usuario;
            InitializeComponent();
            ConfigurarInterfaz();
            IniciarAnimacionTitulo();
            IniciarAnimacionFondo();
        }

        // ===============  UI / ESTILO  ===============
        private void ConfigurarInterfaz()
        {
            this.DoubleBuffered = true;
            this.Paint += (s, e) => DibujarFondo(e.Graphics);

            // --- Sesión activa (arriba-izquierda) ---
            lblSesion = new Label
            {
                Text = $"Sesión activa: {usuarioActivo}",
                ForeColor = Color.FromArgb(130, 180, 255),
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(20, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            panelTop.Controls.Add(lblSesion);
            lblSesion.BringToFront();

            // --- Botón Cerrar sesión (arriba-derecha) ---
            btnLogout = new Button
            {
                Text = "⏻  Cerrar sesión",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 70, 130),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Size = new Size(130, 28),
                Location = new Point(panelTop.Width - 150, 16),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.MouseEnter += (s, e) => btnLogout.BackColor = Color.FromArgb(80, 100, 180);
            btnLogout.MouseLeave += (s, e) => btnLogout.BackColor = Color.FromArgb(60, 70, 130);
            btnLogout.Click += (s, e) => VolverAlLogin();
            panelTop.Controls.Add(btnLogout);
            btnLogout.BringToFront();

            // Re-centrar “Sesión activa” verticalmente si cambia el alto del panel
            panelTop.Resize += (s, e) =>
            {
                lblSesion.Location = new Point(20, (panelTop.Height - lblSesion.Height) / 2);
            };

            // --- Etiquetas título sobre cbTipo y txtValor ---
            // Ajusta offX/sepY si quieres moverlos (sepY = distancia vertical sobre el control)
            int offX = 0;
            int sepY = -58;

            lblTipoTitulo = CrearTituloSobreControl(cbTipo, "Unidad de conversión", offX, sepY);
            lblValorTitulo = CrearTituloSobreControl(txtValor, "Valor a convertir", offX, sepY);

            // Reposicionar en cada resize (NO recrear)
            this.Resize += (s, e) =>
            {
                ActualizarTituloPosicion(cbTipo, lblTipoTitulo, offX, sepY);
                ActualizarTituloPosicion(txtValor, lblValorTitulo, offX, sepY);
                PosicionarEstadoConexion();
            };

            // --- Indicador de conexión (abajo-izquierda, por encima del footer) ---
            lblEstadoConexion = new Label
            {
                Text = "Servidor: Conectado",
                ForeColor = Color.FromArgb(0, 220, 140),
                Font = new Font("Consolas", 9F, FontStyle.Italic),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblEstadoConexion);
            lblEstadoConexion.BringToFront();
            PosicionarEstadoConexion(); // Posición inicial (sin footer aún)

            // --- Footer (abajo centrado) ---
            lblFooter = new Label
            {
                Text = $"Cliente REST | {DateTime.Now:dd MMM yyyy HH:mm}",
                ForeColor = Color.FromArgb(90, 150, 200),
                Font = new Font("Consolas", 9F, FontStyle.Italic),
                Dock = DockStyle.Bottom,
                Height = 22,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblFooter);

            // Ajustar posición del estado ahora que existe el footer
            PosicionarEstadoConexion();

            // Actualiza hora del footer
            var reloj = new System.Windows.Forms.Timer { Interval = 1000 };
            reloj.Tick += (s, e) => lblFooter.Text = $"Cliente REST | {DateTime.Now:dd MMM yyyy HH:mm}";
            reloj.Start();

            // Separador inferior del panelTop (cosmético)
            var separador = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = Color.FromArgb(40, 60, 100)
            };
            panelTop.Controls.Add(separador);
            separador.BringToFront();
        }

        private void PosicionarEstadoConexion()
        {
            if (lblEstadoConexion == null || lblEstadoConexion.IsDisposed) return;

            int margen = 8;
            int footerH = (lblFooter != null && !lblFooter.IsDisposed) ? lblFooter.Height : 0;
            int y = this.ClientSize.Height - footerH - lblEstadoConexion.Height - margen;
            if (y < margen) y = margen;

            lblEstadoConexion.Location = new Point(margen, y);
        }
        private void StyleLabel(Label l)
        {
            l.FlatStyle = FlatStyle.Standard;      // evita glow morado
            l.BackColor = Color.Transparent;       // sin recuadro
            l.UseCompatibleTextRendering = false;  // texto GDI (estable)
        }
        // Crear y posicionar un título sobre un control
        private Label CrearTituloSobreControl(Control control, string texto, int offX = 0, int sepY = 22)
        {
            var lbl = new Label
            {
                Text = texto,
                ForeColor = Color.FromArgb(150, 180, 220),
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                AutoSize = true
            };
            StyleLabel(lbl);
            this.Controls.Add(lbl);
            lbl.BringToFront();

            ActualizarTituloPosicion(control, lbl, offX, sepY);
            control.LocationChanged += (s, e) => ActualizarTituloPosicion(control, lbl, offX, sepY);
            control.SizeChanged += (s, e) => ActualizarTituloPosicion(control, lbl, offX, sepY);
            return lbl;
        }

        private void ActualizarTituloPosicion(Control control, Label lbl, int offX, int sepY)
        {
            // sepY es cuánto “por encima” del control queda el título
            // (a mayor sepY más arriba)
            lbl.Location = new Point(control.Left + offX, Math.Max(5, control.Top - sepY));
        }

        // 🔙 Volver al login
        private void VolverAlLogin()
        {
            var login = new LoginForm();
            login.Show();
            this.Hide();
        }

        // ===============  ANIMACIONES / FONDO  ===============
        private void DibujarFondo(Graphics g)
        {
            Rectangle rect = this.ClientRectangle;
            Color c1 = Color.FromArgb(10, 10, 30);
            Color c2 = Color.FromArgb(25 + colorPhase, 20, 60 + colorPhase);

            using (var brush = new LinearGradientBrush(rect, c1, c2, 120f))
            {
                g.FillRectangle(brush, rect);
            }
        }

        private void IniciarAnimacionFondo()
        {
            colorTimer = new System.Windows.Forms.Timer { Interval = 80 };
            colorTimer.Tick += (s, e) =>
            {
                colorPhase++;
                if (colorPhase > 30) colorPhase = 0;
                this.Invalidate();
            };
            colorTimer.Start();
        }

        private void IniciarAnimacionTitulo()
        {
            glowTimer = new System.Windows.Forms.Timer { Interval = 70 };
            glowTimer.Tick += (s, e) =>
            {
                glowIntensity += glowIncreasing ? 4 : -4;
                if (glowIntensity >= 80) glowIncreasing = false;
                if (glowIntensity <= 0) glowIncreasing = true;

                int g = Math.Min(255, 180 + glowIntensity);
                lblTitulo.ForeColor = Color.FromArgb(100, g, 255);
            };
            glowTimer.Start();
        }

        // ===============  COMBO DRAW / VALIDACIÓN  ===============
        private void cbTipo_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            string text = cbTipo.Items[e.Index].ToString();
            bool isSection = text.StartsWith("—");

            e.DrawBackground();

            Color bg = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                ? Color.FromArgb(60, 110, 255)
                : Color.FromArgb(25, 25, 45);

            Color fg = isSection ? Color.FromArgb(100, 160, 220) : Color.White;

            using (var bgb = new SolidBrush(bg))
                e.Graphics.FillRectangle(bgb, e.Bounds);
            using (var fgb = new SolidBrush(fg))
                e.Graphics.DrawString(text, e.Font, fgb, e.Bounds.X + (isSection ? 20 : 6), e.Bounds.Y + 3);

            e.DrawFocusRectangle();
        }

        private void cbTipo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbTipo.SelectedItem == null) return;
            string item = cbTipo.SelectedItem.ToString();
            if (item.StartsWith("—"))
            {
                cbTipo.SelectedIndex = -1;
                cbTipo.Refresh();
            }
        }

        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-') e.Handled = true;
            if (e.KeyChar == '.' && txtValor.Text.Contains('.')) e.Handled = true;
            if (e.KeyChar == '-' && txtValor.SelectionStart != 0) e.Handled = true;
        }

        // ===============  CONVERSIÓN  ===============
        private async void btnConvertir_Click(object sender, EventArgs e)
        {
            string tipo = cbTipo.SelectedItem?.ToString();
            string valorTexto = txtValor.Text.Trim();

            if (string.IsNullOrEmpty(tipo) || tipo.StartsWith("—"))
            {
                MostrarMensaje("Selecciona un tipo de conversión válido.", Color.OrangeRed);
                return;
            }

            if (!double.TryParse(valorTexto, out double valor))
            {
                MostrarMensaje("Ingrese un número válido.", Color.OrangeRed);
                return;
            }

            bool esTemperatura = tipo.StartsWith("c_") || tipo.StartsWith("f_");
            if (!esTemperatura && valor < 0)
            {
                MostrarMensaje("Solo las conversiones de temperatura aceptan valores negativos.", Color.OrangeRed);
                return;
            }

            btnConvertir.Enabled = false;
            MostrarMensaje("Convirtiendo...", Color.LightSkyBlue);

            var request = new ConversionRequest { Type = tipo, Value = valor };
            var response = await _controller.ConvertAsync(request);

            btnConvertir.Enabled = true;

            if (!string.IsNullOrEmpty(response.Error))
            {
                MostrarMensaje($"Error: {response.Error}", Color.OrangeRed);
                lblEstadoConexion.Text = "Servidor: Desconectado";
                lblEstadoConexion.ForeColor = Color.OrangeRed;
                return;
            }

            lblEstadoConexion.Text = "Servidor: Conectado";
            lblEstadoConexion.ForeColor = Color.FromArgb(0, 220, 140);

            string resultado = $"{response.Input}  ➜  {response.Output}";
            MostrarMensaje(resultado, Color.FromArgb(120, 255, 200));
            await AnimarResultado();
        }

        private async Task AnimarResultado()
        {
            for (int i = 0; i < 3; i++)
            {
                lblResultado.ForeColor = Color.FromArgb(80, 255, 180);
                await Task.Delay(120);
                lblResultado.ForeColor = Color.FromArgb(100, 255, 200);
                await Task.Delay(120);
            }
        }

        private void MostrarMensaje(string texto, Color color)
        {
            lblResultado.ForeColor = color;
            lblResultado.Text = texto;
        }
    }
}
