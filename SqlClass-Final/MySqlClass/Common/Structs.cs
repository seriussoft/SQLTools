using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nTools.SqlTools.Common
{
    /// <summary>
    /// struct with a key and value pair
    /// </summary>
    public struct DBPair
    {
        public string Key;
        public string Value;

        /// <summary>
        /// initialize the struct with a given key and value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public DBPair(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// returns a string in the format "key=value;"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}={1};");
            //return base.ToString();
        }
    }
}
