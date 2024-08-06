using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace C969
{
    public partial class Main : Form
    {
        private ReportGenerator reportGenerator;
        private string connectionString;
        private Timer appointmentTimer;
        public Main()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            reportGenerator = new ReportGenerator(connectionString);
            GenerateAppointmentAlerts();
            appointmentTimer = new Timer();
            appointmentTimer.Interval = 60000;
            appointmentTimer.Tick += AppointmentTimer_Tick;
            appointmentTimer.Start();
        }

        private void ViewAppointmentCalendarByMonth(DateTime selectedDate)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            string query = @"
        SELECT UserID, Start, End, Title, Description
        FROM appointment 
        WHERE YEAR(Start) = @Year AND MONTH(Start) = @Month
    ";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@Year", selectedDate.Year);
                    cmd.Parameters.AddWithValue("@Month", selectedDate.Month);
                    con.Open();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(cmd.ExecuteReader());

                    dataGridView6.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void ViewAppointmentsForSpecificDay(DateTime selectedDate)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            string query = @"
        SELECT UserID, Start, End, Title, Description
        FROM appointment 
        WHERE DATE(Start) = @SelectedDate
    ";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@SelectedDate", selectedDate.Date);
                    con.Open();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(cmd.ExecuteReader());

                    dataGridView6.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private void AppointmentTimer_Tick(object sender, EventArgs e)
        {
            GenerateAppointmentAlerts();
        }
        
        private void Add_Click(object sender, EventArgs e)
        {
            Form1 customerAdd = new Form1();
            customerAdd.FormClosed += (s, args) => { UpdateDataGridView(); };
            customerAdd.textBox1.Text = customerAdd.addressID_Counter + 1.ToString();
            customerAdd.ShowDialog();

        }

        public void GenerateAppointmentAlerts()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            string query = @"
            SELECT UserID, Start 
            FROM appointment 
            WHERE Start BETWEEN NOW() AND DATE_ADD(NOW(), INTERVAL 15 MINUTE)
        ";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                try
                {
                    con.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int userId = reader.GetInt32("UserID");
                            DateTime start = reader.GetDateTime("Start");
                            TimeSpan timeUntilAppointment = start - DateTime.Now;

                            if (timeUntilAppointment <= TimeSpan.FromMinutes(15))
                            {
                                string alertMessage = $"User {userId} has an appointment within 15 minutes.";
                                GenerateAlert(alertMessage);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void GenerateAlert(string message)
        {
            MessageBox.Show(message, "Upcoming Appointment Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CustomerUpdate(object sender, EventArgs e)
        {

        }

        private void CustomerUpdate_Load(object sender, EventArgs e)
        {

        }

        public void Main_Load(object sender, EventArgs e)
        {
            UpdateDataGridView();
            UpdateDataGridView2();
        }

        public void UpdateDataGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string query = "SELECT customer.CustomerID, address.Phone, address.Address, customer.CustomerName, address.AddressID " +
                                   "FROM address " +
                                   "JOIN customer ON address.AddressID = customer.AddressId";

                    MySqlCommand cmd = new MySqlCommand(query, con);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Update();
                    dataGridView1.Refresh();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        public void UpdateDataGridView2()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string query = "SELECT appointment.CustomerID, appointment.Title, appointment.Description, appointment.Start, appointment.End " +
                                   "FROM appointment ";

                    MySqlCommand cmd = new MySqlCommand(query, con);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView2.DataSource = dataTable;
                    dataGridView2.Update();
                    dataGridView2.Refresh();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void CustomerAdd(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                string customerName = dataGridView1.SelectedRows[0].Cells["CustomerName"].Value.ToString();

                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();

                        string query = @"
                    DELETE c, a
                    FROM customer c
                    LEFT JOIN address a ON c.AddressID = a.AddressID
                    WHERE c.customerName = @CustomerName;
                ";

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@CustomerName", customerName);
                        cmd.ExecuteNonQuery();

                        UpdateDataGridView();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("No row is selected.");
            }
        }

        public void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                CustomerUpdate form = new CustomerUpdate(selectedRow);
                form.textBox5.Text = dataGridView1.SelectedRows[0].Cells["Phone"].Value.ToString();
                form.textBox2.Text = dataGridView1.SelectedRows[0].Cells["CustomerName"].Value.ToString();
                form.textBox4.Text = dataGridView1.SelectedRows[0].Cells["Address"].Value.ToString();
                form.textBox5.Text = dataGridView1.SelectedRows[0].Cells["Phone"].Value.ToString();
                form.ShowDialog();
                UpdateDataGridView(); ;
            }
            else
            {
                MessageBox.Show("No row has been selected");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddAppointment form = new AddAppointment();
            form.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                string description = dataGridView2.SelectedRows[0].Cells["Description"].Value.ToString();

                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();

                        string query = @"
                    DELETE FROM appointment
                    WHERE Description = @Description;
                ";

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Description", description);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully.");
                            UpdateDataGridView2();
                            dataGridView2.Refresh();
                        }
                        else
                        {
                            MessageBox.Show("Record not found or already deleted.");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }

            }
            else
            {
                MessageBox.Show("No row is selected.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];
                UpdateAppointment form = new UpdateAppointment(selectedRow);
                DateTime startDateTime = (DateTime)dataGridView2.SelectedRows[0].Cells["Start"].Value;
                string startDateStrung = startDateTime.ToString("MM-dd-yyyy");
                form.textBox1.Text = startDateStrung;
                form.textBox4.Text = dataGridView2.SelectedRows[0].Cells["Title"].Value.ToString();
                form.textBox5.Text = dataGridView2.SelectedRows[0].Cells["Description"].Value.ToString();
                form.textBox3.Text = dataGridView2.SelectedRows[0].Cells["CustomerID"].Value.ToString();
                form.ShowDialog();
                UpdateDataGridView2(); ;
            }
            else
            {
                MessageBox.Show("No row has been selected");
            }
        }

        public static void NumberOfAppointmentTypesByMonth(List<Appointment> appointments)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            var appointmentTypesByMonthReport = reportGenerator.GenerateAppointmentTypesByMonthReport();
            DataTable dataTable = ConvertReportToDataTable(appointmentTypesByMonthReport);
        }

        private DataTable ConvertReportToDataTable(Dictionary<string, Dictionary<string, int>> report)
        {
            DataTable dataTable = new DataTable();

            try
            {
                if (report != null && report.Count > 0)
                {
                    dataTable.Columns.Add("Month", typeof(string));

                    foreach (var monthData in report.Values)
                    {
                        foreach (var type in monthData.Keys)
                        {
                            if (!dataTable.Columns.Contains(type))
                            {
                                dataTable.Columns.Add(type, typeof(int));
                            }
                        }
                    }

                    foreach (var month in report)
                    {
                        DataRow row = dataTable.NewRow();
                        row["Month"] = month.Key;
                        foreach (var count in month.Value)
                        {
                            if (dataTable.Columns.Contains(count.Key))
                            {
                                row[count.Key] = count.Value;
                            }
                        }
                        dataTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while converting the report to DataTable: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dataGridView3.DataSource = dataTable;
            return dataTable;
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            var userScheduleReport = reportGenerator.GenerateUserScheduleReport();
            DataTable dataTable2 = ConvertUserScheduleReportToDataTable(userScheduleReport);
        }

        private DataTable ConvertUserScheduleReportToDataTable(Dictionary<int, List<UserReport>> report)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("UserID", typeof(int));
            dataTable.Columns.Add("AppointmentID", typeof(int));
            dataTable.Columns.Add("Start", typeof(DateTime));
            dataTable.Columns.Add("End", typeof(DateTime));

            foreach (var kvp in report)
            {
                foreach (var userReport in kvp.Value)
                {
                    DataRow row = dataTable.NewRow();
                    row["UserID"] = userReport.UserID;
                    row["AppointmentID"] = userReport.AppointmentID;
                    row["Start"] = userReport.Start;
                    row["End"] = userReport.End;
                    dataTable.Rows.Add(row);
                }
            }

            dataGridView4.DataSource = dataTable;

            return dataTable;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var userScheduleReport = reportGenerator.GenerateUserScheduleReportByCustomerID();

            DataTable dataTable = ConvertReportToDataTable(userScheduleReport);

            dataGridView5.DataSource = dataTable;
        }


        private DataTable ConvertReportToDataTable(Dictionary<int, List<Appointments>> report)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("CustomerID", typeof(int));
            dataTable.Columns.Add("AppointmentID", typeof(int));
            dataTable.Columns.Add("Start", typeof(DateTime));
            dataTable.Columns.Add("End", typeof(DateTime));

            foreach (var pair in report)
            {
                int customerID = pair.Key;
                foreach (var appointment in pair.Value)
                {
                    DataRow row = dataTable.NewRow();
                    row["CustomerID"] = appointment.CustomerID;
                    row["AppointmentID"] = appointment.AppointmentID;
                    row["Start"] = appointment.Start;
                    row["End"] = appointment.End;
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                ViewAppointmentCalendarByMonth(dateTimePicker1.Value);
            }
            else if (radioButton2.Checked)
            {
                ViewAppointmentsForSpecificDay(dateTimePicker1.Value);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            bool viewByMonths = radioButton1.Checked;
            dateTimePicker1.Enabled = viewByMonths;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            bool viewByDays = radioButton2.Checked;
            dateTimePicker1.Enabled = viewByDays;
        }
    }
}
