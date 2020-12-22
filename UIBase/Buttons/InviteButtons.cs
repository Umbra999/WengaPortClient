using System;
using System.Collections.Generic;
using UnityEngine;
using MelonLoader;
using WengaPort.Api;
using WengaPort.Modules;
using VRC.Core;
using WengaPort.Extensions;

namespace WengaPort.Buttons
{
    class InviteButtons
    {

        public static QMSingleButton PortalToInvite;

        public static Transform NotificationMenu = Utils.QuickMenu.transform.Find("NotificationInteractMenu");
        public static GameObject NotificationMenuOBJ = Utils.QuickMenu.transform.Find("NotificationInteractMenu").gameObject;

        public static void InviteButtonInit()
        {
            PortalToInvite = new QMSingleButton("ShortcutMenu", 4, 0, "Drop\nPortal", new Action(() =>
            {
                string ID = "";
                Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>.Enumerator enumerator = Utils.QuickMenu.Notification().details.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string a = enumerator.current.Key;
                    if (a.ToLower() == "worldid")
                    {
                        Il2CppSystem.Object b = enumerator.current.value;
                        ID = b.ToString();
                    }
                }
                PortalHandler.DropPortal(ID);
            }), "Drop a Portal to the Invite");
            PortalToInvite.SetParent(NotificationMenu);

            PortalToInvite = new QMSingleButton("ShortcutMenu", 4, 1, "Check\nInstance", new Action(() =>
            {
                if (PhotonMenu.Ini.GetBool("Toggles", "Bot", true))
                {
                    string ID = "";
                    Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>.Enumerator enumerator = Utils.QuickMenu.Notification().details.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        string a = enumerator.current.Key;
                        if (a.ToLower() == "worldid")
                        {
                            Il2CppSystem.Object b = enumerator.current.value;
                            ID = b.ToString();
                        }
                    }
                    PhotonMenu.Ini.SetString("Toggles", "SocialInstance", ID);
                    PhotonMenu.Ini.SetBool("Toggles", "WhoInInstance", true);
                }
                else
                {
                    Extensions.Logger.WengaLogger("Start the Photonbots before using this feature");
                }
            }), "Check the Instance for Players");
            PortalToInvite.SetParent(NotificationMenu);
        }
    }
}
