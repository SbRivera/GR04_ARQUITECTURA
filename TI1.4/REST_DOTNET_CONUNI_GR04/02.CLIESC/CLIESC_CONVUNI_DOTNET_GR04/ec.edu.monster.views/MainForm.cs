using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using CLIESC_CONVUNI_DOTNET_GR04.Controllers;
using CLIESC_CONVUNI_DOTNET_GR04.Models;

namespace CLIESC_CONVUNI_DOTNET_GR04.Views
{
    public partial class MainForm : Form
    {
        private readonly ConversionController _controller = new ConversionController();
        private readonly string usuarioActivo;

        // UI dinámicos de la tarjeta resultado
        private Panel? resultInnerPanel;
        private Label? resultBigLabel;   // <- SIEMPRE escribimos aquí

        // Headers / hints (decorativos)
        private Label? hdr1, hdr2, hdr3, lblHintTipo;

        // Map categoría -> tipos
        private readonly Dictionary<string, List<OptionItem>> tiposPorCategoria = new();

        public MainForm(string usuario)
        {
            usuarioActivo = usuario;
            InitializeComponent();

            // Asegura que la tarjeta de resultado esté limpia
            cardResultado?.Controls.Clear();

            BuildUI();
            BuildData();
            OnMainResize();
        }

        /* ========================= UI ========================= */

        private void BuildUI()
        {
            // Appbar
            lblSesion.Text = $"Conectado como {usuarioActivo}";
            lblSesion.Font = new Font("Segoe UI", 9f, FontStyle.Regular);

            btnLogout.BackColor = Color.FromArgb(224, 62, 62);
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.MouseEnter += (s, e) => btnLogout.BackColor = Color.FromArgb(245, 90, 90);
            btnLogout.MouseLeave += (s, e) => btnLogout.BackColor = Color.FromArgb(224, 62, 62);
            btnLogout.Click += (s, e) => VolverAlLogin();

            // ===== Tarjeta 1 — Categoría =====
            hdr1 = HeaderLabel("Categoría de Conversión");
            cardCategoria?.Controls.Add(hdr1);

            var hintCat = HintLabel("Seleccionar Categoría");
            cardCategoria?.Controls.Add(hintCat);
            if (hintCat.Parent != null) hintCat.Location = new Point(22, 58);

            cbCategoria ??= new ComboBox();
            StyleField(cbCategoria);
            cbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCategoria.Font = new Font("Segoe UI", 10.5f);
            cbCategoria.Width = 320; cbCategoria.Height = 44;
            PlaceInCard(cardCategoria, cbCategoria, y: 92);
            cbCategoria.SelectedIndexChanged += (s, e) => RecargarTipos();

            // ===== Tarjeta 2 — Tipo =====
            hdr2 = HeaderLabel("Tipo de Conversión");
            cardTipo?.Controls.Add(hdr2);

            lblHintTipo = HintLabel("Primero seleccione una");
            cardTipo?.Controls.Add(lblHintTipo);
            if (lblHintTipo.Parent != null) lblHintTipo.Location = new Point(22, 58);

            cbTipo ??= new ComboBox();
            StyleField(cbTipo);
            cbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTipo.Font = new Font("Segoe UI", 10.5f);
            cbTipo.Width = 320; cbTipo.Height = 44;
            PlaceInCard(cardTipo, cbTipo, y: 92);

            // ===== Tarjeta 3 — Valor =====
            hdr3 = HeaderLabel("Ingrese el Valor");
            cardValor?.Controls.Add(hdr3);

            txtValor ??= new UITheme.PlaceholderTextBox();
            txtValor.Placeholder = "Ingresa el valor numérico";
            txtValor.Font = new Font("Segoe UI", 11f);
            txtValor.Width = 320; txtValor.Height = 44;
            StyleField(txtValor);
            PlaceInCard(cardValor, txtValor, y: 92);

            btnConvertir ??= new UITheme.GradientButton();
            btnConvertir.Text = "CONVERTIR";
            btnConvertir.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            btnConvertir.Size = new Size(320, 44);
            if (cardValor != null)
            {
                btnConvertir.Left = (cardValor.Width - btnConvertir.Width) / 2;
                btnConvertir.Top = 150;
            }

            // CABLEO ÚNICO del click
            btnConvertir.Click -= BtnConvertir_Click;
            btnConvertir.Click += BtnConvertir_Click;
            btnConvertir.BringToFront();
            AcceptButton = btnConvertir;
            cardValor?.Controls.Add(btnConvertir);

            // ===== Tarjeta Resultado =====
            PrepareResultCard();

            // ===== FAB limpiar =====
            btnClearFab.Width = 56; btnClearFab.Height = 56;
            btnClearFab.FlatStyle = FlatStyle.Flat;
            btnClearFab.FlatAppearance.BorderSize = 0;
            btnClearFab.BackColor = Color.Transparent;
            btnClearFab.TabStop = false;
            btnClearFab.Cursor = Cursors.Hand;
            btnClearFab.Paint += DrawTrashFab;
            btnClearFab.Click += (s, e) => ClearAll();
        }

        private void PrepareResultCard()
        {
            if (cardResultado == null) return;
            cardResultado.Controls.Clear();

            // Header
            var hdrRes = new Panel
            {
                BackColor = Color.FromArgb(235, 239, 250),
                Height = 60,
                Dock = DockStyle.Top
            };
            var lblHdrRes = new Label
            {
                Text = "Resultado",
                ForeColor = Color.FromArgb(90, 96, 120),
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(22, 0, 0, 0)
            };
            hdrRes.Controls.Add(lblHdrRes);
            cardResultado.Controls.Add(hdrRes);

            // Panel interior
            resultInnerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(26, 24, 26, 26),
                BackColor = Color.White
            };
            cardResultado.Controls.Add(resultInnerPanel);

            // Label único para el valor convertido
            resultBigLabel = new Label
            {
                Name = "resultBigLabel",
                AutoSize = true,
                Font = new Font("Segoe UI", 22f, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 70, 95),
                Text = "—",
                Visible = true,
                Location = new Point(6, 8)
            };
            resultInnerPanel.Controls.Add(resultBigLabel);

            // Banda de estado inferior (para errores)
            lblMensaje ??= new Label
            {
                AutoSize = false,
                Height = 36,
                Dock = DockStyle.Bottom,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(0, 0, 0, 0),
                ForeColor = Color.FromArgb(120, 120, 120),
                Text = ""
            };
            root.Controls.Add(lblMensaje);

            // Garantiza orden/visibilidad
            resultBigLabel.BringToFront();
            resultInnerPanel.BringToFront();
            cardResultado.BringToFront();
            cardResultado.PerformLayout();
        }

        private static Label HeaderLabel(string text) =>
            new Label
            {
                Text = text,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(22, 22)
            };

        private static Label HintLabel(string text) =>
            new Label
            {
                Text = text,
                AutoSize = true,
                ForeColor = Color.FromArgb(90, 90, 105),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9f, FontStyle.Regular)
            };

        private static void StyleField(Control? c)
        {
            if (c is null) return;
            c.BackColor = Color.White;
            c.ForeColor = Color.FromArgb(40, 40, 40);
        }

        private static void PlaceInCard(Control? parent, Control? child, int y)
        {
            if (parent is null || child is null) return;
            child.Left = (parent.Width - child.Width) / 2;
            child.Top = y;
            if (child.Parent != parent) parent.Controls.Add(child);
        }

        private void OnMainResize()
        {
            if (btnLogout != null)
            {
                btnLogout.Left = this.ClientSize.Width - 190;
                btnLogout.Top = 16;
            }
            if (lblSesion != null)
            {
                lblSesion.Left = this.ClientSize.Width - 360;
                lblSesion.Top = 22;
            }
            if (cardResultado != null)
            {
                cardResultado.Left = (this.ClientSize.Width - cardResultado.Width) / 2;
                cardResultado.Top = 360;
            }
            if (btnClearFab != null)
            {
                btnClearFab.Left = this.ClientSize.Width - btnClearFab.Width - 26;
                btnClearFab.Top = this.ClientSize.Height - btnClearFab.Height - 30;
            }
        }

        private void DrawTrashFab(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int w = btnClearFab.Width, h = btnClearFab.Height;

            using (var shadow = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
                g.FillEllipse(shadow, 4, 6, w - 8, h - 8);

            using (var lg = new LinearGradientBrush(btnClearFab.ClientRectangle,
                       Color.FromArgb(255, 160, 76), Color.FromArgb(237, 93, 74), 45f))
                g.FillEllipse(lg, 0, 0, w - 8, h - 8);

            using (var pen = new Pen(Color.FromArgb(255, 255, 255, 200), 2f))
                g.DrawEllipse(pen, 1, 1, w - 10, h - 10);

            g.TranslateTransform(16, 14);
            using var white = new SolidBrush(Color.White);
            g.FillRectangle(white, 4, 0, 16, 4);
            g.FillRectangle(white, 0, 4, 24, 16);
            using var p = new Pen(Color.White, 2f);
            g.DrawLine(p, 7, 6, 7, 18);
            g.DrawLine(p, 12, 6, 12, 18);
            g.DrawLine(p, 17, 6, 17, 18);
        }

        /* ===================== DATA ===================== */

        private void BuildData()
        {
            tiposPorCategoria["Temperatura"] = new List<OptionItem>
            {
                new("Celsius → Fahrenheit",   "c_to_f",     "°C", "°F"),
                new("Fahrenheit → Celsius",   "f_to_c",     "°F", "°C")
            };
            tiposPorCategoria["Longitud"] = new List<OptionItem>
            {
                new("Centímetros → Pulgadas", "cm_to_inch", "cm", "in"),
                new("Pulgadas → Centímetros", "inch_to_cm", "in", "cm")
            };
            tiposPorCategoria["Peso"] = new List<OptionItem>
            {
                new("Kilogramos → Libras",    "kg_to_lb",   "kg", "lb"),
                new("Libras → Kilogramos",    "lb_to_kg",   "lb", "kg")
            };

            cbCategoria.Items.Clear();
            foreach (var cat in tiposPorCategoria.Keys)
                cbCategoria.Items.Add(cat);

            cbCategoria.SelectedIndex = -1;
            cbTipo.SelectedIndex = -1;
        }

        private void RecargarTipos()
        {
            if (cbTipo == null || cbCategoria == null) return;

            cbTipo.DataSource = null;
            if (cbCategoria.SelectedItem == null)
            {
                cbTipo.SelectedIndex = -1;
                if (lblHintTipo != null) lblHintTipo.Visible = true;
                return;
            }

            var cat = cbCategoria.SelectedItem.ToString()!;
            var list = tiposPorCategoria.TryGetValue(cat, out var l) ? l : new List<OptionItem>();

            cbTipo.DataSource = list;
            cbTipo.DisplayMember = nameof(OptionItem.Text);
            cbTipo.ValueMember = nameof(OptionItem.Value);
            cbTipo.SelectedIndex = -1;

            if (lblHintTipo != null) lblHintTipo.Visible = list.Count == 0;
        }

        /* ===================== CONVERSIÓN ===================== */

        private async void BtnConvertir_Click(object? sender, EventArgs e)
        {
            // feedback inmediato
            if (resultBigLabel != null)
            {
                resultBigLabel.Text = "Convirtiendo…";
                resultBigLabel.ForeColor = Color.FromArgb(60, 70, 95);
                resultBigLabel.Visible = true;
                resultBigLabel.BringToFront();
                resultBigLabel.Parent?.Refresh();
            }

            await ConvertirAsync();
        }

        private async Task ConvertirAsync()
        {
            if (cbTipo?.SelectedItem is not OptionItem sel)
            {
                ShowMessage("Seleccione un tipo de conversión.", Color.OrangeRed);
                return;
            }

            var txt = (txtValor?.Text ?? "").Trim().Replace(',', '.');
            if (!double.TryParse(txt,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out double valor))
            {
                ShowMessage("Ingrese un número válido.", Color.OrangeRed);
                return;
            }

            btnConvertir.Enabled = false;
            ShowMessage("", Color.Transparent);

            var res = await _controller.ConvertAsync(new ConversionRequest { Type = sel.Value, Value = valor });

            btnConvertir.Enabled = true;

            if (!string.IsNullOrWhiteSpace(res.Error))
            {
                ShowMessage($"Error: {res.Error}", Color.OrangeRed);
                return;
            }

            // Fallback si algún middleware cambió el case
            if ((res.Output == null || res.Input == null) && !string.IsNullOrEmpty(res.Raw))
            {
                try
                {
                    using var doc = JsonDocument.Parse(res.Raw);
                    if (res.Input == null && doc.RootElement.TryGetProperty("input", out var i)) res.Input = i.GetString();
                    if (res.Output == null && doc.RootElement.TryGetProperty("output", out var o)) res.Output = o.GetString();
                }
                catch { /* ignore */ }
            }

            var outputText = string.IsNullOrWhiteSpace(res.Output) ? "—" : res.Output!;

            // ---- MOSTRAR SOLO EL RESULTADO GRANDE ----
            if (resultBigLabel != null)
            {
                resultBigLabel.Text = outputText;            // Ej: 113,00 °F
                resultBigLabel.ForeColor = Color.FromArgb(60, 70, 95);
                resultBigLabel.Visible = true;
                resultBigLabel.BringToFront();

                // fuerza repintado visible
                resultBigLabel.Parent?.PerformLayout();
                resultBigLabel.Parent?.Invalidate();
                resultBigLabel.Parent?.Update();
            }

            // Estado (gris) — si no quieres nada, deja cadena vacía
            ShowMessage("", Color.Transparent);
        }

        private void ShowMessage(string text, Color color)
        {
            if (lblMensaje == null) return;
            lblMensaje.Text = text;
            lblMensaje.ForeColor = color;
        }

        private void ClearAll()
        {
            cbCategoria.SelectedIndex = -1;
            cbTipo.DataSource = null;
            cbTipo.SelectedIndex = -1;
            txtValor.Text = "";

            if (resultBigLabel != null)
            {
                resultBigLabel.Text = "—";
                resultBigLabel.ForeColor = Color.FromArgb(60, 70, 95);
                resultBigLabel.Visible = true;
                resultBigLabel.Parent?.Invalidate();
            }

            ShowMessage("", Color.Transparent);
            txtValor.Focus();
        }

        private void VolverAlLogin()
        {
            var login = new LoginForm();
            login.Show();
            this.Hide();
        }

        /* ===================== Helper DTO ===================== */

        private sealed class OptionItem
        {
            public string Text { get; }
            public string Value { get; }
            public string InUnit { get; }
            public string OutUnit { get; }

            public OptionItem(string text, string value, string inUnit, string outUnit)
            {
                Text = text; Value = value; InUnit = inUnit; OutUnit = outUnit;
            }
            public override string ToString() => Text;
        }
    }
}
