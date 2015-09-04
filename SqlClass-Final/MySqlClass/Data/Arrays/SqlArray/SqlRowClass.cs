using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

#pragma warning disable 1591

namespace nTools.SqlTools
{
    public class SqlRowClass : Dictionary<string, string>
    {

        private List<string> colList;

        public SqlRowClass(List<string> columnList)
        {
            colList = columnList;
        }

        public string this[int colIndex]
        {
            get
            {
                return base[colList[colIndex]];
            }

        }
    }
}
