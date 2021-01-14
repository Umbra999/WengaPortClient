using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using WengaPort.Wrappers;

namespace WengaPort.Modules
{
    class BotOrbit
    {
        public static float zSpread = 1f;
        public static float yOffset = 0f;
        public static float xSpread = 1f;
        public static float rotSpeed = 2f;
        static float timer = 0;
        
        public static void StartBot()
        {
            foreach (Renderer Render in Resources.FindObjectsOfTypeAll<Renderer>())
            {
                Render.enabled = false;
            }
            foreach (SkinnedMeshRenderer Render2 in Resources.FindObjectsOfTypeAll<SkinnedMeshRenderer>())
            {
                Render2.enabled = false;
            }
            MelonCoroutines.Start(BotRotation());
        }
        public static IEnumerator BotRotation()
        {
            yield return new WaitForSecondsRealtime(15);
            for (; ; )
            {
                while (RoomManager.field_Internal_Static_ApiWorld_0 == null) yield break;
                FetchRoom();
                if (!CurrentRoom.Contains(RoomManager.field_Internal_Static_ApiWorldInstance_0.idWithTags))
                {
                    yield return new WaitForSecondsRealtime(100);
                    Networking.GoToRoom(CurrentRoom);
                    yield break;
                }
                try
                {
                    Player Target = Utils.PlayerManager.GetPlayer("usr_68e5cfd9-de49-480a-9cc4-da1203927a1c");
                    Physics.gravity = Vector3.zero;
                    Movement.FlyEnable();
                    Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = false;
                    timer += Time.deltaTime * rotSpeed;
                    float x = -Mathf.Cos(timer) * xSpread;
                    float z = Mathf.Sin(timer) * zSpread;
                    Vector3 pos = new Vector3(x, yOffset, z);
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = pos + Target.field_Private_VRCPlayerApi_0.GetBonePosition(HumanBodyBones.Chest);
                    Vector3 Direction = Target.transform.position - VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.transform.position;
                    Direction.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(Direction);
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation = rotation;
                }
                catch {}
                yield return new WaitForEndOfFrame();
            }
            yield break;
        }

        public static string CurrentRoom;
        public static void FetchRoom()
        {
            var directory = new DirectoryInfo("\\\\vmware-host\\Shared Folders\\VRChat");
            if (directory != null && directory.Exists)
            {
                FileInfo target = null;
                foreach (var info in directory.GetFiles("output_log_*.txt", SearchOption.TopDirectoryOnly))
                {
                    if (target == null || info.LastAccessTime.CompareTo(target.LastAccessTime) >= 0)
                    {
                        target = info;
                    }
                }
                if (target != null)
                {
                    var fs = new FileStream(target.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using (var sr = new StreamReader(fs))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line != null)
                            {
                                if (line.Contains("[RoomManager] Joining w"))
                                {
                                    string[] arr = line.Split(new[] { "[RoomManager] Joining " }, StringSplitOptions.None);
                                    CurrentRoom = arr[1];
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
