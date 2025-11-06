using System;
using System.Collections.Generic;
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
        private readonly string usuarioActivo;

        private Label hdr1, hdr2, hdr3;         // headers de tarjetas
        private Label lblHintTipo, lblHintValor;

        // mapping categorías -> lista de tipos (texto, valor)
        private readonly Dictionary<string, List<OptionItem>> tiposPorCategoria = new();

        // ----- Colores de header por tarjeta (maqueta) -----
        static readonly Color HDR_BLUE_A = Color.FromArgb(70, 125, 255);
        static readonly Color HDR_BLUE_B = Color.FromArgb(58, 98, 205);
        static readonly Color HDR_ORANGE_A = Color.FromArgb(255, 173, 72);
        static readonly Color HDR_ORANGE_B = Color.FromArgb(243, 112, 85);
        static readonly Color HDR_GREEN_A = Color.FromArgb(48, 201, 134);
        static readonly Color HDR_GREEN_B = Color.FromArgb(23, 162, 132);

        public MainForm(string usuario)
        {
            usuarioActivo = usuario;
            InitializeComponent();
            BuildUI();
            BuildData();
            OnMainResize();
        }

        private void BuildUI()
        {
            // ===== Appbar texto real =====
            lblSesion.Text = $"Conectado como {usuarioActivo}";
            lblSesion.Font = new Font("Segoe UI", 9f, FontStyle.Regular);

            btnLogout.MouseEnter += (s, e) => btnLogout.BackColor = Color.FromArgb(245, 90, 90);
            btnLogout.MouseLeave += (s, e) => btnLogout.BackColor = Color.FromArgb(224, 62, 62);

            // === Colores por tarjeta (degradado del header) ===
            cardCategoria.TopColor = HDR_BLUE_A; cardCategoria.BottomColor = HDR_BLUE_B;
            cardTipo.TopColor = HDR_ORANGE_A; cardTipo.BottomColor = HDR_ORANGE_B;
            cardValor.TopColor = HDR_GREEN_A; cardValor.BottomColor = HDR_GREEN_B;
            // El resultado va clarito
            cardResultado.TopColor = Color.FromArgb(235, 239, 250);
            cardResultado.BottomColor = Color.FromArgb(235, 239, 250);

            // ====== Card 1 — Categoría ======
            hdr1 = HeaderLabel("Categoría de Conversión");
            cardCategoria.Controls.Add(hdr1);

            cbCategoria = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10.5f),
                Width = 320,
                Height = 44
            };
            StyleField(cbCategoria);
            cbCategoria.SelectedIndexChanged += (s, e) => RecargarTipos();
            PlaceInCard(cardCategoria, cbCategoria, y: 90);

            var hint1 = HintLabel("Seleccionar Categoría");
            cardCategoria.Controls.Add(hint1);
            hint1.Location = new Point((cardCategoria.Width - hint1.Width) / 2, 60);

            // ====== Card 2 — Tipo ======
            hdr2 = HeaderLabel("Tipo de Conversión");
            cardTipo.Controls.Add(hdr2);

            cbTipo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10.5f),
                Width = 320,
                Height = 44
            };
            StyleField(cbTipo);
            PlaceInCard(cardTipo, cbTipo, y: 90);

            lblHintTipo = HintLabel("Primero seleccione una");
            cardTipo.Controls.Add(lblHintTipo);
            lblHintTipo.Location = new Point((cardTipo.Width - lblHintTipo.Width) / 2, 60);

            // ====== Card 3 — Valor ======
            hdr3 = HeaderLabel("Ingrese el Valor");
            cardValor.Controls.Add(hdr3);

            txtValor = new UITheme.PlaceholderTextBox
            {
                Placeholder = "Ingresa el valor numérico",
                Font = new Font("Segoe UI", 11f),
                Width = 320,
                Height = 44
            };
            StyleField(txtValor);
            PlaceInCard(cardValor, txtValor, y: 90);

            btnConvertir = new UITheme.GradientButton
            {
                Text = "CONVERTIR",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Size = new Size(320, 44),
                Top = 150,
                Left = (cardValor.Width - 320) / 2
            };
            btnConvertir.Click += async (s, e) => await ConvertirAsync();
            cardValor.Controls.Add(btnConvertir);

            // ====== Card Resultado ======
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

            // Más aire arriba (Top padding 32 en lugar de 18)
            var inner = new Panel { Dock = DockStyle.Fill, Padding = new Padding(26, 32, 26, 26), BackColor = Color.White };
            cardResultado.Controls.Add(inner);

            lblOutMain = new Label { Text = "—", Font = new Font("Segoe UI", 22f, FontStyle.Bold), AutoSize = true };
            lblIn = new Label { Text = "Ingresa un valor y presiona Convertir.", AutoSize = true, ForeColor = Color.FromArgb(90, 90, 90) };
            lblDetalle = new Label { Text = "", AutoSize = true, ForeColor = Color.FromArgb(90, 90, 90) };

            inner.Controls.Add(lblOutMain);
            inner.Controls.Add(lblIn);
            inner.Controls.Add(lblDetalle);
            lblOutMain.Location = new Point(6, 18); // un poco más abajo
            lblIn.Location = new Point(6, 62);
            lblDetalle.Location = new Point(6, 90);

            // Mensaje inferior (validación / estado)
            lblMensaje = new Label
            {
                AutoSize = false,
                Height = 36,
                Dock = DockStyle.Bottom,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(0, 0, 0, 0)
            };
            root.Controls.Add(lblMensaje);

            // ===== FAB borrar =====
            btnClearFab.Paint += DrawTrashFab;
            btnClearFab.Click += (s, e) => ClearAll();
            AcceptButton = btnConvertir;
        }

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

        // ===== Helpers de estilo/posicionamiento =====
        private static Label HeaderLabel(string text)
        {
            var lbl = new Label
            {
                Text = text,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(22, 22)
            };
            return lbl;
        }

        private static Label HintLabel(string text)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                ForeColor = Color.FromArgb(90, 90, 105),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9f, FontStyle.Regular)
            };
        }

        private static void StyleField(Control c)
        {
            c.BackColor = Color.White;
            c.ForeColor = Color.FromArgb(40, 40, 40);
        }

        private static void PlaceInCard(Control parent, Control child, int y)
        {
            child.Left = (parent.Width - child.Width) / 2;
            child.Top = y;
            parent.Controls.Add(child);
        }

        private void OnMainResize()
        {
            // Reubicar appbar labels/botón
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

            // Centrar resultado (lo bajamos un poco)
            if (cardResultado != null)
            {
                cardResultado.Left = (this.ClientSize.Width - cardResultado.Width) / 2;
                cardResultado.Top = 380; // antes 360
            }

            // FAB en esquina inferior derecha (margen 26/30)
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

            // iconito de papelera
            g.TranslateTransform(16, 14);
            using var white = new SolidBrush(Color.White);
            g.FillRectangle(white, 4, 0, 16, 4);             // tapa
            g.FillRectangle(white, 0, 4, 24, 16);            // cuerpo
            using var p = new Pen(Color.White, 2f);
            g.DrawLine(p, 7, 6, 7, 18);
            g.DrawLine(p, 12, 6, 12, 18);
            g.DrawLine(p, 17, 6, 17, 18);
        }

        // ===== Lógica =====

        private void RecargarTipos()
        {
            cbTipo.DataSource = null;
            if (cbCategoria.SelectedItem == null) { cbTipo.SelectedIndex = -1; return; }

            var cat = cbCategoria.SelectedItem.ToString()!;
            var list = tiposPorCategoria.TryGetValue(cat, out var l) ? l : new List<OptionItem>();
            cbTipo.DataSource = list;
            cbTipo.DisplayMember = nameof(OptionItem.Text);
            cbTipo.ValueMember = nameof(OptionItem.Value);
            cbTipo.SelectedIndex = -1;

            lblHintTipo.Visible = list.Count == 0;
        }

        private async Task ConvertirAsync()
        {
            if (cbTipo.SelectedItem is not OptionItem sel)
            {
                ShowMessage("Seleccione un tipo de conversión.", Color.OrangeRed);
                return;
            }
            if (!double.TryParse(txtValor.Text.Trim(), out double valor))
            {
                ShowMessage("Ingrese un número válido.", Color.OrangeRed);
                return;
            }

            btnConvertir.Enabled = false;
            ShowMessage("Procesando conversión…", Color.FromArgb(120, 180, 255));

            var req = new ConversionRequest { Type = sel.Value, Value = valor };
            var res = await _controller.ConvertAsync(req);

            btnConvertir.Enabled = true;

            if (!string.IsNullOrEmpty(res.Error))
            {
                ShowMessage($"Error: {res.Error}", Color.OrangeRed);
                return;
            }

            // Mostrar usando las unidades del OptionItem seleccionado
            lblOutMain.Text = $"{res.Output:0.######} {sel.OutUnit}";
            lblIn.Text = $"Entrada: {res.Input:0.######} {sel.InUnit}";
            lblDetalle.Text = sel.Text;

            ShowMessage("Conversión realizada con éxito.", Color.FromArgb(22, 118, 38));
        }

        private void ShowMessage(string text, Color color)
        {
            lblMensaje.Text = text;
            lblMensaje.ForeColor = color;
        }

        private void ClearAll()
        {
            cbCategoria.SelectedIndex = -1;
            cbTipo.DataSource = null;
            cbTipo.SelectedIndex = -1;
            txtValor.Text = "";
            lblOutMain.Text = "—";
            lblIn.Text = "Ingresa un valor y presiona Convertir.";
            lblDetalle.Text = "";
            ShowMessage("Campos reiniciados.", Color.FromArgb(60, 60, 60));
            txtValor.Focus();
        }

        private void VolverAlLogin()
        {
            var login = new LoginForm();
            login.Show();
            this.Hide();
        }

        // Item helper
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
