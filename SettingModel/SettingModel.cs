using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonSongRepriseHelper.SettingModel
{
    public interface SettingModel
    {
        string[] GetSettingText();
        void LoadSettingFromText(Dictionary<string, string> configTexts);
    }
}
