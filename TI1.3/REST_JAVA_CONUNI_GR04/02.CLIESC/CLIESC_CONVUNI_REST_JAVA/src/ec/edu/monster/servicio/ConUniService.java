package ec.edu.monster.servicio;

import ec.edu.monster.modelo.ConversionResult;
import java.io.IOException;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.nio.charset.StandardCharsets;
import java.text.DecimalFormat;
import java.util.Locale;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class ConUniService {

    private static final String BASE_URL =
            "http://localhost:8080/WS_ConUni_REST_JAVA_GR04/webresources/ConUni";

    private final HttpClient httpClient;
    private final DecimalFormat decimalFormat;

    public ConUniService() {
        this.httpClient = HttpClient.newHttpClient();
        this.decimalFormat = (DecimalFormat) DecimalFormat.getNumberInstance(Locale.US);
        this.decimalFormat.applyPattern("#.######");
    }

    public ConversionResult convert(String path, double value) throws IOException, InterruptedException {
        String url = String.format(Locale.US, "%s/%s?value=%s",
                BASE_URL, path, decimalFormat.format(value));

        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(url))
                .header("Accept", "application/json")
                .GET()
                .build();

        HttpResponse<String> response =
                httpClient.send(request, HttpResponse.BodyHandlers.ofString(StandardCharsets.UTF_8));

        if (response.statusCode() >= 200 && response.statusCode() < 300) {
            return parseConversion(response.body());
        }
        throw new IOException("Error HTTP " + response.statusCode() + ": " + response.body());
    }

    private ConversionResult parseConversion(String json) throws IOException {
        ConversionResult result = new ConversionResult();
        result.conversion = extractString(json, "conversion");
        result.input = extractDouble(json, "input");
        result.inputUnit = extractString(json, "inputUnit");
        result.output = extractDouble(json, "output");
        result.outputUnit = extractString(json, "outputUnit");
        return result;
    }

    private String extractString(String json, String key) throws IOException {
        Pattern pattern = Pattern.compile("\"" + key + "\"\\s*:\\s*\"([^\"]*)\"");
        Matcher matcher = pattern.matcher(json);
        if (matcher.find()) {
            return matcher.group(1);
        }
        throw new IOException("Campo \"" + key + "\" no encontrado en: " + json);
    }

    private double extractDouble(String json, String key) throws IOException {
        Pattern pattern = Pattern.compile("\"" + key + "\"\\s*:\\s*(-?[0-9]+(?:\\.[0-9]+)?)");
        Matcher matcher = pattern.matcher(json);
        if (matcher.find()) {
            return Double.parseDouble(matcher.group(1));
        }
        throw new IOException("Campo numérico \"" + key + "\" no encontrado en: " + json);
    }
}