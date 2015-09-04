/*******************************
 * SqlTools Namespace v1.0
 *      nathan vanbuskirk
 *      july 10, 2007
 * 
 * contains MySqlClass && SqlDataClass
 * also contains an enum dType{String,Integer,Double,Bool}
 *******************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Sockets;

#pragma warning disable 1591

namespace nTools.SqlTools
{
    /// <summary>
    /// data type
    /// </summary>
    /// <type>enum</type>
    /// <value>String</value>
    /// <value>Integer</value>
    /// <value>Bool</value>
    /// <value>Double</value>
    [Flags] public enum dType{String, Integer, Bool, Double, Decimal, DateTime, }

    /// <summary>
    /// database mode
    /// </summary>
    /// <type>enum</type>
    /// <value>fastest</value>
    /// <value>medium</value>
    /// <value>full</value>
    public enum dMode { fastest, medium, full, }

    /// <summary>
    /// database type
    /// </summary>
    /// <type>enum</type>
    /// <value>None</value>
    /// <value>MySql</value>
    /// <value>MsSql</value>
    public enum dbType { None = 0, MySql = 1, MsSql = 2, }

    /// <summary>
    /// sql utilities
    /// </summary>
    /// <stringArray>static private string[] escString</stringArray>
    /// <stringArray>static private string[] repString</stringArray>
    /// <functions>
    /// static public string mysqlClean(string userDataString)
    /// static public string mysqlStrip(string dbDataString)
    /// static public string mysqlStrip(SqlDataClass dbDataValue)
    /// </functions>
    static public class sqlUtil
    {
        #region fields
        static private string[] escString = new string[] 
                                    {
                                        @"\",
                                        "/",
                                        "@",
                                        "#",
                                        "%",
                                        "&",
                                        "*",
                                        "(",
                                        ")",
                                        "'",
                                        "\"",
                                    };

        static private string[] repString = new string[]
                                    {
                                        @"\\",
                                        @"\/",
                                        @"\@",
                                        @"\#",
                                        @"\%",
                                        @"\&",
                                        @"\*",
                                        @"\(",
                                        @"\)",
                                        @"\'",
                                        "\\\"",
                                    };
        #endregion

        #region strip/clean
        /// <summary>
        /// adds the slashes to the correct characters in your string for adding to the db
        /// </summary>
        /// <param name="userDataString" type="string"></param>
        /// <returns type="string"></returns>
        static public string mysqlClean(string userDataString)
        {
            for (int x = 0; x < repString.Length; x++)
            {
                userDataString = userDataString.Replace(escString[x], repString[x]);
            }

            return userDataString;
        }
        
        /// <summary>
        /// strips the slashes from a string pulled out of the database
        /// </summary>
        /// <param name="dbDataString" type="string"></param>
        /// <returns type="string"></returns>
        static public string mysqlStrip(string dbDataString)
        {
            for(int x=0; x<repString.Length; x++)
            {
                dbDataString = dbDataString.Replace( repString[x], escString[x]);
            }

            return dbDataString;
        }

        /// <summary>
        /// returns the string stripped of all slashes
        /// </summary>
        /// <param name="dbDataValue" type="SqlTools.SqlDataClass"></param>
        /// <returns type="string"></returns>
        static public string mysqlStrip(SqlDataClass dbDataValue)
        {
            if (dbDataValue != null)
            {
                return mysqlStrip(dbDataValue.getString());
            }
            else
            {
                return "no string to strip";
            }
        }
        #endregion

        #region tcp
        /// <summary>
        /// attempts to ping the server of an existing MySqlClass
        /// </summary>
        /// <param name="ipAndPort"></param>
        /// <returns></returns>
        static public bool ping(string[] ipAndPort)
        {
            return ping(ipAndPort[0], Convert.ToInt32(ipAndPort[1]));
        }

        /// <summary>
        /// attempts to ping the ip/port of your sql server
        /// </summary>
        /// <param name="ip" type="string"></param>
        /// <param name="port" type="int"></param>
        /// <returns type="bool"></returns>
        static public bool ping(string ip, int port)
        {
            TcpClient pinger = new TcpClient();

            try
            {
                pinger = new TcpClient(ip, port);

                

                if (pinger.Connected)
                {
                    pinger.Close();
                    return true;
                }
                else
                {
                    Console.WriteLine("not connected???");
                    pinger.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                try
                {
                    if (pinger != null)
                    {
                        pinger.Close();
                    }
                }
                catch 
                {
                    Console.WriteLine(e.Message);
                }

                return false;

            }

            return false;

        }
        #endregion

        static public void refreshSrc(ref SqlBindingSource sqlBindingSrc)
        {
            dbType databaseType = sqlBindingSrc.DatabaseType;
            sqlBindingSrc.DatabaseType = dbType.None;
            sqlBindingSrc.DatabaseType = databaseType;
        }

    }//end static class sqlUtil

}//end of SqlTools
