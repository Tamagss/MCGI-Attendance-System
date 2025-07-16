using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCGI_Attendance_System
{
    public partial class RegisterMember : Form
    {
        private string imageFilePath;

        public RegisterMember()
        {
            InitializeComponent();
        }

        private void RegisterMember_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AttendanceRecord form = new AttendanceRecord();
            form.Show();
            this.Hide();
        }

        private void btnUpldoadPic_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFolder = new OpenFileDialog();
            openFolder.Title = "Upload Member Image";
            openFolder.InitialDirectory = "C:\\";
            openFolder.Filter = "Image Files (*jpg)|*.jpg|All Files(*.*)|*.*";
            openFolder.FilterIndex = 1;

            if (openFolder.ShowDialog() == DialogResult.OK)
            {
                pictBoxReg.Image = new Bitmap(openFolder.FileName);
                Console.WriteLine(openFolder.FileName);
                imageFilePath = openFolder.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string id = txtMemberID.Text;
            string fullName = txtFullName.Text;
            string dateOfBirth = dtpDateofBirth.Value.ToString("yyyy-MM-dd");
            string dateOfBaptism = dtpDateOfBaptism.Value.ToString("yyyy-MM-dd");
            string churchID = txtChuchID.Text;
            string churchStatus = txtChurchStatus.Text;
            string fullPath = "";
            string newFileName = "";

            string dateToday = DateTime.Today.ToString("yyyy-MM-dd");

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(churchID) || string.IsNullOrWhiteSpace(churchStatus) || dateOfBirth == dateToday || dateOfBaptism == dateToday)
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            if (File.Exists(imageFilePath))
            {
                string exePath = AppDomain.CurrentDomain.BaseDirectory;
                string projectRoot = Path.GetFullPath(Path.Combine(exePath, @"..\..\"));

                //Create or ensure the "Member Images" folder exists
                string imagesFolder = Path.Combine(projectRoot, "Files", "Member Images");

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                Console.WriteLine(imagesFolder);

                //Build a unique filename
                string ext = Path.GetExtension(imageFilePath); // e.g. ".jpg"

                newFileName = $"{fullName}{ext}";

                fullPath = Path.Combine(imagesFolder, newFileName);

                try
                {
                    System.IO.File.Copy(imageFilePath, fullPath);
                }
                catch (IOException ex)
                {
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                        System.IO.File.Copy(imageFilePath, fullPath);
                    }
                }
            }

            CRUD insert = new CRUD();

            DialogResult result = MessageBox.Show("Are you sure to save this information?", "Confirmation", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                insert.InsertDataReg(id, fullName, dateOfBirth, dateOfBaptism, churchID, churchStatus, newFileName);
                ClearData();

                MessageBox.Show("Record saved successfully!");
            }
        }

            public void ClearData()
            {
                txtMemberID.Clear();
                txtFullName.Clear();    
                dtpDateofBirth.Value = DateTime.Now;
                dtpDateOfBaptism.Value = DateTime.Now;
                txtChuchID.Clear();
                txtChurchStatus.Clear();
                pictBoxReg.Image = null;
                imageFilePath = string.Empty; // Reset the image file path
            }
        }
    }