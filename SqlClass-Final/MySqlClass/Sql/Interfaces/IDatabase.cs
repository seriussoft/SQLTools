using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using nTools.SqlTools.Common;

namespace nTools.SqlTools.Interfaces
{
    public interface IDatabase : IDisposable
    {
        IList<string> Keys { get; }
        Dictionary<string, IResultSet> ResultSets { get; }
        Dictionary<string, IDbConnection> Connections { get; }
        Dictionary<string, string> Queries { get; }
        Dictionary<string, string> Commands { get; }
        IResultSet LatestResultSet { get; }

        IDatabase Use(string connKey);
        IDatabase As(string newConnKey);

        bool Connect(string connKey, string connStr);
        bool Connect(string connKey, params DBPair[] connArgs);

        bool Connect(string connStr);
        bool Connect(params DBPair[] connArgs);

        void Query(string connKey, string query);
        IResultSet QueryAndGet(string connKey, string query);
        void Query(string query);
        IResultSet QueryAndGet(string query);

        int Command(string connKey, string cmdStr);
        int Command(string cmdStr);

        int Run(string connKey, StringBuilder script);
        int Run(StringBuilder script);

        bool TryDisconnect(string connKey);
        bool TryDisconnectAll();
    }
}
