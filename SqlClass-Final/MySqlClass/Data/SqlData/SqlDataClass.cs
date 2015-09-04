using System;
using System.Collections.Generic;
using System.Text;

namespace nTools.SqlTools
{
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
     *   public putIn(decimal)
     *   public putIn(DateTime)
     *   public putIn(bool)
     *   public putIn(object)
     *   public string getString()
     *   public int getInt()
     *   public double getDouble()
     *   public decimal getDecimal()
     *   public DateTime getDateTime()
     *   public bool getBool()
    ***/

    /// <summary>
    /// the SqlDataClass, it will hold an object and its type. Comes with functions to get its true value,
    /// to store a value, or to get/set a string value
    /// </summary>
    public class SqlDataClass
    {
        private string data;
        private dType type;

        /// <summary>
        /// </summary>
        /// <type>readonly List&lt;string&gt;</type>
        public readonly List<string> errors = new List<string>();

        #region constructors
        /// <summary>
        /// empty constructor
        /// </summary>
        public SqlDataClass() { type = dType.String; data = ""; }

        /// <summary>
        /// overloaded constructor that takes in string and type all at once
        /// </summary>
        /// <param name="sData">(string)</param>
        /// <param name="sType">(dType)</param>
        public SqlDataClass(string sData, dType sType)
        {
            data = sData;
            type = sType;
        }
        
        /// <summary>
        /// overloaded constructor that takes in string,string
        /// </summary>
        /// <param name="sDataI">(string)</param>
        /// <param name="sTypeI">(string)</param>
        public SqlDataClass(string sDataI, string sTypeI)
        {
            this.store(sDataI, sTypeI);
        }
        #endregion

        /// <summary>
        /// stores the data and type to this class
        /// </summary>
        /// <param name="sData">(string)</param>
        /// <param name="sType">(dType)</param>
        public void store(string sData, dType sType)
        {
            data = sData;
            type = sType;
        }

        /// <summary>
        /// stores the data and type to this class (converts string sType to dType)
        /// </summary>
        /// <param name="sData">(string)</param>
        /// <param name="sType">(string)</param>
        public void store(string sData, string sType)
        {
            data = sData;
            sType = sType.ToLower();

            switch (sType)
            {
                case "string": type = dType.String; break;
                case "system.int16":
                case "system.int32":
                case "system.int64":
                case "integer": 
                case "int": type = dType.Integer; break;
                case "system.double": 
                case "double": type = dType.Double; break;
                case "system.decimal": 
                case "decimal": type = dType.Decimal; break;
                case "system.datetime": 
                case "datetime": type = dType.DateTime; break;
                case "system.bool": 
                case "system.boolean": 
                case "bool": 
                case "boolean": type = dType.Bool; break;
                default: type = dType.String; break;
            }

        }

        /// <summary>
        /// returns the dataType of the stored data
        /// </summary>
        /// <returns>(dType)</returns>
        public dType getType()
        {
            return type;
        }

        /// <summary>
        /// returns the value stored in data in the same format as it was stored
        /// in the database.
        /// </summary>
        /// <returns>(object)</returns>
        public object get()
        {
            object oData;

            //might need to fix switch statement and add break; on each part of it.
            //because supposedly c# doesn't support fall-downs like c++ && java
            switch (type)
            {
                case dType.String: oData = data; break;
                case dType.Integer: oData = int.Parse(data); break;
                case dType.Double: oData = double.Parse(data); break;
                case dType.Bool: oData = bool.Parse(data); break;
                case dType.Decimal: oData = Decimal.Parse(data); break;
                case dType.DateTime: oData = DateTime.Parse(data); break;
                default: oData = data; break;
            }

            return oData;
        }

        #region putIn(...)
        /// <summary>
        /// putIn(ref string)
        /// </summary>
        /// <param name="cData">(ref string)</param>
        public void putIn(ref string cData)
        {
            cData = data;
        }

        /// <summary>
        /// putIn(ref int) returns true/false on success
        /// </summary>
        /// <param name="cData">(ref int)</param>
        /// <returns>(bool)</returns>
        public bool putIn(ref int cData)
        {
            if (type.Equals(dType.Integer))
                cData = int.Parse(data);
            else
            {
                errors.Add(this.GetType().ToString() + ":" + this.ToString() + " is not of type int");
                System.Console.WriteLine(this.GetType().ToString() + ":" + this.ToString() + " is not of type int");
                return false;
            }
            return true;
        }

        /// <summary>
        /// putIn(ref double) returns true/false on success
        /// </summary>
        /// <param name="cData">(ref double)</param>
        /// <returns>(bool)</returns>
        public bool putIn(ref double cData)
        {
            if (type.Equals(dType.Double))
                cData = double.Parse(data);
            else
            {
                errors.Add(this.getType().ToString() + ":" + this.ToString() + " is not of type double");
                System.Console.WriteLine(this.getType().ToString() + ":" + this.ToString() + " is not of type double");
                return false;
            }
            return true;
        }

        /// <summary>
        /// putIn(ref bool) returns true/false on success
        /// </summary>
        /// <param name="cData">(ref bool)</param>
        /// <returns>(bool)</returns>
        public bool putIn(ref bool cData)
        {
            if (type.Equals(dType.Bool))
                cData = bool.Parse(data);
            else
            {
                errors.Add(this.GetType().ToString() + ":" + this.ToString() + " is not of type bool");
                System.Console.WriteLine(this.GetType().ToString() + ":" + this.ToString() + " is not of type bool");
                return false;
            }
            return true;
        }

        /// <summary>
        /// putInt(ref Decimal) returns true/false on success
        /// </summary>
        /// <param name="cData">(ref Decimal)</param>
        /// <returns>(bool)</returns>
        public bool putIn(ref Decimal cData)
        {
            if (type.Equals(dType.Decimal))
                cData = Decimal.Parse(data);
            else
            {
                errors.Add(this.GetType().ToString() + ":" + this.ToString() + " is not of type bool");
                System.Console.WriteLine(this.GetType().ToString() + ":" + this.ToString() + " is not of type bool");
                return false;
            }
            return true;
        }

        /// <summary>
        /// putIn(ref DateTime) returns true/false on success
        /// </summary>
        /// <param name="cData">(ref DateTime)</param>
        /// <returns>(bool)</returns>
        public bool putIn(ref DateTime cData)
        {
            if (type.Equals(dType.DateTime))
                cData = DateTime.Parse(data);
            else
            {
                errors.Add(this.GetType().ToString() + ":" + this.ToString() + " is not of type bool");
                System.Console.WriteLine(this.GetType().ToString() + ":" + this.ToString() + " is not of type bool");
                return false;
            }
            return true;
        }

        /// <summary>
        /// putIn(ref object) returns true/false on success
        /// </summary>
        /// <param name="cData">(ref object)</param>
        public void putIn(ref object cData)
        {
            switch (type)
            {
                case dType.String: cData = data; break;
                case dType.Integer: cData = int.Parse(data); break;
                case dType.Double: cData = double.Parse(data); break;
                case dType.Bool: cData = bool.Parse(data); break;
                case dType.Decimal: cData = Decimal.Parse(data); break;
                case dType.DateTime: cData = DateTime.Parse(data); break;
                default: cData = data; break;
            }
        }
        #endregion

        #region get string/int/double/bool
        /// <summary>
        /// returns data as a string no matter what the data type is
        ///  for people who wish to not make a temp SqlDataClass for returns
        ///  from mySqlClass
        /// </summary>
        /// <returns>(string)</returns>
        public string getString()
        {
            return data;
        }

        /// <summary>
        /// returns data as an int no matter what the data type is
        ///  for people who wish to not make a temp SqlDataClass for returns
        ///  from mySqlClass
        /// </summary>
        /// <returns>(int)</returns>
        public int getInt()
        {
            return int.Parse(data);
        }

        /// <summary>
        /// returns data as a double no matter what the data type is
        ///  for people who wish to not make a temp SqlDataClass for returns
        ///  from mySqlClass
        /// </summary>
        /// <returns>(double)</returns>
        public double getDouble()
        {
            return double.Parse(data);
        }

        /// <summary>
        /// returns data as a Decimal no matter what the data type is 
        /// for people who wish to not make a temp SqlDataClass for returns 
        /// from mySqlClass
        /// </summary>
        /// <returns>(Decimal)</returns>
        public Decimal getDecimal()
        {
            return Decimal.Parse(data);
        }

        /// <summary>
        /// returns data as a DateTime no matter what the data type is 
        /// for people who wish to not make a temp SqlDataClass for returns 
        /// from mySqlClass
        /// </summary>
        /// <returns>(DateTime)</returns>
        public DateTime getDateTime()
        {
            return DateTime.Parse(data);
        }

        /// <summary>
        /// returns data as a bool no matter what the data type is
        ///  for people who wish to not make a temp SqlDataClass for returns
        ///  from mySqlClass
        /// </summary>
        /// <returns>(bool)</returns>
        public bool getBool()
        {
            return bool.Parse(data);
        }
        #endregion

    }//end of SqlDataClass
}
