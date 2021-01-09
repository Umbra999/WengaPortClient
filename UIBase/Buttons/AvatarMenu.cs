using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnityEngine;
using WengaPort.Api;
using WengaPort.Modules;
using WengaPort.Wrappers;

namespace WengaPort.Buttons
{
    class AvatarMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton HalfButton;
        public static QMToggleButton BlockToggle;
        public static QMToggleButton BonesToggle;
        public static QMToggleButton ShaderToggle;              
        public static QMToggleButton ParticleToggle;
        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(InteractMenu.ThisMenu, 1, 1, "Avatar \nUtils", "Avatar Settings", null, null, null, Color.yellow);

            ShaderToggle = new QMToggleButton(ThisMenu, 1, 0, "Disable \nShaders", () =>
            {
                GlobalDynamicBones.DisabledShaders.Add(Utils.QuickMenu.SelectedVRCPlayer().UserID());
                PlayerExtensions.ReloadAvatar(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Disabled", () =>
            {
                GlobalDynamicBones.DisabledShaders.Remove(Utils.QuickMenu.SelectedVRCPlayer().UserID());
                PlayerExtensions.ReloadAvatar(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Disable the Player's Shader");

            BonesToggle = new QMToggleButton(ThisMenu, 2, 0, "Dynamic \nBones", () =>
            {
                GlobalDynamicBones.UsersBones.Add(Utils.QuickMenu.SelectedVRCPlayer().UserID());
                PlayerExtensions.ReloadAvatar(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Disabled", () =>
            {
                GlobalDynamicBones.UsersBones.Remove(Utils.QuickMenu.SelectedVRCPlayer().UserID());
                PlayerExtensions.ReloadAvatar(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Enable Touch for this Player");

            BlockToggle = new QMToggleButton(ThisMenu, 3, 0, "Silent \nBlock", () =>
            {
                string id = QuickMenu.prop_QuickMenu_0.field_Private_APIUser_0.id;
                Utils.QuickMenu.SelectedVRCPlayer().gameObject.SetActive(false);
                PortalHandler.blockstrings.Add(id);
            }, "Disabled", () =>
            {
                string id = QuickMenu.prop_QuickMenu_0.field_Private_APIUser_0.id;
                Utils.QuickMenu.SelectedVRCPlayer().gameObject.SetActive(true);
                PortalHandler.blockstrings.Remove(id);
            }, "Block the Player local");

            ParticleToggle = new QMToggleButton(ThisMenu, 4, 0, "Disable \nParticles", () =>
            {
                GlobalDynamicBones.DisabledParticles.Add(Utils.QuickMenu.SelectedVRCPlayer().UserID());
                PlayerExtensions.ReloadAvatar(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Disabled", () =>
            {
                GlobalDynamicBones.DisabledParticles.Remove(Utils.QuickMenu.SelectedVRCPlayer().UserID());
                PlayerExtensions.ReloadAvatar(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Disable the Player's Particles");

            new QMSingleButton(ThisMenu, 1, 1, "Reload \nAvatar", () =>
            {
                PlayerExtensions.ReloadAvatar(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Reload the Player's Avatar Avatar");

            new QMToggleButton(ThisMenu, 2, 1, "Disable \nAudio", () =>
            {
                GlobalDynamicBones.DisabledAudios.Add(Utils.QuickMenu.SelectedVRCPlayer().UserID());
                PlayerExtensions.ReloadAvatar(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Disabled", () =>
            {
                GlobalDynamicBones.DisabledAudios.Remove(Utils.QuickMenu.SelectedVRCPlayer().UserID());
                PlayerExtensions.ReloadAvatar(Utils.QuickMenu.SelectedVRCPlayer());
            }, "Disable the Player's Audio Sources");

            new QMSingleButton(ThisMenu, 3, 1, "Reupload \nAvatar", () =>
            {
                Modules.Reupload.ReuploaderButtons.ReuploadAvatar(Utils.QuickMenu.SelectedVRCPlayer().GetAPIAvatar().id);
            }, "Reupload the Player's Avatar");

            new QMSingleButton(ThisMenu, 4, 1, "Copy \nAvatarID", () =>
            {
                Clipboard.SetText(Utils.QuickMenu.SelectedVRCPlayer().GetAPIAvatar().id);
            }, "Copy the Player's AvatarID to Clipboard");
        }
    }
}
