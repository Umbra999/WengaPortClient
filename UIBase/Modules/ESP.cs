using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WengaPort.Extensions;

namespace WengaPort.Modules
{
    class ESP
    {
		public static void Initialize()
		{
			MelonCoroutines.Start(Loop());
			HighlightColor(Color.green);
		}
		public static void HighlightColor(Color highlightcolor)
		{
			bool flag = Resources.FindObjectsOfTypeAll<HighlightsFXStandalone>().Count != 0;
			if (flag)
			{
				Resources.FindObjectsOfTypeAll<HighlightsFXStandalone>().FirstOrDefault().highlightColor = highlightcolor;
			}
		}

		private static IEnumerator Loop()
		{
			for (; ; )
			{
				yield return new WaitForEndOfFrame();
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
		}
		public static bool ESPEnabled = false;
		private static bool wasEnabled;
	}
}

