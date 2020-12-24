using UnityEngine;
using MelonLoader;
using System.Linq;
using WengaPort.Api;
using System.Diagnostics;
using WengaPort.Modules;
using DiskWars;
using WengaPort.Wrappers;
using System.Collections.Generic;
using WengaPort.Extensions;
using System.IO;
using VRC.Core;
using VRCSDK2;
using VRC.SDKBase;
using System;
using VRC;
using VRC.UI;
using Logger = WengaPort.Extensions.Logger;
using static VRC.SDKBase.VRC_EventHandler;
using WengaPort.ConsoleUtils;
using WengaPort.FoldersManager;

namespace WengaPort.Buttons
{
    class InteractMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton HalfButton;
        public static QMToggleButton HearToggle;
        public static QMToggleButton SpamToggle;
        public static QMToggleButton RpcToggle;
        public static QMToggleButton BlockToggle;
        public static QMToggleButton BonesToggle;

        public static MenuText InfoText1;
        public static MenuText InfoText2;
        public static MenuText InfoText3;
        public static MenuText InfoText4;
        public static MenuText InfoText5;
        public static MenuText InfoText6;
        public static MenuText InfoText7;
        public static MenuText InfoText8;
        public static MenuText InfoText9;
        public static MenuText InfoText10;
        public static MenuText InfoText11;
        public static MenuText InfoText12;
        public static MenuText InfoText13;
        public static MenuText InfoText14;

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton("UserInteractMenu", 4, 0, "UserMenu", "ToolTip", Color.cyan, Color.magenta, Color.black, Color.yellow);
            Panels.TextMenu(ThisMenu, -1.7f, 0.5f, "\nInfo \nList", 3f, 4f, "All Informations about the Player");
            InfoText1 = new MenuText(ThisMenu, -1200f, 350f, "1");
            InfoText2 = new MenuText(ThisMenu, -1200f, 450f, "2");
            InfoText3 = new MenuText(ThisMenu, -1200f, 560f, "3");
            InfoText4 = new MenuText(ThisMenu, -1200f, 650f, "4");
            InfoText5 = new MenuText(ThisMenu, -1200f, 750f, "5");
            InfoText6 = new MenuText(ThisMenu, -1200f, 850f, "6");
            InfoText7 = new MenuText(ThisMenu, -1200f, 950f, "7");
            InfoText8 = new MenuText(ThisMenu, -1200f, 1050f, "8");
            InfoText9 = new MenuText(ThisMenu, -1200f, 1150f, "9");
            InfoText10 = new MenuText(ThisMenu, -1200f, 1250f, "10");
            InfoText11 = new MenuText(ThisMenu, -1200f, 1350f, "11");
            InfoText12 = new MenuText(ThisMenu, -1200f, 1450f, "12");
            InfoText13 = new MenuText(ThisMenu, -1200f, 1550f, "13");
            InfoText14 = new MenuText(ThisMenu, -1200f, 1635f, "14");
            ThisMenu.getMainButton().setActive(false); 

            MelonCoroutines.Start(ReworkedButtonAPI.CreateButton(MenuType.UserInteract, ButtonType.Single, "", 4, 0, delegate ()
            {
                QMStuff.ShowQuickmenuPage(ThisMenu.getMenuName());
                try
                {
                    VRCPlayer vrcplayer = Utils.QuickMenu.SelectedVRCPlayer();
                    InfoText1.SetText("<color=grey>Name:</color> " + vrcplayer.DisplayName());
                    InfoText2.SetText("<color=yellow>FPS:</color> " + vrcplayer.GetFrames());
                    InfoText3.SetText("<color=yellow>Ping:</color> " + vrcplayer.GetPing());
                    InfoText4.SetText("<color=white>Steam:</color> " + vrcplayer.field_Private_UInt64_0.ToString());
                    InfoText5.SetText("<color=purple>Bot:</color> " + vrcplayer.GetIsBot());
                    InfoText6.SetText("<color=lime>Avatar:</color> " + vrcplayer.GetAPIAvatar().name);
                    InfoText7.SetText("<color=yellow>Friend:</color> " + vrcplayer.GetAPIUser().isFriend);
                    InfoText8.SetText("<color=green>LastPlatform:</color> " + vrcplayer.GetAPIUser().last_platform);
                    InfoText9.SetText("<color=red>Moderator:</color> " + vrcplayer.GetAPIUser().hasModerationPowers);
                    InfoText10.SetText("<color=blue>Rank:</color> " + vrcplayer.GetAPIUser().GetRank());
                    InfoText11.SetText("<color=#FC0065>AvatarAuthor:</color> " + vrcplayer.GetAPIAvatar().authorName);
                    InfoText12.SetText("<color=magenta>Avatar Status:</color> " + vrcplayer.GetAPIAvatar().releaseStatus);
                    InfoText13.SetText("<color=grey>Actor Number:</color> " + vrcplayer.GetVRCPlayerApi().playerId);
                    InfoText14.SetText("<color=brown>Status:</color> " + vrcplayer.GetAPIUser().statusDescription);

                    if (!HearOffPlayers.Contains(Utils.QuickMenu.SelectedVRCPlayer().UserID()))
                    {
                        HearToggle.setToggleState(true, false);
                    }
                    else
                    {
                        HearToggle.setToggleState(false, false);
                    }
                    if (PortalHandler.kosstrings.Contains(vrcplayer.UserID()))
                    {
                        SpamToggle.setToggleState(true, false);
                    }
                    else
                    {
                        SpamToggle.setToggleState(false, false);
                    }
                    if (RPCAndEventBlock.Check(vrcplayer.UserID()))
                    {
                        RpcToggle.setToggleState(true, false);
                    }
                    else
                    {
                        RpcToggle.setToggleState(false, false);
                    }
                    if (PortalHandler.blockstrings.Contains(vrcplayer.UserID()))
                    {
                        BlockToggle.setToggleState(true, false);
                    }
                    else
                    {
                        BlockToggle.setToggleState(false, false);
                    }
                    if (GlobalDynamicBones.UsersBones.Contains(vrcplayer.UserID()) || GlobalDynamicBones.FriendOnlyBones.Contains(vrcplayer.UserID()) && GlobalDynamicBones.FriendBones)
                    {
                        BonesToggle.setToggleState(true, false);
                    }
                    else
                    {
                        BonesToggle.setToggleState(false, false);
                    }
                }
                catch (Exception)
                { }

            }, "WengaPort User Menu", Color.black, Color.clear, null, "https://i.imgur.com/jQEn1MA.png"));

            HearToggle = new QMToggleButton("UserInteractMenu", 4, 1, "Hear", () =>
            {
                HearOffPlayers.Remove(Utils.QuickMenu.SelectedVRCPlayer().UserID());
                Utils.QuickMenu.SelectedVRCPlayer().field_Private_Boolean_3 = true;
            }, "Off", () =>
            {
                HearOffPlayers.Add(Utils.QuickMenu.SelectedVRCPlayer().UserID());
                Utils.QuickMenu.SelectedVRCPlayer().field_Private_Boolean_3 = false;
            }, "Mute yourself for this User", Color.cyan, Color.white, false, true);

            HalfButton = new QMSingleButton(ThisMenu, 1, 0.25f, "Spawn \nPrefab", () =>
            {
                VRCPlayer player = Utils.QuickMenu.SelectedVRCPlayer();
                List<GameObject> Prefabs = VRCUiManagerExtension.GetWorldPrefabs();
                foreach (GameObject SelectedPrefab in Prefabs)
                {
                    Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, player.transform.position, player.transform.rotation);
                }
            }, "Spawn Prefabs on the Player");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 1, -0.25f, "Open \nVRChat", () =>
            {
                Process.Start("https://vrchat.com/home/user/" + Utils.QuickMenu.SelectedVRCPlayer().UserID());
            }, "Open VRChat Profile");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 3, -0.25f, "Drop \nPortal", () =>
            {
                PortalHandler.DropCrashPortal(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Drop Normal Portal on the Player");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 3, 0.25f, "Drop \nInfPortal", () =>
            {
                PortalHandler.DropInfinitePortal(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Drop Infinite Portal on the Player");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            RpcToggle = new QMToggleButton(ThisMenu, 4, 0, "Block \nRPC", () =>
            {
                VRCPlayer instance = Utils.QuickMenu.SelectedVRCPlayer();
                if (!RPCAndEventBlock.Check(instance.UserID()))
                {
                    RPCAndEventBlock.AddPlayer(instance.UserID());
                    File.WriteAllLines("WengaPort\\BlockedEvents.json", RPCAndEventBlock.Block);
                }
            }, "Disabled", () =>
            {
                VRCPlayer instance = Utils.QuickMenu.SelectedVRCPlayer();
                if (RPCAndEventBlock.Check(instance.UserID()))
                {
                    RPCAndEventBlock.RemovePlayer(instance.UserID());
                    File.Delete("WengaPort\\BlockedEvents.json");
                    File.WriteAllLines("WengaPort\\BlockedEvents.json", RPCAndEventBlock.Block);
                }
            }, "Block incoming Events/RPCs");

            BlockToggle = new QMToggleButton(ThisMenu, 4, 1, "Silent \nBlock", () =>
            {
                string id = QuickMenu.prop_QuickMenu_0.field_Private_APIUser_0.id;
                Utils.QuickMenu.SelectedVRCPlayer().gameObject.SetActive(false);
                PortalHandler.blockstrings.Remove(id);
            }, "Disabled", () =>
            {
                string id = QuickMenu.prop_QuickMenu_0.field_Private_APIUser_0.id;
                Utils.QuickMenu.SelectedVRCPlayer().gameObject.SetActive(true);
                PortalHandler.blockstrings.Remove(id);
            }, "Block the Player local");

            HalfButton = new QMSingleButton(ThisMenu, 2, -0.25f, "RIP \nVRCA", () =>
            {
                ApiAvatar apiAvatar = Utils.QuickMenu.SelectedVRCPlayer().GetAPIAvatar();
                RippingHandler.DownloadAvatar(apiAvatar);
            }, "Download the VRC File of the Avatar");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            SpamToggle = new QMToggleButton(ThisMenu, 3, 1, "Spam \nPortal", () =>
            {
                string id = QuickMenu.prop_QuickMenu_0.field_Private_APIUser_0.id;
                PortalHandler.kosstrings.Add(id);
            }, "Disabled", () =>
            {
                string id = QuickMenu.prop_QuickMenu_0.field_Private_APIUser_0.id;
                PortalHandler.kosstrings.Remove(id);
            }, "Spam the Player with Portals");

            BonesToggle = new QMToggleButton(ThisMenu, 1, 1, "Dynamic \nBones", () =>
            {
                GlobalDynamicBones.UsersBones.Add(Utils.QuickMenu.SelectedVRCPlayer().UserID());
                PlayerExtensions.ReloadAvatar(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Disabled", () =>
            {
                GlobalDynamicBones.UsersBones.Remove(Utils.QuickMenu.SelectedVRCPlayer().UserID());
                PlayerExtensions.ReloadAvatar(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Enable Touch for this Player");

            HalfButton = new QMSingleButton(ThisMenu, 1, 2.25f, "Teleport \nItems", () =>
            {
                foreach (VRC_ObjectSync vrc_ObjectSync in ItemHandler.World_ObjectSyncs)
			    {
				    Networking.SetOwner(Utils.CurrentUser.field_Private_VRCPlayerApi_0, vrc_ObjectSync.gameObject);
				    vrc_ObjectSync.transform.rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);
				    vrc_ObjectSync.transform.position = Utils.QuickMenu.SelectedVRCPlayer().transform.position;
			    }
            }, "Teleport all Items to the Player");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 3, 1.75f, "Audio \nCrash", () =>
            {
                MelonCoroutines.Start(CameraHandler.TargetAvatarCrash(Utils.QuickMenu.SelectedPlayer(), "avtr_ce9c4c0a-f646-48c6-8856-a53b8d7b9bf4"));
            }, "Crash the Player with a Audiocrash Avatar");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 3, 2.25f, "Material \nCrash", () =>
            {
                MelonCoroutines.Start(CameraHandler.TargetAvatarCrash(Utils.QuickMenu.SelectedPlayer(), "avtr_72b727e2-40f9-4934-8c49-b8dfd545e2ab"));
            }, "Crash the Player with a Stack overflow Avatar");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMToggleButton(ThisMenu, 2, 2, "Attach", () =>
            {
                VRCPlayer instance = Utils.QuickMenu.SelectedVRCPlayer();
                AttachmentManager.SetAttachment(instance);
            }, "Disabled", () =>
            {
                AttachmentManager.Reset();
            }, "Attach on Head");

            HalfButton = new QMSingleButton(ThisMenu, 2, 0.25f, "Forceclone", () =>
            {
                VRCPlayer player = Utils.QuickMenu.SelectedVRCPlayer();
                if (player.GetAPIAvatar().releaseStatus != "private")
                {
                    PlayerExtensions.ChangeAvatar(player.GetAPIAvatar().id);
                    VRConsole.Log(VRConsole.LogsType.Avatar, $"Forceclone --> {player}");
                }
                else
                {
                    Logger.WengaLogger("This Avatar is Private");
                    VRConsole.Log(VRConsole.LogsType.Avatar, "Forceclone --> This Avatar is Private");
                }
            }, "Forceclone the Players public Avatar");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 1, 1.75f, "Draw \nCircles", () =>
            {
                MelonCoroutines.Start(ItemHandler.DrawCircle(Utils.QuickMenu.SelectedPlayer()));
            }, "Draw Circles around the Player");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            object ItemOrbitEnum2 = new object();
            new QMToggleButton(ThisMenu, 2, 1, "Item \nOrbit", () =>
            {
                ItemHandler.ItemOrbitEnabled = true;
                ItemOrbitEnum2 = MelonCoroutines.Start(ItemHandler.ItemRotate(Utils.QuickMenu.SelectedPlayer()));
            }, "Disabled", () =>
            {
                ItemHandler.ItemOrbitEnabled = false;
                MelonCoroutines.Stop(ItemOrbitEnum2);
            }, "Orbit Items around the Player");

            new QMToggleButton(ThisMenu, 4, 2, "Annoying \nSound", () =>
            {
                MelonCoroutines.Start(CameraHandler.AnnoyingCamera(Utils.QuickMenu.SelectedPlayer()));
            }, "Disabled", () =>
            {
                CameraHandler.UserCamAnnoy = false;
            }, "Play Annoying Camera Sounds for this Player");
        }
        public static List<string> HearOffPlayers = new List<string>();
    }
}
