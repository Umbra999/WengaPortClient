using WengaPort.Buttons;
using WengaPort.Extensions;
using WengaPort.Modules;

namespace WengaPort.Loaders
{
    class ModulesLoader
    {
        public static void Initialize()
        {
            LoadingMusic.LoadingSong.Initialize();
            ButtonsMainColor.Initialize();
            ButtonsMainColor.Initialize2();
            ButtonsMainColor.Initialize3();
            AntiMenuOverrender.UIInit();
            WorldButton.Setup();
            WorldDownloadStatus.Setup();
            Nameplates.OnUI();
            if (!UnityEngine.XR.XRDevice.isPresent)
            {
                PatchManager.VRMode = false;
            } 
            PlayerList.Init();
            ESP.Initialize();
            Movement.UIInit();
            MelonLoader.MelonCoroutines.Start(UIChanges.Initialize());
            MelonLoader.MelonCoroutines.Start(UIChanges.UpdateClock());
        }
    }
}
