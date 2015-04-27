using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using BaseLib;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Globussoft;
using Follower;

namespace twtboardpro
{
    public partial class frmScheduler : Form
    {
        public frmScheduler()
        {
            InitializeComponent();
        }

        clsDBQueryManager queryManager = new clsDBQueryManager();

        public static Events Event_StartScheduler = new Events();

        public static Events SchedulerLogger = new Events();

        private void frmScheduler_Load(object sender, EventArgs e)
        {
            dgvScheduler.DataError += new DataGridViewDataErrorEventHandler(dgvScheduler_DataError);

            LoadDataGrid();
        }

        void dgvScheduler_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Console.WriteLine("Error in dgvScheduler_DataError");
        }

        private void btnstartScheduler_Click(object sender, EventArgs e)
        {
            new Thread(() => 
            {

                //Check Scheduler is not Null or Empty ...
                try
                {
                    if ((dgvScheduler.Rows.Count) <2)
                    {
                        MessageBox.Show("Not scheduled any task.");
                        return;
                    }
                }
                catch (Exception)
                {
                }


                MessageBox.Show("Please don't close this Form for Scheduler to keep running. You can minimize it and do other tasks on Main Form though.");

                btnstartScheduler.Invoke(new MethodInvoker(delegate
                    {
                        btnstartScheduler.Enabled = false;
                    }));
               

                while (true)
                {
                    RunScheduledTasks();

                    Thread.Sleep(9000);
                    //Thread.Sleep(90000);
                }
            }).Start();
        }

        private void btnRemoveAccomplishedTasks_Click(object sender, EventArgs e)
        {
            //Remove all Accomplished and not Scheduled Daily
            try
            {
                queryManager.DeleteAccomplishedFromTBScheduler();
                LoadDataGrid();

                btnstartScheduler.Invoke(new MethodInvoker(delegate
                {
                    btnstartScheduler.Enabled = true;
                }));
            }
            catch { }
        }

        private void dgvScheduler_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                DataTable dsDataTable = new DataTable();
                dsDataTable = queryManager.SelectAllFromTBScheduler();

                DialogResult dialogResult = MessageBox.Show("Do you want to delete row from Data Table and Grid.", "", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string Id = dgvScheduler.SelectedRows[0].Cells[0].Value.ToString();

                    queryManager.DeleteSelectedRowFromTBScheduler(Id);
                    
                    try
                    {
                        btnstartScheduler.Invoke(new MethodInvoker(delegate
                                   {
                                       btnstartScheduler.Enabled = true;
                                   }));
                    }
                    catch { }
                   
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch { }
            //MessageBox.Show("Selected row is successfully deleted.");
        }

        private void RunScheduledTasks()
        {
            try
            {
                //DataTable dt = queryManager.SelectUnaccomplishedPastScheduledTimeFromTBScheduler();

                //foreach (Data Row dRow in dt.Rows)
                //{
                //    string Module = dRow["Module"].ToString();

                //    //BaseLib.Module mod = modModule(Module);
                //    modModule(Module);
                //}

                List<string> listModules = queryManager.SelectUnaccomplishedPastScheduledTimeFromTBScheduler();

                foreach (string module in listModules)
                {
                    modModule(module);
                    //Update TBScheduler Set IsAccomplished = 1
                    queryManager.UpdateTBScheduler(module);

                    LoadDataGrid();
                }
            }
            catch { }
        }

        public void LoadDataGrid()
        {
            try
            {
                dgvScheduler.Invoke(new MethodInvoker(delegate
                {
                    dgvScheduler.DataSource = queryManager.SelectAllFromTBScheduler();
                }));
            }
            catch { }
            //this.dgvScheduler.AllowUserToAddRows = false;
        }

        public string strModule(Module module)
        {
            switch (module)
            {
                case Module.WaitAndReply:
                    return threadNaming_WaitAndReply_;

                case Module.Tweet:
                    return threadNaming_Tweet_;

                case Module.Retweet:
                    return threadNaming_Retweet_;

                case Module.Reply:
                    return threadNaming_Reply_;

                case Module.Follow:
                    return threadNaming_Follow_;

                case Module.Unfollow:
                    return threadNaming_Unfollow_;

                case Module.ProfileManager:
                    return threadNaming_ProfileManager_;

                default:
                    return "";
            }



        }

        public void modModule(string module)
        {
            try
            {
                switch (module)
                {
                    case "WaitAndReply_":
                        Log("[ " + DateTime.Now + " ] => [ Scheduler started for : " + module + " ]");
                        RaiseSchedulerEvent(Module.WaitAndReply);
                        break;

                    case "Tweet_":
                        Log("[ " + DateTime.Now + " ] => [ Scheduler started for : " + module + " ]");
                        RaiseSchedulerEvent(Module.Tweet);
                        //return Module.Tweet;
                        break;

                    case "Retweet_":
                        //return Module.Retweet;
                        break;

                    case "Follow":
                        //return Module.Follow;
                        Log("[ " + DateTime.Now + " ] => [ Scheduler started for : " + module + " ]");
                        RaiseSchedulerEvent(Module.Follow);
                        break;

                    case "Unfollow_":
                        //return Module.Unfollow;
                        break;

                    case "ProfileManager_":
                        //return Module.ProfileManager;
                        break;

                    //case Module.ProfileManager:
                    //    return threadNaming_ProfileManager_;

                    default:
                        break;
                    //return Module.None;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Error -->  modModule(string module) switch (module)--> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        } 

        #region Stopping Variables

        string threadNaming_WaitAndReply_ = "WaitAndReply_";
        string threadNaming_Tweet_ = "Tweet_";
        string threadNaming_Retweet_ = "Retweet_";
        string threadNaming_Reply_ = "Reply_";
        string threadNaming_Follow_ = "Follow_";
        string threadNaming_Unfollow_ = "Unfollow_";
        string threadNaming_ProfileManager_ = "ProfileManager_";

        #endregion

        private void RaiseSchedulerEvent(Module module)
        {
            EventsArgs eArgs = new EventsArgs(module);
            Event_StartScheduler.RaiseScheduler(eArgs);
        }

        private void Log(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            SchedulerLogger.LogText(eArgs);
        }

        private void dgvScheduler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmScheduler_FormClosed(object sender, FormClosedEventArgs e)
        {
            Unfollower.IsSheduler_NewUi = true;
        }

       

       
       
    }
}
