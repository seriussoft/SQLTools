using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
//using System.Data.Sql;


namespace nTools.SqlTools.Interfaces
{
    public interface IResultSet
    {
        int RowCount { get; }
        int ColumnCount { get; }
        string[] Keys { get; }

        DataSet sDataSet { get; }
        DataTable SchemaTable { get; }
        DataTable tDataTable { get; }
        IEnumerable<SqlRow> Rows { get; }
        IDbDataAdapter DataAdapter { get; }

        SqlDataClass this[int index] { get; }
        SqlDataClass this[string key] { get; }

        SqlDataClass GetValue(int col);
        SqlDataClass GetValue(string colKey);

        string GetString(int col);
        string GetString(string colKey);

        void SetResultSet(string name, int rowCnt, IDataReader dataReader);

        //void GetValue(int col, out object toStore);
        
        //void GetValue(string colKey, out object toStore);

        bool Next();

        //static dType ParseType(string colKey);


    }
}
