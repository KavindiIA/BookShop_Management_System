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
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9();
            form9.Show();
            this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
            this.Close();
        }

        private void btnSearchBookID_Click(object sender, EventArgs e)
        {
            
        }

        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Connection string to SQL Server
            string cs = @"Data Source=LAPTOP-RTO6S53R\SQLEXPRESS02; 
                Initial Catalog = AttlatticBookShop; Integrated Security=True";

            // Creating a connection to the database
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                // Query to get details based on selected title
                string query = "SELECT bookID, title, author, ISBN, qty, price FROM Book WHERE title = @title";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Check if a title is selected
                    if (!string.IsNullOrEmpty(cmbSearch.Text))
                    {
                        cmd.Parameters.AddWithValue("@title", cmbSearch.Text);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtBookID.Text = reader["bookID"].ToString();
                                txtTitle.Text = reader["title"].ToString();
                                txtAuthor.Text = reader["author"].ToString();
                                txtISBN.Text = reader["ISBN"].ToString();
                                txtQty.Text = reader["qty"].ToString();
                                decimal price = Convert.ToDecimal(reader["price"]);
                                txtPrice.Text = $"${price:F2}";
                            }
                            else
                            {
                                MessageBox.Show("Title not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void Form8_Load_1(object sender, EventArgs e)
        {
            // Connection string to SQL Server
            string cs = @"Data Source=LAPTOP-RTO6S53R\SQLEXPRESS02; 
                Initial Catalog = AttlatticBookShop; Integrated Security=True";

            // Creating a connection to the database
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                // Query to get book titles
                string titleQuery = "SELECT title FROM Book";

                using (SqlCommand titleCmd = new SqlCommand(titleQuery, conn))
                {
                    // Clear existing items in the ComboBox
                    cmbSearch.Items.Clear();

                    using (SqlDataReader titleReader = titleCmd.ExecuteReader())
                    {
                        while (titleReader.Read())
                        {
                            // Add each title to the ComboBox
                            cmbSearch.Items.Add(titleReader["title"].ToString());
                        }
                    }
                }
            }
        }
    }
}

