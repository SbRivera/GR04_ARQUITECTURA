package ec.edu.monster.util;

public class Constants {
    // NAMESPACE ya no se usa en REST, pero lo mantenemos para compatibilidad
    public static final String NAMESPACE = "http://ws.monster.edu.ec/";
    
    // ⚠️ CONFIGURACIÓN IMPORTANTE:
    // Para EMULADOR usar: 10.0.2.2 (apunta a localhost de tu PC)
    // Para DISPOSITIVO FÍSICO usar la IP de tu PC (Ej: 192.168.100.17)
    // Para encontrar tu IP en Windows: ejecuta "ipconfig" en CMD y busca "Dirección IPv4"
    
    // 🔧 Cambia "10.0.2.2" por tu IP real si usas dispositivo físico
    public static final String URL = "http://192.168.100.17:8080/WS_ConUni_REST_JAVA_GR04/webresources/ConUni";
    
    // Si usas dispositivo físico, descomenta esta línea y comenta la de arriba:
    // public static final String URL = "http://192.168.100.17:8080/WS_ConUni_REST_JAVA_GR04/webresources/ConUni";
}
