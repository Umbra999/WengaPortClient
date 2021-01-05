using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace WengaPort.Modules
{
    internal class CacheManager
    {
        private const string NULL_STRING_ARGUMENT = "Tried to check if null string was downloaded";

        private static HashSet<string> directories = new HashSet<string>();

        public static System.Collections.IEnumerator UpdateDirectoriesBackground()
        {
            yield return null;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            directories.Clear();
            foreach (var directoryInfo in new DirectoryInfo(GetCache().path).GetDirectories())
            {
                directories.Add(directoryInfo.Name);
            }
            timer.Stop();
            yield break; 
            
        }

        public static void AddDirectory(string hash)
        {
            directories.Add(hash);
        }


        public static bool HasDownloadedWorld(string id, int version)
        {
            _ = id ?? throw new ArgumentNullException(paramName: nameof(id), message: NULL_STRING_ARGUMENT); //Lazy null check
            if (directories.Contains(ComputeAssetHash(id)))
            {
                if (HasVersion(ComputeAssetHash(id), version))
                    return true;
                else
                    return false;
            }
            else return false;
        }

        public static string ComputeAssetHash(string id)
        {
            return Utilities.ByteArrayToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(id))).ToUpper().Substring(0, 16);
        }

        private static UnityEngine.Cache GetCache()
        {
            return Utilities.GetAssetBundleDownloadManager().field_Private_Cache_0;
        }

        private static bool HasVersion(string hash, int version)
        {
            foreach(DirectoryInfo directoryInfo in new DirectoryInfo(Path.Combine(GetCache().path, hash)).GetDirectories())
            {
                if (directoryInfo.Name.EndsWith(ComputeVersionString(version))) return true;
            }
            return false;
        }

        private static string ComputeVersionString(int version)
        {
            string result = version.ToString("X").ToLower();
            if (result.Length == 3)
            {
                string part = result.Substring(0, 1);
                result = result.Substring(1, result.Length - 1);
                return result += $"0{part}0000";
            }
            else return result += "000000";
        }
    }
}
