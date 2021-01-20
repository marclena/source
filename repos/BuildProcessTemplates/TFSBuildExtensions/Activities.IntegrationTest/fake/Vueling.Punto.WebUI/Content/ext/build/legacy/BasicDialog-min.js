Ext.BasicDialog=function(n,t){var r,i;if(this.el=Ext.get(n),r=Ext.DomHelper,!this.el&&t&&t.autoCreate&&(typeof t.autoCreate=="object"?(t.autoCreate.id||(t.autoCreate.id=n),this.el=r.append(document.body,t.autoCreate,!0)):this.el=r.append(document.body,{tag:"div",id:n,style:"visibility:hidden;"},!0)),n=this.el,n.setDisplayed(!0),n.hide=this.hideAction,this.id=n.id,n.addClass("x-dlg"),Ext.apply(this,t),this.proxy=n.createProxy("x-dlg-proxy"),this.proxy.hide=this.hideAction,this.proxy.setOpacity(.5),this.proxy.hide(),t.width&&n.setWidth(t.width),t.height&&n.setHeight(t.height),this.size=n.getSize(),this.xy=typeof t.x!="undefined"&&typeof t.y!="undefined"?[t.x,t.y]:n.getCenterXY(!0),this.header=n.child("> .x-dlg-hd"),this.body=n.child("> .x-dlg-bd"),this.footer=n.child("> .x-dlg-ft"),this.header||(this.header=n.createChild({tag:"div",cls:"x-dlg-hd",html:"&#160;"},this.body?this.body.dom:null)),this.body||(this.body=n.createChild({tag:"div",cls:"x-dlg-bd"})),this.header.unselectable(),this.title&&this.header.update(this.title),this.focusEl=n.createChild({tag:"a",href:"#",cls:"x-dlg-focus",tabIndex:"-1"}),this.focusEl.swallowEvent("click",!0),this.header.wrap({cls:"x-dlg-hd-right"}).wrap({cls:"x-dlg-hd-left"},!0),this.bwrap=this.body.wrap({tag:"div",cls:"x-dlg-dlg-body"}),this.footer&&this.bwrap.dom.appendChild(this.footer.dom),this.bg=this.el.createChild({tag:"div",cls:"x-dlg-bg",html:'<div class="x-dlg-bg-left"><div class="x-dlg-bg-right"><div class="x-dlg-bg-center">&#160;<\/div><\/div><\/div>'}),this.centerBg=this.bg.child("div.x-dlg-bg-center"),this.autoScroll===!1||this.autoTabs||this.body.setStyle("overflow","auto"),this.toolbox=this.el.createChild({cls:"x-dlg-toolbox"}),this.closable!==!1){this.el.addClass("x-dlg-closable");this.close=this.toolbox.createChild({cls:"x-dlg-close"});this.close.on("click",this.closeClick,this);this.close.addClassOnOver("x-dlg-close-over")}if(this.collapsible!==!1){this.collapseBtn=this.toolbox.createChild({cls:"x-dlg-collapse"});this.collapseBtn.on("click",this.collapseClick,this);this.collapseBtn.addClassOnOver("x-dlg-collapse-over");this.header.on("dblclick",this.collapseClick,this)}if(this.resizable!==!1){this.el.addClass("x-dlg-resizable");this.resizer=new Ext.Resizable(n,{minWidth:this.minWidth||80,minHeight:this.minHeight||80,handles:this.resizeHandles||"all",pinned:!0});this.resizer.on("beforeresize",this.beforeResize,this);this.resizer.on("resize",this.onResize,this)}this.draggable!==!1&&(n.addClass("x-dlg-draggable"),i=this.proxyDrag?new Ext.dd.DDProxy(n.dom.id,"WindowDrag",{dragElId:this.proxy.id}):new Ext.dd.DD(n.dom.id,"WindowDrag"),i.setHandleElId(this.header.id),i.endDrag=this.endMove.createDelegate(this),i.startDrag=this.startMove.createDelegate(this),i.onDrag=this.onDrag.createDelegate(this),i.scroll=!1,this.dd=i);this.modal&&(this.mask=r.append(document.body,{tag:"div",cls:"x-dlg-mask"},!0),this.mask.enableDisplayMode("block"),this.mask.hide(),this.el.addClass("x-dlg-modal"));this.shadow?this.shadow=new Ext.Shadow({mode:typeof this.shadow=="string"?this.shadow:"sides",offset:this.shadowOffset}):this.shadowOffset=0;Ext.useShims&&this.shim!==!1?(this.shim=this.el.createShim(),this.shim.hide=this.hideAction,this.shim.hide()):this.shim=!1;this.autoTabs&&this.initTabs();this.addEvents({keydown:!0,move:!0,resize:!0,beforehide:!0,hide:!0,beforeshow:!0,show:!0});n.on("keydown",this.onKeyDown,this);n.on("mousedown",this.toFront,this);Ext.EventManager.onWindowResize(this.adjustViewport,this,!0);this.el.hide();Ext.DialogManager.register(this);Ext.BasicDialog.superclass.constructor.call(this)};Ext.extend(Ext.BasicDialog,Ext.util.Observable,{shadowOffset:Ext.isIE?6:5,minHeight:80,minWidth:200,minButtonWidth:75,defaultButton:null,buttonAlign:"right",tabTag:"div",firstShow:!0,setTitle:function(n){return this.header.update(n),this},closeClick:function(){this.hide()},collapseClick:function(){this[this.collapsed?"expand":"collapse"]()},collapse:function(){this.collapsed||(this.collapsed=!0,this.el.addClass("x-dlg-collapsed"),this.restoreHeight=this.el.getHeight(),this.resizeTo(this.el.getWidth(),this.header.getHeight()))},expand:function(){this.collapsed&&(this.collapsed=!1,this.el.removeClass("x-dlg-collapsed"),this.resizeTo(this.el.getWidth(),this.restoreHeight))},initTabs:function(){for(var n=this.getTabs();n.getTab(0);)n.removeTab(0);return this.el.select(this.tabTag+".x-dlg-tab").each(function(t){var i=t.dom;n.addTab(Ext.id(i),i.title);i.title=""}),n.activate(0),n},beforeResize:function(){this.resizer.minHeight=Math.max(this.minHeight,this.getHeaderFooterHeight(!0)+40)},onResize:function(){this.refreshSize();this.syncBodyHeight();this.adjustAssets();this.focus();this.fireEvent("resize",this,this.size.width,this.size.height)},onKeyDown:function(n){this.isVisible()&&this.fireEvent("keydown",this,n)},resizeTo:function(n,t){return this.el.setSize(n,t),this.size={width:n,height:t},this.syncBodyHeight(),this.fixedcenter&&this.center(),this.isVisible()&&(this.constrainXY(),this.adjustAssets()),this.fireEvent("resize",this,n,t),this},setContentSize:function(n,t){return t+=this.getHeaderFooterHeight()+this.body.getMargins("tb"),n+=this.body.getMargins("lr")+this.bwrap.getMargins("lr")+this.centerBg.getPadding("lr"),t+=this.body.getPadding("tb")+this.bwrap.getBorderWidth("tb")+this.body.getBorderWidth("tb")+this.el.getBorderWidth("tb"),n+=this.body.getPadding("lr")+this.bwrap.getBorderWidth("lr")+this.body.getBorderWidth("lr")+this.bwrap.getPadding("lr")+this.el.getBorderWidth("lr"),this.tabs&&(t+=this.tabs.stripWrap.getHeight()+this.tabs.bodyEl.getMargins("tb")+this.tabs.bodyEl.getPadding("tb"),n+=this.tabs.bodyEl.getMargins("lr")+this.tabs.bodyEl.getPadding("lr")),this.resizeTo(n,t),this},addKeyListener:function(n,t,i){var r,u,f,e,o;typeof n!="object"||n instanceof Array?r=n:(r=n.key,u=n.shift,f=n.ctrl,e=n.alt);o=function(n,o){var s,h,c;if((!u||o.shiftKey)&&(!f||o.ctrlKey)&&(!e||o.altKey))if(s=o.getKey(),r instanceof Array){for(h=0,c=r.length;h<c;h++)if(r[h]==s){t.call(i||window,n,s,o);return}}else s==r&&t.call(i||window,n,s,o)};this.on("keydown",o);return this},getTabs:function(){return this.tabs||(this.el.addClass("x-dlg-auto-tabs"),this.body.addClass(this.tabPosition=="bottom"?"x-tabs-bottom":"x-tabs-top"),this.tabs=new Ext.TabPanel(this.body.dom,this.tabPosition=="bottom")),this.tabs},addButton:function(n,t,i){var e=Ext.DomHelper,f,r,u;return this.footer||(this.footer=e.append(this.bwrap,{tag:"div",cls:"x-dlg-ft"},!0)),this.btnContainer||(f=this.footer.createChild({cls:"x-dlg-btns x-dlg-btns-"+this.buttonAlign,html:'<table cellspacing="0"><tbody><tr><\/tr><\/tbody><\/table><div class="x-clear"><\/div>'},null,!0),this.btnContainer=f.firstChild.firstChild.firstChild),r={handler:t,scope:i,minWidth:this.minButtonWidth,hideParent:!0},typeof n=="string"?r.text=n:n.tag?r.dhconfig=n:Ext.apply(r,n),u=new Ext.Button(r),u.render(this.btnContainer.appendChild(document.createElement("td"))),this.syncBodyHeight(),this.buttons||(this.buttons=[]),this.buttons.push(u),u},setDefaultButton:function(n){return this.defaultButton=n,this},getHeaderFooterHeight:function(){var n=0,t;return this.header&&(n+=this.header.getHeight()),this.footer&&(t=this.footer.getMargins(),n+=this.footer.getHeight()+t.top+t.bottom),n+=this.bwrap.getPadding("tb")+this.bwrap.getBorderWidth("tb"),n+this.centerBg.getPadding("tb")},syncBodyHeight:function(){var i=this.body,n=this.centerBg,t=this.bwrap,f=this.size.height-this.getHeaderFooterHeight(!1),r,u;i.setHeight(f-i.getMargins("tb"));r=this.header.getHeight();u=this.size.height-r;n.setHeight(u);t.setLeftTop(n.getPadding("l"),r+n.getPadding("t"));t.setHeight(u-n.getPadding("tb"));t.setWidth(this.el.getWidth(!0)-n.getPadding("lr"));i.setWidth(t.getWidth(!0));this.tabs&&(this.tabs.syncHeight(),Ext.isIE&&this.tabs.el.repaint())},restoreState:function(){var n=Ext.state.Manager.get(this.stateId||this.el.id+"-state");return n&&n.width&&(this.xy=[n.x,n.y],this.resizeTo(n.width,n.height)),this},beforeShow:function(){this.expand();this.fixedcenter&&(this.xy=this.el.getCenterXY(!0));this.modal&&(Ext.getBody().addClass("x-body-masked"),this.mask.setSize(Ext.lib.Dom.getViewWidth(!0),Ext.lib.Dom.getViewHeight(!0)),this.mask.show());this.constrainXY()},animShow:function(){var n=Ext.get(this.animateTarget,!0).getBox();this.proxy.setSize(n.width,n.height);this.proxy.setLocation(n.x,n.y);this.proxy.show();this.proxy.setBounds(this.xy[0],this.xy[1],this.size.width,this.size.height,!0,.35,this.showEl.createDelegate(this))},show:function(n){if(this.fireEvent("beforeshow",this)!==!1)return this.syncHeightBeforeShow?this.syncBodyHeight():this.firstShow&&(this.firstShow=!1,this.syncBodyHeight()),this.animateTarget=n||this.animateTarget,this.el.isVisible()||(this.beforeShow(),this.animateTarget?this.animShow():this.showEl()),this},showEl:function(){this.proxy.hide();this.el.setXY(this.xy);this.el.show();this.adjustAssets(!0);this.toFront();this.focus();Ext.isIE&&this.el.repaint();this.fireEvent("show",this)},focus:function(){this.defaultButton?this.defaultButton.focus():this.focusEl.focus()},constrainXY:function(){var n;if(this.constraintoviewport!==!1){this.viewSize||(this.container?(n=this.container.getSize(),this.viewSize=[n.width,n.height]):this.viewSize=[Ext.lib.Dom.getViewWidth(),Ext.lib.Dom.getViewHeight()]);var n=Ext.get(this.container||document).getScroll(),t=this.xy[0],i=this.xy[1],u=this.size.width,f=this.size.height,e=this.viewSize[0],o=this.viewSize[1],r=!1;t+u>e+n.left&&(t=e-u,r=!0);i+f>o+n.top&&(i=o-f,r=!0);t<n.left&&(t=n.left,r=!0);i<n.top&&(i=n.top,r=!0);r&&(this.xy=[t,i],this.isVisible()&&(this.el.setLocation(t,i),this.adjustAssets()))}},onDrag:function(){this.proxyDrag||(this.xy=this.el.getXY(),this.adjustAssets())},adjustAssets:function(n){var t=this.xy[0],i=this.xy[1],r=this.size.width,u=this.size.height;n===!0&&(this.shadow&&this.shadow.show(this.el),this.shim&&this.shim.show());this.shadow&&this.shadow.isVisible()&&this.shadow.show(this.el);this.shim&&this.shim.isVisible()&&this.shim.setBounds(t,i,r,u)},adjustViewport:function(n,t){n&&t||(n=Ext.lib.Dom.getViewWidth(),t=Ext.lib.Dom.getViewHeight());this.viewSize=[n,t];this.modal&&this.mask.isVisible()&&(this.mask.setSize(n,t),this.mask.setSize(Ext.lib.Dom.getViewWidth(!0),Ext.lib.Dom.getViewHeight(!0)));this.isVisible()&&this.constrainXY()},destroy:function(n){if(this.isVisible()&&(this.animateTarget=null,this.hide()),Ext.EventManager.removeResizeListener(this.adjustViewport,this),this.tabs&&this.tabs.destroy(n),Ext.destroy(this.shim,this.proxy,this.resizer,this.close,this.mask),this.dd&&this.dd.unreg(),this.buttons)for(var t=0,i=this.buttons.length;t<i;t++)this.buttons[t].destroy();this.el.removeAllListeners();n===!0&&(this.el.update(""),this.el.remove());Ext.DialogManager.unregister(this)},startMove:function(){this.proxyDrag&&this.proxy.show();this.constraintoviewport!==!1&&this.dd.constrainTo(document.body,{right:this.shadowOffset,bottom:this.shadowOffset})},endMove:function(){this.proxyDrag?(Ext.dd.DDProxy.prototype.endDrag.apply(this.dd,arguments),this.proxy.hide()):Ext.dd.DD.prototype.endDrag.apply(this.dd,arguments);this.refreshSize();this.adjustAssets();this.focus();this.fireEvent("move",this,this.xy[0],this.xy[1])},toFront:function(){return Ext.DialogManager.bringToFront(this),this},toBack:function(){return Ext.DialogManager.sendToBack(this),this},center:function(){var n=this.el.getCenterXY(!0);return this.moveTo(n[0],n[1]),this},moveTo:function(n,t){return this.xy=[n,t],this.isVisible()&&(this.el.setXY(this.xy),this.adjustAssets()),this},alignTo:function(n,t,i){return this.xy=this.el.getAlignToXY(n,t,i),this.isVisible()&&(this.el.setXY(this.xy),this.adjustAssets()),this},anchorTo:function(n,t,i,r){var u=function(){this.alignTo(n,t,i)},f;Ext.EventManager.onWindowResize(u,this);if(f=typeof r,f!="undefined")Ext.EventManager.on(window,"scroll",u,this,{buffer:f=="number"?r:50});return u.call(this),this},isVisible:function(){return this.el.isVisible()},animHide:function(n){var t=Ext.get(this.animateTarget).getBox();this.proxy.show();this.proxy.setBounds(this.xy[0],this.xy[1],this.size.width,this.size.height);this.el.hide();this.proxy.setBounds(t.x,t.y,t.width,t.height,!0,.35,this.hideEl.createDelegate(this,[n]))},hide:function(n){if(this.fireEvent("beforehide",this)!==!1)return this.shadow&&this.shadow.hide(),this.shim&&this.shim.hide(),this.animateTarget?this.animHide(n):(this.el.hide(),this.hideEl(n)),this},hideEl:function(n){this.proxy.hide();this.modal&&(this.mask.hide(),Ext.getBody().removeClass("x-body-masked"));this.fireEvent("hide",this);typeof n=="function"&&n()},hideAction:function(){this.setLeft("-10000px");this.setTop("-10000px");this.setStyle("visibility","hidden")},refreshSize:function(){this.size=this.el.getSize();this.xy=this.el.getXY();Ext.state.Manager.set(this.stateId||this.el.id+"-state",this.el.getBox())},setZIndex:function(n){this.modal&&this.mask.setStyle("z-index",n);this.shim&&this.shim.setStyle("z-index",++n);this.shadow&&this.shadow.setZIndex(++n);this.el.setStyle("z-index",++n);this.proxy&&this.proxy.setStyle("z-index",++n);this.resizer&&this.resizer.proxy.setStyle("z-index",++n);this.lastZIndex=n},getEl:function(){return this.el}});Ext.DialogManager=function(){var t={},n=[],i=null,u=function(n,t){return!n._lastAccess||n._lastAccess<t._lastAccess?-1:1},r=function(){var r,t,f,i;for(n.sort(u),r=Ext.DialogManager.zseed,t=0,f=n.length;t<f;t++)i=n[t],i&&i.setZIndex(r+t*10)};return{zseed:9e3,register:function(i){t[i.id]=i;n.push(i)},unregister:function(i){var u,r;if(delete t[i.id],n.indexOf)r=n.indexOf(i),r!=-1&&n.splice(r,1);else for(r=0,u=n.length;r<u;r++)if(n[r]==i){n.splice(r,1);return}},get:function(n){return typeof n=="object"?n:t[n]},bringToFront:function(n){return n=this.get(n),n!=i&&(i=n,n._lastAccess=(new Date).getTime(),r()),n},sendToBack:function(n){return n=this.get(n),n._lastAccess=-(new Date).getTime(),r(),n},hideAll:function(){for(var n in t)t[n]&&typeof t[n]!="function"&&t[n].isVisible()&&t[n].hide()}}}();Ext.LayoutDialog=function(n,t){t.autoTabs=!1;Ext.LayoutDialog.superclass.constructor.call(this,n,t);this.body.setStyle({overflow:"hidden",position:"relative"});this.layout=new Ext.BorderLayout(this.body.dom,t);this.layout.monitorWindowResize=!1;this.el.addClass("x-dlg-auto-layout");this.center=Ext.BasicDialog.prototype.center;this.on("show",this.layout.layout,this.layout,!0)};Ext.extend(Ext.LayoutDialog,Ext.BasicDialog,{endUpdate:function(){this.layout.endUpdate()},beginUpdate:function(){this.layout.beginUpdate()},getLayout:function(){return this.layout},showEl:function(){Ext.LayoutDialog.superclass.showEl.apply(this,arguments);Ext.isIE7&&this.layout.layout()},syncBodyHeight:function(){Ext.LayoutDialog.superclass.syncBodyHeight.call(this);this.layout&&this.layout.layout()}})