package ec.edu.monster.controller;

import ec.edu.monster.util.Constants;
import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;
import java.util.concurrent.TimeUnit;

public class SoapClient {

    private static final MediaType SOAP_MEDIA_TYPE = MediaType.parse("text/xml; charset=utf-8");
    private static final OkHttpClient client = new OkHttpClient.Builder()
            .connectTimeout(30, TimeUnit.SECONDS)
            .readTimeout(30, TimeUnit.SECONDS)
            .build();

    /**
     * Realiza una llamada SOAP al servicio web .NET de conversión de unidades
     * @param methodName Nombre del método SOAP a invocar (con mayúscula inicial para .NET)
     * @param value Valor a convertir
     * @return Resultado de la conversión
     * @throws Exception Si hay algún error en la comunicación
     */
    public static float callConversion(String methodName, float value) throws Exception {
        // Obtener el nombre correcto del parámetro según el método
        String paramName = Constants.PARAM_NAMES.get(methodName);
        if (paramName == null) {
            throw new Exception("Método no reconocido: " + methodName);
        }
        
        // Construir el envelope SOAP manualmente para .NET
        String soapEnvelope = buildSoapEnvelope(methodName, paramName, value);
        
        // DEBUG: Imprimir el envelope que estamos enviando
        System.out.println("========== REQUEST SOAP ==========");
        System.out.println(soapEnvelope);
        System.out.println("==================================");
        
        // Crear el request body
        RequestBody body = RequestBody.create(soapEnvelope, SOAP_MEDIA_TYPE);
        
        // Construir el request HTTP
        // Nota: .NET requiere SOAPAction con el namespace completo
        String soapAction = Constants.SOAP_ACTION_NAMESPACE + methodName;
        
        Request request = new Request.Builder()
                .url(Constants.URL)
                .addHeader("Content-Type", "text/xml; charset=utf-8")
                .addHeader("SOAPAction", "\"" + soapAction + "\"")  // SOAPAction requerido por .NET
                .post(body)
                .build();
        
        // Ejecutar la petición
        try (Response response = client.newCall(request).execute()) {
            if (!response.isSuccessful()) {
                throw new Exception("Error en la respuesta del servidor: " + response.code());
            }
            
            String responseBody = response.body().string();
            
            // LOG: Imprimir respuesta completa para debug
            System.out.println("========== RESPUESTA SOAP ==========");
            System.out.println(responseBody);
            System.out.println("====================================");
            
            return parseResponse(responseBody, methodName);
        }
    }

    /**
     * Construye el envelope SOAP para la petición según el formato document/literal de .NET
     * El formato debe coincidir con el que espera el servicio WCF (.NET)
     */
    private static String buildSoapEnvelope(String methodName, String paramName, float value) {
        // .NET WCF usa el formato estándar SOAP 1.1 con namespace tempuri.org
        // Probando con parámetro calificado con namespace
        return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" " +
                "xmlns:tem=\"" + Constants.NAMESPACE + "\">" +
                "<soap:Header/>" +
                "<soap:Body>" +
                "<tem:" + methodName + ">" +
                "<tem:" + paramName + ">" + value + "</tem:" + paramName + ">" +
                "</tem:" + methodName + ">" +
                "</soap:Body>" +
                "</soap:Envelope>";
    }

    /**
     * Extrae el resultado numérico de la respuesta SOAP de .NET
     * El servicio WCF devuelve el resultado en un tag <{MethodName}Result> o <return>
     */
    private static float parseResponse(String responseBody, String methodName) throws Exception {
        try {
            System.out.println("========== PARSEANDO RESPUESTA ==========");
            System.out.println("Method name: " + methodName);
            
            // PRIMERO: Intentar con el tag genérico <return> (común en .NET WCF)
            String resultTag = "return";
            int startIndex = responseBody.indexOf("<" + resultTag + ">");
            int endIndex = responseBody.indexOf("</" + resultTag + ">");
            
            System.out.println("Buscando tag: <" + resultTag + ">");
            System.out.println("startIndex: " + startIndex + ", endIndex: " + endIndex);
            
            // Si no encuentra <return>, intentar con {MethodName}Result
            if (startIndex == -1 || endIndex == -1) {
                resultTag = methodName + "Result";
                startIndex = responseBody.indexOf("<" + resultTag + ">");
                endIndex = responseBody.indexOf("</" + resultTag + ">");
                System.out.println("Buscando tag: <" + resultTag + ">");
                System.out.println("startIndex: " + startIndex + ", endIndex: " + endIndex);
            }
            
            // Si aún no encuentra, intentar con diferentes prefijos de namespace
            if (startIndex == -1 || endIndex == -1) {
                String[] prefixes = {"a:", "ns:", "tem:", "s:"};
                for (String prefix : prefixes) {
                    // Probar con return
                    String tagWithPrefix = prefix + "return";
                    startIndex = responseBody.indexOf("<" + tagWithPrefix + ">");
                    if (startIndex != -1) {
                        endIndex = responseBody.indexOf("</" + tagWithPrefix + ">");
                        if (endIndex != -1) {
                            resultTag = tagWithPrefix;
                            System.out.println("✓ Encontrado tag con prefijo: <" + resultTag + ">");
                            break;
                        }
                    }
                    
                    // Probar con {MethodName}Result
                    tagWithPrefix = prefix + methodName + "Result";
                    startIndex = responseBody.indexOf("<" + tagWithPrefix + ">");
                    if (startIndex != -1) {
                        endIndex = responseBody.indexOf("</" + tagWithPrefix + ">");
                        if (endIndex != -1) {
                            resultTag = tagWithPrefix;
                            System.out.println("✓ Encontrado tag con prefijo: <" + resultTag + ">");
                            break;
                        }
                    }
                }
            }
            
            if (startIndex != -1 && endIndex != -1) {
                // Extraer el tag de apertura completo para calcular el inicio del valor
                String openingTag = responseBody.substring(startIndex, responseBody.indexOf(">", startIndex) + 1);
                int valueStart = startIndex + openingTag.length();
                String value = responseBody.substring(valueStart, endIndex).trim();
                
                System.out.println("✓ Valor extraído: '" + value + "'");
                System.out.println("=========================================");
                
                return Float.parseFloat(value);
            }
            
            System.out.println("❌ No se encontró ningún tag válido");
            System.out.println("=========================================");
            throw new Exception("No se pudo parsear la respuesta. Response: " + responseBody);
        } catch (NumberFormatException e) {
            throw new Exception("Error al convertir el resultado a número: " + e.getMessage());
        }
    }
}