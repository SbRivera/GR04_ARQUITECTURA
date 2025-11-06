/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Main.java to edit this template
 */
package cliesc_convuni_rest_java;

import ec.edu.monster.controlador.LoginController;
import ec.edu.monster.servicio.ConUniService;
import ec.edu.monster.vista.LoginView;
import ec.edu.monster.vista.UITheme;
import javax.swing.SwingUtilities;

public class CLIESC_CONVUNI_REST_JAVA {
    public static void main(String[] args) {
        UITheme.applyGlobalTheme();

        SwingUtilities.invokeLater(() -> {
            ConUniService service = new ConUniService();
            LoginView loginView = new LoginView();

            // --- Avatar desde tu ruta absoluta (usar barras dobles \\) ---
            boolean ok = loginView.setAvatarFromPath(
                "C:\\Users\\Marcelo Echeverría Y\\Desktop\\TI1.1\\GR04_ARQUITECTURA\\TI1.3\\REST_JAVA_CONUNI_GR04\\02.CLIESC\\CLIESC_CONVUNI_REST_JAVA\\img\\sullivan.jpg"
            );
            if (!ok) {
                System.out.println("No se pudo cargar la imagen del avatar.");
            }

            LoginController loginController = new LoginController(loginView, service);
            loginView.setController(loginController);
            loginView.showView();
        });
    }
}
