using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using BaseLib.DB_Repository;
using System.Drawing.Drawing2D;
using twtboardpro;
using BaseLib;


namespace twtboardpro
{
    public partial class frmAccountPasswordUpdate : Form
    {
        public System.Drawing.Image image;
        TwitterUser objCheckAccount = new TwitterUser();
        clsDBQueryManager objclsDBQueryManager = new clsDBQueryManager();
        frmAccounts objfrmAccounts = new frmAccounts();
        string userId = string.Empty;
        public frmAccountPasswordUpdate()
        {
            InitializeComponent();
            
        }

        

        private void frmAccountPasswordUpdate_Load(object sender, EventArgs e)
        {
           // image = ServerModule.Properties.Resources.center_bg;

            try
            {
                userId = TwitterUser.userIdForUpdate;
                string passWord = TwitterUser.passwordForUpdate;
                txtUserName.Text = userId;
                txtOldPassword.Text = passWord;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewPassword.Text))
            {
                try
                {
                    objclsDBQueryManager.UpdatePasswordForAccount(userId, txtNewPassword.Text.Trim());
                   
                    MessageBox.Show("Password is updated successfully");
                    btnUpdatePassword.Invoke(new MethodInvoker(delegate
                    {
                        this.Close();
                       
                    }));
                    
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    MessageBox.Show("Error in updating password");
                }
            }
            else 
            {
                MessageBox.Show("Please Enter New Password");
            }
        }

        private void frmAccountPasswordUpdate_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g;
                g = e.Graphics;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(image, 0, 0, this.Width, this.Height);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
