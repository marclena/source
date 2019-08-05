Ext.JsonView=function(n,t,i){Ext.JsonView.superclass.constructor.call(this,n,t,i);var r=this.el.getUpdater();r.setRenderer(this);r.on("update",this.onLoad,this);r.on("failure",this.onLoadException,this);this.addEvents({beforerender:!0,load:!0,loadexception:!0})};Ext.extend(Ext.JsonView,Ext.View,{jsonRoot:"",refresh:function(){var t,n,i,r,u;if(this.clearSelections(),this.el.update(""),t=[],n=this.jsonData,n&&n.length>0)for(i=0,r=n.length;i<r;i++)u=this.prepareData(n[i],i,n),t[t.length]=this.tpl.apply(u);else t.push(this.emptyText);this.el.update(t.join(""));this.nodes=this.el.dom.childNodes;this.updateIndexes(0)},load:function(){var n=this.el.getUpdater();n.update.apply(n,arguments)},render:function(n,t){this.clearSelections();this.el.update("");var i;try{i=Ext.util.JSON.decode(t.responseText);this.jsonRoot&&(i=eval("o."+this.jsonRoot))}catch(r){}this.jsonData=i;this.beforeRender();this.refresh()},getCount:function(){return this.jsonData?this.jsonData.length:0},getNodeData:function(n){var i,t,r;if(n instanceof Array){for(i=[],t=0,r=n.length;t<r;t++)i.push(this.getNodeData(n[t]));return i}return this.jsonData[this.indexOf(n)]||null},beforeRender:function(){this.snapshot=this.jsonData;this.sortInfo&&this.sort.apply(this,this.sortInfo);this.fireEvent("beforerender",this,this.jsonData)},onLoad:function(n,t){this.fireEvent("load",this,this.jsonData,t)},onLoadException:function(n,t){this.fireEvent("loadexception",this,t)},filter:function(n,t){var f,u,o,i,e,r;if(this.jsonData){if(f=[],u=this.snapshot,typeof t=="string"){if(o=t.length,o==0){this.clearFilter();return}for(t=t.toLowerCase(),i=0,e=u.length;i<e;i++)r=u[i],r[n].substr(0,o).toLowerCase()==t&&f.push(r)}else if(t.exec)for(i=0,e=u.length;i<e;i++)r=u[i],t.test(r[n])&&f.push(r);else return;this.jsonData=f;this.refresh()}},filterBy:function(n,t){var r,u,i,e,f;if(this.jsonData){for(r=[],u=this.snapshot,i=0,e=u.length;i<e;i++)f=u[i],n.call(t||this,f)&&r.push(f);this.jsonData=r;this.refresh()}},clearFilter:function(){this.snapshot&&this.jsonData!=this.snapshot&&(this.jsonData=this.snapshot,this.refresh())},sort:function(n,t,i){if(this.sortInfo=Array.prototype.slice.call(arguments,0),this.jsonData){var r=n,u=t&&t.toLowerCase()=="desc",f=function(n,t){var f=i?i(n[r]):n[r],e=i?i(t[r]):t[r];return f<e?u?1:-1:f>e?u?-1:1:0};this.jsonData.sort(f);this.refresh();this.jsonData!=this.snapshot&&this.snapshot.sort(f)}}})