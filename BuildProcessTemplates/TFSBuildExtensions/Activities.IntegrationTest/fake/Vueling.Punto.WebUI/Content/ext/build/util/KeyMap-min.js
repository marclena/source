Ext.KeyMap=function(n,t,i){this.el=Ext.get(n);this.eventName=i||"keydown";this.bindings=[];t&&this.addBinding(t);this.enable()};Ext.KeyMap.prototype={stopEvent:!1,addBinding:function(n){var i,f,e,u,r,h,c;if(n instanceof Array){for(i=0,r=n.length;i<r;i++)this.addBinding(n[i]);return}var t=n.key,l=n.shift,a=n.ctrl,v=n.alt,o=n.fn||n.handler,s=n.scope;if(typeof t=="string"){for(f=[],e=t.toUpperCase(),u=0,r=e.length;u<r;u++)f.push(e.charCodeAt(u));t=f}h=t instanceof Array;c=function(n){var i,r,u;if((!l||n.shiftKey)&&(!a||n.ctrlKey)&&(!v||n.altKey))if(i=n.getKey(),h){for(r=0,u=t.length;r<u;r++)if(t[r]==i){this.stopEvent&&n.stopEvent();o.call(s||window,i,n);return}}else i==t&&(this.stopEvent&&n.stopEvent(),o.call(s||window,i,n))};this.bindings.push(c)},on:function(n,t,i){var r,u,f,e;typeof n!="object"||n instanceof Array?r=n:(r=n.key,u=n.shift,f=n.ctrl,e=n.alt);this.addBinding({key:r,shift:u,ctrl:f,alt:e,fn:t,scope:i})},handleKeyDown:function(n){var i,t,r;if(this.enabled)for(i=this.bindings,t=0,r=i.length;t<r;t++)i[t].call(this,n)},isEnabled:function(){return this.enabled},enable:function(){if(!this.enabled){this.el.on(this.eventName,this.handleKeyDown,this);this.enabled=!0}},disable:function(){this.enabled&&(this.el.removeListener(this.eventName,this.handleKeyDown,this),this.enabled=!1)}}