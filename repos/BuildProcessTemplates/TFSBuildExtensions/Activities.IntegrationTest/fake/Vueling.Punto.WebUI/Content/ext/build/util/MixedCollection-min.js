Ext.util.MixedCollection=function(n,t){this.items=[];this.map={};this.keys=[];this.length=0;this.addEvents("clear","add","replace","remove","sort");this.allowFunctions=n===!0;t&&(this.getKey=t);Ext.util.MixedCollection.superclass.constructor.call(this)};Ext.extend(Ext.util.MixedCollection,Ext.util.Observable,{allowFunctions:!1,add:function(n,t){if(arguments.length==1&&(t=arguments[0],n=this.getKey(t)),typeof n=="undefined"||n===null)this.length++,this.items.push(t),this.keys.push(null);else{var i=this.map[n];if(i)return this.replace(n,t);this.length++;this.items.push(t);this.map[n]=t;this.keys.push(n)}return this.fireEvent("add",this.length-1,t,n),t},getKey:function(n){return n.id},replace:function(n,t){var i,r;return(arguments.length==1&&(t=arguments[0],n=this.getKey(t)),i=this.item(n),typeof n=="undefined"||n===null||typeof i=="undefined")?this.add(n,t):(r=this.indexOfKey(n),this.items[r]=t,this.map[n]=t,this.fireEvent("replace",n,i,t),t)},addAll:function(n){var r,t,u,i;if(arguments.length>1||n instanceof Array)for(r=arguments.length>1?arguments:n,t=0,u=r.length;t<u;t++)this.add(r[t]);else for(i in n)(this.allowFunctions||typeof n[i]!="function")&&this.add(i,n[i])},each:function(n,t){for(var r=[].concat(this.items),i=0,u=r.length;i<u;i++)if(n.call(t||r[i],r[i],i,u)===!1)break},eachKey:function(n,t){for(var i=0,r=this.keys.length;i<r;i++)n.call(t||window,this.keys[i],this.items[i],i,r)},find:function(n,t){for(var i=0,r=this.items.length;i<r;i++)if(n.call(t||window,this.items[i],this.keys[i]))return this.items[i];return null},insert:function(n,t,i){return(arguments.length==2&&(i=arguments[1],t=this.getKey(i)),n>=this.length)?this.add(t,i):(this.length++,this.items.splice(n,0,i),typeof t!="undefined"&&t!=null&&(this.map[t]=i),this.keys.splice(n,0,t),this.fireEvent("add",n,i,t),i)},remove:function(n){return this.removeAt(this.indexOf(n))},removeAt:function(n){var i,t;return n<this.length&&n>=0?(this.length--,i=this.items[n],this.items.splice(n,1),t=this.keys[n],typeof t!="undefined"&&delete this.map[t],this.keys.splice(n,1),this.fireEvent("remove",i,t),i):!1},removeKey:function(n){return this.removeAt(this.indexOfKey(n))},getCount:function(){return this.length},indexOf:function(n){return this.items.indexOf(n)},indexOfKey:function(n){return this.keys.indexOf(n)},item:function(n){var t=typeof this.map[n]!="undefined"?this.map[n]:this.items[n];return typeof t!="function"||this.allowFunctions?t:null},itemAt:function(n){return this.items[n]},key:function(n){return this.map[n]},contains:function(n){return this.indexOf(n)!=-1},containsKey:function(n){return typeof this.map[n]!="undefined"},clear:function(){this.length=0;this.items=[];this.keys=[];this.map={};this.fireEvent("clear")},first:function(){return this.items[0]},last:function(){return this.items[this.length-1]},_sort:function(n,t,i){var s=String(t).toUpperCase()=="DESC"?-1:1,r,f;i=i||function(n,t){return n-t};var u=[],o=this.keys,e=this.items;for(r=0,f=e.length;r<f;r++)u[u.length]={key:o[r],value:e[r],index:r};for(u.sort(function(t,r){var u=i(t[n],r[n])*s;return u==0&&(u=t.index<r.index?-1:1),u}),r=0,f=u.length;r<f;r++)e[r]=u[r].value,o[r]=u[r].key;this.fireEvent("sort",this)},sort:function(n,t){this._sort("value",n,t)},keySort:function(n,t){this._sort("key",n,t||function(n,t){return String(n).toUpperCase()-String(t).toUpperCase()})},getRange:function(n,t){var u=this.items,r,i;if(u.length<1)return[];if(n=n||0,t=Math.min(typeof t=="undefined"?this.length-1:t,this.length-1),r=[],n<=t)for(i=n;i<=t;i++)r[r.length]=u[i];else for(i=n;i>=t;i--)r[r.length]=u[i];return r},filter:function(n,t,i,r){return Ext.isEmpty(t,!1)?this.clone():(t=this.createValueMatcher(t,i,r),this.filterBy(function(i){return i&&t.test(i[n])}))},filterBy:function(n,t){var u=new Ext.util.MixedCollection,f,r,i,e;for(u.getKey=this.getKey,f=this.keys,r=this.items,i=0,e=r.length;i<e;i++)n.call(t||this,r[i],f[i])&&u.add(f[i],r[i]);return u},findIndex:function(n,t,i,r,u){return Ext.isEmpty(t,!1)?-1:(t=this.createValueMatcher(t,r,u),this.findIndexBy(function(i){return i&&t.test(i[n])},null,i))},findIndexBy:function(n,t,i){for(var f=this.keys,u=this.items,r=i||0,e=u.length;r<e;r++)if(n.call(t||this,u[r],f[r]))return r;if(typeof i=="number"&&i>0)for(r=0;r<i;r++)if(n.call(t||this,u[r],f[r]))return r;return-1},createValueMatcher:function(n,t,i){return n.exec||(n=String(n),n=new RegExp((t===!0?"":"^")+Ext.escapeRe(n),i?"":"i")),n},clone:function(){for(var t=new Ext.util.MixedCollection,r=this.keys,i=this.items,n=0,u=i.length;n<u;n++)t.add(r[n],i[n]);return t.getKey=this.getKey,t}});Ext.util.MixedCollection.prototype.get=Ext.util.MixedCollection.prototype.item