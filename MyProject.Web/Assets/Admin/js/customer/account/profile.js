'use strict';

//#region global variables and arrays

//#endregion

//#region document ready function

$(document).ready(function () {
    initAllFunc();
    //setTimeout(function () {
    //	$('#form_account').find('.Contact').attr('disabled',true);
    //}, 750);
    //submit form
    $('button[form="form_account"]').click(function () {
        const btn = $(this);
        const url = `/customer/account/profile`;
        const showSpinner = true;
        const showAlert = true;
        const showToastr = false;
        const btnSpinner = true;
        const msg = "Please fill the form properly...";
        const form = $('form[id="form_account"]');
        scrollTop(75);
        if (formValidate(form, showAlert, showToastr, msg)) {
            simpleFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner);
        }
        return false;
    });

});

//#endregion

//#region Other functions
function simpleFormSubmit_callback(form, success, response, url) {
    if (status) {
        setTimeout(function () {
            location.reload();
        }, 1000);
    }
}
//#endregion

