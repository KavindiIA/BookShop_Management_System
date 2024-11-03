using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtlanticBookShop
{
    public partial class Form11 : Form
    {
        private Timer closeTimer;
        public Form11()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // Initialize the timer
            closeTimer = new Timer();
            closeTimer.Interval = 2000; // 2000 milliseconds = 2 seconds
            closeTimer.Tick += CloseTimer_Tick;
            closeTimer.Start();
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            closeTimer.Stop();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
