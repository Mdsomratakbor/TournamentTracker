﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public const string PrizesFile = "PrizeModel.csv";
        public const string PersonFile = "PersonModel.csv";
        public const string TeamFile = "TeamsModel.csv";
        public const string TournamentFile = "TournamentModels.csv";
        public const string MatchupFile = "MatchupModel.csv";
        public const string MatchupEntryFile = "MatchupEntryModel.csv";
        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnections(DatabaseType db)
        {
            if (db==DatabaseType.Sql)
            {
                // do somthing 
                SqlConnector sql = new SqlConnector();
                Connection = sql;
            }
            else if (db==DatabaseType.TextFile)
            {
                //do somthing
                TextConnector text = new TextConnector();
                Connection = text;
            }
        }

        public static string ConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
