package ec.edu.monster.cliesc_convuni_soap_gr04;

import ec.edu.monster.view.LoginFrame;
import javax.swing.SwingUtilities;
import javax.swing.UIManager;

public class CLIESC_CONVUNI_SOAP_GR04 {
    public static void main(String[] args) {
        try {
            for (UIManager.LookAndFeelInfo info : UIManager.getInstalledLookAndFeels()) {
                if ("Nimbus".equals(info.getName())) {
                    UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (Exception ignored) {}
        SwingUtilities.invokeLater(() -> new LoginFrame().setVisible(true));
    }
}
