// compile with: /doc:xml_summary_tag.xml

using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

using stateType = System.Data.ConnectionState;

using nTools.SqlTools;

namespace SqlTools
{

    /*** MySqlClass
     *   
     *   dependant on SqlDataClass of same namespace
     *   constructor defaults to connStr="",current=0,connStatus=false
     *   constructor accepts string parameter and initializes connection
     *   constructor accepts parameters(strings) server,userId,pass,db and initializes connection
     *   public MySqlClass makeCopyOf(MySqlClass toCopy)
     *   protected String getConn()
     *   public bool setConn(string connection)
     *   public bool setConn(string server, string userId, string pass, string db)
     *   public bool isConnected()
     *   public bool ping()
     *   public void disconn()
     *   public bool query(string toQuery)
     *   public bool next()
     *   public SqlDataClass getVar(int col)
     *   public void getVar(int column, ref object toStore)
     *   public SqlDataClass getVar(string column)
     *   public void getVar(string column, ref object toStore)
     *   public int getColumns()
     *   public int getRows()
     *   public static int numQueries()
     *   public bool setRow(int rowNum)
     *   public dType parseType(string sType)
     *   protected string varIs(int column)
     *   public SqlDataClass nextVar()
     ***/


    /// <summary>
    /// requires that mysql-connector-net-5.0.8.1 is installed and will not function otherwise...
    /// please download that from www.seriussoft.com or redownload the latest version of SqlTools (all necessary files come with it)
    /// </summary>
    /// <type>class</type>
    public class MySqlClass
    {
        #region fields
        //global variables
        protected MySqlConnection mySqlConn;
        private string connStr;
        private bool connStatus;
        protected MySqlDataReader mySqlReader;
        protected MySqlCommand mySqlQuery;
        private int current;
        private int rowNum = 0;
        private string queryString = "";
        private bool isRead;
        private int theLastInsertID = -1;
        private dMode theMode = dMode.full;

        /// <summary>
        /// gets an array of strings. {ipAddress,port}
        /// </summary>
        public string[] pingData
        {
            //connStr = "SERVER=" + server + ";PORT=" + port.ToString() + ";UID=" + userId + ";PASSWORD=" + pass + ";";
            get
            {
                string[] pingArr = new string[2] { "", "" };

                int count = 0;
                string[] contents = connStr.Split(';');

                foreach (string str in contents)
                {
                    if (str.Contains("SERVER="))
                    {
                        pingArr[0] = str.Replace("SERVER=", "");
                        count++;
                    }

                    if (str.Contains("PORT="))
                    {
                        pingArr[1] = str.Replace("PORT=", "");
                        count++;
                    }

                    if (count == 1)
                    {
                        pingArr[1] = "3306";
                    }

                }
                return pingArr;
            }
        }

        /// <summary>
        /// gets or sets the mode of MySqlClass... fastest will not have an SqlDataArrayTable or SqlArrayTable, nor have a row count
        ///  wherease medium will have a rowcount and an SqlDataArrayTable, but no SqlArrayTable, taking a very small hit in speed
        ///  and full, which is default, will house all three of the above, but will take a small hit in speed.
        /// </summary>
        public dMode mode
        {
            get
            {
                return theMode;
            }

            set
            {
                theMode = value;
            }

        }

        /// <summary>
        /// int repesentation of the last Insert ID, will return -1 if you have not inserted anything yet
        /// </summary>
        public int lastInsertID
        {
            get
            {
                return theLastInsertID;
            }
        }

        /// <type>List&lt;string&gt;</type>
        public readonly List<string> errors;//new
        
        /// <type>List&lt;string&gt;</type>
        public static readonly List<string> queries = new List<string>();  //new 

        /// <summary>
        /// SqlArrayClass table representation of a queryResult
        /// </summary>
        private SqlArrayClass theTable;

        /// <summary>
        /// SqlDataArrayClass table representation of a queryResult
        /// </summary>
        private SqlDataArrayClass<SqlDataClass> theDataTable;

        /// <summary>
        /// the connection
        /// </summary>
        public MySql.Data.MySqlClient.MySqlConnection conn
        {
            get 
            {
                return mySqlConn; 
            }
            set 
            {
                this.disconn();
                mySqlConn = value;
                mySqlConn.Open();
            }
        }//end conn

        /// <summary>
        /// the table representation of the result set as strings
        /// </summary>
        public SqlArrayClass table
        {
            get { return theTable; }
        }

        /// <summary>
        /// the table representation of the result set as SqlDataClass's
        /// </summary>
        public SqlDataArrayClass<SqlDataClass> dataTable
        {
            get { return theDataTable; }
        }

        #endregion

        /************************************************************************************************
         * cstr()
         * cstr(string)
         * cstr(string,string,string,string)
         * cstr(string,int,string,string,string)
         * cstr(string,string,string)
         * cstr(string,int,string,string)
         */ 

        #region constructors
        /// <summary>
        /// Empty constructor. Sets current conns to 0. instantiates list&lt;string&gt; errors
        /// </summary>
        public MySqlClass()
        {
            connStr = "";
            current = 0;
            rowNum = 0;
            connStatus = false;
            isRead = false;
            errors = new List<string>();//new
        }

        /// <summary>
        /// overloaded class initiator...for connecting at same step as initiation of class.
        /// 1 parameter(string).
        /// exa. new MySqlClass("SERVER=%server%;UID=%userId%;PASSWORD=%pass%DATABASE=%db%;");
        /// </summary>
        /// <param name="connection" type="string"></param>
        public MySqlClass(string connection)
        {
            errors = new List<string>();
            current = 0;
            rowNum = 0;
            connStatus = false;
            connStr = connection;
            isRead = false;

            try
            {
                mySqlConn = new MySqlConnection(connStr);
                mySqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                connStatus = false;
                errors.Add(e.Message + " in constructor 2.");
                throw new Exception("MySqlClass constructor 2: \n", e);
            }

        }

        /// <summary>
        /// overloaded class initiator for initialzing connection
        /// and class in one go. parameters(strings) server,userId,pass,db
        /// </summary>
        /// <param name="server">(string)</param>
        /// <param name="userId">(string)</param>
        /// <param name="pass">(string)</param>
        /// <param name="db">(string)</param>
        public MySqlClass(string server, string userId, string pass, string db)
        {
            errors = new List<string>();
            current = 0;
            rowNum = 0;
            connStatus = false;
            isRead = false;
            connStr = "SERVER=" + server + ";UID=" + userId + ";PASSWORD=" + pass + "DATABASE=" + db + ";";

            try
            {
                mySqlConn = new MySqlConnection(connStr);
                mySqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                connStatus = false;
                errors.Add(e.Message + " in constructor 3");
                throw new Exception("MySqlClass constructor 3: \n", e);
            }

        }

        /// <summary>
        /// overloaded class initiator for initialzing connection
        /// and class in one go. parameters(strings) server,port,userId,pass,db
        /// </summary>
        /// <param name="server">(string)</param>
        /// <param name="port">int</param>
        /// <param name="userId">(string)</param>
        /// <param name="pass">(string)</param>
        /// <param name="db">(string)</param>
        public MySqlClass(string server, int port, string userId, string pass, string db)
        {
            errors = new List<string>();
            current = 0;
            rowNum = 0;
            connStatus = false;
            isRead = false;
            connStr = "SERVER=" + server + ";PORT=" + port.ToString() + ";UID=" + userId + ";PASSWORD=" + pass + "DATABASE=" + db + ";";

            try
            {
                mySqlConn = new MySqlConnection(connStr);
                mySqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                connStatus = false;
                errors.Add(e.Message + " in constructor 3");
                throw new Exception("MySqlClass constructor 3: \n", e);
            }

        }

        /// <summary>
        /// overloaded class initiator for initialzing connection
        /// and class in one go. parameters(strings) server,userId,pass
        /// </summary>
        /// <param name="server">(string)</param>
        /// <param name="userId">(string)</param>
        /// <param name="pass">(string)</param>
        public MySqlClass(string server, string userId, string pass)
        {
            errors = new List<string>();
            current = 0;
            rowNum = 0;
            connStatus = false;
            isRead = false;
            connStr = "SERVER=" + server + ";UID=" + userId + ";PASSWORD=" + pass + ";";

            try
            {
                mySqlConn = new MySqlConnection(connStr);
                mySqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                connStatus = false;
                errors.Add(e.Message + " in constructor 3");
                throw new Exception("MySqlClass constructor 3: \n", e);
            }

        }

        /// <summary>
        /// overloaded class initiator for initialzing connection
        /// and class in one go. parameters(strings) server,port,userId,pass
        /// </summary>
        /// <param name="server">(string)</param>
        /// <param name="port">int</param>
        /// <param name="userId">(string)</param>
        /// <param name="pass">(string)</param>
        public MySqlClass(string server, int port, string userId, string pass)
        {
            errors = new List<string>();
            current = 0;
            rowNum = 0;
            connStatus = false;
            isRead = false;
            connStr = "SERVER=" + server + ";PORT=" + port.ToString() + ";UID=" + userId + ";PASSWORD=" + pass + ";";

            try
            {
                mySqlConn = new MySqlConnection(connStr);
                mySqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                connStatus = false;
                errors.Add(e.Message + " in constructor 3");
                throw new Exception("MySqlClass constructor 3: \n", e);
            }

        }

        #endregion

        /************************************************************************************************
         * setConn(string,string,string,string)
         * setConn(string,int,string,string,string)
         * setConn(string,string,string)
         * setConn(string,int,string,string)
         * setConn(string)
         */

        #region setConn
        /// <summary>
        /// connects to db with supplied parameters(strings) server,userId,pass,dbName
        /// </summary>
        /// <param name="server">(string)</param>
        /// <param name="userId">(string)</param>
        /// <param name="pass">(string)</param>
        /// <param name="db">(string)</param>
        public void setConn(string server, string userId, string pass, string db)
        {
            connStr = "SERVER=" + server + ";UID=" + userId + ";PASSWORD=" + pass + ";DATABASE=" + db + ";";
            System.Console.WriteLine(connStr);
            try
            {
                mySqlConn = new MySqlConnection(connStr);
                mySqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                connStatus = false;
                errors.Add(e.Message + " setConn(server,userId,pass,db)");
                throw new Exception("setConn(server,userId,pass,db): \n" + e.Message);
            }
        }

        /// <summary>
        /// connects to db with supplied parameters(strings) server,(int)port,userId,pass,dbName
        /// </summary>
        /// <param name="server">(string)</param>
        /// <param name="port">(int)</param>
        /// <param name="userId">(string)</param>
        /// <param name="pass">(string)</param>
        /// <param name="db">(string)</param>
        public void setConn(string server, int port, string userId, string pass, string db)
        {
            connStr = "SERVER=" + server + ";PORT=" + port.ToString() + ";UID=" + userId + ";PASSWORD=" + pass + ";DATABASE=" + db + ";";
            System.Console.WriteLine(connStr);
            try
            {
                mySqlConn = new MySqlConnection(connStr);
                mySqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                connStatus = false;
                errors.Add(e.Message + " setConn(server,port,userId,pass,db)");
                throw new Exception("setConn(server,port,userId,pass,db): \n" + e.Message);
            }
        }

        /// <summary>
        /// connects to db with supplied parameters(strings) server,userId,pass
        /// </summary>
        /// <param name="server">(string)</param>
        /// <param name="userId">(string)</param>
        /// <param name="pass">(string)</param>
        public void setConn(string server, string userId, string pass)
        {
            connStr = "SERVER=" + server + ";UID=" + userId + ";PASSWORD=" + pass + ";";
            System.Console.WriteLine(connStr);
            try
            {
                mySqlConn = new MySqlConnection(connStr);
                mySqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                connStatus = false;
                errors.Add(e.Message + " setConn(server,userId,pass)");
                throw new Exception("setConn(server,userId,pass): \n" + e.Message);
            }
        }

        /// <summary>
        /// connects to db with supplied parameters(strings) server,(int)port,userId,pass
        /// </summary>
        /// <param name="server">(string)</param>
        /// <param name="port">(int)</param>
        /// <param name="userId">(string)</param>
        /// <param name="pass">(string)</param>
        public void setConn(string server,int port, string userId, string pass)
        {
            connStr = "SERVER=" + server + ";PORT=" + port.ToString() + ";UID=" + userId + ";PASSWORD=" + pass + ";";
            System.Console.WriteLine(connStr);
            try
            {
                mySqlConn = new MySqlConnection(connStr);
                mySqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                connStatus = false;
                errors.Add(e.Message + " setConn(server,port,userId,pass)");
                throw new Exception("setConn(server,port,userId,pass): \n" + e.Message);
            }
        }

        /// <summary>
        /// connects to db with supplied connection string in format
        /// "SERVER=%server%;UID=%userId%;PASSWORD=%pass%;DATABASE=%dbName%;"
        /// </summary>
        /// <param name="connection">(string)</param>
        public void setConn(string connection)
        {
            connStr = connection;

            //checks to make sure string contains all 4 of variables required
            //to make a connection and follows the format:
            //"SERVER=%server%;UID=%userId%;PASSWORD=%pass%;DATABASE=%dbName%;"
            if (!connStr.Contains("SERVER="))
            {
                errors.Add("MISSING \"SERVER=%DBSERVER%\"");
                throw new Exception("MISSING \"SERVER=%DBSERVER%\"");
            }
            else if (!connStr.Contains(";UID="))
            {
                errors.Add("MISSING \";UID=%USERID%\"");
                throw new Exception("MISSING \";UID=%USERID%\"");
            }
            else if (!connStr.Contains(";PASSWORD="))
            {
                errors.Add("MISSING \";PASSWORD=%PASS%\"");
                throw new Exception("MISSING \";PASSWORD=%PASS%\"");
            }
            else if (!connStr.Contains(";DATABASE="))
            {
                errors.Add("MISSING \";DATABASE=%DBNAME%\"");
                throw new Exception("MISSING \";DATABASE=%DBNAME%\"");
            }
            else if (!connStr.EndsWith(";"))
                connStr += ";";

            try
            {
                mySqlConn = new MySqlConnection(connStr);
                mySqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                //System.Console.WriteLine(e);
                connStatus = false;
                errors.Add(e.Message + " in setConn(connection)");
                throw new Exception("setConn(connection): \n" + e.Message);
            }
        }
        #endregion

        /************************************************************************************************
         * static MySqlClass makeCopyOf(MySqlClass toCopy)
         * bool isOpen()
         * bool reConn(ref MySqlClass)
         * string getConn()
         * bool isConnected()
         * bool ping()
         * void disconn()
         * bool next()
         */
        #region query & connection methods
        /// <summary>
        /// overloaded class initiator...for copying connection
        /// to a new class so that more than one query can be
        /// made with same connection
        /// </summary>
        /// <param name="toCopy">(MySqlClass) MySqlClassToCopy</param>
        /// <returns>(MySqlClass)</returns>
        static public MySqlClass makeCopyOf(MySqlClass toCopy)
        {
            return new MySqlClass(toCopy.getConn());
        }

        /// <summary>
        /// returns whether or not the connection is open on the current MySqlClass
        /// </summary>
        /// <returns type="bool"></returns>
        public bool isOpen()
        {
            try
            {
                stateType theState = conn.State;
                switch (theState)
                {
                    case stateType.Closed:
                    case stateType.Broken: return false;
                        break;

                    default: return true;
                        break;
                }
            }
            catch (Exception e)
            {
                //
                return false;
            }
        }

        /// <summary>
        /// reConnect the MySqlClass, returning true/false on success
        /// </summary>
        /// <param name="old" type="SqlTools.MySqlClass">the MySqlClass instance to reconnect</param>
        /// <returns type="bool">success of reconnecting</returns>
        static public bool reConn(ref MySqlClass old)
        {
            try
            {
                old = MySqlClass.makeCopyOf(old);
            }
            catch (Exception e)
            {
                old.errors.Add(e.Message + "@ reConn(...)");
                return false;
            }

            return true;

        }

        /// <summary>
        /// gets the connection string
        /// </summary>
        /// <returns>(string) connString</returns>
        protected string getConn()
        {
            return connStr;
        }
        
        /// <summary>
        /// returns true/false if connected
        /// </summary>
        /// <returns>(bool)</returns>
        public bool isConnected()
        {
            return connStatus;
        }

        /// <summary>
        /// sends ping request to server. returns bool
        /// </summary>
        /// <returns>(bool)</returns>
        private bool ping()
        {
            return mySqlConn.Ping();
        }

        /// <summary>
        /// disconnects if connected
        /// </summary>
        public void disconn()
        {
            if (isConnected())
                mySqlConn.Close();
            
            rowNum = 0;
        }

        /// <summary>
        /// sets reader to next resultSet, returning true/false on success
        /// </summary>
        /// <returns>(bool)</returns>
        public bool next()
        {
            if (mySqlReader.Read().Equals(true))
            {
                current++;
                return true;
            }
            else
                return false;
        }

        /*
        /// <summary>
        /// issues a query. returns true/false for success of call
        /// </summary>
        /// <param name="toQuery">(string)</param>
        /// <returns>(bool)</returns>
        public bool query(string toQuery)
        {
            queryString = toQuery;

            rowNum = 0;

            if (connStatus.Equals(true))
            {
                try
                {
                    this.free();
                    mySqlQuery = mySqlConn.CreateCommand();

                    string newQuery = toQuery;

                    if (toQuery.Contains("SELECT") && !toQuery.Contains("COUNT"))
                    {
                        newQuery = newQuery.Replace("SELECT ", "SELECT COUNT(");
                        
                        if(newQuery.Substring(newQuery.IndexOf("ECT "), newQuery.IndexOf("FROM")).Contains(","))
                        {
                            int start = newQuery.IndexOf(',');
                            int howMany = newQuery.IndexOf(" FROM") - start;

                            newQuery = newQuery.Remove(start, howMany);
                            //Console.WriteLine("*******  " + newQuery + "  *******");
                        }
                        else
                        {
                            //do nothing special, because there is only one column
                        }

                        newQuery = newQuery.Replace(" FROM ", ") FROM ");
                        mySqlQuery.CommandText = newQuery;
                        rowNum = int.Parse(mySqlQuery.ExecuteScalar().ToString());
                    }
                    else
                        rowNum = 0;
                    
                    mySqlQuery.CommandText = toQuery;
                    queryString = toQuery;
                    if (isRead.Equals(true))
                        mySqlReader.Close();
                    mySqlReader = mySqlQuery.ExecuteReader();
                    mySqlReader.Read();
                    isRead = true;
                    current = 0;
                    queries.Add(toQuery);//new
                    return true;
                }
                catch (Exception e)
                {
                    errors.Add(e.Message + " in query(toQuery)");
                    //System.Console.WriteLine("can't do somethin here man...gah...your ruining my life...");
                    throw e;
                    //return false;
                }
            }
            else
            {
                errors.Add("Not connected to DB...Cannot query until connected");
                System.Console.WriteLine("Not Connected to a Database!\n" +
                                         "Please Connect and Try Again...\n");
                return false;
            }
        }
        */
        //
        private void bob(){}


        #endregion

        /************************************************************************************************
         * SqlDataClass getVar(int)
         * void getVar(int,ref object)
         * SqlDataClass getVar(string)
         * void getVar(string,ref object)
         */

        #region Get Variables
        /// <summary>
        /// returns value @ column(x) of current row or resultSet
        /// </summary>
        /// <param name="column">(int)</param>
        /// <returns>(SqlDataClass)</returns>
        public SqlDataClass getVar(int column)
        {
            int x = column;
            SqlDataClass tmp = new SqlDataClass();

            //if(mySqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(mySqlReader.GetValue(x).ToString(), parseType(mySqlReader.GetFieldType(x).ToString()));
            }
            catch (Exception e)
            {
                errors.Add(e.Message + " in getVar(int)");
                //current++;//taken out for now...why did i have it here to begin with?
                System.Console.WriteLine(current + ") SoL: " + e.Message);
            }

            if (!tmp.getString().Equals(null))
                return tmp;
            else return new SqlDataClass("no_value_to_be_read", dType.String);

            //else throw "no_value_to_be_read";
        }

        /// <summary>
        /// gets value @ column(x) of current row or resultSet and stores it to
        /// the referenced object
        /// </summary>
        /// <param name="column">(int)</param>
        /// <param name="toStore">(ref object)</param>
        public void getVar(int column, ref object toStore)
        {
            int x = column;
            SqlDataClass tmp = new SqlDataClass();

            //if(mySqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(mySqlReader.GetValue(x).ToString(), parseType(mySqlReader.GetFieldType(x).ToString()));
            }
            catch (Exception e)
            {
                errors.Add(e.Message + " in getVar(int, ref object)");
                //current++;//once again, why is this here? not now...
                System.Console.WriteLine(current + ") SoL: " + e.Message);
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
        /// gets value at supplied column key of current row or resultSet and returns it
        ///  as a SqlDataClass
        /// </summary>
        /// <param name="column">(string)</param>
        /// <returns>(SqlDataClass)</returns>
        public SqlDataClass getVar(string column)
        {
            string x = column;
            SqlDataClass tmp = new SqlDataClass();

            //if(mySqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(mySqlReader[x].ToString(), parseType(mySqlReader[x].GetType().ToString()));
            }
            catch (Exception e)
            {
                errors.Add(e.Message + " in getVar(string)");
                //current++;//wtf? what was i thinking putting this here???
                System.Console.WriteLine(current + ") SoL: " + e.Message);
            }

            if (!tmp.getString().Equals(null))
                return tmp;
            else return new SqlDataClass("no_value_to_be_read", dType.String);

            //else throw "no_value_to_be_read";
        }


        /// <summary>
        /// gets variable at supplied column key, at the current row and stores it to
        /// the referenced object
        /// </summary>
        /// <param name="column">(string)</param>
        /// <param name="toStore">(ref object)</param>
        public void getVar(string column, ref object toStore)
        {
            string x = column;
            SqlDataClass tmp = new SqlDataClass();

            //if(mySqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(mySqlReader[x].ToString(), parseType(mySqlReader[x].GetType().ToString()));
            }
            catch (Exception e)
            {
                errors.Add(e.Message + " in getVar(string, ref object)");
                //current++;//omg, this is getting old, why is it here???
                System.Console.WriteLine(current + ") SoL: " + e.Message);
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
        #endregion

        /************************************************************************************************
         * int getColumns()
         * int getRows()
         * static int numQueries()
         * int numErrors()
         * bool setRow(int)
         * dType parseType(string)
         * string varIs(int)
         */

        #region extra functions

        /// <summary>
        /// gets number of columns at current rowSet
        /// </summary>
        /// <returns>(int)</returns>
        public int getColumns()
        {
            if (mySqlReader.HasRows.Equals(true))
                return mySqlReader.FieldCount;
            else
                return 0;
        }

        /// <summary>
        /// gets the number of rows in the resultSet
        /// </summary>
        /// <returns>(int)</returns>
        public int getRows()
        {
                return rowNum;
        }

        /// <summary>
        /// gets the number of queries in list
        /// </summary>
        /// <returns>(static int)</returns>
        public static int numQueries()
        {
            return queries.Count;
        }

        /// <summary>
        /// gets the number of errors in list
        /// </summary>
        /// <returns>(int)</returns>
        public int numErrors()
        {
            return errors.Count;
        }

        /// <summary>
        /// setRow to desired row number if possible, returns false if impossible
        /// </summary>
        /// <param name="rowNumber">(int)</param>
        /// <returns>bool</returns>
        public bool setRow(int rowNumber)
        {
            if (current > rowNumber)
            {
                errors.Add("You are already on a row past the supplied row number");
                return false;
            }
            else if (getRows() < rowNumber - 1)
            {
                errors.Add("Supplied row number exceeds number of rows returned");
                return false;
            }
            else
            {
                for (int x = current; x < rowNumber; x++)
                {
                    current++;
                    next();
                }
                return true;
            }

        }

        /// <summary>
        /// returns dType.(string/bool/int/double) of value
        /// </summary>
        /// <param name="sType">(string)</param>
        /// <returns>dType</returns>
        public dType parseType(string sType)
        {
            dType tType = new dType();

            sType = sType.ToLower();
            sType = sType.Remove(0, 7);

            switch (sType)
            {
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
        /// <param name="column">(int)</param>
        /// <visibility>protected</visibility>
        /// <returns>(string)</returns>
        protected string varIs(int column)
        {
            int x = column;
            //if (mySqlReader.NextResult().Equals(true))
            System.Console.WriteLine(parseType(mySqlReader.GetValue(x).GetType().ToString()));
            return mySqlReader.GetValue(x).GetType().ToString() + "\n";// +mySqlReader.GetString(x) + "\n";
            //else
            //return "SoS";
        }

        #endregion

        /************************************************************************************************
         * bool setLastInsertID()
         * bool changeDB(string)
         * bool free()
         * bool command(string)
         * bool query(string)
         */

        #region newFunctions

        /// <summary>
        /// sets the last insert id to the result of "SELECT LAST_INSERT_ID()", returns true/false on success
        /// </summary>
        /// <returns type="bool"></returns>
        private bool setLastInsertID()
        {
            if (queryString.Contains("INSERT") || queryString.Contains("insert"))
            {
                try
                {
                    MySqlCommand comm = mySqlConn.CreateCommand();
                    comm.CommandText = "SELECT LAST_INSERT_ID()";
                    
                    //object temp = comm.ExecuteScalar();
                    //Console.WriteLine("{0} of type {1}",temp,temp.GetType());
                    //Console.WriteLine("{0}", temp);
                    //theLastInsertID = Convert.ToInt32(temp);

                    theLastInsertID = Convert.ToInt32(comm.ExecuteScalar());
                    //comm.c
                    return true;
                }
                catch (Exception e)
                {
                    this.errors.Add(e.Message + " in setLastInsertID()");
                    return false;
                }
            }
            else
            {
                //do nothing at all...
                return false;
            }

        }

        /// <summary>
        /// change the db to the supplied dbName
        /// </summary>
        /// <param name="dbName">(string)</param>
        /// <returns>(bool)</returns>
        public bool changeDB(string dbName)
        {
            try
            {
                mySqlConn.ChangeDatabase(dbName);
                queries.Add("CHANGE_DB TO " + dbName + ";");
            }
            catch (Exception e)
            {
                errors.Add(e.Message + " changeDB(" + dbName + ")");
                return false;
            }

            return true;
        }

        /// <summary>
        /// call this everytime you plan to use a new query but don't wish to close the connection
        /// </summary>
        /// <returns type="bool">success of function. if it returns false, check SqlTools.MySqlClass.errors for the problem</returns>
        public bool free()
        {
            try
            {
                mySqlConn.Close();
                mySqlConn.Open();
            }
            catch (Exception e)
            {
                this.errors.Add(e.Message + " in free()");
                return false; 
            }

            return true;
        }

        /// <summary>
        /// execute a command (like update, insert, or delete). use getRows() to find number of rows effected
        /// </summary>
        /// <param name="commandQuery" type="string">the command query...update,insert,delete, etc (do not use select)</param>
        /// <returns type="bool">the success of the commandQuery</returns>
        public bool command(string commandQuery)
        {
            queryString = commandQuery;

            rowNum = 0;

            if (connStatus.Equals(true))
            {
                try
                {
                    
                    this.free();
                    mySqlQuery = mySqlConn.CreateCommand();
                    mySqlQuery.CommandText = commandQuery;
                    queryString = commandQuery;
                    
                    //if (isRead.Equals(true))
                        //mySqlReader.Close();
                    
                    rowNum = mySqlQuery.ExecuteNonQuery();
                    //isRead = true;
                    current = 0;
                    queries.Add(commandQuery);//new

                    //do an if statement later to throw an "insert" event...???
                    setLastInsertID();

                    return true;
                }
                catch (Exception e)
                {
                    errors.Add(e.Message + " in command(commandQuery)");
                    //System.Console.WriteLine("can't do somethin here man...gah...your ruining my life...");
                    throw e;
                    //return false;
                }
            }
            else
            {
                errors.Add("Not connected to DB...Cannot command until connected");
                System.Console.WriteLine("Not Connected to a Database!\n" +
                                         "Please Connect and Try Again...\n");
                return false;
            }
        }

        /// <summary>
        /// issues a query. returns true/false for success of call
        /// </summary>
        /// <param name="toQuery">(string)</param>
        /// <returns>(bool)</returns>
        public bool query(string toQuery)
        {
            
            queryString = toQuery;

            rowNum = 0;

            if (connStatus.Equals(true))
            {
                try
                {
                    this.free();
                    mySqlQuery = mySqlConn.CreateCommand();

                    string newQuery = toQuery;

                    mySqlQuery.CommandText = toQuery;
                    queryString = toQuery;
                    if (isRead.Equals(true))
                        mySqlReader.Close();

                    if (theMode == dMode.full || theMode == dMode.medium)
                    {
                        //count rows in query
                        MySqlDataReader tempCounter = mySqlQuery.ExecuteReader();
                        tempCounter.Read();

                        theDataTable = new SqlDataArrayClass<SqlDataClass>(tempCounter);

                        if (theMode == dMode.full)
                        {
                            theTable = new SqlArrayClass(theDataTable);
                        }

                        rowNum = theDataTable.rows.Count;

                        tempCounter.Close();
                    }

                    mySqlReader = mySqlQuery.ExecuteReader();
                    mySqlReader.Read();
                    isRead = true;
                    current = 0;
                    queries.Add(toQuery);//new
                    return true;
                }
                catch (Exception e)
                {
                    errors.Add(e.Message + " in query(toQuery)");
                    //System.Console.WriteLine("can't do somethin here man...gah...your ruining my life...");
                    throw e;
                    //return false;
                }
            }
            else
            {
                errors.Add("Not connected to DB...Cannot query until connected");
                System.Console.WriteLine("Not Connected to a Database!\n" +
                                         "Please Connect and Try Again...\n");
                return false;
            }
        }

        /*
        public List<string> getColNames()
        {
            List<string> colList = new List<string>();
            try
            {
                for (x = 0; x < getColumns(); x++)
                {
                    colList.Add(mySqlReader.GetName(index));
                    //mySqlReader.
                }
            }
            catch (Exception e)
            {
                errors.Add(e.Message + " in getColNames()");
                return new List<string>();
            }
        }
        */

        /*
        public string getTableName()
        {
            
        }
        */
        private void bob2() { }

        //public 

        #endregion

    }//end of MySqlClass

}
