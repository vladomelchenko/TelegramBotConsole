using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleJSON;
namespace TelegramBotConsole
{
    class Program
    {
        private const string token = "494688910:AAEJ3j2aFJPNjWhVm-Ts_rF4vkgCFSb4SqY";
        public static int LastUpdateId = 0;
        private static int LastUpdate { get; set; } = 0;

        static void Main(string[] args)
        {
            while (true)
            {
                GetUpdates();
                Thread.Sleep(1000);
            }
        }

        static void GetUpdates()
        {
            using (var client = new WebClient())
            {
                string response =
                    client.DownloadString("https://api.telegram.org/bot" + token + "/getUpdates" + "?offset" +
                                          LastUpdateId + 1);

                var N = JSON.Parse(response);
                var resultArray = N["result"].AsArray;
                int Length = resultArray.Count - 1;
                var chatId = 0;
                foreach (JSONNode n in N["result"].AsArray)
                {
                    LastUpdateId = n["update_id"].AsInt;
                    //SendMessage("asd",n["message"]["chat"]["id"].AsInt);
                    chatId = n["message"]["chat"]["id"].AsInt;
                }
                if (LastUpdateId != LastUpdate)
                {
                    SendMessage(N["result"].AsArray[Length]["message"]["text"], chatId);
                    LastUpdate = N["result"].AsArray[Length]["update_id"].AsInt;
                }
            }
        }

        static void SendMessage(string message, int chatId)
        {
            using (var client =new WebClient())
            {
                var pars = new NameValueCollection();
                pars.Add("text",message);
                pars.Add("chat_id",chatId.ToString());
                client.UploadValues("https://api.telegram.org/bot" + token + "/sendMessage",pars);
            }
        }
    }
}
