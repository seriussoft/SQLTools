using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using System.Collections;

using SqlDataRow = nTools.SqlTools.SqlDataRowClass<string, nTools.SqlTools.SqlDataClass>;
using SqlRow = nTools.SqlTools.SqlDataRowClass<string, string>;

namespace nTools.SqlTools
{
    /// <summary>
    /// a table orientation for a MySql result set
    /// </summary>
    public class SqlDataArrayClass<T>
    {
        #region fields
        /// <summary>
        /// will be worked out at a later time
        /// </summary>
        public readonly List<string> colList = new List<string>();
        
        /// <summary>
        /// stored in the form array[rowNum][colNum]
        /// </summary>
        //public readonly List<string[]> rows = new List<string[]>();
        private List<SqlDataRow> theRows = new List<SqlDataRow>();

        /// <summary>
        /// gets the list of SqlDataRows which can be boxed as a List of Dictionary(string,SqlDataClass)
        /// </summary>
        public List<SqlDataRow> rows
        {
            get
            {
                return theRows;
            }
        }

        /// <summary>
        /// returns the value at the given row,col indeces
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns type="string"></returns>
        public SqlDataClass this[int rowIndex, int colIndex]
        {
            get
            {
                return rows[rowIndex][colIndex];
            }
        }

        /// <summary>
        /// returns the value at the given row index, column key
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colKey"></param>
        /// <returns></returns>
        public SqlDataClass this[int rowIndex, string colKey]
        {
            get
            {
                if (colList.Contains(colKey))
                {
                    int colIndex = colList.IndexOf(colKey);
                    return rows[rowIndex][colIndex];
                }
                else
                {
                    return new SqlDataClass("Column Key: " + colKey + " was not found",dType.String);
                }
            }
        }

        /// <summary>
        /// returns the row as a string[] at the given row index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public SqlDataRow this[int rowIndex]
        {
            get
            {
                return rows[rowIndex];
            }
        }

        bool isReady = false;
        #endregion

        public bool readiness
        {
            get { return isReady; }
        }

        #region constructors

        /// <summary>
        /// empty constructor
        /// </summary>
        /// <param name="reader"></param>
        public SqlDataArrayClass(MySqlDataReader reader)
        {
            if (reader.HasRows)
            {
                setUpArray(reader);
            }
            else
            {
                //do nothing.... :( 
            }
        }

        /// <summary>
        /// takes a MySqlClass as parameter...make sure to have already sent a query to the server
        /// and also that you queried with some form of select statement...i give no mercy for 
        /// stupidity...if you don't get an array, but get an error, your bad, not mine ;0
        /// </summary>
        /// <param name="mysql">(SqlTools.MySqlClass)</param>
        /*
        public SqlArrayClass(MySqlClass mysql)
        {
            if (mysql.isConnected())
            {
                //this = mysql.table;
                
                if (MySqlClass.queries.IndexOf("SHOW DATABASES") > -1)
                {
                    System.Console.WriteLine("doing something good so far");
                    setUpArrays2(mysql);
                }
                else
                setUpArrays(mysql);
                
            }
            else
            {
               //do nothing.... :( 
            }
        }
        */

        public SqlDataArrayClass()
        {

        }

        #endregion

        #region setUpArray
        /// <summary>
        /// makes a table of strings that represents the result set
        /// </summary>
        /// <param name="reader"></param>
        void setUpArray(MySqlDataReader reader)
        {
            //get column names stored in colList
            foreach (DataRow row in reader.GetSchemaTable().Rows)
            {
                colList.Add(row[0].ToString());
            }

            int colCount = colList.Count;

            do
            {
                //string[] row = new string[colCount];
                //Hashtable a = new Hashtable();
                //Dictionary<string,string> row1 = new Dictionary<string, string>(colCount);

                SqlDataRow row = new SqlDataRow(colList);

                for(int colNum=0; colNum<colCount; colNum++)
                {
                    //row[colNum] = reader.GetString(colNum).ToString();
                    //row1.Add(colList[colNum], reader.GetString(colNum).ToString());
                    row.Add(colList[colNum], new SqlDataClass(reader.GetString(colNum), reader.GetValue(colNum).GetType().ToString()));
                }

                rows.Add(row);

            }
            while(reader.Read());

            isReady = true;
        }

        /*
        void setUpArrays(MySqlClass mysql)
        {
            for (int colNum = 0; colNum < mysql.getColumns(); colNum++)
            {
                colList.Add(mysql.colName[colNum]);
            }

            for (int rowNum = 0; rowNum < mysql.getRows(); rowNum++)
            {
                List<string> oneRow = new List<string>();
                for(int colNum = 0; colNum < mysql.getColumns(); colNum++)
                {
                    oneRow.Add(mysql.getVar(colNum).getString());
                }
                array.Add(oneRow.ToArray());
                mysql.next();
            }
            isReady = true;
        }

        void setUpArrays2(MySqlClass mysql)
        {
            for (int colNum = 0; colNum < mysql.getColumns(); colNum++)
                colList.Add("");

            string tmpRow = mysql.getVar(0).getString();

            Console.WriteLine(tmpRow);

            while(mysql.next())
            {
                List<string> oneRow = new List<string>();
                oneRow.Add(tmpRow);
                tmpRow = mysql.getVar(0).getString();

                array.Add(oneRow.ToArray());

                Console.WriteLine(tmpRow);
            }

            array.Add(new string[] { tmpRow });

            isReady = true;
        }
        */
        private void bob() { }

        #endregion
    }//end sqlArrayClass

}//end namespace
