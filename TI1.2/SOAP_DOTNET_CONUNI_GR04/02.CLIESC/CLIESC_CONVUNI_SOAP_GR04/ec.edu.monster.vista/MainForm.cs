using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServiceReference1;

namespace ec.edu.monster.vista
{
    public class MainForm : Form
    {
        // Servicio SOAP
        private readonly WSConUniClient _api =
            new WSConUniClient(WSConUniClient.EndpointConfiguration.BasicHttpBinding_IWSConUni);

        // Controles
        private ComboBox cboCategory, cboType;
        private TextBox txtValue;
        private Button btnConvert;
        private Label lblResultTitle, lblResultValue, lblResultMeta;
        private Button fabButton;
        private Panel bg;

        public MainForm(string usuario)
        {
            Text = "Conversor de Unidades";
            Size = new Size(1280, 720);
            MinimumSize = new Size(1200, 650);
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;

            // Fondo degradado
            bg = new Panel { Dock = DockStyle.Fill };
            bg.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var lg = new LinearGradientBrush(
                    bg.ClientRectangle,
                    Color.FromArgb(120, 130, 230),
                    Color.FromArgb(180, 130, 230),
                    135f);
                g.FillRectangle(lg, bg.ClientRectangle);
            };
            Controls.Add(bg);

            // Header
            BuildHeader(usuario);

            // Contenido principal
            BuildMainContent();

            // FAB button
            BuildFabButton();

            Shown += async (_, __) => await EnsureOpenAsync();
            AcceptButton = btnConvert;
        }

        // ==================== HEADER ====================
        private void BuildHeader(string usuario)
        {
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(248, 249, 252)
            };
            bg.Controls.Add(header);

            // Título
            var lblTitle = new Label
            {
                Text = "Conversiones de Unidades",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 85, 135),
                AutoSize = true,
                Location = new Point(35, 22)
            };
            header.Controls.Add(lblTitle);

            // Panel derecho con alineación vertical
            var rightPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 380,
                BackColor = Color.Transparent
            };
            header.Controls.Add(rightPanel);

            // Badge de usuario
            var badge = new Panel
            {
                Size = new Size(200, 34),
                Location = new Point(20, 18),
                BackColor = Color.White
            };
            badge.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                
                // Fondo blanco con bordes redondeados
                using var gp = GetRoundedRect(new Rectangle(0, 0, badge.Width - 1, badge.Height - 1), 17);
                g.FillPath(Brushes.White, gp);
                
                // Borde gris
                using var pen = new Pen(Color.FromArgb(220, 224, 235), 1);
                g.DrawPath(pen, gp);
                
                // Texto "Bienvenido MONSTER"
                var text = $"Bienvenido {usuario.ToUpper()}";
                using var font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
                using var brush = new SolidBrush(Color.FromArgb(40, 45, 60));
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(text, font, brush, badge.ClientRectangle, sf);
            };
            rightPanel.Controls.Add(badge);

            // Botón cerrar sesión
            var btnLogout = new Button
            {
                Text = "CERRAR SESIÓN",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(140, 34),
                Location = new Point(232, 18),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var gp = GetRoundedRect(btnLogout.ClientRectangle, 17);
                using var lg = new LinearGradientBrush(
                    btnLogout.ClientRectangle,
                    Color.FromArgb(255, 85, 90),
                    Color.FromArgb(255, 110, 100),
                    0f);
                g.FillPath(lg, gp);
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(btnLogout.Text, btnLogout.Font, Brushes.White, btnLogout.ClientRectangle, sf);
            };
            btnLogout.Click += (_, __) => { Hide(); Close(); };
            rightPanel.Controls.Add(btnLogout);
        }

        // ==================== CONTENIDO PRINCIPAL ====================
        private void BuildMainContent()
        {
            var container = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40, 65, 40, 30),
                BackColor = Color.Transparent,
                AutoScroll = false
            };
            bg.Controls.Add(container);

            // Panel para las 3 tarjetas superiores
            var topPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 260,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0),
                BackColor = Color.Transparent
            };
            container.Controls.Add(topPanel);

            // Tarjeta 1: Categoría (azul)
            var card1 = BuildCard(
                "Categoría de Conversión",
                Color.FromArgb(77, 103, 247),
                Color.FromArgb(104, 135, 255),
                out var content1
            );
            topPanel.Controls.Add(card1);

            cboCategory = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10.5f),
                Width = 320,
                Height = 35,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(80, 85, 100),
                Location = new Point(30, 15)
            };
            cboCategory.Items.AddRange(new object[] {
                "Seleccionar Categoría",
                "Longitud",
                "Temperatura",
                "Masa"
            });
            cboCategory.SelectedIndex = 0;
            cboCategory.SelectedIndexChanged += CboCategory_SelectedIndexChanged;
            content1.Controls.Add(cboCategory);

            // Tarjeta 2: Tipo (naranja)
            var card2 = BuildCard(
                "Tipo de Conversión",
                Color.FromArgb(255, 168, 76),
                Color.FromArgb(255, 95, 95),
                out var content2
            );
            topPanel.Controls.Add(card2);

            cboType = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10.5f),
                Width = 320,
                Height = 35,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(80, 85, 100),
                Location = new Point(30, 15)
            };
            cboType.Items.Add("Primero seleccione una");
            cboType.SelectedIndex = 0;
            content2.Controls.Add(cboType);

            // Tarjeta 3: Valor (verde)
            var card3 = BuildCard(
                "Ingrese el Valor",
                Color.FromArgb(39, 201, 150),
                Color.FromArgb(53, 210, 135),
                out var content3
            );
            topPanel.Controls.Add(card3);

            txtValue = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f),
                ForeColor = Color.FromArgb(80, 85, 100),
                BackColor = Color.White,
                Text = "0",
                Location = new Point(30, 15),
                Width = 320,
                Height = 35
            };
            txtValue.GotFocus += (s, e) =>
            {
                if (txtValue.Text == "0")
                {
                    txtValue.SelectAll();
                }
            };
            content3.Controls.Add(txtValue);

            // Botón convertir
            btnConvert = new Button
            {
                Text = "CONVERTIR",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(320, 42),
                Location = new Point(30, 65),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(100, 120, 240)
            };
            btnConvert.FlatAppearance.BorderSize = 0;
            btnConvert.Click += Convert_ClickAsync;
            content3.Controls.Add(btnConvert);

            // Ajustar ancho de tarjetas dinámicamente
            topPanel.Resize += (s, e) =>
            {
                int gap = 25;
                int totalGaps = gap * 2;
                int cardWidth = (topPanel.Width - totalGaps) / 3;
                
                if (cardWidth < 320) cardWidth = 320;
                
                card1.Width = cardWidth;
                card2.Width = cardWidth;
                card3.Width = cardWidth;
                card1.Margin = new Padding(0, 0, gap, 0);
                card2.Margin = new Padding(0, 0, gap, 0);
                card3.Margin = new Padding(0, 0, 0, 0);
                
                // Actualizar anchos de controles internos
                int controlWidth = cardWidth - 60;
                
                cboCategory.Width = controlWidth;
                cboType.Width = controlWidth;
                txtValue.Width = controlWidth;
                btnConvert.Width = controlWidth;
            };

            // Tarjeta de resultado
            BuildResultCard(container);
        }

        // ==================== CONSTRUIR TARJETA ====================
        private Panel BuildCard(string title, Color color1, Color color2, out Panel contentArea)
        {
            var card = new Panel
            {
                Width = 380,
                Height = 240,
                BackColor = Color.White
            };

            // Pintar toda la tarjeta con bordes redondeados
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Header con degradado (parte superior)
                var headerRect = new Rectangle(0, 0, card.Width, 60);
                using var headerPath = new GraphicsPath();
                int rad = 15;
                int d = rad * 2;
                
                // Esquinas superiores redondeadas
                headerPath.AddArc(0, 0, d, d, 180, 90);
                headerPath.AddArc(card.Width - d, 0, d, d, 270, 90);
                headerPath.AddLine(card.Width, rad, card.Width, 60);
                headerPath.AddLine(card.Width, 60, 0, 60);
                headerPath.AddLine(0, 60, 0, rad);
                headerPath.CloseFigure();

                using var lgb = new LinearGradientBrush(headerRect, color1, color2, 0f);
                g.FillPath(lgb, headerPath);

                // Ícono "i" en círculo blanco
                var circle = new Rectangle(18, 17, 26, 26);
                g.FillEllipse(Brushes.White, circle);
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString("i", new Font("Segoe UI", 12f, FontStyle.Bold),
                    new SolidBrush(Color.FromArgb(80, 90, 130)), circle, sf);

                // Título del header
                var titleRect = new Rectangle(55, 0, card.Width - 60, 60);
                g.DrawString(title, new Font("Segoe UI", 12.5f, FontStyle.Bold),
                    Brushes.White, titleRect, new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Near
                    });

                // Área de contenido blanco (parte inferior)
                var contentRect = new Rectangle(0, 60, card.Width, card.Height - 60);
                using var contentPath = new GraphicsPath();
                
                // Esquinas inferiores redondeadas
                contentPath.AddLine(0, 60, card.Width, 60);
                contentPath.AddLine(card.Width, 60, card.Width, card.Height - rad);
                contentPath.AddArc(card.Width - d, card.Height - d, d, d, 0, 90);
                contentPath.AddArc(0, card.Height - d, d, d, 90, 90);
                contentPath.AddLine(0, card.Height - rad, 0, 60);
                contentPath.CloseFigure();

                g.FillPath(Brushes.White, contentPath);

                // Borde completo de la tarjeta
                using var borderPath = GetRoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 15);
                using var pen = new Pen(Color.FromArgb(220, 224, 235), 1);
                g.DrawPath(pen, borderPath);
            };

            // Panel de contenido (solo para alojar controles)
            var content = new Panel
            {
                Location = new Point(0, 60),
                Size = new Size(card.Width, card.Height - 60),
                BackColor = Color.Transparent
            };
            card.Controls.Add(content);

            contentArea = content;
            return card;
        }

        // ==================== TARJETA DE RESULTADO ====================
        private void BuildResultCard(Panel container)
        {
            var resultCard = new Panel
            {
                Width = 650,
                Height = 160,
                BackColor = Color.White,
                Location = new Point((container.Width - 650) / 2, 310)
            };
            resultCard.Anchor = AnchorStyles.Top;
            resultCard.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var gp = GetRoundedRect(new Rectangle(0, 0, resultCard.Width - 1, resultCard.Height - 1), 18);
                g.FillPath(Brushes.White, gp);
                
                // Borde sutil
                using var pen = new Pen(Color.FromArgb(220, 224, 235), 1);
                g.DrawPath(pen, gp);
            };

            container.Resize += (s, e) =>
            {
                resultCard.Left = (container.Width - resultCard.Width) / 2;
            };
            container.Controls.Add(resultCard);

            // Título "Resultado"
            lblResultTitle = new Label
            {
                Text = "Resultado",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 120, 220),
                AutoSize = true,
                Location = new Point(25, 20)
            };
            resultCard.Controls.Add(lblResultTitle);

            // Valor del resultado
            lblResultValue = new Label
            {
                Text = "Ingresa un valor y presiona Convertir.",
                Font = new Font("Segoe UI", 10.5f),
                ForeColor = Color.FromArgb(80, 85, 100),
                AutoSize = false,
                Size = new Size(600, 25),
                Location = new Point(25, 50)
            };
            resultCard.Controls.Add(lblResultValue);

            // Metadata
            lblResultMeta = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(130, 135, 150),
                AutoSize = false,
                Size = new Size(600, 22),
                Location = new Point(25, 80)
            };
            resultCard.Controls.Add(lblResultMeta);
        }

        // ==================== FAB BUTTON ====================
        private void BuildFabButton()
        {
            fabButton = new Button
            {
                Size = new Size(56, 56),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            fabButton.FlatAppearance.BorderSize = 0;
            fabButton.Location = new Point(bg.Width - fabButton.Width - 30, bg.Height - fabButton.Height - 30);
            fabButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            
            fabButton.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                
                // Círculo con degradado naranja
                var rect = fabButton.ClientRectangle;
                using var gp = new GraphicsPath();
                gp.AddEllipse(rect);
                
                using var lg = new LinearGradientBrush(
                    rect,
                    Color.FromArgb(255, 140, 80),
                    Color.FromArgb(255, 100, 100),
                    45f);
                g.FillPath(lg, gp);
                
                // Sombra
                using var shadow = new Pen(Color.FromArgb(60, 0, 0, 0), 3);
                g.DrawEllipse(shadow, rect);
                
                // Ícono de limpiar (escoba o brush)
                // Dibujamos un símbolo de borrar/limpiar
                using var pen = new Pen(Color.White, 2.5f);
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                
                // Líneas cruzadas del ícono de limpiar
                int centerX = rect.Width / 2;
                int centerY = rect.Height / 2;
                
                // Círculo con X adentro (símbolo de limpiar/borrar)
                g.DrawEllipse(new Pen(Color.White, 2.5f), centerX - 12, centerY - 12, 24, 24);
                g.DrawLine(new Pen(Color.White, 2.5f), centerX - 6, centerY - 6, centerX + 6, centerY + 6);
                g.DrawLine(new Pen(Color.White, 2.5f), centerX + 6, centerY - 6, centerX - 6, centerY + 6);
            };
            
            fabButton.Click += (s, e) =>
            {
                // Limpiar todos los campos
                cboCategory.SelectedIndex = 0;
                cboType.Items.Clear();
                cboType.Items.Add("Primero seleccione una");
                cboType.SelectedIndex = 0;
                txtValue.Text = "0";
                
                // Limpiar resultados
                lblResultValue.Text = "Ingresa un valor y presiona Convertir.";
                lblResultMeta.Text = "";
            };
            
            bg.Controls.Add(fabButton);
            fabButton.BringToFront();
        }

        // ==================== EVENTOS ====================
        private void CboCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            cboType.Items.Clear();
            switch (cboCategory.SelectedItem?.ToString())
            {
                case "Longitud":
                    cboType.Items.Add("Centímetros (cm) → Pulgadas (in)");
                    cboType.Items.Add("Pulgadas (in) → Centímetros (cm)");
                    break;
                case "Temperatura":
                    cboType.Items.Add("Celsius (°C) → Fahrenheit (°F)");
                    cboType.Items.Add("Fahrenheit (°F) → Celsius (°C)");
                    break;
                case "Masa":
                    cboType.Items.Add("Kilogramos (kg) → Libras (lb)");
                    cboType.Items.Add("Libras (lb) → Kilogramos (kg)");
                    break;
                default:
                    cboType.Items.Add("Primero seleccione una");
                    break;
            }
            if (cboType.Items.Count > 0) cboType.SelectedIndex = 0;
        }

        private async Task EnsureOpenAsync()
        {
            if (_api.State == CommunicationState.Created)
                await _api.OpenAsync();
        }

        private async void Convert_ClickAsync(object? sender, EventArgs e)
        {
            if (cboCategory.SelectedIndex <= 0)
            {
                lblResultValue.Text = "Seleccione una categoría primero.";
                lblResultMeta.Text = "";
                return;
            }

            if (cboType.SelectedIndex < 0 || cboType.SelectedItem?.ToString() == "Primero seleccione una")
            {
                lblResultValue.Text = "Seleccione un tipo de conversión.";
                lblResultMeta.Text = "";
                return;
            }

            var inputText = txtValue.Text;
            if (inputText == txtValue.Tag?.ToString())
                inputText = "";

            if (!TryParse(inputText, out float valor))
            {
                lblResultValue.Text = "Ingrese un valor numérico válido (use coma o punto decimal).";
                lblResultMeta.Text = "";
                return;
            }

            string tipo = cboType.SelectedItem!.ToString()!;
            string cat = cboCategory.SelectedItem!.ToString()!;

            try
            {
                await EnsureOpenAsync();

                float resultado;
                string unidadOrigen, unidadDestino;

                switch (tipo)
                {
                    case "Centímetros (cm) → Pulgadas (in)":
                        resultado = await _api.CentimetrosAPulgadasAsync(valor);
                        unidadOrigen = "cm";
                        unidadDestino = "in";
                        break;
                    case "Pulgadas (in) → Centímetros (cm)":
                        resultado = await _api.PulgadasACentimetrosAsync(valor);
                        unidadOrigen = "in";
                        unidadDestino = "cm";
                        break;
                    case "Celsius (°C) → Fahrenheit (°F)":
                        resultado = await _api.CelsiusAFahrenheitAsync(valor);
                        unidadOrigen = "°C";
                        unidadDestino = "°F";
                        break;
                    case "Fahrenheit (°F) → Celsius (°C)":
                        resultado = await _api.FahrenheitACelsiusAsync(valor);
                        unidadOrigen = "°F";
                        unidadDestino = "°C";
                        break;
                    case "Kilogramos (kg) → Libras (lb)":
                        resultado = await _api.KilogramosALibrasAsync(valor);
                        unidadOrigen = "kg";
                        unidadDestino = "lb";
                        break;
                    case "Libras (lb) → Kilogramos (kg)":
                        resultado = await _api.LibrasAKilogramosAsync(valor);
                        unidadOrigen = "lb";
                        unidadDestino = "kg";
                        break;
                    default:
                        throw new NotSupportedException("Tipo de conversión no soportado");
                }

                lblResultValue.Text = $"{valor:0.####} {unidadOrigen} = {resultado:0.####} {unidadDestino}";
                lblResultMeta.Text = $"{cat} — {tipo}";
            }
            catch (Exception ex)
            {
                lblResultValue.Text = $"Error al convertir: {ex.Message}";
                lblResultMeta.Text = "Verifique que el servicio SOAP esté disponible.";
            }
        }

        // ==================== UTILIDADES ====================
        private static bool TryParse(string text, out float val)
        {
            val = 0;
            if (string.IsNullOrWhiteSpace(text)) return false;
            if (float.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out val)) return true;
            if (float.TryParse(text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out val)) return true;
            return float.TryParse(text.Replace('.', ','), NumberStyles.Float, new CultureInfo("es-ES"), out val);
        }

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
    }
}
