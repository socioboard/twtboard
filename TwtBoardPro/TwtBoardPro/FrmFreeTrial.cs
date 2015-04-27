using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace twtboardpro
{
    public partial class FrmFreeTrial : Form
    {
        public FrmFreeTrial()
        {
            this.BringToFront();
            this.TopMost = true;
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore", "http://twtboardpro.com/pricing.php");
        }

        private void FrmFreeTrial_Load(object sender, EventArgs e)
        {

        }
    }
}
