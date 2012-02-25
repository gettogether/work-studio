using System;
using System.Collections.Generic;
using System.Text;
using CommonLibrary.Utility;

namespace WorkStudioLibrary
{
    public class Initialize
    {
        public static void SetInitialize()
        {
            SetAppSettings();
            LogHelper.SetConfig(System.Web.HttpContext.Current.Server.MapPath(Config.LogConfig));
        }
        public static void SetAppSettings()
        {
            WorkStudioLibrary.Config.Version = Functions.Common.GetVersionNumber();

            WorkStudioLibrary.Config.CredentialHost = ConfigHelper.GetAppSetting("CredentialHost");
            WorkStudioLibrary.Config.CredentialPassword = ConfigHelper.GetAppSetting("CredentialPassword");
            WorkStudioLibrary.Config.CredentialUserName = ConfigHelper.GetAppSetting("CredentialUserName");
            WorkStudioLibrary.Config.EnableCredential = ConfigHelper.GetAppSetting("EnableCredential") == "1";

            WorkStudioLibrary.Config.EmailServer = ConfigHelper.GetAppSetting("EmailServer");

            WorkStudioLibrary.Config.EnableErrorReport = ConfigHelper.GetAppSetting("EnableErrorReport") == "1";
            WorkStudioLibrary.Config.EnableWarningReport = ConfigHelper.GetAppSetting("EnableWarningReport") == "1";
            WorkStudioLibrary.Config.RecipientEmails = ConfigHelper.GetAppSetting("RecipientEmails").Split(',');
            WorkStudioLibrary.Config.ReporterEmail = ConfigHelper.GetAppSetting("ReporterEmail");


            WorkStudioLibrary.Config.EncrKey = ConfigHelper.GetAppSetting("EncrKey");
            WorkStudioLibrary.Config.LogConfig = ConfigHelper.GetAppSetting("LogConfig");
            WorkStudioLibrary.Config.LogDebug = ConfigHelper.GetAppSetting("LogDebug") == "1";
            WorkStudioLibrary.Config.LogInfo = ConfigHelper.GetAppSetting("LogInfo") == "1";

            WorkStudioLibrary.Config.LocalHost = ConfigHelper.GetAppSetting("LocalHost");
        }
    }
}
