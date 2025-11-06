using System;
using System.Threading.Tasks;
using ec.edu.monster.modelo;

namespace ec.edu.monster.controlador
{
    public class ConversionController
    {
        private readonly ConversionService _svc = new();

        public async Task<float> ConvertirAsync(ConversionType tipo, float v)
        {
            float resultado = tipo switch
            {
                ConversionType.CM_A_IN => await _svc.CmAPulgadas(v),
                ConversionType.IN_A_CM => await _svc.PulgadasACm(v),
                ConversionType.C_A_F => await _svc.CelsiusAFahrenheit(v),
                ConversionType.F_A_C => await _svc.FahrenheitACelsius(v),
                ConversionType.KG_A_LB => await _svc.KgALibras(v),
                ConversionType.LB_A_KG => await _svc.LibrasAKg(v),
                _ => throw new NotSupportedException("Tipo no soportado")
            };

            // 🔹 Siempre redondea a 2 decimales antes de retornar
            return (float)Math.Round(resultado, 2, MidpointRounding.AwayFromZero);
        }
    }
}
