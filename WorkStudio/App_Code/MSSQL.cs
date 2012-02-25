using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// MSSQL 的摘要说明
/// </summary>
public class MSSQL
{
    public static string GetSettingsPath()
    {
        return string.Concat(System.Web.HttpContext.Current.Server.MapPath("~/."), "\\Include\\MSSQL\\");
    }

    public static CodeBuilderLibrary.Settings GetSettingsByProjectName(string n)
    {
        CodeBuilderLibrary.Settings s = new CodeBuilderLibrary.Settings();
        s = s.FormXml(System.IO.File.ReadAllText(string.Concat(GetSettingsPath(), n)));
        return s;
    }
}
