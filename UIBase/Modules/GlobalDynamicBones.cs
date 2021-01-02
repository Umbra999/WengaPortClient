using WengaPort.ConsoleUtils;
using System.Collections.Generic;
using System.Linq;
using WengaPort.Utility;
using UnityEngine;
using VRC.SDKBase;
using VRC;
using WengaPort.Wrappers;

namespace WengaPort.Modules
{
    public class GlobalDynamicBones
    {
        public static List<string> UsersBones = new List<string>();
        public static List<string> FriendOnlyBones = new List<string>();

        public static void ProcessDynamicBones(GameObject avatarObject, VRCPlayer player)
        {
            if (UsersBones.Contains(player.UserID()) || GlobalBones || player.UserID() == Utils.CurrentUser.UserID() || FriendOnlyBones.Contains(player.UserID()) && FriendBones)
            {
                //Bones
                foreach (DynamicBone item2 in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.Head).GetComponentsInChildren<DynamicBone>())
                {
                    currentWorldDynamicBones.Add(item2);
                }

                foreach (DynamicBone item2 in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.Chest).GetComponentsInChildren<DynamicBone>())
                {
                    currentWorldDynamicBones.Add(item2);
                }

                foreach (DynamicBone item2 in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.Hips).GetComponentsInChildren<DynamicBone>())
                {
                    currentWorldDynamicBones.Add(item2);
                }

                foreach (DynamicBone item2 in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.RightUpperLeg).GetComponentsInChildren<DynamicBone>())
                {
                    currentWorldDynamicBones.Add(item2);
                }

                foreach (DynamicBone item2 in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.LeftUpperLeg).GetComponentsInChildren<DynamicBone>())
                {
                    currentWorldDynamicBones.Add(item2);
                }

                //Colliders
                foreach (DynamicBoneCollider dynamicBoneCollider in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.LeftHand).GetComponentsInChildren<DynamicBoneCollider>())
                {
                    if (dynamicBoneCollider.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside && dynamicBoneCollider.m_Radius <= 2f && dynamicBoneCollider.m_Height <= 2f)
                    {
                        currentWorldDynamicBoneColliders.Add(dynamicBoneCollider);
                    }
                }
                foreach (DynamicBoneCollider dynamicBoneCollider2 in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.RightHand).GetComponentsInChildren<DynamicBoneCollider>())
                {
                    if (dynamicBoneCollider2.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside && dynamicBoneCollider2.m_Radius <= 2f && dynamicBoneCollider2.m_Height <= 2f)
                    {
                        currentWorldDynamicBoneColliders.Add(dynamicBoneCollider2);
                    }
                }

                foreach (DynamicBoneCollider dynamicBoneCollider3 in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.LeftFoot).GetComponentsInChildren<DynamicBoneCollider>())
                {
                    if (dynamicBoneCollider3.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside && dynamicBoneCollider3.m_Radius <= 2f && dynamicBoneCollider3.m_Height <= 2f)
                    {
                        currentWorldDynamicBoneColliders.Add(dynamicBoneCollider3);
                    }
                }
                foreach (DynamicBoneCollider dynamicBoneCollider4 in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.RightFoot).GetComponentsInChildren<DynamicBoneCollider>())
                {
                    if (dynamicBoneCollider4.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside && dynamicBoneCollider4.m_Radius <= 2f && dynamicBoneCollider4.m_Height <= 2f)
                    {
                        currentWorldDynamicBoneColliders.Add(dynamicBoneCollider4);
                    }
                }

                foreach (DynamicBone dynamicBone in currentWorldDynamicBones.ToList())
                {
                    if (dynamicBone == null)
                    {
                        currentWorldDynamicBones.Remove(dynamicBone);
                    }
                    else
                    {
                        foreach (DynamicBoneCollider dynamicBoneCollider5 in currentWorldDynamicBoneColliders.ToList())
                        {
                            if (dynamicBoneCollider5 == null)
                            {
                                currentWorldDynamicBoneColliders.Remove(dynamicBoneCollider5);
                            }
                            else if (dynamicBone.m_Colliders.IndexOf(dynamicBoneCollider5) == -1)
                            {
                                dynamicBone.m_Colliders.Add(dynamicBoneCollider5);
                            }
                        }
                    }
                }
            }
        }

        public static List<DynamicBone> currentWorldDynamicBones = new List<DynamicBone>();
        public static List<DynamicBoneCollider> currentWorldDynamicBoneColliders = new List<DynamicBoneCollider>();

        public static bool GlobalBones = false;
        public static bool FriendBones = true;
        public static bool OptimizeBones = false;

        public static void OptimizeBone(GameObject AvatarGameobject)
        {
            foreach (DynamicBone item2 in AvatarGameobject.GetComponentInChildren<Animator>().GetComponentsInChildren<DynamicBone>())
            {
                item2.m_DistantDisable = true;
                if (OptimizeBones && item2.m_UpdateRate >= 45)
                {
                    item2.m_UpdateRate = 25;
                }
                else if (item2.m_UpdateRate >= 60)
                {
                    item2.m_UpdateRate = 45;
                }
            }
        }
    }
}