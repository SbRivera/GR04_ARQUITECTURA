using System;
using System.Windows.Forms;

namespace CLIESC_CONVUNI_DOTNET_GR04.Views
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

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
                MessageBox.Show("Credenciales incorrectas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

    }
}
