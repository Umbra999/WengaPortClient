using Il2CppSystem;
using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WengaPort.Api.Object;
using WengaPort.Extensions;

namespace WengaPort.FoldersManager
{
    class Create
    {
        public static IniFile Ini;
        private static long GetDirectorySize(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            return di.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);
        }

        public static void Initialize()
        {
            var pathWithEnv = @"%USERPROFILE%\AppData\LocalLow\VRChat\VRChat\Cache-WindowsPlayer";
            var filePath = System.Environment.ExpandEnvironmentVariables(pathWithEnv);
            if (!Directory.Exists("WengaPort"))
            {
                Directory.CreateDirectory("WengaPort");
            }
            if (!File.Exists("WengaPort\\Config.ini"))
            {
                File.Create("WengaPort\\Config.ini");
                Ini = new IniFile("WengaPort\\Config.ini");
            }
            else
            {
                Ini = new IniFile("WengaPort\\Config.ini");
            }
            if (!File.Exists("WengaPort\\WengaPort.nmasset"))
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile("https://cdn.discordapp.com/attachments/777934995905708063/794314592930496542/WengaPort.nmasset", Path.Combine(System.Environment.CurrentDirectory, "WengaPort/WengaPort.nmasset"));
                }
            }
            if (!Directory.Exists(Path.Combine(System.Environment.CurrentDirectory, "Dependencies")))
                Directory.CreateDirectory(Path.Combine(System.Environment.CurrentDirectory, "Dependencies"));
            if (!File.Exists(Path.Combine(System.Environment.CurrentDirectory, "Dependencies/discord-rpc.dll")))
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile("http://thetrueyoshifan.com/downloads/discord-rpc.dll", Path.Combine(System.Environment.CurrentDirectory, "Dependencies/discord-rpc.dll"));
                }
            }
            if (!File.Exists(Path.Combine(System.Environment.CurrentDirectory, "websocket-sharp.dll")))
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile("https://cdn.discordapp.com/attachments/777934995905708063/781565586424987688/websocket-sharp.dll", Path.Combine(System.Environment.CurrentDirectory, "websocket-sharp.dll"));
                }
            }
            if (!Directory.Exists("WengaPort\\VRCA"))
            {
                Directory.CreateDirectory("WengaPort\\VRCA");
            }
            if (!File.Exists("WengaPort\\Websocket.txt"))
            {
                File.Create("WengaPort\\Websocket.txt");
            }
            if (!Directory.Exists("WengaPort\\Photon"))
            {
                Directory.CreateDirectory("WengaPort\\Photon");
            }
            if (Directory.Exists(filePath))
            {
                if (GetDirectorySize(filePath) >= 17947937170)
                {
                    DirectoryInfo fi = new DirectoryInfo(filePath);
                    foreach (FileInfo file in fi.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in fi.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    Ini.SetBool("Toggles", "QuestSpoof", false);
                    PatchManager.QuestSpoof = false;
                    Logger.WengaLogger("[Cache] Cleaned...");
                }
            }
            if (!File.Exists("WengaPort\\AvatarFavorites.json"))
            {
                string contents = JsonConvert.SerializeObject(new List<AvatarObject>(), Formatting.Indented);
                File.WriteAllText("WengaPort\\AvatarFavorites.json", contents);
            }
        }
    }
}
