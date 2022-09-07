using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonSongRepriseHelper.SettingModel
{
    public class PlayerSetting : SettingModel
    {
        public string MT { get; set; }

        public string ST { get; set; }

        public string H1 { get; set; }

        public string H2 { get; set; }

        public string D1 { get; set; }

        public string D2 { get; set; }

        public string D3 { get; set; }

        public string D4 { get; set; }

        public Dictionary<string, int> PlayerIndex { get; set; }

        public PlayerSetting()
        {
            PlayerIndex = new Dictionary<string, int>();
        }

        public bool isSettingOk()
        {
            return !string.IsNullOrEmpty(MT) && !string.IsNullOrEmpty(ST) && !string.IsNullOrEmpty(H1) && !string.IsNullOrEmpty(H2)
                && !string.IsNullOrEmpty(D1) && !string.IsNullOrEmpty(D2) && !string.IsNullOrEmpty(D3) && !string.IsNullOrEmpty(D3);
        }

        public string[] GetSettingText()
        {
            string[] playerSettings = new string[8];
            playerSettings[0] = "playerMT=" + MT + "|" + PlayerIndex[MT];
            playerSettings[1] = "playerST=" + ST + "|" + PlayerIndex[ST];
            playerSettings[2] = "playerH1=" + H1 + "|" + PlayerIndex[H1];
            playerSettings[3] = "playerH2=" + H2 + "|" + PlayerIndex[H2];
            playerSettings[4] = "playerD1=" + D1 + "|" + PlayerIndex[D1];
            playerSettings[5] = "playerD2=" + D2 + "|" + PlayerIndex[D2];
            playerSettings[6] = "playerD3=" + D3 + "|" + PlayerIndex[D3];
            playerSettings[7] = "playerD4=" + D4 + "|" + PlayerIndex[D4];

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

        public string GetJobByPlayerId(string playerId)
        {
            if(MT == playerId)
            {
                return "MT";
            }
            else if(ST == playerId)
            {
                return "ST";
            }
            else if (H1 == playerId)
            {
                return "H1";
            }
            else if (H2 == playerId)
            {
                return "H2";
            }
            else if (D1 == playerId)
            {
                return "D1";
            }
            else if (D2 == playerId)
            {
                return "D2";
            }
            else if (D3 == playerId)
            {
                return "D3";
            }
            else if (D4 == playerId)
            {
                return "D4";
            }
            return null;
        }

        public bool SetPlayerFromPlayerText(string text)
        {
            string[] textStrArray = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int index = 1;
            foreach(var item in textStrArray)
            {
                string playerId = item.Split(',')[0];
                string playerJob = item.Split(',')[1];

                switch (playerJob)
                {
                    case "MT": MT = playerId;PlayerIndex.Add(playerId, index);index++;break;
                    case "ST": ST = playerId; PlayerIndex.Add(playerId, index); index++; break;
                    case "H1": H1 = playerId; PlayerIndex.Add(playerId, index); index++; break;
                    case "H2": H2 = playerId; PlayerIndex.Add(playerId, index); index++; break;
                    case "D1": D1 = playerId; PlayerIndex.Add(playerId, index); index++; break;
                    case "D2": D2 = playerId; PlayerIndex.Add(playerId, index); index++; break;
                    case "D3": D3 = playerId; PlayerIndex.Add(playerId, index); index++; break;
                    case "D4": D4 = playerId; PlayerIndex.Add(playerId, index); index++; break;
                }
            }

            return index == 9;
        }

        public string BuildPlayerTextBoxStr()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var item in PlayerIndex)
            {
                sb.Append(item.Key).Append(",").Append(this.GetJobByPlayerId(item.Key)).Append("\r\n");
            }

            return sb.ToString();
        }
    }

}
