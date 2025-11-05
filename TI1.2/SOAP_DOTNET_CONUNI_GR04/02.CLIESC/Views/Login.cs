using _02_CLIESC.Controllers;
using _02_CLIESC.Views;
using System;
using System.Windows.Forms;

namespace _02_CLIESC
{
    public partial class Login : Form
    {
        private HomeController controller = new HomeController();

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            if (controller.ValidateLogin(username, password))
            {
                MessageBox.Show("Login Successful");
                new Index(controller).Show();  // Pasamos el controlador al formulario Index
                this.Hide();
            }
            else
            {
                MessageBox.Show("Incorrect username or password");
            }
        }

    }
}
