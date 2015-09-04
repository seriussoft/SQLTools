#region using directives
using System;
#region System.Collections
    using System.Collections;
    using System.Collections.Generic;
#endregion
using System.Text;
#region System.Data
    using System.Data;
    using System.Data.Sql;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
#endregion
using System.Drawing.Design;
#region System.Web
    using System.Web.UI;
    using System.Web.UI.Design.WebControls;
    using System.Web.UI.WebControls;
#endregion
using System.Windows.Forms;
using System.ComponentModel;
#endregion

namespace nTools.SqlTools
{
    //[PersistChildren(false), ParseChildren(false)]
    //[Designer(typeof(DbDataSourceDesigner))]
    public class MsSqlClass : BindingSource //: SqlDataSourceControl//DataSourceControl
    {
        #region bindingClass stuff

        [Category("Data"), DefaultValue("")]
        public string[] ConnString
        {
            get { return this.connStr.Replace("Data Source=","").Replace("User ID=","").Replace("Password=","").Replace("Initial Catalog=","").Split(';'); }
            set
            {
                //throw new Exception(value);
                string[] temp = value;
                //this.setConn("Data Source=" + temp[0] + ";User ID=" + temp[1] + "; Password=" + temp[2] + "; Initial Catalog=" + temp[3] + ";");
                this.setConn(temp[0], temp[1], temp[2], temp[3]);
            }
        }

        [Category("Data"), DefaultValue("")]
        public string QueryString
        {
            get { return this.queryString; }
            set
            {
                if (this.query(value))
                {
                    this.DataSource = this.getDataTable();
                }
            }
        }

        #endregion

        #region Fields

        private bool connStatus;
            private string connStr;
            private int current;
            private bool isRead;
            private string queryString;
            protected static SqlConnection SqlConn;
            protected SqlCommand SqlQuery;
            protected SqlDataReader SqlReader;

            /*
            #region new stuff
            protected static readonly string[] views = { "DefaultView" };
            protected DBView view;
            #endregion
            */


        #endregion

        #region Properties

            
            #region new stuff
            /*
            protected DBView View
            {
                get
                {
                    if (view == null)
                    {
                        view = new DBView(this, views[0]);

                        if (base.IsTrackingViewState)
                        {
                            ((IStateManager)view).TrackViewState();
                        }
                    }

                    return view;
                }
            }

            [Category("Data"), DefaultValue("")]
            public string TypeName
            {
                get { return View.TypeName; }
                set { View.TypeName = value; }
            }

            [Category("Data"), DefaultValue("")]
            public string SelectMethod
            {
                get { return View.SelectMethod; }
                set { View.SelectMethod = value; }
            }

            [PersistenceMode(PersistenceMode.InnerProperty),
             Category("Data"),
             DefaultValue((string)null),
             MergableProperty(false),
             Editor(typeof(ParameterCollectionEditor), typeof(UITypeEditor))]
            public ParameterCollection SelectParameters
            {
                get { return View.SelectParameters; }
            }
            */
            #endregion
            

        #endregion

        #region Methods

            #region Cstrs

            /// <summary>
                /// does not connect to database. run setConn(string) or setConn(string,string,string,string) to conn to the db if you use this Cstr
                /// </summary>
                public MsSqlClass()
                {
                    this.DataSource = this.getDataTable();
                    this.queryString = "";
                    this.connStr = "";
                    this.current = 0;
                    this.connStatus = false;
                    this.isRead = false;
                }

                /// <summary>
                /// connects to the database. throws Exception if incapable of connecting w/ supplied connection string. (check inner exception)
                /// </summary>
                /// <param name="connection" type="string">exa. "Data Source=$server;User ID=$usrID;Password=$pass;Initial Catalog=$dbName;")</param>
                /// <exception cref="">mssql exception is in the inner exception</exception>
                public MsSqlClass(string connection)
                {
                    this.queryString = "";
                    this.current = 0;
                    this.connStatus = false;
                    this.connStr = connection;
                    this.isRead = false;
                    try
                    {
                        SqlConn = new SqlConnection(this.connStr);
                        SqlConn.Open();
                        this.connStatus = true;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        this.connStatus = false;
                        throw new Exception("MsSqlClass Cstr(string): \n", exception);
                    }
                }

                /// <summary>
                /// connects to the database. throws Exception if incapable of connecting w/ supplied info. (check inner exception)
                /// </summary>
                /// <param name="server">(string)</param>
                /// <param name="userId">(string)</param>
                /// <param name="pass">(string)</param>
                /// <param name="db">(string)</param>
                /// <exception cref="">mssql exception is in the inner exception</exception>
                public MsSqlClass(string server, string userId, string pass, string db)
                {
                    this.queryString = "";
                    this.current = 0;
                    this.connStatus = false;
                    this.isRead = false;
                    this.connStr = "Data Source=" + server + ";User ID=" + userId + ";Password=" + pass + ";Initial Catalog=" + db + ";";
                    try
                    {
                        SqlConn = new SqlConnection(this.connStr);
                        SqlConn.Open();
                        this.connStatus = true;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        this.connStatus = false;
                        throw new Exception("MsSqlClass Cstr(string,string,string,string): \n", exception);
                    }
                }

            #endregion

            /// <summary>
            /// if connected, this will disconnect the open mssql connection
            /// </summary>
            public void disconn()
            {
                if (this.isConnected())
                {
                    SqlConn.Close();
                    this.connStatus = false;
                }
            }

            #region Gets

                #region new stuff
                /*
                #region IDataSourceMethods
                protected override DataSourceView GetView(string viewName)
                {
                    if((viewName == null) || (viewName.Length !=0) && (String.Compare(viewName, "DefaultView", StringComparison.OrdinalIgnoreCase) != 0))
                    {
                        throw new ArgumentException("An invalid view was requested", "viewName");
                    }

                    return View;
                }
                
                protected override System.Collections.ICollection GetViewNames()
                {
                    //return base.GetViewNames();
                    return views;
                }
                #endregion

                #region ViewStateManagement
                protected override void LoadViewState(object savedState)
                {
                    Pair previousState = (Pair)savedState;

                    if (savedState == null)
                    {
                        base.LoadViewState(null);
                    }
                    else
                    {
                        base.LoadViewState(previousState.First);

                        if (previousState.Second != null)
                        {
                            ((IStateManager)View).LoadViewState(previousState.Second);
                        }
                    }
                }

                protected override object SaveViewState()
                {
                    Pair currentState = new Pair();

                    currentState.First = base.SaveViewState();

                    if (view != null)
                    {
                        currentState.Second = ((IStateManager)View).SaveViewState();
                    }

                    if ((currentState.First == null) && (currentState.Second == null))
                    {
                        return null;
                    }

                    return currentState;
                }

                protected override void TrackViewState()
                {
                    base.TrackViewState();

                    if (view != null)
                    {
                        ((IStateManager)View).TrackViewState();
                    }
                }
                #endregion

                #region Life Cycle Related Methods

                protected override void OnInit(EventArgs e)
                {
                    base.OnInit(e);

                    // handle the LoadComplete event to update select parameters
                    if (Page != null)
                    {
                        Page.LoadComplete += new EventHandler(UpdateParameterValues);
                    }
                }

                protected virtual void UpdateParameterValues(object sender, EventArgs e)
                {
                    SelectParameters.UpdateValues(Context, this);
                }

                #endregion

                #region Select Method

                public IEnumerable Select()
                {
                    return View.Select(DataSourceSelectArguments.Empty);
                }
                
                #endregion
                */
                #endregion

                /// <summary>
                /// gets number of columns at current rowSet
                /// </summary>
                /// <returns>(int)</returns>
                public int getColumns()
                {
                    if (this.SqlReader.HasRows.Equals(true))
                    {
                        return this.SqlReader.FieldCount;
                    }
                    return 0;
                }

                /// <summary>
                /// gets the connection string
                /// </summary>
                /// <returns>(string)</returns>
                protected string getConn()
                {
                    return this.connStr;
                }

                /// <summary>
                /// gets the number of rows in the resultSet
                /// </summary>
                /// <returns>(int)</returns>
                public int getRows()
                {
                    try
                    {
                        SqlCommand command = SqlConn.CreateCommand();
                        command.CommandText = this.queryString;
                        SqlDataReader reader = command.ExecuteReader();
                        int num = 0;
                        while (reader.Read().Equals(true))
                        {
                            num++;
                        }
                        reader.Close();
                        return num;
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }

                /// <summary>
                /// returns value @ columnn[x] of current row or result set
                /// </summary>
                /// <param name="column">(int)</param>
                /// <returns>SqlDataClass</returns>
                public SqlDataClass getVar(int column)
                {
                    int ordinal = column;
                    SqlDataClass class2 = new SqlDataClass();
                    try
                    {
                        class2.store(this.SqlReader.GetValue(ordinal).ToString(), this.parseType(this.SqlReader.GetFieldType(ordinal).ToString()));
                    }
                    catch (Exception exception)
                    {
                        this.current++;
                        Console.WriteLine(this.current + ") SoL: " + exception.Message);
                    }
                    if (!class2.getString().Equals((string)null))
                    {
                        return class2;
                    }
                    return new SqlDataClass("no_value_to_be_read", dType.String);
                }

                /// <summary>
                /// returns the schema table of the resultSet
                /// </summary>
                /// <returns>(System.Data.DataTable)</returns>
                public DataTable getSchema()
                {
                    if (this.isConnected() && this.isRead)
                    {
                        return this.SqlReader.GetSchemaTable();
                    }
                    else
                    {
                        return new DataTable("empty");
                    }
                }
                
                /// <summary>
                /// returns the dataSet from the resultSet
                /// </summary>
                /// <returns>(System.Data.DataSet)</returns>
                public DataSet getDataSet()
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter(this.SqlQuery.CommandText, this.getConn());
                        DataSet d = new DataSet();
                        da.Fill(d);

                        return d;
                    }
                    catch (Exception ea)
                    {
                        throw new Exception("getDataSet(): \n" + ea.Message);
                    }
                }

                /// <summary>
                /// returns the DataTable from the internal dataSet
                /// </summary>
                /// <returns>(System.Data.DataTable)</returns>
                public DataTable getDataTable()
                {
                    if (this.isConnected() && this.isRead)
                    {
                        return this.getDataSet().Tables[0];
                    }
                    else
                    {
                        return new DataTable("empty");
                    }
                }
                
                /// <summary>
                /// gets value at supplied column key of current row or resultSet and returns it as a SqlDataClass
                /// </summary>
                /// <param name="column">(string)</param>
                /// <returns>(int)</returns>
                public SqlDataClass getVar(string column)
                {
                    string str = column;
                    SqlDataClass class2 = new SqlDataClass();
                    try
                    {
                        class2.store(this.SqlReader[str].ToString(), this.parseType(this.SqlReader[str].GetType().ToString()));
                    }
                    catch (Exception exception)
                    {
                        this.current++;
                        Console.WriteLine(this.current + ") SoL: " + exception.Message);
                    }
                    if (!class2.getString().Equals((string)null))
                    {
                        return class2;
                    }
                    return new SqlDataClass("no_value_to_be_read", dType.String);
                }

                /// <summary>
                /// gets value @ column[x] of current row or resultSet and stores it to the referenced object
                /// </summary>
                /// <param name="column">(int)</param>
                /// <param name="toStore">(ref object)</param>
                public void getVar(int column, ref object toStore)
                {
                    int ordinal = column;
                    SqlDataClass class2 = new SqlDataClass();
                    try
                    {
                        class2.store(this.SqlReader.GetValue(ordinal).ToString(), this.parseType(this.SqlReader.GetFieldType(ordinal).ToString()));
                    }
                    catch (Exception exception)
                    {
                        this.current++;
                        Console.WriteLine(this.current + ") SoL: " + exception.Message);
                    }
                    if (!class2.getString().Equals((string)null))
                    {
                        class2.putIn(ref toStore);
                    }
                    else
                    {
                        new SqlDataClass("no_value_to_be_read", dType.String).putIn(ref toStore);
                    }
                }

                /// <summary>
                /// gets value at supplied column key, at current row, and stores it to the referenced object
                /// </summary>
                /// <param name="column"></param>
                /// <param name="toStore"></param>
                public void getVar(string column, ref object toStore)
                {
                    string str = column;
                    SqlDataClass class2 = new SqlDataClass();
                    try
                    {
                        class2.store(this.SqlReader[str].ToString(), this.parseType(this.SqlReader[str].GetType().ToString()));
                    }
                    catch (Exception exception)
                    {
                        this.current++;
                        Console.WriteLine(this.current + ") SoL: " + exception.Message);
                    }
                    if (!class2.getString().Equals((string)null))
                    {
                        class2.putIn(ref toStore);
                    }
                    else
                    {
                        new SqlDataClass("no_value_to_be_read", dType.String).putIn(ref toStore);
                    }
                }

            #endregion

            #region Others

                /// <summary>
                /// gets connection status
                /// </summary>
                /// <returns>(bool)</returns>
                public bool isConnected()
                {
                    return this.connStatus;
                }

                /// <summary>
                /// returns a copy of the MsSqlClass, and connects the receivee
                /// </summary>
                /// <param name="toCopy">(MsSqlClass)</param>
                /// <returns>(MsSqlClass)</returns>
                public MsSqlClass makeCopyOf(MsSqlClass toCopy)
                {
                    return new MsSqlClass(toCopy.getConn());
                }

                /// <summary>
                /// returns true/false of success of proceeding to next rowSet
                /// </summary>
                /// <returns>(bool)</returns>
                public bool next()
                {
                    if (this.SqlReader.Read().Equals(true))
                    {
                        this.current++;
                        return true;
                    }
                    return false;
                }

                /// <summary>
                /// returns the equivalent dType enum from the given string sType (returns dType.String if a non-existing type is supplied)
                /// </summary>
                /// <param name="sType">(string)</param>
                /// <returns>(dType)</returns>
                public dType parseType(string sType)
                {
                    sType = sType.ToLower();
                    sType = sType.Remove(0, 7);
                    switch (sType)
                    {
                        case "nvarchar":
                        case "string":
                            return dType.String;

                        case "boolean":
                        case "bool":
                            return dType.Bool;

                        case "uint32":
                        case "uint64":
                        case "byte":
                            return dType.Integer;

                        case "single":
                            return dType.Double;
                    }
                    return dType.String;
                }

                /// <summary>
                /// attempts to execute the supplied query (will throw an exception if it cannot execute it: check inner exception for more info).
                /// it returns true on success or false if you are not connected.
                /// </summary>
                /// <param name="toQuery">(string)</param>
                /// <returns>(bool)</returns>
                public bool query(string toQuery)
                {
                    this.queryString = toQuery;
                    if (this.connStatus.Equals(true))
                    {
                        try
                        {
                            this.SqlQuery = SqlConn.CreateCommand();
                            this.SqlQuery.CommandText = toQuery;
                            if (this.isRead.Equals(true))
                            {
                                this.SqlReader.Close();
                            }
                            this.SqlReader = this.SqlQuery.ExecuteReader();
                            this.SqlReader.Read();
                            this.isRead = true;
                            this.current = 0;
                            return true;
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("MsSql.query(string): \n", exception);
                        }
                    }
                    Console.WriteLine("Not Connected to a Database!\nPlease Connect and Try Again...\n");
                    return false;
                }

            #endregion

            #region Sets

                /// <summary>
                /// <para>will connect to the db based on supplied connection...there are many possble exceptions to be thrown:</para>
                /// <para>1) missing server</para>
                /// <para>2) missing uid</para>
                /// <para>3) missing pass</para>
                /// <para>4) missing dbname</para>
                /// <para>5) cannot connect...check inner exception</para>
                /// </summary>
                /// <param name="connection" type="string">exa. "Data Source=$server;User ID=$usrID;Password=$pass;Initial Catalog=$dbName;")</param>
                public void setConn(string connection)
                {
                    //Data Source=SQL01;Initial Catalog=ITDEDEV;Persist Security Info=True;User ID=itdedbusr;Password=Santac1aus3
                    this.connStr = connection;
                    //throw new Exception(connection);

                    #region oldCode-ignore
                    /*if (!this.connStr.Contains("SERVER="))
                    {
                        throw new Exception("MISSING \"SERVER=%DBSERVER%\"");
                    }
                    if (!this.connStr.Contains(";UID="))
                    {
                        throw new Exception("MISSING \";UID=%USERID%\"");
                    }
                    if (!this.connStr.Contains(";PASSWORD="))
                    {
                        throw new Exception("MISSING \";PASSWORD=%PASS%\"");
                    }
                    if (!this.connStr.Contains(";DATABASE="))
                    {
                        throw new Exception("MISSING \";DATABASE=%DBNAME%\"");
                    }
                    if (!this.connStr.EndsWith(";"))
                    {
                        this.connStr = this.connStr + ";";
                    }*/
                    #endregion

                    if (!this.connStr.Contains("Data Source="))
                    {
                        Console.WriteLine(connection);
                        throw new Exception("MISSING \"Data Source=%DBSERVER%\":\n");
                    }
                    if (!this.connStr.Contains(";User ID="))
                    {
                        throw new Exception("MISSING \";User ID=%USERID%\"");
                    }
                    if (!this.connStr.Contains(";Password="))
                    {
                        throw new Exception("MISSING \";Password=%PASS%\"");
                    }
                    if (!this.connStr.Contains(";Initial Catalog="))
                    {
                        throw new Exception("MISSING \";Initial Catalog=%DBNAME%\"");
                    }
                    if (!this.connStr.EndsWith(";"))
                    {
                        this.connStr = this.connStr + ";";
                    }

                    try
                    {
                        SqlConn = new SqlConnection(this.connStr);
                        SqlConn.Open();
                        this.connStatus = true;
                    }
                    catch (Exception exception)
                    {
                        this.connStatus = false;
                        throw new Exception("setConn(connection): \n", exception);
                    }
                }

                /// <summary>
                /// <para>will connect to the db based on supplied connection...</para>
                /// <para>Exception: cannot connect...check inner exception</para>
                /// </summary>
                /// <param name="server">(string)</param>
                /// <param name="userId">(string)</param>
                /// <param name="pass">(string)</param>
                /// <param name="db">(string)</param>
                public void setConn(string server, string userId, string pass, string db)
                {
                    //Data Source=SQL01;Initial Catalog=ITDEDEV;Persist Security Info=True;User ID=itdedbusr;Password=Santac1aus3
                    this.connStr = "Data Source=" + server + ";User ID=" + userId + ";Password=" + pass + ";Initial Catalog=" + db + ";";
                    Console.WriteLine(this.connStr);
                    try
                    {
                        SqlConn = new SqlConnection(this.connStr);
                        SqlConn.Open();
                        this.connStatus = true;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        this.connStatus = false;
                        throw new Exception("setConn(server,userId,pass,db): \n", exception);
                    }
                }

            #endregion

            /// <summary>
            /// puts the type of the value in column[x] at the current row or resultSet into a string
            /// </summary>
            /// <param name="column">(int)</param>
            /// <returns>(string)</returns>
            protected string varIs(int column)
            {
                int ordinal = column;
                Console.WriteLine(this.parseType(this.SqlReader.GetValue(ordinal).GetType().ToString()));
                return (this.SqlReader.GetValue(ordinal).GetType().ToString() + "\n");
            }

        #endregion

    }//end class
}//end namespace
