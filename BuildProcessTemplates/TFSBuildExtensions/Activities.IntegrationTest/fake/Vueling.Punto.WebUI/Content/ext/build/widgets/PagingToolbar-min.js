Ext.PagingToolbar=Ext.extend(Ext.Toolbar,{pageSize:20,displayMsg:"Displaying {0} - {1} of {2}",emptyMsg:"No data to display",beforePageText:"Page",afterPageText:"of {0}",firstText:"First Page",prevText:"Previous Page",nextText:"Next Page",lastText:"Last Page",refreshText:"Refresh",paramNames:{start:"start",limit:"limit"},initComponent:function(){Ext.PagingToolbar.superclass.initComponent.call(this);this.cursor=0;this.bind(this.store)},onRender:function(n,t){Ext.PagingToolbar.superclass.onRender.call(this,n,t);this.first=this.addButton({tooltip:this.firstText,iconCls:"x-tbar-page-first",disabled:!0,handler:this.onClick.createDelegate(this,["first"])});this.prev=this.addButton({tooltip:this.prevText,iconCls:"x-tbar-page-prev",disabled:!0,handler:this.onClick.createDelegate(this,["prev"])});this.addSeparator();this.add(this.beforePageText);this.field=Ext.get(this.addDom({tag:"input",type:"text",size:"3",value:"1",cls:"x-tbar-page-number"}).el);this.field.on("keydown",this.onPagingKeydown,this);this.field.on("focus",function(){this.dom.select()});this.afterTextEl=this.addText(String.format(this.afterPageText,1));this.field.setHeight(18);this.addSeparator();this.next=this.addButton({tooltip:this.nextText,iconCls:"x-tbar-page-next",disabled:!0,handler:this.onClick.createDelegate(this,["next"])});this.last=this.addButton({tooltip:this.lastText,iconCls:"x-tbar-page-last",disabled:!0,handler:this.onClick.createDelegate(this,["last"])});this.addSeparator();this.loading=this.addButton({tooltip:this.refreshText,iconCls:"x-tbar-loading",handler:this.onClick.createDelegate(this,["refresh"])});this.displayInfo&&(this.displayEl=Ext.fly(this.el.dom).createChild({cls:"x-paging-info"}));this.dsLoaded&&this.onLoad.apply(this,this.dsLoaded)},updateInfo:function(){if(this.displayEl){var n=this.store.getCount(),t=n==0?this.emptyMsg:String.format(this.displayMsg,this.cursor+1,this.cursor+n,this.store.getTotalCount());this.displayEl.update(t)}},onLoad:function(n,t,i){if(!this.rendered){this.dsLoaded=[n,t,i];return}this.cursor=i.params?i.params[this.paramNames.start]:0;var u=this.getPageData(),r=u.activePage,f=u.pages;this.afterTextEl.el.innerHTML=String.format(this.afterPageText,u.pages);this.field.dom.value=r;this.first.setDisabled(r==1);this.prev.setDisabled(r==1);this.next.setDisabled(r==f);this.last.setDisabled(r==f);this.loading.enable();this.updateInfo()},getPageData:function(){var n=this.store.getTotalCount();return{total:n,activePage:Math.ceil((this.cursor+this.pageSize)/this.pageSize),pages:n<this.pageSize?1:Math.ceil(n/this.pageSize)}},onLoadError:function(){this.rendered&&this.loading.enable()},readPage:function(n){var t=this.field.dom.value,i;return!t||isNaN(i=parseInt(t,10))?(this.field.dom.value=n.activePage,!1):i},onPagingKeydown:function(n){var i=n.getKey(),r=this.getPageData(),t,u;i==n.RETURN?(n.stopEvent(),(t=this.readPage(r))&&(t=Math.min(Math.max(1,t),r.pages)-1,this.doLoad(t*this.pageSize))):i==n.HOME||i==n.END?(n.stopEvent(),t=i==n.HOME?1:r.pages,this.field.dom.value=t):(i==n.UP||i==n.PAGEUP||i==n.DOWN||i==n.PAGEDOWN)&&(n.stopEvent(),(t=this.readPage(r))&&(u=n.shiftKey?10:1,(i==n.DOWN||i==n.PAGEDOWN)&&(u*=-1),t+=u,t>=1&t<=r.pages&&(this.field.dom.value=t)))},beforeLoad:function(){this.rendered&&this.loading&&this.loading.disable()},doLoad:function(n){var t={},i=this.paramNames;t[i.start]=n;t[i.limit]=this.pageSize;this.store.load({params:t})},onClick:function(n){var r=this.store;switch(n){case"first":this.doLoad(0);break;case"prev":this.doLoad(Math.max(0,this.cursor-this.pageSize));break;case"next":this.doLoad(this.cursor+this.pageSize);break;case"last":var t=r.getTotalCount(),i=t%this.pageSize,u=i?t-i:t-this.pageSize;this.doLoad(u);break;case"refresh":this.doLoad(this.cursor)}},unbind:function(n){n=Ext.StoreMgr.lookup(n);n.un("beforeload",this.beforeLoad,this);n.un("load",this.onLoad,this);n.un("loadexception",this.onLoadError,this);this.store=undefined},bind:function(n){n=Ext.StoreMgr.lookup(n);n.on("beforeload",this.beforeLoad,this);n.on("load",this.onLoad,this);n.on("loadexception",this.onLoadError,this);this.store=n}});Ext.reg("paging",Ext.PagingToolbar)