# 🔧 Solución para el problema de CORS

## Problema Actual
El navegador está bloqueando las peticiones debido a CORS, aunque tienes el filtro configurado.

## ✅ Solución Completa

### 1. Actualiza el archivo `CORSFilter.java`

Reemplaza tu código actual con este:

```java
package ec.edu.monster.ws;

import jakarta.ws.rs.container.*;
import jakarta.ws.rs.core.Response;
import jakarta.ws.rs.ext.Provider;
import java.io.IOException;

@Provider
public class CORSFilter implements ContainerRequestFilter, ContainerResponseFilter {
    
    /**
     * Intercepta las peticiones OPTIONS (preflight)
     */
    @Override
    public void filter(ContainerRequestContext requestContext) throws IOException {
        // Si es una petición OPTIONS, responde inmediatamente
        if ("OPTIONS".equalsIgnoreCase(requestContext.getMethod())) {
            requestContext.abortWith(
                Response.ok()
                    .header("Access-Control-Allow-Origin", "*")
                    .header("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS, HEAD")
                    .header("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization, X-Requested-With")
                    .header("Access-Control-Max-Age", "3600")
                    .build()
            );
        }
    }
    
    /**
     * Agrega los headers CORS a todas las respuestas
     */
    @Override
    public void filter(ContainerRequestContext requestContext, 
                       ContainerResponseContext responseContext) throws IOException {
        responseContext.getHeaders().add("Access-Control-Allow-Origin", "*");
        responseContext.getHeaders().add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS, HEAD");
        responseContext.getHeaders().add("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization, X-Requested-With");
        responseContext.getHeaders().add("Access-Control-Max-Age", "3600");
    }
}
```

### 2. Verifica el archivo `ApplicationConfig.java`

Debe verse así:

```java
package ec.edu.monster.ws;

import java.util.Set;
import jakarta.ws.rs.ApplicationPath;
import jakarta.ws.rs.core.Application;

@ApplicationPath("webresources")
public class ApplicationConfig extends Application {
    @Override
    public Set<Class<?>> getClasses() {
        Set<Class<?>> resources = new java.util.HashSet<>();
        resources.add(ec.edu.monster.ws.ConUniResource.class);
        resources.add(ec.edu.monster.ws.CORSFilter.class);
        return resources;
    }
}
```

### 3. Pasos para aplicar los cambios:

1. **Detén el servidor** si está corriendo
2. **Actualiza el archivo** `CORSFilter.java` con el código de arriba
3. **Recompila el proyecto** (Clean and Build en NetBeans)
4. **Reinicia el servidor** de aplicaciones (GlassFish/Tomcat)
5. **Verifica que el servidor esté corriendo** en `http://localhost:8080`

### 4. Verifica la URL correcta

En tu archivo `api.js` (frontend), asegúrate que la URL sea correcta:

```javascript
const API_BASE_URL = 'http://localhost:8080/tu-app/webresources/ConUni';
```

Reemplaza `tu-app` con el nombre real de tu aplicación.

### 5. Prueba manualmente

Abre tu navegador y prueba estas URLs directamente:

- `http://localhost:8080/tu-app/webresources/ConUni/cm-to-in?value=10`
- `http://localhost:8080/tu-app/webresources/ConUni/c-to-f?value=25`

Si funcionan, el problema era solo el CORS.

---

## 🚀 Cambios Realizados en el Frontend

✅ Mejoré el manejo de errores en `ConversionCard.jsx` para mostrar mensajes claros
✅ Ahora te dirá específicamente si el servidor no está corriendo
✅ Los mensajes de error son más informativos

---

## 📋 Checklist Final

- [ ] Actualicé CORSFilter.java
- [ ] Recompilé el proyecto Java
- [ ] Reinicié el servidor de aplicaciones
- [ ] Verifiqué que el servidor esté en http://localhost:8080
- [ ] Probé las URLs directamente en el navegador
- [ ] La URL en api.js coincide con mi aplicación
- [ ] Recargué la página del frontend (Ctrl + F5)

---

**¡Ahora debería funcionar!** 🎉
