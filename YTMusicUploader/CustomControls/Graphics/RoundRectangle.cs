using System.Drawing;
using System.Drawing.Drawing2D;

namespace JBToolkit.WinForms
{
    /// <summary>
    /// Produces a rectangle graphics path with curved edges
    /// </summary>
    public class RoundedRectangleF
    {
        private readonly float height;
        private readonly float width;
        private readonly float x;
        private readonly float y;

        /// <summary>
        /// A rectangle grpahics paths with curved edges
        /// </summary>
        public RoundedRectangleF(float width, float height, float radius, float x = 0, float y = 0)
        {
            Radius = radius;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            Path = new GraphicsPath();

            if (radius <= 0)
            {
                Path.AddRectangle(new RectangleF(x, y, width, height));
                return;
            }

            var upperLeftRect = new RectangleF(x, y, 2 * radius, 2 * radius);
            var upperRightRect = new RectangleF(width - 2 * radius - 1, x, 2 * radius, 2 * radius);
            var lowerLeftRect = new RectangleF(x, height - 2 * radius - 1, 2 * radius, 2 * radius);
            var lowerRightRect = new RectangleF(width - 2 * radius - 1, height - 2 * radius - 1, 2 * radius,
                2 * radius);

            Path.AddArc(upperLeftRect, 180, 90);
            Path.AddArc(upperRightRect, 270, 90);
            Path.AddArc(lowerRightRect, 0, 90);
            Path.AddArc(lowerLeftRect, 90, 90);
            Path.CloseAllFigures();
        }

        /// <summary>
        /// A rectangle grpahics paths with curved edges
        /// </summary>
        public RoundedRectangleF()
        {
        }

        public GraphicsPath Path { get; }

        public RectangleF Rect => new RectangleF(x, y, width, height);

        public float Radius { get; set; }
    }
}