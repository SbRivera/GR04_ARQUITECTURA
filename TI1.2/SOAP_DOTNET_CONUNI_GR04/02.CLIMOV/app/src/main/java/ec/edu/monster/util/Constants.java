package ec.edu.monster.util;

import java.util.HashMap;
import java.util.Map;

public class Constants {
    // Namespace del servicio SOAP .NET (según WSDL)
    public static final String NAMESPACE = "http://tempuri.org/";
    
    // Namespace para SOAPAction (IWSConUni es la interfaz del servicio)
    public static final String SOAP_ACTION_NAMESPACE = "http://tempuri.org/IWSConUni/";
    
    // URL del servicio SOAP .NET
    // Para emulador usar: http://10.0.2.2:65462/espe.edu.monster.servicio/WSConUni.svc
    // Para dispositivo físico usar la IP de tu PC donde corre el servidor .NET
    // Ejemplo: "http://192.168.100.17:65462/espe.edu.monster.servicio/WSConUni.svc"
    // Para encontrar tu IP en Windows: ipconfig en CMD y busca "Dirección IPv4"
    public static final String URL = "http://localhost:65462/espe.edu.monster.servicio/WSConUni.svc";
    
    // Mapa de nombres de parámetros según el XSD del servicio .NET
    // Formato: nombreMetodo -> nombreParametro
    public static final Map<String, String> PARAM_NAMES = new HashMap<String, String>() {{
        put("CentimetrosAPulgadas", "cm");
        put("PulgadasACentimetros", "inch");
        put("CelsiusAFahrenheit", "c");
        put("FahrenheitACelsius", "f");
        put("KilogramosALibras", "kg");
        put("LibrasAKilogramos", "lb");
    }};
}

