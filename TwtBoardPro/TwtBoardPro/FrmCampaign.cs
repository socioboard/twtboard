using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseLib;
using Globussoft;

namespace twtboardpro
{
    public partial class FrmCampaign : Form
    {
        public FrmCampaign()
        {
            InitializeComponent();
        }

        bool EditCampaign = false;
        bool AddCampaign = false;
        string SelectionType = string.Empty;


        private void btn_FollowKeyWordsFileUpload_Click(object sender, EventArgs e)
        {
            SelectionType = "Username";
            txtKeywordList.Text = "";
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    List<string> tempFile = GlobusFileHelper.ReadLargeFile(ofd.FileName);//GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    txtUsernameList.Text = ofd.FileName;
                }
            }
        }

        private void btnCampaignSetData_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewCampaignName.Text))
            {
                string SelectQuery = "Select * From tb_CampaignData WHERE Campaign_Name = '" + txtNewCampaignName.Text +"'";
                DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "tb_CampaignData");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("Please Enter Another Campaign Name. Name Already In Use");
                }
                else
                {
                    grpBoxCampaignDetaills.Enabled = true;
                    AddCampaign = true;
                }
            }
            else
            {
                MessageBox.Show("Please Enter A Campaign Name");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (EditCampaign)
            {
                string SelectedCampaignName = string.Empty;
                cmboxCampiagnName.Invoke(new MethodInvoker(delegate
                {
                    SelectedCampaignName = cmboxCampiagnName.SelectedItem.ToString();
                }));
                if (SelectionType == "Username")
                {
                    try
                    {
                        string query = "UPDATE tb_CampaignData SET Username_List = '" + txtUsernameList.Text + "' , Keyword_List = '" + "" + "' , Username_Selection = '" + SelectionType + "' WHERE Campaign_Name = '" + SelectedCampaignName + "' ";
                        DataBaseHandler.UpdateQuery(query, "tb_CampaignData");
                        grpBoxCampaignDetaills.Enabled = false;
                        txtUsernameList.Text = "";
                        MessageBox.Show("Edit Campaign Successfully");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else if (SelectionType == "Keyword")
                {
                    try
                    {
                        string query = "UPDATE tb_CampaignData SET Keyword_List = '" + txtKeywordList.Text + "' , Username_List = '" + "" + "', Username_Selection = '" + SelectionType + "' WHERE Campaign_Name = '" + SelectedCampaignName + "' ";
                        DataBaseHandler.UpdateQuery(query, "tb_CampaignData");
                        grpBoxCampaignDetaills.Enabled = false;
                        txtKeywordList.Text = "";
                        MessageBox.Show("Edit Campaign Successfully");
                    }
                    catch (Exception ex)
                    {
 
                    }
                }
                EditCampaign = false;
            }
            else if (AddCampaign)
            {
                try
                {
                    if (SelectionType == "Username")
                    {
                        string query = "INSERT INTO tb_CampaignData (Campaign_Name, Username_List , Keyword_List , Username_Selection) VALUES ('" + txtNewCampaignName.Text + "', '" + txtUsernameList.Text + "' , '" + "" + "' , '" + SelectionType + "')";
                        DataBaseHandler.InsertQuery(query, "tb_CampaignData");
                        grpBoxCampaignDetaills.Enabled = false;
                        txtUsernameList.Text = "";
                        txtKeywordList.Text = "";
                        MessageBox.Show("Campaign Saved");
                        string Query = "Select * From tb_CampaignData";
                        DataSet ds = DataBaseHandler.SelectQuery(Query, "tb_CampaignData");
                        cmboxCampiagnName.Items.Clear();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string CampaignName = dr["Campaign_Name"].ToString();
                            cmboxCampiagnName.Items.Add(CampaignName);
                        }
                    }
                    else if (SelectionType == "Keyword")
                    {
                        string query = "INSERT INTO tb_CampaignData (Campaign_Name, Username_List , Keyword_List , Username_Selection) VALUES ('" + txtNewCampaignName.Text + "', '" + "" + "' , '" + txtKeywordList.Text + "' , '" + SelectionType + "')";
                        DataBaseHandler.InsertQuery(query, "tb_CampaignData");
                        grpBoxCampaignDetaills.Enabled = false;
                        txtUsernameList.Text = "";
                        txtKeywordList.Text = "";
                        MessageBox.Show("Campaign Saved");
                        string Query = "Select * From tb_CampaignData";
                        DataSet ds = DataBaseHandler.SelectQuery(Query, "tb_CampaignData");
                        cmboxCampiagnName.Items.Clear();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string CampaignName = dr["Campaign_Name"].ToString();
                            cmboxCampiagnName.Items.Add(CampaignName);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                AddCampaign = false;
            }
            else
            {
                if(string.IsNullOrEmpty(txtNewCampaignName.Text))
                {
                    MessageBox.Show("Please Enter Campaign Name");
                }
            }
        }

        private void FrmCampaign_Load(object sender, EventArgs e)
        {
            try
            {
                cmboxCampiagnName.SelectedIndex = 0;
                grpBoxCampaignDetaills.Enabled = false;
                string Query = "Select * From tb_CampaignData";
                DataSet ds = DataBaseHandler.SelectQuery(Query, "tb_CampaignData");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string CampaignName = dr["Campaign_Name"].ToString();
                    cmboxCampiagnName.Items.Add(CampaignName);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FrmCampaign_Load() --> " + ex.Message, Globals.Path_CampaignManager);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string SelectedCampaignName = string.Empty;
                cmboxCampiagnName.Invoke(new MethodInvoker(delegate
                {
                    SelectedCampaignName = cmboxCampiagnName.SelectedItem.ToString();
                }));
                if (SelectedCampaignName != "Select Campaign Name")
                {
                    string Query = "Select * From tb_CampaignData";
                    DataSet ds = DataBaseHandler.SelectQuery(Query, "tb_CampaignData");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string CampaignName = dr["Campaign_Name"].ToString();
                        if (SelectedCampaignName == CampaignName)
                        {
                            grpBoxCampaignDetaills.Enabled = true;
                            if (dr[4].ToString() == "Username")
                            {
                                txtUsernameList.Text = dr["Username_List"].ToString();
                                SelectionType = "Username";
                                EditCampaign = true;
                            }
                            else if (dr[4].ToString() == "Keyword")
                            {
                                txtKeywordList.Text = dr["Keyword_List"].ToString();
                                SelectionType = "Keyword";
                                EditCampaign = true;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Select a Valid Campaign Name");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnEdit_Click() --> " + ex.Message, Globals.Path_CampaignManager);
            }
        }

        private void btnKeywordSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SelectionType = "Keyword";
                txtUsernameList.Text = "";
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        List<string> tempFile = GlobusFileHelper.ReadLargeFile(ofd.FileName);//GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        txtKeywordList.Text = ofd.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnKeywordSearch_Click() --> " + ex.Message, Globals.Path_CampaignManager);
            }
        }

        private void btnViewCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                FrmStartCampaign StartCampaign = new FrmStartCampaign();
                StartCampaign.ShowDialog();
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnViewCampaign_Click() --> " + ex.Message, Globals.Path_CampaignManager);
            }
        }

    }
}
