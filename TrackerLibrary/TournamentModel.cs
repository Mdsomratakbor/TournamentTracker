using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    /// <summary>
    /// Represent a Tournament.
    /// </summary>
    public class TournamentModel
    {
        /// <summary>
        /// Represent a The Tournament name.
        /// </summary>
        public string TournamentName { get; set; }
        /// <summary>
        ///  The Tournament Entry Free.
        /// </summary>
        public decimal EntryFree { get; set; }
        /// <summary>
        ///  The Tournament total teams.
        /// </summary>
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();
        /// <summary>
        ///  The Tournament all teams position prize money.
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();
        /// <summary>
        ///  The Tournament total rounds.
        /// </summary>
        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();
    }
}
