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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void customerReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a connection with SQL DB Server
            string cs = @"Data Source = LAPTOP-RTO6S53R\SQLEXPRESS02; 
              Initial Catalog = AttlatticBookShop; Integrated Security = True";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // Command to define SQL Statements
                string sql = "SELECT * FROM Customer";
                SqlCommand cmd = new SqlCommand(sql, con);

                // Read data using DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                // Check if the dataset contains data
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // Bind data with Crystal Report
                    CrystalReport3 rpt3 = new CrystalReport3();
                    rpt3.Load(@"C:\Users\Kavindi\source\repos\AtlanticBookShop\CrystalReport3.rpt");
                    rpt3.SetDataSource(ds.Tables[0]);
                    this.crystalReportViewer1.ReportSource = rpt3;
                }
                else
                {
                    MessageBox.Show("No data found in the Customer table.");
                }

                // Disconnect
                con.Close();
            }
        }

        private void bookManagemetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void salesReportToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void salesReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Create a connection with SQL DB Server
            string cs = @"Data Source = LAPTOP-RTO6S53R\SQLEXPRESS02; 
              Initial Catalog = AttlatticBookShop; Integrated Security = True";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // Command to define SQL Statements
                string sql = "SELECT * FROM OrderDetails od "
                    + "JOIN Customer c ON od.CusID = c.CusID "
                    + "JOIN Book b ON od.BookID = b.BookID";
                SqlCommand cmd = new SqlCommand(sql, con);

                // Read data using DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                // Check if the dataset contains data
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // Bind data with Crystal Report
                    CrystalReport4 rpt4 = new CrystalReport4();
                    rpt4.Load(@"C:\Users\Kavindi\source\repos\AtlanticBookShop\CrystalReport4.rpt");
                    rpt4.SetDataSource(ds.Tables[0]);
                    this.crystalReportViewer1.ReportSource = rpt4;
                }
                else
                {
                    MessageBox.Show("No data found in the Sales table.");
                }

                // Disconnect
                con.Close();
            }
        }
    }
}
