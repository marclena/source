Ext.EventManager=function(){var t,s,h=!1,n,c,e,o,u=Ext.lib.Event,i=Ext.lib.Dom,f=function(){if(!h){if(h=!0,Ext.isReady=!0,s&&clearInterval(s),(Ext.isGecko||Ext.isOpera)&&document.removeEventListener("DOMContentLoaded",f,!1),Ext.isIE){var n=document.getElementById("ie-deferred-loader");n&&(n.onreadystatechange=null,n.parentNode.removeChild(n))}t&&(t.fire(),t.clearListeners())}},a=function(){if(t=new Ext.util.Event,Ext.isGecko||Ext.isOpera)document.addEventListener("DOMContentLoaded",f,!1);else if(Ext.isIE){document.write('<script id="ie-deferred-loader" defer="defer" src="//:"><\/script>');var n=document.getElementById("ie-deferred-loader");n.onreadystatechange=function(){this.readyState=="complete"&&f()}}else Ext.isSafari&&(s=setInterval(function(){var n=document.readyState;n=="complete"&&f()},10));u.on(window,"load",f)},v=function(n,t){var i=new Ext.util.DelayedTask(n);return function(r){r=new Ext.EventObjectImpl(r);i.delay(t.buffer,n,null,[r])}},y=function(n,t,i,r){return function(u){Ext.EventManager.removeListener(t,i,r);n(u)}},p=function(n,t){return function(i){i=new Ext.EventObjectImpl(i);setTimeout(function(){n(i)},t.delay||10)}},l=function(n,t,i,r,f){var e=!i||typeof i=="boolean"?{}:i,s,o;if(r=r||e.fn,f=f||e.scope,s=Ext.getDom(n),!s)throw'Error listening for "'+t+'". Element "'+n+"\" doesn't exist.";o=function(n){n=Ext.EventObject.setEvent(n);var t;if(e.delegate){if(t=n.getTarget(e.delegate,s),!t)return}else t=n.target;e.stopEvent===!0&&n.stopEvent();e.preventDefault===!0&&n.preventDefault();e.stopPropagation===!0&&n.stopPropagation();e.normalized===!1&&(n=n.browserEvent);r.call(f||s,n,t,e)};e.delay&&(o=p(o,e));e.single&&(o=y(o,s,t,r));e.buffer&&(o=v(o,e));r._handlers=r._handlers||[];r._handlers.push([Ext.id(s),t,o]);u.on(s,t,o);if(t=="mousewheel"&&s.addEventListener){s.addEventListener("DOMMouseScroll",o,!1);u.on(window,"unload",function(){s.removeEventListener("DOMMouseScroll",o,!1)})}return t=="mousedown"&&s==document&&Ext.EventManager.stoppedMouseDownEvent.addListener(o),o},w=function(n,t,i){var h=Ext.id(n),f=i._handlers,e=i,r,s,o;if(f)for(r=0,s=f.length;r<s;r++)if(o=f[r],o[0]==h&&o[1]==t){e=o[2];f.splice(r,1);break}u.un(n,t,e);n=Ext.getDom(n);t=="mousewheel"&&n.addEventListener&&n.removeEventListener("DOMMouseScroll",e,!1);t=="mousedown"&&n==document&&Ext.EventManager.stoppedMouseDownEvent.removeListener(e)},b=/^(?:scope|delay|buffer|single|stopEvent|preventDefault|stopPropagation|normalized|args|delegate)$/,r={addListener:function(n,t,i,r,u){var f,e;if(typeof t=="object"){f=t;for(e in f)b.test(e)||(typeof f[e]=="function"?l(n,e,f,f[e],f.scope):l(n,e,f[e]));return}return l(n,t,u,i,r)},removeListener:function(n,t,i){return w(n,t,i)},onDocumentReady:function(n,i,r){if(h){t.addListener(n,i,r);t.fire();t.clearListeners();return}t||a();t.addListener(n,i,r)},onWindowResize:function(t,r,f){if(!n){n=new Ext.util.Event;c=new Ext.util.DelayedTask(function(){n.fire(i.getViewWidth(),i.getViewHeight())});u.on(window,"resize",this.fireWindowResize,this)}n.addListener(t,r,f)},fireWindowResize:function(){n&&((Ext.isIE||Ext.isAir)&&c?c.delay(50):n.fire(i.getViewWidth(),i.getViewHeight()))},onTextResize:function(n,t,i){if(!e){e=new Ext.util.Event;var r=new Ext.Element(document.createElement("div"));r.dom.className="x-text-resize";r.dom.innerHTML="X";r.appendTo(document.body);o=r.dom.offsetHeight;setInterval(function(){r.dom.offsetHeight!=o&&e.fire(o,o=r.dom.offsetHeight)},this.textResizeInterval)}e.addListener(n,t,i)},removeResizeListener:function(t,i){n&&n.removeListener(t,i)},fireResize:function(){n&&n.fire(i.getViewWidth(),i.getViewHeight())},ieDeferSrc:!1,textResizeInterval:50};return r.on=r.addListener,r.un=r.removeListener,r.stoppedMouseDownEvent=new Ext.util.Event,r}();Ext.onReady=Ext.EventManager.onDocumentReady;Ext.onReady(function(){var t=Ext.getBody(),n,i;t&&(n=[Ext.isIE?"ext-ie "+(Ext.isIE6?"ext-ie6":"ext-ie7"):Ext.isGecko?"ext-gecko":Ext.isOpera?"ext-opera":Ext.isSafari?"ext-safari":""],Ext.isMac&&n.push("ext-mac"),Ext.isLinux&&n.push("ext-linux"),Ext.isBorderBox&&n.push("ext-border-box"),Ext.isStrict&&(i=t.dom.parentNode,i&&(i.className+=" ext-strict")),t.addClass(n.join(" ")))});Ext.EventObject=function(){var n=Ext.lib.Event,t={63234:37,63235:39,63232:38,63233:40,63276:33,63277:34,63272:46,63273:36,63275:35},i=Ext.isIE?{1:0,4:1,2:2}:Ext.isSafari?{1:0,2:1,3:2}:{0:0,1:1,2:2};return Ext.EventObjectImpl=function(n){n&&this.setEvent(n.browserEvent||n)},Ext.EventObjectImpl.prototype={browserEvent:null,button:-1,shiftKey:!1,ctrlKey:!1,altKey:!1,BACKSPACE:8,TAB:9,RETURN:13,ENTER:13,SHIFT:16,CONTROL:17,ESC:27,SPACE:32,PAGEUP:33,PAGEDOWN:34,END:35,HOME:36,LEFT:37,UP:38,RIGHT:39,DOWN:40,DELETE:46,F5:116,setEvent:function(t){return t==this||t&&t.browserEvent?t:(this.browserEvent=t,t?(this.button=t.button?i[t.button]:t.which?t.which-1:-1,t.type=="click"&&this.button==-1&&(this.button=0),this.type=t.type,this.shiftKey=t.shiftKey,this.ctrlKey=t.ctrlKey||t.metaKey,this.altKey=t.altKey,this.keyCode=t.keyCode,this.charCode=t.charCode,this.target=n.getTarget(t),this.xy=n.getXY(t)):(this.button=-1,this.shiftKey=!1,this.ctrlKey=!1,this.altKey=!1,this.keyCode=0,this.charCode=0,this.target=null,this.xy=[0,0]),this)},stopEvent:function(){this.browserEvent&&(this.browserEvent.type=="mousedown"&&Ext.EventManager.stoppedMouseDownEvent.fire(this),n.stopEvent(this.browserEvent))},preventDefault:function(){this.browserEvent&&n.preventDefault(this.browserEvent)},isNavKeyPress:function(){var n=this.keyCode;return n=Ext.isSafari?t[n]||n:n,n>=33&&n<=40||n==this.RETURN||n==this.TAB||n==this.ESC},isSpecialKey:function(){var n=this.keyCode;return this.type=="keypress"&&this.ctrlKey||n==9||n==13||n==40||n==27||n==16||n==17||n>=18&&n<=20||n>=33&&n<=35||n>=36&&n<=39||n>=44&&n<=45},stopPropagation:function(){this.browserEvent&&(this.browserEvent.type=="mousedown"&&Ext.EventManager.stoppedMouseDownEvent.fire(this),n.stopPropagation(this.browserEvent))},getCharCode:function(){return this.charCode||this.keyCode},getKey:function(){var n=this.keyCode||this.charCode;return Ext.isSafari?t[n]||n:n},getPageX:function(){return this.xy[0]},getPageY:function(){return this.xy[1]},getTime:function(){return this.browserEvent?n.getTime(this.browserEvent):null},getXY:function(){return this.xy},getTarget:function(n,t,i){var r=Ext.get(this.target);return n?r.findParent(n,t,i):i?r:this.target},getRelatedTarget:function(){return this.browserEvent?n.getRelatedTarget(this.browserEvent):null},getWheelDelta:function(){var n=this.browserEvent,t=0;return n.wheelDelta?t=n.wheelDelta/120:n.detail&&(t=-n.detail/3),t},hasModifier:function(){return this.ctrlKey||this.altKey||this.shiftKey?!0:!1},within:function(n,t){var i=this[t?"getRelatedTarget":"getTarget"]();return i&&Ext.fly(n).contains(i)},getPoint:function(){return new Ext.lib.Point(this.xy[0],this.xy[1])}},new Ext.EventObjectImpl}()