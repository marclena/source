function formatMoney(n){for(var i=n,e=3,o,t=""+n,h="",r,s,f=2,u=0;u<f;u++)h+="0";if(r=t.indexOf("."),r<0)t=t+","+h;else if(s=t.length-r-1,s<=f)for(u=0;u<f-s;u++)t+="0";else i=i*Math.pow(10,f),i=Math.round(i),i=i/Math.pow(10,f),t=new String(i);if(r=t.indexOf("."),t=t.replace(/\./,","),r<0&&(r=t.lentgh),(t.substr(0,1)=="-"||t.substr(0,1)=="+")&&(e=4),!0&&r>e)do o=/([+-]?\d)(\d{3}[\,]\d*)/cad.match(o),t=t.replace(o,RegExp.$1+"."+RegExp.$2);while(t.indexOf(".")>e);return f<0&&(t=t.replace(/\./,"")),t+" €"}function formatPercentWith2Decimals(n){return(Math.round(n*100)/100+"%").replace(/\./,",")}function activateValidOnEnter(n){document.onkeyup=function(t){var i=window.event?event.keyCode:t.keyCode;i==13&&n()}}