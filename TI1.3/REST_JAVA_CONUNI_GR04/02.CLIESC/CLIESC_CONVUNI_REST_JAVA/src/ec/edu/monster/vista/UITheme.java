package ec.edu.monster.vista;

import java.awt.*;
import java.util.Enumeration;
import javax.swing.*;
import javax.swing.border.EmptyBorder;
import javax.swing.text.JTextComponent;

public final class UITheme {
    private UITheme() {}

    /* ======= Paleta (match de la maqueta) ======= */
    // Fondo general (de izquierda violeta a derecha magenta)
    public static final Color BG_A = new Color(126, 76, 229);   // #7E4CE5
    public static final Color BG_B = new Color(196, 118, 255);  // #C476FF

    // Card (degradado interno violeta -> rosado)
    public static final Color CARD_A = new Color(175, 117, 255); // #AF75FF
    public static final Color CARD_B = new Color(229, 140, 236); // #E58CEC

    // Botón principal (azul -> violeta)
    public static final Color BTN_A = new Color(90, 124, 255);   // #5A7CFF
    public static final Color BTN_B = new Color(138, 99, 232);   // #8A63E8

    public static final Color TEXT_MAIN = new Color(38, 38, 45);
    public static final Color TEXT_SOFT = new Color(235, 238, 248);

    
    
    public static void applyGlobalTheme() {
        try {
            UIManager.setLookAndFeel("javax.swing.plaf.nimbus.NimbusLookAndFeel");
        } catch (Exception ignored) {}

        UIManager.put("control", new Color(245, 246, 252));
        UIManager.put("text", TEXT_MAIN);
        UIManager.put("nimbusLightBackground", Color.WHITE);
        UIManager.put("nimbusBase", new Color(95, 90, 170));
        UIManager.put("nimbusBlueGrey", new Color(120, 125, 170));

        setDefaultFont(new Font("Segoe UI", Font.PLAIN, 15));
    }

    private static void setDefaultFont(Font font) {
        for (Enumeration<?> e = UIManager.getDefaults().keys(); e.hasMoreElements();) {
            Object k = e.nextElement();
            Object v = UIManager.get(k);
            if (v instanceof Font) UIManager.put(k, font);
        }
    }

    /* ===== Helpers ===== */

    /** Panel de fondo con degradado del layout final */
    public static JPanel gradientBackgroundPanel(LayoutManager layout) {
        return new JPanel(layout) {
            @Override protected void paintComponent(Graphics g) {
                super.paintComponent(g);
                Graphics2D g2 = (Graphics2D) g.create();
                g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
                g2.setPaint(new GradientPaint(0, 0, BG_A, getWidth(), getHeight(), BG_B));
                g2.fillRect(0, 0, getWidth(), getHeight());
                g2.dispose();
            }
        };
    }

    /** Botón con degradado y hover, radios suaves */
    public static GradientButton createPrimaryButton(String text) {
        GradientButton b = new GradientButton(text, BTN_A, BTN_B);
        b.setForeground(Color.WHITE);
        b.setFocusPainted(false);
        b.setContentAreaFilled(false);
        b.setBorder(new EmptyBorder(12, 22, 12, 22));
        b.setCursor(Cursor.getPredefinedCursor(Cursor.HAND_CURSOR));
        b.setFont(b.getFont().deriveFont(Font.BOLD, 15f));
        return b;
    }

    /** Campos redondeados translúcidos estilo maqueta */
    public static void styleRoundedField(JTextComponent c) {
        c.setOpaque(false);              // pintamos el fondo nosotros
        c.setForeground(new Color(250, 250, 255));
        c.setCaretColor(Color.WHITE);
        c.setBorder(BorderFactory.createEmptyBorder(10, 14, 10, 14));
        c.setPreferredSize(new Dimension(320, 44));
    }

    /** Label de ayuda inferior (gris muy claro) */
    public static JLabel hintLabel(String text) {
        JLabel l = new JLabel(text, SwingConstants.CENTER);
        l.setForeground(new Color(235, 235, 245));
        l.setFont(l.getFont().deriveFont(Font.PLAIN, 12f));
        return l;
    }

    /* ====== Botón degradado ====== */
    public static class GradientButton extends JButton {
        private final Color c1, c2;
        private boolean hover = false;

        public GradientButton(String text, Color c1, Color c2) {
            super(text);
            this.c1 = c1; this.c2 = c2;
            setOpaque(false); setBorderPainted(false);
            addMouseListener(new java.awt.event.MouseAdapter() {
                @Override public void mouseEntered(java.awt.event.MouseEvent e) { hover = true; repaint(); }
                @Override public void mouseExited (java.awt.event.MouseEvent e) { hover = false; repaint(); }
            });
        }

        @Override protected void paintComponent(Graphics g) {
            Graphics2D g2 = (Graphics2D) g.create();
            g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

            Color a = hover ? c1.brighter() : c1;
            Color b = hover ? c2.brighter() : c2;

            g2.setPaint(new GradientPaint(0, 0, a, getWidth(), getHeight(), b));
            g2.fillRoundRect(0, 0, getWidth(), getHeight(), 16, 16);

            // borde/sombra sutil
            g2.setColor(new Color(0, 0, 0, 35));
            g2.drawRoundRect(0, 0, getWidth()-1, getHeight()-1, 16, 16);

            g2.dispose();
            super.paintComponent(g);
        }
    }
}
