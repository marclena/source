Ext.QuickTips=function(){var n,t=[];return{init:function(){n||(n=new Ext.QuickTip({elements:"header,body"}))},enable:function(){n&&(t.pop(),t.length<1&&n.enable())},disable:function(){n&&n.disable();t.push(1)},isEnabled:function(){return n&&!n.disabled},getQuickTip:function(){return n},register:function(){n.register.apply(n,arguments)},unregister:function(){n.unregister.apply(n,arguments)},tips:function(){n.register.apply(n,arguments)}}}()