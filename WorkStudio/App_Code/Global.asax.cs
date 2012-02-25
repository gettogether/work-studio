using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using CommonLibrary.Utility;


public class Global : System.Web.HttpApplication
{
    public Global()
    {

    }
    protected void Application_Start(Object sender, EventArgs e)
    {
        WorkStudioLibrary.Initialize.SetInitialize();
        DataAccess.Log.InitLogging(string.Concat(Server.MapPath("~/."), "/DA.config"));
        //new System.Threading.Thread(new System.Threading.ThreadStart(new AsynchronousInit().Start)).Start();
    }

    protected void Application_Error(Object sender, EventArgs e)
    {
        if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Server != null)
        {
            WorkStudioLibrary.Logging.Files.Log.Error(System.Web.HttpContext.Current.Server.GetLastError());
        }
    }

    public void Session_OnStart(object sender, EventArgs e)
    {
        Session["culture_string"] = "en-us";

    }

    public void Session_End(object sender, EventArgs e)
    {

    }

    //public void Application_OnStart(object sender, EventArgs e)
    //{
    //    string str = string.Format(eventStr, sender.ToString(), "Application_OnStart");
    //    Application["Event"] = str;
    //}
}
public class AsynchronousInit
{
    public void Start()
    {

    }
}