Ext.XTemplate=function(){var t,o;Ext.XTemplate.superclass.constructor.apply(this,arguments);t=this.html;t=["<tpl>",t,"<\/tpl>"].join("");for(var i,s=0,r=[];i=t.match(/<tpl\b[^>]*>((?:(?=([^<]+))\2|<(?!tpl\b[^>]*>))*?)<\/tpl>/);){var h=i[0].match(/^<tpl\b[^>]*?for="(.*?)"/),f=i[0].match(/^<tpl\b[^>]*?if="(.*?)"/),e=i[0].match(/^<tpl\b[^>]*?exec="(.*?)"/),u=null,c=null,l=null,n=h&&h[1]?h[1]:"";if(f&&(u=f&&f[1]?f[1]:null,u&&(c=new Function("values","parent","xindex","xcount","with(values){ return "+Ext.util.Format.htmlDecode(u)+"; }"))),e&&(u=e&&e[1]?e[1]:null,u&&(l=new Function("values","parent","xindex","xcount","with(values){ "+Ext.util.Format.htmlDecode(u)+"; }"))),n)switch(n){case".":n=new Function("values","parent","with(values){ return values; }");break;case"..":n=new Function("values","parent","with(values){ return parent; }");break;default:n=new Function("values","parent","with(values){ return "+n+"; }")}r.push({id:s,target:n,exec:l,test:c,body:i[1]||""});t=t.replace(i[0],"{xtpl"+s+"}");++s}for(o=r.length-1;o>=0;--o)this.compileTpl(r[o]);this.master=r[r.length-1];this.tpls=r};Ext.extend(Ext.XTemplate,Ext.Template,{re:/\{([\w-\.\#]+)(?:\:([\w\.]*)(?:\((.*?)?\))?)?(\s?[\+\-\*\\]\s?[\d\.\+\-\*\\\(\)]+)?\}/g,codeRe:/\{\[((?:\\\]|.|\n)*?)\]\}/g,applySubTemplate:function(n,t,i,r,u){var f=this.tpls[n],e,s,o,h;if(f.test&&!f.test.call(this,t,i,r,u)||f.exec&&f.exec.call(this,t,i,r,u))return"";if(e=f.target?f.target.call(this,t,i):t,i=f.target?t:i,f.target&&e instanceof Array){for(s=[],o=0,h=e.length;o<h;o++)s[s.length]=f.compiled.call(this,e[o],i,o+1,h);return s.join("")}return f.compiled.call(this,e,i,r,u)},compileTpl:function(n){var e=Ext.util.Format,f=this.disableFormats!==!0,i=Ext.isGecko?"+":",",r=function(n,t,r,u,e){if(t.substr(0,4)=="xtpl")return"'"+i+"this.applySubTemplate("+t.substr(4)+", values, parent, xindex, xcount)"+i+"'";var o;return o=t==="."?"values":t==="#"?"xindex":t.indexOf(".")!=-1?t:"values['"+t+"']",e&&(o="("+o+e+")"),r&&f?(u=u?","+u:"",r.substr(0,5)!="this."?r="fm."+r+"(":(r='this.call("'+r.substr(5)+'", ',u=", values")):(u="",r="("+o+" === undefined ? '' : "),"'"+i+r+o+u+")"+i+"'"},u=function(n,t){return"'"+i+"("+t+")"+i+"'"},t;return Ext.isGecko?t="tpl.compiled = function(values, parent, xindex, xcount){ return '"+n.body.replace(/(\r\n|\n)/g,"\\n").replace(/'/g,"\\'").replace(this.re,r).replace(this.codeRe,u)+"';};":(t=["tpl.compiled = function(values, parent, xindex, xcount){ return ['"],t.push(n.body.replace(/(\r\n|\n)/g,"\\n").replace(/'/g,"\\'").replace(this.re,r).replace(this.codeRe,u)),t.push("'].join('');};"),t=t.join("")),eval(t),this},apply:function(n){return this.master.compiled.call(this,n,{},1,1)},applyTemplate:function(n){return this.master.compiled.call(this,n,{},1,1)},compile:function(){return this}});Ext.XTemplate.from=function(n){return n=Ext.getDom(n),new Ext.XTemplate(n.value||n.innerHTML)}