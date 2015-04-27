using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace twtboardpro
{
    public partial class FrmVerfyingImage : Form
    {
        public FrmVerfyingImage()
        {
            InitializeComponent();
        }

        List<string> lstParentVerifyEmail = new List<string>();
        List<string> lstChildVerfiyEmail = new List<string>();
        Dictionary<string, string> lstEmailPass = new Dictionary<string, string>();

        private void btnBrowseEmail_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        lstParentVerifyEmail.Clear();
                        lstParentVerifyEmail = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        txtVerfiyEmail.Text = ofd.FileName;
                    }
                }
            }
            catch
            {
            }
        }

        private void btnStartVerify_Click(object sender, EventArgs e)
        {
            if (lstParentVerifyEmail.Count > 0)
            {
                new Thread(() =>
                    {
                        StartVerification();
                    }).Start();
            }
            else
            {
                MessageBox.Show("Please Upload Email File");
            }
        }

        public void StartVerification()
        {
            foreach (string email in lstParentVerifyEmail)
            {
                string Email = string.Empty;
                string Pass = string.Empty;
                Chilkat.Http Http = new Chilkat.Http();
                EmailActivator.ClsEmailActivator ActivateEmail = new EmailActivator.ClsEmailActivator();
                ActivateEmail.Hotmails(Email, Pass, ref Http);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        lstChildVerfiyEmail.Clear();
                        lstChildVerfiyEmail = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        txtChildVerfiyEmail.Text = ofd.FileName;
                    }
                }
            }
            catch
            {
            }
        }

       
    }
}
