using Harmony;
using MelonLoader;
using System.Linq;
using System.Reflection;
using UnityEngine.UI;
using VRC.Core;
using VRC.UI;
using WengaPort.Extensions;
using WengaPort.Modules;

namespace WengaPort.Modules
{
    [HarmonyPatch(typeof(NetworkManager), "OnJoinedRoom")]
    class OnJoinedRoomPatch
    {
        static void Prefix() => MelonCoroutines.Start(CacheManager.UpdateDirectoriesBackground());
    }

    [HarmonyPatch(typeof(NetworkManager), "OnLeftRoom")]
    class OnLeftRoomPatch
    {
        static void Prefix() => WorldDownloadManager.CancelDownload();
    }

    [HarmonyPatch(typeof(PageWorldInfo), "Method_Public_Void_ApiWorld_ApiWorldInstance_Boolean_Boolean_0")]
    class SetupWorldInfoPatch
    {
        static void Postfix(ApiWorld __0) => WorldButton.UpdateText(__0);
    }
}
