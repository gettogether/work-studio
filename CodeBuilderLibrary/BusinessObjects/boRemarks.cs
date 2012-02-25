using System;
using System.Collections.Generic;
using System.Text;
using CodeBuilderLibrary.DataObjects;

namespace CodeBuilderLibrary.BusinessObjects
{
    public class boRemarks
    {
        public static doRemarks.uoListRemarks GetRemarks(string connectionString, string tableName)
        {
            string sql_rm = string.Empty;
            sql_rm = "select c.name column_name,p.value remark from sysproperties p join sysobjects o on p.id=o.id join syscolumns c on o.id=c.id and c.colid=p.smallid where p.name='MS_Description' and o.name='{0}'";
            sql_rm = string.Format(sql_rm, tableName);
            using (System.Data.IDbConnection conn = DataAccess.ConnectionHelper.CreateConnection(connectionString))
            {
                try
                {
                    return DataMapping.ObjectHelper.FillCollection<doRemarks.uoRemarks, doRemarks.uoListRemarks>
                        (DataAccess.SqlUtil.ExecuteReader(conn, sql_rm));
                }
                catch
                {
                    sql_rm = "select c.name column_name,p.value remark from sys.extended_properties p join sysobjects o on p.major_id=o.id join syscolumns c on o.id=c.id and c.colid=p.minor_id where p.name='MS_Description' and o.name='{0}'";
                    sql_rm = string.Format(sql_rm, tableName);
                    return DataMapping.ObjectHelper.FillCollection<doRemarks.uoRemarks, doRemarks.uoListRemarks>
                        (DataAccess.SqlUtil.ExecuteReader(conn,
                        sql_rm));
                }
            }
        }
    }
}
