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

public partial class MSSQL_Index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //foreach (string s in System.IO.Directory.GetFiles(MSSQL.GetSettingsPath()))
        //{
        //    System.IO.FileInfo fi = new System.IO.FileInfo(s);
        //    Response.Write(fi.Name);
        //}
        Response.Redirect("Template.aspx");
    }


}
