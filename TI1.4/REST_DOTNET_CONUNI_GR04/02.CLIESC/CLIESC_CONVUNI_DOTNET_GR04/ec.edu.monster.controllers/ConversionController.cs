using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CLIESC_CONVUNI_DOTNET_GR04.Models;

namespace CLIESC_CONVUNI_DOTNET_GR04.Controllers
{
    public class ConversionController
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7118/ConUni/ConUni"; // Usa el puerto real de tu servidor REST

        public ConversionController()
        {
            // Permite certificados HTTPS locales (solo para desarrollo)
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            _httpClient = new HttpClient(handler);
        }

        public async Task<ConversionResponse> ConvertAsync(ConversionRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(_baseUrl, content);
                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new ConversionResponse
                    {
                        Error = $"Error del servidor ({response.StatusCode}): {responseText}"
                    };
                }

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<ConversionResponse>(responseText, options);
            }
            catch (HttpRequestException ex)
            {
                // Captura cuando el servidor no responde o el puerto es incorrecto
                return new ConversionResponse
                {
                    Error = $"No se pudo conectar al servidor: {ex.Message}"
                };
            }
        }
    }
}
