using System;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;
using VRC.UI;
using WengaPort.Api;
using UnityEngine.UI;
using WengaPort.Modules;

namespace WengaPort.Extensions
{
	public static class VRCUiManagerExtension
	{
		public static APIUser SelectedAPIUser(this VRCUiManager Instance)
		{
			return Instance.menuContent.GetComponentInChildren<PageUserInfo>().user;
		}

		public static void CloseUI(this VRCUiManager Instance, bool withFade = false)
		{
			try
			{
				Instance.Method_Public_Void_Boolean_0(withFade);
			}
			catch
			{
			}
		}
		public static VRCUiPage GetPage(this VRCUiManager Instance, string screenPath)
		{
			GameObject gameObject = GameObject.Find(screenPath);
			VRCUiPage vrcuiPage = null;
			if (gameObject != null)
			{
				vrcuiPage = gameObject.GetComponent<VRCUiPage>();
				if (vrcuiPage == null)
				{
                    Logger.WengaLogger("Screen Not Found - " + screenPath);
				}
			}
			else
			{
                Logger.WengaLogger("Screen Not Found - " + screenPath);
			}
			return vrcuiPage;
		}
		public static void HideCurrentPopUp(this VRCUiPopupManager instance)
		{
			Utils.VRCUiManager.HideScreen("POPUP");
		}

		public static void Alert(this VRCUiPopupManager instance, string title, string Content, string buttonname, Action action, string button2, Action action2)
		{
			instance.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_1(title, Content, buttonname, action, button2, action2, null);
		}

		public static List<GameObject> GetWorldPrefabs()
		{
			return VRC_SceneDescriptor.Instance.DynamicPrefabs.ToArray().ToList();
		}

		public static List<Material> GetWorldMaterials()
		{
			return VRC_SceneDescriptor.Instance.DynamicMaterials.ToArray().ToList();
		}

		public static void SelectPrefab(Action<GameObject> PrefabAction)
		{
			List<GameObject> Prefabs = GetWorldPrefabs();
			QMNestedButton PrefabsPage = new QMNestedButton("ShortcutMenu", -100, -100, "", "");
			ScrollMenu PrefabsScroll = new ScrollMenu(PrefabsPage);
			PrefabsScroll.SetAction(delegate
			{
				foreach (var prefab in Prefabs)
				{
					PrefabsScroll.Add(new QMSingleButton(PrefabsPage, 0, 0, prefab.name, delegate
					{
						PrefabAction.Invoke(prefab);
					}, prefab.name));
				}
			});
			PrefabsPage.getMainButton().getGameObject().GetComponent<Button>().onClick.Invoke();
			PrefabsPage.DestroyMe();
		}
	}
}
