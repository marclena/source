Ext.LoadMask=function(n,t){if(this.el=Ext.get(n),Ext.apply(this,t),this.store){this.store.on("beforeload",this.onBeforeLoad,this);this.store.on("load",this.onLoad,this);this.store.on("loadexception",this.onLoad,this);this.removeMask=Ext.value(this.removeMask,!1)}else{var i=this.el.getUpdater();i.showLoadIndicator=!1;i.on("beforeupdate",this.onBeforeLoad,this);i.on("update",this.onLoad,this);i.on("failure",this.onLoad,this);this.removeMask=Ext.value(this.removeMask,!0)}};Ext.LoadMask.prototype={msg:"Loading...",msgCls:"x-mask-loading",disabled:!1,disable:function(){this.disabled=!0},enable:function(){this.disabled=!1},onLoad:function(){this.el.unmask(this.removeMask)},onBeforeLoad:function(){this.disabled||this.el.mask(this.msg,this.msgCls)},show:function(){this.onBeforeLoad()},hide:function(){this.onLoad()},destroy:function(){if(this.store)this.store.un("beforeload",this.onBeforeLoad,this),this.store.un("load",this.onLoad,this),this.store.un("loadexception",this.onLoad,this);else{var n=this.el.getUpdater();n.un("beforeupdate",this.onBeforeLoad,this);n.un("update",this.onLoad,this);n.un("failure",this.onLoad,this)}}}