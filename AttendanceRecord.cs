﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCGI_Attendance_System
{
    public partial class AttendanceRecord : Form
    {
        public AttendanceRecord()
        {
            InitializeComponent();
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Replace with your actual column name or index
            if (dataGridView.Columns[e.ColumnIndex].Name == "date")
            {
                if (e.Value != null)
                {
                    if (DateTime.TryParse(e.Value.ToString(), out DateTime dt))
                    {
                        e.Value = dt.ToString("MM-dd-yyyy");
                        e.FormattingApplied = true;
                    }
                }
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            string from = dtpFrom.Value.ToString("yyyy-MM-dd");
            string to = dtpTo.Value.ToString("yyyy-MM-dd");

            CRUD crud = new CRUD();
            DataTable filteredTable = crud.DisplayFilteredData(from, to);
            dataGridView.DataSource = filteredTable;

            if (filteredTable.Rows.Count == 0)
            {
                MessageBox.Show("No records found for the selected date range.");
                DisplayAllRecords();
                return;
            }

            dataGridView.Update();
            dataGridView.Refresh();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Login form = new Login();
            form.Show();
            form.LoginSuccess();
            this.Hide();
        }

        private void AttendanceRecord_Load(object sender, EventArgs e)
        {
            DisplayAllRecords();
        }

        public void DisplayAllRecords()
        {
            CRUD crud = new CRUD();
            dataGridView.CellFormatting += dataGridView_CellFormatting;
            dataGridView.DataSource = crud.DisplayAllData();
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.Columns["id"].HeaderText = "Member ID";
            dataGridView.Columns["name"].HeaderText = "Full Name";
            dataGridView.Columns["date"].HeaderText = "Attendance Date";
            dataGridView.Columns["typeofservice"].HeaderText = "Type of Service";
            dataGridView.Columns["batch"].HeaderText = "Time Batch";
            dataGridView.Columns["locale"].HeaderText = "Locale";
            dataGridView.Columns["churchid"].HeaderText = "Church ID";

            dataGridView.EnableHeadersVisualStyles = false; // Important to allow custom styling
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(11, 62, 131);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dtpFrom.Format = DateTimePickerFormat.Custom;
            dtpFrom.CustomFormat = "dd MMMM yyyy";
            dtpFrom.Value = DateTime.Today;

            dtpTo.Format = DateTimePickerFormat.Custom;
            dtpTo.CustomFormat = "dd MMMM yyyy";
            dtpTo.Value = DateTime.Today;
        }

        private void SearchRecords()
        {
            CRUD crud = new CRUD();
            string searchTerm = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term.");
                DisplayAllRecords();
                return;
            }

            DataTable filteredTable = crud.SearchData(searchTerm);
            dataGridView.DataSource = filteredTable;
            dataGridView.Update();
            dataGridView.Refresh();

            if (filteredTable.Rows.Count == 0)
            {
                DisplayAllRecords();
                MessageBox.Show("No records found.");
                return;
            }
        }

        private void AttendanceRecord_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DisplayAllRecords();
        }

        private void btnRegData_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterMember registerForm = new RegisterMember();
            registerForm.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchRecords();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchRecords();
            }
        }

        private void btnEditData_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView.SelectedRows[0];
                Console.WriteLine(row.Cells["id"].Value);
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }
    }
}