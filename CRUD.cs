using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MCGI_Attendance_System
{
    internal class CRUD
    {
        private string connectionString;

        public CRUD()
        {
            // Build path relative to the project root
            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string dbPath = Path.Combine(projectRoot, "Files", "AttendanceDB.db");

            if (!File.Exists(dbPath))
            {
                Console.WriteLine("Database file NOT FOUND at: " + dbPath);
            }
            else
            {
                Console.WriteLine("Database file FOUND at: " + dbPath);
            }

            // Get database path
            connectionString = $"Data Source={dbPath}";
        }

        public void TestConnection()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("SQLite connection successful.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
            }
        }

        public string SearchAdmin(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT password FROM admins WHERE username = @username";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string pass = reader.GetString(0);
                            return pass;
                        }
                        else
                        {
                            return "Nothing found";
                        }
                    }
                }
            }
        }

        public void InsertData(string fullname, string date, string typeofService, string batch, string locale, string churchID)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO MemberAttendance (name, date, typeofservice, batch, locale, churchid) " +
                    "VALUES (@name, @date, @typeofservice, @batch, @locale, @churchid)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", fullname);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@typeofservice", typeofService);
                    command.Parameters.AddWithValue("@batch", batch);
                    command.Parameters.AddWithValue("@locale", locale);
                    command.Parameters.AddWithValue("@churchid", churchID);

                    int result = command.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? "Insert successful." : "Insert failed.");
                }
            }
        }

        public DataTable DisplayAllData() //return data table
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM MemberAttendance";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table); // Fills DataTable with query result
                        return table;
                    }
                }
            }
        }

        public DataTable DisplayFilteredData(string from, string to) //return data table
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM MemberAttendance WHERE date BETWEEN @from AND @to";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@from", from);
                    command.Parameters.AddWithValue("@to", to);

                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table); // Fills DataTable with query result
                        return table;
                    }
                }
            }
        }

        public void InsertDataReg(string id, string fullname, string dateOfBirth, string dateOfBaptism, string churchID, string churchStatus, string imagePath)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO MemberInformation (memberID, fullName, dateOfBirth, dateOfBaptism, churchID, churchStatus, ImagePath) " +
                    "VALUES (@memberID, @fullName, @dateOfBirth, @dateOfBaptism, @churchID, @churchStatus, @imagePath)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@memberID", id);
                    command.Parameters.AddWithValue("@fullName", fullname);
                    command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                    command.Parameters.AddWithValue("@dateOfBaptism", dateOfBaptism);
                    command.Parameters.AddWithValue("@churchID", churchID);
                    command.Parameters.AddWithValue("@churchStatus", churchStatus);
                    command.Parameters.AddWithValue("@imagePath", imagePath);

                    int result = command.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? "Insert successful." : "Insert failed.");
                }
            }
        }
    }
}