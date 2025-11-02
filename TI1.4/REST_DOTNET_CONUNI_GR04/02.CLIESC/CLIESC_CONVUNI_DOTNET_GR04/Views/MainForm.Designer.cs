using System.Drawing;
using System.Windows.Forms;

namespace CLIESC_CONVUNI_DOTNET_GR04.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cbTipo;
        private TextBox txtValor;
        private Button btnConvertir;
        private Label lblResultado;
        private Label lblTitulo;
        private TableLayoutPanel layoutPanel;
        private Panel panelTop;
        private Panel panelBottom;
        private Label lblUnidadEntrada;
        private Label lblUnidadSalida;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cbTipo = new ComboBox();
            this.txtValor = new TextBox();
            this.btnConvertir = new Button();
            this.lblResultado = new Label();
            this.lblTitulo = new Label();
            this.panelTop = new Panel();
            this.layoutPanel = new TableLayoutPanel();
            this.panelBottom = new Panel();
            this.lblUnidadEntrada = new Label();
            this.lblUnidadSalida = new Label();

            this.SuspendLayout();

            // === Ventana Principal ===
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(10, 10, 25);
            this.ClientSize = new Size(900, 500);
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.ForeColor = Color.White;
            this.Text = "Conversor de Unidades - Cliente REST";
            this.MinimumSize = new Size(750, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            // === Panel superior ===
            this.panelTop.BackColor = Color.FromArgb(20, 20, 45);
            this.panelTop.Dock = DockStyle.Top;
            this.panelTop.Height = 70;

            // === Título ===
            this.lblTitulo.AutoSize = false;
            this.lblTitulo.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(100, 200, 255);
            this.lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTitulo.Dock = DockStyle.Fill;
            this.lblTitulo.Text = "CONVERSOR UNIVERSAL DE UNIDADES";
            this.panelTop.Controls.Add(this.lblTitulo);

            // === Layout principal (responsive) ===
            this.layoutPanel.ColumnCount = 3;
            this.layoutPanel.RowCount = 2;
            this.layoutPanel.Dock = DockStyle.Fill;
            this.layoutPanel.Padding = new Padding(40, 30, 40, 30);
            this.layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            this.layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            this.layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            this.layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            this.layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.layoutPanel.BackColor = Color.FromArgb(15, 15, 35);

            // === ComboBox (tipo de conversión) ===
            this.cbTipo.BackColor = Color.FromArgb(25, 25, 45);
            this.cbTipo.FlatStyle = FlatStyle.Flat;
            this.cbTipo.ForeColor = Color.FromArgb(180, 220, 255);
            this.cbTipo.Font = new Font("Segoe UI", 10F);
            this.cbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbTipo.DrawMode = DrawMode.OwnerDrawFixed;
            this.cbTipo.DrawItem += new DrawItemEventHandler(this.cbTipo_DrawItem);
            this.cbTipo.Items.AddRange(new object[]
            {
                "— Temperatura —",
                "c_to_f",
                "f_to_c",
                "— Longitud —",
                "cm_to_inch",
                "inch_to_cm",
                "— Peso —",
                "kg_to_lb",
                "lb_to_kg"
            });
            this.cbTipo.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            // === TextBox ===
            this.txtValor.BackColor = Color.FromArgb(25, 25, 45);
            this.txtValor.BorderStyle = BorderStyle.FixedSingle;
            this.txtValor.ForeColor = Color.White;
            this.txtValor.Font = new Font("Consolas", 11F);
            this.txtValor.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtValor.KeyPress += new KeyPressEventHandler(this.txtValor_KeyPress);

            // === Botón ===
            this.btnConvertir.BackColor = Color.FromArgb(60, 120, 255);
            this.btnConvertir.FlatStyle = FlatStyle.Flat;
            this.btnConvertir.FlatAppearance.BorderSize = 0;
            this.btnConvertir.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            this.btnConvertir.ForeColor = Color.White;
            this.btnConvertir.Text = "Convertir";
            this.btnConvertir.Width = 120;
            this.btnConvertir.Height = this.txtValor.Height;
            this.btnConvertir.Anchor = AnchorStyles.Left;
            this.btnConvertir.Click += new System.EventHandler(this.btnConvertir_Click);

            // === Etiquetas ===
            this.lblUnidadEntrada.Text = "Unidad de entrada";
            this.lblUnidadEntrada.ForeColor = Color.FromArgb(130, 150, 180);
            this.lblUnidadEntrada.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            this.lblUnidadEntrada.Dock = DockStyle.Bottom;
            this.lblUnidadEntrada.TextAlign = ContentAlignment.MiddleCenter;

            this.lblUnidadSalida.Text = "Unidad de salida";
            this.lblUnidadSalida.ForeColor = Color.FromArgb(130, 150, 180);
            this.lblUnidadSalida.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            this.lblUnidadSalida.Dock = DockStyle.Bottom;
            this.lblUnidadSalida.TextAlign = ContentAlignment.MiddleCenter;

            // === Resultado ===
            this.lblResultado.AutoSize = false;
            this.lblResultado.Dock = DockStyle.Fill;
            this.lblResultado.Font = new Font("Consolas", 13F, FontStyle.Italic);
            this.lblResultado.ForeColor = Color.FromArgb(100, 255, 200);
            this.lblResultado.Text = "Esperando datos...";
            this.lblResultado.TextAlign = ContentAlignment.MiddleCenter;

            // === Panel inferior ===
            this.panelBottom.Dock = DockStyle.Bottom;
            this.panelBottom.Height = 110;
            this.panelBottom.BackColor = Color.FromArgb(10, 10, 25);
            this.panelBottom.Controls.Add(this.lblResultado);

            // === Añadir al layout ===
            this.layoutPanel.Controls.Add(this.cbTipo, 0, 0);
            this.layoutPanel.Controls.Add(this.txtValor, 1, 0);
            this.layoutPanel.Controls.Add(this.btnConvertir, 2, 0);
            this.layoutPanel.Controls.Add(this.lblUnidadEntrada, 0, 1);
            this.layoutPanel.Controls.Add(this.lblUnidadSalida, 2, 1);

            // === Orden de agregado ===
            this.Controls.Add(this.layoutPanel);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);

            this.ResumeLayout(false);

            this.cbTipo.DrawItem += new DrawItemEventHandler(this.cbTipo_DrawItem);
            this.btnConvertir.Click += new System.EventHandler(this.btnConvertir_Click);
            this.txtValor.KeyPress += new KeyPressEventHandler(this.txtValor_KeyPress);

        }
    }
}
