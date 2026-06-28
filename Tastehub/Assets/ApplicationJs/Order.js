$(document).ready(function () {
    $('#btn_Save').click(function () {
        SaveAddress($(this));
    });
    function SaveAddress(Button) {
        var CurAddress1 = $('#CurAddress1').val();
        var CurAddress2 = $('#CurAddress2').val();
        var CurCity = $('#CurCity').val();
        var CurZipCode = $('#CurZipCode').val();
        var ProfileUrl = $('#ProfileUrl').val();
        var isValid = true;
        if (CurAddress1 == null || CurAddress1 == "") {
            alert("please enter current address");
            isValid = false;
        }
        else if (CurAddress1 == null || CurAddress1 == "") {
            alert("please enter current address details"); isValid = false;
        }
        else if (CurAddress2 == null || CurAddress2 == "") {
            alert("please enter current address details"); isValid = false;
        }
        else if (CurZipCode == null || CurZipCode == "") {
            alert("please enter ZipCode"); isValid = false;
        }
        else if (ProfileUrl == null || ProfileUrl == "") {
            alert("please enter Mobile number"); isValid = false;
        }
        else if (ProfileUrl != null || ProfileUrl == "") {
            var filter = /^((\+[1-9]{1,4}[ \-]*)|(\([0-9]{2,3}\)[ \-]*)|([0-9]{2,4})[ \-]*)*?[0-9]{3,4}?[ \-]*[0-9]{3,4}?$/;
            if (filter.test(ProfileUrl)) {
                isValid = true;
            }
            else {
                isValid = false;
                alert("please enter valid Mobile number");
            }
        }
        if (isValid) {
            var userDetailViewModel = {
                'Id': $('#AddressId').val(),
                'CurAddress1': $('#CurAddress1').val(),
                'CurAddress2': $('#CurAddress2').val(),
                'CurCity': $('#CurCity').val(),
                'CurZipCode': $('#CurZipCode').val(),
                'ProfileUrl': $('#ProfileUrl').val(),
                'IsDefault': true,
                // }
            };
            var Url = '/Home/SaveMyAdress';
            $.post(Url, {
                Id: userDetailViewModel.Id, CurAddress1: userDetailViewModel.CurAddress1,
                CurAddress2: userDetailViewModel.CurAddress2, CurCity: userDetailViewModel.CurCity,
                CurZipCode: userDetailViewModel.CurZipCode, ProfileUrl: userDetailViewModel.ProfileUrl, IsDefault: userDetailViewModel.IsDefault
            }, function (Result) {
                if (Result.Status === "Success") {
                    $('#itemCartMessage').text(Result.Message);
                    $(Button).prop("disabled", false);
                    $("#succes-add-to-cart").modal('show');
                    location.reload(true);
                }
                else {
                    $('#itemCartMessage').text(Result.Message);
                    $(Button).prop("disabled", false);
                    alert(Result.Message);
                    location.reload(true);
                }
            });
        }

    };

    $('#btn_PlaceOrder').click(function () {
        var hdnAddress = $('#hdnAddressId').val();
        var hdnPaymentType = $('input[type=radio][name=paymentRadio]:checked').val();
        if ($('#addresscount').val() != "0") {
            OrderNow($(this), hdnAddress, hdnPaymentType);
        }
        else {
            alert('please save address before placing order');
        }
    });
    function OrderNow(Button, hdnAddress, hdnPaymentType) {
        var myKeyVals = { AddressId: hdnAddress, PaymentType: hdnPaymentType };
        $(Button).prop("disabled", true);
        $.ajax({
            type: "POST",
            data: myKeyVals,
            url: "/Home/PlaceOrder",
            dataType: 'json',
            success: function (response) {
                if (response.Status) {
                    $("#succes-Order_placed").modal('show');
                    $(Button).prop("disabled", false);
                    //alert(response.Message);
                    setTimeout(
                        function () {
                            window.location.href = "/Home/MyOrder";
                        }, 2000);
                    
                } else {
                    $(Button).prop("disabled", false);
                    $("#succes-Order_placed").modal('hide');

                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert("error!");  // 
            }

        });
    }
    $('input[type=radio][name=paymentRadio]').on('change', function () {
        switch ($(this).val()) {
            case 'PayOndelivery':
                $('#dvCOD').removeClass('display-none');
                $('#dvCOD').addClass('display');
                $('#dvCard').addClass('display-none');
                break;
            case 'Card':
                $('#dvCard').removeClass('display-none');
                $('#dvCard').addClass('display');
                $('#dvCOD').addClass('display-none');
                break;
        }
    });
    $('.edit-address').click(function () {
        //val = $(this).attr("attb");
        ////document.getElementById(val).removeClass("disabled-div");
        ////$('#'+val).removeClass("disabled-div");
        //var elem = $("#" + val);

        //if (elem.hasClass("disabled-div")) {
        //    elem.removeClass('disabled-div');
        //   // $elem.addClass('yes');
        //}

        //else if (elem.hasClass("yes")) {
        //    //$elem.removeClass('yes');
        //    //$elem.addClass('no');
        //}

    
    });
    
});