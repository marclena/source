var editWnd=null,itemEdit=null,disposeWnd=function(){editWnd!=null&&editWnd.close();editWnd=null};Ext.onReady(function(){function s(n){return disposeWnd(),editForm=u(),Ext.Ajax.request({url:params.UrlFetchConcept,params:{IdConcepto:n},success:function(n){var i,t,r;for(respObj=Ext.decode(n.responseText),editWnd=new EditWindow({title:"Editar Concepto",width:300,height:160,form:editForm}),editWnd.show(),i=editForm.items.items,t=0;t<i.length;t++)r=eval("respObj."+i[t].name),r!=null&&r!=undefined&&i[t].setValue(r);editForm.render()}}),!0}function h(){disposeWnd();editWnd=new EditWindow({title:"Crear Concepto",width:300,height:160,form:u()});editWnd.show()}function o(){disposeWnd();editForm=u();Ext.Ajax.request({url:params.UrlFetchConcept,params:{IdConcepto:recordId},success:function(n){var i,t,r;respObj=Ext.decode(n.responseText);editWnd=new EditWindow({title:"Editar Concepto",width:300,height:160,form:editForm});editWnd.on("ok",function(){o()});editWnd.on("cancel",function(){o()});for(editWnd.show(),i=editForm.items.items,t=0;t<i.length;t++)r=eval("respObj."+i[t].name),r!=null&&r!=undefined&&i[t].setValue(r);editForm.render()}})}function c(){if(records=r.getSelections(),records.length>0){if(confirm("¿Está seguro que desea eliminar los elementos seleccionados?")){var t=[];for(i=0;i<records.length;i++)t[i]=records[i].data.IdConcepto;t=Ext.encode(t);Ext.Ajax.request({url:params.UrlRemove,params:{list:t},success:function(){n.reload();alert("Los elementos han sido eliminados correctamente")},failure:function(){alert("No se pudieron eliminar todos los elementos correnctamente")}})}}else alert("No hay elementos seleccionados")}function u(){return new Ext.FormPanel({labelWidth:65,bodyStyle:"padding:15px 15px 0",defaultType:"textfield",waitMsg:"Cargando...",submitMethod:function(){this.getForm().submit({method:"POST",waitTitle:"Conectando",waitMsg:"Enviando datos...",url:params.UrlSave,success:function(){disposeWnd();n.reload()},failure:function(n,t){t.failureType=="server"?(obj=Ext.util.JSON.decode(t.response.responseText),alert(obj.errors.errorMsg)):(obj=Ext.util.JSON.decode(t.response.responseText),alert(obj.errorMsg));disposeWnd()}})},items:[new Ext.form.Hidden({name:"concept.IdConcepto"}),{fieldLabel:"Concepto",width:170,name:"concept.Concepto",allowBlank:!1}]})}var t=params.ResultsPerPage,r;itemEdit=s;var n=new Ext.data.Store({proxy:new Ext.data.HttpProxy({url:params.UrlFetcher}),reader:new Ext.data.JsonReader({root:"data",totalProperty:"totalCount",id:"IdConcepto",fields:["IdConcepto","Concepto"]}),baseParams:{query:"",limit:t},remoteSort:!0}),f=new Ext.grid.CheckboxSelectionModel,e=new Ext.grid.ColumnModel([f,{id:"IdConcepto",header:"Id",dataIndex:"IdConcepto",width:20,sortable:!0},{header:"Concepto",dataIndex:"Concepto",sortable:!0},{header:"Editar",dataIndex:"Edit",renderer:function(n,t,i){return"<img src='"+params.UrlImageEdit+"' alt='Editar' style='cursor:pointer;' onclick='itemEdit(\""+i.data.IdConcepto+"\")' />"},width:35,sortable:!1}]);e.defaultSortable=!0;r=new Ext.grid.GridPanel({el:"grid",region:"center",autoExpandColumn:"Concepto",header:!1,border:!1,store:n,loadMask:!0,cm:e,sm:f,height:470,viewConfig:{forceFit:!0,scrollOffset:1},bbar:new Ext.PagingToolbar({pageSize:t,store:n,displayInfo:!0,items:["-"]}),tbar:[{text:"Añadir",icon:params.UrlImageAdd,cls:"x-btn-text-icon",handler:h},"-",{text:"Eliminar",icon:params.UrlImageDelete,cls:"x-btn-text-icon",handler:c},"   ","-","   ","Buscar:",new Ext.app.SearchField({store:n})]});r.render();n.load({params:{start:0,limit:t}})})