using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Components_Downloads_DownloadFiles : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        rptResult.DataSource = GetDownloads();
        rptResult.DataBind();
    }

    public static Downloads GetDownloads()
    {
        Downloads ds = new Downloads();
        foreach (string s in System.IO.Directory.GetFiles(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Include\\Downloads\\")))
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(s);
            Downloads.Download d = new Downloads.Download();
            d.FileName = fi.Name;
            d.FileType = fi.Extension;
            d.CreateOn = fi.CreationTime;
            d.Length = fi.Length;
            d.LengthDesc = "Byte";
            if (d.Length > 1024)
            {
                d.LengthDesc = "K";
                d.Length = (long)(d.Length / 1024);
            }
            if (d.Length > 1024)
            {
                d.LengthDesc = "M";
                d.Length = (long)(d.Length / 1024);
            }
            if (d.Length > 1024)
            {
                d.LengthDesc = "G";
                d.Length = (long)(d.Length / 1024);
            }
            ds.Add(d);
        }
        return ds;
    }
}

public class Downloads : System.Collections.Generic.List<Downloads.Download>
{
    public Downloads()
    {

    }
    public class Download
    {
        private string _FileName;

        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        private string _FileType;

        public string FileType
        {
            get { return _FileType; }
            set { _FileType = value; }
        }

        private DateTime _CreateOn;

        public DateTime CreateOn
        {
            get { return _CreateOn; }
            set { _CreateOn = value; }
        }

        private long _Length;

        public long Length
        {
            get { return _Length; }
            set { _Length = value; }
        }

        private string _LgnethDesc;

        public string LengthDesc
        {
            get { return _LgnethDesc; }
            set { _LgnethDesc = value; }
        }


    }
}