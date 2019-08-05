Ext.DomHelper=function(){var i=null,c=/^(?:br|frame|hr|img|input|link|meta|range|spacer|wbr|area|param|col)$/i,l=/^table|tbody|tr|td$/i,t=function(n){var i,r,u,e,f,o,s;if(typeof n=="string")return n;i="";n.tag||(n.tag="div");i+="<"+n.tag;for(r in n)if(r!="tag"&&r!="children"&&r!="cn"&&r!="html"&&typeof n[r]!="function")if(r=="style"){if(u=n.style,typeof u=="function"&&(u=u.call()),typeof u=="string")i+=' style="'+u+'"';else if(typeof u=="object"){i+=' style="';for(e in u)typeof u[e]!="function"&&(i+=e+":"+u[e]+";");i+='"'}}else i+=r=="cls"?' class="'+n.cls+'"':r=="htmlFor"?' for="'+n.htmlFor+'"':" "+r+'="'+n[r]+'"';if(c.test(n.tag))i+="/>";else{if(i+=">",f=n.children||n.cn,f)if(f instanceof Array)for(o=0,s=f.length;o<s;o++)i+=t(f[o],i);else i+=t(f,i);n.html&&(i+=n.html);i+="<\/"+n.tag+">"}return i},r=function(n,t){var u=document.createElement(n.tag||"div"),s=u.setAttribute?!0:!1,i,f,e,o;for(i in n)i!="tag"&&i!="children"&&i!="cn"&&i!="html"&&i!="style"&&typeof n[i]!="function"&&(i=="cls"?u.className=n.cls:s?u.setAttribute(i,n[i]):u[i]=n[i]);if(Ext.DomHelper.applyStyles(u,n.style),f=n.children||n.cn,f)if(f instanceof Array)for(e=0,o=f.length;e<o;e++)r(f[e],u);else r(f,u);return n.html&&(u.innerHTML=n.html),t&&t.appendChild(u),u},n=function(n,t,r,u){i.innerHTML=[t,r,u].join("");for(var e=-1,f=i;++e<n;)f=f.firstChild;return f},u="<table>",f="<\/table>",e=u+"<tbody>",o="<\/tbody>"+f,s=e+"<tr>",h="<\/tr>"+o,a=function(t,r,c,l){i||(i=document.createElement("div"));var a,v=null;if(t=="td"){if(r=="afterbegin"||r=="beforeend")return;r=="beforebegin"?(v=c,c=c.parentNode):(v=c.nextSibling,c=c.parentNode);a=n(4,s,l,h)}else if(t=="tr")r=="beforebegin"?(v=c,c=c.parentNode,a=n(3,e,l,o)):r=="afterend"?(v=c.nextSibling,c=c.parentNode,a=n(3,e,l,o)):(r=="afterbegin"&&(v=c.firstChild),a=n(4,s,l,h));else if(t=="tbody")r=="beforebegin"?(v=c,c=c.parentNode,a=n(2,u,l,f)):r=="afterend"?(v=c.nextSibling,c=c.parentNode,a=n(2,u,l,f)):(r=="afterbegin"&&(v=c.firstChild),a=n(3,e,l,o));else{if(r=="beforebegin"||r=="afterend")return;r=="afterbegin"&&(v=c.firstChild);a=n(2,u,l,f)}return c.insertBefore(a,v),a};return{useDom:!1,markup:function(n){return t(n)},applyStyles:function(n,t){var u,i,r;if(t)if(n=Ext.fly(n),typeof t=="string")for(u=/\s?([a-z\-]*)\:\s?([^;]*);?/gi;(i=u.exec(t))!=null;)n.setStyle(i[1],i[2]);else if(typeof t=="object")for(r in t)n.setStyle(r,t[r]);else typeof t=="function"&&Ext.DomHelper.applyStyles(n,t.call())},insertHtml:function(n,t,i){var f,r,u;if(n=n.toLowerCase(),t.insertAdjacentHTML){if(l.test(t.tagName)&&(f=a(t.tagName.toLowerCase(),n,t,i)))return f;switch(n){case"beforebegin":return t.insertAdjacentHTML("BeforeBegin",i),t.previousSibling;case"afterbegin":return t.insertAdjacentHTML("AfterBegin",i),t.firstChild;case"beforeend":return t.insertAdjacentHTML("BeforeEnd",i),t.lastChild;case"afterend":return t.insertAdjacentHTML("AfterEnd",i),t.nextSibling}throw'Illegal insertion point -> "'+n+'"';}r=t.ownerDocument.createRange();switch(n){case"beforebegin":return r.setStartBefore(t),u=r.createContextualFragment(i),t.parentNode.insertBefore(u,t),t.previousSibling;case"afterbegin":return t.firstChild?(r.setStartBefore(t.firstChild),u=r.createContextualFragment(i),t.insertBefore(u,t.firstChild),t.firstChild):(t.innerHTML=i,t.firstChild);case"beforeend":return t.lastChild?(r.setStartAfter(t.lastChild),u=r.createContextualFragment(i),t.appendChild(u),t.lastChild):(t.innerHTML=i,t.lastChild);case"afterend":return r.setStartAfter(t),u=r.createContextualFragment(i),t.parentNode.insertBefore(u,t.nextSibling),t.nextSibling}throw'Illegal insertion point -> "'+n+'"';},insertBefore:function(n,t,i){return this.doInsert(n,t,i,"beforeBegin")},insertAfter:function(n,t,i){return this.doInsert(n,t,i,"afterEnd","nextSibling")},insertFirst:function(n,t,i){return this.doInsert(n,t,i,"afterBegin","firstChild")},doInsert:function(n,i,u,f,e){var o,s;return n=Ext.getDom(n),this.useDom?(o=r(i,null),(e==="firstChild"?n:n.parentNode).insertBefore(o,e?n[e]:n)):(s=t(i),o=this.insertHtml(f,n,s)),u?Ext.get(o,!0):o},append:function(n,i,u){var f,e;return n=Ext.getDom(n),this.useDom?(f=r(i,null),n.appendChild(f)):(e=t(i),f=this.insertHtml("beforeEnd",n,e)),u?Ext.get(f,!0):f},overwrite:function(n,i,r){return n=Ext.getDom(n),n.innerHTML=t(i),r?Ext.get(n.firstChild,!0):n.firstChild},createTemplate:function(n){var i=t(n);return new Ext.Template(i)}}}()