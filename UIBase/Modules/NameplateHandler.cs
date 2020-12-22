using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using WengaPort.Extensions;

namespace WengaPort.Modules
{
    class NameplateHandler
    {
		public static void givePlateText(Player player, Color color, string rank)
		{
			player.prop_VRCPlayer_0.nameplate.uiName.color = color;
			player.prop_VRCPlayer_0.nameplate.uiNameBackground.enabled = false;
			player.prop_VRCPlayer_0.nameplate.uiIconBackground.enabled = false;
			player.prop_VRCPlayer_0.nameplate.uiQuickStatsBackground.enabled = false;
			player.prop_VRCPlayer_0.nameplate.uiTrustText.color = color;
			player.prop_VRCPlayer_0.nameplate.uiTrustIcon.color = PlayerExtensions.IsFriend(player) ? new Color(1f, 0.81f, 0.03f) : color;
			player.prop_VRCPlayer_0.nameplate.uiTrustText.text =  PlayerExtensions.IsFriend(player) ? $"{rank} (F)" : rank;
			player.prop_VRCPlayer_0.nameplate.uiQuickStats.SetActive(true);
			player.prop_VRCPlayer_0.nameplate.uiPerformanceIcon.enabled = false;
			player.prop_VRCPlayer_0.nameplate.uiPerformanceText.enabled = false;
			player.prop_VRCPlayer_0.nameplate.uiFriendMarker.enabled = false;
		}

		public static void giveAdminText(Player player, Color color, string rank)
		{
			player.prop_VRCPlayer_0.nameplate.uiName.color = color;
			player.prop_VRCPlayer_0.nameplate.uiNameBackground.enabled = false;
			player.prop_VRCPlayer_0.nameplate.uiIconBackground.enabled = false;
			player.prop_VRCPlayer_0.nameplate.uiQuickStatsBackground.enabled = false;
			player.prop_VRCPlayer_0.nameplate.uiTrustText.color = color;
			player.prop_VRCPlayer_0.nameplate.uiTrustIcon.color = color;
			player.prop_VRCPlayer_0.nameplate.uiTrustText.text = rank;
			player.prop_VRCPlayer_0.nameplate.uiPerformanceIcon.enabled = false;
			player.prop_VRCPlayer_0.nameplate.uiPerformanceText.enabled = false;
			player.prop_VRCPlayer_0.nameplate.uiQuickStats.SetActive(true);
			player.prop_VRCPlayer_0.nameplate.uiDevIconBackground.SetActive(true);
			player.prop_VRCPlayer_0.nameplate.uiFriendMarker.enabled = false;
		}

		public static float GetDistandBetweenObjects(GameObject Base, GameObject target)
		{
			Vector3 position = Base.transform.position;
			Vector3 position2 = target.transform.position;
			return Vector3.Distance(position, position2);
		}

		public static string ConvertRGBtoHEX(Color color)
		{
			byte byteR = (byte)(color.r * 255);
			byte byteG = (byte)(color.g * 255);
			byte byteB = (byte)(color.b * 255);
			return byteR.ToString("X2") + byteG.ToString("X2") + byteB.ToString("X2");
		}
    }
}
