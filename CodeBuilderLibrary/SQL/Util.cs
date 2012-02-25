using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBuilderLibrary.SQL
{
    public class Util
    {
        public static string GetStoreProcedureSimple(Settings st, string spName)
        {
            DataObjects.doStoreProcedureParameters.uoListStoreProcedureParameters spParameters = BusinessObjects.boStoreProcedureParameters.GetStoreProcedureParameters(st.ConnectionString, spName);
            StringBuilder sbInput = new StringBuilder();

            for (int i = 0; i < spParameters.Count; i++)
            {
                DataObjects.doStoreProcedureParameters.uoStoreProcedureParameters p = spParameters[i];
                string v = "0";
                if (SQL.CodeScript.GetColumnType(p.data_type).Equals("string"))
                {
                    v = "N''";
                }
                sbInput.Append("\t").Append(p.name).Append(" = ").Append(v);
                if (i < spParameters.Count - 1) sbInput.Append(",");
                sbInput.Append("\t\t--Database type:").Append(p.data_type);
                sbInput.Append(", Length:").Append(p.length);
                sbInput.Append(", Is nullable:").AppendLine(p.isnullable > 0 ? "True" : "False");
            }
            if (sbInput.Length > 0) sbInput.Remove(sbInput.Length - 1, 1);
            sbInput.Insert(0, string.Format("EXEC\t[{0}]\r\n", spName));
            return sbInput.ToString();
        }
    }
}
