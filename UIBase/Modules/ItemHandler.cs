using System;
using UnityEngine;
using VRC;
using MelonLoader;
using System.Collections;
using VRC.Core;
using System.Collections.Generic;
using WengaPort.Wrappers;
using VRCSDK2;
using VRC.SDKBase;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Rendering.PostProcessing;

namespace WengaPort.Modules
{
	class ItemHandler
	{
		public static void BringPickups()
		{
            foreach (VRC_ObjectSync vrc_ObjectSync in World_ObjectSyncs)
            {
                Networking.SetOwner(Utils.CurrentUser.field_Private_VRCPlayerApi_0, vrc_ObjectSync.gameObject);
                vrc_ObjectSync.transform.rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);
                vrc_ObjectSync.transform.position = Utils.CurrentUser.transform.position;
            }
        }

        public static bool ItemCrashToggle = false;
        public static IEnumerator ItemCrash(Player p)
        {
            for (; ;)
            {
                if (!ItemCrashToggle)
                {
                    yield break;
                }
                foreach (VRC.SDKBase.VRC_Trigger vrc_Trigger in UnityEngine.Object.FindObjectsOfType<VRC.SDKBase.VRC_Trigger>())
                {
                    bool hasPickupTriggers = vrc_Trigger.HasPickupTriggers;
                    if (hasPickupTriggers)
                    {
                        vrc_Trigger.TakesOwnershipIfNecessary.ToString();
                        vrc_Trigger.Interact();
                    }
                }
                foreach (VRC.SDKBase.VRC_Pickup vrc_Pickup in UnityEngine.Object.FindObjectsOfType<VRC.SDKBase.VRC_Pickup>())
                {
                    bool flag = vrc_Pickup.GetComponent<Collider>() && !vrc_Pickup.GetComponent<VRC.SDKBase.VRC_SpecialLayer>() && !vrc_Pickup.IsHeld;
                    if (flag)
                    {
                        TakeOwnershipIfNecessary(vrc_Pickup.gameObject);
                        vrc_Pickup.pickupable = true;
                        vrc_Pickup.ThrowVelocityBoostMinSpeed = int.MaxValue;
                        vrc_Pickup.ThrowVelocityBoostScale = int.MaxValue;
                        vrc_Pickup.transform.position = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
                    }
                }
                yield return new WaitForSeconds(7f);
                foreach (VRC.SDKBase.VRC_Pickup vrc_Pickup in UnityEngine.Object.FindObjectsOfType<VRC.SDKBase.VRC_Pickup>())
                {
                    bool flag = vrc_Pickup.GetComponent<Collider>() && !vrc_Pickup.GetComponent<VRC.SDKBase.VRC_SpecialLayer>() && !vrc_Pickup.IsHeld;
                    if (flag)
                    {
                        TakeOwnershipIfNecessary(vrc_Pickup.gameObject);
                        vrc_Pickup.pickupable = true;
                        vrc_Pickup.ThrowVelocityBoostMinSpeed = int.MaxValue;
                        vrc_Pickup.ThrowVelocityBoostScale = int.MaxValue;
                        vrc_Pickup.transform.position = p.transform.position;
                    }
                }
                yield return new WaitForSeconds(7f);
            }
            yield break;
        }

		public static List<VRCSDK2.VRC_Pickup> World_Pickups = new List<VRCSDK2.VRC_Pickup>();

		public static List<VRC_ObjectSync> World_ObjectSyncs = new List<VRC_ObjectSync>();

		public static List<VRCSDK2.VRC_Trigger> World_Triggers = new List<VRCSDK2.VRC_Trigger>();

        public static List<VRC.SDKBase.VRC_MirrorReflection> World_Mirrors = new List<VRC.SDKBase.VRC_MirrorReflection>();

        public static List<VRCStation> World_Chairs = new List<VRCStation>();

        public static List<PostProcessVolume> PostProcess = new List<PostProcessVolume>();

        public static List<VRC.SDKBase.VRC_AvatarPedestal> Pedestals = new List<VRC.SDKBase.VRC_AvatarPedestal>();

        public static bool ChairToggle = true;
        public static bool MirrorToggle = false;
        public static bool ItemToggle = false;
        public static bool PickupToggle = false;
        public static bool PostProcessToggle = false;
        public static bool PedestalToggle = false;
        public static bool NightmodeToggle = false;
        public static bool AutoHoldToggle = false;
        public static bool FastPickupToggle = false;

        public static void TakeOwnershipIfNecessary(GameObject gameObject)
        {
            if (getOwnerOfGameObject(gameObject) != Utils.CurrentUser)
            {
                Networking.SetOwner(Utils.CurrentUser.field_Private_VRCPlayerApi_0, gameObject);
            }
        }

        public static Player getOwnerOfGameObject(GameObject gameObject)
        {
            foreach (Player player in Utils.PlayerManager.GetAllPlayers())
            {
                if (player.field_Private_VRCPlayerApi_0.IsOwner(gameObject))
                {
                    return player;
                }
            }
            return null;
        }
        public static IEnumerator ItemLag(VRCPlayer P)
        {
            foreach (VRCSDK2.VRC_Pickup vrc_Pickup in UnityEngine.Object.FindObjectsOfType<VRCSDK2.VRC_Pickup>())
            {
                Networking.SetOwner(Utils.CurrentUser.field_Private_VRCPlayerApi_0, vrc_Pickup.gameObject);
                vrc_Pickup.transform.position = new Vector3(int.MinValue, int.MinValue, int.MinValue);
                Networking.SetOwner(Utils.CurrentUser.field_Private_VRCPlayerApi_0, vrc_Pickup.gameObject);
            }
            yield return new WaitForSeconds(13f);
            ItemsToPlayer(P);
            yield return new WaitForSeconds(13f);
            foreach (VRCSDK2.VRC_Pickup vrc_Pickup in UnityEngine.Object.FindObjectsOfType<VRCSDK2.VRC_Pickup>())
            {
                Networking.SetOwner(Utils.CurrentUser.field_Private_VRCPlayerApi_0, vrc_Pickup.gameObject);
                vrc_Pickup.transform.position = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
                Networking.SetOwner(Utils.CurrentUser.field_Private_VRCPlayerApi_0, vrc_Pickup.gameObject);
            }
            yield return new WaitForSeconds(13f);
            ItemsToPlayer(P);
            yield break;
        }

        public static void ItemsToPlayer(VRCPlayer Player)
        {
            foreach (VRC_ObjectSync vrc_ObjectSync in World_ObjectSyncs)
            {
                Networking.SetOwner(Utils.CurrentUser.field_Private_VRCPlayerApi_0, vrc_ObjectSync.gameObject);
                vrc_ObjectSync.transform.rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);
                vrc_ObjectSync.transform.position = Player.transform.position;
                Networking.SetOwner(Utils.CurrentUser.field_Private_VRCPlayerApi_0, vrc_ObjectSync.gameObject);
            }
        }

        public static void ItemsToPlayerHead(VRCPlayer Player)
        {
            foreach (VRC_ObjectSync vrc_ObjectSync in World_ObjectSyncs)
            {
                Networking.SetOwner(Utils.CurrentUser.field_Private_VRCPlayerApi_0, vrc_ObjectSync.gameObject);
                vrc_ObjectSync.transform.rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);
                vrc_ObjectSync.transform.position = Player.gameObject.transform.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.Head).transform.position;
                Networking.SetOwner(Utils.CurrentUser.field_Private_VRCPlayerApi_0, vrc_ObjectSync.gameObject);
            }
        }

        public static void DropItems()
        {
            foreach (var vrc_Pickup in Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_Pickup>())
            {
                if (vrc_Pickup.IsHeld)
                {
                    TakeOwnershipIfNecessary(vrc_Pickup.gameObject);
                    vrc_Pickup.Drop();
                }
            }
        }
        public static bool AutoDropItems = false;
        public static IEnumerator DrawCircle(Player Player)
        {
            foreach (VRCSDK2.VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_Pickup>())
            {
                if (!pickup.IsHeld && pickup.gameObject.activeSelf)
                {
                    if (pickup.name == "Grip" || pickup.name.ToLower().Contains("pen") || pickup.name.ToLower().Contains("marker") || pickup.name.ToLower().Contains("Grip") || pickup.name.ToLower().Contains("grip") || pickup.name.ToLower().Contains("Counter"))
                    {
                        TakeOwnershipIfNecessary(pickup.gameObject);
                        Vector3 OriginalPosition = pickup.transform.position;
                        Quaternion OriginalRotation = pickup.transform.rotation;
                        float CircleSpeed = 10f;
                        float alpha = 0f;
                        float a = 1f;
                        float b = 1f;
                        Networking.RPC(RPC.Destination.All, pickup.gameObject, "PickedUp", new Il2CppSystem.Object[0]);
                        Networking.RPC(RPC.Destination.All, pickup.gameObject, "OnPickup", new Il2CppSystem.Object[0]);
                        pickup.gameObject.GetComponent<VRC.SDKBase.VRC_Trigger>().OnPickup();
                        pickup.transform.position = new Vector3(Player.transform.position.x + a * (float)Math.Cos(alpha), Player.transform.position.y + 0.3f, Player.transform.position.z + b * (float)Math.Sin(alpha));
                        pickup.transform.rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);
                        yield return new WaitForSeconds(0.01f);
                        Networking.RPC(RPC.Destination.All, pickup.gameObject, "PenDown", new Il2CppSystem.Object[0]);
                        Networking.RPC(RPC.Destination.All, pickup.gameObject, "OnPuckupUseDown", new Il2CppSystem.Object[0]);
                        Networking.RPC(RPC.Destination.All, pickup.gameObject, "Event_OnPuckupUseDown", new Il2CppSystem.Object[0]);
                        Networking.RPC(RPC.Destination.All, pickup.gameObject, "DrawStart", new Il2CppSystem.Object[0]);
                        int num;
                        for (int i = 0; i < 95; i = num + 1)
                        {
                            alpha += Time.deltaTime * CircleSpeed;
                            pickup.gameObject.GetComponent<VRC.SDKBase.VRC_Trigger>().OnPickupUseDown();
                            pickup.transform.position = new Vector3(Player.transform.position.x + a * (float)Math.Cos(alpha), Player.transform.position.y + 0.3f, Player.transform.position.z + b * (float)Math.Sin(alpha));
                            yield return new WaitForSeconds(0.01f);
                            num = i;
                        }
                        Networking.RPC(RPC.Destination.All, pickup.gameObject, "PenUp", new Il2CppSystem.Object[0]);
                        Networking.RPC(RPC.Destination.All, pickup.gameObject, "OnPuckupUseUp", new Il2CppSystem.Object[0]);
                        Networking.RPC(RPC.Destination.All, pickup.gameObject, "Event_OnPuckupUseUp", new Il2CppSystem.Object[0]);
                        Networking.RPC(RPC.Destination.All, pickup.gameObject, "DrawEnd", new Il2CppSystem.Object[0]);
                        pickup.gameObject.GetComponent<VRC.SDKBase.VRC_Trigger>().OnPickupUseUp();
                        yield return new WaitForSeconds(0.01f);
                        pickup.transform.position = OriginalPosition;
                        pickup.transform.rotation = OriginalRotation;
                        Networking.RPC(RPC.Destination.All, pickup.gameObject, "Dropped", new Il2CppSystem.Object[0]);
                        Networking.RPC(RPC.Destination.All, pickup.gameObject, "OnDrop", new Il2CppSystem.Object[0]);
                    }
                }
            }
            yield break;
        }

        public static IEnumerator ItemRotate(Player player)
        {
            for (; ; )
            {
                if (!ItemOrbitEnabled)
                {
                    yield break;
                }
                GameObject gameObject = new GameObject();
                Transform transform = gameObject.transform;
                transform.position = player.transform.position + new Vector3(0f, 0.2f, 0f);
                gameObject.transform.Rotate(new Vector3(0f, 360f * Time.time, 0f));
                try
                {
                    foreach (VRC_ObjectSync obj in World_ObjectSyncs)
                    {
                        TakeOwnershipIfNecessary(obj.gameObject);
                        obj.transform.position = gameObject.transform.position + gameObject.transform.forward;
                        obj.transform.LookAt(player.transform);
                        gameObject.transform.Rotate(new Vector3(0f, 360 / World_ObjectSyncs.Count, 0f));
                    }
                }
                catch {}
                UnityEngine.Object.Destroy(gameObject);
                yield return new WaitForSeconds(0.035f);
            }
            yield break;
        }
        public static bool ItemOrbitEnabled = false;

        public static List<string> PenNames = new List<string>()
        {
            "pen",
            "marker",
            "grip"
        };

        public static IEnumerator SpreadVirus()
        {
            List<VRCSDK2.VRC_Pickup> AllPickups = UnityEngine.Object.FindObjectsOfType<VRCSDK2.VRC_Pickup>().ToList();
            if (AllPickups != null)
            {
                foreach (var Pickup in AllPickups)
                {
                    foreach (var PenName in PenNames)
                    {
                        if (Pickup.name.ToLower().Contains(PenName) && !Pickup.transform.parent.name.ToLower().Contains("eraser"))
                        {
                            TakeOwnershipIfNecessary(Pickup.gameObject);
                            Pickup.transform.position = new Vector3(0, 0, 0);
                            Pickup.gameObject.GetComponent<VRC.SDKBase.VRC_Trigger>().OnPickup();
                        }
                    }
                }
                float a = 0f;
                float b = 0f;
                float y = 0f;
                for (int i = 0; i < 2000; i++)
                {
                    foreach (var Pickup in AllPickups)
                    {
                        foreach (var PenName in PenNames)
                        {
                            if (Pickup.name.ToLower().Contains(PenName) && !Pickup.transform.parent.name.ToLower().Contains("eraser"))
                            {
                                Pickup.gameObject.GetComponent<VRC.SDKBase.VRC_Trigger>().OnPickupUseDown();
                                yield return new WaitForSeconds(0.01f);
                                float CircleSpeed = 9999;
                                float alpha = 0f;
                                Pickup.transform.rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);
                                for (int x = 0; x < 4000; x++)
                                {
                                    alpha += Time.deltaTime * CircleSpeed;
                                    Pickup.transform.position = new Vector3(0 + a * (float)System.Math.Cos(alpha), 0 + 0.3f, 0 + b * (float)System.Math.Sin(alpha));
                                }
                                a += 0.003f;
                                b += 0.003f;
                            }
                        }
                    }
                }
            }
        }


        public static IEnumerator SpreadVirus2()
        {
            List<VRCSDK2.VRC_Pickup> AllPickups = UnityEngine.Object.FindObjectsOfType<VRCSDK2.VRC_Pickup>().ToList();
            if (AllPickups != null)
            {
                float a = 0f;
                float b = 0f;
                float y = 0.5f;
                foreach (var Pickup in AllPickups)
                {
                    foreach (var PenName in PenNames)
                    {
                        if (Pickup.name.ToLower().Contains(PenName))
                        {
                            TakeOwnershipIfNecessary(Pickup.gameObject);
                            Pickup.transform.position = new Vector3(0, 0, 0);
                            Pickup.gameObject.GetComponent<VRC.SDKBase.VRC_Trigger>().OnPickup();
                            IEnumerator Orbit()
                            {
                                while (true)
                                {
                                    yield return new WaitForEndOfFrame();
                                    if (Pickup != null)
                                    {
                                        GameObject gameObject = new GameObject();
                                        Transform transform = gameObject.transform;

                                        transform.position = new Vector3(0f, y, 0f);
                                        gameObject.transform.Rotate(new Vector3(0f, 360f * Time.time, 0f));
                                        Pickup.transform.position = gameObject.transform.position + gameObject.transform.forward;
                                        gameObject.transform.Rotate(new Vector3(0f, 360 / AllPickups.Count, 0f));
                                        UnityEngine.Object.Destroy(gameObject);
                                        gameObject = null;
                                        transform = null;
                                    }
                                    else yield break;
                                }
                            }
                            MelonCoroutines.Start(Orbit());
                        }
                    }
                }
                for (int i = 0; i < 2000; i++)
                {
                    foreach (var Pickup in AllPickups)
                    {
                        foreach (var PenName in PenNames)
                        {
                            if (Pickup.name.ToLower().Contains(PenName))
                            {
                                Pickup.gameObject.GetComponent<VRC.SDKBase.VRC_Trigger>().OnPickupUseDown();
                                yield return new WaitForSeconds(0.01f);
                                float CircleSpeed = 9999;
                                float alpha = 9999f;
                                Pickup.transform.rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);
                                for (int x = 0; x < 4000; x++)
                                {
                                    alpha += Time.deltaTime * CircleSpeed;
                                    Pickup.transform.position = new Vector3(0 + a * (float)System.Math.Cos(alpha), 0 + 0.3f, 0 + b * (float)System.Math.Sin(alpha));
                                }
                                a += 0.003f;
                                b += 0.003f;
                                y += 0.001f;
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(1);
                MelonCoroutines.Start(SpreadVirus2());
            }
        }

        public static IEnumerator Tornado()
        {
            List<VRCSDK2.VRC_Pickup> AllPickups = UnityEngine.Object.FindObjectsOfType<VRCSDK2.VRC_Pickup>().ToList();
            if (AllPickups != null)
            {
                foreach (var Pickup in AllPickups)
                {
                    TakeOwnershipIfNecessary(Pickup.gameObject);
                    Pickup.transform.position = new Vector3(0, 0, 0);
                    Pickup.gameObject.GetComponent<VRC.SDKBase.VRC_Trigger>().OnPickup();
                }
                float a = 0f;
                float b = 0f;
                float y = 0.5f;
                for (int i = 0; i < 50; i++)
                {
                    foreach (var Pickup in AllPickups)
                    {
                        foreach (var PenName in PenNames)
                        {
                            if (Pickup.name.ToLower().Contains(PenName))
                            {
                                Pickup.gameObject.GetComponent<VRC.SDKBase.VRC_Trigger>().OnPickupUseDown();
                                yield return new WaitForSeconds(0.001f);
                                float CircleSpeed = 20;
                                float alpha = 0f;
                                Pickup.transform.rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);
                                for (int x = 0; x < 95; x++)
                                {
                                    alpha += Time.deltaTime * CircleSpeed;
                                    Pickup.transform.position = new Vector3(0 + a * (float)System.Math.Cos(alpha), y, 0 + b * (float)System.Math.Sin(alpha));
                                    yield return new WaitForSeconds(0.001f);
                                }
                                a += 0.03f;
                                b += 0.03f;
                                if (y < 5)
                                {
                                    Pickup.transform.position = new Vector3(0, 0.5f, 0);
                                    y += 0.03f;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}


