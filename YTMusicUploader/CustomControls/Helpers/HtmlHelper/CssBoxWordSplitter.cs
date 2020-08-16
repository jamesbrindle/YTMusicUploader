using System.Collections.Generic;

namespace System.Drawing.Html
{
    /// <summary>
    ///     Splits text on words for a box
    /// </summary>
    internal class CssBoxWordSplitter
    {
        #region Static

        /// <summary>
        ///     Returns a bool indicating if the specified box white-space processing model specifies
        ///     that sequences of white spaces should be collapsed on a single whitespace
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CollapsesWhiteSpaces(CssBox b)
        {
            return b.WhiteSpace == CssConstants.Normal ||
                   b.WhiteSpace == CssConstants.Nowrap ||
                   b.WhiteSpace == CssConstants.PreLine;
        }

        /// <summary>
        ///     Returns a bool indicating if line breaks at the source should be eliminated
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool EliminatesLineBreaks(CssBox b)
        {
            return b.WhiteSpace == CssConstants.Normal || b.WhiteSpace == CssConstants.Nowrap;
        }

        #endregion

        #region Fields

        private CssBoxWord _curword;

        #endregion

        #region Ctor

        private CssBoxWordSplitter()
        {
            Words = new List<CssBoxWord>();
            _curword = null;
        }

        public CssBoxWordSplitter(CssBox box, string text)
            : this()
        {
            Box = box;
            Text = text.Replace("\r", string.Empty);

        }

        #endregion

        #region Props

        public List<CssBoxWord> Words { get; }


        public string Text { get; }


        public CssBox Box { get; }

        #endregion

        #region Public Metods

        /// <summary>
        ///     Splits the text on words using rules of the specified box
        /// </summary>
        /// <returns></returns>
        public void SplitWords()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            _curword = new CssBoxWord(Box);

            var onspace = IsSpace(Text[0]);

            foreach (char ch in Text)
            {
                if (IsSpace(ch))
                {
                    if (!onspace)
                    {
                        CutWord();
                    }

                    if (IsLineBreak(ch))
                    {
                        _curword.AppendChar('\n');
                        CutWord();
                    }
                    else if (IsTab(ch))
                    {
                        _curword.AppendChar('\t');
                        CutWord();
                    }
                    else
                    {
                        _curword.AppendChar(' ');
                    }

                    onspace = true;
                }
                else
                {
                    if (onspace)
                    {
                        CutWord();
                    }

                    _curword.AppendChar(ch);

                    onspace = false;
                }
            }

            CutWord();
        }

        private void CutWord()
        {
            if (_curword.Text.Length > 0)
            {
                Words.Add(_curword);
            }

            _curword = new CssBoxWord(Box);
        }

        private bool IsSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n';
        }

        private bool IsLineBreak(char c)
        {
            return c == '\n' || c == '\a';
        }

        private bool IsTab(char c)
        {
            return c == '\t';
        }

        #endregion
    }
}