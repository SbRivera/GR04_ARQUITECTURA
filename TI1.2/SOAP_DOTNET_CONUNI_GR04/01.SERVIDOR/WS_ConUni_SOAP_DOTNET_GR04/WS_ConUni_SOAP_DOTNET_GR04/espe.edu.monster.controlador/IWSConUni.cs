using System.ServiceModel;

namespace espe.edu.monster.controlador
{
    [ServiceContract]
    public interface IWSConUni
    {
        [OperationContract] float CentimetrosAPulgadas(float cm);
        [OperationContract] float PulgadasACentimetros(float inch);
        [OperationContract] float CelsiusAFahrenheit(float c);
        [OperationContract] float FahrenheitACelsius(float f);
        [OperationContract] float KilogramosALibras(float kg);
        [OperationContract] float LibrasAKilogramos(float lb);
    }
}
