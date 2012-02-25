using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WorkStudioLibrary.Web
{

    /// <summary>
    /// CallbackTemplatePage 的摘要说明
    /// </summary>
    public class CallbackTemplatePage : TemplatePage
    {
        #region Properties

        public int type
        {
            get { return CommonLibrary.Utility.NumberHelper.ToInt(Request["type"], -1); }
        }

        public int PageIndex
        {
            get { return Request["page"] != null ? Convert.ToInt32(Request["page"]) : 1; }
        }

        public int PageSize
        {
            get { return Request["size"] != null ? Convert.ToInt32(Request["size"]) : 20; }
        }
        private string _DefaultSort;

        public string DefaultSort
        {
            get { return _DefaultSort; }
            set { _DefaultSort = value; _Sort = value; }
        }

        private string _Sort;
        public string Sort
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["sort"]))
                {
                    _Sort = Request["sort"];
                    return _Sort;
                }
                return _Sort;
            }
            set
            {
                _Sort = value;
            }
        }

        private bool _DefaultIsAsc = false;
        public bool DefaultIsAsc
        {
            get { return _DefaultIsAsc; }
            set { _DefaultIsAsc = value; _IsAsc = value; }
        }

        private bool _IsAsc;
        public bool IsAsc
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["asc"]))
                {
                    _IsAsc = Request["asc"] == "Y";
                }
                return _IsAsc;
            }
            set
            {
                _IsAsc = value;
            }
        }

        #endregion

        protected override void OnPreInit(EventArgs e)
        {
            //if (sess == null)
            //{
            //    Response.Clear();
            //    Response.Write("Timeout");
            //    Response.End();
            //}
            //        System.Threading.Thread.Sleep(500);
            base.OnPreInit(e);
        }

        public string GetJsonReturnString(bool success, string message)
        {
            message = message.Replace("\r\n", "").Replace("'", "''").Replace("\n", "").Replace("\"", "\\\"");
            return string.Format("{0}success:{1},message:\"{2}\"{3}", "{", (string)(success ? "true" : "false"), message, "}");
        }

        public void JsonSuccess()
        {
            Response.Write("{success:true}");
        }
        public void JsonError(string message)
        {
            Response.Write(GetJsonReturnString(false, message));
        }

        //public void SetValues<T, C>(DataAccess.Data.Interfaces.IUOBase<T, C> o, string prefix)
        //    where T : class, new()
        //    where C : System.Collections.Generic.ICollection<T>, new()
        //{
        //    Functions.Util.SetValues<T, C>(o, prefix);
        //}

        public int[] GetParameterArrayInt(string paraStr, string preFix, char separator)
        {
            List<int> ret = new List<int>();

            string[] array = paraStr.Replace(preFix, "").Split(separator);
            foreach (string s in array)
                ret.Add(int.Parse(s));
            return ret.ToArray();
        }
        public string[] GetParameterArrayString(string paraStr, string preFix, char separator)
        {
            return paraStr.Replace(preFix, "").Split(separator);
        }
    }
}