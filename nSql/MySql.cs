using System;
using System.Collections.Generic;
using System.Text;
//using MySql;
//using MySql.Data;
using MySql.Data.MySqlClient;

namespace nTools.SqlTools
{
    public class MySqlClass
    {

        #region Fields

            private bool connStatus;
            private string connStr;
            private int current;
            public readonly List<string> errors;
            private bool isRead;
            protected static MySqlConnection mySqlConn;
            protected MySqlCommand mySqlQuery;
            protected MySqlDataReader mySqlReader;
            public static readonly List<string> queries = new List<string>();
            private string queryString;

        #endregion

        #region Methods

        #region Cstrs

        public MySqlClass()
        {
            this.queryString = "";
            this.connStr = "";
            this.current = 0;
            this.connStatus = false;
            this.isRead = false;
            this.errors = new List<string>();
        }

        public MySqlClass(string connection)
        {
            this.queryString = "";
            this.errors = new List<string>();
            this.current = 0;
            this.connStatus = false;
            this.connStr = connection;
            this.isRead = false;
            try
            {
                mySqlConn = new MySqlConnection(this.connStr);
                mySqlConn.Open();
                this.connStatus = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                this.connStatus = false;
                this.errors.Add(exception.Message + " in constructor 2.");
                throw new Exception("MySqlClass constructor 2: \n", exception);
            }
        }

        public MySqlClass(string server, string userId, string pass, string db)
        {
            this.queryString = "";
            this.errors = new List<string>();
            this.current = 0;
            this.connStatus = false;
            this.isRead = false;
            this.connStr = "SERVER=" + server + ";UID=" + userId + ";PASSWORD=" + pass + "DATABASE=" + db + ";";
            try
            {
                mySqlConn = new MySqlConnection(this.connStr);
                mySqlConn.Open();
                this.connStatus = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                this.connStatus = false;
                this.errors.Add(exception.Message + " in constructor 3");
                throw new Exception("MySqlClass constructor 3: \n", exception);
            }
        }
        
        #endregion

        public void disconn()
        {
            if (this.isConnected())
            {
                mySqlConn.Close();
            }
        }

        #region Gets

        public int getColumns()
        {
            if (this.mySqlReader.HasRows.Equals(true))
            {
                return this.mySqlReader.FieldCount;
            }
            return 0;
        }

        protected string getConn()
        {
            return this.connStr;
        }

        public int getRows()
        {
            try
            {
                MySqlCommand command = mySqlConn.CreateCommand();
                command.CommandText = this.queryString;
                MySqlDataReader reader = command.ExecuteReader();
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
                this.errors.Add("There are no rows returned for this query");
                return -1;
            }
        }

        public SqlDataClass getVar(int column)
        {
            int ordinal = column;
            SqlDataClass class2 = new SqlDataClass();
            try
            {
                class2.store(this.mySqlReader.GetValue(ordinal).ToString(), this.parseType(this.mySqlReader.GetFieldType(ordinal).ToString()));
            }
            catch (Exception exception)
            {
                this.errors.Add(exception.Message + " in getVar(int)");
                Console.WriteLine(this.current + ") SoL: " + exception.Message);
            }
            if (!class2.getString().Equals((string)null))
            {
                return class2;
            }
            return new SqlDataClass("no_value_to_be_read", dType.String);
        }

        public SqlDataClass getVar(string column)
        {
            string str = column;
            SqlDataClass class2 = new SqlDataClass();
            try
            {
                class2.store(this.mySqlReader[str].ToString(), this.parseType(this.mySqlReader[str].GetType().ToString()));
            }
            catch (Exception exception)
            {
                this.errors.Add(exception.Message + " in getVar(string)");
                Console.WriteLine(this.current + ") SoL: " + exception.Message);
            }
            if (!class2.getString().Equals((string)null))
            {
                return class2;
            }
            return new SqlDataClass("no_value_to_be_read", dType.String);
        }

        public void getVar(int column, ref object toStore)
        {
            int ordinal = column;
            SqlDataClass class2 = new SqlDataClass();
            try
            {
                class2.store(this.mySqlReader.GetValue(ordinal).ToString(), this.parseType(this.mySqlReader.GetFieldType(ordinal).ToString()));
            }
            catch (Exception exception)
            {
                this.errors.Add(exception.Message + " in getVar(int, ref object)");
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

        public void getVar(string column, ref object toStore)
        {
            string str = column;
            SqlDataClass class2 = new SqlDataClass();
            try
            {
                class2.store(this.mySqlReader[str].ToString(), this.parseType(this.mySqlReader[str].GetType().ToString()));
            }
            catch (Exception exception)
            {
                this.errors.Add(exception.Message + " in getVar(string, ref object)");
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

        public bool isConnected()
        {
            return this.connStatus;
        }

        public MySqlClass makeCopyOf(MySqlClass toCopy)
        {
            return new MySqlClass(toCopy.getConn());
        }

        public bool next()
        {
            if (this.mySqlReader.Read().Equals(true))
            {
                this.current++;
                return true;
            }
            return false;
        }

        public static int numQueries()
        {
            return queries.Count;
        }

        public dType parseType(string sType)
        {
            sType = sType.ToLower();
            sType = sType.Remove(0, 7);
            switch (sType)
            {
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

        public bool ping()
        {
            return mySqlConn.Ping();
        }

        public bool query(string toQuery)
        {
            this.queryString = toQuery;
            if (this.connStatus.Equals(true))
            {
                try
                {
                    this.mySqlQuery = mySqlConn.CreateCommand();
                    this.mySqlQuery.CommandText = toQuery;
                    if (this.isRead.Equals(true))
                    {
                        this.mySqlReader.Close();
                    }
                    this.mySqlReader = this.mySqlQuery.ExecuteReader();
                    this.mySqlReader.Read();
                    this.isRead = true;
                    this.current = 0;
                    queries.Add(toQuery);
                    return true;
                }
                catch (Exception exception)
                {
                    this.errors.Add(exception.Message + " in query(toQuery)");
                    throw exception;
                }
            }
            this.errors.Add("Not connected to DB...Cannot query until connected");
            Console.WriteLine("Not Connected to a Database!\nPlease Connect and Try Again...\n");
            return false;
        }

        #endregion

        #region Sets

        public void setConn(string connection)
        {
            this.connStr = connection;
            if (!this.connStr.Contains("SERVER="))
            {
                this.errors.Add("MISSING \"SERVER=%DBSERVER%\"");
                throw new Exception("MISSING \"SERVER=%DBSERVER%\"");
            }
            if (!this.connStr.Contains(";UID="))
            {
                this.errors.Add("MISSING \";UID=%USERID%\"");
                throw new Exception("MISSING \";UID=%USERID%\"");
            }
            if (!this.connStr.Contains(";PASSWORD="))
            {
                this.errors.Add("MISSING \";PASSWORD=%PASS%\"");
                throw new Exception("MISSING \";PASSWORD=%PASS%\"");
            }
            if (!this.connStr.Contains(";DATABASE="))
            {
                this.errors.Add("MISSING \";DATABASE=%DBNAME%\"");
                throw new Exception("MISSING \";DATABASE=%DBNAME%\"");
            }
            if (!this.connStr.EndsWith(";"))
            {
                this.connStr = this.connStr + ";";
            }
            try
            {
                mySqlConn = new MySqlConnection(this.connStr);
                mySqlConn.Open();
                this.connStatus = true;
            }
            catch (Exception exception)
            {
                this.connStatus = false;
                this.errors.Add(exception.Message + " in setConn(connection)");
                throw new Exception("setConn(connection): \n", exception);
            }
        }

        public void setConn(string server, string userId, string pass, string db)
        {
            this.connStr = "SERVER=" + server + ";UID=" + userId + ";PASSWORD=" + pass + ";DATABASE=" + db + ";";
            Console.WriteLine(this.connStr);
            try
            {
                mySqlConn = new MySqlConnection(this.connStr);
                mySqlConn.Open();
                this.connStatus = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                this.connStatus = false;
                this.errors.Add(exception.Message + " setConn(server,userId,pass,db)");
                throw new Exception("setConn(server,userId,pass,db): \n", exception);
            }
        }

        public bool setRow(int rowNumber)
        {
            if (this.current > rowNumber)
            {
                this.errors.Add("You are already on a row past the supplied row number");
                return false;
            }
            if (this.getRows() < (rowNumber - 1))
            {
                this.errors.Add("Supplied row number exceeds number of rows returned");
                return false;
            }
            for (int i = this.current; i < rowNumber; i++)
            {
                this.current++;
                this.next();
            }
            return true;
        }
        
        #endregion

        protected string varIs(int column)
        {
            int ordinal = column;
            Console.WriteLine(this.parseType(this.mySqlReader.GetValue(ordinal).GetType().ToString()));
            return (this.mySqlReader.GetValue(ordinal).GetType().ToString() + "\n");
        }

        #endregion

    }//end class
}//end namespace