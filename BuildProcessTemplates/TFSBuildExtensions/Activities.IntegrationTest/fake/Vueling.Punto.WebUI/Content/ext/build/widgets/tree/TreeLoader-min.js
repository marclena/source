Ext.tree.TreeLoader=function(n){this.baseParams={};this.requestMethod="POST";Ext.apply(this,n);this.addEvents("beforeload","load","loadexception");Ext.tree.TreeLoader.superclass.constructor.call(this)};Ext.extend(Ext.tree.TreeLoader,Ext.util.Observable,{uiProviders:{},clearOnLoad:!0,load:function(n,t){if(this.clearOnLoad)while(n.firstChild)n.removeChild(n.firstChild);this.doPreload(n)?typeof t=="function"&&t():(this.dataUrl||this.url)&&this.requestData(n,t)},doPreload:function(n){var i,t,r,u;if(n.attributes.children){if(n.childNodes.length<1){for(i=n.attributes.children,n.beginUpdate(),t=0,r=i.length;t<r;t++)u=n.appendChild(this.createNode(i[t])),this.preloadChildren&&this.doPreload(u);n.endUpdate()}return!0}return!1},getParams:function(n){var t=[],i=this.baseParams;for(var r in i)typeof i[r]!="function"&&t.push(encodeURIComponent(r),"=",encodeURIComponent(i[r]),"&");return t.push("node=",encodeURIComponent(n.id)),t.join("")},requestData:function(n,t){this.fireEvent("beforeload",this,n,t)!==!1?this.transId=Ext.Ajax.request({method:this.requestMethod,url:this.dataUrl||this.url,success:this.handleResponse,failure:this.handleFailure,scope:this,argument:{callback:t,node:n},params:this.getParams(n)}):typeof t=="function"&&t()},isLoading:function(){return this.transId?!0:!1},abort:function(){this.isLoading()&&Ext.Ajax.abort(this.transId)},createNode:function(n){return this.baseAttrs&&Ext.applyIf(n,this.baseAttrs),this.applyLoader!==!1&&(n.loader=this),typeof n.uiProvider=="string"&&(n.uiProvider=this.uiProviders[n.uiProvider]||eval(n.uiProvider)),n.leaf?new Ext.tree.TreeNode(n):new Ext.tree.AsyncTreeNode(n)},processResponse:function(n,t,i){var o=n.responseText,u,r,e,f;try{for(u=eval("("+o+")"),t.beginUpdate(),r=0,e=u.length;r<e;r++)f=this.createNode(u[r]),f&&t.appendChild(f);t.endUpdate();typeof i=="function"&&i(this,t)}catch(s){this.handleFailure(n)}},handleResponse:function(n){this.transId=!1;var t=n.argument;this.processResponse(n,t.node,t.callback);this.fireEvent("load",this,t.node,n)},handleFailure:function(n){this.transId=!1;var t=n.argument;this.fireEvent("loadexception",this,t.node,n);typeof t.callback=="function"&&t.callback(this,t.node)}})