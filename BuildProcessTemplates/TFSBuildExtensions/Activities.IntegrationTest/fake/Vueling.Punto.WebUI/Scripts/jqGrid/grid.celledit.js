(function(n){n.jgrid.extend({editCell:function(t,i,r){return this.each(function(){var u=this,e,f,o,s,c,h;if(u.grid&&u.p.cellEdit===!0){if(i=parseInt(i,10),u.p.selrow=u.rows[t].id,u.p.knv||n(u).jqGrid("GridNav"),u.p.savedRow.length>0){if(r===!0&&t==u.p.iRow&&i==u.p.iCol)return;n(u).jqGrid("saveCell",u.p.savedRow[0].id,u.p.savedRow[0].ic)}else window.setTimeout(function(){n("#"+u.p.knv).attr("tabindex","-1").focus()},0);if(e=u.p.colModel[i].name,e!="subgrid"&&e!="cb"&&e!="rn"){if(o=n("td:eq("+i+")",u.rows[t]),u.p.colModel[i].editable!==!0||r!==!0||o.hasClass("not-editable-cell"))parseInt(u.p.iCol,10)>=0&&parseInt(u.p.iRow,10)>=0&&(n("td:eq("+u.p.iCol+")",u.rows[u.p.iRow]).removeClass("edit-cell ui-state-highlight"),n(u.rows[u.p.iRow]).removeClass("selected-row ui-state-hover")),o.addClass("edit-cell ui-state-highlight"),n(u.rows[t]).addClass("selected-row ui-state-hover"),n.isFunction(u.p.onSelectCell)&&(f=o.html().replace(/\&#160\;/ig,""),u.p.onSelectCell.call(u,u.rows[t].id,e,f,t,i));else{parseInt(u.p.iCol,10)>=0&&parseInt(u.p.iRow,10)>=0&&(n("td:eq("+u.p.iCol+")",u.rows[u.p.iRow]).removeClass("edit-cell ui-state-highlight"),n(u.rows[u.p.iRow]).removeClass("selected-row ui-state-hover"));n(o).addClass("edit-cell ui-state-highlight");n(u.rows[t]).addClass("selected-row ui-state-hover");try{f=n.unformat(o,{rowId:u.rows[t].id,colModel:u.p.colModel[i]},i)}catch(l){f=n(o).html()}u.p.autoencode&&(f=n.jgrid.htmlDecode(f));u.p.colModel[i].edittype||(u.p.colModel[i].edittype="text");u.p.savedRow.push({id:t,ic:i,name:e,v:f});n.isFunction(u.p.formatCell)&&(s=u.p.formatCell.call(u,u.rows[t].id,e,f,t,i),s!==undefined&&(f=s));c=n.extend({},u.p.colModel[i].editoptions||{},{id:t+"_"+e,name:e});h=createEl(u.p.colModel[i].edittype,c,f,!0,n.extend({},n.jgrid.ajaxOptions,u.p.ajaxSelectOptions||{}));n.isFunction(u.p.beforeEditCell)&&u.p.beforeEditCell.call(u,u.rows[t].id,e,f,t,i);n(o).html("").append(h).attr("tabindex","0");window.setTimeout(function(){n(h).focus()},0);n("input, select, textarea",o).bind("keydown",function(r){if(r.keyCode===27&&(n("input.hasDatepicker",o).length>0?n(".ui-datepicker").is(":hidden")?n(u).jqGrid("restoreCell",t,i):n("input.hasDatepicker",o).datepicker("hide"):n(u).jqGrid("restoreCell",t,i)),r.keyCode===13&&n(u).jqGrid("saveCell",t,i),r.keyCode==9){if(u.grid.hDiv.loading)return!1;r.shiftKey?n(u).jqGrid("prevCell",t,i):n(u).jqGrid("nextCell",t,i)}r.stopPropagation()});n.isFunction(u.p.afterEditCell)&&u.p.afterEditCell.call(u,u.rows[t].id,e,f,t,i)}u.p.iCol=i;u.p.iRow=t}}})},saveCell:function(t,i){return this.each(function(){var r=this,y,p,w,a,k,d,l,s,g,nt,v;if(r.grid&&r.p.cellEdit===!0){if(y=r.p.savedRow.length>=1?0:null,y!==null){var e=n("td:eq("+i+")",r.rows[t]),u,f,o=r.p.colModel[i],h=o.name,c=n.jgrid.jqID(h);switch(o.edittype){case"select":o.editoptions.multiple?(p=n("#"+t+"_"+c,r.rows[t]),w=[],u=n(p).val(),u?u.join(","):u="",n("option:selected",p).each(function(t,i){w[t]=n(i).text()}),f=w.join(",")):(u=n("#"+t+"_"+c+">option:selected",r.rows[t]).val(),f=n("#"+t+"_"+c+">option:selected",r.rows[t]).text());o.formatter&&(f=u);break;case"checkbox":a=["Yes","No"];o.editoptions&&(a=o.editoptions.value.split(":"));u=n("#"+t+"_"+c,r.rows[t]).attr("checked")?a[0]:a[1];f=u;break;case"password":case"text":case"textarea":case"button":u=n("#"+t+"_"+c,r.rows[t]).val();f=u;break;case"custom":try{if(o.editoptions&&n.isFunction(o.editoptions.custom_value))if(u=o.editoptions.custom_value.call(r,n(".customelement",e),"get"),u===undefined)throw"e2";else f=u;else throw"e1";}catch(b){b=="e1"&&info_dialog(jQuery.jgrid.errors.errcap,"function 'custom_value' "+n.jgrid.edit.msg.nodefined,jQuery.jgrid.edit.bClose);b=="e2"?info_dialog(jQuery.jgrid.errors.errcap,"function 'custom_value' "+n.jgrid.edit.msg.novalue,jQuery.jgrid.edit.bClose):info_dialog(jQuery.jgrid.errors.errcap,b.message,jQuery.jgrid.edit.bClose)}}if(f!=r.p.savedRow[y].v)if(n.isFunction(r.p.beforeSaveCell)&&(k=r.p.beforeSaveCell.call(r,r.rows[t].id,h,u,t,i),k&&(u=k)),d=checkValues(u,i,r),d[0]===!0){if(l={},n.isFunction(r.p.beforeSubmitCell)&&(l=r.p.beforeSubmitCell.call(r,r.rows[t].id,h,u,t,i),l||(l={})),n("input.hasDatepicker",e).length>0&&n("input.hasDatepicker",e).datepicker("hide"),r.p.cellsubmit=="remote")if(r.p.cellurl)s={},r.p.autoencode&&(u=n.jgrid.htmlEncode(u)),s[h]=u,v=r.p.prmNames,g=v.id,nt=v.oper,s[g]=r.rows[t].id,s[nt]=v.editoper,s=n.extend(l,s),n("#lui_"+r.p.id).show(),r.grid.hDiv.loading=!0,n.ajax(n.extend({url:r.p.cellurl,data:n.isFunction(r.p.serializeCellData)?r.p.serializeCellData.call(r,s):s,type:"POST",complete:function(o,c){if(n("#lui_"+r.p.id).hide(),r.grid.hDiv.loading=!1,c=="success")if(n.isFunction(r.p.afterSubmitCell)){var l=r.p.afterSubmitCell.call(r,o,s.id,h,u,t,i);l[0]===!0?(n(e).empty(),n(r).jqGrid("setCell",r.rows[t].id,i,f,!1,!1,!0),n(e).addClass("dirty-cell"),n(r.rows[t]).addClass("edited"),n.isFunction(r.p.afterSaveCell)&&r.p.afterSaveCell.call(r,r.rows[t].id,h,u,t,i),r.p.savedRow.splice(0,1)):(info_dialog(n.jgrid.errors.errcap,l[1],n.jgrid.edit.bClose),n(r).jqGrid("restoreCell",t,i))}else n(e).empty(),n(r).jqGrid("setCell",r.rows[t].id,i,f,!1,!1,!0),n(e).addClass("dirty-cell"),n(r.rows[t]).addClass("edited"),n.isFunction(r.p.afterSaveCell)&&r.p.afterSaveCell.call(r,r.rows[t].id,h,u,t,i),r.p.savedRow.splice(0,1)},error:function(u,f){n("#lui_"+r.p.id).hide();r.grid.hDiv.loading=!1;n.isFunction(r.p.errorCell)?(r.p.errorCell.call(r,u,f),n(r).jqGrid("restoreCell",t,i)):(info_dialog(n.jgrid.errors.errcap,u.status+" : "+u.statusText+"<br/>"+f,n.jgrid.edit.bClose),n(r).jqGrid("restoreCell",t,i))}},n.jgrid.ajaxOptions,r.p.ajaxCellOptions||{}));else try{info_dialog(n.jgrid.errors.errcap,n.jgrid.errors.nourl,n.jgrid.edit.bClose);n(r).jqGrid("restoreCell",t,i)}catch(b){}r.p.cellsubmit=="clientArray"&&(n(e).empty(),n(r).jqGrid("setCell",r.rows[t].id,i,f,!1,!1,!0),n(e).addClass("dirty-cell"),n(r.rows[t]).addClass("edited"),n.isFunction(r.p.afterSaveCell)&&r.p.afterSaveCell.call(r,r.rows[t].id,h,u,t,i),r.p.savedRow.splice(0,1))}else try{window.setTimeout(function(){info_dialog(n.jgrid.errors.errcap,u+" "+d[1],n.jgrid.edit.bClose)},100);n(r).jqGrid("restoreCell",t,i)}catch(b){}else n(r).jqGrid("restoreCell",t,i)}n.browser.opera?n("#"+r.p.knv).attr("tabindex","-1").focus():window.setTimeout(function(){n("#"+r.p.knv).attr("tabindex","-1").focus()},0)}})},restoreCell:function(t,i){return this.each(function(){var r=this,u,f;if(r.grid&&r.p.cellEdit===!0){if(u=r.p.savedRow.length>=1?0:null,u!==null){if(f=n("td:eq("+i+")",r.rows[t]),n.isFunction(n.fn.datepicker))try{n("input.hasDatepicker",f).datepicker("hide")}catch(e){}n(f).empty().attr("tabindex","-1");n(r).jqGrid("setCell",r.rows[t].id,i,r.p.savedRow[u].v,!1,!1,!0);n.isFunction(r.p.afterRestoreCell)&&r.p.afterRestoreCell.call(r,r.rows[t].id,r.p.savedRow[u].v,t,i);r.p.savedRow.splice(0,1)}window.setTimeout(function(){n("#"+r.p.knv).attr("tabindex","-1").focus()},0)}})},nextCell:function(t,i){return this.each(function(){var r=this,f=!1,u;if(r.grid&&r.p.cellEdit===!0){for(u=i+1;u<r.p.colModel.length;u++)if(r.p.colModel[u].editable===!0){f=u;break}f!==!1?n(r).jqGrid("editCell",t,f,!0):r.p.savedRow.length>0&&n(r).jqGrid("saveCell",t,i)}})},prevCell:function(t,i){return this.each(function(){var r=this,f=!1,u;if(r.grid&&r.p.cellEdit===!0){for(u=i-1;u>=0;u--)if(r.p.colModel[u].editable===!0){f=u;break}f!==!1?n(r).jqGrid("editCell",t,f,!0):r.p.savedRow.length>0&&n(r).jqGrid("saveCell",t,i)}})},GridNav:function(){return this.each(function(){function u(i,r,u){if(u.substr(0,1)=="v"){var e=n(t.grid.bDiv)[0].clientHeight,o=n(t.grid.bDiv)[0].scrollTop,s=t.rows[i].offsetTop+t.rows[i].clientHeight,h=t.rows[i].offsetTop;u=="vd"&&s>=e&&(n(t.grid.bDiv)[0].scrollTop=n(t.grid.bDiv)[0].scrollTop+t.rows[i].clientHeight);u=="vu"&&h<o&&(n(t.grid.bDiv)[0].scrollTop=n(t.grid.bDiv)[0].scrollTop-t.rows[i].clientHeight)}if(u=="h"){var c=n(t.grid.bDiv)[0].clientWidth,f=n(t.grid.bDiv)[0].scrollLeft,l=t.rows[i].cells[r].offsetLeft+t.rows[i].cells[r].clientWidth,a=t.rows[i].cells[r].offsetLeft;l>=c+parseInt(f,10)?n(t.grid.bDiv)[0].scrollLeft=n(t.grid.bDiv)[0].scrollLeft+t.rows[i].cells[r].clientWidth:a<f&&(n(t.grid.bDiv)[0].scrollLeft=n(t.grid.bDiv)[0].scrollLeft-t.rows[i].cells[r].clientWidth)}}function e(n,i){var u,r;if(i=="lft")for(u=n+1,r=n;r>=0;r--)if(t.p.colModel[r].hidden!==!0){u=r;break}if(i=="rgt")for(u=n-1,r=n;r<t.p.colModel.length;r++)if(t.p.colModel[r].hidden!==!0){u=r;break}return u}var t=this,f,i,r;t.grid&&t.p.cellEdit===!0&&(t.p.knv=t.p.id+"_kn",f=n("<span style='width:0px;height:0px;background-color:black;' tabindex='0'><span tabindex='-1' style='width:0px;height:0px;background-color:grey' id='"+t.p.knv+"'><\/span><\/span>"),n(f).insertBefore(t.grid.cDiv),n("#"+t.p.knv).focus().keydown(function(f){r=f.keyCode;t.p.direction=="rtl"&&(r==37?r=39:r==39&&(r=37));switch(r){case 38:t.p.iRow-1>0&&(u(t.p.iRow-1,t.p.iCol,"vu"),n(t).jqGrid("editCell",t.p.iRow-1,t.p.iCol,!1));break;case 40:t.p.iRow+1<=t.rows.length-1&&(u(t.p.iRow+1,t.p.iCol,"vd"),n(t).jqGrid("editCell",t.p.iRow+1,t.p.iCol,!1));break;case 37:t.p.iCol-1>=0&&(i=e(t.p.iCol-1,"lft"),u(t.p.iRow,i,"h"),n(t).jqGrid("editCell",t.p.iRow,i,!1));break;case 39:t.p.iCol+1<=t.p.colModel.length-1&&(i=e(t.p.iCol+1,"rgt"),u(t.p.iRow,i,"h"),n(t).jqGrid("editCell",t.p.iRow,i,!1));break;case 13:parseInt(t.p.iCol,10)>=0&&parseInt(t.p.iRow,10)>=0&&n(t).jqGrid("editCell",t.p.iRow,t.p.iCol,!0)}return!1}))})},getChangedCells:function(t){var i=[];return t||(t="all"),this.each(function(){var r=this,u;r.grid&&r.p.cellEdit===!0&&n(r.rows).each(function(f){var e={};n(this).hasClass("edited")&&(n("td",this).each(function(i){if(u=r.p.colModel[i].name,u!=="cb"&&u!=="subgrid")if(t=="dirty"){if(n(this).hasClass("dirty-cell"))try{e[u]=n.unformat(this,{rowId:r.rows[f].id,colModel:r.p.colModel[i]},i)}catch(o){e[u]=n.jgrid.htmlDecode(n(this).html())}}else try{e[u]=n.unformat(this,{rowId:r.rows[f].id,colModel:r.p.colModel[i]},i)}catch(o){e[u]=n.jgrid.htmlDecode(n(this).html())}}),e.id=this.id,i.push(e))})}),i}})})(jQuery)