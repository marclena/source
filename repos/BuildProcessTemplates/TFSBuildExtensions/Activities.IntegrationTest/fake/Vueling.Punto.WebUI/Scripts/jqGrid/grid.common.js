function findPos(n){var t=0,i=0;if(n.offsetParent)do t+=n.offsetLeft,i+=n.offsetTop;while(n=n.offsetParent);return[t,i]}function info_dialog(n,t,i,r){var u={width:290,height:"auto",dataheight:"auto",drag:!0,resize:!1,caption:"<b>"+n+"<\/b>",left:250,top:170,zIndex:1e3,jqModal:!0,modal:!1,closeOnEscape:!0,align:"center",buttonalign:"center",buttons:[]},e,o,f;if(jQuery.extend(u,r||{}),e=u.jqModal,jQuery.fn.jqm&&!e&&(e=!1),o="",u.buttons.length>0)for(f=0;f<u.buttons.length;f++)typeof u.buttons[f].id=="undefined"&&(u.buttons[f].id="info_button_"+f),o+="<a href='javascript:void(0)' id='"+u.buttons[f].id+"' class='fm-button ui-state-default ui-corner-all'>"+u.buttons[f].text+"<\/a>";var h=isNaN(u.dataheight)?u.dataheight:u.dataheight+"px",c="text-align:"+u.align+";",s="<div id='info_id'>";s+="<div id='infocnt' style='margin:0px;padding-bottom:1em;width:100%;overflow:auto;position:relative;height:"+h+";"+c+"'>"+t+"<\/div>";s+=i?"<div class='ui-widget-content ui-helper-clearfix' style='text-align:"+u.buttonalign+";padding-bottom:0.8em;padding-top:0.5em;background-image: none;border-width: 1px 0 0 0;'><a href='javascript:void(0)' id='closedialog' class='fm-button ui-state-default ui-corner-all'>"+i+"<\/a>"+o+"<\/div>":o!=""?"<div class='ui-widget-content ui-helper-clearfix' style='text-align:"+u.buttonalign+";padding-bottom:0.8em;padding-top:0.5em;background-image: none;border-width: 1px 0 0 0;'>"+o+"<\/div>":"";s+="<\/div>";try{jQuery("#info_dialog").attr("aria-hidden")=="false"&&hideModal("#info_dialog",{jqm:e});jQuery("#info_dialog").remove()}catch(l){}createModal({themodal:"info_dialog",modalhead:"info_head",modalcontent:"info_content",scrollelm:"infocnt"},s,u,"","",!0);o&&jQuery.each(u.buttons,function(n){jQuery("#"+this.id,"#info_id").bind("click",function(){return u.buttons[n].onClick.call(jQuery("#info_dialog")),!1})});jQuery("#closedialog","#info_id").click(function(){return hideModal("#info_dialog",{jqm:e}),!1});jQuery(".fm-button","#info_dialog").hover(function(){jQuery(this).addClass("ui-state-hover")},function(){jQuery(this).removeClass("ui-state-hover")});jQuery.isFunction(u.beforeOpen)&&u.beforeOpen();viewModal("#info_dialog",{onHide:function(n){n.w.hide().remove();n.o&&n.o.remove()},modal:u.modal,jqm:e});jQuery.isFunction(u.afterOpen)&&u.afterOpen();try{$("#info_dialog").focus()}catch(l){}}function createEl(n,t,i,r,u){function l(n,t){return jQuery.isFunction(t.dataInit)&&(n.id=t.id,t.dataInit(n),delete t.id,delete t.dataInit),t.dataEvents&&(jQuery.each(t.dataEvents,function(){this.data!==undefined?jQuery(n).bind(this.type,this.data,this.fn):jQuery(n).bind(this.type,this.fn)}),delete t.dataEvents),t}var f="",b,p,s,e,w,k,h,o,a,c,d,y;t.defaultValue&&delete t.defaultValue;switch(n){case"textarea":f=document.createElement("textarea");r?t.cols||jQuery(f).css({width:"98%"}):t.cols||(t.cols=20);t.rows||(t.rows=2);(i=="&nbsp;"||i=="&#160;"||i.length==1&&i.charCodeAt(0)==160)&&(i="");f.value=i;t=l(f,t);jQuery(f).attr(t).attr({role:"textbox",multiline:"true"});break;case"checkbox":if(f=document.createElement("input"),f.type="checkbox",t.value){p=t.value.split(":");i===p[0]&&(f.checked=!0,f.defaultChecked=!0);f.value=p[0];jQuery(f).attr("offval",p[1]);try{delete t.value}catch(v){}}else b=i.toLowerCase(),b.search(/(false|0|no|off|undefined)/i)<0&&b!==""?(f.checked=!0,f.defaultChecked=!0,f.value=i):f.value="on",jQuery(f).attr("offval","off");t=l(f,t);jQuery(f).attr(t).attr("role","checkbox");break;case"select":if(f=document.createElement("select"),f.setAttribute("role","select"),e=[],t.multiple===!0?(s=!0,f.multiple="multiple",$(f).attr("aria-multiselectable","true")):s=!1,typeof t.dataUrl!="undefined")jQuery.ajax(jQuery.extend({url:t.dataUrl,type:"GET",dataType:"html",success:function(n){var r,u;try{delete t.dataUrl;delete t.value}catch(o){}typeof t.buildSelect!="undefined"?(u=t.buildSelect(n),r=jQuery(u).html(),delete t.buildSelect):r=jQuery(n).html();r&&(jQuery(f).append(r),t=l(f,t),typeof t.size=="undefined"&&(t.size=s?3:1),s?(e=i.split(","),e=jQuery.map(e,function(n){return jQuery.trim(n)})):e[0]=jQuery.trim(i),jQuery(f).attr(t),setTimeout(function(){jQuery("option",f).each(function(n){return n===0&&(this.selected=""),$(this).attr("role","option"),(jQuery.inArray(jQuery.trim(jQuery(this).text()),e)>-1||jQuery.inArray(jQuery.trim(jQuery(this).val()),e)>-1)&&(this.selected="selected",!s)?!1:void 0})},0))}},u||{}));else if(t.value){if(s?(e=i.split(","),e=jQuery.map(e,function(n){return jQuery.trim(n)}),typeof t.size=="undefined"&&(t.size=3)):t.size=1,typeof t.value=="function"&&(t.value=t.value()),typeof t.value=="string")for(k=t.value.split(";"),w=0;w<k.length;w++)h=k[w].split(":"),h.length>2&&(h[1]=jQuery.map(h,function(n,t){if(t>0)return n}).join(":")),o=document.createElement("option"),o.setAttribute("role","option"),o.value=h[0],o.innerHTML=h[1],s||jQuery.trim(h[0])!=jQuery.trim(i)&&jQuery.trim(h[1])!=jQuery.trim(i)||(o.selected="selected"),s&&(jQuery.inArray(jQuery.trim(h[1]),e)>-1||jQuery.inArray(jQuery.trim(h[0]),e)>-1)&&(o.selected="selected"),f.appendChild(o);else if(typeof t.value=="object"){a=t.value;for(c in a)a.hasOwnProperty(c)&&(o=document.createElement("option"),o.setAttribute("role","option"),o.value=c,o.innerHTML=a[c],s||jQuery.trim(c)!=jQuery.trim(i)&&jQuery.trim(a[c])!=jQuery.trim(i)||(o.selected="selected"),s&&(jQuery.inArray(jQuery.trim(a[c]),e)>-1||jQuery.inArray(jQuery.trim(c),e)>-1)&&(o.selected="selected"),f.appendChild(o))}t=l(f,t);try{delete t.value}catch(v){}jQuery(f).attr(t)}break;case"text":case"password":case"button":d=n=="button"?"button":"textbox";f=document.createElement("input");f.type=n;f.value=i;t=l(f,t);n!="button"&&(r?t.size||jQuery(f).css({width:"98%"}):t.size||(t.size=20));jQuery(f).attr(t).attr("role",d);break;case"image":case"file":f=document.createElement("input");f.type=n;t=l(f,t);jQuery(f).attr(t);break;case"custom":f=document.createElement("span");try{if(jQuery.isFunction(t.custom_element))if(y=t.custom_element.call(this,i,t),y)y=jQuery(y).addClass("customelement").attr({id:t.id,name:t.name}),jQuery(f).empty().append(y);else throw"e2";else throw"e1";}catch(v){v=="e1"&&info_dialog(jQuery.jgrid.errors.errcap,"function 'custom_element' "+jQuery.jgrid.edit.msg.nodefined,jQuery.jgrid.edit.bClose);v=="e2"?info_dialog(jQuery.jgrid.errors.errcap,"function 'custom_element' "+jQuery.jgrid.edit.msg.novalue,jQuery.jgrid.edit.bClose):info_dialog(jQuery.jgrid.errors.errcap,typeof v=="string"?v:v.message,jQuery.jgrid.edit.bClose)}}return f}function daysInFebruary(n){return n%4==0&&(n%100!=0||n%400==0)?29:28}function DaysArray(n){for(var t=1;t<=n;t++)this[t]=31,(t==4||t==6||t==9||t==11)&&(this[t]=30),t==2&&(this[t]=29);return this}function checkDate(n,t){var i={},h,r,c,l,s;if(n=n.toLowerCase(),h=n.indexOf("/")!=-1?"/":n.indexOf("-")!=-1?"-":n.indexOf(".")!=-1?".":"/",n=n.split(h),t=t.split(h),t.length!=3)return!1;var u=-1,f,e=-1,o=-1;for(r=0;r<n.length;r++)c=isNaN(t[r])?0:parseInt(t[r],10),i[n[r]]=c,f=n[r],f.indexOf("y")!=-1&&(u=r),f.indexOf("m")!=-1&&(o=r),f.indexOf("d")!=-1&&(e=r);return(f=n[u]=="y"||n[u]=="yyyy"?4:n[u]=="yy"?2:-1,l=DaysArray(12),u===-1)?!1:(s=i[n[u]].toString(),f==2&&s.length==1&&(f=1),s.length!=f||i[n[u]]===0&&t[u]!="00")?!1:o===-1?!1:(s=i[n[o]].toString(),s.length<1||i[n[o]]<1||i[n[o]]>12)?!1:e===-1?!1:(s=i[n[e]].toString(),s.length<1||i[n[e]]<1||i[n[e]]>31||i[n[o]]==2&&i[n[e]]>daysInFebruary(i[n[u]])||i[n[e]]>l[i[n[o]]])?!1:!0}function isEmpty(n){return n&&(n.match(/^\s+$/)||n=="")?!0:!1}function checkTime(n){var t;if(!isEmpty(n))if(t=n.match(/^(\d{1,2}):(\d{2})([ap]m)?$/),t){if(t[3]){if(t[1]<1||t[1]>12)return!1}else if(t[1]>23)return!1;if(t[2]>59)return!1}else return!1;return!0}function checkValues(n,t,i){var r,e,u,s,f,o,h;if(typeof t=="string"){for(e=0,len=i.p.colModel.length;e<len;e++)if(i.p.colModel[e].name==t){r=i.p.colModel[e].editrules;t=e;try{u=i.p.colModel[e].formoptions.label}catch(c){}break}}else t>=0&&(r=i.p.colModel[t].editrules);if(r){if(u||(u=i.p.colNames[t]),r.required===!0&&isEmpty(n))return[!1,u+": "+jQuery.jgrid.edit.msg.required,""];if(f=r.required===!1?!1:!0,r.number===!0&&!(f===!1&&isEmpty(n))&&isNaN(n))return[!1,u+": "+jQuery.jgrid.edit.msg.number,""];if(typeof r.minValue!="undefined"&&!isNaN(r.minValue)&&parseFloat(n)<parseFloat(r.minValue))return[!1,u+": "+jQuery.jgrid.edit.msg.minValue+" "+r.minValue,""];if(typeof r.maxValue!="undefined"&&!isNaN(r.maxValue)&&parseFloat(n)>parseFloat(r.maxValue))return[!1,u+": "+jQuery.jgrid.edit.msg.maxValue+" "+r.maxValue,""];if(r.email===!0&&!(f===!1&&isEmpty(n))&&(o=/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i,!o.test(n)))return[!1,u+": "+jQuery.jgrid.edit.msg.email,""];if(r.integer===!0&&!(f===!1&&isEmpty(n))){if(isNaN(n))return[!1,u+": "+jQuery.jgrid.edit.msg.integer,""];if(n%1!=0||n.indexOf(".")!=-1)return[!1,u+": "+jQuery.jgrid.edit.msg.integer,""]}if(r.date===!0&&!(f===!1&&isEmpty(n))&&(s=i.p.colModel[t].formatoptions&&i.p.colModel[t].formatoptions.newformat?i.p.colModel[t].formatoptions.newformat:i.p.colModel[t].datefmt||"Y-m-d",!checkDate(s,n)))return[!1,u+": "+jQuery.jgrid.edit.msg.date+" - "+s,""];if(r.time===!0&&!(f===!1&&isEmpty(n))&&!checkTime(n))return[!1,u+": "+jQuery.jgrid.edit.msg.date+" - hh:mm (am/pm)",""];if(r.url===!0&&!(f===!1&&isEmpty(n))&&(o=/^(((https?)|(ftp)):\/\/([\-\w]+\.)+\w{2,3}(\/[%\-\w]+(\.\w{2,})?)*(([\w\-\.\?\\\/+@&#;`~=%!]*)(\.\w{2,})?)*\/?)/i,!o.test(n)))return[!1,u+": "+jQuery.jgrid.edit.msg.url,""];if(r.custom===!0&&!(f===!1&&isEmpty(n)))return jQuery.isFunction(r.custom_func)?(h=r.custom_func.call(i,n,u),jQuery.isArray(h)?h:[!1,jQuery.jgrid.edit.msg.customarray,""]):[!1,jQuery.jgrid.edit.msg.customfcheck,""]}return[!0,"",""]}var showModal=function(n){n.w.show()},closeModal=function(n){n.w.hide().attr("aria-hidden","true");n.o&&n.o.remove()},hideModal=function(n,t){if(t=jQuery.extend({jqm:!0,gb:""},t||{}),t.onClose){var i=t.onClose(n);if(typeof i=="boolean"&&!i)return}if(jQuery.fn.jqm&&t.jqm===!0)jQuery(n).attr("aria-hidden","true").jqmHide();else{if(t.gb!="")try{jQuery(".jqgrid-overlay:first",t.gb).hide()}catch(r){}jQuery(n).hide().attr("aria-hidden","true")}},createModal=function(n,t,i,r,u,f){var e=document.createElement("div"),a,o,h,c,s,l,v;if(a=jQuery(i.gbox).attr("dir")=="rtl"?!0:!1,e.className="ui-widget ui-widget-content ui-corner-all ui-jqdialog",e.id=n.themodal,o=document.createElement("div"),o.className="ui-jqdialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix",o.id=n.modalhead,jQuery(o).append("<span class='ui-jqdialog-title'>"+i.caption+"<\/span>"),h=jQuery("<a href='javascript:void(0)' class='ui-jqdialog-titlebar-close ui-corner-all'><\/a>").hover(function(){h.addClass("ui-state-hover")},function(){h.removeClass("ui-state-hover")}).append("<span class='ui-icon ui-icon-closethick'><\/span>"),jQuery(o).append(h),a?(e.dir="rtl",jQuery(".ui-jqdialog-title",o).css("float","right"),jQuery(".ui-jqdialog-titlebar-close",o).css("left",.3+"em")):(e.dir="ltr",jQuery(".ui-jqdialog-title",o).css("float","left"),jQuery(".ui-jqdialog-titlebar-close",o).css("right",.3+"em")),c=document.createElement("div"),jQuery(c).addClass("ui-jqdialog-content ui-widget-content").attr("id",n.modalcontent),jQuery(c).append(t),e.appendChild(c),jQuery(e).prepend(o),f===!0?jQuery("body").append(e):jQuery(e).insertBefore(r),typeof i.jqModal=="undefined"&&(i.jqModal=!0),s={},jQuery.fn.jqm&&i.jqModal===!0?(i.left===0&&i.top===0&&(l=[],l=findPos(u),i.left=l[0]+4,i.top=l[1]+4),s.top=i.top+"px",s.left=i.left):(i.left!==0||i.top!==0)&&(s.left=i.left,s.top=i.top+"px"),jQuery("a.ui-jqdialog-titlebar-close",o).click(function(){var t=jQuery("#"+n.themodal).data("onClose")||i.onClose,r=jQuery("#"+n.themodal).data("gbox")||i.gbox;return hideModal("#"+n.themodal,{gb:r,jqm:i.jqModal,onClose:t}),!1}),i.width!==0&&i.width||(i.width=300),i.height!==0&&i.height||(i.height=200),i.zIndex||(i.zIndex=950),v=0,a&&s.left&&!f&&(v=jQuery(i.gbox).width()-(isNaN(i.width)?0:parseInt(i.width,10))-8,s.left=parseInt(s.left,10)+parseInt(v,10)),s.left&&(s.left+="px"),jQuery(e).css(jQuery.extend({width:isNaN(i.width)?"auto":i.width+"px",height:isNaN(i.height)?"auto":i.height+"px",zIndex:i.zIndex,overflow:"hidden"},s)).attr({tabIndex:"-1",role:"dialog","aria-labelledby":n.modalhead,"aria-hidden":"true"}),typeof i.drag=="undefined"&&(i.drag=!0),typeof i.resize=="undefined"&&(i.resize=!0),i.drag)if(jQuery(o).css("cursor","move"),jQuery.fn.jqDrag)jQuery(e).jqDrag(o);else try{jQuery(e).draggable({handle:jQuery("#"+o.id)})}catch(y){}if(i.resize)if(jQuery.fn.jqResize)jQuery(e).append("<div class='jqResize ui-resizable-handle ui-resizable-se ui-icon ui-icon-gripsmall-diagonal-se ui-icon-grip-diagonal-se'><\/div>"),jQuery("#"+n.themodal).jqResize(".jqResize",n.scrollelm?"#"+n.scrollelm:!1);else try{jQuery(e).resizable({handles:"se, sw",alsoResize:n.scrollelm?"#"+n.scrollelm:!1})}catch(y){}i.closeOnEscape===!0&&jQuery(e).keydown(function(t){if(t.which==27){var r=jQuery("#"+n.themodal).data("onClose")||i.onClose;hideModal(this,{gb:i.gbox,jqm:i.jqModal,onClose:r})}})},viewModal=function(n,t){if(t=jQuery.extend({toTop:!0,overlay:10,modal:!1,onShow:showModal,onHide:closeModal,gbox:"",jqm:!0,jqM:!0},t||{}),jQuery.fn.jqm&&t.jqm===!0)t.jqM?jQuery(n).attr("aria-hidden","false").jqm(t).jqmShow():jQuery(n).attr("aria-hidden","false").jqmShow();else{t.gbox!=""&&(jQuery(".jqgrid-overlay:first",t.gbox).show(),jQuery(n).data("gbox",t.gbox));jQuery(n).show().attr("aria-hidden","false");try{jQuery(":input:visible",n)[0].focus()}catch(i){}}}