// ec.edu.monster.controllers/ConversionController.cs
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CLIESC_CONVUNI_DOTNET_GR04.Models;

namespace CLIESC_CONVUNI_DOTNET_GR04.Controllers
{
    public sealed class ConversionController
    {
        private const string BASE_URL = "https://localhost:7118/ConUni/";
        private static readonly string[] CandidatePaths = { "ConUni" };

        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions SerOpts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public ConversionController()
        {
            var baseUri = BASE_URL.EndsWith("/") ? BASE_URL : BASE_URL + "/";
            _http = new HttpClient { BaseAddress = new Uri(baseUri) };
        }

        public async Task<ConversionResponse> ConvertAsync(ConversionRequest req, CancellationToken ct = default)
        {
            var payload = JsonSerializer.Serialize(req, SerOpts);
            using var content = new StringContent(payload, Encoding.UTF8, "application/json");

            using var resp = await _http.PostAsync(CandidatePaths[0], content, ct);
            var body = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
                return new ConversionResponse { Error = $"HTTP {(int)resp.StatusCode}: {body}" };

            var ok = JsonSerializer.Deserialize<ConversionResponse>(body, SerOpts)
                     ?? new ConversionResponse { Error = "Respuesta vacía del servidor." };
            ok.Raw = body;   // para depurar
            return ok;
        }
    }
}
