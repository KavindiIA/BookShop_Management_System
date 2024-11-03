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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace AtlanticBookShop
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();  
            form4.Show();
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Connection string to SQL Server
            string cs = @"Data Source = LAPTOP-RTO6S53R\SQLEXPRESS02; 
                Initial Catalog = AttlatticBookShop; Integrated Security = True";

            // Sample username and password from textboxes
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Define hardcoded admin credentials
            string adminUsername = "adminUser";
            string adminPassword = "adminPass123";

            // Check if username or password fields are empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
            }
            else if (username == adminUsername && password == adminPassword)
            {
                // If the credentials match the admin's, grant admin access
                MessageBox.Show("Admin login successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Open the admin-specific form
                Form6 adminForm = new Form6(); // Replace with your actual admin form class
                adminForm.Show();
                this.Close();
            }
            else
            {
                // Creating a connection to the database
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();

                    // Define the SQL Query to check for username and password
                    string sql = "SELECT username, password FROM Customer WHERE username = @username AND password = @password";

                    // Create SQL command and add parameters
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        // Execute the command and read the result
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) // If a row is returned, login is successful
                            {
                                MessageBox.Show("Login successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // Optionally, redirect to another form or perform additional actions
                                Form7 form7 = new Form7();
                                form7.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }

        }
    }
    
}
