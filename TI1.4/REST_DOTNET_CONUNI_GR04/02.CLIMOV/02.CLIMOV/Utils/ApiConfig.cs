using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.CLIMOV.Utils
{
    public static class ApiConfig
    {
        // ⚠️ CONFIGURACIÓN: Cambia estas constantes según tu escenario
        private const bool USE_EMULATOR = false;  // true = emulador Android, false = dispositivo físico
        private const string YOUR_PC_IP = "192.168.100.17";  // Tu IP de Windows (ipconfig)
        private const string PORT = "7118";
        
        // ⚠️ IMPORTANTE: Esta URL se ajusta automáticamente según la plataforma
        // El servidor REST debe estar corriendo en https://localhost:7118
        public static string BaseUrl
        {
            get
            {
#if ANDROID
                // En Android, "localhost" no funciona
                if (USE_EMULATOR)
                {
                    // Para emulador Android: usa 10.0.2.2 (dirección especial para localhost de Windows)
                    return $"https://10.0.2.2:{PORT}/ConUni/ConUni";
                }
                else
                {
                    // Para dispositivo físico Android: usa la IP de tu PC
                    return $"https://{YOUR_PC_IP}:{PORT}/ConUni/ConUni";
                }
#elif IOS
                // iOS Simulator puede usar localhost
                return $"https://localhost:{PORT}/ConUni/ConUni";
#elif WINDOWS
                // Windows usa localhost
                return $"https://localhost:{PORT}/ConUni/ConUni";
#elif MACCATALYST
                // MacCatalyst usa localhost
                return $"https://localhost:{PORT}/ConUni/ConUni";
#else
                return $"https://localhost:{PORT}/ConUni/ConUni";
#endif
            }
        }

        // Nota: Si sigues teniendo problemas de timeout:
        // 1. Verifica que el servidor esté corriendo (https://localhost:7118/swagger)
        // 2. Verifica el firewall de Windows
        // 3. Si usas dispositivo físico, asegúrate de estar en la misma red WiFi
        // 4. Considera usar HTTP en lugar de HTTPS para desarrollo (más fácil)
    }
}
