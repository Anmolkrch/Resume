$(document).ready(function () {
    function Post(Button) {
        var haserror = false;
        $(Button).prop("disabled", true);
        var ErrorMessage = "Please fill following ";
        var Name = $('#txtName').val()
        var Email = $('#txtEmail').val()
        var Phone = $('#txtPhone').val()
        var Message = $('#txtMessage').val()
        var Url = '/Home/Contact'
        if (Name == "") {
            haserror = true;
            ErrorMessage = ErrorMessage + " Name \n"
            $(Button).prop("disabled", false);
        }
        if (Email == "") {
            haserror = true;
            ErrorMessage = ErrorMessage + " Email \n"
            $(Button).prop("disabled", false);
        }
        else if (!ValidateEmail(Email)) {
            haserror = true;
            ErrorMessage = ErrorMessage + " Email(Invalid) \n"
            $(Button).prop("disabled", false);
        }
        if (Phone == "") {
            haserror = true;
            ErrorMessage = ErrorMessage + " Phone \n"
            $(Button).prop("disabled", false);
        }
        else if (!ValidatePhone(Phone)) {
            haserror = true;
            ErrorMessage = ErrorMessage + " Phone(*Invalid) \n"
            $(Button).prop("disabled", false);
        }
        if (Message == "") {
            haserror = true;
            ErrorMessage = ErrorMessage + " Message \n"
            $(Button).prop("disabled", false);
        }
        if (haserror == true) {
            $('#spanSuccess').removeClass('hide')
            $('#spanSuccess').removeClass('alert-success')
            $('#spanSuccess').addClass('alert-danger')
            $('#spanSuccess').text(ErrorMessage)
        }
        if (haserror == false) {
            $('#spanSuccess').addClass('hide')
            $.post(Url, { Name: Name, Email: Email, PhoneNo: Phone, Message: Message }, function (Result) {
                if (Result.Status == "Success") {
                    $('#spanSuccess').removeClass('hide')
                    $('#spanSuccess').removeClass('alert-danger')
                    $('#spanSuccess').addClass('alert-success')
                    $('#spanSuccess').text(Result.Message);
                    $(Button).prop("disabled", false);
                    reset();
                }
                else {
                    $('#spanSuccess').removeClass('hide')
                    $('#spanSuccess').removeClass('alert-success')
                    $('#spanSuccess').addClass('alert-danger')
                    $('#spanSuccess').text(Result.Message)
                    $(Button).prop("disabled", false);
                }
            });
        }
    }

    function ValidateEmail(emailid) {
        var expr = /^([\w-\.]+)@@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        return expr.test(emailid);
    };
    function ValidatePhone(p) {
        //var phoneRe = /^[2-9]\d{2}[2-9]\d{2}\d{4}$/;
        var phoneRe = /[0-9]{10}/;
        var digits = p.replace(/\D/g, "");
        return phoneRe.test(digits);
    };
    function reset() {
        $('input[type=text]').val('');
        $('input[type=email]').val('');
        $('#txtMessage').val('');
        $('input[type=select]').val('');
        $('input[type=radio]').val('');
        $('input[type=checkbox]').val('');
    }
    $('#btn_Booking').click(function () {
        Post($(this))
    });

    $(document).keypress(function (e) {
        if (e.which == 13) {
            Post($('#btn_Booking'))
        }
    });
    function changeBorder(element, to) {
        element.style.borderColor = to;
    }

});
