using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Callback_XML_Edit : WorkStudioLibrary.Web.CallbackTemplatePage
{
    string CmdString_DataSet = @"xsd {0}.xml /outputdir:
xsd.exe   /d   /l:C#   {0}.xsd   /n:{1}";
    string CmdString_Class = @"xsd {0}.xml /outputdir:
xsd {0}.xsd /c /l:cs /nologo /n:{1} /out:";
    protected void Page_Load(object sender, EventArgs e)
    {
        switch (type)
        {
            case 0:
                GenCodeByXml(Request["xml"], Request["ns"], !string.IsNullOrEmpty(Request["isds"]));
                break;
            default:
                break;
        }
    }

    private void GenCodeByXml(string xml, string nameSpace, bool isDataSet)
    {
        string code = GetCodeByXml(xml, nameSpace, isDataSet);
        Response.Write(string.Concat("<textarea id=\"txtCode\" rows=\"18\" cols=\"10\" style=\"width: 99%; border-width: 0px;\">", code, "</textarea>"));
    }

    public string GetCodeByXml(string xml, string nameSpace, bool isDataSet)
    {
        try
        {
            if (string.IsNullOrEmpty(xml)) return "Xml is null or empty.";
            if (string.IsNullOrEmpty(nameSpace)) return "Namespace is null or empty.";
            string dirBase = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Include\\XML\\");
            string timeFlag = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string fileNoSuffix = string.Concat(dirBase, timeFlag);
            string fileExe = string.Concat(dirBase, timeFlag, ".bat");
            string fileCode = string.Concat(dirBase, timeFlag, ".cs");
            string fileXml = string.Concat(dirBase, timeFlag, ".xml");
            WorkStudioLibrary.Logging.Files.Log.Info(string.Concat("Generate xml to C# code:", fileXml));
            System.IO.File.WriteAllText(fileXml, xml, System.Text.Encoding.UTF8);
            System.IO.File.WriteAllText(fileExe, string.Format(isDataSet ? CmdString_DataSet : CmdString_Class, timeFlag, nameSpace), System.Text.Encoding.ASCII);


            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = fileExe;
            p.StartInfo.WorkingDirectory = dirBase;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string ret = p.StandardOutput.ReadToEnd();



            int timeSlip = 0;
            while (true)
            {
                if (System.IO.File.Exists(fileCode))
                {
                    break;
                }
                timeSlip++;
                if (timeSlip > 20)
                {
                    return ret;
                    //return "Command Timeout";
                }
                System.Threading.Thread.Sleep(500);
            }
            return System.IO.File.ReadAllText(fileCode, System.Text.Encoding.UTF8);
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
}
