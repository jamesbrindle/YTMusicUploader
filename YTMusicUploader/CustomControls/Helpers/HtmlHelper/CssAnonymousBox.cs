namespace System.Drawing.Html
{
    /// <summary>
    ///     Represents an anonymous inline box
    /// </summary>
    /// <remarks>
    ///     To learn more about anonymous inline boxes visit:
    ///     http://www.w3.org/TR/CSS21/visuren.html#anonymous
    /// </remarks>
    public class CssAnonymousBox
        : CssBox
    {
        #region Ctor

        public CssAnonymousBox(CssBox parentBox)
            : base(parentBox)
        {
        }

        #endregion
    }

    /// <summary>
    ///     Represents an anonymous inline box which contains nothing but blank spaces
    /// </summary>
    public class CssAnonymousSpaceBox
        : CssAnonymousBox
    {
        public CssAnonymousSpaceBox(CssBox parentBox)
            : base(parentBox)
        {
        }
    }
}