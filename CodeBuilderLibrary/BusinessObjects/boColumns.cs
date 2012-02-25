using System;
using System.Collections.Generic;
using System.Text;
using CodeBuilderLibrary.DataObjects;

namespace CodeBuilderLibrary.BusinessObjects
{
    public class boColumns
    {
        public static doColumns.uoListColumns GetColumns(string connectionString, string tableName)
        {
            string sql_column = string.Format("select column_name,column_default,is_nullable,data_type,character_maximum_length as max_length from INFORMATION_SCHEMA.COLUMNS where table_name='{0}'", tableName);

            using (System.Data.IDbConnection conn = DataAccess.ConnectionHelper.CreateConnection(connectionString))
            {
                return DataMapping.ObjectHelper.FillCollection<doColumns.uoColumns, doColumns.uoListColumns>(DataAccess.SqlUtil.ExecuteReader(conn, sql_column));
            }
        }
    }
}
