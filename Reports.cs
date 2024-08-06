using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace C969
{
    public class ReportGenerator
    {
        private string connectionString;

        public ReportGenerator(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Dictionary<int, List<UserReport>> GenerateUserScheduleReport()
        {
            Dictionary<int, List<UserReport>> report = new Dictionary<int, List<UserReport>>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT UserID, AppointmentID, CustomerID, Start, End FROM Appointment";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int userId = reader.GetInt32(0); // UserID at index 0
                        int appointmentID = reader.GetInt32(1); // AppointmentID at index 1
                        int customerID = reader.GetInt32(2); // CustomerID at index 2
                        DateTime start = reader.GetDateTime(3); // Start at index 3
                        DateTime end = reader.GetDateTime(4); // End at index 4

                        UserReport userReport = new UserReport(appointmentID, userId, customerID, start, end);

                        if (!report.ContainsKey(userId))
                        {
                            report[userId] = new List<UserReport>();
                        }

                        report[userId].Add(userReport);
                    }
                }
            }

            return report;
        }

        public Dictionary<string, Dictionary<string, int>> GenerateAppointmentTypesByMonthReport()
        {
            Dictionary<string, Dictionary<string, int>> report = new Dictionary<string, Dictionary<string, int>>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT Type, MONTH(Start) AS Month, COUNT(*) AS Count FROM Appointment GROUP BY Type, MONTH(Start)";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string type = reader.GetString("Type");
                        string month = reader.GetInt32("Month").ToString("00");
                        int count = reader.GetInt32("Count");

                        if (!report.ContainsKey(month))
                        {
                            report[month] = new Dictionary<string, int>();
                        }

                        report[month][type] = count;
                    }
                }
            }

            return report;
        }

        public Dictionary<int, List<Appointments>> GenerateUserScheduleReportByCustomerID()
        {
            Dictionary<int, List<Appointments>> report = new Dictionary<int, List<Appointments>>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = @"
            SELECT CustomerID, AppointmentID, Start, End
            FROM Appointment
            ORDER BY CustomerID, Start;
        ";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int appointmentID = reader.GetInt32(1);
                        int customerID = reader.GetInt32(0);
                        DateTime start = reader.GetDateTime(2);
                        DateTime end = reader.GetDateTime(3);

                        Appointments appointment = new Appointments(appointmentID, customerID, start, end);

                        if (!report.ContainsKey(customerID))
                        {
                            report[customerID] = new List<Appointments>();
                        }

                        report[customerID].Add(appointment);
                    }
                }
            }

            return report;
        }
    }
}

