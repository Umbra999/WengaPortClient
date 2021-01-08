using MelonLoader;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.UI;
using System.Collections.Generic;
using System.Diagnostics;
using Il2CppSystem.Collections.Generic;
using UnhollowerBaseLib.Attributes;

namespace WengaPort.Modules.Reupload
{
    public class ReuploaderButtons : MonoBehaviour
    {

        public static Delegate ReferencedDelegate;

        public static IntPtr MethodInfo;

        public static  Il2CppSystem.Collections.Generic.List<MonoBehaviour> AntiGcList;

        public static bool isPrivate = true;

        public static volatile bool ActionsBool;

        public static  System.Collections.Generic.List<Action> Actions = new System.Collections.Generic.List<Action>(10);

        public static  System.Collections.Generic.List<Action> list_2 = new System.Collections.Generic.List<Action>(10);

        private static string NewAvatarID = string.Empty;

        private static Player player_0;

        private static VRCPlayer vrcplayer_0;

        private static APIUser apiuser_0;

        private static VRCAvatarManager vrcavatarManager_0;

        private static ApiAvatar apiAvatar_0;

        private static ApiAvatar apiAvatar_1;

        private static ApiWorld apiWorld_0;

        private static ApiFile apiFile_0;

        private static ApiFile apiFile_1;

        private static string string_2 = string.Empty;

        public static volatile string string_3 = string.Empty;

        private static string string_4 = string.Empty;

        private static string string_5 = string.Empty;

        private static ApiWorld apiWorld_1;

        public static volatile string string_6 = string.Empty;

        public static volatile string string_7 = string.Empty;

        private static string string_8 = string.Empty;

        public static volatile string ReuploadFilePath = string.Empty;

        public static volatile string string_10 = string.Empty;


        private static string AssetBundlePath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "AssetBundles");

        private static string VrcaStorePath = Path.Combine(AssetBundlePath, "VrcaStore");

        private static string UBPUPProgram = "UBPU.exe";

        private static string UBUPPath = Path.Combine(AssetBundlePath, UBPUPProgram);

        private static string ReuploaderModDataPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "ReuploaderModData");

        private static string NameTxtFile = "name.txt";

        private static string NameTxtPath = Path.Combine(ReuploaderModDataPath, NameTxtFile);

        private static string string_18 = string.Empty;

        private static Assembly assembly_0;

        private static Type type_0;

        private static MethodInfo methodInfo_0;

        private static string string_19 = string.Empty;

        private static string string_20 = string.Empty;

        public ReuploaderButtons(IntPtr intptr_1)
            : base(intptr_1)
        {
            AntiGcList = new Il2CppSystem.Collections.Generic.List<MonoBehaviour>(1);
            AntiGcList.Add(this);
        }

        public ReuploaderButtons(Delegate delegate_1, IntPtr intptr_1)
            : base(ClassInjector.DerivedConstructorPointer<ReuploaderButtons>())
        {
            ClassInjector.DerivedConstructorBody(this);
            ReferencedDelegate = delegate_1;
            MethodInfo = intptr_1;
        }

        ~ReuploaderButtons()
        {
            Marshal.FreeHGlobal(MethodInfo);
            MethodInfo = IntPtr.Zero;
            ReferencedDelegate = null;
            AntiGcList.Remove(this);
            AntiGcList = null;
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr intptr_1);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string string_21, string string_22);

        private void FocusConsole()
        {
            SetForegroundWindow(GetConsoleWindow());
        }

        private void ForceConsolefocus()
        {
            IntPtr intPtr = FindWindow(null, "VRChat");
            if (intPtr != IntPtr.Zero)
            {
                SetForegroundWindow(intPtr);
            }
        }
        
        public void Start()
        {
            if (Directory.Exists(AssetBundlePath))
	        {
		Directory.EnumerateDirectories(AssetBundlePath).ToList().ForEach(delegate(string string_0)
		{
			try
			{
				if (string_0.EndsWith("_dump"))
				{
					Directory.Delete(string_0, recursive: true);
				}
			}
			catch (Exception)
			{
			}
		});
		Directory.EnumerateFiles(AssetBundlePath).ToList().ForEach(delegate(string string_0)
		{
			try
			{
				File.Delete(string_0);
			}
			catch (Exception)
			{
			}
		});
		Directory.EnumerateFiles(VrcaStorePath).ToList().ForEach(delegate(string string_0)
		{
			try
			{
				File.Delete(string_0);
			}
			catch (Exception)
			{
			}
		});
	}
	else
	{
		Directory.CreateDirectory(AssetBundlePath);
		Directory.CreateDirectory(VrcaStorePath);
	}
}
        public void Update()
        {
            if (ActionsBool)
            {
                lock (Actions)
                {
                    System.Collections.Generic.List<Action> list = list_2;
                    list_2 = Actions;
                    Actions = list;
                    ActionsBool = false;
                }
                foreach (Action item in list_2)
                {
                    item();
                }
                list_2.Clear();
            }
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    ReuploadSelectedAvatar();
                }
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    FocusConsole();
                    MelonLogger.Log("Input avatarId:");
                    string string_ = Console.ReadLine();
                    ForceConsolefocus();
                    ReuploadAvatar(string_);
                }
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    ReuploadWorld(RoomManager.field_Internal_Static_ApiWorld_0.id);
                }
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    FocusConsole();
                    MelonLogger.Log("Input worldId:");
                    string string_2 = Console.ReadLine();
                    ForceConsolefocus();
                    ReuploadWorld(string_2);
                }
            }
        }

        public static void RegisterAction(Action action_0)
        {
            lock (Actions)
            {
                Actions.Add(action_0);
                ActionsBool = true;
            }
        }

        private void ReuploadWorldAction()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.menuContent.transform.Find("Screens/WorldInfo").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageWorldInfo component = gameObject.GetComponent<PageWorldInfo>();
            if ((bool)component)
            {
                ApiWorld field_Private_ApiWorld_ = component.field_Private_ApiWorld_0;
                if (field_Private_ApiWorld_ != null)
                {
                    ReuploadWorld(field_Private_ApiWorld_.id);
                }
                else
                {
                    MelonLogger.LogError("Couldn't fetch APIWorld");
                }
            }
        }

        private void GetApiAvatar()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.menuContent.transform.Find("Screens/UserInfo").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageUserInfo component = gameObject.GetComponent<PageUserInfo>();
            if (!component)
            {
                return;
            }
            APIUser apiuser_0 = component.user;
            if (apiuser_0 == null)
            {
                return;
            }
            player_0 = PlayerManager.prop_PlayerManager_0.prop_ArrayOf_Player_0.ToList().FirstOrDefault((Player player_0) => (player_0.prop_APIUser_0 != null && player_0.prop_APIUser_0.id.Equals(apiuser_0.id)) ? true : false);
            if (!player_0)
            {
                MelonLogger.LogError("Couldn't fetch Player");
                return;
            }
            apiuser_0 = player_0.prop_APIUser_0;
            if (apiuser_0 != null)
            {
                vrcplayer_0 = player_0.prop_VRCPlayer_0;
                if (!vrcplayer_0)
                {
                    MelonLogger.LogError("Couldn't fetch VRCPlayer");
                    return;
                }
                vrcavatarManager_0 = vrcplayer_0.prop_VRCAvatarManager_0;
                if (!vrcavatarManager_0)
                {
                    MelonLogger.LogError("Couldn't fetch AvatarManager");
                    return;
                }
                apiAvatar_0 = vrcavatarManager_0.prop_ApiAvatar_0;
                if (apiAvatar_0 != null)
                {
                    ReuploadAvatar(apiAvatar_0.id);
                }
                else
                {
                    MelonLogger.LogError("Couldn't fetch ApiAvatar");
                }
            }
            else
            {
                MelonLogger.LogError("Couldn't fetch APIUser");
            }
        }

        public static void ReuploadSelectedAvatar()
        {
            player_0 = QuickMenu.prop_QuickMenu_0.field_Private_Player_0;
            if (!player_0)
            {
                player_0 = Player.prop_Player_0;
                if (!player_0)
                {
                    MelonLogger.LogError("Couldn't fetch player");
                    return;
                }
            }
            apiuser_0 = player_0.prop_APIUser_0;
            if (apiuser_0 == null)
            {
                MelonLogger.LogError("Couldn't fetch APIUser");
                return;
            }
            vrcplayer_0 = player_0.prop_VRCPlayer_0;
            if (!vrcplayer_0)
            {
                MelonLogger.LogError("Couldn't fetch VRCPlayer");
                return;
            }
            vrcavatarManager_0 = vrcplayer_0.prop_VRCAvatarManager_0;
            if (!vrcavatarManager_0)
            {
                MelonLogger.LogError("Couldn't fetch AvatarManager");
                return;
            }
            apiAvatar_0 = vrcavatarManager_0.prop_ApiAvatar_0;
            if (apiAvatar_0 != null)
            {
                ReuploadAvatar(apiAvatar_0.id);
            }
            else
            {
                MelonLogger.LogError("Couldn't fetch ApiAvatar");
            }
        }

        private void ChangeAvatarImage()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.menuContent.transform.Find("Screens/Avatar").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageAvatar component = gameObject.GetComponent<PageAvatar>();
            if (!component)
            {
                return;
            }
            SimpleAvatarPedestal avatar = component.avatar;
            if (!avatar)
            {
                return;
            }
            ApiAvatar apiAvatar_0 = avatar.field_Internal_ApiAvatar_0;
            if (apiAvatar_0 == null || apiAvatar_0.authorId != APIUser.CurrentUser.id)
            {
                return;
            }
            CursorLockMode lockState = Cursor.lockState;
            bool visible = Cursor.visible;
            MelonLogger.Log("Select new image:");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            string text = OpenFileWindows.OpenfileDialog();
            if (!string.IsNullOrEmpty(text))
            {
                Cursor.lockState = lockState;
                Cursor.visible = visible;
                MelonLogger.Log(text);
                string name = apiAvatar_0.name;
                string text2 = apiAvatar_0.unityVersion.ToLower();
                string text3 = apiAvatar_0.platform.ToLower();
                string text4 = ApiWorld.VERSION.ApiVersion.ToString().ToLower();
                if (string.IsNullOrEmpty(text4))
                {
                    text4 = "4";
                }
                string text5 = "Avatar - " + name + " - Image - " + text2 + "_" + text4 + "_" + text3 + "_Release";
                string value = ApiFile.ParseFileIdFromFileAPIUrl(apiAvatar_0.imageUrl);
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                ReuploaderComponent.smethod_0(text, value, text5, delegate (ApiFile apiFile_0, string string_0)
                {
                    apiAvatar_0.imageUrl = apiFile_0.GetFileURL();
                    apiAvatar_0.Save((Action<ApiContainer>)delegate
                    {
                        MelonLogger.Log("Successfully changed avatar image");
                    }, (Action<ApiContainer>)delegate
                    {
                        MelonLogger.LogError("Couldn't change avatar image (POST)");
                    });
                }, delegate (ApiFile apiFile_0, string string_0)
                {
                    MelonLogger.LogError("Couldn't change avatar image (UPLOAD), " + string_0);
                }, delegate
                {
                }, (ApiFile apiFile_0) => false);
            }
            else
            {
                MelonLogger.LogError("Couldn't open filedialog or path was null");
            }
        }
        
        internal static void ReuploadAvatar(string string_21)
        {
            if (string.IsNullOrEmpty(string_21))
            {
                MelonLogger.LogError("AvatarId was empty");
                return;
            }
            API.Fetch<ApiAvatar>(string_21, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
            {
                apiAvatar_0 = apiContainer_0.Model.Cast<ApiAvatar>();
                if (apiAvatar_0 == null)
                {
                    MelonLogger.LogError("Couldn't fetch ApiAvatar");
                }
                else
                {
                    NewAvatarID = GetAvatarID();
                    API.Fetch<ApiAvatar>(NewAvatarID, (Action<ApiContainer>)delegate
                    {
                        MelonLogger.LogError("AvatarId " + NewAvatarID + " already in use!");
                        ReuploadAvatar(string_21);
                    }, (Action<ApiContainer>)delegate
                    {
                        Task.Run(async delegate
                        {
                            await Task.Delay(1000);

                            MelonLogger.Log("AvatarId: " + apiAvatar_0.id + " | AssetUrl: " + apiAvatar_0.assetUrl + " | Author: " + apiAvatar_0.authorName);
                            try
                            {
                                string text2 = await DownloadAvatar(apiAvatar_0);
                                ReuploadFilePath = text2;
                                if (!string.IsNullOrEmpty(ReuploadFilePath))
                                {
                                    MelonLogger.Log("DownloadAvatarSuccess");
                                    await GenerateAssetBundle(ReuploadFilePath);
                                    MelonLogger.Log("AssetBundle created");
                                    if (!string.IsNullOrEmpty(ReuploadFilePath))
                                    {
                                        string unityVersion = apiAvatar_0.unityVersion.ToLower();
                                        string platform = apiAvatar_0.platform.ToLower();
                                        string ApiVersion = ApiWorld.VERSION.ApiVersion.ToString().ToLower();
                                        if (string.IsNullOrEmpty(ApiVersion))
                                        {
                                            ApiVersion = "4";
                                        }
                                        string_20 = "Avatar - " + string_8 + " - Image - " + unityVersion + "_" + ApiVersion + "_" + platform + "_Release";
                                        string_19 = "Avatar - " + string_8 + " - Asset bundle - " + unityVersion + "_" + ApiVersion + "_" + platform + "_Release";
                                        MelonLogger.Log("AvatarNames generated!");
                                        MelonLogger.Log(string_20);
                                        MelonLogger.Log(string_19);
                                        if (!(await OwerWriteAssetBundle(NewAvatarID, apiAvatar_0.id)))
                                        {
                                            MelonLogger.LogError("Failed to set AvatarId!");
                                        }
                                        else if (await CompressAssetBundle())
                                        {
                                            RegisterAction(delegate
                                            {
                                                MelonLogger.Log("Uploading vrca");
                                                ReuploaderComponent.smethod_0(ReuploadFilePath, null, string_19, OnUploadVrcaAsyncSuccess, OnUploadVrcaAsyncFailure, delegate (ApiFile apiFile_0, string string_0, string string_1, float float_0)
                                                {
                                                    
                                                    if (float_0 >= 50f)
                                                    {
                                                        MelonLogger.Log("Uploading vrca 50% done");
                                                    }
                                                }, (ApiFile apiFile_0) => false);
                                            });
                                        }
                                        else
                                        {
                                            MelonLogger.LogError("Failed to recompress AssetBundle!");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MelonLogger.LogError(ex.ToString());
                            }
                        });
                    });
                }
            }, (Action<ApiContainer>)delegate
          {
              MelonLogger.LogError("Couldn't fetch avatar (API)");
          });
        }

        private static void OnUploadVrcaAsyncSuccess(ApiFile apiFile_2, string string_21)
        {
            MelonLogger.Log("OnUploadVrcaAsyncSuccess");
            apiFile_0 = apiFile_2;
            Task.Run(async delegate
            {
                string_10 = await DownloadImage(apiAvatar_0.imageUrl);
                if (!string.IsNullOrEmpty(string_10))
                {
                    MelonLogger.Log("DownloadAvatarImageSuccess");
                    RegisterAction(delegate
                    {
                        MelonLogger.Log("Uploading image");
                        ReuploaderComponent.smethod_0(string_10, null, string_20, OnUploadImageAsyncSuccess, OnUploadImageAsyncFailure, delegate (ApiFile apiFile_0, string string_0, string string_1, float float_0)
                        {
                            if (float_0 >= 50f)
                            {
                                MelonLogger.Log("Uploading image 50% done");
                            }
                        }, (ApiFile apiFile_0) => false);
                    });
                }
            });
        }

        private static void OnUploadVrcaAsyncFailure(ApiFile apiFile_2, string string_21)
        {
            MelonLogger.Log("OnUploadVrcaAsyncFailure");
        }

        private static void OnUploadImageAsyncSuccess(ApiFile apiFile_2, string string_21)
        {
            MelonLogger.Log("OnUploadImageAsyncSuccess");
            apiFile_1 = apiFile_2;
            RegisterAction(delegate
            {
                apiAvatar_1 = new ApiAvatar
                {
                    id = NewAvatarID,
                    authorName = APIUser.CurrentUser.username,
                    authorId = APIUser.CurrentUser.id,
                    name = string_8,
                    imageUrl = apiFile_1.GetFileURL(),
                    assetUrl = apiFile_0.GetFileURL(),
                    description = apiAvatar_0.description,
                    releaseStatus = (isPrivate ? "private" : "public")
                };
                apiAvatar_1.Post((Action<ApiContainer>)OnApiAvatarPostSuccess, (Action<ApiContainer>)OnApiAvatarPostFailure);
            });
        }

        private static void OnUploadImageAsyncFailure(ApiFile apiFile_2, string string_21)
        {
            MelonLogger.Log("OnUploadImageAsyncFailure");
        }

        private static void OnApiAvatarPostSuccess(ApiContainer apiContainer_0)
        {
            MelonLogger.Log("OnApiAvatarPostSuccess");
            if (!ClearOldSession())
            {
                MelonLogger.LogWarning("Error while cleaning up the AssetBundles directory, you can probably ignore this.");
            }
        }

        private static void OnApiAvatarPostFailure(ApiContainer apiContainer_0)
        {
            MelonLogger.Log("OnApiAvatarPostFailure");
            if (!ClearOldSession())
            {
                MelonLogger.LogWarning("Error while cleaning up the AssetBundles directory, you can probably ignore this.");
            }
        }

        private void ReuploadWorld(string worldPath)
        {
            if (string.IsNullOrEmpty(worldPath))
            {
                MelonLogger.LogError("WorldId was empty");
                return;
            }
            API.Fetch<ApiWorld>(worldPath, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
            {
                apiWorld_1 = apiContainer_0.Model.Cast<ApiWorld>();
                if (apiWorld_1 != null)
                {
                    string_5 = GetWorldID();
                    API.Fetch<ApiWorld>(string_5, (Action<ApiContainer>)delegate
                    {
                        MelonLogger.LogError("WorldId " + string_5 + " already in use!");
                        ReuploadWorld(worldPath);
                    }, (Action<ApiContainer>)delegate
                    {
                        Task.Run(async delegate
                        {
                            await Task.Delay(1000);
                            string text = string_6;
                            string_6 = string.Empty;
                            if (!string.IsNullOrEmpty(text) && !text.Equals("YES"))
                            {
                                MelonLogger.Log("WorldId: " + apiWorld_1.id + " | AssetUrl: " + apiWorld_1.assetUrl + " | Author: " + apiWorld_1.authorName);
                                try
                                {
                                    string text2 = await DownloadWorld(apiWorld_1);
                                    ReuploadFilePath = text2;
                                    if (!string.IsNullOrEmpty(ReuploadFilePath))
                                    {
                                        MelonLogger.Log("DownloadWorldSuccess");
                                        await GenerateAssetBundle(ReuploadFilePath);
                                        MelonLogger.Log("AssetBundle created");

                                        await Task.Delay(1000);
                                        if (!string.IsNullOrEmpty(string_4))
                                        {
                                            string text3 = apiWorld_1.unityVersion.ToLower();
                                            string text4 = apiWorld_1.platform.ToLower();
                                            string text5 = ApiWorld.VERSION.ApiVersion.ToString().ToLower();
                                            if (string.IsNullOrEmpty(text5))
                                            {
                                                text5 = "4";
                                            }
                                            string_20 = "World - " + string_4 + " - Image - " + text3 + "_" + text5 + "_" + text4 + "_Release";
                                            string_2 = "World - " + string_4 + " - Asset bundle - " + text3 + "_" + text5 + "_" + text4 + "_Release";
                                            MelonLogger.Log("WorldNames generated!");
                                            MelonLogger.Log(string_20);
                                            MelonLogger.Log(string_2);
                                            if (!(await OwerWriteAssetBundle(string_5, apiWorld_1.id)))
                                            {
                                                MelonLogger.LogError("Failed to set AvatarId!");
                                            }
                                            else if (!(await CompressAssetBundle()))
                                            {
                                                MelonLogger.LogError("Failed to recompress AssetBundle!");
                                            }
                                            else
                                            {
                                                RegisterAction(delegate
                                        {
                                            MelonLogger.Log("Uploading vrcw");
                                            ReuploaderComponent.smethod_0(ReuploadFilePath, null, string_2, OnUploadVrcwAsyncSuccess, OnUploadVrcwAsyncFailure, delegate (ApiFile apiFile_0, string string_0, string string_1, float float_0)
                                                            {
                                                                if (float_0 >= 50f)
                                                                {
                                                                    MelonLogger.Log("Uploading vrcw 50% done");
                                                                }
                                                            }, (ApiFile apiFile_0) => false);
                                        });
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MelonLogger.LogError(ex.ToString());
                                }
                            }
                        });
                    });
                }
                else
                {
                    MelonLogger.LogError("Couldn't fetch ApiWorld");
                }
            }, (Action<ApiContainer>)delegate
            {
                MelonLogger.LogError("Couldn't fetch world (API)");
            });
        }

        private void OnUploadVrcwAsyncSuccess(ApiFile apiFile_2, string string_21)
        {
            MelonLogger.Log("OnUploadVrcwAsyncSuccess");
            apiFile_0 = apiFile_2;
            Task.Run(async delegate
            {
                string_10 = await DownloadImage(apiWorld_1.imageUrl);
                if (!string.IsNullOrEmpty(string_10))
                {
                    MelonLogger.Log("OnUploadVrcwAsyncSuccess");
                    RegisterAction(delegate
                    {
                        MelonLogger.Log("Uploading image");
                        ReuploaderComponent.smethod_0(string_10, null, string_20, OnUploadVrcwAsynSuccess, OnUploadImageAsyncFailure, delegate (ApiFile apiFile_0, string string_0, string string_1, float float_0)
                        {
                            if (float_0 >= 50f)
                            {
                                MelonLogger.Log("Uploading image 50% done");
                            }
                        }, (ApiFile apiFile_0) => false);
                    });
                }
            });
        }

        private static void OnUploadVrcwAsyncFailure(ApiFile apiFile_2, string string_21)
        {
            MelonLogger.Log("OnUploadVrcwAsyncFailure");
        }

        private static void OnUploadVrcwAsynSuccess(ApiFile apiFile_2, string string_21)
        {
            MelonLogger.Log("OnUploadVrcwAsynSuccess");
            apiFile_1 = apiFile_2;
            RegisterAction(delegate
            {
                apiWorld_0 = new ApiWorld
                {
                    id = string_5,
                    authorName = APIUser.CurrentUser.username,
                    authorId = APIUser.CurrentUser.id,
                    name = string_4,
                    imageUrl = apiFile_1.GetFileURL(),
                    assetUrl = apiFile_0.GetFileURL(),
                    description = apiWorld_1.description,
                    releaseStatus = (isPrivate ? "private" : "public")
                };
                apiWorld_0.Post((Action<ApiContainer>)OnApiWorldPostSuccess, (Action<ApiContainer>)OnApiWorldPostFailure);
            });
        }

        private static void OnApiWorldPostSuccess(ApiContainer apiContainer_0)
        {
            MelonLogger.Log("OnApiWorldPostSuccess");
            if (!ClearOldSession())
            {
                MelonLogger.LogWarning("Error while cleaning up the AssetBundles directory, you can probably ignore this.");
            }
        }

        private static void OnApiWorldPostFailure(ApiContainer apiContainer_0)
        {
            MelonLogger.Log("OnApiWorldPostFailure");
            if (!ClearOldSession())
            {
                MelonLogger.LogWarning("Error while cleaning up the AssetBundles directory, you can probably ignore this.");
            }
        }

        private static bool ClearOldSession()
        {
            foreach (string item in Directory.EnumerateFiles(AssetBundlePath))
            {
                if (!item.EndsWith("UBPU.exe"))
                {
                    try
                    {
                        File.Delete(item);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            foreach (string item2 in Directory.EnumerateDirectories(AssetBundlePath))
            {
                if (!item2.EndsWith("VrcaStore"))
                {
                    try
                    {
                        Directory.Delete(item2, recursive: true);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            if (Directory.EnumerateFiles(AssetBundlePath).Count() == 1 && Directory.EnumerateDirectories(AssetBundlePath).Count() == 1)
            {
                return true;
            }
            return false;
        }

        private static string GetAvatarID()
        {
            return "avtr_" + Guid.NewGuid().ToString();
        }

        private string GetWorldID()
        {
            return "wrld_" + Guid.NewGuid().ToString();
        }



        private static int Dunnowtfisdis(byte[] byte_0, byte[] byte_1)
        {
            int num = 0;
            while (true)
            {
                if (num < byte_0.Length - byte_1.Length)
                {
                    bool flag = true;
                    for (int i = 0; i < byte_1.Length; i++)
                    {
                        if (byte_0[num + i] != byte_1[i])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                    num++;
                    continue;
                }
                return -1;
            }
            return num;
        }

        private static async Task<bool> OwerWriteAssetBundle(string string_21, string string_22)
        {
            await Task.Delay(100);
            try
            {
                byte[] array = File.ReadAllBytes(string_18);
                byte[] bytes = Encoding.ASCII.GetBytes(string_22);
                Encoding.ASCII.GetBytes(string_22.ToLower());
                byte[] bytes2 = Encoding.ASCII.GetBytes(string_21);
                if (!string_22.Contains("avtr_") && !string_22.Contains("wrld_"))
                {
                    MelonLogger.LogError("Custom avatar ids aren't supported");
                    return false;
                }
                byte[] array2 = new byte[array.Length + bytes2.Length - bytes.Length];
                byte[] array3 = array;
                int num;
                while ((num = Dunnowtfisdis(array3, bytes)) >= 0)
                {
                    Buffer.BlockCopy(array3, 0, array2, 0, num);
                    Buffer.BlockCopy(bytes2, 0, array2, num, bytes2.Length);
                    Buffer.BlockCopy(array3, num + bytes.Length, array2, num + bytes2.Length, array3.Length - num - bytes.Length);
                    array3 = array2;
                }
                File.WriteAllBytes(string_18, array2);
                MelonLogger.Log("AssetBundle overwritten!");
                return true;
            }
            catch (Exception ex)
            {
                MelonLogger.LogError(ex.ToString());
                return false;
            }
        }

        private static async Task<string> DownloadAvatar(ApiAvatar apiAvatar_2)
        {
            byte[] bytes = await new HttpClient().GetByteArrayAsync(apiAvatar_2.assetUrl);
            string text = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".vrca");
            File.WriteAllBytes(text, bytes);
            MelonLogger.Log("DownloadAvatar");
            return text;
        }

        private static async Task<string> DownloadWorld(ApiWorld apiWorld_2)
        {
            byte[] bytes = await new HttpClient().GetByteArrayAsync(apiWorld_2.assetUrl);
            string text = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".vrcw");
            File.WriteAllBytes(text, bytes);
            MelonLogger.Log("DownloadWorld");
            return text;
        }

        private static async Task<string> DownloadImage(string string_21)
        {
            HttpResponseMessage httpResponseMessage = await new HttpClient().GetAsync(string_21);
            byte[] array = await httpResponseMessage.Content.ReadAsByteArrayAsync();
            if (array == null || array.Length == 0)
            {
                MelonLogger.Log("image was null or 0");
            }
            string text = httpResponseMessage.Content.Headers.GetValues("Content-Type").First().Split('/')[1];
            MelonLogger.Log(text);
            string text2 = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + "." + text);
            MelonLogger.Log(text2);
            File.WriteAllBytes(text2, array);
            MelonLogger.Log("DownloadImage");
            return text2;
        }

        private static string GetDumpedVRCA()
        {
            string result = string.Empty;
            System.Collections.Generic.IEnumerable<string> enumerable = from string_0 in Directory.EnumerateFiles(ReuploadFilePath + "_dump")
                                                                        select new FileInfo(string_0) into fileInfo_0
                                                                        orderby fileInfo_0.CreationTime
                                                                        select fileInfo_0.FullName;
            if (enumerable.Count() != 1)
            {
                foreach (string item in enumerable)
                {
                    if (string.IsNullOrEmpty(Path.GetExtension(item)))
                    {
                        result = item;
                    }
                }
                return result;
            }
            return enumerable.ElementAt(0);
        }


        private static async Task GenerateAssetBundle(string downloadedpath)
        {
            if (File.Exists(downloadedpath))
            {
                string newpath = Path.Combine(AssetBundlePath, Path.GetFileName(downloadedpath));
                File.Move(downloadedpath, newpath);
                if (File.Exists(newpath))
                {
                    File.Delete(downloadedpath);
                    //UBPU.Program.Main(new string[] { newpath });
                    ReuploadFilePath = newpath;

                    MelonLogger.Log("Decompressing assetbundle..");
                    
                    await Task.Delay(1000);
                    MelonLogger.Log("Finished decompressing!");
                    ReuploadFilePath = newpath;
                    
                    string_18 = GetDumpedVRCA();
                    MelonLogger.Log(string_18);
                    File.Delete(newpath);
                    ReuploadFilePath = string_18;
                }
            }
        }


        private static async Task<bool> CompressAssetBundle()
        {
            try
            {
                string text = GetXMLFile();
                if (string.IsNullOrEmpty(text))
                {
                    return false;
                }
                MelonLogger.Log("Compressing assetbundle..");
                string directoryName = Path.GetDirectoryName(Application.dataPath);
                Directory.SetCurrentDirectory(AssetBundlePath);
                //UBPU.Program.Main(new string[2]
                //{
                //    text,
                //    "lz4hc"
                //});
                await Task.Delay(1000);
                Directory.SetCurrentDirectory(directoryName);
                MelonLogger.Log("Finished compressing!");
                ReuploadFilePath = GetLZ4HCFile();
                if (!string.IsNullOrEmpty(ReuploadFilePath))
                {
                    int startIndex = ReuploadFilePath.IndexOf(".LZ4HC");
                    string fileName = Path.GetFileName(ReuploadFilePath.Remove(startIndex, 6));
                    string destFileName = Path.Combine(VrcaStorePath, fileName);
                    File.Move(ReuploadFilePath, destFileName);
                    ReuploadFilePath = destFileName;
                    MelonLogger.Log(ReuploadFilePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.LogError(ex.Message);
                return false;
            }
        }

        private static string GetXMLFile()
        {
            string result = string.Empty;
            System.Collections.Generic.IEnumerable<string> enumerable = from string_0 in Directory.EnumerateFiles(AssetBundlePath)
                                                                        select new FileInfo(string_0) into fileInfo_0
                                                                        orderby fileInfo_0.CreationTime
                                                                        select fileInfo_0.FullName;
            if (enumerable.Any())
            {
                foreach (string item in enumerable)
                {
                    if (item.EndsWith(".xml"))
                    {
                        result = item;
                    }
                }
                return result;
            }
            return result;
        }

        private static string GetLZ4HCFile()
        {
            string result = string.Empty;
            System.Collections.Generic.IEnumerable<string> enumerable = from string_0 in Directory.EnumerateFiles(AssetBundlePath)
                                                                        select new FileInfo(string_0) into fileInfo_0
                                                                        orderby fileInfo_0.CreationTime
                                                                        select fileInfo_0.FullName;
            if (enumerable.Any())
            {
                foreach (string item in enumerable)
                {
                    if (item.EndsWith(".LZ4HC"))
                    {
                        result = item;
                    }
                }
                return result;
            }
            return result;
        }

        object RunUBFU(object obj, object[] parameters)
        {
            return System.Reflection.MethodBase.GetCurrentMethod().Invoke(obj, parameters);
        }
    }
}