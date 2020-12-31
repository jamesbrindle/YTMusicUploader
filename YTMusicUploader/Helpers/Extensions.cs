using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace System
{
    /// <summary>
    /// Application Extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Easily change type
        /// </summary>
        public static T To<T>(this IConvertible obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        /// <summary>
        /// Converts string to int
        /// </summary>
        public static int ToInt(this string str)
        {
            int.TryParse(str, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out int retNum);
            return retNum;
        }

        /// <summary>
        /// Converts string to double
        /// </summary>
        public static double ToDouble(this string str)
        {
            double.TryParse(str, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out double retNum);
            return retNum;
        }

        /// <summary>
        /// Convert boolean to int (0 or 1)
        /// </summary>
        public static int ToBoolAsInt(this bool boolean)
        {
            return Convert.ToInt32(boolean);
        }

        /// <summary>
        /// Convert string representation of boolean to int (0 or 1)
        /// </summary>
        public static int ToBoolAsInt(this string booleanAsString)
        {
            if (booleanAsString.ToLower() == "false" || booleanAsString == "0")
            {
                return 0;
            }

            if (booleanAsString.ToLower() == "true" || booleanAsString == "1")
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        ///  Convert string to boolean
        /// </summary>
        public static bool ToBool(this string booleanAsString)
        {
            return Convert.ToBoolean(ToBoolAsInt(booleanAsString));
        }

        /// <summary>
        ///  Convert int to boolean
        /// </summary>
        public static bool ToBool(this int booleanAsInt)
        {
            return Convert.ToBoolean(booleanAsInt);
        }

        /// <summary>
        /// Adds quotes around string
        /// </summary>
        public static string Quote(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }

            if (s.IndexOf(' ') < 0)
            {
                return s;
            }

            return "\"" + s + "\"";
        }

        /// <summary>
        /// Removes and ", ”, “, ' (quotes) from the string
        /// </summary>
        public static string UnQuote(this string s)
        {
            if (s == null)
                return s;

            return Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(s, "“", ""), "”", ""), "\"", ""), "'", "");
        }

        /// <summary>
        /// 4 digit short date
        /// </summary>
        public static string ToShortDate(this string s)
        {
            if (string.IsNullOrEmpty(s) || s.Length < 4)
            {
                return "----";
            }

            return s.Substring(0, 4);
        }

        /// <summary>
        /// Converts string to Date of DateTime
        /// </summary>
        public static DateTime ParseDate(this string dateStr)
        {
            string[] options = {
                "dd/MM/yyyy",
                "dd-MM-yyyy",
                "dd.MM.yyyy" };

            return DateTime.ParseExact(dateStr.Substring(0, 10), options, CultureInfo.GetCultureInfo("en-GB"), DateTimeStyles.None);
        }

        /// <summary>
        /// Convert to SQL acceptable DateTime string
        /// </summary>
        public static string ToSQLDateTime(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// Convert to JavaScript acceptable DateTime string
        /// </summary>
        public static double ToJavaScriptNumberDate(this DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>
        /// Extension of the SQL 'IN' statement
        /// 
        /// Usage: if ("who".In("what", "who", "where")) { }
        /// 
        /// </summary>
        public static bool In<T>(this T item, params T[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            return items.Contains(item);
        }

        /// <summary>
        /// Use regular expression to find the 'IndexOf' rather than just a static string
        /// </summary>
        public static int RegexIndexOf(this string str, string pattern)
        {
            var m = Regex.Match(str, pattern);
            return m.Success ? m.Index : -1;
        }

        /// <summary>
        /// Use regular expression to find the 'IndexOf' rather than just a static string
        /// </summary>
        public static int RegexIndexOf(this string str, string pattern, int startIndex)
        {
            var m = Regex.Match(str.Substring(startIndex, str.Length - 1 - startIndex), pattern);
            return m.Success ? m.Index : -1;
        }

        /// <summary>
        /// Equivelant to doing str.Contains("somme substring") || str.Contains("another substring") in a cleaner way and is case insensitive
        /// </summary>
        public static bool StringContainsIn(this string inputStr, params string[] containsList)
        {
            if (containsList == null)
            {
                throw new ArgumentNullException("items");
            }

            for (int i = 0; i < containsList.Length; i++)
            {
                if (inputStr.ToLower().Contains(containsList[i].ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// // Enable quick and more natural string.Format calls.
        /// 
        /// I.e. "The co-ordinate is ({0}, {1})".Format(point.X, point.Y);
        /// </summary>
        public static string QFormat(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        /// <summary>
        /// Returns a list of start indexes of all appearences of substring
        /// </summary>
        /// <param name="source"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public static List<int> GetAllIndexesOfString(this string source, string searchString)
        {
            var ret = new List<int>();
            int len = searchString.Length;
            int start = -len;
            while (true)
            {
                start = source.IndexOf(searchString, start + len);
                if (start == -1)
                {
                    break;
                }
                else
                {
                    ret.Add(start);
                }
            }
            return ret;
        }

        /// <summary>
        /// Counts the number of occurrences of a substring in the current string
        /// </summary>
        /// <param name="text">Input text</param>
        /// <param name="pattern">String to match on</param>
        /// <returns>Int count</returns>
        public static int CountSubstringOccurrences(this string text, string pattern)
        {
            if (text == null || pattern == null)
            {
                return 0;
            }

            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;

            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }

            return count;
        }

        /// <summary>
        /// Returns the string between two string. I.e. StringBetween("foosumbar", "foo", "bar") would return "sum"), 
        /// </summary>
        /// <param name="text">This string text</param>
        /// <param name="firstString">First part to match</param>
        /// <param name="lastString">Last part to match</param>
        /// <returns>The string inbetween</returns>
        public static string StringBetween(this string text, string firstString, string lastString)
        {
            string FinalString;

            if (firstString != lastString)
            {
                int Pos1 = text.IndexOf(firstString) + firstString.Length;
                int Pos2 = text.IndexOf(lastString);
                FinalString = text.Substring(Pos1, Pos2 - Pos1);
            }
            else
            {
                var positions = text.GetAllIndexesOfString(firstString);

                int Pos1 = positions[0] + firstString.Length;
                int Pos2 = positions[1];
                FinalString = text.Substring(Pos1, Pos2 - Pos1);
            }

            return FinalString;
        }

        /// <summary>
        /// Turns each first character of all words in a string to uppercase based on CultureInfo
        /// </summary>
        public static string ToTitleCase(this string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }

        /// <summary>
        /// Converts string to filename save string
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="replaceChar"></param>
        /// <returns></returns>
        public static string ToSafeFilename(this string filename)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, '_');
            }
            return filename;
        }

        /// <summary>
        /// Takes a camelCaseWord or PascalCaseWord and "wordifies" it, ie camelCaseWord => Camel Case Word
        /// 
        /// Useful for converting object name or SQL table headers to proper words
        /// </summary>
        public static string Wordify(this string text)
        {
            // if the word is all upper, just return it
            if (!Regex.IsMatch(text, "[a-z]"))
            {
                return text;
            }

            string returnStr = string.Join(" ", Regex.Split(text, @"(?<!^)(?=[A-Z])"));
            return returnStr[0].ToString().ToUpper() + returnStr.Substring(1).Replace("_", " ").Replace("  ", " ");
        }

        /// <summary>
        /// Returns whether or not a given string purely contains number digits
        /// </summary>
        /// <param name="text">This string object</param>
        /// <returns>True or false</returns>
        public static bool IsNumeric(this string text)
        {
            bool isNum = Int32.TryParse(Convert.ToString(text), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out _);
            return isNum;
        }


        /// <summary>
        /// Removes any numbers from string
        /// </summary>
        public static string RemoveNumbers(this string text)
        {
            return Regex.Replace(text, @"[\d-]", string.Empty);
        }

        /// <summary>
        /// Removes any numbers from string
        /// </summary>
        public static string RemoveLetters(this string text)
        {
            return Regex.Replace(text, "[^0-9.]", string.Empty);
        }

        /// <summary>
        /// Removes any numbers from string
        /// </summary>
        public static string StripNumbers(this string text)
        {
            string a = text;
            string b = string.Empty;

            for (int i = 0; i < a.Length; i++)
            {
                if (Char.IsDigit(a[i]))
                {
                    b += a[i];
                }
            }

            return b;
        }

        /// <summary>
        /// Shortens a string adding ... to the end if the number of characters is greater than specified
        /// </summary>
        public static string Ellipse(this string text, int maxCharacters)
        {
            if (text.Length > maxCharacters)
                return text.Substring(0, maxCharacters) + "...";
            return text;
        }

        /// <summary>
        /// Shortens a file path, splitting the path with ... if it's above the max characters cap.
        /// E.g: C:\Documents\...\Photos\Cats
        /// </summary>
        public static string EllipsisPath(this string filePath, int maxLength = 100)
        {
            if (filePath.Length > maxLength)
            {
                // Find last '\' character
                int i = filePath.LastIndexOf('\\');

                string tokenRight = filePath.Substring(i, filePath.Length - i);
                string tokenCenter = @"\...";
                string tokenLeft = filePath.Substring(0, maxLength - (tokenRight.Length + tokenCenter.Length));

                return tokenLeft + tokenCenter + tokenRight;
            }
            else
                return filePath;
        }

        /// <summary>
        /// Remove XML or HTML tags and returns just the inner text
        /// </summary>
        public static string StripHtmlTags(this string str)
        {
            str = Regex.Replace(str, @"<[^>]*>", string.Empty);
            return str;
        }

        /// <summary>
        /// Removes any characters not a number or a letter of the alphabet from a string
        /// </summary>
        public static string RemoveNonAlphaNumerics(this string text)
        {
            var rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(text, "");
        }

        /// <summary>
        /// Returns whether or not a given string contains number digits
        /// </summary>
        /// <param name="text">This string object</param>
        /// <returns>True or false</returns>
        public static bool ContainsNumbers(this string text)
        {
            foreach (char c in text)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns whether or not a given string contains letters
        /// </summary>
        /// <param name="text">This string object</param>
        /// <returns>True or false</returns>
        public static bool ContainsLetters(this string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Similar to 'Parallel.ForEach' but maintains the order in which the collection is iterated / processed.
        /// It doesn't guarentee the the actions will finish in the same order, just that they're iterated over
        /// in order.
        /// 
        /// Usage:  
        ///     orderedElements.AsParallel().ForAllInApproximateOrder(e => DoSomething(e ) );
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForAllInApproximateOrder<TSource>(this ParallelQuery<TSource> source, Action<TSource> action, int? maxDegreesOfParallelism = null)
        {
            if (maxDegreesOfParallelism == null)
            {
                Partitioner.Create(source)
                           .AsParallel()
                           .AsOrdered()
                           .ForAll(e => action(e));
            }
            else
            {
                Partitioner.Create(source)
                           .AsParallel()
                           .AsOrdered()
                           .WithDegreeOfParallelism((int)maxDegreesOfParallelism)
                           .ForAll(e => action(e));
            }
        }

        /// <summary>
        /// Gets the 00:00:00:000 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteStart(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// Gets the 11:59:59:999 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteEnd(this DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Converts single apostrophes to double apostrophes
        /// </summary>
        /// <param name="text">String to check / convert</param>
        /// <returns>SQL accepted string</returns>
        public static string GetSQLAcceptableString(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = text.Replace("'", "''");
            text = text.Replace("''''", "''");
            text = text.Replace("''''''", "''");
            text = text.Replace("''''''''", "''");
            text = text.Replace("''''''''''", "''");
            text = text.Replace("''''''''''''", "''");

            return text;
        }

        /// <summary>
        /// Handles backslash quanity - C# to JavaScript (or Json)
        /// </summary>
        /// <param name="text">String to check / convert</param>
        /// <returns>Javascript accepted string</returns>
        public static string EscapeJavascript(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = text.Replace("'", "\\'");

            text = text.Replace("\\\\\\\\\\", "\\");
            text = text.Replace("\\\\\\\\", "\\");
            text = text.Replace("\\\\\\", "\\");
            text = text.Replace("\\", "\\\\");
            text = text.Replace("\\\\'", @"\'");

            text = text.Replace("\\\b", "\\\\b");
            text = text.Replace("\\\f", "\\\\f");
            text = text.Replace("\\\n", "\\\\n");
            text = text.Replace("\\\r", "\\\\r");
            text = text.Replace("\\\t", "\\\\t");
            text = text.Replace("\\\v", "\\\\v");

            text = text.Replace("\b", "\\b");
            text = text.Replace("\f", "\\f");
            text = text.Replace("\n", "\\n");
            text = text.Replace("\r", "\\r");
            text = text.Replace("\t", "\\t");
            text = text.Replace("\v", "\\v");

            return text;
        }

        /// <summary>
        /// Handles backslash quanity - JavaScript (or Json) to C#
        /// </summary>
        /// <param name="text">String to check / convert</param>
        /// <returns>Javascript accepted string</returns>
        public static string UnescapeJavaScript(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            string unescaped = string.Empty;
            char last1 = new char();
            char last2 = new char();

            for (int i = 0; i < text.Length; i++)
            {
                if (i == 0)
                {
                    unescaped += text[i];
                    last1 = text[i];
                    continue;
                }

                if (text[i].In('n', 'b', 'f', 'r', 't', 'v') && last1 == '\\')
                {
                    if (last2 != '\\')
                    {
                        unescaped = unescaped.Substring(0, unescaped.Length - 1);

                        switch (text[i])
                        {
                            case 'n':
                                unescaped += '\n';
                                break;
                            case 'b':
                                unescaped += '\b';
                                break;
                            case 'f':
                                unescaped += '\f';
                                break;
                            case 'r':
                                unescaped += '\r';
                                break;
                            case 't':
                                unescaped += '\t';
                                break;
                            case 'v':
                                unescaped += '\v';
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        unescaped += text[i];
                    }

                    last2 = last1;
                    last1 = new char();
                }
                else
                {
                    unescaped += text[i];
                    last2 = last1;
                    last1 = text[i];
                }
            }

            return unescaped.Replace("\\\\", "\\");
        }

        /// <summary>
        /// Add comma seperated items to a list. I.e. list.AddRange(5, 4, 8, 4, 2);
        /// </summary>
        public static void AddRange<T, S>(this ICollection<T> list, params S[] values) where S : T
        {
            foreach (var value in values)
            {
                list.Add(value);
            }
        }

        /// <summary>
        /// Makes string XML legal
        /// </summary>
        public static string SanitizeXmlString(this string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            var buffer = new StringBuilder(xml.Length);

            foreach (char c in xml)
            {
                if (IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Is Character XML legal?
        /// </summary>
        public static bool IsLegalXmlChar(this int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }

        /// <summary>
        /// Checks whether JSON is legally formatted
        /// </summary>
        public static bool IsValidJson(this string text)
        {
            try
            {
                JToken.Parse(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks whether XML is legally formatted
        /// </summary>
        public static bool IsValidXml(this string text)
        {
            try
            {
                XDocument.Parse(text);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="dataTable">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> ToList<T>(this DataTable dataTable) where T : class, new()
        {
            try
            {
                var list = new List<T>();

                foreach (var row in dataTable.AsEnumerable())
                {
                    var obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            var propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Create Datatable from an IEnumerable collection
        /// </summary>
        /// <typeparam name="T">Type of list</typeparam>
        /// <param name="list">The list to convert</param>
        /// <returns>DataTable of list</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            var type = typeof(T);
            var properties = type.GetProperties();

            var dataTable = new DataTable();
            foreach (var info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (var entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        /// <summary>
        ///  Create datatable from any object
        /// </summary>
        /// <param name="obj">Object to convert to datatable</param>
        /// <returns>Datatable</returns>
        public static DataTable ToDataTable(this object obj)
        {
            var dt = new DataTable("OutputData");

            var dr = dt.NewRow();
            dt.Rows.Add(dr);

            obj.GetType().GetProperties().ToList().ForEach(f =>
            {
                try
                {
                    f.GetValue(obj, null);
                    dt.Columns.Add(f.Name, f.PropertyType);
                    dt.Rows[0][f.Name] = f.GetValue(obj, null);
                }
                catch { }
            });
            return dt;
        }

        /// <summary>
        /// Convert any object to dynamic object
        /// </summary>
        /// <param name="value">Any object</param>
        /// <returns>The dynamic object</returns>
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
            {
                expando.Add(property.Name, property.GetValue(value));
            }

            return expando as ExpandoObject;
        }

        /// <summary>
        /// Clones an object into an brand new instance (avoid issues with referencing of object)
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="source">The object to clone</param>
        /// <returns>A brand new instance of the object</returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
#pragma warning disable IDE0041 // Use 'is null' check
            if (Object.ReferenceEquals(source, null))
#pragma warning restore IDE0041 // Use 'is null' check
            {
#pragma warning disable IDE0034 // Simplify 'default' expression
                return default(T);
#pragma warning restore IDE0034 // Simplify 'default' expression
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();

            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Converts property dictionary to dynamic object
        /// </summary>
        public static dynamic ToDynamicObject(this Dictionary<string, object> properties)
        {
            return new MyDynObject(properties);
        }

        private sealed class MyDynObject : DynamicObject
        {
            private readonly Dictionary<string, object> _properties;

            public MyDynObject(Dictionary<string, object> properties)
            {
                _properties = properties;
            }

            public override IEnumerable<string> GetDynamicMemberNames()
            {
                return _properties.Keys;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    result = _properties[binder.Name];
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    _properties[binder.Name] = value;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
