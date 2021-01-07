using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using WengaPort.Wrappers;

namespace WengaPort.Modules
{
    class AvatarHider : MonoBehaviour
    {
        public void Update()
        {
            HideDelay += Time.deltaTime;
            if (HideDelay > 2f)
            {
                try
                {
                    foreach (Player p in Utils.PlayerManager.GetAllPlayers())
                    {
                        GameObject avtrObject = p.GetAvatarObject();
                        if (p != Utils.CurrentUser && p != null && avtrObject != null && !PlayerList.BlockList.Contains(p.UserID()))
                        {
                            float dist = Vector3.Distance(Utils.CurrentUser.transform.position, avtrObject.transform.position);
                            bool isActive = p.GetAvatarObject().active;

                            if (isActive && dist > 32)
                            {
                                avtrObject.SetActive(false);
                            }
                            else if (!isActive && dist < 30)
                            {
                                avtrObject.SetActive(true);
                            }
                        }
                    }
                }
                catch { }
                HideDelay = 0f;
            }
        }
        public float HideDelay = 0f;
        public AvatarHider(IntPtr ptr) : base(ptr) { }
    }
}
