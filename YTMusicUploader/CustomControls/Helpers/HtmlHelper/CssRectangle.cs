namespace System.Drawing.Html
{
    public class CssRectangle
    {
        #region Fields

        #endregion

        #region Props

        /// <summary>
        ///     Left of the rectangle
        /// </summary>
        public float Left { get; set; }

        /// <summary>
        ///     Top of the rectangle
        /// </summary>
        public float Top { get; set; }

        /// <summary>
        ///     Width of the rectangle
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        ///     Height of the rectangle
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        ///     Gets or sets the right of the rectangle. When setting, it only affects the Width of the rectangle.
        /// </summary>
        public float Right
        {
            get { return Bounds.Right; }
            set { Width = value - Left; }
        }

        /// <summary>
        ///     Gets or sets the bottom of the rectangle. When setting, it only affects the Height of the rectangle.
        /// </summary>
        public float Bottom
        {
            get { return Bounds.Bottom; }
            set { Height = value - Top; }
        }

        /// <summary>
        ///     Gets or sets the bounds of the rectangle
        /// </summary>
        public RectangleF Bounds
        {
            get { return new RectangleF(Left, Top, Width, Height); }
            set
            {
                Left = value.Left;
                Top = value.Top;
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        ///     Gets or sets the location of the rectangle
        /// </summary>
        public PointF Location
        {
            get { return new PointF(Left, Top); }
            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }

        /// <summary>
        ///     Gets or sets the size of the rectangle
        /// </summary>
        public SizeF Size
        {
            get { return new SizeF(Width, Height); }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        #endregion
    }
}