Ext.View=function(n,t,i){this.el=Ext.get(n);typeof t=="string"&&(t=new Ext.Template(t));t.compile();this.tpl=t;Ext.apply(this,i);this.addEvents({beforeclick:!0,click:!0,dblclick:!0,contextmenu:!0,selectionchange:!0,beforeselect:!0});this.el.on({click:this.onClick,dblclick:this.onDblClick,contextmenu:this.onContextMenu,scope:this});this.selections=[];this.nodes=[];this.cmp=new Ext.CompositeElementLite([]);this.store&&this.setStore(this.store,!0);Ext.View.superclass.constructor.call(this)};Ext.extend(Ext.View,Ext.util.Observable,{selectedClass:"x-view-selected",emptyText:"",getEl:function(){return this.el},refresh:function(){var f=this.tpl,i,t,n,r,u;if(this.clearSelections(),this.el.update(""),i=[],t=this.store.getRange(),t.length<1){this.el.update(this.emptyText);return}for(n=0,r=t.length;n<r;n++)u=this.prepareData(t[n].data,n,t[n]),i[i.length]=f.apply(u);this.el.update(i.join(""));this.nodes=this.el.dom.childNodes;this.updateIndexes(0)},prepareData:function(n){return n},onUpdate:function(n,t){this.clearSelections();var i=this.store.indexOf(t),r=this.nodes[i];this.tpl.insertBefore(r,this.prepareData(t.data));r.parentNode.removeChild(r);this.updateIndexes(i,i)},onAdd:function(n,t,i){var u,r,e,f;if(this.clearSelections(),this.nodes.length==0){this.refresh();return}for(u=this.nodes[i],r=0,e=t.length;r<e;r++)f=this.prepareData(t[r].data),u?this.tpl.insertBefore(u,f):this.tpl.append(this.el,f);this.updateIndexes(i)},onRemove:function(n,t,i){this.clearSelections();this.el.dom.removeChild(this.nodes[i]);this.updateIndexes(i)},refreshNode:function(n){this.onUpdate(this.store,this.store.getAt(n))},updateIndexes:function(n,t){var r=this.nodes,i;for(n=n||0,t=t||r.length-1,i=n;i<=t;i++)r[i].nodeIndex=i},setStore:function(n,t){if(!t&&this.store&&(this.store.un("datachanged",this.refresh,this),this.store.un("add",this.onAdd,this),this.store.un("remove",this.onRemove,this),this.store.un("update",this.onUpdate,this),this.store.un("clear",this.refresh,this)),n){n.on("datachanged",this.refresh,this);n.on("add",this.onAdd,this);n.on("remove",this.onRemove,this);n.on("update",this.onUpdate,this);n.on("clear",this.refresh,this)}this.store=n;n&&this.refresh()},findItemFromChild:function(n){var i=this.el.dom,t;if(!n||n.parentNode==i)return n;for(t=n.parentNode;t&&t!=i;){if(t.parentNode==i)return t;t=t.parentNode}return null},onClick:function(n){var t=this.findItemFromChild(n.getTarget()),i;t?(i=this.indexOf(t),this.onItemClick(t,i,n)!==!1&&this.fireEvent("click",this,i,t,n)):this.clearSelections()},onContextMenu:function(n){var t=this.findItemFromChild(n.getTarget());t&&this.fireEvent("contextmenu",this,this.indexOf(t),t,n)},onDblClick:function(n){var t=this.findItemFromChild(n.getTarget());t&&this.fireEvent("dblclick",this,this.indexOf(t),t,n)},onItemClick:function(n,t,i){return this.fireEvent("beforeclick",this,t,n,i)===!1?!1:((this.multiSelect||this.singleSelect)&&(this.multiSelect&&i.shiftKey&&this.lastSelection?this.select(this.getNodes(this.indexOf(this.lastSelection),t),!1):(this.select(n,this.multiSelect&&i.ctrlKey),this.lastSelection=n),i.preventDefault()),!0)},getSelectionCount:function(){return this.selections.length},getSelectedNodes:function(){return this.selections},getSelectedIndexes:function(){for(var t=[],i=this.selections,n=0,r=i.length;n<r;n++)t.push(i[n].nodeIndex);return t},clearSelections:function(n){this.nodes&&(this.multiSelect||this.singleSelect)&&this.selections.length>0&&(this.cmp.elements=this.selections,this.cmp.removeClass(this.selectedClass),this.selections=[],n||this.fireEvent("selectionchange",this,this.selections))},isSelected:function(n){var t=this.selections;return t.length<1?!1:(n=this.getNode(n),t.indexOf(n)!==-1)},select:function(n,t,i){var u,f,r;if(n instanceof Array)for(t||this.clearSelections(!0),u=0,f=n.length;u<f;u++)this.select(n[u],!0,!0);else r=this.getNode(n),r&&!this.isSelected(r)&&(t||this.clearSelections(!0),this.fireEvent("beforeselect",this,r,this.selections)!==!1&&(Ext.fly(r).addClass(this.selectedClass),this.selections.push(r),i||this.fireEvent("selectionchange",this,this.selections)))},getNode:function(n){return typeof n=="string"?document.getElementById(n):typeof n=="number"?this.nodes[n]:n},getNodes:function(n,t){var u=this.nodes,r,i;if(n=n||0,t=typeof t=="undefined"?u.length-1:t,r=[],n<=t)for(i=n;i<=t;i++)r.push(u[i]);else for(i=n;i>=t;i--)r.push(u[i]);return r},indexOf:function(n){var i,t,r;if(n=this.getNode(n),typeof n.nodeIndex=="number")return n.nodeIndex;for(i=this.nodes,t=0,r=i.length;t<r;t++)if(i[t]==n)return t;return-1}})