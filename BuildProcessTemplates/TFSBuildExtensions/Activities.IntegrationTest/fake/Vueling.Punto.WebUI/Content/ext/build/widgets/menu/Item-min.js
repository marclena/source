Ext.menu.Item=function(n){Ext.menu.Item.superclass.constructor.call(this,n);this.menu&&(this.menu=Ext.menu.MenuMgr.get(this.menu))};Ext.extend(Ext.menu.Item,Ext.menu.BaseItem,{itemCls:"x-menu-item",canActivate:!0,showDelay:200,hideDelay:200,ctype:"Ext.menu.Item",onRender:function(n,t){var i=document.createElement("a");i.hideFocus=!0;i.unselectable="on";i.href=this.href||"#";this.hrefTarget&&(i.target=this.hrefTarget);i.className=this.itemCls+(this.menu?" x-menu-item-arrow":"")+(this.cls?" "+this.cls:"");i.innerHTML=String.format('<img src="{0}" class="x-menu-item-icon {2}" />{1}',this.icon||Ext.BLANK_IMAGE_URL,this.itemText||this.text,this.iconCls||"");this.el=i;Ext.menu.Item.superclass.onRender.call(this,n,t)},setText:function(n){this.text=n;this.rendered&&(this.el.update(String.format('<img src="{0}" class="x-menu-item-icon {2}">{1}',this.icon||Ext.BLANK_IMAGE_URL,this.text,this.iconCls||"")),this.parentMenu.autoWidth())},setIconClass:function(n){var t=this.iconCls;this.iconCls=n;this.rendered&&this.el.child("img.x-menu-item-icon").replaceClass(t,this.iconCls)},handleClick:function(n){this.href||n.stopEvent();Ext.menu.Item.superclass.handleClick.apply(this,arguments)},activate:function(n){return Ext.menu.Item.superclass.activate.apply(this,arguments)&&(this.focus(),n&&this.expandMenu()),!0},shouldDeactivate:function(n){return Ext.menu.Item.superclass.shouldDeactivate.call(this,n)?this.menu&&this.menu.isVisible()?!this.menu.getEl().getRegion().contains(n.getPoint()):!0:!1},deactivate:function(){Ext.menu.Item.superclass.deactivate.apply(this,arguments);this.hideMenu()},expandMenu:function(n){!this.disabled&&this.menu&&(clearTimeout(this.hideTimer),delete this.hideTimer,this.menu.isVisible()||this.showTimer?this.menu.isVisible()&&n&&this.menu.tryActivate(0,1):this.showTimer=this.deferExpand.defer(this.showDelay,this,[n]))},deferExpand:function(n){delete this.showTimer;this.menu.show(this.container,this.parentMenu.subMenuAlign||"tl-tr?",this.parentMenu);n&&this.menu.tryActivate(0,1)},hideMenu:function(){clearTimeout(this.showTimer);delete this.showTimer;!this.hideTimer&&this.menu&&this.menu.isVisible()&&(this.hideTimer=this.deferHide.defer(this.hideDelay,this))},deferHide:function(){delete this.hideTimer;this.menu.hide()}})