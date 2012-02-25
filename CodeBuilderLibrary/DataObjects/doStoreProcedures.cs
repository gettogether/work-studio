using System;
using System.Collections.Generic;
using System.Text;
using DataMapping;

namespace CodeBuilderLibrary.DataObjects
{
    public class doStoreProcedures : DataAccess.Data.DOBase<doStoreProcedures.uoStoreProcedures, doStoreProcedures.uoListStoreProcedures>
    {
        public doStoreProcedures()
        {

        }
        public class uoStoreProcedures : DataAccess.Data.UOBase<uoStoreProcedures, uoListStoreProcedures>
        {
            public uoStoreProcedures()
            {

            }
            private string _name;
            [Mapping("name")]
            public string name
            {
                get { return _name; }
                set { _name = value; }
            }
        }
        public class uoListStoreProcedures : CommonLibrary.ObjectBase.ListBase<uoStoreProcedures>
        {
            public uoListStoreProcedures()
            {

            }
        }
    }
}
