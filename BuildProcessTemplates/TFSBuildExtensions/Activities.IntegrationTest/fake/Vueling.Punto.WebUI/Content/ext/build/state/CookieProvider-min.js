Ext.state.CookieProvider=function(n){Ext.state.CookieProvider.superclass.constructor.call(this);this.path="/";this.expires=new Date((new Date).getTime()+6048e5);this.domain=null;this.secure=!1;Ext.apply(this,n);this.state=this.readCookies()};Ext.extend(Ext.state.CookieProvider,Ext.state.Provider,{set:function(n,t){if(typeof t=="undefined"||t===null){this.clear(n);return}this.setCookie(n,t);Ext.state.CookieProvider.superclass.set.call(this,n,t)},clear:function(n){this.clearCookie(n);Ext.state.CookieProvider.superclass.clear.call(this,n)},readCookies:function(){for(var i={},u=document.cookie+";",f=/\s?(.*?)=(.*?);/g,t,n,r;(t=f.exec(u))!=null;)n=t[1],r=t[2],n&&n.substring(0,3)=="ys-"&&(i[n.substr(3)]=this.decodeValue(r));return i},setCookie:function(n,t){document.cookie="ys-"+n+"="+this.encodeValue(t)+(this.expires==null?"":"; expires="+this.expires.toGMTString())+(this.path==null?"":"; path="+this.path)+(this.domain==null?"":"; domain="+this.domain)+(this.secure==!0?"; secure":"")},clearCookie:function(n){document.cookie="ys-"+n+"=null; expires=Thu, 01-Jan-70 00:00:01 GMT"+(this.path==null?"":"; path="+this.path)+(this.domain==null?"":"; domain="+this.domain)+(this.secure==!0?"; secure":"")}})