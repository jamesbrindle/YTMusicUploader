namespace System.Drawing.Html
{
    /// <summary>
    ///     Represents a word inside an inline box
    /// </summary>
    /// <remarks>
    ///     Because of performance, words of text are the most atomic
    ///     element in the project. It should be characters, but come on,
    ///     imagine the performance when drawing char by char on the device.
    ///     It may change for future versions of the library
    /// </remarks>
    internal class CssBoxWord
        : CssRectangle
    {
        #region Fields

        //private int _spacesAfter;
        //private bool _breakAfter;
        //private int _spacesBefore;
        //private bool _breakBefore;
        private Image _image;

        #endregion

        #region Ctor

        internal CssBoxWord(CssBox owner)
        {
            OwnerBox = owner;
            Text = string.Empty;
        }

        /// <summary>
        ///     Creates a new BoxWord which represents an image
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="image"></param>
        public CssBoxWord(CssBox owner, Image image)
            : this(owner)
        {
            Image = image;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the width of the word including white-spaces
        /// </summary>
        public float FullWidth => Width;

        /// <summary>
        ///     Gets the image this words represents (if one)
        /// </summary>
        public Image Image
        {
            get { return _image; }
            set
            {
                _image = value;

                if (value != null)
                {
                    var w = new CssLength(OwnerBox.Width);
                    var h = new CssLength(OwnerBox.Height);
                    if (w.Number > 0 && w.Unit == CssLength.CssUnit.Pixels)
                    {
                        Width = w.Number;
                    }
                    else
                    {
                        Width = value.Width;
                    }

                    if (h.Number > 0 && h.Unit == CssLength.CssUnit.Pixels)
                    {
                        Height = h.Number;
                    }
                    else
                    {
                        Height = value.Height;
                    }

                    Height += OwnerBox.ActualBorderBottomWidth + OwnerBox.ActualBorderTopWidth +
                              OwnerBox.ActualPaddingTop + OwnerBox.ActualPaddingBottom;
                }
            }
        }

        /// <summary>
        ///     Gets if the word represents an image.
        /// </summary>
        public bool IsImage => Image != null;

        /// <summary>
        ///     Gets a bool indicating if this word is composed only by spaces.
        ///     Spaces include tabs and line breaks
        /// </summary>
        public bool IsSpaces => string.IsNullOrEmpty(Text.Trim());

        /// <summary>
        ///     Gets if the word is composed by only a line break
        /// </summary>
        public bool IsLineBreak => Text == "\n";

        /// <summary>
        ///     Gets if the word is composed by only a tab
        /// </summary>
        public bool IsTab => Text == "\t";

        /// <summary>
        ///     Gets the Box where this word belongs.
        /// </summary>
        public CssBox OwnerBox { get; }

        /// <summary>
        ///     Gets the text of the word
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        ///     Gets or sets an offset to be considered in measurements
        /// </summary>
        internal PointF LastMeasureOffset { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Removes line breaks and tabs on the text of the word,
        ///     replacing them with white spaces
        /// </summary>
        internal void ReplaceLineBreaksAndTabs()
        {
            Text = Text.Replace('\n', ' ');
            Text = Text.Replace('\t', ' ');
        }

        /// <summary>
        ///     Appends the specified char to the word's text
        /// </summary>
        /// <param name="c"></param>
        internal void AppendChar(char c)
        {
            Text += c;
        }

        /// <summary>
        ///     Represents this word for debugging purposes
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                $"{Text.Replace(' ', '-').Replace("\n", "\\n")} ({Text.Length} char{(Text.Length != 1 ? "s" : string.Empty)})";
        }

        #endregion
    }
}