Ext.util.TaskRunner=function(n){n=n||10;var t=[],i=[],r=0,u=!1,e=function(){u=!1;clearInterval(r);r=0},o=function(){u||(u=!0,r=setInterval(s,n))},f=function(n){i.push(n);n.onStop&&n.onStop.apply(n.scope||n)},s=function(){var o,r,u,n,s,h;if(i.length>0){for(r=0,u=i.length;r<u;r++)t.remove(i[r]);if(i=[],t.length<1){e();return}}for(o=(new Date).getTime(),r=0,u=t.length;r<u;++r){if(n=t[r],s=o-n.taskRunTime,n.interval<=s&&(h=n.run.apply(n.scope||n,n.args||[++n.taskRunCount]),n.taskRunTime=o,h===!1||n.taskRunCount===n.repeat)){f(n);return}n.duration&&n.duration<=o-n.taskStartTime&&f(n)}};this.start=function(n){return t.push(n),n.taskStartTime=(new Date).getTime(),n.taskRunTime=0,n.taskRunCount=0,o(),n};this.stop=function(n){return f(n),n};this.stopAll=function(){e();for(var n=0,r=t.length;n<r;n++)t[n].onStop&&t[n].onStop();t=[];i=[]}};Ext.TaskMgr=new Ext.util.TaskRunner