using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WengaPort.Modules
{
    class AttachmentManager : MonoBehaviour 
    {

        public void Update()
        {
            if (RoomManager.field_Internal_Static_ApiWorld_0 == null || TransformParent == null) return;

            Utils.CurrentUser.transform.position = TransformParent.position;

        }
        internal static void SetAttachment(Transform Instance)
        {
            TransformParent = Instance;
        }
        internal static void SetAttachment(VRCPlayer Instance)
        {
            TransformParent = Instance.gameObject.transform;
        }
        internal static void SetAttachment(VRCPlayer Instance, HumanBodyBones bone)
        {
            TransformParent = Instance.gameObject.GetComponent<Animator>().GetBoneTransform(bone);
        }
        internal static void SetOffset(Vector3 Offset)
        {
            XOffset = Offset.x;
            YOffset = Offset.y;
            ZOffset = Offset.z;
        }
        private static Transform TransformParent;
        private static float XOffset = 0;
        private static float YOffset = 0;
        private static float ZOffset = 0;
        public AttachmentManager(IntPtr ptr) : base(ptr) { }
    }
}
