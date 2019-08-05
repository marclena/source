Ext.Toolbar=function(n){n instanceof Array&&(n={buttons:n});Ext.Toolbar.superclass.constructor.call(this,n)},function(){var n=Ext.Toolbar;Ext.extend(n,Ext.BoxComponent,{trackMenus:!0,initComponent:function(){n.superclass.initComponent.call(this);this.items&&(this.buttons=this.items);this.items=new Ext.util.MixedCollection(!1,function(n){return n.itemId||n.id||Ext.id()})},autoCreate:{cls:"x-toolbar x-small-editor",html:'<table cellspacing="0"><tr><\/tr><\/table>'},onRender:function(n,t){this.el=n.createChild(Ext.apply({id:this.id},this.autoCreate),t);this.tr=this.el.child("tr",!0)},afterRender:function(){n.superclass.afterRender.call(this);this.buttons&&(this.add.apply(this,this.buttons),delete this.buttons)},add:function(){for(var n,i=arguments,r=i.length,t=0;t<r;t++)n=i[t],n.isFormField?this.addField(n):n.render?this.addItem(n):typeof n=="string"?n=="separator"||n=="-"?this.addSeparator():n==" "?this.addSpacer():n=="->"?this.addFill():this.addText(n):n.tagName?this.addElement(n):typeof n=="object"&&(n.xtype?this.addField(Ext.ComponentMgr.create(n,"button")):this.addButton(n))},addSeparator:function(){return this.addItem(new n.Separator)},addSpacer:function(){return this.addItem(new n.Spacer)},addFill:function(){return this.addItem(new n.Fill)},addElement:function(t){return this.addItem(new n.Item(t))},addItem:function(n){var t=this.nextBlock();return this.initMenuTracking(n),n.render(t),this.items.add(n),n},addButton:function(t){var u,r,f,i,e;if(t instanceof Array){for(u=[],r=0,f=t.length;r<f;r++)u.push(this.addButton(t[r]));return u}return i=t,t instanceof n.Button||(i=t.split?new n.SplitButton(t):new n.Button(t)),e=this.nextBlock(),this.initMenuTracking(i),i.render(e),this.items.add(i),i},initMenuTracking:function(n){if(this.trackMenus&&n.menu)n.on({menutriggerover:this.onButtonTriggerOver,menushow:this.onButtonMenuShow,menuhide:this.onButtonMenuHide,scope:this})},addText:function(t){return this.addItem(new n.TextItem(t))},insertButton:function(t,i){var u,r,e,f;if(i instanceof Array){for(u=[],r=0,e=i.length;r<e;r++)u.push(this.insertButton(t+r,i[r]));return u}return i instanceof n.Button||(i=new n.Button(i)),f=document.createElement("td"),this.tr.insertBefore(f,this.tr.childNodes[t]),this.initMenuTracking(i),i.render(f),this.items.insert(t,i),i},addDom:function(t){var r=this.nextBlock(),i;return Ext.DomHelper.overwrite(r,t),i=new n.Item(r.firstChild),i.render(r),this.items.add(i),i},addField:function(t){var r=this.nextBlock(),i;return t.render(r),i=new n.Item(r.firstChild),i.render(r),this.items.add(i),i},nextBlock:function(){var n=document.createElement("td");return this.tr.appendChild(n),n},onDestroy:function(){Ext.Toolbar.superclass.onDestroy.call(this);this.rendered&&(this.items&&Ext.destroy.apply(Ext,this.items.items),Ext.Element.uncache(this.tr))},onDisable:function(){this.items.each(function(n){n.disable&&n.disable()})},onEnable:function(){this.items.each(function(n){n.enable&&n.enable()})},onButtonTriggerOver:function(n){this.activeMenuBtn&&this.activeMenuBtn!=n&&(this.activeMenuBtn.hideMenu(),n.showMenu(),this.activeMenuBtn=n)},onButtonMenuShow:function(n){this.activeMenuBtn=n},onButtonMenuHide:function(){delete this.activeMenuBtn}});Ext.reg("toolbar",Ext.Toolbar);n.Item=function(n){this.el=Ext.getDom(n);this.id=Ext.id(this.el);this.hidden=!1};n.Item.prototype={getEl:function(){return this.el},render:function(n){this.td=n;n.appendChild(this.el)},destroy:function(){this.td&&this.td.parentNode&&this.td.parentNode.removeChild(this.td)},show:function(){this.hidden=!1;this.td.style.display=""},hide:function(){this.hidden=!0;this.td.style.display="none"},setVisible:function(n){n?this.show():this.hide()},focus:function(){Ext.fly(this.el).focus()},disable:function(){Ext.fly(this.td).addClass("x-item-disabled");this.disabled=!0;this.el.disabled=!0},enable:function(){Ext.fly(this.td).removeClass("x-item-disabled");this.disabled=!1;this.el.disabled=!1}};Ext.reg("tbitem",n.Item);n.Separator=function(){var t=document.createElement("span");t.className="ytb-sep";n.Separator.superclass.constructor.call(this,t)};Ext.extend(n.Separator,n.Item,{enable:Ext.emptyFn,disable:Ext.emptyFn,focus:Ext.emptyFn});Ext.reg("tbseparator",n.Separator);n.Spacer=function(){var t=document.createElement("div");t.className="ytb-spacer";n.Spacer.superclass.constructor.call(this,t)};Ext.extend(n.Spacer,n.Item,{enable:Ext.emptyFn,disable:Ext.emptyFn,focus:Ext.emptyFn});Ext.reg("tbspacer",n.Spacer);n.Fill=Ext.extend(n.Spacer,{render:function(t){t.style.width="100%";n.Fill.superclass.render.call(this,t)}});Ext.reg("tbfill",n.Fill);n.TextItem=function(t){var i=document.createElement("span");i.className="ytb-text";i.innerHTML=t.text?t.text:t;n.TextItem.superclass.constructor.call(this,i)};Ext.extend(n.TextItem,n.Item,{enable:Ext.emptyFn,disable:Ext.emptyFn,focus:Ext.emptyFn});Ext.reg("tbtext",n.TextItem);n.Button=Ext.extend(Ext.Button,{hideParent:!0,onDestroy:function(){n.Button.superclass.onDestroy.call(this);this.container&&this.container.remove()}});Ext.reg("tbbutton",n.Button);n.SplitButton=Ext.extend(Ext.SplitButton,{hideParent:!0,onDestroy:function(){n.SplitButton.superclass.onDestroy.call(this);this.container&&this.container.remove()}});Ext.reg("tbsplit",n.SplitButton);n.MenuButton=n.SplitButton}()