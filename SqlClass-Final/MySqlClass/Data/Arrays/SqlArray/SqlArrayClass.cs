using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

#pragma warning disable 1591

namespace nTools.SqlTools
{
    /// <summary>
    /// a table orientation for a MySql result set
    /// </summary>
    public class SqlArrayClass
    {
        #region fields
        /// <summary>
        /// will be worked out at a later time
        /// </summary>
        private List<string> theColList = new List<string>();

        /// <summary>
        /// gets the list of column headers
        /// </summary>
        public List<string> colList
        {
            get
            {
                return theColList;
            }
        }
        
        /// <summary>
        /// stored in the form array[rowNum][colNum]
        /// </summary>
        //public readonly List<string[]> rows = new List<string[]>();
        private List<SqlRowClass> theRows = new List<SqlRowClass>();

        /// <summary>
        /// gets the List of SqlRowClass's, can be boxed as List of Dictionary(string,string)
        /// </summary>
        public List<SqlRowClass> rows //= new List<SqlRowClass>()
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
        public string this[int rowIndex, int colIndex]
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
        public string this[int rowIndex, string colKey]
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
                    return "Column Key: " + colKey + " was not found";
                }
            }
        }

        /// <summary>
        /// returns the row as a string[] at the given row index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public SqlRowClass this[int rowIndex]
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
        /// supply someMySqlObject.dataTable to build a table of string values from the resultSet
        /// </summary>
        /// <param name="sqlDataArray" type="SqlTools.SqlDataArrayClass&lt;SqlTools.SqlDataClass&gt;"></param>
        public SqlArrayClass(SqlDataArrayClass<SqlDataClass> sqlDataArray)
        {
            if (sqlDataArray.rows.Count > 0)
            {
                setUpArray(sqlDataArray);
            }
            else
            {
                //do nothing
            }
        }

        /// <summary>
        /// supply the MySqlDataReader here to build a table off of the resultSet
        /// </summary>
        /// <param name="reader"></param>
        public SqlArrayClass(MySqlDataReader reader)
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

        /*
        /// <summary>
        /// takes a MySqlClass as parameter...make sure to have already sent a query to the server
        /// and also that you queried with some form of select statement...i give no mercy for 
        /// stupidity...if you don't get an array, but get an error, your bad, not mine ;0
        /// </summary>
        /// <param name="mysql">(SqlTools.MySqlClass)</param>
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

        public SqlArrayClass()
        {

        }

        #endregion

        #region setUpArray
        
        /// <summary>
        /// makes a table of strings that represents the result set
        /// </summary>
        /// <param name="sqlDataArray"></param>
        void setUpArray(SqlDataArrayClass<SqlDataClass> sqlDataArray)
        {
            try
            {
                theColList = sqlDataArray.colList;
                SqlRowClass row;

                int colCount = colList.Count;

                foreach (SqlDataRowClass<string, SqlDataClass> dataRow in sqlDataArray.rows)
                {
                    row = new SqlRowClass(colList);

                    for (int colNum = 0; colNum < colCount; colNum++)
                    {
                        row.Add(theColList[colNum], dataRow[colNum].getString());                                                
                    }

                    theRows.Add(row);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// makes a table of strings that represents the result set
        /// </summary>
        /// <param name="reader"></param>
        void setUpArray(MySqlDataReader reader)
        {
            //get column names stored in colList
            foreach (DataRow row in reader.GetSchemaTable().Rows)
            {
                theColList.Add(row[0].ToString());
            }

            int colCount = colList.Count;

            do
            {
                //string[] row = new string[colCount];
                //Hashtable a = new Hashtable();
                //Dictionary<string,string> row1 = new Dictionary<string, string>(colCount);

                SqlRowClass row = new SqlRowClass(theColList);

                for(int colNum=0; colNum<colCount; colNum++)
                {
                    //row[colNum] = reader.GetString(colNum).ToString();
                    //row1.Add(colList[colNum], reader.GetString(colNum).ToString());
                    row.Add(colList[colNum], reader.GetString(colNum).ToString());
                }

                theRows.Add(row);

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
