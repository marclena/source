Ext.WindowGroup=function(){var n={},t=[],i=null,e=function(n,t){return!n._lastAccess||n._lastAccess<t._lastAccess?-1:1},u=function(){var i=t,f=i.length,o,n,u;if(f>0)for(i.sort(e),o=i[0].manager.zseed,n=0;n<f;n++)u=i[n],u&&!u.hidden&&u.setZIndex(o+n*10);r()},f=function(n){n!=i&&(i&&i.setActive(!1),i=n,n&&n.setActive(!0))},r=function(){for(var n=t.length-1;n>=0;--n)if(!t[n].hidden){f(t[n]);return}f(null)};return{zseed:9e3,register:function(i){n[i.id]=i;t.push(i);i.on("hide",r)},unregister:function(i){delete n[i.id];i.un("hide",r);t.remove(i)},get:function(t){return typeof t=="object"?t:n[t]},bringToFront:function(n){return(n=this.get(n),n!=i)?(n._lastAccess=(new Date).getTime(),u(),!0):!1},sendToBack:function(n){return n=this.get(n),n._lastAccess=-(new Date).getTime(),u(),n},hideAll:function(){for(var t in n)n[t]&&typeof n[t]!="function"&&n[t].isVisible()&&n[t].hide()},getActive:function(){return i},getBy:function(n,i){for(var r,f=[],u=t.length-1;u>=0;--u)r=t[u],n.call(i||r,r)!==!1&&f.push(r);return f},each:function(t,i){for(var r in n)if(n[r]&&typeof n[r]!="function"&&t.call(i||n[r],n[r])===!1)return}}};Ext.WindowMgr=new Ext.WindowGroup