using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    /// <summary>
    /// Represent one Team a Tournament.
    /// </summary>
    public class TeamModel
    {
        /// <summary>
        /// unique id for every team
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represent List of persong a team.
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();
        /// <summary>
        /// The Tournament of this team name.
        /// </summary>
        public string TeamName { get; set; }

    }
}
