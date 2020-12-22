using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRCSDK2;

namespace WengaPort.Modules
{
    class PedestalHandler
    {
		public static void FetchPedestals()
		{
			originalPedestals = new List<OriginalPedestal>();
			foreach (VRC_AvatarPedestal vrc_AvatarPedestal in Resources.FindObjectsOfTypeAll<VRC_AvatarPedestal>())
			{
				originalPedestals.Add(new OriginalPedestal
				{
					PedestalParent = vrc_AvatarPedestal.gameObject,
					originalActiveStatus = vrc_AvatarPedestal.gameObject.activeSelf
				});
			}
		}
		public static void Disable()
		{
			if (originalPedestals.Count != 0)
			{
				foreach (OriginalPedestal originalPedestal in originalPedestals)
				{
					originalPedestal.PedestalParent.SetActive(false);
				}
			}
		}

		public static void Enable()
		{
			if (originalPedestals.Count != 0)
			{
				foreach (OriginalPedestal originalPedestal in originalPedestals)
				{
					originalPedestal.PedestalParent.SetActive(true);
				}
			}
		}

		public static void Revert()
		{
			if (originalPedestals.Count != 0)
			{
				foreach (OriginalPedestal originalPedestal in originalPedestals)
				{
					originalPedestal.PedestalParent.SetActive(originalPedestal.originalActiveStatus);
				}
			}
		}

		public static List<OriginalPedestal> originalPedestals;
		public class OriginalPedestal
		{
			public GameObject PedestalParent;
			public bool originalActiveStatus;
		}
	}
}
