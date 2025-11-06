package ec.edu.monster.vista;

import ec.edu.monster.controlador.ConversionController;
import ec.edu.monster.modelo.ConversionResult;
import ec.edu.monster.modelo.ConversionType;

import java.awt.*;
import java.text.Normalizer;
import java.util.*;
import javax.swing.*;
import javax.swing.border.EmptyBorder;
import javax.swing.plaf.basic.BasicTextFieldUI;

public class ConversionView extends JFrame {

    private ConversionController controller;
    private Runnable onLogout;

    // Controles con placeholder
    private final HintComboBox<String> comboCategoria =
            new HintComboBox<>(new DefaultComboBoxModel<>(), "Seleccionar Categoría");
    private final HintComboBox<ConversionType> comboTipo =
            new HintComboBox<>(new DefaultComboBoxModel<>(), "Seleccione un tipo");
    private final PlaceholderTextField txtValor = new PlaceholderTextField("Ingresa el valor numérico", 12);

    private final JLabel lblResultadoPrincipal = new JLabel("—");
    private final JLabel lblResultado = new JLabel("Entrada: —");
    private final JLabel lblDetalle = new JLabel("Detalles: —");
    private final JLabel lblMensajes = new JLabel(" ");

    private static final int FIELD_W = 320;
    private static final int FIELD_H = 44;

    private final Map<String, java.util.List<ConversionType>> tiposPorCategoria = new LinkedHashMap<>();

    public ConversionView() {
        super("ConUni • Conversor de unidades");
        UITheme.applyGlobalTheme();
        agruparTiposPorCategoria();
        buildUI();
        setLocationRelativeTo(null);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    }

    /* ===== API ===== */
    public void setController(ConversionController controller) { this.controller = controller; }
    public void setOnLogout(Runnable onLogout) { this.onLogout = onLogout; }
    public void showView() { setVisible(true); }

    public void showResult(ConversionResult result) {
        lblResultadoPrincipal.setText(String.format("%.6f %s", result.output, result.outputUnit));
        lblResultado.setText(String.format("Entrada: %.6f %s", result.input, result.inputUnit));
        lblDetalle.setText(result.conversion);

        lblMensajes.setBackground(new Color(221, 245, 221));
        lblMensajes.setForeground(new Color(22, 118, 38));
        lblMensajes.setText("Conversión realizada con éxito.");
    }

    public void showError(String message) {
        lblMensajes.setBackground(new Color(253, 229, 229));
        lblMensajes.setForeground(new Color(156, 24, 24));
        lblMensajes.setText(message);
    }
    public void showValidationError(String message) { showError(message); }

    /* ===== UI ===== */
    private void buildUI() {
        JPanel root = UITheme.gradientBackgroundPanel(new GridBagLayout());
        root.setBorder(new EmptyBorder(14, 18, 18, 18));
        root.setPreferredSize(new Dimension(1180, 760)); // evita encogerse

        // ===== Appbar =====
        JPanel appbar = new JPanel(new BorderLayout());
        appbar.setOpaque(false);

        JLabel title = new JLabel("Conversiones de Unidades");
        title.setForeground(Color.WHITE);
        title.setFont(title.getFont().deriveFont(Font.BOLD, 24f));
        appbar.add(title, BorderLayout.WEST);

        JPanel right = new JPanel(new FlowLayout(FlowLayout.RIGHT, 10, 0));
        right.setOpaque(false);
        JLabel logged = new JLabel("Conectado como MONSTER");
        logged.setForeground(new Color(230, 236, 255));
        JButton logout = createDangerButton("CERRAR SESIÓN");
        logout.addActionListener(e -> { if (onLogout != null) onLogout.run(); else dispose(); });
        right.add(logged); right.add(logout);
        appbar.add(right, BorderLayout.EAST);

        GridBagConstraints topC = new GridBagConstraints();
        topC.gridx=0; topC.gridy=0; topC.weightx=1;
        topC.fill = GridBagConstraints.HORIZONTAL;
        topC.insets = new Insets(4,4,16,4);
        root.add(appbar, topC);

        // ===== Tarjetas fila superior =====
        JPanel grid = new JPanel(new GridBagLayout());
        grid.setOpaque(false);

        Dimension cardDim      = new Dimension(360, 200); // izq/centro
        Dimension cardDimValue = new Dimension(360, 230); // derecha (valor + botón)

        HeaderCard card1 = new HeaderCard("Categoría de Conversión",
                new Color(80,125,255), new Color(58,98,205));
        card1.setAllSizes(cardDim);

        comboCategoria.setPreferredSize(new Dimension(FIELD_W, FIELD_H));
        FieldContainer catWrap = new FieldContainer(comboCategoria, FIELD_W, FIELD_H);
        card1.addContent(Box.createVerticalStrut(6));
        card1.addContent(catWrap);

        HeaderCard card2 = new HeaderCard("Tipo de Conversión",
                new Color(255,173,72), new Color(243,112,85));
        card2.setAllSizes(cardDim);

        comboTipo.setPreferredSize(new Dimension(FIELD_W, FIELD_H));
        FieldContainer tipoWrap = new FieldContainer(comboTipo, FIELD_W, FIELD_H);
        card2.addContent(Box.createVerticalStrut(6));
        card2.addContent(tipoWrap);

        HeaderCard card3 = new HeaderCard("Ingrese el Valor",
                new Color(48,201,134), new Color(23,162,132));
        card3.setAllSizes(cardDimValue);

        // Campo numérico (UI básica + placeholder)
        txtValor.setUI(new BasicTextFieldUI());
        txtValor.setOpaque(false);
        txtValor.setForeground(new Color(35,35,35));
        txtValor.setCaretColor(new Color(35,35,35));
        txtValor.setBorder(new EmptyBorder(10,14,10,14));
        txtValor.setPreferredSize(new Dimension(FIELD_W, FIELD_H));
        FieldContainer valueWrap = new FieldContainer(txtValor, FIELD_W, FIELD_H);

        JButton btnConvertir = UITheme.createPrimaryButton("CONVERTIR");
        btnConvertir.setPreferredSize(new Dimension(FIELD_W, FIELD_H));
        btnConvertir.setMaximumSize(new Dimension(FIELD_W, FIELD_H));
        btnConvertir.addActionListener(e -> convertir());
        txtValor.addActionListener(e -> convertir());

        card3.addContent(Box.createVerticalStrut(6));
        card3.addContent(valueWrap);
        card3.addContent(Box.createVerticalStrut(10));
        card3.addContent(btnConvertir);

        GridBagConstraints g = new GridBagConstraints();
        g.insets = new Insets(8,8,8,8);
        g.gridy = 0;
        g.gridx = 0; grid.add(card1, g);
        g.gridx = 1; grid.add(card2, g);
        g.gridx = 2; grid.add(card3, g);

        GridBagConstraints gridC = new GridBagConstraints();
        gridC.gridx=0; gridC.gridy=1; gridC.insets = new Insets(0,0,8,0);
        root.add(grid, gridC);

        // ===== Resultado =====
        HeaderCard resultCard = buildResultPanel();
        GridBagConstraints resC = new GridBagConstraints();
        resC.gridx=0; resC.gridy=2; resC.insets = new Insets(6,6,6,6);
        root.add(resultCard, resC);

        // Mensajes (limitamos altura para que no empuje el FAB)
        styleMessageLabelLocal(lblMensajes);
        lblMensajes.setPreferredSize(new Dimension(0, 36));
        lblMensajes.setMaximumSize(new Dimension(Integer.MAX_VALUE, 36));
        GridBagConstraints msgC = new GridBagConstraints();
        msgC.gridx=0; msgC.gridy=3; msgC.insets = new Insets(6, 80, 6, 80);
        msgC.fill = GridBagConstraints.HORIZONTAL;
        root.add(lblMensajes, msgC);

        // Glue para empujar FAB abajo y reservar espacio
        GridBagConstraints glueC = new GridBagConstraints();
        glueC.gridx = 0; glueC.gridy = 4; glueC.weighty = 1;
        glueC.fill = GridBagConstraints.BOTH;
        root.add(Box.createVerticalStrut(120), glueC); // espacio fijo inferior

        // FAB Limpiar (siempre visible)
        JPanel fabWrap = new JPanel(new FlowLayout(FlowLayout.RIGHT, 0, 0));
        fabWrap.setOpaque(false);
        fabWrap.setPreferredSize(new Dimension(0, 72)); // carril propio

        JButton btnClearAll = new FabButton();
        btnClearAll.setToolTipText("Limpiar todos los campos");
        btnClearAll.addActionListener(e -> clearAll());
        fabWrap.add(btnClearAll);

        GridBagConstraints fabC = new GridBagConstraints();
        fabC.gridx = 0; fabC.gridy = 5; fabC.weightx = 1;
        fabC.anchor = GridBagConstraints.SOUTHEAST;
        fabC.insets = new Insets(0, 0, 32, 32); // más dentro de la ventana
        root.add(fabWrap, fabC);

        // Combos
        comboCategoria.addActionListener(e -> recargarTipos());
        comboCategoria.setSelectedItem(null);
        comboTipo.setSelectedItem(null);

        setContentPane(root);
        setMinimumSize(new Dimension(1180, 760));
        pack();
    }

    /* ===== Resultado (contenido más arriba) ===== */
    private HeaderCard buildResultPanel() {
        HeaderCard card = new HeaderCard("Resultado",
                new Color(235,239,250), new Color(235,239,250));
        card.setTitleColor(new Color(90, 96, 120));

        card.setAllSizes(new Dimension(520, 220));
        // mucho menos padding arriba para que el contenido se vea bien
        card.setContentInsets(new Insets(6, 26, 26, 26));

        JPanel content = card.getContent();
        content.setLayout(new BoxLayout(content, BoxLayout.Y_AXIS));

        lblResultadoPrincipal.setFont(lblResultadoPrincipal.getFont().deriveFont(Font.BOLD, 28f));
        lblResultadoPrincipal.setAlignmentX(Component.LEFT_ALIGNMENT);
        lblResultado.setAlignmentX(Component.LEFT_ALIGNMENT);
        lblDetalle.setAlignmentX(Component.LEFT_ALIGNMENT);
        lblResultado.setForeground(new Color(70, 70, 70));
        lblDetalle.setForeground(new Color(90, 90, 90));

        content.add(Box.createVerticalStrut(2));
        content.add(lblResultadoPrincipal);
        content.add(Box.createVerticalStrut(8));
        content.add(lblResultado);
        content.add(Box.createVerticalStrut(4));
        content.add(lblDetalle);

        return card;
    }

    /* ===== Acciones ===== */
    private void convertir() {
        if (controller == null) { showError("No hay controlador asociado."); return; }
        if (comboTipo.getSelectedItem() == null) { showValidationError("Seleccione un tipo de conversión."); return; }
        String texto = txtValor.getText().trim();
        if (texto.isEmpty()) { showValidationError("Introduce un valor numérico."); return; }
        try {
            double valor = Double.parseDouble(texto);
            ConversionType tipo = (ConversionType) comboTipo.getSelectedItem();
            lblMensajes.setBackground(new Color(255,255,255,220));
            lblMensajes.setForeground(new Color(60,60,60));
            lblMensajes.setText("Procesando conversión...");
            controller.performConversion(tipo, valor);
        } catch (NumberFormatException ex) {
            showValidationError("El valor debe ser numérico.");
        }
    }

    private void clearAll() {
        comboCategoria.setSelectedItem(null);
        recargarTipos();
        txtValor.setText("");
        lblResultadoPrincipal.setText("—");
        lblResultado.setText("Entrada: —");
        lblDetalle.setText("Detalles: —");
        lblMensajes.setBackground(new Color(255,255,255,200));
        lblMensajes.setForeground(new Color(60,60,60));
        lblMensajes.setText("Campos reiniciados.");
        txtValor.requestFocus();
    }

    /* ================== Soporte visual local ================== */

    private JButton createDangerButton(String text) {
        UITheme.GradientButton b = new UITheme.GradientButton(
                text, new Color(255,105,97), new Color(224,62,62));
        b.setForeground(Color.WHITE);
        b.setFocusPainted(false);
        b.setContentAreaFilled(false);
        b.setBorder(new EmptyBorder(10,18,10,18));
        b.setCursor(Cursor.getPredefinedCursor(Cursor.HAND_CURSOR));
        b.setFont(b.getFont().deriveFont(Font.BOLD, 14f));
        return b;
    }

    private void styleMessageLabelLocal(JLabel label) {
        label.setOpaque(true);
        label.setBackground(new Color(255,255,255,0));
        label.setHorizontalAlignment(SwingConstants.CENTER);
        label.setBorder(new EmptyBorder(10,12,10,12));
        label.setAlignmentX(Component.CENTER_ALIGNMENT);
        label.setFont(label.getFont().deriveFont(Font.BOLD, 13f));
    }

    /** Tarjeta con header y panel interno de contenido. */
    private static class HeaderCard extends RoundedPanel {
        private final Color headA, headB;
        private String title;
        private Color titleColor = Color.WHITE;
        private int headerH = 56;
        private Insets contentInsets = new Insets(10, 20, 18, 20);

        private final JPanel content = new JPanel();

        HeaderCard(String title, Color headA, Color headB) {
            super(22);
            this.title = title; this.headA = headA; this.headB = headB;
            setBackground(Color.WHITE);
            setOpaque(false);
            setLayout(new BorderLayout());

            content.setOpaque(false);
            content.setLayout(new BoxLayout(content, BoxLayout.Y_AXIS));
            content.setBorder(new EmptyBorder(headerH + contentInsets.top,
                    contentInsets.left, contentInsets.bottom, contentInsets.right));
            add(content, BorderLayout.CENTER);
        }

        void setTitleColor(Color c) { this.titleColor = c; }
        void setContentInsets(Insets i) {
            this.contentInsets = i;
            content.setBorder(new EmptyBorder(headerH + contentInsets.top,
                    contentInsets.left, contentInsets.bottom, contentInsets.right));
        }
        JPanel getContent() { return content; }
        void addContent(Component c) { content.add(c); }
        void setAllSizes(Dimension d) { setPreferredSize(d); setMinimumSize(d); setMaximumSize(d); }

        @Override
        protected void paintComponent(Graphics g) {
            super.paintComponent(g);
            Graphics2D g2 = (Graphics2D) g.create();
            g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

            int w = getWidth(), r = 22;

            g2.setPaint(new GradientPaint(0, 0, headA, w, 0, headB));
            g2.fillRoundRect(0, 0, w - 6, headerH, r, r);
            g2.fillRect(0, headerH - r, w - 6, r);

            g2.setColor(titleColor);
            g2.setFont(getFont().deriveFont(Font.BOLD, 16f));
            g2.drawString(title, 22, 36);

            g2.dispose();
        }
    }

    /** Contenedor blanco redondeado (tamaño fijo) con sombra. */
    private static class FieldContainer extends JComponent {
        private final Dimension size;
        FieldContainer(JComponent inner, int w, int h) {
            setLayout(new BorderLayout());
            setOpaque(false);
            this.size = new Dimension(w, h);
            add(inner, BorderLayout.CENTER);
            setPreferredSize(size); setMinimumSize(size); setMaximumSize(size);
        }
        @Override protected void paintComponent(Graphics g) {
            Graphics2D g2 = (Graphics2D) g.create();
            g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
            int r = 14;
            g2.setColor(new Color(0,0,0,28)); g2.fillRoundRect(3,5,getWidth()-6,getHeight()-6,r,r);
            g2.setColor(Color.WHITE);         g2.fillRoundRect(0,0,getWidth()-6,getHeight()-6,r,r);
            g2.setColor(new Color(220,226,240));
            g2.setStroke(new BasicStroke(2f));
            g2.drawRoundRect(1,1,getWidth()-8,getHeight()-8,r,r);
            g2.dispose();
            super.paintComponent(g);
        }
    }

    /** TextField con placeholder nativo. */
    private static class PlaceholderTextField extends JTextField {
        private String placeholder;
        PlaceholderTextField(String placeholder, int cols) {
            super(cols);
            this.placeholder = placeholder;
            setOpaque(false);
        }
        @Override protected void paintComponent(Graphics g) {
            super.paintComponent(g);
            if (getText().isEmpty() && !isFocusOwner()) {
                Graphics2D g2 = (Graphics2D) g.create();
                g2.setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING, RenderingHints.VALUE_TEXT_ANTIALIAS_ON);
                g2.setColor(new Color(140,145,160));
                FontMetrics fm = g2.getFontMetrics(getFont());
                int y = (getHeight() - fm.getHeight())/2 + fm.getAscent();
                g2.drawString(placeholder, 14, y);
                g2.dispose();
            }
        }
    }

    /** ComboBox que muestra un hint cuando no hay selección. */
    private static class HintComboBox<T> extends JComboBox<T> {
        private final String hint;
        HintComboBox(ComboBoxModel<T> model, String hint) {
            super(model);
            this.hint = hint;
            setRenderer(new DefaultListCellRenderer() {
                @Override
                public Component getListCellRendererComponent(JList<?> list, Object value, int index,
                                                              boolean isSelected, boolean cellHasFocus) {
                    JLabel l = (JLabel) super.getListCellRendererComponent(list, value, index, isSelected, cellHasFocus);
                    if (index == -1 && value == null) {
                        l.setText(hint);
                        l.setForeground(new Color(140,145,160));
                    }
                    return l;
                }
            });
        }
    }

    /** Botón flotante (papelera). */
    private static class FabButton extends JButton {
        FabButton() {
            setPreferredSize(new Dimension(54, 54));
            setOpaque(false); setContentAreaFilled(false);
            setBorderPainted(false); setFocusPainted(false);
            setCursor(Cursor.getPredefinedCursor(Cursor.HAND_CURSOR));
        }
        @Override protected void paintComponent(Graphics g) {
            Graphics2D g2 = (Graphics2D) g.create();
            g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
            int w=getWidth(), h=getHeight();
            g2.setColor(new Color(0,0,0,55)); g2.fillOval(4,6,w-8,h-8);
            g2.setPaint(new GradientPaint(0,0,new Color(255,160,76),w,h,new Color(237,93,74)));
            g2.fillOval(0,0,w-8,h-8);
            g2.setColor(new Color(255,255,255,180)); g2.setStroke(new BasicStroke(2f));
            g2.drawOval(1,1,w-10,h-10);
            g2.setColor(Color.WHITE);
            int ox=12, oy=14, iw=20, ih=16;
            g2.fillRoundRect(ox+2, oy-6, iw-4, 5, 4, 4);
            g2.fillRoundRect(ox, oy, iw, ih, 4, 4);
            g2.setStroke(new BasicStroke(2f));
            g2.drawLine(ox+6,  oy+3,  ox+6,  oy+ih-3);
            g2.drawLine(ox+10, oy+3,  ox+10, oy+ih-3);
            g2.drawLine(ox+14, oy+3,  ox+14, oy+ih-3);
            g2.dispose();
            super.paintComponent(g);
        }
    }

    // ===== Lógica de categorías =====
    private void agruparTiposPorCategoria() {
        String[] orden = {"Longitud","Temperatura","Masa","Volumen","Área","Velocidad","Tiempo","Datos","Otros"};
        for (String cat : orden) tiposPorCategoria.put(cat, new ArrayList<>());

        for (ConversionType t : ConversionType.values()) {
            String nombre = normaliza(t.toString());
            String cat = "Otros";
            if (contiene(nombre, "celsius","fahrenheit","kelvin","temperatura","centigrado")) cat="Temperatura";
            else if (contiene(nombre, "metro","centimetro","kilometro","milla","yarda","pie","pulgada")) cat="Longitud";
            else if (contiene(nombre, "gramo","kilogramo","libra","onza","masa")) cat="Masa";
            else if (contiene(nombre, "litro","mililitro","galon","volumen")) cat="Volumen";
            else if (contiene(nombre, "metro cuadrado","hectarea","area")) cat="Área";
            else if (contiene(nombre, "km/h","kph","mph","velocidad")) cat="Velocidad";
            else if (contiene(nombre, "segundo","minuto","hora","tiempo","dia")) cat="Tiempo";
            else if (contiene(nombre, "bit","byte","kb","mb","gb","tb","datos")) cat="Datos";
            tiposPorCategoria.get(cat).add(t);
        }
        DefaultComboBoxModel<String> m = (DefaultComboBoxModel<String>) comboCategoria.getModel();
        for (Map.Entry<String, java.util.List<ConversionType>> e : tiposPorCategoria.entrySet())
            if (!e.getValue().isEmpty()) m.addElement(e.getKey());

        comboCategoria.setSelectedItem(null);
        comboTipo.setSelectedItem(null);
    }

    private void recargarTipos() {
        String cat = (String) comboCategoria.getSelectedItem();
        java.util.List<ConversionType> lista = tiposPorCategoria.getOrDefault(cat, Collections.emptyList());
        DefaultComboBoxModel<ConversionType> m = new DefaultComboBoxModel<>();
        for (ConversionType t : lista) m.addElement(t);
        comboTipo.setModel(m);
        comboTipo.setSelectedItem(null); // placeholder hasta elegir
    }

    private static String normaliza(String s) {
        String n = Normalizer.normalize(s, Normalizer.Form.NFD)
                .replaceAll("\\p{InCombiningDiacriticalMarks}+", "");
        return n.toLowerCase(Locale.ROOT);
    }
    private static boolean contiene(String base, String... needles) {
        for (String n : needles) if (base.contains(n)) return true;
        return false;
    }
}
