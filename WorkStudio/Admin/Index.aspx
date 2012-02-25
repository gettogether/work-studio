<%@ Page Language="C#" MasterPageFile="~/MP.master" AutoEventWireup="true" CodeFile="Index.aspx.cs"
    Inherits="Admin_Index" Title="Untitled Page" %>

<%@ Register Src="~/Components/WS/TravelToolsBox.ascx" TagName="TravelToolsBox" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MP1" runat="Server">
    <div id="left">
        <uc2:TravelToolsBox ID="TravelToolsBox1" runat="server" />
    </div>
    <div id="content">
        <div style="background-color: #c9e5f3; font-weight: bold; font-size: 11pt;">
            My Account</div>
        <div class="divborderpad" style="min-height: 300px;">
        </div>
    </div>
</asp:Content>
