Ext.LayoutStateManager=function(){this.state={north:{},south:{},east:{},west:{}}};Ext.LayoutStateManager.prototype={init:function(n,t){var i,e,f,r,u;if(this.provider=t,i=t.get(n.id+"-layout-state"),i){e=n.isUpdating();e||n.beginUpdate();for(f in i)typeof i[f]!="function"&&(r=i[f],u=n.getRegion(f),u&&r&&(r.size&&u.resizeTo(r.size),r.collapsed==!0?u.collapse(!0):u.expand(null,!0)));e||n.endUpdate();this.state=i}this.layout=n;n.on("regionresized",this.onRegionResized,this);n.on("regioncollapsed",this.onRegionCollapsed,this);n.on("regionexpanded",this.onRegionExpanded,this)},storeState:function(){this.provider.set(this.layout.id+"-layout-state",this.state)},onRegionResized:function(n,t){this.state[n.getPosition()].size=t;this.storeState()},onRegionCollapsed:function(n){this.state[n.getPosition()].collapsed=!0;this.storeState()},onRegionExpanded:function(n){this.state[n.getPosition()].collapsed=!1;this.storeState()}}