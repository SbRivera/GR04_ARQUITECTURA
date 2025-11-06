using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using _02.CLIMOV.Utils;

namespace _02.CLIMOV.Servicio
{
    public class RestService : IRestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public RestService()
        {
            // URL del servicio REST - Configurada en ApiConfig.cs
            _baseUrl = ApiConfig.BaseUrl;

            // Configurar HttpClientHandler para aceptar certificados SSL autofirmados en desarrollo
#if DEBUG
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => 
                {
                    System.Diagnostics.Debug.WriteLine($"[SSL] Validando certificado: {errors}");
                    return true; // Aceptar todos los certificados en DEBUG
                }
            };
            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(60) // Aumentado a 60 segundos
            };
#else
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(60)
            };
#endif

            System.Diagnostics.Debug.WriteLine($"[REST] RestService inicializado");
            System.Diagnostics.Debug.WriteLine($"[REST] URL configurada: {_baseUrl}");
        }

        // Método auxiliar para hacer peticiones POST al servicio REST
        private async Task<double> ConvertirAsync(string type, double value)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[REST] Iniciando conversión: type={type}, value={value}");
                System.Diagnostics.Debug.WriteLine($"[REST] URL: {_baseUrl}");

                // Crear el objeto de request
                var requestData = new
                {
                    type = type,
                    value = value
                };

                // Serializar a JSON
                var jsonContent = JsonSerializer.Serialize(requestData);
                System.Diagnostics.Debug.WriteLine($"[REST] JSON Request: {jsonContent}");
                
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Hacer la petición POST
                System.Diagnostics.Debug.WriteLine($"[REST] Enviando petición POST...");
                var response = await _httpClient.PostAsync(_baseUrl, content);
                System.Diagnostics.Debug.WriteLine($"[REST] Respuesta recibida: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"[REST] Error: {errorContent}");
                    throw new Exception($"Error en la petición REST: {response.StatusCode} - {errorContent}");
                }

                // Leer y deserializar la respuesta
                var jsonResponse = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[REST] JSON Response: {jsonResponse}");
                
                using (JsonDocument document = JsonDocument.Parse(jsonResponse))
                {
                    var root = document.RootElement;
                    
                    // La respuesta tiene el formato: {"type":"cm","input":"100,00 cm","output":"39,37 in"}
                    // Necesitamos extraer el valor numérico de "output"
                    if (root.TryGetProperty("output", out JsonElement outputElement))
                    {
                        string outputStr = outputElement.GetString();
                        System.Diagnostics.Debug.WriteLine($"[REST] Output string: {outputStr}");
                        
                        // Extraer el número del string (ej: "39,37 in" -> 39.37 o "39.37 in" -> 39.37)
                        string[] parts = outputStr.Split(' ');
                        if (parts.Length > 0)
                        {
                            // Reemplazar coma por punto para que funcione con ambos formatos
                            string numberStr = parts[0].Replace(',', '.');
                            
                            if (double.TryParse(numberStr, System.Globalization.NumberStyles.Any, 
                                System.Globalization.CultureInfo.InvariantCulture, out double result))
                            {
                                System.Diagnostics.Debug.WriteLine($"[REST] Resultado parseado: {result}");
                                return Math.Round(result, 2, MidpointRounding.AwayFromZero);
                            }
                        }
                    }
                }

                throw new Exception("No se pudo extraer el resultado de la respuesta");
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[REST] Error HTTP: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[REST] InnerException: {ex.InnerException?.Message}");
                throw new Exception($"Error de conexión: Verifica que el servidor esté corriendo en {_baseUrl}\n\nDetalle: {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[REST] Timeout: {ex.Message}");
                throw new Exception($"La petición excedió el tiempo de espera. Verifica que el servidor esté corriendo en {_baseUrl}", ex);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[REST] Error general: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[REST] StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        // Conversiones de Longitud
        public async Task<double> CentimetrosAPulgadasAsync(double cm)
        {
            return await ConvertirAsync("cm", cm);
        }

        public async Task<double> PulgadasACentimetrosAsync(double inch)
        {
            return await ConvertirAsync("inch", inch);
        }

        // Conversiones de Temperatura
        public async Task<double> CelsiusAFahrenheitAsync(double c)
        {
            return await ConvertirAsync("c", c);
        }

        public async Task<double> FahrenheitACelsiusAsync(double f)
        {
            return await ConvertirAsync("f", f);
        }

        // Conversiones de Peso
        public async Task<double> KilogramosALibrasAsync(double kg)
        {
            return await ConvertirAsync("kg", kg);
        }

        public async Task<double> LibrasAKilogramosAsync(double lb)
        {
            return await ConvertirAsync("lb", lb);
        }

        public async Task<bool> ValidarConexionAsync()
        {
            try
            {
                // Intenta hacer una conversión simple para validar la conexión
                await ConvertirAsync("cm", 1.0);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
