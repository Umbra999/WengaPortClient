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
            if (!UnityEngine.XR.XRDevice.isPresent)
            {
                ThirdPerson.Initialize();
                PatchManager.VRMode = false;
            } 
            PlayerList.Initialize();
            PlayerList.Init();
            ESP.Initialize();
            LovenseRemote.UIInit();
            Movement.UIInit();
            MelonLoader.MelonCoroutines.Start(UIChanges.Initialize());
            MelonLoader.MelonCoroutines.Start(UIChanges.UpdateClock());
        }
    }
}
