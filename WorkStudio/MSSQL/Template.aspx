<%@ Page Language="C#" MasterPageFile="~/MP.master" AutoEventWireup="true" CodeFile="Template.aspx.cs"
    Inherits="MSSQL_MS_ProjectList" Title="Untitled Page" %>

<%@ Register Src="../Components/MSSQL/Templates.ascx" TagName="Templates"
    TagPrefix="uc1" %>
<%@ Register Src="~/Components/WS/TravelToolsBox.ascx" TagName="TravelToolsBox" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MP1" runat="Server">
    <div id="dv-content" style="margin: 0px 0px 0px 0px; width: 100%; min-height: 600px; float: left;">
        <div id="dv-templates" class="divborderpad" style="text-align: left; min-height: 50px;">
        </div>
        <div style="text-align: right;">
            <input type="button" class="btn6" value="Create A New Project" onclick="AjaxMsg('Create a new template','750','560',SerUrl,'Callback/MSSQL/Template.aspx?type=1&get=1','dv-content',null);" /></div>
    </div>

    <script language="javascript">
 //function CommonCall(Url,type,divId,divBgId,divParId,inputPrefix,parAppend,dType,jsFun)
 //function AjaxMsg(title,width,height,serUrl,page,sId,func)
 $(document).ready(function(){
LoadTemplates();
 });
 function LoadTemplates(){ CommonCall('Callback/MSSQL/Template.aspx',0,'dv-templates','dv-templates','dv-templates','TP_','','text',null);}
  function TemplateExec(txtId,type)
 {
    var errLabel=o('dv-error');
    errLabel.innerHTML='';
    if(IsEmpty(oa(txtId)))
    {
        errLabel.innerHTML='Please input the template.';
    }
    else
    {
        $.ajax({
        url:SerUrl+'Callback/MSSQL/Template.aspx?type='+type+'&content='+oa(txtId),
        type:'post',
        cache:false,
        dataType:'json',
        data:'',
        error: function() {},
        success:function(data, textStatus) 
        {
            if(!data.success){errLabel.innerHTML=data.message;}
            else
            {
                CloseMsgBox();
                LoadTemplates();
            }
        }   
        });
    }
 }
    </script>

</asp:Content>