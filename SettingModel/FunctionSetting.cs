using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonSongRepriseHelper.SettingModel
{
    public class FunctionSetting : SettingModel
    {
        public string PostNamazuSetting { get; set; }

        public bool P2Step1Enable { get; set; }

        public bool P2Step2Enable { get; set; }

        public bool P2Step3Enable { get; set; }

        public bool P2Step4Enable { get; set; }

        public bool P2Step2MarkDisabled { get; set; }

        public bool P2Step4ChangeTowerEnable { get; set; }

        public string[] GetSettingText()
        {
            string[] settingTexts = new string[7];
            settingTexts[0] = "postNamazuUrl=" + PostNamazuSetting;
            settingTexts[1] = "p2Step1Enable=" + P2Step1Enable;
            settingTexts[2] = "p2Step2Enable=" + P2Step2Enable;
            settingTexts[3] = "p2Step3Enable=" + P2Step3Enable;
            settingTexts[4] = "p2Step4Enable=" + P2Step4Enable;
            settingTexts[5] = "p2Step2MarkDisabled=" + P2Step2MarkDisabled;
            settingTexts[6] = "p2Step4ChangeTowerEnable=" + P2Step4ChangeTowerEnable;

            return settingTexts;
        }

        public void LoadSettingFromText(Dictionary<string, string> configTexts)
        {
            if (configTexts.ContainsKey("postNamazuUrl"))
            {
                this.PostNamazuSetting = configTexts["postNamazuUrl"];
            }
            if (configTexts.ContainsKey("p2Step1Enable"))
            {
                this.P2Step1Enable = configTexts["p2Step1Enable"] == "true";
            }
            if (configTexts.ContainsKey("p2Step2Enable"))
            {
                this.P2Step2Enable = configTexts["p2Step2Enable"] == "true";
            }
            if (configTexts.ContainsKey("p2Step3Enable"))
            {
                this.P2Step3Enable = configTexts["p2Step3Enable"] == "true";
            }
            if (configTexts.ContainsKey("p2Step4Enable"))
            {
                this.P2Step4Enable = configTexts["p2Step4Enable"] == "true";
            }
            if (configTexts.ContainsKey("p2Step2MarkDisabled"))
            {
                this.P2Step2MarkDisabled = configTexts["p2Step2MarkDisabled"] == "true";
            }
            if (configTexts.ContainsKey("p2Step4ChangeTowerEnable"))
            {
                this.P2Step4ChangeTowerEnable = configTexts["p2Step4ChangeTowerEnable"] == "true";
            }
        }
    }
}
