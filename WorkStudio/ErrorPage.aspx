<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error</title>
    <link href="<%=ResolveUrl("~") %>css/B2C/blue.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <table width="100%" border="0" style="text-align: center;">
        <tr>
            <td align="center">
                <div style="width: 600px; border: solid 1px #000; padding: 15px; margin-top: 20px;
                    background-color: #FFFFFF;">
                    <div>
                        <table border="0px" cellpadding="0px" cellspacing="0px" width="100%">
                            <tr>
                                <td valign="top" style="width: 80px;" align="left">
                                    <img alt="" src="images/erroricon.jpg" />
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td align="left" style="width: 99%;" valign="top">
                                    <span style="font-weight: bold; font-size: larger;">Error occur</span>
                                    <br />
                                    <br />
                                    <input id="Text1" type="text" value="" readonly="readonly" style="width: 100%; border-width: 0px" />
                                    <hr noshade="noshade" size="0" />
                                    <textarea id="TextArea1" cols="20" rows="15" style="overflow: auto; font-size: 12px;
                                        font-family: Verdana; border-width: 0px; width: 100%" readonly="readonly">Please try again later, Please contact our Technical Support when error is occur again</textarea>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <hr noshade="noshade" size="0" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td align="right">
                                    <%if (Request["aspxerrorpath"].ToUpper().IndexOf("/B2C") > 0)
                                  { %>
                                    <input id="Button1" class="btn4" type="button" onclick="history.back(2);" value="Back" />
                                    <%}
                                      else
                                      { %>
                                      <input id="Button2" class="btn4" type="button" onclick="window.location='login.aspx';" value="Back" />
                                    <%} %>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</body>
</html>
