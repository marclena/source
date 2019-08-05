(function(n){n.jgrid.extend({groupingSetup:function(){return this.each(function(){var r=this,t=r.p.groupingView,i,f,u,e;if(t!==null&&isObject(t))if(t.groupField.length){for(i=0;i<t.groupField.length;i++)if(t.groupOrder[i]||(t.groupOrder[i]="asc"),t.groupText[i]||(t.groupText[i]="{0}"),typeof t.groupColumnShow[i]!="boolean"&&(t.groupColumnShow[i]=!0),typeof t.groupSummary[i]!="boolean"&&(t.groupSummary[i]=!1),t.groupColumnShow[i]===!0?n(r).jqGrid("showCol",t.groupField[i]):n(r).jqGrid("hideCol",t.groupField[i]),t.sortitems[i]=[],t.sortnames[i]=[],t.summaryval[i]=[],t.groupSummary[i])for(t.summary[i]=[],f=r.p.colModel,u=0,e=f.length;u<e;u++)f[u].summaryType&&t.summary[i].push({nm:f[u].name,st:f[u].summaryType,v:""});r.p.scroll=!1;r.p.rownumbers=!1;r.p.subGrid=!1;r.p.treeGrid=!1;r.p.gridview=!0}else r.p.grouping=!1;else r.p.grouping=!1})},groupingPrepare:function(t,i,r,u){return this.each(function(){var f=i[0]?i[0].split(" ").join(""):"",e=this.p.groupingView,o=this;r.hasOwnProperty(f)?r[f].push(t):(r[f]=[],r[f].push(t),e.sortitems[0].push(f),e.sortnames[0].push(n.trim(i[0])),e.summaryval[0][f]=n.extend(!0,{},e.summary[0]));e.groupSummary[0]&&n.each(e.summaryval[0][f],function(){this.v=n.isFunction(this.st)?this.st.call(o,this.v,this.nm,u):n(o).jqGrid("groupingCalculations."+this.st,this.v,this.nm,u)})}),r},groupingToggle:function(t){return this.each(function(){var s=this,i=s.p.groupingView,u=t.lastIndexOf("_"),f=t.substring(0,u+1),e=parseInt(t.substring(u+1))+1,r=i.minusicon,o=i.plusicon;n("#"+t+" span").hasClass(r)?(i.showSummaryOnHide&&i.groupSummary[0]?n("#"+t).nextUntil(".jqfoot").hide():n("#"+t).nextUntil("#"+f+String(e)).hide(),n("#"+t+" span").removeClass(r).addClass(o)):(n("#"+t).nextUntil("#"+f+String(e)).show(),n("#"+t+" span").removeClass(o).addClass(r))}),!1},groupingRender:function(t,i){return this.each(function(){var u=this,r=u.p.groupingView,f="",s="",e,o="";r.groupDataSorted||(r.sortitems[0].sort(),r.sortnames[0].sort(),r.groupOrder[0].toLowerCase()=="desc"&&r.sortitems[0].reverse());o=r.groupCollapse?r.plusicon:r.minusicon;o+=" tree-wrap-"+u.p.direction;n.each(r.sortitems[0],function(h,c){var a,v,l,w,b;for(e=u.p.id+"ghead_"+h,s="<span style='cursor:pointer;' class='ui-icon "+o+"' onclick=\"jQuery('#"+u.p.id+"').jqGrid('groupingToggle','"+e+"');return false;\"><\/span>",f+='<tr id="'+e+'" role="row" class= "ui-widget-content jqgroup ui-row-'+u.p.direction+'"><td colspan="'+i+'">'+s+n.jgrid.format(r.groupText[0],r.sortnames[0][h],t[c].length)+"<\/td><\/tr>",a=0;a<t[c].length;a++)f+=t[c][a].join("");if(r.groupSummary[0]){v="";r.groupCollapse&&!r.showSummaryOnHide&&(v=' style="display:none;"');f+="<tr"+v+' role="row" class="ui-widget-content jqfoot ui-row-'+u.p.direction+'">';var d=r.summaryval[0][c],y=u.p.colModel,p,k=t[c].length;for(l=0;l<i;l++)w="<td "+u.formatCol(l,1,"")+">&#160;<\/td>",b="{0}",n.each(d,function(){if(this.nm==y[l].name){y[l].summaryTpl&&(b=y[l].summaryTpl);this.st=="avg"&&this.v&&k>0&&(this.v=this.v/k);try{p=u.formatter("",this.v,l,this)}catch(t){p=this.v}return w="<td "+u.formatCol(l,1,"")+">"+n.jgrid.format(b,p)+"<\/td>",!1}}),f+=w;f+="<\/tr>"}});n("#"+u.p.id+" tbody:first").append(f);f=null;r.sortitems[0]=[];r.sortnames[0]=[];r.summaryval[0]=[]})},groupingGroupBy:function(t,i){return this.each(function(){var r=this,u,f;for(typeof t=="string"&&(t=[t]),u=r.p.groupingView,r.p.grouping=!0,f=0;f<u.groupField.length;f++)u.groupColumnShow[f]||n(r).jqGrid("showCol",u.groupField[f]);r.p.groupingView=n.extend(r.p.groupingView,i||{});u.groupField=t;n(r).trigger("reloadGrid")})},groupingRemove:function(t){return this.each(function(){var i=this;typeof t=="undefined"&&(t=!0);i.p.grouping=!1;t===!0?n("tr.jqgroup, tr.jqfoot","#"+i.p.id+" tbody:first").remove():n(i).trigger("reloadGrid")})},groupingCalculations:{sum:function(n,t,i){return parseFloat(n||0)+parseFloat(i[t]||0)},min:function(n,t,i){return n===""?parseFloat(i[t]||0):Math.min(parseFloat(n),parseFloat(i[t]||0))},max:function(n,t,i){return n===""?parseFloat(i[t]||0):Math.max(parseFloat(n),parseFloat(i[t]||0))},count:function(n,t,i){return n===""&&(n=0),i.hasOwnProperty(t)?n+1:0},avg:function(n,t,i){return parseFloat(n||0)+parseFloat(i[t]||0)}}})})(jQuery)