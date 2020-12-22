using UnityEngine;
using VRC;
using VRC.Core;
using VRC.UserCamera;
using Transmtn.DTO.Notifications;
using ModerationManager = ObjectPublicObLi1ApSiLi1ApBoSiUnique;
using System.Collections.Generic;

namespace WengaPort.Modules
{
	class Utils
	{
		public static VRCUiPopupManager VRCUiPopupManager
		{
			get
			{
				return VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0;
			}
		}

		public static VRCUiManager VRCUiManager
		{
			get
			{
				return VRCUiManager.prop_VRCUiManager_0;
			}
		}

		public static ModerationManager ModerationManager
		{
			get
			{
				return ModerationManager.prop_ObjectPublicObLi1ApSiLi1ApBoSiUnique_0;
			}
		}

		public static NotificationManager NotificationManager
		{
			get
			{
				return NotificationManager.field_Private_Static_NotificationManager_0;
			}
		}

		public static VRCWebSocketsManager VRCWebSocketsManager
		{
			get
			{
				return VRCWebSocketsManager.field_Private_Static_VRCWebSocketsManager_0;
			}
		}

		public static NetworkManager NetworkManager
		{
			get
			{
				return NetworkManager.field_Internal_Static_NetworkManager_0;
			}
		}

		public static PlayerManager PlayerManager
		{
			get
			{
				return PlayerManager.field_Private_Static_PlayerManager_0;
			}
		}

		public static VRCPlayer CurrentUser
		{
			get
			{
				return VRCPlayer.field_Internal_Static_VRCPlayer_0;
			}
			set
			{
                CurrentUser = CurrentUser;
			}
		}

		public static UserInteractMenu UserInteractMenu
		{
			get
			{
				return Resources.FindObjectsOfTypeAll<UserInteractMenu>()[0];
			}
		}

		public static Camera Camera
		{
			get
			{
				return VRCVrCamera.field_Private_Static_VRCVrCamera_0.screenCamera;
			}
		}

		public static VRCVrCamera VRCVrCamera
		{
			get
			{
				return VRCVrCamera.field_Private_Static_VRCVrCamera_0;
			}
		}

		public static UserCameraController UserCameraController
		{
			get
			{
				return UserCameraController.field_Internal_Static_UserCameraController_0;
			}
		}

		public static VRCTrackingManager VRCTrackingManager
		{
			get
			{
				return VRCTrackingManager.field_Private_Static_VRCTrackingManager_0;
			}
		}
		public static QuickMenu QuickMenu
		{
			get
			{
				return QuickMenu.prop_QuickMenu_0;
			}
		}
        public static object GeneralWrappers { get; internal set; }

		public static readonly List<string> EmojiType = new List<string>
		{
			":D",
			"Like",
			"Heart",
			"D:",
			"Dislike",
			"!!!",
			"xD",
			"=O",
			"???",
			":*",
			"*-*",
			">:{",
			"=|",
			"^P",
			"O_O",
			"=.=",
			"Fire",
			"M-moneys",
			"</3",
			"Present",
			"Beer",
			"Tomato",
			"Zzz...",
			"Thinking...",
			"Pizza",
			"Sunglasses",
			"Music Note",
			"GO!",
			"Wave",
			"Stop",
			"Clouds",
			"Pumpkin",
			"Spooky Ghost",
			"Skull",
			"Sweet-Candy",
			"Candy-Corn",
			"Boo!!",
			"Scary-bats",
			"Spider-Web",
			"",
			"Mistletoe",
			"Snowball",
			"Snowflake",
			"Coal",
			"Candy Cane",
			"Gingerbread",
			"Confetti",
			"Champagne",
			"Presents",
			"Beach-Ball",
			"Drink",
			"Hang Ten",
			"Ice Cream",
			"Life Ring",
			"Neon Shades",
			"Pineapple",
			"Splash",
			"Sun Lotion"
		};

		public static readonly List<string> EmoteType = new List<string>
		{
			"",
			"wave",
			"clap",
			"point",
			"cheer",
			"dance",
			"backflip",
			"die",
			"sadness"
		};
	}
}
