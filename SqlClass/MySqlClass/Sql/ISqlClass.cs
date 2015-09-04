using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

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
      #endregion

      #region Gets
        int getColumns();
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
        void setConn(string connection);
        void setConn(string server, string userId, string pass, string db);
      #endregion

      #region Others
        bool next();
        dType parseType(string sType);
        bool query(string query);
        //string varIs(int column);
      #endregion
    }
}
