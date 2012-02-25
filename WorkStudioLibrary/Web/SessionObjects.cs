using System;
using System.Data;
using System.Configuration;
using System.Web;

namespace WorkStudioLibrary.Web
{
    /// <summary>
    /// SessionObjects 的摘要说明
    /// </summary>
    public class SessionObjects
    {
        public SessionObjects()
        {
            
        }

        private CodeBuilderLibrary.Settings _MsSqlSetting;

        public CodeBuilderLibrary.Settings MsSqlSetting
        {
            get { return _MsSqlSetting; }
            set { _MsSqlSetting = value; }
        }

        private string _Firstname;

        public string Firstname
        {
            get { return _Firstname; }
            set { _Firstname = value; }
        }
        private string _Lastname;

        public string Lastname
        {
            get { return _Lastname; }
            set { _Lastname = value; }
        }

    }
}