﻿using Advanced_Combat_Tracker;
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
        bool clear = false;
        int markOffset = -1;
        const int skywardTripleMark = 0x14A;
        const int sword1Mark = 0x32;
        const int sword2Mark = 0x33;
        const int meteor = 0x11D;

        string[] p2Step2AttackPlayer = new string[2];
        List<string> p2Step3AttackPlayer = new List<string>();
        List<string> p2Step1AttackPlayer = new List<string>();
        P2Step4OutTower p2Step4OutTower = new P2Step4OutTower();
        int p2Step4TowerCount = 0;
        string p2Step3AttackGroupType = "";
        string[][] p2Step3AttackGroup = new string[2][];

        public void DeInitPlugin()
        {
            if(logreader != null)
            {
                logreader.Dispose();
            }
            settingContainer.SaveSetting(Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "DragonSongRepriseHelper.config"));
            ActGlobals.oFormActMain.OnCombatEnd -= OFormActMain_OnCombatEnd;
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            pluginScreenSpace.Text = "绝龙诗小助手";

            settingContainer = new SettingContainer();
            RegisterSettingForm(pluginScreenSpace, pluginStatusText);
            ActGlobals.oFormActMain.OnCombatEnd += OFormActMain_OnCombatEnd;
        }

        private void OFormActMain_OnCombatEnd(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            //初始化所有变量
            clear = false;
            p2Step2AttackPlayer = new string[2];
            p2Step1AttackPlayer = new List<string>();
            p2Step3AttackPlayer = new List<string>();
            p2Step4OutTower = new P2Step4OutTower();
            p2Step4TowerCount = 0;
            p2Step3AttackGroupType = "";
            markOffset = -1;
            Log.Print("战斗结束");
        }

        public void RegisterSettingForm(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            SettingForm settingForm = null;
            settingContainer.OnSettingUpdate(() =>
            {
                string postNamazuUrl = settingContainer.Get("post_namazu_url");
                string playersStr = settingContainer.Get("players");
                var p2FirstStepPlayers = playersStr.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                
                if (logreader != null)
                {
                    logreader.Dispose();
                }
                logreader = new LogReader();
                postNamazuHelper = new PostNamazuHelper(postNamazuUrl);

                //注册点名事件
                logreader.RegisterEvent(27, "^(.+?)TargetIcon(\\s|\\S)+$", (log) =>
                {
                    try
                    {
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
                            P2Step1Process(log, p2FirstStepPlayers);
                        }
                        //点分摊
                        if (trueCode == sword1Mark || trueCode == sword2Mark)
                        {
                            P2Step2Process(log, p2FirstStepPlayers);
                        }
                        //点陨石
                        if (trueCode == meteor)
                        {
                            P2Step3Process(log, p2FirstStepPlayers);
                        }
                    } catch (Exception ex)
                    {
                        Log.Print(ex.ToString());
                    }
                });
                //注册放塔事件
                logreader.RegisterEvent(20, "^(.+?)StartsCasting(.+?)圣骑士埃尔姆诺斯特\\:737C\\:信仰(\\s|\\S)+$", (log) =>
                {
                    try
                    {
                        P2Step4Process(log, p2FirstStepPlayers);
                    }
                    catch(Exception ex)
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
                    if(macroType == 1)
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
                    if(macroType == 2)
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
                    if(playerSettingStrs.Count > 0)
                    {
                        settingForm.SetPlayers(playerSettingStrs);
                    }
                });

                logreader.Init();
            });
            settingContainer.LoadSetting(Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "DragonSongRepriseHelper.config"));

            settingForm = new SettingForm(settingContainer,Test, logreader.Test);
            pluginScreenSpace.Controls.Add(settingForm);
            settingForm.Dock = DockStyle.Fill;

            pluginStatusText.Text = "Plugin Started";
        }

        public void Test()
        {
            string playersStr = settingContainer.Get("players");
            var players = playersStr.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            postNamazuHelper.SendCommand("/mk attack <" + 1 + ">");
            for (int i = 1; i <= 2; i++)
            {
                Random r = new Random();
                postNamazuHelper.SendCommand("/mk attack <" + r.Next(1, players.Length) + ">");
            }
            Clear();
        }

        //一运
        public void P2Step1Process(string log,string[] playerSetting)
        {
            string logSubString = log.Substring(log.IndexOf("]"));
            string playerId = logSubString.Split(':')[2];
            Log.Print("点名" + playerId);
            //int i = Utils.GetPlayerIndex(playerSetting, playerId);
            if (settingContainer.Get("p2step1enable") == "true")
            {
                p2Step1AttackPlayer.Add(playerId);
                if (p2Step1AttackPlayer.Count == 3)
                {
                    foreach (var item in playerSetting)
                    {
                        if (!p2Step1AttackPlayer.Contains(item.Split(',')[0]))
                        {
                            if (item.Contains("MT") || item.Contains("ST"))
                            {
                                continue;
                            }
                            int i = Utils.GetPlayerIndex(playerSetting, item.Split(',')[0]);
                            postNamazuHelper.SendCommand("/mk attack <" + i + ">");
                            Log.Print("无点名" + playerId + ",顺位:" + i);
                        }
                    }
                }
                Clear();
            }
        }

        //二运冲锋
        public void P2Step2Process(string log,string[] playerSetting)
        {
            string logSubString = log.Substring(log.IndexOf("]"));
            string markCodeStr = logSubString.Split(':')[5];
            int markCode = Convert.ToInt32(markCodeStr, 16);
            int trueCode = markCode - markOffset;

            string playerId = logSubString.Split(':')[2];
            
            Log.Print("二运点名" + playerId);
            for (int i = 1; i <= playerSetting.Length; i++)
            {
                if (playerId == playerSetting[i - 1].Split(',')[0])
                {
                    if (settingContainer.Get("p2step2enable") == "true")
                    {
                        Log.Print("判断开始");
                        if (trueCode == sword1Mark)
                        {
                            p2Step2AttackPlayer[0] = playerSetting[i - 1];
                        }
                        if (trueCode == sword2Mark)
                        {
                            p2Step2AttackPlayer[1] = playerSetting[i - 1];
                        }
                        if (p2Step2AttackPlayer[0] != null && p2Step2AttackPlayer[1] != null)
                        {
                            string[] mtgroup = new string[4];
                            string[] stgroup = new string[4];
                            foreach (var item in playerSetting)
                            {
                                switch (item.Split(',')[1])
                                {
                                    case "MT": mtgroup[0] = item; break;
                                    case "H1": mtgroup[1] = item; break;
                                    case "D1": mtgroup[2] = item; break;
                                    case "D3": mtgroup[3] = item; break;
                                    case "ST": stgroup[0] = item; break;
                                    case "H2": stgroup[1] = item; break;
                                    case "D2": stgroup[2] = item; break;
                                    case "D4": stgroup[3] = item; break;
                                }
                            }
                            Log.Print("mtgroup.Contains(p2Step2AttackPlayer[0]) && mtgroup.Contains(p2Step2AttackPlayer[1]) == " + (mtgroup.Contains(p2Step2AttackPlayer[0]) && mtgroup.Contains(p2Step2AttackPlayer[1]) == true ? "true" : "false"));
                            Log.Print("stgroup.Contains(p2Step2AttackPlayer[0]) && stgroup.Contains(p2Step2AttackPlayer[1]) == " + (stgroup.Contains(p2Step2AttackPlayer[0]) && stgroup.Contains(p2Step2AttackPlayer[1]) == true ? "true" : "false"));
                            if (mtgroup.Contains(p2Step2AttackPlayer[0]) && mtgroup.Contains(p2Step2AttackPlayer[1]))
                            {
                                if (settingContainer.Get("p2step2enable") == "true")
                                {
                                    if(settingContainer.Get("p2step2markdisabled") == "true")
                                    {
                                        postNamazuHelper.SendCommand("/y 【冲锋换位提醒】D2 " + stgroup[2].Split(',')[0] + " 进行换位");
                                        postNamazuHelper.SendCommand("/p 【冲锋换位提醒】D2 " + stgroup[2].Split(',')[0] + " 进行换位<se.8>");
                                    }
                                    else
                                    {
                                        postNamazuHelper.SendCommand("/mk circle <" + Utils.GetPlayerIndex(playerSetting, stgroup[2].Split(',')[0]) + ">");
                                    }
                                    Clear();
                                }

                            }
                            if (stgroup.Contains(p2Step2AttackPlayer[0]) && stgroup.Contains(p2Step2AttackPlayer[1]))
                            {
                                if (settingContainer.Get("p2step2enable") == "true")
                                {
                                    if (settingContainer.Get("p2step2markdisabled") == "true")
                                    {
                                        postNamazuHelper.SendCommand("/y 【冲锋换位提醒】D1 " + mtgroup[2].Split(',')[0] + " 进行换位");
                                        postNamazuHelper.SendCommand("/p 【冲锋换位提醒】D1 " + mtgroup[2].Split(',')[0] + " 进行换位<se.8>");
                                    }
                                    else
                                    {
                                        postNamazuHelper.SendCommand("/mk circle <" + Utils.GetPlayerIndex(playerSetting, mtgroup[2].Split(',')[0]) + ">");
                                    }
                                    Clear();
                                }

                            }
                        }
                    }
                    Log.Print("二运点名" + playerId + ",顺位:" + i);
                    break;
                }
            };
        }

        //二运陨石预站位
        public void P2Step3Process(string log, string[] playerSetting)
        {
            string logSubString = log.Substring(log.IndexOf("]"));
            string playerId = logSubString.Split(':')[2];
            p2Step3AttackPlayer.Add(playerId);
            Log.Print("陨石点名" + playerId);
            if (p2Step3AttackPlayer.Count == 2)
            {
                if (settingContainer.Get("p2step3enable") == null || settingContainer.Get("p2step3enable") == "false")
                {
                    return;
                }
                string[] group1 = new string[2];//A点组
                string[] group2 = new string[2];//C点组
                string[] group3 = new string[2];//D点组
                string[] group4 = new string[2];//B点组
                foreach (var item in playerSetting)
                {
                    switch (item.Split(',')[1])
                    {
                        case "MT": group1[0] = item.Split(',')[0]; break;
                        case "D1": group1[1] = item.Split(',')[0]; break;
                        case "ST": group2[0] = item.Split(',')[0]; break;
                        case "D2": group2[1] = item.Split(',')[0]; break;
                        case "H1": group3[0] = item.Split(',')[0]; break;
                        case "D3": group3[1] = item.Split(',')[0]; break;
                        case "H2": group4[0] = item.Split(',')[0]; break;
                        case "D4": group4[1] = item.Split(',')[0]; break;
                    }
                }

                //AC点名 不处理
                if((group1.Contains(p2Step3AttackPlayer[0]) && group2.Contains(p2Step3AttackPlayer[1])) || (group2.Contains(p2Step3AttackPlayer[0]) && group1.Contains(p2Step3AttackPlayer[1])))
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
        public void P2Step4Process(string log, string[] playerSetting)
        {
            if (settingContainer.Get("p2step3enable") == null || settingContainer.Get("p2step3enable") == "false")
            {
                return;
            }
            if (settingContainer.Get("p2step4enable") == null || settingContainer.Get("p2step4enable") == "false")
            {
                return;
            }
            //陨石点名了 进行处理
            if(p2Step3AttackPlayer.Count == 2)
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
                            if (settingContainer.Get("p2step4ChangeTowerEnable") == "true" && p2Step4OutTower.tAOutRight)
                            {
                                string player1 = p2Step3AttackGroup[0][0];
                                string player2 = p2Step3AttackGroup[0][1];
                                postNamazuHelper.SendCommand("/p 【陨石踩塔换位提醒】A组" + player1 + "可与" + player2 + "进行换位");
                                postNamazuHelper.SendCommand("/mk attack <" + Utils.GetPlayerIndex(playerSetting, player1) + ">");
                                postNamazuHelper.SendCommand("/mk attack <" + Utils.GetPlayerIndex(playerSetting, player2) + ">");
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
                                postNamazuHelper.SendCommand("/mk attack <" + Utils.GetPlayerIndex(playerSetting, player1) + ">");
                                postNamazuHelper.SendCommand("/mk attack <" + Utils.GetPlayerIndex(playerSetting, player2) + ">");
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
                                postNamazuHelper.SendCommand("/mk attack <" + Utils.GetPlayerIndex(playerSetting, player1) + ">");
                                postNamazuHelper.SendCommand("/mk attack <" + Utils.GetPlayerIndex(playerSetting, player2) + ">");
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
                                postNamazuHelper.SendCommand("/mk attack <" + Utils.GetPlayerIndex(playerSetting, player1) + ">");
                                postNamazuHelper.SendCommand("/mk attack <" + Utils.GetPlayerIndex(playerSetting, player2) + ">");
                                Clear();
                            }
                        }
                    }
                }
            }
            
        }

        public void Clear()
        {
            if (!clear)
            {
                new Thread(() =>
                {
                    Thread.Sleep(5000);
                    string postNamazuUrl = settingContainer.Get("post_namazu_url");
                    postNamazuHelper.SendCommand("/mk off <1>");
                    Thread.Sleep(500);
                    postNamazuHelper.SendCommand("/mk off <2>");
                    Thread.Sleep(500);
                    postNamazuHelper.SendCommand("/mk off <3>");
                    Thread.Sleep(500);
                    postNamazuHelper.SendCommand("/mk off <4>");
                    Thread.Sleep(500);
                    postNamazuHelper.SendCommand("/mk off <5>");
                    Thread.Sleep(500);
                    postNamazuHelper.SendCommand("/mk off <6>");
                    Thread.Sleep(500);
                    postNamazuHelper.SendCommand("/mk off <7>");
                    Thread.Sleep(500);
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
    }
}