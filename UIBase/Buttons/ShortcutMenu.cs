using UnityEngine;
using MelonLoader;
using WengaPort.Api;
using WengaPort.Wrappers;
using WengaPort.Modules;
using System;
using System.Runtime.InteropServices;
using System.Linq;
using VRC;
using System.Windows.Forms;
using VRC.SDKBase;
using WengaPort.Extensions;
using static VRC.SDKBase.VRC_EventHandler;
using UnityEngine.UI;
using VRC.Udon;

namespace WengaPort.Buttons
{
    class MainMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton HalfButton;
        public static QMToggleButton FlyButton;
        public static QMToggleButton NoClipButton;
        public static QMToggleButton HalfToggleButton;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton("ShortcutMenu", 0, 0, "MainButton", "Main Menu", Color.cyan, Color.magenta, Color.black, Color.yellow);
            ThisMenu.getMainButton().setActive(false); //Hidding this button cause we gonna create a new one with a custom sprite on it -->
            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.ShortCut, ButtonType.Single, "", 0, -1.5f, delegate ()
            {
                QMStuff.ShowQuickmenuPage(ThisMenu.getMenuName()); //Open the menu we created above ^

            }, "Open the Client", Color.black, Color.clear, null, "https://i.imgur.com/tfavyxX.png")); // 292 x 281 png

            Panels.TextMenu(ThisMenu, 2.5f, 1, "\nMenu\nSelector", 4, 3, "Menu Selector");

            QMNestedButton PrefabsPage = new QMNestedButton(ThisMenu, 2.5f, 2f, "World", "World Menu");
            QMNestedButton PrefabsSubPage = new QMNestedButton(PrefabsPage, 1.5f, 1f, "Prefab", "Prefab Menu");
            PrefabsPage.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            GameObject SelectedPrefab = null;
            QMNestedButton Options = new QMNestedButton(PrefabsPage, -1, -1, "", "Options about the prefabs");
            Options.getMainButton().setActive(false);
            new QMSingleButton(PrefabsPage, 0, 0, "Delete \n Prefabs", delegate
            {
                // Add stuff here later 
            }, "Delete all Prefabs in the World");

            new QMSingleButton(Options, 1, 0, "Instatiate\nMe\nGlobal", delegate
            {
                Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, Utils.CurrentUser.transform.position + Utils.CurrentUser.transform.forward * 2f, Utils.CurrentUser.transform.rotation);
            }, "Instatiate Prefab at yourself Global");

            new QMSingleButton(Options, 2, 0, "Instatiate\nMe\nLocal", delegate
            {
                Networking.Instantiate(VrcBroadcastType.Local, SelectedPrefab.name, Utils.CurrentUser.transform.position + Utils.CurrentUser.transform.forward * 2f, Utils.CurrentUser.transform.rotation);
            }, "Instatiate Prefab at yourself Local");

            new QMSingleButton(Options, 3, 0, "Instatiate\nPos-Infinity\nGlobal", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, Vector3.positiveInfinity, Utils.CurrentUser.transform.rotation);
                Item.SetActive(false);
            }, "Instatiate Prefab at Positive-Infinity Global");

            new QMSingleButton(Options, 4, 0, "Instatiate\nPos-Infinity\nLocal", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Local, SelectedPrefab.name, Vector3.positiveInfinity, Utils.CurrentUser.transform.rotation);
                Item.SetActive(false);
            }, "Instatiate Prefab at Positive-Infinity Local");

            new QMSingleButton(Options, 3, 1, "Instatiate\nNeg-Infinity\nGlobal", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, Vector3.negativeInfinity, Utils.CurrentUser.transform.rotation);
                Item.SetActive(false);
            }, "Instatiate Prefab at Negative-Infinity Global");

            new QMSingleButton(Options, 4, 1, "Instatiate\nNeg-Infinity\nLocal", delegate
            {
                GameObject Item = Networking.Instantiate(VrcBroadcastType.Local, SelectedPrefab.name, Vector3.negativeInfinity, Utils.CurrentUser.transform.rotation);
                Item.SetActive(false);
            }, "Instatiate Prefab at Negative-Infinity Global");
            
            ScrollMenu PrefabsScroll = new ScrollMenu(PrefabsSubPage);
            PrefabsScroll.SetAction(delegate
            {
                foreach (var prefab in VRCUiManagerExtension.GetWorldPrefabs())
                {
                    PrefabsScroll.Add(new QMSingleButton(PrefabsSubPage, 0, 0, prefab.name, delegate
                    {
                        SelectedPrefab = prefab;
                        QMStuff.ShowQuickmenuPage(Options.getMenuName());
                    }, prefab.name));
                }
            });

            QMNestedButton UdonExploits = new QMNestedButton(PrefabsPage, 3.5f, 1f, "Udon", "Udon Trigger/Event Menu");
            ScrollMenu UdonEvents = new ScrollMenu(new QMNestedButton(UdonExploits, -5, -5, "", ""));
            ScrollMenu UdonObjects = new ScrollMenu(UdonExploits);
            UdonObjects.SetAction(delegate
            {
                foreach (var Udon in UnityEngine.Object.FindObjectsOfType<UdonBehaviour>())
                {
                    UdonObjects.Add(new QMSingleButton(UdonExploits, 0, 0, Udon.gameObject.name, delegate
                    {
                        UdonEvents.SetAction(delegate
                        {
                            foreach (var UdonEvent in Udon._eventTable)
                            {
                                UdonEvents.Add(new QMSingleButton(UdonEvents.BaseMenu, 0, 0, UdonEvent.Key, delegate
                                {
                                    Udon.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, UdonEvent.Key);
                                }, Udon.gameObject + " Execute " + UdonEvent.Key));
                            }
                        });
                        UdonEvents.BaseMenu.getMainButton().getGameObject().GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                    }, Udon.interactText));
                }
            });

            QMNestedButton TriggerMenu = new QMNestedButton(PrefabsPage, 2.5f, 1f, "Trigger", "VRC Trigger Menu");
            ScrollMenu TriggerObjects = new ScrollMenu(TriggerMenu);
            TriggerObjects.SetAction(delegate
            {
                foreach (var Trigger in UnityEngine.Object.FindObjectsOfType<VRC_Trigger>())
                {
                    TriggerObjects.Add(new QMSingleButton(TriggerMenu, 0, 0, Trigger.gameObject.name, delegate
                    {
                       Trigger.Interact();
                    }, Trigger.interactText));
                }
            });

            HalfButton = new QMSingleButton("ShortcutMenu", 5, 1.25f, "Delete \nPortals", () =>
            {
                PortalHandler.DeletePortals();
            }, "Delete all Portals");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton("ShortcutMenu", 5, 0.25f, "Copy \nWorldID", () =>
            {
                Clipboard.SetText($"{RoomManager.field_Internal_Static_ApiWorld_0.id}:{RoomManager.field_Internal_Static_ApiWorldInstance_0.idWithTags}");
                Extensions.Logger.WengaLogger("World ID copied to Clipboard");
            }, "Copy RoomID to Clipboard");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton("ShortcutMenu", 5, 0.75f, "Forcejoin", () =>
            {
                if (Clipboard.GetText().Contains("wrld_"))
                {
                    Networking.GoToRoom(Clipboard.GetText());
                }
            }, "Join World by Clipboard ID");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            FlyButton = new QMToggleButton("ShortcutMenu", 0, 0, "Fly", () =>
            {
                Movement.FlyEnable();
            }, "Disabled", () =>
            {
                Movement.FlyDisable();
            }, "Toggle Fly Mode");

            QMNestedButton TargetMenu = new QMNestedButton("ShortcutMenu", 5, -0.25f, "Players",
                "All current Players in the World");
            TargetMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);
            ScrollMenu menu = new ScrollMenu(TargetMenu);
            QMNestedButton SelectedMenu = new QMNestedButton(TargetMenu, 0, -1, "Recent", "Select the Recent Player",
               null, null, null, null);
            new QMSingleButton(TargetMenu, 0, 0, "Self Select", () =>
            {
                GeneralWrappers.GetQuickMenu().SelectPlayer(Utils.CurrentUser.prop_Player_0);
            }, "Select Yourself");
            Player SelectedPlayer = null;
            new QMSingleButton(SelectedMenu, 1, 0, "Select",delegate 
            {
                Utils.QuickMenu.SelectPlayer(SelectedPlayer); 
            },"Select the Player");

            new QMSingleButton(SelectedMenu, 2, 0, "Teleport", delegate
            {
                Utils.CurrentUser.transform.position = SelectedPlayer.transform.position;
            }, "Teleport to Player");
            menu.SetAction(delegate
            {
                foreach (Player player in Utils.PlayerManager.GetAllPlayers().ToArray())
                {
                    menu.Add(new QMSingleButton(menu.BaseMenu, 0, 0, player.DisplayName(), new Action(() =>
                    {
                        SelectedPlayer = player;
                        QMStuff.ShowQuickmenuPage(SelectedMenu.getMenuName());
                    }), "Select Player", null, player.GetAPIUser().GetRankColor()));
                }
            });

            NoClipButton = new QMToggleButton("ShortcutMenu", 0, 1, "NoClip", () =>
            {
                Movement.NoClipEnable();
            }, "Disabled", () =>
            {
                Movement.NoClipDisable();
            }, "Toggle NoClip");

            HalfToggleButton = new QMToggleButton("ShortcutMenu", 0, 3, "FM", () =>
            {
                try
                {
                    foreach (Player instance in Utils.PlayerManager.GetAllPlayers().ToArray())
                    {
                        InteractMenu.HearOffPlayers.Remove(instance.GetVRCPlayer().UserID());
                        instance.field_Internal_VRCPlayer_0.field_Internal_Boolean_1 = true;
                        if (!instance.IsFriend())
                        {
                            InteractMenu.HearOffPlayers.Add(instance.GetVRCPlayer().UserID());
                            instance.field_Internal_VRCPlayer_0.field_Internal_Boolean_1 = false;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }, "Off", () =>
            {
                try
                {
                    foreach (Player instance in Utils.PlayerManager.GetAllPlayers().ToArray())
                    {
                        InteractMenu.HearOffPlayers.Remove(instance.GetVRCPlayer().UserID());
                        instance.field_Internal_VRCPlayer_0.field_Internal_Boolean_1 = true;
                    }
                }
                catch (Exception)
                {
                }
            }, "Forcemute non Friends");
            HalfToggleButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2, 1.75f); 
            HalfToggleButton.btnOn.gameObject.GetComponent<RectTransform>().sizeDelta /= new Vector2(2.7f, 1.75f);  
            HalfToggleButton.btnOff.gameObject.GetComponent<RectTransform>().sizeDelta /= new Vector2(2.7f, 1.75f);
            HalfToggleButton.getGameObject().transform.localPosition = UIChanges.GetButtonPosition(-0.25f, 2.53f);
        }
    }
}
