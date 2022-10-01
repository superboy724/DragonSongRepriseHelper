using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DragonSongRepriseHelper
{
    public class LogReader : IDisposable
    {
        bool isInit;
        List<LogEvent> eventList;

        public LogReader()
        {
            isInit = false;
            eventList = new List<LogEvent>();
        }

        public void Init()
        {
            ActGlobals.oFormActMain.OnLogLineRead += OFormActMain_OnLogLineRead;
            
        }

        public void RegisterEvent(int eventCode,string regexp,Action<string> callBack)
        {
            eventList.Add(new LogEvent() {
                CallBack = callBack,
                EventCode = eventCode,
                EventRegexp = regexp
            });
        }

        private void OFormActMain_OnLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {
            try
            {
                if(logInfo.detectedType == 27)
                {
                    Log.Print(logInfo.logLine);
                }
                foreach (var item in eventList)
                {
                    
                    if (logInfo.detectedType == item.EventCode && !string.IsNullOrEmpty(Regex.Match(logInfo.logLine, item.EventRegexp).Value))
                    {
                        Log.Print("命中日志行:" + logInfo.logLine);
                        item.CallBack(logInfo.logLine);
                    }

                }
            }
            catch(Exception ex)
            {
                Log.Print(ex.ToString());
            }
        }

        public void Test()
        {
            var c = new LogLineEventArgs("[23:36:28.429] TargetIcon 1B:1004B2C7:重仓空进去:0000:0000:018A:0000:0000:0000", 27, new DateTime(), null, false);
            var a = new LogLineEventArgs("[23:36:28.429] TargetIcon 1B:1004B2C7:重仓空进去:0000:0000:0072:0000:0000:0000", 27,new DateTime(),null,false);
            var b = new LogLineEventArgs("[23:36:28.429] TargetIcon 1B:1004BEAF:巧克力豆:0000:0000:0073:0000:0000:0000", 27, new DateTime(), null, false);

            var t1 = new LogLineEventArgs("[23:36:28.429] StartsCasting 14:4000D877:圣骑士埃尔姆诺斯特:737C:信仰:E0000000::11.700:91.00:84.41:0000:0000", 20, new DateTime(), null, false);
            var t2 = new LogLineEventArgs("[23:36:28.429] StartsCasting 14:4000D877:圣骑士埃尔姆诺斯特:737C:信仰:E0000000::11.700:91.00:115.59:0000:0000", 20, new DateTime(), null, false);
            var t3 = new LogLineEventArgs("[23:36:28.429] StartsCasting 14:4000D877:圣骑士埃尔姆诺斯特:737C:信仰:E0000000::11.700:115.59:91.00:0000:0000", 20, new DateTime(), null, false);
            var t4 = new LogLineEventArgs("[23:36:28.429] StartsCasting 14:4000D877:圣骑士埃尔姆诺斯特:737C:信仰:E0000000::11.700:118.00:100.00:0000:0000", 20, new DateTime(), null, false);
            var t5 = new LogLineEventArgs("[23:36:28.429] StartsCasting 14:4000D877:圣骑士埃尔姆诺斯特:737C:信仰:E0000000::11.700:1:2:0000:0000", 20, new DateTime(), null, false);
            var t6 = new LogLineEventArgs("[23:36:28.429] StartsCasting 14:4000D877:圣骑士埃尔姆诺斯特:737C:信仰:E0000000::11.700:1:2:0000:0000", 20, new DateTime(), null, false);
            var t7 = new LogLineEventArgs("[23:36:28.429] StartsCasting 14:4000D877:圣骑士埃尔姆诺斯特:737C:信仰:E0000000::11.700:1:2:0000:0000", 20, new DateTime(), null, false);
            var t8 = new LogLineEventArgs("[23:36:28.429] StartsCasting 14:4000D877:圣骑士埃尔姆诺斯特:737C:信仰:E0000000::11.700:1:2:0000:0000", 20, new DateTime(), null, false);

            OFormActMain_OnLogLineRead(false, c);
            OFormActMain_OnLogLineRead(false, c);
            OFormActMain_OnLogLineRead(false, c);
            OFormActMain_OnLogLineRead(false, a);
            OFormActMain_OnLogLineRead(false, b);
            OFormActMain_OnLogLineRead(false, t1);
            OFormActMain_OnLogLineRead(false, t2);
            OFormActMain_OnLogLineRead(false, t3);
            OFormActMain_OnLogLineRead(false, t4);
            OFormActMain_OnLogLineRead(false, t5);
            OFormActMain_OnLogLineRead(false, t6);
            OFormActMain_OnLogLineRead(false, t7);
            OFormActMain_OnLogLineRead(false, t8);
        }

        public void PlayLog(string path)
        {
            var text = File.ReadAllLines(path);
            foreach(var logline in text)
            {
                try
                {
                    string logSubString = logline.Substring(logline.IndexOf("]"));
                    string split = logSubString.Split(':')[0];
                    LogLineEventArgs logInfo = new LogLineEventArgs(logline, Convert.ToInt32(split.Substring(split.Length - 2, 2),16), DateTime.Now,"",true);
                    if (logInfo.detectedType == 27)
                    {
                        Log.Print(logInfo.logLine);
                    }
                    foreach (var item in eventList)
                    {

                        if (logInfo.detectedType == item.EventCode && !string.IsNullOrEmpty(Regex.Match(logInfo.logLine, item.EventRegexp).Value))
                        {
                            Log.Print("命中日志行:" + logInfo.logLine);
                            item.CallBack(logInfo.logLine);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Log.Print(ex.ToString());
                }
            }
        }

        public void Dispose()
        {
            ActGlobals.oFormActMain.OnLogLineRead -= OFormActMain_OnLogLineRead;
        }
    }
}
