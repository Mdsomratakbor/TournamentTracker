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
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes = new List<PrizeModel>();
        public CreateTournamentForm()
        {
            InitializeComponent();
            Initializelists();
        }
        private void Initializelists()
        {
            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = availabeTeams;
            selectTeamDropDown.DisplayMember = "TeamName";
            tournamentTeamListBox.DataSource = null;
            tournamentTeamListBox.DataSource = selectedTeams;
            tournamentTeamListBox.DisplayMember = "TeamName";
            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel team = (TeamModel) selectTeamDropDown.SelectedItem;
            if(team!= null)
            {
                availabeTeams.Remove(team);
                selectedTeams.Add(team);
                Initializelists();
            }
        }
    }
}
