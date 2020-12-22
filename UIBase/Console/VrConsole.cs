using System;
using System.Collections.Generic;
using WengaPort.Api;
using WengaPort.Buttons;
using UnityEngine;
using UnityEngine.UI;
using WengaPort.Extensions;
using System.Linq;

namespace WengaPort.ConsoleUtils
{
    class VRConsole
    {
        public static List<string> AllLogsText = new List<string>();

        public enum LogsType
        {
            Info,
            Protection,
            Voice,
            Avatar,
            Block,
            Join,
            Left,
            Warn,
            Kick,
            Votekick,
            Logout,
            Ban,
            Friend,
            Portal,
            World,
            Clear
        }
        public static void Log(LogsType Type, string Text)
        {
            string Text2 = "";
            switch (Type)
            {
                case LogsType.Info:
                    Text2 = "<color=#ee00ff>[Info]:</color>  " + Text;
                    break;
                case LogsType.Voice:
                    Text2 = "<color=#00ffea>[Voice]:</color>  " + Text;
                    break;
                case LogsType.Avatar:
                    Text2 = "<color=#4800ff>[Avatar]:</color>  " + Text;
                    break;
                case LogsType.Block:
                    Text2 = "<color=#ad0000>[Block]:</color>  " + Text;
                    break;
                case LogsType.Join:
                    Text2 = "<color=#00ff04>[+]:</color>  " + Text;
                    break;
                case LogsType.Left:
                    Text2 = "<color=#ff1a1a>[-]:</color>  " + Text;
                    break;
                case LogsType.Portal:
                    Text2 = "<color=#030b82>[Portal]:</color>  " + Text;
                    break;
                case LogsType.Warn:
                    Text2 = "<color=#ffd900>[Warn]:</color>  " + Text;
                    break;
                case LogsType.Kick:
                    Text2 = "<color=#ff0000>[Kick]:</color>  " + Text;
                    break;
                case LogsType.Votekick:
                    Text2 = "<color=#ff0000>[Votekick]:</color>  " + Text;
                    break;
                case LogsType.Logout:
                    Text2 = "<color=#4800ff>[Logout]:</color>  " + Text;
                    break;
                case LogsType.Ban:
                    Text2 = "<color=#c20000>[Ban]:</color>  " + Text;
                    break;
                case LogsType.Friend:
                    Text2 = "<color=#ccff00>[Friend]:</color>  " + Text;
                    break;
                case LogsType.Clear:
                    Text2 = "";
                    AllLogsText.Clear();
                    ConsoleMenu.QMInfo.SetText("");
                    break;
                case LogsType.World:
                    Text2 = "<color=#db5400>[World]:</color>  " + Text;
                    break;
                case LogsType.Protection:
                    Text2 = "<color=#eb0000>[Protection]:</color>  " + Text;
                    break;

            }
            AllLogsText.Insert(0, Text2);
            ConsoleMenu.QMInfo.SetText(string.Join("\n", AllLogsText.Take(20)));
        }
    }
}
