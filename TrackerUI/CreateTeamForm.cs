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
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> avalilabeTeamMembers = new List<PersonModel>();
        private List<PersonModel> selectedTeamMebers = new List<PersonModel>();

        public CreateTeamForm()
        {
            InitializeComponent();
            createSampleData();
            wireUpList();
        }
        private void createSampleData()
        {
            avalilabeTeamMembers.Add(new PersonModel { FirstName = "Md somrat", LastName = "akbor" });
            avalilabeTeamMembers.Add(new PersonModel { FirstName = "Md Moazzam", LastName = "hossain" });

            selectedTeamMebers.Add(new PersonModel { FirstName = "Md somrats", LastName = "akbors" });
            selectedTeamMebers.Add(new PersonModel { FirstName = "Md Moazzams", LastName = "hossains" });
        }

        private void wireUpList()
        {
            selectTeamMemberDropDown.DataSource = avalilabeTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";
            teamMemberListBox.DataSource = selectedTeamMebers;
            teamMemberListBox.DisplayMember = "FullName";

        }
        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PersonModel model = new PersonModel();
                model.FirstName = firstNameValue.Text;
                model.LastName = lastNameValue.Text;
                model.EmailAddress = emailValue.Text;
                model.CellPhoneNumber = cellPhoneValue.Text;
                GlobalConfig.Connection.CreatePerson(model);
                firstNameValue.Text = string.Empty;
                lastNameValue.Text = string.Empty;
                emailValue.Text = string.Empty;
                cellPhoneValue.Text = string.Empty;



            }
            else
            {
                MessageBox.Show("you need all fill all fields.");
            }
        }
        private bool ValidateForm()
        {
            if (firstNameValue.Text.Length == 0)
            {
                return false;
            }
            if(lastNameValue.Text.Length == 0)
            {
                return false;
            }
            if(emailValue.Text.Length == 0)
            {
                return false;
            }
            if(cellPhoneValue.Text.Length == 0)
            {
                return false;
            }
            return true;
        }
    }
}
