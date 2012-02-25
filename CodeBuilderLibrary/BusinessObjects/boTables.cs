using System;
using System.Collections.Generic;
using System.Text;
using CodeBuilderLibrary.DataObjects;

namespace CodeBuilderLibrary.BusinessObjects
{
    public class boTables
    {
        public static doTables.uoListTables GetTables(string connectionString)
        {
            using (System.Data.IDbConnection conn = DataAccess.ConnectionHelper.CreateConnection(connectionString))
            {
                return DataMapping.ObjectHelper.FillCollection<doTables.uoTables, doTables.uoListTables>
                    (DataAccess.SqlUtil.ExecuteReader(conn,
                    "SELECT name FROM sysobjects WHERE type = 'U' or type='V' order by name"));
            }
        }
    }
}
