/*!
** Unobtrusive Ajax support library for jQuery
** Copyright (C) Microsoft Corporation. All rights reserved.
*/
(function(n){function i(n,t){for(var i=window,r=(n||"").split(".");i&&r.length;)i=i[r.shift()];return typeof i=="function"?i:(t.push(n),Function.constructor.apply(null,t))}function r(n){return n==="GET"||n==="POST"}function e(n,t){r(t)||n.setRequestHeader("X-HTTP-Method-Override",t)}function o(t,i,r){var u;r.indexOf("application/x-javascript")===-1&&(u=(t.getAttribute("data-ajax-mode")||"").toUpperCase(),n(t.getAttribute("data-ajax-update")).each(function(t,r){var f;switch(u){case"BEFORE":f=r.firstChild;n("<div />").html(i).contents().each(function(){r.insertBefore(this,f)});break;case"AFTER":n("<div />").html(i).contents().each(function(){r.appendChild(this)});break;default:n(r).html(i)}}))}function u(t,u){var s,h,f,c;(s=t.getAttribute("data-ajax-confirm"),!s||window.confirm(s))&&(h=n(t.getAttribute("data-ajax-loading")),c=t.getAttribute("data-ajax-loading-duration")||0,n.extend(u,{type:t.getAttribute("data-ajax-method")||undefined,url:t.getAttribute("data-ajax-url")||undefined,beforeSend:function(n){var r;return e(n,f),r=i(t.getAttribute("data-ajax-begin"),["xhr"]).apply(this,arguments),r!==!1&&h.show(c),r},complete:function(){h.hide(c);i(t.getAttribute("data-ajax-complete"),["xhr","status"]).apply(this,arguments)},success:function(n,r,u){o(t,n,u.getResponseHeader("Content-Type")||"text/html");i(t.getAttribute("data-ajax-success"),["data","status","xhr"]).apply(this,arguments)},error:i(t.getAttribute("data-ajax-failure"),["xhr","status","error"])}),u.data.push({name:"X-Requested-With",value:"XMLHttpRequest"}),f=u.type.toUpperCase(),r(f)||(u.type="POST",u.data.push({name:"X-HTTP-Method-Override",value:f})),n.ajax(u))}function s(t){var i=n(t).data(f);return!i||!i.validate||i.validate()}var t="unobtrusiveAjaxClick",f="unobtrusiveValidation";n("a[data-ajax=true]").live("click",function(n){n.preventDefault();u(this,{url:this.href,type:"GET",data:[]})});n("form[data-ajax=true] input[type=image]").live("click",function(i){var r=i.target.name,u=n(i.target),f=u.parents("form")[0],e=u.offset();n(f).data(t,[{name:r+".x",value:Math.round(i.pageX-e.left)},{name:r+".y",value:Math.round(i.pageY-e.top)}]);setTimeout(function(){n(f).removeData(t)},0)});n("form[data-ajax=true] :submit").live("click",function(i){var r=i.target.name,u=n(i.target).parents("form")[0];n(u).data(t,r?[{name:r,value:i.target.value}]:[]);setTimeout(function(){n(u).removeData(t)},0)});n("form[data-ajax=true]").live("submit",function(i){var r=n(this).data(t)||[];(i.preventDefault(),s(this))&&u(this,{url:this.action,type:this.method||"GET",data:r.concat(n(this).serializeArray())})})})(jQuery)