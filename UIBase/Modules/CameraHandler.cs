using MelonLoader;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRC.UserCamera;
using WengaPort.Wrappers;

namespace WengaPort.Modules
{
	class CameraHandler
	{
		public static bool cameramode = false;
		public static void CameraTrigger()
		{
			if (Input.GetKeyDown(KeyCode.K) && Input.GetKey(KeyCode.LeftControl))
			{
				if (cameramode)
				{
					SetCameraMode(CameraMode.Off);
					cameramode = false;
				}
				else
				{
					SetCameraMode(CameraMode.Photo);
					cameramode = true;
				}
			}
		}
		public static void TakePicture(int timer)
		{
			UserCameraController userCameraController = Utils.UserCameraController;
			userCameraController.photoCaptureCooldown = 0f;
			userCameraController.StartCoroutine(userCameraController.Method_Private_IEnumerator_Int32_PDM_0(timer));
		}

		public static void Disable()
		{
			UserCameraController userCameraController = Utils.UserCameraController;
			userCameraController.enabled = false;
		}

		public static bool AnnoyingCam = false;

		public static void Enable()
		{
			UserCameraController userCameraController = Utils.UserCameraController;
			userCameraController.enabled = true;
			userCameraController.StopAllCoroutines();
		}

		public static bool DesktopCam = false;
		public static void EnableDesktopCam()
		{
			DesktopCam = true;
			UserCameraController instance = Utils.UserCameraController;
			instance.prop_EnumPublicSealedvaOfPhVi4vUnique_0 = EnumPublicSealedvaOfPhVi4vUnique.Photo;
		}

		public static void DisableDesktopCam()
		{
			DesktopCam = false;
			UserCameraController instance = Utils.UserCameraController;
			instance.prop_EnumPublicSealedvaOfPhVi4vUnique_0 = EnumPublicSealedvaOfPhVi4vUnique.Off;
		}

		public static UserCameraController UserCameraController
		{
			get
			{
				return UserCameraController.field_Internal_Static_UserCameraController_0;
			}
		}
		public static void SetCameraMode(CameraMode mode)
		{
			mode = (CameraMode)Utils.UserCameraController.prop_EnumPublicSealedvaOfPhVi4vUnique_0;
		}
		public enum CameraMode
		{
			Off,
			Photo,
			Video
		}

		public static void PictureRPC(Player Target)
		{
			UserCameraIndicator field_Internal_UserCameraIndicator_ = Utils.UserCameraController.field_Internal_UserCameraIndicator_0;
			field_Internal_UserCameraIndicator_.PhotoCapture(Target);
		}

		public static bool CameraLag = false;

		private static bool _isZoomed;
		private static bool _enableZoom = true;
		private static float _beforeZoomFOV = 60f;
		private static float _FOV = 60f;
		private static float _zoomMultiplier = 6f;
		public static void Zoom()
        {
			if (Input.GetKey(KeyCode.LeftAlt) && _enableZoom && !_isZoomed && !Input.GetKey(KeyCode.Tab))
			{
				_isZoomed = true;
				_beforeZoomFOV = 60f;
				_FOV /= _zoomMultiplier;
				GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud/ReticleParent").SetActive(false);
			}
			else
			{
				if (Input.GetKeyUp(KeyCode.LeftAlt) && _enableZoom && _isZoomed || !Application.isFocused)
				{
					_isZoomed = false;
					_FOV = _beforeZoomFOV;
					GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud/ReticleParent").SetActive(true);
				}
			}
			Camera.main.fieldOfView = _FOV;
		}

		public static bool UserCamAnnoy = false;
		internal static IEnumerator AnnoyingCamera(Player player)
		{
			UserCameraController instance = Utils.UserCameraController;
			instance.prop_EnumPublicSealedvaOfPhVi4vUnique_0 = EnumPublicSealedvaOfPhVi4vUnique.Photo;
			instance.prop_EnumPublicSealedvaNoSmLo4vUnique_0 = EnumPublicSealedvaNoSmLo4vUnique.Look_At;
			instance.prop_EnumPublicSealedvaAtLoWoCO5vUnique_0 = EnumPublicSealedvaAtLoWoCO5vUnique.World;
			TakePicture(int.MaxValue);
			UserCamAnnoy = true;
			yield return new WaitForSeconds(0.3f);
			for (; ; )
			{
				while (RoomManager.field_Internal_Static_ApiWorld_0 == null) yield break;
				while (player.field_Internal_VRCPlayer_0 == null) yield break;
				instance.transform.position = player.transform.position;
				instance.field_Private_Vector3_0 = player.transform.position;
				instance.photoCaptureCooldown = 0f;
				PictureRPC(player);
				
				if (!UserCamAnnoy)
                {
					instance.StopAllCoroutines();
					yield return new WaitForSeconds(0.1f);
					instance.transform.position = Utils.CurrentUser.transform.position;
					instance.field_Private_Vector3_0 = Utils.CurrentUser.transform.position; ;
					instance.prop_EnumPublicSealedvaOfPhVi4vUnique_0 = EnumPublicSealedvaOfPhVi4vUnique.Off;
					instance.prop_EnumPublicSealedvaNoSmLo4vUnique_0 = EnumPublicSealedvaNoSmLo4vUnique.None;
					instance.prop_EnumPublicSealedvaAtLoWoCO5vUnique_0 = EnumPublicSealedvaAtLoWoCO5vUnique.Attached;
					instance.prop_EnumPublicSealedvaOfPhVi4vUnique_0 = EnumPublicSealedvaOfPhVi4vUnique.Photo;
					yield return new WaitForSeconds(0.1f);
					instance.prop_EnumPublicSealedvaOfPhVi4vUnique_0 = EnumPublicSealedvaOfPhVi4vUnique.Off;
					yield break;
				}
				yield return new WaitForSeconds(0.5f);
			}
			yield break;
		}

		internal static IEnumerator CameraCrash()
		{
			PhotonModule.EmojiRPC(29);
			Extensions.Logger.WengaLogger("[CamCrash] Start");
			UserCameraController instance = Utils.UserCameraController;
			instance.prop_EnumPublicSealedvaOfPhVi4vUnique_0 = EnumPublicSealedvaOfPhVi4vUnique.Photo;
			instance.prop_EnumPublicSealedvaNoSmLo4vUnique_0 = EnumPublicSealedvaNoSmLo4vUnique.Look_At;
			instance.prop_EnumPublicSealedvaAtLoWoCO5vUnique_0 = EnumPublicSealedvaAtLoWoCO5vUnique.World;
			yield return new WaitForSeconds(0.5f);
			instance.transform.position = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
			instance.field_Private_Vector3_0 = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
			instance.transform.rotation = new Quaternion(float.NaN, float.NaN, float.NaN, float.NaN);
			instance.field_Internal_UserCameraIndicator_0.Speaker.enabled = false;
			instance.photoCaptureCooldown = 0f;
			yield return new WaitForSeconds(0.5f);
			for (int i = 0; i < 45; i++)
            {
				TakePicture(int.MaxValue);
			}
			yield return new WaitForSeconds(5.5f);
			instance.transform.position = new Vector3(int.MinValue, int.MinValue, int.MinValue);
			instance.field_Private_Vector3_0 = new Vector3(int.MinValue, int.MinValue, int.MinValue);
			instance.transform.rotation = new Quaternion(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
			yield return new WaitForSeconds(4f);
			instance.StopAllCoroutines();
			yield return new WaitForSeconds(0.5f);
			instance.field_Internal_UserCameraIndicator_0.Speaker.enabled = true;
			instance.transform.position = Utils.CurrentUser.transform.position;
			instance.field_Private_Vector3_0 = Utils.CurrentUser.transform.position;;
			instance.prop_EnumPublicSealedvaOfPhVi4vUnique_0 = EnumPublicSealedvaOfPhVi4vUnique.Off;
			yield return new WaitForSeconds(0.5f);
			instance.prop_EnumPublicSealedvaNoSmLo4vUnique_0 = EnumPublicSealedvaNoSmLo4vUnique.None;
			instance.prop_EnumPublicSealedvaAtLoWoCO5vUnique_0 = EnumPublicSealedvaAtLoWoCO5vUnique.Attached;
			instance.prop_EnumPublicSealedvaOfPhVi4vUnique_0 = EnumPublicSealedvaOfPhVi4vUnique.Photo;
			yield return new WaitForSeconds(0.5f);
			instance.prop_EnumPublicSealedvaOfPhVi4vUnique_0 = EnumPublicSealedvaOfPhVi4vUnique.Off;
			Extensions.Logger.WengaLogger("[CamCrash] Crash Done");
			yield break;
		}

		public static void ToggleBlockAll()
		{
			{
				foreach (var Player in Utils.PlayerManager.GetAllPlayers())
				{
					var userinfo = GameObject.Find("Screens").transform.Find("UserInfo").GetComponent<VRC.UI.PageUserInfo>();
					{
						userinfo.user = new APIUser
						{
							id = Player.field_Private_APIUser_0.id
						};

						if (userinfo.user.id != APIUser.CurrentUser.id)
						{
							userinfo.ToggleBlock();
						}
					}
				}
			}
		}

		public static void ToggleBlockOne(Player player)
		{
			{
				var userinfo = GameObject.Find("Screens").transform.Find("UserInfo").GetComponent<VRC.UI.PageUserInfo>();
				{
					userinfo.user = new APIUser
					{
						id = player.field_Private_APIUser_0.id
					};
					if (userinfo.user.id != APIUser.CurrentUser.id)
					{
						userinfo.ToggleBlock();
					}
				}
			}
		}

		private static bool CrashPlayerRunning = false;
		internal static IEnumerator TargetCamCrash(Player Target)
		{
			if (CrashPlayerRunning)
            {
				Extensions.Logger.WengaLogger("A Crash is already in Action");
			}
			else if (!CrashPlayerRunning)
			{
				PortalHandler.Camstrings.Clear();
				CrashPlayerRunning = true;
				Extensions.Logger.WengaLogger("Crash Started on " + Target.DisplayName());
				foreach (var player in Utils.PlayerManager.GetAllPlayers())
				{
					PortalHandler.Camstrings.Add(player);
				}
				foreach (var player in PortalHandler.Camstrings)
				{
					if (player != null)
						if (player.UserID() != Target.UserID())
						{
							ToggleBlockOne(player);
							yield return new WaitForSeconds(0.33f);
						}
				}
				yield return new WaitForSeconds(1.5f);
				MelonCoroutines.Start(CameraCrash());
				yield return new WaitForSeconds(12.5f);
				foreach (var player in PortalHandler.Camstrings)
				{
					if (player != null)
						if (player.UserID() != Target.UserID())
						{
							ToggleBlockOne(player);
							yield return new WaitForSeconds(0.33f);
						}
				}
				Extensions.Logger.WengaLogger("Crash Done on " + Target.DisplayName());
				CrashPlayerRunning = false;
			}
			yield break;
		}

		public static string BackAvatarID;
		public static IEnumerator AvatarCrash(bool active)
		{
			if (!active)
			{
				PlayerExtensions.ChangeAvatar(BackAvatarID);
				yield return new WaitForSeconds(1);
				SelfHide.Initialize(false);
				PlayerExtensions.ReloadAvatar(Utils.CurrentUser);
			}
			else
            {
				BackAvatarID = Utils.CurrentUser.GetAPIAvatar().id;
				SelfHide.Initialize(true);
				PlayerExtensions.ChangeAvatar("avtr_5c66e56a-9fe7-4f62-9c7a-93bf8b75d717");
			}
			yield break;
		}

		internal static IEnumerator TargetAvatarCrash(Player Target, string AvatarID)
		{
			if (CrashPlayerRunning)
			{
				Extensions.Logger.WengaLogger("A Crash is already in Action");
			}
			else if (!CrashPlayerRunning)
			{
				PortalHandler.Camstrings.Clear();
				CrashPlayerRunning = true;
				Extensions.Logger.WengaLogger("Crash Started on " + Target.DisplayName());
				foreach (var player in Utils.PlayerManager.GetAllPlayers())
				{
					PortalHandler.Camstrings.Add(player);
				}
				foreach (var player in PortalHandler.Camstrings)
				{
					if (player != null)
						if (player.UserID() != Target.UserID())
						{
							ToggleBlockOne(player);
							yield return new WaitForSeconds(0.33f);
						}
				}
				var BackSwitch = Utils.CurrentUser.GetAPIAvatar().id;
				SelfHide.Initialize(true);
				yield return new WaitForSeconds(2f);
				PlayerExtensions.ChangeAvatar(AvatarID);
				yield return new WaitForSeconds(15f);
				PlayerExtensions.ChangeAvatar(BackSwitch);
				yield return new WaitForSeconds(1.5f);
				SelfHide.Initialize(false);
				PlayerExtensions.ReloadAvatar(Utils.CurrentUser);
				yield return new WaitForSeconds(2f);
				foreach (var player in PortalHandler.Camstrings)
				{
					if (player != null)
						if (player.UserID() != Target.UserID())
						{
							ToggleBlockOne(player);
							yield return new WaitForSeconds(0.33f);
						}
				}
				Extensions.Logger.WengaLogger("Crash Done on " + Target.DisplayName());
				CrashPlayerRunning = false;
			}
			yield break;
		}

		public static bool MenuRemover = false;
		public static bool FloorRemover = false;
		internal static IEnumerator PortalColliderRemover()
        {
			for (; ; )
			{
				if (!FloorRemover)
                {
					yield break;
                }
				GameObject InfPortal2 = Networking.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "Portals/PortalInternalDynamic", new Vector3(int.MaxValue, int.MaxValue, int.MaxValue) * 268, new Quaternion(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity));
				InfPortal2.transform.position = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue) * 268;
				Networking.RPC(RPC.Destination.AllBufferOne, InfPortal2, "ConfigurePortal", new Il2CppSystem.Object[]
				{
					(Il2CppSystem.String)"wrld_5b89c79e-c340-4510-be1b-476e9fcdedcc",
					(Il2CppSystem.String)PhotonModule.RandomNumberString(4),
					new Il2CppSystem.Int32
					{
						m_value = int.MinValue
					}.BoxIl2CppObject()
				});
				InfPortal2.SetActive(false);
				yield return new WaitForSeconds(4);
				GameObject InfPortal3 = Networking.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "Portals/PortalInternalDynamic", new Vector3(int.MinValue, int.MinValue, int.MinValue) * 268, new Quaternion(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity));
				InfPortal3.transform.position = new Vector3(int.MinValue, int.MinValue, int.MinValue) * 268;
				Networking.RPC(RPC.Destination.AllBufferOne, InfPortal3, "ConfigurePortal", new Il2CppSystem.Object[]
				{
					(Il2CppSystem.String)"wrld_5b89c79e-c340-4510-be1b-476e9fcdedcc",
					(Il2CppSystem.String)PhotonModule.RandomNumberString(4),
					new Il2CppSystem.Int32
					{
						m_value = int.MinValue
					}.BoxIl2CppObject()
				});
				InfPortal3.SetActive(false);
				yield return new WaitForSeconds(4);
			}
			yield break;
		}

		public static void DisablePostProcess(bool state)
		{
			if (state)
            {
				ItemHandler.PostProcessToggle = true;
				foreach (var volume in Resources.FindObjectsOfTypeAll<PostProcessVolume>())
				{
					if (volume.isActiveAndEnabled)
					{
						volume.enabled = false;
						ItemHandler.PostProcess.Add(volume);
					}
				}
			}
			else
            {
				ItemHandler.PostProcessToggle = false;
				foreach (var volume in ItemHandler.PostProcess)
                {
					volume.enabled = true;
					ItemHandler.PostProcess.Remove(volume);
				}
            }
		}

		public static void DisableAvatarPedestals(bool state)
		{
			if (state)
			{
				ItemHandler.PedestalToggle = true;
				foreach (var volume in Resources.FindObjectsOfTypeAll<VRC_AvatarPedestal>())
				{
					if (volume.isActiveAndEnabled)
					{
						volume.enabled = false;
						ItemHandler.Pedestals.Add(volume);
					}
				}
			}
			else
			{
				ItemHandler.PedestalToggle = false;
				foreach (var volume in ItemHandler.Pedestals)
				{
					volume.enabled = true;
					ItemHandler.Pedestals.Remove(volume);
				}
			}
		}
		private static GameObject NightLayer = new GameObject();

		public static void NightMode(bool state)
		{
			if (state)
			{
				ItemHandler.NightmodeToggle = true;
				NightLayer = Object.Instantiate(new GameObject(), null,true);
				NightLayer.layer = 8;
				NightLayer.SetActive(true);
				NightLayer.transform.position = Utils.CurrentUser.transform.position;
				NightLayer.transform.rotation = Utils.CurrentUser.transform.rotation;
			}
			else
			{
				ItemHandler.NightmodeToggle = false;
				Object.Destroy(NightLayer);
			}
		}
	}
}
