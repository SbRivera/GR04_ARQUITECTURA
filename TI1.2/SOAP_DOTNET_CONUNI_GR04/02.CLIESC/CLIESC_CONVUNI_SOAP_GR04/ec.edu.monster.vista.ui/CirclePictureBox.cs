using System.Drawing.Drawing2D;

internal class CirclePictureBox : PictureBox
{
    public int BorderSize { get; set; } = 6;
    public Color BorderColor { get; set; } = Color.White;
    public bool Shadow { get; set; } = true;

    public CirclePictureBox()
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
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var r = ClientRectangle;

        // región del control circular
        using (var outer = new GraphicsPath())
        {
            outer.AddEllipse(r);
            Region = new Region(outer);
        }

        // sombra suave
        if (Shadow)
        {
            var sh = r; sh.Offset(0, 6); sh.Inflate(-4, -4);
            using var sb = new SolidBrush(Color.FromArgb(45, 0, 0, 0));
            e.Graphics.FillEllipse(sb, sh);
        }

        // anillo blanco
        var ring = r; ring.Inflate(-4, -4);
        using (var pen = new Pen(BorderColor, BorderSize))
            e.Graphics.DrawEllipse(pen, ring);

        // clip interior para la imagen (dentro del anillo)
        var imgRect = r; imgRect.Inflate(-(BorderSize + 6), -(BorderSize + 6));
        using var gp = new GraphicsPath();
        gp.AddEllipse(imgRect);

        var state = e.Graphics.Save();
        e.Graphics.SetClip(gp, CombineMode.Replace);
        base.OnPaint(e);          // pinta la imagen con el SizeMode que tengas
        e.Graphics.Restore(state);
    }
}
