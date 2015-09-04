using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
//using System.Collections.Generic;

#pragma warning disable 1591

namespace nTools.SqlTools
{
    public class SqlDataRowClass<String, T> : Dictionary<String, T>
    {
        private List<String> _colList;

        public SqlDataRowClass(List<String> columnList)
        {
            _colList = columnList;
        }

        public T this[int colIndex]
        {
            get
            {
                return base[_colList[colIndex]];
            }

        }
    }

    /*
    public class CityCollection : IEnumerable<string>
    {
        string[] m_Cities = { "New York", "Paris", "London" };
        public IEnumerator<string> GetEnumerator()
        {
            for (int i = 0; i < m_Cities.Length; i++)
                yield return m_Cities[i];
        }
        public IEnumerator IEnumerable.GetEnumerator()
        {
            for(int i = 0; i < m_Cities.Length; i++)
                yield return m_Cities[i];
        }
    }*/


    /// <summary>Row of SqlData</summary>
    public class SqlRow //: IEnumerable<SqlDataClass>
    {
        #region Fields
        private Dictionary<String, SqlDataClass> _sqlDataList;
        private string[] _keyArr;
        //private int _count = 0;
        #endregion

        #region Properties and Indexes
        /// <summary>gets the number of fields in the current row</summary>
        public int Count
        {
            get { return _keyArr.Length; }
        }

        /// <summary>gets or sets the value referenced by the key</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SqlDataClass this[string key]
        {
            get { return _sqlDataList[key]; }
            set { _sqlDataList[key] = value; }
        }

        /// <summary>gets or sets the value referenced by the column #</summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public SqlDataClass this[int column]
        {
            get { return this[_keyArr[column]];}
            set { this[_keyArr[column]] = value; }
        }
        #endregion

        /// <summary>Enumerator for the current row</summary>
        /// <returns></returns>
        public IEnumerable<SqlDataClass> Fields()
        {
            foreach (SqlDataClass dv in _sqlDataList.Values)
            {
                yield return dv;
            }
        }

        /// <summary>Empty constructor</summary>
        public SqlRow()
        {
            _keyArr = new string[] { };
            _sqlDataList = new Dictionary<string, SqlDataClass>();
        }

        /// <summary>applies an array of keys to the row</summary>
        /// <param name="keys"></param>
        public void setKeys(string[] keys)
        {
            _sqlDataList = new Dictionary<string, SqlDataClass>();
            _keyArr = keys;
        }

        /// <summary>sets the key at the referenced column number</summary>
        /// <param name="colNum"></param>
        /// <param name="key"></param>
        public void setKey(int colNum, string key)
        {
            _keyArr[colNum] = key;
        }

        #region Add / Set Methods

        /// <summary>sets value of or adds value to the row using the column key as reference</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void setValue(string key, SqlDataClass value)
        {
            if (_sqlDataList.ContainsKey(key))
            {
                _sqlDataList[key] = value;
            }
            else
            {
                _sqlDataList.Add(key, value);
            }
        }

        /// <summary>sets value of or adds value to the row using the column number as reference</summary>
        /// <param name="col"></param>
        /// <param name="value"></param>
        public void setValue(int col, SqlDataClass value)
        {
            setValue(_keyArr[col], value);
        }

        /// <summary>adds value to or sets value of the row using the column key as reference</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void addValue(string key, SqlDataClass value)
        {
            //calls setValue to do the check, so if it exists, it gets changed, else it gets added
            setValue(key, value);
        }

        /// <summary>adds value to the row using the column number as reference</summary>
        /// <param name="col"></param>
        /// <param name="value"></param>
        public void addValue(int col, SqlDataClass value)
        {
            addValue(_keyArr[col], value);
        }

        #endregion



    }

}
