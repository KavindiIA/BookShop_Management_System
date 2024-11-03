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
using System.Xml.Linq;

namespace AtlanticBookShop
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void ClearFields()
        {
            txtSearchID.Text = string.Empty;
            txtBookID.Text = string.Empty;  
            txtTitle.Clear();
            txtAuthor.Clear();
            txtISBN.Clear();
            numericUpDown1.Value = numericUpDown1.Minimum;
            txtPrice.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBookID.Text) || string.IsNullOrEmpty(txtTitle.Text) || string.IsNullOrEmpty(txtAuthor.Text) || string.IsNullOrEmpty(txtISBN.Text) || string.IsNullOrEmpty(txtPrice.Text) /*|| numericUpDown1.Value != numericUpDown1.Minimum*/)
            {
                MessageBox.Show("Please fill the all information.");
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

                    // Define the SQL Insert Statement
                    string sql = "INSERT INTO Book (bookID, title, author, ISBN, qty, price) VALUES (@bookID, @title, @author, @ISBN, @qty, @price)";

                    // Create SQL command and add parameters
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add parameters with values from the GUI controls
                        cmd.Parameters.AddWithValue("@bookID", this.txtBookID.Text);
                        cmd.Parameters.AddWithValue("@title", this.txtTitle.Text);
                        cmd.Parameters.AddWithValue("@author", this.txtAuthor.Text);
                        cmd.Parameters.AddWithValue("@ISBN", this.txtISBN.Text);
                        cmd.Parameters.AddWithValue("@qty", this.numericUpDown1.Value);  
                        cmd.Parameters.AddWithValue("@price", float.Parse(this.txtPrice.Text));

                        // Execute the command
                        int ret = cmd.ExecuteNonQuery();

                        MessageBox.Show("No of records inserted : " + ret, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
            }
        }

        private void btnSearchBookID_Click(object sender, EventArgs e)
        {
             // Check if BookID is provided
             if (string.IsNullOrEmpty(txtSearchID.Text))
             {
                MessageBox.Show("Please enter a BookID to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
             }

             // Connection string to SQL Server
             string cs = @"Data Source = LAPTOP-RTO6S53R\SQLEXPRESS02; 
                 Initial Catalog = AttlatticBookShop; Integrated Security = True";

             // Creating a connection to the database
             using (SqlConnection conn = new SqlConnection(cs))
             {
                conn.Open();

                // Define the SQL Select Statement to retrieve details based on BookID
                string sql = "SELECT bookID, title, author, ISBN, qty, price FROM Book WHERE bookID = @bookID";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Add parameter for BookID
                    cmd.Parameters.AddWithValue("@bookID", this.txtSearchID.Text);

                    // Execute the command and read data
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Populate the text boxes and numeric up-down with retrieved data
                            txtBookID.Text = reader["bookID"].ToString();
                            txtTitle.Text = reader["title"].ToString();
                            txtAuthor.Text = reader["author"].ToString();
                            txtISBN.Text = reader["ISBN"].ToString();
                            numericUpDown1.Value = Convert.ToInt32(reader["qty"]);
                            txtPrice.Text = reader["price"].ToString();
                        }
                        else
                        {
                            // BookID not found
                            MessageBox.Show("BookID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            // Clear the fields if needed
                            ClearFields();
                        }
                    }
                }
             }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBookID.Text) || string.IsNullOrEmpty(txtTitle.Text) || string.IsNullOrEmpty(txtAuthor.Text) || string.IsNullOrEmpty(txtISBN.Text) || string.IsNullOrEmpty(txtPrice.Text) || numericUpDown1.Value == numericUpDown1.Minimum)
            {
                MessageBox.Show("Please fill in all information.");
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
                    string sql = @"UPDATE Book 
                       SET title = @title, 
                           author = @author, 
                           ISBN = @ISBN, 
                           qty = @qty, 
                           price = @price
                       WHERE bookID = @bookID";

                    // Create SQL command and add parameters
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add parameters with values from the GUI controls
                        cmd.Parameters.AddWithValue("@bookID", this.txtBookID.Text);
                        cmd.Parameters.AddWithValue("@title", this.txtTitle.Text);
                        cmd.Parameters.AddWithValue("@author", this.txtAuthor.Text);
                        cmd.Parameters.AddWithValue("@ISBN", this.txtISBN.Text);
                        cmd.Parameters.AddWithValue("@qty", this.numericUpDown1.Value);
                        cmd.Parameters.AddWithValue("@price", float.Parse(this.txtPrice.Text));

                        // Execute the command and get the number of rows affected
                        int ret = cmd.ExecuteNonQuery();

                        if (ret > 0)
                        {
                            MessageBox.Show("Record updated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No record found with the specified Book ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Check if the bookID field is not empty
            if (string.IsNullOrEmpty(txtSearchID.Text))
            {
                MessageBox.Show("Please enter the Book ID to delete.");
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

                    // Define the SQL Delete Statement
                    string sql = "DELETE FROM Book WHERE bookID = @bookID";

                    // Create SQL command and add parameter
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add the bookID parameter with value from the txtBookID textbox
                        cmd.Parameters.AddWithValue("@bookID", this.txtSearchID.Text);

                        // Execute the command
                        int ret = cmd.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (ret > 0)
                        {
                            MessageBox.Show("Record deleted successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No record found with the given Book ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }

        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            // Create a connection with SQL DB Server
            string cs = @"Data Source = LAPTOP-RTO6S53R\SQLEXPRESS02; 
              Initial Catalog = AttlatticBookShop; Integrated Security = True";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // Command to define SQL Statements
                string sql = "SELECT * FROM Book";
                SqlCommand cmd = new SqlCommand(sql, con);

                // Read data using DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                // Check if the dataset contains data
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // Bind data with Crystal Report
                    CrystalReport2 rpt1 = new CrystalReport2();
                    rpt1.Load(@"C:\Users\Kavindi\source\repos\AtlanticBookShop\CrystalReport2.rpt");
                    rpt1.SetDataSource(ds.Tables[0]);
                    this.crystalReportViewer1.ReportSource = rpt1;
                }
                else
                {
                    MessageBox.Show("No data found in the Book table.");
                }

                // Disconnect
                con.Close();
            }
        }
    }
}
