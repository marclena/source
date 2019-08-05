Ext.CompositeElement=function(n){this.elements=[];this.addElements(n)};Ext.CompositeElement.prototype={isComposite:!0,addElements:function(n){var i,r,t,u;if(!n)return this;for(typeof n=="string"&&(n=Ext.Element.selectorFunction(n)),i=this.elements,r=i.length-1,t=0,u=n.length;t<u;t++)i[++r]=Ext.get(n[t]);return this},fill:function(n){return this.elements=[],this.add(n),this},filter:function(n){var t=[];return this.each(function(i){i.is(n)&&(t[t.length]=i.dom)}),this.fill(t),this},invoke:function(n,t){for(var r=this.elements,i=0,u=r.length;i<u;i++)Ext.Element.prototype[n].apply(r[i],t);return this},add:function(n){return typeof n=="string"?this.addElements(Ext.Element.selectorFunction(n)):n.length!==undefined?this.addElements(n):this.addElements([n]),this},each:function(n,t){for(var r=this.elements,i=0,u=r.length;i<u;i++)if(n.call(t||r[i],r[i],this,i)===!1)break;return this},item:function(n){return this.elements[n]||null},first:function(){return this.item(0)},last:function(){return this.item(this.elements.length-1)},getCount:function(){return this.elements.length},contains:function(n){return this.indexOf(n)!==-1},indexOf:function(n){return this.elements.indexOf(Ext.get(n))},removeElement:function(n,t){var r,f,i,u;if(n instanceof Array){for(r=0,f=n.length;r<f;r++)this.removeElement(n[r]);return this}return i=typeof n=="number"?n:this.indexOf(n),i!==-1&&this.elements[i]&&(t&&(u=this.elements[i],u.dom?u.remove():Ext.removeNode(u)),this.elements.splice(i,1)),this},replaceElement:function(n,t,i){var r=typeof n=="number"?n:this.indexOf(n);return r!==-1&&(i?this.elements[r].replaceWith(t):this.elements.splice(r,1,Ext.get(t))),this},clear:function(){this.elements=[]}},function(){Ext.CompositeElement.createCall=function(n,t){n[t]||(n[t]=function(){return this.invoke(t,arguments)})};for(var n in Ext.Element.prototype)typeof Ext.Element.prototype[n]=="function"&&Ext.CompositeElement.createCall(Ext.CompositeElement.prototype,n)}();Ext.CompositeElementLite=function(n){Ext.CompositeElementLite.superclass.constructor.call(this,n);this.el=new Ext.Element.Flyweight};Ext.extend(Ext.CompositeElementLite,Ext.CompositeElement,{addElements:function(n){var i,r,t,u;if(n)if(n instanceof Array)this.elements=this.elements.concat(n);else for(i=this.elements,r=i.length-1,t=0,u=n.length;t<u;t++)i[++r]=n[t];return this},invoke:function(n,t){for(var r=this.elements,u=this.el,i=0,f=r.length;i<f;i++)u.dom=r[i],Ext.Element.prototype[n].apply(u,t);return this},item:function(n){return this.elements[n]?(this.el.dom=this.elements[n],this.el):null},addListener:function(n,t,i,r){for(var f=this.elements,u=0,e=f.length;u<e;u++)Ext.EventManager.on(f[u],n,t,i||f[u],r);return this},each:function(n,t){for(var u=this.elements,r=this.el,i=0,f=u.length;i<f;i++)if(r.dom=u[i],n.call(t||r,r,this,i)===!1)break;return this},indexOf:function(n){return this.elements.indexOf(Ext.getDom(n))},replaceElement:function(n,t,i){var u=typeof n=="number"?n:this.indexOf(n),r;return u!==-1&&(t=Ext.getDom(t),i&&(r=this.elements[u],r.parentNode.insertBefore(t,r),Ext.removeNode(r)),this.elements.splice(u,1,t)),this}});Ext.CompositeElementLite.prototype.on=Ext.CompositeElementLite.prototype.addListener;Ext.DomQuery&&(Ext.Element.selectorFunction=Ext.DomQuery.select);Ext.Element.select=function(n,t,i){var r;if(typeof n=="string")r=Ext.Element.selectorFunction(n,i);else if(n.length!==undefined)r=n;else throw"Invalid selector";return t===!0?new Ext.CompositeElement(r):new Ext.CompositeElementLite(r)};Ext.select=Ext.Element.select