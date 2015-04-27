using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Globussoft;
using BaseLib;
using System.Data.SQLite;
using twtboardpro;
using System.Threading;
using System.IO;
using System.Drawing.Drawing2D;

namespace twtboardpro
{
    public partial class frmAccounts : Form
    {
        public frmAccounts()
        {
            InitializeComponent();
            //BaseLib.DataBaseHandler.CONstr = "Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\twtboardpro\\DB_twtboardpro.db" + ";Version=3;"; 
        }

        public static BaseLib.Events AccountsLogEvents = new BaseLib.Events();

        clsFBAccount objclsFBAccount = new clsFBAccount();

        List<string> ValidProxies = new List<string>();

        List<string> ValidPrivateProxies = new List<string>();
        private System.Drawing.Image image;
        public Dictionary<string, TweetAccountManager> dictionary_TweetAccountOne = new Dictionary<string, TweetAccountManager>();

        private void AddToListBox(string log)
        {
            try
            {
                if (lstLogger.InvokeRequired)
                {
                    lstLogger.Invoke(new MethodInvoker(delegate
                        {
                            lstLogger.Items.Add(log);
                            lstLogger.SelectedIndex = lstLogger.Items.Count - 1;
                        }));
                }
                else
                {
                    lstLogger.Items.Add(log);
                    lstLogger.SelectedIndex = lstLogger.Items.Count - 1;
                }
            }
            catch (Exception)
            {
            }
        }


        private void btnLoadAccounts_Click(object sender, EventArgs e)
        {
            try
            {
                //System.Threading.Thread loadAccountsThread = new System.Threading.Thread(LoadAccounts);
                System.Threading.Thread loadAccountsThread = new System.Threading.Thread(LoadAccountsMod);
                loadAccountsThread.SetApartmentState(System.Threading.ApartmentState.STA);
                loadAccountsThread.Start();
            }
            catch { }

        }
       

        private void LoadAccounts()
        {

            using (OpenFileDialog ofd = new OpenFileDialog())
            {

                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);


                    foreach (string item in templist)
                    {
                        try
                        {
                            ThreadPool.SetMaxThreads(50, 50);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadAccountswithMultithreaded), new object[] { item });
                            Thread.Sleep(200);
                        }
                        catch { }

                       
                    }

                }

            }

            //Load Data Grid View
            new System.Threading.Thread(() =>
            {
                LoadDataGrid();
            }).Start();

           
            #region Commented
            //using (OpenFileDialog ofd = new OpenFileDialog())
            //{
            //    ofd.Filter = "Text Files (*.txt)|*.txt";
            //    ofd.InitialDirectory = Application.StartupPath;
            //    if (ofd.ShowDialog() == DialogResult.OK)
            //    {
            //        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
            //        foreach (string item in templist)
            //        {
            //            if (!Globals.listAccounts.Contains(item))
            //            {
            //                Globals.listAccounts.Add(item);
            //            }
            //        }

            //        if (Globals.listAccounts.Count > 0)
            //        {
            //            string account = Globals.listAccounts[0];

            //            int DataCount = account.Split(':').Length;
            //            if (DataCount == 2)
            //            {
            //                Globals.accountMode = AccountMode.NoProxy;
            //            }
            //            else if (DataCount == 4)
            //            {
            //                Globals.accountMode = AccountMode.PublicProxy;
            //            }
            //            else if (DataCount > 5)
            //            {
            //                Globals.accountMode = AccountMode.PrivateProxy;
            //            }
            //        }

            //        switch (Globals.accountMode)
            //        {
            //            case AccountMode.NoProxy:
            //                foreach (string item in templist)
            //                {
            //                    try
            //                    {
            //                        dgvAccounts.Rows.Add(item.Split(':')[0], item.Split(':')[1], "", "", "", "", "");
            //                        BaseLib.DataBaseHandler.InsertQuery("insert into tb_FBAccount values(" + item.Split(':')[0] + "," + item.Split(':')[1] + ")", "tb_FBAccount");
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        MessageBox.Show("The Accounts file is in Invalid format. The correct format is \n"+
            //                        "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");

            //                    }
            //                }
            //                break;
            //            case AccountMode.PublicProxy:
            //                foreach (string item in templist)
            //                {
            //                    dgvAccounts.Rows.Add(item.Split(':')[0], item.Split(':')[1], item.Split(':')[2], item.Split(':')[3], "", "", "");
            //                }
            //                break;
            //            case AccountMode.PrivateProxy:
            //                foreach (string item in templist)
            //                {
            //                    dgvAccounts.Rows.Add(item.Split(':')[0], item.Split(':')[1], item.Split(':')[2], item.Split(':')[3],item.Split(':')[4], item.Split(':')[5], "");
            //                }
            //                break;
            //            default:
            //                break;
            //        }

            //        Console.WriteLine(Globals.listAccounts.Count + " Accounts loaded");
            //        AddToManagePages(Globals.listAccounts.Count + " Accounts loaded");
            //    }
            //} 
            #endregion
        }

        private void LoadAccountswithMultithreaded(object acc)
        {
            Array paramsArray = new object[0];
            paramsArray = (Array)acc;
            string account = Convert.ToString(((object[])(paramsArray))[0]);
            string[] AccArr = account.Split(':');

            string accountUser = string.Empty;
            string accountPass = string.Empty;
            string proxyAddress = string.Empty;
            string proxyPort = string.Empty;
            string proxyUserName = string.Empty;
            string proxyPassword = string.Empty;

            if (AccArr.Count() > 1)
            {
                accountUser = account.Split(':')[0];
                accountPass = account.Split(':')[1];
                proxyAddress = string.Empty;
                proxyPort = string.Empty;
                proxyUserName = string.Empty;
                proxyPassword = string.Empty;

                int DataCount = account.Split(':').Length;
                if (DataCount == 2)
                {
                    //Globals.accountMode = AccountMode.NoProxy;

                }
                else if (DataCount == 4)
                {
                    //Globals.accountMode = AccountMode.PublicProxy;
                    proxyAddress = account.Split(':')[2];
                    proxyPort = account.Split(':')[3];
                }
                else if (DataCount > 5)
                {
                    //Globals.accountMode = AccountMode.PrivateProxy;
                    proxyAddress = account.Split(':')[2];
                    proxyPort = account.Split(':')[3];
                    proxyUserName = account.Split(':')[4];
                    proxyPassword = account.Split(':')[5];
                }

                ///Set this to "0" if loading unprofiled accounts
                string profileStatus = "0";

                ///Insert new accounts in Database
                try
                {
                    ///Add to Database
                    BaseLib.DataBaseHandler.InsertQuery("Insert into tb_FBAccount values('" + accountUser + "','" + accountPass + "','" + proxyAddress + "','" + proxyPort + "','" + proxyUserName + "','" + proxyPassword + "','" + "" + "','" + profileStatus + "')", "tb_FBAccount");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
            else
            {
                AddToListBox("[ " + DateTime.Now + " ] => [ Account has some problem : " + accountUser + " ]");
            }
        }

        int counter = 0;
        private void LoadAccountsModewithMultithreaded(object acc)
        {
            Array paramsArray = new object[0];
            paramsArray = (Array)acc;
            string account = Convert.ToString(((object[])(paramsArray))[0]);
            string[] AccArr = account.Split(':');

            string accountUser = string.Empty;
            string accountPass = string.Empty;
            string screanName = string.Empty;
            string proxyAddress = string.Empty;
            string proxyPort = string.Empty;
            string proxyUserName = string.Empty;
            string proxyPassword = string.Empty;
            string status = string.Empty;

            if (AccArr.Count() > 1)
            {
                accountUser = account.Split(':')[0];
                accountPass = account.Split(':')[1];
                screanName = string.Empty;
                proxyAddress = string.Empty;
                proxyPort = string.Empty;
                proxyUserName = string.Empty;
                proxyPassword = string.Empty;
                status = string.Empty;

                int DataCount = account.Split(':').Length;
                if (DataCount == 2)
                {
                    //Globals.accountMode = AccountMode.NoProxy;
                }
                else if (DataCount == 3)
                {
                    //Globals.accountMode = AccountMode.PublicProxy;
                    screanName = account.Split(':')[2];
                }
                else if (DataCount == 4)
                {
                    //Globals.accountMode = AccountMode.PublicProxy;
                    proxyAddress = account.Split(':')[2];
                    proxyPort = account.Split(':')[3];
                }
                else if (DataCount == 5)
                {
                    //Globals.accountMode = AccountMode.PublicProxy;
                    screanName = account.Split(':')[2];
                    proxyAddress = account.Split(':')[3];
                    proxyPort = account.Split(':')[4];
                }
                
                else if (DataCount > 5 && DataCount < 7)
                {
                    //Globals.accountMode = AccountMode.PrivateProxy;
                    proxyAddress = account.Split(':')[2];
                    proxyPort = account.Split(':')[3];
                    proxyUserName = account.Split(':')[4];
                    proxyPassword = account.Split(':')[5];
                    //dt.Rows.Add(accountUser, accountPass, string.Empty , string.Empty, proxyAddress, proxyPort, proxyUserName, proxyPassword, "", "0");
                }
                else if (DataCount == 7)
                {
                    //Globals.accountMode = AccountMode.PrivateProxy;
                    screanName = account.Split(':')[2];
                    proxyAddress = account.Split(':')[3];
                    proxyPort = account.Split(':')[4];
                    proxyUserName = account.Split(':')[5];
                    proxyPassword = account.Split(':')[6];
                    //dt.Rows.Add(accountUser, accountPass, string.Empty , string.Empty, proxyAddress, proxyPort, proxyUserName, proxyPassword, "", "0");
                }
                
                if (Globals.IsFreeVersion)
                {
                    if (dgvAccounts.Rows.Count >= 5)
                    {
                        FrmFreeTrial frmFreeTrial = new FrmFreeTrial();
                        frmFreeTrial.TopMost = true;
                        frmFreeTrial.BringToFront();
                        frmFreeTrial.ShowDialog();
                        //break;
                    }
                }

                //dt.Rows.Add(accountUser, accountPass, screanName, string.Empty, proxyAddress, proxyPort, proxyUserName, proxyPassword, "", "0");
                BaseLib.DataBaseHandler.InsertQuery("INSERT INTO tb_FBAccount (UserName, Password, Screen_Name, FollowerCount, ProxyAddress, ProxyPort, ProxyUserName, ProxyPassword, ProfileName, ProfileStatus, GroupName , Status) VALUES ('" + accountUser + "','" + accountPass + "', '" + screanName + "', '' , '" + proxyAddress + "','" + proxyPort + "','" + proxyUserName + "','" + proxyPassword + "','" + "" + "' , '" + "" + "' ,'" + "" + "', '0')", "tb_FBAccount");

                try
                {
                    TweetAccountManager twitter = new TweetAccountManager();
                    twitter.Username = accountUser;
                    twitter.Password = accountPass;
                    twitter.proxyAddress = proxyAddress;
                    twitter.proxyPort = proxyPort;
                    twitter.proxyUsername = proxyUserName;
                    twitter.proxyPassword = proxyPassword;
                    twitter.profileStatus = 0;
                    twitter.AccountStatus = "";

                    LoadDataGridAfterAccountCreation();
                    if (!string.IsNullOrEmpty(twitter.Username))
                    {
                        TweetAccountContainer.dictionary_TweetAccount.Add(twitter.Username, twitter);
                        dictionary_TweetAccountOne.Add(twitter.Username, twitter);
                    }

                    
                   
                }
                catch { }

            }
            else
            {
                AddToListBox("[ " + DateTime.Now + " ] => [ Account has some problem : " + accountUser + " ]");
            }
        }

        DataSet ds = null;
        List<string> lst_AccountForUpload = new List<string>();
        private void LoadAccountsMod()
        {
            try
            {
                if (Globals.IsFreeVersion)
                {
                    string DeleteQuery = "Delete from tb_FBAccount";
                    DataBaseHandler.DeleteQuery(DeleteQuery, "tb_FBAccount");

                    LoadDataGrid();
                }

                //DataTable dt_Existing = objclsFBAccount.SelectAccoutsForGridView();

                //if (Globals.IsFreeVersion)
                //{
                //    if (dt_Existing.Rows.Count >= 5)
                //    {
                //        FrmFreeTrial frmFreeTrial = new FrmFreeTrial();
                //        frmFreeTrial.ShowDialog();
                //    }
                //}

                //DataSet ds = new DataSet();

                DataTable dt = new DataTable();


                using (OpenFileDialog ofd = new OpenFileDialog())
                {

                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        dt.Columns.Add("UserName");
                        dt.Columns.Add("Password");
                        dt.Columns.Add("ScreenName");
                        dt.Columns.Add("FollowerCount");
                        dt.Columns.Add("ProxyAddress");
                        dt.Columns.Add("ProxyPort");
                        dt.Columns.Add("ProxyUserName");
                        dt.Columns.Add("ProxyPassword");
                        dt.Columns.Add("ProfileName");
                        dt.Columns.Add("ProfileStatus");
                        dt.Columns.Add("Group Name");
                        dt.Columns.Add("Status");
                        ds = new DataSet();
                        ds.Tables.Add(dt);

                        dgvAccounts.Invoke(new MethodInvoker(delegate
                        {
                            textBox12.Text = ofd.FileName;
                            clsDBQueryManager ObjclsDBQueryManager = new clsDBQueryManager();
                            ObjclsDBQueryManager.InsertOrUpdateSetting("LoadAccounts", "LoadAccounts", StringEncoderDecoder.Encode(textBox12.Text));
                            dgvAccounts.DataSource = null;
                        }));

                        lst_AccountForUpload = GlobusFileHelper.ReadFiletoStringList(ofd.FileName).Distinct().ToList();

                        if (Globals.IsFreeVersion)
                        {

                            try
                            {
                                lst_AccountForUpload.RemoveRange(5, lst_AccountForUpload.Count - 5);
                            }
                            catch { }

                        }
                        //try
                        //{
                        //    if (PnlPicLodder.Visible != true || PnlPicLodder.Visible != true)
                        //    {
                        //        PnlPicLodder.Invoke(new MethodInvoker(delegate
                        //        {
                        //            PnlPicLodder.Visible = true;
                        //        }));
                        //        PnlPicLodder.Invoke(new MethodInvoker(delegate
                        //        {
                        //            PicLodder.Visible = true;
                        //        }));
                        //    }
                        //}
                        //catch
                        //{

                        //}

                        foreach (string item in lst_AccountForUpload)
                        {
                            try
                            {
                                ThreadPool.SetMaxThreads(500, 300);
                                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadAccountsModewithMultithreaded), new object[] { item });
                                Thread.Sleep(50);
                            }
                            catch { }
                        }
                       

                        //LoadDataGridAfterAccountCreation();

                        //#region commented -- prabhat -- 15.02.14
                        //dgvAccounts.Invoke(new MethodInvoker(delegate
                        //{
                        //    dgvAccounts.DataSource = dt;
                        //}));

                        //AddToListBox("[ " + DateTime.Now + " ] => [ " + dt.Rows.Count + " Accounts Loaded ]");
                        //Log("[ " + DateTime.Now + " ] => [ " + dt.Rows.Count + " Accounts Loaded ]"); 
                        //#endregion
                    }

                    #region commented -- prabhat -- 15.02.14
                    //foreach (DataRow dRow in dt.Rows)
                    //{

                    //    string accountUser = dRow[0].ToString();
                    //    string accountPass = dRow[1].ToString();
                    //    string screanName = dRow[2].ToString();
                    //    string proxyAddress = dRow[4].ToString();
                    //    string proxyPort = dRow[5].ToString();
                    //    string proxyUserName = dRow[6].ToString();
                    //    string proxyPassword = dRow[7].ToString();
                    //    string profilename = dRow[8].ToString();
                    //    string profileStatus = dRow[9].ToString();
                    //    string groupname = dRow[10].ToString();
                    //    string status = dRow[11].ToString();
                    //    ///Insert new accounts in Database
                    //    try
                    //    {
                    //        //Add to Database
                    //        BaseLib.DataBaseHandler.InsertQuery("INSERT INTO tb_FBAccount (UserName, Password, Screen_Name, FollowerCount, ProxyAddress, ProxyPort, ProxyUserName, ProxyPassword, ProfileName, ProfileStatus, GroupName , Status) VALUES ('" + accountUser + "','" + accountPass + "', '" + screanName + "', '' , '" + proxyAddress + "','" + proxyPort + "','" + proxyUserName + "','" + proxyPassword + "','" + profilename + "' , '" + profileStatus + "' ,'" + groupname + "', '" + status + "')", "tb_FBAccount");
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.StackTrace);

                    //        GlobusFileHelper.AppendStringToTextfileNewLine("Method Name :- LoadAccountsMod   Error :- " + ex.StackTrace, Globals.Path_AccountUploadingErrorLog);
                    //    }

                    //} 
                    #endregion

                    try
                    {
                        PnlPicLodder.Invoke(new MethodInvoker(delegate
                                   {
                                       PnlPicLodder.Visible = false;
                                   }));
                        PnlPicLodder.Invoke(new MethodInvoker(delegate
                        {
                            PicLodder.Visible = false;
                        }));
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("Method Name :- LoadAccountsMod   Error :- " + ex.StackTrace, Globals.Path_AccountUploadingErrorLog);
                    }

                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Method Name :- LoadAccountsMod   Error :- " + ex.StackTrace, Globals.Path_AccountUploadingErrorLog);
            }
            finally
            {
                Thread.Sleep(100 * lst_AccountForUpload.Count);
                AddToListBox("[ " + DateTime.Now + " ] => [ " + lst_AccountForUpload.Count + " Accounts loaded ]");
            }
        }

   
        public void LoadDataGrid()
        {
            try
            {
                DataTable dt = objclsFBAccount.SelectAccoutsForGridView();
               
                //ds = new DataSet();
                //ds.Tables.Add(dt);

                dgvAccounts.Invoke(new MethodInvoker(delegate
                {
                    dgvAccounts.DataSource = dt;
                    
                }));

                Globals.listAccounts.Clear();
                TweetAccountContainer.dictionary_TweetAccount.Clear();
               ///Add Twitter instances to TweetAccountContainer.dictionary_TweetAccount

                foreach (DataRow dRow in dt.Rows)
                {
                    try
                    {
                        TweetAccountManager twitter = new TweetAccountManager();
                        twitter.Username = dRow[0].ToString();
                        twitter.Password = dRow[1].ToString();
                        twitter.Screen_name = dRow[2].ToString();
                        twitter.FollowerCount = dRow[3].ToString();
                        twitter.FollwingCount = dRow[4].ToString();
                        twitter.proxyAddress = dRow[5].ToString();
                        twitter.proxyPort = dRow[6].ToString();
                        twitter.proxyUsername = dRow[7].ToString();
                        twitter.proxyPassword = dRow[8].ToString();
                        twitter.GroupName = dRow[11].ToString();
                        twitter.AccountStatus = dRow[12].ToString();

                        //if (!string.IsNullOrEmpty(dRow[8].ToString()))
                        //{
                        //    facebooker.profileStatus = int.Parse(dRow[7].ToString());
                        //}
                        if (!string.IsNullOrEmpty(twitter.Username))
                        {
                            if (twitter.AccountStatus != "Suspended")
                            {
                                Globals.listAccounts.Add(twitter.Username + ":" + twitter.Password + ":" + twitter.proxyAddress + ":" + twitter.proxyPort + ":" + twitter.proxyUsername + ":" + twitter.proxyPassword + ":" + twitter.GroupName + ":" + twitter.AccountStatus);
                                TweetAccountContainer.dictionary_TweetAccount.Add(twitter.Username, twitter);

                                dictionary_TweetAccountOne.Add(twitter.Username, twitter);
                                //Checked if user working on free version the it will not allowed to use mex 5 accounts.
                                if (Globals.IsFreeVersion)
                                {
                                    if (TweetAccountContainer.dictionary_TweetAccount.Count >= 5)
                                    {
                                        if (TweetAccountContainer.dictionary_TweetAccount.Count >= 5)
                                        {
                                            FrmFreeTrial frmFreeTrial = new FrmFreeTrial();
                                            frmFreeTrial.TopMost = true;
                                            frmFreeTrial.BringToFront();
                                            frmFreeTrial.ShowDialog();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Method Name :- LoadDataGrid   >>  Error :- " + ex.StackTrace, Globals.Path_AccountUploadingErrorLog);
                    }
                }

                Console.WriteLine(Globals.listAccounts.Count + " Accounts loaded");
                //AddToListBox("[ " + DateTime.Now + " ] => [ " + Globals.listAccounts.Count + " Accounts loaded ]");
                //Log("[ " + DateTime.Now + " ] => [ " + Globals.listAccounts.Count + " Accounts loaded ]");
                this.Invoke(new MethodInvoker(delegate
                {
                    if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                    {
                        lblAcccountStatus.Text = " " + TweetAccountContainer.dictionary_TweetAccount.Count + " Accounts loaded";
                        AddToListBox("[ " + DateTime.Now + " ] => [ " + TweetAccountContainer.dictionary_TweetAccount.Count + " Accounts loaded ]");
                        Log("[ " + DateTime.Now + " ] => [ " + TweetAccountContainer.dictionary_TweetAccount.Count + " Accounts loaded ]");
                    }
                }));
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Method Name :- LoadDataGrid   >>  Error :- " + ex.StackTrace, Globals.Path_AccountUploadingErrorLog);
            }
            
            
        }

        private void LoadDataGridAfterAccountCreation()
        {
           
            try
            {
                DataTable dt = objclsFBAccount.SelectAccoutsForGridView();

                //ds = new DataSet();
                //ds.Tables.Add(dt);

                dgvAccounts.Invoke(new MethodInvoker(delegate
                {
                    dgvAccounts.DataSource = dt;
                }));

                Globals.listAccounts.Clear();
                TweetAccountContainer.dictionary_TweetAccount.Clear();
                ///Add Twitter instances to TweetAccountContainer.dictionary_TweetAccount

                foreach (DataRow dRow in dt.Rows)
                {
                    try
                    {
                        TweetAccountManager twitter = new TweetAccountManager();
                        twitter.Username = dRow[0].ToString();
                        twitter.Password = dRow[1].ToString();
                        twitter.Screen_name = dRow[2].ToString();
                        twitter.FollowerCount = dRow[3].ToString();
                        twitter.FollwingCount = dRow[4].ToString();
                        twitter.proxyAddress = dRow[5].ToString();
                        twitter.proxyPort = dRow[6].ToString();
                        twitter.proxyUsername = dRow[7].ToString();
                        twitter.proxyPassword = dRow[8].ToString();
                        twitter.GroupName = dRow[11].ToString();
                        twitter.AccountStatus = dRow[12].ToString();

                        //if (!string.IsNullOrEmpty(dRow[8].ToString()))
                        //{
                        //    facebooker.profileStatus = int.Parse(dRow[7].ToString());
                        //}
                        if (!string.IsNullOrEmpty(twitter.Username))
                        {
                            if (twitter.AccountStatus != "Suspended")
                            {
                                Globals.listAccounts.Add(twitter.Username + ":" + twitter.Password + ":" + twitter.proxyAddress + ":" + twitter.proxyPort + ":" + twitter.proxyUsername + ":" + twitter.proxyPassword + ":" + twitter.GroupName + ":" + twitter.AccountStatus);
                                TweetAccountContainer.dictionary_TweetAccount.Add(twitter.Username, twitter);

                                //Checked if user working on free version the it will not allowed to use mex 5 accounts.
                                if (Globals.IsFreeVersion)
                                {
                                    if (TweetAccountContainer.dictionary_TweetAccount.Count >= 5)
                                    {
                                        if (TweetAccountContainer.dictionary_TweetAccount.Count >= 5)
                                        {
                                            FrmFreeTrial frmFreeTrial = new FrmFreeTrial();
                                            frmFreeTrial.TopMost = true;
                                            frmFreeTrial.BringToFront();
                                            frmFreeTrial.ShowDialog();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Method Name :- LoadDataGrid   >>  Error :- " + ex.StackTrace, Globals.Path_AccountUploadingErrorLog);
                    }
                }
               
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Method Name :- LoadDataGrid   >>  Error :- " + ex.StackTrace, Globals.Path_AccountUploadingErrorLog);
            }
        
            

            this.Invoke(new MethodInvoker(delegate
            {
                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                {
                    lblAcccountStatus.Text = " " + TweetAccountContainer.dictionary_TweetAccount.Count + " Accounts loaded (" + lst_AccountForUpload.Count() + ") ";
                    //AddToListBox("[ " + DateTime.Now + " ] => [ " + TweetAccountContainer.dictionary_TweetAccount.Count + " Accounts loaded ]");
                    //Log("[ " + DateTime.Now + " ] => [ " + TweetAccountContainer.dictionary_TweetAccount.Count + " Accounts loaded ]");
                }
            }));

           
        }
        private void Log(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            AccountsLogEvents.LogText(eventArgs);
        }

        private void frmAccounts_Load(object sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
            new Thread(() =>
            {
                image = Properties.Resources.app_bg;
                dictionary_TweetAccountOne = new Dictionary<string, TweetAccountManager>();
                if (Globals.IsFreeVersion)
                {
                    FrmFreeTrial frmFreeTrial = new FrmFreeTrial();
                    frmFreeTrial.TopMost = true;
                    frmFreeTrial.BringToFront();
                    frmFreeTrial.ShowDialog();
                }

                LoadDataGrid();
                LoadFileData();
                checkFreeTrialAccount();
                btnRefresh.Invoke(new MethodInvoker(delegate
                {
                    btnRefresh.Enabled = true;

                }));
            }).Start();
        }

        public void checkFreeTrialAccount()
        {
            if (Globals.IsFreeVersion)
            {

                try
                {
                    DataTable dt1 = objclsFBAccount.SelectAccoutsForGridView();

               this.Invoke(new MethodInvoker(delegate
               {
                   if (dt1.Rows.Count == 5)
                   {
                       btnLoadAccounts.Enabled = false;
                       btnClearAccounts.Enabled = false;
                   }
               }));
                }
                catch (Exception)
                {

                }

            }
        }

        public void LoadFileData()
        {
            string accountData = string.Empty;
            string laoddata = string.Empty;

            clsDBQueryManager DataBase = new clsDBQueryManager();
            DataTable dt = DataBase.SelectSettingData();
            foreach (DataRow row in dt.Rows)
            {
                if ("LoadAccounts" == row[1].ToString())
                {
                    accountData = StringEncoderDecoder.Decode(row[2].ToString());
                }
            }
            this.Invoke(new MethodInvoker(delegate
                {
                    if (File.Exists(accountData))
                    {
                        textBox12.Text = accountData;
                        AddToListBox("[ " + DateTime.Now + " ] => [ Last Accounts Loaded From : " + accountData + " ]");
                    }
                    else
                    {
                        //AddToListBox("[ " + DateTime.Now + " ] => [ File : " + accountData + " : Does not exsist ]");
                        AddToListBox("[ " + DateTime.Now + " ] => [ Please upload accounts ]");
                    }
                }));
        }
        private void btnClearAccounts_Click(object sender, EventArgs e)
        {
            if (dgvAccounts.Rows.Count <= 1)
            {
                MessageBox.Show("There are no Accounts in DataBase");
                return;
            }
            else if (MessageBox.Show("Do you really want to delete all the accounts from Database", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string DeleteQuery = "Delete from tb_FBAccount";
                DataBaseHandler.DeleteQuery(DeleteQuery, "tb_FBAccount");
                TweetAccountContainer.dictionary_TweetAccount.Clear();
                LoadDataGrid();
                AddToListBox("[ " + DateTime.Now + " ] => [ Accounts Cleared ]");
            }

            lblAcccountStatus.Text = string.Empty;
        }

        List<string> lstProxies = new List<string>();

        ProxyUtilitiesFromDataBase proxyFetcher = new ProxyUtilitiesFromDataBase();

        private void btnAssignProxy_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Assign Private Proxies from Database???", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    List<string> lstProxies = proxyFetcher.GetPrivateProxies();
                    if (lstProxies.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
                        {
                            accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
                        }
                        proxyFetcher.AssignProxiesToAccounts(lstProxies, accountsPerProxy);//AssignProxiesToAccounts(lstProxies);
                        LoadDataGrid();   //Refresh Datagrid
                    }
                    else
                    {
                        MessageBox.Show("Please assign private proxies from Proxies Tab in Main Page OR Upload a proxies Text File");
                    }
                }
                catch { }
            }
            else
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        lstProxies = new List<string>();

                        lstProxies = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
                        {
                            accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
                        }
                        proxyFetcher.AssignProxiesToAccounts(lstProxies, accountsPerProxy);//AssignProxiesToAccounts(lstProxies);
                        LoadDataGrid();   //Refresh Datagrid
                        //AssignProxiesToAccountsModified();
                    }
                }
            }
        }


        private void ButonClearProxies_Click(object sender, EventArgs e)
        {
            if (dgvAccounts.Rows.Count <= 1)
            {
                MessageBox.Show("There are no Accounts in DataBase");
                return;
            }
            else
            {
                new Thread(() =>
                    {

                        #region Remove Proxy from database
                        try
                        {
                            DataSet ds = new DataSet();

                            using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                            {

                                //using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE ProxyAddress = '" + proxyAddress + "'", con))
                                using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount", con))
                                {
                                    ad.Fill(ds);

                                    if (ds.Tables[0].Rows.Count >= 1)
                                    {
                                        if (MessageBox.Show("Do you really want to clear all proxies from Accounts", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            AddToListBox("[ " + DateTime.Now + " ] => [ Please wait... ]");

                                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                            {
                                                string UpdateQuery = "Update tb_FBAccount Set ProxyAddress='" + "" + "', ProxyPort='" + "" + "', ProxyUserName='" + "" + "', ProxyPassword='" + "" + "' WHERE UserName='" + ds.Tables[0].Rows[i]["UserName"].ToString() + "'";
                                                DataBaseHandler.UpdateQuery(UpdateQuery, "tb_FBAccount");
                                            }

                                            AddToListBox("[ " + DateTime.Now + " ] => [ Proxies cleared ]");
                                            MessageBox.Show("Proxies cleared");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No Proxies To Clear");
                                    }

                                }

                                LoadDataGrid(); 
                            }
                        }

                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error in Clearing Proxies :- " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                        #endregion


                    }).Start();
            }

        }


        int accountsPerProxy = 10;  //Change this to change Number of Accounts to be set per proxy
        static int i = 0;
        /// <summary>
        /// Assigns "accountsPerProxy" number of proxies to accounts in Database, only picks up only those accounts where ProxyAddress is Null or Empty
        /// </summary>
        private void AssignProxiesToAccounts(List<string> lstProxies)
        {
            //string SelectQuery = "Select * from tb_FBAccount";
            //DataSet initialDS = DataBaseHandler.SelectQuery(SelectQuery, "tb_FBAccount");

            DataSet ds = new DataSet();

            if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
            {
                accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
            }
            else
            {
                MessageBox.Show("You entered invalid Accounts per Proxy... Default value \"10\" Set");
            }

            using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
            {

                foreach (string item in lstProxies)
                {
                    ds.Clear();

                    string account = item;

                    string proxyAddress = string.Empty;
                    string proxyPort = string.Empty;
                    string proxyUserName = string.Empty;
                    string proxyPassword = string.Empty;

                    int DataCount = account.Split(':').Length;

                    if (DataCount == 2)
                    {
                        proxyAddress = account.Split(':')[0];
                        proxyPort = account.Split(':')[1];
                    }
                    else if (DataCount > 2)
                    {
                        proxyAddress = account.Split(':')[0];
                        proxyPort = account.Split(':')[1];
                        proxyUserName = account.Split(':')[2];
                        proxyPassword = account.Split(':')[3];
                    }

                    //using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE ProxyAddress = '" + proxyAddress + "'", con))
                    using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE ProxyAddress = '" + proxyAddress + "' and ProxyPort = '" + proxyPort + "'", con))
                    {
                        ad.Fill(ds);
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            ds.Clear();
                            using (SQLiteDataAdapter ad1 = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE ProxyAddress = '" + "" + "' OR ProxyAddress = '" + null + "'", con))
                            {
                                ad1.Fill(ds);

                                int count = accountsPerProxy;  //Set count = accountsPerProxy so that it sets max this number of accounts to each proxy
                                if (ds.Tables[0].Rows.Count < count)
                                {
                                    count = ds.Tables[0].Rows.Count;
                                }
                                for (int i = 0; i < count; i++)
                                {
                                    string UpdateQuery = "Update tb_FBAccount Set ProxyAddress='" + proxyAddress + "', ProxyPort='" + proxyPort + "', ProxyUserName='" + proxyUserName + "', ProxyPassword='" + proxyPassword + "' WHERE UserName='" + ds.Tables[0].Rows[i]["UserName"].ToString() + "'";
                                    DataBaseHandler.UpdateQuery(UpdateQuery, "tb_FBAccount");
                                }
                            }

                        }
                    }

                }

            }

            LoadDataGrid();   //Refresh Datagrid

        }

        private void AssignProxiesToAccountsModified()
        {
            //string SelectQuery = "Select * from tb_FBAccount";
            //DataSet initialDS = DataBaseHandler.SelectQuery(SelectQuery, "tb_FBAccount");

            DataSet ds = new DataSet();

            using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
            {

                foreach (string item in lstProxies)
                {
                    ds.Clear();

                    string account = item;

                    string proxyAddress = string.Empty;
                    string proxyPort = string.Empty;
                    string proxyUserName = string.Empty;
                    string proxyPassword = string.Empty;

                    int DataCount = account.Split(':').Length;

                    //if (DataCount == 2)
                    //{
                    proxyAddress = account.Split(':')[0];
                    proxyPort = "7866";
                    proxyUserName = "idea01";
                    proxyPassword = "kkidufg";
                    //}
                    //else if (DataCount > 2)
                    //{
                    //    proxyAddress = account.Split(':')[0];
                    //    proxyPort = account.Split(':')[1];
                    //    proxyUserName = account.Split(':')[2];
                    //    proxyPassword = account.Split(':')[3];
                    //}

                    using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE ProxyAddress = '" + proxyAddress + "'", con))
                    {
                        ad.Fill(ds);
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            ds.Clear();
                            using (SQLiteDataAdapter ad1 = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE ProxyAddress = '" + "" + "' and ProxyAddress = '" + null + "'", con))
                            {
                                ad1.Fill(ds);

                                int count = 10;
                                if (ds.Tables[0].Rows.Count < 10)
                                {
                                    count = ds.Tables[0].Rows.Count;
                                }
                                for (int i = 0; i < count; i++)
                                {
                                    //ds.Tables[0].Rows[i]["ProxyAddress"] = proxyAddress;
                                    //ds.Tables[0].Rows[i]["ProxyPort"] = proxyPort;
                                    //ds.Tables[0].Rows[i]["ProxyUserName"] = proxyUserName;
                                    //ds.Tables[0].Rows[i]["ProxyPassword"] = proxyPassword;

                                    string UpdateQuery = "Update tb_FBAccount Set ProxyAddress='" + proxyAddress + "', ProxyPort='" + proxyPort + "', ProxyUserName='" + proxyUserName + "', ProxyPassword='" + proxyPassword + "' WHERE UserName='" + ds.Tables[0].Rows[i]["UserName"].ToString() + "'";
                                    DataBaseHandler.UpdateQuery(UpdateQuery, "tb_FBAccount");
                                }
                            }

                        }
                    }

                }

            }

            LoadDataGrid();


        }

        bool IsExport = false;
        private void btnExportAccounts_Click(object sender, EventArgs e)
        {
            if (dgvAccounts.Rows.Count <= 1)
            {
                MessageBox.Show("There are no Accounts in DataBase");
                return;
            }

            if (chkExportAccounts.Checked)
            {
                string str = string.Empty;
                for (int i = 0; i < chkListBoxExportAccount.Items.Count; i++)
                {
                    if (chkListBoxExportAccount.GetItemChecked(i))
                    {
                         str = (string)chkListBoxExportAccount.Items[i];
                    }
                }
                if (string.IsNullOrEmpty(str))
                {
                    MessageBox.Show("Please check any one option for export.");
                    return;
                }

            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ExportAccountsToFile(ofd.FileName);
                    if (IsExport)
                    {
                        AddToListBox("Accounts Exported to ---" + ofd.FileName);
                    }
                    else {
                        AddToListBox("Accounts Not Exported .");
                    }
                }
            }
        }

        private void ExportAccountsToFile(string filePath)
        {
            
            DataTable dt = objclsFBAccount.SelectAccoutsForGridView();

            foreach (DataRow dRow in dt.Rows)
            {
                try
                {
                    string user = dRow[0].ToString();
                    string pass = dRow[1].ToString();
                    string Username = dRow[2].ToString();
                    string proxyAdd = dRow[5].ToString();
                    string proxyPort = dRow[6].ToString();
                    string proxyUser = dRow[7].ToString();
                    string proxyPass = dRow[8].ToString();
                    if (!chkExportAccounts.Checked)
                    {
                        string data = user + ":" + pass + ":" + Username + ":" + proxyAdd + ":" + proxyPort + ":" + proxyUser + ":" + proxyPass;
                        GlobusFileHelper.AppendStringToTextfileNewLine(data, filePath);
                        IsExport = true;
                    }
                    else
                    {
                        for (int i = 0; i < chkListBoxExportAccount.Items.Count; i++)
                        {
                            if (chkListBoxExportAccount.GetItemChecked(i))
                            {
                                string str = (string)chkListBoxExportAccount.Items[i];

                                if ((!string.IsNullOrEmpty(proxyAdd)) && (!string.IsNullOrEmpty(proxyPort)) && string.IsNullOrEmpty(proxyUser) && string.IsNullOrEmpty(proxyPass))
                                {
                                    if (str.Contains("Public Proxies Account"))
                                    {
                                        IsExport = true;
                                        string data = user + ":" + pass + ":" + Username + ":" + proxyAdd + ":" + proxyPort + ":" + proxyUser + ":" + proxyPass;
                                        GlobusFileHelper.AppendStringToTextfileNewLine(data, filePath);
                                        break;
                                    }
                                }
                                else if ((!string.IsNullOrEmpty(proxyAdd)) && (!string.IsNullOrEmpty(proxyPort)) && (!string.IsNullOrEmpty(proxyUser)) && (!string.IsNullOrEmpty(proxyPass)))
                                {
                                    if (str.Contains("Private Proxies Account"))
                                    {
                                        IsExport = true;
                                        string data = user + ":" + pass + ":" + Username + ":" + proxyAdd + ":" + proxyPort + ":" + proxyUser + ":" + proxyPass;
                                        GlobusFileHelper.AppendStringToTextfileNewLine(data, filePath);
                                        break;
                                    }
                                }
                                else if ((string.IsNullOrEmpty(proxyAdd)) && (string.IsNullOrEmpty(proxyPort)) && (string.IsNullOrEmpty(proxyUser)) && (string.IsNullOrEmpty(proxyPass)))
                                {
                                    if (str.Contains("Without Proxies Account"))
                                    {
                                        IsExport = true;
                                        string data = user + ":" + pass + ":" + Username + ":" + proxyAdd + ":" + proxyPort + ":" + proxyUser + ":" + proxyPass;
                                        GlobusFileHelper.AppendStringToTextfileNewLine(data, filePath);
                                        break;
                                    }
                                }
                                else if (string.IsNullOrEmpty(str))
                                {
                                    MessageBox.Show("Please Choose Minimum one option For export.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }

            }

        }

        private void frmAccounts_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmMain_NewUI.IsAccountopen = true;
            PnlPicLodder.Visible = false;
            PicLodder.Visible = false;
            Globals.IsRefreshAccountExecute = true;

            try
            {
                foreach (Thread item in lstThreadAppFollow)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    {
                    }
                }
            }
            catch { }
        }

        string userNameForPasswordUpdate = string.Empty;
        string passwordForPasswordUpdate = string.Empty;

        private void dgvAccounts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Globals.IsFreeVersion)
            {
                if (dgvAccounts.Rows.Count < 1)
                {
                    MessageBox.Show("There are no Accounts in DataBase");
                    return;
                }
                else
                {
                    try
                    {
                        frmAccountPasswordUpdate objfrmAccountPasswordUpdate = new frmAccountPasswordUpdate();
                        TwitterUser objCheckAccount = new TwitterUser();

                        userNameForPasswordUpdate = dgvAccounts.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                        passwordForPasswordUpdate = dgvAccounts.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
                        objCheckAccount.UpdatePassword(userNameForPasswordUpdate, passwordForPasswordUpdate);
                        objfrmAccountPasswordUpdate.Show();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
        }

        private void groupBoxProfileDetails_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g;

            //g = e.Graphics;

            //g.SmoothingMode = SmoothingMode.HighQuality;

            //// Draw the background.
            ////g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            ////// Draw the image.
            //g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

        private void frmAccounts_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g;

            //g = e.Graphics;

            //g.SmoothingMode = SmoothingMode.HighQuality;
            //g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

        private void chkExportAccounts_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkExportAccounts.Checked)
                {
                    chkListBoxExportAccount.Enabled = true;
                }
                else
                {
                    chkListBoxExportAccount.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void chkListBoxExportAccount_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                for (int ix = 0; ix < chkListBoxExportAccount.Items.Count; ++ix)
                    if (ix != e.Index) chkListBoxExportAccount.SetItemChecked(ix, false);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        int counter_Account = 0;
        Thread thread_StartRefresh = null;
        List<Thread> lstThreadAppFollow = new List<Thread>();
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            
           
           new Thread(() =>
           {
               StartRefresh();
           }).Start();
        }

        
        private void StartRefresh()
        {
            Globals.IsRefreshAccountExecute = false;
            counter_Account = TweetAccountContainer.dictionary_TweetAccount.Count;
               //dictionary_TweetAccountOne = new Dictionary<string, TweetAccountManager>();
               //dictionary_TweetAccountOne = TweetAccountContainer.dictionary_TweetAccount;
            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
            {
                foreach (KeyValuePair<string, TweetAccountManager> item in dictionary_TweetAccountOne)
                {
                    try
                    {

                        ThreadPool.QueueUserWorkItem(new WaitCallback(StartCheckingAccount), new object[] { item });
                        Thread.Sleep(1000);

                        //if user is check fast follow option then delay is not working on that condition ...!!

                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> startCheckingAccount() -- foreach loop Foreach Dictiinoary --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }
            }
            else
            {
                AddToListBox("[ " + DateTime.Now + " ] => [ Please Upload Accounts. ]");
            }
        }

        private void StartCheckingAccount(object parameters)
        {

            TweetAccountManager tweetAccountManager = new TweetAccountManager();

            lstThreadAppFollow.Add(Thread.CurrentThread);
            lstThreadAppFollow = lstThreadAppFollow.Distinct().ToList();
            Thread.CurrentThread.IsBackground = true;

            try
            {
                Array paramsArray = new object[3];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);


                AddToListBox("[ " + DateTime.Now + " ] => [ Starting Checking Account : " + keyValue.Key + " ]");

                tweetAccountManager = keyValue.Value;

                if (!tweetAccountManager.IsLoggedIn)
                {
                    tweetAccountManager.Login();
                }
                if (tweetAccountManager.IsLoggedIn)
                {
                    AddToListBox("[ " + DateTime.Now + " ] => [ SuccessFully Logged In with Account:  " + keyValue.Key + " ]");
                }
                if (tweetAccountManager.AccountStatus == "Account Suspended")
                {
                    AddToListBox("[ " + DateTime.Now + " ] => [ Suspended  Account:  " + keyValue.Key + " ]");
                    clsDBQueryManager database = new clsDBQueryManager();
                    database.UpdateSuspendedAcc(tweetAccountManager.Username);
                    return;
                }

                DataTable dt = objclsFBAccount.SelectAccoutsForGridView();

                if (!Globals.IsRefreshAccountExecute)
                {
                    btnRefresh.Invoke(new MethodInvoker(delegate
                    {
                        dgvAccounts.DataSource = dt;

                    }));
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> startCheckingAccount() -- Method --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
            finally
            {
                counter_Account--;
                
                if (counter_Account == 0)
                {
                    if (btnRefresh.InvokeRequired)
                    {
                        btnRefresh.Invoke(new MethodInvoker(delegate
                        {
                            AddToListBox("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddToListBox("---------------------------------------------------------------------------------------------------------------------------");

                        }));
                    }
                }
            }
        }

       

    }
}
