Ext.grid.RowNumberer=function(n){Ext.apply(this,n);this.rowspan&&(this.renderer=this.renderer.createDelegate(this))};Ext.grid.RowNumberer.prototype={header:"",width:23,sortable:!1,fixed:!0,dataIndex:"",id:"numberer",rowspan:undefined,renderer:function(n,t,i,r){return this.rowspan&&(t.cellAttr='rowspan="'+this.rowspan+'"'),r+1}}