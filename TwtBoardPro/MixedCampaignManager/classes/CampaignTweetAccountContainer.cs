using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampaignManager
{
    public class CampaignTweetAccountContainer
    {
        public  Dictionary<string, MixedCampaignManager.classes.CampaignAccountManager> dictionary_CampaignAccounts = new Dictionary<string, MixedCampaignManager.classes.CampaignAccountManager>();
    }

    public class CampaignAccountsList
    {
        public static Dictionary<string, MixedCampaignManager.classes.CampaignAccountManager> dictionary_CampaignAccounts = new Dictionary<string, MixedCampaignManager.classes.CampaignAccountManager>();
    }
}
