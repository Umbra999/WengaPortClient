using WengaPort.Api;
using WengaPort.Wrappers;
using UnityEngine;
using UnityEngine.UI;
using WengaPort.Modules;
using System.Linq;
using VRC.SDKBase;
using System;

namespace WengaPort.Buttons
{
    class MovementMenu
    {
        public static QMNestedButton ThisMenu;

        public static void Initialize()
        {
            ThisMenu = new QMNestedButton(MainMenu.ThisMenu, 3.5f, 0f, "Movement", "Movement Menu", null, null, null, Color.yellow);
            ThisMenu.getMainButton().getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1, 2);

            new QMToggleButton(ThisMenu, 1, 0, "Speed", () =>
            {
                Movement.IncreaseSpeed();
            }, "Disabled", () =>
            {
                Movement.DecreaseSpeed();
            }, "Toggle Speed");

            new QMToggleButton(ThisMenu, 2, 0, "Inf \nJump", () =>
            {
                Movement.InfJump = true;
            }, "Disabled", () =>
            {
                Movement.InfJump = false;
            }, "Jump infinite High");
        }
    }
}
