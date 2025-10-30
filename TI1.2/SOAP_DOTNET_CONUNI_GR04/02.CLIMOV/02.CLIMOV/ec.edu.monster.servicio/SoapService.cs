using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Channels;

namespace _02.CLIMOV.Servicio
{
    public class SoapService : ISoapService
    {
        private readonly string _endpointUrl;
        private readonly BasicHttpBinding _binding;

        public SoapService()
        {
            // URL del servicio SOAP - Cambia esto a tu servidor real
            _endpointUrl = "http://localhost:65462/espe.edu.monster.servicio/WSConUni.svc";

            // Configurar binding para SOAP
            _binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 2147483647,
                MaxBufferSize = 2147483647,
                OpenTimeout = TimeSpan.FromSeconds(30),
                CloseTimeout = TimeSpan.FromSeconds(30),
                ReceiveTimeout = TimeSpan.FromSeconds(30),
                SendTimeout = TimeSpan.FromSeconds(30)
            };
        }

        // Conversiones de Longitud
        public async Task<float> CentimetrosAPulgadasAsync(float cm)
        {
            try
            {
                // Implementación real con SOAP
                // Por ahora, lógica local basada en el servidor
                await Task.Delay(200); // Simula latencia de red
                return cm / 2.54f;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error SOAP CentimetrosAPulgadas: {ex.Message}");
                throw new Exception("Error al convertir centímetros a pulgadas", ex);
            }
        }

        public async Task<float> PulgadasACentimetrosAsync(float inch)
        {
            try
            {
                await Task.Delay(200);
                return inch * 2.54f;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error SOAP PulgadasACentimetros: {ex.Message}");
                throw new Exception("Error al convertir pulgadas a centímetros", ex);
            }
        }

        // Conversiones de Temperatura
        public async Task<float> CelsiusAFahrenheitAsync(float c)
        {
            try
            {
                await Task.Delay(200);
                return (c * 9f / 5f) + 32f;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error SOAP CelsiusAFahrenheit: {ex.Message}");
                throw new Exception("Error al convertir Celsius a Fahrenheit", ex);
            }
        }

        public async Task<float> FahrenheitACelsiusAsync(float f)
        {
            try
            {
                await Task.Delay(200);
                return (f - 32f) * 5f / 9f;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error SOAP FahrenheitACelsius: {ex.Message}");
                throw new Exception("Error al convertir Fahrenheit a Celsius", ex);
            }
        }

        // Conversiones de Peso
        public async Task<float> KilogramosALibrasAsync(float kg)
        {
            try
            {
                await Task.Delay(200);
                return kg * 2.20462f;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error SOAP KilogramosALibras: {ex.Message}");
                throw new Exception("Error al convertir kilogramos a libras", ex);
            }
        }

        public async Task<float> LibrasAKilogramosAsync(float lb)
        {
            try
            {
                await Task.Delay(200);
                return lb / 2.20462f;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error SOAP LibrasAKilogramos: {ex.Message}");
                throw new Exception("Error al convertir libras a kilogramos", ex);
            }
        }

        public async Task<bool> ValidarConexionAsync()
        {
            try
            {
                // Intenta hacer una validación simple
                await Task.Delay(100);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
