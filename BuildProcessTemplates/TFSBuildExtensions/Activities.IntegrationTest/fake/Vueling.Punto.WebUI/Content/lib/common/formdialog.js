var FormDialog=function(){this.doCancel=function(){this.dlg.hide()};this.doOK=function(){this.form.submit({waitMsg:"Espere..."});this.dlg.hide()};this.dlg=new Ext.BasicDialog("FormDlg",{autoCreate:!0,width:500,height:200,modal:!0,proxyDrag:!0,shadow:!0,collapsible:!1,resizable:!1,closable:!1,title:"Sample Dialog"});this.dlg.addKeyListener(27,this.doCancel,this.dlg);this.dlg.addButton("Aceptar",this.doOK,this);this.dlg.addButton("Cancelar",this.doCancel,this);this.form=new Ext.form.Form({url:"MyFormURL"});this.sampleText=new Ext.form.TextField({msgTarget:"under",allowBlank:!1,id:"sampleText",name:"sampleText",fieldLabel:"Sample Text",width:300,blankText:"Enter Sample Text"});this.form.add(this.sampleText);this.dlg.body.setStyle("padding","8pt");this.form.render(this.dlg.body)},dialog;SampleDialog.prototype={show:function(){this.reset();this.dlg.show();var n=this.sampleText.el;setTimeout(function(){n.focus()},0)},reset:function(){this.form.reset()}};dialog=null;dialog||(dialog=new SampleDialog);dialog.show(this)