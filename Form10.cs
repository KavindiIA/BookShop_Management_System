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
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form10_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
            this.Close();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            // Check if BookID is provided
            if (string.IsNullOrEmpty(txtOrderID.Text))
            {
                MessageBox.Show("Please enter a OrderID to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string sql = "SELECT od.orderID, c.fullName, c.email, c.phoneNo, od.date, " +
                     "b.title, b.price AS unitPrice, od.totQuantity AS quantity, od.total AS totalPrice " +
                     "FROM OrderDetails od " +
                     "JOIN Customer c ON od.cusID = c.cusID " +
                     "JOIN Book b ON od.bookID = b.bookID " +
                     "WHERE od.orderID = @orderID;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Add parameter for BookID
                    cmd.Parameters.AddWithValue("@orderID", this.txtOrderID.Text);

                    // Execute the command and read data
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtOrderIDBill.Text = reader["orderID"].ToString();
                            txtCusName.Text = reader["fullName"].ToString();
                            txtEmail.Text = reader["email"].ToString();
                            txtPhoneNo.Text = reader["phoneNo"].ToString();

                            DateTime orderDate = Convert.ToDateTime(reader["date"]);
                            txtDate.Text = orderDate.ToShortDateString();

                            txtTitle.Text = reader["title"].ToString();
                            txtUPrice.Text = "$" + Convert.ToDecimal(reader["unitPrice"]).ToString("F2");
                            txtQty.Text = reader["quantity"].ToString();
                            txtTotal.Text = "$" + Convert.ToDecimal(reader["totalPrice"]).ToString("F2"); 
                        }
                        else
                        {
                            MessageBox.Show("OrderID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                }
            }
        }
    }
}
