using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Components_AT_CPGuide : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private int _step = 1;
    public int Step
    {
        get { return _step; }
        set { _step = value; }
    }
    public string GetTitle()
    {
        switch (Step)
        {
            case 1:
                return "Search";
            case 2:
                return "Select";
            case 3:
                return "Make a Request";
            case 4:
                return "Acknowledgement";
            default:
                return string.Empty;
        }
    }
    public string GetStepImgCSS(int value)
    {
        if (Step == value)
            return "imgC";
        else if (Step < value)
            return "imgA";
        else
            return "imgB";
    }
    public string GetStepBgCSS(int value)
    {
        if (Step == value)
            return "p_C";
        else if (Step < value)
            return "p_B";
        else
            return "p_A";
    }
}
