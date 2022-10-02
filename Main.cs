using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DragonSongRepriseHelper
{
    public class DragonSongRepriseHelper : IActPluginV1
    {
        SettingContainer settingContainer;
        LogReader logreader;
        PostNamazuHelper postNamazuHelper;
        SettingForm settingForm = null;
        bool clear = false;
        bool isDebug = false;
        int markOffset = -1;
        const int skywardTripleMark = 0x14A;
        const int sword1Mark = 0x32;
        const int sword2Mark = 0x33;
        const int meteor = 0x11D;

        //P3麻将点名
        const int mahjong1 = 0x13F;
        const int mahjong2 = mahjong1 + 1;
        const int mahjong3 = mahjong1 + 2;
        //P3放塔点名
        //AC3 原地 AC4上箭头 AC5下箭头
        const int towerPosInPlace = 0xAC3;
        const int towerPosFront = 0xAC4;
        const int towerPosBack = 0xAC5;
        //P3八人塔
        const int eightTowerOne = 0x6717;
        const int eightTowerTwo = 0x6718;
        const int eightTowerThree = 0x6719;
        const int eightTowerFour = 0x671A;

        //P4红蓝点名
        const int p4redmark = 0xAD7;
        const int p4bluemark = 0xAD8;

        string[] p2Step2AttackPlayer = new string[2];
        List<string> p2Step3AttackPlayer = new List<string>();
        List<string> p2Step1AttackPlayer = new List<string>();
        P2Step4OutTower p2Step4OutTower = new P2Step4OutTower();
        int p2Step4TowerCount = 0;
        string p2Step3AttackGroupType = "";
        string[][] p2Step3AttackGroup = new string[2][];
        List<string> p3Step1MarkMahjong1Player = new List<string>(3);
        List<string> p3Step1MarkMahjong2Player = new List<string>(2);
        List<string> p3Step1MarkMahjong3Player = new List<string>(3);
        Dictionary<string, int> p3Step1TowerPos = new Dictionary<string, int>();
        int[] p3Step2TowerPos = new int[4];
        int p3Step2TowerCount = 0;
        //P3八人塔结束后 初始站位
        //D3 D4 H1 H2 MT ST D1 D2
        //开启P3八人塔机制后，该数组会修改为按照机制正常处理后所在位置
        int[] p3Step2EndPos = new int[8] { 1, 2, 3, 4, 1, 2, 3, 4 };
        //尼德霍格非本体ID
        Dictionary<string,int> nidhoggNotBossId = new Dictionary<string,int>();
        bool isP3Step2Endtoolpid = false;
        List<string> p4step1MarkPlayer = new List<string>(8);
        List<string> p4step1ChangePlayer = new List<string>(8);
        string[] p4step2FirstAttackPlayer = new string[2];

        public void DeInitPlugin()
        {
            if(logreader != null)
            {
                logreader.Dispose();
            }
            settingContainer.SaveSetting(Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "DragonSongRepriseHelper.v2.config"));
            ActGlobals.oFormActMain.OnCombatEnd -= OFormActMain_OnCombatEnd;
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            pluginScreenSpace.Text = "绝龙诗小助手";

            settingContainer = new SettingContainer();
            RegisterSettingForm(pluginScreenSpace, pluginStatusText);
            RegisterEvent();

            ActGlobals.oFormActMain.OnCombatEnd += OFormActMain_OnCombatEnd;
            ActGlobals.oFormActMain.OnCombatStart += OFormActMain_OnCombatStart;
        }

        private void OFormActMain_OnCombatStart(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            postNamazuHelper = new PostNamazuHelper(settingContainer.FunctionSetting.PostNamazuSetting);
            Log.Print("战斗开始");
        }

        private void OFormActMain_OnCombatEnd(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            //初始化所有变量
            ResetCombat();
        }

        public void RegisterSettingForm(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            settingContainer.LoadSetting(Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "DragonSongRepriseHelper.v2.config"));

            settingForm = new SettingForm(settingContainer,Test,()=> {
                isDebug = true;
                ResetCombat();
                OpenFileDialog fileDialog = new OpenFileDialog();
                if(fileDialog.ShowDialog() == DialogResult.OK)
                {
                    logreader.PlayLog(fileDialog.FileName);
                }
                isDebug = false;
            });
            pluginScreenSpace.Controls.Add(settingForm);
            settingForm.Dock = DockStyle.Fill;

            pluginStatusText.Text = "Plugin Started";
        }

        public void RegisterEvent()
        {
            if (logreader != null)
            {
                logreader.Dispose();
            }
            logreader = new LogReader();
            postNamazuHelper = new PostNamazuHelper(settingContainer.FunctionSetting.PostNamazuSetting);


            //注册点名事件
            logreader.RegisterEvent(27, "^(.+?)TargetIcon(\\s|\\S)+$", (log) =>
            {
                try
                {
                    if (!settingContainer.IsRaidMode)
                    {
                        return;
                    }
                    string logSubString = log.Substring(log.IndexOf("]"));
                    string markCodeStr = logSubString.Split(':')[5];
                    int markCode = Convert.ToInt32(markCodeStr, 16);
                    if (markOffset == -1)
                    {
                        markOffset = markCode - skywardTripleMark;
                        Log.Print("mark offset:" + markOffset);
                    }
                    int trueCode = markCode - markOffset;
                    //点穿天
                    if (trueCode == skywardTripleMark)
                    {
                        P2Step1Process(log);
                    }
                    //点分摊
                    if (trueCode == sword1Mark || trueCode == sword2Mark)
                    {
                        P2Step2Process(log);
                    }
                    //点陨石
                    if (trueCode == meteor)
                    {
                        P2Step3Process(log);
                    }
                    //点麻将
                    if(trueCode == mahjong1 || trueCode == mahjong2 || trueCode == mahjong3)
                    {
                        P3Step1PreProcess(log);
                    }
                }
                catch (Exception ex)
                {
                    Log.Print(ex.ToString());
                }
            });
            //注册放塔事件
            logreader.RegisterEvent(20, "^(.+?)StartsCasting(.+?)圣骑士埃尔姆诺斯特\\:737C\\:信仰(\\s|\\S)+$", (log) =>
            {
                if (!settingContainer.IsRaidMode)
                {
                    return;
                }
                try
                {
                    P2Step4Process(log);
                }
                catch (Exception ex)
                {
                    Log.Print(ex.ToString());
                }
            });
            //注册获取小队玩家事件
            logreader.RegisterEvent(00, "^(\\s|\\S)+(JJSBTT|DSRH)\\s\\{(\\s|\\S)+\\}$", (log) =>
            {
                int macroType = 0;
                string logSubString = log.Substring(log.IndexOf("]"));
                List<string> playerSettingStrs = new List<string>();

                if (logSubString.Contains("DSRH {"))
                {
                    macroType = 1;
                }
                if (logSubString.Contains("JJSBTT {"))
                {
                    macroType = 2;
                }
                if (macroType == 1)
                {
                    var index = logSubString.IndexOf("DSRH {");
                    if (index != -1)
                    {
                        var after = logSubString.Substring(index + "DSRH {".Length);
                        index = after.IndexOf("}");
                        if (index != -1)
                        {
                            var playerIds = after.Substring(0, index).Split(':');

                            foreach (var item in playerIds)
                            {
                                if (string.IsNullOrEmpty(item))
                                {
                                    continue;
                                }
                                playerSettingStrs.Add(item + ",请设置职责");
                            }
                        }
                    }
                }
                if (macroType == 2)
                {
                    var index = logSubString.IndexOf("JJJSBTT {");
                    if (index != -1)
                    {
                        var after = logSubString.Substring(index + "JJJSBTT {".Length);
                        index = after.IndexOf("}");
                        if (index != -1)
                        {
                            var playerIds = after.Substring(0, index).Split(':');
                            foreach (var item in playerIds)
                            {
                                if (string.IsNullOrEmpty(item))
                                {
                                    continue;
                                }
                                playerSettingStrs.Add(item + ",请设置职责");
                            }
                        }
                    }
                }
                if (playerSettingStrs.Count > 0)
                {
                    settingForm.SetPlayers(playerSettingStrs);
                }
            });
            //注册P3尼德霍格放塔点名事件
            logreader.RegisterEvent(26, "^(.+?)StatusAdd(.+?)\\:(AC3|AC4|AC5)\\:(\\s|\\S)+$", log => {
                if (!settingContainer.IsRaidMode)
                {
                    return;
                }
                P3Step1Process(log);
            });
            //注册进入副本事件
            logreader.RegisterEvent(01, "(\\s|\\S)+", log =>
            {
                Log.Print(log);
                string logSubString = log.Substring(log.IndexOf("]"));
                string raidCode = logSubString.Split(':')[1];
                string raidName = logSubString.Split(':')[2];
                if (raidCode == "3C8")
                {
                    settingContainer.IsRaidMode = true;
                }
                else
                {
                    settingContainer.IsRaidMode = false;
                }
                Log.Print("当前副本:" + raidName);
            });
            //注册P4开场红蓝点名事件
            logreader.RegisterEvent(26, "^(.+?)StatusAdd(.+?)\\:(AD7|AD8)\\:(\\s|\\S)+$", log =>
            {
                if (!settingContainer.IsRaidMode)
                {
                    return;
                }
                P4Step1Process(log);
            });
            //注册P4幻象冲事件
            logreader.RegisterEvent(21, "^(.+?)ActionEffect(.+?)\\:68C4\\:(\\s|\\S)+$", log =>
            {
                if (!settingContainer.IsRaidMode)
                {
                    return;
                }
                P4Step2Process(log);
            });
            logreader.RegisterEvent(20, "^(.+?)StartsCasting(.+?)\\:(6717|6718|6719|671A)\\:黑暗龙炎冲(\\s|\\S)+$",log=> {
                if (!settingContainer.IsRaidMode)
                {
                    return;
                }
                P3Step2Process(log);
            });
            logreader.RegisterEvent(35, "^(.+?)Tether(.+?)\\:0054\\:(\\s|\\S)+$", log =>
            {
                if (!settingContainer.IsRaidMode)
                {
                    return;
                }
                P3Step2EndProcess(log);
            });
            logreader.Init();
        }

        public void Test()
        {
            postNamazuHelper.SendCommand("/mk attack <" + 1 + ">");
            for (int i = 1; i <= 2; i++)
            {
                Random r = new Random();
                postNamazuHelper.SendCommand("/mk attack <" + r.Next(1, settingContainer.PlayerSetting.PlayerIndex.Count) + ">");
            }
            Clear();
        }

        //一运
        public void P2Step1Process(string log)
        {
            string logSubString = log.Substring(log.IndexOf("]"));
            string playerId = logSubString.Split(':')[2];
            Log.Print("点名" + playerId);
            //int i = Utils.GetPlayerIndex(playerSetting, playerId);
            if (settingContainer.FunctionSetting.P2Step1Enable)
            {
                p2Step1AttackPlayer.Add(playerId);
                if (p2Step1AttackPlayer.Count == 3)
                {
                    foreach (var item in settingContainer.PlayerSetting.PlayerIndex)
                    {
                        if (item.Key != p2Step1AttackPlayer[0] && item.Key != p2Step1AttackPlayer[1] && item.Key != p2Step1AttackPlayer[2]
                            && item.Key != settingContainer.PlayerSetting.MT && item.Key != settingContainer.PlayerSetting.ST)
                        {
                            postNamazuHelper.SendCommand("/mk attack <" + item.Value + ">");
                            Log.Print("无点名" + item.Key + ",顺位:" + item.Value);
                        }
                    }
                }
                Clear();
            }
        }

        //二运冲锋
        public void P2Step2Process(string log)
        {
            string logSubString = log.Substring(log.IndexOf("]"));
            string markCodeStr = logSubString.Split(':')[5];
            int markCode = Convert.ToInt32(markCodeStr, 16);
            int trueCode = markCode - markOffset;

            string playerId = logSubString.Split(':')[2];
            
            Log.Print("二运点名" + playerId);
            if (settingContainer.FunctionSetting.P2Step2Enable)
            {
                Log.Print("判断开始");
                if (trueCode == sword1Mark)
                {
                    p2Step2AttackPlayer[0] = playerId;
                }
                if (trueCode == sword2Mark)
                {
                    p2Step2AttackPlayer[1] = playerId;
                }
                if (p2Step2AttackPlayer[0] != null && p2Step2AttackPlayer[1] != null)
                {
                    string[] mtgroup = new string[4];
                    string[] stgroup = new string[4];
                    mtgroup[0] = settingContainer.PlayerSetting.MT;
                    mtgroup[1] = settingContainer.PlayerSetting.H1;
                    mtgroup[2] = settingContainer.PlayerSetting.D1;
                    mtgroup[3] = settingContainer.PlayerSetting.D3;
                    stgroup[0] = settingContainer.PlayerSetting.ST;
                    stgroup[1] = settingContainer.PlayerSetting.H2;
                    stgroup[2] = settingContainer.PlayerSetting.D2;
                    stgroup[3] = settingContainer.PlayerSetting.D4;
                    Log.Print("mtgroup.Contains(p2Step2AttackPlayer[0]) && mtgroup.Contains(p2Step2AttackPlayer[1]) == " + (mtgroup.Contains(p2Step2AttackPlayer[0]) && mtgroup.Contains(p2Step2AttackPlayer[1]) == true ? "true" : "false"));
                    Log.Print("stgroup.Contains(p2Step2AttackPlayer[0]) && stgroup.Contains(p2Step2AttackPlayer[1]) == " + (stgroup.Contains(p2Step2AttackPlayer[0]) && stgroup.Contains(p2Step2AttackPlayer[1]) == true ? "true" : "false"));
                    if (mtgroup.Contains(p2Step2AttackPlayer[0]) && mtgroup.Contains(p2Step2AttackPlayer[1]))
                    {
                        if (settingContainer.FunctionSetting.P2Step2Enable)
                        {
                            postNamazuHelper.SendCommand("/y 【冲锋换位提醒】D2 " + stgroup[2] + " 进行换位");
                            postNamazuHelper.SendCommand("/p 【冲锋换位提醒】D2 " + stgroup[2] + " 进行换位<se.8>");
                            if (!settingContainer.FunctionSetting.P2Step2MarkDisabled)
                            {
                                postNamazuHelper.SendCommand("/mk circle <" + settingContainer.PlayerSetting.PlayerIndex[stgroup[2]] + ">");
                            }
                            Clear();
                        }

                    }
                    if (stgroup.Contains(p2Step2AttackPlayer[0]) && stgroup.Contains(p2Step2AttackPlayer[1]))
                    {
                        if (settingContainer.FunctionSetting.P2Step2Enable)
                        {
                            postNamazuHelper.SendCommand("/y 【冲锋换位提醒】D1 " + mtgroup[2] + " 进行换位");
                            postNamazuHelper.SendCommand("/p 【冲锋换位提醒】D1 " + mtgroup[2] + " 进行换位<se.8>");
                            if (!settingContainer.FunctionSetting.P2Step2MarkDisabled)
                            {
                                postNamazuHelper.SendCommand("/mk circle <" + settingContainer.PlayerSetting.PlayerIndex[mtgroup[2]] + ">");
                            }
                            Clear();
                        }

                    }
                }
            }
            Log.Print("二运点名" + playerId);
        }

        //二运陨石预站位
        public void P2Step3Process(string log)
        {
            string logSubString = log.Substring(log.IndexOf("]"));
            string playerId = logSubString.Split(':')[2];
            p2Step3AttackPlayer.Add(playerId);
            Log.Print("陨石点名" + playerId);
            if (p2Step3AttackPlayer.Count == 2)
            {
                if (!settingContainer.FunctionSetting.P2Step3Enable)
                {
                    return;
                }
                string[] group1 = new string[2];//A点组
                string[] group2 = new string[2];//C点组
                string[] group3 = new string[2];//D点组
                string[] group4 = new string[2];//B点组
                group1[0] = settingContainer.PlayerSetting.MT;
                group1[1] = settingContainer.PlayerSetting.D1;
                group2[0] = settingContainer.PlayerSetting.ST;
                group2[1] = settingContainer.PlayerSetting.D2;
                group3[0] = settingContainer.PlayerSetting.H1;
                group3[1] = settingContainer.PlayerSetting.D3;
                group4[0] = settingContainer.PlayerSetting.H2;
                group4[1] = settingContainer.PlayerSetting.D4;

                //AC点名 不处理
                if ((group1.Contains(p2Step3AttackPlayer[0]) && group2.Contains(p2Step3AttackPlayer[1])) || (group2.Contains(p2Step3AttackPlayer[0]) && group1.Contains(p2Step3AttackPlayer[1])))
                {
                    p2Step3AttackGroupType = "AC";
                    p2Step3AttackGroup[0] = group1;
                    p2Step3AttackGroup[1] = group2;
                    Log.Print("AC点名 不处理");
                    PrintP2Step3AttackGroup();
                    return;
                }
                //BD点名 不处理
                if ((group3.Contains(p2Step3AttackPlayer[0]) && group4.Contains(p2Step3AttackPlayer[1])) || (group4.Contains(p2Step3AttackPlayer[0]) && group3.Contains(p2Step3AttackPlayer[1])))
                {
                    p2Step3AttackGroupType = "BD";
                    p2Step3AttackGroup[0] = group4;
                    p2Step3AttackGroup[1] = group3;
                    PrintP2Step3AttackGroup();

                    Log.Print("BD点名 不处理");
                    return;
                }
                //AB点名 换位
                if ((group1.Contains(p2Step3AttackPlayer[0]) && group4.Contains(p2Step3AttackPlayer[1])) || (group4.Contains(p2Step3AttackPlayer[0]) && group1.Contains(p2Step3AttackPlayer[1])))
                {
                    p2Step3AttackGroupType = "AC";
                    p2Step3AttackGroup[0] = group1;
                    p2Step3AttackGroup[1] = group4;
                    PrintP2Step3AttackGroup();

                    Log.Print("AB点名 换位");
                    postNamazuHelper.SendCommand("/y 【陨石换位提醒】B组与C组换位<se.8>");
                    postNamazuHelper.SendCommand("/p 【陨石换位提醒】B组与C组换位<se.8>");
                    return;
                }
                //AD点名 换位
                if ((group1.Contains(p2Step3AttackPlayer[0]) && group3.Contains(p2Step3AttackPlayer[1])) || (group3.Contains(p2Step3AttackPlayer[0]) && group1.Contains(p2Step3AttackPlayer[1])))
                {
                    p2Step3AttackGroupType = "AC";
                    p2Step3AttackGroup[0] = group1;
                    p2Step3AttackGroup[1] = group3;
                    PrintP2Step3AttackGroup();

                    Log.Print("AD点名 换位");
                    postNamazuHelper.SendCommand("/y 【陨石换位提醒】D组与C组换位<se.8>");
                    postNamazuHelper.SendCommand("/p 【陨石换位提醒】D组与C组换位<se.8>");
                    return;
                }

                //BC点名 换位
                if ((group2.Contains(p2Step3AttackPlayer[0]) && group4.Contains(p2Step3AttackPlayer[1])) || (group4.Contains(p2Step3AttackPlayer[0]) && group2.Contains(p2Step3AttackPlayer[1])))
                {
                    p2Step3AttackGroupType = "AC";
                    p2Step3AttackGroup[0] = group4;
                    p2Step3AttackGroup[1] = group2;
                    PrintP2Step3AttackGroup();

                    Log.Print("BC点名 换位");
                    postNamazuHelper.SendCommand("/y 【陨石换位提醒】B组与A组换位<se.8>");
                    postNamazuHelper.SendCommand("/p 【陨石换位提醒】B组与A组换位<se.8>");
                    return;
                }
                //CD点名 换位
                if ((group2.Contains(p2Step3AttackPlayer[0]) && group3.Contains(p2Step3AttackPlayer[1])) || (group3.Contains(p2Step3AttackPlayer[0]) && group2.Contains(p2Step3AttackPlayer[1])))
                {
                    p2Step3AttackGroupType = "AC";
                    p2Step3AttackGroup[0] = group3;
                    p2Step3AttackGroup[1] = group2;
                    PrintP2Step3AttackGroup();

                    Log.Print("CD点名 换位");
                    postNamazuHelper.SendCommand("/y 【陨石换位提醒】D组与A组换位<se.8>");
                    postNamazuHelper.SendCommand("/p 【陨石换位提醒】D组与A组换位<se.8>");
                    return;
                }
            }
        }

        //二运放陨石
        public void P2Step4Process(string log)
        {
            if (!settingContainer.FunctionSetting.P2Step3Enable)
            {
                return;
            }
            if (!settingContainer.FunctionSetting.P2Step4Enable)
            {
                return;
            }
            //陨石点名了 进行处理
            if (p2Step3AttackPlayer.Count == 2)
            {
                if(p2Step4TowerCount == 8)
                {
                    return;
                }
                string logSubString = log.Substring(log.IndexOf("]"));
                string pointXStr = logSubString.Split(':')[8];
                string pointYStr = logSubString.Split(':')[9];
                Log.Print("p2Step4TowerCount:" + p2Step4TowerCount);
                p2Step4OutTower.SetExistOutTower(Convert.ToDouble(pointXStr), Convert.ToDouble(pointYStr));
                p2Step4TowerCount++;
                if(p2Step4TowerCount == 8)
                {
                    p2Step4OutTower.PrintLog();
                    //AC组跑陨石情况
                    if (p2Step3AttackGroupType == "AC")
                    {
                        if ((p2Step4OutTower.tAOutLeft && p2Step4OutTower.tCOutRight && !p2Step4OutTower.tCOutLeft) && (!p2Step4OutTower.tAOutMid && !p2Step4OutTower.tCOutMid))
                        {
                            postNamazuHelper.SendCommand("/p 【陨石飙车提醒】120度塔出现,C点组注意陨石间距<se.8>");
                            postNamazuHelper.SendCommand("/y 【陨石飙车提醒】120度塔出现,C点组注意陨石间距");
                            Log.Print("阴间塔命中,groupType:" + p2Step3AttackGroupType);
                            //A点有右塔，可建议换位
                            if (settingContainer.FunctionSetting.P2Step4ChangeTowerEnable && p2Step4OutTower.tAOutRight)
                            {
                                string player1 = p2Step3AttackGroup[0][0];
                                string player2 = p2Step3AttackGroup[0][1];
                                postNamazuHelper.SendCommand("/p 【陨石踩塔换位提醒】A组" + player1 + "可与" + player2 + "进行换位");
                                postNamazuHelper.SendCommand("/mk attack <" + settingContainer.PlayerSetting.PlayerIndex[player1] + ">");
                                postNamazuHelper.SendCommand("/mk attack <" + settingContainer.PlayerSetting.PlayerIndex[player2] + ">");
                                Clear();
                            }
                        }

                        if ((p2Step4OutTower.tAOutRight && p2Step4OutTower.tCOutLeft && !p2Step4OutTower.tAOutLeft) && (!p2Step4OutTower.tAOutMid && !p2Step4OutTower.tCOutMid))
                        {
                            postNamazuHelper.SendCommand("/p 【陨石飙车提醒】120度塔出现,A点组注意陨石间距<se.8>");
                            postNamazuHelper.SendCommand("/y 【陨石飙车提醒】120度塔出现,A点组注意陨石间距");
                            Log.Print("阴间塔命中,groupType:" + p2Step3AttackGroupType);
                            //C点有右塔，可建议换位
                            if (p2Step4OutTower.tCOutRight)
                            {
                                string player1 = p2Step3AttackGroup[1][0];
                                string player2 = p2Step3AttackGroup[1][1];
                                postNamazuHelper.SendCommand("/p 【陨石踩塔换位提醒】C组" + player1 + "可与" + player2 + "进行换位");
                                postNamazuHelper.SendCommand("/mk attack <" + settingContainer.PlayerSetting.PlayerIndex[player1] + ">");
                                postNamazuHelper.SendCommand("/mk attack <" + settingContainer.PlayerSetting.PlayerIndex[player2] + ">");
                                Clear();
                            }
                        }
                    }

                    if(p2Step3AttackGroupType == "BD")
                    {
                        //BD组跑陨石情况
                        if ((p2Step4OutTower.tBOutLeft && p2Step4OutTower.tDOutRight && !p2Step4OutTower.tDOutLeft) && (!p2Step4OutTower.tBOutMid && !p2Step4OutTower.tDOutMid))
                        {
                            postNamazuHelper.SendCommand("/p 【陨石飙车提醒】120度塔出现,D点组注意陨石间距<se.8>");
                            postNamazuHelper.SendCommand("/y 【陨石飙车提醒】120度塔出现,D点组注意陨石间距");
                            Log.Print("阴间塔命中,groupType:" + p2Step3AttackGroupType);
                            //B点有右塔，可建议换位
                            if (p2Step4OutTower.tBOutRight)
                            {
                                string player1 = p2Step3AttackGroup[0][0];
                                string player2 = p2Step3AttackGroup[0][1];
                                postNamazuHelper.SendCommand("/p 【陨石踩塔换位提醒】B组" + player1 + "可与" + player2 + "进行换位");
                                postNamazuHelper.SendCommand("/mk attack <" + settingContainer.PlayerSetting.PlayerIndex[player1] + ">");
                                postNamazuHelper.SendCommand("/mk attack <" + settingContainer.PlayerSetting.PlayerIndex[player2] + ">");
                                Clear();
                            }
                        }
                        if ((p2Step4OutTower.tBOutRight && p2Step4OutTower.tDOutLeft && !p2Step4OutTower.tBOutLeft) && (!p2Step4OutTower.tBOutMid && !p2Step4OutTower.tDOutMid))
                        {
                            postNamazuHelper.SendCommand("/p 【陨石飙车提醒】120度塔出现,B点组注意陨石间距<se.8>");
                            postNamazuHelper.SendCommand("/y 【陨石飙车提醒】120度塔出现,B点组注意陨石间距");
                            //D点有右塔，可建议换位
                            if (p2Step4OutTower.tDOutRight)
                            {
                                string player1 = p2Step3AttackGroup[1][0];
                                string player2 = p2Step3AttackGroup[1][1];
                                postNamazuHelper.SendCommand("/p 【陨石踩塔换位提醒】D组" + player1 + "可与" + player2 + "进行换位");
                                postNamazuHelper.SendCommand("/mk attack <" + settingContainer.PlayerSetting.PlayerIndex[player1] + ">");
                                postNamazuHelper.SendCommand("/mk attack <" + settingContainer.PlayerSetting.PlayerIndex[player2] + ">");
                                Clear();
                            }
                        }
                    }
                }
            }
            
        }

        //P3点麻将
        public void P3Step1PreProcess(string log)
        {
            string logSubString = log.Substring(log.IndexOf("]"));
            string markCodeStr = logSubString.Split(':')[5];
            int markCode = Convert.ToInt32(markCodeStr, 16);
            int trueCode = markCode - markOffset;
            string playerId = logSubString.Split(':')[2];

            if (trueCode == mahjong1)
            {
                Log.Print("麻将1点名:" + playerId);
                p3Step1MarkMahjong1Player.Add(playerId);
            }
            if (trueCode == mahjong2)
            {
                Log.Print("麻将2点名:" + playerId);
                p3Step1MarkMahjong2Player.Add(playerId);
            }
            if (trueCode == mahjong3)
            {
                Log.Print("麻将3点名:" + playerId);
                p3Step1MarkMahjong3Player.Add(playerId);
            }
        }

        //P3点箭头圆圈
        public void P3Step1Process(string log)
        {
            //00|2022-09-07T20:54:36.0000000+08:00|112F||陷入了“选定目标：黑暗破碎冲”效果。|6ce23a831a3c4b46  上箭头
            //00|2022-09-07T20:54:36.0000000+08:00|112F||太阳海岸陷入了“选定目标：黑暗回避跳跃”效果。|03617372651d4871 下箭头
            //[2022-09-07T20:54:37.1610000+08:00] StatusAdd 1A:AC3:选定目标：黑暗高跳:9.00:E0000000::1005B87C:露缇:00:62713:

            string logSubString = log.Substring(log.IndexOf("]"));
            string gainCodeStr = logSubString.Split(':')[1];
            string gainName = logSubString.Split(':')[2];
            string playerId = logSubString.Split(':')[7];

            Log.Print("P3放塔点名" + playerId + " 类型:" + gainCodeStr + " 技能名:" + gainName);

            if (p3Step1TowerPos.ContainsKey(playerId))
            {
                Log.Print(playerId + "点名存在 跳过");
                return;
            }

            p3Step1TowerPos.Add(playerId, Convert.ToInt32(gainCodeStr, 16));

            if(p3Step1TowerPos.Count == 8 && p3Step1MarkMahjong1Player.Count == 3 && p3Step1MarkMahjong2Player.Count == 2 && p3Step1MarkMahjong3Player.Count == 3)
            {
                //小组顺序，类型，顺位,玩家ID
                List<Tuple<int, string, string,string>> commands = new List<Tuple<int, string, string, string>>();
                bool group1NotAllInPlace = false;
                bool group2NotAllInPlace = false;
                bool group3NotAllInPlace = false;
                foreach (var item in p3Step1MarkMahjong1Player)
                {
                    if (p3Step1TowerPos.ContainsKey(item))
                    {
                        group1NotAllInPlace = group1NotAllInPlace | p3Step1TowerPos[item] != towerPosInPlace;
                    }
                }
                foreach (var item in p3Step1MarkMahjong2Player)
                {
                    if (p3Step1TowerPos.ContainsKey(item))
                    {
                        group2NotAllInPlace = group2NotAllInPlace | p3Step1TowerPos[item] != towerPosInPlace;
                    }
                }
                foreach (var item in p3Step1MarkMahjong3Player)
                {
                    if (p3Step1TowerPos.ContainsKey(item))
                    {
                        group3NotAllInPlace = group3NotAllInPlace | p3Step1TowerPos[item] != towerPosInPlace;
                    }
                }

                Log.Print("麻将1组是否有前后塔：" + group1NotAllInPlace);
                Log.Print("麻将2组是否有前后塔：" + group2NotAllInPlace);
                Log.Print("麻将3组是否有前后塔：" + group3NotAllInPlace);

                //麻将1组标点
                if (group1NotAllInPlace)
                {
                    foreach (var item in p3Step1MarkMahjong1Player)
                    {
                        if (p3Step1TowerPos.ContainsKey(item))
                        {
                            if (p3Step1TowerPos[item] == towerPosInPlace)
                            {
                                commands.Add(new Tuple<int, string, string,string>(settingContainer.PlayerSetting.PlayerIndex[item], "attack", "3",item));
                            }
                            if (p3Step1TowerPos[item] == towerPosFront)
                            {
                                commands.Add(new Tuple<int, string, string, string>(settingContainer.PlayerSetting.PlayerIndex[item], "attack", "1", item));
                            }
                            if (p3Step1TowerPos[item] == towerPosBack)
                            {
                                commands.Add(new Tuple<int, string, string, string>(settingContainer.PlayerSetting.PlayerIndex[item], "attack", "2", item));
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in p3Step1MarkMahjong1Player)
                    {
                        commands.Add(new Tuple<int, string, string,string>(settingContainer.PlayerSetting.PlayerIndex[item], "attack", "",item));
                    }
                }

                //麻将2组标点
                if (group2NotAllInPlace)
                {
                    foreach (var item in p3Step1MarkMahjong2Player)
                    {
                        if (p3Step1TowerPos.ContainsKey(item))
                        {
                            if (p3Step1TowerPos[item] == towerPosFront)
                            {
                                commands.Add(new Tuple<int, string, string,string>(settingContainer.PlayerSetting.PlayerIndex[item], "stop", "1", item));
                            }
                            if (p3Step1TowerPos[item] == towerPosBack)
                            {
                                commands.Add(new Tuple<int, string, string, string>(settingContainer.PlayerSetting.PlayerIndex[item], "stop", "2", item));
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in p3Step1MarkMahjong2Player)
                    {
                        commands.Add(new Tuple<int, string, string, string>(settingContainer.PlayerSetting.PlayerIndex[item], "stop", "", item));
                    }
                }

                //麻将3组标点
                if (group3NotAllInPlace)
                {
                    foreach (var item in p3Step1MarkMahjong3Player)
                    {
                        if (p3Step1TowerPos.ContainsKey(item))
                        {
                            if (p3Step1TowerPos[item] == towerPosInPlace)
                            {
                                commands.Add(new Tuple<int, string, string,string>(settingContainer.PlayerSetting.PlayerIndex[item], "bind", "3", item));
                            }
                            if (p3Step1TowerPos[item] == towerPosFront)
                            {
                                commands.Add(new Tuple<int, string, string, string>(settingContainer.PlayerSetting.PlayerIndex[item], "bind", "1", item));
                            }
                            if (p3Step1TowerPos[item] == towerPosBack)
                            {
                                commands.Add(new Tuple<int, string, string, string>(settingContainer.PlayerSetting.PlayerIndex[item], "bind", "2", item));
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in p3Step1MarkMahjong3Player)
                    {
                        commands.Add(new Tuple<int, string, string, string>(settingContainer.PlayerSetting.PlayerIndex[item], "bind", "", item));
                    }
                }

                foreach(var item in commands)
                {
                    Log.Print("头顶标记:" + item.Item4 + ",type:" + item.Item2 + ",index:" + item.Item3);
                }

                if (settingContainer.FunctionSetting.P3Step1Enable)
                {
                    foreach (var item in commands)
                    {
                        postNamazuHelper.SendCommand("/mk " + item.Item2 + item.Item3 + " <" + item.Item1 + ">");
                    }

                    postNamazuHelper.SendCommand("/p 【放塔提醒】");
                    //一组消息提醒
                    if (group1NotAllInPlace)
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("/p 麻将1：");
                        foreach (var item in commands)
                        {
                            if (item.Item2 == "attack")
                            {
                                switch (item.Item3)
                                {
                                    case "1": str.Append("上箭头《攻击1》(").Append(item.Item4).Append(") "); break;
                                    case "2": str.Append("下箭头《攻击2》(").Append(item.Item4).Append(") "); break;
                                    case "3": str.Append("原地塔《攻击3》(").Append(item.Item4).Append(") "); break;
                                }
                            }
                        }
                        postNamazuHelper.SendCommand(str.ToString());
                    }
                    else
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("/p 麻将1(均为原地塔)：");
                        foreach (var item in commands)
                        {
                            if (item.Item2 == "attack")
                            {
                                str.Append(item.Item4).Append(" ");
                            }
                        }

                        postNamazuHelper.SendCommand(str.ToString());
                    }
                    //二组消息提醒
                    if (group2NotAllInPlace)
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("/p 麻将2：");
                        foreach (var item in commands)
                        {
                            if (item.Item2 == "stop")
                            {
                                switch (item.Item3)
                                {
                                    case "1": str.Append("上箭头《禁止1》(").Append(item.Item4).Append(") "); break;
                                    case "2": str.Append("下箭头《禁止2》(").Append(item.Item4).Append(") "); break;
                                }
                            }
                        }
                        postNamazuHelper.SendCommand(str.ToString());
                    }
                    else
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("/p 麻将2(均为原地塔)：");
                        foreach (var item in commands)
                        {
                            if (item.Item2 == "stop")
                            {
                                str.Append(item.Item4).Append(" ");
                            }
                        }

                        postNamazuHelper.SendCommand(str.ToString());
                    }
                    //三组消息提醒
                    if (group3NotAllInPlace)
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("/p 麻将3：");
                        foreach (var item in commands)
                        {
                            if (item.Item2 == "bind")
                            {
                                switch (item.Item3)
                                {
                                    case "1": str.Append("上箭头《止步1》(").Append(item.Item4).Append(") "); break;
                                    case "2": str.Append("下箭头《止步2》(").Append(item.Item4).Append(") "); break;
                                    case "3": str.Append("原地塔《止步3》(").Append(item.Item4).Append(") "); break;
                                }
                            }
                        }
                        postNamazuHelper.SendCommand(str.ToString());
                    }
                    else
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("/p 麻将3(均为原地塔)：");
                        foreach (var item in commands)
                        {
                            if (item.Item2 == "bind")
                            {
                                str.Append(item.Item4).Append(" ");
                            }
                        }

                        postNamazuHelper.SendCommand(str.ToString());
                    }

                    Clear(20000);
                }
            }
        }

        //P3八人塔换位
        public void P3Step2Process(string log)
        {
            string logSubString = log.Substring(log.IndexOf("]"));
            string towerCode = logSubString.Split(':')[3];
            string bossID = logSubString.Split(':')[1];
            double posX = Convert.ToDouble(logSubString.Split(':')[8]);
            double posY = Convert.ToDouble(logSubString.Split(':')[9]);
            //[22:06:22.682] StartsCasting 14:400195AB:尼德霍格:6718:黑暗龙炎冲:400195AB:尼德霍格:4.700:108.00:108.00:0.00:-2.18
            //D3塔
            if (posX == 92 && posY == 92)
            {
                p3Step2TowerPos[0] = Convert.ToInt32(towerCode, 16);
                nidhoggNotBossId.Add(bossID,1);
            }
            //D4塔
            if (posX == 108 && posY == 92)
            {
                p3Step2TowerPos[1] = Convert.ToInt32(towerCode, 16);
                nidhoggNotBossId.Add(bossID,2);
            }
            //H1塔
            if (posX == 92 && posY == 108)
            {
                p3Step2TowerPos[2] = Convert.ToInt32(towerCode, 16);
                nidhoggNotBossId.Add(bossID,3);
            }
            //H2塔
            if (posX == 108 && posY == 108)
            {
                p3Step2TowerPos[3] = Convert.ToInt32(towerCode, 16);
                nidhoggNotBossId.Add(bossID,4);
            }
            

            p3Step2TowerCount++;
            if(p3Step2TowerCount == 4)
            {
                string str = "";
                for (int i = 0;i<= p3Step2TowerPos.Length - 1; i++)
                {
                    if(i == 0)
                    {
                        str += "D3塔类型：";
                    }
                    if (i == 1)
                    {
                        str += " D4塔类型：";
                    }
                    if (i == 2)
                    {
                        str += " H1塔类型：";
                    }
                    if (i == 3)
                    {
                        str += " H2塔类型：";
                    }

                    str += (p3Step2TowerPos[i] - eightTowerOne + 1).ToString();
                }
                Log.Print(str);
                //换位类型 1=左 2=对侧 3=右
                //MT ST D1 D2
                int[] changeType = new int[4] { 0, 0, 0, 0 };
                if(p3Step2TowerPos[0] == eightTowerOne)
                {
                    //左塔（2号D4）
                    if(p3Step2TowerPos[1] != eightTowerOne && p3Step2TowerPos[1] != eightTowerTwo)
                    {
                        changeType[0] = 1;
                        p3Step2EndPos[4] = 2;
                    }
                    //右塔（3号H1）
                    else if (p3Step2TowerPos[2] != eightTowerOne && p3Step2TowerPos[2] != eightTowerTwo)
                    {
                        changeType[0] = 3;
                        p3Step2EndPos[4] = 3;
                    }
                    //对侧塔（4号H2）
                    else if (p3Step2TowerPos[3] != eightTowerOne && p3Step2TowerPos[3] != eightTowerTwo)
                    {
                        changeType[0] = 2;
                        p3Step2EndPos[4] = 4;
                    }
                }
                if (p3Step2TowerPos[1] == eightTowerOne)
                {
                    //左塔（4号H2）
                    if (p3Step2TowerPos[3] != eightTowerOne && p3Step2TowerPos[3] != eightTowerTwo)
                    {
                        changeType[1] = 1;
                        p3Step2EndPos[5] = 4;
                    }
                    //右边（1号D3）
                    else if (p3Step2TowerPos[0] != eightTowerOne && p3Step2TowerPos[0] != eightTowerTwo)
                    {
                        changeType[1] = 3;
                        p3Step2EndPos[5] = 1;
                    }
                    //对侧塔（3号H1）
                    else if (p3Step2TowerPos[2] != eightTowerOne && p3Step2TowerPos[2] != eightTowerTwo)
                    {
                        changeType[1] = 2;
                        p3Step2EndPos[5] = 3;
                    }
                }
                if (p3Step2TowerPos[2] == eightTowerOne)
                {
                    //左塔（1号D3）
                    if (p3Step2TowerPos[0] != eightTowerOne && p3Step2TowerPos[0] != eightTowerTwo)
                    {
                        changeType[2] = 1;
                        p3Step2EndPos[6] = 1;
                    }
                    //右边（4号H2）
                    else if (p3Step2TowerPos[3] != eightTowerOne && p3Step2TowerPos[3] != eightTowerTwo)
                    {
                        changeType[2] = 3;
                        p3Step2EndPos[6] = 4;
                    }
                    //对侧塔（2号D4）
                    else if (p3Step2TowerPos[1] != eightTowerOne && p3Step2TowerPos[1] != eightTowerTwo)
                    {
                        changeType[2] = 2;
                        p3Step2EndPos[6] = 2;
                    }
                }
                if (p3Step2TowerPos[3] == eightTowerOne)
                {
                    //左塔（3号H1）
                    if (p3Step2TowerPos[2] != eightTowerOne && p3Step2TowerPos[2] != eightTowerTwo)
                    {
                        changeType[3] = 1;
                        p3Step2EndPos[7] = 3;
                    }
                    //右边（2号D4）
                    else if (p3Step2TowerPos[1] != eightTowerOne && p3Step2TowerPos[1] != eightTowerTwo)
                    {
                        changeType[3] = 3;
                        p3Step2EndPos[7] = 2;
                    }
                    //对侧塔（1号D3）
                    else if (p3Step2TowerPos[0] != eightTowerOne && p3Step2TowerPos[0] != eightTowerTwo)
                    {
                        changeType[3] = 2;
                        p3Step2EndPos[7] = 1;
                    }
                }

                Stack<string> commands = new Stack<string>();
                commands.Push("/mk bind{0} <{1}>");
                commands.Push("/mk attack{0} <{1}>");
                string message = "/p 【八人塔换位】";
                bool isNotNeedChange = false;
                if (!this.settingContainer.FunctionSetting.P3Step2Enable)
                {
                    return;
                }
                for (int i = 0;i<= changeType.Length - 1; i++)
                {
                    if (changeType[i] != 0)
                    {
                        string command = commands.Pop();
                        string index = null;
                        string type = null;
                        string playerId = null;
                        isNotNeedChange = true;
                        if (i == 0)
                        {
                            playerId = settingContainer.PlayerSetting.MT;
                        }
                        if (i == 1)
                        {
                            playerId = settingContainer.PlayerSetting.ST;
                        }
                        if (i == 2)
                        {
                            playerId = settingContainer.PlayerSetting.D1;
                        }
                        if (i == 3)
                        {
                            playerId = settingContainer.PlayerSetting.D2;
                        }
                        Log.Print(playerId + "换位情况：" + changeType[i]);
                        index = settingContainer.PlayerSetting.PlayerIndex[playerId].ToString();
                        type = changeType[i].ToString();
                        command = string.Format(command, type, index);
                        postNamazuHelper.SendCommand(command);
                        message += playerId + "(" + (type == "1" ? "左" : (type == "2" ? "对侧" : "右")) + ") ";
                        Clear(3000);
                    }
                }
                if (isNotNeedChange)
                {
                    message += "无";
                }
                postNamazuHelper.SendCommand(message);
            }
        }

        //P3八人塔放线分身位置
        public void P3Step2EndProcess(string log)
        {
            string logSubString = log.Substring(log.IndexOf("]"));
            string bossID = logSubString.Split(':')[1];

            if (nidhoggNotBossId.ContainsKey(bossID))
            {
                int pos = nidhoggNotBossId[bossID];
                int attackId = 0;
                Log.Print("尼德霍格分身出线ID：" + bossID + ",位置:" + pos);
                
                if (!this.settingContainer.FunctionSetting.P3Step2EndEnable || isP3Step2Endtoolpid)
                {
                    return;
                }
                isP3Step2Endtoolpid = true;
                if (pos == 1)
                {
                    postNamazuHelper.SendCommand("/p 【分身线出现位置】左上（4点）（D3组）");
                    attackId = 4;
                }
                if (pos == 2)
                {
                    postNamazuHelper.SendCommand("/p 【分身线出现位置】右上（1点）（D4组）");
                    attackId = 1;
                }
                if (pos == 3)
                {
                    postNamazuHelper.SendCommand("/p 【分身线出现位置】左下（3点）（H1组）");
                    attackId = 3;
                }
                if (pos == 4)
                {
                    postNamazuHelper.SendCommand("/p 【分身线出现位置】右下（2点）（H2组）");
                    attackId = 2;
                }
                int index = this.settingContainer.PlayerSetting.PlayerIndex[this.settingContainer.PlayerSetting.ST];
                postNamazuHelper.SendCommand("/mk attack" + attackId + " <" + index + ">");
                
                Clear();
            }
        }

        //P4开场红蓝点名
        public void P4Step1Process(string log)
        {
            string logSubString = log.Substring(log.IndexOf("]"));
            string gainCodeStr = logSubString.Split(':')[1];
            string gainName = logSubString.Split(':')[2];
            string playerId = logSubString.Split(':')[7];

            Log.Print("P4红蓝点名" + playerId + " 类型:" + gainCodeStr + " 技能名:" + gainName);

            var gainCode = Convert.ToInt32(gainCodeStr, 16);
            if(gainCode == p4redmark && !this.settingContainer.PlayerSetting.IsDps(playerId))
            {
                p4step1ChangePlayer.Add(playerId);
            }
            if (gainCode == p4bluemark && this.settingContainer.PlayerSetting.IsDps(playerId))
            {
                p4step1ChangePlayer.Add(playerId);
            }

            p4step1MarkPlayer.Add(playerId);
            if(p4step1MarkPlayer.Count == 8)
            {
                Stack<string> markList = BuildMarkStack();
                StringBuilder sb = new StringBuilder();
                foreach (var item in p4step1ChangePlayer)
                {
                    if (settingContainer.FunctionSetting.P4Step1Enable)
                    {
                        postNamazuHelper.SendCommand(string.Format(markList.Pop(), this.settingContainer.PlayerSetting.PlayerIndex[item]));
                        sb.Append(item + " ");
                        Clear();
                    }
                    Log.Print("需换位:" + item);
                }

                postNamazuHelper.SendCommand(string.Format("/p 【红蓝换位】" + sb.ToString()));
            }
        }

        //P4幻象冲
        public void P4Step2Process(string log)
        {
            if(p4step2FirstAttackPlayer[0] != null && p4step2FirstAttackPlayer[1] != null)
            {
                return;
            }
            if(p4step2FirstAttackPlayer[0] == null || p4step2FirstAttackPlayer[1] == null)
            {
                string logSubString = log.Substring(log.IndexOf("]"));
                string playerId = logSubString.Split(':')[6];

                if(p4step2FirstAttackPlayer[0] == null)
                {
                    p4step2FirstAttackPlayer[0] = playerId;
                    return;
                }

                if (p4step2FirstAttackPlayer[1] == null)
                {
                    p4step2FirstAttackPlayer[1] = playerId;
                }
            }

            foreach (var item in p4step2FirstAttackPlayer)
            {
                Log.Print("P4幻象冲第一次:" + item);
            }

            string[] tnRank = new string[] { this.settingContainer.PlayerSetting.MT,
                this.settingContainer.PlayerSetting.ST,
                this.settingContainer.PlayerSetting.H1,
                this.settingContainer.PlayerSetting.H2 };

            int index1 = -1;
            int index2 = -1;

            for(int i = 0; i <= 3; i++)
            {
                if(tnRank[i] == p4step2FirstAttackPlayer[0])
                {
                    index1 = i;
                }
                if (tnRank[i] == p4step2FirstAttackPlayer[1])
                {
                    index2 = i;
                }
            }

            if (index1 == -1 || index2 == -1)
            {
                if (this.settingContainer.FunctionSetting.P4Step2Enable)
                {
                    postNamazuHelper.SendCommand(string.Format("/mk attack <{0}>", this.settingContainer.PlayerSetting.PlayerIndex[p4step2FirstAttackPlayer[0]]));
                    postNamazuHelper.SendCommand(string.Format("/mk attack <{0}>", this.settingContainer.PlayerSetting.PlayerIndex[p4step2FirstAttackPlayer[1]]));
                    postNamazuHelper.SendCommand("/p 【幻象冲第一次点名】" + p4step2FirstAttackPlayer[0] + " " + p4step2FirstAttackPlayer[1] + "");
                    Clear(15000);
                }
                Log.Print("非正常处理方式，随机标记");
            }

            if (index1 < index2)
            {
                if (this.settingContainer.FunctionSetting.P4Step2Enable)
                {
                    
                    Log.Print("标1:" + p4step2FirstAttackPlayer[0]);
                    Log.Print("标2:" + p4step2FirstAttackPlayer[1]);
                    postNamazuHelper.SendCommand(string.Format("/mk attack1 <{0}>", this.settingContainer.PlayerSetting.PlayerIndex[p4step2FirstAttackPlayer[0]]));
                    postNamazuHelper.SendCommand(string.Format("/mk attack2 <{0}>", this.settingContainer.PlayerSetting.PlayerIndex[p4step2FirstAttackPlayer[1]]));
                    postNamazuHelper.SendCommand("/p 【幻象冲第一次点名】" + p4step2FirstAttackPlayer[0] + "(高顺位) " + p4step2FirstAttackPlayer[1] + "(低顺位)");
                    Clear(15000);
                    return;
                }
                
            }

            if (index1 > index2)
            {
                if (this.settingContainer.FunctionSetting.P2Step2Enable)
                {
                    Log.Print("标1:" + p4step2FirstAttackPlayer[1]);
                    Log.Print("标2:" + p4step2FirstAttackPlayer[0]);
                    postNamazuHelper.SendCommand(string.Format("/mk attack2 <{0}>", this.settingContainer.PlayerSetting.PlayerIndex[p4step2FirstAttackPlayer[0]]));
                    postNamazuHelper.SendCommand(string.Format("/mk attack1 <{0}>", this.settingContainer.PlayerSetting.PlayerIndex[p4step2FirstAttackPlayer[1]]));
                    postNamazuHelper.SendCommand("/p 【幻象冲第一次点名】" + p4step2FirstAttackPlayer[1] + "(高顺位) " + p4step2FirstAttackPlayer[0] + "(低顺位)");
                    Clear(15000);
                    return;
                }
            }
        }
        public void Clear(int wait = 5000)
        {
            if (isDebug)
            {
                postNamazuHelper.SendCommand("/e 清理 timer:" + wait);
            }
            if (!clear)
            {
                new Thread(() =>
                {
                    Thread.Sleep(wait);
                    postNamazuHelper.SendCommand("/mk off <1>");
                    Thread.Sleep(100);
                    postNamazuHelper.SendCommand("/mk off <2>");
                    Thread.Sleep(100);
                    postNamazuHelper.SendCommand("/mk off <3>");
                    Thread.Sleep(100);
                    postNamazuHelper.SendCommand("/mk off <4>");
                    Thread.Sleep(100);
                    postNamazuHelper.SendCommand("/mk off <5>");
                    Thread.Sleep(100);
                    postNamazuHelper.SendCommand("/mk off <6>");
                    Thread.Sleep(100);
                    postNamazuHelper.SendCommand("/mk off <7>");
                    Thread.Sleep(100);
                    postNamazuHelper.SendCommand("/mk off <8>");
                    clear = false;
                }).Start();
                clear = true;
            }
            
        }

        public void PrintP2Step3AttackGroup()
        {
            for(int i = 0;i<=p2Step3AttackGroup.Length - 1; i++)
            {
                for (int y = 0; y <= p2Step3AttackGroup.Length - 1; y++)
                {
                    Log.Print("Group:" + i + "player:" + p2Step3AttackGroup[i][y]);
                }
            }
        }

        public Stack<string> BuildMarkStack()
        {
            Stack<string> res = new Stack<string>();
            res.Push("/mk bind2 <{0}>");
            res.Push("/mk bind1 <{0}>");
            res.Push("/mk stop2 <{0}>");
            res.Push("/mk stop1 <{0}>");
            res.Push("/mk attack4 <{0}>");
            res.Push("/mk attack3 <{0}>");
            res.Push("/mk attack2 <{0}>");
            res.Push("/mk attack1 <{0}>");

            return res;
        }

        public void ResetCombat()
        {
            clear = false;
            p2Step2AttackPlayer = new string[2];
            p2Step1AttackPlayer = new List<string>();
            p2Step3AttackPlayer = new List<string>();
            p2Step4OutTower = new P2Step4OutTower();
            p2Step4TowerCount = 0;
            p2Step3AttackGroupType = "";
            p2Step3AttackGroup = new string[2][];
            p3Step1MarkMahjong1Player = new List<string>(3);
            p3Step1MarkMahjong2Player = new List<string>(2);
            p3Step1MarkMahjong3Player = new List<string>(3);
            p3Step1TowerPos = new Dictionary<string, int>();
            p4step1MarkPlayer = new List<string>(8);
            p4step1ChangePlayer = new List<string>();
            p4step2FirstAttackPlayer = new string[2];
            p3Step2TowerPos = new int[4];
            p3Step2TowerCount = 0;
            p3Step2EndPos = new int[8] { 1,2,3,4,1,2,3,4 };
            nidhoggNotBossId = new Dictionary<string, int>();
            isP3Step2Endtoolpid = false;
            markOffset = -1;
            Log.Print("战斗结束");
        }
    }
}
