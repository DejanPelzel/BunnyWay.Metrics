using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyWay.Metrics
{
    /// <summary>
    /// Tag representation for the metrics
    /// </summary>
    public struct Tag
    {
        /// <summary>
        /// The name of the tag
        /// </summary>
        public string TagName;
        /// <summary>
        /// The value of the tag
        /// </summary>
        public string Value;

        /// <summary>
        /// Create a new Tag object
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="value"></param>
        public Tag(string tagName, string value)
        {
            this.TagName = tagName;
            this.Value = value;
        }

        /// <summary>
        /// Get an encoded data string to use with InfluxDBWriter
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return (this.TagName + "=" + this.Value).Replace(" ", "\\ ").Replace(",", "\\,");
        }
    }
}
