using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JBToolkit.WinForms
{
    /// <summary>
    /// Nicer looking 'round' edge button
    /// </summary>
    [Description("Text")]
    public sealed class RoundButton : Control, IButtonControl
    {
        #region AltoButton

        public RoundButton()
        {
            Width = 65;
            Height = 30;

            stroke = true;
            strokeColor = Color.Gray;

            // grey

            //inactive1 = Color.FromArgb(224, 224, 224);
            //inactive2 = Color.FromArgb(224, 224, 224);
            //active1 = Color.FromArgb(192, 192, 192);
            //active2 = Color.FromArgb(192, 192, 192);

            // blue

            inactive1 = Color.FromArgb(215, 228, 242);
            inactive2 = Color.FromArgb(185, 209, 234);
            active1 = Color.FromArgb(135, 206, 235);
            active2 = Color.FromArgb(215, 228, 242);

            strokeColor = Color.FromArgb(215, 228, 242); ;

            radius = 4;
            roundedRect = new RoundedRectangleF(Width, Height, radius);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
            ForeColor = Color.Black;
            Font = new Font("Segoe UI", 8, FontStyle.Bold);
            state = MouseState.Leave;
            Transparency = false;
        }

        #endregion

        #region Variables

        private int radius;
        private MouseState state;
        private RoundedRectangleF roundedRect;
        private Color inactive1, inactive2, active1, active2;
        private Color strokeColor;
        private bool stroke;

        public bool Stroke
        {
            get { return stroke; }
            set
            {
                stroke = value;
                Invalidate();
            }
        }

        public Color StrokeColor
        {
            get { return strokeColor; }
            set
            {
                strokeColor = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        protected override void OnPaint(PaintEventArgs e)
        {
            #region Transparency

            if (Transparency)
            {
                ColourHelper.MakeTransparent(this, e.Graphics);
            }

            #endregion

            #region Drawing

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            roundedRect = new RoundedRectangleF(Width, Height, radius);
            e.Graphics.FillRectangle(Brushes.Transparent, ClientRectangle);

            int R1 = (active1.R + inactive1.R) / 2;
            int G1 = (active1.G + inactive1.G) / 2;
            int B1 = (active1.B + inactive1.B) / 2;

            int R2 = (active2.R + inactive2.R) / 2;
            int G2 = (active2.G + inactive2.G) / 2;
            int B2 = (active2.B + inactive2.B) / 2;

            var rect = new Rectangle(0, 0, Width, Height);

            if (Enabled)
            {
                if (state == MouseState.Leave)
                {
                    using (var inactiveGB = new LinearGradientBrush(rect, inactive1, inactive2, 90f))
                    {
                        e.Graphics.FillPath(inactiveGB, roundedRect.Path);
                    }
                }
                else if (state == MouseState.Enter)
                {
                    using (var activeGB = new LinearGradientBrush(rect, active1, active2, 90f))
                    {
                        e.Graphics.FillPath(activeGB, roundedRect.Path);
                    }
                }
                else if (state == MouseState.Down)
                {
                    using (var downGB =
                        new LinearGradientBrush(rect, Color.FromArgb(R1, G1, B1), Color.FromArgb(R2, G2, B2), 90f))
                    {
                        e.Graphics.FillPath(downGB, roundedRect.Path);
                    }
                }

                if (stroke)
                {
                    using (var pen = new Pen(strokeColor, 1))
                    using (var path = new RoundedRectangleF(Width - (radius > 0 ? 0 : 1), Height - (radius > 0 ? 0 : 1),
                        radius).Path)
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }
            else
            {
                var linear1 = Color.FromArgb(224, 224, 224);
                var linear2 = Color.FromArgb(224, 224, 224);

                using (var inactiveGB = new LinearGradientBrush(rect, linear1, linear2, 90f))
                {
                    e.Graphics.FillPath(inactiveGB, roundedRect.Path);
                    e.Graphics.DrawPath(new Pen(inactiveGB), roundedRect.Path);
                }
            }

            #endregion

            #region Text Drawing

            using (var sf = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            })
            using (Brush brush = new SolidBrush(ForeColor))
            {

                if (this.Image == null)
                {
                    e.Graphics.DrawString(Text, Font, brush, ClientRectangle, sf);
                }
                else
                {
                    e.Graphics.DrawString(Text, Font, brush, ClientRectangle.Width / 2 + (Image.Width / 2), ClientRectangle.Height / 2 + 1, sf);
                    e.Graphics.DrawImage(Image, 8, 6, Image.Width, Image.Height);
                }
            }

            #endregion

            #region Background Image Drawing

            if (BackgroundImage != null)
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.InterpolationMode = InterpolationMode.Low;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                using (var sf = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                })
                using (Brush brush = new SolidBrush(ForeColor))
                {
                    e.Graphics.DrawImage(BackgroundImage,
                         (roundedRect.Rect.Size.Width - BackgroundImage.Size.Width) / 2 + 1,
                         (roundedRect.Rect.Size.Height - BackgroundImage.Size.Height) / 2 + 1
                        );
                }
            }

            #endregion

            base.OnPaint(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            OnClick(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate();
            base.OnResize(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Cursor = Cursors.Hand;

            state = MouseState.Enter;
            base.OnMouseEnter(e);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Cursor = Cursors.Default;

            state = MouseState.Leave;
            base.OnMouseLeave(e);
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Capture = false;
            state = MouseState.Down;
            base.OnMouseDown(e);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (state != MouseState.Leave)
            {
                state = MouseState.Enter;
            }

            base.OnMouseUp(e);
            Invalidate();
        }

        #endregion

        #region Properties

        public int Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                Invalidate();
            }
        }

        public Color Inactive1
        {
            get { return inactive1; }
            set
            {
                inactive1 = value;
                Invalidate();
            }
        }

        public Color Inactive2
        {
            get { return inactive2; }
            set
            {
                inactive2 = value;
                Invalidate();
            }
        }

        public Color Active1
        {
            get { return active1; }
            set
            {
                active1 = value;
                Invalidate();
            }
        }

        public Color Active2
        {
            get { return active2; }
            set
            {
                active2 = value;
                Invalidate();
            }
        }

        public bool Transparency { get; set; }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }

            set
            {
                base.BackgroundImage = value;
                Invalidate();
            }
        }

        public Image Image { get; set; }


        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                Invalidate();
            }
        }

        public DialogResult DialogResult
        {
            get { return DialogResult.OK; }
            set { }
        }

        public void NotifyDefault(bool value)
        {
        }

        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }

        #endregion
    }

    public enum MouseState
    {
        Enter,
        Leave,
        Down,
        Up
    }
}