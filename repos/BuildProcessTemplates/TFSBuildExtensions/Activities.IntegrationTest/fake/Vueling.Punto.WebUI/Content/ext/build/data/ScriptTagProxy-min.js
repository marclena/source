Ext.data.ScriptTagProxy=function(n){Ext.data.ScriptTagProxy.superclass.constructor.call(this);Ext.apply(this,n);this.head=document.getElementsByTagName("head")[0]};Ext.data.ScriptTagProxy.TRANS_ID=1e3;Ext.extend(Ext.data.ScriptTagProxy,Ext.data.DataProxy,{timeout:3e4,callbackParam:"callback",nocache:!0,load:function(n,t,i,r,u){var h,f,o;if(this.fireEvent("beforeload",this,n)!==!1){h=Ext.urlEncode(Ext.apply(n,this.extraParams));f=this.url;f+=(f.indexOf("?")!=-1?"&":"?")+h;this.nocache&&(f+="&_dc="+(new Date).getTime());var s=++Ext.data.ScriptTagProxy.TRANS_ID,e={id:s,cb:"stcCallback"+s,scriptId:"stcScript"+s,params:n,arg:u,url:f,callback:i,scope:r,reader:t},c=this;window[e.cb]=function(n){c.handleResponse(n,e)};f+=String.format("&{0}={1}",this.callbackParam,e.cb);this.autoAbort!==!1&&this.abort();e.timeoutId=this.handleFailure.defer(this.timeout,this,[e]);o=document.createElement("script");o.setAttribute("src",f);o.setAttribute("type","text/javascript");o.setAttribute("id",e.scriptId);this.head.appendChild(o);this.trans=e}else i.call(r||this,null,u,!1)},isLoading:function(){return this.trans?!0:!1},abort:function(){this.isLoading()&&this.destroyTrans(this.trans)},destroyTrans:function(n,t){if(this.head.removeChild(document.getElementById(n.scriptId)),clearTimeout(n.timeoutId),t){window[n.cb]=undefined;try{delete window[n.cb]}catch(i){}}else window[n.cb]=function(){window[n.cb]=undefined;try{delete window[n.cb]}catch(t){}}},handleResponse:function(n,t){this.trans=!1;this.destroyTrans(t,!0);var i;try{i=t.reader.readRecords(n)}catch(r){this.fireEvent("loadexception",this,n,t.arg,r);t.callback.call(t.scope||window,null,t.arg,!1);return}this.fireEvent("load",this,n,t.arg);t.callback.call(t.scope||window,i,t.arg,!0)},handleFailure:function(n){this.trans=!1;this.destroyTrans(n,!1);this.fireEvent("loadexception",this,null,n.arg);n.callback.call(n.scope||window,null,n.arg,!1)}})