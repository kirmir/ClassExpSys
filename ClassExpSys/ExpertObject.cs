using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClassExpSys
{
    /// <summary>
    /// Perpesents object in expert system.
    /// </summary>
    public class ExpertObject
    {
        /// <summary>
        /// Gets the name of object.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the list of IDs of object attributes.
        /// </summary>
        public List<int> Attributes { get; private set; }

        /// <summary>
        /// Creates expert system object with specified name.
        /// </summary>
        /// <param name="objName">Object name.</param>
        public ExpertObject(string objName)
        {
            Name = objName;
            Attributes = new List<int>();
        }

        /// <summary>
        /// Sets attributes of object.
        /// </summary>
        /// <param name="attrStr">String with comma separated attributes IDs</param>
        public void SetAttributes(string attrStr)
        {
            Regex regex = new Regex("[0-9][0-9]*");
            MatchCollection attrs = regex.Matches(attrStr);
            try
            {
                foreach (Match attr in attrs)
                {
                    Attributes.Add(int.Parse(attr.Value));
                }
            }
            catch (Exception)
            {
                throw new FormatException("Invalid attribute ID.");
            }
        }
    }
}
