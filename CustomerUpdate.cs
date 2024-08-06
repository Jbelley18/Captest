using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace C969
{
    public partial class CustomerUpdate : Form
    {
        private DataGridViewRow _selectedRow;
        public CustomerUpdate(DataGridViewRow selectedRow)
        {
            InitializeComponent();
            _selectedRow = selectedRow;
        }

        private void CustomerUpdate_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            _selectedRow.Cells["CustomerName"].Value = textBox2.Text;
            _selectedRow.Cells["Address"].Value = textBox4.Text;
            _selectedRow.Cells["Phone"].Value = textBox5.Text;

            string query = "UPDATE address SET Address = @Address, Phone = @Phone WHERE AddressID = @AddressID;" +
                           "UPDATE customer SET CustomerName = @CustomerName WHERE AddressID = @AddressID;";

            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    MySqlCommand updateCMD = new MySqlCommand(query, con);
                    updateCMD.Parameters.AddWithValue("@@AddressID", _selectedRow.Cells["AddressID"].Value);
                    updateCMD.Parameters.AddWithValue("@Phone", textBox5.Text);
                    updateCMD.Parameters.AddWithValue("@CustomerName", textBox2.Text);
                    updateCMD.Parameters.AddWithValue("@Address", textBox4.Text);
                    updateCMD.ExecuteNonQuery();

                    Main form = new Main();
                    form.UpdateDataGridView();

                    MessageBox.Show("Customer updated successfully");
                    this.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error" + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
