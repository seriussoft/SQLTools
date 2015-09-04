using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;

using nTools.SqlTools;

#pragma warning disable 1591

namespace nTools.SqlTools.Archives
{
    public class MsSqlClass : ISqlClass
    {
        /************************************************************************************************
         * protected:
         * static SqlConnection SqlConn
         * SqlDataReader SqlReader
         * SqlCommand SqlQuery
         * 
         * private:
         * string connStr
         * bool connStatus
         * int current
         * string queryString
         * bool isRead
         */

        #region Fields
        protected static SqlConnection _sqlConn;
        private string _connStr;
        private bool _connStatus;
        protected SqlDataReader _sqlReader;
        protected SqlCommand _sqlQuery;
        private int _current;
        private string _queryString = "";
        private bool _isRead;
        private int _rowNum = 0;
        private int _lastInsertID = -1;
        private static List<string> _errors;
        private int _rowCount = -1;
        private int _colCOunt = -1;
        private static List<string> _queries;
        private static List<string> _queryErrorList;
        #endregion

        /************************************************************************************************
         * string[] ConnArray(get;set;)
         * string ConnString{get;}
         * string QueryString{get;set;}
         * bool IsConnected{get;}
         * static IList<string> Errors{get;}
         * 
         */

        #region Properties
        
        public string[] ConnArray
        {
            get { return new string[] { }; }
            set { }
        }

        public string ConnString
        {
            get { return _connStr; }
            set { setConn(value); } 
        }

        public string QueryString 
        {
            get { return _queryString; }
            set { query(value); } 
        }

        public bool IsConnected { get { return _connStatus; } }

        public static IList<string> Errors { get { return _errors.AsReadOnly(); } }

        public static IList<string> Queries { get { return _queries.AsReadOnly(); } }

        public int ColumnCount 
        { 
            /*get { return _colCOunt; }*/
            get
            {
                if (_sqlReader.HasRows.Equals(true))
                    return _sqlReader.FieldCount;
                else
                    return 0;
            }
        }

        public int RowCount { get { return _rowCount; } }

        public bool Connected { get { return _connStatus; } }

        public static IList<string> QueriesFailed { get { return _queryErrorList.AsReadOnly(); } }

        #endregion

        /****** Constructors ****************************************************************************
         * Constructors
         * 
         * cstr()
         * cstr(string)
         * cstr(string,string,string,string)
         */

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public MsSqlClass()
        {
            _connStr = "";
            _current = 0;
            _connStatus = false;
            _isRead = false;
            if (_queries == null)
            {
                _queries = new List<string>();
            }
            if (_errors == null)
            {
                _errors = new List<string>();
            }
            if (_queryErrorList == null)
            {
                _queryErrorList = new List<string>();
            }
        }
    
        /// <summary>
        ///overloaded class initiator...for connecting at same
        ///step as initiation of class. parameter(string)
        ///new MsSqlClass("SERVER=%server%;UID=%userId%;PASSWORD=%pass%;DATABASE=%db%;");
        /// </summary>
        /// <param name="connection"></param>
        public MsSqlClass(string connection)
        {
            _current = 0;
            _connStatus = false;
            _connStr = connection;
            _isRead = false;

            if (_queries == null)
            {
                _queries = new List<string>();
            }
            if (_errors == null)
            {
                _errors = new List<string>();
            }
            if (_queryErrorList == null)
            {
                _queryErrorList = new List<string>();
            }

            try 
            { 
                _sqlConn = new SqlConnection(_connStr);
                _sqlConn.Open();
                _connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                _connStatus = false;
                throw new Exception("MsSqlClass constructor 2: \n", e);
            }

        }
        
        /// <summary>
        /// overloaded class initiator for initialzing connection
        /// and class in one go. parameters(strings) server,userId,pass,db
        /// </summary>
        /// <param name="server"></param>
        /// <param name="userId"></param>
        /// <param name="pass"></param>
        /// <param name="db"></param>
        public MsSqlClass(string server, string userId, string pass, string db)
        {
            _current = 0;
            _connStatus = false;
            _isRead = false;
            _connStr = "Data Source=" + server + ";User ID=" + userId + ";Password=" + pass + "Initial Catalog=" + db + ";";

            if (_queries == null)
            {
                _queries = new List<string>();
            }
            if (_errors == null)
            {
                _errors = new List<string>();
            }
            if (_queryErrorList == null)
            {
                _queryErrorList = new List<string>();
            }

            try
            {
                _sqlConn = new SqlConnection(_connStr);
                _sqlConn.Open();
                _connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                _connStatus = false;
                //throw new Exception("MsSqlClass constructor 3: \n", e);
                throw new Exception("MySql Cstr(string,string,string,string)\n", e);
            }

        }

        #endregion


        #region Methods

        /// <summary>
        /// overloaded class initiator...for copying connection
        /// to a new class so that more than one query can be
        /// made with same connection
        /// </summary>
        /// <param name="toCopy"></param>
        /// <returns></returns>
        public static MsSqlClass makeCopyOf(MsSqlClass toCopy)
        {
            return new MsSqlClass(toCopy.ConnString/*toCopy.getConn()*/);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Obsolete("This method is deprecated and will be be removed in v4.2.1.\n Use ConnString propery instead.")]
        protected String getConn()
        {
            return _connStr;
        }

        /************************************************************************************************
         * setConn(string,string,string,string)
         * setConn(string,bool)
         * setConn(string)
         */

        #region setConn

        /// <summary>
        /// connects to db with supplied parameters(strings) server,userId,pass,dbName
        /// </summary>
        /// <param name="server"></param>
        /// <param name="userId"></param>
        /// <param name="pass"></param>
        /// <param name="db"></param>
        public bool setConn(string server, string userId, string pass, string db)
        {
            _connStr = "SERVER=" + server +";UID=" + userId +";PASSWORD=" + pass +";DATABASE=" + db +";";
            System.Console.WriteLine(_connStr);
            try 
            { 
                _sqlConn = new SqlConnection(_connStr);
                _sqlConn.Open();
                _connStatus = true;
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                _connStatus = false;
                _errors.Add("setConn(string,string,string,string);\n" + e.Message);
                return false;
                //throw new Exception("setConn(server,userId,pass,db): \n", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="validate"></param>
        public bool setConn(string connection, bool validate)
        {
            _connStr = connection;

            if (validate)
            {
                return setConn(connection);
                //return;
            }

            int x = 0;

            try
            {
                _sqlConn = new SqlConnection(_connStr);
                //SqlConn.ConnectionString = 
                x++;
                _sqlConn.Open();
                x++;
                _connStatus = true;
                x++;
                return true;
            }
            catch (Exception e)
            {
                //System.Console.WriteLine(e);
                _connStatus = false;
                _errors.Add("setConn(string,bool){" + x.ToString() + "}: \n" + e.Message);
                return false;
                //throw new Exception("setConn(connection,validate){" + x.ToString() + "}: \n", e);
            }

        }

        /// <summary>
        /// connects to db with supplied connection string in format
        /// "SERVER=%server%;UID=%userId%;PASSWORD=%pass%;DATABASE=%dbName%;"
        /// </summary>
        /// <param name="connection"></param>
        public bool setConn(string connection)
        {
            _connStr = connection;

        //checks to make sure string contains all 4 of variables required
        //to make a connection and follows the format:
        //"SERVER=%server%;UID=%userId%;PASSWORD=%pass%;DATABASE=%dbName%;"
            /*
            if (!connStr.Contains("DATA SOURCE=") && !connStr.Contains("DATABASE="))
                throw new Exception("MISSING \"DATA SOURCE=%DBSERVER%\"" + " or \"DATABASE=%DBSERVER%\"");
            else if (!connStr.Contains(";UID="))
                throw new Exception("MISSING \";UID=%USERID%\"");
            else if (!connStr.Contains(";PASSWORD="))
                throw new Exception("MISSING \";PASSWORD=%PASS%\"");
            else if (!connStr.Contains(";INITIAL CATALOG="))
                throw new Exception("MISSING \";INITIAL CATALOG=%DBNAME%\"");
            else if (!connStr.EndsWith(";"))
                connStr += ";";
            */

            if (!_connStr.EndsWith(";"))
                _connStr += ";";
        
            try
            {
                _sqlConn = new SqlConnection(_connStr);
                _sqlConn.Open();
                _connStatus = true;
                return true;
            }
            catch (Exception e)
            {
                //System.Console.WriteLine(e);
                _connStatus = false;
                _errors.Add("setConn(string): \n" + e.Message);
                return false;
                //throw new Exception("setConn(connection): \n", e);
            }
        }

        /// <summary>
        /// disconnects if connected
        /// </summary>
        public void disconn()
        {
            free();

            if (IsConnected/*isConnected()*/)
                _sqlConn.Close();
        }

        /// <summary>
        /// frees up the datareader (limited to one open datareader per connection)
        /// <para>call this method if you recieve an error about too many open datareaders for the connection</para>
        /// </summary>
        public void free()
        {
            try
            {
                _sqlReader.Close();
            }
            catch { }
            //_sqlReader.Dispose();
        }

        #endregion



        /// <summary>
        /// returns true/false if connected
        /// </summary>
        /// <returns></returns>
        [Obsolete("This method is deprecated and will be removed in v4.2.1.\n Use IsConnected property instead.")]
        public bool isConnected()
        {
            return _connStatus;
        }

        /*
         * System.Data.SqlClient doesn't allow for the nifty feature of ping()
         * like the MySqlClient does. too bad...it makes me sad...   
        public bool ping()
        {
            return SqlConn.Ping();
        }
        */





        #region Gets

        public DataTable getDataTable()
        {
            return new DataTable();
        }

        public DataTable getSchema()
        {
            try
            {
                return _sqlReader.GetSchemaTable();
            }
            catch (Exception e)
            {
                _errors.Add("getSchema(): \n" + e.Message);
                return new DataTable();
            }
        }

        public DataSet getDataSet()
        {
            return new DataSet();
        }

        /// <summary>
        /// returns value @ column(x) of current row or resultSet
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public SqlDataClass getVar(int column)
        {
            int x = column;
            SqlDataClass tmp = new SqlDataClass();

            //if(SqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(_sqlReader.GetValue(x).ToString(), parseType(_sqlReader.GetFieldType(x).ToString()));
            }
            catch (Exception e)
            {
                _current++;
                System.Console.WriteLine(_current + ") SoL: " + e.Message);
            }

            if (!tmp.getString().Equals(null))
                return tmp;
            else return new SqlDataClass("no_value_to_be_read",dType.String);
            
            //else throw "no_value_to_be_read";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="toStore"></param>
        public void getVar(int column, ref object toStore)
        {
            int x = column;
            SqlDataClass tmp = new SqlDataClass();

            //if(SqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(_sqlReader.GetValue(x).ToString(), parseType(_sqlReader.GetFieldType(x).ToString()));
            }
            catch (Exception e)
            {
                _current++;
                System.Console.WriteLine(_current + ") SoL: " + e.Message);
            }

            if (!tmp.getString().Equals(null))
                tmp.putIn(ref toStore);
            else
            {
                tmp = new SqlDataClass("no_value_to_be_read", dType.String);
                tmp.putIn(ref toStore);
            }

            //else throw "no_value_to_be_read";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public SqlDataClass getVar(string column)
        {
            string x = column;
            SqlDataClass tmp = new SqlDataClass();

            //if(SqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(_sqlReader[x].ToString(), parseType(_sqlReader[x].GetType().ToString()));
            }
            catch (Exception e)
            {
                _current++;
                System.Console.WriteLine(_current + ") SoL: " + e.Message);
            }

            if (!tmp.getString().Equals(null))
                return tmp;
            else return new SqlDataClass("no_value_to_be_read", dType.String);

            //else throw "no_value_to_be_read";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="toStore"></param>
        public void getVar(string column, ref object toStore)
        {
            string x = column;
            SqlDataClass tmp = new SqlDataClass();

            //if(SqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(_sqlReader[x].ToString(), parseType(_sqlReader[x].GetType().ToString()));
            }
            catch (Exception e)
            {
                _current++;
                System.Console.WriteLine(_current + ") SoL: " + e.Message);
            }

            if (!tmp.getString().Equals(null))
                tmp.putIn(ref toStore);
            else
            {
                tmp = new SqlDataClass("no_value_to_be_read", dType.String);
                tmp.putIn(ref toStore);
            }

            //else throw "no_value_to_be_read";
        }

        /// <summary>
        /// gets number of columns at current rowSet
        /// </summary>
        /// <returns></returns>
        [Obsolete("This method is deprecated and will be removed in v4.2.1.\n Use ColumnCount property instead.")]
        public int getColumns()
        {
            if (_sqlReader.HasRows.Equals(true))
                return _sqlReader.FieldCount;
            else
                return 0;
        }

        /// <summary>
        /// gets number of rows returned
        /// </summary>
        /// <returns></returns>
        [Obsolete("This method is deprecated and will be removed in v4.2.1.\n Use RowCount property instead.")]
        public int getRows()
        {
            //string rowCall = "SELECT COUNT(" + columnNames + ") FROM " + tableName;
            
            try
            {
                SqlCommand rows = _sqlConn.CreateCommand();
                rows.CommandText = _queryString;
                SqlDataReader rowsFound = rows.ExecuteReader();
                int numRows = 0;

                while (rowsFound.Read().Equals(true))
                    numRows++;
                    
                rowsFound.Close();
                return numRows;
            }
            catch(Exception e)
            {
                return -1;
            }
        }

        #endregion

        #region Others

        private void getRowCount(object query)
        {
            SqlConnection conn = new SqlConnection();

            try
            {    
                conn.ConnectionString = _connStr;
                conn.Open();

                SqlCommand cmd = new SqlCommand((string)query, conn);
                cmd.ExecuteScalar();

                cmd.CommandText = "SELECT @@ROWCOUNT;";
                _rowCount = (int)cmd.ExecuteScalar();

                conn.Close();
            }
            catch (Exception e)
            {
                _errors.Add(e.Message + "\n in getRowCount(string)");
                try { conn.Close(); }
                catch { }
            }
        }

        /// <summary>
        /// issues a query. returns true/false for success of call
        /// </summary>
        /// <param name="toQuery"></param>
        /// <returns></returns>
        public bool query(string toQuery)
        {
            _queryString = toQuery;

            if (_connStatus.Equals(true))
            {

                System.Threading.Thread rowCntThread = new System.Threading.Thread(getRowCount);

                try
                {
                    try { free(); }
                    catch { }
                    _sqlQuery = _sqlConn.CreateCommand();
                    _sqlQuery.CommandText = toQuery;
                    if (_isRead.Equals(true))
                        _sqlReader.Close();

                    rowCntThread.Start(_queryString);

                    _sqlReader = _sqlQuery.ExecuteReader();
                    _sqlReader.Read();
                    _isRead = true;
                    _current = 0;

                    //pull rowcount
                    SqlCommand cmd = new SqlCommand("SELECT @@ROWCOUNT;", _sqlConn);
                    _rowCount = int.Parse(cmd.ExecuteScalar().ToString());

                    while (rowCntThread.IsAlive) { }

                    return true;
                }
                catch (Exception e)
                {
                    //System.Console.WriteLine("can't do somethin here man...gah...your ruining my life...");
                    _queryErrorList.Add(toQuery);
                    _errors.Add(e.Message + "\nin query(string)" );

                    while (rowCntThread.IsAlive) { }
                    try { _sqlReader.Close(); }
                    catch { }
                    return false;
                }
            }
            else
            {
                _queryErrorList.Add(toQuery);
                _errors.Add("Cannot query unless connceted to a database!\n Please connect and try again...");
                System.Console.WriteLine("Not Connected to a Database!\n" +
                                         "Please Connect and Try Again...\n");
                return false;
            }
        }

        /// <summary>
        /// issues a command to the database. returns true/false for success of call
        /// <para>(only use this for insert,update,delete,drop commands, not for selecte queries!)</para>
        /// </summary>
        /// <param name="commandQuery"></param>
        /// <returns></returns>
        public bool command(string commandQuery)
        {
            _queryString = commandQuery;

            _rowNum = 0;

            if (_connStatus.Equals(true))
            {
                try
                {
                    this.free();
                    _sqlQuery = _sqlConn.CreateCommand();
                    _sqlQuery.CommandText = commandQuery;

                    _rowNum = _sqlQuery.ExecuteNonQuery();
                    _current = 0;

                    _queries.Add(commandQuery);

                    return true;
                }
                catch (Exception e)
                {
                    _queryErrorList.Add(commandQuery);
                    _errors.Add(e.Message + "\n in command(string)");
                    return false;
                }
            }
            else
            {
                //errors.Add("Not connected to DB...Cannot command until connected");
                System.Console.WriteLine("Not connected to a Database!\nPlease connect and try again...\n");
                _errors.Add("query(string):\n Not connected to a Database!\nPlease connect and try again...");
                _queryErrorList.Add(commandQuery);
                return false;
            }
            
        }

        /// <summary>
        /// sets reader to next resultSet
        /// </summary>
        /// <returns></returns>
        public bool next()
        {
            if (_sqlReader.Read().Equals(true))
            {
                _current++;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// returns dType.(string/bool/int/double) of value
        /// </summary>
        /// <param name="sType"></param>
        /// <returns></returns>
        public dType parseType(string sType)
        {
            dType tType = new dType();

            sType = sType.ToLower();
            sType = sType.Remove(0, 7);

            switch (sType)
            {
                case "nvarchar": //string
                case "string": tType = dType.String; break;
                case "boolean": //bool
                case "bool": tType = dType.Bool; break;
                case "uint32": //int
                case "uint64": //int
                case "byte": tType = dType.Integer; break;
                case "single": tType = dType.Double; break;
                default: tType = dType.String; break;
            }

            //System.Console.WriteLine(sType);

            return tType;
        }

        /// <summary>
        /// puts the type and value into a string
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        protected string varIs(int column)
        {
            int x = column;
            //if (SqlReader.NextResult().Equals(true))
            System.Console.WriteLine(parseType(_sqlReader.GetValue(x).GetType().ToString()));
            return _sqlReader.GetValue(x).GetType().ToString() + "\n";// +SqlReader.GetString(x) + "\n";
            //else
                //return "SoS";
        }

        #endregion

        #endregion

        #region Enumeration Function(s)
        /// <summary>
        /// Forwards only IEnumeratore on the rows of the result set
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SqlRow> Rows()
        {
            MsSqlClass tmp = MsSqlClass.makeCopyOf(this);
            tmp.query(this.QueryString);

            int rowMax = tmp._sqlReader.FieldCount;
            SqlRow row;

            do
            {
                row = new SqlRow();
                row.setKeys(new string[rowMax]);

                for (int x = 0; x < rowMax; x++)
                {
                    row.setKey(x, tmp._sqlReader.GetName(x));
                    row.setValue(x, tmp.getVar(x));
                }

                yield return row;

            }
            while (tmp.next());

            tmp.disconn();
        }
        #endregion

    }//end of MsSqlClass

}
