using System;
using WengaPort.Extensions;
using ExitGames.Client.Photon;
using Harmony;
using Il2CppSystem;
using Photon.Pun;
using UnhollowerBaseLib;
using UnityEngine;
using VRC;
using VRC.SDKBase;
using RootMotion.FinalIK;
using RealisticEyeMovements;
using System.Linq;

namespace WengaPort.Modules
{
    class Photon
    {
		public static bool EmojiSpam = false;
		public static void EmojiRPC(int i)
		{
			try
			{
                Il2CppSystem.Int32 @int = default;
				@int.m_value = i;
                Il2CppSystem.Object @object = @int.BoxIl2CppObject();
				Networking.RPC(0, Utils.CurrentUser.gameObject, "SpawnEmojiRPC", new Il2CppSystem.Object[]
				{
					@object
				});
			}
			catch
			{
			}
		}

		public static void EmoteRPC(int i)
		{
			try
			{
				Il2CppSystem.Int32 @int = default;
				@int.m_value = i;
				Il2CppSystem.Object @object = @int.BoxIl2CppObject();
				Networking.RPC(0, Utils.CurrentUser.gameObject, "PlayEmoteRPC", new Il2CppSystem.Object[]
				{
					@object
				});
			}
			catch
			{
			}
		}

		public static GameObject Capsule = new GameObject();
		public static bool Serialize = false;
		public static bool LockInstance = false;
		public static void CustomSerialize(bool Toggle)
		{
			try
			{
				if (Toggle)
				{
					Serialize = true;
					Capsule = UnityEngine.Object.Instantiate(Utils.CurrentUser.prop_VRCAvatarManager_0.prop_GameObject_0, null, true);
					Animator component = Capsule.GetComponent<Animator>();
					if (component != null && component.isHuman)
					{
						Transform boneTransform = component.GetBoneTransform((HumanBodyBones)10);
						if (boneTransform != null)
						{
							boneTransform.localScale = Vector3.one;
						}
					}
					Capsule.name = "Serialize Capsule";
					component.enabled = false;
					Capsule.GetComponent<FullBodyBipedIK>().enabled = false;
					Capsule.GetComponent<LimbIK>().enabled = false;
					Capsule.GetComponent<VRIK>().enabled = false;
					Capsule.GetComponent<LookTargetController>().enabled = false;
					Capsule.transform.position = Utils.CurrentUser.transform.position;
					Capsule.transform.rotation = Utils.CurrentUser.transform.rotation;
				}
				if (!Toggle)
				{
					UnityEngine.Object.Destroy(Capsule);
					Serialize = false;
				}
			}
			catch
            {}
		}

		private static string RandomString(int length)
		{
			char[] array = "abcdefghlijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789".ToArray();
			string text = "";
			Il2CppSystem.Random random = new Il2CppSystem.Random(new Il2CppSystem.Random().Next(length));
			for (int i = 0; i < length; i++)
			{
				text += array[random.Next(array.Length)].ToString();
			}
			return text;
		}

		public static string RandomNumberString(int length)
		{
			string text = "";
			for (int i = 0; i < length; i++)
			{
				text += new System.Random().Next(0, int.MaxValue).ToString("X8");
			}
			return text;
		}

		public static bool DisconnectToggle = false;
		public static bool DebugSpamToggle = false;

		private static VRC_EventHandler handler;

		public static void DisconnectLobby()
		{
			if (handler == null)
			{
				handler = Resources.FindObjectsOfTypeAll<VRC_EventHandler>()[0];
			}
            VRC_EventHandler.VrcEvent vrcEvent = new VRC_EventHandler.VrcEvent
			{
				EventType = (VRC_EventHandler.VrcEventType)20,
				ParameterObject = handler.gameObject,
				ParameterInt = 1,
				ParameterFloat = 0f,
				ParameterString = "<3 WengaPort <3 | " + RandomString(820) + " | <3 WengaPort <3",
				ParameterBoolOp = (VRC_EventHandler.VrcBooleanOp)(-1),
				ParameterBytes = new Il2CppStructArray<byte>(0L)
			};
			int Type = 0;
			Player player = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray()[new Il2CppSystem.Random().Next(0, PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.Count)];
			handler.TriggerEvent(vrcEvent, (VRC_EventHandler.VrcBroadcastType)Type, player.gameObject, 0f);
			Player player2 = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray()[new Il2CppSystem.Random().Next(0, PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.Count)];
			handler.TriggerEvent(vrcEvent, (VRC_EventHandler.VrcBroadcastType)Type, player2.gameObject, 0f);
		}

		public static void DebugSpam()
		{
			if (handler == null)
			{
				handler = Resources.FindObjectsOfTypeAll<VRC_EventHandler>()[0];
			}
			VRC_EventHandler.VrcEvent vrcEvent = new VRC_EventHandler.VrcEvent
			{
				EventType = (VRC_EventHandler.VrcEventType)16,
				ParameterObject = handler.gameObject,
				ParameterInt = 1,
				ParameterFloat = 0f,
				ParameterString = "Fuck your Debug?, UwU <3 WengaPort <3 \nFuck your Debug?, UwU <3 WengaPort <3 \nFuck your Debug?, UwU <3 WengaPort <3 \nFuck your Debug?, UwU <3 WengaPort <3 \nFuck your Debug?, UwU <3 WengaPort <3 \nFuck your Debug?, UwU <3 WengaPort <3 \nFuck your Debug?, UwU <3 WengaPort <3 \nFuck your Debug?, UwU <3 WengaPort <3 \nFuck your Debug?, UwU <3 WengaPort <3 \nFuck your Debug?, UwU <3 WengaPort <3 \nFuck your Debug?, UwU <3 WengaPort <3 \n",
				ParameterBoolOp = (VRC_EventHandler.VrcBooleanOp)(-1),
				ParameterBytes = new Il2CppStructArray<byte>(0L)
			};
			int Type = 4;
			Player player = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray()[new Il2CppSystem.Random().Next(0, PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.Count)];
			handler.TriggerEvent(vrcEvent, (VRC_EventHandler.VrcBroadcastType)Type, player.gameObject, 0f);
			Player player2 = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray()[new Il2CppSystem.Random().Next(0, PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.Count)];
			handler.TriggerEvent(vrcEvent, (VRC_EventHandler.VrcBroadcastType)Type, player2.gameObject, 0f);
		}
	}
}
