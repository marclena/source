Ext.tree.TreeEditor=function(n,t){t=t||{};var i=t.events?t:new Ext.form.TextField(t);if(Ext.tree.TreeEditor.superclass.constructor.call(this,i),this.tree=n,n.rendered)this.initEditor(n);else n.on("render",this.initEditor,this)};Ext.extend(Ext.tree.TreeEditor,Ext.Editor,{alignment:"l-l",autoSize:!1,hideEl:!1,cls:"x-small-editor x-tree-editor",shim:!1,shadow:"frame",maxWidth:250,editDelay:350,initEditor:function(n){n.on("beforeclick",this.beforeNodeClick,this);this.on("complete",this.updateNode,this);this.on("beforestartedit",this.fitToTree,this);this.on("startedit",this.bindScroll,this,{delay:10});this.on("specialkey",this.onSpecialKey,this)},fitToTree:function(n,t){var i=this.tree.getTreeEl().dom,r=t.dom,u;i.scrollLeft>r.offsetLeft&&(i.scrollLeft=r.offsetLeft);u=Math.min(this.maxWidth,(i.clientWidth>20?i.clientWidth:i.offsetWidth)-Math.max(0,r.offsetLeft-i.scrollLeft)-5);this.setSize(u,"")},triggerEdit:function(n){this.completeEdit();this.editNode=n;this.startEdit(n.ui.textNode,n.text)},bindScroll:function(){this.tree.getTreeEl().on("scroll",this.cancelEdit,this)},beforeNodeClick:function(n,t){var i=this.lastClick?this.lastClick.getElapsed():0;return this.lastClick=new Date,i>this.editDelay&&this.tree.getSelectionModel().isSelected(n)?(t.stopEvent(),this.triggerEdit(n),!1):void 0},updateNode:function(n,t){this.tree.getTreeEl().un("scroll",this.cancelEdit,this);this.editNode.setText(t)},onHide:function(){Ext.tree.TreeEditor.superclass.onHide.call(this);this.editNode&&this.editNode.ui.focus()},onSpecialKey:function(n,t){var i=t.getKey();i==t.ESC?(t.stopEvent(),this.cancelEdit()):i!=t.ENTER||t.hasModifier()||(t.stopEvent(),this.completeEdit())}})