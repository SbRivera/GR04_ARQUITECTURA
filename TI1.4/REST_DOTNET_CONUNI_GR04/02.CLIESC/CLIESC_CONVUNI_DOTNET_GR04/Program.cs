using System;
using System.Windows.Forms;
using CLIESC_CONVUNI_DOTNET_GR04.Views;

namespace CLIESC_CONVUNI_DOTNET_GR04
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
