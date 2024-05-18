using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace WindowsFormsApp8
{
    public partial class Form1 : Form
    {
        private MySqlConnection connection;
        private MySqlDataAdapter adapterBooks;
        private MySqlDataAdapter adapterSubscriptions;
        private DataSet dataSet;

        private string connectionString = "server=localhost;user=root;password=2005;database=1523;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private void dataGridViewBooks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            SaveChanges(adapterBooks, "books");
        }

        private void SaveChanges(MySqlDataAdapter adapter, string tableName)
        {
            try
            {
                adapter.Update(dataSet, tableName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message);
            }
        }

        private void LoadData()
        {
            connection = new MySqlConnection(connectionString);
            dataSet = new DataSet();

            try
            {
                connection.Open();

                // Load Books table
                adapterBooks = new MySqlDataAdapter("SELECT b_id, b_name, b_year, b_quantity FROM books", connection);
                MySqlCommandBuilder commandBuilderBooks = new MySqlCommandBuilder(adapterBooks);
                adapterBooks.Fill(dataSet, "books");
                dataGridView1.DataSource = dataSet.Tables["books"];

                // Load Subscriptions table
                adapterSubscriptions = new MySqlDataAdapter("SELECT * FROM subscriptions", connection);
                MySqlCommandBuilder commandBuilderSubscriptions = new MySqlCommandBuilder(adapterSubscriptions);
                adapterSubscriptions.Fill(dataSet, "subscriptions");
                dataGridView2.DataSource = dataSet.Tables["subscriptions"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveChanges(adapterBooks, "books");
            SaveChanges(adapterSubscriptions, "subscriptions");
            MessageBox.Show("Changes saved successfully.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataRow newRow = dataSet.Tables["books"].NewRow();
            newRow["b_name"] = "New Book";
            newRow["b_year"] = DateTime.Now.Year;
            newRow["b_quantity"] = 1;
            dataSet.Tables["books"].Rows.Add(newRow);
        }
    }
}