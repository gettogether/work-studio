//C#:public const string JSON_FORMAT = "['{0}','{1}'],";
//var C=[['BBB','BENSON(BBB)'],['BOS','BOSTON(BOS)']];
//new SuggestFromJS('txtDst',null,c,10,C,'Search()','/',true);
//new SuggestFromAjax('txtAirline',null,SerUrl+'Callback.aspx?Type=14','Search()',',',true);
String.prototype.insert=function(index,str){if(index==0){return str+this.substr(index);}else{return this.substring(0,index)+str+this.substr(index);}}
function SuggestFromAjax(objInputId,objCodeID,searchUrl,defaulUrl,listMax,evalCode,separator,isReturnCode,enabledCookie,defaultTitle,onblurFun)
{
    SuggestOnload(objInputId,objCodeID,searchUrl,defaulUrl,null,null,listMax,evalCode,separator,isReturnCode,enabledCookie,defaultTitle,onblurFun);
}
function SuggestFromJS(objInputId,objCodeID,searchList,defaultList,listMax,evalCode,separator,isReturnCode,defaultTitle,onblurFun,defaultWidth)
{
    SuggestOnload(objInputId,objCodeID,null,null,searchList,defaultList,listMax,evalCode,separator,isReturnCode,false,defaultTitle,onblurFun,defaultWidth);
}
function SuggestOnload(oTID,oCID,sUrl,dUrl,sList,dList,lstMax,evalCode,sp,reCode,eCookie,dTitle,onblurFun,defaultWidth)
{
    if(sUrl==null&&sUrl==""&&sList==null){alert("Datasource error.");}
    function getOs(){if(navigator.userAgent.indexOf("MSIE")>0){if(navigator.userAgent.indexOf("MSIE 6")>0)return 1;else return 0};if(isFirefox=navigator.userAgent.indexOf("Firefox")>0)return 2;if(isSafari=navigator.userAgent.indexOf("Safari")>0)return 3;if(isCamino=navigator.userAgent.indexOf("Camino")>0)return 4;if(isMozilla=navigator.userAgent.indexOf("Gecko/")>0)return 5;return 0;}
    var acr = false;
    var isAJ=false;
    var dDef=false;
    var oT=o(oTID);
    var selectedItem=false;
    var HasMatchRec=false;
    if(oT.attributes["enable_popular"]!=null && oT.attributes["enable_popular"].value=='0'){dUrl=null;dList=null;}
    if(dUrl!=null||dList!=null){dDef=true;}
    if(sUrl!=null){isAJ=true;}
    if(isAJ){try {acr = new XMLHttpRequest();} catch (trymicrosoft) {try {acr = new ActiveXObject("Msxml2.XMLHTTP");} catch (othermicrosoft) {try {acr = new ActiveXObject("Microsoft.XMLHTTP");} catch (failed) {acr = false;}}}if (!acr){alert("Error initializing XMLHttpRequest!");isAJ=false;};}
    function ac(url)
    {
        setInputCss(true);
        acr.abort();
        acr.open("post", encodeURI(url+"&"+Math.random()), true);
        acr.onreadystatechange = reac;
        acr.setRequestHeader("Content-Type","application/x-www-form-urlencoded; charset=UTF-8"); 
        acr.send(null);
    }
    var delay;
    var nF="We can not find any results for \'{0}\'.";
    var __cookieID="";
    var defaultCookieID=oTID+"Default";
    var searchCookieID=oTID+"Search";
    var olID="OLAC"+oTID;
    var oLst=o(olID);
    var lsT="";
    if (oT==null){alert('Miss the "'+oTID+'"');return;}
    if(oT.attributes["notfound"]!=null){nF=oT.attributes["notfound"].value;}
    if(oT.attributes["listtitle"]!=null){lsT=oT.attributes["listtitle"].value;}
    oT.setAttribute("autocomplete","off");
    if(oT.style.width!="")
        {oT.style.backgroundPosition=(oT.style.width.replace('px','')-14)+"px";}
    else
        {oT.style.backgroundPosition=(oT.clientWidth-14)+"px";}
    delCK(defaultCookieID);
    delCK(searchCookieID);
    setInputCss(false);
    
    var objCode=null;
    if(oCID!=null || oCID!="")objCode=o(oCID);
    var isOverList=false;
    var idx=-1;
    //var iTmp; 
    var iCount=0;

    oT.onclick=GetList;//oT.ondblclick=GetList;
    oT.onblur=closeSGList;
    oT.onkeyup=checkKeyCode;
    oT.onkeydown=checkEnterKeyCode;
    //oT.onchange=onChanged;
                
    function checkKeyCode(e)
    {
        var k=isIE()?event.keyCode:e.keyCode;
        if (k==40||k==38){}
        else if (k==13){}
        else if (k==33||k==34||k==36||k==35||k==37||k==39||k==20||k==16||k==17||k==18){}
        else{GetList();}
    }
    function checkEnterKeyCode(e)
    {
        var k=isIE()?event.keyCode:e.keyCode;
        if (k==40||k==38)
        { 
            var isUp=false;
            if(k==40){isUp=true;}
            chageSelection(isUp);
        }
        if (k==13)
        {
            outSelection(idx,true);
            if(idx==-1)eval(evalCode);
            idx=-1;
            return false;
        }
        if(k==9&&onblurFun!=null&&onblurFun!=""&&oT.value!="")
        {
            outSelection(0,true);
            return false;
        }
    }
    function getInputValue()
    {
        if(sp==null)return oT.value;
        var s=oT.value;
        if(s.lastIndexOf(sp)!=-1)
            return s.substr(s.lastIndexOf(sp)+1,s.length+1);
        else
            return oT.value;
    }
    function setInputValue(o)
    {
        var s=""
        if(reCode)
            s=o.attributes['code'].nodeValue;
        else
        {
            if(o.attributes['code'].nodeValue!="")
                s=o.attributes['text'].nodeValue;
            else
                s="";
        }
        if(sp!=null)
           return oT.value.substr(0,oT.value.lastIndexOf(sp)+1)+s;
        else        
            return s;
    }
    function GetList()
    {   
        if(oT.value!="")
        {    
            setCode("");
            clearTimeout(delay);
            if(isAJ)
            {
                if(eCookie)
                {
                    if(getCK(searchCookieID)==null || getCK(searchCookieID)=="")
                    {
                        delay=setTimeout(function(){if(oT.value!=""){__cookieID=searchCookieID;ac(sUrl.replace("{0}",oT.value));}},500);
                    }
                    else
                    {
                        displaySGList(getCK(searchCookieID));
                    }
                }
                else
                    delay=setTimeout(function(){if(oT.value!="")ac(sUrl.replace("{0}",oT.value));},500);
            }
            else
                delay=setTimeout(function(){if(oT.value!="")displaySGList(sList);},0);
        }
        else if(dDef)
        {
            if(isAJ)
            {
                if(getCK(defaultCookieID)==null || getCK(defaultCookieID)=="")
                    {delay=setTimeout(function(){__cookieID=defaultCookieID;ac(dUrl);},0);}
                else
                    {displaySGList(getCK(defaultCookieID));}
            }
            else
                displaySGList(dList);
        }
        else{closeSGList();}
    }
    function criFrameShadow(obj)
    {   
        if(getOs()!=1)return;
        var oFID="iFrmaeObj"+oTID;
        if(o(oFID)!=null){document.body.removeChild(o(oFID));}
        if(obj!=null)
        {
            var oF=document.createElement("iframe");
            oF.id=oFID;
            document.body.appendChild(oF);
            var f=o(oFID);
            f.style.cssText="position: absolute;background: #000000;";
            f.style.zIndex=obj.style.zIndex-1;
            f.style.height=obj.clientHeight+2+"px";
            f.style.width=obj.offsetWidth+"px";
            f.style.left=obj.style.left;
            f.style.top=obj.style.top;
        }
        else{if(o(oFID)!=null){document.body.removeChild(o(oFID));}}
    }
    function formatResult(r,isMatch)
    {
        iCount=0;
        var strInput=getInputValue();
        strInput=strInput.toUpperCase();
        var _html="";
        if(isMatch)
        {
            var iListCount=0;
            for(var iTmp=0;iTmp<r.length;iTmp++)
            {
                if(r[iTmp][0].toUpperCase()==strInput)
                {
                    //r[iTmp][1]=r[iTmp][1].replace("("+r[iTmp][0]+")","<b>("+r[iTmp][0]+")</b>");
                    _html="<li class=\"sg_item\" code=\""+r[iTmp][0]+"\" text=\""+r[iTmp][1]+"\" >"+r[iTmp][1].replace("("+r[iTmp][0]+")","<b>("+r[iTmp][0]+")</b>")+"</li>"+_html;
                    iListCount++;
                    iCount++;
                }
                else if(r[iTmp][0].indexOf(strInput)==0 || (isAJ && !eCookie))
                {
                    _html+="<li class=\"sg_item\" code=\""+r[iTmp][0]+"\" text=\""+r[iTmp][1]+"\" >"+r[iTmp][1]+"</li>";
                    iListCount++;
                    iCount++;
                }
                else if(r[iTmp][1].toUpperCase().indexOf(strInput)==0)
                {
                    _html+="<li class=\"sg_item\" code=\""+r[iTmp][0]+"\" text=\""+r[iTmp][1]+"\" >"+r[iTmp][1]+"</li>";
                    iListCount++;
                    iCount++;
                }
                else if(r[iTmp].length>2 && r[iTmp][2].toUpperCase().indexOf(strInput)>=0)
                {
                    _html+="<li class=\"sg_item\" code=\""+r[iTmp][0]+"\" text=\""+r[iTmp][1]+"\" >"+r[iTmp][1]+"</li>";
                    iListCount++;
                    iCount++;
                }
                if(lstMax!=null)
                {
                    if(iListCount==lstMax)
                        break;
                }
            }
        }
        else
        {
            for(var iTmp=0;iTmp<r.length;iTmp++)
            {
                _html+="<li class=\"sg_item\" code=\""+r[iTmp][0]+"\" text=\""+r[iTmp][1]+"\">"+r[iTmp][1]+"</li>";
            }
        }
        if(_html=="")
        {
            HasMatchRec=false;
            _html="<li class=\"sg_item sg_curr_item\" code=\"\" text=\"\" >"+nF.replace("{0}",strInput)+"</li>";
        }
        else
            HasMatchRec=true;
        return _html;
    }
    function formatAjaxResult(result)
    {
        if(eCookie|| __cookieID!=""){setCK(__cookieID,result);__cookieID="";}
        var isMatch=true;
        if(result==null || result=='')result='[]';
        eval('var r='+result);
        return formatResult(r,isMatch);
    }
    function formatJSResult(result)
    {
        if(oT.value!=""){return formatResult(result,true);}
        else{return formatResult(result,false);}
    }
    function displaySGList(result)
    {
        if(document.activeElement.id!=oT.id){setInputCss(false);return;}
        if(o(olID)!=null){document.body.removeChild(o(olID));}
        var olObj=document.createElement("ol");
        var title="";
	    olObj.id=olID;
	    olObj.style.zIndex=900;
	    document.body.appendChild(olObj);
        idx=-1;
        
	    oLst=o(olID);
	    oLst.onmouseover=function(){isOverList=true;};
	    oLst.onmouseout=function(){isOverList=false;};
        oLst.scrollTop=0+"px";
	    oLst.className="suggest_list";
        
        if(dTitle&&oT.value==""){if(lsT!=null && lsT!=""){title="<li class=\"listTitle\" code=\"\" text=\"\" ><b style='color:#3D60A4;'>"+lsT+"</b></li>";}}
        if(isAJ)
            oLst.innerHTML=title+formatAjaxResult(result)
        else
            oLst.innerHTML=title+formatJSResult(result);

        if(oLst.childNodes.length>0)
        {
            for(var i=0;i<oLst.childNodes.length;i++)
            {
                if(title!=""&&i==0){continue;}
                oLst.childNodes[i].onclick=mouseOnClick;
                if(getOs()!=1)
                    oLst.childNodes[i].onmousemove=function(){if(idx!=-1){oLst.childNodes[idx].className="sg_item";}this.className="sg_curr_item";}
                else
                    oLst.childNodes[i].onmouseover=function(){if(idx!=-1){oLst.childNodes[idx].className="sg_item";}this.className="sg_curr_item";}
                oLst.childNodes[i].onmouseout=function(){this.className="sg_item";}
            }

            if(oLst.childNodes.length>10)    
                oLst.style.height=oLst.childNodes[0].offsetHeight*10+"px";
            else
                oLst.style.height=oLst.childNodes[0].offsetHeight*oLst.childNodes.length+"px";
            
            if(oLst.offsetWidth-oLst.clientWidth>5)
                oLst.style.width=oLst.offsetWidth+20+"px";
            else
                oLst.style.width=oLst.offsetWidth+10+"px";
            if(oLst.offsetWidth<oT.offsetWidth)
                oLst.style.width=oT.clientWidth+"px";
            
            if(defaultWidth!=null && HasMatchRec)
                oLst.style.width=defaultWidth+"px";
            oLst.style.left=getAbsoluteLeft(oT)+"px";
            oLst.style.top=getAbsoluteHeight(oT)+getAbsoluteTop(oT)+"px";
            
            criFrameShadow(oLst);
            oLst.style.display='';
            chageSelection(true);
            setInputCss(false);
        }
    }
    function outSelection(Index,closeList)
    {
        if(Index != -1)
        {
            oT.value = setInputValue(oLst.childNodes[Index]);
            setCode(oLst.childNodes[Index].attributes['code'].nodeValue);
            selectedItem=true;
            if(closeList){closeSGList();}
         }
    }
    function mouseOnClick()
    {
        isOverList=false;
        selectedItem=true;
        setCode(this.attributes['code'].nodeValue);
        oT.value=setInputValue(this);
        closeSGList();
    }
//    function onChanged()
//    {
//        if(onblurFun!=null&&onblurFun!=""){eval(onblurFun);}
//        //isOverList=false;
//        //if(oT.value!=""&&HasMatchRec&&selectedItem==false){outSelection(0,false);}
//    }
    function closeSGList(e)
    {   
        if(isOverList){this.focus();}
        else
        {
            if(o(olID)!=null)
            {
                //if(!isIE()){if(oT!=null){oT.focus();}}
                document.body.removeChild(o(olID));
                criFrameShadow(null);
                setInputCss(false);
                if(onblurFun!=null&&onblurFun!=""){eval(onblurFun);}
                selectedItem=true;
            }
            else
            {return true;}
        }
    }
    function chageSelection(isUp)
    {
        if(oLst==null)return;
        if (isUp){idx++;}else{idx--;}

        var maxIndex = oLst.childNodes.length-1;
        if (idx<0){idx=0;}
        if (idx>maxIndex){idx=maxIndex;}
        for (var iTmp=0;iTmp<=maxIndex;iTmp++)
        {
            var objLI=oLst.childNodes[iTmp];
            if (iTmp==idx)
            {
                if(oLst.offsetHeight<oLst.scrollHeight)
                {
                    if(objLI.offsetTop<oLst.scrollTop || objLI.offsetTop>oLst.scrollTop+oLst.clientHeight)
                        oLst.scrollTop=objLI.offsetTop;
                    if(objLI.offsetTop>=oLst.clientHeight+oLst.scrollTop)
                        oLst.scrollTop+=objLI.offsetHeight;
                    if(objLI.offsetTop<oLst.scrollTop)
                        oLst.scrollTop-=objLI.offsetHeight;
                }
                objLI.className="sg_curr_item";
            }
            else if(objLI.className=="sg_curr_item")
            {objLI.className="sg_item";}
        }
    }
    function getAbsoluteHeight(ob){return ob.offsetHeight;}
    function getAbsoluteLeft(ob){var s_el=0;el=ob;while(el){s_el=s_el+el.offsetLeft;el=el.offsetParent;}; return s_el;}
    function getAbsoluteTop(ob){var s_el=0;el=ob;while(el){s_el=s_el+el.offsetTop ;el=el.offsetParent;}; return s_el;}
    function isIE(){return (document.all)? true:false;}
    function setCode(value){if(objCode!=null)objCode.value=value;}
    function setInputCss(isLoading)
    {   
        if(oT==null)return;
        var css=oT.className.replace("sg_txtSearch","").replace("sg_txtLoading","").replace(" ","");
        if(isLoading){oT.className=css+" sg_txtLoading";}
        else{oT.className=css+" sg_txtSearch";}
    }
    function reac(){if(acr.readyState==4){if(acr.status==200){displaySGList(acr.responseText);}else if(acr.status==404){alert("Request URL does not exist");}else if(acr.status==0){}else{alert("Error: status code is "+acr.status);}}}
}
