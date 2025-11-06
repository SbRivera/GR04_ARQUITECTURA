using System.Drawing.Drawing2D;

internal class RoundedBox : Panel
{
    public int Radius { get; set; } = 12;

    public RoundedBox()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.UserPaint |
                 ControlStyles.ResizeRedraw, true);
        UpdateStyles();
        BackColor = Color.Transparent;
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        using var gp = Round(ClientRectangle, Radius);
        Region = new Region(gp);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var r = ClientRectangle;
        using var gp = Round(r, Radius);
        using var br = new SolidBrush(Color.White);
        using var pen = new Pen(Color.FromArgb(200, 210, 240), 1.6f);

        e.Graphics.FillPath(br, gp);
        e.Graphics.DrawPath(pen, gp);
    }

    private static GraphicsPath Round(Rectangle rect, int radius)
    {
        int d = radius * 2;
        var gp = new GraphicsPath();
        gp.AddArc(rect.X, rect.Y, d, d, 180, 90);
        gp.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        gp.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        gp.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        gp.CloseFigure();
        return gp;
    }
}
