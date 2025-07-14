using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.Entity.Core.Metadata.Edm;
using System.Management.Instrumentation;

namespace MCGI_Attendance_System
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            CRUD crud = new CRUD();
            crud.TestConnection();
            this.KeyPreview = true;
            pnlDashboard.Visible = false;
            pnlAttendanceforOtherLocale.Visible = false;
            dateTimePicker.Format = DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = "dd MMMM yyyy";
            dateTimePicker.Value = DateTime.Today;
        }

        public void LoginSuccess()
        {
            tblPanelLayout.Controls.Remove(pnlLogin);
            tblPanelLayout.Controls.Add(pnlDashboard);
            pnlDashboard.Visible = true;
        }

        public void login()
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            CRUD crud = new CRUD();

            if (crud.SearchAdmin(username) != "Nothing found")
            {
                if (crud.SearchAdmin(username) == password)
                {
                    Console.WriteLine("Login Success!");
                    LoginSuccess();
                }
                else
                {
                    MessageBox.Show("Invalid username of password!");
                    txtPassword.Text = "";
                    txtUsername.Text = "";
                }
            }
        }

        public void ClearData()
        {
            string date = dateTimePicker.Value.ToString("MM-dd-yy");
            txtFullname.Text = "";
            cmbTypeOfService.SelectedIndex = -1;
            cmbBatch.SelectedIndex = -1;
            txtLocale.Text = "";
            txtChurchID.Text = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        private void btnAttendanceforOtherLocale_Click(object sender, EventArgs e)
        {
            tblPanelLayout.Controls.Remove(pnlDashboard);
            tblPanelLayout.Controls.Add(pnlAttendanceforOtherLocale);
            pnlAttendanceforOtherLocale.Visible = true;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            tblPanelLayout.Controls.Remove(pnlAttendanceforOtherLocale);
            tblPanelLayout.Controls.Add(pnlDashboard);
            pnlDashboard.Visible = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string date = dateTimePicker.Value.ToString("yyyy-MM-dd");
            string fullname = txtFullname.Text;
            string typeofService = cmbTypeOfService.Text;
            string batch = cmbBatch.Text;
            string locale = txtLocale.Text;
            string churchID = txtChurchID.Text;

            if (fullname != "" && typeofService != "" && batch != "" && locale != "" && churchID != "")
            {
                
                DialogResult result = MessageBox.Show("Are you sure to save this information?", "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    CRUD crud = new CRUD();
                    crud.InsertData(fullname, date, typeofService, batch, locale, churchID);
                    ClearData();

                    MessageBox.Show("Record saved successfully!");
                }
            }
            else
            {
                MessageBox.Show("Please input the complete details first!");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnViewSummary_Click(object sender, EventArgs e)
        {
            this.Hide();

            AttendanceRecord record = new AttendanceRecord();
            record.Show();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnAttendanceRecord_Click(object sender, EventArgs e)
        {

        }
    }
}