using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Drawing.Drawing2D;

namespace MixedCampaignManager
{
    public partial class frm_Status : Form
    {
        DataSet Ds;

        #region frm_Status
        public frm_Status()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        } 
        #endregion

        #region frm_Status_Load
        private void frm_Status_Load(object sender, EventArgs e)
        {
            rdobtn_SearchAllReport.Checked = true;
            Ds = RepositoryClasses.cls_DbRepository.getAllStatusFromCapaign();
            dataGridView1.Name = "Campaign Report";
            //Ds.Tables[0].Columns["indx"].ColumnName = "Index";
            dataGridView1.DataSource = Ds.Tables[0];
        } 
        #endregion

        #region Btn_Search_Click
        private void Btn_Search_Click(object sender, EventArgs e)
        {
            String Query = string.Empty;
            String SearchingValue = txt_searchingKeyWord.Text;
            DataView Dview = new DataView();
            DataTable Dt = new DataTable();

            if (Ds.Tables.Count != 0 && Ds.Tables[0].Rows.Count > 0)
            {
                Dview.Table = Ds.Tables[0];
            }
            else
            {
                return;
            }

            if (rdobtn_SearchReportCampName.Checked)
            {
                Dview.RowFilter = ("CampaignName = '" + SearchingValue + "'");
            }
            else if (rdobtn_SearchReportUserName.Checked)
            {
                Dview.RowFilter = ("UserName = '" + SearchingValue + "'");
            }
            else if (rdobtn_SearchReportFollowerName.Checked)
            {
                Dview.RowFilter = ("FollowerName = '" + SearchingValue + "'");
            }
            else if (rdobtn_SearchReportDateTimeName.Checked)
            {
                //DataRow[] result = table.Select("Date > #6/7/2013#");
                Dview.RowFilter = ("DateTime = '" + SearchingValue + "'");
            }
            else if (rdobtn_SearchAllReport.Checked)
            {
                dataGridView1.DataSource = Ds.Tables[0];
                return;
            }
            else
            {

            }

            Dview.RowStateFilter = DataViewRowState.CurrentRows;
            dataGridView1.DataSource = Dview;

        } 
        #endregion

        #region frm_Status_Paint
        private void frm_Status_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(Properties.Resources.app_bg, 0, 0, this.Width, this.Height);
        } 
        #endregion

    }
}
