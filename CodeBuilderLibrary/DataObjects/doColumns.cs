using System;
using System.Collections.Generic;
using System.Text;
using DataMapping;

namespace CodeBuilderLibrary.DataObjects
{
    public class doColumns : DataAccess.Data.DOBase<doColumns.uoColumns, doColumns.uoListColumns>
    {
        public doColumns()
        {

        }
        public doColumns(string connectionKey)
        {

        }
        public class uoColumns : DataAccess.Data.UOBase<uoColumns, uoListColumns>
        {
            public uoColumns()
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
                    else if (_column_name.Equals("class"))
                    {
                        _column_name = "Class";
                    }
                }
            }
            private string _column_default;
            [Mapping("column_default")]
            public string column_default
            {
                get { return _column_default; }
                set { _column_default = value; }
            }
            private string _is_nullable;
            [Mapping("is_nullable")]
            public string is_nullable
            {
                get { return _is_nullable; }
                set { _is_nullable = value; }
            }
            private string _data_type;
            [Mapping("data_type")]
            public string data_type
            {
                get { return _data_type; }
                set { _data_type = value; }
            }
            private string _max_length;
            [Mapping("maxmax_length")]
            public string max_length
            {
                get { return _max_length; }
                set { _max_length = value; }
            }
        }
        public class uoListColumns : CommonLibrary.ObjectBase.ListBase<uoColumns>
        {
            public uoListColumns()
            {

            }
        }
    }
}
