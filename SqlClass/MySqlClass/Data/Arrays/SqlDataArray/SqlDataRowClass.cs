using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace nTools.SqlTools
{
    public class SqlDataRowClass<String, T> : Dictionary<String, T>
    {
        private List<String> colList;

        public SqlDataRowClass(List<String> columnList)
        {
            colList = columnList;
        }

        public T this[int colIndex]
        {
            get
            {
                return base[colList[colIndex]];
            }

        }
    }

}
