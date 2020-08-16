using System.Collections.Generic;

namespace System.Drawing.Html
{
    /// <summary>
    ///     Represents a line of text.
    /// </summary>
    /// <remarks>
    ///     To learn more about line-boxes see CSS spec:
    ///     http://www.w3.org/TR/CSS21/visuren.html
    /// </remarks>
    internal class CssLineBox
    {
        #region Ctors

        /// <summary>
        ///     Creates a new LineBox
        /// </summary>
        public CssLineBox(CssBox ownerBox)
        {
            Rectangles = new Dictionary<CssBox, RectangleF>();
            RelatedBoxes = new List<CssBox>();
            Words = new List<CssBoxWord>();
            OwnerBox = ownerBox;
            OwnerBox.LineBoxes.Add(this);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the maximum bottom of the words
        /// </summary>
        /// <returns></returns>
        public float GetMaxWordBottom()
        {
            var res = float.MinValue;

            foreach (var word in Words)
            {
                res = Math.Max(res, word.Bottom);
            }

            return res;
        }

        #endregion

        /// <summary>
        ///     Lets the linebox add the word an its box to their lists if necessary.
        /// </summary>
        /// <param name="word"></param>
        internal void ReportExistanceOf(CssBoxWord word)
        {
            if (!Words.Contains(word))
            {
                Words.Add(word);
            }

            if (!RelatedBoxes.Contains(word.OwnerBox))
            {
                RelatedBoxes.Add(word.OwnerBox);
            }
        }

        /// <summary>
        ///     Return the words of the specified box that live in this linebox
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        internal List<CssBoxWord> WordsOf(CssBox box)
        {
            var r = new List<CssBoxWord>();

            foreach (var word in Words)
            {
                if (word.OwnerBox.Equals(box))
                {
                    r.Add(word);
                }
            }

            return r;
        }

        /// <summary>
        ///     Updates the specified rectangle of the specified box.
        /// </summary>
        /// <param name="box"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <param name="b"></param>
        internal void UpdateRectangle(CssBox box, float x, float y, float r, float b)
        {
            var leftspacing = box.ActualBorderLeftWidth + box.ActualPaddingLeft;
            var rightspacing = box.ActualBorderRightWidth + box.ActualPaddingRight;
            var topspacing = box.ActualBorderTopWidth + box.ActualPaddingTop;
            var bottomspacing = box.ActualBorderBottomWidth + box.ActualPaddingTop;

            if (box.FirstHostingLineBox != null && box.FirstHostingLineBox.Equals(this) || box.IsImage)
            {
                x -= leftspacing;
            }

            if (box.LastHostingLineBox != null && box.LastHostingLineBox.Equals(this) || box.IsImage)
            {
                r += rightspacing;
            }

            if (!box.IsImage)
            {
                y -= topspacing;
                b += bottomspacing;
            }


            if (!Rectangles.ContainsKey(box))
            {
                Rectangles.Add(box, RectangleF.FromLTRB(x, y, r, b));
            }
            else
            {
                var f = Rectangles[box];
                Rectangles[box] = RectangleF.FromLTRB(
                    Math.Min(f.X, x), Math.Min(f.Y, y),
                    Math.Max(f.Right, r), Math.Max(f.Bottom, b));
            }

            if (box.ParentBox != null && box.ParentBox.Display == CssConstants.Inline)
            {
                UpdateRectangle(box.ParentBox, x, y, r, b);
            }
        }

        /// <summary>
        ///     Copies the rectangles to their specified box
        /// </summary>
        internal void AssignRectanglesToBoxes()
        {
            foreach (var b in Rectangles.Keys)
            {
                b.Rectangles.Add(this, Rectangles[b]);
            }
        }

        /// <summary>
        ///     Draws the rectangles for debug purposes
        /// </summary>
        /// <param name="g"></param>
        internal void DrawRectangles(Graphics g)
        {
            foreach (var b in Rectangles.Keys)
            {
                if (float.IsInfinity(Rectangles[b].Width))
                {
                    continue;
                }

                g.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.Black)),
                    Rectangle.Round(Rectangles[b]));
                g.DrawRectangle(Pens.Red, Rectangle.Round(Rectangles[b]));
            }
        }

        /// <summary>
        ///     Gets the baseline Height of the rectangle
        /// </summary>
        /// <param name="b"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public float GetBaseLineHeight(CssBox b, Graphics g)
        {
            var f = b.ActualFont;
            var ff = f.FontFamily;
            var s = f.Style;
            return f.GetHeight(g) * ff.GetCellAscent(s) / ff.GetLineSpacing(s);
        }

        /// <summary>
        ///     Sets the baseline of the words of the specified box to certain height
        /// </summary>
        /// <param name="g">Device info</param>
        /// <param name="b">box to check words</param>
        /// <param name="baseline">baseline</param>
        internal void SetBaseLine(Graphics g, CssBox b, float baseline)
        {
            var ws = WordsOf(b);

            if (!Rectangles.ContainsKey(b))
            {
                return;
            }

            var r = Rectangles[b];

            //Save top of words related to the top of rectangle
            var gap = 0f;

            if (ws.Count > 0)
            {
                gap = ws[0].Top - r.Top;
            }
            else
            {
                var firstw = b.FirstWordOccourence(b, this);

                if (firstw != null)
                {
                    gap = firstw.Top - r.Top;
                }
            }

            //New top that words will have
            //float newtop = baseline - (Height - OwnerBox.FontDescent - 3); //OLD
            var newtop = baseline - GetBaseLineHeight(b, g); //OLD

            if (b.ParentBox != null &&
                b.ParentBox.Rectangles.ContainsKey(this) &&
                r.Height < b.ParentBox.Rectangles[this].Height)
            {
                //Do this only if rectangle is shorter than parent's
                var recttop = newtop - gap;
                var newr = new RectangleF(r.X, recttop, r.Width, r.Height);
                Rectangles[b] = newr;
                b.OffsetRectangle(this, gap);
            }
            foreach (var w in ws)
            {
                if (!w.IsImage)
                {
                    w.Top = newtop;
                }
            }
        }

        /// <summary>
        ///     Returns the words of the linebox
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var ws = new string[Words.Count];
            for (var i = 0; i < ws.Length; i++)
            {
                ws[i] = Words[i].Text;
            }

            return string.Join(" ", ws);
        }

        #region Fields

        #endregion

        #region Props

        /// <summary>
        ///     Gets a list of boxes related with the linebox.
        ///     To know the words of the box inside this linebox, use the <see cref="WordsOf" /> method.
        /// </summary>
        public List<CssBox> RelatedBoxes { get; }


        /// <summary>
        ///     Gets the words inside the linebox
        /// </summary>
        public List<CssBoxWord> Words { get; }

        /// <summary>
        ///     Gets the owner box
        /// </summary>
        public CssBox OwnerBox { get; }

        /// <summary>
        ///     Gets a List of rectangles that are to be painted on this linebox
        /// </summary>
        public Dictionary<CssBox, RectangleF> Rectangles { get; }

        #endregion
    }
}