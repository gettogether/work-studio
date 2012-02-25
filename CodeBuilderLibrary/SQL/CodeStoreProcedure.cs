using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CodeBuilderLibrary.SQL
{
    public class CodeStoreProcedure
    {
        public static string GetDataObject(Settings st, string objName, string spName, string simpleSql)
        {
            using (IDbConnection conn = DataAccess.ConnectionHelper.CreateConnection(st.ConnectionString))
            {
                IDataReader dr = DataAccess.SqlUtil.ExecuteReader(conn, simpleSql);
                return GetDataObject(st, dr, objName, spName);
            }
        }

        public static string GetDataObject(Settings cf, IDataReader dr, string objName, string storeProcName)
        {
            int index = 1;
            StringBuilder sbReturn = new StringBuilder();
            Dictionary<int, ColumnMapping.ColumnInfos> tableColumnInfos = new Dictionary<int, ColumnMapping.ColumnInfos>();
            tableColumnInfos[index] = ColumnMapping.GetColumnInfo(dr);
            while (dr.NextResult())
            {
                index++;
                tableColumnInfos[index] = ColumnMapping.GetColumnInfo(dr);
            }
            sbReturn.AppendLine(string.Format(@"
//------------------------------------------------------------------------------
// <auto-generated>
//     Date time = {1}
//     This code was generated by tool,Version={0}.
//     Changes to this code may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------", Config.Version, DateTime.Now));
            sbReturn.AppendLine("using System;");
            sbReturn.AppendLine("using System.Collections.Generic;");
            sbReturn.AppendLine("using System.Text;");
            sbReturn.AppendLine("using DataAccess.Data;");
            sbReturn.AppendLine("using DataMapping;");
            sbReturn.AppendLine("using System.Data;");
            sbReturn.AppendLine("");
            sbReturn.Append("namespace ").AppendLine(cf.DataObjectNameSpace);
            sbReturn.AppendLine("{");
            StringBuilder sbObjects = new StringBuilder();
            StringBuilder sbAttributes = new StringBuilder();
            for (int i = 1; i <= tableColumnInfos.Count; i++)
            {
                sbObjects.AppendLine("");
                sbObjects.AppendLine(GetSubDataObject(cf, i, tableColumnInfos[i]));
                sbObjects.Append("\t\t").AppendLine(string.Format("public class {0}Result{1}: List<{2}Result{1}>", cf.UOListPrefix, i, cf.UOPrefix));
                sbObjects.AppendLine("\t\t{");
                sbObjects.AppendLine(string.Format("\t\t\tpublic {0}Result{1}()", cf.UOListPrefix, i));
                sbObjects.AppendLine("\t\t\t{");
                sbObjects.AppendLine("\t\t\t}");
                sbObjects.AppendLine("\t\t}");

                sbAttributes.AppendLine("");
                sbAttributes.AppendLine(string.Format("\t\tprivate {0}Result{1} _Result{1};", cf.UOListPrefix, i));
                sbAttributes.AppendLine(string.Format("\t\tpublic {0}Result{1} Result{1}", cf.UOListPrefix, i));
                sbAttributes.AppendLine("\t\t{");
                sbAttributes.AppendLine("\t\t\tget");
                sbAttributes.AppendLine("\t\t\t{");
                sbAttributes.AppendLine(string.Format("\t\t\t\treturn this._Result{0};", i));
                sbAttributes.AppendLine("\t\t\t}");
                sbAttributes.AppendLine("\t\t\tset");
                sbAttributes.AppendLine("\t\t\t{");
                sbAttributes.AppendLine(string.Format("\t\t\t\tthis._Result{0} = value;", i));
                sbAttributes.AppendLine("\t\t\t}");
                sbAttributes.AppendLine("\t\t}");
            }
            sbReturn.AppendLine(string.Format("\tpublic class {0}{1} : StoreProcBase<{0}{1}, {0}{1}.Results>", cf.DOPrefix, objName));
            sbReturn.AppendLine("\t{");
            sbReturn.AppendLine(string.Format("\t\tpublic {0}{1}()", cf.DOPrefix, objName));
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine(string.Format("\t\t\tStoreProcInfo = new StoreProcInformation({0}, \"{1}\");", cf.ConnectionKey, storeProcName));
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\tpublic override Results GetResults(IDataParameter[] parameters)");
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine("\t\t\tusing(this.StoreProcInfo.Connection)");
            sbReturn.AppendLine("\t\t\t{");
            sbReturn.AppendLine(string.Format("\t\t\t\tResults results{0} = new Results();", objName));
            sbReturn.AppendLine("\t\t\t\tIDataReader dr = GetDataReader(parameters);");
            sbReturn.AppendLine(string.Format("\t\t\t\tresults{0}.Result1 = ObjectHelper.FillCollection<{1}Result1, {2}Result1>(dr);", objName, cf.UOPrefix, cf.UOListPrefix));
            for (int i = 2; i <= tableColumnInfos.Count; i++)
            {
                sbReturn.AppendLine("\t\t\t\tif (dr.NextResult())");
                sbReturn.AppendLine("\t\t\t\t{");
                sbReturn.AppendLine(string.Format("\t\t\t\t\t\tresults{0}.Result{1} = ObjectHelper.FillCollection<{2}Result{1}, {3}Result{1}>(dr);", objName, i, cf.UOPrefix, cf.UOListPrefix));
                sbReturn.AppendLine("\t\t\t\t}");
            }
            sbReturn.AppendLine(string.Format("\t\t\t\treturn results{0};", objName));
            sbReturn.AppendLine("\t\t\t}");
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\tpublic override IDataReader GetDataReader(IDataParameter[] parameters)");
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine("\t\t\treturn DataAccess.SqlUtil.ExecuteProcedureReader(StoreProcInfo.Connection, StoreProcInfo.StoreProcName, parameters);");
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\tpublic override DataSet GetDataSet(IDataParameter[] parameters)");
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine("\t\t\tusing(this.StoreProcInfo.Connection)");
            sbReturn.AppendLine("\t\t\t{");
            sbReturn.AppendLine("\t\t\t\treturn DataAccess.SqlUtil.ExecuteProcedureDataSet(StoreProcInfo.Connection, StoreProcInfo.StoreProcName, parameters);");
            sbReturn.AppendLine("\t\t\t}");
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\tpublic class Results");
            sbReturn.AppendLine("\t\t\t{");
            sbReturn.AppendLine("\t\t\t\t#region Attributes");
            sbReturn.AppendLine(sbAttributes.ToString());
            sbReturn.AppendLine("\t\t\t\t#endregion");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t\t\tpublic Results()");
            sbReturn.AppendLine("\t\t\t\t{");
            sbReturn.AppendLine("\t\t\t\t}");
            sbReturn.AppendLine("\t\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t#region Return objects");
            sbReturn.Append(sbObjects.ToString());
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t#endregion");
            sbReturn.AppendLine("\t}");
            sbReturn.AppendLine("}");
            return sbReturn.ToString();
        }
        public static string GetSubDataObject(Settings cf, int index, ColumnMapping.ColumnInfos columnInfos)
        {
            StringBuilder sbReturn = new StringBuilder();
            sbReturn.AppendLine(string.Format("\t\tpublic class {0}Result{1}", cf.UOPrefix, index));
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine(string.Format("\t\t\tpublic {0}Result{1}()", cf.UOPrefix, index));
            sbReturn.AppendLine("\t\t\t{");
            sbReturn.AppendLine("\t\t\t}");
            StringBuilder sb_columns = new StringBuilder();
            sb_columns.AppendLine("\t\t\t#region Columns");
            foreach (ColumnMapping.ColumnInfo c in columnInfos)
            {
                sb_columns.AppendLine(string.Format("\t\t\tprivate {0} _{1};", c.ColumnType, c.Column));
                sb_columns.Append(string.Format("\t\t\t[Mapping(\"{0}", c.Column));
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

            sbReturn.AppendLine(sb_columns.ToString());
            sbReturn.Append("\t\t}");
            return sbReturn.ToString();
        }
        public static string GetBusinessObject(Settings cf, string connectionString, string objName, string storeProcName)
        {
            StringBuilder sbArgs = new StringBuilder();
            StringBuilder sbArgsValues = new StringBuilder();
            StringBuilder sbParameters = new StringBuilder();
            sbParameters.AppendLine("\t\t\tList<IDataParameter> parameters = new List<IDataParameter>();");
            sbParameters.AppendLine("");
            foreach (DataObjects.doStoreProcedureParameters.uoStoreProcedureParameters p in BusinessObjects.boStoreProcedureParameters.GetStoreProcedureParameters(connectionString, storeProcName))
            {
                if (p.isoutparam > 0) continue;
                string paramName = p.name.Replace("@", "");
                if (sbArgs.Length > 0) sbArgs.Append(", ");
                if (sbArgsValues.Length > 0) sbArgsValues.Append(", ");
                sbArgsValues.Append(paramName);
                sbArgs.Append(CodeScript.GetColumnType(p.data_type)).Append(" ").Append(paramName);
                if (CodeScript.GetColumnType(p.data_type).Equals("DateTime"))
                {
                    sbParameters.AppendLine(string.Format("\t\t\tparameters.Add(new System.Data.SqlClient.SqlParameter(\"{0}\", {1}.ToString(\"yyyyMMdd\", System.Globalization.DateTimeFormatInfo.InvariantInfo)));", p.name, paramName));
                }
                else
                {
                    sbParameters.AppendLine(string.Format("\t\t\tparameters.Add(new System.Data.SqlClient.SqlParameter(\"{0}\", {1}));", p.name, paramName));
                }
            }
            StringBuilder sbReturn = new StringBuilder();
            sbReturn.AppendLine("using System;");
            sbReturn.AppendLine("using System.Collections.Generic;");
            sbReturn.AppendLine("using System.Text;");
            sbReturn.AppendLine("using System.Data;");
            sbReturn.AppendLine("using DataAccess;");
            sbReturn.AppendLine("using DataAccess.Data;");
            sbReturn.AppendLine("using DataMapping;");
            sbReturn.Append("using ").Append(cf.DataObjectNameSpace).AppendLine(";");
            sbReturn.AppendLine("");

            sbReturn.Append("namespace ").AppendLine(cf.BusinessObjectNameSpace);
            sbReturn.AppendLine("{");
            sbReturn.AppendLine(string.Format("\tpublic class {0}{1}", cf.BOPrefix, objName));
            sbReturn.AppendLine("\t{");
            sbReturn.Append("\t\t#region This source code was auto-generated by tool,Version=").AppendLine(Config.Version);

            sbReturn.AppendLine(string.Format(@"
                //------------------------------------------------------------------------------
                // <auto-generated>
                //     Date time = {1}
                //     This code was generated by tool,Version={0}.
                //     Changes to this code may cause incorrect behavior and will be lost if
                //     the code is regenerated.
                // </auto-generated>
                //------------------------------------------------------------------------------", Config.Version, DateTime.Now));
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t/// <summary>");
            sbReturn.AppendLine("\t\t/// Get parameters");
            sbReturn.AppendLine("\t\t/// </summary>");
            sbReturn.AppendLine(string.Format("\t\tpublic static IDataParameter[] GetParameters({0})", sbArgs.ToString()));
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine(sbParameters.ToString());
            sbReturn.AppendLine("\t\t\treturn parameters.ToArray();");
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t/// <summary>");
            sbReturn.AppendLine("\t\t/// Get object result");
            sbReturn.AppendLine("\t\t/// </summary>");
            sbReturn.AppendLine(string.Format("\t\tpublic static {0}{1}.Results GetResults({3}{2})", cf.DOPrefix, objName, sbArgs.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, objName));
            if (cf.IsPassConnectionStringToBusiness)
            {
                sbReturn.AppendLine("\t\t\tda.StoreProcInfo.ConnectionString = connString;");
            }
            sbReturn.AppendLine(string.Format("\t\t\treturn da.GetResults(GetParameters({0}));", sbArgsValues.ToString()));
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t/// <summary>");
            sbReturn.AppendLine("\t\t/// Get DataSet result");
            sbReturn.AppendLine("\t\t/// </summary>");
            sbReturn.AppendLine(string.Format("\t\tpublic static DataSet GetDataSet({1}{0})", sbArgs.ToString(), cf.IsPassConnectionStringToBusiness ? "string connString, " : ""));
            sbReturn.AppendLine("\t\t{");
            sbReturn.AppendLine(string.Format("\t\t\t{0}{1} da = new {0}{1}();", cf.DOPrefix, objName));
            if (cf.IsPassConnectionStringToBusiness)
            {
                sbReturn.AppendLine("\t\t\tda.ConnInfo.ConnectionString = connString;");
            }
            sbReturn.AppendLine(string.Format("\t\t\treturn da.GetDataSet(GetParameters({0}));", sbArgsValues.ToString()));
            sbReturn.AppendLine("\t\t}");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t#endregion");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t#region User extensions");
            sbReturn.AppendLine("");
            sbReturn.AppendLine("\t\t#endregion");
            sbReturn.AppendLine("\t}");
            sbReturn.AppendLine("}");
            return sbReturn.ToString();
        }
    }
}
