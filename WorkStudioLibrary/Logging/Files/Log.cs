using System;
using System.Collections.Generic;
using System.Text;
using CommonLibrary.Utility;

namespace WorkStudioLibrary.Logging.Files
{
    public class Log
    {
        public const string COMPART_STRING = "----------------------------------------------------------------";
        public static string GetSpecificInformation()
        {
            System.Web.HttpRequest request = null;
            if (System.Web.HttpContext.Current != null)
                request = System.Web.HttpContext.Current.Request;
            StringBuilder sb_ret = new StringBuilder();
            //SecurityLibrary.DataObject.Utils.LoginProfiles lp = SessionHelper.GetProfiles();
            //if (lp != null)
            //{
            //    sb_ret.AppendLine("Login Information: ");
            //    sb_ret.Append("Company Code: ").AppendLine(lp.Company.CompanyCode);
            //    sb_ret.Append("Company Name: ").AppendLine(lp.Company.CompanyName);
            //    sb_ret.Append("Team Code: ").AppendLine(lp.User.TeamCode);
            //    sb_ret.Append("User: ").AppendLine(lp.User.UserSurname + lp.User.UserGivenName);
            //    sb_ret.Append("UserCode: ").AppendLine(lp.User.UserCode);
            //    sb_ret.Append("Email: ").AppendLine(lp.User.Email);
            //    sb_ret.AppendLine(COMPART_STRING);
            //}
            if (request != null)
            {
                sb_ret.AppendLine("Specific Information: ");
                sb_ret.Append("Server Software: ").AppendLine(request.ServerVariables["SERVER_SOFTWARE"]);
                sb_ret.Append("Physical Path: ").AppendLine(request.ServerVariables["APPL_PHYSICAL_PATH"]);
                sb_ret.Append("Http Host: ").AppendLine(request.ServerVariables["HTTP_HOST"]);
                sb_ret.Append("Local Address: ").AppendLine(request.ServerVariables["LOCAL_ADDR"]);
                sb_ret.Append("Server port: ").AppendLine(request.ServerVariables["SERVER_PORT"]);
                sb_ret.Append("Path Translated: ").AppendLine(request.ServerVariables["PATH_TRANSLATED"]);
                sb_ret.Append("Request Type: ").AppendLine(request.RequestType);
                sb_ret.Append("Url: ").AppendLine(request.Url.ToString());
                sb_ret.Append("Post Data: ").AppendLine(request.Form.ToString());
                sb_ret.Append("Gateway Interface: ").AppendLine(request.ServerVariables["GATEWAY_INTERFACE"]);
                sb_ret.Append("Url Referrer: ").AppendLine(request.UrlReferrer == null ? "" : request.UrlReferrer.ToString());
                sb_ret.Append("User Host Address: ").AppendLine(request.UserHostAddress);
                //sb_ret.Append("Http Accept Language: ").AppendLine(request.ServerVariables["HTTP_ACCEPT_LANGUAGE"]);
                sb_ret.Append("Http User Agent: ").AppendLine(request.ServerVariables["HTTP_USER_AGENT"]);
                //sb_ret.Append("Instance Meta Path: ").AppendLine(request.ServerVariables["INSTANCE_META_PATH"]);
                sb_ret.AppendLine(COMPART_STRING);
                return sb_ret.ToString();
            }
            return sb_ret.ToString();
        }
        public static void Debug(string message)
        {
            if (!Config.LogDebug) return;
            LogHelper.WriteInfo(Enums.Loggers.WarningLog, COMPART_STRING);
            LogHelper.WriteInfo(Enums.Loggers.DebugLog, message);
        }
        public static void Error(Exception ex, params string[] additional_info)
        {
            StringBuilder sb_log = new StringBuilder();
            sb_log.Append("Log Time: ").AppendLine(DateHelper.FormatDateTimeToString(DateTime.Now, DateHelper.DateFormat.ddMMMyyyyspwt));
            sb_log.AppendLine(COMPART_STRING);
            if (additional_info != null && additional_info.Length > 0)
            {
                sb_log.AppendLine("Additional information:");
                foreach (string s in additional_info)
                {
                    sb_log.AppendLine(s);
                }
            }
            sb_log.AppendLine(ex.ToString());
            sb_log.AppendLine(COMPART_STRING);
            sb_log.AppendLine(GetSpecificInformation());
            string log = sb_log.ToString();
            LogHelper.WriteError(Enums.Loggers.Default, COMPART_STRING);
            LogHelper.WriteError(Enums.Loggers.Default, ex.ToString());
            if (Config.EnableErrorReport)
            {
                SendEmail("Error: " + ex.Message.Replace("\r\n", ""), log);
            }
        }
        public static void SendEmail(string subject, string body)
        {
            Functions.EmailHelper.SendEmail(Config.ReporterEmail, Config.RecipientEmails, subject, body, false);
        }
        public static void Info(string message)
        {
            Web.SessionObjects sess = Web.SessionHelper.GetCurrentSession();
            if (sess != null && !string.IsNullOrEmpty(sess.Lastname))
            {
                message = string.Concat(sess.Lastname, " : ", message);
            }
            Info(null, message);
        }
        public static void Info(string logger, string message)
        {
            if (!Config.LogInfo) return;
            if (string.IsNullOrEmpty(logger))
            {
                LogHelper.WriteInfo(Enums.Loggers.InfoLog, COMPART_STRING);
                LogHelper.WriteInfo(Enums.Loggers.InfoLog, message);
            }
            else
            {
                LogHelper.WriteInfo(logger, COMPART_STRING);
                LogHelper.WriteInfo(logger, message);
            }
        }
        public static void Warning(params string[] message)
        {
            Warning(null, message);
        }
        public static void Warning(string logger, string[] message)
        {
            string subject = "";
            if (message.Length > 1)
                subject = message[0];
            else
            {
                int i = message[0].IndexOf("\r\n");
                if (i > 0)
                {
                    subject = message[0].Substring(0, i);
                }
                else
                {
                    if (message[0].Length > 100)
                        subject = message[0].Substring(0, 100);
                    else
                        subject = message[0].Substring(0, message[0].Length);
                }
            }
            StringBuilder sb_log = new StringBuilder();
            sb_log.Append("Log Time: ").AppendLine(DateHelper.FormatDateTimeToString(DateTime.Now, DateHelper.DateFormat.ddMMMyyyyspwt));
            sb_log.AppendLine(COMPART_STRING);
            foreach (string s in message)
            {
                sb_log.AppendLine(s);
            }
            if (string.IsNullOrEmpty(logger))
            {
                LogHelper.WriteWarn(Enums.Loggers.WarningLog, COMPART_STRING);
                LogHelper.WriteWarn(Enums.Loggers.WarningLog, StringHelper.ArrayToString(message, "\r\n"));
            }
            else
            {
                LogHelper.WriteWarn(logger, COMPART_STRING);
                LogHelper.WriteWarn(logger, StringHelper.ArrayToString(message, "\r\n"));
            }
            sb_log.AppendLine(COMPART_STRING);
            sb_log.AppendLine(GetSpecificInformation());
            if (Config.EnableWarningReport)
            {
                SendEmail("Warning: " + subject.Replace("\r\n", ""), sb_log.ToString());
            }
        }
    }
}
