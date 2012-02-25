using System;
using System.Collections.Generic;
using System.Text;
using DataMapping;

namespace CodeBuilderLibrary.DataObjects
{
    public class doRemarks : DataAccess.Data.DOBase<doRemarks.uoRemarks, doRemarks.uoListRemarks>
    {
        public doRemarks()
        {

        }
        public doRemarks(string connectionKey)
        {

        }
        public class uoRemarks : DataAccess.Data.UOBase<uoRemarks, uoListRemarks>
        {
            public uoRemarks()
            {

            }
            private string _column_name;
            [Mapping("column_name")]
            public string column_name
            {
                get { return _column_name; }
                set
                {
                    _column_name = value;
                    if (_column_name.Equals("ref"))
                    {
                        _column_name = "Ref";
                    }
                }
            }
            private string _remark;
            [Mapping("remark")]
            public string remark
            {
                get { return _remark; }
                set { _remark = value; }
            }
        }
        public class uoListRemarks : CommonLibrary.ObjectBase.ListBase<uoRemarks>
        {
            public uoListRemarks()
            {

            }
        }
    }
}
