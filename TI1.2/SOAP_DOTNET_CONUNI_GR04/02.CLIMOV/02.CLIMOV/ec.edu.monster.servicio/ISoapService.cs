using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.CLIMOV.Servicio
{
    public interface ISoapService
    {
        // Conversiones de Longitud
        Task<float> CentimetrosAPulgadasAsync(float cm);
        Task<float> PulgadasACentimetrosAsync(float inch);

        // Conversiones de Temperatura
        Task<float> CelsiusAFahrenheitAsync(float c);
        Task<float> FahrenheitACelsiusAsync(float f);

        // Conversiones de Peso
        Task<float> KilogramosALibrasAsync(float kg);
        Task<float> LibrasAKilogramosAsync(float lb);

        Task<bool> ValidarConexionAsync();
    }
}
