using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;

namespace nTools.SqlTools
{
    public class MsSqlClass
    {
        //global variables
        protected static SqlConnection SqlConn;
        private string connStr;
        private bool connStatus;
        protected SqlDataReader SqlReader;
        protected SqlCommand SqlQuery;
        private int current;
        private string queryString = "";
        private bool isRead;

        public MsSqlClass()
        {
            connStr = "";
            current = 0;
            connStatus = false;
            isRead = false;
        }

    //overloaded class initiator...for connecting at same
    //step as initiation of class. parameter(string)
    //new MsSqlClass("SERVER=%server%;UID=%userId%;PASSWORD=%pass%;DATABASE=%db%;");
        public MsSqlClass(string connection)
        {
            current = 0;
            connStatus = false;
            connStr = connection;
            isRead = false;
            
            try 
            { 
                SqlConn = new SqlConnection(connStr);
                SqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                connStatus = false;
                throw new Exception("MsSqlClass constructor 2: \n", e);
            }

        }

    //overloaded class initiator for initialzing connection
    //and class in one go. parameters(strings) server,userId,pass,db
        public MsSqlClass(string server, string userId, string pass, string db)
        {
            current = 0;
            connStatus = false;
            isRead = false;
            connStr = "Data Source=" + server + ";User ID=" + userId + ";Password=" + pass + "Initial Catalog=" + db + ";";
            
            try
            {
                SqlConn = new SqlConnection(connStr);
                SqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                connStatus = false;
                //throw new Exception("MsSqlClass constructor 3: \n", e);
                throw new Exception(e.Message.ToString() + "\n\nyou suck\n\n", e);
            }

        }

    //overloaded class initiator...for copying connection
    //to a new class so that more than one query can be
    //made with same connection
        public MsSqlClass makeCopyOf(MsSqlClass toCopy)
        {
            return new MsSqlClass(toCopy.getConn());
        }

        protected String getConn()
        {
            return connStr;
        }

    //connects to db with supplied parameters(strings) server,userId,pass,dbName
        public void setConn(string server, string userId, string pass, string db)
        {
            connStr = "SERVER=" + server +";UID=" + userId +";PASSWORD=" + pass +";DATABASE=" + db +";";
            System.Console.WriteLine(connStr);
            try 
            { 
                SqlConn = new SqlConnection(connStr);
                SqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                connStatus = false;
                throw new Exception("setConn(server,userId,pass,db): \n", e);
            }
        }

    //connects to db with supplied connection string in format
    //"SERVER=%server%;UID=%userId%;PASSWORD=%pass%;DATABASE=%dbName%;"
        public void setConn(string connection)
        {
            connStr = connection;

        //checks to make sure string contains all 4 of variables required
        //to make a connection and follows the format:
        //"SERVER=%server%;UID=%userId%;PASSWORD=%pass%;DATABASE=%dbName%;"
            if (!connStr.Contains("SERVER="))
                throw new Exception("MISSING \"SERVER=%DBSERVER%\"");
            else if (!connStr.Contains(";UID="))
                throw new Exception("MISSING \";UID=%USERID%\"");
            else if (!connStr.Contains(";PASSWORD="))
                throw new Exception("MISSING \";PASSWORD=%PASS%\"");
            else if (!connStr.Contains(";DATABASE="))
                throw new Exception("MISSING \";DATABASE=%DBNAME%\"");
            else if (!connStr.EndsWith(";"))
                connStr += ";";

            try
            {
                SqlConn = new SqlConnection(connStr);
                SqlConn.Open();
                connStatus = true;
            }
            catch (Exception e)
            {
                //System.Console.WriteLine(e);
                connStatus = false;
                throw new Exception("setConn(connection): \n", e);
            }
        }

    //returns true/false if connected
        public bool isConnected()
        {
            return connStatus;
        }

/*
 *      System.Data.SqlClient doesn't allow for the nifty feature of ping()
 *      like the MySqlClient does. too bad...it makes me sad...
    
        public bool ping()
        {
            return SqlConn.Ping();
        }
*/

    //disconnects if connected
        public void disconn()
        {
            if (isConnected())
                SqlConn.Close();
        }

    //issues a query. returns true/false for success of call
        public bool query(string toQuery)
        {
            queryString = toQuery;

            if (connStatus.Equals(true))
            {
                try
                {
                    SqlQuery = SqlConn.CreateCommand();
                    SqlQuery.CommandText = toQuery;
                    if(isRead.Equals(true))
                        SqlReader.Close();
                    SqlReader = SqlQuery.ExecuteReader();
                    SqlReader.Read();
                    isRead = true;
                    current = 0;
                    return true;
                }
                catch (Exception e)
                {
                    //System.Console.WriteLine("can't do somethin here man...gah...your ruining my life...");
                    throw e;
                    //return false;
                }
            }
            else
            {
                System.Console.WriteLine("Not Connected to a Database!\n" +
                                         "Please Connect and Try Again...\n");
                return false;
            }
        }

    //sets reader to next resultSet
        public bool next()
        {
            if (SqlReader.Read().Equals(true))
            {
                current++;
                return true;
            }
            else
                return false;
        }

    //returns value @ column(x) of current row or resultSet
        public SqlDataClass getVar(int column)
        {
            int x = column;
            SqlDataClass tmp = new SqlDataClass();

            //if(SqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(SqlReader.GetValue(x).ToString(), parseType(SqlReader.GetFieldType(x).ToString()));
            }
            catch (Exception e)
            {
                current++;
                System.Console.WriteLine(current + ") SoL: " + e.Message);
            }

            if (!tmp.getString().Equals(null))
                return tmp;
            else return new SqlDataClass("no_value_to_be_read",dType.String);
            
            //else throw "no_value_to_be_read";
        }

        public void getVar(int column, ref object toStore)
        {
            int x = column;
            SqlDataClass tmp = new SqlDataClass();

            //if(SqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(SqlReader.GetValue(x).ToString(), parseType(SqlReader.GetFieldType(x).ToString()));
            }
            catch (Exception e)
            {
                current++;
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

        public SqlDataClass getVar(string column)
        {
            string x = column;
            SqlDataClass tmp = new SqlDataClass();

            //if(SqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(SqlReader[x].ToString(), parseType(SqlReader[x].GetType().ToString()));
            }
            catch (Exception e)
            {
                current++;
                System.Console.WriteLine(current + ") SoL: " + e.Message);
            }

            if (!tmp.getString().Equals(null))
                return tmp;
            else return new SqlDataClass("no_value_to_be_read", dType.String);

            //else throw "no_value_to_be_read";
        }

        public void getVar(string column, ref object toStore)
        {
            string x = column;
            SqlDataClass tmp = new SqlDataClass();

            //if(SqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(SqlReader[x].ToString(), parseType(SqlReader[x].GetType().ToString()));
            }
            catch (Exception e)
            {
                current++;
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

        //gets number of columns at current rowSet
        public int getColumns()
        {
            if (SqlReader.HasRows.Equals(true))
                return SqlReader.FieldCount;
            else
                return 0;
        }

        public int getRows()
        {
            //string rowCall = "SELECT COUNT(" + columnNames + ") FROM " + tableName;
            
            try
            {
                SqlCommand rows = SqlConn.CreateCommand();
                rows.CommandText = queryString;
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

    //returns dType.(string/bool/int/double) of value
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

    //puts the type and value into a string
        protected string varIs(int column)
        {
            int x = column;
            //if (SqlReader.NextResult().Equals(true))
            System.Console.WriteLine(parseType(SqlReader.GetValue(x).GetType().ToString()));
            return SqlReader.GetValue(x).GetType().ToString() + "\n";// +SqlReader.GetString(x) + "\n";
            //else
                //return "SoS";
        }

    }//end of MsSqlClass

}
