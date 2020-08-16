using System.Collections.Generic;

namespace System.Drawing.Html
{
    /// <summary>
    ///     Helps on CSS Layout
    /// </summary>
    internal static class CssLayoutEngine
    {
        #region Fields

        #endregion

        #region Inline Boxes

        /// <summary>
        ///     Creates line boxes for the specified blockbox
        /// </summary>
        /// <param name="g"></param>
        /// <param name="blockBox"></param>
        public static void CreateLineBoxes(Graphics g, CssBox blockBox)
        {
            blockBox.LineBoxes.Clear();

            var maxRight = blockBox.ActualRight - blockBox.ActualPaddingRight - blockBox.ActualBorderRightWidth;

            //Get the start x and y of the blockBox
            var startx =
                blockBox.Location.X + blockBox.ActualPaddingLeft - 0 +
                blockBox.ActualBorderLeftWidth;
            var starty = blockBox.Location.Y + blockBox.ActualPaddingTop - 0 + blockBox.ActualBorderTopWidth;
            var curx = startx + blockBox.ActualTextIndent;
            var cury = starty;

            //Reminds the maximum bottom reached
            var maxBottom = starty;

            //Extra amount of spacing that should be applied to lines when breaking them.
            var lineSpacing = 0f;

            //First line box
            var line = new CssLineBox(blockBox);

            //Flow words and boxes
            FlowBox(g, blockBox, blockBox, maxRight, lineSpacing, startx, ref line, ref curx, ref cury, ref maxBottom);

            //Gets the rectangles foreach linebox
            foreach (var linebox in blockBox.LineBoxes)
            {
                BubbleRectangles(blockBox, linebox);
                linebox.AssignRectanglesToBoxes();
                ApplyAlignment(g, linebox);
                if (blockBox.Direction == CssConstants.Rtl)
                {
                    ApplyRightToLeft(linebox);
                }

                //linebox.DrawRectangles(g);
            }

            blockBox.ActualBottom = maxBottom + blockBox.ActualPaddingBottom + blockBox.ActualBorderBottomWidth;
        }

        /// <summary>
        ///     Recursively flows the content of the box using the inline model
        /// </summary>
        /// <param name="g">Device Info</param>
        /// <param name="blockbox">Blockbox that contains the text flow</param>
        /// <param name="box">Current box to flow its content</param>
        /// <param name="maxright">Maximum reached right</param>
        /// <param name="linespacing">Space to use between rows of text</param>
        /// <param name="startx">x starting coordinate for when breaking lines of text</param>
        /// <param name="line">Current linebox being used</param>
        /// <param name="curx">Current x coordinate that will be the left of the next word</param>
        /// <param name="cury">Current y coordinate that will be the top of the next word</param>
        /// <param name="maxbottom">Maximum bottom reached so far</param>
        private static void FlowBox(Graphics g, CssBox blockbox, CssBox box, float maxright, float linespacing,
            float startx, ref CssLineBox line, ref float curx, ref float cury, ref float maxbottom)
        {
            box.FirstHostingLineBox = line;

            foreach (var b in box.Boxes)
            {
                var leftspacing = b.ActualMarginLeft + b.ActualBorderLeftWidth + b.ActualPaddingLeft;
                var rightspacing = b.ActualMarginRight + b.ActualBorderRightWidth + b.ActualPaddingRight;

                b.RectanglesReset();
                b.MeasureWordsSize(g);

                curx += leftspacing;

                if (b.Words.Count > 0)
                {
                    #region Flow words

                    foreach (var word in b.Words)
                    {
                        //curx += word.SpacesBeforeWidth;

                        if (b.WhiteSpace != CssConstants.Nowrap && curx + word.Width + rightspacing > maxright ||
                            word.IsLineBreak)
                        {
                            #region Break line

                            curx = startx;
                            cury = maxbottom + linespacing;

                            line = new CssLineBox(blockbox);

                            if (word.IsImage || word.Equals(b.FirstWord))
                            {
                                curx += leftspacing;
                            }

                            #endregion
                        }

                        line.ReportExistanceOf(word);

                        word.Left = curx; // -word.LastMeasureOffset.X + 1;
                        word.Top = cury; // - word.LastMeasureOffset.Y;

                        curx = word.Right; // +word.SpacesAfterWidth;
                        maxbottom = Math.Max(maxbottom,
                            word.Bottom); //+ (word.IsImage ? topspacing + bottomspacing : 0));
                    }

                    #endregion
                }
                else
                {
                    FlowBox(g, blockbox, b, maxright, linespacing, startx, ref line, ref curx, ref cury, ref maxbottom);
                }

                curx += rightspacing;
            }

            box.LastHostingLineBox = line;
        }

        /// <summary>
        ///     Recursively creates the rectangles of the blockBox, by bubbling from deep to outside of the boxes
        ///     in the rectangle structure
        /// </summary>
        private static void BubbleRectangles(CssBox box, CssLineBox line)
        {
            if (box.Words.Count > 0)
            {
                float x = float.MaxValue, y = float.MaxValue, r = float.MinValue, b = float.MinValue;
                var words = line.WordsOf(box);

                if (words.Count > 0)
                {
                    foreach (var word in words)
                    {
                        x = Math.Min(x, word.Left); // - word.SpacesBeforeWidth);
                        r = Math.Max(r, word.Right); // + word.SpacesAfterWidth);
                        y = Math.Min(y, word.Top);
                        b = Math.Max(b, word.Bottom);
                    }
                    line.UpdateRectangle(box, x, y, r, b);
                }
            }
            else
            {
                foreach (var b in box.Boxes)
                {
                    BubbleRectangles(b, line);
                }
            }
        }

        /// <summary>
        ///     Gets the white space width of the specified box
        /// </summary>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float WhiteSpace(Graphics g, CssBox b)
        {
            var space = " .";
            var w = 0f;
            var onError = 5f;

            var sf = new StringFormat();
            sf.SetMeasurableCharacterRanges(new[] { new CharacterRange(0, 1) });
            var regs = g.MeasureCharacterRanges(space, b.ActualFont,
                new RectangleF(0, 0, float.MaxValue, float.MaxValue), sf);

            if (regs.Length == 0)
            {
                return onError;
            }

            w = regs[0].GetBounds(g).Width;

            if (!(string.IsNullOrEmpty(b.WordSpacing) || b.WordSpacing == CssConstants.Normal))
            {
                w += CssValue.ParseLength(b.WordSpacing, 0, b);
            }

            return w;
        }

        /// <summary>
        ///     Applies vertical and horizontal alignment to words in lineboxes
        /// </summary>
        /// <param name="g"></param>
        /// <param name="lineBox"></param>
        private static void ApplyAlignment(Graphics g, CssLineBox lineBox)
        {
            #region Horizontal alignment

            switch (lineBox.OwnerBox.TextAlign)
            {
                case CssConstants.Right:
                    ApplyRightAlignment(lineBox);
                    break;
                case CssConstants.Center:
                    ApplyCenterAlignment(lineBox);
                    break;
                case CssConstants.Justify:
                    ApplyJustifyAlignment(lineBox);
                    break;
                default:
                    ApplyLeftAlignment();
                    break;
            }

            #endregion

            ApplyVerticalAlignment(g, lineBox);
        }

        /// <summary>
        ///     Applies right to left direction to words
        /// </summary>
        /// <param name="line"></param>
        private static void ApplyRightToLeft(CssLineBox line)
        {
            var left = line.OwnerBox.ClientLeft;
            var right = line.OwnerBox.ClientRight;

            foreach (var word in line.Words)
            {
                var diff = word.Left - left;
                var wright = right - diff;
                word.Left = wright - word.Width;
            }
        }

        /// <summary>
        ///     Gets the ascent of the font
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Font metrics from http://msdn.microsoft.com/en-us/library/xwf9s90b(VS.71).aspx
        /// </remarks>
        public static float GetAscent(Font f)
        {
            var mainAscent = f.Size * f.FontFamily.GetCellAscent(f.Style) / f.FontFamily.GetEmHeight(f.Style);
            return mainAscent;
        }

        /// <summary>
        ///     Gets the descent of the font
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Font metrics from http://msdn.microsoft.com/en-us/library/xwf9s90b(VS.71).aspx
        /// </remarks>
        public static float GetDescent(Font f)
        {
            var mainDescent = f.Size * f.FontFamily.GetCellDescent(f.Style) / f.FontFamily.GetEmHeight(f.Style);
            return mainDescent;
        }

        /// <summary>
        ///     Gets the line spacing of the font
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Font metrics from http://msdn.microsoft.com/en-us/library/xwf9s90b(VS.71).aspx
        /// </remarks>
        public static float GetLineSpacing(Font f)
        {
            var s = f.Size * f.FontFamily.GetLineSpacing(f.Style) / f.FontFamily.GetEmHeight(f.Style);
            return s;
        }

        /// <summary>
        ///     Applies vertical alignment to the linebox
        /// </summary>
        /// <param name="g"></param>
        /// <param name="lineBox"></param>
        private static void ApplyVerticalAlignment(Graphics g, CssLineBox lineBox)
        {
            var baseline = lineBox.GetMaxWordBottom() - GetDescent(lineBox.OwnerBox.ActualFont) - 2;
            var boxes = new List<CssBox>(lineBox.Rectangles.Keys);

            foreach (var b in boxes)
            {
                GetAscent(b.ActualFont);
                GetDescent(b.ActualFont);

                //Important notes on http://www.w3.org/TR/CSS21/tables.html#height-layout
                switch (b.VerticalAlign)
                {
                    case CssConstants.Sub:
                        lineBox.SetBaseLine(g, b, baseline + lineBox.Rectangles[b].Height * .2f);
                        break;
                    case CssConstants.Super:
                        lineBox.SetBaseLine(g, b, baseline - lineBox.Rectangles[b].Height * .2f);
                        break;
                    case CssConstants.TextTop:

                        break;
                    case CssConstants.TextBottom:

                        break;
                    case CssConstants.Top:

                        break;
                    case CssConstants.Bottom:

                        break;
                    case CssConstants.Middle:

                        break;
                    default:
                        //case: baseline
                        lineBox.SetBaseLine(g, b, baseline);
                        break;
                }

                ////Graphic cues
                //g.FillRectangle(Brushes.Aqua, r.Left, r.Top, r.Width, ascent);
                //g.FillRectangle(Brushes.Yellow, r.Left, r.Top + ascent, r.Width, descent);
                //g.DrawLine(Pens.Fuchsia, r.Left, baseline, r.Right, baseline);
            }
        }

        /// <summary>
        ///     Applies special vertical alignment for table-cells
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cell"></param>
        public static void ApplyCellVerticalAlignment(Graphics g, CssBox cell)
        {
            if (cell.VerticalAlign == CssConstants.Top || cell.VerticalAlign == CssConstants.Baseline)
            {
                return;
            }

            var cellbot = cell.ClientBottom;
            var bottom = cell.GetMaximumBottom(cell, 0f);
            var dist = 0f;

            if (cell.VerticalAlign == CssConstants.Bottom)
            {
                dist = cellbot - bottom;
            }
            else if (cell.VerticalAlign == CssConstants.Middle)
            {
                dist = (cellbot - bottom) / 2;
            }

            foreach (var b in cell.Boxes)
            {
                b.OffsetTop(dist);
            }

            //float top = cell.ClientTop;
            //float bottom = cell.ClientBottom;
            //bool middle = cell.VerticalAlign == CssConstants.Middle;

            //foreach (LineBox line in cell.LineBoxes)
            //{
            //    for (int i = 0; i < line.RelatedBoxes.Count; i++)
            //    {

            //        float diff = bottom - line.RelatedBoxes[i].Rectangles[line].Bottom;
            //        if (middle) diff /= 2f;
            //        RectangleF r = line.RelatedBoxes[i].Rectangles[line];
            //        line.RelatedBoxes[i].Rectangles[line] = new RectangleF(r.X, r.Y + diff, r.Width, r.Height);

            //    }

            //    foreach (BoxWord word in line.Words)
            //    {
            //        float gap = word.Top - top;
            //        word.Top = bottom - gap - word.Height;
            //    }
            //}
        }

        /// <summary>
        ///     Applies centered alignment to the text on the linebox
        /// </summary>
        /// <param name="lineBox"></param>
        private static void ApplyJustifyAlignment(CssLineBox lineBox)
        {
            if (lineBox.Equals(lineBox.OwnerBox.LineBoxes[lineBox.OwnerBox.LineBoxes.Count - 1]))
            {
                return;
            }

            var indent = lineBox.Equals(lineBox.OwnerBox.LineBoxes[0]) ? lineBox.OwnerBox.ActualTextIndent : 0f;
            var textSum = 0f;
            var words = 0f;
            var availWidth = lineBox.OwnerBox.ClientRectangle.Width - indent;

            #region Gather text sum

            foreach (var w in lineBox.Words)
            {
                textSum += w.Width;
                words += 1f;
            }

            #endregion

            if (words <= 0f)
            {
                return; //Avoid Zero division
            }

            var spacing = (availWidth - textSum) / words; //Spacing that will be used
            var curx = lineBox.OwnerBox.ClientLeft + indent;

            foreach (var word in lineBox.Words)
            {
                word.Left = curx;
                curx = word.Right + spacing;

                if (word == lineBox.Words[lineBox.Words.Count - 1])
                {
                    word.Left = lineBox.OwnerBox.ClientRight - word.Width;
                }
            }
        }

        /// <summary>
        ///     Applies centered alignment to the text on the linebox
        /// </summary>
        private static void ApplyCenterAlignment(CssLineBox line)
        {
            if (line.Words.Count == 0)
            {
                return;
            }

            var lastWord = line.Words[line.Words.Count - 1];
            var right = line.OwnerBox.ActualRight - line.OwnerBox.ActualPaddingRight -
                        line.OwnerBox.ActualBorderRightWidth;
            var diff = right - lastWord.Right - lastWord.LastMeasureOffset.X -
                       lastWord.OwnerBox.ActualBorderRightWidth - lastWord.OwnerBox.ActualPaddingRight;
            diff /= 2;

            if (diff <= 0)
            {
                return;
            }

            foreach (var word in line.Words)
            {
                word.Left += diff;
            }

            foreach (var b in line.Rectangles.Keys)
            {
                var r = b.Rectangles[line];
                b.Rectangles[line] = new RectangleF(r.X + diff, r.Y, r.Width, r.Height);
            }
        }

        /// <summary>
        ///     Applies right alignment to the text on the linebox
        /// </summary>
        private static void ApplyRightAlignment(CssLineBox line)
        {
            if (line.Words.Count == 0)
            {
                return;
            }

            var lastWord = line.Words[line.Words.Count - 1];
            var right = line.OwnerBox.ActualRight - line.OwnerBox.ActualPaddingRight -
                        line.OwnerBox.ActualBorderRightWidth;
            var diff = right - lastWord.Right - lastWord.LastMeasureOffset.X -
                       lastWord.OwnerBox.ActualBorderRightWidth - lastWord.OwnerBox.ActualPaddingRight;


            if (diff <= 0)
            {
                return;
            }

            //if (line.OwnerBox.Direction == CssConstants.Rtl)
            //{

            //}

            foreach (var word in line.Words)
            {
                word.Left += diff;
            }

            foreach (var b in line.Rectangles.Keys)
            {
                var r = b.Rectangles[line];
                b.Rectangles[line] = new RectangleF(r.X + diff, r.Y, r.Width, r.Height);
            }
        }

        /// <summary>
        ///     Simplest alignment, just arrange words.
        /// </summary>
        private static void ApplyLeftAlignment()
        {
            //No alignment needed.

            //foreach (LineBoxRectangle r in line.Rectangles)
            //{
            //    float curx = r.Left + (r.Index == 0 ? r.OwnerBox.ActualPaddingLeft + r.OwnerBox.ActualBorderLeftWidth / 2 : 0);

            //    if (r.SpaceBefore) curx += r.OwnerBox.ActualWordSpacing;

            //    foreach (BoxWord word in r.Words)
            //    {
            //        word.Left = curx;
            //        word.Top = r.Top;// +r.OwnerBox.ActualPaddingTop + r.OwnerBox.ActualBorderTopWidth / 2;

            //        curx = word.Right + r.OwnerBox.ActualWordSpacing;
            //    }
            //}
        }

        #endregion
    }
}