using System;
using System.Collections.Generic;
using System.Text;
using DataMapping;

namespace CodeBuilderLibrary.DataObjects
{
    public class doPKs : DataAccess.Data.DOBase<doPKs.uoPKs, doPKs.uoListPKs>
    {
        public doPKs()
        {

        }
        public enum Columns
        {
            name
        }
        public class uoPKs : DataAccess.Data.UOBase<uoPKs, uoListPKs>
        {
            public uoPKs()
            {

            }            
            private string _name;
            [Mapping("name")]
            public string name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    if (_name.Equals("ref"))
                    {
                        _name = "Ref";
                    }
                }
            }
        }
        public class uoListPKs : CommonLibrary.ObjectBase.ListBase<uoPKs>
        {
            public uoListPKs()
            {

            }
        }
    }
}
