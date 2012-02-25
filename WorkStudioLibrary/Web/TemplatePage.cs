using System;
using System.Collections.Generic;
using System.Text;

namespace WorkStudioLibrary.Web
{
    public class TemplatePage : CommonLibrary.WebObject.TemplatePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            if (Session["culture_string"] == null) Session["culture_string"] = "en-us";
            //if (sess == null)
            //{
            //    Response.Redirect("~/Login.aspx?to=1&lang=" + CommonLibrary.Utility.MutiLanguage.GetLanguageString(), true);
            //}
            if (sess == null)
            {
                sess = new WorkStudioLibrary.Web.SessionObjects();
                sess.Lastname = System.Web.HttpContext.Current.Request.UserHostAddress;
                Session["culture_string"] = "en-us";
                CommonLibrary.Utility.LogHelper.Write(
                    "Login",
                    CommonLibrary.Utility.LogHelper.LogTypes.Info,
                    string.Concat(Request.UserHostAddress,
                    " ",
                    CommonLibrary.WebObject.WebHelper.GetBrowserInfo()));
                System.Web.HttpContext.Current.Session["login_time"] = DateTime.Now;
            }
            base.OnPreInit(e);
        }
        public SessionObjects sess
        {
            get { return Session[Definition.Session.SESSION_KEY] == null ? null : (SessionObjects)Session[Definition.Session.SESSION_KEY]; }
            set { Session[Definition.Session.SESSION_KEY] = value; }
        }
        public TemplatePage()
        {

        }

    }
}
