Ext.grid.GridPanel=Ext.extend(Ext.Panel,{ddText:"{0} selected row{1}",minColumnWidth:25,monitorWindowResize:!0,maxRowsToMeasure:0,trackMouseOver:!0,enableDragDrop:!1,enableColumnMove:!0,enableColumnHide:!0,enableHdMenu:!0,stripeRows:!1,autoExpandColumn:!1,autoExpandMin:50,autoExpandMax:1e3,view:null,loadMask:!1,rendered:!1,viewReady:!1,stateEvents:["columnmove","columnresize","sortchange"],initComponent:function(){Ext.grid.GridPanel.superclass.initComponent.call(this);this.autoScroll=!1;this.columns&&this.columns instanceof Array&&(this.colModel=new Ext.grid.ColumnModel(this.columns),delete this.columns);this.ds&&(this.store=this.ds,delete this.ds);this.cm&&(this.colModel=this.cm,delete this.cm);this.sm&&(this.selModel=this.sm,delete this.sm);this.store=Ext.StoreMgr.lookup(this.store);this.addEvents("click","dblclick","contextmenu","mousedown","mouseup","mouseover","mouseout","keypress","keydown","cellmousedown","rowmousedown","headermousedown","cellclick","celldblclick","rowclick","rowdblclick","headerclick","headerdblclick","rowcontextmenu","cellcontextmenu","headercontextmenu","bodyscroll","columnresize","columnmove","sortchange")},onRender:function(){var n,t;Ext.grid.GridPanel.superclass.onRender.apply(this,arguments);n=this.body;this.el.addClass("x-grid-panel");t=this.getView();t.init(this);n.on("mousedown",this.onMouseDown,this);n.on("click",this.onClick,this);n.on("dblclick",this.onDblClick,this);n.on("contextmenu",this.onContextMenu,this);n.on("keydown",this.onKeyDown,this);this.relayEvents(n,["mousedown","mouseup","mouseover","mouseout","keypress"]);this.getSelectionModel().init(this);this.view.render()},initEvents:function(){Ext.grid.GridPanel.superclass.initEvents.call(this);this.loadMask&&(this.loadMask=new Ext.LoadMask(this.bwrap,Ext.apply({store:this.store},this.loadMask)))},initStateEvents:function(){Ext.grid.GridPanel.superclass.initStateEvents.call(this);this.colModel.on("hiddenchange",this.saveState,this,{delay:100})},applyState:function(n){var u=this.colModel,f=n.columns,t,o,i,r,e;if(f)for(t=0,o=f.length;t<o;t++)i=f[t],r=u.getColumnById(i.id),r&&(r.hidden=i.hidden,r.width=i.width,e=u.getIndexById(i.id),e!=t&&u.moveColumn(e,t));n.sort&&this.store[this.store.remoteSort?"setDefaultSort":"sort"](n.sort.field,n.sort.direction)},getState:function(){for(var r,n={columns:[]},t=0,i;i=this.colModel.config[t];t++)n.columns[t]={id:i.id,width:i.width},i.hidden&&(n.columns[t].hidden=!0);return r=this.store.getSortState(),r&&(n.sort=r),n},afterRender:function(){Ext.grid.GridPanel.superclass.afterRender.call(this);this.view.layout();this.viewReady=!0},reconfigure:function(n,t){this.loadMask&&(this.loadMask.destroy(),this.loadMask=new Ext.LoadMask(this.bwrap,Ext.apply({store:n},this.initialConfig.loadMask)));this.view.bind(n,t);this.store=n;this.colModel=t;this.rendered&&this.view.refresh(!0)},onKeyDown:function(n){this.fireEvent("keydown",n)},onDestroy:function(){if(this.rendered){this.loadMask&&this.loadMask.destroy();var n=this.body;n.removeAllListeners();this.view.destroy();n.update("")}this.colModel.purgeListeners();Ext.grid.GridPanel.superclass.onDestroy.call(this)},processEvent:function(n,t){var i,f;this.fireEvent(n,t);var r=t.getTarget(),u=this.view,e=u.findHeaderIndex(r);e!==!1?this.fireEvent("header"+n,this,e,t):(i=u.findRowIndex(r),f=u.findCellIndex(r),i!==!1&&(this.fireEvent("row"+n,this,i,t),f!==!1&&this.fireEvent("cell"+n,this,i,f,t)))},onClick:function(n){this.processEvent("click",n)},onMouseDown:function(n){this.processEvent("mousedown",n)},onContextMenu:function(n){this.processEvent("contextmenu",n)},onDblClick:function(n){this.processEvent("dblclick",n)},walkCells:function(n,t,i,r,u){var e=this.colModel,o=e.getColumnCount(),s=this.store,h=s.getCount(),f=!0;if(i<0)for(t<0&&(n--,f=!1);n>=0;){for(f||(t=o-1),f=!1;t>=0;){if(r.call(u||this,n,t,e)===!0)return[n,t];t--}n--}else for(t>=o&&(n++,f=!1);n<h;){for(f||(t=0),f=!1;t<o;){if(r.call(u||this,n,t,e)===!0)return[n,t];t++}n++}return null},getSelections:function(){return this.selModel.getSelections()},onResize:function(){Ext.grid.GridPanel.superclass.onResize.apply(this,arguments);this.viewReady&&this.view.layout()},getGridEl:function(){return this.body},stopEditing:function(){},getSelectionModel:function(){return this.selModel||(this.selModel=new Ext.grid.RowSelectionModel(this.disableSelection?{selectRow:Ext.emptyFn}:null)),this.selModel},getStore:function(){return this.store},getColumnModel:function(){return this.colModel},getView:function(){return this.view||(this.view=new Ext.grid.GridView(this.viewConfig)),this.view},getDragDropText:function(){var n=this.selModel.getCount();return String.format(this.ddText,n,n==1?"":"s")}});Ext.reg("grid",Ext.grid.GridPanel)