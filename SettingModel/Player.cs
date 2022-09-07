using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonSongRepriseHelper.SettingModel
{
    public class Player : SettingModel
    {
        string MT { get; set; }

        string ST { get; set; }

        string H1 { get; set; }

        string H2 { get; set; }

        string D1 { get; set; }

        string D2 { get; set; }

        string D3 { get; set; }

        string D4 { get; set; }

        public bool isSettingOk()
        {
            return !string.IsNullOrEmpty(MT) && !string.IsNullOrEmpty(ST) && !string.IsNullOrEmpty(H1) && !string.IsNullOrEmpty(H2)
                && !string.IsNullOrEmpty(D1) && !string.IsNullOrEmpty(D2) && !string.IsNullOrEmpty(D3) && !string.IsNullOrEmpty(D3);
        }

        public string[] GetSettingText()
        {
            string[] playerSettings = new string[8];
            playerSettings[0] = "playerMT=" + MT;
            playerSettings[1] = "playerST=" + ST;
            playerSettings[2] = "playerH1=" + H1;
            playerSettings[3] = "playerH2=" + H2;
            playerSettings[4] = "playerD1=" + D1;
            playerSettings[5] = "playerD2=" + D2;
            playerSettings[6] = "playerD3=" + D3;
            playerSettings[7] = "playerD4=" + D4;

            return playerSettings;
        }

        public void LoadSettingFromText(Dictionary<string,string> configTexts)
        {
            if (configTexts.ContainsKey("MT"))
            {
                this.MT = configTexts["MT"];
            }
            if (configTexts.ContainsKey("ST"))
            {
                this.ST = configTexts["ST"];
            }
            if (configTexts.ContainsKey("ST"))
            {
                this.H1 = configTexts["H1"];
            }
            if (configTexts.ContainsKey("H2"))
            {
                this.H2 = configTexts["H2"];
            }
            if (configTexts.ContainsKey("D1"))
            {
                this.D1 = configTexts["D1"];
            }
            if (configTexts.ContainsKey("D2"))
            {
                this.D2 = configTexts["D2"];
            }
            if (configTexts.ContainsKey("D3"))
            {
                this.D3 = configTexts["D3"];
            }
            if (configTexts.ContainsKey("D4"))
            {
                this.D4 = configTexts["D4"];
            }
        }
    }

}
