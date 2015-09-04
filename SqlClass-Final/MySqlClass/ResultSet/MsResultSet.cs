using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using System.Data.DataSetExtensions;
using System.Data.SqlClient;

using nTools.SqlTools.Interfaces;

#pragma warning disable 1591

namespace nTools.SqlTools.Common
{

    /*System.Data.SqlClient.SqlDataReader,*/
    public class MsResultSet : IResultSet 
    {
		#region Fields (8) 

        private int _columnCount = 0;
        private SqlDataAdapter _dataAdapter;
        private SqlDataReader _dataReader;
        private DataSet _dataSet;
        private string[] _keys;
        private string _name;
        private int _rowCount = 0;
        private DataTable _schemaTable;

		#endregion Fields 

		#region Properties (10) 

        /// <summary>
        /// gets number of columns returned by latest query
        /// </summary>
        public int ColumnCount { get { return _columnCount; } }

        public IDbDataAdapter DataAdapter
        {
            get { return _dataAdapter as IDbDataAdapter; }
        }

        /// <summary>
        /// gets string[] of keys for the columns
        /// </summary>
        public string[] Keys { get { return _keys; } }

        /// <summary>
        /// gets number of rows returned by latest query
        /// </summary>
        public int RowCount { get { return _rowCount; } }

        /// <summary>
        /// Enumerable Collection of SqlRows
        /// </summary>
        public IEnumerable<SqlRow> Rows
        {
            get
            {
                int rowMax = _dataReader.FieldCount;
                SqlRow row;

                do
                {
                    row = new SqlRow();
                    //string[] rows = new string[tmp.mySqlReader.FieldCount];

                    row.setKeys(new string[rowMax]);

                    for (int x = 0; x < rowMax; x++)
                    {
                        row.setKey(x, _dataReader.GetName(x));
                        row.setValue(x, GetValue(x));
                    }

                    yield return row;
                }
                while (Next());
            }
        }

        /// <summary>
        /// gets the SchemaTable
        /// </summary>
        public DataTable SchemaTable { get { return _schemaTable; } }

        /// <summary>
        /// gets the DataSet
        /// </summary>
        public DataSet sDataSet { get { return _dataSet; } }

        /// <summary>
        /// gets the DataTable
        /// </summary>
        public DataTable tDataTable { get { return sDataSet.Tables[0]; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnKey"></param>
        /// <returns></returns>
        public SqlDataClass this[string columnKey]
        {
            get
            {
                return new SqlDataClass();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public SqlDataClass this[int columnIndex]
        {
            get
            {
                return new SqlDataClass();
            }
        }

		#endregion Properties 

		#region Constructors (1) 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rowCount"></param>
        /// <param name="dataReader"></param>
        public MsResultSet(string name, int rowCount, SqlDataReader dataReader)
        {
            InitResultSet(name, rowCount, dataReader);
        }

		#endregion Constructors 

		#region Methods (7) 

		#region Public Methods (6) 

        public string GetString(int columnKey)
        {
            return "";
        }

        public string GetString(string columnKey)
        {
            return "";
        }

        public SqlDataClass GetValue(string columnKey)
        {
            return new SqlDataClass();
        }

        public SqlDataClass GetValue(int columnIndex)
        {
            return new SqlDataClass();
        }

        public bool Next()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rowCount"></param>
        /// <param name="dataReader"></param>
        public void SetResultSet(string name, int rowCount, IDataReader dataReader)
        {
            InitResultSet(name, rowCount, dataReader as SqlDataReader);
        }

		#endregion Public Methods 
		#region Protected Methods (1) 

        /// <summary>
        /// call by constructor and Set function to initialize the resultSet
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rowCount"></param>
        /// <param name="dataReader"></param>
        protected void InitResultSet(string name, int rowCount, SqlDataReader dataReader)
        {
            _dataReader = dataReader;// as MySqlDataReader;
            _name = name;
            _rowCount = rowCount;

            _schemaTable = _dataReader != null ? _dataReader.GetSchemaTable() : null;

            _keys = _schemaTable != null ? new List<string>((from row in _schemaTable.AsEnumerable() select (string)row["ColumnName"])).ToArray() : null;
        }

		#endregion Protected Methods 

		#endregion Methods 
    }
}
