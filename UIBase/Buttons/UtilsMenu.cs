using WengaPort.Api;
using WengaPort.Wrappers;
using UnityEngine;
using UnityEngine.UI;
using WengaPort.Modules;
using System;
using VRC.UI;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRCSDK2;
using WengaPort.Extensions;
using RootMotion.FinalIK;
using System.Windows.Forms;
using static WengaPort.Buttons.SocialMenu;
using System.Linq;
using Logger = WengaPort.Extensions.Logger;

namespace WengaPort.Buttons
{
    class UtilsMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton HalfButton;   

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(MainMenu.ThisMenu, 2.5f, 1f, "Utils", "Util Menu", null, null, null, Color.yellow);

            new QMToggleButton(ThisMenu, 1, 0, "Selfhide", () =>
            {
                SelfHide.Initialize(true);
            }, "Disabled", () =>
            {
                SelfHide.Initialize(false);
            }, "Hide yourself");

            new QMToggleButton(ThisMenu, 2, 0, "Headlight", () =>
            {
                Headlight.Toggle(true);
            }, "Disabled", () =>
            {
                Headlight.Toggle(false);
            }, "Toggle Headlight");

            new QMToggleButton(ThisMenu, 3, 0, "World \nTrigger", () =>
            {
                PatchManager.WorldTrigger = true;
            }, "Disabled", () =>
            {
                PatchManager.WorldTrigger = false;
            }, "Make all Triggers Global");

            new QMToggleButton(ThisMenu, 4, 0, "Serialize", () =>
            {
                Modules.Photon.CustomSerialize(true);
            }, "Disabled", () =>
            {
                Modules.Photon.CustomSerialize(false);
            }, "Dont send any Movement");

            new QMToggleButton(ThisMenu, 2, 1, "Offline", () =>
            {
                PatchManager.OfflineMode = true;
            }, "Online", () =>
            {
                PatchManager.OfflineMode = true;
            }, "Spoof yourself Offline");

            new QMToggleButton(ThisMenu, 3, 1, "Lock \nInstance", () =>
            {
                Modules.Photon.LockInstance = true;
            }, "Disabled", () =>
            {
                Modules.Photon.LockInstance = false;
            }, "Lock the Instance as Master");

            HalfButton = new QMSingleButton(ThisMenu, 3, 2.25f, "Portal \nby ID", () =>
            {
                string ID = Clipboard.GetText();
                if (ID.Contains("wrld_"))
                {
                    PortalHandler.DropPortal(ID);
                }
            }, "Drop Portal by ID");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 3, 1.75f, "Rejoin", () =>
            {
                var CurrentRoom = RoomManager.field_Internal_Static_ApiWorld_0.id + ":" + RoomManager.field_Internal_Static_ApiWorldInstance_0.idWithTags;
                Networking.GoToRoom(CurrentRoom);
            }, "Rejoin the current World");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 1, 2.25f, "Clear \nDrawings", () =>
            {
                RoomCleaner.CleanRoom();
            }, "Clear all Drawings");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 2, 2.25f, "Respawn \nItems", () =>
            {
                foreach (VRC_ObjectSync vrc_ObjectSync in ItemHandler.World_ObjectSyncs)
                {
                    Networking.SetOwner(Utils.CurrentUser.field_Private_VRCPlayerApi_0, vrc_ObjectSync.gameObject);
                    vrc_ObjectSync.Respawn();
                }
            }, "Respawn all Items");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 2, 1.75f, "Force \nChange", () =>
            {
                string ID = Clipboard.GetText();
                if (ID.Contains("avtr_"))
                {
                    PlayerExtensions.ChangeAvatar(ID);
                }
            }, "Change into AvatarID");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            HalfButton = new QMSingleButton(ThisMenu, 1, 1.75f, "Bring\nItems", () =>
            {
                ItemHandler.BringPickups();
            }, "Bring all Pickups to you");
            HalfButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMToggleButton(ThisMenu, 1, 1, "ESP", () =>
            {
                ESP.ESPEnabled = true;
            }, "Disabled", () =>
            {
                ESP.ESPEnabled = false;
            }, "Toggle ESP bubbles");

            new QMToggleButton(ThisMenu, 4, 2, "Global \nDynbones", () =>
            {
                GlobalDynamicBones.GlobalBones = true;
                foreach (Player player in Utils.PlayerManager.GetAllPlayers())
                {
                    PlayerExtensions.ReloadAvatar(player);
                }
            }, "Disabled", () =>
            {
                GlobalDynamicBones.GlobalBones = false;
                foreach (Player player in Utils.PlayerManager.GetAllPlayers())
                {
                    PlayerExtensions.ReloadAvatar(player);
                }
            }, "Toggle Dynamic bones for everyone");

            new QMToggleButton(ThisMenu, 4, 1, "Friend \nDynbones", () =>
            {
                GlobalDynamicBones.FriendBones = true;
                foreach (string player in GlobalDynamicBones.FriendOnlyBones)
                {
                    var P = Utils.PlayerManager.GetPlayer(player);
                    PlayerExtensions.ReloadAvatar(P);
                }
            }, "Disabled", () =>
            {
                GlobalDynamicBones.FriendBones = false;
                foreach (string player in GlobalDynamicBones.FriendOnlyBones)
                {
                    var P = Utils.PlayerManager.GetPlayer(player);
                    PlayerExtensions.ReloadAvatar(P);
                }
            }, "Toggle Dynamic bones for Friends Only", Color.cyan, Color.white, false, true);
        }
    }
}
