package ec.edu.monster.controller;

import ec.edu.monster.util.Constants;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import org.json.JSONObject;
import java.util.concurrent.TimeUnit;

public class RestClient {

    private static final OkHttpClient client = new OkHttpClient.Builder()
            .connectTimeout(30, TimeUnit.SECONDS)
            .readTimeout(30, TimeUnit.SECONDS)
            .build();

    /**
     * Realiza una llamada REST al servicio web de conversión de unidades
     * @param methodName Nombre del método REST a invocar
     * @param paramName Nombre del parámetro (no usado en REST, solo para compatibilidad)
     * @param value Valor a convertir
     * @return Resultado de la conversión
     * @throws Exception Si hay algún error en la comunicación
     */
    public static float callConversion(String methodName, String paramName, float value) throws Exception {
        // Mapear el nombre del método SOAP al endpoint REST
        String endpoint = mapMethodToEndpoint(methodName);
        
        // Construir la URL completa con el parámetro query
        String url = Constants.URL + "/" + endpoint + "?value=" + value;
        
        // Log para debug
        android.util.Log.d("RestClient", "URL: " + url);
        android.util.Log.d("RestClient", "Método original: " + methodName);
        android.util.Log.d("RestClient", "Endpoint mapeado: " + endpoint);
        
        // Construir el request HTTP GET
        Request request = new Request.Builder()
                .url(url)
                .addHeader("Accept", "application/json")
                .get()
                .build();
        
        // Ejecutar la petición
        try (Response response = client.newCall(request).execute()) {
            if (!response.isSuccessful()) {
                String errorMsg = "Error HTTP " + response.code();
                if (response.body() != null) {
                    errorMsg += ": " + response.body().string();
                }
                android.util.Log.e("RestClient", errorMsg);
                throw new Exception(errorMsg);
            }
            
            String responseBody = response.body().string();
            android.util.Log.d("RestClient", "Respuesta: " + responseBody);
            return parseJsonResponse(responseBody);
        } catch (Exception e) {
            android.util.Log.e("RestClient", "Error en callConversion: " + e.getMessage(), e);
            throw e;
        }
    }

    /**
     * Mapea los nombres de métodos SOAP a los endpoints REST
     * cm-to-in, in-to-cm, c-to-f, f-to-c, kg-to-lb, lb-to-kg
     */
    private static String mapMethodToEndpoint(String methodName) {
        // Convertir de formato SOAP (ej: "centimetrosAPulgadas" o "cmToIn") a formato REST (ej: "cm-to-in")
        String normalized = methodName.toLowerCase().replaceAll("[^a-z]", "");
        
        switch (normalized) {
            // Longitud
            case "cmtoin":
            case "centimetrosapulgadas":
                return "cm-to-in";
            case "intocm":
            case "pulgadasacentimetros":
                return "in-to-cm";
            
            // Temperatura
            case "ctof":
            case "celsiusafahrenheit":
                return "c-to-f";
            case "ftoc":
            case "fahrenheitacelsius":
                return "f-to-c";
            
            // Masa
            case "kgtolb":
            case "kilogramosalibras":
                return "kg-to-lb";
            case "lbtokg":
            case "librasakilogramos":
                return "lb-to-kg";
            
            default:
                // Si ya viene en formato REST, devolverlo tal cual
                return methodName;
        }
    }

    /**
     * Extrae el resultado numérico de la respuesta JSON
     * El servicio REST devuelve un JSON con estructura:
     * {"conversion":"...", "input":10.0, "inputUnit":"cm", "output":3.937008, "outputUnit":"in"}
     */
    private static float parseJsonResponse(String responseBody) throws Exception {
        try {
            android.util.Log.d("RestClient", "Parseando JSON: " + responseBody);
            
            JSONObject jsonResponse = new JSONObject(responseBody);
            
            // Extraer el valor del campo "output"
            if (jsonResponse.has("output")) {
                double output = jsonResponse.getDouble("output");
                android.util.Log.d("RestClient", "Valor extraído: " + output);
                return (float) output;
            }
            
            // Si no tiene "output", mostrar los campos disponibles
            android.util.Log.e("RestClient", "Campos disponibles: " + jsonResponse.toString());
            throw new Exception("La respuesta JSON no contiene el campo 'output'. JSON: " + responseBody);
        } catch (org.json.JSONException e) {
            android.util.Log.e("RestClient", "Error JSON: " + e.getMessage());
            throw new Exception("Error al parsear JSON: " + e.getMessage() + ". Respuesta: " + responseBody);
        }
    }
}
