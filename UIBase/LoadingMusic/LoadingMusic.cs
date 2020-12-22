using System.IO;
using System.Threading;
using UnityEngine;

namespace WengaPort.LoadingMusic
{
    class LoadingSong
    {
        //This is to replace the loading screen music
        public static void Initialize()
        {
            if (!File.Exists("WengaPort\\LoadingMusic.ogg")) return;

            var audioLoader = new WWW(string.Format("file://{0}", string.Concat(Directory.GetCurrentDirectory(), "/WengaPort/LoadingMusic.ogg")).Replace("\\", "/"));
            while (audioLoader.progress < 1)
                Thread.Sleep(1);
            var newMenuClip = audioLoader.GetAudioClip(false, false, AudioType.OGGVORBIS);
            newMenuClip.name = "New Music";
            GameObject Music = GameObject.Find("LoadingSound");
            VRCUiManager.prop_VRCUiManager_0.transform.Find("LoadingBackground_TealGradient_Music/LoadingSound")
                .GetComponent<AudioSource>().clip = newMenuClip;
            VRCUiManager.prop_VRCUiManager_0.transform.Find("MenuContent/Popups/LoadingPopup/LoadingSound")
                .GetComponent<AudioSource>().clip = newMenuClip;
            VRCUiManager.prop_VRCUiManager_0.transform.Find("LoadingBackground_TealGradient_Music/LoadingSound")
                .GetComponent<AudioSource>().Play();
            VRCUiManager.prop_VRCUiManager_0.transform.Find("MenuContent/Popups/LoadingPopup/LoadingSound")
                .GetComponent<AudioSource>().Play();
        }
    }
}
