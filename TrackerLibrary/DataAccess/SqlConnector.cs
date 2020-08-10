using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);
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
                foreach (PersonModel team in model.TeamMembers)
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

        public void CreateTournament(TournamentModel model)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(db)))
                {
                    try
                    {
                        SaveTournament(connection, model);
                        SaveTournamentPrizes(connection, model);
                        SaveTournamentEntries(connection, model);
                        SaveTournamentRounds(connection, model);
                        scope.Complete();
                    }
                    catch (Exception e)
                    {

                        connection.BeginTransaction().Rollback();
                        throw e;
                    }

                }
            }
        }

        private void SaveTournament(IDbConnection connection, TournamentModel model)
        {
            var p = new DynamicParameters();
            p.Add("@TournamentName", model.TournamentName);
            p.Add("@EntryFree", model.EntryFree);
            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            connection.Execute("dbo.spTournament_Insert", p, commandType: CommandType.StoredProcedure);
            model.Id = p.Get<int>("@id");
        }
        private void SaveTournamentPrizes(IDbConnection connection, TournamentModel model)
        {
            foreach (PrizeModel prize in model.Prizes)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@PrizeId", prize.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }
        private void SaveTournamentEntries(IDbConnection connection, TournamentModel model)
        {
            foreach (TeamModel team in model.EnteredTeams)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@TeamId", team.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }
        private void SaveTournamentRounds(IDbConnection connection, TournamentModel model)
        {
            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    var p = new DynamicParameters();
                    p.Add("@TournamentId", model.Id);
                    p.Add("@MatchupRound", matchup.MatchupRound);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                    connection.Execute("dbo.spMatchups_Insert", p, commandType: CommandType.StoredProcedure);
                    matchup.Id = p.Get<int>("@id");
                    foreach (MatchupEntryModel entry in matchup.Entries)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId", matchup.Id);
                        if (entry.ParentMatchup == null)
                        {
                            p.Add("@ParentMatchupId", null);
                        }
                        else
                        {
                            p.Add("@ParentMatchupId", entry.ParentMatchup.Id);

                        }
                        if (entry.TeamCompeting == null)
                        {
                            p.Add("@TeamCompetingId", null);
                        }
                        else
                        {
                            p.Add("@TeamCompetingId", entry.TeamCompeting.Id);
                        }

                        p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                        connection.Execute("dbo.spMatchupEntries_Insert", p, commandType: CommandType.StoredProcedure);
                    }
                }
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
                foreach (TeamModel team in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@TeamId", team.Id);
                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMemberGetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            return output;
        }

        public List<TournamentModel> GetTounamentAll()
        {
            List<TournamentModel> output = new List<TournamentModel>();
            using (TransactionScope scope = new TransactionScope())
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString(db)))
                {
                    try
                    {
                        output = connection.Query<TournamentModel>("dbo.spTournament_GetAll").ToList();
                        var p = new DynamicParameters();
                        foreach (TournamentModel t in output)
                        {
                            t.Prizes = connection.Query<PrizeModel>("dbo.spPrizes_GetByTournament").ToList();
                            t.EnteredTeams = connection.Query<TeamModel>("dbo.spTeam_GetByTournament").ToList();
                            foreach (TeamModel team in t.EnteredTeams)
                            {
                                p = new DynamicParameters();
                                p.Add("@TeamId", team.Id);
                                team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMemberGetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                            }
                            p = new DynamicParameters();
                            p.Add("@TournamentId", t.Id);
                            List<MatchupModel> matchups = connection.Query<MatchupModel>("dbo.spMatchups_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();
                            foreach (MatchupModel m in matchups)
                            {
                                p = new DynamicParameters();
                                p.Add("@MatchupId", m.Id);
                                m.Entries = connection.Query<MatchupEntryModel>("dbo.spMatchupEntries_GetByMatchup", p, commandType: CommandType.StoredProcedure).ToList();
                                List<TeamModel> allTeams = GetTeamAll();
                                if (m.WinnerId > 0)
                                {
                                    m.Winner = allTeams.Where(x => x.Id == m.WinnerId).FirstOrDefault();
                                }
                                foreach (var me in m.Entries)
                                {
                                    if (me.TeamCompetingId > 0)
                                    {
                                        me.TeamCompeting = allTeams.Where(x => x.Id == me.TeamCompetingId).FirstOrDefault();
                                    }
                                    if (me.ParentMatchupId > 0)
                                    {
                                        me.ParentMatchup = matchups.Where(x => x.Id == me.ParentMatchupId).FirstOrDefault();
                                    }
                                }
                            }
                            List<MatchupModel> currRow = new List<MatchupModel>();
                            int currRound = 1;
                            foreach (MatchupModel m in matchups)
                            {
                                if (m.MatchupRound > currRound)
                                {
                                    t.Rounds.Add(currRow);
                                    currRow = new List<MatchupModel>();
                                    currRound += 1;
                                }
                                currRow.Add(m);
                            }
                            t.Rounds.Add(currRow);
                        }


                        scope.Complete();
                    }
                    catch (Exception e)
                    {
                        connection.BeginTransaction().Rollback();
                        throw e;
                    }
                }
            }
            return output;
        }
    }
}
