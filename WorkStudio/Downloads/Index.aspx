<%@ Page Language="C#" MasterPageFile="~/MP.master" AutoEventWireup="true" CodeFile="Index.aspx.cs"
    Inherits="Downloads_Index" Title="Untitled Page" %>

<%@ Register Src="../Components/Downloads/DownloadFiles.ascx" TagName="DownloadFiles"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MP1" runat="Server">
    <div style="float: left;width:100%;">
        <uc1:DownloadFiles ID="DownloadFiles1" runat="server" />
    </div>
</asp:Content>
