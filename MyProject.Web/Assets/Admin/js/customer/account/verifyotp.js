'use strict'
var resendingOtp = false;

$(document).ready(function () {

    if ($('input[name="OTPExpired"]').val().toLowerCase() == "true") {
        timerOTP(0, $('#form_otp_verification'));
    }
    else {
        timerOTP(5 * 60, $('#form_otp_verification'));
    }

    /*otp insertion*/
    $('.digit-group').find('input').each(function () {
        $(this).attr('maxlength', 1);
        $(this).on('keyup', function (e) {
            var form = $($(this).closest('form'));
            if (e.keyCode === 8 || e.keyCode === 37) {
                var prev = $(this).prev('input');

                if (prev.length) {
                    $(prev).select();
                }
            } else if ((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 65 && e.keyCode <= 90) || (e.keyCode >= 96 && e.keyCode <= 105) || e.keyCode === 39) {
                var next = $(this).next('input');

                if (next.length) {
                    $(next).select();
                } else {

                    let OTP = "";
                    $(form).find('.digit-group').find('input').each(function () {
                        OTP = OTP + $(this).val();
                    });
                    $('input[name="OTP"]').val(OTP);

                    if ($(form).attr('data-submission') === 'autosubmit') {
                        //form.submit();
                        $('button[form="form_otp_verification"]').trigger("click");
                    }
                }
            }
        });
    });

    //form
    $('button[form="form_otp_verification"]').click(function () {
        const btn = $(this);
        const url = `/customer/account/verifyOTP`;
        const showSpinner = true;
        const showAlert = true;
        const showToastr = false;
        const btnSpinner = true;
        const msg = "Please fill the form properly...";
        const form = $('form[id="form_otp_verification"]');
        if (formValidate(form, showAlert, showToastr, msg)) {
            simpleFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner);
        }
        return false;
    });

    //resend otp
    $('.resend-code').click(function () {
        const btn = $('button[form="form_otp_verification"]');
        const url = `/customer/account/resendOTP`;
        const showSpinner = false;
        const showAlert = true;
        const showToastr = false;
        const btnSpinner = false;
        //const msg = "Please fill the form properly...";
        const form = $('form[id="form_otp_verification"]');
        if (!resendingOtp) {
            resendingOtp = true;
            resendOTPFunc(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner);
        }
        return false;
    });

});

function timerOTP(time, form) {

    let timerOn = true;

    function timer(remaining) {
        var m = Math.floor(remaining / 60);
        var s = remaining % 60;

        m = m < 10 ? '0' + m : m;
        s = s < 10 ? '0' + s : s;
        $(form).find('.timer').text(m + ':' + s);
        remaining -= 1;

        if (remaining >= 0 && timerOn) {
            setTimeout(function () {
                timer(remaining);
            }, 1000);
            return;
        }

        if (!timerOn) {
            // Do validate stuff here
            return;
        }

        // Do timeout stuff here
        //alert('Timeout for otp');
        EnableOtp(false, form);
    }

    timer(time); //seconds
}

function simpleFormSubmit_callback(form, status, response, url) {
    if (status) {
        setTimeout(function () {
            window.location.href = url;
        }, 3000);
    }
}

function resendOTPFunc(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner) {

    disableSubmitButton(btn, true, '', showSpinner);
    ToastrMessage(icon_info, msg_loading, "", 6);

    EnableOtp(true, form);

    $.ajax({
        url: '/Customer/Account/resendOTP',
        type: 'Post',
        data: {
            __RequestVerificationToken: $(form).find('input[name="__RequestVerificationToken"]').val(),
            email: $('input[name="Email"]').val(),
        },
        success: function (response) {

            if (response.success) {
                ToastrMessageHide();

                //success message
                if (response.message) {
                    showFormMessage(showAlert, showToastr, form, icon_success, response.message, (response.url ? response.url : ""), 8);
                }

                //empty form fields
                $(form).find('.form-field').val('').removeClass("border-success border-danger");
                EnableOtp(true, form);
                timerOTP(5 * 60, form);

            } else {
                AlertMessage(form, fa_icon_warning, response.message, '', 6);
                EnableOtp(false, form);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorFunction(xhr, ajaxOptions, thrownError, btn);
            EnableOtp(false, form);
        },
        failure: function (xhr, ajaxOptions, thrownError) {
            ErrorFunction(xhr, ajaxOptions, thrownError, btn);
            EnableOtp(false, form);
        }
    });

    resendingOtp = false;
}

function EnableOtp(flag, form) {

    if (flag) {
        $(form).find('.resend-code').hide();
        $(form).find('.digit-group input').attr('disabled', false);
        $(form).find('.timer').removeClass('text-danger');
        $(form).find('button[form="form_otp_verification"]').attr('disabled', false);
    }
    else {
        $(form).find('.resend-code').show();
        $(form).find('.digit-group input').attr('disabled', true);
        $(form).find('.timer').addClass('text-danger');
        $(form).find('button[form="form_otp_verification"]').attr('disabled', true);
    }

}

/*
$(document).ready(function () {
    if (window.location.href.includes("#")) {

        var url = window.location.href;
        var Contact = url.substring(url.indexOf('#') + 1);
        $("#otpContact").html(Contact);
        history.pushState({}, null, window.location.href.replace("#" + Contact, ""));
        TimerOTP(5);
    }
    else {
        TimerOTP(120);
    }

    $('.digit-group').find('input').each(function () {
        $(this).attr('maxlength', 1);
        $(this).on('keyup', function (e) {
            var form = $($(this).form());

            if (e.keyCode === 8 || e.keyCode === 37) {
                var prev = form.find('input#' + $(this).data('previous'));

                if (prev.length) {
                    $(prev).select();
                }
            } else if ((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 65 && e.keyCode <= 90) || (e.keyCode >= 96 && e.keyCode <= 105) || e.keyCode === 39) {
                var next = form.find('input#' + $(this).data('next'));

                if (next.length) {
                    $(next).select();
                } else {
                    if (form.data('autosubmit')) {
                        form.submit();
                    }
                }
            }
        });
    });

    //OTP Verification
    $('#OTPVerificationForm').submit(function () {
        var form = $(this);
        $('#btnOtpVerfication').html('<span class="fa fa-spinner fa-spin"></span> Submit').attr('disabled', true);

        var OTP = "";
        $('.digit-group').find('input').each(function () {
            OTP = OTP + $(this).val();
        });
        var otp = Number(OTP);
        var contact = $('#otpContact').html();
        contact = contact.replace("+", "");

        if (otp == null || otp == NaN || otp == "") {
            showErrorMsg(form, 'danger', "Invalid OTP");
            $('#btnOtpVerfication').html('Submit').attr('disabled', false);
        }
        else if (contact == null || contact == "") {
            showErrorMsg(form, 'danger', "Invalid Contact number");
            $('#btnOtpVerfication').html('Submit').attr('disabled', false);
        }
        else {
            $.ajax({
                url: $(form).attr('action'),
                type: 'Post',
                data: {
                    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                    Contact: contact,
                    otp: otp,
                },
                success: function (response) {
                    if (response.success) {
                        showErrorMsg(form, 'success', response.message);
                        setTimeout(function () {
                            window.location.href = response.url;
                        }, 3000);
                    } else {
                        showErrorMsg(form, 'danger', response.message);
                        $('#btnOtpVerfication').html('Submit').attr('disabled', false);
                        if (response.description) {
                            $(form).prepend$(response.description);
                        }
                    }
                }
            });
        }
        return false;
    });
    //OTP Verification

    //OTP Resend


    $('#otpResend').click(function () {

        var form = $(this).closest('form');

        var contact = $('#otpContact').html();
        $('#otpResend').hide();
        $('#btnOtpVerfication').html('<span class="fa fa-circle-notch fa-spin"></span> Submit').attr('disabled', true);
        $.ajax({
            url: '/Customer/Account/resendOTP',
            type: 'Post',
            data: {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                Contact: contact,
            },
            success: function (response) {

                if (response.success) {
                    showErrorMsg(form, 'success', response.message);
                    $('#btnOtpVerfication').html('Submit').attr('disabled', false).show();
                    TimerOTP(120); // Countdown Timer
                } else {
                    showErrorMsg(form, 'danger', response.message);
                    $('#otpResend').show();
                }
            },
            error: function (e) {
                showErrorMsg(form, 'danger', "Ooops, something went wrong.Try to refresh this page or feel free to contact us if the problem persists.");
                $('#btnOtpVerfication').html('Submit').attr('disabled', false).show();
                $('#otpResend').show();
            },
            failure: function (e) {
                showErrorMsg(form, 'danger', "Ooops, something went wrong.Try to refresh this page or feel free to contact us if the problem persists.");
                $('#btnOtpVerfication').html('Submit').attr('disabled', false).show();
                $('#otpResend').show();
            }
        });
        return false;
    });

    //OTP Resend
});

var showErrorMsg = function (form, type, msg) {
    var alert = $('<div class="alert kt-alert kt-alert--outline alert alert-' + type + ' " role="alert">\
            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><i class="fa fa-times"></i></button>\
            <span></span>\
        </div>');

    form.find('.alert').remove();
    alert.prependTo(form);
    //KTUtil.animateClass(alert[0], 'fadeIn animated');
    $(alert).show();
    setTimeout(function () {
        $(alert).hide();
    }, 6000);
    alert.find('span').html(msg);
}

function TimerOTP(time) {

    let timerOn = true;

    function timer(remaining) {
        var m = Math.floor(remaining / 60);
        var s = remaining % 60;

        m = m < 10 ? '0' + m : m;
        s = s < 10 ? '0' + s : s;
        document.getElementById('timer').innerHTML = m + ':' + s;
        remaining -= 1;

        if (remaining >= 0 && timerOn) {
            setTimeout(function () {
                timer(remaining);
            }, 1000);
            return;
        }

        if (!timerOn) {
            // Do validate stuff here
            return;
        }

        // Do timeout stuff here
        //alert('Timeout for otp');
        $('#btnOtpVerfication').attr('disabled', true).hide();
        $('#otpResend').show();
    }

    timer(time); //seconds
}
*/