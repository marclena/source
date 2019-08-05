Ext.ContentPanel=function(n,t,i){n.autoCreate&&(t=n,n=Ext.id());this.el=Ext.get(n);!this.el&&t&&t.autoCreate&&(typeof t.autoCreate=="object"?(t.autoCreate.id||(t.autoCreate.id=t.id||n),this.el=Ext.DomHelper.append(document.body,t.autoCreate,!0)):this.el=Ext.DomHelper.append(document.body,{tag:"div",cls:"x-layout-inactive-content",id:t.id||n},!0));this.closable=!1;this.loaded=!1;this.active=!1;typeof t=="string"?this.title=t:Ext.apply(this,t);this.resizeEl=this.resizeEl?Ext.get(this.resizeEl,!0):this.el;this.addEvents({activate:!0,deactivate:!0,resize:!0});this.autoScroll&&this.resizeEl.setStyle("overflow","auto");i=i||this.content;i&&this.setContent(i);t&&t.url&&this.setUrl(this.url,this.params,this.loadOnce);Ext.ContentPanel.superclass.constructor.call(this)};Ext.extend(Ext.ContentPanel,Ext.util.Observable,{tabTip:"",setRegion:function(n){this.region=n;n?this.el.replaceClass("x-layout-inactive-content","x-layout-active-content"):this.el.replaceClass("x-layout-active-content","x-layout-inactive-content")},getToolbar:function(){return this.toolbar},setActiveState:function(n){this.active=n;n?this.fireEvent("activate",this):this.fireEvent("deactivate",this)},setContent:function(n,t){this.el.update(n,t)},ignoreResize:function(n,t){return this.lastSize&&this.lastSize.width==n&&this.lastSize.height==t?!0:(this.lastSize={width:n,height:t},!1)},getUpdater:function(){return this.el.getUpdater()},load:function(){return this.el.load.apply(this.el,arguments),this},setUrl:function(n,t,i){this.refreshDelegate&&this.removeListener("activate",this.refreshDelegate);this.refreshDelegate=this._handleRefresh.createDelegate(this,[n,t,i]);this.on("activate",this.refreshDelegate);return this.el.getUpdater()},_handleRefresh:function(n,t,i){if(!i||!this.loaded){var r=this.el.getUpdater();r.update(n,t,this._setLoaded.createDelegate(this))}},_setLoaded:function(){this.loaded=!0},getId:function(){return this.el.id},getEl:function(){return this.el},adjustForComponents:function(n,t){if(this.resizeEl!=this.el&&(n-=this.el.getFrameWidth("lr"),t-=this.el.getFrameWidth("tb")),this.toolbar){var i=this.toolbar.getEl();t-=i.getHeight();i.setWidth(n)}return this.adjustments&&(n+=this.adjustments[0],t+=this.adjustments[1]),{width:n,height:t}},setSize:function(n,t){if(this.fitToFrame&&!this.ignoreResize(n,t)){this.fitContainer&&this.resizeEl!=this.el&&this.el.setSize(n,t);var i=this.adjustForComponents(n,t);this.resizeEl.setSize(this.autoWidth?"auto":i.width,this.autoHeight?"auto":i.height);this.fireEvent("resize",this,i.width,i.height)}},getTitle:function(){return this.title},setTitle:function(n){this.title=n;this.region&&this.region.updatePanelTitle(this,n)},isClosable:function(){return this.closable},beforeSlide:function(){this.el.clip();this.resizeEl.clip()},afterSlide:function(){this.el.unclip();this.resizeEl.unclip()},refresh:function(){this.refreshDelegate&&(this.loaded=!1,this.refreshDelegate())},destroy:function(){this.el.removeAllListeners();var n=document.createElement("span");n.appendChild(this.el.dom);n.innerHTML="";this.el.remove();this.el=null}});Ext.ContentPanel.prototype.getUpdateManager=Ext.ContentPanel.prototype.getUpdater;Ext.GridPanel=function(n,t){this.wrapper=Ext.DomHelper.append(document.body,{tag:"div",cls:"x-layout-grid-wrapper x-layout-inactive-content"},!0);this.wrapper.dom.appendChild(n.getGridEl().dom);Ext.GridPanel.superclass.constructor.call(this,this.wrapper,t);this.toolbar&&this.toolbar.el.insertBefore(this.wrapper.dom.firstChild);n.monitorWindowResize=!1;n.autoHeight=!1;n.autoWidth=!1;this.grid=n;this.grid.getGridEl().replaceClass("x-layout-inactive-content","x-layout-component-panel")};Ext.extend(Ext.GridPanel,Ext.ContentPanel,{getId:function(){return this.grid.id},getGrid:function(){return this.grid},setSize:function(n,t){if(!this.ignoreResize(n,t)){var i=this.grid,r=this.adjustForComponents(n,t);i.getGridEl().setSize(r.width,r.height);i.autoSize()}},beforeSlide:function(){this.grid.getView().scroller.clip()},afterSlide:function(){this.grid.getView().scroller.unclip()},destroy:function(){this.grid.destroy();delete this.grid;Ext.GridPanel.superclass.destroy.call(this)}});Ext.NestedLayoutPanel=function(n,t){Ext.NestedLayoutPanel.superclass.constructor.call(this,n.getEl(),t);n.monitorWindowResize=!1;this.layout=n;this.layout.getEl().addClass("x-layout-nested-layout")};Ext.extend(Ext.NestedLayoutPanel,Ext.ContentPanel,{setSize:function(n,t){var i,r,u;this.ignoreResize(n,t)||(i=this.adjustForComponents(n,t),r=this.layout.getEl(),r.setSize(i.width,i.height),u=r.dom.offsetWidth,this.layout.layout(),Ext.isIE&&!this.initialized&&(this.initialized=!0,this.layout.layout()))},getLayout:function(){return this.layout}});Ext.ScrollPanel=function(n,t,i){var f,r,u;t=t||{};t.fitToFrame=!0;Ext.ScrollPanel.superclass.constructor.call(this,n,t,i);this.el.dom.style.overflow="hidden";f=this.el.wrap({cls:"x-scroller x-layout-inactive-content"});this.el.removeClass("x-layout-inactive-content");this.el.on("mousewheel",this.onWheel,this);r=f.createChild({cls:"x-scroller-up",html:"&#160;"},this.el.dom);u=f.createChild({cls:"x-scroller-down",html:"&#160;"});r.unselectable();u.unselectable();r.on("click",this.scrollUp,this);u.on("click",this.scrollDown,this);r.addClassOnOver("x-scroller-btn-over");u.addClassOnOver("x-scroller-btn-over");r.addClassOnClick("x-scroller-btn-click");u.addClassOnClick("x-scroller-btn-click");this.adjustments=[0,-(r.getHeight()+u.getHeight())];this.resizeEl=this.el;this.el=f;this.up=r;this.down=u};Ext.extend(Ext.ScrollPanel,Ext.ContentPanel,{increment:100,wheelIncrement:5,scrollUp:function(){this.resizeEl.scroll("up",this.increment,{callback:this.afterScroll,scope:this})},scrollDown:function(){this.resizeEl.scroll("down",this.increment,{callback:this.afterScroll,scope:this})},afterScroll:function(){var n=this.resizeEl,t=n.dom.scrollTop,i=n.dom.scrollHeight,r=n.dom.clientHeight;this.up[t==0?"addClass":"removeClass"]("x-scroller-btn-disabled");this.down[i-t<=r?"addClass":"removeClass"]("x-scroller-btn-disabled")},setSize:function(){Ext.ScrollPanel.superclass.setSize.apply(this,arguments);this.afterScroll()},onWheel:function(n){var t=n.getWheelDelta();this.resizeEl.dom.scrollTop-=t*this.wheelIncrement;this.afterScroll();n.stopEvent()},setContent:function(n,t){this.resizeEl.update(n,t)}})