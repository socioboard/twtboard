using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MixedCampaignManager.CustomUserControls
{
    public partial class replyusercontrols : UserControl
    {
        public replyusercontrols()
        {
            InitializeComponent();
        }

        BaseLib.NumberHelper Numberhelper = new BaseLib.NumberHelper();
        public BaseLib.Events replyusercontrolslogEvents = new BaseLib.Events();

       

        private void txt_campReplyKeyword_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(((TextBox)sender).Text))
                classes.cls_variables._replyKeyword = (((TextBox)sender).Text);
        }

        private void chk_campReplybyUser_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
                classes.cls_variables._IsReplyUsername = 1;
            else
                classes.cls_variables._IsReplyUsername = 0;
        }

        private void chk_campReplyPerDay_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
                classes.cls_variables._IsreplyParDay = 1;
            else
                classes.cls_variables._IsreplyParDay = 0;
        }

        private void txt_campMaximumNoReply_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) > 10)
                classes.cls_variables._NoofreplyParDay = int.Parse((((TextBox)sender).Text));
            else
                classes.cls_variables._NoofreplyParDay = 10;
        }

        private void txt_campNoOfRepliesParAc_TextChanged_1(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) > 10)
                classes.cls_variables._NoofreplyParAc = int.Parse((((TextBox)sender).Text));
            else
                classes.cls_variables._NoofreplyParAc = 10;
        }

        private void btn_UploadreplyMsg_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_replyMsg.Text = ofd.FileName;
                    classes.cls_variables._replyMsgFilePath = ofd.FileName;
                }
            }
        }

        private void txt_replyMsg_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(((TextBox)sender).Text))
                classes.cls_variables._replyMsgFilePath = ((((TextBox)sender).Text));
        }

        private void ChkUniqueMessage_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
                classes.cls_variables._IsUniqueMessage = 1;
            else
                classes.cls_variables._IsUniqueMessage = 0;
        }

        private void btnUpload_Keyword_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_campReplyKeyword.Text = ofd.FileName;
                    classes.cls_variables._replyKeyword = ofd.FileName;
                }
            }
        }

        private void chkReplyUseDuplicateMsg_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (((CheckBox)sender).Checked == true)
                    classes.cls_variables._replyUseDuplicateMsg = 1;
                else
                    classes.cls_variables._replyUseDuplicateMsg = 0;
            }
            catch { }
        }

      

         

    }
}
