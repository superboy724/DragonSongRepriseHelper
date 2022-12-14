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

        public bool P3Step1Enable { get; set; }

        public bool P3Step2Enable { get; set; }

        public bool P4Step1Enable { get; set; }

        public bool P4Step2Enable { get; set; }

        public bool P3Step2EndEnable { get; set; }

        public bool P5Step1Enable { get; set; }

        public bool P5Step3Enable { get; set; }

        public bool P6Step2Enable { get; set; }

        public string[] GetSettingText()
        {
            string[] settingTexts = new string[15];
            settingTexts[0] = "postNamazuUrl=" + PostNamazuSetting;
            settingTexts[1] = "p2Step1Enable=" + (P2Step1Enable ? "true" : "false");
            settingTexts[2] = "p2Step2Enable=" + (P2Step2Enable ? "true" : "false");
            settingTexts[3] = "p2Step3Enable=" + (P2Step3Enable ? "true" : "false");
            settingTexts[4] = "p2Step4Enable=" + (P2Step4Enable ? "true" : "false");
            settingTexts[5] = "p2Step2MarkDisabled=" + (P2Step2MarkDisabled ? "true" : "false");
            settingTexts[6] = "p2Step4ChangeTowerEnable=" + (P2Step4ChangeTowerEnable ? "true" : "false");
            settingTexts[7] = "p3Step1Enable=" + (P3Step1Enable ? "true" : "false");
            settingTexts[8] = "p4Step1Enable=" + (P4Step1Enable ? "true" : "false");
            settingTexts[9] = "p4Step2Enable=" + (P4Step2Enable ? "true" : "false");
            settingTexts[10] = "p3Step2Enable=" + (P3Step2Enable ? "true" : "false");
            settingTexts[11] = "p3Step2EndEnable=" + (P3Step2EndEnable ? "true" : "false");
            settingTexts[12] = "p5Step1Enable=" + (P5Step1Enable ? "true" : "false");
            settingTexts[13] = "p6Step2Enable=" + (P6Step2Enable ? "true" : "false");
            settingTexts[14] = "p5Step3Enable=" + (P5Step3Enable ? "true" : "false");

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
            if (configTexts.ContainsKey("p3Step1Enable"))
            {
                this.P3Step1Enable = configTexts["p3Step1Enable"] == "true";
            }
            if (configTexts.ContainsKey("p4Step1Enable"))
            {
                this.P4Step1Enable = configTexts["p4Step1Enable"] == "true";
            }
            if (configTexts.ContainsKey("p4Step2Enable"))
            {
                this.P4Step2Enable = configTexts["p4Step2Enable"] == "true";
            }
            if (configTexts.ContainsKey("p3Step2Enable"))
            {
                this.P3Step2Enable = configTexts["p3Step2Enable"] == "true";
            }
            if (configTexts.ContainsKey("p3Step2EndEnable"))
            {
                this.P3Step2EndEnable = configTexts["p3Step2EndEnable"] == "true";
            }
            if (configTexts.ContainsKey("p5Step1Enable"))
            {
                this.P5Step1Enable = configTexts["p5Step1Enable"] == "true";
            }
            if (configTexts.ContainsKey("p6Step2Enable"))
            {
                this.P6Step2Enable = configTexts["p6Step2Enable"] == "true";
            }
            if (configTexts.ContainsKey("p5Step3Enable"))
            {
                this.P5Step3Enable = configTexts["p5Step3Enable"] == "true";
            }
        }
    }
}
