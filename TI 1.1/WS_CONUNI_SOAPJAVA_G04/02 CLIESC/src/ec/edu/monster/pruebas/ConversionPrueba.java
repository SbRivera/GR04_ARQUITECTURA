package ec.edu.monster.pruebas;

import ec.edu.monster.controlador.ConversionControlador;
import ec.edu.monster.vista.VistaConversion;
import java.util.Scanner;

public class ConversionPrueba {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        // Implementación de VistaConversion para uso en consola
        VistaConversion vista = new VistaConversion() {
            @Override
            public String obtenerCentimetro() {
                System.out.print("Ingrese el valor en centímetros para convertir a pulgadas: ");
                return scanner.nextLine();
            }

            @Override
            public String obtenerPulgada() {
                System.out.print("Ingrese el valor en pulgadas para convertir a centímetros: ");
                return scanner.nextLine();
            }

            @Override
            public void setControlador(ConversionControlador controlador) {
                // No se requiere implementación para la consola
            }
        };

        // Instancia del controlador
        ConversionControlador controlador = new ConversionControlador(vista);

        // Opciones de menú para seleccionar la conversión
        boolean continuar = true;
        while (continuar) {
            System.out.println("\nSeleccione una opción:");
            System.out.println("1. Convertir centímetros a pulgadas");
            System.out.println("2. Convertir pulgadas a centímetros");
            System.out.println("0. Salir");

            System.out.print("Opción: ");
            String opcion = scanner.nextLine();

            switch (opcion) {
                case "1":
                    controlador.convertirCentimetrosAPulgadas();
                    break;
                case "2":
                    controlador.convertirPulgadasACentimetros();
                    break;
                case "0":
                    continuar = false;
                    System.out.println("Saliendo del programa...");
                    break;
                default:
                    System.out.println("Opción no válida. Intente nuevamente.");
            }
        }
        scanner.close();
    }
}
