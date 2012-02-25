using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBuilderLibrary.SQL
{
    public class CodeDataReader
    {
        public static string GetDataObject(Settings cf, ColumnMapping.ColumnInfos ci, string tableName, string PK, string sql)
        {
            string[] primaryKeys = PK.Split(',');
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
            foreach (string p in primaryKeys)
            {
                if (sbPks.Length > 0) sbPks.Append(", ");
                sbPks.Append("\"").Append(p).Append("\"");
            }
            sbPks.Insert(0, "new string[] {");
            sbPks.Append("}");

            foreach (ColumnMapping.ColumnInfo c in ci)
            {
                bool isPrimaryKey = false;
                foreach (string s in primaryKeys)
                {
                    if (c.Column.Trim().ToLower().Equals(s.Trim().ToLower()))
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
                //sbColumnInfo.Append("Database Type:").Append(c.ColumnType);
                //if (!string.IsNullOrEmpty(c.max_length))
                //{
                //    sbColumnInfo.Append(",Max Length:").Append(c.max_length);
                //}
                //sbColumnInfo.Append(",Is Nullable:").Append(c.is_nullable);
                //if (!string.IsNullOrEmpty(c.column_default))
                //{
                //    sbColumnInfo.Append(",Default Value:").Append(c.column_default);
                //}
                //foreach (doRemarks.uoRemarks r in listRemarks)
                //{
                //    if (r.column_name.Equals(c.column_name))
                //    {
                //        sbColumnInfo.Append(",Remark:").Append(r.remark);
                //        break;
                //    }
                //}
                //sb_ret.AppendLine("\t\t\t/// <summary>");
                //sb_ret.Append("\t\t\t///").AppendLine(sbColumnInfo.ToString());
                //sb_ret.AppendLine("\t\t\t/// </summary>");
                sb_ret.Append("\t\t\t").Append(c.Column).AppendLine(",");
                sb_columns.AppendLine(string.Format("\t\t\tprivate {0} _{1};", c.ColumnType, c.Column));
                //sb_columns.AppendLine("\t\t\t/// <summary>");
                //sb_columns.Append("\t\t\t///").AppendLine(sbColumnInfo.ToString());
                //sb_columns.AppendLine("\t\t\t/// </summary>");
                sb_columns.Append(string.Format("\t\t\t[Mapping(\"{0}", c.Column));
                if (isPrimaryKey)
                {
                    sb_columns.Append(",un-insert,un-update");
                }
                else if (cf.UnInsertAndUnUpdate.ToLower().IndexOf(c.Column.ToLower()) >= 0)
                {
                    sb_columns.Append(",un-insert,un-update");
                }
                else if (cf.UnInsert.ToLower().IndexOf(c.Column.ToLower()) >= 0)
                {
                    sb_columns.Append(",un-insert");
                }
                else if (cf.UnUpdate.ToLower().IndexOf(c.Column.ToLower()) >= 0)
                {
                    sb_columns.Append(",un-update");
                }
                sb_columns.AppendLine("\")]");
                sb_columns.AppendLine(string.Format("\t\t\tpublic {0} {1}", c.ColumnType, c.Column));
                sb_columns.AppendLine("\t\t\t{");
                sb_columns.AppendLine("\t\t\t\tget");
                sb_columns.AppendLine("\t\t\t\t{");
                sb_columns.AppendLine(string.Format("\t\t\t\t\treturn _{0};", c.Column));
                sb_columns.AppendLine("\t\t\t\t}");
                sb_columns.AppendLine("\t\t\t\tset");
                sb_columns.AppendLine("\t\t\t\t{");
                sb_columns.AppendLine(string.Format("\t\t\t\t\t_{0} = value;", c.Column));
                sb_columns.AppendLine("\t\t\t\t}");
                sb_columns.AppendLine("\t\t\t}");
            }
            sb_columns.AppendLine("\t\t\t#endregion");
            sb_ret.AppendLine("\t\t}");
            sb_ret.AppendLine(string.Format("\t\tpublic {1}{0}()", tableName, cf.DOPrefix));
            sb_ret.AppendLine("\t\t{");
            if (primaryKeys != null && primaryKeys.Length > 0)
            {
                sb_ret.AppendLine(string.Format("\t\t\tConnInfo = new ConnectionInformation(\"{0}\", {1}, {2});", sql, cf.ConnectionKey, sbPks.ToString()));
            }
            else
            {
                sb_ret.AppendLine(string.Format("\t\t\tConnInfo = new ConnectionInformation(\"{0}\", {1});", sql, cf.ConnectionKey));
            }
            sb_ret.AppendLine("\t\t\tConnInfo.IsSqlSentence = true;");
            sb_ret.AppendLine("\t\t}");
            sb_ret.AppendLine(string.Format("\t\tpublic class {1}{0} : UOBase<{1}{0}, {2}{0}>", tableName, cf.UOPrefix, cf.UOListPrefix));
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
        private static string GetColumnType(ColumnMapping.ColumnInfos cis, string columnName)
        {
            foreach (ColumnMapping.ColumnInfo c in cis)
            {
                if (c.Column.Trim().ToLower() == columnName.Trim().ToLower())
                {
                    return c.ColumnType;
                }
            }
            return "System.String";
        }
        public static string GetBusinessObject(Settings cf, ColumnMapping.ColumnInfos cis, string tableName, string PK)
        {
            StringBuilder sb_ret = new StringBuilder();
            StringBuilder sbCondition = new StringBuilder();
            StringBuilder sbParameter = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            StringBuilder sbPagingCondition = new StringBuilder();
            string[] primaryKeys = PK.Split(',');
            sbPagingCondition.AppendLine("\t\t\tParameterCollection objectConditions = new ParameterCollection();");
            sbPagingCondition.AppendLine("\t\t\tTokenTypes tt = tokenTypes;");
            sbPagingCondition.AppendLine("\t\t\tParameterType pt = isAnd ? ParameterType.And : ParameterType.Or;");
            foreach (ColumnMapping.ColumnInfo c in cis)
            {
                if (c.ColumnType.Equals("System.String"))
                {
                    sbPagingCondition.AppendLine(string.Format("\t\t\tif (!string.IsNullOrEmpty(parameterObj.{0}))", c.Column));
                    sbPagingCondition.AppendLine("\t\t\t{");
                    sbPagingCondition.AppendLine(string.Format("\t\t\t\tobjectConditions.AddCondition(pt, GetColumnTokenType(tt,{0}{1}.Columns.{2},extTokens), {0}{1}.Columns.{2},parameterObj.{2});", cf.DOPrefix, tableName, c.Column));
                    sbPagingCondition.AppendLine("\t\t\t}");
                }
                else if (c.ColumnType.Equals("System.Int32") || c.ColumnType.Equals("System.Decimal") || c.ColumnType.Equals("System.Single") || c.ColumnType.Equals("System.Int16") || c.ColumnType.Equals("System.Int64"))
                {
                    sbPagingCondition.AppendLine(string.Format("\t\t\tif (parameterObj.{0} != 0 || (extTokens != null && extTokens.ContainsKey({1}{2}.Columns.{0})))", c.Column, cf.DOPrefix, tableName));
                    sbPagingCondition.AppendLine("\t\t\t{");
                    sbPagingCondition.AppendLine(string.Format("\t\t\t\tobjectConditions.AddCondition(pt, GetColumnTokenType(tt,{0}{1}.Columns.{2},extTokens), {0}{1}.Columns.{2},parameterObj.{2});", cf.DOPrefix, tableName, c.Column));
                    sbPagingCondition.AppendLine("\t\t\t}");
                }
                else if (c.ColumnType.Equals("System.DateTime"))
                {
                    sbPagingCondition.AppendLine(string.Format("\t\t\tif (parameterObj.{0} != DateTime.MinValue)", c.Column));
                    sbPagingCondition.AppendLine("\t\t\t{");
                    sbPagingCondition.AppendLine(string.Format("\t\t\t\tobjectConditions.AddCondition(pt, GetColumnTokenType(tt,{0}{1}.Columns.{2},extTokens), {0}{1}.Columns.{2},parameterObj.{2});", cf.DOPrefix, tableName, c.Column));
                    sbPagingCondition.AppendLine("\t\t\t}");
                }
            }
            sbCondition.AppendLine("\t\t\tParameterCollection primaryConditions = new ParameterCollection();");
            for (int i = 0; i < primaryKeys.Length; i++)
            {
                if (sbParameter.Length > 0) sbParameter.Append(",");
                sbParameter.Append(GetColumnType(cis, primaryKeys[i])).Append(" ").Append(primaryKeys[i]);
                if (sbValues.Length > 0) sbValues.Append(",");
                sbValues.Append(primaryKeys[i]);
                if (i == 0)
                {
                    sbCondition.AppendLine(string.Format("\t\t\tprimaryConditions.AddCondition(ParameterType.Initial, TokenTypes.Equal, {0}{1}.Columns.{2}, {2});", cf.DOPrefix, tableName, primaryKeys[i], primaryKeys[i]));
                }
                else
                {
                    sbCondition.AppendLine(string.Format("\t\t\tprimaryConditions.AddCondition(ParameterType.And, TokenTypes.Equal, {0}{1}.Columns.{2}, {2});", cf.DOPrefix, tableName, primaryKeys[i], primaryKeys[i]));
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
            if (primaryKeys != null && primaryKeys.Length > 0)
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
            sb_ret.AppendLine(string.Format("\t\t\treturn da.GetRecordsCount(GetConditionsByObject(parameterObj, isAnd, tokenTypes, extTokens));", cf.DOPrefix, tableName));
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
            sb_ret.AppendLine(string.Format("\t\t\treturn GetList({0}parameterObj, true, TokenTypes.Equal, null);", cf.IsPassConnectionStringToBusiness ? "connString, " : ""));
            sb_ret.AppendLine("\t\t}");
            if (primaryKeys != null && primaryKeys.Length > 0)
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
                sb_ret.AppendLine(string.Format("\t\t\treturn parameterObj.GetPagingList(GetConditionsByObject(parameterObj, true, TokenTypes.Like,null), pageNumber, pageSize, sortBy, isAsc);", cf.DOPrefix, tableName));
                sb_ret.AppendLine("\t\t}");
            }
            sb_ret.AppendLine("");
            sb_ret.AppendLine("\t\t#endregion");
            //if (primaryKeys != null && primaryKeys.Length > 0)
            //{
            //    sb_ret.AppendLine("");
            //    sb_ret.AppendLine("\t\t#region Update functions");
            //    sb_ret.AppendLine("\t\t///<summary>");
            //    sb_ret.AppendLine("\t\t///Update object by primary key.");
            //    sb_ret.AppendLine("\t\t///</summary>");
            //    sb_ret.AppendLine(string.Format("\t\tpublic static bool UpdateObject({0}{1}.{2}{1} obj, {3})", cf.DOPrefix, tableName, cf.UOPrefix, sbParameter.ToString()));
            //    sb_ret.AppendLine("\t\t{");
            //    sb_ret.AppendLine(string.Format("\t\t\treturn obj.Update(GetConditionsByPrimaryKey({0}), obj) > 0;", sbValues.ToString()));
            //    sb_ret.AppendLine("\t\t}");

            //    sb_ret.AppendLine("");
            //    sb_ret.AppendLine("\t\t///<summary>");
            //    sb_ret.AppendLine("\t\t///Update object by primary key(with transation).");
            //    sb_ret.AppendLine("\t\t///</summary>");
            //    sb_ret.AppendLine(string.Format("\t\tpublic static bool UpdateObject({0}{1}.{2}{1} obj, {3}, IDbConnection connection, IDbTransaction transaction)", cf.DOPrefix, tableName, cf.UOPrefix, sbParameter.ToString()));
            //    sb_ret.AppendLine("\t\t{");
            //    sb_ret.AppendLine(string.Format("\t\t\treturn obj.Update(connection, transaction, GetConditionsByPrimaryKey({0}), obj) > 0;", sbValues.ToString()));
            //    sb_ret.AppendLine("\t\t}");
            //    sb_ret.AppendLine("\t\t#endregion");

            //    sb_ret.AppendLine("");
            //    sb_ret.AppendLine("\t\t#region Delete functions");
            //    sb_ret.AppendLine("\t\t///<summary>");
            //    sb_ret.AppendLine("\t\t///Delete object by primary key.");
            //    sb_ret.AppendLine("\t\t///</summary>");
            //    sb_ret.AppendLine(string.Format("\t\tpublic static int Delete({0})", sbParameter.ToString()));
            //    sb_ret.AppendLine("\t\t{");
            //    sb_ret.AppendLine(string.Format("\t\t\treturn new {0}{1}().Delete(GetConditionsByPrimaryKey({2}));", cf.DOPrefix, tableName, sbValues.ToString()));
            //    sb_ret.AppendLine("\t\t}");

            //    sb_ret.AppendLine("");
            //    sb_ret.AppendLine("\t\t///<summary>");
            //    sb_ret.AppendLine("\t\t///Delete object by primary key(with transation).");
            //    sb_ret.AppendLine("\t\t///</summary>");
            //    sb_ret.AppendLine(string.Format("\t\tpublic static int Delete({0}, IDbConnection connection, IDbTransaction transaction)", sbParameter.ToString()));
            //    sb_ret.AppendLine("\t\t{");
            //    sb_ret.AppendLine(string.Format("\t\t\treturn new {0}{1}().Delete(connection, transaction, GetConditionsByPrimaryKey({2}));", cf.DOPrefix, tableName, sbValues.ToString()));
            //    sb_ret.AppendLine("\t\t}");
            //    sb_ret.AppendLine("\t\t#endregion");
            //    sb_ret.AppendLine("");
            //}
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
