using _02.CLIWEB.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _02.CLIWEB.Services
{
    public class ConUniService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://localhost:7118/ConUni/ConUni";

        public ConUniService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Resultado> ConvertirAsync(string tipo, double valor)
        {
            var requestBody = new
            {
                type = tipo,
                value = valor
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(BASE_URL, content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var resultado = JsonSerializer.Deserialize<Resultado>(responseString, options);
            return resultado ?? new Resultado();
        }
    }
}
