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

public partial class Testing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write(StringDifferenceIndex("1234561789", "12345678911").ToString());

    }

    public static int StringDifferenceIndex(string a, string b)
    {
        int len_a = a.Length;
        int len_b = b.Length;
        int len = len_a < len_b ? len_a : len_b;
        for (int i = 0; i < len; i++)
        {
            if (a[i] != b[i])
            {
                return i;
            }
        }
        return len;
    }
}



