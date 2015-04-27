using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseLib;
using System.Threading;
using System.Text.RegularExpressions;

namespace MixedCampaignManager.CustomUserControls
{
    public partial class retweetusercontrols : UserControl
    {
        public retweetusercontrols()
        {
            InitializeComponent();
        }

        BaseLib.NumberHelper Numberhelper = new BaseLib.NumberHelper();
        public BaseLib.Events retweetusercontrolslogEvents = new BaseLib.Events();
        private void chk_campRetweetPerDay_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
                classes.cls_variables._IsRetweetParDay = 1;
            else
                classes.cls_variables._IsRetweetParDay = 0;
        }


        private void chk_campRetweetbyUser_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
                classes.cls_variables._IsUsername = 1;
            else
                classes.cls_variables._IsUsername = 0;
        }

        private void txt_campMaximumNoRetweet_TextChanged(object sender, EventArgs e)
        {
           // string CheckNumber=TextBox.sender.Text

           bool CheckNumeric= CheckNumericValue(sender);
           if (CheckNumeric)
           {

           }
           else
           {
               MessageBox.Show("Please enter numeric value in text box.");
           }
            #region MyRegion

            //else
            //{

            //    MessageBox.Show("Please enter numeric value in text box.");
            //    ((TextBox)sender).Text = "10";
            //    ((TextBox)sender).Focus();
            //    //MessageBox.Show("Please enter numeric value in text box.");
            //    return;
            //}
            //if ( (int.Parse(((TextBox)sender).Text) >= 10))
            //    classes.cls_variables._NoofRetweetParDay = int.Parse((((TextBox)sender).Text));
            //else
            //    classes.cls_variables._NoofRetweetParDay = int.Parse((((TextBox)sender).Text)); 
            #endregion
        }

        private bool CheckNumericValue(object sender)
        {
            bool CheckStatus = false;
            try
            {

                if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) > 10)
                {
                    classes.cls_variables._NoofRetweetParDay = int.Parse((((TextBox)sender).Text));
                    return CheckStatus=true;
                }
                else
                {
                    classes.cls_variables._NoofRetweetParDay = 10;
                    CheckStatus = true;
                    return CheckStatus;
                }
            }
            catch
            {
                CheckStatus = false;
            }
            return CheckStatus;
        }

       
        private void ReTweetUserControlLog(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            retweetusercontrolslogEvents.LogText(eventArgs);
        }

        private void txt_campReTweetKeyword_TextChanged(object sender, EventArgs e)
        {
            if (BaseLib.NumberHelper.ValidateNumber(((TextBox)sender).Text))
            {
                MessageBox.Show("Please enter value in text box.");
                ((TextBox)sender).Focus();
                return;
            }
            if (!String.IsNullOrEmpty(((TextBox)sender).Text))
            {
                classes.cls_variables._retweetKeyword = ((TextBox)sender).Text;
            }
        }


        private void txt_campNoOfRetweetsParAc_TextChanged(object sender, EventArgs e)
        {
            if (!BaseLib.NumberHelper.ValidateNumber(((TextBox)sender).Text) || String.IsNullOrEmpty(((TextBox)sender).Text))
            {
                MessageBox.Show("Please enter Numeric value in text box.");
                ((TextBox)sender).Text = "10";
                ((TextBox)sender).Focus();
                return;
            }

            if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) >= 10)
            {
                classes.cls_variables._NoofRetweetParAc = int.Parse(((TextBox)sender).Text);
            }
            else
                classes.cls_variables._NoofRetweetParAc = int.Parse((((TextBox)sender).Text));
        }

        private void chkUniqueMessageRetweet_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
                classes.cls_variables._IsUniqueMessage = 1;
            else
                classes.cls_variables._IsUniqueMessage = 0;
        }


    }
}
