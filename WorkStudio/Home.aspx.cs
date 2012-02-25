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
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

public partial class Home : WorkStudioLibrary.Web.TemplatePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (CommonLibrary.Utility.MutiLanguage.GetCultureType() != CommonLibrary.Utility.MutiLanguage.Languages.en_us)
        //    Response.Redirect("Home.aspx?lang=en-us");
        try
        {
            //throw new Exception("test exception");
        }
        catch (Exception ex)
        {
            //string str = CommonLibrary.Utility.ExceptionHelper.Process(ex);
            Response.Write(ex.ToString());
        }

    }
}


