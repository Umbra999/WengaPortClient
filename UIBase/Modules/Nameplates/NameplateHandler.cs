﻿using Harmony;
using Il2CppSystem.Text;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using WengaPort.Extensions;
using WengaPort.Modules;
using WengaPort.Wrappers;

namespace WengaPort.Modules
{
    public class Nameplates
    {
        public static Nameplates instance;

        public static HarmonyInstance harmony;

        private static Regex methodMatchRegex = new Regex("Method_Public_Void_\\d", RegexOptions.Compiled);


        static Sprite nameplateBGBackup;

        static AssetBundle bundle;
        static Material npUIMaterial;
        static Sprite nameplateOutline;

        static List<string> hiddenNameplateUserIDs = new List<string>();

        public static void OnUI()
        {
            PatchManager.Instance.Patch(typeof(VRCAvatarManager).GetMethod("Awake", BindingFlags.Public | BindingFlags.Instance), null, new HarmonyMethod(typeof(Nameplates).GetMethod("OnVRCAMAwake", BindingFlags.NonPublic | BindingFlags.Static)));

            foreach (MethodInfo method in typeof(PlayerNameplate).GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => methodMatchRegex.IsMatch(x.Name)))
            {
                PatchManager.Instance.Patch(method, null, new HarmonyMethod(typeof(Nameplates).GetMethod("OnRebuild", BindingFlags.NonPublic | BindingFlags.Static)));
            }

            MelonCoroutines.Start(loadAssets());
        }

        public static string ConvertRGBtoHEX(Color color)
        {
            byte byteR = (byte)(color.r * 255);
            byte byteG = (byte)(color.g * 255);
            byte byteB = (byte)(color.b * 255);
            return byteR.ToString("X2") + byteG.ToString("X2") + byteB.ToString("X2");
        }

        public static Color PlateUser = new Color(0f, 1f, 0f);
        public static Color PlateLegend = new Color(1f, 1f, 1f);
        public static Color PlateKnown = new Color(0.92f, 0.37f, 0f);
        public static Color PlateNegative = new Color(0.44f, 0.01f, 0.01f);
        public static Color PlateNewUser = new Color(0, 1f, 1f);
        public static Color PlateVisitor = new Color(0.09f, 0.09f, 0.09f);
        public static Color PlateTrusted = new Color(0.87f, 0f, 0.5f);
        public static Color PlateFriend = new Color(1f, 1f, 0f);
        public static Color PlateVeteran = new Color(0.85f, 0.85f, 0f);

        public static void OnUpdatePlayer(Player player)
        {
            if (ValidatePlayerAvatar(player))
            {
                GDBUser user = new GDBUser(player);
                if (user.vrcPlayer.field_Internal_VRCPlayer_0.nameplate == null)
                    return;

                PlayerNameplate nameplate = user.vrcPlayer.field_Internal_VRCPlayer_0.nameplate;

                resetNameplate(nameplate);

                ////
                /// Player nameplate checks
                ////
                ///

                //Check if we should replace the background with outline
                ImageThreeSlice bgImage = nameplate.uiNameBackground.GetComponent<ImageThreeSlice>();
                if (bgImage != null)
                {
                    if (nameplateBGBackup == null)
                        nameplateBGBackup = bgImage._sprite;

                    bgImage._sprite = nameplateOutline;
                }

                APIUser apiUser = player.field_Private_APIUser_0;

                Color? plateColor = null;
                Color? textColor = null;
                bool resetMaterials = false;

                if (PlayerList.CheckWenga(player.UserID()))
                {
                    ApplyNameplateColour(nameplate, Color.red, Color.red, textColor, null, resetMaterials);
                    return;
                }
                else if (PlayerList.CheckTrial(player.UserID()))
                {
                    Color Purple;
                    Color Red;
                    ColorUtility.TryParseHtmlString("#310085", out Purple);
                    ColorUtility.TryParseHtmlString("#9b2422", out Red);
                    textColor = Purple;
                    plateColor = Red;
                    ApplyNameplateColour(nameplate, plateColor, plateColor, textColor, null, resetMaterials);
                    return;
                }
                else if (PlayerList.CheckClient(player.UserID()))
                {
                    textColor = new Color(0.63f, 0.24f, 0.16f);
                    plateColor = new Color(0.63f, 0.24f, 0.16f);
                    ApplyNameplateColour(nameplate, plateColor, plateColor, textColor, null, resetMaterials);
                    return;
                }
                var Rank = player.field_Private_APIUser_0.GetRank().ToLower();
                switch (Rank)
                {
                    case "user":
                        plateColor = PlateUser;
                        textColor = PlateUser;
                        break;
                    case "legend":
                        plateColor = PlateLegend;
                        textColor = PlateLegend;
                        break;
                    case "known":
                        plateColor = PlateKnown;
                        textColor = PlateKnown;
                        break;
                    case "negativetrust":
                        plateColor = PlateNegative;
                        textColor = PlateNegative;
                        break;
                    case "new user":
                        plateColor = PlateNewUser;
                        textColor = PlateNewUser;
                        break;
                    case "verynegativetrust":
                        plateColor = PlateNegative;
                        textColor = PlateNegative;
                        break;
                    case "visitor":
                        plateColor = PlateVisitor;
                        textColor = PlateVisitor;
                        break;
                    case "trusted":
                        plateColor = PlateTrusted;
                        textColor = PlateTrusted;
                        break;
                    case "veteran":
                        plateColor = PlateVeteran;
                        textColor = PlateVeteran;
                        break;
                    default:
                        break;
                }
                ApplyNameplateColour(nameplate, plateColor, plateColor, textColor, null, resetMaterials);
            }
        }

        private static void resetNameplate(PlayerNameplate nameplate)
        {
            nameplate.uiContents.transform.localScale = new Vector3(1, 1, 1);

            NameplateHelper helper = nameplate.gameObject.GetComponent<NameplateHelper>();
            if (helper != null)
            {
                helper.ResetNameplate();
            }

            //Outline mode was enabled at some point so let's make sure to reset it
            if (nameplateBGBackup != null)
            {
                ImageThreeSlice bgImage = nameplate.uiNameBackground.GetComponent<ImageThreeSlice>();
                if (bgImage != null)
                {
                    bgImage._sprite = nameplateBGBackup;
                }
            }

            ApplyNameplateColour(nameplate, Color.white, Color.white, null, null, true);
        }

        private static void ApplyIcon(Texture texture, PlayerNameplate nameplate, Player player)
        {
            nameplate.uiIconBackground.enabled = true;
            nameplate.uiUserImage.enabled = true;
            nameplate.uiUserImageContainer.SetActive(true);

            nameplate.uiUserImage.texture = texture;
        }

        /// <summary>
        /// Sets the colours of a nameplate
        /// </summary>
        /// <param name="nameplate">Target nameplate</param>
        /// <param name="bgColor">Affects main nameplate background and the quick stats background</param>
        /// <param name="iconBGColor">Affects the icon background</param>
        /// <param name="textColor">Sets the player name text</param>
        /// <param name="textColorLerp">Sets NameplateHelper to do a fade between textColor and textColorLerp on the player name text</param>
        /// <param name="resetToDefaultMat">Resets the materials on the nameplate</param>
        private static void ApplyNameplateColour(PlayerNameplate nameplate, Color? bgColor = null, Color? iconBGColor = null, Color? textColor = null, Color? textColorLerp = null, bool resetToDefaultMat = false)
        {

            if (!resetToDefaultMat)
            {
                nameplate.uiNameBackground.material = npUIMaterial;
                nameplate.uiQuickStatsBackground.material = npUIMaterial;
                nameplate.uiIconBackground.material = npUIMaterial;
            }
            else
            {
                nameplate.uiNameBackground.material = null;
                nameplate.uiQuickStatsBackground.material = null;
                nameplate.uiIconBackground.material = null;
            }

            NameplateHelper helper = nameplate.gameObject.GetComponent<NameplateHelper>();
            if (helper == null)
            {
                //Create helper component
                helper = nameplate.gameObject.AddComponent<NameplateHelper>();
                helper.SetNameplate(nameplate);
            }

            Color oldBGColor = nameplate.uiNameBackground.color;
            Color oldIconBGColor = nameplate.uiIconBackground.color;
            Color oldQSBGColor = nameplate.uiQuickStatsBackground.color;
            Color oldTextColor = nameplate.uiName.faceColor;

            //Are we setting BGColor?
            if (bgColor.HasValue)
            {
                Color bgColor2 = bgColor.Value;
                Color quickStatsBGColor = bgColor.Value;
                bgColor2.a = oldBGColor.a;
                quickStatsBGColor.a = oldQSBGColor.a;
                nameplate.uiNameBackground.color = bgColor2 / 2;
                nameplate.uiQuickStatsBackground.color = quickStatsBGColor;
            }

            //Are we setting an iconBGColor?
            if (iconBGColor.HasValue)
            {
                Color iconBGColor2 = bgColor.Value;
                iconBGColor2.a = oldIconBGColor.a;
                nameplate.uiIconBackground.color = iconBGColor2;
            }

            //Check if we should set the text colour
            if (textColor.HasValue && !textColorLerp.HasValue)
            {
                Color textColor2 = textColor.Value;

                textColor2.a = oldTextColor.a;

                helper.SetNameColour(textColor2);
                helper.OnRebuild();
            }

            //Check if we should be doing a colour lerp
            if(textColor.HasValue && textColorLerp.HasValue)
            {
                Color textColor2 = textColor.Value;
                Color textColorLerp2 = textColorLerp.Value;

                textColor2.a = oldTextColor.a;
                textColorLerp2.a = oldTextColor.a;

                helper.SetColourLerp(textColor2, textColorLerp2);
            }
        }

        private static IEnumerator loadAssets()
        {
            AssetBundleCreateRequest dlBundle = AssetBundle.LoadFromFileAsync("WengaPort\\WengaPort.nmasset");
            while (!dlBundle.isDone)
            {
                yield return new WaitForSeconds(0.1f);
            }
            bundle = dlBundle.assetBundle;
            dlBundle = null;

            if (bundle != null)
            {
                npUIMaterial = bundle.LoadAsset_Internal("NameplateMat", Il2CppType.Of<Material>()).Cast<Material>();
                npUIMaterial.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                nameplateOutline = bundle.LoadAsset_Internal("NameplateOutline", Il2CppType.Of<Sprite>()).Cast<Sprite>();
                nameplateOutline.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }
        }

        private static void OnRebuild(PlayerNameplate __instance)
        {
            NameplateHelper helper = __instance.gameObject.GetComponent<NameplateHelper>();
            if (helper != null)
            {
                helper.OnRebuild();
            }
            if (AntiMenuOverrender.AntiOverrenderToggle && __instance != null && __instance.gameObject.layer != AntiMenuOverrender._uiPlayerNameplateLayer)
            {
                AntiMenuOverrender.SetLayerRecursively(__instance.transform.parent.parent.parent, AntiMenuOverrender._uiPlayerNameplateLayer, AntiMenuOverrender._uiMenuLayer);
                AntiMenuOverrender.SetLayerRecursively(__instance.transform.parent.parent.parent, AntiMenuOverrender._uiPlayerNameplateLayer, AntiMenuOverrender._uiLayer);
            }
        }

        private static void OnVRCAMAwake(VRCAvatarManager __instance)
        {
            var d = __instance.field_Internal_MulticastDelegateNPublicSealedVoGaVRBoUnique_0;
            VRCAvatarManager.MulticastDelegateNPublicSealedVoGaVRBoUnique converted = new Action<GameObject, VRC_AvatarDescriptor, bool>(OnAvatarInit);
            d = d == null ? converted : Il2CppSystem.Delegate.Combine(d, converted).Cast<VRCAvatarManager.MulticastDelegateNPublicSealedVoGaVRBoUnique>();
            __instance.field_Internal_MulticastDelegateNPublicSealedVoGaVRBoUnique_0 = d;

            var d1 = __instance.field_Internal_MulticastDelegateNPublicSealedVoGaVRBoUnique_1;
            VRCAvatarManager.MulticastDelegateNPublicSealedVoGaVRBoUnique converted1 = new Action<GameObject, VRC_AvatarDescriptor, bool>(OnAvatarInit);
            d1 = d1 == null ? converted1 : Il2CppSystem.Delegate.Combine(d1, converted1).Cast<VRCAvatarManager.MulticastDelegateNPublicSealedVoGaVRBoUnique>();
            __instance.field_Internal_MulticastDelegateNPublicSealedVoGaVRBoUnique_1 = d1;

        }

        public static void OnAvatarInit(GameObject go, VRC_AvatarDescriptor avatarDescriptor, bool state)
        {
            if (avatarDescriptor != null)
            {
                foreach (Player player in Utils.PlayerManager.GetAllPlayers())
                {
                    if (player.field_Private_APIUser_0 == null)
                        continue;

                    VRCPlayer vrcPlayer = player.field_Internal_VRCPlayer_0;
                    if (vrcPlayer == null)
                        continue;

                    VRCAvatarManager vrcAM = vrcPlayer.prop_VRCAvatarManager_0;
                    if (vrcAM == null)
                        continue;

                    VRC_AvatarDescriptor descriptor = vrcAM.prop_VRC_AvatarDescriptor_0;
                    if ((descriptor == null) || (descriptor != avatarDescriptor))
                        continue;

                    OnUpdatePlayer(player);
                    break;
                }
            }
        }

        public static Player getPlayerFromPlayerlist(string userID)
        {
            foreach (var player in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
            {
                if (player.field_Private_APIUser_0 != null)
                {
                    if (player.field_Private_APIUser_0.id.Equals(userID))
                        return player;
                }
            }
            return null;
        }

        static bool ValidatePlayerAvatar(Player player)
        {
            return !(player == null ||
                     player.field_Internal_VRCPlayer_0 == null ||
                     player.field_Internal_VRCPlayer_0.isActiveAndEnabled == false ||
                     player.field_Internal_VRCPlayer_0.field_Internal_Animator_0 == null ||
                     player.field_Internal_VRCPlayer_0.field_Internal_GameObject_0 == null ||
                     player.field_Internal_VRCPlayer_0.field_Internal_GameObject_0.name.IndexOf("Avatar_Utility_Base_") == 0);
        }

    }
}
