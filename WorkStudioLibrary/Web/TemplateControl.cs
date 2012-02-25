using System;
using System.Data;
using System.Configuration;
using System.Web;

namespace WorkStudioLibrary.Web
{
    /// <summary>
    /// TemplateControl 的摘要说明
    /// </summary>
    public class TemplateControl : System.Web.UI.UserControl
    {
        public SessionObjects sess
        {
            get { return Session[Definition.Session.SESSION_KEY] == null ? null : (SessionObjects)Session[Definition.Session.SESSION_KEY]; }
            set { Session[Definition.Session.SESSION_KEY] = value; }
        }
        public TemplateControl()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
    }
}