using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonSongRepriseHelper
{
    public class Utils
    {
        public static int GetPlayerIndex(string[] playerList,string playerId)
        {
            for (int i = 1; i <= playerList.Length; i++)
            {
                if(playerId == playerList[i - 1].Split(',')[0])
                {
                    return i;
                }
            }

            return 0;
        }
    }
}
