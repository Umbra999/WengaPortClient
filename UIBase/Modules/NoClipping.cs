using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRC.UserCamera;
using WengaPort.Extensions;
using WengaPort.Wrappers;

namespace WengaPort.Modules
{
    class NoClipping
    {
        public static void ChangeNearClipPlane(float value)
        {
            VRCVrCamera vrCamera = VRCVrCamera.field_Private_Static_VRCVrCamera_0;
            if (!vrCamera)
                return;
            Camera screenCamera = vrCamera.screenCamera;
            if (!screenCamera)
                return;
            screenCamera.nearClipPlane = value;
            ChangePhotoCameraNearField(value);
        }
        public static void ChangePhotoCameraNearField(float value)
        {
            var cameraController = UserCameraController.field_Internal_Static_UserCameraController_0;
            if (cameraController == null)
                return;
            Camera cam = cameraController.photoCamera.GetComponent<Camera>();
            if (cam != null)
                cam.nearClipPlane = value;
        }

        public static IEnumerator SetNearClipPlane(float znear)
        {
            yield return new WaitForSecondsRealtime(16);
            Extensions.Logger.WengaLogger("[Utils] Clipping Adjusted");
            ChangeNearClipPlane(znear);
        }
    }
}
