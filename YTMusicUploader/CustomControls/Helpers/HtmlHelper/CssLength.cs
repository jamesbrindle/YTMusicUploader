using System.Globalization;

namespace System.Drawing.Html
{
    /// <summary>
    ///     Represents and gets info about a CSS Length
    /// </summary>
    /// <remarks>
    ///     http://www.w3.org/TR/CSS21/syndata.html#length-units
    /// </remarks>
    public class CssLength
    {
        #region Enum

        /// <summary>
        ///     Represents the possible units of the CSS lengths
        /// </summary>
        /// <remarks>
        ///     http://www.w3.org/TR/CSS21/syndata.html#length-units
        /// </remarks>
        public enum CssUnit
        {
            None,

            Ems,

            Pixels,

            Ex,

            Inches,

            Centimeters,

            Milimeters,

            Points,

            Picas
        }

        #endregion

        #region Ctor

        /// <summary>
        ///     Creates a new CssLength from a length specified on a CSS style sheet or fragment
        /// </summary>
        /// <param name="length">Length as specified in the Style Sheet or style fragment</param>
        public CssLength(string length)
        {
            Length = length;
            _number = 0f;
            Unit = CssUnit.None;
            IsPercentage = false;

            //Return zero if no length specified, zero specified
            if (string.IsNullOrEmpty(length) || length == "0")
            {
                return;
            }

            //If percentage, use ParseNumber
            if (length.EndsWith("%"))
            {
                _number = CssValue.ParseNumber(length, 1);
                IsPercentage = true;
                return;
            }

            //If no units, has error
            if (length.Length < 3)
            {
                float.TryParse(length, out _number);
                HasError = true;
                return;
            }

            //Get units of the length
            var u = length.Substring(length.Length - 2, 2);

            //Number of the length
            var number = length.Substring(0, length.Length - 2);

            switch (u)
            {
                case CssConstants.Em:
                    Unit = CssUnit.Ems;
                    IsRelative = true;
                    break;
                case CssConstants.Ex:
                    Unit = CssUnit.Ex;
                    IsRelative = true;
                    break;
                case CssConstants.Px:
                    Unit = CssUnit.Pixels;
                    IsRelative = true;
                    break;
                case CssConstants.Mm:
                    Unit = CssUnit.Milimeters;
                    break;
                case CssConstants.Cm:
                    Unit = CssUnit.Centimeters;
                    break;
                case CssConstants.In:
                    Unit = CssUnit.Inches;
                    break;
                case CssConstants.Pt:
                    Unit = CssUnit.Points;
                    break;
                case CssConstants.Pc:
                    Unit = CssUnit.Picas;
                    break;
                default:
                    HasError = true;
                    return;
            }

            if (!float.TryParse(number, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out _number))
            {
                HasError = true;
            }
        }

        #endregion

        #region Fields

        private readonly float _number;

        #endregion

        #region Props

        /// <summary>
        ///     Gets the number in the length
        /// </summary>
        public float Number => _number;

        /// <summary>
        ///     Gets if the length has some parsing error
        /// </summary>
        public bool HasError { get; }


        /// <summary>
        ///     Gets if the length represents a precentage (not actually a length)
        /// </summary>
        public bool IsPercentage { get; }


        /// <summary>
        ///     Gets if the length is specified in relative units
        /// </summary>
        public bool IsRelative { get; }

        /// <summary>
        ///     Gets the unit of the length
        /// </summary>
        public CssUnit Unit { get; }

        /// <summary>
        ///     Gets the length as specified in the string
        /// </summary>
        public string Length { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     If length is in Ems, returns its value in points
        /// </summary>
        /// <param name="emSize">Em size factor to multiply</param>
        /// <returns>Points size of this em</returns>
        /// <exception cref="InvalidOperationException">If length has an error or isn't in ems</exception>
        public CssLength ConvertEmToPoints(float emSize)
        {
            if (HasError)
            {
                throw new InvalidOperationException("Invalid length");
            }

            if (Unit != CssUnit.Ems)
            {
                throw new InvalidOperationException("Length is not in ems");
            }

            return new CssLength(
                $"{Convert.ToSingle(Number * emSize).ToString("0.0", NumberFormatInfo.InvariantInfo)}pt");
        }

        /// <summary>
        ///     If length is in Ems, returns its value in pixels
        /// </summary>
        /// <returns>Pixels size of this em</returns>
        /// <exception cref="InvalidOperationException">If length has an error or isn't in ems</exception>
        public CssLength ConvertEmToPixels(float pixelFactor)
        {
            if (HasError)
            {
                throw new InvalidOperationException("Invalid length");
            }

            if (Unit != CssUnit.Ems)
            {
                throw new InvalidOperationException("Length is not in ems");
            }

            return new CssLength(
                $"{Convert.ToSingle(Number * pixelFactor).ToString("0.0", NumberFormatInfo.InvariantInfo)}px");
        }

        /// <summary>
        ///     Returns the length formatted ready for CSS interpreting.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (HasError)
            {
                return string.Empty;
            }

            if (IsPercentage)
            {
                return string.Format(NumberFormatInfo.InvariantInfo, "{0}%", Number);
            }
            var u = string.Empty;

            switch (Unit)
            {
                case CssUnit.None:
                    break;
                case CssUnit.Ems:
                    u = "em";
                    break;
                case CssUnit.Pixels:
                    u = "px";
                    break;
                case CssUnit.Ex:
                    u = "ex";
                    break;
                case CssUnit.Inches:
                    u = "in";
                    break;
                case CssUnit.Centimeters:
                    u = "cm";
                    break;
                case CssUnit.Milimeters:
                    u = "mm";
                    break;
                case CssUnit.Points:
                    u = "pt";
                    break;
                case CssUnit.Picas:
                    u = "pc";
                    break;
            }

            return string.Format(NumberFormatInfo.InvariantInfo, "{0}{1}", Number, u);
        }

        #endregion
    }
}