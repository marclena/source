Ext.util.TextMetrics=function(){var n;return{measure:function(t,i,r){return n||(n=Ext.util.TextMetrics.Instance(t,r)),n.bind(t),n.setFixedWidth(r||"auto"),n.getSize(i)},createInstance:function(n,t){return Ext.util.TextMetrics.Instance(n,t)}}}();Ext.util.TextMetrics.Instance=function(n,t){var i=new Ext.Element(document.createElement("div")),r;return document.body.appendChild(i.dom),i.position("absolute"),i.setLeftTop(-1e3,-1e3),i.hide(),t&&i.setWidth(t),r={getSize:function(n){i.update(n);var t=i.getSize();return i.update(""),t},bind:function(n){i.setStyle(Ext.fly(n).getStyles("font-size","font-style","font-weight","font-family","line-height"))},setFixedWidth:function(n){i.setWidth(n)},getWidth:function(n){return i.dom.style.width="auto",this.getSize(n).width},getHeight:function(n){return this.getSize(n).height}},r.bind(n),r};Ext.Element.measureText=Ext.util.TextMetrics.measure