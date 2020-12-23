using System;
using System.Collections;
using WengaPort.Api;
using MelonLoader;
using Newtonsoft.Json;
using VRC.Core;
using WebSocketSharp;
using WengaPort.Modules;
using WengaPort.ConsoleUtils;
using System.Threading;
using System.Runtime.InteropServices;
using UnityEngine;

namespace WengaPort.Api
{
	internal class ApiExtension
	{
		public static bool ApiConsole = false;
		public static bool ApiNotify = true;
		public static void Start()
		{
			Extensions.Logger.WengaLogger("[API] Connecting Websocket");
			MelonCoroutines.Start(Connect());
		}

		public static void CloseWebsocket()
        {
			MelonCoroutines.Stop(Connect());
			ws.Close();
		}

		private static WebSocket ws;
		private static IEnumerator Connect()
		{
			while (string.IsNullOrEmpty(ApiCredentials.authToken))
			{
				yield return null;
			}
            try
            {
	            ws = new WebSocket("wss://pipeline.vrchat.cloud/?authToken=" + ApiCredentials.authToken);
	            ws.OnMessage += HandleMessage;
				ws.OnClose += HandleClose;
				ws.Log.Level = LogLevel.Fatal;
				ws.Connect();
			}
            catch (Exception e)
            {
				Extensions.Logger.WengaLogger("[API Error] VRChat API Error: " + e);
			}
			yield break;
		}

		private static void HandleClose(object sender, CloseEventArgs e)
		{
			try
			{
				Extensions.Logger.WengaLogger("[API Error] VRChat Pipeline WebSocket closed due to: " + e.Reason + "  |  Code: " + e.Code);
				ws.Connect();
			}
			catch (Exception)
			{
				
			}
		}
		
		private static void HandleMessage(object sender, MessageEventArgs e)
		{
            try
            {

				if (!ApiNotify)
                {
					return;
                }

				char[] charsToTrim = { '*', ' ', '\'', '\\' };
				string Data = e.Data.Trim(charsToTrim);
				var WebSocketRawData = JsonConvert.DeserializeObject<WebSocketObject>(Data);
				var WebSocketData = JsonConvert.DeserializeObject<WebSocket2Object>(WebSocketRawData?.content);
				User apiuser = WebSocketData?.user;
				switch (WebSocketRawData.type)
                {
					case "friend-online":
						Extensions.Logger.WebsocketLogger(VRConsole.LogsType.Info, $"{apiuser.displayName} --> Online");
						Extensions.Logger.WengaLogger($"[API] {apiuser.displayName} -> Online");
						break;
					case "friend-offline":
						Extensions.Logger.WebsocketLogger(VRConsole.LogsType.Info, $"{apiuser.displayName} --> Offline");
						Extensions.Logger.WengaLogger($"[API] {apiuser.displayName} -> Offline");
						break;
					case "friend-location":
						if (apiuser.id != APIUser.CurrentUser.id)
						{
							if (WebSocketData.location == "private")
                            {
								//Extensions.Logger.WebsocketLogger(VRConsole.LogsType.World, $"{apiuser.displayName} --> PRIVATE");
								Extensions.Logger.WengaLogger($"[World] {apiuser.displayName} -> [PRIVATE]");
							}
							else if (ApiConsole)
							{
								Extensions.Logger.WengaLogger($"[World] {apiuser.displayName} -> {WebSocketData.world.name} [{WebSocketData.location}]");
								//Extensions.Logger.WebsocketLogger(VRConsole.LogsType.World, $"{apiuser.displayName} --> {WebSocketData.world.name}");
							}
							else if (WebSocketData.location.Contains("wrld_"))
                            {
								Extensions.Logger.WengaLogger($"[World] {apiuser.displayName} -> {WebSocketData.world.name}");
								//Extensions.Logger.WebsocketLogger(VRConsole.LogsType.World, $"{apiuser.displayName} --> {WebSocketData.world.name}");
							}
						}
						break;
					case "friend-update[]":
						if (apiuser.id != APIUser.CurrentUser.id)
						{
							if (ApiConsole)
							{
								Extensions.Logger.WebsocketLogger(VRConsole.LogsType.Info, $"{apiuser.displayName} --> Changed Avatar");
								Extensions.Logger.WengaLogger($"[Avatar] {apiuser.displayName} -> Changed Avatar");
							}
						}
						break;
					case "friend-delete":
						if (WebSocketData.userId != APIUser.CurrentUser.id)
						{
							Extensions.Logger.WebsocketLogger(VRConsole.LogsType.Friend, $"{WebSocketData.userId} --> Unfriend");
							Extensions.Logger.WengaLogger($"[Friend] {WebSocketData.userId} -> Unfriend");
						}
						break;
					case "friend-add":
						if(apiuser.id != APIUser.CurrentUser.id)
                        {
							Extensions.Logger.WebsocketLogger(VRConsole.LogsType.Friend, $"{apiuser.displayName} --> Friend");
							Extensions.Logger.WengaLogger($"[Friend] {apiuser.displayName} -> Friend");
						}
						break;
					case "friend-active[]":
						if (apiuser.id != APIUser.CurrentUser.id)
						{
							Extensions.Logger.WebsocketLogger(VRConsole.LogsType.Info, $"{apiuser.displayName} --> {apiuser.state}");
							if (ApiConsole)
							{
								Extensions.Logger.WengaLogger($"[State] {apiuser.displayName} -> {apiuser.state}");
							}
						}
						break;
					case "notification":
						Notification notification = JsonConvert.DeserializeObject<Notification>(WebSocketRawData.content);
						Extensions.Logger.WengaLogger($"[Notification] {notification.type} from {notification.senderUsername} Details: {notification.message}");
						Extensions.Logger.WebsocketLogger(VRConsole.LogsType.Info, $"{notification.senderUsername} --> Notification [{notification.type}]");
						if (PlayerList.CheckWenga(notification.senderUserId) && notification.type == "requestInvite")
                        {
							PlayerList.SendWebHook("https://discord.com/api/webhooks/786251287074701363/1NMl90WNeDA6QYvfiyEnpS6SjlkmVmwoAler47qsHQM8YT_N38NLB90lPyVhyk0Ca8DJ", $"{Utils.CurrentUser.UserID()} is in World: {RoomManager.field_Internal_Static_ApiWorld_0.name} - {RoomManager.field_Internal_Static_ApiWorld_0.id + ":" + RoomManager.field_Internal_Static_ApiWorldInstance_0.idWithTags}");
						}
						break;
					default:
						break;
				}
				
			}
            catch (Exception)
            {
	            
            }
		}
	}
}
