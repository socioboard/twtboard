using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Follower;
using BaseLib;
using System.Threading;
using System.Drawing.Drawing2D;


namespace MixedCampaignManager.CustomUserControls
{
    public partial class followusecontrols : UserControl
    {
        public followusecontrols()
        {
            InitializeComponent();
        }


        BaseLib.NumberHelper Numberhelper = new BaseLib.NumberHelper();
        public static BaseLib.Events CampaignFollowerLogEvents = new BaseLib.Events();

        private void followusecontrols_Load(object sender, EventArgs e)
        {

        }

        private void grp_Followusercontrol_Enter(object sender, EventArgs e)
        {


        }

        private void btnPathUserIDs_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_FollowUserIDsfilePath.Invoke(new MethodInvoker(delegate
                        {
                            txt_FollowUserIDsfilePath.Text = ofd.FileName;
                            classes.cls_variables._followersfilepath = ofd.FileName;
                        }));
                }
            }
        }

        private void rdBtnDivideEqually_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton RdioBtn = (RadioButton)sender;
                if (RdioBtn.Text == "Divide Equally" && RdioBtn.Checked == true)
                {
                    classes.cls_variables._DivideEqually = 1;
                    classes.cls_variables._NoofUsers = 0;
                    txtScrapeNoOfUsers.Invoke(new MethodInvoker(delegate
                        {
                            txtScrapeNoOfUsers.Text = "";
                        }));
                }
                else
                    classes.cls_variables._DivideEqually = 0;
            }
            catch (Exception)
            {
            }
        }

        private void rdBtn_DivideByGivenNo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton RdioBtn = (RadioButton)sender;
                if (RdioBtn.Text == "Divide Given By User" && RdioBtn.Checked == true)
                {
                    classes.cls_variables._DivideByGivenNo = 1;
                    txtScrapeNoOfUsers.Invoke(new MethodInvoker(delegate
                        {
                            txtScrapeNoOfUsers.Enabled = true;
                            txtScrapeNoOfUsers.Focus();
                        }));
                }
                else
                {
                    classes.cls_variables._DivideByGivenNo = 0;
                    txtScrapeNoOfUsers.Invoke(new MethodInvoker(delegate
                    {
                        txtScrapeNoOfUsers.Enabled = false;

                    }));
                }
            }
            catch (Exception)
            {
            }
        }

        private void chkUseDivide_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkUseDivide.Checked == true)
                {
                    if (rdBtn_DivideEqually.Checked)
                    {
                        grp_DivideData.Enabled = true;
                        grp_FollowCountParAc.Enabled = false;
                        classes.cls_variables._DivideEqually = 1;
                    }
                    else if (rdBtn_DivideByGivenNo.Checked)
                    {
                        grp_DivideData.Enabled = true;
                        grp_FollowCountParAc.Enabled = false;
                        classes.cls_variables._DivideByGivenNo = 1;
                    }
                }
                else
                {
                    grp_DivideData.Enabled = false;
                    classes.cls_variables._DivideEqually = 0;
                    classes.cls_variables._DivideByGivenNo = 0;
                    classes.cls_variables._NoofUsers = 0;
                    grp_FollowCountParAc.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
        }

        private void txtScrapeNoOfUsers_TextChanged(object sender, EventArgs e)
        {
            if (!BaseLib.NumberHelper.ValidateNumber(((TextBox)sender).Text) && !String.IsNullOrEmpty(((TextBox)sender).Text))
            {
                MessageBox.Show("Please Enter Numeric Value in Text Box.");
                txtScrapeNoOfUsers.Focus();
                ((TextBox)sender).Text = "10";
                return;
            }
            if (!String.IsNullOrEmpty(((TextBox)sender).Text) && BaseLib.NumberHelper.ValidateNumber(((TextBox)sender).Text))
                classes.cls_variables._NoofUsers = int.Parse((((TextBox)sender).Text));
            else
                classes.cls_variables._NoofUsers = 0;
        }

        private void txtScrapeNoOfUsers_Validating(object sender, CancelEventArgs e)
        {
            //if (!BaseLib.NumberHelper.ValidateNumber(((TextBox)sender).Text))
            //{
            //    MessageBox.Show("Please Enter Numeric Value in Text Box.");
            //    txtScrapeNoOfUsers.Focus();
            //    return;
            //}
        }

        private void txt_followParAccount_TextChanged(object sender, EventArgs e)
        {
            if (!BaseLib.NumberHelper.ValidateNumber(((TextBox)sender).Text))
            {
                MessageBox.Show("Please Enter Numeric Value in Text Box.");
                ((TextBox)sender).Text = "10";
                ((TextBox)sender).Focus();

                return;
            }
            if (!String.IsNullOrEmpty(((TextBox)sender).Text) && BaseLib.NumberHelper.ValidateNumber(((TextBox)sender).Text))
                classes.cls_variables._NoofFollowParAC = int.Parse((((TextBox)sender).Text));
            else
                classes.cls_variables._NoofFollowParAC = 0;
        }

        void logEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                Log(eArgs.log);
            }
        }

        private void Log(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            CampaignFollowerLogEvents.LogText(eventArgs);
        }

        private void txt_FollowUserIDsfilePath_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(((TextBox)sender).Text))
                classes.cls_variables._followersfilepath = (((TextBox)sender).Text);
        }
    }
}
