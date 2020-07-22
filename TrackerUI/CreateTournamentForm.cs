using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form
    {
        List<TeamModel> availabeTeams = GlobalConfig.Connection.GetTeamAll();
        public CreateTournamentForm()
        {
            InitializeComponent();
            Initializelists();
        }
        private void Initializelists()
        {
            selectTeamDropDown.DataSource = availabeTeams;
            selectTeamDropDown.DisplayMember = "TeamName";
        }

    }
}
