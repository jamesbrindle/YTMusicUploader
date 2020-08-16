using System.Text.RegularExpressions;

namespace System.Drawing.Html
{
    /// <summary>
    ///     Collection of regular expressions used when parsing
    /// </summary>
    internal static class Parser
    {
        /// <summary>
        ///     Extracts properties and values from a Css property block; e.g. property:value;
        /// </summary>
        public const string CssProperties = @";?[^;\s]*:[^\{\}:;]*(\}|;)?";

        /// <summary>
        ///     Extracts CSS style comments; e.g. /* comment */
        /// </summary>
        public const string CssComments = @"/\*[^*/]*\*/";

        /// <summary>
        ///     Extracts CSS at-rules; e.g. @media print { block1{} block2{} }
        /// </summary>
        public const string CssAtRules = @"@.*\{\s*(\s*" + CssBlocks + @"\s*)*\s*\}";

        /// <summary>
        ///     Extracts the media types from a media at-rule; e.g. @media print, 3d, screen {
        /// </summary>
        public const string CssMediaTypes = @"@media[^\{\}]*\{";

        /// <summary>
        ///     Extracts defined blocks in CSS.
        ///     WARNING: Blocks will include blocks inside at-rules.
        /// </summary>
        public const string CssBlocks = @"[^\{\}]*\{[^\{\}]*\}";

        /// <summary>
        ///     Extracts a number; e.g.  5, 6, 7.5, 0.9
        /// </summary>
        public const string CssNumber = @"{[0-9]+|[0-9]*\.[0-9]+}";

        /// <summary>
        ///     Extracts css percentages from the string; e.g. 100% .5% 5.4%
        /// </summary>
        public const string CssPercentage = @"([0-9]+|[0-9]*\.[0-9]+)\%";

        /// <summary>
        ///     Extracts CSS lengths; e.g. 9px 3pt .89em
        /// </summary>
        public const string CssLength = @"([0-9]+|[0-9]*\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)";

        /// <summary>
        ///     Extracts CSS colors; e.g. black white #fff #fe98cd rgb(5,5,5) rgb(45%, 0, 0)
        /// </summary>
        public const string CssColors =
                @"(#\S{6}|#\S{3}|rgb\(\s*[0-9]{1,3}\%?\s*\,\s*[0-9]{1,3}\%?\s*\,\s*[0-9]{1,3}\%?\s*\)|maroon|red|orange|yellow|olive|purple|fuchsia|white|lime|green|navy|blue|aqua|teal|black|silver|gray)"
            ;

        /// <summary>
        ///     Extracts line-height values (normal, numbers, lengths, percentages)
        /// </summary>
        public const string CssLineHeight = "(normal|" + CssNumber + "|" + CssLength + "|" + CssPercentage + ")";

        /// <summary>
        ///     Extracts CSS border styles; e.g. solid none dotted
        /// </summary>
        public const string CssBorderStyle = @"(none|hidden|dotted|dashed|solid|double|groove|ridge|inset|outset)";

        /// <summary>
        ///     Extracts CSS border widthe; e.g. 1px thin 3em
        /// </summary>
        public const string CssBorderWidth = "(" + CssLength + "|thin|medium|thick)";

        /// <summary>
        ///     Extracts font-family values
        /// </summary>
        public const string CssFontFamily = "(\"[^\"]*\"|'[^']*'|\\S+\\s*)(\\s*\\,\\s*(\"[^\"]*\"|'[^']*'|\\S+))*";

        /// <summary>
        ///     Extracts CSS font-styles; e.g. normal italic oblique
        /// </summary>
        public const string CssFontStyle = "(normal|italic|oblique)";

        /// <summary>
        ///     Extracts CSS font-variant values; e.g. normal, small-caps
        /// </summary>
        public const string CssFontVariant = "(normal|small-caps)";

        /// <summary>
        ///     Extracts font-weight values; e.g. normal, bold, bolder...
        /// </summary>
        public const string CssFontWeight = "(normal|bold|bolder|lighter|100|200|300|400|500|600|700|800|900)";

        /// <summary>
        ///     Exracts font sizes: xx-small, larger, small, 34pt, 30%, 2em
        /// </summary>
        public const string CssFontSize = "(" + CssLength + "|" + CssPercentage +
                                          "|xx-small|x-small|small|medium|large|x-large|xx-large|larger|smaller)";

        /// <summary>
        ///     Gets the font-size[/line-height]? on the font shorthand property.
        ///     Check http://www.w3.org/TR/CSS21/fonts.html#font-shorthand
        /// </summary>
        public const string CssFontSizeAndLineHeight = CssFontSize + @"(\/" + CssLineHeight + @")?(\s|$)";

        /// <summary>
        ///     Extracts HTML tags
        /// </summary>
        public const string HtmlTag = @"<[^<>]*>";

        /// <summary>
        ///     Extracts attributes from a HTML tag; e.g. att=value, att="value"
        /// </summary>
        public const string HmlTagAttributes = "[^\\s]*\\s*=\\s*(\"[^\"]*\"|[^\\s]*)";

        #region Methods

        /// <summary>
        ///     Extracts matches from the specified source
        /// </summary>
        /// <param name="regex">Regular expression to extract matches</param>
        /// <param name="source">Source to extract matches</param>
        /// <returns>Collection of matches</returns>
        public static MatchCollection Match(string regex, string source)
        {
            var r = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return r.Matches(source);
        }

        /// <summary>
        ///     Searches the specified regex on the source
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Search(string regex, string source)
        {
            return Search(regex, source, out int position);
        }

        /// <summary>
        ///     Searches the specified regex on the source
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="source"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string Search(string regex, string source, out int position)
        {
            var matches = Match(regex, source);

            if (matches.Count > 0)
            {
                position = matches[0].Index;
                return matches[0].Value;
            }
            position = -1;

            return null;
        }

        #endregion
    }
}