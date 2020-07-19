using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnection Connections { get; private set; }

        public static void InitializeConnections(string connectionType)
        {
            if (connectionType == "sql")
            {
                // do somthing 
                SqlConnector sql = new SqlConnector();
                Connections = sql;
            }
            else if (connectionType == "text")
            {
                //do somthing
                TextConnector text = new TextConnector();
                Connections = text;
            }
        }

        public static string ConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
