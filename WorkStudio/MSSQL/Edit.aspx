<%@ Page Language="C#" MasterPageFile="~/MP.master" AutoEventWireup="true" CodeFile="Edit.aspx.cs"
    Inherits="MS_MSSQL" Title="Untitled Page" %>

<%@ Register Src="~/Components/WS/TravelToolsBox.ascx" TagName="TravelToolsBox" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MP1" runat="Server">
    <%=SplitHeader %>
    <div id="navcontainer">
        <ul id="navlist">
            <li id="active" onclick="ft(this);" class="table-objects"><a href="#">From Table</a></li>
            <li onclick="ftQR(this);" class="query-objects"><a href="#">From SQL Script</a></li>
            <li onclick="ftSP(this);" class="store-procedure-objects"><a href="#">From Store Procedure</a></li>
        </ul>
    </div>
    <div id="edit-content">
        <div id="dv-table-objects" class="tab">
            <div style="float: left; width: 190px;">
                <div id="dv-tables" style="min-height: 50px; _height: 50px;">
                </div>
            </div>
            <div style="margin: 0px 0px 0px 5px; width: 779px; min-height: 600px; float: right;">
                <div style="background-color: #336699; font-size: 8pt;padding-left:2px;color:White;">
                    Data Object&nbsp;<span style="font-size: 8pt;"></span></div>
                <div style="border: solid 1px #336699; " id="dv-table-do">
<%--                    <div id="dv-table-do-display" ondblclick="this.style.display='none';o('dv-table-do-value').style.display='';o('txt-table-do-value').select();"
                        style="min-height: 300px; height: 300px; text-align: left; overflow-y: scroll;
                        width: 100%;">
                    </div>--%>
                    <div id="dv-table-do-value" 
                        style=" background-color: #ffffff;">
                        <textarea rows="15" id="txt-table-do-value" style="width: 99%; border-width: 0px;"><%=DO_String %></textarea></div>
                </div>
                <div style="background-color: #336699; font-size: 8pt; padding-left: 2px; margin: 5px 0px 0px 0px;color:White;">
                    Business Object&nbsp;<span style="font-size: 8pt;"></span></div>
                <div style="border: solid 1px #336699;" id="dv-table-bo">
<%--                    <div id="dv-table-bo-display" ondblclick="this.style.display='none';o('dv-table-bo-value').style.display='';o('txt-table-bo-value').select();"
                        style="min-height: 300px; height: 300px; text-align: left; overflow-y: scroll;
                        width: 100%;">
                    </div>--%>
                    <div id="dv-table-bo-value">
                        <textarea rows="15" id="txt-table-bo-value" style="width: 99%; border-width: 0px;"><%=DO_String %></textarea></div>
                </div>
            </div>
        </div>
        <div id="dv-query-objects" style="display: none;" class="tab">
            <div style="margin: 0px 0px 0px 0px; width: 100%; min-height: 600px; float: left;">
                <div style="background-color: #336699; font-size: 8pt; color: White;padding-left:2px;color:White;">
                    Query&nbsp;<span style="font-size: 8pt;">(Enter script to generate objects)</span></div>
                <div style="border: solid 1px #336699;" id="dv-query-parameter">
                    <table style="width: 100%;" cellpadding="1px" cellspacing="1px">
                        <tr>
                            <td>
                                Object Name</td>
                            <td>
                                <input type="text" id="QR_Name" required="1" class="txt" style="width: 200px;" value="SysObjectsFields" /></td>
                            <td>
                                Primary Key(s)</td>
                            <td>
                                <input type="text" id="QR_PrimaryKeys" required="1" class="txt" style="width: 380px;"
                                    value="column_name" /></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Query SQL</td>
                            <td colspan="3">
                                <textarea id="QR_Sql" cols="10" rows="10" style="height: 100px; width: 99%;" class="txt">select column_name,column_default,is_nullable,data_type,character_maximum_length as max_length from INFORMATION_SCHEMA.COLUMNS where table_name='sysobjects'</textarea>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right">
                                <input type="button" class="btn5" onclick="GenCodeByQuery();" value="Generate" /></td>
                        </tr>
                    </table>
                </div>
                <div style="background-color: #336699; font-size: 8pt;margin: 5px 0px 0px 0px;padding-left:2px;color:White;">
                    Data Object&nbsp;<span style="font-size: 8pt;"></span></div>
                <div style="border: solid 1px #336699; " id="dv-query-do">
<%--                    <div id="dv-query-do-display" ondblclick="this.style.display='none';o('dv-query-do-value').style.display='';o('txt-query-do-value').select();"
                        style="min-height: 250px; height: 250px; text-align: left; overflow-y: scroll;
                        width: 100%;">
                    </div>--%>
                    <div id="dv-query-do-value">
                        <textarea rows="15" id="txt-query-do-value" style="width: 99%; border-width: 0px;"><%=DO_String %></textarea></div>
                </div>
                <div style="background-color: #336699; font-size: 8pt; padding-left: 2px; margin: 5px 0px 0px 0px;color:White;">
                    Business Object&nbsp;<span style="font-size: 8pt;"></span></div>
                <div style="border: solid 1px #336699;" id="dv-query-bo">
<%--                    <div id="dv-query-bo-display" ondblclick="this.style.display='none';o('dv-query-bo-value').style.display='';o('txt-query-bo-value').select();"
                        style="min-height: 250px; height: 250px; text-align: left; overflow-y: scroll;
                        width: 100%;">
                    </div>--%>
                    <div id="dv-query-bo-value">
                        <textarea rows="15" id="txt-query-bo-value" style="width: 99%; border-width: 0px;"><%=DO_String %></textarea></div>
                </div>
            </div>
        </div>
        <div id="dv-store-procedure-objects" style="display: none;" class="tab">
            <div style="float: left; width: 190px;">
                <div id="dv-store-procedures" style="min-height: 50px; _height: 50px;"></div>
            </div>
            <div style="margin: 0px 0px 0px 5px; width: 779px; min-height: 600px; float: right;">
                <div id="dv-store-procedure-parameters">
                    <div style="background-color: #336699; font-size: 8pt; padding-left: 2px; color: White;">
                        Store Procedure</div>
                    <div style="border: solid 1px #336699; margin-right: 0px; background-color: #ffffff;">
                        <table style="width: 100%;" cellpadding="1px" cellspacing="1px">
                            <tr>
                                <td style="width: 100px;">
                                    Object Name</td>
                                <td>
                                    <input type="text" id="SP_ObjectName" required="1" class="txt" style="width: 200px;"
                                        value="ShowProcess" /></td>
                                <td>
                                    Name</td>
                                <td>
                                    <input type="text" id="SP_StoreProcedureName" class="txt" style="width: 200px;" value="sp_who" /></td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    Store Procedure</td>
                                <td colspan="4">
                                    <div id="dv-store-procedure-simple">
                                        <textarea rows='10' cols="10" id='SP_Sql' style='width: 99%; font-size: 8pt; height: 100px;'
                                            class="txt">sp_who</textarea>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="right">
                                    <input type="button" class="btn5" onclick="GenCodeByStoreProcedure();" value="Generate" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div style="background-color: #336699; font-size: 8pt; padding-left: 2px;margin-top:5px;color:White;">
                    Data Object&nbsp;<span style="font-size: 8pt;"></span></div>
                <div style="border: solid 1px #336699; " id="dv-store-procedure-do">
<%--                    <div id="dv-store-procedure-do-display" ondblclick="this.style.display='none';o('dv-store-procedure-do-value').style.display='';o('txt-store-procedure-do-value').select();"
                        style="min-height: 200px; height: 200px; text-align: left; overflow-y: scroll;
                        width: 100%;">
                    </div>--%>
                    <div id="dv-store-procedure-do-value">
                        <textarea rows="15" id="txt-store-procedure-do-value" style="width: 99%; border-width: 0px;"><%=DO_String %></textarea></div>
                </div>
                <div style="background-color: #336699; font-size: 8pt; padding-left: 2px; margin: 5px 0px 0px 0px;color:White;">
                    Business Object&nbsp;<span style="font-size: 8pt;"></span></div>
                <div style="border: solid 1px #336699;" id="dv-store-procedure-bo">
<%--                    <div id="dv-store-procedure-bo-display" ondblclick="this.style.display='none';o('dv-store-procedure-bo-value').style.display='';o('txt-store-procedure-bo-value').select();"
                        style="min-height: 200px; height: 200px; text-align: left; overflow-y: scroll;
                        width: 100%;">
                    </div>--%>
                    <div id="dv-store-procedure-bo-value">
                        <textarea rows="15" id="txt-store-procedure-bo-value" style="width: 99%; border-width: 0px;"><%=DO_String %></textarea></div>
                </div>
            </div>
        </div>
    </div>

    <script language="javascript">
var ProjectName='<%=Request["pn"] %>';
$(document).ready(function()
{
    CommonCall('Callback/MSSQL/Edit.aspx',0,'dv-tables','dv-tables','dv-tables','EP_','&pn='+ProjectName,'text',"GenDoTableCode('sysobjects');");
});
function GenTableCode(table)
 {
    InitView('table');
    GenDoTableCode(table);
}
function InitView(t)
{
    //o('dv-'+t+'-do-display').style.display='';
    //o('dv-'+t+'-do-value').style.display='none';
    //o('dv-'+t+'-bo-display').style.display='';
    //o('dv-'+t+'-bo-value').style.display='none';
}
function GenDoTableCode(table)
{
     //CommonCall('Callback/MSSQL/Edit.aspx',1,'dv-table-do-display','dv-table-do','dv-table-do','EP_','&pn='+ProjectName+'&tn='+table+'&hl=1','text',"GenBoTableCode('"+table+"')");
     CommonCall('Callback/MSSQL/Edit.aspx',1,'dv-table-do-value','dv-table-do','dv-table-do','EP_','&pn='+ProjectName+'&tn='+table,'text',"GenBoTableCode('"+table+"')");
}
function GenBoTableCode(table)
{
     //CommonCall('Callback/MSSQL/Edit.aspx',2,'dv-table-bo-display','dv-table-bo','dv-table-bo','EP_','&pn='+ProjectName+'&tn='+table+'&hl=1','text',null);
     CommonCall('Callback/MSSQL/Edit.aspx',2,'dv-table-bo-value','dv-table-bo','dv-table-bo','EP_','&pn='+ProjectName+'&tn='+table,'text',null);
}
function ftQR(obj)
{
    ft(obj);
    if(oa('txt-query-do-value')=='')GenCodeByQuery();
}
function GenCodeByQuery()
{
    if(!ValidateTxt('dv-query-parameter') || oa('QR_Sql')==''){MsgBox('Invalid Parameters','System Information',true,'dv-query-parameter');return false;}
    InitView('query');
    GenDoQueryCode();
}
function GenDoQueryCode()
{
    var parameters='&tn='+oa('QR_Name')+'&sql='+oa('QR_Sql')+'&pk='+oa('QR_PrimaryKeys');
    //CommonCall('Callback/MSSQL/Edit.aspx',3,'dv-query-do-display','dv-query-do','dv-query-do','QR_','&hl=1&pn='+ProjectName+parameters,'text','GenBoQueryCode();');
    CommonCall('Callback/MSSQL/Edit.aspx',3,'dv-query-do-value','dv-query-do','dv-query-do','QR_','&pn='+ProjectName+parameters,'text','GenBoQueryCode();');
}
function GenBoQueryCode()
{
    var parameters='&tn='+oa('QR_Name')+'&sql='+oa('QR_Sql')+'&pk='+oa('QR_PrimaryKeys');
    //CommonCall('Callback/MSSQL/Edit.aspx',4,'dv-query-bo-display','dv-query-bo','dv-query-bo','QR_','&hl=1&pn='+ProjectName+parameters,'text',null);
    CommonCall('Callback/MSSQL/Edit.aspx',4,'dv-query-bo-value','dv-query-bo','dv-query-bo','QR_','&pn='+ProjectName+parameters,'text',null);
}
function ftSP(obj)
{
    ft(obj);
    if(o('dv-store-procedures').innerHTML=='')CommonCall('Callback/MSSQL/Edit.aspx',5,'dv-store-procedures','dv-store-procedures','dv-store-procedures','EP_','&pn='+ProjectName,'text',"if(oa('txt-store-procedure-do-value')=='')GenDoStoreProcedure();");
}
function GenStoreProcSimple(spn)
{
    o('SP_ObjectName').value=spn;
    o('SP_StoreProcedureName').value=spn;
    CommonCall('Callback/MSSQL/Edit.aspx',6,'dv-store-procedure-simple','dv-store-procedure-parameters','dv-store-procedure-simple','EP_','&pn='+ProjectName+'&spn='+spn,'text',null);
}
function GenCodeByStoreProcedure()
{
    if(!ValidateTxt('dv-store-procedure-parameters') || oa('SP_Sql')==''){MsgBox('Invalid Parameters','System Information',true,'dv-store-procedure-parameters');return false;}
    InitView('store-procedure');
    GenDoStoreProcedure();
}
function GenDoStoreProcedure()
{
    var parameters='&on='+oa('SP_ObjectName')+'&sql='+oa('SP_Sql')+'&spn='+oa('SP_StoreProcedureName');
    //CommonCall('Callback/MSSQL/Edit.aspx',7,'dv-store-procedure-do-display','dv-store-procedure-do','dv-store-procedure-do','QR_','&hl=1&pn='+ProjectName+parameters,'text','GenBoStoreProcedure();');
    CommonCall('Callback/MSSQL/Edit.aspx',7,'dv-store-procedure-do-value','dv-store-procedure-do','dv-store-procedure-do','QR_','&pn='+ProjectName+parameters,'text','GenBoStoreProcedure();');
}
function GenBoStoreProcedure()
{
    var parameters='&on='+oa('SP_ObjectName')+'&sql='+oa('SP_Sql')+'&spn='+oa('SP_StoreProcedureName');
   // CommonCall('Callback/MSSQL/Edit.aspx',8,'dv-store-procedure-bo-display','dv-store-procedure-bo','dv-store-procedure-bo','QR_','&hl=1&pn='+ProjectName+parameters,'text',null);
    CommonCall('Callback/MSSQL/Edit.aspx',8,'dv-store-procedure-bo-value','dv-store-procedure-bo','dv-store-procedure-bo','QR_','&pn='+ProjectName+parameters,'text',null);
}
    </script>

</asp:Content>
