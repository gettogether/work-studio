<%@ Page Language="C#" MasterPageFile="~/MP.master" AutoEventWireup="true" CodeFile="Index.aspx.cs"
    Inherits="MSSQL_Index" Title="Untitled Page" %>

<%@ Register Src="../Components/MSSQL/Templates.ascx" TagName="Templates"
    TagPrefix="uc1" %>
<%@ Register Src="~/Components/WS/TravelToolsBox.ascx" TagName="TravelToolsBox" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MP1" runat="Server">
    <div id="left">
        <uc2:TravelToolsBox ID="TravelToolsBox1" runat="server" />
    </div>
    <div id="content">
        <div id="dv-templates" class="divborderpad" style="text-align: left; min-height: 50px;padding:">
        </div>
        <div style="text-align: right;">
            <input type="button" class="btn6" value="Create New Project" onclick="AjaxMsg('Create a new template','750','560',SerUrl,'Callback/MSSQL/Index.aspx?type=1&g=1','content',null);" /></div>
    </div>

    <script language="javascript">
 //function CommonCall(Url,type,divId,divBgId,divParId,inputPrefix,parAppend,dType,jsFun)
 //function AjaxMsg(title,width,height,serUrl,page,sId,func)
 $(document).ready(function(){
LoadTemplates();
 });
 function LoadTemplates(){ CommonCall('Callback/MSSQL/Index.aspx',0,'dv-templates','dv-templates','dv-templates','TP_','','text',null);}
 function CreateTemplate(txtId)
 {
    TemplateExec(txtId,1);
 }
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
        url:SerUrl+'Callback/MSSQL/Index.aspx?type='+type+'&c='+oa(txtId),
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
