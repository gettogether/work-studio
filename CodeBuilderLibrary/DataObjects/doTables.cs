using System;
using System.Collections.Generic;
using System.Text;
using DataMapping;

namespace CodeBuilderLibrary.DataObjects
{
    public class doTables : DataAccess.Data.DOBase<doTables.uoTables, doTables.uoListTables>
    {
        public doTables()
        {

        }
        public class uoTables : DataAccess.Data.UOBase<uoTables, uoListTables>
        {
            public uoTables()
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
        public class uoListTables : CommonLibrary.ObjectBase.ListBase<uoTables>
        {
            public uoListTables()
            {

            }
        }
    }
}
