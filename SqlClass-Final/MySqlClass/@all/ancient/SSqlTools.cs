/*******************************
 * MySqlTools Namespace v1.0
 *      nathan vanbuskirk
 *      july 10, 2007
 * 
 * contains MySqlClass && SqlDataClass
 * also contains an enum dType{String,Integer,Double,Bool}
 *******************************/

using System;
//using System.Exception;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace MySqlTools
{
    [Flags] public enum dType{String, Integer, Bool, Double}

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
     *   public bool query(string toQuery)
     *   public SqlDataClass nextVar()
    ***/

    public class MySqlClass
    {
    //global variables
        protected static MySqlConnection mySqlConn;
        private string connStr;
        private bool connStatus;
        protected MySqlDataReader mySqlReader;
        protected MySqlCommand mySqlQuery;
        private int current;
        private string queryString = "";
        private bool isRead;

        public MySqlClass()
        {
            connStr = "";
            current = 0;
            connStatus = false;
            isRead = false;
        }

    //overloaded class initiator...for connecting at same
    //step as initiation of class. parameter(string)
    //new MySqlClass("SERVER=%server%;UID=%userId%;PASSWORD=%pass%;DATABASE=%db%;");
        public MySqlClass(string connection)
        {
            current = 0;
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
                throw new Exception("MySqlClass constructor 2: \n", e);
            }

        }

    //overloaded class initiator for initialzing connection
    //and class in one go. parameters(strings) server,userId,pass,db
        public MySqlClass(string server, string userId, string pass, string db)
        {
            current = 0;
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
                throw new Exception("MySqlClass constructor 3: \n", e);
            }

        }

    //overloaded class initiator...for copying connection
    //to a new class so that more than one query can be
    //made with same connection
        public MySqlClass makeCopyOf(MySqlClass toCopy)
        {
            return new MySqlClass(toCopy.getConn());
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
                mySqlConn = new MySqlConnection(connStr);
                mySqlConn.Open();
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
                mySqlConn = new MySqlConnection(connStr);
                mySqlConn.Open();
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

        public bool ping()
        {
            return mySqlConn.Ping();
        }

    //disconnects if connected
        public void disconn()
        {
            if (isConnected())
                mySqlConn.Close();
        }

    //issues a query. returns true/false for success of call
        public bool query(string toQuery)
        {
            queryString = toQuery;

            if (connStatus.Equals(true))
            {
                try
                {
                    mySqlQuery = mySqlConn.CreateCommand();
                    mySqlQuery.CommandText = toQuery;
                    if(isRead.Equals(true))
                        mySqlReader.Close();
                    mySqlReader = mySqlQuery.ExecuteReader();
                    mySqlReader.Read();
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
            if (mySqlReader.Read().Equals(true))
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

            //if(mySqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(mySqlReader.GetValue(x).ToString(), parseType(mySqlReader.GetFieldType(x).ToString()));
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

            //if(mySqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(mySqlReader.GetValue(x).ToString(), parseType(mySqlReader.GetFieldType(x).ToString()));
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

            //if(mySqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(mySqlReader[x].ToString(), parseType(mySqlReader[x].GetType().ToString()));
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

            //if(mySqlReader.GetValue.Equals(true))
            try
            {
                tmp.store(mySqlReader[x].ToString(), parseType(mySqlReader[x].GetType().ToString()));
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
            if (mySqlReader.HasRows.Equals(true))
                return mySqlReader.FieldCount;
            else
                return 0;
        }

        public int getRows()
        {
            //string rowCall = "SELECT COUNT(" + columnNames + ") FROM " + tableName;
            
            try
            {
                MySqlCommand rows = mySqlConn.CreateCommand();
                rows.CommandText = queryString;
                MySqlDataReader rowsFound = rows.ExecuteReader();
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
            //if (mySqlReader.NextResult().Equals(true))
            System.Console.WriteLine(parseType(mySqlReader.GetValue(x).GetType().ToString()));
            return mySqlReader.GetValue(x).GetType().ToString() + "\n";// +mySqlReader.GetString(x) + "\n";
            //else
                //return "SoS";
        }

    }//end of MySqlClass

/***********************************************************************************/

    /*** SqlData class
     *   
     *   holds one string(data) and one dType(dataType)
     *   depends on enum dType used in this namespace above
     *   constructor just stores default data = "", type = dType.String
     *   another constructor stores data and type as parameters from user  
     *   public void store(string sData, dType sType)
     *   public void store(string sData, string sType)
     *   public dType getType()
     *   public object get()
     *   public putIn(string)
     *   public putIn(int)
     *   public putIn(double)
     *   public putIn(bool)
     *   public putIn(object)
     *   public string getString()
     *   public int getInt()
     *   public double getDouble()
     *   public bool getBool()
    ***/

    public class SqlDataClass
    {
        private string data;
        private dType type;

        public SqlDataClass() { type = dType.String; data = ""; }

    //overloaded constructor that takes in string and type all at once
        public SqlDataClass(string sData, dType sType)
        {
            data = sData;
            type = sType;
        }

    //overloaded constructor that takes in string,string
        public SqlDataClass(string sDataI, string sTypeI)
        {
            this.store(sDataI,sTypeI);
        }

    //stores the data and type to this class
        public void store(string sData, dType sType)
        {
            data = sData;
            type = sType;
        }

    //stores the data and type to this class (converts string sType to dType)
        public void store(string sData, string sType)
        {
            data = sData;
            sType = sType.ToLower();

            switch (sType)
            {
                case "string": type = dType.String; break;
                case "system.int32":
                case "system.int64":
                case "integer": type =dType.Integer; break;
                case "int": type = dType.Integer; break;
                case "double": type = dType.Double; break;
                case "bool": type = dType.Bool; break;
                case "boolean": type = dType.Bool; break;
                default: type = dType.String; break;
            }

        }

    //returns the dataType of the stored data
        public dType getType()
        {
            return type;
        }

    //returns the value stored in data in the same format as it was stored
    //in the database.
        public object get()
        {
            object oData;

        //might need to fix switch statement and add break; on each part of it.
        //because supposedly c# doesn't support fall-downs like c++ && java
            switch(type)
            {
                case dType.String: oData = data; break;
                case dType.Integer: oData = int.Parse(data); break;
                case dType.Double: oData = double.Parse(data); break;
                case dType.Bool: oData = bool.Parse(data); break;
                default: oData = data; break;
            }

            return oData;
        }

    //putIn(ref string/int/double/bool/object)
        public void putIn(ref string cData)
        {
                cData = data;
        }

        public void putIn(ref int cData)
        {
            if (type.Equals(dType.Integer))
                cData = int.Parse(data);
            else
                System.Console.WriteLine(this.GetType().ToString() + ":" + this.ToString() + " is not of type int");
        }

        public void putIn(ref double cData)
        {
            if (type.Equals(dType.Double))
                cData = double.Parse(data);
            else
                System.Console.WriteLine(this.getType().ToString() + ":" + this.ToString() + " is not of type double");
        }

        public void putIn(ref bool cData)
        {
            if(type.Equals(dType.Bool))
                cData = bool.Parse(data);
            else
                System.Console.WriteLine(this.GetType().ToString() + ":" + this.ToString() + " is not of type bool");
        }

        public void putIn(ref object cData)
        {
            switch (type)
            {
                case dType.String: cData = data; break;
                case dType.Integer: cData = int.Parse(data); break;
                case dType.Double: cData = double.Parse(data); break;
                case dType.Bool: cData = bool.Parse(data); break;
                default: cData = data; break;
            }
        }

    //returns data as a string no matter what the data type is
    //for people who wish to not make a temp SqlDataClass for returns
    //from mySqlClass
        public string getString()
        {
            return data;
        }

    //returns data as an int no matter what the data type is
    //for people who wish to not make a temp SqlDataClass for returns
    //from mySqlClass
        public int getInt()
        {
            return int.Parse(data);
        }

    //returns data as a double no matter what the data type is
    //for people who wish to not make a temp SqlDataClass for returns
    //from mySqlClass
        public double getDouble()
        {
            return double.Parse(data);
        }

    //returns data as a bool no matter what the data type is
    //for people who wish to not make a temp SqlDataClass for returns
    //from mySqlClass
        public bool getBool()
        {
            return bool.Parse(data);
        }

    }//end of SqlDataClass

}//end of MySqlTools
