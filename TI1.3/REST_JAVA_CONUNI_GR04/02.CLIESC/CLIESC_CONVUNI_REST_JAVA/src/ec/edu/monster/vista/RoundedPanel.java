package ec.edu.monster.vista;

import java.awt.*;
import javax.swing.*;

public class RoundedPanel extends JPanel {
    private final int cornerRadius;

    public RoundedPanel(int cornerRadius) {
        super();
        this.cornerRadius = cornerRadius;
        setOpaque(false);
    }

    @Override
    protected void paintComponent(Graphics g) {
        int shadowGap = 6;
        int width = getWidth();
        int height = getHeight();

        Graphics2D g2 = (Graphics2D) g.create();
        g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

        g2.setColor(new Color(0, 0, 0, 30));
        g2.fillRoundRect(4, 4, width - 1, height - 1, cornerRadius, cornerRadius);

        g2.setColor(getBackground());
        g2.fillRoundRect(0, 0, width - shadowGap, height - shadowGap, cornerRadius, cornerRadius);

        g2.dispose();
        super.paintComponent(g);
    }
}