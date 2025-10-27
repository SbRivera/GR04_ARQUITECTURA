package ec.edu.monster.controlador;

import ec.edu.monster.modelo.Credentials;
import ec.edu.monster.servicio.ConUniService;
import ec.edu.monster.vista.ConversionView;
import ec.edu.monster.vista.LoginView;
import javax.swing.SwingUtilities;

public class LoginController {
    private static final String USER = "MONSTER";
    private static final String PASS = "MONSTER9";

    private final LoginView view;
    private final ConUniService service;

    public LoginController(LoginView view, ConUniService service) {
        this.view = view;
        this.service = service;
    }

    public void attemptLogin(Credentials credentials) {
        if (USER.equalsIgnoreCase(credentials.username())
                && PASS.equals(credentials.password())) {

            SwingUtilities.invokeLater(() -> {
                ConversionView conversionView = new ConversionView();
                ConversionController controller = new ConversionController(conversionView, service);
                conversionView.setController(controller);
                conversionView.showView();
            });
            view.close();
        } else {
            view.showError("Credenciales inválidas.");
        }
    }
}