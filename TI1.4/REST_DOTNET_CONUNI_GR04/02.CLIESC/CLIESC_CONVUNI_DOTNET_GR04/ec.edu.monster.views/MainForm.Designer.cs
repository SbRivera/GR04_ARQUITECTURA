using System.Drawing;
using System.Windows.Forms;

namespace CLIESC_CONVUNI_DOTNET_GR04.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Contenedor raíz (fondo con gradiente)
        private UITheme.GradientPanel root;

        // Appbar
        private Panel appbar;
        private Label lblTitulo;
        private Label lblSesion;
        private Button btnLogout;

        // Tarjetas
        private UITheme.CardPanel cardCategoria;
        private UITheme.CardPanel cardTipo;
        private UITheme.CardPanel cardValor;
        private UITheme.CardPanel cardResultado;

        // Controles de entrada
        private ComboBox cbCategoria;
        private ComboBox cbTipo;
        private UITheme.PlaceholderTextBox txtValor;
        private UITheme.GradientButton btnConvertir;

        // Resultado / mensajes
        private Label lblOutMain;
        private Label lblIn;
        private Label lblDetalle;
        private Label lblMensaje;

        // FAB limpiar
        private Button btnClearFab;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ===== Root =====
            root = new UITheme.GradientPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(14, 18, 18, 18),
            };
            this.Controls.Add(root);

            // ===== Ventana =====
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1180, 760);
            this.MinimumSize = new Size(1000, 640);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "ConUni • Conversor de unidades";
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.DoubleBuffered = true;

            // ===== Appbar =====
            appbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = Color.Transparent
            };

            lblTitulo = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Left,
                Width = 520,
                Text = "Conversiones de Unidades",
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 22f, FontStyle.Bold)
            };
            appbar.Controls.Add(lblTitulo);

            // sesión + logout (se agregan desde code-behind también)
            lblSesion = new Label
            {
                AutoSize = true,
                Text = "Conectado como …",
                ForeColor = Color.FromArgb(230, 236, 255),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(this.ClientSize.Width - 360, 22)
            };
            appbar.Controls.Add(lblSesion);

            btnLogout = new Button
            {
                Text = "CERRAR SESIÓN",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(224, 62, 62),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 34),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(this.ClientSize.Width - 190, 16),
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) => VolverAlLogin();
            appbar.Controls.Add(btnLogout);

            root.Controls.Add(appbar);

            // ===== Tarjetas (tres en fila) =====
            cardCategoria = new UITheme.CardPanel { Size = new Size(360, 210), Top = 108, Left = 28 };
            cardTipo = new UITheme.CardPanel { Size = new Size(360, 210), Top = 108, Left = 28 + 380 };
            cardValor = new UITheme.CardPanel { Size = new Size(360, 230), Top = 96, Left = 28 + 380 + 380 };

            // Contenido interno se agrega desde code-behind (para mantener lógica allí)
            root.Controls.Add(cardCategoria);
            root.Controls.Add(cardTipo);
            root.Controls.Add(cardValor);

            // ===== Resultado =====
            cardResultado = new UITheme.CardPanel
            {
                Top = 380, // bajamos un poco desde el inicio
                Left = (1180 - 480) / 2 - 18,
                Size = new Size(480, 220)
            };
            root.Controls.Add(cardResultado);

            // ===== FAB (papelera) =====
            btnClearFab = new Button
            {
                Width = 56,
                Height = 56,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                TabStop = false
            };
            btnClearFab.FlatAppearance.BorderSize = 0;
            btnClearFab.Cursor = Cursors.Hand;
            btnClearFab.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            root.Controls.Add(btnClearFab);

            // evento resize para reposicionar elementos dinámicos (FAB/appbar labels)
            this.Resize += (s, e) => OnMainResize();

            this.ResumeLayout(false);
        }
    }
}
