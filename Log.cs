using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DragonSongRepriseHelper
{
    public class Log
    {
        public delegate void UpdateTxt(string msg);
        public static TextBox bindTb;

        public static void Print(string str)
        {
            if(bindTb != null)
            {
                bindTb.BeginInvoke(new UpdateTxt((t)=> {
                    bindTb.AppendText(t + "\r\n");
                }), str);
            }
        }
    }
}
