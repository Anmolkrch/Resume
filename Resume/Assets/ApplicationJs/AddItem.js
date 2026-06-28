$(document).ready(function () {

    $(".cart").click(function () {
        var Url = '/Home/AddItemCart';
        AddItemCart($(this), Url, true);
    });

    $(".removeCart").click(function () {
        var Url = '/Home/RemoveItemCart';
        AddItemCart($(this), Url, false);
    });
    //$("#LoginModel").modal('hide');
    function AddItemCart(Button, Url, status) {
        var haserror = false;
        var UserId = $("#hdnUserId").val();
        var ItemId = Button.attr('attr');
       /// addItemLocalStorage(UserId, ItemId, status);
        if (UserId === "0") {
            haserror === true;
            $("#LoginModel").modal('show');
        }
        else {
            if (haserror === false) {
                $('#spanSuccess').addClass('hide');
                $.post(Url, { UserId: UserId, ItemId: ItemId }, function (Result) {
                    if (Result.Status === "Success") {
                        $('#itemCartMessage').text(Result.Message);
                        $(Button).prop("disabled", false);
                        $("#succes-add-to-cart").modal('show');
                        Button.next(".addcart").removeClass('display-none');
                        var htm = Button.next(".addcart");
                        if (Button.context.hasAttribute('full')) {
                            Button.addClass('display-none');
                        }
                        if (status && !Button.context.hasAttribute('full')) {
                            
                            htm.context.previousElementSibling.innerHTML = Result.count
                        }
                        else if (status) {
                            if (!htm.context.previousElementSibling.hasAttribute("attr")) {
                                htm.context.previousElementSibling.innerHTML = Result.count
                            }

                        }
                        else {
                            if (Result.count == "0") {
                                htm.context.parentElement.classList.add("display-none");
                                Button.context.parentElement.parentElement.firstElementChild.
                                    nextElementSibling.nextElementSibling.classList.remove("display-none");
                            }
                            else {
                                htm.context.nextElementSibling.innerHTML = Result.count;
                            }
                            
                        }
                    }
                    else {
                        $('#spanSuccess').removeClass('hide');
                        $('#spanSuccess').removeClass('alert-success');
                        $('#spanSuccess').addClass('alert-danger');
                        $('#spanSuccess').text(Result.Message);
                        $(Button).prop("disabled", false);
                        alert(Result.Message);
                    }
                });
            }
        }

    }
    function addItemLocalStorage(UserId, ItemId, status) {
        // Check browser support
        if (typeof (Storage) !== "undefined") {
            // Store
            if (localStorage.getItem('Item'+UserId) !== null && localStorage.getItem('Item' + UserId)!=="") {
                appendToStorage('Item' + UserId, UserId + "-" + ItemId);
            }
            localStorage.setItem('Item'+UserId, UserId +"-"+ ItemId);

            alert(localStorage.getItem('Item'+UserId));
            // Retrieve
            //document.getElementById("result").innerHTML = localStorage.getItem("Item"+UserId);
        } else {
            //document.getElementById("result").innerHTML = "Sorry, your browser does not support Web Storage...";
        }

        function appendToStorage(name, data) {
            var old = localStorage.getItem(name);
            if (old === null) old = "";
            localStorage.setItem(name, old + data);
        }
    }
    var $btns = $('.btnCategory').click(function () {
        if (this.id == '0') {
            $('#ct > div').show();
        } else {
            var $el = $('.' + this.id).show();
            $('#ct > div').not($el).hide();
        }
        $btns.removeClass('active');
        $btns.find('.btn-category').removeClass('btn-category-bgcolor');
        $(this).addClass('active');
        $(this).find('.btn-category').addClass('btn-category-bgcolor');
    })
});