package ec.edu.monster.vista;

import java.awt.*;
import javax.swing.*;
import javax.swing.border.CompoundBorder;
import javax.swing.border.EmptyBorder;
import javax.swing.border.LineBorder;
import javax.swing.text.JTextComponent;

public final class UITheme {
    private UITheme() {}

    public static final Color PRIMARY_TOP = new Color(24, 62, 122);
    public static final Color PRIMARY_BOTTOM = new Color(11, 21, 55);
    public static final Color BACKGROUND = new Color(234, 240, 247);
    public static final Color CARD = new Color(255, 255, 255);
    public static final Color ACCENT = new Color(255, 123, 67);
    public static final Color ACCENT_DARK = new Color(232, 99, 39);
    public static final Color BORDER_SOFT = new Color(205, 218, 233);

    public static void applyGlobalTheme() {
        try {
            UIManager.setLookAndFeel("javax.swing.plaf.nimbus.NimbusLookAndFeel");
        } catch (Exception ex) {
            System.err.println("No se pudo establecer Nimbus: " + ex.getMessage());
        }

        UIManager.put("control", BACKGROUND);
        UIManager.put("text", new Color(35, 35, 35));
        UIManager.put("nimbusBase", PRIMARY_BOTTOM);
        UIManager.put("nimbusBlueGrey", new Color(70, 90, 120));
        UIManager.put("nimbusLightBackground", CARD);

        Font baseFont = new Font("Segoe UI", Font.PLAIN, 15);
        setDefaultFont(baseFont);
    }

    private static void setDefaultFont(Font font) {
        UIManager.getDefaults().keys().asIterator().forEachRemaining(key -> {
            Object value = UIManager.get(key);
            if (value instanceof Font) {
                UIManager.put(key, font);
            }
        });
    }

    public static void styleTextField(JTextComponent field) {
        field.setOpaque(true);
        field.setBackground(Color.WHITE);
        field.setBorder(new CompoundBorder(
                new LineBorder(BORDER_SOFT, 2, true),
                new EmptyBorder(6, 12, 6, 12)
        ));
    }

    public static void styleComboBox(JComboBox<?> combo) {
        combo.setBackground(Color.WHITE);
        combo.setBorder(new LineBorder(BORDER_SOFT, 2, true));
        combo.setFocusable(false);
    }

    public static void stylePrimaryButton(AbstractButton button) {
        button.setBackground(ACCENT);
        button.setForeground(Color.WHITE);
        button.setBorder(new LineBorder(ACCENT_DARK, 2, true));
        button.setFocusPainted(false);
        button.setCursor(new Cursor(Cursor.HAND_CURSOR));
        button.setFont(button.getFont().deriveFont(Font.BOLD));
        button.setOpaque(true);
    }

    public static void styleMessageLabel(JLabel label) {
        label.setOpaque(true);
        label.setBackground(new Color(255, 255, 255, 0));
        label.setHorizontalAlignment(SwingConstants.CENTER);
        label.setBorder(new EmptyBorder(10, 12, 10, 12));
        label.setAlignmentX(Component.CENTER_ALIGNMENT);
        label.setFont(label.getFont().deriveFont(Font.BOLD, 13f));
    }
}