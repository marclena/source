function LoadPoints(){$("#siHayResultados").css("display","none");var n=GetFilterPoints();GetDetailOfPoints(n)}function GetFilterPoints(){var n={};return email=n.Email=$("#emailInput").val()!=""?$("#emailInput").val():null,customerNumber=n.CustomerNumber=$("#customerNumberInput").val()!=""?$("#customerNumberInput").val():null,n}function FilterPointsNotEmpty(n){return n.Email!=null||n.CustomerNumber!=null?!0:!1}function SetDetailOfPoints(n){$("#purchasedPoints").html(n==null?"":n.PurchasedPoints);$("#giftedPoints").html(n==null?"":n.GiftedPoints);$("#receivedPoints").html(n==null?"":n.ReceivedPoints);$("#callCenterPoints").html(n==null?"":n.CallCenterPoints);$("#totalPoints").html(n==null?"":n.TotalPoints)}function GetDetailOfPoints(n){FilterPointsNotEmpty(n)&&($.ajax({type:"POST",url:"GetPointsByNavitaire",contentType:"application/json; charset=utf-8",data:JSON.stringify(n),dataType:"json",success:function(n){n==null?($("#siHayResultados").css("display","none"),$("#noHayResultados").css("display",""),setTimeout(function(){$("#noHayResultados").css("display","none")},5e3)):($("#siHayResultados").css("display",""),$("#limiteWebCantidadSpan").html(n),$("#limiteWebCantidadInput").val(n),$("#limiteWeb").css("display",""))}}),GetAmountPointsByCustomer(n))}function GetAmountPointsByCustomer(n){$.ajax({type:"POST",url:"GetAmountPointsByCustomer",contentType:"application/json; charset=utf-8",data:JSON.stringify(n),dataType:"json",success:function(n){$("#historico").css("display","");$("#result").css("display","");$("input:radio[id=purchasedPointsRadioButton]").click();SetDetailOfPoints(n)}})}function EditPoints(){$("#limiteWebCantidadInput").css("display","");$("#limiteWebCantidadSpan").css("display","none");$("#SavePoints").css("display","")}function SavePoints(){var t=$("#limiteWebCantidadInput").val(),n;if(isNaN(t)){alert("Debe ser un número");return}if(t<0||t>1e4){alert("El número debe estar entre 0 y 10000");return}n={};n.Email=email;n.CustomerNumber=customerNumber;n.PromotionCode=t;$.ajax({type:"POST",url:"SavePointsTransaction",contentType:"application/json; charset=utf-8",data:JSON.stringify(n),dataType:"json",success:function(t){t.Error?(EditPoints(),alert("Los puntos NO han sido guardados")):(GetAmountPointsByCustomer(n),alert("Los puntos han sido guardados correctamente"))}});$("#limiteWebCantidadInput").css("display","none");$("#limiteWebCantidadSpan").css("display","");$("#limiteWebCantidadSpan").html(t);$("#SavePoints").css("display","none")}function GridAnualHistoric_onDataBinding(n){var t=GetFilterPoints();n.data={email:t.Email,customerNumber:t.CustomerNumber,type:$("input[name=points]:checked","#historico").val()}}var email,customerNumber