using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ec.edu.monster.vista.ui
{
    /// <summary>
    /// Avatar circular con borde doble y sombra suave.
    /// Soporta fondo transparente (pinta el fondo del padre).
    /// </summary>
    public sealed class CircularAvatar : Control
    {
        private Image? _avatarImage;

        public Image? AvatarImage
        {
            get => _avatarImage;
            set { _avatarImage = value; Invalidate(); }
        }

        public int OuterBorderSize { get; set; } = 8;            // aro blanco
        public Color OuterBorderColor { get; set; } = Color.White;

        public int InnerBorderSize { get; set; } = 2;            // aro interno tenue
        public Color InnerBorderColor { get; set; } = Color.FromArgb(215, 210, 240);

        public bool Shadow { get; set; } = true;                  // sombra inferior suave
        public int ShadowOffsetY { get; set; } = 6;
        public int ShadowBlur { get; set; } = 14;                 // “desenfoque” (simulado)
        public int ShadowAlpha { get; set; } = 70;

        public CircularAvatar()
        {
            // Habilitar estilos para dibujar manualmente y transparencia
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent; // ya no disparará la excepción
            Size = new Size(120, 120);
        }

        // Pinta “transparencia real”: renderiza el fondo y contenido del padre bajo este control
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (BackColor == Color.Transparent && Parent != null)
            {
                var g = e.Graphics;
                var state = g.Save();
                g.TranslateTransform(-Left, -Top);

                var pe = new PaintEventArgs(g, Parent.ClientRectangle);
                InvokePaintBackground(Parent, pe);
                InvokePaint(Parent, pe);

                g.Restore(state);
                return;
            }
            base.OnPaintBackground(e);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
                Parent.BackColorChanged += (_, __) => Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Rectángulos base
            var bounds = ClientRectangle;
            if (bounds.Width < 2 || bounds.Height < 2) return;

            // Sombra (debajo)
            if (Shadow)
            {
                // Rect de sombra: un poco más pequeño y desplazado hacia abajo
                var shadowRect = Rectangle.Inflate(bounds, -OuterBorderSize - 6, -OuterBorderSize - 6);
                shadowRect.Offset(0, ShadowOffsetY);

                using var path = new GraphicsPath();
                path.AddEllipse(shadowRect);

                // Sombra suave simulada con varias elipses concéntricas
                for (int i = ShadowBlur; i >= 1; i--)
                {
                    int a = Math.Max(0, (int)(ShadowAlpha * (i / (float)ShadowBlur)));
                    using var br = new SolidBrush(Color.FromArgb(a, 0, 0, 0));
                    var r = Rectangle.Inflate(shadowRect, i, i);
                    g.FillEllipse(br, r);
                }
            }

            // Área para imagen (descontando bordes)
            int totalBorder = OuterBorderSize + InnerBorderSize + 2;
            var imageRect = Rectangle.Inflate(bounds, -totalBorder, -totalBorder);

            // Clip circular y dibujo de imagen
            using (var clipPath = new GraphicsPath())
            {
                clipPath.AddEllipse(imageRect);
                var oldClip = g.Clip; // guardar clip
                g.SetClip(clipPath);

                if (AvatarImage != null)
                {
                    // Ajuste manteniendo proporción dentro del círculo
                    var img = AvatarImage;
                    Rectangle dest = imageRect;

                    float ratioImg = img.Width / (float)img.Height;
                    float ratioDest = dest.Width / (float)dest.Height;

                    if (ratioImg > ratioDest)
                    {
                        // imagen más ancha: ajusta por alto
                        int w = (int)(dest.Height * ratioImg);
                        int x = dest.X + (dest.Width - w) / 2;
                        dest = new Rectangle(x, dest.Y, w, dest.Height);
                    }
                    else
                    {
                        // imagen más alta: ajusta por ancho
                        int h = (int)(dest.Width / ratioImg);
                        int y = dest.Y + (dest.Height - h) / 2;
                        dest = new Rectangle(dest.X, y, dest.Width, h);
                    }

                    g.DrawImage(img, dest);
                }
                else
                {
                    // Placeholder si no hay imagen
                    using var br = new LinearGradientBrush(imageRect,
                        Color.FromArgb(130, 160, 255),
                        Color.FromArgb(180, 120, 255),
                        LinearGradientMode.ForwardDiagonal);
                    g.FillEllipse(br, imageRect);

                    using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    using var f = new Font("Segoe UI", Math.Max(12, imageRect.Width / 4f), FontStyle.Bold);
                    using var txtBr = new SolidBrush(Color.White);
                    g.DrawString("M", f, txtBr, imageRect, sf);
                }

                // restaurar clip
                g.SetClip(oldClip, CombineMode.Replace);
                oldClip?.Dispose();
            }

            // Borde interno tenue
            if (InnerBorderSize > 0)
            {
                var innerRect = Rectangle.Inflate(imageRect, InnerBorderSize / 2, InnerBorderSize / 2);
                using var penInner = new Pen(InnerBorderColor, InnerBorderSize);
                g.DrawEllipse(penInner, innerRect);
            }

            // Borde blanco externo
            if (OuterBorderSize > 0)
            {
                var outerRect = Rectangle.Inflate(bounds, -(OuterBorderSize / 2) - 1, -(OuterBorderSize / 2) - 1);
                using var penOuter = new Pen(OuterBorderColor, OuterBorderSize);
                g.DrawEllipse(penOuter, outerRect);
            }
        }
    }
}
