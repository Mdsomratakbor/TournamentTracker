using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.DataAccess
{
    public static class TextConnectorProcess
    {
        public static string FullFilePath(this string fileName)
        {          
            return $"{ConfigurationManager.AppSettings["filePath"]}\\{fileName}";
        }
        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }
            return File.ReadAllLines(file).ToList();
        }
        public static List<PrizeModel> ConvertPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();
            foreach(string line in lines)
            {
                string[] cols = line.Split(',');
                PrizeModel p = new PrizeModel();
                p.Id =int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);
                output.Add(p);
            }
            return output;
        }

        public static List<PersonModel> ConvertPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                PersonModel p = new PersonModel();
                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAddress = cols[3];
                p.CellPhoneNumber = cols[4];
                output.Add(p);
            }
            return output;
        }

        public static List<TeamModel> ConvertTeamModels(this List<string> lines, string pernosFileName)
        {
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> person = pernosFileName.FullFilePath().LoadFile().ConvertPersonModels();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                TeamModel t = new TeamModel();
                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];
                string[] personIds = cols[2].Split('|');
                foreach (string id in personIds)
                {
                    t.TeamMembers.Add(person.Where(x => x.Id == int.Parse(id)).FirstOrDefault());
                }
                output.Add(t);
            }
            return output;
        }

        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (PrizeModel p in models)
            {
                lines.Add($"{p.Id},{p.PlaceNumber},{p.PlaceName},{p.PrizeAmount},{p.PrizePercentage}");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
        public static void SaveToPersonFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (PersonModel p in models)
            {
                lines.Add($"{p.Id},{p.FirstName},{p.LastName},{p.EmailAddress},{p.CellPhoneNumber}");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
        public static void SaveToTeamFile(this List<TeamModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (TeamModel t in models)
            {
                lines.Add($"{t.Id},{t.TeamName},{ConvertPersonListString(t.TeamMembers)}");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
        public static void SaveToTournamentFile(this List<TournamentModel> models,string fileName)
        {
            List<string> lines = new List<string>();
            foreach (TournamentModel tournament  in models)
            {
                lines.Add($"{tournament.Id},{tournament.TournamentName},{tournament.EntryFree},{ConvertTeamListToString(tournament.EnteredTeams)},{""},{ConvertPrizeListToString(tournament.Prizes)},{ConvertRoundListString(tournament.Rounds)}");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
        public static void SaveRoundsToFile( this TournamentModel model, string matchupFile, string matchupEntryFile)
        {
            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    matchup.SaveMatchupToFile(matchupFile, matchupEntryFile);
                    
                }
            }
        }
        public static void SaveMatchupToFile(this MatchupModel matchup, string matchupFile, string matchupEntryFile)
        {        
            List<MatchupModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();

            int currentId = 1;
            if (matchups.Count > 0)
            {
                currentId = matchups.OrderByDescending(x=>x.Id).First().Id+1; 
            }
            matchup.Id = currentId;
            matchups.Add(matchup);
            foreach (MatchupEntryModel entry in matchup.Entries)
            {
                entry.SaveEntryToFile(matchupEntryFile);
            }
            List<string> lines = new List<string>();
            foreach (MatchupModel m in matchups)
            {
                string winner = string.Empty;
                if(m.Winner != null)
                {
                    winner = m.Winner.Id.ToString();
                }
    lines.Add($"{m.Id},{ConvertMatchupEntryListToString(m.Entries)},{winner},{m.MatchupRound}");
            }
            File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
        }
        public static void SaveEntryToFile(this MatchupEntryModel entry, string matchupEntryFile)
        {
            List<MatchupEntryModel> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntryModels();
            int currentId = 1;
            if (entries.Count > 0)
            {
                currentId = entries.OrderByDescending(x => x.Id).First().Id + 1;
            }
            entry.Id = currentId;
            entries.Add(entry);
            List<string> lines= new List<string>();
            foreach (MatchupEntryModel e in entries)
            {
                string parent = string.Empty;
                if(e.ParentMatchup != null)
                {
                    parent = e.ParentMatchup.Id.ToString();
                }
                lines.Add($"{e.Id},{e.TeamCompeting.Id},{e.Score},{parent}");
            }
           File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
        }

        private static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = string.Empty;
            if (teams.Count == 0)
            { 
                return string.Empty;
            }
            foreach (TeamModel team in teams)
            {
                output += $"{team.Id}|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }
        public static string ConvertPrizeListToString(List<PrizeModel> prizes)
        {
            string output = string.Empty;
            if (prizes.Count == 0)
            {
                return string.Empty;
            }
            foreach (PrizeModel prize in prizes)
            {
                output += $"{prize.Id}|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }
        private static string ConvertPersonListString(List<PersonModel> person)
        {
            string output = string.Empty;
            if (person.Count == 0)
            {
                return string.Empty;
            }
            foreach (PersonModel p in person)
            {
                output += $"{p.Id}|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }
        private static string ConvertRoundListString(List<List<MatchupModel>> rounds)
        {
            string output = string.Empty;
            if (rounds.Count == 0)
            {
                return string.Empty;
            }
            foreach (List<MatchupModel> r in rounds)
            {
                output = $"{ConvertMatchUpListToString(r)}|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }
        private static string ConvertMatchUpListToString(List<MatchupModel> matchups)
        {
            string output = string.Empty;
            if (matchups.Count == 0)
            {
                return string.Empty;
            }
            foreach (MatchupModel m in matchups)
            {
                output += $"{m.Id}^";
            }
            output = output.Substring(0,output.Length-1);
            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModels(this List<string> lines, string teamFileName, string personFileName, string prizeFileName)
        {
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamFileName.FullFilePath().LoadFile().ConvertTeamModels(personFileName);
            List<PrizeModel> prizes = prizeFileName.FullFilePath().LoadFile().ConvertPrizeModels();
            List<MatchupModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();
            foreach(string line in lines)
            {
                string[] cols = line.Split(',');
                TournamentModel tournament = new TournamentModel();
                tournament.Id = int.Parse(cols[0]);
                tournament.TournamentName = cols[1];
                tournament.EntryFree = decimal.Parse(cols[2]);
                string[] teamIds = cols[3].Split('|');
                foreach (string id in teamIds)
                {
                    tournament.EnteredTeams.Add(teams.Where(x=>x.Id==int.Parse(id)).First());
                }
                string[] prizeIds = cols[4].Split('|');
                foreach (string id in prizeIds)
                {
                    tournament.Prizes.Add(prizes.Where(x => x.Id == int.Parse(id)).First());
                }
                string[] rounds = cols[5].Split('|');
            
                foreach (string round in rounds)
                {
                    string[] msText = round.Split('^');
                    List<MatchupModel> ms = new List<MatchupModel>();
                    foreach (string matchupModelTextId in msText)
                    {
                        ms.Add(matchups.Where(x=>x.Id == int.Parse(matchupModelTextId)).First());
                    }
                    tournament.Rounds.Add(ms);
                }
              
                output.Add(tournament);
            }
            return output;
        }

        private static List<MatchupEntryModel> ConvertStringToMatchupEntryModels(string input)
        {
            string[] ids = input.Split('|');
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();
            List<MatchupEntryModel> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntryModels();
            foreach (string id in ids)
            {
                output.Add(entries.Where(x =>x.Id == int.Parse(id)).First());
            }
            return output;
        }

        public static List<MatchupEntryModel> ConvertToMatchupEntryModels(this List<string> lines)
        {
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                MatchupEntryModel matchupEntry= new MatchupEntryModel();
                matchupEntry.Id = int.Parse(cols[0]);
                matchupEntry.TeamCompeting = LookupTeamById(int.Parse(cols[1]));
                matchupEntry.Score = double.Parse(cols[2]);
                int parentId = 0;
                if(int.TryParse(cols[3], out parentId))
                {
                    matchupEntry.ParentMatchup = LookupMatchupById(int.Parse(cols[3]));
                }
                else
                {
                    matchupEntry.ParentMatchup = null; 
                }

                output.Add(matchupEntry);
            }
            return output;
        }
        private static MatchupModel LookupMatchupById(int id)
        {
            List<MatchupModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();
            return matchups.Where(x=>x.Id == id).First(); 
        }
        private static TeamModel LookupTeamById(int id)
        {
            List<TeamModel> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertTeamModels(GlobalConfig.PersonFile);
            return teams.Where(x=>x.Id == id).First();
        }
        public static List<MatchupModel> ConvertToMatchupModels(this List<string> lines)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                MatchupModel p = new MatchupModel();
                p.Id = int.Parse(cols[0]);
                p.Entries = ConvertStringToMatchupEntryModels(cols[1]) ;
                p.Winner = LookupTeamById(int.Parse(cols[2]));
                p.MatchupRound = int.Parse(cols[3]);
                output.Add(p);
            }
            return output;
        }

        private static string ConvertMatchupEntryListToString(List<MatchupEntryModel> entries) 
        {
            string output = string.Empty;
            if (entries.Count == 0)
            {
                return string.Empty;
            }
            foreach (MatchupEntryModel e in entries)
            {
                output += $"{e.Id }|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }
    }
}
