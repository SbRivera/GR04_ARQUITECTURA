using _02_CLIESC.Controllers;
using System;
using System.Windows.Forms;
using System.Globalization;

namespace _02_CLIESC.Views
{
    internal partial class Index : Form
    {
        private HomeController controller;

        public Index(HomeController controller)
        {
            this.controller = controller;
            InitializeComponent();
        }

        private async void btnConvertToFahrenheit_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtCelsius.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double celsius))
            {
                string result = await controller.ConvertToFahrenheitAsync(celsius);
                lblResult.Text = "El Resultado es: " + result + " °F";
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        private async void btnConvertToCelsius_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtFahrenheit.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double fahrenheit))
            {
                string result = await controller.ConvertToCelsiusAsync(fahrenheit);
                lblResult.Text = "El Resultado es: " + result + " °C";
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Login loginForm = new Login();
            loginForm.Show();
        }
    }
}
