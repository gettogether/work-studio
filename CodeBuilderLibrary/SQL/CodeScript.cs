using System;
using System.Collections.Generic;
using System.Text;
using CodeBuilderLibrary.DataObjects;

namespace CodeBuilderLibrary.SQL
{
    public static class CodeScript
    {
        public static string GetColumnType(string type)
        {
            string reval;

            switch (type.ToLower())
            {
                case "int":
                    reval = "int";
                    break;
                case "text":
                    reval = "string";
                    break;
                case "bigint":
                    reval = "Int64";
                    break;
                case "binary":
                    reval = "byte[]";
                    break;
                case "bit":
                    reval = "bool";
                    break;
                case "char":
                    reval = "string";
                    break;
                case "datetime":
                    reval = "DateTime";
                    break;
                case "decimal":
                    reval = "decimal";
                    break;
                case "float":
                    reval = "double";
                    break;
                case "image":
                    reval = "byte[]";
                    break;
                case "money":
                    reval = "decimal";
                    break;
                case "nchar":
                    reval = "string";
                    break;
                case "ntext":
                    reval = "string";
                    break;
                case "numeric":
                    reval = "decimal";
                    break;
                case "nvarchar":
                    reval = "string";
                    break;
                case "real":
                    reval = "Single";
                    break;
                case "smalldatetime":
                    reval = "DateTime";
                    break;
                case "smallint":
                    reval = "Int16";
                    break;
                case "smallmoney":
                    reval = "decimal";
                    break;
                case "timestamp":
                    reval = "DateTime";
                    break;
                case "tinyint":
                    reval = "byte";
                    break;
                case "uniqueidentifier":
                    reval = "Guid";
                    break;
                case "varbinary":
                    reval = "byte[]";
                    break;
                case "varchar":
                    reval = "string";
                    break;
                case "Variant":
                    reval = "object";
                    break;
                default:
                    reval = "string";
                    break;
            }
            return reval;
        }
        public static string GetColumnType(doColumns.uoListColumns columns, string column)
        {
            foreach (doColumns.uoColumns c in columns)
            {
                if (c.column_name.Equals(column))
                {
                    return GetColumnType(c.data_type);
                }
            }
            return "string";
        }
        public static string GetDataObject(Settings cf, string tableName, doColumns.uoListColumns listColumn, doPKs.uoListPKs listPK, doRemarks.uoListRemarks listRemarks)
        {
            StringBuilder sb_ret = new StringBuilder();
            sb_ret.AppendLine(string.Format(@"
//------------------------------------------------------------------------------
// <auto-generated>
//     Date time = {1}
//     This code was generated by tool,Version={0}.
//     Changes to this code may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------", Config.Version, DateTime.Now));
            sb_ret.AppendLine("using System;");
            sb_ret.AppendLine("using System.Collections.Generic;");
            sb_ret.AppendLine("using System.Text;");
            sb_ret.AppendLine("using DataAccess.Data;");
            sb_ret.AppendLine("using DataMapping;");
            sb_ret.AppendLine("");
            sb_ret.Append("namespace ").AppendLine(cf.DataObjectNameSpace);
            sb_ret.AppendLine("{");
            sb_ret.AppendLine(string.Format("\tpublic class {1}{0} : DOBase<{1}{0}.{2}{0}, {1}{0}.{3}{0}>", tableName, cf.DOPrefix, cf.UOPrefix, cf.UOListPrefix));
            sb_ret.AppendLine("\t{");
            sb_ret.AppendLine("\t\tpublic enum Columns");
            sb_ret.AppendLine("\t\t{");
            StringBuilder sb_columns = new StringBuilder();
            sb_columns.AppendLine("\t\t\t#region Columns");
            StringBuilder sbPks = new StringBuilder();
            foreach (doPKs.uoPKs p in listPK)
            {
                if (p.name.Trim().IndexOf(" ") > 0) p.name = p.name.Trim().Replace(" ", "_space_");
                if (sbPks.Length > 0) sbPks.Append(", ");
                sbPks.Append("\"").Append(p.name).Append("\"");
            }
            sbPks.Insert(0, "new string[] {");
            sbPks.Append("}");
            foreach (doColumns.uoColumns c in listColumn)
            {
                if (c.column_name.Trim().IndexOf(" ") > 0) c.column_name = c.column_name.Trim().Replace(" ", "_space_");
                bool isPrimaryKey = false;
                foreach (doPKs.uoPKs p in listPK)
                {
                    if (c.column_name.Equals(p.name))
                    {
                        isPrimaryKey = true;
                        break;
                    }
                }
                StringBuilder sbColumnInfo = new StringBuilder();
                if (isPrimaryKey)
                {
                    sbColumnInfo.Append("Primary Key,");
                }
                sbColumnInfo.Append("Database Type:").Append(c.data_type);
                if (!string.IsNullOrEmpty(c.max_length))
                {
                    sbColumnInfo.Append(",Max Length:").Append(c.max_length);
                }
                sbColumnInfo.Append(",Is Nullable:").Append(c.is_nullable);
                if (!string.IsNullOrEmpty(c.column_default))
                {
                    sbColumnInfo.Append(",Default Value:").Append(c.column_default);
                }
                foreach (doRemarks.uoRemarks r in listRemarks)
                {
                    if (r.column_name.Equals(c.column_name))
                    {
                        sbColumnInfo.Append(",Remark:").Append(r.remark);
                        break;
                    }
                }
                sb_ret.AppendLine("\t\t\t/// <summary>");
                sb_ret.Append("\t\t\t///").AppendLine(sbColumnInfo.ToString());
                sb_ret.AppendLine("\t\t\t/// </summary>");
                sb_ret.Append("\t\t\t").Append(c.column_name).AppendLine(",");
                string vtype = CodeScript.GetColumnType(c.data_type);
                sb_columns.AppendLine(string.Format("\t\t\tprivate {0} _{1};", vtype, c.column_name));
                sb_columns.AppendLine("\t\t\t/// <summary>");
                sb_columns.Append("\t\t\t///").AppendLine(sbColumnInfo.ToString());
                sb_columns.AppendLine("\t\t\t/// </summary>");
                sb_columns.Append(string.Format("\t\t\t[Mapping(\"{0}", c.column_name));
                if (isPrimaryKey)
                {
                    sb_columns.Append(",un-insert,un-update");
                }
                else if (cf.UnInsertAndUnUpdate.ToLower().IndexOf(c.column_name.ToLower()) >= 0)
                {
                    sb_columns.Append(",un-insert,un-update");
                }
                else if (cf.UnInsert.ToLower().IndexOf(c.column_name.ToLower()) >= 0)
                {
                    sb_columns.Append(",un-insert");
                }
                else if (cf.UnUpdate.ToLower().IndexOf(c.column_name.ToLower()) >= 0)
                {
                    sb_columns.Append(",un-update");
                }
                sb_columns.AppendLine("\")]");
                sb_columns.AppendLine(string.Format("\t\t\tpublic {0} {1}", vtype, c.column_name));
                sb_columns.AppendLine("\t\t\t{");
                sb_columns.AppendLine("\t\t\t\tget");
                sb_columns.AppendLine("\t\t\t\t{");
                sb_columns.AppendLine(string.Format("\t\t\t\t\treturn _{0};", c.column_name));
                sb_columns.AppendLine("\t\t\t\t}");
                sb_columns.AppendLine("\t\t\t\tset");
                sb_columns.AppendLine("\t\t\t\t{");
                sb_columns.AppendLine(string.Format("\t\t\t\t\t_{0} = value;", c.column_name));
                sb_columns.AppendLine("\t\t\t\t}");
                sb_columns.AppendLine("\t\t\t}");
            }
            sb_columns.AppendLine("\t\t\t#endregion");
            sb_ret.AppendLine("\t\t}");
            sb_ret.AppendLine(string.Format("\t\tpublic {1}{0}()", tableName, cf.DOPrefix));
            sb_ret.AppendLine("\t\t{");
            if (listPK.Count > 0)
            {
                sb_ret.AppendLine(string.Format("\t\t\tConnInfo = new ConnectionInformation(\"{0}\", {1},{2});", tableName, cf.ConnectionKey, sbPks.ToString()));
            }
            else
            {
                sb_ret.AppendLine(string.Format("\t\t\tConnInfo = new ConnectionInformation(\"{0}\", {1});", tableName, cf.ConnectionKey));
            }
            sb_ret.AppendLine("\t\t}");
            sb_ret.AppendLine(string.Format("\t\tpublic class {1}{0} : UOBase<{1}{0},{2}{0}>", tableName, cf.UOPrefix, cf.UOListPrefix));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine(sb_columns.ToString());
            sb_ret.AppendLine(string.Format("\t\t\tpublic {1}{0}()", tableName, cf.UOPrefix));
            sb_ret.AppendLine("\t\t\t{");
            sb_ret.AppendLine(string.Format("\t\t\t\tConnInfo = new {0}{1}().ConnInfo;", cf.DOPrefix, tableName));
            sb_ret.AppendLine("\t\t\t}");
            sb_ret.AppendLine("\t\t}");
            sb_ret.AppendLine(string.Format("\t\tpublic class {1}{0} : CommonLibrary.ObjectBase.ListBase<{2}{0}>", tableName, cf.UOListPrefix, cf.UOPrefix));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine(string.Format("\t\t\tpublic {1}{0}()", tableName, cf.UOListPrefix));
            sb_ret.AppendLine("\t\t\t{");
            sb_ret.AppendLine("\t\t\t}");
            sb_ret.AppendLine("\t\t}");
            sb_ret.AppendLine("\t}");
            sb_ret.AppendLine("}");
            return sb_ret.ToString();
        }
        public static string GetBusinessObject(Settings cf, string tableName, doColumns.uoListColumns listColumn, doPKs.uoListPKs listPK)
        {
            StringBuilder sb_ret = new StringBuilder();
            StringBuilder sbCondition = new StringBuilder();
            StringBuilder sbParameter = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            StringBuilder sbPagingCondition = new StringBuilder();
            sbPagingCondition.AppendLine("\t\t\tParameterCollection objectConditions = new ParameterCollection();");
            sbPagingCondition.AppendLine("\t\t\tTokenTypes tt = tokenTypes;");
            sbPagingCondition.AppendLine("\t\t\tParameterType pt = isAnd ? ParameterType.And : ParameterType.Or;");
            foreach (doColumns.uoColumns c in listColumn)
            {
                if (c.column_name.Trim().IndexOf(" ") > 0) c.column_name = c.column_name.Replace(" ", "_space_");
                if (GetColumnType(c.data_type).Equals("string"))
                {
                    sbPagingCondition.AppendLine(string.Format("\t\t\tif (!string.IsNullOrEmpty(parameterObj.{0}))", c.column_name));
                    sbPagingCondition.AppendLine("\t\t\t{");
                    sbPagingCondition.AppendLine(string.Format("\t\t\t\tobjectConditions.AddCondition(pt, GetColumnTokenType(tt,{0}{1}.Columns.{2},extTokens), {0}{1}.Columns.{2},parameterObj.{2});", cf.DOPrefix, tableName, c.column_name));
                    sbPagingCondition.AppendLine("\t\t\t}");
                }
                else if (GetColumnType(c.data_type).Equals("int") || GetColumnType(c.data_type).Equals("decimal") || GetColumnType(c.data_type).Equals("float"))
                {
                    sbPagingCondition.AppendLine(string.Format("\t\t\tif (parameterObj.{0} != 0 || (extTokens != null && extTokens.ContainsKey({1}{2}.Columns.{0})))", c.column_name, cf.DOPrefix, tableName));
                    sbPagingCondition.AppendLine("\t\t\t{");
                    sbPagingCondition.AppendLine(string.Format("\t\t\t\tobjectConditions.AddCondition(pt, GetColumnTokenType(tt,{0}{1}.Columns.{2},extTokens), {0}{1}.Columns.{2},parameterObj.{2});", cf.DOPrefix, tableName, c.column_name));
                    sbPagingCondition.AppendLine("\t\t\t}");
                }
                else if (GetColumnType(c.data_type).Equals("DateTime"))
                {
                    sbPagingCondition.AppendLine(string.Format("\t\t\tif (parameterObj.{0} != DateTime.MinValue)", c.column_name));
                    sbPagingCondition.AppendLine("\t\t\t{");
                    sbPagingCondition.AppendLine(string.Format("\t\t\t\tobjectConditions.AddCondition(pt, GetColumnTokenType(tt,{0}{1}.Columns.{2},extTokens), {0}{1}.Columns.{2},parameterObj.{2});", cf.DOPrefix, tableName, c.column_name));
                    sbPagingCondition.AppendLine("\t\t\t}");
                }
            }
            sbCondition.AppendLine("\t\t\tParameterCollection primaryConditions = new ParameterCollection();");
            for (int i = 0; i < listPK.Count; i++)
            {
                if (listPK[i].name.Trim().IndexOf(" ") > 0) listPK[i].name = listPK[i].name.Trim().Replace(" ", "_space_");
                if (sbParameter.Length > 0) sbParameter.Append(",");
                sbParameter.Append(GetColumnType(listColumn, listPK[i].name)).Append(" ").Append(listPK[i].name);
                if (sbValues.Length > 0) sbValues.Append(",");
                sbValues.Append(listPK[i].name);
                if (i == 0)
                {
                    sbCondition.AppendLine(string.Format("\t\t\tprimaryConditions.AddCondition(ParameterType.Initial, TokenTypes.Equal, {0}{1}.Columns.{2}, {2});", cf.DOPrefix, tableName, listPK[i].name, listPK[i].name));
                }
                else
                {
                    sbCondition.AppendLine(string.Format("\t\t\tprimaryConditions.AddCondition(ParameterType.And, TokenTypes.Equal, {0}{1}.Columns.{2}, {2});", cf.DOPrefix, tableName, listPK[i].name, listPK[i].name));
                }
            }
            sb_ret.AppendLine("using System;");
            sb_ret.AppendLine("using System.Collections.Generic;");
            sb_ret.AppendLine("using System.Text;");
            sb_ret.AppendLine("using System.Data;");
            sb_ret.AppendLine("using DataAccess;");
            sb_ret.AppendLine("using DataAccess.Data;");
            sb_ret.AppendLine("using DataMapping;");
            sb_ret.Append("using ").Append(cf.DataObjectNameSpace).AppendLine(";");
            sb_ret.AppendLine("");

            sb_ret.Append("namespace ").AppendLine(cf.BusinessObjectNameSpace);
            sb_ret.AppendLine("{");
            sb_ret.AppendLine(string.Format("\tpublic class {0}{1}", cf.BOPrefix, tableName));
            sb_ret.AppendLine("\t{");

            sb_ret.Append("\t\t#region This source code was auto-generated by tool,Version=").AppendLine(Config.Version);

            sb_ret.AppendLine(string.Format(@"
                //------------------------------------------------------------------------------
                // <auto-generated>
                //     Date time = {1}
                //     This code was generated by tool,Version={0}.
                //     Changes to this code may cause incorrect behavior and will be lost if
                //     the code is regenerated.
                // </auto-generated>
                //------------------------------------------------------------------------------", Config.Version, DateTime.Now));
            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t#region Condition functions");
            if (listPK.Count > 0)
            {
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Get conditions by primary key.");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static ParameterCollection GetConditionsByPrimaryKey({0})", sbParameter.ToString()));
                sb_ret.AppendLine("\t\t{");
                sb_ret.Append(sbCondition.ToString());
                sb_ret.AppendLine("\t\t\treturn primaryConditions;");
                sb_ret.AppendLine("\t\t}");
            }
            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get the tokenType of the column of condition query.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tprivate static TokenTypes GetColumnTokenType(TokenTypes defaultTokenType,{0}{1}.Columns column,Dictionary<{0}{1}.Columns,TokenTypes> extTokens)", cf.DOPrefix, tableName, cf.UOPrefix));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine("\t\t\tif (extTokens != null && extTokens.ContainsKey(column))");
            sb_ret.AppendLine("\t\t\t\treturn extTokens[column];");
            sb_ret.AppendLine("\t\t\telse");
            sb_ret.AppendLine("\t\t\t\treturn defaultTokenType;");
            sb_ret.AppendLine("\t\t}");
            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get conditions by object with Multi-TokenType.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static ParameterCollection GetConditionsByObject({0}{1}.{2}{1} parameterObj, bool isAnd, TokenTypes tokenTypes, Dictionary<{0}{1}.Columns, TokenTypes> extTokens)", cf.DOPrefix, tableName, cf.UOPrefix));
            sb_ret.AppendLine("\t\t{");
            sb_ret.Append(sbPagingCondition.ToString());
            sb_ret.AppendLine("\t\t\treturn objectConditions;");
            sb_ret.AppendLine("\t\t}");
            sb_ret.AppendLine("\t\t#endregion");
            sb_ret.AppendLine("");

            sb_ret.AppendLine("\t\t#region Query functions");
            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get all records.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static {0}{1}.{2}{1} GetAllList({3})", cf.DOPrefix, tableName, cf.UOListPrefix, cf.IsPassConnectionStringToBusiness ? "string connString" : ""));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, tableName));
            if (cf.IsPassConnectionStringToBusiness)
            {
                sb_ret.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
            }
            sb_ret.AppendLine("\t\t\treturn da.GetAllList();");
            sb_ret.AppendLine("\t\t}");

            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get all records count.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static int GetAllRecordsCount({0})", cf.IsPassConnectionStringToBusiness ? "string connString" : ""));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, tableName));
            if (cf.IsPassConnectionStringToBusiness)
            {
                sb_ret.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
            }
            sb_ret.AppendLine("\t\t\treturn da.GetRecordsCount();");
            sb_ret.AppendLine("\t\t}");

            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get records count.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static int GetRecordsCount({3}{0}{1}.{2}{1} parameterObj)", cf.DOPrefix, tableName, cf.UOPrefix, cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine(string.Format("\t\t\treturn GetRecordsCount({0}parameterObj, true, TokenTypes.Equal,null);", cf.IsPassConnectionStringToBusiness ? "connString, " : ""));
            sb_ret.AppendLine("\t\t}");

            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get records count.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static int GetRecordsCount({3}{0}{1}.{2}{1} parameterObj, bool isAnd, TokenTypes tokenTypes, Dictionary<{0}{1}.Columns, TokenTypes> extTokens)", cf.DOPrefix, tableName, cf.UOPrefix, cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sb_ret.AppendLine("\t\t{");
            sb_ret.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, tableName));
            if (cf.IsPassConnectionStringToBusiness)
            {
                sb_ret.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
            }
            sb_ret.AppendLine("\t\t\treturn da.GetRecordsCount(GetConditionsByObject(parameterObj, isAnd, tokenTypes, extTokens));");
            sb_ret.AppendLine("\t\t}");

            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get list by object.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static {0}{1}.{2}{1} GetList({4}{0}{1}.{3}{1} parameterObj, bool isAnd, TokenTypes tokenTypes, Dictionary<{0}{1}.Columns, TokenTypes> extTokens)", cf.DOPrefix, tableName, cf.UOListPrefix, cf.UOPrefix, cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sb_ret.AppendLine("\t\t{");
            if (cf.IsPassConnectionStringToBusiness)
            {
                sb_ret.AppendLine("\t\t\tparameterObj.ConnInfo.ConnectionString = connString;");
            }
            sb_ret.AppendLine("\t\t\treturn parameterObj.GetList(GetConditionsByObject(parameterObj, isAnd, tokenTypes, extTokens));");
            sb_ret.AppendLine("\t\t}");

            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t///<summary>");
            sb_ret.AppendLine("\t\t///Get list by object.");
            sb_ret.AppendLine("\t\t///</summary>");
            sb_ret.AppendLine(string.Format("\t\tpublic static {0}{1}.{2}{1} GetList({4}{0}{1}.{3}{1} parameterObj)", cf.DOPrefix, tableName, cf.UOListPrefix, cf.UOPrefix, cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sb_ret.AppendLine("\t\t{");
            if (cf.IsPassConnectionStringToBusiness)
            {
                sb_ret.AppendLine("\t\t\tparameterObj.ConnInfo.ConnectionString = connString;");
            }
            sb_ret.AppendLine(string.Format("\t\t\treturn GetList({0}parameterObj, true, TokenTypes.Equal, null);", cf.IsPassConnectionStringToBusiness ? "connString, " : ""));
            sb_ret.AppendLine("\t\t}");
            if (listPK.Count > 0)
            {
                sb_ret.AppendLine("");
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Get object by primary key.");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static {0}{1}.{2}{1} GetObject({4}{3})", cf.DOPrefix, tableName, cf.UOPrefix, sbParameter.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
                sb_ret.AppendLine("\t\t{");
                sb_ret.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, tableName));
                if (cf.IsPassConnectionStringToBusiness)
                {
                    sb_ret.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
                }
                sb_ret.AppendLine(string.Format("\t\t\t{0}{1}.{2}{1} l = da.GetList(GetConditionsByPrimaryKey({3}));", cf.DOPrefix, tableName, cf.UOListPrefix, sbValues.ToString()));
                sb_ret.AppendLine("\t\t\treturn l.Count > 0 ? l[0] : null;");
                sb_ret.AppendLine("\t\t}");

                sb_ret.AppendLine("");
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Get paging list.");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static PagingResult<{0}{1}.{3}{1}, {0}{1}.{2}{1}> GetPagingList({4}{0}{1}.{3}{1} parameterObj,int pageNumber, int pageSize,string sortBy,bool isAsc, bool isAnd, TokenTypes tokenTypes, Dictionary<{0}{1}.Columns, TokenTypes> extTokens)", cf.DOPrefix, tableName, cf.UOListPrefix, cf.UOPrefix, cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
                sb_ret.AppendLine("\t\t{");
                if (cf.IsPassConnectionStringToBusiness)
                {
                    sb_ret.AppendLine("\t\t\tparameterObj.ConnInfo.ConnectionString = connString;");
                }
                sb_ret.AppendLine(string.Format("\t\t\treturn parameterObj.GetPagingList(GetConditionsByObject(parameterObj, isAnd, tokenTypes,extTokens), pageNumber, pageSize, sortBy, isAsc);", cf.DOPrefix, tableName));
                sb_ret.AppendLine("\t\t}");

                sb_ret.AppendLine("");
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Get paging list.");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static PagingResult<{0}{1}.{3}{1}, {0}{1}.{2}{1}> GetPagingList({4}{0}{1}.{3}{1} parameterObj,int pageNumber, int pageSize,string sortBy,bool isAsc)", cf.DOPrefix, tableName, cf.UOListPrefix, cf.UOPrefix, cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
                sb_ret.AppendLine("\t\t{");
                if (cf.IsPassConnectionStringToBusiness)
                {
                    sb_ret.AppendLine("\t\t\tparameterObj.ConnInfo.ConnectionString = connString;");
                }
                sb_ret.AppendLine(string.Format("\t\t\treturn parameterObj.GetPagingList(GetConditionsByObject(parameterObj, true, TokenTypes.Like,null), pageNumber, pageSize, sortBy, isAsc);", cf.IsPassConnectionStringToBusiness ? "string connString" : ""));
                sb_ret.AppendLine("\t\t}");
            }
            sb_ret.AppendLine("\t\t#endregion");
            if (listPK.Count > 0)
            {
                sb_ret.AppendLine("");
                sb_ret.AppendLine("\t\t#region Update functions");
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Update object by primary key.");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static bool UpdateObject({4}{0}{1}.{2}{1} obj, {3})", cf.DOPrefix, tableName, cf.UOPrefix, sbParameter.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
                sb_ret.AppendLine("\t\t{");
                if (cf.IsPassConnectionStringToBusiness)
                {
                    sb_ret.AppendLine("\t\t\tobj.ConnInfo.ConnectionString = connString;");
                }
                sb_ret.AppendLine(string.Format("\t\t\treturn obj.Update(GetConditionsByPrimaryKey({0})) > 0;", sbValues.ToString()));
                sb_ret.AppendLine("\t\t}");

                sb_ret.AppendLine("");
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Update object by primary key(with transation).");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static bool UpdateObject({4}{0}{1}.{2}{1} obj, {3}, IDbConnection connection, IDbTransaction transaction)", cf.DOPrefix, tableName, cf.UOPrefix, sbParameter.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
                sb_ret.AppendLine("\t\t{");
                if (cf.IsPassConnectionStringToBusiness)
                {
                    sb_ret.AppendLine("\t\t\tobj.ConnInfo.ConnectionString = connString;");
                }
                sb_ret.AppendLine(string.Format("\t\t\treturn obj.Update(connection, transaction, GetConditionsByPrimaryKey({0})) > 0;", sbValues.ToString()));
                sb_ret.AppendLine("\t\t}");
                sb_ret.AppendLine("\t\t#endregion");

                sb_ret.AppendLine("");
                sb_ret.AppendLine("\t\t#region Delete functions");
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Delete object by primary key.");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static int Delete({1}{0})", sbParameter.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
                sb_ret.AppendLine("\t\t{");
                sb_ret.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, tableName));
                if (cf.IsPassConnectionStringToBusiness)
                {
                    sb_ret.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
                }
                sb_ret.AppendLine(string.Format("\t\t\treturn da.Delete(GetConditionsByPrimaryKey({0}));", sbValues.ToString()));
                sb_ret.AppendLine("\t\t}");

                sb_ret.AppendLine("");
                sb_ret.AppendLine("\t\t///<summary>");
                sb_ret.AppendLine("\t\t///Delete object by primary key(with transation).");
                sb_ret.AppendLine("\t\t///</summary>");
                sb_ret.AppendLine(string.Format("\t\tpublic static int Delete({1}{0}, IDbConnection connection, IDbTransaction transaction)", sbParameter.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
                sb_ret.AppendLine("\t\t{");
                sb_ret.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, tableName));
                if (cf.IsPassConnectionStringToBusiness)
                {
                    sb_ret.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
                }
                sb_ret.AppendLine(string.Format("\t\t\treturn da.Delete(connection, transaction, GetConditionsByPrimaryKey({0}));", sbValues.ToString()));
                sb_ret.AppendLine("\t\t}");
                sb_ret.AppendLine("\t\t#endregion");
                sb_ret.AppendLine("");
            }
            sb_ret.AppendLine("\t\t#endregion");

            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t#region User extensions");
            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t#endregion");

            sb_ret.AppendLine("\t}");
            sb_ret.AppendLine("}");
            return sb_ret.ToString();
        }
    }
}
