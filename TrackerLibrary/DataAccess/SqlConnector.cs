﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class SqlConnector : IDataConnection
    {
        private const string db = "Tournaments";
        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAddres", model.EmailAddress);
                p.Add("@CellPhoneNumber", model.CellPhoneNumber);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spPerson_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
                return model;
            }
        }

        public PrizeModel CreatePrize(PrizeModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32,direction:ParameterDirection.Output);
                connection.Execute("dbo.spPrizes_Insert", p, commandType:CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
                return model;
            }
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@TeamName", model.TeamName);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spTeams_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
                foreach(PersonModel team in model.TeamMembers)
                {
                     p = new DynamicParameters();
                    p.Add("@TeamId", model.Id);
                    p.Add("@PersonId", team.Id);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                    connection.Execute("dbo.spTeamsMember_Insert", p, commandType: CommandType.StoredProcedure);
                }
                return model;
            }
            
        }

        public TournamentModel CreateTournament(TournamentModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@TournamentName", model.TournamentName);
                p.Add("@EntryFree", model.EntryFree);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spTournament_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
                foreach (PrizeModel prize in model.Prizes)
                {
                    p = new DynamicParameters();
                    p.Add("@TournamentId", model.Id);
                    p.Add("@PrizeId", prize.Id);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                    connection.Execute("dbo.spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);
                }
                foreach (TeamModel team in model.EnteredTeams)
                {
                    p = new DynamicParameters();
                    p.Add("@TournamentId", model.Id);
                    p.Add("@TeamId", team.Id);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                    connection.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);
                }
                return model;
            }
        }

        public List<PersonModel> GetPersonAll()
        {
            List<PersonModel> output = new List<PersonModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(db)))
            {
               
                
                output = connection.Query<PersonModel>("dbo.spPersonGetAll").ToList();
            }
            return output;
        }

        public List<TeamModel> GetTeamAll()
        {
            List<TeamModel> output = new List<TeamModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(db)))
            {
                output = connection.Query<TeamModel>("dbo.spTeamGetAll").ToList();
                foreach(TeamModel team in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@TeamId", team.Id);
                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMemberGetByTeam",p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            return output;
        }
    }
}
