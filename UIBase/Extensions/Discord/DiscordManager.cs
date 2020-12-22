using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VRC;
using VRC.Core;
using MelonLoader;
using UnityEngine;
using WengaPort;

namespace DiscordRichPresence
{
    internal static class DiscordManager
    {
        private static DiscordRpc.RichPresence presence;
        private static DiscordRpc.EventHandlers eventHandlers;
        private static bool running = false;
        private static string roomId = "";
        public static void Init()
        {
            eventHandlers = default;
            eventHandlers.errorCallback = delegate (int code, string message)
            {
                WengaPort.Extensions.Logger.WengaLogger(string.Concat(new object[]
                {
                    "[WengaPort Presence] (E",
                    code,
                    ") ",
                    message
                }));
            };
            presence.state = "Loading Screen";
            presence.details = "Not logged in (" + (UnityEngine.XR.XRDevice.isPresent ? "VR" : "PC") + ")";
            presence.largeImageKey = "logo";
            presence.smallImageKey = "small";
            presence.smallImageText = "Get Logout :<";
            presence.partySize = 0;
            presence.partyMax = 0;
            presence.startTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            presence.partyId = "";
            presence.largeImageText = "WengaPort";

            try
            {
                string optionalSteamId = null;
                ApiServerEnvironment serverEnvironment = VRCApplicationSetup.prop_VRCApplicationSetup_0.ServerEnvironment;
                DiscordRpc.Initialize("770056934236880897", ref eventHandlers, true, optionalSteamId);
                DiscordRpc.UpdatePresence(ref presence);
                running = true;
                WengaPort.Extensions.Logger.WengaLogger("[WengaPort Discord] RichPresence Init");
            }
            catch (Exception arg)
            {
                WengaPort.Extensions.Logger.WengaLogger("[WengaPort Discord] Unable to init discord RichPresence:");
                WengaPort.Extensions.Logger.WengaLogger("[WengaPort Discord] " + arg);
            }
        }

        public static string RoomChanged(string worldName, string worldAndRoomId, string roomIdWithTags, ApiWorldInstance.AccessType accessType, int maxPlayers)
        {
            if (!running)
            {
                return null;
            }
            if (!worldAndRoomId.Equals(""))
            {
                presence.state = "[World Hidden]";
                presence.partyId = "";
                presence.startTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            }
            else
            {
                presence.state = "[Loading Screen]";
                presence.partyId = "";
                presence.partyMax = 0;
                presence.startTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                presence.joinSecret = "";
            }
            DiscordRpc.UpdatePresence(ref presence);
            return presence.joinSecret;
        }


        public static void UserChanged(string displayName)
        {
            if (!running)
            {
                return;
            }
            if (!displayName.Equals(""))
            {
            presence.details = "Logged in (" + (UnityEngine.XR.XRDevice.isPresent ? "VR" : "PC") + ")";
            DiscordRpc.UpdatePresence(ref presence);
            return;
            }
            presence.details = "Not logged in (" + (UnityEngine.XR.XRDevice.isPresent ? "VR" : "PC") + ")";
            RoomChanged("", "", "", 0, 0);
        }

        public static void Update()
        {
            if (!running)
            {
                return;
            }
            APIUser currentUser2 = APIUser.CurrentUser;
            UserChanged(((currentUser2 != null) ? currentUser2.displayName : null) ?? "");
            string text = "";
            ApiWorld currentRoom = RoomManager.field_Internal_Static_ApiWorld_0;
            if (((currentRoom != null) ? currentRoom.currentInstanceIdOnly : null) != null)
            {
                text = currentRoom.id + ":" + currentRoom.currentInstanceIdWithTags;
            }
            if (roomId != text)
            {
                roomId = text;
                if (roomId != "")
                {
                    RoomChanged(currentRoom.name, currentRoom.id + ":" + currentRoom.currentInstanceIdOnly, currentRoom.currentInstanceIdWithTags, currentRoom.currentInstanceAccess, currentRoom.capacity);
                }
                else
                {
                    RoomChanged("", "", "", ApiWorldInstance.AccessType.InviteOnly, 0);
                }
            }
        }


        public static void OnApplicationQuit()
        {
            if (!running)
            {
                return;
            }
            DiscordRpc.Shutdown();
        }


        public static string GenerateRandomString(int length)
        {
            string text = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] array = new char[length];
            System.Random random = new System.Random();
            for (int i = 0; i < length; i++)
            {
                array[i] = text[random.Next(text.Length)];
            }
            return new string(array);
        }
    }
}