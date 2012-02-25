<%@ Page Language="C#" MasterPageFile="~/MP.master" AutoEventWireup="true" CodeFile="Edit.aspx.cs"
    Inherits="XML_Edit" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MP1" runat="Server">
    <div style="float: left; width: 100%;" id="dv-parameter">
        <div style="float: left; border: solid 1px black; width: 100%;background-color:#336699;color:White;">
            <table>
                <tr>
                    <td>
                        Name Space</td>
                    <td>
                        <input type="text" id="txtNameSpace" class="txt" style="width: 300px;" value="AutoTicketLibrary.XmlObject" /></td>
                    <td>
                        Generate Type</td>
                    <td>
                        <input type="radio" id="GenC" name="GenType" checked="checked" /><label for="GenC">Class</label><input
                            type="radio" name="GenType" id="GenS" /><label for="GenS">Data Set</label></td>
                </tr>
            </table>
        </div>
        <div style="float: left; width: 100%; background-color: #336699; padding-left: 2px;
            color: White; margin-top: 5px;">
            XML</div>
        <div style="border: solid 1px #336699; float: left; width: 100%;">
            <textarea id="txtXml" rows="10" cols="10" style="width: 99%; border-width: 0px;"><?xml version="1.0"?>
<BannerCollections xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://www.opentravel.org/OTA/2003/05">
  <Setup>
    <Key>Admin/Index.aspx,1,en-us</Key>
    <BannerID>1000</BannerID>
  </Setup>
  <Setup>
    <Key>Admin/Index.aspx,2,en-us</Key>
    <BannerID>100</BannerID>
  </Setup>
  <Banner>
    <ID>1</ID>
    <IsFlash>True</IsFlash>
    <URL>http://www.baidu.com</URL>
    <Target>http://www.baidu.com</Target>
  </Banner>
  <Banner>
    <ID>2</ID>
    <IsFlash>True</IsFlash>
    <URL>http://www.baidu.com</URL>
    <Target>http://www.baidu.com</Target>
  </Banner>
</BannerCollections></textarea></div>
        <div style="float: left;background-color:#cccccc;width:100%;margin-top:5px;text-align:right;border:solid 1px #336699;padding:0px 0px 2px 0px;">
            <input type="button" value="Generate" onclick="GenCodeByXml();" class="btn5" /></div>
    </div>
    <div style="float: left; width: 100%; background-color: #336699; padding-left: 2px;
        color: White; margin-top: 5px;">
        Code</div>
    <div style="border: solid 1px #336699; float: left; width: 100%;" id="dv-code">
        <textarea id="txtCode" rows="18" cols="10" style="width: 99%; border-width: 0px;"></textarea></div>

    <script language="javascript">
function GenCodeByXml()
{
    CommonCall('Callback/XML/Edit.aspx',0,'dv-code','dv-parameter','dv-parameter','XML_','&ns='+oa('txtNameSpace')+'&xml='+oa('txtXml')+(o('GenS').checked?'&isds=1':''),'text',null);
}
    </script>

</asp:Content>
