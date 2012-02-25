using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;

namespace WorkStudioLibrary.Web
{
    public class HtmlHelper
    {
        public static string[] PAGE_SELECTOR = new string[] { "5,5", "10,10", "20,20", "30,30", "50,50", "100,100", "1000,All" };
        public const string STR_SELECT = "<select id=\"{0}\" name=\"{0}\" class={1} title=\"{2}\">{3}</select>";
        public const string STR_SELECT_WITH_EVENT = "<select id=\"{0}\" name=\"{0}\" class={1} title=\"{2}\" {4}>{3}</select>";
        public const string STR_SELECT_ITEM = "<option value=\"{0}\">{1}</option>";
        public const string STR_SELECT_ITEM_SELECTED = "<option value=\"{0}\" selected=\"selected\">{1}</option>";
        private static string _SortHeaderFmt = "<a href=\"Javascript:{0}\" style=\"white-space:nowrap;\">{1}{2}</a>";
        private static string _SortHeaderWithStlyleFmt = "<a href=\"Javascript:{0}\" style=\"{3}\">{1}</a>{2}";
        private const string MENU_MAIN = "<div id=\"tabs11\" style='font-weight: bold;'><ul>{0}<li></ul></div>";
        private const string MENU = "<li><a href=\"{0}\"><span>{1}</span></a></li>";
        private const string MENU_ACTIVE = "<li id=\"current\"><a href=\"{0}\"><span>{1}</span></a></li>";
        private const string MAIN_MENU = "<li class=\"ml_normal\"></li><li class=\"mm_normal\"><a href=\"{0}{1}\" style=\"color: #231f20;\">{2}</a></li><li class=\"mr_normal\"></li>";
        private const string MAIN_MENU_ACTIVE = "<li class=\"ml_active\"></li><li class=\"mm_active\"><a href=\"{0}{1}\" style=\"color: #fffe03;\">{2}</a></li><li class=\"mr_active\"></li>";

        private const string STR_BUTTON = "<input id=\"{0}\" class=\"{1}\" type=\"button\" onclick=\"{2}\" value=\"{3}\"/>";
        private const string STR_DISABLEBUTTON = "<input type=\"button\" value=\"{3}\" onclick=\"{2}\" class=\"{1}\" id=\"{0}\" style=\"cursor: default; color: #BFBFBF;\" enable=\"false\" disabled=\"\"/>";
        private const string STR_WHITESPACE = "&nbsp;";
        public HtmlHelper()
        {

        }
        public static string GenMenus(Page p)
        {
            StringBuilder sb_menus = new StringBuilder();
            string absolute_url = p.Request.Url.AbsolutePath;
            System.Collections.Generic.List<string> lMenus = new System.Collections.Generic.List<string>();
            //lMenus.Add("1,Home.aspx,Home|Home.aspx");
            lMenus.Add("1,XML/Index.aspx,XML|XML/Index.aspx,XML/Edit.aspx");
            lMenus.Add("1,MSSQL/Index.aspx,MS SQL|MSSQL/Index.aspx,MSSQL/Template.aspx,MSSQL/Edit.aspx");
            //lMenus.Add("1,MYSQL/Index.aspx,MY SQL|MYSQL/Index.aspx");
            //lMenus.Add("1,Access/Index.aspx,Access|Access/Index.aspx");
            lMenus.Add("1,Downloads/Index.aspx,Downloads|Downloads/Index.aspx");
            //lMenus.Add("1,Admin/Index.aspx,Setting|Admin/Index.aspx");
            

            //lMenus.Add("1,Login.aspx?logout=1,Logout|Login.aspx");
            string resolve_url = p.ResolveUrl("~");
            foreach (string s in lMenus)
            {
                string[] menu_info = s.Split('|');
                string[] menu_detail = menu_info[0].Split(',');
                string[] menu_page_ref = menu_info[1].Split(',');
                bool is_find_active_page = false;
                foreach (string pr in menu_page_ref)
                {
                    if (absolute_url.ToLower().EndsWith(pr.ToLower()))
                    {
                        sb_menus.Append(string.Format(MAIN_MENU_ACTIVE, resolve_url, menu_detail[1], menu_detail[2]));
                        is_find_active_page = true;
                        break;
                    }
                }
                if (!is_find_active_page)
                {
                    sb_menus.Append(string.Format(MAIN_MENU, resolve_url, menu_detail[1], menu_detail[2]));
                }
            }
            return sb_menus.ToString();
        }

        public static string GenRecordInfo(int recordCount)
        {
            return string.Format(Resources.Resource.RecordsFoundString, recordCount);
        }
        public static string GenPageSize(string id, string css, decimal pageSize)
        {
            return GenPageSize(id, css, pageSize, null);
        }
        public static string GenPageSize(string id, string css, decimal pageSize, string evalCode)
        {
            StringBuilder sb_r = new StringBuilder();
            StringBuilder sb_ret = new StringBuilder();
            foreach (string s in HtmlHelper.PAGE_SELECTOR)
            {
                string[] info = s.Split(',');
                if (decimal.Parse(info[0]) == pageSize)
                    sb_ret.Append(string.Format(STR_SELECT_ITEM_SELECTED, info[0], info[1]));
                else
                    sb_ret.Append(string.Format(STR_SELECT_ITEM, info[0], info[1]));
            }
            if (evalCode != null & evalCode != "")
                return sb_r.AppendFormat(Resources.Resource.ShowString, string.Format(STR_SELECT_WITH_EVENT, id, css, "", sb_ret.ToString(), "onchange=\"ChangePageSize(this,'" + evalCode + "')\" style='width:50px;'")).ToString();
            else
                return sb_r.AppendFormat(Resources.Resource.ShowString, string.Format(STR_SELECT_WITH_EVENT, id, css, "", sb_ret.ToString(), "onchange='ChangePageSize(this)' style='width:50px;'")).ToString();
        }

        public static string MsgBoxHtml(string text, Page page)
        {
            string MsgBoxFormat = "<table width=\"100%\"><tr><td align=\"center\"><table border=\"0px\" cellpadding=\"0px\" cellspacing=\"0px\" width=\"450px\"><tr><td class=\"ad-lt\" /><td class=\"ad-mt\" /><td class=\"ad-rt\" /></tr><tr><td class=\"ad-l\" /><td class=\"ad-bg\" align=\"left\" style=\"height: 100px;\"><table width=\"90%\" border=\"0\"><tr><td style=\"padding: 5px 10px 5px 10px; width: 10%\">{0}</td><td align=\"left\" style=\"width: 90%\">{1}</td></tr></table></td><td class=\"ad-r\" /></tr><tr><td class=\"ad-lb\" /><td class=\"ad-mb\" /><td class=\"ad-rb\" /></tr></table></td></tr></table>";
            string Img = "<img src=\"" + page.ResolveUrl("~") + "images/error1.gif\" alt=\"\" />";
            return string.Format(MsgBoxFormat, Img, text);
        }
        public static string GetSort(string key, string sortColumn, bool isAsc)
        {
            if (key.ToLower() == sortColumn.ToLower())
            {
                if (isAsc)
                    return "<span class=\"asc\">&nbsp;</span>";
                else
                    return "<span class=\"desc\">&nbsp;</span>";
            }
            return string.Empty;
        }
        public static string GetSortHeader(string js, string title, string sortBy, bool isAsc, string currentSortBy)
        {
            return string.Format(_SortHeaderFmt, js, title, GetSort(currentSortBy, sortBy, isAsc));
        }
        public static string GetSortHeader(string js, string title, string sortBy, bool isAsc, string currentSortBy, string linkStyle)
        {
            return string.Format(_SortHeaderWithStlyleFmt, js, title, GetSort(currentSortBy, sortBy, isAsc), linkStyle);
        }

    }
}