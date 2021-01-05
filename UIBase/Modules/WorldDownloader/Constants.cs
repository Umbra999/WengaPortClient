using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WengaPort.Modules
{
    internal static class Constants
    {
        public const string WORLD_BUTTON_NAME = "PreloadWorldButton";
        public const string DOWNLOAD_STATUS_NAME = "DownloadStatusText";
        public const string DOWNLOAD_STATUS_IDLE_TEXT = "";
        public const string BUTTON_IDLE_TEXT = "Predownload";
        public const string BUTTON_BUSY_TEXT = "Cancel Download";
        public const string BUTTON_ALREADY_DOWNLOADED_TEXT = "Downloaded";
        public const string DOWNLOAD_ERROR_TITLE = "World Predownload Failed";
        public const string DOWNLOAD_ERROR_MSG = "API Info not loaded, try again";
        public const string DOWNLOAD_ERROR_BTN_TEXT = "Dismiss";
        public const string DOWNLOAD_SUCCESS_TITLE = "WengaPort";
        public const string DOWNLOAD_SUCCESS_MSG = "Your World is downloaded";
        public const string DOWNLOAD_SUCCESS_LEFT_BTN_TEXT = "Go to World Page";
        public const string DOWNLOAD_SUCCESS_RIGHT_BTN_TEXT = "Dismiss";

        public static readonly Vector2 WORLD_BUTTON_POS = new Vector2(570, -188f);
    }
}
