<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Guide.ascx.cs" Inherits="Components_AT_CPGuide" %>
<div id="pkgbook">
    <%--<div>
        <ul>
            <li class="imgPkg info_no">
                <%=Step.ToString() %></li>
            <li>&nbsp;</li>
            <li class="info_title">
                <%=GetTitle()%></li>
        </ul>
    </div>--%>
    <div class="divborderpkg" style="width: 772px;">
        &nbsp;
    </div>
    <div class="info_content">
        <ul>
            <li class="<%=GetStepBgCSS(1) %>"><span class="<%=GetStepImgCSS(1) %>">1</span><span><%=GetLocalResourceObject("Search") %></span></li>
            <li class="<%=GetStepBgCSS(2) %>"><span class="<%=GetStepImgCSS(2) %>">2</span><span><%=GetLocalResourceObject("Select") %></span></li>
            <li class="<%=GetStepBgCSS(3) %>"><span class="<%=GetStepImgCSS(3) %>">3</span><span><%=GetLocalResourceObject("Request") %></span></li>
            <li class="<%=GetStepBgCSS(4) %>"><span class="<%=GetStepImgCSS(4) %>">4</span><span><%=GetLocalResourceObject("Acknowledgement")%></span></li>
        </ul>
    </div>
</div>
