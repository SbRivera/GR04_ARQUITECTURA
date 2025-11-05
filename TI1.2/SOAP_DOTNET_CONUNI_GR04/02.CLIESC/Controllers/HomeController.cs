using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;

namespace _02_CLIESC.Controllers
{
    public class HomeController
    {
        private static readonly HttpClient client = new HttpClient();
        private const string mainUrl = "http://localhost:9095/Conversion/";

        public bool ValidateLogin(string username, string password)
        {
            // Simula una validación de login (en un caso real, esto debería consultar una base de datos o un servicio)
            return username == "MONSTER" && password == "MONSTER9";
        }

        public async Task<string> ConvertToFahrenheitAsync(double celsius)
        {
            string url = mainUrl + $"CelsiusToFahrenheit/{celsius.ToString(CultureInfo.InvariantCulture)}";
            try
            {
                return await client.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public async Task<string> ConvertToCelsiusAsync(double fahrenheit)
        {
            string url = mainUrl + $"FahrenheitToCelsius/{fahrenheit.ToString(CultureInfo.InvariantCulture)}";
            try
            {
                return await client.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
