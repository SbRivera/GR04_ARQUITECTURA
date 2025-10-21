using System;
using System.Globalization;
using System.Threading.Tasks;
using WSConUniConsumer.Services;
using WSConUniServiceReference;

namespace WSConUniConsumer.Services
{
    public class WSConUniProxy : IDisposable
    {
        private readonly WSConUniClient _client;
        private static readonly string ResultFormat = "#,##0.####";

        public WSConUniProxy()
        {
            _client = new WSConUniClient();
        }

        public async Task<string> ConvertAsync(string tipo, float valor)
        {
            float resultado = tipo switch
            {
                "cmToIn" => await _client.CentimetrosAPulgadasAsync(valor),
                "inToCm" => await _client.PulgadasACentimetrosAsync(valor),
                "cToF" => await _client.CelsiusAFahrenheitAsync(valor),
                "fToC" => await _client.FahrenheitACelsiusAsync(valor),
                "kgToLb" => await _client.KilogramosALibrasAsync(valor),
                "lbToKg" => await _client.LibrasAKilogramosAsync(valor),
                _ => throw new InvalidOperationException("Operación inválida")
            };

            return resultado.ToString(ResultFormat, CultureInfo.InvariantCulture);
        }

        public void Dispose()
        {
            if (_client.State == System.ServiceModel.CommunicationState.Faulted)
            {
                _client.Abort();
            }
            else
            {
                _client.Close();
            }
        }
    }
}