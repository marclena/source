Ext.Panel=Ext.extend(Ext.Container,{baseCls:"x-panel",collapsedCls:"x-panel-collapsed",maskDisabled:!0,animCollapse:Ext.enableFx,headerAsText:!0,buttonAlign:"right",collapsed:!1,collapseFirst:!0,minButtonWidth:75,elements:"body",toolTarget:"header",collapseEl:"bwrap",slideAnchor:"t",deferHeight:!0,expandDefaults:{duration:.25},collapseDefaults:{duration:.25},initComponent:function(){var t,n,i;if(Ext.Panel.superclass.initComponent.call(this),this.addEvents("bodyresize","titlechange","collapse","expand","beforecollapse","beforeexpand","beforeclose","close","activate","deactivate"),this.tbar&&(this.elements+=",tbar",typeof this.tbar=="object"&&(this.topToolbar=this.tbar),delete this.tbar),this.bbar&&(this.elements+=",bbar",typeof this.bbar=="object"&&(this.bottomToolbar=this.bbar),delete this.bbar),this.header===!0?(this.elements+=",header",delete this.header):this.title&&this.header!==!1&&(this.elements+=",header"),this.footer===!0&&(this.elements+=",footer",delete this.footer),this.buttons)for(t=this.buttons,this.buttons=[],n=0,i=t.length;n<i;n++)t[n].render?this.buttons.push(t[n]):this.addButton(t[n]);if(this.autoLoad)this.on("render",this.doAutoLoad,this,{delay:10})},createElement:function(n,t){if(this[n]){t.appendChild(this[n].dom);return}if(n==="bwrap"||this.elements.indexOf(n)!=-1)if(this[n+"Cfg"])this[n]=Ext.fly(t).createChild(this[n+"Cfg"]);else{var i=document.createElement("div");i.className=this[n+"Cls"];this[n]=Ext.get(t.appendChild(i))}},onRender:function(n,t){var u,o,r,f,i,h,c,l,e,a,v,s;if(Ext.Panel.superclass.onRender.call(this,n,t),this.createClasses(),this.el?(this.el.addClass(this.baseCls),this.header=this.el.down("."+this.headerCls),this.bwrap=this.el.down("."+this.bwrapCls),u=this.bwrap?this.bwrap:this.el,this.tbar=u.down("."+this.tbarCls),this.body=u.down("."+this.bodyCls),this.bbar=u.down("."+this.bbarCls),this.footer=u.down("."+this.footerCls),this.fromMarkup=!0):this.el=n.createChild({id:this.id,cls:this.baseCls},t),o=this.el,r=o.dom,this.cls&&this.el.addClass(this.cls),this.buttons&&(this.elements+=",footer"),this.frame){o.insertHtml("afterBegin",String.format(Ext.Element.boxMarkup,this.baseCls));this.createElement("header",r.firstChild.firstChild.firstChild);this.createElement("bwrap",r);var i=this.bwrap.dom,y=r.childNodes[1],p=r.childNodes[2];i.appendChild(y);i.appendChild(p);f=i.firstChild.firstChild.firstChild;this.createElement("tbar",f);this.createElement("body",f);this.createElement("bbar",f);this.createElement("footer",i.lastChild.firstChild.firstChild);this.footer||(this.bwrap.dom.lastChild.className+=" x-panel-nofooter")}else this.createElement("header",r),this.createElement("bwrap",r),i=this.bwrap.dom,this.createElement("tbar",i),this.createElement("body",i),this.createElement("bbar",i),this.createElement("footer",i),this.header||(this.body.addClass(this.bodyCls+"-noheader"),this.tbar&&this.tbar.addClass(this.tbarCls+"-noheader"));if(this.border===!1&&(this.el.addClass(this.baseCls+"-noborder"),this.body.addClass(this.bodyCls+"-noborder"),this.header&&this.header.addClass(this.headerCls+"-noborder"),this.footer&&this.footer.addClass(this.footerCls+"-noborder"),this.tbar&&this.tbar.addClass(this.tbarCls+"-noborder"),this.bbar&&this.bbar.addClass(this.bbarCls+"-noborder")),this.bodyBorder===!1&&this.body.addClass(this.bodyCls+"-noborder"),this.bodyStyle&&this.body.applyStyles(this.bodyStyle),this.bwrap.enableDisplayMode("block"),this.header&&(this.header.unselectable(),this.headerAsText&&(this.header.dom.innerHTML='<span class="'+this.headerTextCls+'">'+this.header.dom.innerHTML+"<\/span>",this.iconCls&&this.setIconClass(this.iconCls))),this.floating&&this.makeFloating(this.floating),this.collapsible&&(this.tools=this.tools?this.tools.slice(0):[],this.hideCollapseTool||this.tools[this.collapseFirst?"unshift":"push"]({id:"toggle",handler:this.toggleCollapse,scope:this}),this.titleCollapse&&this.header)){this.header.on("click",this.toggleCollapse,this);this.header.setStyle("cursor","pointer")}if(this.tools?(h=this.tools,this.tools={},this.addTool.apply(this,h)):this.tools={},this.buttons&&this.buttons.length>0)for(c=this.footer.createChild({cls:"x-panel-btns-ct",cn:{cls:"x-panel-btns x-panel-btns-"+this.buttonAlign,html:'<table cellspacing="0"><tbody><tr><\/tr><\/tbody><\/table><div class="x-clear"><\/div>'}},null,!0),l=c.getElementsByTagName("tr")[0],e=0,a=this.buttons.length;e<a;e++)v=this.buttons[e],s=document.createElement("td"),s.className="x-panel-btn-td",v.render(l.appendChild(s));this.tbar&&this.topToolbar&&(this.topToolbar instanceof Array&&(this.topToolbar=new Ext.Toolbar(this.topToolbar)),this.topToolbar.render(this.tbar));this.bbar&&this.bottomToolbar&&(this.bottomToolbar instanceof Array&&(this.bottomToolbar=new Ext.Toolbar(this.bottomToolbar)),this.bottomToolbar.render(this.bbar))},setIconClass:function(n){var r=this.iconCls,t,i;this.iconCls=n;this.rendered&&this.header&&(this.frame?(this.header.addClass("x-panel-icon"),this.header.replaceClass(r,this.iconCls)):(t=this.header.dom,i=t.firstChild&&String(t.firstChild.tagName).toLowerCase()=="img"?t.firstChild:null,i?Ext.fly(i).replaceClass(r,this.iconCls):Ext.DomHelper.insertBefore(t.firstChild,{tag:"img",src:Ext.BLANK_IMAGE_URL,cls:"x-panel-inline-icon "+this.iconCls})))},makeFloating:function(n){this.floating=!0;this.el=new Ext.Layer(typeof n=="object"?n:{shadow:this.shadow!==undefined?this.shadow:"sides",shadowOffset:this.shadowOffset,constrain:!1,shim:this.shim===!1?!1:undefined},this.el)},getTopToolbar:function(){return this.topToolbar},getBottomToolbar:function(){return this.bottomToolbar},addButton:function(n,t,i){var r={handler:t,scope:i,minWidth:this.minButtonWidth,hideParent:!0},u;return typeof n=="string"?r.text=n:Ext.apply(r,n),u=new Ext.Button(r),this.buttons||(this.buttons=[]),this.buttons.push(u),u},addTool:function(){var i;if(this[this.toolTarget]){this.toolTemplate||(i=new Ext.Template('<div class="x-tool x-tool-{id}">&#160;<\/div>'),i.disableFormats=!0,i.compile(),Ext.Panel.prototype.toolTemplate=i);for(var r=0,u=arguments,e=u.length;r<e;r++){var n=u[r],f="x-tool-"+n.id+"-over",t=this.toolTemplate.insertFirst(this[this.toolTarget],n,!0);this.tools[n.id]=t;t.enableDisplayMode("block");t.on("click",this.createToolHandler(t,n,f,this));if(n.on)t.on(n.on);n.hidden&&t.hide();n.qtip&&(typeof n.qtip=="object"?Ext.QuickTips.register(Ext.apply({target:t.id},n.qtip)):t.dom.qtip=n.qtip);t.addClassOnOver(f)}}},onShow:function(){if(this.floating)return this.el.show();Ext.Panel.superclass.onShow.call(this)},onHide:function(){if(this.floating)return this.el.hide();Ext.Panel.superclass.onHide.call(this)},createToolHandler:function(n,t,i,r){return function(u){n.removeClass(i);u.stopEvent();t.handler&&t.handler.call(t.scope||n,u,n,r)}},afterRender:function(){if(this.fromMarkup&&this.height===undefined&&!this.autoHeight&&(this.height=this.el.getHeight()),!this.floating||this.hidden||this.initHidden||this.el.show(),this.title&&this.setTitle(this.title),this.setAutoScroll(),this.html&&(this.body.update(typeof this.html=="object"?Ext.DomHelper.markup(this.html):this.html),delete this.html),this.contentEl){var n=Ext.getDom(this.contentEl);Ext.fly(n).removeClass(["x-hidden","x-hide-display"]);this.body.dom.appendChild(n)}this.collapsed&&(this.collapsed=!1,this.collapse(!1));Ext.Panel.superclass.afterRender.call(this);this.initEvents()},setAutoScroll:function(){this.rendered&&this.autoScroll&&this.body.setOverflow("auto")},getKeyMap:function(){return this.keyMap||(this.keyMap=new Ext.KeyMap(this.el,this.keys)),this.keyMap},initEvents:function(){this.keys&&this.getKeyMap();this.draggable&&this.initDraggable()},initDraggable:function(){this.dd=new Ext.Panel.DD(this,typeof this.draggable=="boolean"?null:this.draggable)},beforeEffect:function(){this.floating&&this.el.beforeAction();this.el.addClass("x-panel-animated")},afterEffect:function(){this.syncShadow();this.el.removeClass("x-panel-animated")},createEffect:function(n,t,i){var r={scope:i,block:!0};return n===!0?(r.callback=t,r):(r.callback=n.callback?function(){t.call(i);Ext.callback(n.callback,n.scope)}:t,Ext.applyIf(r,n))},collapse:function(n){if(!this.collapsed&&!this.el.hasFxBlock()&&this.fireEvent("beforecollapse",this,n)!==!1){var t=n===!0||n!==!1&&this.animCollapse;this.beforeEffect();this.onCollapse(t,n);return this}},onCollapse:function(n,t){n?this[this.collapseEl].slideOut(this.slideAnchor,Ext.apply(this.createEffect(t||!0,this.afterCollapse,this),this.collapseDefaults)):(this[this.collapseEl].hide(),this.afterCollapse())},afterCollapse:function(){this.collapsed=!0;this.el.addClass(this.collapsedCls);this.afterEffect();this.fireEvent("collapse",this)},expand:function(n){if(this.collapsed&&!this.el.hasFxBlock()&&this.fireEvent("beforeexpand",this,n)!==!1){var t=n===!0||n!==!1&&this.animCollapse;this.el.removeClass(this.collapsedCls);this.beforeEffect();this.onExpand(t,n);return this}},onExpand:function(n,t){n?this[this.collapseEl].slideIn(this.slideAnchor,Ext.apply(this.createEffect(t||!0,this.afterExpand,this),this.expandDefaults)):(this[this.collapseEl].show(),this.afterExpand())},afterExpand:function(){this.collapsed=!1;this.afterEffect();this.fireEvent("expand",this)},toggleCollapse:function(n){return this[this.collapsed?"expand":"collapse"](n),this},onDisable:function(){this.rendered&&this.maskDisabled&&this.el.mask();Ext.Panel.superclass.onDisable.call(this)},onEnable:function(){this.rendered&&this.maskDisabled&&this.el.unmask();Ext.Panel.superclass.onEnable.call(this)},onResize:function(n,t){if(n!==undefined||t!==undefined){if(this.collapsed){if(this.queuedBodySize={width:n,height:t},!this.queuedExpand&&this.allowQueuedExpand!==!1){this.queuedExpand=!0;this.on("expand",function(){delete this.queuedExpand;this.onResize(this.queuedBodySize.width,this.queuedBodySize.height);this.doLayout()},this,{single:!0})}}else typeof n=="number"?this.body.setWidth(this.adjustBodyWidth(n-this.getFrameWidth())):n=="auto"&&this.body.setWidth(n),typeof t=="number"?this.body.setHeight(this.adjustBodyHeight(t-this.getFrameHeight())):t=="auto"&&this.body.setHeight(t);this.fireEvent("bodyresize",this,n,t)}this.syncShadow()},adjustBodyHeight:function(n){return n},adjustBodyWidth:function(n){return n},onPosition:function(){this.syncShadow()},onDestroy:function(){var n,t;if(this.tools)for(n in this.tools)Ext.destroy(this.tools[n]);if(this.buttons)for(t in this.buttons)Ext.destroy(this.buttons[t]);Ext.destroy(this.topToolbar,this.bottomToolbar);Ext.Panel.superclass.onDestroy.call(this)},getFrameWidth:function(){var n=this.el.getFrameWidth("lr"),t,i;return this.frame&&(t=this.bwrap.dom.firstChild,n+=Ext.fly(t).getFrameWidth("l")+Ext.fly(t.firstChild).getFrameWidth("r"),i=this.bwrap.dom.firstChild.firstChild.firstChild,n+=Ext.fly(i).getFrameWidth("lr")),n},getFrameHeight:function(){var n=this.el.getFrameWidth("tb"),t,i,r;return n+=(this.tbar?this.tbar.getHeight():0)+(this.bbar?this.bbar.getHeight():0),this.frame?(t=this.el.dom.firstChild,i=this.bwrap.dom.lastChild,n+=t.offsetHeight+i.offsetHeight,r=this.bwrap.dom.firstChild.firstChild.firstChild,n+=Ext.fly(r).getFrameWidth("tb")):n+=(this.header?this.header.getHeight():0)+(this.footer?this.footer.getHeight():0),n},getInnerWidth:function(){return this.getSize().width-this.getFrameWidth()},getInnerHeight:function(){return this.getSize().height-this.getFrameHeight()},syncShadow:function(){this.floating&&this.el.sync(!0)},getLayoutTarget:function(){return this.body},setTitle:function(n,t){return this.title=n,this.header&&this.headerAsText&&this.header.child("span").update(n),t&&this.setIconClass(t),this.fireEvent("titlechange",this,n),this},getUpdater:function(){return this.body.getUpdater()},load:function(){var n=this.body.getUpdater();return n.update.apply(n,arguments),this},beforeDestroy:function(){Ext.Element.uncache(this.header,this.tbar,this.bbar,this.footer,this.body)},createClasses:function(){this.headerCls=this.baseCls+"-header";this.headerTextCls=this.baseCls+"-header-text";this.bwrapCls=this.baseCls+"-bwrap";this.tbarCls=this.baseCls+"-tbar";this.bodyCls=this.baseCls+"-body";this.bbarCls=this.baseCls+"-bbar";this.footerCls=this.baseCls+"-footer"},createGhost:function(n,t,i){var r=document.createElement("div"),u;return r.className="x-panel-ghost "+(n?n:""),this.header&&r.appendChild(this.el.dom.firstChild.cloneNode(!0)),Ext.fly(r.appendChild(document.createElement("ul"))).setHeight(this.bwrap.getHeight()),r.style.width=this.el.dom.offsetWidth+"px",i?Ext.getDom(i).appendChild(r):this.container.dom.appendChild(r),t!==!1&&this.el.useShim!==!1?(u=new Ext.Layer({shadow:!1,useDisplay:!0,constrain:!1},r),u.show(),u):new Ext.Element(r)},doAutoLoad:function(){this.body.load(typeof this.autoLoad=="object"?this.autoLoad:{url:this.autoLoad})}});Ext.reg("panel",Ext.Panel)