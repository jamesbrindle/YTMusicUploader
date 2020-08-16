using System.Collections.Generic;

namespace System.Drawing.Html
{
    internal class CssTable
    {
        #region Subclasses

        /// <summary>
        ///     Used to make space on vertical cell combination
        /// </summary>
        public class SpacingBox
            : CssBox
        {
            public readonly CssBox ExtendedBox;

            public SpacingBox(CssBox tableBox, ref CssBox extendedBox, int startRow)
                : base(tableBox, new HtmlTag("<none colspan=" + extendedBox.GetAttribute("colspan", "1") + ">"))
            {
                ExtendedBox = extendedBox;
                Display = CssConstants.None;

                StartRow = startRow;
                EndRow = startRow + int.Parse(extendedBox.GetAttribute("rowspan", "1")) - 1;
            }

            #region Props

            /// <summary>
            ///     Gets the index of the row where box starts
            /// </summary>
            public int StartRow { get; }

            /// <summary>
            ///     Gets the index of the row where box ends
            /// </summary>
            public int EndRow { get; }

            #endregion
        }

        #endregion

        #region Fields

        private float[] _columnMinWidths;

        #endregion

        #region Ctor

        private CssTable()
        {
            BodyRows = new List<CssBox>();
            Columns = new List<CssBox>();
            AllRows = new List<CssBox>();
        }

        public CssTable(CssBox tableBox, Graphics g)
            : this()
        {
            if (!(tableBox.Display == CssConstants.Table || tableBox.Display == CssConstants.InlineTable))
            {
                throw new ArgumentException("Box is not a table", nameof(tableBox));
            }

            TableBox = tableBox;

            MeasureWords(tableBox, g);

            Analyze(g);
        }

        #endregion

        #region Props

        /// <summary>
        ///     Gets if the user specified a width for the table
        /// </summary>
        public bool WidthSpecified { get; private set; }

        /// <summary>
        ///     Hosts a list of all rows in the table, including those on the TFOOT, THEAD and TBODY
        /// </summary>
        public List<CssBox> AllRows { get; }

        /// <summary>
        ///     Gets the box that represents the caption of this table, if any.
        ///     WARNING: May be null
        /// </summary>
        public CssBox Caption { get; private set; }

        /// <summary>
        ///     Gets the column count of this table
        /// </summary>
        public int ColumnCount { get; private set; }

        /// <summary>
        ///     Gets the minimum width of each column
        /// </summary>
        public float[] ColumnMinWidths
        {
            get
            {
                if (_columnMinWidths == null)
                {
                    _columnMinWidths = new float[ColumnWidths.Length];

                    foreach (var row in AllRows)
                    {
                        foreach (var cell in row.Boxes)
                        {
                            var colspan = GetColSpan(cell);
                            var col = GetCellRealColumnIndex(row, cell);
                            var affectcol = col + colspan - 1;
                            var spannedwidth = GetSpannedMinWidth(row, col, colspan) +
                                               (colspan - 1) * HorizontalSpacing;

                            _columnMinWidths[affectcol] = Math.Max(_columnMinWidths[affectcol],
                                cell.GetMinimumWidth() - spannedwidth);
                        }
                    }
                }

                return _columnMinWidths;
            }
        }

        /// <summary>
        ///     Gets the declared Columns on the TABLE tag
        /// </summary>
        public List<CssBox> Columns { get; }

        /// <summary>
        ///     Gets an array indicating the withs of each column.
        ///     This must have the same count than <see cref="Columns" />
        /// </summary>
        public float[] ColumnWidths { get; private set; }

        /// <summary>
        ///     Gets the boxes that represents the table-row Boxes of the table,
        ///     including those inside of the TBODY tags
        /// </summary>
        public List<CssBox> BodyRows { get; }

        /// <summary>
        ///     Gets the table-footer-group Box
        ///     WARNING: May be null
        /// </summary>
        public CssBox FooterBox { get; private set; }

        /// <summary>
        ///     Gets the table-header-group Box
        ///     WARNING: May be null
        /// </summary>
        public CssBox HeaderBox { get; private set; }

        /// <summary>
        ///     Gets the actual horizontal spacing of the table
        /// </summary>
        public float HorizontalSpacing
        {
            get
            {
                if (TableBox.BorderCollapse == CssConstants.Collapse)
                {
                    return -1f;
                }

                return TableBox.ActualBorderSpacingHorizontal;
            }
        }

        /// <summary>
        ///     Gets the actual vertical spacing of the table
        /// </summary>
        public float VerticalSpacing
        {
            get
            {
                if (TableBox.BorderCollapse == CssConstants.Collapse)
                {
                    return -1f;
                }

                return TableBox.ActualBorderSpacingVertical;
            }
        }

        /// <summary>
        ///     Gets the row count of this table, including the rows inside the table-row-group,
        ///     table-row-heaer and table-row-footer Boxes
        /// </summary>
        public int RowCount { get; private set; }

        /// <summary>
        ///     Gets the original table box
        /// </summary>
        public CssBox TableBox { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Analyzes the Table and assigns values to this CssTable object.
        ///     To be called from the constructor
        /// </summary>
        private void Analyze(Graphics g)
        {
            GetAvailableWidth();
            var availCellSpace = float.NaN; //Will be set later

            #region Assign box kinds

            foreach (var b in TableBox.Boxes)
            {
                b.RemoveAnonymousSpaces();
                switch (b.Display)
                {
                    case CssConstants.TableCaption:
                        Caption = b;
                        break;
                    case CssConstants.TableColumn:
                        for (var i = 0; i < GetSpan(b); i++)
                        {
                            Columns.Add(CreateColumn(b));
                        }

                        break;
                    case CssConstants.TableColumnGroup:
                        if (b.Boxes.Count == 0)
                        {
                            var gspan = GetSpan(b);
                            for (var i = 0; i < gspan; i++)
                            {
                                Columns.Add(CreateColumn(b));
                            }
                        }
                        else
                        {
                            foreach (var bb in b.Boxes)
                            {
                                var bbspan = GetSpan(bb);
                                for (var i = 0; i < bbspan; i++)
                                {
                                    Columns.Add(CreateColumn(bb));
                                }
                            }
                        }
                        break;
                    case CssConstants.TableFooterGroup:
                        if (FooterBox != null)
                        {
                            BodyRows.Add(b);
                        }
                        else
                        {
                            FooterBox = b;
                        }

                        break;
                    case CssConstants.TableHeaderGroup:
                        if (HeaderBox != null)
                        {
                            BodyRows.Add(b);
                        }
                        else
                        {
                            HeaderBox = b;
                        }

                        break;
                    case CssConstants.TableRow:
                        BodyRows.Add(b);
                        break;
                    case CssConstants.TableRowGroup:
                        foreach (var unused in b.Boxes)
                        {
                            if (b.Display == CssConstants.TableRow)
                            {
                                BodyRows.Add(b);
                            }
                        }

                        break;
                }
            }

            #endregion

            #region Gather AllRows

            if (HeaderBox != null)
            {
                AllRows.AddRange(HeaderBox.Boxes);
            }

            AllRows.AddRange(BodyRows);
            if (FooterBox != null)
            {
                AllRows.AddRange(FooterBox.Boxes);
            }

            #endregion

            #region Insert EmptyBoxes for vertical cell spanning

            if (!TableBox.TableFixed)
            {
                var currow = 0;
                // ReSharper disable once NotAccessedVariable
                var curcol = 0;
                var rows = BodyRows;

                foreach (var row in rows)
                {
                    row.RemoveAnonymousSpaces();
                    curcol = 0;
                    for (var k = 0; k < row.Boxes.Count; k++)
                    {
                        var cell = row.Boxes[k];
                        var rowspan = GetRowSpan(cell);
                        var realcol = GetCellRealColumnIndex(row, cell); //Real column of the cell

                        for (var i = currow + 1; i < currow + rowspan; i++)
                        {
                            var colcount = 0;
                            for (var j = 0; j <= rows[i].Boxes.Count; j++)
                            {
                                if (colcount == realcol)
                                {
                                    rows[i].Boxes.Insert(colcount, new SpacingBox(TableBox, ref cell, currow));
                                    break;
                                }
                                colcount++;
                                realcol -= GetColSpan(rows[i].Boxes[j]) - 1;
                            }
                        } // End for (int i = currow + 1; i < currow + rowspan; i++)
                        curcol++;
                    } // End foreach (Box cell in row.Boxes)
                    currow++;
                } // End foreach (Box row in rows)

                TableBox.TableFixed = true;
            } // End if (!TableBox.TableFixed)

            #endregion

            #region Determine Row and Column Count, and ColumnWidths

            //Rows
            RowCount = BodyRows.Count +
                       (HeaderBox?.Boxes.Count ?? 0) +
                       (FooterBox?.Boxes.Count ?? 0);

            //Columns
            if (Columns.Count > 0)
            {
                ColumnCount = Columns.Count;
            }
            else
            {
                foreach (var b in AllRows) //Check trhough rows
                {
                    ColumnCount = Math.Max(ColumnCount, b.Boxes.Count);
                }
            }

            //Initialize column widths array
            ColumnWidths = new float[ColumnCount];

            //Fill them with NaNs
            for (var i = 0; i < ColumnWidths.Length; i++)
            {
                ColumnWidths[i] = float.NaN;
            }

            availCellSpace = GetAvailableCellWidth();

            if (Columns.Count > 0)
            {
                #region Fill ColumnWidths array by scanning column widths

                for (var i = 0; i < Columns.Count; i++)
                {
                    var len = new CssLength(Columns[i].Width); //Get specified width

                    if (len.Number > 0) //If some width specified
                    {
                        if (len.IsPercentage) //Get width as a percentage
                        {
                            ColumnWidths[i] = CssValue.ParseNumber(Columns[i].Width, availCellSpace);
                        }
                        else if (len.Unit == CssLength.CssUnit.Pixels || len.Unit == CssLength.CssUnit.None)
                        {
                            ColumnWidths[i] = len.Number; //Get width as an absolute-pixel value
                        }
                    }
                }

                #endregion
            }
            else
            {
                #region Fill ColumnWidths array by scanning width in table-cell definitions

                foreach (var row in AllRows)
                {
                    //Check for column width in table-cell definitions
                    for (var i = 0; i < ColumnCount; i++)
                    {
                        if (float.IsNaN(ColumnWidths[i]) && //Check if no width specified for column
                            i < row.Boxes.Count && //And there's a box to check
                            row.Boxes[i].Display == CssConstants.TableCell) //And the box is a table-cell
                        {
                            var len = new CssLength(row.Boxes[i].Width); //Get specified width

                            if (len.Number > 0) //If some width specified
                            {
                                var colspan = GetColSpan(row.Boxes[i]);
                                var flen = 0f;
                                if (len.IsPercentage) //Get width as a percentage
                                {
                                    flen = CssValue.ParseNumber(row.Boxes[i].Width, availCellSpace);
                                }
                                else if (len.Unit == CssLength.CssUnit.Pixels || len.Unit == CssLength.CssUnit.None)
                                {
                                    flen = len.Number; //Get width as an absolute-pixel value
                                }

                                flen /= Convert.ToSingle(colspan);

                                for (var j = i; j < i + colspan; j++)
                                {
                                    ColumnWidths[j] = flen;
                                }
                            }
                        }
                    }
                }

                #endregion
            }

            #endregion

            #region Determine missing Column widths

            if (WidthSpecified) //If a width was specified,
            {
                //Assign NaNs equally with space left after gathering not-NaNs
                var numberOfNans = 0;
                var occupedSpace = 0f;

                //Calculate number of NaNs and occuped space
                foreach (float widths in ColumnWidths)
                {
                    if (float.IsNaN(widths))
                    {
                        numberOfNans++;
                    }
                    else
                    {
                        occupedSpace += widths;
                    }
                }

                //Determine width that will be assigned to un asigned widths
                var nanWidth = (availCellSpace - occupedSpace) / Convert.ToSingle(numberOfNans);

                for (var i = 0; i < ColumnWidths.Length; i++)
                {
                    if (float.IsNaN(ColumnWidths[i]))
                    {
                        ColumnWidths[i] = nanWidth;
                    }
                }
            }
            else
            {
                //Assign NaNs using full width
                var _maxFullWidths = new float[ColumnWidths.Length];

                //Get the maximum full length of NaN boxes
                foreach (var row in AllRows)
                {
                    for (var i = 0; i < row.Boxes.Count; i++)
                    {
                        var col = GetCellRealColumnIndex(row, row.Boxes[i]);

                        if (float.IsNaN(ColumnWidths[col]) &&
                            i < row.Boxes.Count &&
                            GetColSpan(row.Boxes[i]) == 1)
                        {
                            _maxFullWidths[col] = Math.Max(_maxFullWidths[col], row.Boxes[i].GetFullWidth(g));
                        }
                    }
                }

                for (var i = 0; i < ColumnWidths.Length; i++)
                {
                    if (float.IsNaN(ColumnWidths[i]))
                    {
                        ColumnWidths[i] = _maxFullWidths[i];
                    }
                }
            }

            #endregion

            #region Reduce widths if necessary

            var curCol = 0;
            var reduceAmount = 1f;

            //While table width is larger than it should, and width is reductable
            while (GetWidthSum() > GetAvailableWidth() && CanReduceWidth())
            {
                while (!CanReduceWidth(curCol))
                {
                    curCol++;
                }

                ColumnWidths[curCol] -= reduceAmount;

                curCol++;

                if (curCol >= ColumnWidths.Length)
                {
                    curCol = 0;
                }
            }

            #endregion

            #region Check for minimum sizes (increment widths if necessary)

            foreach (var row in AllRows)
            {
                foreach (var cell in row.Boxes)
                {
                    var colspan = GetColSpan(cell);
                    var col = GetCellRealColumnIndex(row, cell);
                    var affectcol = col + colspan - 1;

                    if (ColumnWidths[col] < ColumnMinWidths[col])
                    {
                        var diff = ColumnMinWidths[col] - ColumnWidths[col];
                        ColumnWidths[affectcol] = ColumnMinWidths[affectcol];

                        if (col < ColumnWidths.Length - 1)
                        {
                            ColumnWidths[col + 1] -= diff;
                        }
                    }
                }
            }

            #endregion

            #region Set table padding

            TableBox.Padding = "0"; //Ensure there's no padding

            #endregion

            #region Layout cells

            //Actually layout cells!
            var startx = TableBox.ClientLeft + HorizontalSpacing;
            var starty = TableBox.ClientTop + VerticalSpacing;
            var curx = startx;
            var cury = starty;
            var maxRight = startx;
            var maxBottom = 0f;
            var currentrow = 0;

            foreach (var row in AllRows)
            {
                if (row is CssAnonymousSpaceBlockBox || row is CssAnonymousSpaceBox)
                {
                    continue;
                }

                curx = startx;
                curCol = 0;

                foreach (var cell in row.Boxes)
                {
                    if (curCol >= ColumnWidths.Length)
                    {
                        break;
                    }

                    var rowspan = GetRowSpan(cell);
                    var width = GetCellWidth(GetCellRealColumnIndex(row, cell), cell);

                    cell.Location = new PointF(curx, cury);
                    cell.Size = new SizeF(width, 0f);
                    cell.MeasureBounds(g); //That will automatically set the bottom of the cell

                    //Alter max bottom only if row is cell's row + cell's rowspan - 1
                    var sb = cell as SpacingBox;
                    if (sb != null)
                    {
                        if (sb.EndRow == currentrow)
                        {
                            maxBottom = Math.Max(maxBottom, sb.ExtendedBox.ActualBottom);
                        }
                    }
                    else if (rowspan == 1)
                    {
                        maxBottom = Math.Max(maxBottom, cell.ActualBottom);
                    }
                    maxRight = Math.Max(maxRight, cell.ActualRight);
                    curCol++;
                    curx = cell.ActualRight + HorizontalSpacing;
                }

                foreach (var cell in row.Boxes)
                {
                    var spacer = cell as SpacingBox;

                    if (spacer == null && GetRowSpan(cell) == 1)
                    {
                        cell.ActualBottom = maxBottom;
                        CssLayoutEngine.ApplyCellVerticalAlignment(g, cell);
                    }
                    else if (spacer != null && spacer.EndRow == currentrow)
                    {
                        spacer.ExtendedBox.ActualBottom = maxBottom;
                        CssLayoutEngine.ApplyCellVerticalAlignment(g, spacer.ExtendedBox);
                    }
                }

                cury = maxBottom + VerticalSpacing;
                currentrow++;
            }

            TableBox.ActualRight = maxRight + HorizontalSpacing + TableBox.ActualBorderRightWidth;
            TableBox.ActualBottom = maxBottom + VerticalSpacing + TableBox.ActualBorderBottomWidth;

            #endregion
        }

        /// <summary>
        ///     Gets the spanned width of a cell
        ///     (With of all columns it spans minus one)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="realcolindex"></param>
        /// <param name="colspan"></param>
        /// <returns></returns>
        private float GetSpannedMinWidth(CssBox row, int realcolindex, int colspan)
        {
            var w = 0f;

            for (var i = realcolindex; i < row.Boxes.Count || i < realcolindex + colspan - 1; i++)
            {
                w += ColumnMinWidths[i];
            }

            return w;
        }

        /// <summary>
        ///     Gets the cell column index checking its position and other cells colspans
        /// </summary>
        /// <param name="row"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        private int GetCellRealColumnIndex(CssBox row, CssBox cell)
        {
            var i = 0;

            foreach (var b in row.Boxes)
            {
                if (b.Equals(cell))
                {
                    break;
                }

                i += GetColSpan(b);
            }

            return i;
        }

        /// <summary>
        ///     Gets the cells width, taking colspan and being in the specified column
        /// </summary>
        /// <param name="column"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private float GetCellWidth(int column, CssBox b)
        {
            var colspan = Convert.ToSingle(GetColSpan(b));
            var sum = 0f;

            for (var i = column; i < column + colspan; i++)
            {
                if (column >= ColumnWidths.Length)
                {
                    break;
                }

                if (ColumnWidths.Length <= i)
                {
                    break;
                }

                sum += ColumnWidths[i];
            }

            sum += (colspan - 1) * HorizontalSpacing;

            return sum;
        }

        /// <summary>
        ///     Gets the colspan of the specified box
        /// </summary>
        /// <param name="b"></param>
        private int GetColSpan(CssBox b)
        {
            var att = b.GetAttribute("colspan", "1");

            if (!int.TryParse(att, out int colspan))
            {
                return 1;
            }

            return colspan;
        }

        /// <summary>
        ///     Gets the rowspan of the specified box
        /// </summary>
        /// <param name="b"></param>
        private int GetRowSpan(CssBox b)
        {
            var att = b.GetAttribute("rowspan", "1");

            if (!int.TryParse(att, out int rowspan))
            {
                return 1;
            }

            return rowspan;
        }

        /// <summary>
        ///     Recursively measures the specified box
        /// </summary>
        /// <param name="b"></param>
        /// <param name="g"></param>
        private void Measure(CssBox b, Graphics g)
        {
            if (b == null)
            {
                return;
            }

            foreach (var bb in b.Boxes)
            {
                bb.MeasureBounds(g);
                Measure(bb, g);
            }
        }

        /// <summary>
        ///     Recursively measures words inside the box
        /// </summary>
        /// <param name="b"></param>
        /// <param name="g"></param>
        private void MeasureWords(CssBox b, Graphics g)
        {
            if (b == null)
            {
                return;
            }

            foreach (var bb in b.Boxes)
            {
                bb.MeasureWordsSize(g);
                MeasureWords(bb, g);
            }
        }

        /// <summary>
        ///     Gets the number of reductable columns
        /// </summary>
        /// <returns></returns>
        private int GetReductableColumns()
        {
            var response = 0;

            for (var i = 0; i < ColumnWidths.Length; i++)
            {
                if (CanReduceWidth(i))
                {
                    response++;
                }
            }

            return response;
        }

        /// <summary>
        ///     Tells if the columns widths can be reduced,
        ///     by checking the minimum widths of all cells
        /// </summary>
        /// <returns></returns>
        private bool CanReduceWidth()
        {
            for (var i = 0; i < ColumnWidths.Length; i++)
            {
                if (CanReduceWidth(i))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Tells if the specified column can be reduced,
        ///     by checking its minimum width
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private bool CanReduceWidth(int columnIndex)
        {
            if (ColumnWidths.Length >= columnIndex || ColumnMinWidths.Length >= columnIndex)
            {
                return false;
            }

            return ColumnWidths[columnIndex] > ColumnMinWidths[columnIndex];
        }

        /// <summary>
        ///     Gets the available width for the whole table.
        ///     It also sets the value of <see cref="WidthSpecified" />
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///     The table's width can be larger than the result of this method, because of the minimum
        ///     size that individual boxes.
        /// </remarks>
        private float GetAvailableWidth()
        {
            var tblen = new CssLength(TableBox.Width);

            if (tblen.Number > 0)
            {
                WidthSpecified = true;

                if (tblen.IsPercentage)
                {
                    return CssValue.ParseNumber(tblen.Length, TableBox.ParentBox.AvailableWidth);
                }

                return tblen.Number;
            }
            return TableBox.ParentBox.AvailableWidth;
        }

        /// <summary>
        ///     Gets the width available for cells
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///     It takes away the cell-spacing from <see cref="GetAvailableWidth()" />
        /// </remarks>
        private float GetAvailableCellWidth()
        {
            return GetAvailableWidth() -
                   HorizontalSpacing * (ColumnCount + 1) -
                   TableBox.ActualBorderLeftWidth - TableBox.ActualBorderRightWidth;
        }

        /// <summary>
        ///     Gets the current sum of column widths
        /// </summary>
        /// <returns></returns>
        private float GetWidthSum()
        {
            var f = 0f;

            foreach (float widths in ColumnWidths)
            {
                if (float.IsNaN(widths))
                {
                    throw new Exception("CssTable Algorithm error: There's a NaN in column widths");
                }
                else
                {
                    f += widths;
                }
            }

            //Take cell-spacing
            f += HorizontalSpacing * (ColumnWidths.Length + 1);

            //Take table borders
            f += TableBox.ActualBorderLeftWidth + TableBox.ActualBorderRightWidth;

            return f;
        }

        /// <summary>
        ///     Gets the span attribute of the tag of the specified box
        /// </summary>
        /// <param name="b"></param>
        private int GetSpan(CssBox b)
        {
            var f = CssValue.ParseNumber(b.GetAttribute("span"), 1);

            return Math.Max(1, Convert.ToInt32(f));
        }

        /// <summary>
        ///     Creates the column with the specified width
        /// </summary>
        /// <param name="modelBox"></param>
        /// <returns></returns>
        private CssBox CreateColumn(CssBox modelBox)
        {
            return modelBox;
            //Box b = new Box(null, new HtmlTag(string.Format("<COL style=\"width:{0}\" >", width)));
            //b.Width = width;
            //return b;
        }

        #endregion
    }
}