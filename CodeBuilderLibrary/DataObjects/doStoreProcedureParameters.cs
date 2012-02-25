using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Data;
using DataMapping;

namespace CodeBuilderLibrary.DataObjects
{
    public class doStoreProcedureParameters : DOBase<doStoreProcedureParameters.uoStoreProcedureParameters, doStoreProcedureParameters.uoListStoreProcedureParameters>
    {
        public enum Columns
        {
            name,
            data_type,
            length,
            isnullable,
            isoutparam,
        }
        public doStoreProcedureParameters()
        {
            ConnInfo = new ConnectionInformation("select sc.name,st.name data_type,st.length,sc.isnullable,isoutparam from systypes st right join syscolumns sc on st.xtype=sc.xtype right join sysobjects so on sc.id=so.id where so.xtype='p' and category=0 and so.name='sp_paging' and st.name<>'sysname'", "Adholidays", new string[] { "name" });
            ConnInfo.IsSqlSentence = true;
        }
        public class uoStoreProcedureParameters : UOBase<uoStoreProcedureParameters, uoListStoreProcedureParameters>
        {
            #region Columns
            private System.String _name;
            [Mapping("name,un-insert,un-update")]
            public System.String name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                }
            }
            private System.String _data_type;
            [Mapping("data_type")]
            public System.String data_type
            {
                get
                {
                    return _data_type;
                }
                set
                {
                    _data_type = value;
                }
            }
            private System.Int16 _length;
            [Mapping("length")]
            public System.Int16 length
            {
                get
                {
                    return _length;
                }
                set
                {
                    _length = value;
                }
            }
            private System.Int32 _isnullable;
            [Mapping("isnullable")]
            public System.Int32 isnullable
            {
                get
                {
                    return _isnullable;
                }
                set
                {
                    _isnullable = value;
                }
            }
            private System.Int32 _isoutparam;
            [Mapping("isoutparam")]
            public System.Int32 isoutparam
            {
                get
                {
                    return _isoutparam;
                }
                set
                {
                    _isoutparam = value;
                }
            }
            #endregion

            public uoStoreProcedureParameters()
            {
                ConnInfo = new doStoreProcedureParameters().ConnInfo;
            }
        }
        public class uoListStoreProcedureParameters : CommonLibrary.ObjectBase.ListBase<uoStoreProcedureParameters>
        {
            public uoListStoreProcedureParameters()
            {
            }
        }
    }
}
