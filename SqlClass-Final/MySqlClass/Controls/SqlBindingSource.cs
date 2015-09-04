using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

using nTools.SqlTools.Archives;

#pragma warning disable 1591

namespace nTools.SqlTools
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class SqlBindingSource : BindingSource
    {
      #region Fields
        Connection conn = new Connection();
        dbType databaseType = dbType.None;
        string query = "";
        ISqlClass sqlClass;
      #endregion

      #region Properties
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("SqlData"), DefaultValue("")]
        public string Query
        {
            get { return this.query; }
            set
            {
                if (query != value)
                {
                    query = value;
                }
            }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("SqlData"), DefaultValue(dbType.None)]
        public dbType DatabaseType
        {
            get { return this.databaseType; }
            set 
            { 
                databaseType = value;
                switch (databaseType)
                {
                    case dbType.MySql: 
                        sqlClass = new MySqlClass();
                        break;
                    case dbType.MsSql:
                        sqlClass = new MsSqlClass();
                        break;
                    default :
                        //do nothing????
                        break;
                }

                //set connection if using mysql/mssql and if connection exists...
                //if there is a query, then run it and update the DataSource
                if (this.databaseType != dbType.None && this.conn.Password != "" && this.conn.Schema != "" && this.conn.Server != "" && this.conn.UserID != "")
                {
                    this.sqlClass.setConn(conn.getConn(this.databaseType));

                    if (this.query != "")
                    {
                        this.sqlClass.query(this.query);
                        //this.Clear();
                        this.DataSource = this.sqlClass.getDataTable();
                        //this.Dispose();
                        //this.ResetBindings(false);
                    }
                    else
                    {
                        //this.Clear();
                        //this.Dispose();
                    }
                }
                else
                {
                    try
                    {
                        //this.Clear();
                        //this.Dispose();
                    }
                    catch { }
                }

                
                
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("SqlData")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Connection Conn
        {
            get
            {
                return this.conn;
            }

            set
            {
                this.conn = value;

                //set connection if using mysql/mssql and if connection exists...
                //if there is a query, then run it and update the DataSource
                if (this.databaseType != dbType.None && this.conn.Password != "" && this.conn.Schema != "" && this.conn.Server != "" && this.conn.UserID != "")
                {
                    this.sqlClass.setConn(conn.getConn(this.databaseType));

                    if (this.query != "")
                    {
                        this.sqlClass.query(this.query);
                        //this.Clear();
                        this.DataSource = this.sqlClass.getDataTable();
                        //this.Dispose();
                        //this.ResetBindings(false);
                    }
                    else
                    {
                        //this.Clear();
                        //this.Dispose();
                    }
                }
            }
        }//end property
      #endregion

      #region Methods
        public void refresh()
        {
            try
            {
                dbType temp = this.DatabaseType;
                this.DatabaseType = dbType.None;
                this.DatabaseType = temp;
            }
            catch (Exception ea)
            {
                throw new Exception("refresh(): \n" + ea.Message);
            }
        }
      #endregion

    }//end class



    [TypeConverter(typeof(ConnectionConverter))]
    public class Connection
    {
        #region Fields
        private string server = "",
                       port = "",
                       userID = "",
                       pass = "",
                       schema = "",
                       mySqlConnStr = "",
                       msSqlConnStr = "";
        #endregion

        #region UnEditable Properties
        public string MySqlConnStr
        {
            get { return mySqlConnStr; }
        }

        public string MsSqlConnStr
        {
            get { return msSqlConnStr; }
        }
        #endregion

        private void makeString()
        {
            mySqlConnStr = "SERVER=" + server;
            msSqlConnStr = "Data Source=" + server;

            if (port.Trim() != "")
            {
                mySqlConnStr += ";PORT=" + port;
                //msSqlConnstr += ";=" + port;
            }

            mySqlConnStr += ";UID=" + userID + ";PASSWORD=" + pass + ";DATABASE=" + schema + ";";
            msSqlConnStr += ";User ID=" + userID + ";Password=" + pass + ";Initial Catalog=" + schema + ";";
        }

        public string getConn(dbType databaseType)
        {
            switch (databaseType)
            {
                case dbType.MySql:
                    return this.mySqlConnStr;
                    //break;
                case dbType.MsSql:
                    return this.msSqlConnStr;
                    //break;
                default:
                    return "";
                    //break;
            }
        }

        #region Editable Properties
        [Browsable(true),NotifyParentProperty(true),EditorBrowsable(EditorBrowsableState.Always),DefaultValue("")]
        public string Server
        {
            get
            {
                return server;
            }
            set
            {
                if (value == "localhost" || value == "127.0.0.1")
                {
                    if (server != "127.0.0.1" && server != "localhost")
                    {
                        server = "127.0.0.1";
                        makeString();
                    }
                }

                if (server != value)
                {
                    server = value;
                    makeString();
                }
            }
        }

        [Browsable(true),NotifyParentProperty(true),EditorBrowsable(EditorBrowsableState.Always),DefaultValue("")]
        public string Port
        {
            get
            {
                return port;
            }
            set
            {
                if (port != value)
                {
                    port = value;
                    makeString();
                }
            }
        }

        [Browsable(true), NotifyParentProperty(true), EditorBrowsable(EditorBrowsableState.Always), DefaultValue("")]
        public string UserID
        {
            get
            {
                return userID;
            }
            set
            {
                if (userID != value)
                {
                    userID = value;
                    makeString();
                }
            }
        }

        [Browsable(true), NotifyParentProperty(true), EditorBrowsable(EditorBrowsableState.Always), DefaultValue("")]
        public string Password
        {
            get
            {
                return pass;
            }
            set
            {
                if (pass != value)
                {
                    pass = value;
                    makeString();
                }
            }
        }

        [Browsable(true), NotifyParentProperty(true), EditorBrowsable(EditorBrowsableState.Always), DefaultValue("")]
        public string Schema
        {
            get
            {
                return schema;
            }
            set
            {
                if (schema != value)
                {
                    schema = value;
                    makeString();
                }
            }
        }
        #endregion
    }

    public class ConnectionConverter : ExpandableObjectConverter
    {
        // This override prevents the PropertyGrid from 
        // displaying the full type name in the value cell.
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return "";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

}
