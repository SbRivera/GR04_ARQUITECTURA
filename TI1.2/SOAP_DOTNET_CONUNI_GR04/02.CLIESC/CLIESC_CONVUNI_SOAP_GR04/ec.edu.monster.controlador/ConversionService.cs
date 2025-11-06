using System.Threading.Tasks;
// Usa el namespace que configuraste al agregar el Connected Service:
using ServiceReference1;  // <--- cámbialo si elegiste otro

namespace ec.edu.monster.controlador
{
    public class ConversionService
    {
        // Nota: el nombre del cliente y del EndpointConfiguration
        // puede cambiar según tu WSDL. Si difiere, ajústalo aquí.
        private readonly WSConUniClient _client =
            new WSConUniClient(WSConUniClient.EndpointConfiguration.BasicHttpBinding_IWSConUni);

        public Task<float> CmAPulgadas(float v) => _client.CentimetrosAPulgadasAsync(v);
        public Task<float> PulgadasACm(float v) => _client.PulgadasACentimetrosAsync(v);
        public Task<float> CelsiusAFahrenheit(float v) => _client.CelsiusAFahrenheitAsync(v);
        public Task<float> FahrenheitACelsius(float v) => _client.FahrenheitACelsiusAsync(v);
        public Task<float> KgALibras(float v) => _client.KilogramosALibrasAsync(v);
        public Task<float> LibrasAKg(float v) => _client.LibrasAKilogramosAsync(v);
    }
}
