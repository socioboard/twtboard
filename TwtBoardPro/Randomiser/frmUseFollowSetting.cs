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
    public partial class frmUseFollowSetting : Form
    {
        private System.Drawing.Image image;
        public frmUseFollowSetting()
        {
            InitializeComponent();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                clsUseFollowSetting obj_clsUseFollowSetting = clsUseFollowSetting.GetObject();

                if (chkDontFollowUsersThatUnfollowedBefore.Checked)
                {
                    try
                    {
                        obj_clsUseFollowSetting._DontFollowUsersThatUnfollowedBefore = true;
                    }
                    catch
                    {
                    }
                }

                if (chkDontFollowUsersWithNoPicture.Checked)
                {
                    try
                    {
                        obj_clsUseFollowSetting._DontFollowUsersWithNoPicture = true;
                    }
                    catch
                    {
                    }
                }

                if (chkDontFollowUsersWithManyLinks.Checked)
                {
                    try
                    {
                        obj_clsUseFollowSetting._DontFollowUsersWithManyLinks = true;
                        obj_clsUseFollowSetting._NoOfLinks = Convert.ToInt32(txtNoOfLinks.Text);
                    }
                    catch
                    {
                    }
                }

                if (chkDontFollowUsersWithFollowingsFollowersRatio.Checked)
                {
                    try
                    {
                        obj_clsUseFollowSetting._DontFollowUsersWithFollowingsFollowersRatio = true;
                        obj_clsUseFollowSetting._FollowingsFollowersRatio = Convert.ToInt32(txtFollowingsFollowersRatio.Text);
                    }
                    catch
                    {
                    }
                }

                if (chkBox_NoOFfollow.Checked)
                {
                    try
                    {
                        obj_clsUseFollowSetting._NoOFfollow = true;
                        obj_clsUseFollowSetting._MaximumFollow = Convert.ToInt32(txt_MaximumFollow.Text);
                    }
                    catch
                    {
                    }
                }

                if (chkboxUseGroups.Checked)
                {
                    try
                    {
                        obj_clsUseFollowSetting._UseGroups = true;
                        obj_clsUseFollowSetting._UseGroup = txtUseGroup.Text;
                    }
                    catch
                    {
                    }
                }

                if (MessageBox.Show("Follow Setting Is Set !", "Notification", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    this.Close();

                }
            }
            catch
            {
            }
        }

        private void frmUseFollowSetting_Paint(object sender, PaintEventArgs e)
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

        private void frmUseFollowSetting_Load(object sender, EventArgs e)
        {
            try
            {
                //Get Image 
                image = Properties.Resources.app_bg;
            }
            catch
            {
            }
        }
       
    }
}
