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
    public partial class TournamentViewForm : Form
    {
        private TournamentModel tournament;
        public TournamentViewForm(TournamentModel tornamentModel)
        {
            InitializeComponent();
            tournament = tornamentModel;
            LoadFormData();
        }
        private void LoadFormData()
        {
            tournamentName.Text = tournament.TournamentName;
        }

    }
}
