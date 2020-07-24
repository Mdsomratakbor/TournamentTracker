using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizeTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = FindNumberOfRounds(randomizeTeams.Count);
            int byes = NumberOfByes(rounds, randomizeTeams.Count);
            model.Rounds.Add(CreateFirstRounds(byes, randomizeTeams));
            CreateOthersRounds(model, rounds);
        }
        private static void CreateOthersRounds(TournamentModel model, int rounds)
        {
            int round = 2;
            List<MatchupModel> previousRound = model.Rounds[0]; 
        }
        private static List<MatchupModel> CreateFirstRounds(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel current = new MatchupModel();
            foreach (TeamModel team in teams)
            {
                current.Entries.Add(new MatchupEntryModel { TeamCompeting = team});
                if(byes<0 || current.Entries.Count > 1)
                {
                    current.MatchupRound = 1;
                    output.Add(current);
                    current = new MatchupModel();
                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }
            }
            return output;
        }
        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            int output = 0;
            int totalTeams = 1;
            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }
            output = totalTeams - numberOfTeams;
            return output;
        }
        private static int FindNumberOfRounds(int teamCount)
        {
            int output = 1;
            int val = 2;
            while (val < teamCount)
            {
                output += 1;
                val *= 2;
            }
            return output;
        }
        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}
