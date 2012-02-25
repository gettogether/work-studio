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
using CodeBuilderLibrary.DataObjects;
using System.Text.RegularExpressions;

public partial class MS_MSSQL : System.Web.UI.Page
{
    private string _DO_String;

    public string DO_String
    {
        get { return _DO_String; }
        set { _DO_String = value; }
    }

    private string _DO_String_Display;

    public string DO_String_Display
    {
        get { return _DO_String_Display; }
        set { _DO_String_Display = value; }
    }


    private string _BO_String;

    public string BO_String
    {
        get { return _BO_String; }
        set { _BO_String = value; }
    }

    public string SplitHeader
    {
        get
        {
            if (!CommonLibrary.WebObject.WebHelper.IsIE(Page))
            {
                return "<div style='width:100%;height:45px;'></div>";
            }
            else
                return "<div style='width:100%;height:15px;'></div>";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //DO_String = GetString();

        //DO_String_Display = DO_String;// new HtmlCodeColor.CodeColor(new HtmlCodeColor.CSharpAdapter()).ColorCode(DO_String);

        //BO_String = GetString1();

        //CodeBuilderLibrary.Settings st = new CodeBuilderLibrary.Settings();
        //st.ConnectionString = "Data Source=.;Initial Catalog=WTMS;Persist Security Info=True;User ID=sa;Password=gzuat";

        //System.IO.File.WriteAllText(MSSQL.GetSettingsPath() + st.ProjectName, st.ToXml());


    }

    #region Testing

    public string GetString()
    {
        CodeBuilderLibrary.Settings cf = new CodeBuilderLibrary.Settings();
        CodeBuilderLibrary.DataObjects.doColumns.uoListColumns cs = new CodeBuilderLibrary.DataObjects.doColumns.uoListColumns();
        CodeBuilderLibrary.DataObjects.doColumns.uoColumns c = new CodeBuilderLibrary.DataObjects.doColumns.uoColumns();

        doPKs.uoListPKs listPks = CodeBuilderLibrary.BusinessObjects.boPks.GetPrimaryKeys(cf.ConnectionString, "spt_values");
        doColumns.uoListColumns listColumns = CodeBuilderLibrary.BusinessObjects.boColumns.GetColumns(cf.ConnectionString, "spt_values");
        doRemarks.uoListRemarks listRemarks = CodeBuilderLibrary.BusinessObjects.boRemarks.GetRemarks(cf.ConnectionString, "spt_values");
        return CodeBuilderLibrary.SQL.CodeScript.GetDataObject(cf, "Login", listColumns, listPks, listRemarks);
    }

    public string GetString1()
    {
        CodeBuilderLibrary.Settings cf = new CodeBuilderLibrary.Settings();
        CodeBuilderLibrary.DataObjects.doColumns.uoListColumns cs = new CodeBuilderLibrary.DataObjects.doColumns.uoListColumns();
        CodeBuilderLibrary.DataObjects.doColumns.uoColumns c = new CodeBuilderLibrary.DataObjects.doColumns.uoColumns();
        doPKs.uoListPKs listPks = CodeBuilderLibrary.BusinessObjects.boPks.GetPrimaryKeys(cf.ConnectionString, "spt_values");
        doColumns.uoListColumns listColumns = CodeBuilderLibrary.BusinessObjects.boColumns.GetColumns(cf.ConnectionString, "spt_values");
        doRemarks.uoListRemarks listRemarks = CodeBuilderLibrary.BusinessObjects.boRemarks.GetRemarks(cf.ConnectionString, "spt_values");
        return CodeBuilderLibrary.SQL.CodeScript.GetBusinessObject(cf, "Login", listColumns, listPks);
    }

    #endregion
}


