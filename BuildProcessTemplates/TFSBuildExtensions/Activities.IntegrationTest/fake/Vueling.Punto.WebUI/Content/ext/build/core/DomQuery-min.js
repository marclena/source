Ext.DomQuery=function(){function o(n){while((n=n.nextSibling)&&n.nodeType!=1);return n}function s(n){while((n=n.previousSibling)&&n.nodeType!=1);return n}function b(n){var e=++t,r,f,i,u;for(n[0].setAttribute("_nodup",e),r=[n[0]],i=1,u=n.length;i<u;i++)f=n[i],!f.getAttribute("_nodup")!=e&&(f.setAttribute("_nodup",e),r[r.length]=f);for(i=0,u=n.length;i<u;i++)n[i].removeAttribute("_nodup");return r}function k(n){var r,i;if(!n)return[];var s=n.length,o,u,f=n,e,c=-1;if(!s||typeof n.nodeType!="undefined"||s==1)return n;if(h&&typeof n[0].selectSingleNode!="undefined")return b(n);for(r=++t,n[0]._nodup=r,u=1;o=n[u];u++)if(o._nodup!=r)o._nodup=r;else{for(f=[],i=0;i<u;i++)f[++c]=n[i];for(i=u+1;e=n[i];i++)e._nodup!=r&&(e._nodup=r,f[++c]=e);return f}return f}function d(n,i){for(var f,e=++t,r=0,u=n.length;r<u;r++)n[r].setAttribute("_qdiff",e);for(f=[],r=0,u=i.length;r<u;r++)i[r].getAttribute("_qdiff")!=e&&(f[f.length]=i[r]);for(r=0,u=n.length;r<u;r++)n[r].removeAttribute("_qdiff");return f}function g(n,i){var e=n.length,f,u,r,o;if(!e)return i;if(h&&n[0].selectSingleNode)return d(n,i);for(f=++t,r=0;r<e;r++)n[r]._qdiff=f;for(u=[],r=0,o=i.length;r<o;r++)i[r]._qdiff!=f&&(u[u.length]=i[r]);return u}var i={},r={},u={},c=/\S/,n=/^\s+|\s+$/g,l=/\{(\d+)\}/g,e=/^(\s?[\/>+~]\s?|\s|$)/,a=/^(#)?([\w-\*]+)/,v=/(\d*)n\+?(\d*)/,y=/\D/,h=window.ActiveXObject?!0:!1,t;return eval("var batch = 30803;"),t=30803,{getStyle:function(n,t){return Ext.fly(n).getStyle(t)},compile:function(t,i){var o,y,c,p,v;i=i||"select";var u=["var f = function(root){\n var mode; ++batch; var n = root || document;\n"],r=t,w,b=Ext.DomQuery.matchers,k=b.length,s,h=r.match(e);for(h&&h[1]&&(u[u.length]='mode="'+h[1].replace(n,"")+'";',r=r.replace(h[1],""));t.substr(0,1)=="/";)t=t.substr(1);while(r&&w!=r){for(w=r,o=r.match(a),i=="select"?o?(u[u.length]=o[1]=="#"?'n = quickId(n, mode, root, "'+o[2]+'");':'n = getNodes(n, mode, "'+o[2]+'");',r=r.replace(o[0],"")):r.substr(0,1)!="@"&&(u[u.length]='n = getNodes(n, mode, "*");'):o&&(u[u.length]=o[1]=="#"?'n = byId(n, null, "'+o[2]+'");':'n = byTag(n, "'+o[2]+'");',r=r.replace(o[0],""));!(s=r.match(e));){for(y=!1,c=0;c<k;c++)if(p=b[c],v=r.match(p.re),v){u[u.length]=p.select.replace(l,function(n,t){return v[t]});r=r.replace(v[0],"");y=!0;break}if(!y)throw'Error parsing selector, parsing failed at "'+r+'"';}s[1]&&(u[u.length]='mode="'+s[1].replace(n,"")+'";',r=r.replace(s[1],""))}return u[u.length]="return nodup(n);\n}",eval(u.join("")),f},select:function(t,r){var e,f,o,h,u,s;for(r&&r!=document||(r=document),typeof r=="string"&&(r=document.getElementById(r)),e=t.split(","),f=[],o=0,h=e.length;o<h;o++){if(u=e[o].replace(n,""),!i[u]&&(i[u]=Ext.DomQuery.compile(u),!i[u]))throw u+" is not a valid selector";s=i[u](r);s&&s!=document&&(f=f.concat(s))}return e.length>1?k(f):f},selectNode:function(n,t){return Ext.DomQuery.select(n,t)[0]},selectValue:function(t,i,r){var f,e;return t=t.replace(n,""),u[t]||(u[t]=Ext.DomQuery.compile(t,"select")),f=u[t](i),f=f[0]?f[0]:f,e=f&&f.firstChild?f.firstChild.nodeValue:null,e===null||e===undefined||e===""?r:e},selectNumber:function(n,t,i){var r=Ext.DomQuery.selectValue(n,t,i||0);return parseFloat(r)},is:function(n,t){typeof n=="string"&&(n=document.getElementById(n));var i=n instanceof Array,r=Ext.DomQuery.filter(i?n:[n],t);return i?r.length==n.length:r.length>0},filter:function(t,i,u){i=i.replace(n,"");r[i]||(r[i]=Ext.DomQuery.compile(i,"simple"));var f=r[i](t);return u?g(f,t):f},matchers:[{re:/^\.([\w-]+)/,select:'n = byClassName(n, null, " {1} ");'},{re:/^\:([\w-]+)(?:\(((?:[^\s>\/]*|.*?))\))?/,select:'n = byPseudo(n, "{1}", "{2}");'},{re:/^(?:([\[\{])(?:@)?([\w-]+)\s?(?:(=|.=)\s?['"]?(.*?)["']?)?[\]\}])/,select:'n = byAttribute(n, "{2}", "{4}", "{3}", "{1}");'},{re:/^#([\w-]+)/,select:'n = byId(n, null, "{1}");'},{re:/^@([\w-]+)/,select:'return {firstChild:{nodeValue:attrValue(n, "{1}")}};'}],operators:{"=":function(n,t){return n==t},"!=":function(n,t){return n!=t},"^=":function(n,t){return n&&n.substr(0,t.length)==t},"$=":function(n,t){return n&&n.substr(n.length-t.length)==t},"*=":function(n,t){return n&&n.indexOf(t)!==-1},"%=":function(n,t){return n%t==0},"|=":function(n,t){return n&&(n==t||n.substr(0,t.length+1)==t+"-")},"~=":function(n,t){return n&&(" "+n+" ").indexOf(" "+t+" ")!=-1}},pseudos:{"first-child":function(n){for(var i=[],f=-1,t,r=0,u;u=t=n[r];r++){while((t=t.previousSibling)&&t.nodeType!=1);t||(i[++f]=u)}return i},"last-child":function(n){for(var i=[],f=-1,t,r=0,u;u=t=n[r];r++){while((t=t.nextSibling)&&t.nodeType!=1);t||(i[++f]=u)}return i},"nth-child":function(n,t){for(var u,l,r,f=[],o=-1,s=v.exec(t=="even"&&"2n"||t=="odd"&&"2n+1"||!y.test(t)&&"n+"+t||t),h=(s[1]||1)-0,e=s[2]-0,c=0,i;i=n[c];c++){if(u=i.parentNode,batch!=u._batch){for(l=0,r=u.firstChild;r;r=r.nextSibling)r.nodeType==1&&(r.nodeIndex=++l);u._batch=batch}h==1?(e==0||i.nodeIndex==e)&&(f[++o]=i):(i.nodeIndex+e)%h==0&&(f[++o]=i)}return f},"only-child":function(n){for(var i=[],u=-1,r=0,t;t=n[r];r++)s(t)||o(t)||(i[++u]=t);return i},empty:function(n){for(var r=[],o=-1,u=0,t;t=n[u];u++){for(var s=t.childNodes,f=0,i,e=!0;i=s[f];)if(++f,i.nodeType==1||i.nodeType==3){e=!1;break}e&&(r[++o]=t)}return r},contains:function(n,t){for(var r=[],f=-1,u=0,i;i=n[u];u++)(i.textContent||i.innerText||"").indexOf(t)!=-1&&(r[++f]=i);return r},nodeValue:function(n,t){for(var r=[],f=-1,u=0,i;i=n[u];u++)i.firstChild&&i.firstChild.nodeValue==t&&(r[++f]=i);return r},checked:function(n){for(var i=[],u=-1,r=0,t;t=n[r];r++)t.checked==!0&&(i[++u]=t);return i},not:function(n,t){return Ext.DomQuery.filter(n,t,!0)},any:function(n,t){for(var r,o=t.split("|"),u=[],s=-1,f,e=0,i;i=n[e];e++)for(r=0;f=o[r];r++)if(Ext.DomQuery.is(i,f)){u[++s]=i;break}return u},odd:function(n){return this["nth-child"](n,"odd")},even:function(n){return this["nth-child"](n,"even")},nth:function(n,t){return n[t-1]||[]},first:function(n){return n[0]||[]},last:function(n){return n[n.length-1]||[]},has:function(n,t){for(var f=Ext.DomQuery.select,r=[],e=-1,u=0,i;i=n[u];u++)f(t,i).length>0&&(r[++e]=i);return r},next:function(n,t){for(var r,e=Ext.DomQuery.is,u=[],s=-1,f=0,i;i=n[f];f++)r=o(i),r&&e(r,t)&&(u[++s]=i);return u},prev:function(n,t){for(var r,e=Ext.DomQuery.is,u=[],o=-1,f=0,i;i=n[f];f++)r=s(i),r&&e(r,t)&&(u[++o]=i);return u}}}}();Ext.query=Ext.DomQuery.select