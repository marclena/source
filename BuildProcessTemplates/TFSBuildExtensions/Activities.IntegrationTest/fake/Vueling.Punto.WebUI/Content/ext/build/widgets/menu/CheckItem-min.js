Ext.menu.CheckItem=function(n){if(Ext.menu.CheckItem.superclass.constructor.call(this,n),this.addEvents("beforecheckchange","checkchange"),this.checkHandler)this.on("checkchange",this.checkHandler,this.scope);Ext.menu.MenuMgr.registerCheckable(this)};Ext.extend(Ext.menu.CheckItem,Ext.menu.Item,{itemCls:"x-menu-item x-menu-check-item",groupClass:"x-menu-group-item",checked:!1,ctype:"Ext.menu.CheckItem",onRender:function(){Ext.menu.CheckItem.superclass.onRender.apply(this,arguments);this.group&&this.el.addClass(this.groupClass);this.checked&&(this.checked=!1,this.setChecked(!0,!0))},destroy:function(){Ext.menu.MenuMgr.unregisterCheckable(this);Ext.menu.CheckItem.superclass.destroy.apply(this,arguments)},setChecked:function(n,t){this.checked!=n&&this.fireEvent("beforecheckchange",this,n)!==!1&&(this.container&&this.container[n?"addClass":"removeClass"]("x-menu-item-checked"),this.checked=n,t!==!0&&this.fireEvent("checkchange",this,n))},handleClick:function(){this.disabled||this.checked&&this.group||this.setChecked(!this.checked);Ext.menu.CheckItem.superclass.handleClick.apply(this,arguments)}})