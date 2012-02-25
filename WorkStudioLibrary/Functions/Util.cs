using System;
using System.Collections.Generic;
using System.Text;

namespace WorkStudioLibrary.Functions
{
    public class Util
    {
        //public static void SetValues<T, C>(DataAccess.Data.Interfaces.IUOBase<T, C> o, string prefix)
        //    where T : class, new()
        //    where C : System.Collections.Generic.ICollection<T>, new()
        //{
        //    foreach (string k in System.Web.HttpContext.Current.Request.Form.AllKeys)
        //    {
        //        if (k.StartsWith(prefix))
        //        {
        //            string column = k.Substring(k.IndexOf("_") + 1);
        //            o[column, CommonLibrary.Utility.DateHelper.GetDateString(CommonLibrary.Utility.DateHelper.DateFormat.ddMMyyyys)] = System.Web.HttpUtility.UrlDecode(System.Web.HttpContext.Current.Request[k]);
        //        }
        //    }
        //}

        public static string GetRandomPassword()
        {
            return new Random().Next().ToString().Substring(0, 6);
        }
    }
}