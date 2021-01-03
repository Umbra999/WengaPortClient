using System;
using System.Collections.Generic;
using System.IO;

namespace WengaPort.Extensions
{
	internal class RPCAndEventBlock
	{
		public static bool Check(string UserID)
		{
			return Block.Contains(UserID);
		}

		public static void AddPlayer(string UserID)
		{
            Block.Add(UserID);
		}

		public static void RemovePlayer(string UserID)
		{
			if (Block.Contains(UserID))
			{
                Block.Remove(UserID);
			}
		}
		public static List<string> Block = new List<string>();
		public static List<string> EventBlock = new List<string>();
	}
}
