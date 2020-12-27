using UnityEngine;
using MelonLoader;
using WengaPort.Api;
using WengaPort.Wrappers;
using WengaPort.Buttons;
using UnityEngine.UI;
using WengaPort.Modules;
using System;
using VRC;
using System.Linq;
using VRCSDK2;
using System.Collections.Generic;
using VRC.SDKBase;
using WengaPort.Extensions;

namespace WengaPort.Buttons
{
    class SafetyMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMToggleButton VRToggle;
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(MainMenu.ThisMenu, 1.5f, 1f, "Safety", "Safety Utils", null, null, null, Color.yellow);

            new QMToggleButton(ThisMenu, 1, 0, "Forcemute", () =>
            {
                try
                {
                    foreach (Player instance in Utils.PlayerManager.GetAllPlayers().ToArray())
                    {
                        InteractMenu.HearOffPlayers.Add(instance.GetVRCPlayer().UserID());
                        instance.field_Internal_VRCPlayer_0.field_Internal_Boolean_1 = false;
                    }
                }
                catch (Exception)
                {
                }
            }, "Disabled", () =>
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
            }, "Forcemute Everyone");

            new QMToggleButton(ThisMenu, 0, 0, "Mini \nHide", () =>
            {
                PatchManager.MiniHide = true;
                PlayerExtensions.ReloadAvatar(Utils.CurrentUser);
            }, "Disabled", () =>
            {
                PatchManager.MiniHide = false;
                PlayerExtensions.ReloadAvatar(Utils.CurrentUser);
            }, "Be a Mini hide Roboter");

            new QMToggleButton(ThisMenu, 2, 1, "Antiblock", () =>
            {
                PatchManager.AntiBlock = true;
            }, "Disabled", () =>
            {
                PatchManager.AntiBlock = false;
            }, "See blocked People", Color.cyan, Color.white, false, true);

            VRToggle = new QMToggleButton(ThisMenu, 5, 0, "VR", () =>
            {
                PatchManager.VRMode = true;
            }, "Desktop", () =>
            {
                PatchManager.VRMode = false;
            }, "Spoof yourself VR/Desktop");

            new QMToggleButton(ThisMenu, 4, 2, "Spoof \nFrames", () =>
            {
                PatchManager.FrameSpoof = true;
            }, "Disabled", () =>
            {
                PatchManager.FrameSpoof = false;
            }, "Spoof your FPS", Color.cyan, Color.white, false, true);

            new QMToggleButton(ThisMenu, 3, 2, "Spoof \nPing", () =>
            {
                PatchManager.PingSpoof = true;
            }, "Disabled", () =>
            {
                PatchManager.PingSpoof = false;
            }, "Spoof your Ping", Color.cyan, Color.white, false, true);

            new QMToggleButton(ThisMenu, 1, 2, "Disable \nChairs", () =>
            {
                ItemHandler.ChairToggle = true;
                var objects = Resources.FindObjectsOfTypeAll<VRCStation>();
                foreach (var item in objects)
                {
                    if (item.gameObject.active)
                    {
                        item.gameObject.SetActive(false);
                        ItemHandler.World_Chairs.Add(item);
                    }
                }
            }, "Disabled", () =>
            {
                ItemHandler.ChairToggle = false;
                foreach (var item in ItemHandler.World_Chairs)
                {
                    item.gameObject.SetActive(true);
                }
            }, "Disable all Chairs", Color.cyan, Color.white, false, true);

            List<VRC.SDKBase.VRC_MirrorReflection> WorldMirrors = new List<VRC.SDKBase.VRC_MirrorReflection>();
            new QMToggleButton(ThisMenu, 5, 1, "Disable \nMirror", () =>
            {
                ItemHandler.MirrorToggle = true;
                var objects = Resources.FindObjectsOfTypeAll<VRC.SDKBase.VRC_MirrorReflection>();
                foreach (var item in objects)
                {
                    if (item.gameObject.active)
                    {
                        item.gameObject.SetActive(false);
                        ItemHandler.World_Mirrors.Add(item);
                    }
                }
            }, "Disabled", () =>
            {
                ItemHandler.MirrorToggle = false;
                foreach (var item in ItemHandler.World_Mirrors)
                {
                    item.gameObject.SetActive(true);
                }
            }, "Disable all Mirrors");

            new QMToggleButton(ThisMenu, 2, 0, "Disable \nItems", () =>
            {
                ItemHandler.ItemToggle = true;
                foreach (VRCSDK2.VRC_Pickup item in UnityEngine.Object.FindObjectsOfType<VRCSDK2.VRC_Pickup>())
                {
                    ItemHandler.World_Pickups.Add(item);
                    item.gameObject.SetActive(false);
                }
            }, "Disabled", () =>
            {
                ItemHandler.ItemToggle = false;
                foreach (VRC.SDKBase.VRC_Pickup item in ItemHandler.World_Pickups)
                {
                    item.gameObject.SetActive(true);
                }
            }, "Disable all Items");

            new QMToggleButton(ThisMenu, 3, 0, "Anti \nPortal", () =>
            {
                PortalHandler.AntiPortal = true;
            }, "Disabled", () =>
            {
                PortalHandler.AntiPortal = false;
            }, "Delete all spawned Portals");

            new QMToggleButton(ThisMenu, 4, 0, "Anti \nTrigger", () =>
            {
                PatchManager.AntiWorldTrigger = true;
            }, "Disabled", () =>
            {
                PatchManager.AntiWorldTrigger = false;
            }, "Make all global Triggers local");

            new QMToggleButton(ThisMenu, 1, 1, "Anti \nMaster", () =>
            {
                PatchManager.AntiMasterDC = true;
            }, "Disabled", () =>
            {
                PatchManager.AntiMasterDC = false;
            }, "Advanced Anti Master Disconnect (will disable Health in Maps)");

            new QMToggleButton(ThisMenu, 3, 1, "Anti \nVideoplayer", () =>
            {
                PatchManager.BlockPlayer = true;
            }, "Disabled", () =>
            {
                PatchManager.BlockPlayer = false;
            }, "Prevents you from IP Logging via Players");

            new QMToggleButton(ThisMenu, 4, 1, "Disable \nPedestals", () =>
            {
                PedestalHandler.Disable();
            }, "Disabled", () =>
            {
                PedestalHandler.Revert();
            }, "Disable all Avatar Pedestals");

            new QMToggleButton(ThisMenu, 2, 2, "Anti \nUdon", () =>
            {
                PatchManager.AntiUdon = true;
            }, "Disabled", () =>
            {
                PatchManager.AntiUdon = false;
            }, "Prevents you from Udon Exploits/Events");
        }
    }
}
