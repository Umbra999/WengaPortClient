﻿using UnityEngine;
using MelonLoader;
using WengaPort.Api;
using WengaPort.Wrappers;
using WengaPort.Buttons;
using UnityEngine.UI;
using WengaPort.Modules;
using System;
using VRC;
using System.Linq;
using VRCSDK2;
using System.Collections.Generic;
using VRC.SDKBase;
using WengaPort.Extensions;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using Logger = WengaPort.Extensions.Logger;

namespace WengaPort.Buttons
{
    class DebugMenu
    {
        public static QMNestedButton ThisMenu;
        public static QMSingleButton HalfButton;
        public static QMSingleButton LittleSquare;
        public static QMToggleButton HalfToggleButton;

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(MainMenu.ThisMenu, 3.5f, 2f, "Debug", "Debug Utils", null, null, null, Color.yellow);
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMToggleButton(ThisMenu, 1, 0, "Event \nLog", () =>
            {
                PatchManager.EventLog = true;
            }, "Disabled", () =>
            {
                PatchManager.EventLog = false;
            }, "Toggle Event Logging");

            new QMToggleButton(ThisMenu, 2, 0, "RPC \nLog", () =>
            {
                PatchManager.RPCLog = true;
            }, "Disabled", () =>
            {
                PatchManager.RPCLog = false;
            }, "Toggle RPC Logging", Color.cyan, Color.white, false, true);

            new QMToggleButton(ThisMenu, 3, 0, "API \nLog", () =>
            {
                ApiExtension.ApiNotify = true;
            }, "Disabled", () =>
            {
                ApiExtension.ApiNotify = false;
            }, "Toggle API Logging", Color.cyan, Color.white, false, true);

            new QMToggleButton(ThisMenu, 4, 0, "API \nExtension", () =>
            {
                ApiExtension.ApiConsole = true;
            }, "Disabled", () =>
            {
                ApiExtension.ApiConsole= false;
            }, "Toggle More API Deatails Logging");

            new QMToggleButton(ThisMenu, 1, 1, "Event \nExtension", () =>
            {
                PatchManager.DictLog = true;
            }, "Disabled", () =>
            {
                PatchManager.DictLog = false;
            }, "Toggle More Event Logging");

            new QMToggleButton(ThisMenu, 2, 1, "Operation \nLog", () =>
            {
                PatchManager.OperationLog = true;
            }, "Disabled", () =>
            {
                PatchManager.OperationLog = false;
            }, "Toggle Operation Logging");

            new QMToggleButton(ThisMenu, 3, 1, "World \nTravel", () =>
            {
                Modules.Photon.WorldTravel = true;
                MelonCoroutines.Start(Modules.Photon.RankUp());
            }, "Disabled", () =>
            {
                Modules.Photon.WorldTravel = false;
            }, "Spoof your location to different worlds");
        }
    }
}
