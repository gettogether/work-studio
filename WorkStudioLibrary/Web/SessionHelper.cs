using System;
using System.Collections.Generic;
using System.Text;

namespace WorkStudioLibrary.Web
{
    public class SessionHelper
    {
        public static SessionObjects GetCurrentSession()
        {
            return System.Web.HttpContext.Current.Session[Definition.Session.SESSION_KEY] == null ? null : (SessionObjects)System.Web.HttpContext.Current.Session[Definition.Session.SESSION_KEY];
        }
       
    }
}
