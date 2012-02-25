<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Components_AT_ATHeader" %>
<div id="header" class="noprint">
<div id="menu">
<div id="menu_left">
<%if (sess != null)
{%>
<p style="width: 5px;">
&nbsp;</p>
<p style="width: 15px;" class="imgUser">
&nbsp;</p>
<p>
<b></b>&nbsp;<<%=sess.Lastname%>&nbsp;<%=sess.Firstname %> >
</p>
<p style="width: 80px;">
&nbsp;</p>
<p class="imgClock">
&nbsp;
</p>
<p>
<%=CommonLibrary.Utility.DateHelper.FormatDateTimeToString((DateTime)Session["login_time"], CommonLibrary.Utility.DateHelper.DateFormat.ddMMMyyyyml) %>
</p>
<p style="width: 80px;">
&nbsp;</p>

<%}%>
<%else
{%>
<p style="width: 10px;">
&nbsp;</p>
<%} %>
</div>
<div id="menu_right">
<ul id="navCircle">
<%=WorkStudioLibrary.Web.HtmlHelper.GenMenus(this.Page)%>
</ul>
</div>
<div class="menu_border">
&nbsp;
</div>
</div>
</div>