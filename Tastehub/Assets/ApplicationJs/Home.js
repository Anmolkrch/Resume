$(document).ready(function () {
    function Post(Button) {
        var haserror = false;
        $(Button).prop("disabled", true);
        var ErrorMessage = "Please fill following ";
        var Name = $('#txtName').val()
        var Email = $('#txtEmail').val()
        var Phone = $('#txtPhone').val()
        var Date = $('.txtBookingDate ').val()
        var Time = $('#txtTime').val()
        var People = $('#country1').val()
        var Url = '/Home/BookingRequest'
        if (Name == "") {
            haserror = true;
            ErrorMessage = ErrorMessage + " Name \n"
            $(Button).prop("disabled", false);
            // changeBorder(this, 'red');
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
        if (Date == "") {
            haserror = true;
            ErrorMessage = ErrorMessage + " Date \n"
            $(Button).prop("disabled", false);
        }
        if (Time == "") {
            haserror = true;
            ErrorMessage = ErrorMessage + " Time \n"
            $(Button).prop("disabled", false);
        }
        if (People == "") {
            haserror = true;
            ErrorMessage = ErrorMessage + " People \n"
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
            $.post(Url, { Name: Name, Email: Email, PhoneNo: Phone, People: People, Date: Date.toString(), Time: Time }, function (Result) {
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
        var expr = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        //var expr = /^([\w-\.]+)@@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
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
        $('#textarea').val('');
        $('input[type=select]').val('');
        $('input[type=radio]').val('');
        $('input[type=checkbox]').val('');
    }
    $('#btn_Booking').click(function () {
        Post($(this));
    });
    $('#btn_Login').click(function () {
        Login($(this));
    });
    $(document).keypress(function (e) {
        if (e.which == 13) {
            Post($('#btn_Booking'));
        }
    });
    function changeBorder(element, to) {
        element.style.borderColor = to;
    }
    function Login(Button) {
        var haserror = false;
        $(Button).prop("disabled", true);
        var ErrorMessage = "Please enter ";
        var Email = $('#txtLoginEmail').val();
        var Password = $('#Password').val();
        var Url = '/Home/Login';
        if (Email === "") {
            haserror = true;
            ErrorMessage = ErrorMessage + " email \n";
            $(Button).prop("disabled", false);
        }
        else if (!ValidateEmail(Email)) {
            haserror = true;
            ErrorMessage = ErrorMessage + " Email(Invalid) \n";
            $(Button).prop("disabled", false);
        }
        if (Password == "") {
            haserror = true;
            ErrorMessage = ErrorMessage + " password \n";
            $(Button).prop("disabled", false);
        }
        if (haserror == true) {
            $('#spanSuccessLogin').removeClass('hide');
            $('#spanSuccessLogin').removeClass('alert-success');
            $('#spanSuccessLogin').addClass('alert-danger');
            $('#spanSuccessLogin').text(ErrorMessage);
        }
        if (haserror == false) {
            $('#spanSuccessLogin').addClass('hide')
            $.post(Url, { Email: Email, Password: Password }, function (Result) {
                if (Result.Status == "Success") {
                    $('#spanSuccessLogin').text(Result.Message);
                    $('#spanSuccessLogin').removeClass('alert-danger');
                    $('#spanSuccessLogin').addClass('alert-success');
                    $('#spanSuccessLogin').removeClass('hide');
                    $(Button).prop("disabled", false);
                    $("#LoginModel").modal('hide');
                    if (Result.Role === "Admin" || Result.Role === "SuperAdmin") {
                        location.href = '/Account/Login';
                    }
                    else
                        location.href = '/Home/Item';
                }
                else {
                    $('#spanSuccessLogin').removeClass('hide');
                    $('#spanSuccessLogin').removeClass('alert-success');
                    $('#spanSuccessLogin').addClass('alert-danger');
                    $('#spanSuccessLogin').text(Result.Message);
                    $(Button).prop("disabled", false);
                    $("#LoginModel").modal('show');
                }
            });
        }
    }
});