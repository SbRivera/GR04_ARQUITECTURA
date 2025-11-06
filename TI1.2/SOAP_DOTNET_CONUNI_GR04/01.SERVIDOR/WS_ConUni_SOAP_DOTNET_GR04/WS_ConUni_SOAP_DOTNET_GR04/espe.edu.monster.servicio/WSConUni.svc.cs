using System;
using System.ServiceModel;
using espe.edu.monster.controlador;

namespace espe.edu.monster.servicio
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class WSConUni : IWSConUni
    {
        private float Round2(float value)
        {
            // Redondea a 2 decimales exactos
            return (float)Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public float CentimetrosAPulgadas(float cm)
        {
            return Round2(cm / 2.54f);
        }

        public float PulgadasACentimetros(float inch)
        {
            return Round2(inch * 2.54f);
        }

        public float CelsiusAFahrenheit(float c)
        {
            return Round2((c * 9f / 5f) + 32f);
        }

        public float FahrenheitACelsius(float f)
        {
            return Round2((f - 32f) * 5f / 9f);
        }

        public float KilogramosALibras(float kg)
        {
            return Round2(kg * 2.20462f);
        }

        public float LibrasAKilogramos(float lb)
        {
            return Round2(lb / 2.20462f);
        }
    }
}
