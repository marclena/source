Ext.util.CSS=function(){var t=null,n=document,i=/(-[a-z])/gi,r=function(n,t){return t.charAt(1).toUpperCase()};return{createStyleSheet:function(t,i){var u,f=n.getElementsByTagName("head")[0],r=n.createElement("style");if(r.setAttribute("type","text/css"),i&&r.setAttribute("id",i),Ext.isIE)f.appendChild(r),u=r.styleSheet,u.cssText=t;else{try{r.appendChild(n.createTextNode(t))}catch(e){r.cssText=t}f.appendChild(r);u=r.styleSheet?r.styleSheet:r.sheet||n.styleSheets[n.styleSheets.length-1]}return this.cacheStyleSheet(u),u},removeStyleSheet:function(t){var i=n.getElementById(t);i&&i.parentNode.removeChild(i)},swapStyleSheet:function(t,i){this.removeStyleSheet(t);var r=n.createElement("link");r.setAttribute("rel","stylesheet");r.setAttribute("type","text/css");r.setAttribute("id",t);r.setAttribute("href",i);n.getElementsByTagName("head")[0].appendChild(r)},refreshCache:function(){return this.getRules(!0)},cacheStyleSheet:function(n){var r,i;t||(t={});try{for(r=n.cssRules||n.rules,i=r.length-1;i>=0;--i)t[r[i].selectorText]=r[i]}catch(u){}},getRules:function(i){var u,r,f;if(t==null||i)for(t={},u=n.styleSheets,r=0,f=u.length;r<f;r++)try{this.cacheStyleSheet(u[r])}catch(e){}return t},getRule:function(n,t){var r=this.getRules(t),i;if(!(n instanceof Array))return r[n];for(i=0;i<n.length;i++)if(r[n[i]])return r[n[i]];return null},updateRule:function(n,t,u){var e,f;if(n instanceof Array){for(f=0;f<n.length;f++)if(this.updateRule(n[f],t,u))return!0}else if(e=this.getRule(n),e)return e.style[t.replace(i,r)]=u,!0;return!1}}}()