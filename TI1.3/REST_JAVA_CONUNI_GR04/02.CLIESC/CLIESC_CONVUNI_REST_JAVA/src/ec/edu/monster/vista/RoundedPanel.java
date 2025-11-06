package ec.edu.monster.vista;

import java.awt.*;
import javax.swing.*;

/** Card con esquinas grandes, sombra suave y soporte de fondo con degradado. */
public class RoundedPanel extends JPanel {
    private int cornerRadius = 28;
    private int shadowX = 10, shadowY = 12;
    private int shadowBlurAlpha = 55;

    private boolean gradient = false;
    private Color gStart = UITheme.CARD_A, gEnd = UITheme.CARD_B;

    public RoundedPanel(int cornerRadius) {
        setOpaque(false);
        this.cornerRadius = cornerRadius;
        setBackground(Color.WHITE);
    }

    public void setGradient(Color start, Color end) {
        this.gradient = true;
        this.gStart = start;
        this.gEnd = end;
        repaint();
    }

    @Override
    protected void paintComponent(Graphics g) {
        int w = getWidth(), h = getHeight();

        Graphics2D g2 = (Graphics2D) g.create();
        g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

        // Sombra (simulada)
        g2.setColor(new Color(30, 20, 80, shadowBlurAlpha));
        g2.fillRoundRect(shadowX, shadowY, w - shadowX * 2 + 8, h - shadowY * 2 + 8, cornerRadius + 8, cornerRadius + 8);

        // Fondo de la card
        if (gradient) {
            g2.setPaint(new GradientPaint(0, 0, gStart, 0, h, gEnd));
        } else {
            g2.setPaint(getBackground());
        }
        g2.fillRoundRect(0, 0, w - shadowX, h - shadowY, cornerRadius, cornerRadius);

        // Luz suave superior (glass)
        if (gradient) {
            g2.setPaint(new GradientPaint(0, 0, new Color(255, 255, 255, 100),
                    0, Math.max(60, h/3), new Color(255, 255, 255, 0)));
            g2.fillRoundRect(8, 8, w - shadowX - 16, Math.max(50, h/4), cornerRadius - 8, cornerRadius - 8);
        }

        g2.dispose();
        super.paintComponent(g);
    }

    @Override
    public Insets getInsets() {
        return new Insets(22, 22, 26, 32);
    }
}
