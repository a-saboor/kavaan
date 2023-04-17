'use strict'

$(document).ready(function () {

    //contact us form / enquiry
    $('button[form="form_forgot_password"]').click(function () {
        const btn = $(this);
        const url = `/customer/account/forgotpassword`;
        const showSpinner = true;
        const showAlert = true;
        const showToastr = false;
        const btnSpinner = true;
        const msg = "Please fill the form properly...";
        const form = $('form[id="form_forgot_password"]');
        if (formValidate(form, showAlert, showToastr, msg)) {
            simpleFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner);
        }
        return false;
    });

});

function simpleFormSubmit_callback(form, status, response, url) {
    if (response.verifyAgain) {
        setTimeout(function () {
            window.location.href = url;
        }, 3000);
    }
}



/*
$(document).ready(function () {
    $('#CustomerForgetPasswordForm').submit(function () {
        var form = $(this);
        $('#btnSubmit').html('<span class="fa fa-spinner fa-spin"></span> Submit').attr('disabled', true);
        $.ajax({
            url: $(form).attr('action'),
            type: 'Post',
            data: $(form).serialize(),
            success: function (response) {
                if (response.success) {
                    showErrorMsg(form, 'success', response.message);
                } else {
                    showErrorMsg(form, 'danger', response.message);
                }
                $('#btnSubmit').html('Submit').attr('disabled', false);
            }
        });
        return false;
    })
});

var showErrorMsg = function (form, type, msg) {
    var alert = $('<div class="alert kt-alert kt-alert--outline alert alert-' + type + ' " role="alert">\
            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><i class="fa fa-times"></i></button>\
            <span></span>\
        </div>');

    form.find('.alert').remove();
    alert.prependTo(form);
    //KTUtil.animateClass(alert[0], 'fadeIn animated');
    $(alert).slideDown();
    alert.find('span').html(msg);
}
*/