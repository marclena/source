var Scriptaculous={Version:"1.7.0",require:function(n){document.write('<script type="text/javascript" src="'+n+'"><\/script>')},load:function(){if(typeof Prototype=="undefined"||typeof Element=="undefined"||typeof Element.Methods=="undefined"||parseFloat(Prototype.Version.split(".")[0]+"."+Prototype.Version.split(".")[1])<1.5)throw"script.aculo.us requires the Prototype JavaScript framework >= 1.5.0";$A(document.getElementsByTagName("script")).findAll(function(n){return n.src&&n.src.match(/scriptaculous\.js(\?.*)?$/)}).each(function(n){var i=n.src.replace(/scriptaculous\.js(\?.*)?$/,""),t=n.src.match(/\?.*load=([a-z,]*)/);(t?t[1]:"builder,effects,dragdrop,controls,slider").split(",").each(function(n){Scriptaculous.require(i+n+".js")})})}};Scriptaculous.load()