using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseLib;
using MixedCampaignManager.classes;
using CampaignManager;
using System.Threading;
using Tweeter;
using System.IO;
using System.Drawing.Drawing2D;
using System.Data.SQLite;
using Follower;
using BaseLib;

namespace MixedCampaignManager
{
    public partial class frm_mixcampaignmanager : Form
    {
        public frm_mixcampaignmanager()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        CustomUserControls.tweetusercontrols tweetusercontrol;
        CustomUserControls.followusecontrols followusercontrol;
        CustomUserControls.retweetusercontrols retweetusercontrol;
        CustomUserControls.replyusercontrols replyusercontrol;
        CustomUserControls.TweetMentionUserUsingScrapedList TweetMentionUserUsingScrapedList;
        private System.Drawing.Image image;


        #region

        String _CmpName = String.Empty;
        String _AccountFilePath = String.Empty;
        static DataSet CompaignsDataSet = new DataSet();
        int Threads = 7;
        public static BaseLib.Events PopUpUpdate = new BaseLib.Events();
        public static void UpdatePopUpofmentionUser(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            PopUpUpdate.LogText(eArgs);
        }
        #endregion


        private void frm_mixcampaignmanager_Load(object sender, EventArgs e)
        {            
            cls_variables.lstCampaignStartShedular.Clear();
            //dgv_campaign.RowHeadersVisible = false;
            Cls_FollowStart.CampaignStopLogevents.addToLogger += new EventHandler(CampaignnameLog);

            Cls_FollowStart.CampaignStartLogevents.addToLogger += new EventHandler(FeatureNameLogLog);

            Cls_FollowStart.CampaignFinishedLogevents.addToLogger += new EventHandler(FinishedNameLog);
            Cls_Retweet.CampaignFinishedLogevents.addToLogger += new EventHandler(FinishedNameLog);
            Cls_ReplyStart.CampaignFinishedLogevents.addToLogger += new EventHandler(FinishedNameLog);
            Cls_StartTweetProcess.CampaignFinishedLogevents.addToLogger += new EventHandler(FinishedNameLog);

            //Cls_StartTweetProcess.CampaignStopLogevents.addToLogger += new EventHandler(CampaignnameLog);

            //Cls_StartTweetProcess.CampaignStartLogevents.addToLogger += new EventHandler(FeatureNameLogStartTweetMessages);

            CustomUserControls.tweetusercontrols.logEvents.addToLogger += new EventHandler(logEvents_addToLogger);
            //set background image in form ..
            image = Properties.Resources.app_bg;

            //Set min or max date of date time picker control ..
            dateTimePicker_Start.MinDate = DateTime.Now;
            dateTimePicker_End.MinDate = DateTime.Now;

            //set start up possition of window in desktop screen ..
            this.StartPosition = FormStartPosition.CenterScreen;

            //checked by default value in featur checkbox list ...
            chklstbox_campaign.Invoke(new MethodInvoker(delegate
                {

                    chklstbox_campaign.SetItemCheckState(0, CheckState.Checked);
                    chklstbox_campaign.SelectedIndex = 0;
                }));

            btn_UpdateCampaign.Enabled = false;


            #region << Check table availability & Upload All campaign's >>
            //check Required table is available in Db

            MixedCampaignManager.RepositoryClasses.CreateTableRepository TableRepo = new RepositoryClasses.CreateTableRepository();

            try
            {
                if (TableRepo.CheckAndCreateTable())
                {
                    LoadCampaign();
                }
                else
                {
                    MessageBox.Show("Required tables is not Exist in Data Base.");
                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ Required tables is not Exist in Data Base. ]");
                }
            }
            catch(Exception ex)
            {
                ErrorLogger.AddToErrorLogText("1.frm_mixcampaignmanager_Load :" + ex.Message);
            }
            
            #endregion


            #region << check Scheduled daily Task >>

            //if User is scheduled ant campain on daily 
            // so its checking all campaign and start process on scheduler basis

            if (CompaignsDataSet.Tables.Count != 0 && CompaignsDataSet.Tables[0].Rows.Count != 0)
            {
                new Thread(() =>
                    {
                        ScheduledTasks();
                    }).Start();
            }

            #endregion

           
        }

        void CampaignnameLog(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                StoprunningCampaign(eArgs.log);
            }
        }

        void FeatureNameLogLog(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                StartFollowCampaign(eArgs.log, eArgs.log);
            }
        }

        void FinishedNameLog(object sender, EventArgs e)
        {
            try
            {
                Cls_StartTweetProcess.CampaignStartLogevents.addToLogger -= FeatureNameLogStartTweetMessages;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("2.FinishedNameLog :" + ex.Message);
            }
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                controlStartButtonImage(eArgs.log);
            }
        }

        void FeatureNameLogStartTweetMessages(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                startTweet(eArgs.log, eArgs.log);
            }
        }

        private void btn_uploadaccounts_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txt_accountfilepath.Text = ofd.FileName;
                        _AccountFilePath = string.Empty;
                        if (!String.IsNullOrEmpty(ofd.FileName))
                        {
                            _AccountFilePath = ofd.FileName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("3.btn_uploadaccounts_Click :" + ex.Message);
            }
        }

        private void chklstbox_campaign_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                for (int ix = 0; ix < chklstbox_campaign.Items.Count; ++ix)
                {
                    if (ix != e.Index) chklstbox_campaign.SetItemChecked(ix, false);
                }


                if (e.Index == 0 && e.NewValue == CheckState.Checked)
                {
                    if (tweetusercontrol != null)
                    {
                        grp_settings.Invoke(new MethodInvoker(delegate
                        {
                            tweetusercontrol.Dispose();
                            GC.Collect();
                        }));
                    }
                    grp_settings.Invoke(new MethodInvoker(delegate
                    {
                        grp_settings.Controls.Clear();
                        followusercontrol = new MixedCampaignManager.CustomUserControls.followusecontrols();
                        followusercontrol.Dock = DockStyle.Top;
                        followusercontrol.Visible = true;

                        panel1.Invoke(new MethodInvoker(delegate
                            {
                                panel1.AutoScroll = true;
                            }));
                        int i = followusercontrol.Height;
                        grp_settings.Height = i + 15;
                        grp_settings.Controls.Add(followusercontrol);
                        grp_settings.Refresh();
                    }));
                }
                else if (e.Index == 1 && e.NewValue == CheckState.Checked)
                {
                    if (followusercontrol != null)
                    {
                        grp_settings.Invoke(new MethodInvoker(delegate
                        {
                            followusercontrol.Dispose();
                            GC.Collect();

                        }));
                    }
                    grp_settings.Invoke(new MethodInvoker(delegate
                    {
                        panel1.Invoke(new MethodInvoker(delegate
                        {
                            panel1.AutoScroll = false;
                        }));

                        grp_settings.Controls.Clear();
                        tweetusercontrol = new MixedCampaignManager.CustomUserControls.tweetusercontrols();
                        tweetusercontrol.Dock = DockStyle.Top;

                        tweetusercontrol.Visible = true;
                        int i = tweetusercontrol.Height;
                        grp_settings.Height = i + 15;
                        grp_settings.Controls.Add(tweetusercontrol);
                        grp_settings.Refresh();
                    }));
                }
                else if (e.Index == 2 && e.NewValue == CheckState.Checked)
                {
                    if (retweetusercontrol != null)
                    {
                        grp_settings.Invoke(new MethodInvoker(delegate
                        {
                            retweetusercontrol.Dispose();
                            GC.Collect();

                        }));
                    }
                    grp_settings.Invoke(new MethodInvoker(delegate
                    {
                        panel1.Invoke(new MethodInvoker(delegate
                        {
                            panel1.AutoScroll = false;
                        }));

                        grp_settings.Controls.Clear();
                        retweetusercontrol = new MixedCampaignManager.CustomUserControls.retweetusercontrols();
                        retweetusercontrol.Dock = DockStyle.Top;
                        retweetusercontrol.Visible = true;

                        int i = retweetusercontrol.Height;
                        grp_settings.Height = i + 15;
                        grp_settings.Controls.Add(retweetusercontrol);
                        grp_settings.Refresh();
                    }));
                }
                else if (e.Index == 3 && e.NewValue == CheckState.Checked)
                {

                    if (replyusercontrol != null)
                    {
                        grp_settings.Invoke(new MethodInvoker(delegate
                        {
                            replyusercontrol.Dispose();
                            GC.Collect();

                        }));
                    }
                    grp_settings.Invoke(new MethodInvoker(delegate
                    {
                        panel1.Invoke(new MethodInvoker(delegate
                        {
                            panel1.AutoScroll = false;
                        }));

                        grp_settings.Controls.Clear();
                        replyusercontrol = new MixedCampaignManager.CustomUserControls.replyusercontrols();
                        replyusercontrol.Dock = DockStyle.Top;
                        replyusercontrol.Visible = true;
                        int i = replyusercontrol.Height;
                        grp_settings.Height = i + 15;
                        grp_settings.Controls.Add(replyusercontrol);
                        grp_settings.Refresh();
                    }));
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("4.chklstbox_campaign_ItemCheck :" + ex.Message);
            }
        }

        private void btn_savecampaign_Click(object sender, EventArgs e)
        {

            try
            {
                string checkItemlstCampaign = string.Empty;
                foreach (string item in chklstbox_campaign.CheckedItems)
                {
                    checkItemlstCampaign = item;
                }
                //Save new Campaign
                string Result = Validations("Save");

                if (!string.IsNullOrEmpty(Result))
                {
                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ " + Result + " ]");
                }
                else
                {
                    if (chklstbox_campaign.SelectedItem == "Follow" || checkItemlstCampaign == "Follow")
                    {
                        SaveFollowSettings();
                    }
                    else if (chklstbox_campaign.SelectedItem == "Tweet" || checkItemlstCampaign == "Tweet")
                    {
                        SaveTweetSettings();
                    }
                    else if (chklstbox_campaign.SelectedItem == "Retweet" || checkItemlstCampaign == "Retweet")
                    {
                        SavingReTweetSettings();
                    }
                    else if (chklstbox_campaign.SelectedItem == "Reply" || checkItemlstCampaign == "Reply")
                    {
                        SaveReplySettings();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("5.btn_savecampaign_Click :" + ex.Message);
            }
        }

        public string Validations(string Function)
        {
            string Result = string.Empty;
            try
            {
                if (Function == "Save")
                {
                    if (String.IsNullOrEmpty(txt_CampaignName.Text))
                    {
                        MessageBox.Show("Please Enter Campaign name.");
                        return Result = "Please Enter Campaign name.";
                    }
                    else
                    {
                        try
                        {
                            DataRow[] Drow = CompaignsDataSet.Tables[0].Select("CampaignName = '" + txt_CampaignName.Text + "'");
                            if (Drow.Count() != 0)
                            {
                                MessageBox.Show("Please enter different name of campaign.");
                                return Result = "Please enter different name of campaign.";
                            }
                        }
                        catch (Exception)
                        {
                        }

                        _CmpName = (txt_CampaignName.Text);
                    }
                }
                else
                {
                    _CmpName = (txt_CampaignName.Text);
                }


                if (String.IsNullOrEmpty(txt_accountfilepath.Text))
                {
                    MessageBox.Show("Please Select Account File.");
                    return Result = "Please Enter Campaign name.";
                }
                else
                    _AccountFilePath = (txt_accountfilepath.Text);

                if (String.IsNullOrEmpty(dateTimePicker_Start.Text))
                {
                    MessageBox.Show("Delay start from process.");
                    return Result = "Delay start from process.";
                }
                else
                    classes.cls_variables._StartFrom = dateTimePicker_Start.Text;

                if (String.IsNullOrEmpty(dateTimePicker_End.Text))
                {
                    MessageBox.Show("Delay End Time (In Second)");
                    return Result = "Delay End Time (In Second)";
                }
                else
                    classes.cls_variables._EndTo = dateTimePicker_End.Text;

                if (String.IsNullOrEmpty(txt_DelayFrom.Text))
                {
                    MessageBox.Show("Please Enter Delay Start Time");
                    return Result = "Please Enter Delay Start Time";
                }
                else
                    classes.cls_variables._DelayFrom = int.Parse(txt_DelayFrom.Text);

                if (String.IsNullOrEmpty(txt_DelayTo.Text))
                {
                    MessageBox.Show("Please Enter Delay End Time");
                    return Result = "Please Enter Delay End Time";
                }
                else
                    classes.cls_variables._DelayTo = int.Parse(txt_DelayTo.Text);

                if (classes.cls_variables._DelayFrom > classes.cls_variables._DelayTo)
                {
                    MessageBox.Show("Please Enter Correct value in delay.");
                    return Result = "Please Enter Correct value in delay.";
                }

                if (chkbox_IsScheduledDaily.Checked == true)
                {
                    var startTime = dateTimePicker_Start.Value;
                    var endtime = dateTimePicker_End.Value;
                    if (endtime > startTime)
                    {
                        classes.cls_variables._StartFrom = startTime.ToString();
                        classes.cls_variables._EndTo = endtime.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Please Enter Correct time/ End Time is grater than Start Time", "Warning");
                        return Result = "Please Enter Correct time/ End Time is grater than Start Time.";
                    }
                }

                if (chklstbox_campaign.SelectedItem == "Follow")
                {
                    if (string.IsNullOrEmpty(classes.cls_variables._followersfilepath))
                    {
                        MessageBox.Show("Please Upload Following User Name File.");
                        return Result = "Please Upload Following User Name File.";
                    }

                    if (classes.cls_variables._DivideByGivenNo == 1 && classes.cls_variables._NoofUsers == 0)
                    {
                        MessageBox.Show("Please enter a numeric value in Divided by user.");

                        #region
                        Control frmfollowusercontrols = new Control();
                        frmfollowusercontrols = followusercontrol.TopLevelControl.Controls.Find("txtScrapeNoOfUsers", true)[0];

                        if (frmfollowusercontrols != null)
                        {
                            if (frmfollowusercontrols.Name == "txtScrapeNoOfUsers")
                            {
                                if (frmfollowusercontrols != null)
                                {
                                    frmfollowusercontrols.Invoke(new MethodInvoker(delegate
                                    {
                                        frmfollowusercontrols.Focus();
                                    }));
                                }
                            }
                        }
                        #endregion

                        return Result = "Please enter a numeric value in Divided by user.";
                    }
                }
                if (!(MixedCampaignManager.classes.cls_variables._Istweetwithimage == 1))
                {
                    if (chklstbox_campaign.SelectedItem == "Tweet")
                    {
                        if (string.IsNullOrEmpty(classes.cls_variables._TweetMSGfilepath))
                        {
                            MessageBox.Show("Please Upload Tweet message File.");
                            return Result = "Please Upload Tweet Message File.";
                        }
                    }
                }

                if (chklstbox_campaign.SelectedItem == "Retweet")
                {
                    if (string.IsNullOrEmpty(classes.cls_variables._retweetKeyword))
                    {
                        MessageBox.Show("Please enter retweet keyword.");
                        return Result = "Please enter retweet keyword.";
                    }
                }
                if (chklstbox_campaign.SelectedItem == "Reply")
                {
                    if (string.IsNullOrEmpty(classes.cls_variables._replyKeyword))
                    {
                        MessageBox.Show("Please enter rereply keyword.");
                        return Result = "Please enter reply keyword.";
                    }
                    if (string.IsNullOrEmpty(classes.cls_variables._replyMsgFilePath))
                    {
                        MessageBox.Show("Please enter rereply message File.");
                        return Result = "Please enter reply message File.";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("6.Validations :" + ex.Message);
            }

            return Result;
        }

        private void txt_DelayFrom_TextChanged(object sender, EventArgs e)
        {
            String DelayFrom = ((TextBox)sender).Text;
            if (!String.IsNullOrEmpty(DelayFrom))
            {
                try
                {
                    if ((classes.cls_variables._DelayFrom != int.Parse(DelayFrom)) && int.Parse(DelayFrom) != 0)
                        classes.cls_variables._DelayFrom = int.Parse(DelayFrom);
                }
                catch { MessageBox.Show("Please Enter Correct value In delay Time."); ErrorLogger.AddToErrorLogText("6.txt_DelayFrom_TextChanged :"); };
            }
        }

        private void txt_DelayTo_TextChanged(object sender, EventArgs e)
        {
            String Delayto = ((TextBox)sender).Text;
            if (!String.IsNullOrEmpty(Delayto))
            {
                try
                {
                    if ((classes.cls_variables._DelayTo != int.Parse(Delayto)) && int.Parse(Delayto) != 0)
                        classes.cls_variables._DelayTo = int.Parse(Delayto);
                }
                catch { MessageBox.Show("Please Enter Correct value In delay Time."); ErrorLogger.AddToErrorLogText("7.txt_DelayTo_TextChanged :"); };

            }
            else
            {
                MessageBox.Show("Please Enter Correct value In delay Time.");
            }
        }

        private void chkbox_IsScheduledDaily_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                classes.cls_variables._IsScheduledDaily = 1;
                lb_ScheduleTime.Enabled = true;
                lb_To.Enabled = true;
                dateTimePicker_Start.Enabled = true;
                dateTimePicker_End.Enabled = true;
            }
            else
            {
                classes.cls_variables._IsScheduledDaily = 0;
                classes.cls_variables._StartFrom = "";
                classes.cls_variables._EndTo = "";
                lb_ScheduleTime.Enabled = false;
                lb_To.Enabled = false;
                dateTimePicker_Start.Enabled = false;
                dateTimePicker_End.Enabled = false;
            }
        }

        private void txtNoOfFollowThreads_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                String NoOfthread = ((TextBox)sender).Text;
                if (!String.IsNullOrEmpty(NoOfthread))
                {
                    //if ((7 <= int.Parse(NoOfthread)) && int.Parse(NoOfthread) != 0)
                    if (int.Parse(NoOfthread) != 0)
                        Threads = int.Parse(NoOfthread);
                    else
                    {
                        MessageBox.Show("Please Enter number of threads.");
                        Threads = 7;
                        txtNoOfFollowThreads.Text = "7";
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter number of threads.");
                }
            }

            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("8.txtNoOfFollowThreads_Validating :" + ex.Message);
            }
        }

        private void dgv_campaign_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                if (e.ColumnIndex == 2)
                {
                    if (e.RowIndex >= 0)
                    {
                        #region ---- Edit Campaign's ----

                        chklstbox_campaign.Invoke(new MethodInvoker(delegate
                        {
                            chklstbox_campaign.SelectionMode = SelectionMode.One;
                        }));

                        btn_UpdateCampaign.Enabled = true;

                        string CampaignName = dgv_campaign.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                        string FeaturName = dgv_campaign.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();

                        // Your code would go here below is just the code I used to test 
                        Bitmap ImgBitmap = (Bitmap)(dgv_campaign.Rows[e.RowIndex].Cells[3].FormattedValue);
                        string Img = GetImageValue(ImgBitmap);

                        if (Img == "OFF")
                        {
                            AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [  " + CampaignName + " is running. Please stop process before Editing. ]");
                            MessageBox.Show(CampaignName + " is running. Please stop process before Editing.", "Warning");
                            return;
                        }

                        if (FeaturName.Contains("Follow"))
                        {
                            //Edit Followers Campaign ...
                            new Thread(() =>
                              {
                                  editFollowerCampaign(CampaignName, FeaturName);
                              }).Start();
                        }
                        else if (FeaturName.Contains("Tweet"))
                        {
                            new Thread(() =>
                            {
                                editingTweetCampaign(CampaignName, FeaturName);
                            }).Start();
                        }
                        else if (FeaturName.Contains("Retweet"))
                        {
                            new Thread(() =>
                            {
                                editingReTweetCampaign(CampaignName, FeaturName);
                            }).Start();
                        }
                        else if (FeaturName.Contains("Reply"))
                        {
                            new Thread(() =>
                            {
                                editingReplyCampaign(CampaignName, FeaturName);
                            }).Start();
                        }

                        #endregion
                    }
                }
                else if (e.ColumnIndex == 3)
                {

                    if (dgv_campaign.Columns[e.ColumnIndex].Name == "BtnOnOff")
                    {
                        if (e.RowIndex >= 0)
                        {
                            // Your code would go here below is just the code I used to test 
                            Bitmap ImgBitmap = (Bitmap)(dgv_campaign.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                            string Img = GetImageValue(ImgBitmap);

                            string CampaignName = dgv_campaign.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                            string FeaturName = dgv_campaign.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();

                            if (Img == "ON")
                            {
                                #region ---- Start Camapign's ----

                                dgv_campaign.Invoke(new MethodInvoker(delegate
                                {
                                    dgv_campaign.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (System.Drawing.Image)Properties.Resources.off;
                                }));

                                //Start Processes according to User Selection 
                                AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ Start Campaign :- " + CampaignName + " ]");

                                if (FeaturName.Contains("Follow"))
                                {
                                    new Thread(() =>
                                    {
                                        StartFollowCampaign(CampaignName, FeaturName);
                                    }).Start();
                                }
                                else if (FeaturName.Contains("Tweet"))
                                {

                                    new Thread(() =>
                                    {
                                        //create new event to print log messages ..!!

                                        startTweet(CampaignName, FeaturName);
                                    }).Start();
                                }
                                else if (FeaturName.Contains("Retweet"))
                                {
                                    new Thread(() =>
                                    {
                                        StartReTweetCampaign(CampaignName, FeaturName);
                                    }).Start();
                                }
                                else if (FeaturName.Contains("Reply"))
                                {
                                    new Thread(() =>
                                    {
                                        StartReplyCampaign(CampaignName, FeaturName);
                                    }).Start();
                                }

                                #endregion
                            }
                            else if (Img == "OFF")
                            {
                                #region ---- Stop campaign's ----
                                //If process is already running 
                                // and user wants to stop all process 
                                // then click off option and aborting all running processes 
                                dgv_campaign.Invoke(new MethodInvoker(delegate
                                {
                                    dgv_campaign.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (System.Drawing.Image)Properties.Resources.on;
                                }));

                                new Thread(() =>
                                    {
                                        bool doStartScheduler = true;
                                        StoprunningCampaign(CampaignName, doStartScheduler);
                                    }).Start();

                                #endregion
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
                else if (e.ColumnIndex == 4)
                {
                    if (e.RowIndex >= 0)
                    {
                        #region ---- Pause/Stop Campaign's ----

                        chklstbox_campaign.Invoke(new MethodInvoker(delegate
                        {
                            chklstbox_campaign.SelectionMode = SelectionMode.One;
                        }));

                        btn_UpdateCampaign.Enabled = true;

                        string CampaignName = dgv_campaign.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                        string FeaturName = dgv_campaign.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();

                        string textPauseResume = dgv_campaign.Rows[e.RowIndex].Cells[4].FormattedValue.ToString();


                        DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

                        DataRow DrCampaignDetails = drModelDetails[0];
                        bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;


                        if (textPauseResume == "Pause")
                        {
                                DataGridViewButtonCell dgbtn = null;
                                if (!IsSchedulDaily)
                                {
                                    dgbtn = (DataGridViewButtonCell)(dgv_campaign.Rows[e.RowIndex].Cells["CampaignPauseStop"]);
                                    dgbtn.UseColumnTextForButtonValue = false;
                                    
                                    dgv_campaign.CurrentCell = dgv_campaign.Rows[e.RowIndex].Cells["CampaignPauseStop"];
                                    dgv_campaign.CurrentCell.ReadOnly = false;
                                    dgbtn.Value = "Resume";
                                    dgv_campaign.CurrentCell.ReadOnly = true;

                                    
                                    new Thread(() =>
                                    {

                                        PausedrunningCampaign(CampaignName);
                                    }).Start();
                                }
                                else
                                {
                                    MessageBox.Show("This campaign is with scheduling. so it can't be paused.");
                                }
                            //dgv_campaign.Invoke(new MethodInvoker(delegate
                            //{
                            //    dgv_campaign.Rows[e.RowIndex].Cells["CampaignPauseStop"].Value = "Resume";

                            //}));
                           
                            
                        }
                        else
                        {
                            if (textPauseResume == "Resume")
                            {
                                DataGridViewButtonCell dgbtn = null;
                                dgbtn = (DataGridViewButtonCell)(dgv_campaign.Rows[e.RowIndex].Cells["CampaignPauseStop"]);

                                dgbtn.UseColumnTextForButtonValue = false;
                                dgv_campaign.CurrentCell = dgv_campaign.Rows[e.RowIndex].Cells["CampaignPauseStop"];
                                dgv_campaign.CurrentCell.ReadOnly = false;
                                dgbtn.Value = "Pause";
                                dgv_campaign.CurrentCell.ReadOnly = true;

                                new Thread(() =>
                                {
                                    
                                    ResumedrunningCampaign(CampaignName);
                                }).Start();
                            }
                            else
                            {
                                MessageBox.Show("This campaign is with scheduling. so it can't be paused.");
                            }
                        }
                        #endregion
                    }

                }
            }

            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("9.dgv_campaign_CellClick :" + ex.Message);
            }
            //End if Colum Index ==3
        }



        void CampaignFollowerLogEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToCampaignLoggerListBox(eArgs.log);
            }
        }

        void logEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToCampaignLoggerListBox(eArgs.log);
            }
        }

        public void AddToCampaignLoggerListBox(string log)
        {
            try
            {
                if (campaignLogger.InvokeRequired)
                {
                    campaignLogger.Invoke(new MethodInvoker(delegate
                    {
                        campaignLogger.Items.Add(log);
                        campaignLogger.SelectedIndex = campaignLogger.Items.Count - 1;
                    }));
                }
                else
                {
                    campaignLogger.Items.Add(log);
                    campaignLogger.SelectedIndex = campaignLogger.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("10.AddToCampaignLoggerListBox :" + ex.Message);
            }
        }

        private void btn_UpdateCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                //Update follow campaign 
                string Result = Validations("Update");

                if (!string.IsNullOrEmpty(Result))
                {
                }
                else
                {
                    if (chklstbox_campaign.CheckedItems[0] == "Follow")
                    {
                        UpdateFollowerCampaign();
                        classes.cls_variables._DivideEqually = 0;
                        classes.cls_variables._DivideByGivenNo = 0;

                    }
                    else if (chklstbox_campaign.CheckedItems[0] == "Tweet")
                    {
                        UpdatetweetCampaign();
                        //Cls_StartTweetProcess._IsTweetProcessStart = true;
                    }
                    else if (chklstbox_campaign.CheckedItems[0] == "Retweet")
                    {

                        UpdateRetweetCampaign();
                    }
                    else if (chklstbox_campaign.CheckedItems[0] == "Reply")
                    {

                        UpdateReplyCampaign();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("11.btn_UpdateCampaign_Click :" + ex.Message);
            }
        }


        #region <--- Follow campaign functions --->

        public void SaveFollowSettings()
        {
            try
            {
                string query = "Insert into 'Campaign_follow'(CampaignName, AcFilePath, FollowingFilePath,DividEql, DivideByUser, NoOfUser, FastFollow, NoOfFollowPerAc, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module)"
                    + " values ('" + _CmpName + "','" + _AccountFilePath + "','" + classes.cls_variables._followersfilepath + "','" + classes.cls_variables._DivideEqually + "'"
                    + ",'" + classes.cls_variables._DivideByGivenNo + "','" + classes.cls_variables._NoofUsers + "','" + classes.cls_variables._IsFastFollow + "','" + classes.cls_variables._NoofFollowParAC + "','" + classes.cls_variables._IsScheduledDaily + "','" + classes.cls_variables._StartFrom + "','" + classes.cls_variables._EndTo + "'"
                    + ",'" + classes.cls_variables._DelayFrom + "', '" + classes.cls_variables._DelayTo + "', '" + Threads + "', 'Follow')";



                RepositoryClasses.cls_DbRepository.InsertQuery(query, "Campaign_follow");

                AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [  " + _CmpName + " is Saved. ]");

                //Reload all Saved campaign....
                LoadCampaign();

                //Re- start scheduled campaigns ... 
                //ScheduledTasks();
                if (classes.cls_variables._IsScheduledDaily == 1)
                {
                    ScheduledTasks_Updated(_CmpName);
                }

                ///Clear campaign 
                ClearCamapigns("Follow");
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("12.SaveFollowSettings :" + ex.Message);
            }
        }

        public void UpdateFollowerCampaign()
        {
            try
            {
                string query = "UPDATE Campaign_follow SET AcFilePath ='" + _AccountFilePath + "',FollowingFilePath='" + classes.cls_variables._followersfilepath + "'"
                        + " , DividEql= '" + classes.cls_variables._DivideEqually + "' , DivideByUser='" + classes.cls_variables._DivideByGivenNo + "',NoOfUser='" + classes.cls_variables._NoofUsers + "', FastFollow='" + classes.cls_variables._IsFastFollow + "'"
                        + ", NoOfFollowPerAc= '" + classes.cls_variables._NoofFollowParAC + "', ScheduledDaily='" + classes.cls_variables._IsScheduledDaily + "', StartTime='" + classes.cls_variables._StartFrom + "', EndTime='" + classes.cls_variables._EndTo + "'"
                        + ",DelayFrom='" + classes.cls_variables._DelayFrom + "',DelayTo='" + classes.cls_variables._DelayTo + "', Threads='" + Threads + "' WHERE CampaignName='" + _CmpName + "';";

                RepositoryClasses.cls_DbRepository.UpdateQuery(query, "Campaign_follow");

                foreach (DataGridViewRow dRow in dgv_campaign.Rows)
                {
                    string dgv_CampaignName = dRow.Cells["CampaignName"].Value.ToString();
                    //string dgv_Module = dRow.Cells["Module"].Value.ToString();

                    if (_CmpName == dgv_CampaignName)
                    {
                        //dRow.Cells["CampaignName"].Value = "HardCoded";
                        if (classes.cls_variables._IsScheduledDaily == 1)
                        {
                            dRow.Cells["BtnOnOff"].Value = (System.Drawing.Image)Properties.Resources.off;
                        }
                        else
                        {
                            dRow.Cells["BtnOnOff"].Value = (System.Drawing.Image)Properties.Resources.on;
                        }

                        //dgv_campaign.Refresh();
                    }
                }
                chklstbox_campaign.Invoke(new MethodInvoker(delegate
                {
                    chklstbox_campaign.SelectionMode = SelectionMode.One;
                }));
                ScheduledTasks_Updated(_CmpName);
                //LoadCampaign();

                AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [  " + _CmpName + " is Updated. ]");

                ///Clear campaign 
                ClearCamapigns("Follow");
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("13.UpdateFollowerCampaign :" + ex.Message);
            }
        }

        public void editFollowerCampaign(String CampaignName, String featurName)
        {
            try
            {
                DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

                if (drModelDetails.Count() == 0)
                {

                }

                //Get 1st row from arrey 
                DataRow DrCampaignDetails = drModelDetails[0];

                #region
                //Set all Details 
                //string AcFilePath = DrCampaignDetails.ItemArray[2].ToString();
                //string FollowFilePath = DrCampaignDetails.ItemArray[3].ToString();
                //bool divideEql = (Convert.ToInt32(DrCampaignDetails.ItemArray[4]) == 1) ? true : false;
                //bool dividebyUser = (Convert.ToInt32(DrCampaignDetails.ItemArray[5]) == 1) ? true : false;
                //int NoOfUsersDivid = Convert.ToInt32(DrCampaignDetails.ItemArray[6]);
                //bool IsfastFollow = (Convert.ToInt32(DrCampaignDetails.ItemArray[7]) == 1) ? true : false;
                //int NoOfFollowPerAc = Convert.ToInt32(DrCampaignDetails.ItemArray[8]);
                //bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[9]) == 1) ? true : false;
                //DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[10].ToString());
                //DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[11].ToString());
                //int DelayStar = Convert.ToInt32(DrCampaignDetails.ItemArray[12]);
                //int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[13]);
                //int Threads = Convert.ToInt32(DrCampaignDetails.ItemArray[14]);
                #endregion

                _CmpName = string.Empty;
                _CmpName = DrCampaignDetails.ItemArray[1].ToString();
                string AcFilePath = DrCampaignDetails.ItemArray[2].ToString();
                string FollowFilePath = DrCampaignDetails.ItemArray[3].ToString();
                bool divideEql = (Convert.ToInt32(DrCampaignDetails.ItemArray[5]) == 1) ? true : false;
                bool dividebyUser = (Convert.ToInt32(DrCampaignDetails.ItemArray[6]) == 1) ? true : false;
                int NoOfUsersDivid = Convert.ToInt32(DrCampaignDetails.ItemArray[7]);
                bool IsfastFollow = (Convert.ToInt32(DrCampaignDetails.ItemArray[8]) == 1) ? true : false;
                int NoOfFollowPerAc = Convert.ToInt32(DrCampaignDetails.ItemArray[9]);
                bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());
                int DelayStar = Convert.ToInt32(DrCampaignDetails.ItemArray[15]);
                int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[16]);
                Threads = Convert.ToInt32(DrCampaignDetails.ItemArray[17]);

                //Fill All deatials in control boxes ..
                chklstbox_campaign.Invoke(new MethodInvoker(delegate
                {
                    chklstbox_campaign.SetItemCheckState(0, CheckState.Checked);
                    chklstbox_campaign.SelectedIndex = 0;
                    chklstbox_campaign.SelectionMode = SelectionMode.None;
                }));

                txt_CampaignName.Invoke(new MethodInvoker(delegate
                {
                    txt_CampaignName.ReadOnly = true;
                    txt_CampaignName.Text = CampaignName;
                }));
                txt_accountfilepath.Invoke(new MethodInvoker(delegate
                {
                    txt_accountfilepath.Text = AcFilePath;
                }));

                // create control class OBJ 
                Control frmfollowusercontrols;

                // Get control from user page
                //and put all save value in boxes 
                if (!string.IsNullOrEmpty(FollowFilePath))
                {
                    #region
                    frmfollowusercontrols = new Control();
                    frmfollowusercontrols = followusercontrol.TopLevelControl.Controls.Find("txt_FollowUserIDsfilePath", true)[0];

                    if (frmfollowusercontrols != null)
                    {
                        if (frmfollowusercontrols.Name == "txt_FollowUserIDsfilePath")
                        {
                            frmfollowusercontrols.Invoke(new MethodInvoker(delegate
                            {
                                frmfollowusercontrols.Text = FollowFilePath;
                            }));
                        }
                    }
                    #endregion
                }

                if (divideEql == true || dividebyUser == true)
                {
                    #region
                    frmfollowusercontrols = new Control();
                    frmfollowusercontrols = followusercontrol.TopLevelControl.Controls.Find("chkUseDivide", true)[0];
                    if (frmfollowusercontrols != null)
                    {
                        if (frmfollowusercontrols.Name == "chkUseDivide")
                        {
                            frmfollowusercontrols.Invoke(new MethodInvoker(delegate
                            {
                                CheckBox chkBox = (CheckBox)frmfollowusercontrols;
                                chkBox.Checked = true;
                            }));
                        }
                    }
                    if (divideEql == true)
                    {
                        #region
                        frmfollowusercontrols = new Control();
                        frmfollowusercontrols = followusercontrol.TopLevelControl.Controls.Find("rdBtn_DivideEqually", true)[0];

                        if (frmfollowusercontrols != null)
                        {
                            if (frmfollowusercontrols.Name == "rdBtn_DivideEqually")
                            {
                                frmfollowusercontrols.Invoke(new MethodInvoker(delegate
                                {
                                    RadioButton rdobtn = (RadioButton)frmfollowusercontrols;
                                    rdobtn.Checked = true;
                                }));
                            }
                        }
                        #endregion
                    }
                    else if (dividebyUser == true)
                    {
                        #region
                        frmfollowusercontrols = new Control();
                        frmfollowusercontrols = followusercontrol.TopLevelControl.Controls.Find("rdBtn_DivideByGivenNo", true)[0];

                        if (frmfollowusercontrols != null)
                        {
                            if (frmfollowusercontrols.Name == "rdBtn_DivideByGivenNo")
                            {
                                frmfollowusercontrols.Invoke(new MethodInvoker(delegate
                                {
                                    RadioButton rdobtn = (RadioButton)frmfollowusercontrols;
                                    rdobtn.Checked = true;
                                }));

                                if (NoOfUsersDivid != 0)//
                                {
                                    frmfollowusercontrols = new Control();
                                    frmfollowusercontrols = followusercontrol.TopLevelControl.Controls.Find("txtScrapeNoOfUsers", true)[0];
                                    if (frmfollowusercontrols != null)
                                    {
                                        frmfollowusercontrols.Invoke(new MethodInvoker(delegate
                                        {
                                            frmfollowusercontrols.Text = NoOfUsersDivid.ToString();
                                        }));
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    frmfollowusercontrols = new Control();
                    frmfollowusercontrols = followusercontrol.TopLevelControl.Controls.Find("chkUseDivide", true)[0];
                    if (frmfollowusercontrols != null)
                    {
                        if (frmfollowusercontrols.Name == "chkUseDivide")
                        {
                            frmfollowusercontrols.Invoke(new MethodInvoker(delegate
                            {
                                CheckBox chkBox = (CheckBox)frmfollowusercontrols;
                                chkBox.Checked = false;
                            }));
                        }
                    }
                }

                if (NoOfFollowPerAc != 0)
                {
                    #region
                    frmfollowusercontrols = new Control();
                    frmfollowusercontrols = followusercontrol.TopLevelControl.Controls.Find("txt_followParAccount", true)[0];

                    if (frmfollowusercontrols != null)
                    {
                        if (frmfollowusercontrols.Name == "txt_followParAccount")
                        {
                            frmfollowusercontrols.Invoke(new MethodInvoker(delegate
                            {
                                frmfollowusercontrols.Text = NoOfFollowPerAc.ToString();
                            }));
                        }
                    }
                    #endregion
                }


                if (IsfastFollow == true)
                {
                    #region
                    frmfollowusercontrols = new Control();
                    frmfollowusercontrols = followusercontrol.TopLevelControl.Controls.Find("chk_FastFollow", true)[0];

                    if (frmfollowusercontrols != null)
                    {
                        if (frmfollowusercontrols.Name == "chk_FastFollow")
                        {
                            frmfollowusercontrols.Invoke(new MethodInvoker(delegate
                            {
                                CheckBox chkBox = (CheckBox)frmfollowusercontrols;
                                chkBox.Checked = true;
                            }));
                        }
                    }
                    #endregion
                }

                if (IsSchedulDaily == true)
                {
                    #region
                    chkbox_IsScheduledDaily.Invoke(new MethodInvoker(delegate
                    {
                        chkbox_IsScheduledDaily.Checked = true;
                    }));

                    dateTimePicker_Start.Invoke(new MethodInvoker(delegate
                    {
                        if (SchedulerStartTime < DateTime.Now)
                            dateTimePicker_Start.MinDate = SchedulerStartTime;
                        else
                            dateTimePicker_Start.MinDate = DateTime.Now;

                        dateTimePicker_Start.Value = SchedulerStartTime;
                    }));

                    dateTimePicker_End.Invoke(new MethodInvoker(delegate
                    {
                        if (SchedulerEndTime < DateTime.Now)
                            dateTimePicker_End.MinDate = SchedulerEndTime;
                        else
                            dateTimePicker_End.MinDate = DateTime.Now;

                        dateTimePicker_End.Value = (SchedulerEndTime);
                    }));

                    #endregion
                }

                if (DelayStar != 0)
                {
                    txt_DelayFrom.Invoke(new MethodInvoker(delegate { txt_DelayFrom.Text = DelayStar.ToString(); }));
                }

                if (DelayEnd != 0)
                {
                    txt_DelayTo.Invoke(new MethodInvoker(delegate { txt_DelayTo.Text = DelayEnd.ToString(); }));
                }
                if (Threads != 0)
                {
                    txtNoOfFollowThreads.Invoke(new MethodInvoker(delegate { txtNoOfFollowThreads.Text = Threads.ToString(); }));
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("14.editFollowerCampaign :" + ex.Message);
            }
        }



        public void StartFollowCampaignThreadPool(object parameters)
        {

            //Add List Of Working thread
            //we are using this list when we stop/abort running camp processes..
            try
            {
                Array paramsArray = new object[2];
                paramsArray = (Array)parameters;
                string CampaignName = (string)paramsArray.GetValue(0);
                string featurName = (string)paramsArray.GetValue(1);
                try
                {
                    MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName, Thread.CurrentThread);
                }
                catch (Exception ex)
                {
                    ErrorLogger.AddToErrorLogText("15.StartFollowCampaignThreadPool :" + ex.Message);
                }


                //Get Detals from Data Set table by Campaign Name
                DataRow[] drModelDetails;
                lock (this)
                {
                    //modified date [11-02-15]
                    try
                    {
                        string query = "Select indx, CampaignName, AcFilePath, FollowingFilePath, '0' as Image,DividEql, DivideByUser, NoOfUser, FastFollow, NoOfFollowPerAc, '0' as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'0' as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath,'0' as IsDuplicate, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_follow "
                 + " UNION ALL "
                 + "Select indx, CampaignName, AcFilePath, TweetMsgFilePath,TweetImageFolderPath as Image,DuplicateMsg, AllTweetParAc as DivideByUser, HashTag as NoOfUser, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc as TweetPac,TweetWithImage as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'' as IsUniqueMessage, TweetUploadUserFilePath, IsUploadUserFilePath, '0' as IsDuplicate,IsTweetMentionUserScrapedList,TweetMentionUserNameScrapedList,IsTweetFollowersScrapedList,IsTweetFollowingScrapedList,NoOfTweetMentionUserScrapedList,NoOfScrapedUserScrapedList from Campaign_tweet "
                 + " UNION ALL "
                 + " SELECT  indx, CampaignName, AcFilePath, Keyword, '0' as Image,IsUsername,'0' as DivideByUser,'0' as NoOfUser, RetweetParDay, NoofRetweetParDay, NoofRetweetParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, '0' as IsDuplicate ,'0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_retweet "
                 + " UNION ALL "
                 + " SELECT  indx, CampaignName, AcFilePath, ReplyFilePath, Keyword as Image,  IsUsername,'0' as DivideByUser,'0' as NoOfUser, ReplyParDay, NoofReplyParDay, NoofReplyParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, IsDuplicateMessage, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_reply ";

                        CompaignsDataSet = RepositoryClasses.cls_DbRepository.selectQuery(query, "Union");
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.AddToErrorLogText("16.StartFollowCampaignThreadPool :" + ex.Message);
                    }

                    drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

                }

                if (drModelDetails.Count() == 0)
                {
                    return;
                }


                //Get 1st row from arrey 
                DataRow DrCampaignDetails = drModelDetails[0];
                bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());


                int startTime = SchedulerStartTime.Hour * 60 + SchedulerStartTime.Minute;
                int sysTime = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                int endTime = SchedulerEndTime.Hour * 60 + SchedulerEndTime.Minute;

                //Create Object of tweetProcess class 

                classes.Cls_FollowStart ObjFollowProcess = new Cls_FollowStart();

                ObjFollowProcess.CampaignFollowLogEvents.addToLogger += new EventHandler(logEvents_addToLogger);

                if (IsSchedulDaily)
                {
                    if (_tempcount == 0)
                    {
                        //MessageBox.Show(CampaignName + " task is scheduled. Task start timing  :- " + SchedulerStartTime);
                    }
                    _tempcount++;
                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ " + CampaignName + " task is scheduled. Task start timing :- " + SchedulerStartTime + " ]");

                    while (true)
                    {
                        if ((SchedulerStartTime.Hour) == (DateTime.Now.Hour) && SchedulerStartTime.Minute == (DateTime.Now.Minute))
                        //if (startTime <= sysTime && endTime >= sysTime && (Cls_FollowStart._IsFollowProcessStart == true))
                        {

                            //Cls_FollowStart._IsFollowProcessStart = false;  temprory commented.
                            Cls_FollowStart.campName = CampaignName;
                            ObjFollowProcess.StartProcess(CampaignName, featurName, DrCampaignDetails);
                            break;

                        }
                        Thread.Sleep(15 * 1000);

                    }
                  

                }
                else
                {

                    //Start Process
                    ObjFollowProcess.StartProcess(CampaignName, featurName, DrCampaignDetails);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("17.StartFollowCampaignThreadPool :" + ex.Message);
            }
        }




        static int _tempcount = 0;
        public void StartFollowCampaign(String CampaignName, String featurName)
        {
            //Add List Of Working thread
            //we are using this list when we stop/abort running camp processes..
            try
            {
                try
                {
                    MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName, Thread.CurrentThread);
                }
                catch (Exception ex)
                {
                    ErrorLogger.AddToErrorLogText("18.StartFollowCampaign :" + ex.Message);
                }

                //Get Detals from Data Set table by Campaign Name
                DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

                if (drModelDetails.Count() == 0)
                {
                    return;
                }

                //Get 1st row from arrey 
                DataRow DrCampaignDetails = drModelDetails[0];
                bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());

                int startTime = SchedulerStartTime.Hour * 60 + SchedulerStartTime.Minute;
                int sysTime = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                int endTime = SchedulerEndTime.Hour * 60 + SchedulerEndTime.Minute;
                //Create Object of tweetProcess class 

                classes.Cls_FollowStart ObjFollowProcess = new Cls_FollowStart();

                ObjFollowProcess.CampaignFollowLogEvents.addToLogger += new EventHandler(logEvents_addToLogger);

                if (IsSchedulDaily)
                {
                    if (_tempcount == 0)
                    {
                        //MessageBox.Show(CampaignName + " task is scheduled. Task start timing  :- " + SchedulerStartTime);
                    }
                    _tempcount++;
                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [  " + CampaignName + " task is scheduled. Task start timing :- " + SchedulerStartTime + " ]");
                    while (true)
                    {
                        if (SchedulerStartTime.Hour == (DateTime.Now.Hour) && SchedulerStartTime.Minute == (DateTime.Now.Minute) && (Cls_FollowStart._IsFollowProcessStart == true))
                        //if (startTime <= sysTime && endTime >= sysTime && (Cls_FollowStart._IsFollowProcessStart == true))
                        {
                            //Cls_FollowStart._IsFollowProcessStart = false;   //temprory commented.
                            Cls_FollowStart.campName = CampaignName;
                            ObjFollowProcess.StartProcess(CampaignName, featurName, DrCampaignDetails);
                            break;
                        }
                        Thread.Sleep(15 * 1000);
                    }
                    
                }
                else
                {
                    ObjFollowProcess.StartProcess(CampaignName, featurName, DrCampaignDetails);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("19.StartFollowCampaign :" + ex.Message);
            }

        }

        #endregion


        #region <--- Tweet campaign functions --->

        public void SaveTweetSettings()
        {
            try
            {
                //string query = "Insert into 'Campaign_tweet'(CampaignName, AcFilePath, TweetMsgFilePath, DuplicateMsg, AllTweetParAc, HashTag, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module)"
                //    + " values ('" + _CmpName + "','" + _AccountFilePath + "','" + classes.cls_variables._TweetMSGfilepath + "','" + classes.cls_variables._IsDuplicatMSG + "'"
                //    + ",'" + classes.cls_variables._IsAllTweetParAC + "','" + classes.cls_variables._IsHashTage + "','" + classes.cls_variables._IsTweetParDay + "','" + classes.cls_variables._MaxNoOfTweetsParDay + "','" + classes.cls_variables._NoOfTweetsParAC + "',"
                //    + "'" + +classes.cls_variables._IsScheduledDaily + "','" + classes.cls_variables._StartFrom + "','" + classes.cls_variables._EndTo + "','" + classes.cls_variables._DelayFrom + "', '" + classes.cls_variables._DelayTo + "', '" + Threads + "', 'Tweet')";


                string query = "Insert into 'Campaign_tweet'(CampaignName, AcFilePath, TweetMsgFilePath, TweetImageFolderPath,TweetUploadUserFilePath,IsUploadUserFilePath, DuplicateMsg, AllTweetParAc, HashTag, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc, TweetWithImage, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,IsTweetMentionUserScrapedList,TweetMentionUserNameScrapedList,IsTweetFollowersScrapedList,IsTweetFollowingScrapedList,NoOfTweetMentionUserScrapedList,NoOfScrapedUserScrapedList)"
                   + " values ('" + _CmpName + "','" + _AccountFilePath + "','" + classes.cls_variables._TweetMSGfilepath + "','" + classes.cls_variables._TweetImageFolderPath + "','" + classes.cls_variables._TweetUploadUserList + "','" + classes.cls_variables._IsTweetUploadUserList + "','" + classes.cls_variables._IsDuplicatMSG + "'"
                   + ",'" + classes.cls_variables._IsAllTweetParAC + "','" + classes.cls_variables._IsHashTage + "','" + classes.cls_variables._IsTweetParDay + "','" + classes.cls_variables._MaxNoOfTweetsParDay + "','" + classes.cls_variables._NoOfTweetsParAC + "',"
                   + "'" + classes.cls_variables._Istweetwithimage + "','" + +classes.cls_variables._IsScheduledDaily + "','" + classes.cls_variables._StartFrom + "','" + classes.cls_variables._EndTo + "','" + classes.cls_variables._DelayFrom + "', '" + classes.cls_variables._DelayTo + "', '" + Threads + "', 'Tweet','" + classes.cls_variables._IsTweetMentionUserScrapedList + "','" + classes.cls_variables._TweetMentionUserName + "','" + classes.cls_variables._IsTweetFollowerList + "','" + classes.cls_variables._IsTweetFollowingList + "','" + classes.cls_variables._NoOfTweetMentionUserName + "','" + classes.cls_variables._NoOfTweetScrapedUser + "')";


                RepositoryClasses.cls_DbRepository.InsertQuery(query, "Campaign_tweet");

                AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [  " + _CmpName + " is Save. ]");

                //Reload all Saved campaign....
                LoadCampaign();

                //Re- start scheduled campaigns ... 
                if (classes.cls_variables._IsScheduledDaily == 1)
                {
                    ScheduledTasks_Updated(_CmpName);
                }

                ///Clear campaign 
                ClearCamapigns("Tweet");

                classes.cls_variables._TweetMSGfilepath = string.Empty;
                //classes.cls_variables._TweetImageFolderPath = string.Empty;
                //classes.cls_variables._TweetUploadUserList = string.Empty;
                classes.cls_variables._IsDuplicatMSG = 0;
                classes.cls_variables._IsAllTweetParAC = 0;
                classes.cls_variables._IsHashTage = 0;
                classes.cls_variables._IsTweetParDay = 0;
                classes.cls_variables._MaxNoOfTweetsParDay = 10;
                classes.cls_variables._NoOfTweetsParAC = 10;
                classes.cls_variables._Istweetwithimage = 0;
                classes.cls_variables._IsTweetUploadUserList = 0;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("20.SaveTweetSettings :" + ex.Message);
            }
        }

        public void UpdatetweetCampaign()
        {
            try
            {
                string query = "UPDATE Campaign_tweet SET AcFilePath ='" + _AccountFilePath + "',TweetMsgFilePath='" + classes.cls_variables._TweetMSGfilepath + "',TweetImageFolderPath='" + classes.cls_variables._TweetImageFolderPath + "'"
                        + " , DuplicateMsg= '" + classes.cls_variables._IsDuplicatMSG + "' , AllTweetParAc='" + classes.cls_variables._IsAllTweetParAC + "',HashTag='" + classes.cls_variables._IsHashTage + "', TweetParDay='" + classes.cls_variables._IsTweetParDay + "'"
                        + ", NoOfTweetParDay='" + classes.cls_variables._MaxNoOfTweetsParDay + "', NoOfTweetPerAc= '" + classes.cls_variables._NoOfTweetsParAC + "', TweetWithImage= '" + classes.cls_variables._Istweetwithimage + "', ScheduledDaily='" + classes.cls_variables._IsScheduledDaily + "',"
                        + " StartTime='" + classes.cls_variables._StartFrom + "', EndTime='" + classes.cls_variables._EndTo + "',DelayFrom='" + classes.cls_variables._DelayFrom + "',DelayTo='" + classes.cls_variables._DelayTo + "', Threads='" + Threads + "'"
                        + " ,TweetUploadUserFilePath = '" + classes.cls_variables._TweetUploadUserList + "' , IsUploadUserFilePath = '" + classes.cls_variables._IsTweetUploadUserList + "', IsTweetMentionUserScrapedList='" + classes.cls_variables._IsTweetMentionUserScrapedList + "',TweetMentionUserNameScrapedList='" + classes.cls_variables._TweetMentionUserName + "',IsTweetFollowersScrapedList='" + classes.cls_variables._IsTweetFollowerList + "',IsTweetFollowingScrapedList='" + classes.cls_variables._IsTweetFollowingList + "',NoOfTweetMentionUserScrapedList='" + classes.cls_variables._NoOfTweetMentionUserName + "',NoOfScrapedUserScrapedList='" + classes.cls_variables._NoOfTweetScrapedUser + "' where  CampaignName='" + _CmpName + "';";

                RepositoryClasses.cls_DbRepository.UpdateQuery(query, "Campaign_tweet");

                //  LoadCampaign();


                //LoadUpdatedCampaign(_CmpName);
                foreach (DataGridViewRow dRow in dgv_campaign.Rows)
                {
                    string dgv_CampaignName = dRow.Cells["CampaignName"].Value.ToString();
                    //string dgv_Module = dRow.Cells["Module"].Value.ToString();

                    if (_CmpName == dgv_CampaignName)
                    {
                        //dRow.Cells["CampaignName"].Value = "HardCoded";
                        if (classes.cls_variables._IsScheduledDaily == 1)
                        {
                            dRow.Cells["BtnOnOff"].Value = (System.Drawing.Image)Properties.Resources.off;
                        }
                        else
                        {
                            dRow.Cells["BtnOnOff"].Value = (System.Drawing.Image)Properties.Resources.on;
                        }

                        //dgv_campaign.Refresh();
                    }
                }
                chklstbox_campaign.Invoke(new MethodInvoker(delegate
                {
                    chklstbox_campaign.SelectionMode = SelectionMode.One;
                }));
                AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ " + _CmpName + " is Updated. ]");

                // ScheduledTasks();
                ScheduledTasks_Updated(_CmpName);

                ///Clear campaign 
                ClearCamapigns("Tweet");

                classes.cls_variables._TweetMSGfilepath = string.Empty;
                //classes.cls_variables._TweetImageFolderPath = string.Empty;
                //classes.cls_variables._TweetUploadUserList = string.Empty;
                classes.cls_variables._IsDuplicatMSG = 0;
                classes.cls_variables._IsAllTweetParAC = 0;
                classes.cls_variables._IsHashTage = 0;
                classes.cls_variables._IsTweetParDay = 0;
                classes.cls_variables._MaxNoOfTweetsParDay = 10;
                classes.cls_variables._NoOfTweetsParAC = 10;
                classes.cls_variables._Istweetwithimage = 0;
                classes.cls_variables._IsTweetUploadUserList = 0;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("21.UpdatetweetCampaign :" + ex.Message);
            }
        }

        public void startTweet(String CampaignName, String featurName)
        {

            //Add List Of Working thread
            //we are using this list when we stop/abort running camp processes..
            try
            {
                try
                {
                    MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName, Thread.CurrentThread);
                }
                catch (Exception ex)
                {
                    ErrorLogger.AddToErrorLogText("22.startTweet :" + ex.Message);
                }
                string query = "Select indx, CampaignName, AcFilePath, FollowingFilePath, '0' as Image,DividEql, DivideByUser, NoOfUser, FastFollow, NoOfFollowPerAc, '0' as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'0' as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath,'0' as IsDuplicate, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_follow "
                 + " UNION ALL "
                 + "Select indx, CampaignName, AcFilePath, TweetMsgFilePath,TweetImageFolderPath as Image,DuplicateMsg, AllTweetParAc as DivideByUser, HashTag as NoOfUser, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc as TweetPac,TweetWithImage as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'' as IsUniqueMessage, TweetUploadUserFilePath, IsUploadUserFilePath, '0' as IsDuplicate,IsTweetMentionUserScrapedList,TweetMentionUserNameScrapedList,IsTweetFollowersScrapedList,IsTweetFollowingScrapedList,NoOfTweetMentionUserScrapedList,NoOfScrapedUserScrapedList from Campaign_tweet "
                 + " UNION ALL "
                 + " SELECT  indx, CampaignName, AcFilePath, Keyword, '0' as Image,IsUsername,'0' as DivideByUser,'0' as NoOfUser, RetweetParDay, NoofRetweetParDay, NoofRetweetParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, '0' as IsDuplicate ,'0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_retweet "
                 + " UNION ALL "
                 + " SELECT  indx, CampaignName, AcFilePath, ReplyFilePath, Keyword as Image,  IsUsername,'0' as DivideByUser,'0' as NoOfUser, ReplyParDay, NoofReplyParDay, NoofReplyParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, IsDuplicateMessage, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_reply ";

                CompaignsDataSet = RepositoryClasses.cls_DbRepository.selectQuery(query, "Union");
                //Get Detals from Data Set table by Campaign Name
                DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

                if (drModelDetails.Count() == 0)
                {
                    return;
                }

                //Get 1st row from arrey 
                DataRow DrCampaignDetails = drModelDetails[0];
                bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());

                //Create Object of tweetProcess class 

                classes.Cls_StartTweetProcess ObjTweetProcess = new Cls_StartTweetProcess();

                ObjTweetProcess.CampaignTweetLogEvents.addToLogger += new EventHandler(logEvents_addToLogger);

                if (IsSchedulDaily)
                {
                    if (_tempcount == 0)
                    {
                        //MessageBox.Show(CampaignName + " task is scheduled. Task start timing  :- " + SchedulerStartTime);
                    }
                    _tempcount++;
                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ " + CampaignName + " task is scheduled. Task start timing :- " + SchedulerStartTime + " ]");

                    while (true)
                    {
                        if ((SchedulerStartTime.Hour) == (DateTime.Now.Hour) && SchedulerStartTime.Minute == (DateTime.Now.Minute) && (ObjTweetProcess._IsTweetProcessStart == true))
                        {
                            Cls_StartTweetProcess.campName = CampaignName;
                            //ObjTweetProcess._IsTweetProcessStart = false;
                            ObjTweetProcess._IsTweetProcessStart = false;
                            ObjTweetProcess.startTweeting(CampaignName, featurName, DrCampaignDetails);
                            break;
                        }
                        Thread.Sleep(15 * 1000);
                    }
                    

                }
                else
                {

                    //Start Process
                    ObjTweetProcess.startTweeting(CampaignName, featurName, DrCampaignDetails);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("23.startTweet :" + ex.Message);
            }
        }



        public void startTweetusingthreadPool(object parameters)
        {

            //Add List Of Working thread
            //we are using this list when we stop/abort running camp processes..
            try
            {
                Array paramsArray = new object[2];
                paramsArray = (Array)parameters;
                string CampaignName = (string)paramsArray.GetValue(0);
                string featurName = (string)paramsArray.GetValue(1);
                try
                {
                    MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName, Thread.CurrentThread);
                }
                catch (Exception ex)
                {
                    ErrorLogger.AddToErrorLogText("24.startTweetusingthreadPool :" + ex.Message);
                }


                //Get Detals from Data Set table by Campaign Name
                DataRow[] drModelDetails;
                lock (this)
                {
                    //modified date [11-02-15]
                    try
                    {
                        string query = "Select indx, CampaignName, AcFilePath, FollowingFilePath, '0' as Image,DividEql, DivideByUser, NoOfUser, FastFollow, NoOfFollowPerAc, '0' as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'0' as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath,'0' as IsDuplicate, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_follow "
                 + " UNION ALL "
                 + "Select indx, CampaignName, AcFilePath, TweetMsgFilePath,TweetImageFolderPath as Image,DuplicateMsg, AllTweetParAc as DivideByUser, HashTag as NoOfUser, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc as TweetPac,TweetWithImage as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'' as IsUniqueMessage, TweetUploadUserFilePath, IsUploadUserFilePath, '0' as IsDuplicate,IsTweetMentionUserScrapedList,TweetMentionUserNameScrapedList,IsTweetFollowersScrapedList,IsTweetFollowingScrapedList,NoOfTweetMentionUserScrapedList,NoOfScrapedUserScrapedList from Campaign_tweet "
                 + " UNION ALL "
                 + " SELECT  indx, CampaignName, AcFilePath, Keyword, '0' as Image,IsUsername,'0' as DivideByUser,'0' as NoOfUser, RetweetParDay, NoofRetweetParDay, NoofRetweetParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, '0' as IsDuplicate ,'0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_retweet "
                 + " UNION ALL "
                 + " SELECT  indx, CampaignName, AcFilePath, ReplyFilePath, Keyword as Image,  IsUsername,'0' as DivideByUser,'0' as NoOfUser, ReplyParDay, NoofReplyParDay, NoofReplyParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, IsDuplicateMessage, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_reply ";

                        CompaignsDataSet = RepositoryClasses.cls_DbRepository.selectQuery(query, "Union");
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.AddToErrorLogText("25.startTweetusingthreadPool :" + ex.Message);
                    }

                    drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

                }

                if (drModelDetails.Count() == 0)
                {
                    return;
                }

                //string selectQuery = "Select * from Campaign_tweet where CampaignName = "+ CampaignName;
                //DataSet ds = MixedCampaignManager.RepositoryClasses.cls_DbRepository.selectQuery(selectQuery, "Campaign_tweet");

                //bool IsSchedulDaily = (Convert.ToInt32(ds.Tables["Campaign_tweet"].Rows[0]["ScheduledDaily"].ToString()) == 1) ? true : false;
                //DateTime SchedulerStartTime = Convert.ToDateTime(ds.Tables["Campaign_tweet"].Rows[0]["StartTime"].ToString());
                //DateTime SchedulerEndTime = Convert.ToDateTime(ds.Tables["Campaign_tweet"].Rows[0]["EndTime"].ToString());

                //Get 1st row from arrey 
                DataRow DrCampaignDetails = drModelDetails[0];
                bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());

                int startTime = SchedulerStartTime.Hour * 60 + SchedulerStartTime.Minute;
                int sysTime = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                int endTime = SchedulerEndTime.Hour * 60 + SchedulerEndTime.Minute;


                //Create Object of tweetProcess class 

                classes.Cls_StartTweetProcess ObjTweetProcess = new Cls_StartTweetProcess();

                ObjTweetProcess.CampaignTweetLogEvents.addToLogger += new EventHandler(logEvents_addToLogger);

                if (IsSchedulDaily)
                {
                    if (_tempcount == 0)
                    {
                        //MessageBox.Show(CampaignName + " task is scheduled. Task start timing  :- " + SchedulerStartTime);
                    }
                    _tempcount++;
                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ " + CampaignName + " task is scheduled. Task start timing :- " + SchedulerStartTime + " ]");

                    while (true)
                    {
                        if ((SchedulerStartTime.Hour) == (DateTime.Now.Hour) && SchedulerStartTime.Minute == (DateTime.Now.Minute))
                        //if (startTime <= sysTime && endTime >= sysTime && (Cls_FollowStart._IsFollowProcessStart == true))
                        {

                            Cls_StartTweetProcess.campName = CampaignName;
                            //ObjTweetProcess._IsTweetProcessStart = false;
                            ObjTweetProcess._IsTweetProcessStart = false;
                            ObjTweetProcess.startTweeting(CampaignName, featurName, DrCampaignDetails);
                            break;

                        }
                        Thread.Sleep(15 * 1000);
                    }
                    

                }
                else
                {

                    //Start Process
                    ObjTweetProcess.startTweeting(CampaignName, featurName, DrCampaignDetails);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("25.startTweetusingthreadPool :" + ex.Message);
            }
        }




        public void startTweetusingthreadPool_AfterStop(object parameters)
        {

            //Add List Of Working thread
            //we are using this list when we stop/abort running camp processes..
            try
            {
                Array paramsArray = new object[2];
                paramsArray = (Array)parameters;
                string CampaignName = (string)paramsArray.GetValue(0);
                string featurName = (string)paramsArray.GetValue(1);
                try
                {
                    MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName, Thread.CurrentThread);
                }
                catch (Exception ex)
                {
                    ErrorLogger.AddToErrorLogText("26.startTweetusingthreadPool_AfterStop :" + ex.Message);
                }


                //Get Detals from Data Set table by Campaign Name
                DataRow[] drModelDetails;
                lock (this)
                {
                    drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

                }

                if (drModelDetails.Count() == 0)
                {
                    return;
                }

                //Get 1st row from arrey 
                DataRow DrCampaignDetails = drModelDetails[0];
                bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());

                //Create Object of tweetProcess class 

                classes.Cls_StartTweetProcess ObjTweetProcess = new Cls_StartTweetProcess();

                ObjTweetProcess.CampaignTweetLogEvents.addToLogger += new EventHandler(logEvents_addToLogger);

                if (IsSchedulDaily)
                {
                    if (_tempcount == 0)
                    {
                        //MessageBox.Show(CampaignName + " task is scheduled. Task start timing  :- " + SchedulerStartTime);
                    }
                    _tempcount++;
                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ " + CampaignName + " task is scheduled. Task start timing :- " + SchedulerStartTime + " ]");

                    foreach (DataGridViewRow dRow in dgv_campaign.Rows)
                    {
                        string dgv_CampaignName = dRow.Cells["CampaignName"].Value.ToString();
                        //string dgv_Module = dRow.Cells["Module"].Value.ToString();

                        if (CampaignName == dgv_CampaignName)
                        {
                            //dRow.Cells["CampaignName"].Value = "HardCoded";
                            //if (classes.cls_variables._IsScheduledDaily == 1)
                            {
                                dgv_campaign.Invoke(new MethodInvoker(delegate
                                {
                                    dRow.Cells["BtnOnOff"].Value = (System.Drawing.Image)Properties.Resources.on;
                                }));
                            }


                            //dgv_campaign.Refresh();
                        }
                    }

                    while (true)
                    {
                        //Update dataset
                        lock (this)
                        {
                            //modified date [11-02-15]
                            try
                            {
                                string query = "Select indx, CampaignName, AcFilePath, FollowingFilePath, '0' as Image,DividEql, DivideByUser, NoOfUser, FastFollow, NoOfFollowPerAc, '0' as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'0' as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath,'0' as IsDuplicate, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_follow "
                         + " UNION ALL "
                         + "Select indx, CampaignName, AcFilePath, TweetMsgFilePath,TweetImageFolderPath as Image,DuplicateMsg, AllTweetParAc as DivideByUser, HashTag as NoOfUser, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc as TweetPac,TweetWithImage as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'' as IsUniqueMessage, TweetUploadUserFilePath, IsUploadUserFilePath, '0' as IsDuplicate,IsTweetMentionUserScrapedList,TweetMentionUserNameScrapedList,IsTweetFollowersScrapedList,IsTweetFollowingScrapedList,NoOfTweetMentionUserScrapedList,NoOfScrapedUserScrapedList from Campaign_tweet "
                         + " UNION ALL "
                         + " SELECT  indx, CampaignName, AcFilePath, Keyword, '0' as Image,IsUsername,'0' as DivideByUser,'0' as NoOfUser, RetweetParDay, NoofRetweetParDay, NoofRetweetParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, '0' as IsDuplicate ,'0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_retweet "
                         + " UNION ALL "
                         + " SELECT  indx, CampaignName, AcFilePath, ReplyFilePath, Keyword as Image,  IsUsername,'0' as DivideByUser,'0' as NoOfUser, ReplyParDay, NoofReplyParDay, NoofReplyParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, IsDuplicateMessage, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_reply ";

                                CompaignsDataSet = RepositoryClasses.cls_DbRepository.selectQuery(query, "Union");
                            }
                            catch (Exception)
                            {

                            }

                            drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");
                            if (drModelDetails.Count() == 0)
                            {
                                return;
                            }
                        }

                        DrCampaignDetails = drModelDetails[0];
                        IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                        SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                        SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());

                        int startTime = SchedulerStartTime.Hour * 60 + SchedulerStartTime.Minute;
                        int sysTime = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                        int endTime = SchedulerEndTime.Hour * 60 + SchedulerEndTime.Minute;

                        if ((SchedulerStartTime.Hour) == (DateTime.Now.Hour) && SchedulerStartTime.Minute == (DateTime.Now.Minute))
                        //if (startTime <= sysTime && endTime >= sysTime && (Cls_FollowStart._IsFollowProcessStart == true))
                        {


                            foreach (DataGridViewRow dRow in dgv_campaign.Rows)
                            {
                                string dgv_CampaignName = dRow.Cells["CampaignName"].Value.ToString();
                                //string dgv_Module = dRow.Cells["Module"].Value.ToString();

                                if (CampaignName == dgv_CampaignName)
                                {
                                    //dRow.Cells["CampaignName"].Value = "HardCoded";
                                    //if (classes.cls_variables._IsScheduledDaily == 1)
                                    {
                                        dgv_campaign.Invoke(new MethodInvoker(delegate
                                        {
                                            dRow.Cells["BtnOnOff"].Value = (System.Drawing.Image)Properties.Resources.off;
                                        }));
                                    }


                                    //dgv_campaign.Refresh();
                                }
                            }



                            Cls_StartTweetProcess.campName = CampaignName;
                            //ObjTweetProcess._IsTweetProcessStart = false;
                            ObjTweetProcess._IsTweetProcessStart = false;
                            ObjTweetProcess.startTweeting(CampaignName, featurName, DrCampaignDetails);
                            break;

                        }
                        Thread.Sleep(15 * 1000);
                    }
                    

                }
                else
                {

                    //Start Process
                    ObjTweetProcess.startTweeting(CampaignName, featurName, DrCampaignDetails);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("27.startTweetusingthreadPool_AfterStop :" + ex.Message);
            }
        }


        public void startFollowusingthreadPool_AfterStop(object parameters)
        {

            //Add List Of Working thread
            //we are using this list when we stop/abort running camp processes..
            try
            {
                Array paramsArray = new object[2];
                paramsArray = (Array)parameters;
                string CampaignName = (string)paramsArray.GetValue(0);
                string featurName = (string)paramsArray.GetValue(1);
                try
                {
                    MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName, Thread.CurrentThread);
                }
                catch (Exception ex)
                {
                    ErrorLogger.AddToErrorLogText("28.startFollowusingthreadPool_AfterStop :" + ex.Message);
                }


                //Get Detals from Data Set table by Campaign Name
                DataRow[] drModelDetails;
                lock (this)
                {
                    drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

                }

                if (drModelDetails.Count() == 0)
                {
                    return;
                }

                //Get 1st row from arrey 
                DataRow DrCampaignDetails = drModelDetails[0];
                bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());

                //Create Object of tweetProcess class 

                classes.Cls_FollowStart ObjFollowProcess = new Cls_FollowStart();

                ObjFollowProcess.CampaignFollowLogEvents.addToLogger += new EventHandler(logEvents_addToLogger);

                if (IsSchedulDaily)
                {
                    if (_tempcount == 0)
                    {
                        //MessageBox.Show(CampaignName + " task is scheduled. Task start timing  :- " + SchedulerStartTime);
                    }
                    _tempcount++;
                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ " + CampaignName + " task is scheduled. Task start timing :- " + SchedulerStartTime + " ]");

                    foreach (DataGridViewRow dRow in dgv_campaign.Rows)
                    {
                        string dgv_CampaignName = dRow.Cells["CampaignName"].Value.ToString();
                        //string dgv_Module = dRow.Cells["Module"].Value.ToString();

                        if (CampaignName == dgv_CampaignName)
                        {
                            //dRow.Cells["CampaignName"].Value = "HardCoded";
                            //if (classes.cls_variables._IsScheduledDaily == 1)
                            {
                                dgv_campaign.Invoke(new MethodInvoker(delegate
                                {
                                    dRow.Cells["BtnOnOff"].Value = (System.Drawing.Image)Properties.Resources.on;
                                }));
                            }


                            //dgv_campaign.Refresh();
                        }
                    }

                    while (true)
                    {
                        //Update dataset
                        lock (this)
                        {
                            //modified date [11-02-15]
                            try
                            {
                                string query = "Select indx, CampaignName, AcFilePath, FollowingFilePath, '0' as Image,DividEql, DivideByUser, NoOfUser, FastFollow, NoOfFollowPerAc, '0' as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'0' as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath,'0' as IsDuplicate, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_follow "
                         + " UNION ALL "
                         + "Select indx, CampaignName, AcFilePath, TweetMsgFilePath,TweetImageFolderPath as Image,DuplicateMsg, AllTweetParAc as DivideByUser, HashTag as NoOfUser, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc as TweetPac,TweetWithImage as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'' as IsUniqueMessage, TweetUploadUserFilePath, IsUploadUserFilePath, '0' as IsDuplicate,IsTweetMentionUserScrapedList,TweetMentionUserNameScrapedList,IsTweetFollowersScrapedList,IsTweetFollowingScrapedList,NoOfTweetMentionUserScrapedList,NoOfScrapedUserScrapedList from Campaign_tweet "
                         + " UNION ALL "
                         + " SELECT  indx, CampaignName, AcFilePath, Keyword, '0' as Image,IsUsername,'0' as DivideByUser,'0' as NoOfUser, RetweetParDay, NoofRetweetParDay, NoofRetweetParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, '0' as IsDuplicate ,'0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_retweet "
                         + " UNION ALL "
                         + " SELECT  indx, CampaignName, AcFilePath, ReplyFilePath, Keyword as Image,  IsUsername,'0' as DivideByUser,'0' as NoOfUser, ReplyParDay, NoofReplyParDay, NoofReplyParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, IsDuplicateMessage, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_reply ";

                                CompaignsDataSet = RepositoryClasses.cls_DbRepository.selectQuery(query, "Union");
                            }
                            catch (Exception ex)
                            {
                                ErrorLogger.AddToErrorLogText("28.startFollowusingthreadPool_AfterStop :" + ex.Message);
                            }

                            drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");
                            if (drModelDetails.Count() == 0)
                            {
                                return;
                            }
                        }


                        try
                        {
                            DrCampaignDetails = drModelDetails[0];
                            IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                            SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                            SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());


                            int startTime = SchedulerStartTime.Hour * 60 + SchedulerStartTime.Minute;
                            int sysTime = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                            int endTime = SchedulerEndTime.Hour * 60 + SchedulerEndTime.Minute;

                            if ((SchedulerStartTime.Hour) == (DateTime.Now.Hour) && SchedulerStartTime.Minute == (DateTime.Now.Minute))
                            //if (startTime <= sysTime && endTime >= sysTime && (Cls_FollowStart._IsFollowProcessStart == true))
                            {


                                foreach (DataGridViewRow dRow in dgv_campaign.Rows)
                                {
                                    string dgv_CampaignName = dRow.Cells["CampaignName"].Value.ToString();
                                    //string dgv_Module = dRow.Cells["Module"].Value.ToString();

                                    if (CampaignName == dgv_CampaignName)
                                    {
                                        //dRow.Cells["CampaignName"].Value = "HardCoded";
                                        //if (classes.cls_variables._IsScheduledDaily == 1)
                                        {
                                            dgv_campaign.Invoke(new MethodInvoker(delegate
                                            {
                                                dRow.Cells["BtnOnOff"].Value = (System.Drawing.Image)Properties.Resources.off;
                                            }));
                                        }


                                        //dgv_campaign.Refresh();
                                    }
                                }



                                //Cls_FollowStart._IsFollowProcessStart = false;  temprory commented.
                                Cls_FollowStart.campName = CampaignName;
                                ObjFollowProcess.StartProcess(CampaignName, featurName, DrCampaignDetails);
                                break;

                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLogger.AddToErrorLogText("128.startprocessfollow :" + ex.Message);
                        }
                        Thread.Sleep(15 * 1000);
                    }
                    

                }
                else
                {

                    //Start Process
                    ObjFollowProcess.StartProcess(CampaignName, featurName, DrCampaignDetails);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("29.startFollowusingthreadPool_AfterStop :" + ex.Message);
            }
        }


        
        public void editingTweetCampaign(String CampaignName, String featurName)
        {
            try
            {
                DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

                if (drModelDetails.Count() == 0)
                {

                }

                //Get 1st row from arrey 
                DataRow DrCampaignDetails = drModelDetails[0];

                #region
                //_CmpName = string.Empty;
                //_CmpName = DrCampaignDetails.ItemArray[1].ToString();
                //string AcFilePath = DrCampaignDetails.ItemArray[2].ToString();
                //string TweetFilePath = DrCampaignDetails.ItemArray[3].ToString();
                //bool IsDuplicatMsg = (Convert.ToInt32(DrCampaignDetails.ItemArray[5]) == 1) ? true : false;
                //bool IsAllTweetParAc = (Convert.ToInt32(DrCampaignDetails.ItemArray[6]) == 1) ? true : false;
                //bool IsHashTag = (Convert.ToInt32(DrCampaignDetails.ItemArray[7]) == 1) ? true : false;
                //bool IsTweetParDey = (Convert.ToInt32(DrCampaignDetails.ItemArray[8]) == 1) ? true : false;
                //int NoOfTweetPerday = Convert.ToInt32(DrCampaignDetails.ItemArray[9]);
                //int NoOfTweetPerAc = Convert.ToInt32(DrCampaignDetails.ItemArray[10]);
                //bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[11]) == 1) ? true : false;
                //DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[12].ToString());
                //DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                //int DelayStar = Convert.ToInt32(DrCampaignDetails.ItemArray[14]);
                //int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[15]);
                //int Threads = Convert.ToInt32(DrCampaignDetails.ItemArray[16]);
                #endregion

                _CmpName = string.Empty;
                _CmpName = DrCampaignDetails.ItemArray[1].ToString();
                string AcFilePath = DrCampaignDetails.ItemArray[2].ToString();
                string TweetFilePath = DrCampaignDetails.ItemArray[3].ToString();
                string TweetImageFolderPath = DrCampaignDetails.ItemArray[4].ToString();
                bool IsDuplicatMsg = (Convert.ToInt32(DrCampaignDetails.ItemArray[5]) == 1) ? true : false;
                bool IsAllTweetParAc = (Convert.ToInt32(DrCampaignDetails.ItemArray[6]) == 1) ? true : false;
                bool IsHashTag = (Convert.ToInt32(DrCampaignDetails.ItemArray[7]) == 1) ? true : false;
                bool IsTweetParDey = (Convert.ToInt32(DrCampaignDetails.ItemArray[8]) == 1) ? true : false;
                int NoOfTweetPerday = Convert.ToInt32(DrCampaignDetails.ItemArray[9]);
                int NoOfTweetPerAc = Convert.ToInt32(DrCampaignDetails.ItemArray[10]);
                bool IsTweetWithImage = (Convert.ToInt32(DrCampaignDetails.ItemArray[11]) == 1) ? true : false;
                bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());
                int DelayStar = Convert.ToInt32(DrCampaignDetails.ItemArray[15]);
                int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[16]);
                int Threads = Convert.ToInt32(DrCampaignDetails.ItemArray[17]);
                string UserListFilePath = DrCampaignDetails.ItemArray[20].ToString();
                bool IsUserList = (Convert.ToInt32(DrCampaignDetails.ItemArray[21]) == 1) ? true : false;


                bool IsTweetMentionUserScrapedList = (Convert.ToInt32(DrCampaignDetails.ItemArray[23]) == 1) ? true : false;
                string TweetMentionUserNameScrapedList = DrCampaignDetails.ItemArray[24].ToString();
                bool IsTweetFollowerScrapedList = (Convert.ToInt32(DrCampaignDetails.ItemArray[25]) == 1) ? true : false;
                bool IsTweetFollowingScrapedList = (Convert.ToInt32(DrCampaignDetails.ItemArray[26]) == 1) ? true : false;
                int NoOfTweetMentionUserScrapedList = Convert.ToInt32(DrCampaignDetails.ItemArray[27]);
                int NoOfTweetScrapedUserScrapedLIst = Convert.ToInt32(DrCampaignDetails.ItemArray[28]);



                chklstbox_campaign.Invoke(new MethodInvoker(delegate
                {
                    chklstbox_campaign.SetItemCheckState(1, CheckState.Checked);
                    chklstbox_campaign.SelectedIndex = 1;
                    chklstbox_campaign.SelectionMode = SelectionMode.None;
                }));

                txt_CampaignName.Invoke(new MethodInvoker(delegate
                {
                    txt_CampaignName.ReadOnly = true;
                    txt_CampaignName.Text = CampaignName;
                }));
                txt_accountfilepath.Invoke(new MethodInvoker(delegate
                {
                    txt_accountfilepath.Text = "";
                    txt_accountfilepath.Text = AcFilePath;
                }));

                // create control class OBJ 
                Control frmusercontrols;

                // Get control from user page
                //and put all save value in boxes 

                #region
                //Put file address in Tweet MSG text box.
                frmusercontrols = new Control();
                frmusercontrols = tweetusercontrol.TopLevelControl.Controls.Find("txt_CmpTweetMessageFile", true)[0];

                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "txt_CmpTweetMessageFile")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {
                            if (!string.IsNullOrEmpty(TweetFilePath))
                                frmusercontrols.Text = TweetFilePath;
                            else
                                frmusercontrols.Text = TweetFilePath;
                        }));

                        //txt_accountfilepath.Invoke(new MethodInvoker(delegate
                        //{
                        //    txt_accountfilepath.Text = "";
                        //    txt_accountfilepath.Text = AcFilePath;
                        //}));
                    }
                }
                #endregion


                // Is Tweet With Image 

                #region
                frmusercontrols = new Control();
                frmusercontrols = tweetusercontrol.TopLevelControl.Controls.Find("chk_TweetWithImage", true)[0];
                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "chk_TweetWithImage")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {
                            CheckBox chkBox = (CheckBox)frmusercontrols;
                            if (IsTweetWithImage == true)
                                chkBox.Checked = true;
                            else
                                chkBox.Checked = false;
                        }));
                    }
                }

                if (IsTweetWithImage)
                {
                    frmusercontrols = new Control();
                    frmusercontrols = tweetusercontrol.TopLevelControl.Controls.Find("txt_TweetImageFilePath", true)[0];
                    if (frmusercontrols != null)
                    {
                        if (frmusercontrols.Name == "txt_TweetImageFilePath")
                        {
                            frmusercontrols.Invoke(new MethodInvoker(delegate
                            {

                                if (!string.IsNullOrEmpty(TweetImageFolderPath))
                                    frmusercontrols.Text = TweetImageFolderPath;
                                else
                                    frmusercontrols.Text = TweetImageFolderPath;
                            }));
                        }
                    }
                }
                #endregion


                #region  IScrapedUserListMentionUser

                frmusercontrols = new Control();
                frmusercontrols = tweetusercontrol.TopLevelControl.Controls.Find("chkTweetMentionUserScrapedList", true)[0];
                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "chkTweetMentionUserScrapedList")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {
                            CheckBox chkBox = (CheckBox)frmusercontrols;
                            chkBox.Checked = false;
                            if (IsTweetMentionUserScrapedList == true)
                                chkBox.Checked = true;
                            else
                                chkBox.Checked = false;
                        }));
                    }
                }

                //frmusercontrols = new Control();
                TweetMentionUserUsingScrapedList = new MixedCampaignManager.CustomUserControls.TweetMentionUserUsingScrapedList();
                frmusercontrols = TweetMentionUserUsingScrapedList.Controls.Find("txtRetweetMentionUserName", true)[0];

                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "txtRetweetMentionUserName")
                    {
                        //frmusercontrols.Invoke(new MethodInvoker(delegate
                        //{

                        if (!string.IsNullOrEmpty(TweetMentionUserNameScrapedList))
                            frmusercontrols.Text = TweetMentionUserNameScrapedList;
                        else
                            frmusercontrols.Text = TweetMentionUserNameScrapedList;
                        //}));
                    }
                }



                //frmusercontrols = new Control();
                TweetMentionUserUsingScrapedList = new MixedCampaignManager.CustomUserControls.TweetMentionUserUsingScrapedList();
                frmusercontrols = TweetMentionUserUsingScrapedList.Controls.Find("chkTweetFollowers", true)[0];

                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "chkTweetFollowers")
                    {
                        //frmusercontrols.Invoke(new MethodInvoker(delegate
                        //{
                        CheckBox chkBox = (CheckBox)frmusercontrols;
                        chkBox.Checked = false;
                        if (IsTweetFollowerScrapedList == true)
                        {
                            chkBox.Checked = true;
                        }
                        else
                        {
                            chkBox.Checked = true;
                            chkBox.Checked = false;
                        }
                        //}));
                    }
                }

                //frmusercontrols = new Control();
                TweetMentionUserUsingScrapedList = new MixedCampaignManager.CustomUserControls.TweetMentionUserUsingScrapedList();
                frmusercontrols = TweetMentionUserUsingScrapedList.Controls.Find("chkTweetFollowing", true)[0];
                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "chkTweetFollowing")
                    {
                        //frmusercontrols.Invoke(new MethodInvoker(delegate
                        //{
                        CheckBox chkBox = (CheckBox)frmusercontrols;
                        chkBox.Checked = false;
                        if (IsTweetFollowingScrapedList == true)
                        {
                            chkBox.Checked = true;
                        }
                        else
                        {
                            chkBox.Checked = true;
                            chkBox.Checked = false;
                        }
                        //}));
                    }
                }

                //frmusercontrols = new Control();
                TweetMentionUserUsingScrapedList = new MixedCampaignManager.CustomUserControls.TweetMentionUserUsingScrapedList();
                frmusercontrols = TweetMentionUserUsingScrapedList.Controls.Find("txtTweetMentionNoofUserCount", true)[0];
                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "txtTweetMentionNoofUserCount")
                    {
                        //frmusercontrols.Invoke(new MethodInvoker(delegate
                        //{
                        if (!string.IsNullOrEmpty(NoOfTweetMentionUserScrapedList.ToString()))
                        {
                            frmusercontrols.Text = string.Empty;
                            frmusercontrols.Text = NoOfTweetMentionUserScrapedList.ToString();
                        }
                        else
                        {
                            frmusercontrols.Text = NoOfTweetMentionUserScrapedList.ToString();
                        }
                        // }));
                    }
                }

                //frmusercontrols = new Control();
                TweetMentionUserUsingScrapedList = new MixedCampaignManager.CustomUserControls.TweetMentionUserUsingScrapedList();
                frmusercontrols = TweetMentionUserUsingScrapedList.Controls.Find("txtTweetCountMentinUser", true)[0];
                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "txtTweetCountMentinUser")
                    {
                        //frmusercontrols.Invoke(new MethodInvoker(delegate
                        //{
                        if (!string.IsNullOrEmpty(NoOfTweetScrapedUserScrapedLIst.ToString()))
                        {
                            frmusercontrols.Text = string.Empty;
                            frmusercontrols.Text = NoOfTweetScrapedUserScrapedLIst.ToString();

                        }
                        else
                        {
                            frmusercontrols.Text = NoOfTweetScrapedUserScrapedLIst.ToString();
                        }
                        //}));
                    }
                }

                #endregion


                #region  Put Folder path in Tweet Image text box.

                frmusercontrols = new Control();
                frmusercontrols = tweetusercontrol.TopLevelControl.Controls.Find("chkTweetMentionUserScrapedList", true)[0];

                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "chkTweetMentionUserScrapedList")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {

                        }));
                    }
                }
                #endregion


                #region  Put UserUPload text box.

                frmusercontrols = new Control();
                frmusercontrols = tweetusercontrol.TopLevelControl.Controls.Find("txt_CmpTweetMentionUserList", true)[0];

                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "txt_CmpTweetMentionUserList")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {
                            if (!string.IsNullOrEmpty(UserListFilePath))
                                frmusercontrols.Text = UserListFilePath;
                            else
                                frmusercontrols.Text = UserListFilePath;
                        }));
                    }
                }
                #endregion

                // check if UserUpload is checked
                #region
                frmusercontrols = new Control();
                frmusercontrols = tweetusercontrol.TopLevelControl.Controls.Find("chkCampTweetMentionUser", true)[0];
                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "chkCampTweetMentionUser")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {
                            CheckBox chkBox = (CheckBox)frmusercontrols;
                            if (IsUserList == true)
                                chkBox.Checked = true;
                            else
                                chkBox.Checked = false;
                        }));
                    }
                }
                #endregion
                // check if user MSg with Duplicate
                #region
                frmusercontrols = new Control();
                frmusercontrols = tweetusercontrol.TopLevelControl.Controls.Find("chkboxKeepSingleMessage", true)[0];
                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "chkboxKeepSingleMessage")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {
                            CheckBox chkBox = (CheckBox)frmusercontrols;
                            if (IsDuplicatMsg == true)
                                chkBox.Checked = true;
                            else
                                chkBox.Checked = false;
                        }));
                    }
                }
                #endregion

                // check if All Tweets par account is checked

                #region
                frmusercontrols = new Control();
                frmusercontrols = tweetusercontrol.TopLevelControl.Controls.Find("chkAllTweetsPerAccount", true)[0];
                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "chkAllTweetsPerAccount")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {
                            CheckBox chkBox = (CheckBox)frmusercontrols;
                            if (IsAllTweetParAc == true)
                                chkBox.Checked = true;
                            else
                                chkBox.Checked = false;
                        }));
                    }
                }
                #endregion

                // check if hash tag is checked
                #region
                frmusercontrols = new Control();
                frmusercontrols = tweetusercontrol.TopLevelControl.Controls.Find("chkbosUseHashTags", true)[0];
                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "chkbosUseHashTags")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {
                            CheckBox chkBox = (CheckBox)frmusercontrols;
                            if (IsHashTag == true)
                                chkBox.Checked = true;
                            else
                                chkBox.Checked = false;
                        }));
                    }
                }
                #endregion

                // check if Tweet Per day checked 

                #region
                frmusercontrols = new Control();
                frmusercontrols = tweetusercontrol.TopLevelControl.Controls.Find("ChkboxTweetPerday", true)[0];
                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "ChkboxTweetPerday")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {
                            CheckBox chkBox = (CheckBox)frmusercontrols;
                            if (IsTweetParDey == true)
                                chkBox.Checked = true;
                            else
                                chkBox.Checked = false;
                        }));
                    }
                }
                #endregion


                if (NoOfTweetPerday != 0)
                {
                    #region
                    //Put file address in Tweet MSG text box.
                    frmusercontrols = new Control();
                    frmusercontrols = tweetusercontrol.TopLevelControl.Controls.Find("txtMaximumTweet", true)[0];

                    if (frmusercontrols != null)
                    {
                        if (frmusercontrols.Name == "txtMaximumTweet")
                        {
                            frmusercontrols.Invoke(new MethodInvoker(delegate
                            {
                                frmusercontrols.Text = NoOfTweetPerday.ToString();
                            }));
                        }
                    }
                    #endregion
                }


                if (NoOfTweetPerAc != 0)
                {
                    #region
                    //Put file address in Tweet MSG text box.
                    frmusercontrols = new Control();
                    frmusercontrols = tweetusercontrol.TopLevelControl.Controls.Find("txtNoOfTweets", true)[0];

                    if (frmusercontrols != null)
                    {
                        if (frmusercontrols.Name == "txtNoOfTweets")
                        {
                            frmusercontrols.Invoke(new MethodInvoker(delegate
                            {
                                frmusercontrols.Text = NoOfTweetPerAc.ToString();
                            }));
                        }
                    }
                    #endregion
                }


                if (IsSchedulDaily == true)
                {
                    #region

                    chkbox_IsScheduledDaily.Invoke(new MethodInvoker(delegate
                        {
                            chkbox_IsScheduledDaily.Checked = true;
                        }));

                    dateTimePicker_Start.Invoke(new MethodInvoker(delegate
                    {
                        dateTimePicker_Start.MinDate = SchedulerStartTime.Date;

                        dateTimePicker_Start.Value = (SchedulerStartTime);
                    }));

                    dateTimePicker_End.Invoke(new MethodInvoker(delegate
                    {
                        dateTimePicker_End.MinDate = SchedulerEndTime.Date;

                        dateTimePicker_End.Value = (SchedulerEndTime);
                    }));
                    #endregion
                }
                else
                {
                    chkbox_IsScheduledDaily.Invoke(new MethodInvoker(delegate
                    {
                        chkbox_IsScheduledDaily.Checked = false;
                    }));
                }

                if (DelayStar != 0)
                {
                    txt_DelayFrom.Invoke(new MethodInvoker(delegate { txt_DelayFrom.Text = DelayStar.ToString(); }));
                }

                if (DelayEnd != 0)
                {
                    txt_DelayTo.Invoke(new MethodInvoker(delegate { txt_DelayTo.Text = DelayEnd.ToString(); }));
                }
                if (Threads != 0)
                {
                    txtNoOfFollowThreads.Invoke(new MethodInvoker(delegate { txtNoOfFollowThreads.Text = Threads.ToString(); }));
                }
                UpdatePopUpofmentionUser("");
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("30.editingtweetcampaign :" + ex.Message);
            }
            
        }

        #endregion

        
        #region <--- Retweet campaign functions --->

        public void SavingReTweetSettings()
        {
            try
            {
                string query = "Insert into 'Campaign_retweet'(CampaignName, AcFilePath, Keyword, IsUsername,UniqueMessage, RetweetParDay, NoofRetweetParDay, NoofRetweetParAc, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module)"
                    + " values ('" + _CmpName + "','" + _AccountFilePath + "','" + classes.cls_variables._retweetKeyword + "','" + classes.cls_variables._IsUsername + "'"
                    + ",'" + classes.cls_variables._IsUniqueMessage + "','" + classes.cls_variables._IsRetweetParDay + "','" + classes.cls_variables._NoofRetweetParDay + "','" + classes.cls_variables._NoofRetweetParAc + "',"
                    + "'" + +classes.cls_variables._IsScheduledDaily + "','" + classes.cls_variables._StartFrom + "','" + classes.cls_variables._EndTo + "','" + classes.cls_variables._DelayFrom + "', '" + classes.cls_variables._DelayTo + "', '" + Threads + "', 'Retweet')";


                RepositoryClasses.cls_DbRepository.InsertQuery(query, "Campaign_tweet");

                AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [  " + _CmpName + " is Save. ]");

                //Reload all Saved campaign....
                LoadCampaign();

                //Re- start scheduled campaigns ... 
                ScheduledTasks();

                ///Clear campaign 
                ClearCamapigns("Retweet");
            }
            catch (Exception)
            {
            }
        }

        public void UpdateRetweetCampaign()
        {
            try
            {
                string query = "UPDATE Campaign_retweet SET AcFilePath ='" + _AccountFilePath + "',Keyword='" + classes.cls_variables._retweetKeyword + "'"
                        + " , IsUsername= '" + classes.cls_variables._IsUsername + "' ,UniqueMessage='" + classes.cls_variables._IsUniqueMessage + "', RetweetParDay='" + classes.cls_variables._IsRetweetParDay + "'"
                        + ", NoofRetweetParDay='" + classes.cls_variables._NoofRetweetParDay + "', NoofRetweetParAc= '" + classes.cls_variables._NoofRetweetParAc + "'"
                        + ", ScheduledDaily='" + classes.cls_variables._IsScheduledDaily + "', StartTime='" + classes.cls_variables._StartFrom + "', EndTime='" + classes.cls_variables._EndTo + "'"
                        + ",DelayFrom='" + classes.cls_variables._DelayFrom + "',DelayTo='" + classes.cls_variables._DelayTo + "', Threads='" + Threads + "' WHERE CampaignName='" + _CmpName + "';";

                RepositoryClasses.cls_DbRepository.UpdateQuery(query, "Campaign_tweet");

                LoadCampaign();

                AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [  " + _CmpName + " is Updated. ]");

                ///Clear campaign 
                ClearCamapigns("Retweet");


            }
            catch (Exception)
            {
            }
        }

        public void editingReTweetCampaign(String CampaignName, String featurName)
        {
            DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

            if (drModelDetails.Count() == 0)
            {

            }

            //Get 1st row from arrey 
            DataRow DrCampaignDetails = drModelDetails[0];

            _CmpName = string.Empty;
            _CmpName = DrCampaignDetails.ItemArray[1].ToString();
            string AcFilePath = DrCampaignDetails.ItemArray[2].ToString();
            string _retweetKeyword = DrCampaignDetails.ItemArray[3].ToString();
            bool _IsUsername = (Convert.ToInt32(DrCampaignDetails.ItemArray[5]) == 1) ? true : false;
            bool _IsUniqueMessage = (Convert.ToInt32(DrCampaignDetails.ItemArray[19]) == 1) ? true : false;
            bool _IsRetweetParDay = (Convert.ToInt32(DrCampaignDetails.ItemArray[8]) == 1) ? true : false;
            int _NoofRetweetParDay = Convert.ToInt32(DrCampaignDetails.ItemArray[9]);
            int _NoofRetweetParAc = Convert.ToInt32(DrCampaignDetails.ItemArray[10]);
            bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
            DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
            DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());
            int DelayStar = Convert.ToInt32(DrCampaignDetails.ItemArray[15]);
            int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[16]);
            int Threads = Convert.ToInt32(DrCampaignDetails.ItemArray[17]);




            chklstbox_campaign.Invoke(new MethodInvoker(delegate
            {
                chklstbox_campaign.SetItemCheckState(2, CheckState.Checked);
                chklstbox_campaign.SelectedIndex = 2;
                chklstbox_campaign.SelectionMode = SelectionMode.None;
            }));

            txt_CampaignName.Invoke(new MethodInvoker(delegate
            {
                txt_CampaignName.ReadOnly = true;
                txt_CampaignName.Text = CampaignName;
            }));
            txt_accountfilepath.Invoke(new MethodInvoker(delegate
            {
                txt_accountfilepath.Text = AcFilePath;
            }));


            // create control class OBJ 
            Control frmusercontrols;

            // Get control from user page
            //and put all save value in boxes 

            #region
            //Put keyword in reTweet keyword text box.
            frmusercontrols = new Control();
            frmusercontrols = retweetusercontrol.TopLevelControl.Controls.Find("txt_campReTweetKeyword", true)[0];

            if (frmusercontrols != null)
            {
                if (frmusercontrols.Name == "txt_campReTweetKeyword")
                {
                    frmusercontrols.Invoke(new MethodInvoker(delegate
                    {
                        if (!string.IsNullOrEmpty(_retweetKeyword))
                            frmusercontrols.Text = _retweetKeyword;
                        else
                            frmusercontrols.Text = _retweetKeyword;
                    }));
                }
            }
            #endregion


            #region
            frmusercontrols = new Control();
            frmusercontrols = retweetusercontrol.TopLevelControl.Controls.Find("chk_campRetweetbyUser", true)[0];
            if (frmusercontrols != null)
            {
                if (frmusercontrols.Name == "chk_campRetweetbyUser")
                {
                    frmusercontrols.Invoke(new MethodInvoker(delegate
                    {
                        CheckBox chkBox = (CheckBox)frmusercontrols;
                        if (_IsUsername == true)
                            chkBox.Checked = true;
                        else
                            chkBox.Checked = false;
                    }));
                }
            }
            #endregion


            #region
            frmusercontrols = new Control();
            frmusercontrols = retweetusercontrol.TopLevelControl.Controls.Find("chk_campRetweetPerDay", true)[0];
            if (frmusercontrols != null)
            {
                if (frmusercontrols.Name == "chk_campRetweetPerDay")
                {
                    frmusercontrols.Invoke(new MethodInvoker(delegate
                    {
                        CheckBox chkBox = (CheckBox)frmusercontrols;
                        if (_IsRetweetParDay == true)
                            chkBox.Checked = true;
                        else
                            chkBox.Checked = false;
                    }));
                }
            }
            #endregion


            #region
            frmusercontrols = new Control();
            frmusercontrols = retweetusercontrol.TopLevelControl.Controls.Find("chkUniqueMessageRetweet", true)[0];
            if (frmusercontrols != null)
            {
                if (frmusercontrols.Name == "chkUniqueMessageRetweet")
                {
                    frmusercontrols.Invoke(new MethodInvoker(delegate
                    {
                        CheckBox chkBox = (CheckBox)frmusercontrols;
                        if (_IsUniqueMessage == true)
                            chkBox.Checked = true;
                        else
                            chkBox.Checked = false;
                    }));
                }
            }
            #endregion


            if (_NoofRetweetParDay != 0)
            {
                #region
                //Put file address in Tweet MSG text box.
                frmusercontrols = new Control();
                frmusercontrols = retweetusercontrol.TopLevelControl.Controls.Find("txt_campMaximumNoRetweet", true)[0];

                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "txt_campMaximumNoRetweet")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {
                            frmusercontrols.Text = _NoofRetweetParDay.ToString();
                        }));
                    }
                }
                #endregion
            }


            if (_NoofRetweetParAc != 0)
            {
                #region
                //Put file address in Tweet MSG text box.
                frmusercontrols = new Control();
                frmusercontrols = retweetusercontrol.TopLevelControl.Controls.Find("txt_campNoOfRetweetsParAc", true)[0];

                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "txt_campNoOfRetweetsParAc")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {
                            frmusercontrols.Text = _NoofRetweetParAc.ToString();
                        }));
                    }
                }
                #endregion
            }

            if (IsSchedulDaily == true)
            {
                #region

                chkbox_IsScheduledDaily.Invoke(new MethodInvoker(delegate
                {
                    chkbox_IsScheduledDaily.Checked = true;
                }));

                dateTimePicker_Start.Invoke(new MethodInvoker(delegate
                {
                    dateTimePicker_Start.MinDate = SchedulerStartTime.Date;

                    dateTimePicker_Start.Value = (SchedulerStartTime);
                }));

                dateTimePicker_End.Invoke(new MethodInvoker(delegate
                {
                    dateTimePicker_End.MinDate = SchedulerEndTime.Date;

                    dateTimePicker_End.Value = (SchedulerEndTime);
                }));
                #endregion
            }
            else
            {
                chkbox_IsScheduledDaily.Invoke(new MethodInvoker(delegate
                {
                    chkbox_IsScheduledDaily.Checked = false;
                }));
            }

            if (DelayStar != 0)
            {
                txt_DelayFrom.Invoke(new MethodInvoker(delegate { txt_DelayFrom.Text = DelayStar.ToString(); }));
            }

            if (DelayEnd != 0)
            {
                txt_DelayTo.Invoke(new MethodInvoker(delegate { txt_DelayTo.Text = DelayEnd.ToString(); }));
            }
            if (Threads != 0)
            {
                txtNoOfFollowThreads.Invoke(new MethodInvoker(delegate { txtNoOfFollowThreads.Text = Threads.ToString(); }));
            }

        }

        public void StartReTweetCampaign(String CampaignName, String featurName)
        {
            //Add List Of Working thread
            //we are using this list when we stop/abort running camp processes..
            try
            {
                MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName, Thread.CurrentThread);
            }
            catch (Exception)
            {
            }

            //Get Detals from Data Set table by Campaign Name
            DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

            if (drModelDetails.Count() == 0)
            {
                return;
            }

            //Get 1st row from arrey 
            DataRow DrCampaignDetails = drModelDetails[0];
            bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
            DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
            DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());


            classes.Cls_Retweet startProcess = new Cls_Retweet();

            startProcess.CampaignReTweetLogEvents.addToLogger += new EventHandler(logEvents_addToLogger);

            if (IsSchedulDaily)
            {
                MessageBox.Show(CampaignName + " task is scheduled. Task start timing  :- " + SchedulerStartTime);
                AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [  " + CampaignName + " task is scheduled. Task start timing  :- " + SchedulerStartTime + " ]");
                while (true)
                {
                    if ((SchedulerStartTime.Hour) == (DateTime.Now.Hour) && SchedulerStartTime.Minute == (DateTime.Now.Minute) && (startProcess._IsReTweetProcessStart == true))
                    {
                        startProcess._IsReTweetProcessStart = false;
                        startProcess.StartRetweetProcess(CompaignsDataSet, CampaignName);
                        break;
                    }
                }
            }
            else
            {
                startProcess.StartRetweetProcess(CompaignsDataSet, CampaignName);
            }
        }

        #endregion


        #region <--- Replay campaign functions --->


        public void SaveReplySettings()
        {
            try
            {
                string query = "Insert into 'Campaign_reply'(CampaignName, AcFilePath, ReplyFilePath, Keyword, IsUsername,UniqueMessage, ReplyParDay, NoofReplyParDay, NoofReplyParAc, ScheduledDaily, IsDuplicateMessage, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module)"
                    + " values ('" + _CmpName + "','" + _AccountFilePath + "','" + classes.cls_variables._replyMsgFilePath + "','" + classes.cls_variables._replyKeyword + "','" + classes.cls_variables._IsReplyUsername + "'"
                    + ",'" + classes.cls_variables._IsUniqueMessage + "','" + classes.cls_variables._IsreplyParDay + "','" + classes.cls_variables._NoofreplyParDay + "','" + classes.cls_variables._NoofreplyParAc + "',"
                    + "'" + classes.cls_variables._IsScheduledDaily + "','" + classes.cls_variables._replyUseDuplicateMsg + "','" + classes.cls_variables._StartFrom + "','" + classes.cls_variables._EndTo + "','" + classes.cls_variables._DelayFrom + "', '" + classes.cls_variables._DelayTo + "', '" + Threads + "', 'Reply')";


                RepositoryClasses.cls_DbRepository.InsertQuery(query, "Campaign_reply");

                AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [  " + _CmpName + " is Save. ]");

                //Reload all Saved campaign....
                LoadCampaign();

                //Re- start scheduled campaigns ... 
                ScheduledTasks();

                ///Clear campaign 
                ClearCamapigns("Reply");
            }
            catch (Exception)
            {
            }
        }

        public void UpdateReplyCampaign()
        {
            try
            {
                string query = "UPDATE Campaign_reply SET AcFilePath ='" + _AccountFilePath + "',ReplyFilePath='" + classes.cls_variables._replyMsgFilePath + "',Keyword='" + classes.cls_variables._replyKeyword + "'"
                        + " , IsUsername= '" + classes.cls_variables._IsReplyUsername + "' ,UniqueMessage='" + classes.cls_variables._IsUniqueMessage + "', ReplyParDay='" + classes.cls_variables._IsRetweetParDay + "'"
                        + ", NoofReplyParDay='" + classes.cls_variables._NoofreplyParDay + "', NoofReplyParAc= '" + classes.cls_variables._NoofreplyParAc + "'"
                        + ", ScheduledDaily='" + classes.cls_variables._IsScheduledDaily + "',IsDuplicateMessage='" + classes.cls_variables._replyUseDuplicateMsg + "', StartTime='" + classes.cls_variables._StartFrom + "', EndTime='" + classes.cls_variables._EndTo + "'"
                        + ",DelayFrom='" + classes.cls_variables._DelayFrom + "',DelayTo='" + classes.cls_variables._DelayTo + "', Threads='" + Threads + "' WHERE CampaignName='" + _CmpName + "';";

                RepositoryClasses.cls_DbRepository.UpdateQuery(query, "Campaign_reply");

                LoadCampaign();

                AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [  " + _CmpName + " is Updated. ]");

                ///Clear campaign 
                ClearCamapigns("Reply");
            }
            catch (Exception)
            {
            }
        }

        public void editingReplyCampaign(String CampaignName, String featurName)
        {
            DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

            if (drModelDetails.Count() == 0)
            {

            }

            //Get 1st row from arrey 
            DataRow DrCampaignDetails = drModelDetails[0];

            //SELECT  indx, CampaignName, AcFilePath, ReplyFilePath, Keyword,  IsUsername,'','', ReplyParDay, NoofReplyParDay, NoofReplyParAc, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module from Campaign_reply
            _CmpName = string.Empty;
            _CmpName = DrCampaignDetails.ItemArray[1].ToString();
            string AcFilePath = DrCampaignDetails.ItemArray[2].ToString();
            string _ReplyMsgfilePath = DrCampaignDetails.ItemArray[3].ToString();
            string _ReplyKeyword = DrCampaignDetails.ItemArray[4].ToString();
            bool _IsUsername = (Convert.ToInt32(DrCampaignDetails.ItemArray[5]) == 1) ? true : false;
            bool _IsUniqueMessage = (Convert.ToInt32(DrCampaignDetails.ItemArray[19]) == 1) ? true : false;
            bool _IsReplyParDay = (Convert.ToInt32(DrCampaignDetails.ItemArray[8]) == 1) ? true : false;
            int _NoofReplyParDay = Convert.ToInt32(DrCampaignDetails.ItemArray[9]);
            int _NoofReplyParAc = Convert.ToInt32(DrCampaignDetails.ItemArray[10]);
            //bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[11]) == 1) ? true : false;
            //DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[12].ToString());
            //DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
            //int DelayStar = Convert.ToInt32(DrCampaignDetails.ItemArray[14]);
            //int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[15]);
            //int Threads = Convert.ToInt32(DrCampaignDetails.ItemArray[16]);

            bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
            DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
            DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());
            int DelayStar = Convert.ToInt32(DrCampaignDetails.ItemArray[15]);
            int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[16]);
            int Threads = Convert.ToInt32(DrCampaignDetails.ItemArray[17]);
            bool IsDuplicateMessage = (Convert.ToInt32(DrCampaignDetails.ItemArray[22]) == 1) ? true : false;

            chklstbox_campaign.Invoke(new MethodInvoker(delegate
            {
                chklstbox_campaign.SetItemCheckState(3, CheckState.Checked);
                chklstbox_campaign.SelectedIndex = 3;
                chklstbox_campaign.SelectionMode = SelectionMode.None;
            }));

            txt_CampaignName.Invoke(new MethodInvoker(delegate
            {
                txt_CampaignName.ReadOnly = true;
                txt_CampaignName.Text = CampaignName;
            }));
            txt_accountfilepath.Invoke(new MethodInvoker(delegate
            {
                txt_accountfilepath.Text = AcFilePath;
            }));

            // create control class OBJ 
            Control frmusercontrols;

            // Get control from user page
            //and put all save value in boxes 


            #region
            //Put keyword in reTweet keyword text box.
            frmusercontrols = new Control();
            frmusercontrols = replyusercontrol.TopLevelControl.Controls.Find("txt_replyMsg", true)[0];

            if (frmusercontrols != null)
            {
                if (frmusercontrols.Name == "txt_replyMsg")
                {
                    frmusercontrols.Invoke(new MethodInvoker(delegate
                    {
                        if (!string.IsNullOrEmpty(_ReplyMsgfilePath))
                            frmusercontrols.Text = _ReplyMsgfilePath;
                        else
                            frmusercontrols.Text = _ReplyMsgfilePath;
                    }));
                }
            }
            #endregion


            #region
            //Put keyword in reTweet keyword text box.
            frmusercontrols = new Control();
            frmusercontrols = replyusercontrol.TopLevelControl.Controls.Find("txt_campReplyKeyword", true)[0];

            if (frmusercontrols != null)
            {
                if (frmusercontrols.Name == "txt_campReplyKeyword")
                {
                    frmusercontrols.Invoke(new MethodInvoker(delegate
                    {
                        if (!string.IsNullOrEmpty(_ReplyKeyword))
                            frmusercontrols.Text = _ReplyKeyword;
                        else
                            frmusercontrols.Text = _ReplyKeyword;
                    }));
                }
            }
            #endregion


            #region
            frmusercontrols = new Control();
            frmusercontrols = replyusercontrol.TopLevelControl.Controls.Find("chk_campReplybyUser", true)[0];
            if (frmusercontrols != null)
            {
                if (frmusercontrols.Name == "chk_campReplybyUser")
                {
                    frmusercontrols.Invoke(new MethodInvoker(delegate
                    {
                        CheckBox chkBox = (CheckBox)frmusercontrols;
                        if (_IsUsername == true)
                            chkBox.Checked = true;
                        else
                            chkBox.Checked = false;
                    }));
                }
            }
            #endregion


            #region
            frmusercontrols = new Control();
            frmusercontrols = replyusercontrol.TopLevelControl.Controls.Find("chk_campReplyPerDay", true)[0];
            if (frmusercontrols != null)
            {
                if (frmusercontrols.Name == "chk_campReplyPerDay")
                {
                    frmusercontrols.Invoke(new MethodInvoker(delegate
                    {
                        CheckBox chkBox = (CheckBox)frmusercontrols;
                        if (_IsReplyParDay == true)
                            chkBox.Checked = true;
                        else
                            chkBox.Checked = false;
                    }));
                }
            }
            #endregion

            #region
            frmusercontrols = new Control();
            frmusercontrols = replyusercontrol.TopLevelControl.Controls.Find("ChkUniqueMessage", true)[0];
            if (frmusercontrols != null)
            {
                if (frmusercontrols.Name == "ChkUniqueMessage")
                {
                    frmusercontrols.Invoke(new MethodInvoker(delegate
                    {
                        CheckBox chkBox = (CheckBox)frmusercontrols;
                        if (_IsUniqueMessage == true)
                            chkBox.Checked = true;
                        else
                            chkBox.Checked = false;
                    }));
                }
            }
            #endregion


            #region
            frmusercontrols = new Control();
            frmusercontrols = replyusercontrol.TopLevelControl.Controls.Find("chkReplyUseDuplicateMsg", true)[0];
            if (frmusercontrols != null)
            {
                if (frmusercontrols.Name == "chkReplyUseDuplicateMsg")
                {
                    frmusercontrols.Invoke(new MethodInvoker(delegate
                    {
                        CheckBox chkBox = (CheckBox)frmusercontrols;
                        if (IsDuplicateMessage == true)
                            chkBox.Checked = true;
                        else
                            chkBox.Checked = false;
                    }));
                }
            }
            #endregion

            if (_NoofReplyParDay != 0)
            {
                #region
                //Put file address in Tweet MSG text box.
                frmusercontrols = new Control();
                frmusercontrols = replyusercontrol.TopLevelControl.Controls.Find("txt_campMaximumNoReply", true)[0];

                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "txt_campMaximumNoReply")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {
                            frmusercontrols.Text = _NoofReplyParDay.ToString();
                        }));
                    }
                }
                #endregion
            }


            if (_NoofReplyParAc != 0)
            {
                #region
                //Put file address in Tweet MSG text box.
                frmusercontrols = new Control();
                frmusercontrols = replyusercontrol.TopLevelControl.Controls.Find("txt_campNoOfRepliesParAc", true)[0];

                if (frmusercontrols != null)
                {
                    if (frmusercontrols.Name == "txt_campNoOfRepliesParAc")
                    {
                        frmusercontrols.Invoke(new MethodInvoker(delegate
                        {
                            frmusercontrols.Text = _NoofReplyParAc.ToString();
                        }));
                    }
                }
                #endregion
            }

            if (IsSchedulDaily == true)
            {
                #region

                chkbox_IsScheduledDaily.Invoke(new MethodInvoker(delegate
                {
                    chkbox_IsScheduledDaily.Checked = true;
                }));

                dateTimePicker_Start.Invoke(new MethodInvoker(delegate
                {
                    dateTimePicker_Start.MinDate = SchedulerStartTime.Date;

                    dateTimePicker_Start.Value = (SchedulerStartTime);
                }));

                dateTimePicker_End.Invoke(new MethodInvoker(delegate
                {
                    dateTimePicker_End.MinDate = SchedulerEndTime.Date;

                    dateTimePicker_End.Value = (SchedulerEndTime);
                }));
                #endregion
            }
            else
            {
                chkbox_IsScheduledDaily.Invoke(new MethodInvoker(delegate
                {
                    chkbox_IsScheduledDaily.Checked = false;
                }));
            }

            if (DelayStar != 0)
            {
                txt_DelayFrom.Invoke(new MethodInvoker(delegate { txt_DelayFrom.Text = DelayStar.ToString(); }));
            }

            if (DelayEnd != 0)
            {
                txt_DelayTo.Invoke(new MethodInvoker(delegate { txt_DelayTo.Text = DelayEnd.ToString(); }));
            }
            if (Threads != 0)
            {
                txtNoOfFollowThreads.Invoke(new MethodInvoker(delegate { txtNoOfFollowThreads.Text = Threads.ToString(); }));
            }
        }

        public void StartReplyCampaign(String CampaignName, String featurName)
        {

            //Add List Of Working thread
            //we are using this list when we stop/abort running camp processes..
            try
            {
                MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName, Thread.CurrentThread);
            }
            catch (Exception)
            {
            }

            //Get Detals from Data Set table by Campaign Name
            DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

            if (drModelDetails.Count() == 0)
            {

            }

            //Get 1st row from arrey 
            DataRow DrCampaignDetails = drModelDetails[0];

            bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;

            DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());

            DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());


            classes.Cls_ReplyStart startProcess = new Cls_ReplyStart();

            startProcess.CampaignreplyLogEvents.addToLogger += new EventHandler(logEvents_addToLogger);

            if (IsSchedulDaily)
            {
                MessageBox.Show(CampaignName + " task is scheduled. Task start timing  :- " + SchedulerStartTime);
                AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [  " + CampaignName + " task is scheduled. Task start timing :- " + SchedulerStartTime + " ]");

                while (true)
                {
                    if ((SchedulerStartTime.Hour) == (DateTime.Now.Hour) && SchedulerStartTime.Minute == (DateTime.Now.Minute) && (startProcess._IsReplyProcessStart == true))
                    {
                        startProcess.StartProcess(CompaignsDataSet, CampaignName);
                        break;
                    }
                }
            }
            else
            {
                startProcess.StartProcess(CompaignsDataSet, CampaignName);
            }
        }


        #endregion


        // Get Pixels of Image From BitMap
        public string GetImageValue(Bitmap bitmap)
        {
            string Img = string.Empty;
            try
            {
                Bitmap bmp = new Bitmap(1, 1);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // updated: the Interpolation mode needs to be set to 
                    // HighQualityBilinear or HighQualityBicubic or this method
                    // doesn't work at all.  With either setting, the results are
                    // slightly different from the averaging method.
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bitmap, new Rectangle(0, 0, 1, 1));
                }
                Color pixel = bmp.GetPixel(0, 0);
                // pixel will contain average values for entire orig Bitmap
                byte avgR = pixel.R;

                if (avgR.Equals(75))
                {
                    Img = "ON";
                }
                else if (avgR == 212)
                {
                    Img = "OFF";
                }
            }
            catch { };

            return Img;
        }




        //Load Campaign ...
        public void LoadCampaign()
        {
            try
            {
                chklstbox_campaign.Invoke(new MethodInvoker(delegate
                {
                    chklstbox_campaign.SelectionMode = SelectionMode.One;
                }));

                dgv_campaign.Invoke(new MethodInvoker(delegate
                {
                    dgv_campaign.Rows.Clear();
                    dgv_campaign.Refresh();
                }));

                this.dgv_campaign.AllowUserToAddRows = true;

                CompaignsDataSet.Clear();

                //string query = "Select indx, CampaignName, AcFilePath, FollowingFilePath, '',DividEql, DivideByUser, NoOfUser, FastFollow, NoOfFollowPerAc, '', ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module from Campaign_follow "
                //               + " UNION ALL "
                //               + "Select indx, CampaignName, AcFilePath, TweetMsgFilePath, '',DuplicateMsg, AllTweetParAc, HashTag, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module from Campaign_tweet "
                //               + " UNION ALL "
                //               + " SELECT  indx, CampaignName, AcFilePath, Keyword, '',IsUsername,'','', RetweetParDay, NoofRetweetParDay, NoofRetweetParAc, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module from Campaign_retweet "
                //               + " UNION ALL "
                //               + " SELECT  indx, CampaignName, AcFilePath, ReplyFilePath, Keyword,  IsUsername,'','', ReplyParDay, NoofReplyParDay, NoofReplyParAc, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module from Campaign_reply ";

                string query = "Select indx, CampaignName, AcFilePath, FollowingFilePath, '0' as Image,DividEql, DivideByUser, NoOfUser, FastFollow, NoOfFollowPerAc, '0' as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'0' as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath,'0' as IsDuplicate, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_follow "
               + " UNION ALL "
               + "Select indx, CampaignName, AcFilePath, TweetMsgFilePath,TweetImageFolderPath as Image,DuplicateMsg, AllTweetParAc as DivideByUser, HashTag as NoOfUser, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc as TweetPac,TweetWithImage as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'' as IsUniqueMessage, TweetUploadUserFilePath, IsUploadUserFilePath, '0' as IsDuplicate,IsTweetMentionUserScrapedList,TweetMentionUserNameScrapedList,IsTweetFollowersScrapedList,IsTweetFollowingScrapedList,NoOfTweetMentionUserScrapedList,NoOfScrapedUserScrapedList from Campaign_tweet "
               + " UNION ALL "
               + " SELECT  indx, CampaignName, AcFilePath, Keyword, '0' as Image,IsUsername,'0' as DivideByUser,'0' as NoOfUser, RetweetParDay, NoofRetweetParDay, NoofRetweetParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, '0' as IsDuplicate ,'0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_retweet "
               + " UNION ALL "
               + " SELECT  indx, CampaignName, AcFilePath, ReplyFilePath, Keyword as Image,  IsUsername,'0' as DivideByUser,'0' as NoOfUser, ReplyParDay, NoofReplyParDay, NoofReplyParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, IsDuplicateMessage, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_reply ";

                CompaignsDataSet = RepositoryClasses.cls_DbRepository.selectQuery(query, "Union");


                DataView dataView = new DataView();
                dataView.Table = CompaignsDataSet.Tables[0];
                DataTable newTable = dataView.Table.DefaultView.ToTable(true, "CampaignName", "Module");


                for (int i = 0; i < newTable.Rows.Count; i++)
                {
                    DataGridViewRow row = (DataGridViewRow)dgv_campaign.Rows[0].Clone();
                    row.Cells[0].Value = newTable.Rows[i]["CampaignName"].ToString();
                    row.Cells[1].Value = newTable.Rows[i]["Module"].ToString();
                    
                    var result = MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.ContainsKey(newTable.Rows[i]["CampaignName"].ToString());
                    if (result)
                    {
                        row.Cells[3].Value = (System.Drawing.Image)Properties.Resources.off;
                    }
                    else
                    {
                        row.Cells[3].Value = (System.Drawing.Image)Properties.Resources.on;
                    }

                    
                    dgv_campaign.Invoke(new MethodInvoker(delegate
                    {
                        try
                        {
                            dgv_campaign.Rows.Add(row);
                        }
                        catch (Exception)
                        {
                        }
                    }));
                }

                this.dgv_campaign.AllowUserToAddRows = false;

            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("31.LoadCampaign :" + ex.Message);
            }
        }


       

        public void LoadUpdatedCampaign(string _CmpNameToEdit)
        {
            try
            {
                chklstbox_campaign.Invoke(new MethodInvoker(delegate
                {
                    chklstbox_campaign.SelectionMode = SelectionMode.One;
                }));

                dgv_campaign.Invoke(new MethodInvoker(delegate
                {
                    dgv_campaign.Rows.Clear();
                    dgv_campaign.Refresh();
                }));

                this.dgv_campaign.AllowUserToAddRows = true;

                CompaignsDataSet.Clear();

                //string query = "Select indx, CampaignName, AcFilePath, FollowingFilePath, '',DividEql, DivideByUser, NoOfUser, FastFollow, NoOfFollowPerAc, '', ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module from Campaign_follow "
                //               + " UNION ALL "
                //               + "Select indx, CampaignName, AcFilePath, TweetMsgFilePath, '',DuplicateMsg, AllTweetParAc, HashTag, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module from Campaign_tweet "
                //               + " UNION ALL "
                //               + " SELECT  indx, CampaignName, AcFilePath, Keyword, '',IsUsername,'','', RetweetParDay, NoofRetweetParDay, NoofRetweetParAc, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module from Campaign_retweet "
                //               + " UNION ALL "
                //               + " SELECT  indx, CampaignName, AcFilePath, ReplyFilePath, Keyword,  IsUsername,'','', ReplyParDay, NoofReplyParDay, NoofReplyParAc, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module from Campaign_reply ";

                string query = "Select indx, CampaignName, AcFilePath, FollowingFilePath, '0' as Image,DividEql, DivideByUser, NoOfUser, FastFollow, NoOfFollowPerAc, '0' as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'0' as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath,'0' as IsDuplicate, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_follow "
               + " UNION ALL "
               + "Select indx, CampaignName, AcFilePath, TweetMsgFilePath,TweetImageFolderPath as Image,DuplicateMsg, AllTweetParAc as DivideByUser, HashTag as NoOfUser, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc as TweetPac,TweetWithImage as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'' as IsUniqueMessage, TweetUploadUserFilePath, IsUploadUserFilePath, '0' as IsDuplicate,IsTweetMentionUserScrapedList,TweetMentionUserNameScrapedList,IsTweetFollowersScrapedList,IsTweetFollowingScrapedList,NoOfTweetMentionUserScrapedList,NoOfScrapedUserScrapedList from Campaign_tweet "
               + " UNION ALL "
               + " SELECT  indx, CampaignName, AcFilePath, Keyword, '0' as Image,IsUsername,'0' as DivideByUser,'0' as NoOfUser, RetweetParDay, NoofRetweetParDay, NoofRetweetParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, '0' as IsDuplicate ,'0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_retweet "
               + " UNION ALL "
               + " SELECT  indx, CampaignName, AcFilePath, ReplyFilePath, Keyword as Image,  IsUsername,'0' as DivideByUser,'0' as NoOfUser, ReplyParDay, NoofReplyParDay, NoofReplyParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, IsDuplicateMessage, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_reply ";

                CompaignsDataSet = RepositoryClasses.cls_DbRepository.selectQuery(query, "Union");


                DataView dataView = new DataView();
                dataView.Table = CompaignsDataSet.Tables[0];
                DataTable newTable = dataView.Table.DefaultView.ToTable(true, "CampaignName", "Module");


                for (int i = 0; i < newTable.Rows.Count; i++)
                {

                    DataGridViewRow row = (DataGridViewRow)dgv_campaign.Rows[0].Clone();
                    row.Cells[0].Value = newTable.Rows[i]["CampaignName"].ToString();
                    row.Cells[1].Value = newTable.Rows[i]["Module"].ToString();


                    string CmpName = newTable.Rows[i]["CampaignName"].ToString();

                    if (_CmpNameToEdit == CmpName)
                    {
                        foreach (DataRow dRow in dgv_campaign.Rows)
                        {
                            string dgv_CampaignName = dRow["CampaignName"].ToString();
                            string dgv_Module = dRow["Module"].ToString();

                            if (_CmpNameToEdit == dgv_CampaignName)
                            {
                                //dRow[]
                            }
                        }

                        var result = MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.ContainsKey(newTable.Rows[i]["CampaignName"].ToString());
                        if (result)
                        {
                            row.Cells[3].Value = (System.Drawing.Image)Properties.Resources.off;
                        }
                        else
                        {
                            row.Cells[3].Value = (System.Drawing.Image)Properties.Resources.on;
                        }

                        dgv_campaign.Invoke(new MethodInvoker(delegate
                        {
                            try
                            {
                                dgv_campaign.Rows.Add(row);
                            }
                            catch (Exception)
                            {
                            }
                        }));
                    }
                }

                this.dgv_campaign.AllowUserToAddRows = false;

            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("32.LoadUpdatedCampaign :" + ex.Message);
            }
        }


        //after complition of camp put button image green from red.
        public void controlStartButtonImage(string campaignName)
        {
            int CHeckDailySudule = 0;
            //try
            //{
            //    DataView dataView = new DataView();
            //    dataView.Table = CompaignsDataSet.Tables[0];
            //    DataTable newTable = dataView.Table.DefaultView.ToTable(true, "CampaignName", "Module");
            //    for (int i = 0; i < newTable.Rows.Count; i++)
            //    {
            //        DataGridViewRow row = (DataGridViewRow)dgv_campaign.Rows[0].Clone();
            //        row.Cells[0].Value = newTable.Rows[i]["CampaignName"].ToString();
            //        row.Cells[1].Value = newTable.Rows[i]["Module"].ToString();
            //        CHeckDailySudule = Convert.ToInt32(newTable.Rows[i]["ScheduledDaily"]);
            //        if (campaignName == row.Cells[0].Value)
            //        {
            //            dgv_campaign.Invoke(new MethodInvoker(delegate
            //            {
            //                dgv_campaign.Rows[i].Cells[3].Value = (System.Drawing.Image)Properties.Resources.on;
            //            }));

            //            break;
            //        }


            //    }
            //}
            //catch (Exception ex)
            //{
            //}

            try
            {
                bool IsSchedulDaily = false;
                DataSet ds = new DataSet();
                Thread newThread = new Thread(() =>
                {
                    //string selectQuery = "Select * from Campaign_tweet where CampaignName = '" + campaignName+"'";
                    string selectQuery = "Select indx, CampaignName, AcFilePath, FollowingFilePath, '0' as Image,DividEql, DivideByUser, NoOfUser, FastFollow, NoOfFollowPerAc, '0' as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'0' as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath,'0' as IsDuplicate, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_follow "
                   + " UNION ALL "
                   + "Select indx, CampaignName, AcFilePath, TweetMsgFilePath,TweetImageFolderPath as Image,DuplicateMsg, AllTweetParAc as DivideByUser, HashTag as NoOfUser, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc as TweetPac,TweetWithImage as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'' as IsUniqueMessage, TweetUploadUserFilePath, IsUploadUserFilePath, '0' as IsDuplicate,IsTweetMentionUserScrapedList,TweetMentionUserNameScrapedList,IsTweetFollowersScrapedList,IsTweetFollowingScrapedList,NoOfTweetMentionUserScrapedList,NoOfScrapedUserScrapedList from Campaign_tweet "
                   + " UNION ALL "
                   + " SELECT  indx, CampaignName, AcFilePath, Keyword, '0' as Image,IsUsername,'0' as DivideByUser,'0' as NoOfUser, RetweetParDay, NoofRetweetParDay, NoofRetweetParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, '0' as IsDuplicate ,'0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_retweet "
                   + " UNION ALL "
                   + " SELECT  indx, CampaignName, AcFilePath, ReplyFilePath, Keyword as Image,  IsUsername,'0' as DivideByUser,'0' as NoOfUser, ReplyParDay, NoofReplyParDay, NoofReplyParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, IsDuplicateMessage, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_reply ";

                    ds = RepositoryClasses.cls_DbRepository.selectQuery(selectQuery, "Union");
                    //ds = MixedCampaignManager.RepositoryClasses.cls_DbRepository.selectQuery(selectQuery, "Campaign_tweet");
                    DataRow[] drModelDetails = ds.Tables[0].Select("CampaignName = '" + campaignName + "'");

                    if (drModelDetails.Count() == 0)
                    {
                        return;
                    }

                    //Get 1st row from arrey 
                    DataRow DrCampaignDetails = drModelDetails[0];

                    try
                    {
                        //IsSchedulDaily = (Convert.ToInt32(ds.Tables["Campaign_tweet"].Rows[0]["ScheduledDaily"].ToString()) == 1) ? true : false;
                        IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                    }
                    catch (Exception ex)
                    {

                    }
                });
                newThread.Start();

                newThread.Join();


                if (IsSchedulDaily)
                {

                    ScheduledTasks_AfterStop(campaignName);
                }
                else
                {
                    checkProcess(campaignName);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("33.controlStartButtonImage :" + ex.Message);
            }
        }
        //

        public void checkProcess(string CampaignName)
        {
            foreach (DataGridViewRow dRow in dgv_campaign.Rows)
            {
                string dgv_CampaignName = dRow.Cells["CampaignName"].Value.ToString();
                //string dgv_Module = dRow.Cells["Module"].Value.ToString();

                if (CampaignName == dgv_CampaignName)
                {
                    //dRow.Cells["CampaignName"].Value = "HardCoded";
                    //if (classes.cls_variables._IsScheduledDaily == 1)
                    {
                        dgv_campaign.Invoke(new MethodInvoker(delegate
                        {
                            dRow.Cells["BtnOnOff"].Value = (System.Drawing.Image)Properties.Resources.on;
                        }));
                    }


                    //dgv_campaign.Refresh();
                }
            }
        }



        //checking process according to scheduler 
        //List<string> lstCampaignStartShedular = new List<string>();
        public static int COunt = 0;
        public void ScheduledTasks()
        {

            try
            {
                DataSet CompaignsDataSet1 = new DataSet();


                DataRow[] CampDataRows = CompaignsDataSet.Tables[0].Select("ScheduledDaily = '1'");
                CompaignsDataSet1.Merge(CompaignsDataSet);
                if (CampDataRows.Count() == 0)
                {
                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ Not scheduled any Task. ]");
                    return;
                }

                if (COunt == 0)
                {
                    // MessageBox.Show("Some task is scheduled. Please do not close your Campain manager window.");
                    COunt = COunt + 1;
                }

                foreach (DataRow CampRow in CampDataRows)
                {
                    try
                    {
                        // CampRow.AcceptChanges();
                        string CampaignName = string.Empty;
                        string Module = string.Empty;
                        if (CampRow.Table.Columns["CampaignName"] != null)
                        {

                            CampaignName = CampRow["CampaignName"].ToString();
                            Module = CampRow["Module"].ToString();

                            foreach (DataGridViewRow DgvRow in dgv_campaign.Rows)
                            {
                                if (DgvRow != null)
                                {
                                    var GridRowCampName = DgvRow.Cells[0].Value;
                                    int rowindex = DgvRow.Index;
                                    if (CampaignName == GridRowCampName)
                                    {

                                        dgv_campaign.Invoke(new MethodInvoker(delegate
                                        {
                                            dgv_campaign.Rows[rowindex].Cells[3].Value = Properties.Resources.off;
                                        }));
                                    }
                                }
                            }
                        }


                        //Start Camapign 
                        if (!cls_variables.lstCampaignStartShedular.Contains(CampaignName))
                        {
                            switch (Module)
                            {

                                case "Follow":
                                    new Thread(() =>
                                    {
                                        StartFollowCampaign(CampaignName, Module);
                                    }).Start();
                                    break;
                                case "Tweet":
                                    //new Thread(() =>
                                    //{
                                    //    startTweet(CampaignName, Module);

                                    ThreadPool.QueueUserWorkItem(new WaitCallback(startTweetusingthreadPool), new object[] { CampaignName, Module });
                                    break;
                                case "Retweet":
                                    new Thread(() =>
                                    {
                                        StartReTweetCampaign(CampaignName, Module);
                                    }).Start();
                                    break;
                                case "Reply":
                                    new Thread(() =>
                                    {                                        
                                        StartReplyCampaign(CampaignName, Module);
                                    }).Start();
                                    break;
                                default:
                                    break;
                            }
                            cls_variables.lstCampaignStartShedular.Add(CampaignName);    //add campaign name in list for checking next time its not working
                            //Thread.Sleep(15*1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.AddToErrorLogText("34.sheduledTask :" + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("35.sheduledTask :" + ex.Message);
            }

        }



        public void ScheduledTasks_Updated(string Cmp_Name)
        {

            try
            {
                DataSet CompaignsDataSet1 = new DataSet();

                string query = "Select indx, CampaignName, AcFilePath, FollowingFilePath, '0' as Image,DividEql, DivideByUser, NoOfUser, FastFollow, NoOfFollowPerAc, '0' as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'0' as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath,'0' as IsDuplicate, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_follow "
                  + " UNION ALL "
                  + "Select indx, CampaignName, AcFilePath, TweetMsgFilePath,TweetImageFolderPath as Image,DuplicateMsg, AllTweetParAc as DivideByUser, HashTag as NoOfUser, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc as TweetPac,TweetWithImage as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'' as IsUniqueMessage, TweetUploadUserFilePath, IsUploadUserFilePath, '0' as IsDuplicate,IsTweetMentionUserScrapedList,TweetMentionUserNameScrapedList,IsTweetFollowersScrapedList,IsTweetFollowingScrapedList,NoOfTweetMentionUserScrapedList,NoOfScrapedUserScrapedList from Campaign_tweet "
                  + " UNION ALL "
                  + " SELECT  indx, CampaignName, AcFilePath, Keyword, '0' as Image,IsUsername,'0' as DivideByUser,'0' as NoOfUser, RetweetParDay, NoofRetweetParDay, NoofRetweetParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, '0' as IsDuplicate ,'0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_retweet "
                  + " UNION ALL "
                  + " SELECT  indx, CampaignName, AcFilePath, ReplyFilePath, Keyword as Image,  IsUsername,'0' as DivideByUser,'0' as NoOfUser, ReplyParDay, NoofReplyParDay, NoofReplyParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, IsDuplicateMessage, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_reply ";

                CompaignsDataSet = RepositoryClasses.cls_DbRepository.selectQuery(query, "Union");
                DataRow[] CampDataRows = CompaignsDataSet.Tables[0].Select("ScheduledDaily = '1'");
                CompaignsDataSet1.Merge(CompaignsDataSet);
                if (CampDataRows.Count() == 0)
                {
                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ Not scheduled any Task. ]");
                    return;
                }

                if (COunt == 0)
                {
                    // MessageBox.Show("Some task is scheduled. Please do not close your Campain manager window.");
                    COunt = COunt + 1;
                }

                foreach (DataRow CampRow in CampDataRows)
                {
                    try
                    {
                        // CampRow.AcceptChanges();
                        string CampaignName = string.Empty;
                        string Module = string.Empty;
                        if (CampRow.Table.Columns["CampaignName"] != null)
                        {

                            CampaignName = CampRow["CampaignName"].ToString();
                            Module = CampRow["Module"].ToString();

                            foreach (DataGridViewRow DgvRow in dgv_campaign.Rows)
                            {
                                if (DgvRow != null)
                                {
                                    string GridRowCampName = DgvRow.Cells[0].Value.ToString();
                                    int rowindex = DgvRow.Index;
                                    if (Cmp_Name == GridRowCampName)
                                    {

                                        dgv_campaign.Invoke(new MethodInvoker(delegate
                                        {
                                            dgv_campaign.Rows[rowindex].Cells[3].Value = Properties.Resources.off;
                                        }));

                                        //Start Camapign 
                                        if (!cls_variables.lstCampaignStartShedular.Contains(Cmp_Name))
                                        {
                                            switch (Module)
                                            {

                                                case "Follow":
                                                    //new Thread(() =>
                                                    //{
                                                    //    StartFollowCampaign(Cmp_Name, Module);
                                                    //}).Start();
                                                    ThreadPool.QueueUserWorkItem(new WaitCallback(StartFollowCampaignThreadPool), new object[] { Cmp_Name, Module });
                                                    break;
                                                case "Tweet":
                                                    //new Thread(() =>
                                                    //{
                                                    //    startTweet(CampaignName, Module);

                                                    ThreadPool.QueueUserWorkItem(new WaitCallback(startTweetusingthreadPool), new object[] { Cmp_Name, Module });
                                                    break;
                                                case "Retweet":
                                                    new Thread(() =>
                                                    {
                                                        StartReTweetCampaign(Cmp_Name, Module);
                                                    }).Start();
                                                    break;
                                                case "Reply":
                                                    new Thread(() =>
                                                    {
                                                        StartReplyCampaign(Cmp_Name, Module);
                                                    }).Start();
                                                    break;
                                                default:
                                                    break;
                                            }
                                            cls_variables.lstCampaignStartShedular.Add(Cmp_Name);    //add campaign name in list for checking next time its not working
                                            //Thread.Sleep(15*1000);
                                            break;
                                        }
                                    }
                                }
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.AddToErrorLogText("35.sheduledTaskUpdated :" + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("36.sheduledTaskUpdated :" + ex.Message);
            }

        }


        public void ScheduledTasks_AfterStop(string Cmp_Name)
        {

            try
            {
                DataSet CompaignsDataSet1 = new DataSet();
                if (cls_variables.lstCampaignStartShedular.Contains(Cmp_Name))
                {
                    try
                    {
                        cls_variables.lstCampaignStartShedular.Remove(Cmp_Name);
                    }
                    catch { };
                }


                DataRow[] CampDataRows = CompaignsDataSet.Tables[0].Select("ScheduledDaily = '1'");
                CompaignsDataSet1.Merge(CompaignsDataSet);
                if (CampDataRows.Count() == 0)
                {
                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ Not scheduled any Task. ]");
                    return;
                }

                if (COunt == 0)
                {
                    // MessageBox.Show("Some task is scheduled. Please do not close your Campain manager window.");
                    COunt = COunt + 1;
                }

                //foreach (DataRow CampRow in CampDataRows)
                //{
                //    try
                //    {
                //        // CampRow.AcceptChanges();
                //        string CampaignName = string.Empty;
                //        string Module = string.Empty;
                //        if (CampRow.Table.Columns["CampaignName"] != null)
                //        {

                //            CampaignName = CampRow["CampaignName"].ToString();
                //            Module = CampRow["Module"].ToString();

                //            foreach (DataGridViewRow DgvRow in dgv_campaign.Rows)
                //            {
                //                if (DgvRow != null)
                //                {
                //                    var GridRowCampName = DgvRow.Cells[0].Value;
                //                    int rowindex = DgvRow.Index;
                //                    if (Cmp_Name == GridRowCampName)
                //                    {

                //                        //dgv_campaign.Invoke(new MethodInvoker(delegate
                //                        //{
                //                        //    dgv_campaign.Rows[rowindex].Cells[3].Value = Properties.Resources.off;
                //                        //}));
                //                    }
                //                }
                //            }
                //        }


                string CampaignName = string.Empty;
                string Module = string.Empty;
                foreach (DataRow CampRow in CampDataRows)
                {
                    bool isCheck = false;
                    try
                    {
                        // CampRow.AcceptChanges();

                        if (CampRow.Table.Columns["CampaignName"] != null)
                        {

                            CampaignName = CampRow["CampaignName"].ToString();
                            Module = CampRow["Module"].ToString();
                            foreach (DataGridViewRow DgvRow in dgv_campaign.Rows)
                            {
                                if (DgvRow != null)
                                {
                                    string GridRowCampName = DgvRow.Cells[0].Value.ToString();
                                    int rowindex = DgvRow.Index;
                                    if (Cmp_Name == GridRowCampName)
                                    {
                                        isCheck = true;
                                        break;
                                    }
                                }
                            }
                            if (isCheck)
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.AddToErrorLogText("36.sheduledTaskAfterStop :" + ex.Message);
                    }
                }



                //Start Camapign 
                if (!cls_variables.lstCampaignStartShedular.Contains(Cmp_Name))
                {
                    switch (Module)
                    {

                        case "Follow":
                            //new Thread(() =>
                            //{
                            //    StartFollowCampaign(Cmp_Name, Module);
                            //}).Start();
                            ThreadPool.QueueUserWorkItem(new WaitCallback(startFollowusingthreadPool_AfterStop), new object[] { Cmp_Name, Module });
                            break;
                        case "Tweet":
                            //new Thread(() =>
                            //{
                            //    startTweet(CampaignName, Module);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(startTweetusingthreadPool_AfterStop), new object[] { Cmp_Name, Module });
                            break;
                        case "Retweet":
                            new Thread(() =>
                            {
                                StartReTweetCampaign(Cmp_Name, Module);
                            }).Start();
                            break;
                        case "Reply":
                            new Thread(() =>
                            {
                                StartReplyCampaign(Cmp_Name, Module);
                            }).Start();
                            break;
                        default:
                            break;
                    }
                    cls_variables.lstCampaignStartShedular.Add(Cmp_Name);    //add campaign name in list for checking next time its not working
                    //Thread.Sleep(15*1000);
                }
                //}
                //catch { };
                //}

            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("37.sheduledTaskAfterStop :" + ex.Message);
            }

        }


        // Abort the corrent Campaign process ....
        public void StoprunningCampaign(String CampignName, bool doStartScheduler=false)
        {
            try
            {
                Dictionary<string, Thread> temp_WorkingThreads = new Dictionary<string, Thread>();

                Dictionary<string, Thread> selectedValues = MixedCampaignManager.classes.cls_variables.Lst_WokingThreads
                    .Where(x => (x.Key.Contains(CampignName)))
                    .ToDictionary(x => x.Key, x => x.Value);


                if (selectedValues.Count != 0)
                {
                    foreach (KeyValuePair<string, Thread> selectedValues_item in selectedValues)
                    {
                        String _ThreadKey = selectedValues_item.Key;
                        Thread _ThreadValue = selectedValues_item.Value;

                        if (MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.ContainsKey(_ThreadKey))
                        {
                            MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Remove(_ThreadKey);
                        }

                        if (CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.ContainsKey(_ThreadKey))
                        {
                            CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.Remove(_ThreadKey);

                        }

                        _ThreadValue.Abort();
                        CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.Clear();
                        cls_variables.lstCampaignStartShedular.Remove(CampignName);

                    }

                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ Process is aborted for campaign :- " + CampignName + " ]");
                }

                #region Old code
                //List<Thread> selectedValues = MixedCampaignManager.classes.cls_variables.Lst_WokingThreads
                //    .Where(x => (x.Key.Contains(CampignName)))
                //    .Select(x => x.Value)
                //    .ToList();



                //if (selectedValues.Count != 0)
                //{
                //    foreach (Thread selectedValues_item in selectedValues)
                //    {
                //        MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Remove(
                //        selectedValues_item.Abort();
                //    }

                //    AddToCampaignLoggerListBox("Process is aborted for campaign :- " + CampignName);
                //}
                #endregion

            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("37.stopCampaign :" + ex.Message);
            }
            if (doStartScheduler)
            {
                //Run Scheduled Task if it's daily Scheuled
                try
                {
                    if (cls_variables._IsScheduledDaily == 1)
                    {
                        ////ScheduledTasks_Updated(CampignName);
                        //ScheduledTasks_AfterStop(CampignName);
                    }
                }
                catch { };
            }
        }


        public void PausedrunningCampaign(String CampignName)
        {
            try
            {
                Dictionary<string, Thread> temp_WorkingThreads = new Dictionary<string, Thread>();

                Dictionary<string, Thread> selectedValues = MixedCampaignManager.classes.cls_variables.Lst_WokingThreads
                    .Where(x => (x.Key.Contains(CampignName)))
                    .ToDictionary(x => x.Key, x => x.Value);


                if (selectedValues.Count != 0)
                {
                    foreach (KeyValuePair<string, Thread> selectedValues_item in selectedValues)
                    {
                        String _ThreadKey = selectedValues_item.Key;
                        Thread _ThreadValue = selectedValues_item.Value;

                        if (MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.ContainsKey(_ThreadKey))
                        {
                            //MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Remove(_ThreadKey);
                        }

                        if (CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.ContainsKey(_ThreadKey))
                        {
                            //CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.Remove(_ThreadKey);

                        }

                        try
                        {
                            _ThreadValue.Suspend();
                        }
                        catch { }
                    }

                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ Process is Paused for campaign :- " + CampignName + " ]");
                }

                

            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("37.PauseCampaign :" + ex.Message);
            }
           
        }


        public void ResumedrunningCampaign(String CampignName)
        {
            try
            {
                Dictionary<string, Thread> temp_WorkingThreads = new Dictionary<string, Thread>();

                Dictionary<string, Thread> selectedValues = MixedCampaignManager.classes.cls_variables.Lst_WokingThreads
                    .Where(x => (x.Key.Contains(CampignName)))
                    .ToDictionary(x => x.Key, x => x.Value);


                if (selectedValues.Count != 0)
                {
                    foreach (KeyValuePair<string, Thread> selectedValues_item in selectedValues)
                    {
                        String _ThreadKey = selectedValues_item.Key;
                        Thread _ThreadValue = selectedValues_item.Value;

                        if (MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.ContainsKey(_ThreadKey))
                        {
                            //MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Remove(_ThreadKey);
                        }

                        if (CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.ContainsKey(_ThreadKey))
                        {
                            //CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.Remove(_ThreadKey);

                        }

                        try
                        {
                            _ThreadValue.Resume();
                        }
                        catch { }
                      

                    }

                    AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ Process is Resumed for campaign :- " + CampignName + " ]");
                }



            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("37.ResumedCampaign :" + ex.Message);
            }

        }

        //Split long List in small list of bunches ..
        public static List<List<string>> Split(List<string> source, int splitNumber)
        {
            if (splitNumber <= 0)
            {
                splitNumber = 1;
            }

            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / splitNumber)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }



        /// <Delete campaigns>
        /// Deleting selected campain
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_campaign_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                string campaignName = dgv_campaign.SelectedRows[0].Cells[0].Value.ToString();
                string featurName = dgv_campaign.SelectedRows[0].Cells[1].Value.ToString();
                DialogResult dialogResult = MessageBox.Show("Do you want to delete " + campaignName + " from Data Table and Grid.", "", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //delete row and data from Table and gridview row ..
                    bool status = RepositoryClasses.cls_DbRepository.DeleteSetting(campaignName, featurName);
                    if (status)
                    {
                        try
                        {
                            DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + campaignName + "'");

                            if (drModelDetails.Count() != 0)
                                CompaignsDataSet.Tables[0].Rows.Remove(drModelDetails[0]);

                            AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ " + campaignName + " is Deleted. ]");

                        }
                        catch (Exception ex)
                        {
                            ErrorLogger.AddToErrorLogText("37.campaigndeleting :" + ex.Message);
                        }

                    }
                    else
                    {
                        AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [ " + campaignName + " is not Deleted. ]");
                    }
                }
                else
                {
                    e.Cancel = true;
                }

            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText("38.campaigndeleting :" + ex.Message);
            }

        }

        /// <Change BackGround Image>
        /// Set Back ground image..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_mixcampaignmanager_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the image.
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }



        /// <Call Report Table>
        /// Show Repost Form ...
        /// to display all success fully inserted Data 
        /// from report table 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Status_Click(object sender, EventArgs e)
        {
            frm_Status frmStatus = new frm_Status();
            frmStatus.Show();
        }



        /// < Close window and stop all Running threads >
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_mixcampaignmanager_FormClosing(object sender, FormClosingEventArgs e)
        {
            Unfollower.IsCampaignManagerOpen = true;
            if (MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Count == 0)
            {
                ((Form)sender).Dispose();
                GC.Collect();
                return;
            }
            if ((MessageBox.Show("Do you want to close window. When you closed campaign window than all process aborted.!", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                foreach (DataGridViewRow Rowitem in dgv_campaign.Rows)
                {
                    string CampaignName = Rowitem.Cells[0].FormattedValue.ToString();
                    string FeaturName = Rowitem.Cells[1].FormattedValue.ToString();

                    // Your code would go here below is just the code I used to test 
                    Bitmap ImgBitmap = (Bitmap)(Rowitem.Cells[3].FormattedValue);
                    string Img = GetImageValue(ImgBitmap);

                    if (Img == "OFF")
                    {
                        new Thread(() =>
                        {
                            StoprunningCampaign(CampaignName);
                        }).Start();
                    }
                }
            }
            else
            {
                e.Cancel = true;
            }
        }


        /// <Clear all camapign controls after saving and updation>
        /// 
        /// </summary>
        /// <param name="FeatureName"></param>
        public void ClearCamapigns(String FeatureName)
        {
            ///Clear Text box of campaign Name
            txt_CampaignName.Invoke(new MethodInvoker(delegate
                {
                    txt_CampaignName.Text = "";
                    txt_CampaignName.ReadOnly = false;
                    _CmpName = string.Empty;
                }));


            // clear account file Text box 
            txt_accountfilepath.Invoke(new MethodInvoker(delegate
            {
                txt_accountfilepath.Text = "";
            }));


            //Clear selected feature ...
            if (FeatureName == "Follow")
            {
                #region
                grp_settings.Invoke(new MethodInvoker(delegate
                    {
                        grp_settings.Controls.Clear();
                        followusercontrol = new MixedCampaignManager.CustomUserControls.followusecontrols();
                        followusercontrol.Dock = DockStyle.Top;
                        followusercontrol.Visible = true;
                        panel1.Invoke(new MethodInvoker(delegate
                        {
                            panel1.AutoScroll = true;
                        }));
                        int i = followusercontrol.Height;
                        grp_settings.Height = i + 15;
                        grp_settings.Controls.Add(followusercontrol);
                        grp_settings.Refresh();
                    }));
                #endregion
            }
            else if (FeatureName == "Tweet")
            {
                #region
                grp_settings.Invoke(new MethodInvoker(delegate
                        {
                            panel1.Invoke(new MethodInvoker(delegate
                            {
                                panel1.AutoScroll = false;
                            }));

                            grp_settings.Controls.Clear();
                            tweetusercontrol = new MixedCampaignManager.CustomUserControls.tweetusercontrols();
                            tweetusercontrol.Dock = DockStyle.Top;
                            tweetusercontrol.Visible = true;
                            int i = tweetusercontrol.Height;
                            grp_settings.Height = i + 15;
                            grp_settings.Controls.Add(tweetusercontrol);
                            grp_settings.Refresh();
                        }));
                #endregion
            }
            else if (FeatureName == "Retweet")
            {
                #region
                grp_settings.Invoke(new MethodInvoker(delegate
                       {
                           panel1.Invoke(new MethodInvoker(delegate
                           {
                               panel1.AutoScroll = false;
                           }));

                           grp_settings.Controls.Clear();
                           retweetusercontrol = new MixedCampaignManager.CustomUserControls.retweetusercontrols();
                           retweetusercontrol.Dock = DockStyle.Top;
                           retweetusercontrol.Visible = true;
                           int i = retweetusercontrol.Height;
                           grp_settings.Height = i + 15;
                           grp_settings.Controls.Add(retweetusercontrol);
                           grp_settings.Refresh();
                       }));
                #endregion
            }
            else if (FeatureName == "Reply")
            {
                #region
                grp_settings.Invoke(new MethodInvoker(delegate
                       {
                           panel1.Invoke(new MethodInvoker(delegate
                           {
                               panel1.AutoScroll = false;
                           }));

                           grp_settings.Controls.Clear();
                           replyusercontrol = new MixedCampaignManager.CustomUserControls.replyusercontrols();
                           replyusercontrol.Dock = DockStyle.Top;
                           replyusercontrol.Visible = true;
                           int i = replyusercontrol.Height;
                           grp_settings.Height = i + 15;
                           grp_settings.Controls.Add(replyusercontrol);
                           grp_settings.Refresh();
                       }));
                #endregion
            }


            #region

            chkbox_IsScheduledDaily.Invoke(new MethodInvoker(delegate
            {
                chkbox_IsScheduledDaily.Checked = false;
            }));

            dateTimePicker_Start.Invoke(new MethodInvoker(delegate
            {
                dateTimePicker_Start.MinDate = DateTime.Now.Date;

                dateTimePicker_Start.Value = DateTime.Now;
            }));

            dateTimePicker_End.Invoke(new MethodInvoker(delegate
            {
                dateTimePicker_End.MinDate = DateTime.Now.Date;

                dateTimePicker_End.Value = DateTime.Now;
            }));
            #endregion

            txt_DelayFrom.Invoke(new MethodInvoker(delegate { txt_DelayFrom.Text = "20"; }));

            txt_DelayTo.Invoke(new MethodInvoker(delegate { txt_DelayTo.Text = "25"; }));

            txtNoOfFollowThreads.Invoke(new MethodInvoker(delegate { txtNoOfFollowThreads.Text = "7"; }));
        }

        private void campaignLogger_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Globals.IsCopyLoggerData)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    if (campaignLogger.SelectedItem != null)
                    {
                        foreach (object row in campaignLogger.SelectedItems)
                        {
                            sb.Append(row.ToString());
                            sb.AppendLine();
                        }
                    }
                    if (!(sb.Length == 0))
                    {
                        sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
                    }
                    Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                } 
            }
        }


 


    }
}
