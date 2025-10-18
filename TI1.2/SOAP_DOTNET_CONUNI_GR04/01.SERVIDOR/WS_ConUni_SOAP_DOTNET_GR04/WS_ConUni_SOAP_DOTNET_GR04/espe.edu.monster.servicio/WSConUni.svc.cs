using System.ServiceModel;
using espe.edu.monster.controlador;

namespace espe.edu.monster.servicio
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class WSConUni : IWSConUni
    {
        public float CentimetrosAPulgadas(float cm) => cm / 2.54f;
        public float PulgadasACentimetros(float inch) => inch * 2.54f;

        public float CelsiusAFahrenheit(float c) => (c * 9f / 5f) + 32f;
        public float FahrenheitACelsius(float f) => (f - 32f) * 5f / 9f;

        public float KilogramosALibras(float kg) => kg * 2.20462f;
        public float LibrasAKilogramos(float lb) => lb / 2.20462f;
    }
}
