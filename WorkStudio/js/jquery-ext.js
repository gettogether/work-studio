function CommonSearch(Url,type,divId,divBgId,divParId,inputPrefix,parAppend,pageIndex,pageSize,sortBy,isAsc,dType,jsFun)
{
    var params="";
    params=getParams(divParId,inputPrefix);
    if(IsEmpty(params))params="";
    if(!IsEmpty(pageIndex)){params+="&page="+pageIndex;}
    if(!IsEmpty(pageSize)){params+="&size="+pageSize;}
    if(!IsEmpty(sortBy)){params+="&sort="+sortBy;}
    if(!IsEmpty(isAsc)){params+="&asc="+(isAsc?"Y":"N");}
    if(!IsEmpty(parAppend)){params+=parAppend;}
    params="type="+type+"&parms="+params;
    CallAjaxForGetList(SerUrl+Url,params,divId,divBgId,dType,jsFun);
}

function CommonCall(Url,type,divId,divBgId,divParId,inputPrefix,parAppend,dType,jsFun)
{
    var params="";
    params=getParams(divParId,inputPrefix);
    if(IsEmpty(params))params="";
    if(!IsEmpty(parAppend)){params+=parAppend;}
    params="type="+type+"&parms="+params;
    CallAjaxForGetList(SerUrl+Url,params,divId,divBgId,dType,jsFun);
}

function CallAjaxForGetList(url, params, divId, divBgId, dType, jsFun) {
    if (dType == null) dType = "text";
    $.ajax({
        url: url,
        type: 'post',
        random: Math.random(),
        cache: false,
        dataType: dType,
        data: params,
        beforeSend: function() {
            if (!IsEmpty(divBgId)) sld(divBgId);
        },
        error: function() {
            if (!IsEmpty(divBgId)) CloseMsgBox();
            $("#" + divId).html('occur error!');
        },
        success: function(data, textStatus) {
            if (!IsEmpty(divBgId)) CloseMsgBox();
            if (dType == 'text') {
                if (IsTimeout(data)) RedirectLogin();
                $("#" + divId).html(data);
                if (jsFun != null) eval(jsFun);
            }
            else if (dType == 'json') {
                if (data.success) {
                    if (jsFun != null) eval(jsFun);
                }
                else {
                    MsgBox(data.message, 'System Information', true, divBgId);
                }
            }
        }
    });
}
//CallAjaxForDelSel('Callback/Sys/RightRef.aspx',3,'RC','keys','dv_rightRef_list','content','','','','json','SearchRightRefList();')
function CallAjaxForDelSel(Url,type,chkPrefix,callBackParaName,divId,divBgId,divParId,inputPrefix,parAppend,dType,jsFunc,isChecked)
{
    var params="";
    if(IsEmpty(dType)) dType='json';
    if(!IsEmpty(callBackParaName)) params="&"+callBackParaName+'='+GetSelect(divId,chkPrefix,isChecked);
    if(!IsEmpty(params))parAppend=parAppend+params;
    CommonCall(Url,type,divId,divBgId,divParId,inputPrefix,parAppend,dType,jsFunc);
}
function ConfirmAndDelSel(Url, type, chkPrefix, callBackParaName, divId, divBgId, divParId, inputPrefix, parAppend, dType, jsFunc, isChecked, confirmMsg, atLeastMsg)
{
   if (atLeastMsg == null)
        atLeastMsg = 'You must select at least one item!', 'Information';
   if (confirmMsg == null)
       confirmMsg = 'Are you sure to delete the select items?'
   if (GetSelect(divId, chkPrefix, isChecked) == '') { MsgBox(atLeastMsg, true, divId); return; } 
   Confirm(confirmMsg,'Information',true,divBgId,'CallAjaxForDelSel(\''+Url+'\','+type+',\''+chkPrefix+'\',\''+callBackParaName+'\',\''+divId+'\',\''+divBgId+'\',\''+divParId+'\',\''+inputPrefix+'\',\''+parAppend+'\',\''+dType+'\',\''+jsFunc+'\','+isChecked+')');
}
function ConfirmAndDelSelCss(content,title,Url,type,chkPrefix,callBackParaName,divId,divBgId,divParId,inputPrefix,parAppend,dType,jsFunc,isChecked)
{
   if(GetSelect(divId,chkPrefix,isChecked)==''){ MsgBox('You must select at least one item!','Information',true,divId);return;} 
   Confirm(content,title,true,divBgId,'CallAjaxForDelSel(\''+Url+'\','+type+',\''+chkPrefix+'\',\''+callBackParaName+'\',\''+divId+'\',\''+divBgId+'\',\''+divParId+'\',\''+inputPrefix+'\',\''+parAppend+'\',\''+dType+'\',\''+jsFunc+'\','+isChecked+')','btn7','btn4');
}
function ValidateTxt(dvForm)
{
    var success=true;
    $('input:text',"#"+dvForm).each(function (i){
        this.className="txt";
        if(isRequired(this) && this.style.display!='none')
        {
            if(this.value==""){this.className="txtrq";success=false;}
        }});
    return success;
}
function ValidateTxtAndSel(dvForm)
{
    var success=true;
    success=ValidateTxt(dvForm)&&ValidateSel(dvForm);    
    return success;
}
function ValidateSel(dvForm)
{
   var success=true;
    $('select',"#"+dvForm).each(function (i){
        this.className="select";
        if(isRequired(this))
        {
            if(this.value==""){this.className="select_rq";success=false;}
        }});
    return success;
}
function SelectAll(chkObj,chkPrefix,divID)
{
    var chks;
    if(IsEmpty(divID))
       chks=$('input:checkbox');
    else
       chks=$('input:checkbox','#'+divID)           
    chks.each(function(i){
    if(IsEmpty(chkPrefix)||this.id.indexOf(chkPrefix,0)>-1)
        {
            this.checked=chkObj.checked;
        }
    });     
}

function GetSelect(divId,chkPrefix,isChecked,separator)
{
    var sel="";
    if(IsEmpty(isChecked)) isChecked=true;
    var chks;
    if(IsEmpty(divId))
       chks=$('input:checkbox');
    else
       chks=$('input:checkbox','#'+divId)          
    chks.each(function(i){
        if(this.checked==isChecked&&this.id.indexOf(chkPrefix,0)>-1) {
           sel+=this.value+(separator==null? ",":separator);
        }
    });
    var l = separator == null ? sel.length - 1 : sel.length - separator.length;
    sel=sel.substr(0,l);
    return encodeURIComponent(sel);
}

function AjaxMsg(title,width,height,serUrl,page,sId,func)
{
    var f=func;
    if(f==null)f=EndRequestAndValidate;
    MsgAjax(title,width,height,serUrl,page,sId,f);
}