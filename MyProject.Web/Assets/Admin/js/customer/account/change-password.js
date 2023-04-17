'use strict';

$(document).ready(function () {

    initAllFunc();

    //contact us form / enquiry
    $('button[form="form_change_password"]').click(function () {
        const btn = $(this);
        const url = `/customer/account/changepassword`;
        const showSpinner = true;
        const showAlert = true;
        const showToastr = false;
        const btnSpinner = true;
        const msg = "Please fill the form properly...";
        const form = $('form[id="form_change_password"]');
        if (formValidate(form, showAlert, showToastr, msg)) {
            simpleFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner);
        }
        return false;
    });

});