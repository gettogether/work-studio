using System;
using System.Collections.Generic;
using System.Text;
using CodeBuilderLibrary.DataObjects;

namespace CodeBuilderLibrary.BusinessObjects
{
    public class boStoreProcedures
    {
        public static doStoreProcedures.uoListStoreProcedures GetStoreProcedures(string connectionString)
        {
            using (System.Data.IDbConnection conn = DataAccess.ConnectionHelper.CreateConnection(connectionString))
            {
                string sql_sp = "select name from sysobjects where xtype='p' and category=0 order by name";
                return DataMapping.ObjectHelper.FillCollection<doStoreProcedures.uoStoreProcedures, doStoreProcedures.uoListStoreProcedures>
                    (DataAccess.SqlUtil.ExecuteReader(conn,
                    sql_sp));
            }
        }
    }
}
