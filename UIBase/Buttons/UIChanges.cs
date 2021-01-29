﻿using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using WengaPort.Modules;
using WengaPort.Wrappers;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace WengaPort.Buttons
{
    class UIChanges
    {
        public static IEnumerator UpdateClock()
        {
            for (; ; )
            {
                while (VRCPlayer.field_Internal_Static_VRCPlayer_0 == null) yield return null;
                var Shortcut = GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu");
                if (Shortcut.gameObject.active == true)
                {
                    var InfoBar = GameObject.Find("/UserInterface/QuickMenu/QuickMenu_NewElements/_CONTEXT/QM_Context_User_Hover/_UserStatus/Text");
                    TimeSpan span = TimeSpan.FromSeconds((double)new decimal(MainLoader.MainLoader.RoomTime));
                    string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
                    InfoBar.GetComponent<Text>().text = $"{DateTime.Now.ToLongTimeString()} - Room: {elapsedTime} - FPS: {Mathf.Round(1f / Time.deltaTime)}";
                }
                yield return new WaitForSeconds(1f);
            }
        }
        public static Vector2 GetButtonPosition(float x, float y)
        {
            return new Vector2(-1050f + x * 420f, 1570f + y * -420f);
        }
        public static void SetQuickMenuCollider(float x, float y)
        {
            Utils.QuickMenu.GetComponent<BoxCollider>().size += new Vector3(x * 840, y * 840);
        }

        public static void SetSizeButtonfor(GameObject Button, float xSize, float ySize)
        {
            Button.GetComponent<RectTransform>().sizeDelta /= new Vector2(xSize, ySize);
        }

        public static IEnumerator Initialize()
        {
            UnityEngine.Application.targetFrameRate = 144;
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;
            while (VRCPlayer.field_Internal_Static_VRCPlayer_0 == null) yield return null;
            SetQuickMenuCollider(0f,0.5f);
            var FirstPanel = GameObject.Find("/UserInterface/QuickMenu/QuickMenu_NewElements/_Background/Panel");
            var SecondPanel = GameObject.Find("/UserInterface/QuickMenu/QuickMenu_NewElements/_CONTEXT/QM_Context_ToolTip/Panel");
            var ThirdPanel = GameObject.Find("/UserInterface/QuickMenu/QuickMenu_NewElements/_CONTEXT/QM_Context_User_Hover/Panel");
            var FourthPanel = GameObject.Find("/UserInterface/QuickMenu/QuickMenu_NewElements/_InfoBar");
            var FivePanel = GameObject.Find("/UserInterface/MenuContent/Backdrop/Backdrop/Background");
            var SixPanel = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked");
            var SevenPanel = GameObject.Find("/UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort/Content/SafetyPageTab/Image (1)");
            var EightPanel = GameObject.Find("/UserInterface/MenuContent/Backdrop/Backdrop/Image");
            var NinePanel = GameObject.Find("/UserInterface/MenuContent/Screens/Settings/Button_AdvancedOptions/Image_NEW");
            var TenPanel = GameObject.Find("/UserInterface/MenuContent/Screens/Settings_Safety/TitlePanel/Button_PerformanceOptions/Image_NEW");
            var ElevenPanel = GameObject.Find("/UserInterface/MenuContent/Screens/Settings/Button_EditBindings/Image_NEW");
            var settingsTitleText = GeneralWrappers.GetVRCUiManager().menuContent.transform.Find("Screens/Settings/TitlePanel/TitleText");
            var TitleText = GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/TitlePanel (1)/TitleText");
            var EarlyAccess = Utils.QuickMenu.transform.Find("ShortcutMenu/EarlyAccessText");
            var PingText = Utils.QuickMenu.transform.Find("ShortcutMenu/PingText").gameObject;
            var FrameText = Utils.QuickMenu.transform.Find("ShortcutMenu/FPSText").gameObject;
            var WorldText = Utils.QuickMenu.transform.Find("ShortcutMenu/WorldText").gameObject;
            var BuildText = Utils.QuickMenu.transform.Find("ShortcutMenu/BuildNumText").gameObject;
            var NameText = Utils.QuickMenu.transform.Find("ShortcutMenu/NameText").gameObject;
            var Ads1 = Utils.QuickMenu.transform.Find("ShortcutMenu/VRCPlusMiniBanner").gameObject;
            var Ads2 = Utils.QuickMenu.transform.Find("ShortcutMenu/VRCPlusThankYou").gameObject;
            var Ads3 = Utils.QuickMenu.transform.Find("ShortcutMenu/UserIconButton").gameObject;
            var Ads4 = Utils.QuickMenu.transform.Find("ShortcutMenu/UserIconCameraButton").gameObject;
            var Ads5 = Utils.QuickMenu.transform.Find("ShortcutMenu/HeaderContainer/VRCPlusBanner").gameObject;
            var QMReportWorldButton = Utils.QuickMenu.transform.Find("ShortcutMenu/ReportWorldButton").gameObject;
            GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud/VoiceDotParent/VoiceDotDisabled").GetComponent<FadeCycleEffect>().enabled = false;
            GameObject.Find("UserInterface/PlayerDisplay/BlackFade/inverted_sphere").SetActive(false);
            GameObject QMSafetyButton = Utils.QuickMenu.transform.Find("ShortcutMenu/SafetyButton").gameObject;
            GameObject QMSettingsButton = Utils.QuickMenu.transform.Find("ShortcutMenu/SettingsButton").gameObject;
            GameObject QMAvatarButton = Utils.QuickMenu.transform.Find("ShortcutMenu/AvatarButton").gameObject;
            GameObject QMSocialButton = Utils.QuickMenu.transform.Find("ShortcutMenu/SocialButton").gameObject;
            GameObject QMCameraButton = Utils.QuickMenu.transform.Find("ShortcutMenu/CameraButton").gameObject;
            GameObject QMUIElementsButton = Utils.QuickMenu.transform.Find("ShortcutMenu/UIElementsButton").gameObject;
            GameObject QMEmoteButton = Utils.QuickMenu.transform.Find("ShortcutMenu/EmoteButton").gameObject;
		    GameObject QMEmojiButton = Utils.QuickMenu.transform.Find("ShortcutMenu/EmojiButton").gameObject;
            GameObject QMWorldsButton = Utils.QuickMenu.transform.Find("ShortcutMenu/WorldsButton").gameObject;
            GameObject QMCalibrateButton = Utils.QuickMenu.transform.Find("ShortcutMenu/CalibrateButton").gameObject;
            GameObject QMSitButton = Utils.QuickMenu.transform.Find("ShortcutMenu/SitButton").gameObject;
            GameObject QMRankToggle = Utils.QuickMenu.transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors").gameObject;
            GameObject QMHomeButton = Utils.QuickMenu.transform.Find("ShortcutMenu/GoHomeButton").gameObject;
            GameObject QMRespawnButton = Utils.QuickMenu.transform.Find("ShortcutMenu/RespawnButton").gameObject;
            GameObject InteractReport = Utils.QuickMenu.transform.Find("UserInteractMenu/ReportAbuseButton").gameObject;
            var MicButton = GameObject.Find("/UserInterface/QuickMenu/MicControls");
            var MicTextButton = GameObject.Find("/UserInterface/QuickMenu/MicControls/MuteText");
            var UIElementsMenu = GameObject.Find("/UserInterface/QuickMenu/UIElementsMenu");
            var NewElementsMenu = GameObject.Find("/UserInterface/QuickMenu/QuickMenu_NewElements");
            GameObject QMHeaderContainer = Utils.QuickMenu.transform.Find("ShortcutMenu/HeaderContainer").gameObject;

            Vector2 ButtonSize = QMSafetyButton.GetComponent<RectTransform>().sizeDelta / new Vector2(1f, 2.0175f);
            if (QMSafetyButton.GetComponent<RectTransform>().sizeDelta != ButtonSize)
            {
                SetSizeButtonfor(QMSafetyButton, 1f, 2.0175f);
                QMSafetyButton.transform.localPosition = GetButtonPosition(5f, -0.5f);         
            }

            if (QMSettingsButton.GetComponent<RectTransform>().sizeDelta != ButtonSize)
            {
                SetSizeButtonfor(QMSettingsButton, 1f, 2.0175f);
                QMSettingsButton.transform.localPosition = GetButtonPosition(4f, -0.5f);
            }

            SetSizeButtonfor(QMEmojiButton, 1f, 2.0175f);
            SetSizeButtonfor(QMEmoteButton, 1f, 2.0175f);
            SetSizeButtonfor(QMCameraButton, 1f, 2.0175f);
            SetSizeButtonfor(QMUIElementsButton, 1f, 2.0175f);
            SetSizeButtonfor(QMSocialButton, 1f, 2.0175f);
            SetSizeButtonfor(QMAvatarButton, 1f, 2.0175f);
            SetSizeButtonfor(QMWorldsButton, 1f, 2.0175f);
            SetSizeButtonfor(QMHomeButton, 1f, 2.0175f);
            SetSizeButtonfor(QMRespawnButton, 1f, 2.025f);
            QMEmoteButton.transform.localPosition = GetButtonPosition(4f, 2.20f);
            QMEmojiButton.transform.localPosition = GetButtonPosition(3f, 2.20f);
            QMCameraButton.transform.localPosition = GetButtonPosition(2f, 2.20f);
            QMUIElementsButton.transform.localPosition = GetButtonPosition(1f, 2.20f);
            QMWorldsButton.transform.localPosition = GetButtonPosition(1f, -0.5f);
            QMSocialButton.transform.localPosition = GetButtonPosition(3f, -0.5f);
            QMAvatarButton.transform.localPosition = GetButtonPosition(2f, -0.5f);
            QMCalibrateButton.transform.localPosition = GetButtonPosition(5, 2.25f);
            QMSitButton.transform.localPosition = GetButtonPosition(5, 2.25f);
            QMRankToggle.transform.localPosition = GetButtonPosition(5, -1.25f);
            NewElementsMenu.transform.localPosition = GetButtonPosition(2.25f, 3f);
            QMHomeButton.transform.localPosition = GetButtonPosition(0, -0.5f);
            QMRespawnButton.transform.localPosition = GetButtonPosition(0f, 2f);
            MicButton.transform.localPosition = GetButtonPosition(0.25f, 4.7f);
            QMHeaderContainer.transform.localPosition = GetButtonPosition(2.5f, -1.75f);
            EarlyAccess.transform.localPosition = GetButtonPosition(1.45f, 2.55f);
            BuildText.transform.localPosition = GetButtonPosition(2.4f, 2.55f);
            WorldText.transform.localPosition = GetButtonPosition(1.75f, 2.75f);
            var UserDetailsButton = Utils.QuickMenu.transform.Find("UserInteractMenu/DetailsButton").gameObject;
            UserDetailsButton.GetComponent<RectTransform>().sizeDelta /= new Vector2(2f, 1f);
            UserDetailsButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(210f, 0f);

            settingsTitleText.GetComponent<Text>().color = Color.blue;
            settingsTitleText.GetComponent<Text>().text = "WengaPort";
            TitleText.GetComponent<Text>().color = Color.blue;
            TitleText.GetComponent<Text>().text = "WengaPort";
            EarlyAccess.GetComponent<Text>().text = "WengaPort";

            Object.Destroy(Ads1);
            Object.Destroy(Ads2);
            Object.Destroy(Ads5);
            Object.Destroy(InteractReport);
            Object.Destroy(MicTextButton);
            Object.Destroy(FirstPanel);
            Object.Destroy(SecondPanel);
            Object.Destroy(ThirdPanel);
            Object.Destroy(FivePanel);
            Object.Destroy(SixPanel);
            Object.Destroy(SevenPanel);
            Object.Destroy(EightPanel);
            Object.Destroy(NinePanel);
            Object.Destroy(TenPanel);
            Object.Destroy(ElevenPanel);
            Object.Destroy(QMReportWorldButton);
            Object.Destroy(FrameText);
            Object.Destroy(PingText);
            Object.Destroy(NameText);
            Ads3.transform.SetParent(UIElementsMenu.transform);
            Ads4.transform.SetParent(UIElementsMenu.transform);
            FourthPanel.transform.localScale = new Vector3(0, 0, 0);
            FourthPanel.transform.position = new Vector3(999 * 999, 999 * 999, 999 * 999);
            var cursorManager = VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0;
            var leftHand = cursorManager.handLeftCursor;
            var rightHand = cursorManager.handRightCursor;
            AdjustParticleSystems(leftHand.gameObject);
            AdjustParticleSystems(rightHand.gameObject);
            Console.Title = "WengaPort";
        }
        internal static void AdjustParticleSystems(GameObject cursorRoot)
        {
            var endFlare = cursorRoot.transform.Find("plasma_beam_flare_blue");
            var endSparks = endFlare.Find("plasma_beam_spark_002");
            var startParticle = cursorRoot.transform.Find("plasma_beam_muzzle_blue");
            endSparks.GetComponent<ParticleSystem>().enableEmission = false;
            startParticle.GetComponent<ParticleSystem>().enableEmission = false;
        }
    }
}
