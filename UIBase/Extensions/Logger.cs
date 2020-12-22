using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using WengaPort.ConsoleUtils;

namespace WengaPort.Extensions
{
    class Logger
    {
        public static void WengaLogger(object obj) 
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("[" + DateTime.Now.ToShortTimeString() + "]" + " [WengaPort] " + obj);
        }

        public static void WebsocketLogger(VRConsole.LogsType LogType, object obj)
        {
            try
            {
                File.WriteAllText("WengaPort\\Websocket.txt", obj.ToString());
                VRConsole.Log(LogType, File.ReadAllText("WengaPort\\Websocket.txt"));
            }
            catch
            { }
        }
    }
}
