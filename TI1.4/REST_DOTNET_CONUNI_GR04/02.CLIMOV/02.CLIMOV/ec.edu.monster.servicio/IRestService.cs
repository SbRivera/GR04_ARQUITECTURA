using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.CLIMOV.Servicio
{
    public interface IRestService
    {
        // Conversiones de Longitud
        Task<double> CentimetrosAPulgadasAsync(double cm);
        Task<double> PulgadasACentimetrosAsync(double inch);

        // Conversiones de Temperatura
        Task<double> CelsiusAFahrenheitAsync(double c);
        Task<double> FahrenheitACelsiusAsync(double f);

        // Conversiones de Peso
        Task<double> KilogramosALibrasAsync(double kg);
        Task<double> LibrasAKilogramosAsync(double lb);

        Task<bool> ValidarConexionAsync();
    }
}
