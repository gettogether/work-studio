<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DownloadFiles.ascx.cs"
    Inherits="Components_Downloads_DownloadFiles" %>
<table style="width: 100%; border: 1px solid #336699;" class="tb-header" cellpadding="1px"
    cellspacing="1px">
    <tr style="background-color: #336699; color: White; font-weight: bold;">
        <td style="white-space: nowrap;">
            File Name</td>
        <td>
            Create On</td>
        <td>
            File Type</td>
        <td>
            Size</td>
        <td>
        </td>
    </tr>
    <asp:Repeater ID="rptResult" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <b>
                        <%#Eval("FileName") %>
                    </b>
                </td>
                <td>
                    <%#Eval("CreateOn") %>
                </td>
                <td>
                    <%#Eval("FileType") %>
                </td>
                <td>
                    <%#Eval("Length") %>
                    <%#Eval("LengthDesc") %>
                </td>
                <td>
                    <a href="../Include/Downloads/<%#Eval("FileName") %>">Download</a>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <div class="line1">
                    </div>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
