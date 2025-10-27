package ec.edu.monster.vista;

import ec.edu.monster.controlador.ConversionController;
import ec.edu.monster.modelo.ConversionResult;
import ec.edu.monster.modelo.ConversionType;
import java.awt.*;
import javax.swing.*;
import javax.swing.border.EmptyBorder;

public class ConversionView extends JFrame {
    private ConversionController controller;

    private final JComboBox<ConversionType> comboConversion =
            new JComboBox<>(ConversionType.values());
    private final JTextField txtValor = new JTextField(12);

    private final JLabel lblResultadoPrincipal = new JLabel("—");
    private final JLabel lblResultado = new JLabel("Resultado: —");
    private final JLabel lblDetalle = new JLabel("Detalles: —");
    private final JLabel lblMensajes = new JLabel(" ");

    public ConversionView() {
        super("ConUni • Conversor de unidades");
        buildUI();
        setLocationRelativeTo(null);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    }

    public void setController(ConversionController controller) {
        this.controller = controller;
    }

    public void showView() {
        setVisible(true);
    }

    public void showResult(ConversionResult result) {
        lblResultadoPrincipal.setText(String.format("%.6f %s", result.output, result.outputUnit));
        lblResultado.setText(String.format("Entrada: %.6f %s", result.input, result.inputUnit));
        lblDetalle.setText(result.conversion);

        lblMensajes.setBackground(new Color(221, 245, 221));
        lblMensajes.setForeground(new Color(22, 118, 38));
        lblMensajes.setText("Conversión realizada con éxito. ¡Resultados fresquitos!");
    }

    public void showError(String message) {
        lblMensajes.setBackground(new Color(253, 229, 229));
        lblMensajes.setForeground(new Color(156, 24, 24));
        lblMensajes.setText(message);
    }

    public void showValidationError(String message) {
        showError(message);
    }

    private void buildUI() {
        UITheme.styleTextField(txtValor);
        UITheme.styleComboBox(comboConversion);

        JButton btnConvertir = new JButton("Convertir");
        UITheme.stylePrimaryButton(btnConvertir);
        btnConvertir.addActionListener(e -> convertir());
        txtValor.addActionListener(e -> convertir());

        JPanel header = new ConversionHeader();
        header.setPreferredSize(new Dimension(420, 120));

        RoundedPanel formCard = new RoundedPanel(18);
        formCard.setBackground(UITheme.CARD);
        formCard.setBorder(new EmptyBorder(22, 26, 18, 26));
        formCard.setLayout(new GridBagLayout());

        GridBagConstraints gbc = new GridBagConstraints();
        gbc.insets = new Insets(12, 12, 12, 12);

        gbc.gridx = 0; gbc.gridy = 0; gbc.anchor = GridBagConstraints.LINE_END;
        formCard.add(new JLabel("Conversión:"), gbc);
        gbc.gridx = 1; gbc.anchor = GridBagConstraints.LINE_START;
        formCard.add(comboConversion, gbc);

        gbc.gridx = 0; gbc.gridy = 1; gbc.anchor = GridBagConstraints.LINE_END;
        formCard.add(new JLabel("Valor:"), gbc);
        gbc.gridx = 1; gbc.anchor = GridBagConstraints.LINE_START;
        formCard.add(txtValor, gbc);

        gbc.gridx = 0; gbc.gridy = 2; gbc.gridwidth = 2; gbc.anchor = GridBagConstraints.CENTER;
        gbc.insets = new Insets(20, 12, 4, 12);
        formCard.add(btnConvertir, gbc);

        RoundedPanel resultCard = buildResultPanel();

        UITheme.styleMessageLabel(lblMensajes);

        JPanel content = new JPanel();
        content.setOpaque(false);
        content.setLayout(new BoxLayout(content, BoxLayout.Y_AXIS));
        content.add(formCard);
        content.add(Box.createVerticalStrut(14));
        content.add(resultCard);
        content.add(Box.createVerticalStrut(12));
        content.add(lblMensajes);

        JPanel root = new JPanel(new BorderLayout(0, 18));
        root.setBackground(UITheme.BACKGROUND);
        root.setBorder(new EmptyBorder(18, 22, 18, 22));
        root.add(header, BorderLayout.NORTH);
        root.add(content, BorderLayout.CENTER);

        setContentPane(root);
        pack();
        setMinimumSize(new Dimension(520, getHeight()));
    }

    private RoundedPanel buildResultPanel() {
        RoundedPanel card = new RoundedPanel(18);
        card.setBackground(UITheme.CARD);
        card.setBorder(new EmptyBorder(20, 28, 20, 28));
        card.setLayout(new BoxLayout(card, BoxLayout.Y_AXIS));
        card.setAlignmentX(Component.LEFT_ALIGNMENT);

        JLabel titulo = new JLabel("Detalle de la conversión");
        titulo.setFont(titulo.getFont().deriveFont(Font.BOLD, 17f));
        titulo.setAlignmentX(Component.LEFT_ALIGNMENT);

        lblResultadoPrincipal.setFont(lblResultadoPrincipal.getFont().deriveFont(Font.BOLD, 28f));
        lblResultadoPrincipal.setAlignmentX(Component.LEFT_ALIGNMENT);
        lblResultado.setAlignmentX(Component.LEFT_ALIGNMENT);
        lblDetalle.setAlignmentX(Component.LEFT_ALIGNMENT);

        lblResultado.setForeground(new Color(70, 70, 70));
        lblDetalle.setForeground(new Color(90, 90, 90));

        card.add(titulo);
        card.add(Box.createVerticalStrut(8));
        card.add(lblResultadoPrincipal);
        card.add(Box.createVerticalStrut(10));
        card.add(lblResultado);
        card.add(Box.createVerticalStrut(6));
        card.add(lblDetalle);

        return card;
    }

    private void convertir() {
        if (controller == null) {
            showError("No hay controlador asociado.");
            return;
        }
        String texto = txtValor.getText().trim();
        if (texto.isEmpty()) {
            showValidationError("Introduce un valor numérico.");
            return;
        }
        try {
            double valor = Double.parseDouble(texto);
            ConversionType tipo = (ConversionType) comboConversion.getSelectedItem();
            lblMensajes.setBackground(new Color(255, 255, 255, 220));
            lblMensajes.setForeground(new Color(60, 60, 60));
            lblMensajes.setText("Procesando conversión...");
            controller.performConversion(tipo, valor);
        } catch (NumberFormatException ex) {
            showValidationError("El valor debe ser numérico. Sin jeroglíficos, por favor.");
        }
    }

    private static class ConversionHeader extends JPanel {
        ConversionHeader() {
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

            textG.setColor(Color.WHITE);
            textG.setFont(textG.getFont().deriveFont(Font.BOLD, 24f));
            textG.drawString("Conversor REST Monster", 24, 48);

            textG.setColor(new Color(220, 228, 240));
            textG.setFont(textG.getFont().deriveFont(Font.PLAIN, 15f));
            textG.drawString("Selecciona una conversión", 24, 74);

            textG.dispose();
        }
    }
}