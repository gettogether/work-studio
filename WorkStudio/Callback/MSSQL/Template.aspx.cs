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
using System.Text.RegularExpressions;

public partial class Callback_MSSQL_Index : WorkStudioLibrary.Web.CallbackTemplatePage
{
    public string ProjectName
    {
        get { return Request["pn"]; }
    }

    public string Content
    {
        get { return Request["content"]; }
    }

    public bool IsGetNewTemplate
    {
        get { return !string.IsNullOrEmpty(Request["get"]); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        switch (type)
        {
            case 0:
                GetTemplates();
                break;
            case 1:
                CreateTemplates();
                break;
            case 2:
                EditTemplates();
                break;
            default:
                break;
        }
    }

    #region Functions

    private void GetTemplates()
    {
        Components_MSSQL_Templates c = Page.LoadControl("~/Components/MSSQL/Templates.ascx") as Components_MSSQL_Templates;
        this.Controls.Add(c);
    }

    private string GetTemplateContent(string xml, string btnText, string jsFunc)
    {
        return string.Concat(
                        "<div style='padding:3px 0px 3px 4px;'><textarea id='txtTemplate' name='txtTemplate' style='width:99%;border:solid 1px #336699;' cols='90' rows='27'>",
                       xml,
                        "</textarea><div id='dv-error' style='color:red;text-align:center;'></div>",
                        "<div style='text-align:center;padding:5px 5px 2px 5px;'><input type='button' onclick=\"",
                        jsFunc,
                        "\" class='btn5' value='",
                        btnText,
                        "' /><input type='button' onclick='CloseMsgBox();' class='btn5' value='Cancel' /></div></div>"
                        );
    }

    private void EditTemplates()
    {
        if (IsGetNewTemplate)
        {
            try { Response.Write(GetTemplateContent(MSSQL.GetSettingsByProjectName(ProjectName).ToXml(), "Save", "TemplateExec('txtTemplate',2);")); }
            catch (Exception ex) { Response.Write(ex.Message); }
            Response.End();
        }
        else
        {
            try
            {
                CodeBuilderLibrary.Settings st = new CodeBuilderLibrary.Settings();
                st = st.FormXml(Content);
                string filePath = string.Concat(MSSQL.GetSettingsPath(), st.ProjectName);
                WorkStudioLibrary.Logging.Files.Log.Info(string.Concat("Edit template:", st.ProjectName));
                System.IO.File.WriteAllText(MSSQL.GetSettingsPath() + st.ProjectName, st.ToXml());
                JsonSuccess();
            }
            catch (Exception ex)
            {
                JsonError(ex.Message);
            }
        }
    }

    private void CreateTemplates()
    {
        if (IsGetNewTemplate)
        {
            Response.Write(GetTemplateContent(new CodeBuilderLibrary.Settings().ToXml(), "Create", "TemplateExec('txtTemplate',1);"));
            Response.End();
        }
        else
        {
            try
            {
                CodeBuilderLibrary.Settings st = new CodeBuilderLibrary.Settings();
                st = st.FormXml(Content);
                string filePath = string.Concat(MSSQL.GetSettingsPath(), st.ProjectName);
                WorkStudioLibrary.Logging.Files.Log.Info(string.Concat("Create / Edit template:", st.ProjectName));
                if (System.IO.File.Exists(filePath))
                {
                    JsonError(string.Concat("Project name \"", st.ProjectName, "\" was existed."));
                }
                else
                {
                    System.IO.File.WriteAllText(MSSQL.GetSettingsPath() + st.ProjectName, st.ToXml());
                    JsonSuccess();
                }

            }
            catch (Exception ex)
            {
                JsonError(ex.Message);
            }
        }
    }

    #endregion
}