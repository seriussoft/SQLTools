using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using nTools.SqlTools.Common;

namespace nTools.SqlTools
{

    public enum ConnectionProtocol
    {
        socket = 1,
        tcp = 2,
        pipe = 4,
        unix = 8,
        memory = 16,
    }

    public static partial class MsConn
    {
        public static DBPair User(string user) { return new DBPair("User ID", user); }

        public static DBPair Db(string db) { return new DBPair("Database", db); }

        public static DBPair Server(string server) { return new DBPair("Server", server); }

        public static DBPair Pass(string pass) { return new DBPair("Password", pass); }

        public static DBPair Trusted(bool trusted) { return new DBPair("Trusted_Connection", trusted.ToString()); }

        public static DBPair ServerAndInstance(string server, string instance) { return new DBPair("Server", string.Format(@"{0}\{1}", server, instance)); }
    }

    public static partial class MyConn
    {
        public static DBPair User(string user) { return new DBPair("Uid", user); }

        public static DBPair Db(string db) { return new DBPair("Database", db); }

        public static DBPair Server(string server) { return new DBPair("Server", server); }

        public static DBPair Pass(string pass) { return new DBPair("Pwd", pass); }

        public static DBPair Port(string port) { return new DBPair("Port", port); }

        public static DBPair Encryption(bool encrypt) { return new DBPair("Encryption", encrypt.ToString()); }

        public static DBPair DefaultCmdTimeout(int seconds) { return new DBPair("default command timeout", seconds.ToString()); }

        public static DBPair DefaultConnTimeout(int seconds) { return new DBPair("Connection Timeout", seconds.ToString()); }

        public static DBPair Protocol(ConnectionProtocol protocol) { return new DBPair("Protocol", protocol.ToString()); }
    }

    public static partial class ODBConn
    {
        public static DBPair Driver(string driver) { return new DBPair("Driver", string.Format("{{0}}", driver)); }

        public static DBPair Server(string server) { return new DBPair("Server", server); }

        public static DBPair Db(string db) { return new DBPair("Database", db); }

        public static DBPair User(string user) { return new DBPair("User", user); }

        public static DBPair Pass(string pass) { return new DBPair("Password", pass); }

        public static DBPair Option(int option) { return new DBPair("Option", option.ToString()); } 
    }
}
