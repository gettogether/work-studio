using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DataAccess;
using DataAccess.Data;
using DataMapping;
using CodeBuilderLibrary.DataObjects;

namespace CodeBuilderLibrary.BusinessObjects
{
    public class boStoreProcedureParameters
    {
        public static doStoreProcedureParameters.uoListStoreProcedureParameters GetStoreProcedureParameters(string connectionString, string spName)
        {
            using (System.Data.IDbConnection conn = DataAccess.ConnectionHelper.CreateConnection(connectionString))
            {
                string sql_spp = string.Format("select sc.name,st.name data_type,st.length,sc.isnullable,isoutparam from systypes st right join syscolumns sc on st.xtype=sc.xtype right join sysobjects so on sc.id=so.id where so.xtype='p' and category=0 and so.name='{0}' and st.name<>'sysname'", spName);
                return DataMapping.ObjectHelper.FillCollection<doStoreProcedureParameters.uoStoreProcedureParameters, doStoreProcedureParameters.uoListStoreProcedureParameters>
                    (DataAccess.SqlUtil.ExecuteReader(conn,
                    sql_spp));
            }
        }
    }
}
