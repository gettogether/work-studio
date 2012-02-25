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
using System.Text.RegularExpressions;

public partial class Callback_MSSQL_Index : WorkStudioLibrary.Web.CallbackTemplatePage
{
    public string ProjectName
    {
        get { return Request["pn"]; }

    }

    public bool IsHighLight
    {
        get { return !string.IsNullOrEmpty(Request["hl"]); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        switch (type)
        {
            case 0:
                GetTables();
                break;
            case 1:
                GenDataObjectByTable(Request["tn"]);
                break;
            case 2:
                GenBusinessObjectByTable(Request["tn"]);
                break;
            case 3:
                GetDataObjectByQuery(Request["tn"], Request["pk"], Request["sql"]);
                break;
            case 4:
                GetBusinessObjectByQuery(Request["tn"], Request["pk"], Request["sql"]);
                break;
            case 5:
                GetStoreProcedures();
                break;
            case 6:
                GetStoreProcedureParameters(Request["spn"]);
                break;
            case 7:
                GetDataObjectByStoreProce(Request["on"], Request["spn"], Request["sql"]);
                break;
            case 8:
                GetDataBusinessByStoreProce(Request["on"], Request["spn"], Request["sql"]);
                break;
            default:
                break;
        }
    }

    #region Functions

    #region From Table

    private void GenDataObjectByTable(string table)
    {
        if (!string.IsNullOrEmpty(ProjectName) && !string.IsNullOrEmpty(table))
        {
            WorkStudioLibrary.Logging.Files.Log.Info(string.Format("Get data object by table:{0},{1}", ProjectName, table));
            CodeBuilderLibrary.Settings st = MSSQL.GetSettingsByProjectName(ProjectName);
            CodeBuilderLibrary.DataObjects.doPKs.uoListPKs listPks = CodeBuilderLibrary.BusinessObjects.boPks.GetPrimaryKeys(st.ConnectionString, table);
            CodeBuilderLibrary.DataObjects.doColumns.uoListColumns listColumns = CodeBuilderLibrary.BusinessObjects.boColumns.GetColumns(st.ConnectionString, table);
            CodeBuilderLibrary.DataObjects.doRemarks.uoListRemarks listRemarks = CodeBuilderLibrary.BusinessObjects.boRemarks.GetRemarks(st.ConnectionString, table);
            string doString = CodeBuilderLibrary.SQL.CodeScript.GetDataObject(st, table, listColumns, listPks, listRemarks);
            if (!IsHighLight)
            {
                Response.Write(string.Concat("<textarea rows='22' id='txt-table-do-value' style='width:99%;font-size: 8pt;border-width:0px;'>", doString, "</textarea>"));

            }
            else
            {
                //Response.Write(new HtmlCodeColor.CodeColor(new HtmlCodeColor.CSharpAdapter()).ColorCode(doString));
                Response.Write(doString.Replace("\r\n", "<br />").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));
            }
        }
    }

    private void GenBusinessObjectByTable(string table)
    {
        if (!string.IsNullOrEmpty(ProjectName) && !string.IsNullOrEmpty(table))
        {
            WorkStudioLibrary.Logging.Files.Log.Info(string.Format("Get business object by table:{0},{1}", ProjectName,table));
            CodeBuilderLibrary.Settings st = MSSQL.GetSettingsByProjectName(ProjectName);
            CodeBuilderLibrary.DataObjects.doPKs.uoListPKs listPks = CodeBuilderLibrary.BusinessObjects.boPks.GetPrimaryKeys(st.ConnectionString, table);
            CodeBuilderLibrary.DataObjects.doColumns.uoListColumns listColumns = CodeBuilderLibrary.BusinessObjects.boColumns.GetColumns(st.ConnectionString, table);
            string boString = CodeBuilderLibrary.SQL.CodeScript.GetBusinessObject(st, table, listColumns, listPks);
            if (!IsHighLight)
            {
                Response.Write(string.Concat("<textarea rows='22' id='txt-table-bo-value' style='width:99%;font-size: 8pt;border-width:0px;'>", boString, "</textarea>"));

            }
            else
            {
                //Response.Write(new HtmlCodeColor.CodeColor(new HtmlCodeColor.CSharpAdapter()).ColorCode(boString));
                Response.Write(boString.Replace("\r\n", "<br />").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));
            }
        }
    }

    private void GetTables()
    {
        if (!string.IsNullOrEmpty(ProjectName))
        {
            CodeBuilderLibrary.Settings st = MSSQL.GetSettingsByProjectName(ProjectName);
            Components_MSSQL_Tables ct = Page.LoadControl("~/Components/MSSQL/Tables.ascx") as Components_MSSQL_Tables;
            ct.ConnectionString = st.ConnectionString;
            this.Controls.Add(ct);
        }
    }

    #endregion

    #region From Query

    private void GetDataObjectByQuery(string tableName, string primaryKeys, string sql)
    {
        if (!string.IsNullOrEmpty(ProjectName) && !string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(primaryKeys))
        {
            try
            {
                WorkStudioLibrary.Logging.Files.Log.Info(string.Format("Get data object by query:{0},{1},{2},{3}", ProjectName, tableName, primaryKeys, sql));
                CodeBuilderLibrary.Settings st = MSSQL.GetSettingsByProjectName(ProjectName);
                System.Data.IDataReader dr = DataAccess.SqlUtil.ExecuteReader(DataAccess.ConnectionHelper.CreateConnection(st.ConnectionString), sql);
                CodeBuilderLibrary.ColumnMapping.ColumnInfos cis = CodeBuilderLibrary.ColumnMapping.GetColumnInfo(dr);
                string doString = CodeBuilderLibrary.SQL.CodeDataReader.GetDataObject(st, cis, tableName, primaryKeys, sql);
                if (!IsHighLight)
                {
                    Response.Write(string.Concat("<textarea rows='15' id='txt-query-do-value' style='width:99%;font-size: 8pt;border-width:0px;'>", doString, "</textarea>"));

                }
                else
                {
                    //Response.Write(new HtmlCodeColor.CodeColor(new HtmlCodeColor.CSharpAdapter()).ColorCode(doString));
                    Response.Write(doString.Replace("\r\n", "<br />").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        else
        {
            Response.Write(string.Format("Invalid Parameters[Project Name:{0},Table Name:{1},Primary Key(s):{2},SQL:{3}", ProjectName, tableName, primaryKeys, sql));
        }
    }

    private void GetBusinessObjectByQuery(string tableName, string primaryKeys, string sql)
    {
        if (!string.IsNullOrEmpty(ProjectName) && !string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(primaryKeys))
        {
            try
            {
                WorkStudioLibrary.Logging.Files.Log.Info(string.Format("Get business object by query:{0},{1},{2},{3}", ProjectName, tableName, primaryKeys, sql));
                CodeBuilderLibrary.Settings st = MSSQL.GetSettingsByProjectName(ProjectName);
                System.Data.IDataReader dr = DataAccess.SqlUtil.ExecuteReader(DataAccess.ConnectionHelper.CreateConnection(st.ConnectionString), sql);
                CodeBuilderLibrary.ColumnMapping.ColumnInfos cis = CodeBuilderLibrary.ColumnMapping.GetColumnInfo(dr);
                string boString = CodeBuilderLibrary.SQL.CodeDataReader.GetBusinessObject(st, cis, tableName, primaryKeys);
                if (!IsHighLight)
                {
                    Response.Write(string.Concat("<textarea rows='15' id='txt-query-bo-value' style='width:99%;font-size: 8pt;border-width:0px;'>", boString, "</textarea>"));

                }
                else
                {
                    //Response.Write(new HtmlCodeColor.CodeColor(new HtmlCodeColor.CSharpAdapter()).ColorCode(doString));
                    Response.Write(boString.Replace("\r\n", "<br />").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        else
        {
            Response.Write(string.Format("Invalid Parameters[Project Name:{0},Table Name:{1},Primary Key(s):{2},SQL:{3}", ProjectName, tableName, primaryKeys, sql));
        }
    }

    #endregion

    #region From Store Procedure

    private void GetStoreProcedures()
    {
        if (!string.IsNullOrEmpty(ProjectName))
        {
            CodeBuilderLibrary.Settings st = MSSQL.GetSettingsByProjectName(ProjectName);
            Components_MSSQL_StoreProcedures sp = Page.LoadControl("~/Components/MSSQL/StoreProcedures.ascx") as Components_MSSQL_StoreProcedures;
            sp.ConnectionString = st.ConnectionString;
            this.Controls.Add(sp);
        }
    }

    private void GetStoreProcedureParameters(string spName)
    {
        if (!string.IsNullOrEmpty(ProjectName) && !string.IsNullOrEmpty(spName))
        {
            string simpleSql = CodeBuilderLibrary.SQL.Util.GetStoreProcedureSimple(MSSQL.GetSettingsByProjectName(ProjectName), spName);
            Response.Write(string.Concat("<textarea rows='10' cols=\"10\" id='SP_Sql' style='width:99%;font-size: 8pt;height:100px;' class=\"txt\">", simpleSql, "</textarea>"));
        }
    }

    private void GetDataObjectByStoreProce(string objName, string spName, string sql)
    {
        if (!string.IsNullOrEmpty(ProjectName) && !string.IsNullOrEmpty(objName) && !string.IsNullOrEmpty(spName) && !string.IsNullOrEmpty(sql))
        {
            try
            {
                WorkStudioLibrary.Logging.Files.Log.Info(string.Format("Get data object by store procedure:{0},{1},{2},{3}", ProjectName, spName, objName, sql));
                CodeBuilderLibrary.Settings st = MSSQL.GetSettingsByProjectName(ProjectName);
                string doString = CodeBuilderLibrary.SQL.CodeStoreProcedure.GetDataObject(st, objName, spName, sql);
                if (!IsHighLight)
                {
                    Response.Write(string.Concat("<textarea rows='15' id='txt-store-procedure-do-value' style='width:99%;font-size: 8pt;border-width:0px;'>", doString, "</textarea>"));
                }
                else
                {
                    //Response.Write(new HtmlCodeColor.CodeColor(new HtmlCodeColor.CSharpAdapter()).ColorCode(doString));
                    Response.Write(doString.Replace("\r\n", "<br />").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        else
        {
            Response.Write(string.Format("Invalid Parameters[Project Name:{0},Object Name:{1},Store Procedure:{2},SQL:{3}", ProjectName, objName, spName, sql));
        }
    }
    private void GetDataBusinessByStoreProce(string objName, string spName, string sql)
    {
        if (!string.IsNullOrEmpty(ProjectName) && !string.IsNullOrEmpty(objName) && !string.IsNullOrEmpty(spName) && !string.IsNullOrEmpty(sql))
        {
            try
            {
                WorkStudioLibrary.Logging.Files.Log.Info(string.Format("Get business object by store procedure:{0},{1},{2},{3}", ProjectName, spName, objName, sql));
                CodeBuilderLibrary.Settings st = MSSQL.GetSettingsByProjectName(ProjectName);
                string doString = CodeBuilderLibrary.SQL.CodeStoreProcedure.GetBusinessObject(st, st.ConnectionString, objName, spName);
                if (!IsHighLight)
                {
                    Response.Write(string.Concat("<textarea rows='15' id='txt-store-procedure-bo-value' style='width:99%;font-size: 8pt;border-width:0px;'>", doString, "</textarea>"));
                }
                else
                {
                    //Response.Write(new HtmlCodeColor.CodeColor(new HtmlCodeColor.CSharpAdapter()).ColorCode(doString));
                    Response.Write(doString.Replace("\r\n", "<br />").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        else
        {
            Response.Write(string.Format("Invalid Parameters[Project Name:{0},Object Name:{1},Store Procedure:{2},SQL:{3}", ProjectName, objName, spName, sql));
        }
    }

    #endregion

    #endregion
}


namespace HtmlCodeColor
{
    /// <summary>
    /// CodeColor为代码着色器
    /// </summary>
    public class CodeColor
    {
        private ICodeColorAdapter _Adp;

        public CodeColor(ICodeColorAdapter adp)
        {
            _Adp = adp;
        }

        public string ColorCode(string src)
        {
            string retCode = src;
            retCode = System.Web.HttpUtility.HtmlEncode(retCode);
            retCode = _Adp.ColorKeyword(retCode);
            retCode = _Adp.ColorComment(retCode);
            retCode = _Adp.IndentCode(retCode);
            retCode = _Adp.CollapseCode(retCode);
            return retCode;
        }
    }
    /// <summary>
    /// ICodeColorAdapter为代码着色适配器接口
    /// </summary>
    public interface ICodeColorAdapter
    {
        /// <summary>
        /// 为注释着色，例如：对C#，包括///型、//型和/**/的注释
        /// </summary>
        /// <param name="src">输入源码</param>
        /// <returns>格式化后的源码</returns>
        string ColorComment(string src);

        /// <summary>
        /// 为关键字着色
        /// </summary>
        /// <param name="src">输入源码</param>
        /// <returns>格式化后的源码</returns>
        string ColorKeyword(string src);

        /// <summary>
        /// 使代码支持折叠显示，例如：对C#需要折叠的代码有：namespace, class, method, region, comment, property等
        /// </summary>
        /// <param name="src">输入源码</param>
        /// <returns>格式化后的源码</returns>
        string CollapseCode(string src);

        /// <summary>
        /// 代码缩进
        /// </summary>
        /// <param name="src">输入源码</param>
        /// <returns>格式化后的源码</returns>
        string IndentCode(string src);
    }
    /// <summary>
    /// C#代码着色适配器
    /// </summary>
    public class CSharpAdapter : ICodeColorAdapter
    {
        #region Constructors

        public CSharpAdapter()
        {
            //null
        }

        #endregion

        #region Private Members

        /// <summary>
        /// 为//和/**/类型的注释着色
        /// </summary>
        /// <param name="src">输入源码</param>
        /// <returns>格式化后的源码</returns>
        private string ColorBasicComment(string src)
        {
            string retCode = src;

            Regex r1 = new Regex(@"(^|;)([ \t]*)(//.*$)", RegexOptions.Multiline);
            retCode = r1.Replace(retCode, "$1$2<span style=\"color: green\">$3</span>");
            Regex r2 = new Regex(@"(^|[ \t]+)(/\*[^\*/]*\*/[ \t\r]*$)", RegexOptions.Multiline);
            retCode = r2.Replace(retCode, new MatchEvaluator(this.ColorBasicComment2Evaluator));

            return retCode;
        }

        /// <summary>
        /// 为///类型的注释着色
        /// </summary>
        /// <param name="src">输入源码</param>
        /// <returns>格式化后的源码</returns>
        private string ColorXmlComment(string src)
        {
            string retCode = src;

            Regex r1 = new Regex(@"(/// *)&lt;(.*)&gt;", RegexOptions.Multiline);
            retCode = r1.Replace(retCode, new MatchEvaluator(this.ColorXmlCommentEvaluator));
            Regex r2 = new Regex(@"(/// *)(.*$)", RegexOptions.Multiline);
            retCode = r2.Replace(retCode, "<span style=\"color: gray\">$1</span>$2");

            return retCode;
        }

        /// <summary>
        /// 为折叠显示代码构建HtmlTable框架
        /// </summary>
        /// <param name="src">输入源码</param>
        /// <returns>格式化后的源码</returns>
        private string DrawCollapseFrameTable(string src)
        {
            System.Text.StringBuilder retCode = new System.Text.StringBuilder();

            string frameHeader = "<table border=0 cellpadding=1 cellspacing=0 width=100%>";
            string frameTailer = "</table>";

            retCode.Append(frameHeader);

            string[] lines = src.Split('\n');

            foreach (string line in lines)
            {
                string formatedLine = line.Trim();

                string lineHeader = "<tr><td style='width: 0px'><table style='width: 9px; height: 9px'><tr><td></td></tr></table></td><td style='width: 0px'></td><td style='width: 100%'><code style='font: 9pt Tahoma'>";
                string lineTailer = "</code></td></tr>";

                formatedLine = lineHeader + formatedLine + lineTailer;

                retCode.Append(formatedLine);
            }

            retCode.Append(frameTailer);

            return retCode.ToString();
        }

        /// <summary>
        /// 折叠Region
        /// </summary>
        /// <param name="src">输入源码</param>
        /// <returns>格式化后的源码</returns>
        private string CollapseRegion(string src)
        {
            string retCode = src;

            string lineHeader = "<tr><td style='width: 0px'><table style='width: 9px; height: 9px'><tr><td></td></tr></table></td><td style='width: 0px'></td><td style='width: 100%'><code style='font: 9pt Tahoma'>";

            Regex r = new Regex(lineHeader + @"(&nbsp;)*<span style=\""color: blue\"">#region<.*$", RegexOptions.Multiline);
            while (r.Match(retCode).Success)
            {
                //add "+" tag
                retCode = r.Replace(retCode, new MatchEvaluator(this.CollapseRegionEvaluator));
            }

            return retCode;
        }
        /// <summary>
        /// 为一行源码中的关键字着色
        /// </summary>
        /// <param name="codeLine">某一行源码</param>
        /// <param name="keywordList">关键字列表</param>
        /// <returns>格式化后的源码</returns>
        /// <summary>
        private string ColorKeyword(string codeLine, string[] keywordList)
        {
            string retCode = codeLine;

            if (!retCode.StartsWith("//"))
            {
                foreach (string keyword in KEYWORD_LIST)
                {
                    Regex r = new Regex(@"(^|\s+|,|\)|\(|\{|\}|\[|\]|\.|=|;)(" + keyword + @")(\s+|,|\)|\(|\{|\}|\[|\]|\.|=|;|$)");

                    retCode = r.Replace(retCode, "$1<span style=\"color: blue\">$2</span>$3");
                }
            }

            retCode = ClearColoredKeyworsInString(retCode);

            return retCode.Trim();
        }

        /// <summary>
        /// 清除字符串中的被着色的关键字
        /// </summary>
        /// <param name="codeLine">某一行源码</param>
        /// <returns>格式化后的源码</returns>
        private string ClearColoredKeyworsInString(string codeLine)
        {
            System.Text.StringBuilder retCode = new System.Text.StringBuilder();

            string str = codeLine.Trim();

            int indexOfQuot = str.IndexOf("&quot;");
            int lengthOfQuot = "&quot;".Length;

            if (indexOfQuot >= 0)
            {
                while (indexOfQuot >= 0)
                {
                    retCode.Append(str.Substring(0, indexOfQuot + lengthOfQuot));
                    str = str.Substring(indexOfQuot + lengthOfQuot);

                    indexOfQuot = str.IndexOf("&quot;");

                    if (indexOfQuot >= 0)
                    {
                        string inStr = str.Substring(0, indexOfQuot + lengthOfQuot);

                        inStr = inStr.Replace("\"color: blue\"", "\"\"");

                        retCode.Append(inStr);
                        str = str.Substring(indexOfQuot + lengthOfQuot);
                    }

                    indexOfQuot = str.IndexOf("&quot;");
                }
            }

            retCode.Append(str);

            return retCode.ToString();
        }


        public static string[] KEYWORD_LIST = { 
			"abstract", "event", "new", "struct", "as", "explicit", "null", "switch",
			"base", "extern", "object", "this",	"bool", "false", "operator", "throw",
			"break", "finally", "out", "true", "byte", "fixed", "override", "try",
            "case", "float", "params", "typeof", "catch", "for", "private", "uint",
            "char", "foreach", "protected", "ulong", "checked", "goto", "public", 
			"unchecked", "class", "if", "readonly", "unsafe", "const", "implicit", 
			"ref", "ushort", "continue", "in", "return", "using", "decimal", "int", 
			"sbyte", "virtual",	"default", "interface", "sealed", "volatile", 
			"delegate", "internal", "short", "void", "do", "is", "sizeof", "while", 
			"double", "lock", "stackalloc", "else", "long", "static", "enum", 
			"namespace", "string", "get", "set", "#region", "#endregion", "true", 
			"false","ListBase"
		};

        private string ColorXmlCommentEvaluator(Match m)
        {
            Regex r = new Regex("(^.*&gt;)(.*)(&lt;/.*$)");
            if (r.Match(m.Value).Success)
            {
                return r.Replace(m.Value, "<span style=\"color: gray\">$1</span>$2<span style=\"color: gray\">$3</span>");
            }
            else
            {
                return "<span style=\"color: gray\">" + m.Value + "</span>";
            }
        }

        private string ColorBasicComment2Evaluator(Match m)
        {
            System.Text.StringBuilder retCode = new System.Text.StringBuilder();

            string[] lines = m.Value.Split('\n');

            foreach (string line in lines)
            {
                retCode.Append("<span style=\"color: green\">" + line + "</span>" + "\n");
            }

            return retCode.ToString();
        }

        private string CollapseRegionEvaluator(Match m)
        {
            //get region code block

            string retCode = m.Value;

            string lineHeader = "<tr><td style='width: 0px'><table style='width: 9px; height: 9px'><tr><td></td></tr></table></td><td style='width: 0px'></td><td style='width: 100%'><code style='font: 9pt Tahoma'>";
            string lineTailer = "</code></td></tr>";

            Regex r = new Regex("^" + lineHeader + @"(&nbsp;)*<span style=\""color: blue\"">#region</span>[^<]*" + lineTailer, RegexOptions.Multiline);
            retCode = r.Replace(retCode, new MatchEvaluator(this.CollapseRegionEvaluator2));

            //find out #region - #endregion block & add collapse code
            string endRegionLinePattern = "<tr><td style='width: 0px'><table style='width: 9px; height: 9px'><tr><td></td></tr></table></td><td style='width: 0px'></td><td style='width: 100%'><code style='font: 9pt Tahoma'>(&nbsp;)*<span style=\"color: blue\">#endregion</span></code></td></tr>";
            Regex r2 = new Regex(endRegionLinePattern);
            string endRegionLine = r2.Match(retCode).Value;
            string formatedEndRegionLine = endRegionLine.Replace("<td style='width: 0px'></td><td style='width: 100%'>", "<td style='width: 0px'><span style='position: relative; left: -12px; font: 12px'><font color=#808080 face='Tahoma'>-</font></span></td><td style='width: 100%'>");
            string regionBlock = retCode.Substring(0, retCode.IndexOf(endRegionLine)) + formatedEndRegionLine;

            string spaces = new Regex("(&nbsp;)+").Match(regionBlock).Value;
            string regionDescWidthTag = new Regex("#region</span> ([^<]*)</code>").Match(regionBlock).Value;
            string regionDesc = regionDescWidthTag.Substring(15, regionDescWidthTag.Length - ("#region</span> ").Length - ("</code>").Length);

            regionBlock = regionBlock.Replace("</tr><tr><td style='width: 0px'>", "</tr><tr style='display: none'><td style='width: 0px'>");
            regionBlock = regionBlock.Substring(0, regionBlock.IndexOf("<code style='font: 9pt Tahoma'>") + "<code style='font: 9pt Tahoma'>".Length) +
                "<span>" + spaces + "</span>" + "<span style='border: 1px solid gray'><font color=gray>" + regionDesc + "</font></span>" +
                regionBlock.Substring(regionBlock.IndexOf("</code>"));

            retCode = regionBlock + retCode.Substring(retCode.IndexOf(endRegionLine) + endRegionLine.Length);

            return retCode;
        }


        private string CollapseRegionEvaluator2(Match m)
        {
            string retCode = m.Value;

            //add rectangle
            retCode = retCode.Replace("style='width: 9px; height: 9px'", "style='position: relative; left: -6px; top: 0px; border-color: #808080; border-style: solid; border-width: 1px; background: white; width: 9px; height: 9px'");

            string spaces = new Regex("(&nbsp;)+").Match(m.Value).Value;
            string regionDescWidthTag = new Regex("#region</span> ([^<]*)</code>").Match(m.Value).Value;
            string regionDesc = regionDescWidthTag.Substring(15, regionDescWidthTag.Length - ("#region</span> ").Length - ("</code>").Length);

            //add "+" tag
            retCode = retCode.Replace("<td style='width: 0px'></td>", "<td style='width: 0px'><span style='position: relative; left: -16px; top: -1px; font: 12px; cursor: hand' onclick='if (this.childNodes[0].innerHTML == \"_\") { this.childNodes[0].innerHTML = \"+\"; this.style.top = \"-1px\"; this.parentNode.nextSibling.childNodes[0].innerHTML = \"<span>" + spaces + "</span><span><font color=gray>" + regionDesc + "</font></span>\"; this.parentNode.nextSibling.childNodes[0].childNodes[1].style.border = \"1px solid gray\"; for (var i = 1 + this.parentNode.parentNode.rowIndex; i < this.parentNode.parentNode.parentNode.childNodes.length; i++) { this.parentNode.parentNode.parentNode.childNodes[i].style.display=\"none\"; if ( this.parentNode.parentNode.parentNode.childNodes[i].childNodes[1].innerText.length > 0 ) { break; } } } else { this.childNodes[0].innerHTML = \"_\"; this.style.top = \"-6px\"; var oldDesc = this.parentNode.nextSibling.childNodes[0].childNodes[1].childNodes[0].innerHTML; this.parentNode.nextSibling.childNodes[0].innerHTML = \"<span>" + spaces + "</span><span><font color=blue>#region </font></span><span></span>\"; this.parentNode.nextSibling.childNodes[0].childNodes[2].innerHTML = oldDesc; for (var i = 1 + this.parentNode.parentNode.rowIndex; i < this.parentNode.parentNode.parentNode.childNodes.length; i++) { this.parentNode.parentNode.parentNode.childNodes[i].style.display=\"block\"; if ( this.parentNode.parentNode.parentNode.childNodes[i].childNodes[1].innerText.length > 0 ) { break; } } }'><font color=#808080 face='Tahoma'>+</font></span></td>");

            return retCode;
        }

        #endregion

        #region ICodeColorAdapter 成员

        /// <summary>
        /// 为注释着色，例如：对C#，包括///型、//型和/**/的注释
        /// </summary>
        /// <param name="src">输入源码</param>
        /// <returns>格式化后的源码</returns>
        public string ColorComment(string src)
        {
            string retCode = src;
            retCode = ColorBasicComment(retCode);
            retCode = ColorXmlComment(retCode);
            return retCode;
        }

        /// <summary>
        /// 为关键字着色
        /// </summary>
        /// <param name="src">输入源码</param>
        /// <returns>格式化后的源码</returns>
        public string ColorKeyword(string src)
        {
            System.Text.StringBuilder retCode = new System.Text.StringBuilder();

            string[] lines = src.Split('\n');

            if (lines != null && lines.Length > 0)
            {
                bool isInComment = false;

                foreach (string line in lines)
                {
                    string formatedLine = line.Trim();

                    if (new Regex(@"(^|[ \t]+)(/\*)").Match(line).Success)
                    {
                        isInComment = true;
                    }

                    if (!isInComment)
                    {
                        formatedLine = ColorKeyword(line, KEYWORD_LIST);
                    }

                    if (new Regex(@"\*/[ \t\r]*$").Match(line).Success)
                    {
                        isInComment = false;
                    }

                    retCode.Append(formatedLine + "\n");
                }
            }

            return retCode.ToString();
        }

        /// <summary>
        /// 使代码支持折叠显示
        /// </summary>
        /// <param name="src">输入源码</param>
        /// <returns>格式化后的源码</returns>
        public string CollapseCode(string src)
        {
            string retCode = src;
            retCode = DrawCollapseFrameTable(retCode);
            retCode = CollapseRegion(retCode);
            return retCode;
        }

        /// <summary>
        /// 代码缩进
        /// </summary>
        /// <param name="src">输入源码</param>
        /// <returns>格式化后的源码</returns>
        public string IndentCode(string src)
        {
            System.Text.StringBuilder retCode = new System.Text.StringBuilder();

            int indent = 0;
            string[] lines = src.Split('\n');

            foreach (string line in lines)
            {
                string formatedLine = line.Trim();

                Regex r = new Regex(@"\}(\}|\s)*;?\s*$");
                if (r.Match(line).Success)
                {
                    indent--;
                }

                for (int i = 0; i < indent; i++)
                {
                    formatedLine = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + formatedLine;
                }

                retCode.Append(formatedLine + "\n");

                if (line.EndsWith("{"))
                {
                    indent++;
                }
                else if (line.StartsWith("{"))
                {
                    indent++;
                }
            }

            return retCode.ToString();
        }
        #endregion
    }
}