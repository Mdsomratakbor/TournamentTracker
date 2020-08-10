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
    public partial class TournamentDashboardForm : Form
    {
        List<TournamentModel> tournamentmodels = GlobalConfig.Connection.GetTounamentAll();
        public TournamentDashboardForm()
        {
            InitializeComponent();
        }

        private void headerLabel_Click(object sender, EventArgs e)
        {

        }

        private void TournamentDashboardForm_Load(object sender, EventArgs e)
        {

        }
    }
}
