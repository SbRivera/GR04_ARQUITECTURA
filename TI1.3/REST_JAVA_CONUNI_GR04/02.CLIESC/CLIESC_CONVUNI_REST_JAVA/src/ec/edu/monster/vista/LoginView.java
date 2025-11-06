package ec.edu.monster.vista;

import ec.edu.monster.controlador.LoginController;
import ec.edu.monster.modelo.Credentials;

import java.awt.*;
import java.awt.event.FocusAdapter;
import java.awt.event.FocusEvent;
import java.net.URL;
import javax.swing.*;
import javax.swing.border.EmptyBorder;
import javax.swing.plaf.basic.BasicPasswordFieldUI;
import javax.swing.plaf.basic.BasicTextFieldUI;

public class LoginView extends JFrame {
    // Tamaños compactos
    private static final int FIELD_W = 340;
    private static final int FIELD_H = 44;

    private LoginController controller;

    private final HintTextField txtUsuario = new HintTextField("Usuario");
    private final HintPasswordField txtPassword = new HintPasswordField("Contraseña");
    private final JLabel lblMensaje = new JLabel(" ");

    private Image avatarImage = null; // se pinta en el avatar circular

    public LoginView() {
        super("ConUni • Acceso");
        UITheme.applyGlobalTheme();
        buildUI();
        setResizable(false);
        setLocationRelativeTo(null);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    }

    /* ================== Avatar ================== */
    /** Usa una imagen ya cargada (se reescala automáticamente). */
    public void setAvatar(Image img) { this.avatarImage = img; repaint(); }

    /** Carga la imagen desde una ruta del sistema (C:\... o /home/...). */
    public boolean setAvatarFromPath(String absoluteOrRelativePath) {
        ImageIcon icon = new ImageIcon(absoluteOrRelativePath);
        if (icon.getIconWidth() > 0) {
            setAvatar(icon.getImage());
            return true;
        }
        return false;
    }

    /** Carga la imagen desde el classpath (p.ej. "/ec/edu/monster/vista/assets/sully.png"). */
    public boolean setAvatarFromResource(String resourcePathOnClasspath) {
        URL url = getClass().getResource(resourcePathOnClasspath);
        if (url != null) {
            ImageIcon icon = new ImageIcon(url);
            setAvatar(icon.getImage());
            return true;
        }
        return false;
    }
    /* ============================================ */

    public void setController(LoginController controller) { this.controller = controller; }
    public void showView() { setVisible(true); }
    public void close() { dispose(); }

    public void showError(String message) {
        lblMensaje.setForeground(new Color(255, 215, 220));
        lblMensaje.setText(message);
        txtPassword.setText("");
        txtPassword.requestFocus();
    }

    private void buildUI() {
        // Fondo degradado
        JPanel root = UITheme.gradientBackgroundPanel(new GridBagLayout());
        root.setBorder(new EmptyBorder(20, 20, 20, 20));

        // Card principal (compacta)
        RoundedPanel card = new RoundedPanel(28);
        card.setGradient(UITheme.CARD_A, UITheme.CARD_B);
        card.setPreferredSize(new Dimension(440, 480));
        card.setLayout(new BoxLayout(card, BoxLayout.Y_AXIS));

        card.add(Box.createVerticalStrut(6)); // margen top

        // Avatar circular
        JComponent avatar = new JComponent() {
            @Override protected void paintComponent(Graphics g) {
                super.paintComponent(g);
                Graphics2D g2 = (Graphics2D) g.create();
                g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
                g2.setRenderingHint(RenderingHints.KEY_INTERPOLATION, RenderingHints.VALUE_INTERPOLATION_BILINEAR);

                int d = Math.min(getWidth(), getHeight());
                int x = (getWidth()-d)/2, y = (getHeight()-d)/2;

                // fondo blanco translúcido
                g2.setColor(new Color(255, 255, 255, 220));
                g2.fillOval(x, y, d, d);

                if (avatarImage != null) {
                    Shape clip = new java.awt.geom.Ellipse2D.Float(x, y, d, d);
                    g2.setClip(clip);
                    g2.drawImage(avatarImage, x, y, d, d, null); // escalado suave
                    g2.setClip(null);
                } else {
                    g2.setColor(new Color(120, 90, 230));
                    g2.setFont(getFont().deriveFont(Font.BOLD, 22f));
                    String s = "M";
                    FontMetrics fm = g2.getFontMetrics();
                    g2.drawString(s, x + (d - fm.stringWidth(s))/2,
                                     y + (d + fm.getAscent()-fm.getDescent())/2);
                }

                // aro
                g2.setStroke(new BasicStroke(4f));
                g2.setColor(Color.WHITE);
                g2.drawOval(x+2, y+2, d-4, d-4);
                g2.dispose();
            }
        };
        avatar.setPreferredSize(new Dimension(88, 88));
        avatar.setMaximumSize(avatar.getPreferredSize());
        avatar.setAlignmentX(Component.CENTER_ALIGNMENT);
        card.add(avatar);

        card.add(Box.createVerticalStrut(6));

        // Títulos
        JLabel titulo = new JLabel("¡Bienvenido!");
        titulo.setForeground(Color.WHITE);
        titulo.setFont(titulo.getFont().deriveFont(Font.BOLD, 26f));
        titulo.setAlignmentX(Component.CENTER_ALIGNMENT);
        card.add(titulo);

        JLabel subtitulo = new JLabel("Conversor de Unidades");
        subtitulo.setForeground(UITheme.TEXT_SOFT);
        subtitulo.setFont(subtitulo.getFont().deriveFont(Font.PLAIN, 13f));
        subtitulo.setAlignmentX(Component.CENTER_ALIGNMENT);
        card.add(subtitulo);

        card.add(Box.createVerticalStrut(12));

        // ===== Campos (placeholders dentro, sin fondo Nimbus) =====
        txtUsuario.setUI(new BasicTextFieldUI());
        txtPassword.setUI(new BasicPasswordFieldUI());

        txtUsuario.setOpaque(false);
        txtPassword.setOpaque(false);
        txtUsuario.setForeground(Color.WHITE);
        txtPassword.setForeground(Color.WHITE);
        txtUsuario.setCaretColor(Color.WHITE);
        txtPassword.setCaretColor(Color.WHITE);
        txtUsuario.setSelectionColor(new Color(120, 160, 255, 140));
        txtPassword.setSelectionColor(new Color(120, 160, 255, 140));
        txtUsuario.setSelectedTextColor(Color.WHITE);
        txtPassword.setSelectedTextColor(Color.WHITE);

        UITheme.styleRoundedField(txtUsuario);
        UITheme.styleRoundedField(txtPassword);
        txtUsuario.setPreferredSize(new Dimension(FIELD_W, FIELD_H));
        txtPassword.setPreferredSize(new Dimension(FIELD_W, FIELD_H));

        FieldWrapper fwUser = new FieldWrapper(txtUsuario, FIELD_W, FIELD_H);
        FieldWrapper fwPass = new FieldWrapper(txtPassword, FIELD_W, FIELD_H);
        fwUser.setAlignmentX(Component.CENTER_ALIGNMENT);
        fwPass.setAlignmentX(Component.CENTER_ALIGNMENT);

        card.add(fwUser);
        card.add(Box.createVerticalStrut(10));
        card.add(fwPass);
        card.add(Box.createVerticalStrut(12));

        // Botón MISMO tamaño que los campos
        JButton btnIngresar = UITheme.createPrimaryButton("INICIAR SESIÓN");
        Dimension same = new Dimension(FIELD_W, FIELD_H);
        btnIngresar.setPreferredSize(same);
        btnIngresar.setMinimumSize(same);
        btnIngresar.setMaximumSize(same);
        btnIngresar.setAlignmentX(Component.CENTER_ALIGNMENT);
        btnIngresar.addActionListener(e -> enviar());
        txtPassword.addActionListener(e -> enviar());
        card.add(btnIngresar);

        card.add(Box.createVerticalStrut(6));

        // Hint + mensaje
        JLabel hint = new JLabel("Por favor, ingrese sus credenciales");
        hint.setForeground(UITheme.TEXT_SOFT);
        hint.setFont(hint.getFont().deriveFont(Font.PLAIN, 12f));
        hint.setAlignmentX(Component.CENTER_ALIGNMENT);
        card.add(hint);

        lblMensaje.setOpaque(false);
        lblMensaje.setHorizontalAlignment(SwingConstants.CENTER);
        lblMensaje.setForeground(UITheme.TEXT_SOFT);
        lblMensaje.setFont(lblMensaje.getFont().deriveFont(Font.PLAIN, 12f));
        lblMensaje.setAlignmentX(Component.CENTER_ALIGNMENT);
        card.add(lblMensaje);

        // Centrar card en el fondo
        GridBagConstraints rc = new GridBagConstraints();
        rc.gridx = 0; rc.gridy = 0; rc.anchor = GridBagConstraints.CENTER;
        root.add(card, rc);

        setContentPane(root);
        setMinimumSize(new Dimension(880, 580));
        pack();
    }

    private void enviar() {
        lblMensaje.setText("Validando credenciales...");
        if (controller != null) {
            Credentials cr = new Credentials(
                    txtUsuario.getText().trim(),
                    new String(txtPassword.getPassword())
            );
            controller.attemptLogin(cr);
        }
    }

    /* ====== Fondo del campo (violeta translúcido + borde suave) ====== */
    private static class FieldWrapper extends JComponent {
        private final JComponent inner;
        private final Dimension size;

        FieldWrapper(JComponent inner, int w, int h) {
            setLayout(new BorderLayout());
            setOpaque(false);
            this.inner = inner;
            this.size = new Dimension(w, h);
            add(inner, BorderLayout.CENTER);
            setPreferredSize(size);
            setMinimumSize(size);
            setMaximumSize(size);
        }

        @Override protected void paintComponent(Graphics g) {
            super.paintComponent(g);
            Graphics2D g2 = (Graphics2D) g.create();
            g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
            int r = 12;

            // Fondo violeta translúcido (contraste adecuado)
            g2.setColor(new Color(110, 85, 210, 110));
            g2.fillRoundRect(0, 0, getWidth(), getHeight(), r, r);

            // Borde lavanda tenue
            g2.setColor(new Color(220, 200, 255, 180));
            g2.setStroke(new BasicStroke(2f));
            g2.drawRoundRect(1, 1, getWidth()-2, getHeight()-2, r, r);

            g2.dispose();
        }

        @Override public Insets getInsets() { return new Insets(0, 0, 0, 0); }
    }

    /* ====== TextField/Password con placeholder ====== */
    private static class HintTextField extends JTextField {
        private final String hint;
        public HintTextField(String hint) {
            this.hint = hint;
            setOpaque(false);
            setForeground(Color.WHITE);
            setCaretColor(Color.WHITE);
            setBorder(new EmptyBorder(10, 14, 10, 14));
            addFocusListener(new FocusAdapter() { @Override public void focusGained(FocusEvent e){ repaint(); } @Override public void focusLost(FocusEvent e){ repaint(); }});
        }
        @Override protected void paintComponent(Graphics g) {
            super.paintComponent(g);
            if (getText().isEmpty() && !isFocusOwner()) {
                Graphics2D g2 = (Graphics2D) g.create();
                g2.setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING, RenderingHints.VALUE_TEXT_ANTIALIAS_ON);
                g2.setColor(new Color(255, 255, 255, 230));
                g2.setFont(getFont());
                g2.drawString(hint, getInsets().left, getHeight()/2 + g2.getFontMetrics().getAscent()/2 - 3);
                g2.dispose();
            }
        }
    }

    private static class HintPasswordField extends JPasswordField {
        private final String hint;
        public HintPasswordField(String hint) {
            this.hint = hint;
            setOpaque(false);
            setForeground(Color.WHITE);
            setCaretColor(Color.WHITE);
            setBorder(new EmptyBorder(10, 14, 10, 14));
            addFocusListener(new FocusAdapter() { @Override public void focusGained(FocusEvent e){ repaint(); } @Override public void focusLost(FocusEvent e){ repaint(); }});
        }
        @Override protected void paintComponent(Graphics g) {
            super.paintComponent(g);
            if (getPassword().length == 0 && !isFocusOwner()) {
                Graphics2D g2 = (Graphics2D) g.create();
                g2.setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING, RenderingHints.VALUE_TEXT_ANTIALIAS_ON);
                g2.setColor(new Color(255, 255, 255, 230));
                g2.setFont(getFont());
                g2.drawString(hint, getInsets().left, getHeight()/2 + g2.getFontMetrics().getAscent()/2 - 3);
                g2.dispose();
            }
        }
    }
}
