$(document).ready(function () {
    $('#btn_Save').click(function () {
        SaveAddress($(this))
    });
    function SaveAddress(Button) {
        //var dataObject = JSON.stringify(
        //    {
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
            if (Result.Status == "Success") {
                $('#itemCartMessage').text(Result.Message);
                $(Button).prop("disabled", false);
                $("#succes-add-to-cart").modal('show');
            }
            else {
                $('#itemCartMessage').text(Result.Message);
                $(Button).prop("disabled", false);
                alert(Result.Message);
            }
        });
        window.location.href = "/Home/MyAddresses";
    }
    $('.edit-address').click(function () {
        val = $(this).attr("attb");
        //document.getElementById(val).removeClass("disabled-div");
        //$('#'+val).removeClass("disabled-div");
        var elem = $("#" + val);
        var item=document.getElementById('new 43')
        if (elem.hasClass("disabled-div")) {
            elem.removeClass('disabled-div');
           // $elem.addClass('yes');
        }

        else if (elem.hasClass("yes")) {
            //$elem.removeClass('yes');
            //$elem.addClass('no');
        }


    });

});