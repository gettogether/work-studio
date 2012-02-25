using System;
using System.Collections.Generic;
using System.Text;
using CommonLibrary.Utility;
using WorkStudioLibrary.Logging.Files;

namespace WorkStudioLibrary.Functions
{
    public class Common
    {
        public static string GetVersionNumber()
        {
            System.Diagnostics.FileVersionInfo version_info =
                System.Diagnostics.FileVersionInfo.GetVersionInfo(
                System.Web.HttpContext.Current.Server.MapPath(@"~\Bin\WorkStudioLibrary.dll"));
            string version = version_info.FileVersion;
            if (!string.IsNullOrEmpty(version) && version.Length == 7)
            {
                version = version.Substring(0, 5);
            }
            return version;
        }
        public static string DesEncrypt(string str)
        {
            try
            {
                return SecretHelper.DesEncrypt(str, Config.EncrKey);
            }
            catch (Exception ex)
            {
                Log.Error(ex, new string[] { ex.Message, string.Format("str={0},Config.EncrKey={1}", str, Config.EncrKey) });
                return str;
            }
        }

        public static string DesDecrypt(string str)
        {
            try
            {
                return SecretHelper.DesDecrypt(str, Config.EncrKey);
            }
            catch (Exception ex)
            {
                Log.Error(ex, new string[] { ex.Message, string.Format("str={0},Config.EncrKey={1}", str, Config.EncrKey) });
                return str;
            }
        }
    }
}
