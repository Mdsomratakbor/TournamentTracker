﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public interface IDataConnection
    {
        PrizeModel CreatePrize(PrizeModel model);
        PersonModel CreatePerson(PersonModel model);
        List<PersonModel> GetPersonAll();
        void CreateTournament(TournamentModel model);
        List<TeamModel> GetTeamAll();
        TeamModel CreateTeam(TeamModel model);
        List<TournamentModel> GetTounamentAll();
    }
}
