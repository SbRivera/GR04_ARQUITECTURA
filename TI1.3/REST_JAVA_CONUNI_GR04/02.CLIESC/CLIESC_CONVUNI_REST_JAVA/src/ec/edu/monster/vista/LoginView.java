package ec.edu.monster.vista;

import ec.edu.monster.controlador.LoginController;
import ec.edu.monster.modelo.Credentials;
import java.awt.*;
import javax.swing.*;
import javax.swing.border.EmptyBorder;

public class LoginView extends JFrame {
    private LoginController controller;

    private final JTextField txtUsuario = new JTextField(14);
    private final JPasswordField txtPassword = new JPasswordField(14);
    private final JLabel lblMensaje = new JLabel(" ");

    public LoginView() {
        super("ConUni • Acceso");
        buildUI();
        setResizable(false);
        setLocationRelativeTo(null);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    }

    public void setController(LoginController controller) {
        this.controller = controller;
    }

    public void showView() {
        setVisible(true);
    }

    public void close() {
        dispose();
    }

    public void showError(String message) {
        lblMensaje.setForeground(new Color(170, 31, 31));
        lblMensaje.setBackground(new Color(253, 229, 229));
        lblMensaje.setText(message);
        txtPassword.setText("");
        txtPassword.requestFocus();
    }

    private void buildUI() {
        UITheme.styleTextField(txtUsuario);
        UITheme.styleTextField(txtPassword);

        JButton btnIngresar = new JButton("Ingresar");
        UITheme.stylePrimaryButton(btnIngresar);
        btnIngresar.addActionListener(e -> enviar());
        txtPassword.addActionListener(e -> enviar());

        JPanel header = new GradientHeader("Bienvenido a ConUni",
                "Ingresa tus credenciales monstruosas");
        header.setPreferredSize(new Dimension(360, 120));

        RoundedPanel formCard = new RoundedPanel(18);
        formCard.setBackground(UITheme.CARD);
        formCard.setLayout(new GridBagLayout());
        formCard.setBorder(new EmptyBorder(20, 26, 20, 26));

        GridBagConstraints gbc = new GridBagConstraints();
        gbc.insets = new Insets(10, 10, 10, 10);

        gbc.gridx = 0; gbc.gridy = 0; gbc.anchor = GridBagConstraints.LINE_END;
        formCard.add(new JLabel("Usuario:"), gbc);
        gbc.gridx = 1; gbc.anchor = GridBagConstraints.LINE_START;
        formCard.add(txtUsuario, gbc);

        gbc.gridx = 0; gbc.gridy = 1; gbc.anchor = GridBagConstraints.LINE_END;
        formCard.add(new JLabel("Contraseña:"), gbc);
        gbc.gridx = 1; gbc.anchor = GridBagConstraints.LINE_START;
        formCard.add(txtPassword, gbc);

        gbc.gridx = 0; gbc.gridy = 2; gbc.gridwidth = 2; gbc.anchor = GridBagConstraints.CENTER;
        gbc.insets = new Insets(18, 10, 8, 10);
        formCard.add(btnIngresar, gbc);

        UITheme.styleMessageLabel(lblMensaje);

        JPanel root = new JPanel(new BorderLayout(0, 18));
        root.setBorder(new EmptyBorder(20, 24, 20, 24));
        root.setBackground(UITheme.BACKGROUND);
        root.add(header, BorderLayout.NORTH);

        JPanel center = new JPanel();
        center.setOpaque(false);
        center.setLayout(new BoxLayout(center, BoxLayout.Y_AXIS));
        formCard.setAlignmentX(Component.CENTER_ALIGNMENT);
        lblMensaje.setAlignmentX(Component.CENTER_ALIGNMENT);

        center.add(formCard);
        center.add(Box.createVerticalStrut(12));
        center.add(lblMensaje);

        root.add(center, BorderLayout.CENTER);

        setContentPane(root);
        pack();
    }

    private void enviar() {
        lblMensaje.setForeground(new Color(60, 60, 60));
        lblMensaje.setBackground(new Color(255, 255, 255, 200));
        lblMensaje.setText("Validando credenciales...");
        if (controller != null) {
            Credentials credentials = new Credentials(
                    txtUsuario.getText().trim(),
                    new String(txtPassword.getPassword())
            );
            controller.attemptLogin(credentials);
        }
    }

    private static class GradientHeader extends JPanel {
        private final String title;
        private final String subtitle;

        private GradientHeader(String title, String subtitle) {
            this.title = title;
            this.subtitle = subtitle;
            setOpaque(false);
        }

        @Override
        protected void paintComponent(Graphics g) {
            super.paintComponent(g);
            Graphics2D g2 = (Graphics2D) g.create();
            g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
            g2.setPaint(new GradientPaint(0, 0, UITheme.PRIMARY_TOP,
                    getWidth(), getHeight(), UITheme.PRIMARY_BOTTOM));
            g2.fillRoundRect(0, 0, getWidth(), getHeight(), 18, 18);
            g2.dispose();

            Graphics2D textG = (Graphics2D) g.create();
            textG.setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING,
                    RenderingHints.VALUE_TEXT_ANTIALIAS_ON);

            Font titleFont = getFont().deriveFont(Font.BOLD, 24f);
            Font subtitleFont = getFont().deriveFont(Font.PLAIN, 15f);

            textG.setColor(Color.WHITE);
            textG.setFont(titleFont);
            textG.drawString(title, 24, 48);

            textG.setColor(new Color(220, 230, 240));
            textG.setFont(subtitleFont);
            textG.drawString(subtitle, 24, 72);

            textG.dispose();
        }
    }
}