using System;
using Transmtn.DTO.Notifications;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WengaPort.Modules
{

    internal static class ExtensionMethods
    {
        public static void SetAction(this Button button, Action action)
        {
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(action));
        }

        public static void SetButtonAction(this GameObject gameObject, Action action) =>
            gameObject.GetComponent<Button>().SetAction(action);

        public static Text GetTextComponentInChildren(this GameObject gameObject) =>
            gameObject.GetComponentInChildren<Text>();

        public static RectTransform GetRectTrans(this GameObject gameObject) =>
            gameObject.GetComponent<RectTransform>();

        public static void SetAnchoredPos(this RectTransform transform, Vector2 pos) =>
            transform.anchoredPosition = pos;

        public static void SetName(this GameObject gameObject, string name) =>
            gameObject.name = name;

        public static void SetText(this GameObject gameObject, string text) =>
            gameObject.GetTextComponentInChildren().text = text;
    }
}
