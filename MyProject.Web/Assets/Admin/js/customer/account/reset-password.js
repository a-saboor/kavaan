'use strict';

$(document).ready(function () {

    //contact us form / enquiry
    $('button[form="form_reset_password"]').click(function () {
        const btn = $(this);
        const url = `/customer/account/resetpassword`;
        const showSpinner = true;
        const showAlert = true;
        const showToastr = false;
        const btnSpinner = true;
        const msg = "Please fill the form properly...";
        const form = $('form[id="form_reset_password"]');
        if (formValidate(form, showAlert, showToastr, msg)) {
            simpleFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner);
        }
        return false;
    });

});


function simpleFormSubmit_callback(form, status, response, url) {
    if (status) {
        setTimeout(function () {
            window.location.href = url;
        }, 3000);
    }
}
