using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConUniClient
{
    internal class Program
    {
        // ======= Config del servicio .NET =======
        private const string URL = "https://localhost:7118/ConUni/ConUni"; // POST JSON

        // Login local quemado
        private const string USER = "MONSTER";
        private const string PASS = "MONSTER9";
        private const int MAX_TRIES = 3;

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        // Si el API está en https://localhost con cert de desarrollo
        private static readonly HttpClient HTTP = new(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (msg, cert, chain, err) => true
        })
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        // Para extraer número de "1.81 kg" si quieres solo el valor
        private static readonly Regex NUM_FROM_OUTPUT = new(
            @"([+-]?(?:\d+(?:\.\d*)?|\.\d+)(?:[eE][+-]?\d+)?)",
            RegexOptions.Compiled
        );

        static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("========================================");
            Console.WriteLine("  Cliente Consola — ConUni REST (.NET)");
            Console.WriteLine("========================================\n");

            if (!DoLogin())
            {
                Console.WriteLine("Demasiados intentos. Saliendo...");
                return;
            }
            Console.WriteLine("\nLogin correcto.\n");

            bool salir = false;
            while (!salir)
            {
                Menu();
                Console.Write("Elige opción: ");
                var op = Console.ReadLine()?.Trim();

                try
                {
                    switch (op)
                    {
                        case "1":
                            {
                                double v = LeerDouble("Centímetros: ");
                                string outStr = await CallAsync("cm_to_inch", v);
                                double r = ExtraerNumero(outStr);
                                Console.WriteLine($"{v:F6} cm → {r:F6} in\n");
                                break;
                            }
                        case "2":
                            {
                                double v = LeerDouble("Pulgadas: ");
                                string outStr = await CallAsync("inch_to_cm", v);
                                double r = ExtraerNumero(outStr);
                                Console.WriteLine($"{v:F6} in → {r:F6} cm\n");
                                break;
                            }
                        case "3":
                            {
                                double v = LeerDouble("°C: ");
                                string outStr = await CallAsync("c_to_f", v);
                                double r = ExtraerNumero(outStr);
                                Console.WriteLine($"{v:F6} °C → {r:F6} °F\n");
                                break;
                            }
                        case "4":
                            {
                                double v = LeerDouble("°F: ");
                                string outStr = await CallAsync("f_to_c", v);
                                double r = ExtraerNumero(outStr);
                                Console.WriteLine($"{v:F6} °F → {r:F6} °C\n");
                                break;
                            }
                        case "5":
                            {
                                double v = LeerDouble("Kilogramos: ");
                                string outStr = await CallAsync("kg_to_lb", v);
                                double r = ExtraerNumero(outStr);
                                Console.WriteLine($"{v:F6} kg → {r:F6} lb\n");
                                break;
                            }
                        case "6":
                            {
                                double v = LeerDouble("Libras: ");
                                string outStr = await CallAsync("lb_to_kg", v);
                                double r = ExtraerNumero(outStr);
                                Console.WriteLine($"{v:F6} lb → {r:F6} kg\n");
                                break;
                            }
                        case "0":
                            salir = true;
                            Console.WriteLine("¡Hasta luego!");
                            break;

                        default:
                            Console.WriteLine("Opción no válida.\n");
                            break;
                    }
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("⏱️  Timeout consultando el servicio.\n");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"🌐 Error HTTP: {ex.Message}\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}\n");
                }
            }
        }

        // ======= Llamada al servicio .NET (POST JSON) =======
        private static async Task<string> CallAsync(string type, double value)
        {
            var payload = new { type, value };
            var json = JsonSerializer.Serialize(payload, JsonOpts);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var res = await HTTP.PostAsync(URL, content);
            var body = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"HTTP {(int)res.StatusCode}: {body}");
            }

            // Respuesta ejemplo: { "type":"lb_to_kg","input":"4.00 lb","output":"1.81 kg" }
            using var doc = JsonDocument.Parse(body);
            if (!doc.RootElement.TryGetProperty("output", out var outputProp))
                throw new InvalidOperationException("No se encontró 'output' en la respuesta: " + body);

            return outputProp.GetString() ?? "";
        }

        // ======= Utilidades =======
        private static void Menu()
        {
            Console.WriteLine("--------- MENÚ ---------");
            Console.WriteLine("1) cm → in");
            Console.WriteLine("2) in → cm");
            Console.WriteLine("3) °C → °F");
            Console.WriteLine("4) °F → °C");
            Console.WriteLine("5) kg → lb");
            Console.WriteLine("6) lb → kg");
            Console.WriteLine("0) Salir");
            Console.WriteLine("------------------------");
        }

        private static bool DoLogin()
        {
            for (int i = 1; i <= MAX_TRIES; i++)
            {
                Console.Write("Usuario: ");
                string? u = Console.ReadLine()?.Trim();

                Console.Write("Contraseña: ");
                string? p = Console.ReadLine()?.Trim();

                if (u == USER && p == PASS)
                    return true;

                Console.WriteLine($"Credenciales inválidas ({i}/{MAX_TRIES})\n");
            }
            return false;
        }

        private static double LeerDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine()?.Trim();
                if (double.TryParse(s, System.Globalization.NumberStyles.Float,
                                    System.Globalization.CultureInfo.InvariantCulture, out double v))
                    return v;
                Console.WriteLine("Ingresa un número válido.\n");
            }
        }

        private static double ExtraerNumero(string outputConUnidades)
        {
            var m = NUM_FROM_OUTPUT.Match(outputConUnidades ?? string.Empty);
            return m.Success ? double.Parse(m.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture) : double.NaN;
        }
    }
}
