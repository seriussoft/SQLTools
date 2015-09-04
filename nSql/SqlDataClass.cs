using System;
using System.Collections.Generic;
using System.Text;

namespace nTools.SqlTools
{
    /// <summary>
    /// <value>String</value>
    /// /// <value>Integer</value>
    /// /// <value>Bool</value>
    /// /// <value>Double</value>
    /// </summary>
    [Flags]
    public enum dType
    {
        String,
        Integer,
        Bool,
        Double
    }


    public class SqlDataClass
    {
        #region Fields

        private string data;
        public readonly List<string> errors;
        private dType type;

        #endregion

        #region Methods


        public SqlDataClass()
        {
            this.errors = new List<string>();
            this.type = dType.String;
            this.data = "";
        }

        /// <summary>
        /// overloaded cstr that takes in string and type all at once
        /// </summary>
        /// <param name="sData">(string)</param>
        /// <param name="sType">(dType)</param>
        public SqlDataClass(string sData, dType sType)
        {
            this.errors = new List<string>();
            this.data = sData;
            this.type = sType;
        }

        /// <summary>
        /// overloaded cstr that takes in string, string
        /// </summary>
        /// <param name="sDataI">(string)</param>
        /// <param name="sTypeI">(string)</param>
        public SqlDataClass(string sDataI, string sTypeI)
        {
            this.errors = new List<string>();
            this.store(sDataI, sTypeI);
        }

        /// <summary>
        /// returns value stored in data in the same format as it was stored in db
        /// </summary>
        /// <returns>(object)</returns>
        public object get()
        {
            switch (this.type)
            {
                case dType.String:
                    return this.data;

                case dType.Integer:
                    return int.Parse(this.data);

                case dType.Bool:
                    return bool.Parse(this.data);

                case dType.Double:
                    return double.Parse(this.data);
            }
            return this.data;
        }

        /// <summary>
        /// returns data as bool no matter what data type.
        /// is for people who wish to not make a temp SqlDataClass for returns
        /// </summary>
        /// <returns>(bool)</returns>
        public bool getBool()
        {
            return bool.Parse(this.data);
        }

        /// <summary>
        /// returns data as double no matter what data type.
        /// is for people who wish to not make a temp SqlDataClass for returns
        /// </summary>
        /// <returns>(double)</returns>
        public double getDouble()
        {
            return double.Parse(this.data);
        }

        /// <summary>
        /// returns data as int no matter what data type.
        /// is for people who wish to not make a temp SqlDataClass for returns
        /// </summary>
        /// <returns>(int)</returns>
        public int getInt()
        {
            return int.Parse(this.data);
        }

        /// <summary>
        /// returns data as string no matter what data type.
        /// is for people who wish to not make a temp SqlDataClass for returns
        /// </summary>
        /// <returns>(string)</returns>
        public string getString()
        {
            return this.data;
        }

        /// <summary>
        /// returns the dataType of the stored data
        /// </summary>
        /// <returns>(dType)</returns>
        public dType getType()
        {
            return this.type;
        }

        /// <summary>
        /// returns true/false on success
        /// </summary>
        /// <param name="cData">(ref bool)</param>
        /// <returns>(bool)</returns>
        public bool putIn(ref bool cData)
        {
            if (this.type.Equals(dType.Bool))
            {
                cData = bool.Parse(this.data);
                return true;
            }
            this.errors.Add(base.GetType().ToString() + ":" + this.ToString() + " is not of type bool");
            Console.WriteLine(base.GetType().ToString() + ":" + this.ToString() + " is not of type bool");
            return false;
        }

        /// <summary>
        /// returns true/false on success
        /// </summary>
        /// <param name="cData">(ref double)</param>
        /// <returns>(bool)</returns>
        public bool putIn(ref double cData)
        {
            if (this.type.Equals(dType.Double))
            {
                cData = double.Parse(this.data);
                return true;
            }
            this.errors.Add(this.getType().ToString() + ":" + this.ToString() + " is not of type double");
            Console.WriteLine(this.getType().ToString() + ":" + this.ToString() + " is not of type double");
            return false;
        }

        /// <summary>
        /// returns true/false on success
        /// </summary>
        /// <param name="cData">(ref int)</param>
        /// <returns>(bool)</returns>
        public bool putIn(ref int cData)
        {
            if (this.type.Equals(dType.Integer))
            {
                cData = int.Parse(this.data);
                return true;
            }
            this.errors.Add(base.GetType().ToString() + ":" + this.ToString() + " is not of type int");
            Console.WriteLine(base.GetType().ToString() + ":" + this.ToString() + " is not of type int");
            return false;
        }

        /// <summary>
        /// returns true/false on success
        /// </summary>
        /// <param name="cData">(ref object)</param>
        /// <returns>(bool)</returns>
        public void putIn(ref object cData)
        {
            switch (this.type)
            {
                case dType.String:
                    cData = this.data;
                    return;

                case dType.Integer:
                    cData = int.Parse(this.data);
                    return;

                case dType.Bool:
                    cData = bool.Parse(this.data);
                    return;

                case dType.Double:
                    cData = double.Parse(this.data);
                    return;
            }
            cData = this.data;
        }

        /// <summary>
        /// returns true/false on success
        /// </summary>
        /// <param name="cData">(ref string)</param>
        /// <returns>(bool)</returns>
        public void putIn(ref string cData)
        {
            cData = this.data;
        }

        /// <summary>
        /// stores the data and type to this class
        /// </summary>
        /// <param name="sData">(string)</param>
        /// <param name="sType">(dType)</param>
        public void store(string sData, dType sType)
        {
            this.data = sData;
            this.type = sType;
        }

        /// <summary>
        /// stores the data and type to this class (converts string sType to dType)
        /// </summary>
        /// <param name="sData">(string)</param>
        /// <param name="sType">(string)</param>
        public void store(string sData, string sType)
        {
            this.data = sData;
            sType = sType.ToLower();
            switch (sType)
            {
                case "string":
                    this.type = dType.String;
                    return;

                case "system.int32":
                case "system.int64":
                case "integer":
                    this.type = dType.Integer;
                    return;

                case "int":
                    this.type = dType.Integer;
                    return;

                case "double":
                    this.type = dType.Double;
                    return;

                case "bool":
                    this.type = dType.Bool;
                    return;

                case "boolean":
                    this.type = dType.Bool;
                    return;
            }
            this.type = dType.String;
        }

        #endregion
    }//end class
}//end namespace