using System;
using System.Windows.Forms;
using ec.edu.monster.vista;

namespace CLIESC_CONVUNI_SOAP_GR04
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new LoginForm());
        }
    }
}
