using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseLib;
using System.Text.RegularExpressions;
using Follower;
using BaseLib;

namespace twtboardpro
{
    public partial class FrmSettings : Form
    {
        public FrmSettings()
        {
            InitializeComponent();
        }

        public string CaptchaHost = string.Empty;
        public string CaptchaPort = string.Empty;
        public string CaptchaUsername = string.Empty;
        public string CaptchaPassword = string.Empty;

        private void btnSaveDBC_Click(object sender, EventArgs e)
        {
            
            if (!string.IsNullOrEmpty(txtDBCUsername.Text.Trim()) && !string.IsNullOrEmpty(txtDBCPassword.Text.Trim()))
            {
                BaseLib.Globals.DBCUsername = txtDBCUsername.Text;
                BaseLib.Globals.DBCPassword = txtDBCPassword.Text;

                //***  Save tb_Setting**************************///////

                clsDBQueryManager objclsSettingDB = new clsDBQueryManager();

                try
                {
                    DataTable dt = objclsSettingDB.SelectSettingData();
                    foreach (DataRow row in dt.Rows)
                    {
                        if ("DeathByCaptcha" == row[1].ToString())
                        {
                            objclsSettingDB.DeleteDBCDecaptcherData("DeathByCaptcha");
                        }
                    }
                    objclsSettingDB.InsertDBCData(StringEncoderDecoder.Encode(txtDBCUsername.Text), "DeathByCaptcha", StringEncoderDecoder.Encode(txtDBCPassword.Text));
                    MessageBox.Show("Settings Saved");
                    this.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
            else
            {
                MessageBox.Show("Please fill all Death By Captcha Details");
            }
        }

        private void LoadDBCSettings()
        {
            clsDBQueryManager objclsSettingDB = new clsDBQueryManager();

            try
            {
                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {
                    if ("DeathByCaptcha" == row[1].ToString())
                    {
                        txtDBCUsername.Text = StringEncoderDecoder.Decode(row[0].ToString());
                        txtDBCPassword.Text = StringEncoderDecoder.Decode(row[2].ToString());
                        BaseLib.Globals.DBCUsername = StringEncoderDecoder.Decode(txtDBCUsername.Text);
                        BaseLib.Globals.DBCPassword = StringEncoderDecoder.Decode(txtDBCPassword.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void FrmSettings_Load_1(object sender, EventArgs e)
        {
            if (Globals.IsGlobalDelay)
            {
                chkUseGlobalDelay.Checked = true;
            }
            else
            {
                chkUseGlobalDelay.Checked = false;
            }


            txtDBpath.Text = Globals.path_AppDataFolder;
            LoadDBCSettings();

            if (Globals.IsMobileVersion == true)
            {
                chkUseMobileVersion.Checked = true;
            }
            else
            {
                chkUseMobileVersion.Checked = false;
            }
        }

        private void FrmSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            Unfollower.IsSettingOpen_NewUi = true;
        }

        private void chkUseMobileVersion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseMobileVersion.Checked)
            {
                Globals.IsMobileVersion = true;
            }
            else 
            {
                Globals.IsMobileVersion = false;
            }
        }

        private void chkUseGlobalDelay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseGlobalDelay.Checked)
            {
                Globals.IsGlobalDelay = true;
                //Event_DisableDelaySettings("");
                //Event_DisableDelaySettingsUnfollower("");
            }
            else
            {
                //Event_EnableDelaySettings("");
                //Event_EnableDelaySettingsUnfollower("");
                Globals.IsGlobalDelay = false;
            }
        }


        public static Events EnableDelaySettings = new Events();
        public static Events DisableDelaySettings = new Events();
        public static void Event_EnableDelaySettings(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            EnableDelaySettings.LogText(eArgs);
        }
        public static void Event_DisableDelaySettings(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            DisableDelaySettings.LogText(eArgs);
        }

        public static void Event_EnableDelaySettingsUnfollower(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            EnableDelaySettings.LogText(eArgs);
        }
        public static void Event_DisableDelaySettingsUnfollower(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            DisableDelaySettings.LogText(eArgs);
        }

        private void btnSaveDelaySetting_Click(object sender, EventArgs e)
        {
            if (GlobusRegex.ValidateNumber(txtGlobalMinDelay.Text))
            {
                Globals.MinGlobalDelay = Convert.ToInt32(txtGlobalMinDelay.Text);
                Globals.IsCheckValueOfDelay = true;
            }
            else
            {
                Globals.IsCheckValueOfDelay = false;
                MessageBox.Show("Please Fill right Delay value.");
                return;
            }
            if (GlobusRegex.ValidateNumber(txtGlobalMaxDelay.Text))
            {
                Globals.MaxGlobalDelay = Convert.ToInt32(txtGlobalMaxDelay.Text);
                Globals.IsCheckValueOfDelay = true;
            }
            else
            {
                Globals.IsCheckValueOfDelay = false;
                MessageBox.Show("Please Fill right Delay value.");
                return;
            }

            if (Globals.MinGlobalDelay > 0 && Globals.MaxGlobalDelay > 0&& Globals.IsCheckValueOfDelay)
            {
                //Event_DisableDelaySettings("");
                MessageBox.Show("Delay value saved.");
            }
            else
            {
                //Event_EnableDelaySettings("");
                MessageBox.Show("Please Fill right Delay value.");
                return;
            }

        }

        private void chkCopyLoggerData_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCopyLoggerData.Checked)
            {
                Globals.IsCopyLoggerData = true;
            }
            else
            {
                Globals.IsCopyLoggerData = false;
            }
        }


    }
}

