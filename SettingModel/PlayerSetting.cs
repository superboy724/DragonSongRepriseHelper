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

        public bool IsSettingOk()
        {
            return !string.IsNullOrEmpty(MT) && !string.IsNullOrEmpty(ST) && !string.IsNullOrEmpty(H1) && !string.IsNullOrEmpty(H2)
                && !string.IsNullOrEmpty(D1) && !string.IsNullOrEmpty(D2) && !string.IsNullOrEmpty(D3) && !string.IsNullOrEmpty(D4);
        }

        public string[] GetSettingText()
        {
            string[] playerSettings = new string[8];
            playerSettings[0] = "playerMT=" + MT + "|" + (string.IsNullOrEmpty(MT) ? "" : PlayerIndex[MT].ToString());
            playerSettings[1] = "playerST=" + ST + "|" + (string.IsNullOrEmpty(ST) ? "" : PlayerIndex[ST].ToString());
            playerSettings[2] = "playerH1=" + H1 + "|" + (string.IsNullOrEmpty(H1) ? "" : PlayerIndex[H1].ToString());
            playerSettings[3] = "playerH2=" + H2 + "|" + (string.IsNullOrEmpty(H2) ? "" : PlayerIndex[H2].ToString());
            playerSettings[4] = "playerD1=" + D1 + "|" + (string.IsNullOrEmpty(D1) ? "" : PlayerIndex[D1].ToString());
            playerSettings[5] = "playerD2=" + D2 + "|" + (string.IsNullOrEmpty(D2) ? "" : PlayerIndex[D2].ToString());
            playerSettings[6] = "playerD3=" + D3 + "|" + (string.IsNullOrEmpty(D3) ? "" : PlayerIndex[D3].ToString());
            playerSettings[7] = "playerD4=" + D4 + "|" + (string.IsNullOrEmpty(D4) ? "" : PlayerIndex[D4].ToString());

            return playerSettings;
        }

        public void LoadSettingFromText(Dictionary<string,string> configTexts)
        {
            if (configTexts.ContainsKey("playerMT"))
            {
                if (configTexts["playerMT"].Contains("|"))
                {
                    string playerId = configTexts["playerMT"].Split('|')[0];
                    string playerIndex = configTexts["playerMT"].Split('|')[1];
                    if(!(string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(playerIndex)))
                    {
                        this.MT = playerId;
                        this.PlayerIndex.Add(playerId, Convert.ToInt32(playerIndex));
                    }
                    
                }
                
            }
            if (configTexts.ContainsKey("playerST"))
            {
                string playerId = configTexts["playerST"].Split('|')[0];
                string playerIndex = configTexts["playerST"].Split('|')[1];
                if (!(string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(playerIndex)))
                {
                    this.ST = playerId;
                    this.PlayerIndex.Add(playerId, Convert.ToInt32(playerIndex));
                }
            }
            if (configTexts.ContainsKey("playerH1"))
            {
                string playerId = configTexts["playerH1"].Split('|')[0];
                string playerIndex = configTexts["playerH1"].Split('|')[1];
                if (!(string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(playerIndex)))
                {
                    this.H1 = playerId;
                    this.PlayerIndex.Add(playerId, Convert.ToInt32(playerIndex));
                }
            }
            if (configTexts.ContainsKey("playerH2"))
            {
                string playerId = configTexts["playerH2"].Split('|')[0];
                string playerIndex = configTexts["playerH2"].Split('|')[1];
                if (!(string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(playerIndex)))
                {
                    this.H2 = playerId;
                    this.PlayerIndex.Add(playerId, Convert.ToInt32(playerIndex));
                }
            }
            if (configTexts.ContainsKey("playerD1"))
            {
                string playerId = configTexts["playerD1"].Split('|')[0];
                string playerIndex = configTexts["playerD1"].Split('|')[1];
                if (!(string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(playerIndex)))
                {
                    this.D1 = playerId;
                    this.PlayerIndex.Add(playerId, Convert.ToInt32(playerIndex));
                }
            }
            if (configTexts.ContainsKey("playerD2"))
            {
                string playerId = configTexts["playerD2"].Split('|')[0];
                string playerIndex = configTexts["playerD2"].Split('|')[1];
                if (!(string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(playerIndex)))
                {
                    this.D2 = playerId;
                    this.PlayerIndex.Add(playerId, Convert.ToInt32(playerIndex));
                }
            }
            if (configTexts.ContainsKey("playerD3"))
            {
                string playerId = configTexts["playerD3"].Split('|')[0];
                string playerIndex = configTexts["playerD3"].Split('|')[1];
                if (!(string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(playerIndex)))
                {
                    this.D3 = playerId;
                    this.PlayerIndex.Add(playerId, Convert.ToInt32(playerIndex));
                }
            }
            if (configTexts.ContainsKey("playerD4"))
            {
                string playerId = configTexts["playerD4"].Split('|')[0];
                string playerIndex = configTexts["playerD4"].Split('|')[1];
                if (!(string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(playerIndex)))
                {
                    this.D4 = playerId;
                    this.PlayerIndex.Add(playerId, Convert.ToInt32(playerIndex));
                }
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
            PlayerIndex.Clear();
            MT = null;
            ST = null;
            H1 = null;
            H2 = null;
            D1 = null;
            D2 = null;
            D3 = null;
            D4 = null;
            string[] textStrArray = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int index = 1;
            foreach(var item in textStrArray)
            {
                if (!item.Contains(","))
                {
                    continue;
                }
                string playerId = item.Split(',')[0];
                string playerJob = item.Split(',')[1];
                if(string.IsNullOrEmpty(playerId))
                {
                    continue;
                }
                if (this.PlayerIndex.ContainsKey(playerId))
                {
                    continue;
                }

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
            string[] strs = new string[8];
            foreach(var item in PlayerIndex)
            {
                StringBuilder temp = new StringBuilder();
                temp.Append(item.Key).Append(",").Append(this.GetJobByPlayerId(item.Key)).Append("\r\n");
                strs[item.Value - 1] = temp.ToString();

            }

            foreach(var item in strs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    sb.Append(item);
                }
            }

            return sb.ToString();
        }

        public void PlayerSettingClear()
        {
            this.MT = null;
            this.ST = null;
            this.H1 = null;
            this.H2 = null;
            this.D1 = null;
            this.D2 = null;
            this.D3 = null;
            this.D4 = null;
            this.PlayerIndex.Clear();
        }

        public bool IsDps(string playerId)
        {
            if(playerId == D1 || playerId == D2 || playerId == D3 || playerId == D4)
            {
                return true;
            }
            return false;
        }
    }

}
