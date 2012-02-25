/// <reference path="jquery-vsdoc.js" />
// JScript 文件
var loading = "<img src='" + SerUrl + "images/loading.gif' />";
var sloading = "<img src='" + SerUrl + "images/s_loading.gif' />";
var hloading = "<img src='" + SerUrl + "images/hbload.gif' />";
var errorImg = "<img src='" + SerUrl + "images/problem.gif' style='padding:0px 05px 0px 5px;' />";
function IsTimeout(ret_text) {
    return ret_text == "Timeout";
}
function RedirectLogin() {
    window.location = SerUrl + 'login.aspx' + '?to=1' + GetPara("lang");
}
function EndRequestAndValidate(str, objId) {
    if (IsTimeout(str)) RedirectLogin();
    else o(objId).innerHTML = str;
}
function ValidateSession(ret_text) {
    if (IsTimeout(ret_text)) { RedirectLogin(); return false; }
    else
        return true;
}
function GetPara(lang) {
    if (GetUrlParaValue(lang))
    { return "&lang=" + GetUrlParaValue("lang"); }
    return "";
}
function GetUrlParaValue(argname) {
    var str = location.href;
    var submatch;
    if (submatch = str.match(/\?([^#]*)#?/)) {
        var argstr = '&' + submatch[1];
        var returnPattern = function(str) {
            return str.replace(/&([^=]+)=([^&]*)/, 'o1:"o2",');
        };
        argstr = argstr.replace(/&([^=]+)=([^&]*)/g, returnPattern);
        eval('var retvalue = {' + argstr.substr(0, argstr.length - 1) + '};');
        return retvalue[argname];
    }
    return null;
}
//ft(this,'navcontainer','edit-content','tab')
//fucus the ul>li tab
function ft(selObj,dvNavigatorId,dvEditId,tabCssName,dvPrefix)//focus this
{
    if(selObj.id=='active')return;
    tabCssName=IsEmpty(tabCssName)?'tab':tabCssName;
    dvPrefix=(dvPrefix==null||dvPrefix==undefined)? 'dv-':dvPrefix;
    dvNavigatorId=IsEmpty(dvNavigatorId)?'navcontainer':dvNavigatorId;
    dvEditId=IsEmpty(dvEditId)?'edit-content':dvEditId;
    $('li','#'+dvNavigatorId).each(function (i){this.id='';});
    $('div.'+tabCssName,'#'+dvEditId).css('display','none');
    selObj.id='active';
    $('#'+dvPrefix+selObj.className,'#'+dvEditId).fadeIn();//.css('display','');
}
