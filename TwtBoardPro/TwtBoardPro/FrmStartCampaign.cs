using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseLib;
using System.Threading;
using Globussoft;
using BaseLib;

namespace twtboardpro
{
    public partial class FrmStartCampaign : Form
    {
        public FrmStartCampaign()
        {
            InitializeComponent();
        }

        public static BaseLib.Events startFlyCreationEvent = new BaseLib.Events();

        private void FrmStartCampaign_Load(object sender, EventArgs e)
        {
            LoadDataGrid();
        }

        public void LoadDataGrid()
        {
            try
            {
                DataTable dt = SelectAccoutsForGridView();

                dgvCampaigndata.Invoke(new MethodInvoker(delegate
                {
                    dgvCampaigndata.DataSource = dt;
                }));
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Campaign --> LoadDataGrid() --> " + ex.Message, Globals.Path_CampaignManager);
            }
        }


        public DataTable SelectAccoutsForGridView()
        {
            try
            {
                List<string> lstAccount = new List<string>();
                string strQuery = "SELECT * FROM tb_CampaignData";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_CampaignData");

                DataTable dt = ds.Tables["tb_CampaignData"];

                return dt;
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Campaign --> SelectAccoutsForGridView() --> " + ex.Message, Globals.Path_CampaignManager);
                return new DataTable();
            }
        }

        private void btnStartCampagin_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCampaigndata.RowCount > 0)
                {
                    string[] Array = new string[4];
                    Globals.Array[0] = dgvCampaigndata.SelectedRows[0].Cells[1].Value.ToString();
                    Globals.Array[1] = dgvCampaigndata.SelectedRows[0].Cells[2].Value.ToString();
                    Globals.Array[2] = dgvCampaigndata.SelectedRows[0].Cells[3].Value.ToString();
                    Globals.Array[3] = dgvCampaigndata.SelectedRows[0].Cells[4].Value.ToString();
                    this.Close();

                    RaiseFlyCreationEvent();
                }
                else
                {
                    MessageBox.Show("No Campaign in DataBase");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Campaign --> btnStartCampagin_Click() --> " + ex.Message, Globals.Path_CampaignManager);
            }
        }

        private void RaiseFlyCreationEvent()
        {
            EventsArgs eArgs = new EventsArgs("Starting Campaign");
            startFlyCreationEvent.LogText(eArgs);
        }


    }
}
