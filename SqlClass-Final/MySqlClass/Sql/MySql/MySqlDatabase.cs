using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using nTools.SqlTools.Interfaces;
using nTools.SqlTools.Common;

namespace nTools.SqlTools.Databases
{
    public class MySqlDatabase : IDatabase, IDisposable
    {
		#region Fields (7) 

        private Dictionary<string, string> _commands;
        private Dictionary<string, IDbConnection> _connections;
        private List<string> _keys;
        private string _latestKey;
        private Dictionary<string, string> _queries;
        private Dictionary<string, IResultSet> _resultSets;
        private Dictionary<string, StringBuilder> _scripts;

		#endregion Fields 

		#region Properties (6) 

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Commands { get { return _commands; } }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, IDbConnection> Connections { get { return _connections; } }

        /// <summary>
        /// 
        /// </summary>
        public IList<string> Keys { get { return _keys.AsReadOnly(); } }

        /// <summary>
        /// 
        /// </summary>
        public IResultSet LatestResultSet { get { return _latestKey != null && _keys.Contains(_latestKey) ? _resultSets[_latestKey] : null; } }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Queries { get { return _queries; } }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, IResultSet> ResultSets { get { return _resultSets; } }

		#endregion Properties 

		#region Constructors (2) 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connKey"></param>
        /// <param name="connectionString"></param>
        public MySqlDatabase(string connKey, string connStr) : this()
        {
            Connect(connKey, connStr);
        }

        /// <summary>
        /// 
        /// </summary>
        public MySqlDatabase()
        {

        }

		#endregion Constructors 

		#region Methods (4) 

		#region Public Methods (4) 

        public bool Connect(string connKey, string connStr)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connKey"></param>
        /// <param name="connArgs"></param>
        /// <returns></returns>
        public bool Connect(string connKey, DBPair[] connArgs)
        {
            StringBuilder sb = new StringBuilder();

            for (int x = 0; x < connArgs.Length; x++)
            {
                sb.Append(connArgs[x].ToString());
            }

            return Connect(connKey, sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connArgs"></param>
        /// <returns></returns>
        public bool Connect(params DBPair[] connArgs)
        {
            return Connect("Default", connArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Connect(string connStr)
        {
            return Connect("Default", connStr);
        }

		#endregion Public Methods 

		#endregion Methods 


    }
}
