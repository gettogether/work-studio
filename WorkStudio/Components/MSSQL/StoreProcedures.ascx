<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StoreProcedures.ascx.cs"
    Inherits="Components_MSSQL_StoreProcedures" %>
<div style="padding: 0 5px; text-align: left; background-color: #336699; color: White;">
    Store Procedure(Click to generate)</div>
<div style="border: solid 1px #336699;" style="width: 188px;">
    <%if (string.IsNullOrEmpty(Error))
  { %>
    <div style="margin: 2px; padding: 0 5px; text-align: left; overflow: scroll; max-height: 600px;">
        <table width="100%" cellspacing="0" cellpadding="2" border="0" style="background-color: #FFFFFF">
            <tbody>
                <tr>
                    <td valign="top">
                        <table width="100%" cellspacing="0" cellpadding="1" border="0">
                            <asp:Repeater ID="rptResult" runat="server">
                                <ItemTemplate>
                                    <tr valign="top" id="tr1">
                                        <td>
                                            <a href="#" onclick="GenStoreProcSimple('<%#Eval("name") %>');">
                                                <%#Eval("name") %>
                                            </a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="line2">
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <%}
            else
            { %>
    <%=Error %>
    <%} %>
</div>
