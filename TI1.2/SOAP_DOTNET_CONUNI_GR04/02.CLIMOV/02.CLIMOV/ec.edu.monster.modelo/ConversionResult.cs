using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.CLIMOV.Modelo
{
    public class ConversionResult
    {
        public float ValorOriginal { get; set; }
        public float ValorConvertido { get; set; }
        public string UnidadOrigen { get; set; }
        public string UnidadDestino { get; set; }
        public string TipoConversion { get; set; } // Longitud, Temperatura, Peso
        public DateTime FechaConversion { get; set; }
    }

    public enum TipoConversion
    {
        Longitud,
        Temperatura,
        Peso
    }

    public class UnidadConversion
    {
        public string Nombre { get; set; }
        public string Simbolo { get; set; }
        public TipoConversion Tipo { get; set; }
    }
}
