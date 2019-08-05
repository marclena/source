Ext.tree.DefaultSelectionModel=function(n){this.selNode=null;this.addEvents("selectionchange","beforeselect");Ext.apply(this,n);Ext.tree.DefaultSelectionModel.superclass.constructor.call(this)};Ext.extend(Ext.tree.DefaultSelectionModel,Ext.util.Observable,{init:function(n){this.tree=n;n.getTreeEl().on("keydown",this.onKeyDown,this);n.on("click",this.onNodeClick,this)},onNodeClick:function(n){this.select(n)},select:function(n){var t=this.selNode;if(t!=n&&this.fireEvent("beforeselect",this,n,t)!==!1){if(t)t.ui.onSelectedChange(!1);this.selNode=n;n.ui.onSelectedChange(!0);this.fireEvent("selectionchange",this,n,t)}return n},unselect:function(n){this.selNode==n&&this.clearSelections()},clearSelections:function(){var n=this.selNode;if(n){n.ui.onSelectedChange(!1);this.selNode=null;this.fireEvent("selectionchange",this,null)}return n},getSelectedNode:function(){return this.selNode},isSelected:function(n){return this.selNode==n},selectPrevious:function(){var i=this.selNode||this.lastSelNode,t,n;if(!i)return null;if(t=i.previousSibling,t){if(!t.isExpanded()||t.childNodes.length<1)return this.select(t);for(n=t.lastChild;n&&n.isExpanded()&&n.childNodes.length>0;)n=n.lastChild;return this.select(n)}return i.parentNode&&(this.tree.rootVisible||!i.parentNode.isRoot)?this.select(i.parentNode):null},selectNext:function(){var n=this.selNode||this.lastSelNode,t;return n?n.firstChild&&n.isExpanded()?this.select(n.firstChild):n.nextSibling?this.select(n.nextSibling):n.parentNode?(t=null,n.parentNode.bubble(function(){if(this.nextSibling)return t=this.getOwnerTree().selModel.select(this.nextSibling),!1}),t):null:null},onKeyDown:function(n){var t=this.selNode||this.lastSelNode,r=this,i;if(t){i=n.getKey();switch(i){case n.DOWN:n.stopEvent();this.selectNext();break;case n.UP:n.stopEvent();this.selectPrevious();break;case n.RIGHT:n.preventDefault();t.hasChildNodes()&&(t.isExpanded()?t.firstChild&&this.select(t.firstChild,n):t.expand());break;case n.LEFT:n.preventDefault();t.hasChildNodes()&&t.isExpanded()?t.collapse():t.parentNode&&(this.tree.rootVisible||t.parentNode!=this.tree.getRootNode())&&this.select(t.parentNode,n)}}}});Ext.tree.MultiSelectionModel=function(n){this.selNodes=[];this.selMap={};this.addEvents("selectionchange");Ext.apply(this,n);Ext.tree.MultiSelectionModel.superclass.constructor.call(this)};Ext.extend(Ext.tree.MultiSelectionModel,Ext.util.Observable,{init:function(n){this.tree=n;n.getTreeEl().on("keydown",this.onKeyDown,this);n.on("click",this.onNodeClick,this)},onNodeClick:function(n,t){this.select(n,t,t.ctrlKey)},select:function(n,t,i){if(i!==!0&&this.clearSelections(!0),this.isSelected(n))return this.lastSelNode=n,n;this.selNodes.push(n);this.selMap[n.id]=n;this.lastSelNode=n;n.ui.onSelectedChange(!0);return this.fireEvent("selectionchange",this,this.selNodes),n},unselect:function(n){if(this.selMap[n.id]){n.ui.onSelectedChange(!1);var i=this.selNodes,t=i.indexOf(n);t!=-1&&this.selNodes.splice(t,1);delete this.selMap[n.id];this.fireEvent("selectionchange",this,this.selNodes)}},clearSelections:function(n){var i=this.selNodes,t,r;if(i.length>0){for(t=0,r=i.length;t<r;t++)i[t].ui.onSelectedChange(!1);this.selNodes=[];this.selMap={};n!==!0&&this.fireEvent("selectionchange",this,this.selNodes)}},isSelected:function(n){return this.selMap[n.id]?!0:!1},getSelectedNodes:function(){return this.selNodes},onKeyDown:Ext.tree.DefaultSelectionModel.prototype.onKeyDown,selectNext:Ext.tree.DefaultSelectionModel.prototype.selectNext,selectPrevious:Ext.tree.DefaultSelectionModel.prototype.selectPrevious})