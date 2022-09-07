using DragonSongRepriseHelper.SettingModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonSongRepriseHelper
{
    public class SettingContainer
    {
        public FunctionSetting FunctionSetting { get; set; }
        public PlayerSetting PlayerSetting { get; set; }

        public SettingContainer()
        {
            FunctionSetting = new FunctionSetting();
            PlayerSetting = new PlayerSetting();
        }

        public void LoadSetting(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }
            else
            {
                var data = File.ReadAllLines(path);
                Dictionary<string, string> settingsStr = new Dictionary<string, string>();
                foreach(var item in data)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        settingsStr.Add(item.Split('=')[0], item.Split('=')[1]);
                    }
                }

                FunctionSetting.LoadSettingFromText(settingsStr);
                PlayerSetting.LoadSettingFromText(settingsStr);
            }
        }

        public void SaveSetting(string path)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);

            var functionSettingStr = FunctionSetting.GetSettingText();
            var playerSettingStr = PlayerSetting.GetSettingText();

            foreach(var item in functionSettingStr)
            {
                sw.WriteLine(item);
            }

            foreach (var item in playerSettingStr)
            {
                sw.WriteLine(item);
            }

            sw.Flush();
            sw.Close();
            fs.Flush();
            fs.Close();
        }

    }
}
