Ext.state.Manager=function(){var n=new Ext.state.Provider;return{setProvider:function(t){n=t},get:function(t,i){return n.get(t,i)},set:function(t,i){n.set(t,i)},clear:function(t){n.clear(t)},getProvider:function(){return n}}}()