using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace System.Drawing.Html
{
    /// <summary>
    ///     Represents a block of CSS property values
    /// </summary>
    /// <remarks>
    ///     To learn more about CSS blocks visit CSS spec:
    ///     http://www.w3.org/TR/CSS21/syndata.html#block
    /// </remarks>
    public class CssBlock
    {
        #region Fields

        #endregion

        #region Ctor

        /// <summary>
        ///     Initializes internal's
        /// </summary>
        private CssBlock()
        {
            PropertyValues = new Dictionary<PropertyInfo, string>();
            Properties = new Dictionary<string, string>();
        }

        /// <summary>
        ///     Creates a new block from the block's source
        /// </summary>
        /// <param name="blockSource"></param>
        public CssBlock(string blockSource)
            : this()
        {
            BlockSource = blockSource;

            //Extract property assignments
            var matches = Parser.Match(Parser.CssProperties, blockSource);

            //Scan matches
            foreach (Match match in matches)
            {
                //Split match by colon
                var chunks = match.Value.Split(':');

                if (chunks.Length != 2)
                {
                    continue;
                }

                //Extract property name and value
                var propName = chunks[0].Trim();
                var propValue = chunks[1].Trim();

                //Remove semicolon
                if (propValue.EndsWith(";"))
                {
                    propValue = propValue.Substring(0, propValue.Length - 1).Trim();
                }

                //Add property to list
                Properties.Add(propName, propValue);

                //Register only if property checks with reflection
                if (CssBox._properties.ContainsKey(propName))
                {
                    PropertyValues.Add(CssBox._properties[propName], propValue);
                }
            }
        }

        #endregion

        #region Props

        /// <summary>
        ///     Gets the properties and its values
        /// </summary>
        public Dictionary<string, string> Properties { get; }

        /// <summary>
        ///     Gets the dictionary with property-ready values
        /// </summary>
        public Dictionary<PropertyInfo, string> PropertyValues { get; }


        /// <summary>
        ///     Gets the block's source
        /// </summary>
        public string BlockSource { get; }

        #endregion

        #region Method

        /// <summary>
        ///     Updates the PropertyValues dictionary
        /// </summary>
        internal void UpdatePropertyValues()
        {
            PropertyValues.Clear();

            foreach (var prop in Properties.Keys)
            {
                if (CssBox._properties.ContainsKey(prop))
                {
                    PropertyValues.Add(CssBox._properties[prop], Properties[prop]);
                }
            }
        }

        /// <summary>
        ///     Asigns the style on this block o the specified box
        /// </summary>
        /// <param name="b"></param>
        public void AssignTo(CssBox b)
        {
            foreach (var prop in PropertyValues.Keys)
            {
                var value = PropertyValues[prop];

                if (value == CssConstants.Inherit && b.ParentBox != null)
                {
                    value = Convert.ToString(prop.GetValue(b.ParentBox, null));
                }

                prop.SetValue(b, value, null);
            }
        }

        #endregion
    }
}