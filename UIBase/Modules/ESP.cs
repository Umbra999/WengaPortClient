﻿using System;
using System.Linq;
using UnityEngine;

namespace WengaPort.Modules
{
    class ESP : MonoBehaviour
    {
		public static void HighlightColor(Color highlightcolor)
		{
			bool flag = Resources.FindObjectsOfTypeAll<HighlightsFXStandalone>().Count != 0;
			if (flag)
			{
				Resources.FindObjectsOfTypeAll<HighlightsFXStandalone>().FirstOrDefault().highlightColor = highlightcolor;
			}
		}

		public void Update()
		{
			if (ESPEnabled)
			{
				foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
				{
					if (gameObject.transform.Find("SelectRegion"))
					{
						HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
					}
				}
				wasEnabled = true;
			}
			else if (!ESPEnabled && wasEnabled)
			{
				foreach (GameObject gameObject2 in GameObject.FindGameObjectsWithTag("Player"))
				{
					if (gameObject2.transform.Find("SelectRegion"))
					{
						HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(gameObject2.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
					}
				}
				wasEnabled = false;
			}
		}
		public static bool ESPEnabled = false;
		private static bool wasEnabled;

		public ESP(IntPtr ptr) : base(ptr) { }
	}
}

