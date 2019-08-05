Ext.grid.PropertyRecord=Ext.data.Record.create([{name:"name",type:"string"},"value"]);Ext.grid.PropertyStore=function(n,t){this.grid=n;this.store=new Ext.data.Store({recordType:Ext.grid.PropertyRecord});this.store.on("update",this.onUpdate,this);t&&this.setSource(t);Ext.grid.PropertyStore.superclass.constructor.call(this)};Ext.extend(Ext.grid.PropertyStore,Ext.util.Observable,{setSource:function(n){var i,t;this.source=n;this.store.removeAll();i=[];for(t in n)this.isEditableValue(n[t])&&i.push(new Ext.grid.PropertyRecord({name:t,value:n[t]},t));this.store.loadRecords({records:i},{},!0)},onUpdate:function(n,t,i){if(i==Ext.data.Record.EDIT){var r=t.data.value,u=t.modified.value;this.grid.fireEvent("beforepropertychange",this.source,t.id,r,u)!==!1?(this.source[t.id]=r,t.commit(),this.grid.fireEvent("propertychange",this.source,t.id,r,u)):t.reject()}},getProperty:function(n){return this.store.getAt(n)},isEditableValue:function(n){return n&&n instanceof Date?!0:typeof n=="object"||typeof n=="function"?!1:!0},setValue:function(n,t){this.source[n]=t;this.store.getById(n).set("value",t)},getSource:function(){return this.source}});Ext.grid.PropertyColumnModel=function(n,t){var i,r,u;this.grid=n;i=Ext.grid;i.PropertyColumnModel.superclass.constructor.call(this,[{header:this.nameText,width:50,sortable:!0,dataIndex:"name",id:"name"},{header:this.valueText,width:50,resizable:!1,dataIndex:"value",id:"value"}]);this.store=t;this.bselect=Ext.DomHelper.append(document.body,{tag:"select",cls:"x-grid-editor x-hide-display",children:[{tag:"option",value:"true",html:"true"},{tag:"option",value:"false",html:"false"}]});r=Ext.form;u=new r.Field({el:this.bselect,bselect:this.bselect,autoShow:!0,getValue:function(){return this.bselect.value=="true"}});this.editors={date:new i.GridEditor(new r.DateField({selectOnFocus:!0})),string:new i.GridEditor(new r.TextField({selectOnFocus:!0})),number:new i.GridEditor(new r.NumberField({selectOnFocus:!0,style:"text-align:left;"})),boolean:new i.GridEditor(u)};this.renderCellDelegate=this.renderCell.createDelegate(this);this.renderPropDelegate=this.renderProp.createDelegate(this)};Ext.extend(Ext.grid.PropertyColumnModel,Ext.grid.ColumnModel,{nameText:"Name",valueText:"Value",dateFormat:"m/j/Y",renderDate:function(n){return n.dateFormat(this.dateFormat)},renderBool:function(n){return n?"true":"false"},isCellEditable:function(n){return n==1},getRenderer:function(n){return n==1?this.renderCellDelegate:this.renderPropDelegate},renderProp:function(n){return this.getPropertyName(n)},renderCell:function(n){var t=n;return n instanceof Date?t=this.renderDate(n):typeof n=="boolean"&&(t=this.renderBool(n)),Ext.util.Format.htmlEncode(t)},getPropertyName:function(n){var t=this.grid.propertyNames;return t&&t[n]?t[n]:n},getCellEditor:function(n,t){var r=this.store.getProperty(t),u=r.data.name,i=r.data.value;return this.grid.customEditors[u]?this.grid.customEditors[u]:i instanceof Date?this.editors.date:typeof i=="number"?this.editors.number:typeof i=="boolean"?this.editors.boolean:this.editors.string}});Ext.grid.PropertyGrid=Ext.extend(Ext.grid.EditorGridPanel,{enableColumnMove:!1,stripeRows:!1,trackMouseOver:!1,clicksToEdit:1,enableHdMenu:!1,viewConfig:{forceFit:!0},initComponent:function(){var n,t;this.customEditors=this.customEditors||{};this.lastEditRow=null;n=new Ext.grid.PropertyStore(this);this.propStore=n;t=new Ext.grid.PropertyColumnModel(this,n);n.store.sort("name","ASC");this.addEvents("beforepropertychange","propertychange");this.cm=t;this.ds=n.store;Ext.grid.PropertyGrid.superclass.initComponent.call(this);this.selModel.on("beforecellselect",function(n,t,i){if(i===0)return this.startEditing.defer(200,this,[t,1]),!1},this)},onRender:function(){Ext.grid.PropertyGrid.superclass.onRender.apply(this,arguments);this.getGridEl().addClass("x-props-grid")},afterRender:function(){Ext.grid.PropertyGrid.superclass.afterRender.apply(this,arguments);this.source&&this.setSource(this.source)},setSource:function(n){this.propStore.setSource(n)},getSource:function(){return this.propStore.getSource()}})