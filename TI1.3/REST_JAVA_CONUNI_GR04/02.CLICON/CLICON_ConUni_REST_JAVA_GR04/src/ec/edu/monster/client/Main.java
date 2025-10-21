package ec.edu.monster.client;

import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.nio.charset.StandardCharsets;
import java.time.Duration;
import java.util.Scanner;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Main {

    // Base del servicio (ajústala si cambias ApplicationPath)
    private static final String BASE =
        "http://localhost:8080/WS_ConUni_REST_JAVA_GR04/webresources/ConUni";

    // “Login” local quemado (solo para entrar al menú)
    private static final String USER = "MONSTER";
    private static final String PASS = "MONSTER9";

    private static final HttpClient HTTP = HttpClient.newBuilder()
            .connectTimeout(Duration.ofSeconds(5))
            .build();
    private static final Pattern OUTPUT_JSON = Pattern.compile(
            "\"output\"\\s*:\\s*([+-]?(?:\\d+\\.?\\d*|\\.\\d+)(?:[eE][+-]?\\d+)?)");

    public static void main(String[] args) {
        try { System.setOut(new java.io.PrintStream(System.out, true, "UTF-8")); } catch (Exception ignored) {}
        Scanner sc = new Scanner(System.in, StandardCharsets.UTF_8);

        System.out.println("========================================");
        System.out.println("  Cliente Consola — ConUni REST (Ant)");
        System.out.println("========================================\n");

        if (!login(sc)) {
            System.out.println("Demasiados intentos. Saliendo...");
            return;
        }

        boolean salir = false;
        while (!salir) {
            menu();
            System.out.print("Elige opción: ");
            String op = sc.nextLine().trim();

            try {
                switch (op) {
                    case "1": {
                        double v = leerDouble(sc, "Centímetros: ");
                        double r = call("/cm-to-in", v);
                        System.out.printf("%.6f cm → %.6f in%n%n", v, r);
                        break;
                    }
                    case "2": {
                        double v = leerDouble(sc, "Pulgadas: ");
                        double r = call("/in-to-cm", v);
                        System.out.printf("%.6f in → %.6f cm%n%n", v, r);
                        break;
                    }
                    case "3": {
                        double v = leerDouble(sc, "°C: ");
                        double r = call("/c-to-f", v);
                        System.out.printf("%.6f °C → %.6f °F%n%n", v, r);
                        break;
                    }
                    case "4": {
                        double v = leerDouble(sc, "°F: ");
                        double r = call("/f-to-c", v);
                        System.out.printf("%.6f °F → %.6f °C%n%n", v, r);
                        break;
                    }
                    case "5": {
                        double v = leerDouble(sc, "Kilogramos: ");
                        double r = call("/kg-to-lb", v);
                        System.out.printf("%.6f kg → %.6f lb%n%n", v, r);
                        break;
                    }
                    case "6": {
                        double v = leerDouble(sc, "Libras: ");
                        double r = call("/lb-to-kg", v);
                        System.out.printf("%.6f lb → %.6f kg%n%n", v, r);
                        break;
                    }
                    case "0":
                        salir = true;
                        System.out.println("¡Hasta luego!");
                        break;
                    default:
                        System.out.println("Opción no válida.\n");
                }
            } catch (Exception ex) {
                System.out.println("❌ Error: " + ex.getMessage() + "\n");
            }
        }
    }

    private static void menu() {
        System.out.println("--------- MENÚ ---------");
        System.out.println("1) cm → in");
        System.out.println("2) in → cm");
        System.out.println("3) °C → °F");
        System.out.println("4) °F → °C");
        System.out.println("5) kg → lb");
        System.out.println("6) lb → kg");
        System.out.println("0) Salir");
        System.out.println("------------------------");
    }

    private static boolean login(Scanner sc) {
        final int MAX = 3;
        for (int i = 1; i <= MAX; i++) {
            System.out.print("Usuario: ");
            String u = sc.nextLine().trim();
            System.out.print("Contraseña: ");
            String p = sc.nextLine().trim();
            if (USER.equals(u) && PASS.equals(p)) {
                System.out.println("\nLogin correcto.\n");
                return true;
            }
            System.out.printf("Credenciales inválidas (%d/%d)%n%n", i, MAX);
        }
        return false;
    }

    private static double leerDouble(Scanner sc, String prompt) {
        while (true) {
            System.out.print(prompt);
            try { return Double.parseDouble(sc.nextLine().trim()); }
            catch (NumberFormatException e) { System.out.println("Ingresa un número válido.\n"); }
        }
    }

    private static double call(String path, double value) throws Exception {
    URI uri = URI.create(BASE + path + "?value=" + value);
    HttpRequest req = HttpRequest.newBuilder(uri)
            .timeout(Duration.ofSeconds(10))
            .header("Accept", "application/json")
            .GET()
            .build();

    HttpResponse<String> res = HTTP.send(req, HttpResponse.BodyHandlers.ofString(StandardCharsets.UTF_8));

    if (res.statusCode() != 200) {
        throw new RuntimeException("HTTP " + res.statusCode() + ": " + res.body());
    }

    // res.body() es JSON, ej.: {"conversion":"...","output":123.45,...}
    String body = res.body();
    Matcher m = OUTPUT_JSON.matcher(body);
    if (!m.find()) {
        throw new RuntimeException("No se encontró el campo 'output' en: " + body);
    }
    return Double.parseDouble(m.group(1));
}
}
