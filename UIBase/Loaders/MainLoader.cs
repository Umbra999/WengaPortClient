using MelonLoader;
using System.Diagnostics;
using System;
using WengaPort.Loaders;
using WengaPort.Buttons;
using VRC.Core;
using WengaPort.Modules;
using UnityEngine;
using WengaPort.Wrappers;
using WengaPort.Extensions;
using VRC;
using VRCSDK2;
using System.Linq;
using System.Collections.Generic;
using DiscordRichPresence;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine.UI;
using Logger = WengaPort.Extensions.Logger;
using WengaPort.ConsoleUtils;
using UnhollowerRuntimeLib;

namespace WengaPort.MainLoader
{
    public class MainLoader : MelonMod
    {
        public static string Version = "1.0.0";
        public unsafe override void OnApplicationStart() // Runs after Game Initialization.
        {
            FoldersManager.Create.Initialize();
            OnStart();
            Console.Title = $"WengaPort";
            PatchManager.QuestIni();
            ClassInjector.RegisterTypeInIl2Cpp<Movement>();
            ClassInjector.RegisterTypeInIl2Cpp<LovenseRemote>();
            ClassInjector.RegisterTypeInIl2Cpp<PlayerList>();
            ClassInjector.RegisterTypeInIl2Cpp<ThirdPerson>();
            ClassInjector.RegisterTypeInIl2Cpp<AvatarFavs>();
            ClassInjector.RegisterTypeInIl2Cpp<AttachmentManager>();
            ClassInjector.RegisterTypeInIl2Cpp<NameplateHelper>();
        }

        public override void OnLevelIsLoading() // Runs when a Scene is Loading or when a Loading Screen is Shown. Currently only runs if the Mod is used in BONEWORKS.
        {

        }

        public override void OnLevelWasLoaded(int level) // Runs when a Scene has Loaded.
        {
            if (level == -1) GameObject.Find("UserInterface/MenuContent/Screens/Settings/OtherOptionsPanel/AllowAvatarCopyingToggle").GetComponent<UiSettingConfig>().SetEnable(false);
            VRConsole.AllLogsText.Clear();
            GlobalDynamicBones.currentWorldDynamicBoneColliders.Clear();
            GlobalDynamicBones.currentWorldDynamicBones.Clear();
            PlayerList.BlockList.Clear();
            new Thread(delegate ()
            {
                PatchManager.EventDelay = true;
                Thread.Sleep(9000);
                PatchManager.EventDelay = false;
            }).Start();
        }

        public override void OnLevelWasInitialized(int level) // Runs when a Scene has Initialized.
        {
            switch (level)
            {
                case 0:
                case 1:
                    break;
                default:
                    MelonCoroutines.Start(NoClipping.SetNearClipPlane(0.001f));
                    //if (APIUser.CurrentUser.id == "usr_3be96c4e-7ca2-477f-b351-c4358c294a0a")
                    //{
                    //    BotOrbit.StartBot();
                    //}
                    break;
            }

            RoomTime = 0f;
            ItemHandler.World_Pickups.Clear();
            ItemHandler.World_ObjectSyncs.Clear();
            ItemHandler.World_Triggers.Clear();
            ItemHandler.World_Mirrors.Clear();
            ItemHandler.World_Chairs.Clear();

            ItemHandler.World_ObjectSyncs = Resources.FindObjectsOfTypeAll<VRC_ObjectSync>().ToArray().ToList();
            ItemHandler.World_Triggers = UnityEngine.Object.FindObjectsOfType<VRC_Trigger>().ToArray().ToList();

            PedestalHandler.FetchPedestals();
            
            if (ItemHandler.ChairToggle)
            {
                foreach (var item in Resources.FindObjectsOfTypeAll<VRC.SDKBase.VRCStation>())
                {
                    if (item.gameObject.active)
                    {
                        item.gameObject.SetActive(false);
                        ItemHandler.World_Chairs.Add(item);
                    }
                }
            }
            if (ItemHandler.MirrorToggle)
            {
                foreach (var item in Resources.FindObjectsOfTypeAll<VRC.SDKBase.VRC_MirrorReflection>())
                {
                    if (item.gameObject.active)
                    {
                        item.gameObject.SetActive(false);
                        ItemHandler.World_Mirrors.Add(item);
                    }
                }
            }
            if (ItemHandler.ItemToggle)
            {
                foreach (VRC_Pickup item in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    item.gameObject.SetActive(false);
                    ItemHandler.World_Pickups.Add(item);
                }
            }
            if (ItemHandler.PickupToggle)
            {
                foreach (VRC_Pickup item in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    if (item.pickupable)
                    {
                        item.pickupable = false;
                        ItemHandler.World_Pickups.Add(item);
                    }
                }
            }
        }

        public override void OnUpdate() // Runs once per frame.
        {
            LoadingDelay += Time.deltaTime;
            if (LoadingDelay > 13f)
            {
                AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0.field_Private_Boolean_0 = false;
                LoadingDelay = 0f;
            }
            if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != true) ButtonsMainColor.Initialize3(); //Really scuffed but this is to disable the blue loading screen
            if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
            {
                RoomTime += Time.deltaTime;
                if (UnityEngine.XR.XRDevice.isPresent)
                {
                    Movement.QuickMenuFly();
                }
                else
                {
                    CameraHandler.Zoom();

                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.K))
                    {
                        if (!CameraHandler.DesktopCam)
                        {
                            CameraHandler.EnableDesktopCam();
                        }
                        else
                        {
                            CameraHandler.DisableDesktopCam();
                        }
                    }

                    else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        var r = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                        if (Physics.Raycast(r, out RaycastHit raycastHit))
                        {
                            VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = raycastHit.point;
                        }
                    }

                    else if (Input.GetKeyDown(KeyCode.Mouse2))
                    {
                        Modules.Photon.EmojiRPC(29);
                        Modules.Photon.EmoteRPC(3);
                    }

                    else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha0))
                    {
                        Resources.FindObjectsOfTypeAll<DebugLogGui>().First().visible = !Resources.FindObjectsOfTypeAll<DebugLogGui>().First().visible;
                    }
                }

                try
                {
                    inviteDot.GetComponent<Image>().color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time / 2f, 1f), 1f, 1f));
                    reqDot.GetComponent<Image>().color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time / 2f, 1f), 1f, 1f));
                    if (menu != null && menu.active && !button.interactable)
                    {
                        button.interactable = true;
                        button.m_Interactable = true;
                    }
                    if (Modules.Photon.DisconnectToggle)
                    {
                        DCDelay += Time.deltaTime;
                        if (DCDelay >= 0.2f)
                        {
                            Modules.Photon.DisconnectLobby();
                            DCDelay = 0f;
                        }
                    }
                    if (Modules.Photon.DebugSpamToggle)
                    {
                        //Modules.Photon.DebugSpam();
                        Modules.Photon.PortalDebugSpam();
                    }

                    if (CameraHandler.AnnoyingCam)
                    {
                        DCDelay += Time.deltaTime;
                        if (DCDelay >= 1.2f)
                        {
                            CameraHandler.PictureRPC(Utils.CurrentUser.GetVRC_Player());
                            DCDelay = 0f;
                        }
                    }

                    if (RoomCleaner.MirrorSpam)
                    {
                        delaySpamMirrors += Time.deltaTime;
                        if (delaySpamMirrors >= 0.7f)
                        {
                            RoomCleaner.SpamMirrors();
                            delaySpamMirrors = 0f;
                        }
                    }

                    if (CameraHandler.CameraLag)
                    {
                        CameraHandler.DesyncV3();
                    }

                    if (Modules.Photon.EmojiSpam)
                    {
                        int i = 29;
                        Modules.Photon.EmojiRPC(i);
                    }

                    if (ItemHandler.AutoDropItems)
                    {
                        DropDelay += Time.deltaTime;
                        if (DropDelay > 1f)
                        {
                            ItemHandler.DropItems();
                            DropDelay = 0f;
                        }
                    }
                    Delay += Time.deltaTime;
                    if (Delay > 4f)
                    {
                        DiscordManager.Update();
                        if (PortalHandler.kosstrings.Count > 0)
                        {
                            foreach (Player player in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray())
                            {
                                if (PortalHandler.kosstrings.Contains(player.UserID()))
                                {
                                    PortalHandler.DropInfinitePortal(player.GetVRCPlayer());
                                }
                            }
                        }
                        Delay = 0f;
                    }
                }
                catch
                { }
            }
        }
        public float Delay = 0f;
        public float PlateDelay = 0f;
        public float delaySpamMirrors = 0f;   
        public float DCDelay = 0f;
        public float DropDelay = 0f;
        public float LoadingDelay = 0f;
        public static float RoomTime = 0f;

        public override void OnFixedUpdate() // Can run multiple times per frame. Mostly used for Physics.
        {

        }

        public virtual void OnStart()
        {

        } 

        public override void OnLateUpdate() // Runs once per frame after OnUpdate and OnFixedUpdate have finished.
        {

        }

        public override void OnGUI()
        {
            if (!UnityEngine.XR.XRDevice.isPresent)
            {
                OnGui.GuiInit();
            }
        }

        public override void OnApplicationQuit() // Runs when the Game is told to Close.
        {
            PhotonMenu.Ini.SetBool("Toggles", "Exit", true);
            Api.ApiExtension.CloseWebsocket();
            DiscordManager.OnApplicationQuit();
            Process.GetCurrentProcess().Kill();
        }

        public override void OnModSettingsApplied() // Runs when Mod Preferences get saved to UserData/modprefs.ini.
        {

        }

        public override void VRChat_OnUiManagerInit() // Runs upon VRChat's UiManager Initialization. Only runs if the Mod is used in VRChat.
        {
            ButtonsLoader.Initialize();
            ModulesLoader.Initialize();
            DiscordManager.Init();
            PatchManager.JoinInitialize();
            PatchManager.InitPatch();
            VRCPlayer.field_Internal_Static_Color_0 = new Color(255, 0, 0);
            VRCPlayer.field_Internal_Static_Color_1 = new Color(255, 255, 0);
            VRCPlayer.field_Internal_Static_Color_2 = new Color(0, 0, 0);
            VRCPlayer.field_Internal_Static_Color_3 = new Color(0, 255, 255);
            VRCPlayer.field_Internal_Static_Color_4 = new Color(0, 255, 0);
            VRCPlayer.field_Internal_Static_Color_5 = new Color(0.92f, 0.37f, 0f);
            VRCPlayer.field_Internal_Static_Color_6 = new Color(222, 0, 128);
            VRCPlayer.field_Internal_Static_Color_7 = new Color(184, 86, 0);
            VRCPlayer.field_Internal_Static_Color_8 = new Color(255, 255, 255);
            VRCPlayer.field_Internal_Static_Color_9 = new Color(255, 255, 255);
            menu = GameObject.Find("/UserInterface/MenuContent/Screens/UserInfo/User Panel/OnlineFriend");
            button = GameObject.Find("/UserInterface/MenuContent/Screens/UserInfo/User Panel/OnlineFriend/Actions/Invite").GetComponent<Button>();
            inviteDot = GameObject.Find("/UserInterface/UnscaledUI/HudContent/Hud/NotificationDotParent/InviteDot");
            reqDot = GameObject.Find("/UserInterface/UnscaledUI/HudContent/Hud/NotificationDotParent/InviteRequestDot");
            var Client = new GameObject();
            UnityEngine.Object.DontDestroyOnLoad(Client);
            Client.AddComponent<Movement>();
            Client.AddComponent<LovenseRemote>();
            Client.AddComponent<PlayerList>();
            Client.AddComponent<ThirdPerson>();
            Client.AddComponent<AvatarFavs>();
            Client.AddComponent<AttachmentManager>();
            Client.AddComponent<NameplateHelper>();
        }
        GameObject menu;
        GameObject inviteDot;
        GameObject reqDot;
        Button button;
    }
}