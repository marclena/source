Ext.menu.BaseItem=function(n){if(Ext.menu.BaseItem.superclass.constructor.call(this,n),this.addEvents("click","activate","deactivate"),this.handler)this.on("click",this.handler,this.scope)};Ext.extend(Ext.menu.BaseItem,Ext.Component,{canActivate:!1,activeClass:"x-menu-item-active",hideOnClick:!0,hideDelay:100,ctype:"Ext.menu.BaseItem",actionMode:"container",render:function(n,t){this.parentMenu=t;Ext.menu.BaseItem.superclass.render.call(this,n);this.container.menuItemId=this.id},onRender:function(n){this.el=Ext.get(this.el);n.dom.appendChild(this.el.dom)},setHandler:function(n,t){this.handler&&this.un("click",this.handler,this.scope);this.on("click",this.handler=n,this.scope=t)},onClick:function(n){this.disabled||this.fireEvent("click",this,n)===!1||this.parentMenu.fireEvent("itemclick",this,n)===!1?n.stopEvent():this.handleClick(n)},activate:function(){if(this.disabled)return!1;var n=this.container;return n.addClass(this.activeClass),this.region=n.getRegion().adjust(2,2,-2,-2),this.fireEvent("activate",this),!0},deactivate:function(){this.container.removeClass(this.activeClass);this.fireEvent("deactivate",this)},shouldDeactivate:function(n){return!this.region||!this.region.contains(n.getPoint())},handleClick:function(){this.hideOnClick&&this.parentMenu.hide.defer(this.hideDelay,this.parentMenu,[!0])},expandMenu:function(){},hideMenu:function(){}})