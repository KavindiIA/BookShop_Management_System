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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Connection string to SQL Server
            string cs = @"Data Source = LAPTOP-RTO6S53R\SQLEXPRESS02; 
  Initial Catalog = AttlatticBookShop; Integrated Security = True";

            // Sample values from registration form
            string fullName = txtFullname.Text;
            string username = txtUsername.Text;
            string email = txtEmail.Text;
            string phoneNo = txtPhoneNo.Text;
            string address = txtAddress.Text;
            string seqQuestion = cmbQuestion.Text;
            string answer = txtAnswer.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Check if any field is empty
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phoneNo) || string.IsNullOrEmpty(address) ||
                string.IsNullOrEmpty(seqQuestion) || string.IsNullOrEmpty(answer) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all the information.");
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
                // Creating a connection to the database
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();

                    // Define the SQL Insert Statement
                    string sql = "INSERT INTO Customer (fullName, username, email, phoneNo, address, seqQestion, answer, password) " +
                                 "VALUES (@fullName, @username, @email, @phoneNo, @address, @seqQestion, @answer, @password)";

                    // Create SQL command and add parameters
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@fullName", fullName);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phoneNo", phoneNo);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@seqQestion", seqQuestion);
                        cmd.Parameters.AddWithValue("@answer", answer);
                        cmd.Parameters.AddWithValue("@password", password);

                        // Execute the command
                        int ret = cmd.ExecuteNonQuery();

                        if (ret > 0)
                        {
                            MessageBox.Show("Registration successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            Form2 form2 = new Form2();
                            form2.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("An error occurred. Registration failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }
    }
}
