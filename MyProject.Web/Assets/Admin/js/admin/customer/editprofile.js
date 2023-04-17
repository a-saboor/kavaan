$(document).ready(function () {
    $('#EmiratesID').inputmask();

    //$('#btnFormSubmit').submit(function () {
    //    
    //    var e = $("#btnSaveFacility");
    //    $(e).addClass('spinner spinner-light spinner-left').prop('disabled', true);
    //    $(e).find('i').hide();

    //    $.ajax({
    //        url: '/Admin/Customer/Update/',
    //        type: 'POST',
    //        data: {
    //            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
    //            customer: {

    //                ID: GetURLParameter(),
    //                FirstName: $('input[name=FirstName]').val(),
    //                LastName: $('input[name=LastName]').val(),
    //                UserName: $('input[name=UserName]').val(),
    //                CustomerCountry: $('input[name=ContactNo]').val(),
    //                CustomerCity: $('input[name=CustomerCity]').val(),
    //                Email: $('input[name=Email]').val(),
    //                Password: $('input[name=Password]').val(),
    //                PhoneCode: $('input[name=PhoneCode]').val(),
    //                Contact: $('input[name=Contact]').val(),
    //                ZipCode: $('input[name=ZipCode]').val(),
    //                EmiratesID: $('input[name=EmiratesID]').val(),
    //                PoBox: $('input[name=PoBox]').val(),
    //                PassportNo: $('input[name=PassportNo]').val(),
    //                Address2: $('textarea[name=Address2]').val(),
    //                Address: $('textarea[name=Address]').val(),
    //                EmiratesIDExpiry: $('input[name=EmiratesIDExpiry]').val(),
    //                PassportExpiry: $('input[name=PassportExpiry]').val(),
    //            }
    //        },
    //        success: function (response) {
    //            if (response.success) {
    //                toastr.success(response.message);
    //            }
    //            else {
    //                toastr.error(response.message);
    //            }
    //            $(e).find('i').show();
    //            $(e).removeClass('spinner spinner-light spinner-left').prop('disabled', false);
    //        },
    //        error: function (e) {
    //            $(e).find('i').show();
    //            $(e).removeClass('spinner spinner-light spinner-left').prop('disabled', false);
    //            toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
    //        },
    //        failure: function (e) {
    //            $(e).find('i').show();
    //            $(e).removeClass('spinner spinner-light spinner-left').prop('disabled', false);
    //            toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
    //        }
    //    });

    //    return false;
    //});

});

function GetURLParameter() {
    var sPageURL = window.location.href;
    var indexOfLastSlash = sPageURL.lastIndexOf("/");

    if (indexOfLastSlash > 0 && sPageURL.length - 1 != indexOfLastSlash)
        return sPageURL.substring(indexOfLastSlash + 1);
    else
        return 0;
}