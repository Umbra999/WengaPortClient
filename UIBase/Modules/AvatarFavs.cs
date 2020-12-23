using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using MelonLoader;
using System.Collections;
using VRC.Core;
using UnhollowerBaseLib;
using VRC.UI;
using WengaPort.Utility;
using WengaPort.Modules;
using WengaPort.Api;
using WengaPort.Api.Object;
using System.IO;
using Newtonsoft.Json;

namespace WengaPort.Modules
{
    class AvatarFavs : MonoBehaviour
    {
        public void Start()
        {
            Extensions.Logger.WengaLogger("[AvatarFavs] Start. . .");
            avatarPage = GameObject.Find("UserInterface/MenuContent/Screens/Avatar");
            PublicAvatarList = GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Vertical Scroll View/Viewport/Content/Public Avatar List");
            currPageAvatar = avatarPage.GetComponent<PageAvatar>();
            new MenuButton(Utility.MenuType.AvatarMenu, MenuButtonType.PlaylistButton, "Fav/Unfav", -600f, 375, delegate 
            {
                if(!AvatarObjects.Exists(m => m.id == currPageAvatar.avatar.field_Internal_ApiAvatar_0.id))
                {
                    FavoriteAvatar(currPageAvatar.avatar.field_Internal_ApiAvatar_0);
                }
                else
                {
                    UnfavoriteAvatar(currPageAvatar.avatar.field_Internal_ApiAvatar_0);
                }
               MelonCoroutines.Start(RefreshMenu(1));
            });
            AvatarList = new VRCList(PublicAvatarList.transform.parent, "WengaPort Favorites", 0);
            AvatarObjects = JsonConvert.DeserializeObject<List<AvatarObject>>(File.ReadAllText("WengaPort\\AvatarFavorites.json"));
            Extensions.Logger.WengaLogger("[AvatarFavs] Done!");
        } 

        public void Update()
        {
            try
            {

                if (avatarPage.activeSelf && !JustOpened)
                {
                    JustOpened = true;
                    MelonCoroutines.Start(RefreshMenu(1f));
                }

                if (!avatarPage.activeSelf && JustOpened)
                    JustOpened = false;
            }
            catch { }
        }

        private static IEnumerator RefreshMenu(float v)
        {
            yield return new WaitForSeconds(v);
            var avilist = new Il2CppSystem.Collections.Generic.List<ApiAvatar>();
            AvatarObjects.ForEach(avi => avilist.Add(avi.ToApiAvatar()));
            AvatarList.RenderElement(avilist);
            yield break;
        }
       
        internal static void FavoriteAvatar(ApiAvatar avatar)
        {
            if (!AvatarObjects.Exists(avi => avi.id == avatar.id))
                AvatarObjects.Insert(0, new AvatarObject(avatar));
            MelonCoroutines.Start(RefreshMenu(1f));
            string contents = JsonConvert.SerializeObject(AvatarObjects, Formatting.Indented);
            File.WriteAllText("WengaPort\\AvatarFavorites.json", contents);
        }
        internal static void UnfavoriteAvatar(ApiAvatar avatar)
        {
            if(AvatarObjects.Exists(avi => avi.id == avatar.id))
            {
                AvatarObjects.Remove(AvatarObjects.Where(avi => avi.id == avatar.id).FirstOrDefault());
            }
            MelonCoroutines.Start(RefreshMenu(1f));
            string contents = JsonConvert.SerializeObject(AvatarObjects, Formatting.Indented);
            File.WriteAllText("WengaPort\\AvatarFavorites.json", contents);
        }

        private static VRCList AvatarList;
        private static List<AvatarObject> AvatarObjects = new List<AvatarObject>();
        private bool JustOpened = false;
        private static GameObject avatarPage;
        private static PageAvatar currPageAvatar;
        private static GameObject PublicAvatarList;
        public AvatarFavs(IntPtr ptr) : base(ptr) {}
    }
}
