using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBuilderLibrary
{
    public class ColumnMapping
    {
        #region Attributes
        public class ColumnInfos : List<ColumnInfo>
        {
            public ColumnInfos()
            {

            }
        }
        public class ColumnInfo
        {
            private string _Column;

            public string Column
            {
                get { return _Column; }
                set { _Column = value; }
            }
            private string _ColumnType;

            public string ColumnType
            {
                get { return _ColumnType; }
                set { _ColumnType = value; }
            }

        }
        #endregion
        public static ColumnInfos GetColumnInfo(System.Data.IDataReader dr)
        {
            ColumnInfos ret = new ColumnInfos();
            string m = string.Empty;
            for (int i = 0; i < dr.FieldCount; i++)
            {
                ColumnInfo ci = new ColumnInfo();
                ci.Column = dr.GetName(i);
                ci.ColumnType = dr.GetFieldType(i).ToString();
                ret.Add(ci);
            }
            return ret;
        }
    }
}
