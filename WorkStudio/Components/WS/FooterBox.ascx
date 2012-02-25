<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FooterBox.ascx.cs" Inherits="Components_AT_FooterBox" %>
<table width="100%" cellspacing="0" cellpadding="0" border="0">
    <tbody>
        <tr>
            <td width="20%">
            </td>
            <td align="center" width="60%">
                Copyright © Westminster Management System All rights reserved</td>
            <td align="right" width="20%">
                <%=GetLocalResourceObject("Version") %>
                <%=WorkStudioLibrary.Config.Version%>
            </td>
        </tr>
    </tbody>
</table>
