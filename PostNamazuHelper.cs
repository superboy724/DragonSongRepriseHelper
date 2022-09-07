using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DragonSongRepriseHelper
{
    public class PostNamazuHelper
    {
        public string Url { get; set; }

        public PostNamazuHelper(string url)
        {
            this.Url = url;
        }

        public void SendCommand(string command)
        {
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 50;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.Timeout = 100;
                var sendStream = request.GetRequestStream();
                var sw = new StreamWriter(sendStream);
                sw.Write(command);
                sw.Flush();
                sw.Close();
                sendStream.Close();

                WebResponse myWebResponse = request.GetResponse();
                myWebResponse.Close();
            }
            catch { }

        }
    }
}
