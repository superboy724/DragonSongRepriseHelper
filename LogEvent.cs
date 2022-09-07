using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonSongRepriseHelper
{
    public class LogEvent
    {
        public int EventCode { get; set; }

        public string EventRegexp { get; set; }

        public Action<string> CallBack { get; set; }
    }
}
