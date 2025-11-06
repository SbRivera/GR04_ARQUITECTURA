// _02.CLIMOV.Modelo/ConversionResult.cs
using System;

namespace _02.CLIMOV.Modelo
{
    public class ConversionResult
    {
        private float _valorOriginal;
        private float _valorConvertido;

        private static float Round2(float v) =>
            (float)Math.Round(v, 2, MidpointRounding.AwayFromZero);

        public float ValorOriginal
        {
            get => _valorOriginal;
            set => _valorOriginal = Round2(value);
        }

        public float ValorConvertido
        {
            get => _valorConvertido;
            set => _valorConvertido = Round2(value);
        }

        public string UnidadOrigen { get; set; }
        public string UnidadDestino { get; set; }
        public string TipoConversion { get; set; } // Longitud, Temperatura, Peso
        public DateTime FechaConversion { get; set; }
    }

    public enum TipoConversion { Longitud, Temperatura, Peso }

    public class UnidadConversion
    {
        public string Nombre { get; set; }
        public string Simbolo { get; set; }
        public TipoConversion Tipo { get; set; }
    }
}
