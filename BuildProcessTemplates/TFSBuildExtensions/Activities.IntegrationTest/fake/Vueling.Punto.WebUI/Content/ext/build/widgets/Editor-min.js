Ext.Editor=function(n,t){this.field=n;Ext.Editor.superclass.constructor.call(this,t)};Ext.extend(Ext.Editor,Ext.Component,{value:"",alignment:"c-c?",shadow:"frame",constrain:!1,swallowKeys:!0,completeOnEnter:!1,cancelOnEsc:!1,updateEl:!1,initComponent:function(){Ext.Editor.superclass.initComponent.call(this);this.addEvents("beforestartedit","startedit","beforecomplete","complete","specialkey")},onRender:function(n){this.el=new Ext.Layer({shadow:this.shadow,cls:"x-editor",parentEl:n,shim:this.shim,shadowOffset:4,id:this.id,constrain:this.constrain});this.el.setStyle("overflow",Ext.isGecko?"auto":"hidden");this.field.msgTarget!="title"&&(this.field.msgTarget="qtip");this.field.render(this.el);Ext.isGecko&&this.field.el.dom.setAttribute("autocomplete","off");this.field.on("specialkey",this.onSpecialKey,this);this.swallowKeys&&this.field.el.swallowEvent(["keydown","keypress"]);this.field.show();this.field.on("blur",this.onBlur,this);if(this.field.grow)this.field.on("autosize",this.el.sync,this.el,{delay:1})},onSpecialKey:function(n,t){this.completeOnEnter&&t.getKey()==t.ENTER?(t.stopEvent(),this.completeEdit()):this.cancelOnEsc&&t.getKey()==t.ESC?this.cancelEdit():this.fireEvent("specialkey",n,t)},startEdit:function(n,t){var r,i;if(this.editing&&this.completeEdit(),this.boundEl=Ext.get(n),r=t!==undefined?t:this.boundEl.dom.innerHTML,this.rendered||this.render(this.parentEl||document.body),this.fireEvent("beforestartedit",this,this.boundEl,r)!==!1){if(this.startValue=r,this.field.setValue(r),this.autoSize){i=this.boundEl.getSize();switch(this.autoSize){case"width":this.setSize(i.width,"");break;case"height":this.setSize("",i.height);break;default:this.setSize(i.width,i.height)}}this.el.alignTo(this.boundEl,this.alignment);this.editing=!0;this.show()}},setSize:function(n,t){this.field.setSize(n,t);this.el&&this.el.sync()},realign:function(){this.el.alignTo(this.boundEl,this.alignment)},completeEdit:function(n){if(this.editing){var t=this.getValue();if(this.revertInvalid===!1||this.field.isValid()||(t=this.startValue,this.cancelEdit(!0)),String(t)===String(this.startValue)&&this.ignoreNoChange){this.editing=!1;this.hide();return}this.fireEvent("beforecomplete",this,t,this.startValue)!==!1&&(this.editing=!1,this.updateEl&&this.boundEl&&this.boundEl.update(t),n!==!0&&this.hide(),this.fireEvent("complete",this,t,this.startValue))}},onShow:function(){this.el.show();this.hideEl!==!1&&this.boundEl.hide();this.field.show();Ext.isIE&&!this.fixIEFocus?(this.fixIEFocus=!0,this.deferredFocus.defer(50,this)):this.field.focus();this.fireEvent("startedit",this.boundEl,this.startValue)},deferredFocus:function(){this.editing&&this.field.focus()},cancelEdit:function(n){this.editing&&(this.setValue(this.startValue),n!==!0&&this.hide())},onBlur:function(){this.allowBlur!==!0&&this.editing&&this.completeEdit()},onHide:function(){if(this.editing){this.completeEdit();return}this.field.blur();this.field.collapse&&this.field.collapse();this.el.hide();this.hideEl!==!1&&this.boundEl.show()},setValue:function(n){this.field.setValue(n)},getValue:function(){return this.field.getValue()},beforeDestroy:function(){this.field.destroy();this.field=null}});Ext.reg("editor",Ext.Editor)