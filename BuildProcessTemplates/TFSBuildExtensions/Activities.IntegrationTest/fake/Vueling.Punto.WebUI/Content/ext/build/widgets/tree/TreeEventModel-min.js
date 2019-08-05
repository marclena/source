Ext.tree.TreeEventModel=function(n){this.tree=n;this.tree.on("render",this.initEvents,this)};Ext.tree.TreeEventModel.prototype={initEvents:function(){var n=this.tree.getTreeEl();n.on("click",this.delegateClick,this);n.on("mouseover",this.delegateOver,this);n.on("mouseout",this.delegateOut,this);n.on("dblclick",this.delegateDblClick,this);n.on("contextmenu",this.delegateContextMenu,this)},getNode:function(n){var i,t;return(i=n.getTarget(".x-tree-node-el",10))&&(t=Ext.fly(i,"_treeEvents").getAttributeNS("ext","tree-node-id"),t)?this.tree.getNodeById(t):null},getNodeTarget:function(n){var t=n.getTarget(".x-tree-node-icon",1);return t||(t=n.getTarget(".x-tree-node-el",6)),t},delegateOut:function(n,t){if(this.beforeEvent(n)&&(t=this.getNodeTarget(n),t&&!n.within(t,!0)))this.onNodeOut(n,this.getNode(n))},delegateOver:function(n,t){if(this.beforeEvent(n)&&(t=this.getNodeTarget(n),t))this.onNodeOver(n,this.getNode(n))},delegateClick:function(n){if(this.beforeEvent(n))if(n.getTarget("input[type=checkbox]",1))this.onCheckboxClick(n,this.getNode(n));else if(n.getTarget(".x-tree-ec-icon",1))this.onIconClick(n,this.getNode(n));else if(this.getNodeTarget(n))this.onNodeClick(n,this.getNode(n))},delegateDblClick:function(n){if(this.beforeEvent(n)&&this.getNodeTarget(n))this.onNodeDblClick(n,this.getNode(n))},delegateContextMenu:function(n){if(this.beforeEvent(n)&&this.getNodeTarget(n))this.onNodeContextMenu(n,this.getNode(n))},onNodeClick:function(n,t){t.ui.onClick(n)},onNodeOver:function(n,t){t.ui.onOver(n)},onNodeOut:function(n,t){t.ui.onOut(n)},onIconClick:function(n,t){t.ui.ecClick(n)},onCheckboxClick:function(n,t){t.ui.onCheckChange(n)},onNodeDblClick:function(n,t){t.ui.onDblClick(n)},onNodeContextMenu:function(n,t){t.ui.onContextMenu(n)},beforeEvent:function(n){return this.disabled?(n.stopEvent(),!1):!0},disable:function(){this.disabled=!0},enable:function(){this.disabled=!1}}