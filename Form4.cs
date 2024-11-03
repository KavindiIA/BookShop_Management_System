using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtlanticBookShop
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(txtAnswer.Text) || string.IsNullOrEmpty(txtConfirmPassword.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtSQuestion.Text) || string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Please fill in all information.");
            }
            else if (password.Length < 8 || password.Length > 12)
            {
                MessageBox.Show("Password must be between 8 and 12 characters.");
            }
            else if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please confirm your password.");
            }
            else
            {
                // Connection string to SQL Server
                string cs = @"Data Source = LAPTOP-RTO6S53R\SQLEXPRESS02; 
                    Initial Catalog = AttlatticBookShop; Integrated Security = True";

                // Creating a connection to the database
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();

                    // Define the SQL Update Statement
                    string sql = @"UPDATE Customer 
                       SET password = @password
                       WHERE username = @username";

                    // Create SQL command and add parameters
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add parameters with values from the GUI controls
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@password", this.txtPassword.Text);

                        // Execute the command and get the number of rows affected
                        int ret = cmd.ExecuteNonQuery();

                        if (ret > 0)
                        {
                            MessageBox.Show("Record updated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Form2 frm = new Form2();
                            frm.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No record found with the specified Book ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }  
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
            this.Close();
        }

        private void btnSearchname_Click(object sender, EventArgs e)
        {
            // Check if username is provided
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Please enter a Username to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Connection string to SQL Server
            string cs = @"Data Source = LAPTOP-RTO6S53R\SQLEXPRESS02; 
                 Initial Catalog = AttlatticBookShop; Integrated Security = True";

            // Creating a connection to the database
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                // Define the SQL Select Statement to retrieve details based on username
                string sql = "SELECT seqQestion FROM Customer WHERE username = @username";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Add parameter for username
                    cmd.Parameters.AddWithValue("@username", this.txtUsername.Text);

                    // Execute the command and read data
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        txtUsername.ReadOnly = true;
                        txtAnswer.ReadOnly = false;

                        if (reader.Read())
                        {
                            // Populate the text boxes with retrieved data
                            txtSQuestion.Text = reader["seqQestion"].ToString();
                        }
                        else
                        {
                            // username not found
                            MessageBox.Show("Username not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btnSearchAnswer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtSQuestion.Text) || string.IsNullOrEmpty(txtAnswer.Text))
            {
                MessageBox.Show("Please enter information to the required fields to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Connection string to SQL Server
            string cs = @"Data Source = LAPTOP-RTO6S53R\SQLEXPRESS02; 
                Initial Catalog = AttlatticBookShop; Integrated Security = True";

            // Creating a connection to the database
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                // Define the SQL Select Statement to retrieve the answer based on username
                string sql = "SELECT answer FROM Customer WHERE username = @username";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Add parameter for username
                    cmd.Parameters.AddWithValue("@username", this.txtUsername.Text);

                    // Execute the command and read data
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the stored answer from the database
                            string storedAnswer = reader["answer"].ToString();

                            // Now check if the provided answer is correct
                            if (txtAnswer.Text.Equals(storedAnswer, StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show("Answer is correct! You can now update your information.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtUsername.ReadOnly = false;
                                txtAnswer.ReadOnly = true;
                                txtPassword.ReadOnly = false;
                                txtConfirmPassword.ReadOnly = false;
                            }
                            else
                            {
                                MessageBox.Show("Incorrect answer. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            // Username not found
                            MessageBox.Show("Username not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
