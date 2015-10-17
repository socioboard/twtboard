using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Randomiser
{
    public partial class frmUseFollowBySearchKeyword : Form
    {
        private System.Drawing.Image image;

        List<string> lstFollowBySearchKeyword = new List<string>();

        public frmUseFollowBySearchKeyword()
        {
            InitializeComponent();
        }

        private void frmUseFollowBySearchKeyword_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g;
                g = e.Graphics;
                g.SmoothingMode = SmoothingMode.HighQuality;
                // Draw the background.
                //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));
                //// Draw the image.
                g.DrawImage(image, 0, 0, this.Width, this.Height);
            }
            catch
            {
            }
        }

        private void frmUseFollowBySearchKeyword_Load(object sender, EventArgs e)
        {
            try
            {
                //Get Image 
                image = twtboardpro.Properties.Resources.app_bg;
            }
            catch
            {
            }
        }

        private void btn_FollowKeyWordsFileUpload_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txt_FollowBySearchKey.Text = ofd.FileName;
                        lstFollowBySearchKeyword = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    }
                    lstFollowBySearchKeyword=lstFollowBySearchKeyword.Distinct().ToList();
                }
            }
            catch
            {
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                clsFollowBySearchKeywords obj_clsFollowBySearchKeywords = clsFollowBySearchKeywords.GetObject();

                if (!string.IsNullOrEmpty(txt_FollowBySearchKey.Text) && lstFollowBySearchKeyword.Count > 0)
                {
                    try
                    {
                        obj_clsFollowBySearchKeywords.lstKeywords = lstFollowBySearchKeyword;

                    }
                    catch
                    {
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload The Follow By Search Keywords File !");
                    return;
                }

                if (chk_followbysinglekeywordperaccount.Checked)
                {
                    obj_clsFollowBySearchKeywords._Followbysinglekeywordperaccount = true;
                }

                if (!string.IsNullOrEmpty(txt_FollowByPerAccount.Text))
                {
                    try
                    {
                        obj_clsFollowBySearchKeywords._NoFollowByPerAccount = Convert.ToInt32(txt_FollowByPerAccount.Text);
                    }
                    catch
                    {
                        txt_FollowByPerAccount.Text = (5).ToString();
                        obj_clsFollowBySearchKeywords._NoFollowByPerAccount = 5;
                    }
                }

                if (MessageBox.Show("Follow By Search Keywords Is Set !", "Notification", MessageBoxButtons.OK) == DialogResult.OK) ;
                {
                    this.Close();
                }
            }
            catch
            {
            }
        }
    }
}
