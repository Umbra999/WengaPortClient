using ExitGames.Client.Photon;
using Harmony;
using Il2CppSystem;
using Il2CppSystem.Collections;
using MelonLoader;
using Newtonsoft.Json.Linq;
using RootMotion.FinalIK;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Transmtn;
using UnhollowerBaseLib;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.Networking;
using VRC.SDKBase;
using WengaPort.Buttons;
using WengaPort.ConsoleUtils;
using WengaPort.FoldersManager;
using WengaPort.Modules;
using WengaPort.Wrappers;
using static VRC.SDKBase.VRC_EventHandler;

namespace WengaPort.Extensions
{
    internal class PatchManager
    {
        public static HarmonyInstance Instance = HarmonyInstance.Create("WengaPortPatches");

        private static HarmonyMethod GetPatch(string name)
        {
            return new HarmonyMethod(typeof(PatchManager).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        }

        public unsafe static void InitPatch()
        {
            try
            {
                Instance.Patch(AccessTools.Method(typeof(VRC_EventHandler), "InternalTriggerEvent", null, null), GetPatch("TriggerEvent"), null, null);
                Instance.Patch(AccessTools.Property(typeof(PhotonPeer), "RoundTripTime").GetMethod, null, GetPatch("FakePing"), null);
                Instance.Patch(AccessTools.Property(typeof(PhotonPeer), "RoundTripTimeVariance").GetMethod, null, GetPatch("FakePing"), null);
                Instance.Patch(typeof(ObjectPublicIPhotonPeerListenerObStNuStOb1CoObBoDiUnique).GetMethod("OnEvent"), GetPatch("OnEvent"), null);
                Instance.Patch(typeof(VRCAvatarManager).GetMethod("Method_Private_Boolean_GameObject_PDM_0"), GetPatch("OnAvatarInstantiate"));
                Instance.Patch(AccessTools.Method(typeof(API), "SendPutRequest", null, null), GetPatch("RequestPatch"), null, null);
                Instance.Patch(AccessTools.Method(typeof(PostOffice), "Put", null, null), GetPatch("ReceivedNotificationPatch"), null, null);
                Instance.Patch(typeof(PortalTrigger).GetMethod(nameof(PortalTrigger.OnTriggerEnter), BindingFlags.Public | BindingFlags.Instance), GetPatch("EnterPortalPrefix"), null, null);
                Instance.Patch(typeof(PhotonPeerPublicPo1Di2ByObTyUnique).GetMethod("Method_Public_Virtual_New_Boolean_Byte_Object_ObjectPublicObByObInByObObUnique_SendOptions_0"), GetPatch("OpRaiseEventPrefix"), null, null);
                Instance.Patch(AccessTools.Method(typeof(VRC_EventDispatcherRFC), "Method_Public_Void_Player_VrcEvent_VrcBroadcastType_Int32_Single_0", null, null), GetPatch("CaughtEventPatch"), null, null);
                Instance.Patch(AccessTools.Property(typeof(Time), "smoothDeltaTime").GetMethod, null, GetPatch("FakeFrames"), null);
                Instance.Patch(typeof(UdonSync).GetMethod(nameof(UdonSync.UdonSyncRunProgramAsRPC)), GetPatch("UdonSyncPatch"), null);
                Instance.Patch(AccessTools.Property(typeof(Tools), "Platform").GetMethod, null, GetPatch("ModelSpoof"));
                Instance.Patch(typeof(IKSolverHeuristic).GetMethods().Where(m => m.Name.Equals("IsValid") && m.GetParameters().Length == 1).First(), prefix: new HarmonyMethod(typeof(PatchManager).GetMethod("IsValid", BindingFlags.NonPublic | BindingFlags.Static)));
                MethodInfo SteamPatch = typeof(PhotonPeer).Assembly.GetType("ExitGames.Client.Photon.EnetPeer").GetMethod("EnqueueOperation", (BindingFlags)(-1));
                Instance.Patch(SteamPatch, GetPatch("EnqueueOperationPatched"), null, null);
                Logger.WengaLogger("[Patches] Trigger");
                Logger.WengaLogger("[Patches] PingSpoof");
                Logger.WengaLogger("[Patches] Events");
                Logger.WengaLogger("[Patches] Moderations");
                Logger.WengaLogger("[Patches] Notification");
                Logger.WengaLogger("[Patches] Portals");
                Logger.WengaLogger("[Patches] FrameSpoof");
                Logger.WengaLogger("[Patches] Udon");
                Logger.WengaLogger("[Patches] Safety");
            }
            catch (System.Exception arg)
            {
                Logger.WengaLogger(string.Format("[Patches] Failed Patching \n{0}", arg));
            }
            try
            {
                MethodInfo[] methods = typeof(ObjectPublicAbstractSealedInObInObObObObUnique).GetMethods(BindingFlags.Public | BindingFlags.Static);
                for (int i = 0; i < methods.Length; i++)
                    if (methods[i].Name == "Method_Public_Static_IEnumerator_String_GameObject_AvatarPerformanceStats_0" || methods[i].Name == "Method_Public_Static_IEnumerator_GameObject_AvatarPerformanceStats_EnumPublicSealedvaNoExGoMePoVe7vUnique_MulticastDelegateNPublicSealedVoUnique_0" || methods[i].Name == "Method_Public_Static_Void_String_GameObject_AvatarPerformanceStats_0")
                        Instance.Patch(methods[i], GetPatch("CalculatePerformance"), null, null);
            }
            catch (System.Exception e)
            {
                Logger.WengaLogger("Failed to patch Performance Scanners: " + e);
            }
        }

        private static bool IsInitialized;
        private static bool SeenFire;
        private static bool AFiredFirst;

        public static event System.Action<Player> OnJoin;

        public static event System.Action<Player> OnLeave;

        private static bool IsValid(ref IKSolverHeuristic __instance, ref bool __result)
        {
            if (__instance.maxIterations > 64)
            {
                __result = false;
                Logger.WengaLogger("[Room] [Protection] Prevented Malicious IK Interaction");
                VRConsole.Log(VRConsole.LogsType.Protection, "Prevented Malicious IK Interaction");
                return false;
            }
            return true;
        }

        private static bool EnterPortalPrefix(PortalTrigger __instance, MethodInfo __originalMethod)
        {
            try
            {
                var portalInternal = __instance.field_Private_PortalInternal_0;
                if (Vector3.Distance(Utils.CurrentUser.transform.position, __instance.transform.position) > 0.8f)
                {
                    return false;
                }
                {
                    Utils.VRCUiPopupManager.Alert("Enter Portal", $"{portalInternal.field_Private_ApiWorld_0.name}", "Yes", new System.Action(() =>
                    {
                        Networking.GoToRoom(portalInternal.field_Private_ApiWorld_0.id + ":" + portalInternal.field_Private_String_1);
                        Utils.VRCUiPopupManager.HideCurrentPopUp();
                    }), "No", new System.Action(() =>
                    {
                        Utils.VRCUiPopupManager.HideCurrentPopUp();
                    }));
                    return false;
                }
            }
            catch { }
            return true;
        }

        public static void QuestIni()
        {
            if (Create.Ini.GetBool("Toggles", "QuestSpoof", true))
            {
                QuestSpoof = true;
            }
            else
            {
                QuestSpoof = false;
            }
        }

        public static bool QuestSpoof = false;

        private static void ModelSpoof(ref string __result)
        {
            if (QuestSpoof)
            {
                if (RoomManager.field_Internal_Static_ApiWorldInstance_0 == null)
                {
                    __result = "android";
                }
            }
        }

        private static bool CalculatePerformance() => false;

        public static void EventHandlerA(Player player)
        {
            if (!SeenFire)
            {
                AFiredFirst = true;
                SeenFire = true;
            }
            (AFiredFirst ? OnJoin : OnLeave)?.Invoke(player);
        }

        public static void EventHandlerB(Player player)
        {
            if (!SeenFire)
            {
                AFiredFirst = false;
                SeenFire = true;
            }
            (AFiredFirst ? OnLeave : OnJoin)?.Invoke(player);
        }

        public static void JoinInitialize()
        {
            if (IsInitialized) return;
            if (ReferenceEquals(NetworkManager.field_Internal_Static_NetworkManager_0, null)) return;

            var field0 = NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_ObjectPublicHa1UnT1Unique_1_Player_0;
            var field1 = NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_ObjectPublicHa1UnT1Unique_1_Player_1;

            AddDelegate(field0, EventHandlerA);
            AddDelegate(field1, EventHandlerB);

            IsInitialized = true;

            OnJoin += OnPlayerJoin;
            OnLeave += OnPlayerLeft;
        }

        private static void AddDelegate(ObjectPublicHa1UnT1Unique<Player> field, System.Action<Player> eventHandlerA)
        {
            field.field_Private_HashSet_1_UnityAction_1_T_0.Add(eventHandlerA);
        }

        private static void FakePing(ref short __result)
        {
            if (PingSpoof)
            {
                __result = -69;
            }
        }

        private static void FakeFrames(ref float __result)
        {
            if (FrameSpoof)
            {
                __result = (float)1 / 1000;
                return;
            }
        }

        private static bool returnFalse()
        {
            return false;
        }

        private static void OnAvatarInstantiate(ref GameObject __0, ref VRCAvatarManager __instance)
        {
            try
            {
                if (__0 == null || __instance?.field_Private_VRCPlayer_0?.field_Private_Player_0?.prop_APIUser_0 == null || __instance.field_Private_ApiAvatar_0 == null) return;
                var player = __instance.field_Private_VRCPlayer_0;
                var Avatar = __instance.field_Private_ApiAvatar_0;
                var AvatarID = __instance.field_Private_ApiAvatar_0.id;
                var AvatarGameobject = __0;
                if (player.GetAPIAvatar() == Avatar)
                Logger.WengaLogger($"[Room] [Avatar] {player.DisplayName()} -> {Avatar.name} [{Avatar.releaseStatus}]");
                VRConsole.Log(VRConsole.LogsType.Avatar, $"{player.DisplayName()} --> {Avatar.name} [{Avatar.releaseStatus}]");
                GlobalDynamicBones.ProcessDynamicBones(AvatarGameobject, player);
                foreach (DynamicBone item2 in AvatarGameobject.GetComponentInChildren<Animator>().GetComponentsInChildren<DynamicBone>())
                {
                    item2.m_DistantDisable = true;
                }
            }
            catch
            { }
        }

        private static bool OpRaiseEventPrefix(ref byte __0, ref Il2CppSystem.Object __1, ref ObjectPublicObByObInByObObUnique __2, ref SendOptions __3)
        {
            try
            {
                switch (__0)
                {
                    case 202:
                        return !Modules.Photon.Invisible;
                    case 254:
                        return !Modules.Photon.Invisible;
                    case 7:
                        return !Modules.Photon.Serialize;
                    case 206:
                        return !Modules.Photon.Serialize;
                    case 201:
                        return !Modules.Photon.Serialize;
                    case 4:
                        return !Modules.Photon.LockInstance;
                    case 5:
                        return !Modules.Photon.LockInstance;
                    case 1:
                        return !Modules.Photon.Forcemute;
                    default:
                        break;
                }
            }
            catch
            { }
            return true;
        }

        private static void OnPlayerLeft(Player __0)
        {
            try
            {
                VRConsole.Log(VRConsole.LogsType.Left, __0.DisplayName());
                Logger.WengaLogger($"[-] {__0.DisplayName()}");
            }
            catch
            { }
        }

        public static bool LoginDelay = true;

        private static void OnPlayerJoin(Player __0)
        {
            try
            {
                //PlayerList.IsAllowedClient();
                VRConsole.Log(VRConsole.LogsType.Join, __0.DisplayName());
                Logger.WengaLogger($"[+] {__0.DisplayName()}");

                if (GlobalDynamicBones.FriendBones)
                {
                    PlayerList.DynBoneAdder(__0);
                }
                if (LoginDelay)
                {
                    LoginDelay = false;
                    MelonCoroutines.Start(QuestSpoofer());
                    ExploitMenu.ButtonToggles();
                    Api.ApiExtension.Start();
                }
            }
            catch
            {
            }
        }

        public static System.Collections.IEnumerator QuestSpoofer()
        {
            yield return new WaitForSeconds(6);
            QuestSpoof = false;
        }

        private static bool RequestPatch(ref string __0, ref System.Collections.Generic.Dictionary<string, object> __2)
        {
            bool flag3 = OfflineMode && (__0 == "visits" || __0 == "joins");
            return !flag3;
        }

        public static bool WorldTrigger = false;
        public static bool AntiWorldTrigger = false;
        public static bool AntiUdon = false;
        public static bool AntiMasterDC = false;
        public static bool OfflineMode = false;
        public static bool FrameSpoof = true;
        public static bool PingSpoof = true;
        public static bool EventLog = false;
        public static bool DictLog = false;
        public static bool RPCLog = true;

        private static bool UdonSyncPatch(string __0)
        {
            if (AntiUdon)
            {
                Logger.WengaLogger($"[Room] [Protection] Prevented Udon Event [{__0}]");
                VRConsole.Log(VRConsole.LogsType.Protection, $"Prevented Udon Event [{__0}]");
                return false;
            }
            return true;
        }

        private static bool TriggerEvent(ref VrcEvent __0, ref VrcBroadcastType __1, ref int __2)
        {
            try
            {
                if (WorldTrigger)
                {
                    __1 = 0;
                }
            }
            catch
            { }
            return true;
        }

        private static bool EnqueueOperationPatched(Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object> __0, byte __1)
        {
            try
            {
                if (__1 == 252)
                {
                    maskProperties(__0, 251);
                }
            }
            catch (System.Exception value)
            {
                Logger.WengaLogger(value);
            }
            return true;
        }

        public static bool VRMode = true;
        public static bool InvisibleHide = false;
        public static bool MiniHide = false;
        public static bool AntiBlock = true;
        private static void maskProperties(Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object> param, byte propertyIndex)
        {
            try
            {
                Hashtable hashtable = param[propertyIndex].Cast<Hashtable>();
                //if (hashtable.ContainsKey("steamUserID"))
                //{
                //    hashtable["steamUserID"] = "666";
                //}
                if (hashtable.ContainsKey("inVRMode"))
                {
                    hashtable["inVRMode"] = new Il2CppSystem.Boolean()
                    {
                        m_value = VRMode
                    }.BoxIl2CppObject();
                }
                if (hashtable.ContainsKey("avatarEyeHeight"))
                {
                    if (InvisibleHide)
                    {
                        hashtable["avatarEyeHeight"] = new Il2CppSystem.Int32()
                        {
                            m_value = int.MinValue
                        }.BoxIl2CppObject();
                    }
                    else if (MiniHide)
                    {
                        hashtable["avatarEyeHeight"] = new Il2CppSystem.Int32()
                        {
                            m_value = 1
                        }.BoxIl2CppObject();
                    }
                }
            }
            catch (System.Exception value)
            {
                Logger.WengaLogger(value);
            }
        }

        public static bool BlockPlayer = false;
        public static bool EventDelay = false;

        private static byte[] IgnoreCodes = new byte[]
        {
            1,7,8,9,206,201,226
        };

        private static bool OnEvent(ref EventData __0)
        {
            try
            {
                if (!IgnoreCodes.Contains(__0.Code) && DictLog)
                {
                    object Data = Utils.Serialization.FromIL2CPPToManaged<object>(__0.Parameters);
                    Logger.WengaLogger($"[Event {__0.Code}] Event \n{Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented)}");
                }
                else if (EventLog)
                {
                    Logger.WengaLogger($"[PhotonEvent] {__0.Code}");
                }
                switch (__0.Code)   
                {
                    case 33:
                        object Data = Utils.Serialization.FromIL2CPPToManaged<object>(__0.Parameters);
                        if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"10\": false,"))
                        {
                            JObject jObject = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented));
                            var jo = JObject.Parse(jObject.ToString());
                            var id = jo["245"]["1"].ToString();

                            var ParsedID = Utils.PlayerManager.GetPlayerWithPlayerID(int.Parse(id)).GetAPIUser().DisplayName();
                            if (ParsedID == null)
                            {
                                ParsedID = "?";
                            }

                            Logger.WengaLogger($"[Moderation] {ParsedID} unblocked you");
                            VRConsole.Log(VRConsole.LogsType.Block, $"{ParsedID} --> Unblocked You");
                            PlayerList.BlockList.Remove(Utils.PlayerManager.GetPlayerWithPlayerID(int.Parse(id)).UserID());
                            if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"11\": false"))
                            {
                                Logger.WengaLogger($"[Moderation] {ParsedID} unmuted you");
                                VRConsole.Log(VRConsole.LogsType.Voice, $"{ParsedID} --> Unmuted You");
                            }
                            else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"11\": true"))
                            {
                                Logger.WengaLogger($"[Moderation] {ParsedID} muted you");
                                VRConsole.Log(VRConsole.LogsType.Voice, $"{ParsedID} --> Muted You");
                            }
                        }
                        else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"10\": true,"))
                        {
                            JObject jObject = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented));
                            var jo = JObject.Parse(jObject.ToString());
                            var id = jo["245"]["1"].ToString();

                            var ParsedID = Utils.PlayerManager.GetPlayerWithPlayerID(int.Parse(id)).GetAPIUser().DisplayName();
                            if (ParsedID == null)
                            {
                                ParsedID = "?";
                            }

                            Logger.WengaLogger($"[Moderation] {ParsedID} blocked you");
                            VRConsole.Log(VRConsole.LogsType.Block, $"{ParsedID} --> Blocked You");
                            PlayerList.BlockList.Add(Utils.PlayerManager.GetPlayerWithPlayerID(int.Parse(id)).UserID());
                            if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"11\": false"))
                            {
                                Logger.WengaLogger($"[Moderation] {ParsedID} unmuted you");
                                VRConsole.Log(VRConsole.LogsType.Voice, $"{ParsedID} --> Unmuted You");
                            }
                            else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"11\": true"))
                            {
                                Logger.WengaLogger($"[Moderation] {ParsedID} muted you");
                                VRConsole.Log(VRConsole.LogsType.Voice, $"{ParsedID} --> Muted You");
                            }
                            return !AntiBlock;
                        }
                        else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("You have been warned"))
                        {
                            Logger.WengaLogger($"[Moderation] The Room Owner warned you");
                            VRConsole.Log(VRConsole.LogsType.Warn, $"RoomOwner --> Warn You");
                            return false;
                        }
                        else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("A vote kick has been"))
                        {
                            Logger.WengaLogger("[Moderation] Someone votekick Someone");
                            VRConsole.Log(VRConsole.LogsType.Votekick, $"[?] --> [?]");
                        }
                        else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("Unable to start a vote to kick"))
                        {
                            Logger.WengaLogger("[Moderation] Failed at Votekick");
                            VRConsole.Log(VRConsole.LogsType.Votekick, $"Failed at Votekick");
                            return false;
                        }
                        else if (Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented).Contains("\"0\": 8"))
                        {
                            Logger.WengaLogger("[Moderation] The Room Owner ForceMicOff You");
                            VRConsole.Log(VRConsole.LogsType.Voice, $"RoomOwner --> MicOff You");
                            return false;
                        }
                        break;
                    case 2:
                        object Data2 = Utils.Serialization.FromIL2CPPToManaged<object>(__0.Parameters);
                        if (Newtonsoft.Json.JsonConvert.SerializeObject(Data2, Newtonsoft.Json.Formatting.Indented).Contains("You have been kicked"))
                        {
                            Logger.WengaLogger("[Moderation] RoomOwner kicked you");
                            VRConsole.Log(VRConsole.LogsType.Kick, $"RoomOwner --> You");
                            return false;
                        }
                        break;
                    case 1:
                        Il2CppStructArray<byte> il2CppStructArray2 = new Il2CppStructArray<byte>(__0.CustomData.Pointer);
                        bool flag4 = il2CppStructArray2.Length > 1750;
                        if (flag4)
                        {
                            Logger.WengaLogger($"[Room] [Protection] Prevented USpeak Event");
                            VRConsole.Log(VRConsole.LogsType.Protection, $"Prevented Malicious USpeak");
                            return false;
                        }
                        break;
                    case 209:
                        if (!EventDelay && Utils.PlayerManager.GetPlayerWithPlayerID(__0.Sender).GetVRCPlayer().GetIsBot())
                        {
                            Logger.WengaLogger($"[Room] [Protection] Prevented PhotonEvent {__0.Code} from {Utils.PlayerManager.GetPlayer(__0.Sender).DisplayName()}");
                            VRConsole.Log(VRConsole.LogsType.Protection, $"{Utils.PlayerManager.GetPlayer(__0.Sender).DisplayName()} --> Bot Event [209]");
                            return false;
                        }
                        break;
                    case 210:
                        if (!EventDelay && Utils.PlayerManager.GetPlayerWithPlayerID(__0.Sender).GetVRCPlayer().GetIsBot())
                        {
                            Logger.WengaLogger($"[Room] [Protection] Prevented PhotonEvent {__0.Code} from {Utils.PlayerManager.GetPlayer(__0.Sender).DisplayName()}");
                            VRConsole.Log(VRConsole.LogsType.Protection, $"{Utils.PlayerManager.GetPlayer(__0.Sender).DisplayName()} --> Bot Event [210]");
                            return false;
                        }
                        break;
                    case 6:
                        if (!EventDelay && Utils.PlayerManager.GetPlayerWithPlayerID(__0.Sender).GetVRCPlayer().GetIsBot())
                        {
                            Logger.WengaLogger($"[Room] [Protection] Prevented PhotonEvent {__0.Code} from {Utils.PlayerManager.GetPlayer(__0.Sender).DisplayName()}");
                            VRConsole.Log(VRConsole.LogsType.Protection, $"{Utils.PlayerManager.GetPlayer(__0.Sender).DisplayName()} --> Bot Event [6]");
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch
            { }
            return true;
        }

        private static bool IsNaN(Vector3 v3)
        {
            return float.IsNaN(v3.x) || float.IsNaN(v3.y) || float.IsNaN(v3.z);
        }

        private static bool IsNanPos(Vector3 v3)
        {
            return int.MaxValue < v3.x || int.MaxValue < v3.y || int.MaxValue < v3.z || int.MinValue > v3.x || int.MinValue > v3.y || int.MinValue > v3.z;
        }

        private static bool CaughtEventPatch(ref Player __0, ref VrcEvent __1, ref VrcBroadcastType __2, ref int __3, ref float __4)
        {
            try
            {
                Player instance = __0;
                string name = __1.ParameterObject.name;
                string text = instance.DisplayName();
                string a = instance.UserID();
                Il2CppSystem.Object[] array = Networking.DecodeParameters(__1.ParameterBytes);
                if (array == null)
                {
                    array = new Il2CppSystem.Object[0];
                }
                Player player = null;
                string a2 = "";
                string text3 = "";
                try
                {
                    player = Utils.PlayerManager.GetPlayer(Il2CppSystem.Convert.ToString(array[0]));
                    text3 += player.DisplayName();
                    for (int i = 1; i < array.Length - 1; i++)
                    {
                        text3 = text3 + " [" + Il2CppSystem.Convert.ToString(array[i]) + "]";
                    }
                    a2 = player.UserID();
                }
                catch
                { }
                string text4 = "";
                foreach (Il2CppSystem.Object value in array)
                {
                    text4 = text4 + "[" + Il2CppSystem.Convert.ToString(value) + "]";
                }
                if (!(__1.ParameterObject.name == "USpeak") && !(__1.ParameterString == "SetTimerRPC") && RPCLog)
                {
                    System.Console.ForegroundColor = System.ConsoleColor.Magenta;
                    MelonConsole.SetColor(System.ConsoleColor.DarkBlue);

                    System.Console.WriteLine(string.Format("\n[RPC] \nPLAYER: {0} \nOBJECT: {1}  \nEXECUTED: {2} \nFOR: {3} \nType: {4} [{5}] L: {6}", new object[]
                        {
                                    text,__1.ParameterObject.name,__1.ParameterString,(player == null) ? text4 : text3,__1.EventType,__2,array.Length
                        }));
                    System.Console.ResetColor();
                    MelonConsole.SetColor(System.ConsoleColor.White);
                }
                bool flag = instance.GetVRCPlayer().GetIsBot() || RPCAndEventBlock.Check(instance.UserID());
                if (flag)
                {
                    System.Console.ForegroundColor = System.ConsoleColor.Red;
                    MelonConsole.SetColor(System.ConsoleColor.Red);
                    System.Console.WriteLine(string.Concat(new string[]
                    {
                        "\n[BLOCKED RPC] \nPLAYER: ",text," \nEXECUTED:", __1.ParameterString, " \nOBJECT: ", __1.ParameterObject.name," [", __2.ToString(),"](",__3.ToString(),"/",__4.ToString(),")"
                    }));
                    System.Console.ResetColor();
                    return false;
                }

                if (__1.ParameterObject != null)
                {
                    if (__0.field_Private_APIUser_0.id != APIUser.CurrentUser.id && IsNaN(__1.ParameterObject.transform.position))
                    {
                        VRConsole.Log(VRConsole.LogsType.Protection, text + " --> used Infinity-Events");
                        Logger.WengaLogger("[Room] [Protection] Prevented " + text + " from using Infinity-Events");
                        return false;
                    }
                    else if (__0.field_Private_APIUser_0.id != APIUser.CurrentUser.id && IsNanPos(__1.ParameterObject.transform.position))
                    {
                        VRConsole.Log(VRConsole.LogsType.Protection, text + " --> spawned Infinity-Objects");
                        Logger.WengaLogger("[Room] [Protection] Prevented " + text + " from spawning Objects at Infinity");
                        return false;
                    }
                    else if (__1.ParameterObject.name == "VRCVideoSync" && BlockPlayer)
                    {
                        Logger.WengaLogger("[Room] [Protection] Video not loading - Sync disabled");
                        VRConsole.Log(VRConsole.LogsType.Protection, "Video Sync --> Disabled");
                        return false;
                    }
                }

                if (__1.ParameterString != null)
                {
                    switch (__1.ParameterString)
                    {
                        case "SetTimerRPC":
                            return true;

                        case "ConfigurePortal":
                            VRConsole.Log(VRConsole.LogsType.Portal, text + " --> Portaldrop");
                            Logger.WengaLogger($"[Room] [Portal] {text} spawned a Portal");
                            if (PortalHandler.AntiPortal)
                            {
                                return false;
                            }
                            break;

                        case "PlayEffect":
                            VRConsole.Log(VRConsole.LogsType.Portal, text + " --> Portal entered");
                            Logger.WengaLogger("[Room] [Portal]" + text + " entered a Portal");
                            break;

                        case "_DestroyObject":
                            VRConsole.Log(VRConsole.LogsType.Portal, text + " --> Destroyed Object");
                            Logger.WengaLogger($"[Room] [Portal] {text} destroyed an Object");
                            break;

                        case "CleanRoomRPC":
                            VRConsole.Log(VRConsole.LogsType.Info, text + " --> Cleaned Room");
                            Logger.WengaLogger($"[Room] [Info] {text} cleaned the Room");
                            break;

                        case "PlayEmoteRPC":
                            //VRConsole.Log(VRConsole.LogsType.Info,$" {text} --> Emote");
                            Logger.WengaLogger($"[Room] [Info] {text} played Emote");
                            break;

                        case "SpawnEmojiRPC":
                            VRConsole.Log(VRConsole.LogsType.Info, $"{text} --> Emoji");
                            Logger.WengaLogger($"[Room] [Info] {text} played Emoji");
                            break;

                        case "_InstantiateObject":
                            if (text4.Contains("Infinity") && __0.field_Private_APIUser_0.id != APIUser.CurrentUser.id)
                            {
                                VRConsole.Log(VRConsole.LogsType.Protection, $"{text} --> Instantiate Infinity-Objects");
                                Logger.WengaLogger($"[Room] [Protection] Prevented {text} from Instantiating Objects at Infinity");
                                return false;
                            }
                            else
                            {
                                VRConsole.Log(VRConsole.LogsType.Info, $"{text} --> Instantiate Object");
                                Logger.WengaLogger($"[Room] [Info] {text} instantiated Object");
                            }
                            break;

                        case "ChangeVisibility":
                            if (text4.Contains("True"))
                            {
                                VRConsole.Log(VRConsole.LogsType.Info, $"{text} --> Camera Show");
                                Logger.WengaLogger($"[Room] [Info] {text} showed the Camera");
                            }
                            else if (text4.Contains("False"))
                            {
                                VRConsole.Log(VRConsole.LogsType.Info, $"{text} --> Camera Hide");
                                Logger.WengaLogger($"[Room] [Info] {text} hide the Camera");
                            }
                            break;
                        default:
                            break;
                    }
                }
                VrcBroadcastType vrcBroadcastType = __2;
                VrcBroadcastType vrcBroadcastType2 = vrcBroadcastType;
                if (vrcBroadcastType2 == VrcBroadcastType.Local)
                {
                    return true;
                }
                VrcEventType eventType = __1.EventType;
                VrcEventType vrcEventType = eventType;
                if (vrcBroadcastType == 0 && AntiMasterDC)
                {
                    if (a != APIUser.CurrentUser.id)
                    {
                        VRConsole.Log(VRConsole.LogsType.Protection, $"{text} --> Always Event [{eventType}]");
                        Logger.WengaLogger($"[Room] [Protection] Prevented {text} from using Event Disconnect Exploit [{eventType}]");
                    }
                    return a == APIUser.CurrentUser.id;
                }
                if (a != APIUser.CurrentUser.id && eventType == VrcEventType.SetGameObjectActive && AntiWorldTrigger)
                {
                    if (vrcBroadcastType == 0 || vrcBroadcastType == VrcBroadcastType.AlwaysUnbuffered || vrcBroadcastType == VrcBroadcastType.AlwaysBufferOne)
                    {
                        VRConsole.Log(VRConsole.LogsType.Protection, $"{text} --> WorldTrigger");
                        Logger.WengaLogger($"[Room] [Protection] Prevented {text} from using Worldtrigger");
                        return false;
                    }
                }
            }
            catch
            { }
            return true;
        }
    }
}