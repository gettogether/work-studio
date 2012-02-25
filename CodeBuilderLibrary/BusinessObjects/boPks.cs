using System;
using System.Collections.Generic;
using System.Text;
using CodeBuilderLibrary.DataObjects;

namespace CodeBuilderLibrary.BusinessObjects
{
    public class boPks
    {
        public static doPKs.uoListPKs GetPrimaryKeys(string connectionString, string tableName)
        {
            string sql_pk = string.Format("select syscolumns.name from syscolumns,sysobjects,sysindexes,sysindexkeys where syscolumns.id = object_id('{0}') and sysobjects.xtype = 'pk' and sysobjects.parent_obj = syscolumns.id and sysindexes.id = syscolumns.id and sysobjects.name = sysindexes.name and sysindexkeys.id = syscolumns.id and sysindexkeys.indid = sysindexes.indid and syscolumns.colid = sysindexkeys.colid", tableName);
            using (System.Data.IDbConnection conn = DataAccess.ConnectionHelper.CreateConnection(connectionString))
            {
                return DataMapping.ObjectHelper.FillCollection<doPKs.uoPKs, doPKs.uoListPKs>
                    (DataAccess.SqlUtil.ExecuteReader(conn, sql_pk));
            }
        }
    }
}
