Ext.DataView=Ext.extend(Ext.BoxComponent,{selectedClass:"x-view-selected",emptyText:"",last:!1,initComponent:function(){Ext.DataView.superclass.initComponent.call(this);typeof this.tpl=="string"&&(this.tpl=new Ext.XTemplate(this.tpl));this.addEvents("beforeclick","click","containerclick","dblclick","contextmenu","selectionchange","beforeselect");this.all=new Ext.CompositeElementLite;this.selected=new Ext.CompositeElementLite},onRender:function(){this.el||(this.el=document.createElement("div"));Ext.DataView.superclass.onRender.apply(this,arguments)},afterRender:function(){Ext.DataView.superclass.afterRender.call(this);this.el.on({click:this.onClick,dblclick:this.onDblClick,contextmenu:this.onContextMenu,scope:this});if(this.overClass)this.el.on({mouseover:this.onMouseOver,mouseout:this.onMouseOut,scope:this});this.store&&this.setStore(this.store,!0)},refresh:function(){this.clearSelections(!1,!0);this.el.update("");var n=this.store.getRange();if(n.length<1){this.el.update(this.emptyText);this.all.clear();return}this.tpl.overwrite(this.el,this.collectData(n,0));this.all.fill(Ext.query(this.itemSelector,this.el.dom));this.updateIndexes(0)},prepareData:function(n){return n},collectData:function(n,t){for(var r=[],i=0,u=n.length;i<u;i++)r[r.length]=this.prepareData(n[i].data,t+i,n[i]);return r},bufferRender:function(n){var t=document.createElement("div");return this.tpl.overwrite(t,this.collectData(n)),Ext.query(this.itemSelector,t)},onUpdate:function(n,t){var i=this.store.indexOf(t),u=this.isSelected(i),f=this.all.elements[i],r=this.bufferRender([t],i)[0];this.all.replaceElement(i,r,!0);u&&(this.selected.replaceElement(f,r),this.all.item(i).addClass(this.selectedClass));this.updateIndexes(i,i)},onAdd:function(n,t,i){if(this.all.getCount()==0){this.refresh();return}var u=this.bufferRender(t,i),r;i<this.all.getCount()?(r=this.all.item(i).insertSibling(u,"before",!0),this.all.elements.splice(i,0,r)):(r=this.all.last().insertSibling(u,"after",!0),this.all.elements.push(r));this.updateIndexes(i)},onRemove:function(n,t,i){this.deselect(i);this.all.removeElement(i,!0);this.updateIndexes(i)},refreshNode:function(n){this.onUpdate(this.store,this.store.getAt(n))},updateIndexes:function(n,t){var r=this.all.elements,i;for(n=n||0,t=t||(t===0?0:r.length-1),i=n;i<=t;i++)r[i].viewIndex=i},setStore:function(n,t){if(!t&&this.store&&(this.store.un("beforeload",this.onBeforeLoad,this),this.store.un("datachanged",this.refresh,this),this.store.un("add",this.onAdd,this),this.store.un("remove",this.onRemove,this),this.store.un("update",this.onUpdate,this),this.store.un("clear",this.refresh,this)),n){n=Ext.StoreMgr.lookup(n);n.on("beforeload",this.onBeforeLoad,this);n.on("datachanged",this.refresh,this);n.on("add",this.onAdd,this);n.on("remove",this.onRemove,this);n.on("update",this.onUpdate,this);n.on("clear",this.refresh,this)}this.store=n;n&&this.refresh()},findItemFromChild:function(n){return Ext.fly(n).findParent(this.itemSelector,this.el)},onClick:function(n){var t=n.getTarget(this.itemSelector,this.el),i;t?(i=this.indexOf(t),this.onItemClick(t,i,n)!==!1&&this.fireEvent("click",this,i,t,n)):this.fireEvent("containerclick",this,n)!==!1&&this.clearSelections()},onContextMenu:function(n){var t=n.getTarget(this.itemSelector,this.el);t&&this.fireEvent("contextmenu",this,this.indexOf(t),t,n)},onDblClick:function(n){var t=n.getTarget(this.itemSelector,this.el);t&&this.fireEvent("dblclick",this,this.indexOf(t),t,n)},onMouseOver:function(n){var t=n.getTarget(this.itemSelector,this.el);t&&t!==this.lastItem&&(this.lastItem=t,Ext.fly(t).addClass(this.overClass))},onMouseOut:function(n){this.lastItem&&(n.within(this.lastItem,!0)||(Ext.fly(this.lastItem).removeClass(this.overClass),delete this.lastItem))},onItemClick:function(n,t,i){return this.fireEvent("beforeclick",this,t,n,i)===!1?!1:(this.multiSelect?(this.doMultiSelection(n,t,i),i.preventDefault()):this.singleSelect&&(this.doSingleSelection(n,t,i),i.preventDefault()),!0)},doSingleSelection:function(n,t,i){i.ctrlKey&&this.isSelected(t)?this.deselect(t):this.select(t,!1)},doMultiSelection:function(n,t,i){if(i.shiftKey&&this.last!==!1){var r=this.last;this.selectRange(r,t,i.ctrlKey);this.last=r}else(i.ctrlKey||this.simpleSelect)&&this.isSelected(t)?this.deselect(t):this.select(t,i.ctrlKey||i.shiftKey||this.simpleSelect)},getSelectionCount:function(){return this.selected.getCount()},getSelectedNodes:function(){return this.selected.elements},getSelectedIndexes:function(){for(var t=[],i=this.selected.elements,n=0,r=i.length;n<r;n++)t.push(i[n].viewIndex);return t},getSelectedRecords:function(){for(var n=[],i=this.selected.elements,t=0,r=i.length;t<r;t++)n[n.length]=this.store.getAt(i[t].viewIndex);return n},getRecords:function(n){for(var t=[],r=n,i=0,u=r.length;i<u;i++)t[t.length]=this.store.getAt(r[i].viewIndex);return t},getRecord:function(n){return this.store.getAt(n.viewIndex)},clearSelections:function(n,t){(this.multiSelect||this.singleSelect)&&(t||this.selected.removeClass(this.selectedClass),this.selected.clear(),this.last=!1,n||this.fireEvent("selectionchange",this,this.selected.elements))},isSelected:function(n){return this.selected.contains(this.getNode(n))},deselect:function(n){if(this.isSelected(n)){var n=this.getNode(n);this.selected.removeElement(n);this.last==n.viewIndex&&(this.last=!1);Ext.fly(n).removeClass(this.selectedClass);this.fireEvent("selectionchange",this,this.selected.elements)}},select:function(n,t,i){var u,f,r;if(n instanceof Array)for(t||this.clearSelections(!0),u=0,f=n.length;u<f;u++)this.select(n[u],!0,!0);else r=this.getNode(n),t||this.clearSelections(!0),r&&!this.isSelected(r)&&this.fireEvent("beforeselect",this,r,this.selected.elements)!==!1&&(Ext.fly(r).addClass(this.selectedClass),this.selected.add(r),this.last=r.viewIndex,i||this.fireEvent("selectionchange",this,this.selected.elements))},selectRange:function(n,t,i){i||this.clearSelections(!0);this.select(this.getNodes(n,t),!0)},getNode:function(n){return typeof n=="string"?document.getElementById(n):typeof n=="number"?this.all.elements[n]:n},getNodes:function(n,t){var u=this.all.elements,r,i;if(n=n||0,t=typeof t=="undefined"?u.length-1:t,r=[],n<=t)for(i=n;i<=t;i++)r.push(u[i]);else for(i=n;i>=t;i--)r.push(u[i]);return r},indexOf:function(n){return(n=this.getNode(n),typeof n.viewIndex=="number")?n.viewIndex:this.all.indexOf(n)},onBeforeLoad:function(){this.loadingText&&(this.clearSelections(!1,!0),this.el.update('<div class="loading-indicator">'+this.loadingText+"<\/div>"),this.all.clear())}});Ext.reg("dataview",Ext.DataView)