$('#btn_OrderNow').click(function () {

    PlaceOrderNow($(this));
        
});
function removeCart(itemId){
   // console.log($(this).attr('attr'));
    console.log(itemId);
    var Url = '/Home/RemoveItemCart';
    AddItemCart(itemId, Url,false);
}


function PlaceOrderNow(Button) {
    var cartSpiceModel = [];
    
    $(".dd1").each(function (item, x) {
        var index = $(this).context.id;
        var spices = $("#" + index).find("option:selected").text();
        var cartId =$("#" + index).attr("valu");
        var cartitem = { CartId: cartId, Spices: $.trim(spices) };
        cartSpiceModel.push(cartitem);
    });
    cartSpiceModel = JSON.stringify({ 'cartSpiceModel': cartSpiceModel });
    $(Button).prop("disabled", true);
    $.ajax({
        type: "POST",
        data: cartSpiceModel,
        contentType: 'application/json; charset=utf-8',
        url: "/Home/OrderNow",
        dataType: 'json',
        success: function (response) {
            if (response.Status == "Success") {
                //$("#succes-Order_placed").modal('show');
                //$(Button).prop("disabled", false);
                ////alert(response.Message);
                window.location.href = "/Home/Order";
            } else {
                //$(Button).prop("disabled", false);
                //$("#succes-Order_placed").modal('hide');

                //alert(response.responseText);
            }
        },
        error: function (response) {
            alert("error!");  // 
        }

    });
}
function AddItemCart(itemId, Url, status) {
    var haserror = false;
    var UserId = $("#hdnUserId").val();
    if (UserId === "0") {
        haserror === true;
        $("#LoginModel").modal('show');
    }
    else {
        if (haserror === false) {
            $('#spanSuccess').addClass('hide');
            $.post(Url, { UserId: UserId, ItemId: itemId }, function (Result) {
                if (Result.Status === "Success") {
                    $('#itemCartMessage').text(Result.Message);
                    $("#succes-add-to-cart").modal('show');
                    ////setTimeout(
                    ////    function () {
                    ////        location.reload();
                    ////    }, 5000);
                    location.reload();
                }
                else {
                      location.reload();
                }
            });
        }
    }

}