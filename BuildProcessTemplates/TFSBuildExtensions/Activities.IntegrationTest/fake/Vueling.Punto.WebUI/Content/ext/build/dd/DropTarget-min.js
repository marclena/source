Ext.dd.DropTarget=function(n,t){this.el=Ext.get(n);Ext.apply(this,t);this.containerScroll&&Ext.dd.ScrollManager.register(this.el);Ext.dd.DropTarget.superclass.constructor.call(this,this.el.dom,this.ddGroup||this.group,{isTarget:!0})};Ext.extend(Ext.dd.DropTarget,Ext.dd.DDTarget,{dropAllowed:"x-dd-drop-ok",dropNotAllowed:"x-dd-drop-nodrop",isTarget:!0,isNotifyTarget:!0,notifyEnter:function(){return this.overClass&&this.el.addClass(this.overClass),this.dropAllowed},notifyOver:function(){return this.dropAllowed},notifyOut:function(){this.overClass&&this.el.removeClass(this.overClass)},notifyDrop:function(){return!1}})