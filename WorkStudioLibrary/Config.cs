using System;
using System.Collections.Generic;
using System.Text;

namespace WorkStudioLibrary
{
    public class Config
    {
        public static string Version;
        public static string EncrKey;
        public static string LogConfig;
        public static bool LogInfo = false;
        public static bool LogDebug = false;

        public static string EmailServer;
        public static string CredentialUserName;
        public static string CredentialPassword;
        public static string CredentialHost;
        public static bool EnableCredential = false;

        public static string[] RecipientEmails;
        public static bool EnableErrorReport = false;
        public static bool EnableWarningReport = false;
        public static string ReporterEmail;

        public static string SpecialAdminEmail;

        public static string LocalHost;

        public Config()
        {
        }
        public class ConnectionKeys
        {

        }
    }
}
