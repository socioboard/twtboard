using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PopupControl;
using System.Drawing.Drawing2D;
using BaseLib;

namespace MixedCampaignManager.CustomUserControls
{
    public partial class TweetMentionUserUsingScrapedList : UserControl
    {
        private System.Drawing.Image image;

        public TweetMentionUserUsingScrapedList()
        {
            InitializeComponent();

            //InitializeComponent();
            MinimumSize = Size;
            MaximumSize = new Size(Size.Width * 2, Size.Height * 2);
            DoubleBuffered = true;
            ResizeRedraw = true;

            ToolTip Tooltip = new ToolTip();
            Tooltip.SetToolTip(this.txtRetweetMentionUserName, "If u want to see updated userName.Click on textbox  to see the updated userName");
            //txtRetweetMentionUserName.GotFocus += new EventHandler(txtRetweetMentionUserName_GotFocus);
            //getUpdatedUserName();
            frm_mixcampaignmanager.PopUpUpdate.addToLogger += new EventHandler(CampaignnameLog);
        }

        void txtRetweetMentionUserName_GotFocus(object sender, EventArgs e)
        {
            try
            {
                string userName = txtRetweetMentionUserName.Text;
                string UserCount = txtTweetMentionNoofUserCount.Text;
                string totalCountResult = txtTweetCountMentinUser.Text;
                if (userName == "Enter UserName")
                {
                    txtRetweetMentionUserName.Text = classes.cls_variables._TweetMentionUserName;
                    txtTweetMentionNoofUserCount.Text = Convert.ToString(classes.cls_variables._NoOfTweetMentionUserName);
                    txtTweetCountMentinUser.Text = Convert.ToString(classes.cls_variables._NoOfTweetScrapedUser);
                    if (MixedCampaignManager.classes.cls_variables._IsTweetFollowerList == 1)
                    {
                        chkTweetFollowers.Checked = true;
                    }
                    else
                    {
                        chkTweetFollowers.Checked = false;
                    }

                    if (MixedCampaignManager.classes.cls_variables._IsTweetFollowingList == 1)
                    {
                        chkTweetFollowing.Checked = true;
                    }
                    else
                    {
                        chkTweetFollowing.Checked = false;
                    }
                    
                }
                else
                {
                    txtRetweetMentionUserName.Text = classes.cls_variables._TweetMentionUserName;
                    txtTweetMentionNoofUserCount.Text = Convert.ToString(classes.cls_variables._NoOfTweetMentionUserName);
                    txtTweetCountMentinUser.Text = Convert.ToString(classes.cls_variables._NoOfTweetScrapedUser);
                    if (MixedCampaignManager.classes.cls_variables._IsTweetFollowerList == 1)
                    {
                        chkTweetFollowers.Checked = true;
                    }
                    else
                    {
                        chkTweetFollowers.Checked = false;
                    }
                    if (MixedCampaignManager.classes.cls_variables._IsTweetFollowingList == 1)
                    {
                        chkTweetFollowing.Checked = true;
                    }
                    else
                    {
                        chkTweetFollowing.Checked = false;
                    }
                }
            }
            catch { }
        }

        public  void getUpdatedUserName(string temp)
        {
            try
            {
                txtRetweetMentionUserName.Invoke(new MethodInvoker(delegate
                {
                    string userName = txtRetweetMentionUserName.Text;
                    string UserCount = txtTweetMentionNoofUserCount.Text;
                    string totalCountResult = txtTweetCountMentinUser.Text;
                    if (userName == "Enter UserName")
                    {
                        txtRetweetMentionUserName.Text = classes.cls_variables._TweetMentionUserName;
                        txtTweetMentionNoofUserCount.Text = Convert.ToString(classes.cls_variables._NoOfTweetMentionUserName);
                        txtTweetCountMentinUser.Text = Convert.ToString(classes.cls_variables._NoOfTweetScrapedUser);
                        if (MixedCampaignManager.classes.cls_variables._IsTweetFollowerList == 1)
                        {
                            chkTweetFollowers.Checked = true;
                        }
                        else
                        {
                            chkTweetFollowers.Checked = false;
                        }
                        if (MixedCampaignManager.classes.cls_variables._IsTweetFollowingList == 1)
                        {
                            chkTweetFollowing.Checked = true;
                        }
                        else
                        {
                            chkTweetFollowing.Checked = false;
                        }

                    }
                    else
                    {
                        txtRetweetMentionUserName.Text = classes.cls_variables._TweetMentionUserName;
                        txtTweetMentionNoofUserCount.Text = Convert.ToString(classes.cls_variables._NoOfTweetMentionUserName);
                        txtTweetCountMentinUser.Text = Convert.ToString(classes.cls_variables._NoOfTweetScrapedUser);
                        if (MixedCampaignManager.classes.cls_variables._IsTweetFollowerList == 1)
                        {
                            chkTweetFollowers.Checked = true;
                        }
                        else
                        {
                            chkTweetFollowers.Checked = false;
                        }
                        if (MixedCampaignManager.classes.cls_variables._IsTweetFollowingList == 1)
                        {
                            chkTweetFollowing.Checked = true;
                        }
                        else
                        {
                            chkTweetFollowing.Checked = false;
                        }
                    }
                }));
            }
            catch { }
        }
        

        protected override void WndProc(ref Message m)
        {
            if ((Parent as Popup).ProcessResizing(ref m))
            {
                return;
            }
            base.WndProc(ref m);
        }
        
        private void TweetMentionUserUsingScrapedList_Load(object sender, EventArgs e)
        {
            //image = Properties.Resources.app_bg;
            
        }

        private void TweetMentionUserUsingScrapedList_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g;

            //g = e.Graphics;

            //g.SmoothingMode = SmoothingMode.HighQuality;
            //g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

        private void txtRetweetMentionUserName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                
                    //txtRetweetMentionUserName.Text = "";
                    if (!String.IsNullOrEmpty(((TextBox)sender).Text))
                        classes.cls_variables._TweetMentionUserName = (((TextBox)sender).Text);
                    

                    if (!string.IsNullOrEmpty(classes.cls_variables._TweetMentionUserName))
                    {
                        txtRetweetMentionUserName.Text = classes.cls_variables._TweetMentionUserName;
                    }
                    else
                    {
                        txtRetweetMentionUserName.Text = "Enter UserName";
                    }
            }
            catch { }

            
        }

        private void txtTweetMentionNoofUserCount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) >= 1)
                    classes.cls_variables._NoOfTweetMentionUserName = int.Parse((((TextBox)sender).Text));
                else
                    classes.cls_variables._NoOfTweetMentionUserName = 1;
            }
            catch
            { }
        }

        private void txtTweetCountMentinUser_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) >= 1)
                    classes.cls_variables._NoOfTweetScrapedUser = int.Parse((((TextBox)sender).Text));
                else
                    classes.cls_variables._NoOfTweetScrapedUser = 50;
            }
            catch
            { }
        }

        private void chkTweetFollowers_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                //grpMentionUserScrapedList.Invoke(new MethodInvoker(delegate
                //{
                    MixedCampaignManager.classes.cls_variables._IsTweetFollowerList = 1;
                    
                //}));

            }
            else
            {
                //grpMentionUserScrapedList.Invoke(new MethodInvoker(delegate
                //{
                    MixedCampaignManager.classes.cls_variables._IsTweetFollowerList = 0;
                    
                //}));
            }
        }

        private void chkTweetFollowing_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                //grpMentionUserScrapedList.Invoke(new MethodInvoker(delegate
                //{
                    MixedCampaignManager.classes.cls_variables._IsTweetFollowingList = 1;

                //}));

            }
            else
            {
                //grpMentionUserScrapedList.Invoke(new MethodInvoker(delegate
                //{
                    MixedCampaignManager.classes.cls_variables._IsTweetFollowingList = 0;

                //}));
            }
        }

        private void grpMentionUserScrapedList_Enter(object sender, EventArgs e)
        {
            //getUpdatedUserName();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            //getUpdatedUserName();
        }

        void CampaignnameLog(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                getUpdatedUserName(eArgs.log);
            }
        }
    }
}
