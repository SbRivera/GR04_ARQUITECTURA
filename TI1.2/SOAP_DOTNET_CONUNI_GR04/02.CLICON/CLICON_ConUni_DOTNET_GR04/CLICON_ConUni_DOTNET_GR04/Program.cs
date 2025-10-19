using System;
using System.ServiceModel;
using CLICON_ConUni_DOTNET_GR04.ConUniRef; // <== tu namespace del Service Reference

namespace CLICON_ConUni_SOAP_DOTNET_GR04
{
    class Program
    {
        // ===== Config “quemada” del login (solo cliente) =====
        private const string USERNAME = "MONSTER";
        private const string PASSWORD = "MONSTER9";

        // Opcional: si quieres sobreescribir la URL del endpoint en runtime
        private const string SERVICE_URL = "http://localhost:65462/espe.edu.monster.servicio/WSConUni.svc";

        static void Main(string[] args)
        {
            Console.Title = "Cliente SOAP .NET - ConUni";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1) Login local (no llama al servidor)
            if (!LoginLocal())
            {
                Console.WriteLine("Se agotaron los intentos. Saliendo...");
                return;
            }
            Console.WriteLine("✅ Login correcto.\n");

            // 2) Crear cliente WCF (proxy)
            var client = new WSConUniClient();

            // (Opcional) forzar URL si difiere de la registrada en la referencia
            client.Endpoint.Address = new EndpointAddress(SERVICE_URL);

            // (Opcional) timeouts de operación
            client.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(15);

            // 3) Menú principal
            while (true)
            {
                Console.WriteLine("=== CLIENTE SOAP CONUNI ===");
                Console.WriteLine("1) Centímetros → Pulgadas");
                Console.WriteLine("2) Pulgadas → Centímetros");
                Console.WriteLine("3) Celsius → Fahrenheit");
                Console.WriteLine("4) Fahrenheit → Celsius");
                Console.WriteLine("5) Kilogramos → Libras");
                Console.WriteLine("6) Libras → Kilogramos");
                Console.WriteLine("0) Salir");
                Console.Write("Opción: ");
                string op = Console.ReadLine();
                if (op == "0") break;

                try
                {
                    switch (op)
                    {
                        case "1":
                            Console.Write("Centímetros: ");
                            float cm = float.Parse(Console.ReadLine());
                            Console.WriteLine($"{cm} cm = {client.CentimetrosAPulgadas(cm)} in\n");
                            break;

                        case "2":
                            Console.Write("Pulgadas: ");
                            float inch = float.Parse(Console.ReadLine());
                            Console.WriteLine($"{inch} in = {client.PulgadasACentimetros(inch)} cm\n");
                            break;

                        case "3":
                            Console.Write("Celsius: ");
                            float c = float.Parse(Console.ReadLine());
                            Console.WriteLine($"{c} °C = {client.CelsiusAFahrenheit(c)} °F\n");
                            break;

                        case "4":
                            Console.Write("Fahrenheit: ");
                            float f = float.Parse(Console.ReadLine());
                            Console.WriteLine($"{f} °F = {client.FahrenheitACelsius(f)} °C\n");
                            break;

                        case "5":
                            Console.Write("Kilogramos: ");
                            float kg = float.Parse(Console.ReadLine());
                            Console.WriteLine($"{kg} kg = {client.KilogramosALibras(kg)} lb\n");
                            break;

                        case "6":
                            Console.Write("Libras: ");
                            float lb = float.Parse(Console.ReadLine());
                            Console.WriteLine($"{lb} lb = {client.LibrasAKilogramos(lb)} kg\n");
                            break;

                        default:
                            Console.WriteLine("Opción no válida.\n");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Error al consumir el servicio: " + ex.Message + "\n");
                }
            }

            try { client.Close(); } catch { client.Abort(); }
        }

        // ===== Utilidades =====
        private static bool LoginLocal()
        {
            const int MAX = 3;
            for (int i = 1; i <= MAX; i++)
            {
                Console.Write("Usuario: ");
                string u = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Contraseña: ");
                string p = ReadPassword(); // sin eco
                Console.WriteLine();

                if (u.Equals(USERNAME, StringComparison.OrdinalIgnoreCase) && p == PASSWORD)
                    return true;

                Console.WriteLine($"Credenciales inválidas (intento {i}/{MAX}).\n");
            }
            return false;
        }

        // Lee contraseña ocultando caracteres
        private static string ReadPassword()
        {
            var pass = string.Empty;
            ConsoleKeyInfo k;
            while ((k = Console.ReadKey(intercept: true)).Key != ConsoleKey.Enter)
            {
                if (k.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass = pass.Substring(0, pass.Length - 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(k.KeyChar))
                {
                    pass += k.KeyChar;
                    Console.Write("*");
                }
            }
            return pass;
        }
    }
}
