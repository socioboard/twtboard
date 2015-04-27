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
    public partial class frmForPremimumUsersOnly : Form
    {
        public frmForPremimumUsersOnly()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore", "http://twtboardpro.com/pricing.php");
        }
    }
}
