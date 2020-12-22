using MelonLoader;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using WengaPort.Buttons;
using WengaPort.Wrappers;

namespace WengaPort.Modules
{
    internal class Movement : MelonMod
    {
        public static void NoClipEnable()
        {
            if (Utils.CurrentUser == null)
            {
                Utils.CurrentUser = VRCPlayer.field_Internal_Static_VRCPlayer_0;
            }
            Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = false;
            FlyEnable();
            MainMenu.FlyButton.setToggleState(true, false);
            NoClipToggle = true;
        }

        public static void NoClipDisable()
        {
            Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = true;
            FlyDisable();
            MainMenu.FlyButton.setToggleState(false, false);
            NoClipToggle = false;
        }

        public static void IncreaseSpeed()
        {
            UnityEngine.Object.FindObjectOfType<LocomotionInputController>().walkSpeed = 4;
            UnityEngine.Object.FindObjectOfType<LocomotionInputController>().runSpeed = 8;
            UnityEngine.Object.FindObjectOfType<LocomotionInputController>().strafeSpeed = 4;
        }

        public static void DecreaseSpeed()
        {
            UnityEngine.Object.FindObjectOfType<LocomotionInputController>().walkSpeed = 2;
            UnityEngine.Object.FindObjectOfType<LocomotionInputController>().runSpeed = 4;
            UnityEngine.Object.FindObjectOfType<LocomotionInputController>().strafeSpeed = 2;
        }

        public static void QuickMenuFly()
        {
            try
            {
                var Shortcut = GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu");
                if (Shortcut.gameObject.active == false && !NoClipToggle)
                {
                    Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = true;
                }
                else if (Shortcut.gameObject.active == true && !NoClipToggle)
                {
                    Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = false;
                }
            }
            catch (Exception)
            { }
        }

        public static bool FlyToggle = false;
        public static bool InfJump = false;
        public static bool NoClipToggle = false;
        public static bool ShortcutActive = false;
        public static bool ShortcutInactive = true;

        public static void FlyEnable()
        {
            FlyToggle = true;
        }

        public static void FlyDisable()
        {
            FlyToggle = false;
            Physics.gravity = gravity;
            Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = true;
            MainMenu.NoClipButton.setToggleState(false, false);
            NoClipToggle = false;
        }

        public override void VRChat_OnUiManagerInit()
        {
            isInVR = GeneralWrappers.IsInVr();
        }

        public static void JumpInit()
        {
            if (InfJump && !FlyToggle)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    currentPlayer = Utils.CurrentUser;
                    if (currentPlayer == null || transform == null)
                    {
                        currentPlayer = Utils.CurrentUser;
                        transform = Camera.main.transform;
                    }
                    currentPlayer.transform.position += transform.transform.up * 3.5f * Time.deltaTime;
                    if (Input.GetKey((KeyCode)119))
                    {
                        currentPlayer.transform.position += transform.transform.forward * 3.5f * Time.deltaTime;
                    }
                    if (Input.GetKey((KeyCode)97))
                    {
                        currentPlayer.transform.position += transform.transform.right * -1f * 3.5f * Time.deltaTime;
                    }
                    if (Input.GetKey((KeyCode)100))
                    {
                        currentPlayer.transform.position += transform.transform.right * 3.5f * Time.deltaTime;
                    }
                    if (Input.GetKey((KeyCode)115))
                    {
                        currentPlayer.transform.position += transform.transform.forward * -1f * 3.5f * Time.deltaTime;
                    }
                    Physics.gravity = Vector3.zero;
                }
                else
                {
                    Physics.gravity = gravity;
                }
            }
        }

        public static void FlyInit()
        {
            if (Input.GetKeyDown(KeyCode.F) & Input.GetKey(KeyCode.LeftControl))
            {
                if (FlyToggle)
                {
                    MainMenu.NoClipButton.setToggleState(false, false);
                    MainMenu.FlyButton.setToggleState(false, false);
                    FlyDisable();
                }
                else
                {
                    FlyEnable();
                    MainMenu.FlyButton.setToggleState(true, false);
                }
            }
            if (Input.GetKeyDown(KeyCode.G) & Input.GetKey(KeyCode.LeftControl))
            {
                if (NoClipToggle)
                {
                    MainMenu.NoClipButton.setToggleState(false, false);
                    MainMenu.FlyButton.setToggleState(false, false);
                    FlyDisable();
                    NoClipDisable();
                }
                else
                {
                    NoClipEnable();
                    FlyEnable();
                    MainMenu.FlyButton.setToggleState(true, false);
                    MainMenu.NoClipButton.setToggleState(true, false);
                }
            }
            if (currentPlayer == null || transform == null)
            {
                currentPlayer = Utils.CurrentUser;
                transform = Camera.main.transform;
                isInVR = UnityEngine.XR.XRDevice.isPresent;
            }
            if (FlyToggle)
            {
                if (RoomManager.field_Internal_Static_ApiWorldInstance_0 == null)
                {
                    return;
                }
                try
                {
                    bool keyDown = Input.GetKeyDown((KeyCode)304);
                    if (keyDown)
                    {
                        FlySpeed *= 2f;
                    }
                    bool keyUp = Input.GetKeyUp((KeyCode)304);
                    if (keyUp)
                    {
                        FlySpeed /= 2f;
                    }
                    bool vrflyToggle = VRFlyToggle;
                    if (vrflyToggle)
                    {
                        bool flag3 = isInVR;
                        if (flag3)
                        {
                            bool flag4 = Math.Abs(Input.GetAxis("Vertical")) != 0f;
                            if (flag4)
                            {
                                currentPlayer.transform.position += transform.transform.forward * FlySpeed * Time.deltaTime * Input.GetAxis("Vertical");
                            }
                            bool flag5 = Math.Abs(Input.GetAxis("Horizontal")) != 0f;
                            if (flag5)
                            {
                                currentPlayer.transform.position += transform.transform.right * FlySpeed * Time.deltaTime * Input.GetAxis("Horizontal");
                            }
                            bool flag6 = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") < 0f;
                            if (flag6)
                            {
                                currentPlayer.transform.position += transform.transform.up * FlySpeed * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical");
                            }
                            bool flag7 = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") > 0f;
                            if (flag7)
                            {
                                currentPlayer.transform.position += transform.transform.up * FlySpeed * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical");
                            }
                        }
                        else
                        {
                            bool key = Input.GetKey((KeyCode)101);
                            if (key)
                            {
                                currentPlayer.transform.position += transform.transform.up * FlySpeed * Time.deltaTime;
                            }
                            bool key2 = Input.GetKey((KeyCode)113);
                            if (key2)
                            {
                                currentPlayer.transform.position += transform.transform.up * -1f * FlySpeed * Time.deltaTime;
                            }
                            bool key3 = Input.GetKey((KeyCode)119);
                            if (key3)
                            {
                                currentPlayer.transform.position += transform.transform.forward * FlySpeed * Time.deltaTime;
                            }
                            bool key4 = Input.GetKey((KeyCode)97);
                            if (key4)
                            {
                                currentPlayer.transform.position += transform.transform.right * -1f * FlySpeed * Time.deltaTime;
                            }
                            bool key5 = Input.GetKey((KeyCode)100);
                            if (key5)
                            {
                                currentPlayer.transform.position += transform.transform.right * FlySpeed * Time.deltaTime;
                            }
                            bool key6 = Input.GetKey((KeyCode)115);
                            if (key6)
                            {
                                currentPlayer.transform.position += transform.transform.forward * -1f * FlySpeed * Time.deltaTime;
                            }
                        }
                    }
                    else
                    {
                        bool flag8 = isInVR;
                        if (flag8)
                        {
                            bool flag9 = Math.Abs(Input.GetAxis("Vertical")) != 0f;
                            if (flag9)
                            {
                                currentPlayer.transform.position += currentPlayer.transform.forward * FlySpeed * Time.deltaTime * Input.GetAxis("Vertical");
                            }
                            bool flag10 = Math.Abs(Input.GetAxis("Horizontal")) != 0f;
                            if (flag10)
                            {
                                currentPlayer.transform.position += currentPlayer.transform.right * FlySpeed * Time.deltaTime * Input.GetAxis("Horizontal");
                            }
                            bool flag11 = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") < 0f;
                            if (flag11)
                            {
                                currentPlayer.transform.position += currentPlayer.transform.up * FlySpeed * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical");
                            }
                            bool flag12 = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") > 0f;
                            if (flag12)
                            {
                                currentPlayer.transform.position += currentPlayer.transform.up * FlySpeed * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical");
                            }
                        }
                        else
                        {
                            bool key7 = Input.GetKey((KeyCode)101);
                            if (key7)
                            {
                                currentPlayer.transform.position += currentPlayer.transform.up * FlySpeed * Time.deltaTime;
                            }
                            bool key8 = Input.GetKey((KeyCode)113);
                            if (key8)
                            {
                                currentPlayer.transform.position += currentPlayer.transform.up * -1f * FlySpeed * Time.deltaTime;
                            }
                            bool key9 = Input.GetKey((KeyCode)119);
                            if (key9)
                            {
                                currentPlayer.transform.position += currentPlayer.transform.forward * FlySpeed * Time.deltaTime;
                            }
                            bool key10 = Input.GetKey((KeyCode)97);
                            if (key10)
                            {
                                currentPlayer.transform.position += currentPlayer.transform.right * -1f * FlySpeed * Time.deltaTime;
                            }
                            bool key11 = Input.GetKey((KeyCode)100);
                            if (key11)
                            {
                                currentPlayer.transform.position += currentPlayer.transform.right * FlySpeed * Time.deltaTime;
                            }
                            bool key12 = Input.GetKey((KeyCode)115);
                            if (key12)
                            {
                                currentPlayer.transform.position += currentPlayer.transform.forward * -1f * FlySpeed * Time.deltaTime;
                            }
                        }
                    }
                    Physics.gravity = Vector3.zero;
                }
                catch (Exception)
                {
                }
            }

            if (Rotate)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    currentPlayer.transform.Rotate(Vector3.right, RotateSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    currentPlayer.transform.Rotate(Vector3.left, RotateSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    currentPlayer.transform.Rotate(Vector3.back, RotateSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    currentPlayer.transform.Rotate(Vector3.forward, RotateSpeed * Time.deltaTime);
                }
                alignTrackingToPlayer?.Invoke();
            }
        }

        internal static AlignTrackingToPlayerDelegate GetAlignTrackingToPlayerDelegate
        {
            get
            {
                if (alignTrackingToPlayerMethod == null)
                {
                    alignTrackingToPlayerMethod = typeof(VRCPlayer).GetMethods(BindingFlags.Instance | BindingFlags.Public).
                        First((MethodInfo m) => m.ReturnType == typeof(void)
                        && m.GetParameters().Length == 0
                        && m.XRefScanForMethod("get_Transform", null)
                        && m.XRefScanForMethod(null, "Player")
                        && m.XRefScanForMethod("Vector3_Quaternion", "VRCPlayer")
                        && m.XRefScanForMethod(null, "VRCTrackingManager")
                        && m.XRefScanForMethod(null, "InputStateController"));
                }
                return (AlignTrackingToPlayerDelegate)Delegate.CreateDelegate(typeof(AlignTrackingToPlayerDelegate), Utils.CurrentUser, alignTrackingToPlayerMethod);
            }
        }

        private static AlignTrackingToPlayerDelegate alignTrackingToPlayer;
        private static MethodInfo alignTrackingToPlayerMethod;

        internal static void ToggleRotate(bool state)
        {
            if (state)
            {
                Rotate = true;
                alignTrackingToPlayer = GetAlignTrackingToPlayerDelegate;
            }
            else
            {
                Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = false;
                Rotate = false;
                Quaternion localRotation = Utils.CurrentUser.transform.localRotation;
                Utils.CurrentUser.transform.localRotation = new Quaternion(0f, localRotation.y, 0f, localRotation.w);
                alignTrackingToPlayer();
                Utils.CurrentUser.gameObject.GetComponent<CharacterController>().enabled = true;
            }
        }

        internal delegate void AlignTrackingToPlayerDelegate();

        public static Vector3 gravity = Physics.gravity;
        private static VRCPlayer currentPlayer;
        private static Transform transform;
        public static bool isInVR;
        public static bool VRFlyToggle = true;
        public static bool Rotate = false;
        public static float FlySpeed = 4.2f;
        public static float RotateSpeed = 150f;
    }
}