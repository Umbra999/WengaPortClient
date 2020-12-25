using UnityEngine;
using WengaPort.Modules;
using VRC.Core;
using WengaPort.Extensions;
using System;
using WengaPort.Utility;
using MenuType = WengaPort.Utility.MenuType;
using WengaPort.Wrappers;
using System.Diagnostics;
using VRC.UI;
using MelonLoader;
using UnityEngine.UI;
using WengaPort.FoldersManager;
using VRC.SDKBase;
using VRC;
using UnhollowerRuntimeLib;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

namespace WengaPort.Buttons
{
    class SocialMenu
    {
        public static void Initialize()
        {
            new MenuButton(MenuType.UserInfo, MenuButtonType.PlaylistButton, "Teleport", 585f, -280f, new Action(() =>
            {
                APIUser apiuser = Utils.VRCUiManager.SelectedAPIUser();
                Player p = Utils.PlayerManager.GetPlayer(apiuser.UserID());
                Utils.CurrentUser.transform.position = p.transform.position;
            }));

            new MenuButton(MenuType.UserInfo, MenuButtonType.PlaylistButton, "Drop Portal", -100f, -280f, new Action(() =>
            {
                APIUser user = Utils.VRCUiManager.SelectedAPIUser();
                if (user.location.Contains("wrld_"))
                {
                    PortalHandler.DropPortal(user.location);
                }
            }));

            new MenuButton(MenuType.UserInfo, MenuButtonType.PlaylistButton, "Check Instance", 355f, -280f, new Action(() =>
            {
                if (PhotonMenu.Ini.GetBool("Toggles", "Bot", true))
                {
                    APIUser user = Utils.VRCUiManager.SelectedAPIUser();
                    if (user.location.Contains("wrld_"))
                    {
                        Extensions.Logger.WengaLogger("Checking " + user.location);
                        PhotonMenu.Ini.SetString("Toggles", "SocialInstance", user.location);
                        PhotonMenu.Ini.SetBool("Toggles", "WhoInInstance", true);
                    }
                }
                else
                {
                    Extensions.Logger.WengaLogger("Start the Photonbots before using this feature");
                }
            }));

            new MenuButton(MenuType.WorldInfoMenu, MenuButtonType.ReportButton, "Check Instance", 65f, -145f, new Action(() =>
            {
                if (PhotonMenu.Ini.GetBool("Toggles", "Bot", true))
                {
                    var WorldScreen = GameObject.Find("Screens").transform.Find("WorldInfo");
                    var Infos = WorldScreen.transform.GetComponentInChildren<PageWorldInfo>();

                    var wid = Infos.field_Private_ApiWorld_0.id + ":" + Infos.worldInstance.idWithTags;  
                    if (wid.Contains("wrld_"))
                    {
                        Extensions.Logger.WengaLogger("Checking " + wid);
                        PhotonMenu.Ini.SetString("Toggles", "SocialInstance", wid);
                        PhotonMenu.Ini.SetBool("Toggles", "WhoInInstance", true);
                    }
                }
                else
                {
                    Extensions.Logger.WengaLogger("Start the Photonbots before using this feature");
                }
            }));

            new MenuButton(MenuType.WorldInfoMenu, MenuButtonType.ReportButton, "RIP VRCW", 65f, -255f, new Action(() =>
            {
                var WorldScreen = GameObject.Find("Screens").transform.Find("WorldInfo");
                var Info = WorldScreen.transform.GetComponentInChildren<PageWorldInfo>();
                ApiWorld apiWorld = Info.field_Private_ApiWorld_0;
                RippingHandler.DownloadWorld(apiWorld);
            }));

            new MenuButton(MenuType.UserInfo, MenuButtonType.PlaylistButton, "Open VRChat", 125f, -280f, new Action(() =>
            {
                Process.Start("https://vrchat.com/home/user/" + Utils.VRCUiManager.SelectedAPIUser().UserID());
            }));

            Button button2 = GameObject.Find("/UserInterface/MenuContent/Screens/UserInfo/User Panel/OnlineFriend/JoinButton").GetComponent<Button>();

            if (PatchManager.QuestSpoof || Create.Ini.GetBool("Toggles", "QuestSpoof", true))
            {
                button2.onClick.RemoveAllListeners();
                button2.onClick.AddListener(new Action(() =>
                {
                    if (Utils.VRCUiManager.SelectedAPIUser() != null)
                    {
                        APIUser user = Utils.VRCUiManager.SelectedAPIUser();
                        if (user.location.Contains("wrld_"))
                        {
                            Networking.GoToRoom(user.location);
                        }
                    }
                }));
            }

            //GameObject unblockButton = UnityEngine.Object.Instantiate(GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/GoButton").transform, GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel").transform).gameObject;
            //unblockButton.GetComponentInChildren<Text>().text = "Unblock Loading";
            //unblockButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            //unblockButton.GetComponent<Button>().onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(delegate
            //{
            //    AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0.field_Private_Boolean_0 = false;
            //    if (EventSystem.current.currentSelectedGameObject == unblockButton)
            //        EventSystem.current.SetSelectedGameObject(null);
            //})));
            //unblockButton.GetComponent<Transform>().localPosition = new Vector3(-2.4f, -124f, 0);
            //GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress").transform.localPosition = new Vector3(0, 17, 0);
        }
    }
}