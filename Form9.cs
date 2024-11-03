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
    public partial class Form9 : Form
    {
        private int availableQty = 0;

        public Form9()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
            this.Close();
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            // Connection string to SQL Server
            string cs = @"Data Source=LAPTOP-RTO6S53R\SQLEXPRESS02; 
        Initial Catalog = AttlatticBookShop; Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                string customerFullName = txtFullname.Text;

                // Query to get the Customer ID based on the full name
                string customerQuery = "SELECT cusID FROM Customer WHERE FullName = @fullName";

                string customerId = ""; // Variable to store the found CusID
                using (SqlCommand customerCmd = new SqlCommand(customerQuery, conn))
                {
                    customerCmd.Parameters.AddWithValue("@fullName", customerFullName);

                    // Execute the reader to get the customer ID
                    using (SqlDataReader customerReader = customerCmd.ExecuteReader())
                    {
                        if (customerReader.Read())
                        {
                            customerId = customerReader["cusID"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Customer not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                // Get the current quantity of the book
                int currentQty = 0;
                string bookID = txtBookID.Text; 
                string qtyQuery = "SELECT qty FROM Book WHERE bookID = @bookID";

                using (SqlCommand qtyCmd = new SqlCommand(qtyQuery, conn))
                {
                    qtyCmd.Parameters.AddWithValue("@bookID", bookID);

                    using (SqlDataReader qtyReader = qtyCmd.ExecuteReader())
                    {
                        if (qtyReader.Read())
                        {
                            currentQty = Convert.ToInt32(qtyReader["qty"]);
                        }
                        else
                        {
                            MessageBox.Show("Book not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; 
                        }
                    }
                }

                // Check if the requested quantity is available
                decimal requestedQuantity = numericUpDown1.Value;
                if (requestedQuantity > currentQty)
                {
                    MessageBox.Show("Not enough stock available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }

                // Now insert the order details into OrderDetail table
                string insertQuery = "INSERT INTO OrderDetails (bookID, cusID, date, totQuantity, total) VALUES (@bookID, @cusID, @date, @totQuantity, @total);";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@bookID", bookID);
                    insertCmd.Parameters.AddWithValue("@cusID", customerId);
                    insertCmd.Parameters.AddWithValue("@date", dateTimePicker1.Value);
                    insertCmd.Parameters.AddWithValue("@totQuantity", requestedQuantity);
                    insertCmd.Parameters.AddWithValue("@total", decimal.Parse(txtTotal.Text, System.Globalization.NumberStyles.Currency));

                    // Execute the insert command
                    int rowsAffected = insertCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Update the Book table to subtract the quantity sold
                        int newQty = currentQty - (int)requestedQuantity;
                        string updateQuery = "UPDATE Book SET qty = @newQty WHERE bookID = @bookID";

                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@newQty", newQty);
                            updateCmd.Parameters.AddWithValue("@bookID", bookID);
                            updateCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Order placed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form10 frm10 = new Form10();
                        frm10.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to place order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Form9_Load(object sender, EventArgs e)
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
                                txtISBN.Text = reader["ISBN"].ToString();
                                txtPrice.Text = "$" + reader["price"].ToString();

                                availableQty = Convert.ToInt32(reader["qty"]);
                                numericUpDown1.Maximum = availableQty;
                            }
                            else
                            {
                                MessageBox.Show("BookID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtPrice.Text.Trim('$'), out decimal unitPrice)) //To remove $ sign
            {
                decimal quantity = numericUpDown1.Value;
                decimal totalPrice = unitPrice * quantity;

                // Display the total price (you might want to use a Label or another TextBox)
                txtTotal.Text = totalPrice.ToString("C"); // Format as currency
            }
            else
            {
                txtTotal.Text = "Invalid Price"; // Handle cases where price is not a valid decimal
            }
        }
    }
}
