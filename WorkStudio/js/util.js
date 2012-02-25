function SelectAdd(obj, value, text, position) {
    var userAgent = window.navigator.userAgent;
    if (userAgent.indexOf("MSIE") > 0) {
        var option = document.createElement("option");
        option.value = value;
        option.innerText = text;
        obj.insertBefore(option, obj.options[position]);
    }
    else {
        obj.insertBefore(new Option(text, value), obj.options[position]);
    }
}

function o(obj) { return document.getElementById(obj); }
function oaen(obj) { return encodeURIComponent(o(obj).value); }
function oa(obj) { return o(obj).value; }
function sf(sId) { try { o(sId).focus(); } catch (e) { } }
function IsEmpty(fData) { return ((fData == null) || (fData.length == 0)) }
function IsEmail(mail) { return (new RegExp(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/).test(mail)); }
function IsEmails(mails) { var m = mails.split(','); for (var i = 0; i < m.length; i++) { if (!IsEmail(m[i])) return false; } return true; }
function chkEmail(sId, dId, allowEmpty) {
    var v = oa(sId);
    if (allowEmpty) {
        if (IsEmpty(v)) { return true; }
    }
    if (IsEmail(v)) { return true; }
    else { o(dId).innerHTML = getAttibuteValue(sId, 'erremail'); sf(sId); }
    return false;
}
function IsPhone(fData) { var str; var fDatastr = ""; if (IsEmpty(fData)) { return false; } for (var i = 0; i < fData.length; i++) { str = fData.substring(i, i + 1); if (str != "(" && str != ")" && str != "（" && str != "）" && str != "+" && str != "-" && str != " ") { fDatastr = fDatastr + str; } } if (isNaN(fDatastr)) { return false } return true; }
function copyAttributes(obj, copyTo) {
    for (var i = 0; i < obj.attributes.length; i++) {
        if (obj.attributes[i].name != "disabled")
        { copyTo.setAttribute(obj.attributes[i].name, obj.attributes[i].value); }
    }
    copyTo.disabled = obj.disabled;
    copyTo.className = obj.className;
    copyTo.style.cssText = obj.style.cssText;
}
function getParams(_id, str) {
    if (IsEmpty(_id)) return '';
    var params = "";
    function setParams(subObj) {
        if (subObj.id.indexOf(str, 0) >= 0) {
            if (subObj.type.toLowerCase() == "checkbox")
            { params += "&" + subObj.id + "=" + subObj.checked; }
            else { params += "&" + subObj.id + "=" + encodeURIComponent(subObj.value); }
        }
    }

    var obj = o(_id);
    for (var i = 0; i <= obj.getElementsByTagName("input").length - 1; i++) {
        setParams(obj.getElementsByTagName("input")[i]);
    }
    for (var i = 0; i <= obj.getElementsByTagName("select").length - 1; i++) {
        setParams(obj.getElementsByTagName("select")[i]);
    }
    return params;
}

function tableScroll(divId, tbId, hdrId) {
    var ds = o(tbId);
    var dsDIV = o(divId);
    var pad = ds.getAttribute("cellpadding");
    var dsTR = ds.getElementsByTagName("tr")[0];
    var divHdr = o(hdrId);
    //divdocument.createElement("div");
    //divDS.parentNode.appendChild(divHdr);
    //divHdr.style.cssText="position: absolute; text-align: center;";
    divHdr.style.width = dsDIV.clientWidth + "px";

    var tbHdr = document.createElement("table");
    //tbHdr.id='tbHdr';
    divHdr.appendChild(tbHdr);
    copyAttributes(ds, tbHdr);

    var tr = tbHdr.insertRow(0);
    copyAttributes(dsTR, tr);
    for (var i = 0; i <= dsTR.getElementsByTagName("td").length - 1; i++) {
        var dsTD = dsTR.getElementsByTagName("td")[i];
        var td = tr.insertCell(i);
        td.innerHTML = dsTD.innerHTML;
        copyAttributes(dsTD, td);
        td.style.width = dsTD.offsetWidth - (pad * 2) + "px";
    }
    tbHdr.style.width = ds.clientWidth + "px";
}
String.prototype.trim = function()
{ return (this.replace(new RegExp("^([\\s]+)|([\\s]+)$", "gm"), "")) };
Array.prototype.clean = function(C)
{ var B = []; for (var A = 0; A < this.length; A++) { if (this[A] != C) { B.push(this[A]) } } return B };
Array.prototype.indexOf = function(B) { for (var A = 0; A < this.length; A++) { if (this[A] == B) { return A } } return -1 };
function a(objId, url, pars, method, onComplete, asynchronous, args) { url += ((url.indexOf('?') > 0 ? "&" : "?") + ("r=" + Math.random())); var xmlHttp; if (window.XMLHttpRequest) { try { xmlHttp = new XMLHttpRequest() } catch (e1) { alert("HTTP error ") } } else { if (window.ActiveXObject) { try { xmlHttp = new ActiveXObject("Msxml2.XMLHTTP") } catch (e1) { try { xmlHttp = new ActiveXObject("Microsoft.XMLHTTP") } catch (e2) { xmlHttp = false } } } } xmlHttp.onreadystatechange = function() { if (xmlHttp.readyState == 4) { if (onComplete != null) { onComplete(xmlHttp.responseText, objId, args) } } }; if (method.toLowerCase() == "get") { url = url + "?" + pars; xmlHttp.open("GET", encodeURI(url), asynchronous); xmlHttp.send(null) } else { xmlHttp.open("POST", url, asynchronous); xmlHttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded"); xmlHttp.send(encodeURI(pars)) } }
function EndRequest(responseTest, objId, args) { if (o(objId) != null) o(objId).innerHTML = responseTest; }
function addEvent(obj, evenTypeName, fn) { if (obj.addEventListener) { obj.addEventListener(evenTypeName, fn, true); return true } else if (obj.attachEvent) { return obj.attachEvent("on" + evenTypeName, fn) } else { return false } }
function removeEvent(obj, type, fn) {
    if (obj.removeEventListener) obj.removeEventListener(type, fn, false);
    else if (obj.detachEvent) {
        obj.detachEvent("on" + type, obj[type + fn]);
        obj[type + fn] = null;
        obj["e" + type + fn] = null;
    }
}
function getUrlParameter(seekParameter) {
    var url = location.href;
    var parameters = url.substr(url.indexOf("?") + 1);
    var parameterItems = parameters.split("&");
    var parameterName;
    var parameterVar = "";
    for (var i = 0; i < parameterItems.length; i++) {
        parameterName = parameterItems[i].split('=')[0];
        parameterVar = parameterItems[i].split('=')[1];
        if (parameterName == seekParameter) {
            return (parameterVar);
        }
    }
    return "";
}
function setUrlParam(url, param, v) {
    var re = new RegExp("(\\\?|&)" + param + "=([^&]+)(&|$)", "i");
    var m = url.match(re);
    if (m) {
        return (url.replace(re, function($0, $1, $2) { return ($0.replace($2, v)); }));
    }
    else {
        if (url.indexOf('?') == -1)
            return (url + '?' + param + '=' + v);
        else
            return (url + '&' + param + '=' + v);
    }
}
var Browser = new Object();
Browser.isIE = window.ActiveXObject ? true : false;
Browser.isIE7 = Browser.isIE && window.XMLHttpRequest;
Browser.isIE8 = Browser.isIE && navigator.userAgent.indexOf('8.0') > 0;
Browser.isMozilla = Browser.isIE ? false : (typeof document.implementation != 'undefined') && (typeof document.implementation.createDocument != 'undefined') && (typeof HTMLDocument != 'undefined');
GetBrowser = function() {
    var ua = navigator.userAgent;
    var i;

    var re = /(MSIE [1-9]\.[0-9])/;
    var matchResult = ua.match(re);
    if (ua.indexOf("TencentTraveler") >= 0) {
        return "TencentTraveler-" + matchResult[1] + navigator.appMinorVersion;
    }
    else if (ua.indexOf("Maxthon") >= 0) {
        return "Maxthon-" + matchResult[1] + navigator.appMinorVersion;
    }
    else if (ua.toLowerCase().indexOf("myie") >= 0) {
        return "MyIE-" + matchResult[1] + navigator.appMinorVersion;
    }
    else if (ua.indexOf("MSIE") >= 0) {
        return matchResult[1] + navigator.appMinorVersion;
    }
    i = ua.indexOf("Firefox");
    if (i >= 0) {
        return ua.substring(i);
    }
    i = ua.indexOf("Netscape");
    if (i >= 0) {
        return ua.substring(i);
    }
    i = ua.indexOf("Opera");
    if (i >= 0) {
        return ua.substring(i);
    }
    i = ua.indexOf("Konqueror");
    if (i >= 0) {
        return ua.substring(i, i + 13);
    }
    return "OTHER";
}
function getAttibuteValue(sId, text) {
    var oj = o(sId);
    if (oj.attributes[text] != null) { return oj.attributes[text].value; }
    return "";
}
function isRequired(obj) {
    if (obj != null) {
        if (obj.disabled != null && obj.disabled == true) { return false; }
        return obj.attributes["required"] == null ? false : true;
    }
    else { return false; }
}
function DateControl(sId, butId, onchange, allowEmpty, left, top, error_lb, isAddErrCss) {
    var oj = o(sId);
    var errMsg = "Invalid date format";
    if (oj == null) { alert('Miss the "' + sId + '"'); return; }

    if (oj.attributes["errmsg"] != null) { errMsg = oj.attributes["errmsg"].value; }
    if (butId != null) {
        if (left == null || top == null) {
            Calendar.setup({
                inputField: sId,
                ifFormat: "%d/%m/%Y",
                showsTime: false,
                button: butId,
                singleClick: true,
                step: 1
            });
        }
        else {
            Calendar.setup({
                inputField: sId,
                ifFormat: "%d/%m/%Y",
                showsTime: false,
                button: butId,
                singleClick: true,
                step: 1,
                position: [left, top]
            });
        }
    }
    var lv = oj.value;
    oj.onfocus = "";
    oj.onblur = "";
    oj.onchange = function() { if (formatDate(oj, allowEmpty)) { if (isAddErrCss) { oj.className = 'txt'; o(error_lb).innerHTML = ''; } if (onchange != null && oj.value != "") { onchange(sId); } } else { if (isAddErrCss) { oj.className = 'txtrq'; } inputErr(); } };

    function inputErr() {
        if (error_lb == null)
            Error(errMsg);
        else
            o(error_lb).innerHTML = errMsg;
        oj.value = lv;
    }
}
//format ddMMyyyy,ddMMMyyyy to dd/MM/yyyy 
function formatDate(oj, allowEmpty) {
    var str = oj.value;
    if (allowEmpty) { if (str == "") return true; }
    if (str == "") { return false; }
    var month_array = new Array("", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OTC", "NOV", "DEC");
    var d = str;
    var m = d.match(/^(\d{2})([a-zA-Z]{3}|\d{2})(\d{2,4})$/);
    if (!(m == null || m.length < 4)) {
        for (var j = 0; j < month_array.length; j++) {
            if (month_array[j] == m[2].toUpperCase()) {
                m[2] = j;
                if (j < 10) {
                    m[2] = "0" + m[2];
                }
            }
        }
        if (isNaN(m[2]) == false) {
            if (m[3].length < 3) {
                m[3] = "20" + m[3];
            }
        }
        str = m[1] + "/" + m[2] + "/" + m[3];
    }
    if (ValidateDate(str)) {
        oj.value = str;
        lv = str;
        return true;
    }
    else {
        return false;
    }

}
function ValidateDate(str) {
    try {
        var dateinfo = str.split("/");

        if (dateinfo.length == 3) {
            if (dateinfo[2].length != 4 || parseInt(dateinfo[1]) > 12 || dateinfo[1].length != 2)
                return false;

            var day = parseInt(dateinfo[0], 10);
            var moth = parseInt(dateinfo[1], 10);
            var year = parseInt(dateinfo[2], 10);
            var maxday = new Date(parseInt(year), parseInt(moth), 0).getDate();

            if (parseInt(day, 10) > parseInt(maxday, 10) || (parseInt(year, 10) > 2099 || parseInt(year, 10) < 1900)
                || parseInt(moth, 10) == 0 || parseInt(day, 10) == 0
                || isNaN(day) || isNaN(moth) || isNaN(year)) {
                return false;
            }
            else
                return true;
        }
        else
            return false;
    }
    catch (e) {
        alert(e);
        return false;
    }
}
function ConvertDate(str) {
    var a = new Array();
    a = str.split('/');
    return new Date(a[2], a[1] - 1, a[0]);
}
function getDateToStr(date) {
    var d = date.getDate();
    var day = (d < 10) ? '0' + d : d;
    var m = date.getMonth() + 1;
    var month = (m < 10) ? '0' + m : m;
    var yy = date.getYear();
    var year = (yy < 1000) ? yy + 1900 : yy;
    return day + "/" + month + "/" + year;
}
function compareDate(date1, date2, changeId, num, enforce) {
    if (isNaN(date1) || isNaN(date2))
        return true;
    if (num == null) { num = 1; }
    if (daysElapsed(date2, date1) >= num && !enforce) {
        return true;
    }
    else {
        var d = DateAdd("D", num, date1);
        o(changeId).value = getDateToStr(d);
        return false;
    }
}
var imgTipsSrc = "images/tips.gif";
function GetHtml(content, isError) {
    var imgsrc = SerUrl + imgTipsSrc;
    if (isError)
        imgsrc = imgErrorSrc;
    var fontcolor = "";
    if (isError)
        fontcolor = "red";
    var layer = "<div id='myDiv'>"
        + "<table onclick=\"RemovePopMain()\" width='238' border='0' cellspacing='5' class='boxblue' style='z-index:200'>"
        + "<tr>"
        + "<td valign=top><img src='" + imgsrc + "'></td>"
        + "<td width=100% valign=top><font color='" + fontcolor + "'>" + content + "</font></td>"
        + "</tr>"
        + "</table>"
        + "</div>";
    return layer;
}
function getScroll() {
    var t, l, w, h;
    if (document.documentElement && document.documentElement.scrollTop) {
        t = document.documentElement.scrollTop;
        l = document.documentElement.scrollLeft;
        w = document.documentElement.scrollWidth;
        h = document.documentElement.scrollHeight;
    }
    else if (document.body) {
        t = document.body.scrollTop;
        l = document.body.scrollLeft;
        w = document.body.scrollWidth;
        h = document.body.scrollHeight;
    }
    return { t: t, l: l, w: w, h: h };
}
function ShowDiv(content, width, height, bordercolor, borderstyle) {
    ShowDiv(content, width, height, bordercolor, "", 0, 0);
}
function ShowDiv(content, width, height, bordercolor, borderstyle, left, top, sId) {
    if (o("alert_div") != null) {
        CloseMsgBox();
    }

    var scrollLeft = (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft);
    var scrollTop = (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop);
    var clientWidth;
    if (window.innerWidth) {
        clientWidth = Math.min(window.innerWidth, document.documentElement.clientWidth);
    }
    else {
        clientWidth = document.documentElement.clientWidth;
    }
    var clientHeight;
    if (window.innerHeight) {
        clientHeight = Math.min(window.innerHeight, document.documentElement.clientHeight);
    }
    else {
        clientHeight = document.documentElement.clientHeight;
    }
    var alert_div = document.createElement("div");

    with (alert_div) {
        id = "alert_div";
        if(borderstyle=="MSGBOX")id="alert_div_MSGBOX";
        style.position = "absolute";
        style.zIndex = "1001";
        style.textAlign = 'center';
        style.verticalAlign = 'middle';
        if (width > 0)
            style.width = width + "px";
        if (height > 0)
            style.height = height + 'px';

        switch (borderstyle) {
            case "MSGBOX":
                var border = "<table border='0px' cellpadding='0px' cellspacing='0px' width='100%'><tr><td colspan='3' style='height: 5px' /></tr><tr><td class='msg-lt' /><td class='msg-mt' /><td class='msg-rt' /></tr><tr><td class='msg-l' /><td class='msg-bg' align='left'>" + content + "</td><td class='msg-r' /></tr><tr><td class='msg-lb' /><td class='msg-mb' /><td class='msg-rb' /></tr></table>";
                innerHTML = border;
                break;
            case "SHADOW":
                var border = "<div id=\"shadow\"><div class=\"shadow1\"><div class=\"shadow2\"><div class=\"shadow3\"><div class=\"content\">" + content + "</div></div></div></div></div>";
                innerHTML = border;
                break;
            default:
                innerHTML = content;
                if(bordercolor!=''){
                style.border = "1px solid " + bordercolor;
                style.backgroundColor = '#F4F4F4';}
                break;
        }
    }

    document.body.appendChild(alert_div);
    var bgObj = o('bgDiv');
    if (bgObj != null && sId != null) {
        var sTop = GetPosition(bgObj).top + (GetPosition(bgObj).height - height) / 2 - 20;
        alert_div.style.left = GetPosition(bgObj).left + (GetPosition(bgObj).width - width) / 2 + 'px';
        alert_div.style.top = sTop + 'px';
    }
    else {
        alert_div.style.left = (left == 0 ? scrollLeft + ((clientWidth - width) / 2) : left) + 'px';
        alert_div.style.top = (top == 0 ? scrollTop + ((clientHeight - height) / 2 - 20) : top) + 'px';
    }
}
function MsgBoxAction(content, title, script_str, sId) {
    MsgBoxFun(false, content, title, true, sId, script_str);
}
function MsgBox(content, title, is_error, sId) {
    MsgBoxFun(false, content, title, is_error, sId, "");
}
function Confirm(content, title, is_error, sId, script_str, btn_ok_css, btn_cancel_css) {
    MsgBoxFun(true, content, title, is_error, sId, script_str, btn_ok_css, btn_cancel_css);
}

function MsgIframe(title, width, height, serUrl, page, sId) {
    if (IsEmpty(title)) title = "System Information";
    var content_body = "<table width='100%' border='0px' cellpadding='0px' cellspacing='0px'><tr class=\"msg-title\"><td align='left'>&nbsp;" + title +
    "</td><td align='right' style='text-align:right;'><div onclick='CloseMsgBox();' class='msg-cs'></div></td></tr><tr><td align='center' colspan='2' id='td_content' style='background-color:white;'>" +
     "<iframe id=\"iframe_target\" frameborder=\"0\" marginwidth=\"0\" marginheight=\"0\" style=\" width:" + (width - 10) + "px; height:" + (height - 50) + "px;\" src=\"" + serUrl + page + "\"></iframe>" +
    "</td></tr></table>";
    ShowBackgroup(sId);
    ShowDiv(content_body, width, height, "#FFB97A", "MSGBOX", 0, 0, sId);
    ShowBackgroup(sId);
    sf("_MsgBtn");
}

function MsgAjax(title, width, height, serUrl, page, sId, func) {
    if (IsEmpty(title)) title = "System Information";
    var content_body = "<table width='100%' border='0px' cellpadding='0px' cellspacing='0px'><tr class=\"msg-title\"><td align='left'>&nbsp;" + title +
    "</td><td align='right' style='text-align:right;'><div onclick='CloseMsgBox();' class='msg-cs'></div></td></tr><tr><td colspan='2' id='ajax-content' style='background-color:white;'><div style='text-align:center;width:100%;'><img src='" + serUrl + "images/loading.gif' /></div>" +
    "</td></tr></table>";
    a("ajax-content", serUrl + page, '', 'post', func == null ? EndRequest : func, true);
    ShowBackgroup(sId);
    ShowDiv(content_body, width, height, "#FFB97A", "MSGBOX", 0, 0, sId);
    ShowBackgroup(sId);
    sf("_MsgBtn");
}

function MsgBoxFun(is_confirm, content, title, is_error, sId, script_str, btn_ok_css, btn_cancel_css) {
    if (IsEmpty(btn_ok_css)) btn_ok_css = "btn";
    if (IsEmpty(btn_cancel_css)) btn_cancel_css = "btn";
    if (IsEmpty(title)) title = "System Information";
    if (is_error) content = "<table width='100%'><tr><td align='center' style='padding-left: 10px; padding-right: 20px; width: 1px'><img src='" + SerUrl + "images/error1.gif' /></td><td width='99%' align='left'>" + content + "</td></tr></table>";
    var content_body = "<table width='100%' border='0px' cellpadding='0px' cellspacing='0px'><tr class=\"msg-title\"><td>&nbsp;" + title +
    "</td></tr><tr><td class=\"msg-bb\" /></tr><tr height='80px'><td align='center'>" + content +
    "</td></tr><tr><td align='center'>";
    var btn_name = "OK";
    if (is_confirm) { content_body += "<input type='button' id=\"_MsgBtn\" class='" + btn_ok_css + "' width='50px' value='OK' onclick=\"" + script_str + "\"/>&nbsp;"; btn_name = "Cancel"; }
    var click_event = 'CloseMsgBox();';
    if (script_str != '' && !is_confirm) click_event = script_str;
    var btn_css;
    if (btn_name == "Cancel") btn_css = btn_cancel_css;
    else btn_css = btn_ok_css;
    content_body += "<input type='button' id=\"_MsgBtn\" class='" + btn_css + "' width='50px' value='" + btn_name + "' onclick=\"" + click_event + "\"/></td></tr><tr><td height='10px'></td></tr></table>";
    ShowBackgroup(sId);
    ShowDiv(content_body, 300, 130, "#FFB97A", "MSGBOX", 0, 0, sId);
    ShowBackgroup(sId);
    sf("_MsgBtn");
}
function Error(content) {
    MsgBox(content, "", true);
}
function sld(sId)//ShowLoading
{
    CM();
    ShowLoading(loading, sId);
}
function ShowLoading(l, sId)//ShowLoading
{
    ShowBackgroup(sId);
    if(sId!=null && GetPosition(o(sId)).width<250)
    {ShowDiv("<div class='loading'></div>", GetPosition(o(sId)).width/2, GetPosition(o(sId)).height/2, "", "", 0, 0, sId);
    }else
    {ShowDiv("<div style='border:solid 3px #c9e5f3;height:94px;'><table height='100%' width='100%'><tr><td valign='middle' align='right' width='100px'><div class='loading'></div></td><td valign='middle' align='left'>Please Wait......<input type=text id='_sldtext' width='0px' /></td></tr></table></div>", 250, 100, "#7ec2e9", "#ffffff", 0, 0, sId);}
    try { if (o('_sldtext') != null) { o('_sldtext').focus(); o('_sldtext').style.display = "none"; } } catch (e) { }
}
function ShowBackgroup(sId) {
    if (o(sId) == null) sId = null;
    ShowSelect(false, sId);
    var issb = false;
    var obj = sId ? o(sId) : null;
    if (obj == null)
    { obj = document.body; issb = true; }

    if (o("bgDiv") != null) return;
    var bgObj = document.createElement("div");
    bgObj.id = 'bgDiv';
    document.body.appendChild(bgObj);

    bgObj = o('bgDiv');
    if (!Browser.isIE || Browser.isIE8) {
        bgObj.style.top = GetPosition(obj).top + "px";
        bgObj.style.left = GetPosition(obj).left + "px";
    }
    else {
        bgObj.style.top = GetPosition(obj).top + 3 + "px";
        bgObj.style.left = GetPosition(obj).left + 3 + "px";
    }
    bgObj.style.width = GetPosition(obj).width + "px";
    if (issb)
        bgObj.style.height = String(document.body.offsetHeight < screen.height ? screen.height : document.body.offsetHeight + 20) + "px";
    else
        bgObj.style.height = GetPosition(obj).height + "px";
    bgObj.style.backgroundColor = "Black";
    bgObj.style.position = "absolute";
    bgObj.style.opacity = "0.25";
    bgObj.style.filter = "progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=15";
    //bgObj.style.width=String(document.body.offsetWidth+20) + "px";
    //bgObj.style.height= String(document.body.offsetHeight<screen.height? screen.height : document.body.offsetHeight+20) + "px";
    bgObj.style.zIndex = "1000";
}
function ShowSelect(xShow, objId) {
    var selects = null;
    if (!IsEmpty(objId))
        selects = o(objId).getElementsByTagName("SELECT");
    else
        selects = document.getElementsByTagName("SELECT");
    for (i = 0; i < selects.length; i++) {
        if (xShow == true) {
            selects[i].style.display = "";
        }
        else {
            selects[i].style.display = "none";
        }
    }
    ShowObject(xShow,null);
}
function ShowObject(xShow, objId) {
    var selects = null;
    if (!IsEmpty(objId))
        selects = o(objId).getElementsByTagName("object");
    else
        selects = document.getElementsByTagName("object");
    for (i = 0; i < selects.length; i++) {
        if (xShow == true) {
            selects[i].style.display = "";
        }
        else {
            selects[i].style.display = "none";
        }
    }
}
function CloseMsgBox() {
$('#bgDiv').fadeOut();if(o('alert_div_MSGBOX')==null){$('#alert_div').fadeOut();window.setTimeout("CM();",250);}else{CM();}
}
function CM() {
    if (o('alert_div') != null)document.body.removeChild(o("alert_div"));
    if(o('alert_div_MSGBOX')!=null)document.body.removeChild(o("alert_div_MSGBOX"));
    ShowSelect(true);
    var bgObj = document.getElementById("bgDiv");
    if (bgObj != null) {
        document.body.removeChild(bgObj);
    }
    if (bgObj != null) {
        bgObj = null;
    }
}
function showTip(objId, isShow, tips, iWidth, iHeight) {
    var obj = o(objId);
    addEvent(obj, "click", RemovePopMain);
    if (isShow == 1) {
        //obj.className="focusinp";
        if (!IsEmpty(tips)) {
            if (GetBrowser().indexOf("Firefox") >= 0)
                popupDialog(GetHtml(tips, false), GetPosition(obj).top + GetPosition(obj).height, GetPosition(obj).left, iWidth, iHeight);
            else
                popupDialog(GetHtml(tips, false), GetPosition(obj).top + GetPosition(obj).height + 1, GetPosition(obj).left + 1, iWidth, iHeight);
        }
        else {
            RemovePopMain();
        }
    }
    else {
        //obj.className="normalinp";
        RemovePopMain();
    }
}

function popupDialog(html, top, left, iWidth, iHeight) {
    if (!window.popUpReady) {
        createPopUpDom(iWidth, iHeight);
    }
    var content = o("popUpMain");
    var strHTML = html;
    with (content) {
        innerHTML = html; //g_Templet["dialog"].replace(/__Title__/gi,title).replace(/__HTML__/gi,html);;
        style.display = "";
        style.left = left + "px";
        style.top = top + "px";
    }
    var content_iframe = o("popUpIframe");
    var strHTML = html;
    with (content_iframe) {
        height = content.height + 300 + "px";
        style.display = "";
        style.left = left + "px";
        style.top = top + "px";
    }
}
function RemovePopMain() {
    if (o('popUpMain') != null) {
        o('popUpMain').style.display = 'none';
    }
    if (o('popUpIframe') != null) {
        o('popUpIframe').style.display = 'none';
    }
}
function createPopUpDom(iWidth, iHeight) {
    var content = document.createElement("div");
    with (content) {
        id = "popUpMain";
        style.cssText = "position:absolute;z-index:900;";
        //className = "dragItem";
    }
    document.body.appendChild(content);

    var content_iframe = document.createElement("div");
    with (content_iframe) {
        id = "popUpIframe";

        style.cssText = "position:absolute;z-index: 899;padding-left:0px;padding-right:1px; padding-bottom:1px; padding-top:0px;";
        //className = "dragItem";
        height = content.height + "px";
    }
    document.body.appendChild(content_iframe);
    if (GetBrowser().indexOf("Firefox") >= 0) {
        iWidth = iWidth - 5;
        iHeight = iHeight - 1;
    }
    o('popUpIframe').innerHTML = "<iframe id='hidenIframe' frameborder='1px' style='width: " + iWidth + "px; height:" + iHeight + "px; background: #E4F0FE; color: Transparent;position: absolute; z-index: 898;'></iframe>";

    window.popUpReady = true;
}

function GetPosition(obj) {
    var top = 0;
    var left = 0;
    var width = obj.offsetWidth;
    var height = obj.offsetHeight;
    if (height == 0) { height = screen.height; }
    while (obj.offsetParent) {
        top += obj.offsetTop;
        left += obj.offsetLeft;
        obj = obj.offsetParent;
    }
    return { "top": top, "left": left, "width": width, "height": height };
}
function GetCurrentPage() {
    var u = document.location.href;
    var i = u.indexOf('?');
    if (i > 0)
        u = u.substring(0, u.indexOf('?'))
    var info = u.split('/');
    var page_info = info[info.length - 1].split('?');
    //if(page_info[0]=="Index.aspx")
    //return "Search.aspx";
    //else
    return page_info[0];
}
//Vinson
function GetParentPage() {
    var u = document.location.href;
    var i = u.indexOf('?');
    if (i > 0)
        u = u.substring(0, u.indexOf('?'))
    var info = u.split('/');
    var page_info = info[info.length - 2].split('?');
    return page_info[0];
}
function focusInput(focusClass, normalClass) {
    var elements = document.getElementsByTagName("input");
    for (var i = 0; i < elements.length; i++) {
        if (elements[i].type != "button" && elements[i].type != "submit" && elements[i].type != "reset" && elements[i].id != "txtDst" && elements[i].id != "txtAirline" && elements[i].id != "txtDstTwo" && elements[i].id != "txtAir" && elements[i].id != "txtChkIn") {
            if (elements[i].type == "text") {
                elements[i].className = normalClass;
                elements[i].onfocus = function() { this.className = focusClass; };
                elements[i].onblur = function() { this.className = normalClass || ''; };
            }
        }
    }
}
function RegCss(src) {
    var styles = document.getElementsByTagName('link');
    for (var i = 0; i < styles.length; i++) {
        if (styles[i].type == 'text/css' && styles[i].href == src)
            return;
    }
    var style = document.createElement("link");
    style.setAttribute("type", "text/css");
    style.type = 'text/css';
    style.setAttribute("href", src);
    style.setAttribute("rel", "stylesheet");
    document.getElementsByTagName("head")[0].appendChild(style);
}
function RegJs(src) {
    var jslist = document.getElementsByTagName('script');
    for (var i = 0; i < jslist.length; i++) {
        if (jslist[i].src == src)
            return;
    }
    var js = document.createElement("script");
    js.type = 'text/javascript';
    js.src = src;
    document.body.appendChild(js);
}

function findA(obj) {
    TagA = obj.getElementsByTagName("a");
    if (TagA.length > 0)
        return TagA[0];
}
function findContentDIV(obj) {
    TagDiv = obj.childNodes;
    var arrDiv = new Array;
    for (i = 0; i < TagDiv.length; i++) {
        var objDiv = TagDiv[i];
        var re = /div/i;
        var arr = re.exec(objDiv.tagName);
        if (arr != null) {
            if (arr.index == 0) {
                arrDiv.push(objDiv);
            }
        }
    }
    return arrDiv;
}
function chShift(obj, groupDivID, type) {
    obj.style.cursor = "pointer";
    var t = obj.parentNode;
    var tA = t.getElementsByTagName("a");
    var tParent = o(groupDivID);
    var cssNormal = "";
    var cssNormalL = "";
    var cssNormalR = "";
    var cssActive = "";
    var cssActiveL = "";
    var cssActiveR = "";
    var tParentDIV = findContentDIV(tParent);

    if (type == "S") {
        cssNormal = "st_normal";
        cssActive = "st_active";
    }
    else if (type == "N") {
        cssNormal = "mm_normal";
        cssNormalL = "ml_normal";
        cssNormalR = "mr_normal";
        cssActive = "mm_active";
        cssActiveL = "ml_active";
        cssActiveR = "mr_active";
    }
    else {
        cssNormal = "";
        cssActive = "";
    }

    for (i = 0; i < tA.length; i++) {
        if (cssNormal != "") {
            tA[i].className = cssNormal;
            GetPrevObj(tA[i]).className = cssNormalL;
            GetNextObj(tA[i]).className = cssNormalR;
        }

        tParentDIV[i].style.display = "none";
        if (tA[i] == findA(obj)) {
            if (cssActive != "") {
                tA[i].className = cssActive;
                GetPrevObj(tA[i]).className = cssActiveL;
                GetNextObj(tA[i]).className = cssActiveR;
            }
            tParentDIV[i].style.display = "block";
        }
    }
    function GetPrevObj(obj) {
        var oo = obj.parentNode.previousSibling;
        if (oo != null && oo.nodeName == "LI") { return oo; }
        oo = obj.parentNode.previousSibling.previousSibling;
        if (oo != null && oo.nodeName == "LI") { return oo; }
    }
    function GetNextObj(obj) {
        var oo = obj.parentNode.nextSibling;
        if (oo != null && oo.nodeName == "LI") { return oo; }
        oo = obj.parentNode.nextSibling.nextSibling;
        if (oo != null && oo.nodeName == "LI") { return oo; }
    }
}
function daysElapsed(date1, date2) {
    var difference = Date.UTC(date1.getYear(), date1.getMonth(), date1.getDate(), 0, 0, 0) - Date.UTC(date2.getYear(), date2.getMonth(), date2.getDate(), 0, 0, 0);
    return difference / 1000 / 60 / 60 / 24;
}
function TimeCom(dateValue) {
    var newCom = new Date(dateValue);
    this.year = newCom.getYear();
    this.year = (this.year < 1900 ? (1900 + this.year) : this.year);
    this.month = newCom.getMonth() + 1;
    this.day = newCom.getDate();
    this.hour = newCom.getHours();
    this.minute = newCom.getMinutes();
    this.second = newCom.getSeconds();
    this.msecond = newCom.getMilliseconds();
    this.week = newCom.getDay();
}
function GetDateString(d) {
    return "";
}
function DateDiff(interval, date1, date2) {
    var TimeCom1 = new TimeCom(date1);
    var TimeCom2 = new TimeCom(date2);
    var result;
    switch (String(interval).toLowerCase()) {
        case "y":
        case "year":
            result = TimeCom1.year - TimeCom2.year;
            break;
        case "n":
        case "month":
            result = (TimeCom1.year - TimeCom2.year) * 12 + (TimeCom1.month - TimeCom2.month);
            break;
        case "d":
        case "day":
            result = Math.round((Date.UTC(TimeCom1.year, TimeCom1.month - 1, TimeCom1.day) - Date.UTC(TimeCom2.year, TimeCom2.month - 1, TimeCom2.day)) / (1000 * 60 * 60 * 24));
            break;
        case "h":
        case "hour":
            result = Math.round((Date.UTC(TimeCom1.year, TimeCom1.month - 1, TimeCom1.day, TimeCom1.hour) - Date.UTC(TimeCom2.year, TimeCom2.month - 1, TimeCom2.day, TimeCom2.hour)) / (1000 * 60 * 60));
            break;
        case "m":
        case "minute":
            result = Math.round((Date.UTC(TimeCom1.year, TimeCom1.month - 1, TimeCom1.day, TimeCom1.hour, TimeCom1.minute) - Date.UTC(TimeCom2.year, TimeCom2.month - 1, TimeCom2.day, TimeCom2.hour, TimeCom2.minute)) / (1000 * 60));
            break;
        case "s":
        case "second":
            result = Math.round((Date.UTC(TimeCom1.year, TimeCom1.month - 1, TimeCom1.day, TimeCom1.hour, TimeCom1.minute, TimeCom1.second) - Date.UTC(TimeCom2.year, TimeCom2.month - 1, TimeCom2.day, TimeCom2.hour, TimeCom2.minute, TimeCom2.second)) / 1000);
            break;
        case "ms":
        case "msecond":
            result = Date.UTC(TimeCom1.year, TimeCom1.month - 1, TimeCom1.day, TimeCom1.hour, TimeCom1.minute, TimeCom1.second, TimeCom1.msecond) - Date.UTC(TimeCom2.year, TimeCom2.month - 1, TimeCom2.day, TimeCom2.hour, TimeCom2.minute, TimeCom2.second, TimeCom1.msecond);
            break;
        case "w":
        case "week":
            result = Math.round((Date.UTC(TimeCom1.year, TimeCom1.month - 1, TimeCom1.day) - Date.UTC(TimeCom2.year, TimeCom2.month - 1, TimeCom2.day)) / (1000 * 60 * 60 * 24)) % 7;
            break;
        default:
            result = "invalid";
    }
    return (result);
}

function DateAdd(interval, num, dateValue) {
    var newCom = new TimeCom(dateValue);
    switch (String(interval).toLowerCase()) {
        case "y": case "year": newCom.year += num; break;
        case "n": case "month": newCom.month += num; break;
        case "d": case "day": newCom.day += num; break;
        case "h": case "hour": newCom.hour += num; break;
        case "m": case "minute": newCom.minute += num; break;
        case "s": case "second": newCom.second += num; break;
        case "ms": case "msecond": newCom.msecond += num; break;
        case "w": case "week": newCom.day += num * 7; break;
        default: return ("invalid");
    }
    var now = newCom.year + "/" + newCom.month + "/" + newCom.day + " " + newCom.hour + ":" + newCom.minute + ":" + newCom.second;
    return (new Date(now));
}
function FormatUrl(type) {
    var str_url, str_pos, str_para, new_para, new_url, str_lang;
    var arr_param = new Array();
    new_para = "";
    str_lang = "";
    new_para = "";
    str_lang = "lang=" + type;
    str_url = window.location.href;
    str_url = str_url.replace("#", "");
    str_pos = str_url.indexOf("?");
    str_para = str_url.substring(str_pos + 1);
    new_url = window.location.href.split("?")[0];

    if (str_pos > 0) {
        arr_param = str_para.split("&");
        for (var i = 0; i < arr_param.length; i++) {
            var temp_str = new Array()
            temp_str = arr_param[i].split("=")
            if (temp_str[0].toUpperCase() != "LANG") {
                var obj = new Object()
                obj.param_name = temp_str[0]
                obj.param_str = temp_str[1]
                arr_param[i] = obj
            }
        }

        for (var i = 0; i < arr_param.length; i++) {
            if (arr_param[i].param_name != null && arr_param[i].param_str != null)
                new_para += "&" + arr_param[i].param_name + "=" + arr_param[i].param_str;
        }
    }

    if (new_para != "")
        new_url = new_url + "?" + str_lang + new_para;
    else
        new_url = new_url + "?" + str_lang;

    window.location.href = new_url;
    return false;
}
function onlyNumber(obj, e) {
    function Invalid() {
        if (Browser.isIE) { e.returnValue = false; }
        else { e.preventDefault(); }
    }
    var k = window.event ? e.keyCode : e.which;
    if (k == 0) { return true; }
    if (k == 46) { return obj.value.indexOf('.') == -1 ? true : Invalid(); }
    if (((k > 47) && (k < 58)) || k == 8 || k == 9 || k == 33 || k == 34 || k == 36 || k == 35 || k == 37 || k == 38 || k == 39 || k == 40 || k == 20 || k == 16 || k == 17 || k == 18)
    { return true; }
    else
    { Invalid(); }
}
function MaskTime(_sId) {
    var obj = o(_sId);
    if (obj == null) return;
    obj.setAttribute("autocomplete", "off");
    if (obj == null) return;
    obj.maxLength = 5;
    obj.onkeypress = _maskTime;
    obj.onblur = fixTime;

    function _maskTime(e) {
        var k = (Browser.isIE ? event.keyCode : (e.keyCode == 0 ? e.charCode : e.keyCode));
        if (IntNumber(e)) {
            if (k == 8) return true;
            var postion;
            if (Browser.isIE) {
                if (document.selection.createRange().text != "" && obj.value.indexOf(document.selection.createRange().text, 0) == 0)
                { postion = 0; }
                else
                { postion = obj.value.length; }
            }
            else { postion = obj.selectionStart; }
            switch (postion) {
                case 0:
                    return k <= 50 ? true : false;
                case 1:
                    if (obj.value == '2')
                        return k <= 51 ? true : false;
                    else
                        return true;
                case 2:
                    if (obj.value.indexOf(":", 2) < 0) obj.value += ":";
                    return k <= 53 ? true : false;
                case 3:
                    return k <= 53 ? true : false;
                default:
                    return true;
            }
        }
        else
            return false;
    }
    function fixTime(e) {
        switch (obj.value.length) {
            case 0:
                return true;
            case 1:
                obj.value += '0:00'; return false;
            case 2:
                obj.value += ':00'; return false;
            case 3:
                obj.value += '00'; return false;
            case 4:
                obj.value += '0'; return false;
            default:
                return true;
        }
    }
}
function IntNumber(e) {
    var k = (Browser.isIE ? event.keyCode : (e.keyCode == 0 ? e.charCode : e.keyCode));
    if (((k > 47) && (k < 58)) || k == 8 || k == 9 || k == 33 || k == 34 || k == 36 || k == 35 || k == 37 || k == 38 || k == 39 || k == 40 || k == 20 || k == 16 || k == 17 || k == 18)
    { return true; }
    else
    { return false; }
}

//Scroll To Control
function elementPosition(obj) {
    var curleft = 0, curtop = 0;

    if (obj.offsetParent) {
        curleft = obj.offsetLeft;
        curtop = obj.offsetTop;

        while (obj = obj.offsetParent) {
            curleft += obj.offsetLeft;
            curtop += obj.offsetTop;
        }
    }

    return { x: curleft, y: curtop };
}

function ScrollToControl(id, abligate_height) {
    var elem = document.getElementById(id);
    var scrollPos = elementPosition(elem).y;
    scrollPos = scrollPos - document.documentElement.scrollTop;
    var remainder = scrollPos % 50;
    var repeatTimes = (scrollPos - remainder) / 50;
    ScrollSmoothly(scrollPos, repeatTimes);
    var h = 0;
    if (!IsEmpty(abligate_height)) h = abligate_height;
    window.scrollBy(0, remainder - h);
}
var repeatCount = 0;
var cTimeout;
var timeoutIntervals = new Array();
var timeoutIntervalSpeed;
function ScrollSmoothly(scrollPos, repeatTimes) {
    if (repeatCount < repeatTimes) {
        window.scrollBy(0, 50);
    }
    else {
        repeatCount = 0;
        clearTimeout(cTimeout);
        return;
    }
    repeatCount++;
    cTimeout = setTimeout("ScrollSmoothly('" + scrollPos + "','" + repeatTimes + "')", 10);
}
//End Scroll To Control
function PrintPage(start, end) {
    if (start != null) eval(start);
    window.print();
    if (end != null) {
        if (Browser.isIE)
        { eval(end); }
        else
        { setTimeout(end, 5000); }
    }
}
function EnterEvent(fn, e) {
    if (e.keyCode == 13) {
        eval(fn);
    }
}
function isNumAndGtZero(num) {
    return !isNaN(num) && num >= 0;
}
function getCK(name) { var start = document.cookie.indexOf(name + "="); var len = start + name.length + 1; if ((!start) && (name != document.cookie.substring(0, name.length))) { return null; } if (start == -1) return null; var end = document.cookie.indexOf(';', len); if (end == -1) end = document.cookie.length; return unescape(document.cookie.substring(len, end)); }
function setCK(name, value, expires, path, domain, secure) { var today = new Date(); today.setTime(today.getTime()); if (expires) { expires = expires * 1000 * 60 * 60 * 24; } var expires_date = new Date(today.getTime() + (expires)); document.cookie = name + '=' + escape(value) + ((expires) ? ';expires=' + expires_date.toGMTString() : '') + ((path) ? ';path=' + path : '') + ((domain) ? ';domain=' + domain : '') + ((secure) ? ';secure' : ''); }
function delCK(name, path, domain) { if (getCK(name)) { document.cookie = name + '=' + ((path) ? ';path=' + path : '') + ((domain) ? ';domain=' + domain : '') + ';expires=Thu, 01-Jan-1970 00:00:01 GMT'; } }
function ChgBtnEnable(btnId, isEnable) {
    var btn = o(btnId);
    if (isEnable) {
        btn.style.color = "";
        btn.style.cursor = "pointer";
        btn.disabled = false;
    }
    else {
        btn.style.color = "#BFBFBF";
        btn.style.cursor = "default";
        btn.disabled = true;
    }
}
function ReplaceStr(str, orgStr, regOption, repStr) {
    return str.replace(new RegExp(orgStr, regOption), repStr);
}