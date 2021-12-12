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

namespace ContactBook
{
    public partial class Form1 : Form
    {
        string connectionString = @"Server=localhost;Database=contactbookdb;Uid=root;Pwd=password;";
        int contactID = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            using(MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("contactAddOrEdit", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_contactID", contactID);
                mySqlCmd.Parameters.AddWithValue("_firstName", textBoxFName.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_lastName", textBoxLName.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_Contact", textBoxContact.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_Email", textBoxEmail.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_Address", textBoxAddress.Text.Trim());
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Submitted Successfully!");
                clear();
                GridFill();
            }
        }

        void GridFill()
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlData = new MySqlDataAdapter("contactViewAll", mysqlCon);
                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtblContact = new DataTable();
                sqlData.Fill(dtblContact);
                dataGridView.DataSource = dtblContact;
                dataGridView.Columns[0].Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clear();
            GridFill();
        }

        void clear()
        {
            textBoxFName.Text = textBoxLName.Text = textBoxContact.Text = textBoxEmail.Text = textBoxAddress.Text = textBoxSearch.Text = "";
            contactID = 0;
            buttonCreate.Text = "Create";
            buttonDelete.Enabled = false;
        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
           
            if (dataGridView.CurrentRow.Index != -1)
            {
                textBoxFName.Text = dataGridView.CurrentRow.Cells[1].Value.ToString();
                textBoxLName.Text = dataGridView.CurrentRow.Cells[2].Value.ToString();
                textBoxContact.Text = dataGridView.CurrentRow.Cells[3].Value.ToString();
                textBoxEmail.Text = dataGridView.CurrentRow.Cells[4].Value.ToString();
                textBoxAddress.Text = dataGridView.CurrentRow.Cells[5].Value.ToString();
                contactID = Convert.ToInt32(dataGridView.CurrentRow.Cells[0].Value.ToString());
                buttonCreate.Text = "Update";
                buttonDelete.Enabled = true;
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlData = new MySqlDataAdapter("contactSearchByValue", mysqlCon);
                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlData.SelectCommand.Parameters.AddWithValue("_searchValue", textBoxSearch.Text);
                DataTable dtblContact = new DataTable();
                sqlData.Fill(dtblContact);
                dataGridView.DataSource = dtblContact;
                dataGridView.Columns[0].Visible = false;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("contactDeleteByID", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_contactID", contactID);
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Deleted Successfully!");
                clear();
                GridFill();
            }
        }
    }
}
