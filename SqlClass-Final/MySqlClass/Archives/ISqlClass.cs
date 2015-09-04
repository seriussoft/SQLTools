using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

#pragma warning disable 1591

namespace nTools.SqlTools
{
    /// <summary>
    /// <para>this is the SqlClass Interface that all future SqlClasses will derive from if </para>
    /// <para>they wish to be used by the SqlBindingSource</para>
    /// </summary>
    public interface ISqlClass
    {

      #region Properties
        string[] ConnArray { get; set; }
        string ConnString { get; }
        string QueryString { get; set; }
        bool IsConnected { get; }
        int RowCount { get; }
        int ColumnCount { get; }
      #endregion

      #region Gets
        [Obsolete("This method is deprecated and will be removed in v4.2.1.\n Use ColumnCount property instead.")]
        int getColumns();
        [Obsolete("This method is deprecated and will be removed in v4.2.1.\n Use RowCount property instead.")]
        int getRows();

        DataTable getSchema();
        DataTable getDataTable();
        DataSet getDataSet();

        SqlDataClass getVar(int column);
        SqlDataClass getVar(string column);
        void getVar(int column, ref object toStore);
        void getVar(string column, ref object toStore);
      #endregion

      #region Sets
        bool setConn(string connection);
        bool setConn(string server, string userId, string pass, string db);
        void disconn();
      #endregion

      #region Others
        bool next();
        dType parseType(string sType);
        bool query(string query);
        bool command(string commandQuery);
        //string varIs(int column);
      #endregion
    }
}
