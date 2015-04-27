using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Globussoft;
using BaseLib;

namespace MixedCampaignManager.CustomUserControls
{
    public partial class AccountCreatorUserControl : UserControl
    {

        public static List<string> AccountUsersName = new List<string>();
        public static List<string> AccountNames = new List<string>();
        public static List<string> Emails = new List<string>();
        public static List<string> Images = new List<string>();
        public static List<string> ProfileLocation = new List<string>();
        public static List<string> ProfileDescription = new List<string>();
        public static List<string> ProfileUrls = new List<string>();

        public static BaseLib.Events Sm_CoountCreatorLogEvents = new BaseLib.Events();


        public AccountCreatorUserControl()
        {
            InitializeComponent();
        }

        private void AccountCreatorUserControl_Load(object sender, EventArgs e)
        {

        }

        private void txt_AccountDetailsFilePath_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_AccountDetails_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog ofd = new FolderBrowserDialog())
            {
                ofd.SelectedPath = Application.StartupPath + "\\Profile\\Pics";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //SET FILE PATH IN TEXT BOX..
                    txt_AccountDetailsFilePath.Text = ofd.SelectedPath;

                    MixedCampaignManager.classes.AccountCreatorEntities._AccountdeatilsFolderPath = ofd.SelectedPath;

                    if (!string.IsNullOrEmpty(MixedCampaignManager.classes.AccountCreatorEntities._AccountdeatilsFolderPath))
                    {
                        #region GET ALL  FILE FROM FOLDER..

                        string[] Files = Directory.GetFiles(MixedCampaignManager.classes.AccountCreatorEntities._AccountdeatilsFolderPath);

                        foreach (string file in Files)
                        {
                            if ((file.Contains("UserName.txt")) || (file.Contains("username.txt")))
                            {
                                AccountUsersName = GlobusFileHelper.ReadFiletoStringList(file);

                                Log(AccountUsersName.Count + " User Names uploaded.");
                            }
                            else if ((file.Contains("Name.txt")) || (file.Contains("name.txt")) || (file.Contains("Names.txt")))
                            {
                                AccountNames = GlobusFileHelper.ReadFiletoStringList(file);

                                Log(AccountNames.Count + " Names uploaded.");
                            }
                            if ((file.Contains("Email.txt")) || (file.Contains("email.txt")) || (file.Contains("emails.txt")) || (file.Contains("Emails.txt")))
                            {
                                Emails = GlobusFileHelper.ReadFiletoStringList(file);

                                Log(Emails.Count + " Emails uploaded.");
                            }
                        }//END FOREACH
                        #endregion
                    }//END IF 


                }
            }//END USING IF

        }

        private void btn_AccountProfileImage_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog ofd = new FolderBrowserDialog())
            {
                Images.Clear();

                ofd.SelectedPath = Application.StartupPath + "\\Profile\\Pics";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //SET FILE PATH IN TEXT BOX..
                    txt_ImageFolderPath.Text = ofd.SelectedPath;

                    MixedCampaignManager.classes.AccountCreatorEntities.ImageFolderPath = ofd.SelectedPath;

                    if (!string.IsNullOrEmpty(MixedCampaignManager.classes.AccountCreatorEntities.ImageFolderPath))
                    {
                        #region GET ALL  FILE FROM FOLDER..

                        string[] Files = Directory.GetFiles(MixedCampaignManager.classes.AccountCreatorEntities.ImageFolderPath);

                        foreach (string file in Files)
                        {
                            FileInfo Info = new FileInfo(file);
                            if ((Info.Extension.Contains(".JPG")) || (Info.Extension.Contains(".JPEG"))
                                || (Info.Extension.Contains(".jpg")) || (Info.Extension.Contains(".jpeg"))
                                || (Info.Extension.Contains(".PNG")) || (Info.Extension.Contains(".png")))
                            {
                                Images.Add(file);
                            }

                        }//END FOREACH

                        Log("[ " + DateTime.Now + " ] => [ " + Images.Count + " Images uploaded. ]");

                        #endregion
                    }//END IF 
                }
            }//END USING IF

        }

        private void btn_AccountProfileDetails_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog ofd = new FolderBrowserDialog())
            {
                ofd.SelectedPath = Application.StartupPath + "\\Profile\\Pics";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //SET FILE PATH IN TEXT BOX..
                    txt_ProfileDetailsFilePath.Text = ofd.SelectedPath;

                    MixedCampaignManager.classes.AccountCreatorEntities.ProfileDetailsFolderPath = ofd.SelectedPath;


                    if (!string.IsNullOrEmpty(MixedCampaignManager.classes.AccountCreatorEntities.ProfileDetailsFolderPath))
                    {
                        #region GET ALL  FILE FROM FOLDER..
                        string[] Files = Directory.GetFiles(MixedCampaignManager.classes.AccountCreatorEntities.ProfileDetailsFolderPath);

                        foreach (string file in Files)
                        {
                            if (((file.Contains("ProfileLocation.txt")) || (file.Contains("profilelocation.txt")) || (file.Contains("Profilelocations.txt"))))
                            {
                                ProfileLocation = GlobusFileHelper.ReadFiletoStringList(file);
                                Log(ProfileLocation.Count + " Profile locations uploaded.");
                            }
                            if (((file.Contains("ProfileDescription.txt")) || (file.Contains("profiledescription.txt"))))
                            {
                                ProfileDescription = GlobusFileHelper.ReadFiletoStringList(file);
                                Log(ProfileDescription.Count + " Profile description uploaded.");
                            }
                            if (((file.Contains("ProfileUrl.txt")) || (file.Contains("profileurl.txt")) || (file.Contains("profileurl.txt")) || (file.Contains("ProfileUrls.txt"))))
                            {
                                ProfileUrls = GlobusFileHelper.ReadFiletoStringList(file);
                                Log(ProfileUrls.Count + " Profile Url's uploaded.");
                            }
                        }//END FOREACH
                        #endregion
                    }//END IF 

                }
            }//END USING IF

        }

        private void chk_SmProfileImage_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                MixedCampaignManager.classes.AccountCreatorEntities._ProfileImage = true;
            }
            else
            {
                MixedCampaignManager.classes.AccountCreatorEntities._ProfileImage = false;
            }
        }

        private void chk_SmLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                MixedCampaignManager.classes.AccountCreatorEntities._Location = true;
            }
            else
            {
                MixedCampaignManager.classes.AccountCreatorEntities._Location = false;
            }
        }

        private void chk_SmProfileDescription_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                MixedCampaignManager.classes.AccountCreatorEntities._Description = true;
            }
            else
            {
                MixedCampaignManager.classes.AccountCreatorEntities._Description = false;
            }
        }

        private void chk_SmProfileUrl_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                MixedCampaignManager.classes.AccountCreatorEntities._ProfileUrl = true;
            }
            else
            {
                MixedCampaignManager.classes.AccountCreatorEntities._ProfileUrl = false;
            }
        }

        private void chk_SmUseOnlyString_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                MixedCampaignManager.classes.AccountCreatorEntities._UseString = true;
            }
            else
            {
                MixedCampaignManager.classes.AccountCreatorEntities._UseString = false;
            }
        }

        private void chk_SmUseOnlyNumbers_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                MixedCampaignManager.classes.AccountCreatorEntities._OnlyNumbers = true;
            }
            else
            {
                MixedCampaignManager.classes.AccountCreatorEntities._OnlyNumbers = false;
            }
        }

        private void chk_SmUseOnlyBoth_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                MixedCampaignManager.classes.AccountCreatorEntities._UseBoth = true;
            }
            else
            {
                MixedCampaignManager.classes.AccountCreatorEntities._UseBoth = false;
            }
        }

        private void Log(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            Sm_CoountCreatorLogEvents.LogText(eventArgs);
        }

    }
}
