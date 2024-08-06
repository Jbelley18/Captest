using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C969
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
        public int customerID_Counter = 0;

        public int addressID_Counter = 0;

        public int countryID_Counter = 0;

        public int cityID_Counter = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            MySqlConnection con = new MySqlConnection(connectionString);

            try
            {
                con.Open();

                string address = textBox4.Text; 
                string addressid = addressID_Counter++.ToString();
                string address2 = "";
                string city = "";
                string country = "";
                int cityId = cityID_Counter++;
                string postalCode = "12345";
                string phone = textBox5.Text;
                int customerID = customerID_Counter++;
                string customerName = textBox2.Text;
                DateTime createDate = DateTime.Now;
                string createdBy = "sqlUser";
                string lastUpdate = createDate.ToString("yyyy-MM-dd HH:mm:ss");
                string lastUpdateBy = "sqlUser";
                bool active = true;


                int countryId = countryID_Counter++;

                string countryQuery = "INSERT INTO country (CountryId, Country, CreateDate, CreatedBy, LastUpdate, LastUpdateBy) " +
                                            "VALUES (@CountryId, @Country, @CreateDate, @CreatedBy, @LastUpdate, @LastUpdateBy)";

                MySqlCommand countryCmd = new MySqlCommand(countryQuery, con);
                countryCmd.Parameters.AddWithValue("@CountryId", countryId);
                countryCmd.Parameters.AddWithValue("@Country", country);
                countryCmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                countryCmd.Parameters.AddWithValue("@CreatedBy", "sqlUser");
                countryCmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                countryCmd.Parameters.AddWithValue("@LastUpdateBy", "sqlUser");
                countryCmd.ExecuteNonQuery();

                int countryId2 = (int)countryCmd.LastInsertedId;

                string cityQuery = "INSERT INTO city (CityID, City, CountryID, CreateDate, CreatedBy, LastUpdate, LastUpdateBy) " +
                                    "VALUES (@CityID, @City, @CountryID, @CreateDate, @CreatedBy, @LastUpdate, @LastUpdateBy)";

                MySqlCommand cityCmd = new MySqlCommand(cityQuery, con);
                cityCmd.Parameters.AddWithValue("@CityID", cityId);
                cityCmd.Parameters.AddWithValue("@City", city);
                cityCmd.Parameters.AddWithValue("@CountryID", countryId2);
                cityCmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                cityCmd.Parameters.AddWithValue("@CreatedBy", "sqlUser");
                cityCmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                cityCmd.Parameters.AddWithValue("@LastUpdateBy", "sqlUser");
                cityCmd.ExecuteNonQuery();

                int cityId2 = (int)cityCmd.LastInsertedId;

                string addressQuery = "INSERT INTO address (AddressID, Address, Address2, CityId, PostalCode, Phone, CreateDate, CreatedBy, LastUpdate, LastUpdateBy) " +
                                       "VALUES (@AddressID, @Address, @Address2, @CityId, @PostalCode, @Phone, @CreateDate, @CreatedBy, @LastUpdate, @LastUpdateBy)";

                MySqlCommand addressCmd = new MySqlCommand(addressQuery, con);
                addressCmd.Parameters.AddWithValue("@AddressID", addressid);
                addressCmd.Parameters.AddWithValue("@Address", address);
                addressCmd.Parameters.AddWithValue("@Address2", address2);
                addressCmd.Parameters.AddWithValue("@CityId", cityId2);
                addressCmd.Parameters.AddWithValue("@PostalCode", postalCode);
                addressCmd.Parameters.AddWithValue("@Phone", phone);
                addressCmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                addressCmd.Parameters.AddWithValue("@CreatedBy", "sqlUser");
                addressCmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                addressCmd.Parameters.AddWithValue("@LastUpdateBy", "sqlUser");
                addressCmd.ExecuteNonQuery();

                int addressId2 = (int)addressCmd.LastInsertedId;

                string customerQuery = "INSERT INTO customer (CustomerID, CustomerName, AddressId, Active, CreateDate, CreatedBy, LastUpdate, LastUpdateBy) " +
                                        "VALUES (@CustomerID, @CustomerName, @AddressId, @Active, @CreateDate, @CreatedBy, @LastUpdate, @LastUpdateBy)";

                MySqlCommand customerCmd = new MySqlCommand(customerQuery, con);
                customerCmd.Parameters.AddWithValue("@CustomerID", customerID);
                customerCmd.Parameters.AddWithValue("@CustomerName", customerName);
                customerCmd.Parameters.AddWithValue("@AddressId", addressId2);
                customerCmd.Parameters.AddWithValue("@Active", active);
                customerCmd.Parameters.AddWithValue("@CreateDate", createDate);
                customerCmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                customerCmd.Parameters.AddWithValue("@LastUpdate", lastUpdate);
                customerCmd.Parameters.AddWithValue("@LastUpdateBy", lastUpdateBy);
                customerCmd.ExecuteNonQuery();

                Main form = new Main();

                form.UpdateDataGridView();

                MessageBox.Show("Customer added successfully.");
                this.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void ValidateTextBoxes()
        {
            if (textBox2.Text.Length > 0 && textBox4.Text.Length > 0 && textBox5.Text.Length >= 10)
            {
                button1.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                button1.Enabled = false;
                textBox2.BackColor = Color.Red;
            }
            else
            {
                textBox2.BackColor = Color.White;

            }
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                button1.Enabled = false;
                textBox4.BackColor = Color.Red;
            }
            else
            {
                textBox4.BackColor = Color.White;
            }
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                button1.Enabled = false;
                textBox5.BackColor = Color.Red;
            }
            else
            {
                textBox5.BackColor = Color.White;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string phone = textBox5.Text;

            Regex regex = new Regex(@"([\-]?\d[\-]?){10}");

            Match match = regex.Match(phone);

            ToolTip tooltip1 = new ToolTip();

            if (match.Success)
            {
                button1.Enabled = true;
                textBox5.BackColor = Color.White;
            }
            else if(match.Success == false || string.IsNullOrEmpty(textBox5.Text))
            {
                tooltip1.SetToolTip(textBox5, "Enter a valid phone number");
                textBox5.BackColor = Color.Red;
                button1.Enabled = false;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                button1.Enabled = false;
                textBox4.BackColor = Color.Red;
            }
            else
            {
                textBox4.BackColor = Color.White;
                ValidateTextBoxes();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                button1.Enabled = false;
                textBox2.BackColor = Color.Red;
            }
            else
            {
                textBox2.BackColor = Color.White;
                ValidateTextBoxes();
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
