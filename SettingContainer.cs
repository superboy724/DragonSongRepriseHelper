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
        Dictionary<string, string> Settings { get; set; }
        Action cbk;

        public SettingContainer()
        {
            this.Settings = new Dictionary<string, string>();
        }

        public void LoadSetting(string path)
        {
            if (!File.Exists(path))
            {
                Settings = new Dictionary<string, string>();
            }
            else
            {
                var data = File.ReadAllLines(path);
                foreach(var item in data)
                {
                    Settings.Add(item.Split('=')[0], (item.Split('=')[1]).Replace("<<line>>","\r\n"));
                }
            }

            cbk();
        }

        public void SaveSetting(string path)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            foreach(var item in Settings)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    continue;
                }
                sw.WriteLine(item.Key + "=" + item.Value.Replace("\r\n", "<<line>>"));
            }
            sw.Close();
            fs.Close();
        }

        public void UpdateKey(string key,string value)
        {
            Settings[key] = value;
            cbk();
        }

        public void OnSettingUpdate(Action callback)
        {
            cbk = callback;
        }

        public string Get(string key)
        {
            if (this.Settings.ContainsKey(key))
            {
                return this.Settings[key];
            }
            else
            {
                return "";
            }
            
        }
    }
}
