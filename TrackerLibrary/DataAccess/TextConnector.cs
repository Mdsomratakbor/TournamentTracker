using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModel.csv";
        private const string PersonFile = "PersonModel.csv";
        private const string TeamFile = "TeamsModel.csv";

        public PersonModel CreatePerson(PersonModel model)
        {
            List<PersonModel> persons = PersonFile.FullFilePath().LoadFile().ConvertPersonModels();
            int currentId = 1;
            if (persons.Count > 0)
            {
                currentId = persons.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            persons.Add(model);
            persons.SaveToPersonFile(PersonFile);
            return model;
        }

        public PrizeModel CreatePrize(PrizeModel model)
        {
           List<PrizeModel> prizes= PrizesFile.FullFilePath().LoadFile().ConvertPrizeModels();
            int currentId = 1;
            if (prizes.Count > 0)
            {
                 currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
          
            model.Id = currentId;
            prizes.Add(model);
            prizes.SaveToPrizeFile(PrizesFile);
            return model;
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            List<TeamModel> Teams = TeamFile.FullFilePath().LoadFile().ConvertTeamModels(PersonFile);
            int currentId = 1;
            if (Teams.Count > 0)
            {
                currentId = Teams.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;
            Teams.Add(model);
            Teams.SaveToTeamFile(TeamFile);
            return model;
        }

        public TournamentModel CreateTournament(TournamentModel model)
        {
            throw new NotImplementedException();
        }

        public List<PersonModel> GetPersonAll()
        {
            return PersonFile.FullFilePath().LoadFile().ConvertPersonModels();
        }

        public List<TeamModel> GetTeamAll()
        {
            return TeamFile.FullFilePath().LoadFile().ConvertTeamModels(PersonFile);
        }
    }
}
