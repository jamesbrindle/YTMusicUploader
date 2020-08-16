using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace System.Drawing.Html
{
    public static class CssValue
    {
        /// <summary>
        ///     Evals a number and returns it. If number is a percentage, it will be multiplied by <see cref="hundredPercent" />
        /// </summary>
        /// <param name="number">Number to be parsed</param>
        /// <param name="hundredPercent"></param>
        /// <returns>Parsed number. Zero if error while parsing.</returns>
        public static float ParseNumber(string number, float hundredPercent)
        {
            if (string.IsNullOrEmpty(number))
            {
                return 0f;
            }

            var toParse = number;
            var isPercent = number.EndsWith("%");

            if (isPercent)
            {
                toParse = number.Substring(0, number.Length - 1);
            }

            if (!float.TryParse(toParse, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out float result))
            {
                return 0f;
            }

            if (isPercent)
            {
                result = result / 100f * hundredPercent;
            }

            return result;
        }

        /// <summary>
        ///     Parses a length. Lengths are followed by an unit identifier (e.g. 10px, 3.1em)
        /// </summary>
        /// <param name="length">Specified length</param>
        /// <param name="hundredPercent">Equivalent to 100 percent when length is percentage</param>
        /// <param name="box"></param>
        /// <returns></returns>
        public static float ParseLength(string length, float hundredPercent, CssBox box)
        {
            return ParseLength(length, hundredPercent, box, box.GetEmHeight(), false);
        }

        /// <summary>
        ///     Parses a length. Lengths are followed by an unit identifier (e.g. 10px, 3.1em)
        /// </summary>
        /// <param name="length">Specified length</param>
        /// <param name="hundredPercent">Equivalent to 100 percent when length is percentage</param>
        /// <param name="box"></param>
        /// <param name="emFactor"></param>
        /// <param name="returnPoints">Allows the return float to be in points. If false, result will be pixels</param>
        /// <returns></returns>
        public static float ParseLength(string length, float hundredPercent, CssBox box, float emFactor,
            bool returnPoints)
        {
            //Return zero if no length specified, zero specified
            if (string.IsNullOrEmpty(length) || length == "0")
            {
                return 0f;
            }

            //If percentage, use ParseNumber
            if (length.EndsWith("%"))
            {
                return ParseNumber(length, hundredPercent);
            }

            //If no units, return zero
            if (length.Length < 3)
            {
                return 0f;
            }

            //Get units of the length
            var unit = length.Substring(length.Length - 2, 2);

            //Factor will depend on the unit
            var factor = 1f;

            //Number of the length
            var number = length.Substring(0, length.Length - 2);

            switch (unit)
            {
                case CssConstants.Em:
                    factor = emFactor;
                    break;
                case CssConstants.Px:
                    factor = 1f;
                    break;
                case CssConstants.Mm:
                    factor = 3f; //3 pixels per millimeter
                    break;
                case CssConstants.Cm:
                    factor = 37f; //37 pixels per centimeter
                    break;
                case CssConstants.In:
                    factor = 96f; //96 pixels per inch
                    break;
                case CssConstants.Pt:
                    factor = 96f / 72f; // 1 point = 1/72 of inch

                    if (returnPoints)
                    {
                        return ParseNumber(number, hundredPercent);
                    }

                    break;
                case CssConstants.Pc:
                    factor = 96f / 72f * 12f; // 1 pica = 12 points
                    break;
                default:
                    factor = 0f;
                    break;
            }


            return factor * ParseNumber(number, hundredPercent);
        }

        /// <summary>
        ///     Parses a color value in CSS style; e.g. #ff0000, red, rgb(255,0,0), rgb(100%, 0, 0)
        /// </summary>
        /// <param name="colorValue">Specified color value; e.g. #ff0000, red, rgb(255,0,0), rgb(100%, 0, 0)</param>
        /// <returns>System.Drawing.Color value</returns>
        public static Color GetActualColor(string colorValue)
        {
            var r = 0;
            var g = 0;
            var b = 0;
            var onError = Color.Empty;

            if (string.IsNullOrEmpty(colorValue))
            {
                return onError;
            }

            colorValue = colorValue.ToLower().Trim();

            if (colorValue.StartsWith("#"))
            {
                #region hexadecimal forms

                var hex = colorValue.Substring(1);

                if (hex.Length == 6)
                {
                    r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                    g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                    b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                }
                else if (hex.Length == 3)
                {
                    r = int.Parse(new string(hex.Substring(0, 1)[0], 2), NumberStyles.HexNumber);
                    g = int.Parse(new string(hex.Substring(1, 1)[0], 2), NumberStyles.HexNumber);
                    b = int.Parse(new string(hex.Substring(2, 1)[0], 2), NumberStyles.HexNumber);
                }
                else
                {
                    return onError;
                }

                #endregion
            }
            else if (colorValue.StartsWith("rgb(") && colorValue.EndsWith(")"))
            {
                #region RGB forms

                var rgb = colorValue.Substring(4, colorValue.Length - 5);
                var chunks = rgb.Split(',');

                if (chunks.Length == 3)
                {
                    unchecked
                    {
                        r = Convert.ToInt32(ParseNumber(chunks[0].Trim(), 255f));
                        g = Convert.ToInt32(ParseNumber(chunks[1].Trim(), 255f));
                        b = Convert.ToInt32(ParseNumber(chunks[2].Trim(), 255f));
                    }
                }
                else
                {
                    return onError;
                }

                #endregion
            }
            else
            {
                #region Color Constants

                var hex = string.Empty;

                switch (colorValue)
                {
                    case CssConstants.Maroon:
                        hex = "#800000";
                        break;
                    case CssConstants.Red:
                        hex = "#ff0000";
                        break;
                    case CssConstants.Orange:
                        hex = "#ffA500";
                        break;
                    case CssConstants.Olive:
                        hex = "#808000";
                        break;
                    case CssConstants.Purple:
                        hex = "#800080";
                        break;
                    case CssConstants.Fuchsia:
                        hex = "#ff00ff";
                        break;
                    case CssConstants.White:
                        hex = "#ffffff";
                        break;
                    case CssConstants.Lime:
                        hex = "#00ff00";
                        break;
                    case CssConstants.Green:
                        hex = "#008000";
                        break;
                    case CssConstants.Navy:
                        hex = "#000080";
                        break;
                    case CssConstants.Blue:
                        hex = "#0000ff";
                        break;
                    case CssConstants.Aqua:
                        hex = "#00ffff";
                        break;
                    case CssConstants.Teal:
                        hex = "#008080";
                        break;
                    case CssConstants.Black:
                        hex = "#000000";
                        break;
                    case CssConstants.Silver:
                        hex = "#c0c0c0";
                        break;
                    case CssConstants.Gray:
                        hex = "#808080";
                        break;
                    case CssConstants.Yellow:
                        hex = "#FFFF00";
                        break;
                }

                if (string.IsNullOrEmpty(hex))
                {
                    return onError;
                }

                var c = GetActualColor(hex);
                r = c.R;
                g = c.G;
                b = c.B;

                #endregion
            }

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        ///     Parses a border value in CSS style; e.g. 1px, 1, thin, thick, medium
        /// </summary>
        /// <param name="borderValue"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float GetActualBorderWidth(string borderValue, CssBox b)
        {
            if (string.IsNullOrEmpty(borderValue))
            {
                return GetActualBorderWidth(CssConstants.Medium, b);
            }

            switch (borderValue)
            {
                case CssConstants.Thin:
                    return 1f;
                case CssConstants.Medium:
                    return 2f;
                case CssConstants.Thick:
                    return 4f;
                default:
                    return Math.Abs(ParseLength(borderValue, 1, b));
            }
        }

        /// <summary>
        ///     Split the value by spaces; e.g. Useful in values like 'padding:5 4 3 inherit'
        /// </summary>
        /// <param name="value">Value to be splitted</param>
        /// <returns>Splitted and trimmed values</returns>
        public static string[] SplitValues(string value)
        {
            return SplitValues(value, ' ');
        }

        /// <summary>
        ///     Split the value by the specified separator; e.g. Useful in values like 'padding:5 4 3 inherit'
        /// </summary>
        /// <param name="value">Value to be splitted</param>
        /// <param name="separator"></param>
        /// <returns>Splitted and trimmed values</returns>
        public static string[] SplitValues(string value, char separator)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new string[] { };
            }

            var values = value.Split(separator);
            var result = new List<string>();

            foreach (string v in values)
            {
                var val = v.Trim();

                if (!string.IsNullOrEmpty(val))
                {
                    result.Add(val);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        ///     Detects the type name in a path.
        ///     E.g. Gets System.Drawing.Graphics from a path like System.Drawing.Graphics.Clear
        /// </summary>
        /// <param name="path"></param>
        /// <param name="moreInfo"></param>
        /// <returns></returns>
        private static Type GetTypeInfo(string path, ref string moreInfo)
        {
            var lastDot = path.LastIndexOf('.');

            if (lastDot < 0)
            {
                return null;
            }

            var type = path.Substring(0, lastDot);
            moreInfo = path.Substring(lastDot + 1);
            moreInfo = moreInfo.Replace("(", string.Empty).Replace(")", string.Empty);


            foreach (var a in HtmlRenderer.References)
            {
                var t = a.GetType(type, false, true);

                if (t != null)
                {
                    return t;
                }
            }

            return null;
        }

        /// <summary>
        ///     Returns the object specific to the path
        /// </summary>
        /// <param name="path"></param>
        /// <returns>One of the following possible objects: FileInfo, MethodInfo, PropertyInfo</returns>
        private static object DetectSource(string path)
        {
            if (path.StartsWith("method:", StringComparison.CurrentCultureIgnoreCase))
            {
                var methodName = string.Empty;
                var t = GetTypeInfo(path.Substring(7), ref methodName);
                if (t == null)
                {
                    return null;
                }

                var method = t.GetMethod(methodName);

                if (!method.IsStatic || method.GetParameters().Length > 0)
                {
                    return null;
                }

                return method;
            }
            if (path.StartsWith("property:", StringComparison.CurrentCultureIgnoreCase))
            {
                var propName = string.Empty;
                var t = GetTypeInfo(path.Substring(9), ref propName);
                if (t == null)
                {
                    return null;
                }

                var prop = t.GetProperty(propName);

                return prop;
            }
            if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
            {
                return new Uri(path);
            }

            return new FileInfo(path);
        }

        /// <summary>
        ///     Gets the image of the specified path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Image GetImage(string path)
        {
            var source = DetectSource(path);

            var finfo = source as FileInfo;
            var prop = source as PropertyInfo;
            var method = source as MethodInfo;

            try
            {
                if (finfo != null)
                {
                    if (!finfo.Exists)
                    {
                        return null;
                    }

                    return Image.FromFile(finfo.FullName);
                }
                if (prop != null)
                {
                    // ReSharper disable once CheckForReferenceEqualityInstead.1
                    if (!prop.PropertyType.IsSubclassOf(typeof(Image)) && !prop.PropertyType.Equals(typeof(Image)))
                    {
                        return null;
                    }

                    return prop.GetValue(null, null) as Image;
                }
                if (method != null)
                {
                    if (!method.ReturnType.IsSubclassOf(typeof(Image)))
                    {
                        return null;
                    }

                    return method.Invoke(null, null) as Image;
                }
                return null;
            }
            catch
            {
                return new Bitmap(50, 50);
            }
        }

        /// <summary>
        ///     Gets the content of the stylesheet specified in the path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetStyleSheet(string path)
        {
            var source = DetectSource(path);

            var finfo = source as FileInfo;
            var prop = source as PropertyInfo;
            var method = source as MethodInfo;

            try
            {
                if (finfo != null)
                {
                    if (!finfo.Exists)
                    {
                        return null;
                    }

                    var sr = new StreamReader(finfo.FullName);
                    var result = sr.ReadToEnd();
                    sr.Dispose();

                    return result;
                }
                if (prop != null)
                {
                    // ReSharper disable once CheckForReferenceEqualityInstead.1
                    if (!prop.PropertyType.Equals(typeof(string)))
                    {
                        return null;
                    }

                    return prop.GetValue(null, null) as string;
                }
                if (method != null)
                {
                    // ReSharper disable once CheckForReferenceEqualityInstead.1
                    if (!method.ReturnType.Equals(typeof(string)))
                    {
                        return null;
                    }

                    return method.Invoke(null, null) as string;
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     Executes the desired action when the user clicks a link
        /// </summary>
        /// <param name="href"></param>
        public static void GoLink(string href)
        {
            var source = DetectSource(href);

            var finfo = source as FileInfo;
            var method = source as MethodInfo;
            var uri = source as Uri;

            if (finfo != null || uri != null)
            {
                var nfo = new ProcessStartInfo(href) { UseShellExecute = true };

                Process.Start(nfo);
            }
            else if (method != null)
            {
                method.Invoke(null, null);
            }
        }
    }
}