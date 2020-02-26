using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Contact_App
{
    public partial class Form1 : Form
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-84LNPPD;Initial Catalog=ContactDB;Integrated Security=True");
        int ContactID = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                if (btnSave.Text == "Save")
                {
                    SqlCommand sqlCmd = new SqlCommand("ContactAddOrEdit", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@mode", "Add");
                    sqlCmd.Parameters.AddWithValue("@ContactID", 0);
                    sqlCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@MobileNumber", txtMobileNumber.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Saved succesfully");
                }
                else
                {
                    SqlCommand sqlCmd = new SqlCommand("ContactAddOrEdit", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@mode", "Edit");
                    sqlCmd.Parameters.AddWithValue("@ContactID", ContactID);
                    sqlCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@MobileNumber", txtMobileNumber.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Updated succesfully");
                }
                Reset();
                FillDataGridView();
            }
            
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error Message");
           }
            finally
            {
                sqlCon.Close();
            }

        }
        void FillDataGridView()
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();

            SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewOrSearch", sqlCon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDa.SelectCommand.Parameters.AddWithValue("@ContactName", txtSearch.Text.Trim());
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            dgvContacts.DataSource = dtbl;
            dgvContacts.Columns[0].Visible = false;
            sqlCon.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillDataGridView();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error Message");
            }
        }

        private void dgvContacts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvContacts_DoubleClick(object sender, EventArgs e)
        {
            if (dgvContacts.CurrentRow.Index != -1)
            {
                ContactID= Convert.ToInt32(dgvContacts.CurrentRow.Cells[0].Value.ToString());
                txtName.Text = dgvContacts.CurrentRow.Cells[1].Value.ToString();
                txtMobileNumber.Text = dgvContacts.CurrentRow.Cells[2].Value.ToString();
                txtAddress.Text = dgvContacts.CurrentRow.Cells[3].Value.ToString();
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        void Reset()
        {
            txtName.Text = txtMobileNumber.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            ContactID = 0;
            btnDelete.Enabled = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Reset();
            FillDataGridView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
               
                    SqlCommand sqlCmd = new SqlCommand("ContactDeletion", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    
                    sqlCmd.Parameters.AddWithValue("@ContactID", ContactID);
                    
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Deleted succesfully");
                Reset();
                FillDataGridView();

                }
            catch ( Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");

            }
        }
    }
}
