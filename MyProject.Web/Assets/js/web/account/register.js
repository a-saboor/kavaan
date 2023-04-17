'use strict'
//#region Global Variables and Arrays

const LoginForm = $('#register_section .login-div form');
const SignupForm = $('#register_section .signup-div form');
const EmailForm = $('#register_section .email-verify-div form');
const OtpForm = $('#register_section .otp-div form');
const ResetPasswordForm = $('#register_section .reset-password-div form');
const NewPasswordForm = $('#register_section .new-password-div form');

var currentEmail = "";
var currentCode = "";
var currentContact = "";
var bookingPopupAfterLogin = false;
var bookingServiceID = 0;

//#endregion

//#region document ready function
$(document).ready(function () {

	/*login*/
	$('#register_section .login-div form').submit(function () {
		var form = $(this);
		if (validateForm(form, 'input', 1)) {
			AlertMessage(form, fa_icon_info, msg_loading, '', 6);
			loginSubmit(form);
		}
		return false;
	});

	/*signup*/
	$('#register_section .signup-div form').submit(function () {
		var form = $(this);
		if (validateForm(form, 'input', 1)) {
			AlertMessage(form, fa_icon_info, msg_loading, '', 6);
			signupSubmit(form);
		}
		return false;
	});

	/*otp verification*/
	$('#register_section .otp-div form').submit(function () {
		var form = $(this);
		AlertMessage(form, fa_icon_info, msg_loading, '', 6);
		otpSubmit(form);
		return false;
	});

	/*otp insertion*/
	$('#register_section .otp-div .digit-group').find('input').each(function () {
		$(this).attr('maxlength', 1);
		$(this).on('keyup', function (e) {
			var parent = $($(this).closest('form'));
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
					if ($(parent).attr('data-submission') === 'autosubmit') {
						parent.submit();
					}
				}
			}
		});
	});

	/*email verification*/
	$('#register_section .email-verify-div form').submit(function () {
		var form = $(this);
		AlertMessage(form, fa_icon_info, msg_loading, '', 6);
		emailVerify(form);
		return false;
	});

	/*reset password*/
	$('#register_section .reset-password-div form').submit(function () {
		var form = $(this);
		if (validateForm(form, 'input', 1)) {
			AlertMessage(form, fa_icon_info, msg_loading, '', 6);
			resetPasswordSubmit(form);
		}
		return false;
	});

	/*new password*/
	$('#register_section .new-password-div form').submit(function () {
		var form = $(this);
		if (validateForm(form, 'input', 1)) {
			AlertMessage(form, fa_icon_info, msg_loading, '', 6);
			newPasswordSubmit(form);
		}
		return false;
	});

});
//#endregion

//#region Popup functions
function showRegisterSection(alertMessage = "", returnUrl = "") {
	emptyRegisterSectionInputs();

	var form = $('#register_section .login-div form');
	if (alertMessage) {
		AlertMessage(form, fa_icon_info, alertMessage, '', 10);
	}
	else {
		//AlertMessageHide();
	}
	if (returnUrl) {
		$(form).find('input[name="ReturnUrl"]').val(returnUrl);
	}
	else {
		//$(form).find('input[name="ReturnUrl"]').val('');
	}

	$('#register_section').show().find('.login-div').show().find('.Contact').val('').focus();
}
function showSignupSection(alertMessage = "", returnUrl = "") {
	emptyRegisterSectionInputs();
	$('#register_section').show().find('.signup-div').show().find('.Contact').val('').focus();

	var form = $('#register_section .signup-div form');
	if (alertMessage) {
		AlertMessage(form, fa_icon_info, alertMessage, '', 10);
	}
	if (returnUrl) {
		//$(form).find('input[name="ReturnUrl"]').val(returnUrl);
		//sessionStorage.setItem("bookingUrl", returnUrl);
		//$.cookie('_bookingUrl', returnUrl, { expires: 1, path: '/' });
	}
}
function closeRegisterSection() {
	$('#register_section').hide();
	emptyRegisterSectionInputs();
	$('#register_section .main-div').hide();

	//$.removeCookie('_bookingUrl', { path: '/' });
	AlertMessageHide();
	//sessionStorage.removeItem("bookingUrl");//remove returnUrl for BookNow
}
function changeRegisterSection(thisElementClass, otherElementClass) {
	$('#register_section ' + thisElementClass).slideUp();
	emptyRegisterSectionInputs();
	$('#register_section ' + otherElementClass).slideDown();

	if (otherElementClass === '.login-div')
		$('#register_section .loginp-div .Contact').focus();
	else if (otherElementClass === '.signup-div')
		$('#register_section .signup-div .Contact').focus();
	else if (otherElementClass === '.otp-div')
		$('#register_section .otp-div .digits:eq(0)').focus();
	else if (otherElementClass === '.email-verify-div')
		$('#register_section .email-verify-div .Email').focus();
	else if (otherElementClass === '.reset-password-div')
		$('#register_section .reset-password-div .Contact').focus();
	else if (otherElementClass === '.new-password-div')
		$('#register_section .new-password-div .Password').focus();
}
//#endregion

//#region Other functions

function sendOTPVerification(currentElementClass, resendOtp, code, contact, isOtpAccess = 1) {

	$(OtpForm).find('.otp-contact').attr('data-code', currentCode);
	$(OtpForm).find('.otp-contact').attr('data-value', currentContact);

	if (isOtpAccess) {
		$(OtpForm).find('.otp-contact').text(currentCode + currentContact);
		$(OtpForm).find('.incorrect-details').text("Incorrect phone number?");
		if (resendOtp) {
			resendVerificationCode($('#register_section .otp-div .resend-code'));
		}
		setTimeout(function () {
			changeRegisterSection(currentElementClass, '.otp-div');
			timerOTP((2 * 60), OtpForm);
			$(OtpForm).find('.timer').removeClass('text-danger');
			$(OtpForm).find('button[type="submit"]').attr('disabled', false).slideDown();
			$(OtpForm).find('.resend-code').slideUp();
		}, 2000);
	}
	else {
		$(OtpForm).find('.otp-contact').text(currentEmail);
		$(OtpForm).find('.incorrect-details').text("Incorrect email address?");
		if (resendOtp) {
			resendVerificationCode($('#register_section .otp-div .resend-code'));
		}
		setTimeout(function () {
			changeRegisterSection(currentElementClass, '.otp-div');
			timerOTP((5 * 60), OtpForm);
			$(OtpForm).find('.timer').removeClass('text-danger');
			$(OtpForm).find('button[type="submit"]').attr('disabled', false).slideDown();
			$(OtpForm).find('.resend-code').slideUp();
		}, 2000);
	}
}

function sendEmailVerification(currentElementClass, email) {
	setTimeout(function () {
		//$('#register_section .email-verify-div button[type="submit"]').text('Resend Verification Email');
		changeRegisterSection(currentElementClass, '.email-verify-div');
		$('#register_section .email-verify-div input[type="email"]').val(email);
	}, 2000);
}

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
		$(form).find('.timer').addClass('text-danger');
		$(form).find('button[type="submit"]').attr('disabled', true).slideUp();
		$(form).find('.resend-code').slideDown();
	}

	timer(time); //seconds
}

function emptyRegisterSectionInputs() {
	$('#register_section .input-field').val('');
	$('#register_section .form-text-validation').text('');
	$('#register_section .border-green-600').removeClass('border-green-600');
	$('#register_section .border-red-600').removeClass('border-red-600');
}

function validateForm(form, type = 'input', nextElem = 1) {
	//insert, remove validate message on input fields
	$.each($(form).find('.form-field'), function (idx, elem) {

		if ($(elem).attr('data-strict') === "1") {

			//has value
			if ((!$(elem).attr('multiple') && $(elem).val())
				||
				($(elem).attr('multiple') && $(elem).val().length)) {
				if ($(elem).hasClass('password') || $(elem).hasClass('email') || $(elem).hasClass('confirm-password')) {
					if ($(elem).closest('.form-group').find('.form-text-validation').text() && $(elem).hasClass('border-danger')) {
					}
					else {
						$(elem).closest('.form-group').find('.form-text-validation').text('');
					}
				}
				else {
					$(elem).closest('.form-group').find('.form-text-validation').text('');
				}
			}
			//validate
			else {
				$(elem).closest('.form-group').find('.form-text-validation').text("* " + $(elem).closest('.form-group').find('label').text() + ChangeString(' is required.', 'مطلوب. '));
			}

			//
		}

	});

	var emptyField = 0;
	$.each($(form).find('.form-text-validation'), function (k, elem) {
		if ($(elem).text()) {
			emptyField = 1;
		}
	});
	if (!emptyField)
		return true;
	return false;
}

function validatePassword(elem) {

	var value = $(elem).val();
	var passRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z@@*$&#%!~()\d]{8,}$/;
	if (value) {
		if (!value.match(passRegex)) {
			$(elem).addClass('border-red-600')
				.removeClass('border-green-600')
				.closest('.form-group').find('.form-text-validation').text(ChangeString('Min. 8 characters, at least one uppercase letter, one lowercase letter, and one number', 'دقيقة. 8 أحرف ، حرف كبير واحد على الأقل ، وحرف صغير واحد ، ورقم واحد '));
		}
		else {
			$(elem).addClass('border-green-600')
				.removeClass('border-red-600')
				.closest('.form-group').find('.form-text-validation').text('');
		}
	}
	else {
		$(elem).removeClass('border-red-600 border-green-600')
			.closest('.form-group').find('.form-text-validation').text("* " + $(elem).closest('.form-group').find('label').text() + ChangeString(' is required.', 'مطلوب. '));
	}
}

function validateConfirmPassword(elem) {

	const form = $(elem).closest('form');
	const value = $(elem).val();
	if (value) {
		if (value != $(form).find('input[name="Password"]').val()) {
			$(elem).addClass('border-red-600')
				.removeClass('border-green-600')
				.closest('.form-group').find('.form-text-validation').text(ChangeString("Confirm password doesn't match, Type again!", "تأكيد كلمة المرور غير مطابق ، اكتب مرة أخرى!"));
		}
		else {
			$(elem).addClass('border-green-600')
				.removeClass('border-red-600')
				.closest('.form-group').find('.form-text-validation').text('');
		}
	}
	else {
		$(elem).removeClass('border-red-600 border-green-600')
			.closest('.form-group').find('.form-text-validation').text("* " + $(elem).closest('.form-group').find('label').text() + ChangeString(' is required.', 'مطلوب. '));
	}
}

function showNewPasswordDiv(currentElementClass, contact, otp) {
	setTimeout(function () {
		changeRegisterSection(currentElementClass, '.new-password-div');
		NewPasswordForm.find('input[name="Contact"]').val(contact);
		NewPasswordForm.find('input[name="OTP"]').val(otp);
	}, 1500);
}

//#endregion

//#region Ajax functions
function loginSubmit(form) {
	var btn = $(form).find('button[type="submit"]');
	disableSubmitButton(btn, true);

	if (!$('#returnUrl').val()) {
		const urlParams = new URLSearchParams(window.location.search);
		const param_x = urlParams.get('returnUrl');
		if (param_x)
			$('#returnUrl').val(param_x);
		else {
			//if (!window.location.pathname.includes('home'))
			//	$('#returnUrl').val(window.location.pathname);
		}
	}

	const code = $(form).find('.flag-input').attr('data-code');
	const contact_no = $(form).find('.Contact').val();

	//var contact = code + contact_no;
	$(form).find('input[name="PhoneCode"]').val(code);
	$(form).find('input[name="Contact"]').val(contact_no);

	currentCode = code;
	currentContact = contact_no;

	//ajax start
	$.ajax({
		url: $(form).attr('action'),//@culture/customer/login
		type: 'Post',
		data: $(form).serialize(),
		success: function (response) {
			if (response.success) {
				AlertMessage(form, fa_icon_success, response.message, '', 6)
				setTimeout(function () {
					if (!bookingPopupAfterLogin) {
						window.location.href = response.url;
					}
					else {
						bookingPopupAfterLogin = false;
						session = true;
						closeRegisterSection();
						changeSession();
						openBookingPopup(bookingServiceID);
					}
				}, 1500);
			} else {
				if (response.verifyAgain) {
					if (response.verifyByContact) {
						AlertMessage(form, fa_icon_warning, response.message, `sendOTPVerification('.login-div',true,${code},${contact_no})`, 30);
						sendOTPVerification('.login-div', true, code, contact_no);
					}
					else if (response.verifyByEmail) {
						AlertMessage(form, fa_icon_warning, response.message, `sendEmailVerification('.login-div')`, 30)
						sendEmailVerification('.login-div');
					}
				}
				AlertMessage(form, fa_icon_warning, response.message, '', 6)
				disableSubmitButton(btn, false);
			}
		},
		error: function (e) {
			AlertMessage(form, fa_icon_warning, ServerError, '', 6)
			disableSubmitButton(btn, false);
		},
		failure: function (e) {
			AlertMessage(form, fa_icon_warning, ServerError, '', 6)
			disableSubmitButton(btn, false);
		}
	});
}
function signupSubmit(form) {
	var btn = $(form).find('button[type="submit"]');
	disableSubmitButton(btn, true);

	const code = $(form).find('.flag-input').attr('data-code');
	const contact_no = $(form).find('.Contact').val();

	//var contact = code + contact_no;
	$(form).find('input[name="PhoneCode"]').val(code);
	$(form).find('input[name="Contact"]').val(contact_no);

	currentCode = code;
	currentContact = contact_no;
	currentEmail = $(form).find('input[name="Email"]').val();

	//ajax start
	$.ajax({
		url: $(form).attr('action'),//@culture/customer/signup
		type: 'Post',
		data: $(form).serialize(),
		success: function (response) {
			if (response.success) {
				AlertMessage(form, fa_icon_success, response.message, '', 6);
				if (response.verifyByEmail) {
					//sendEmailVerification('.signup-div', response.email);
					sendOTPVerification('.signup-div', false, code, contact_no, false);
				}
				else if (response.verifyByContact) {
					if (response.isOTPSent) {
						sendOTPVerification('.login-div', true, code, contact_no);
					}
					else {
						AlertMessage(form, fa_icon_info, response.message, '', 6);
					}
				}
				//setTimeout(function () {
				//	window.location.href = response.url;
				//}, 1500);
			} else {
				AlertMessage(form, fa_icon_warning, response.message, '', 6);
			}
			disableSubmitButton(btn, false);

		},
		error: function (e) {
			AlertMessage(form, fa_icon_warning, ServerError, '', 6)
			disableSubmitButton(btn, false);
		},
		failure: function (e) {
			AlertMessage(form, fa_icon_warning, ServerError, '', 6)
			disableSubmitButton(btn, false);
		}
	});
}
function otpSubmit(form) {

	var btn = $(form).find('button[type="submit"]');
	disableSubmitButton(btn, true);
	var code = $(form).find('.otp-contact').attr('data-code');
	var contact = $(form).find('.otp-contact').attr('data-value');
	var OTP = "";
	$(form).find('.digit-group').find('input').each(function () {
		OTP = OTP + $(this).val();
	});
	var otp = Number(OTP);

	if (otp == null || otp == NaN || otp == "") {
		AlertMessage(form, fa_icon_warning, lang == 'en' ? "Invalid OTP!" : "كلمة مرور صالحة لمرة واحدة!", '', 6)
	}
	else if (contact == null || contact == "") {
		AlertMessage(form, fa_icon_warning, lang == 'en' ? "Invalid Contact number!" : "رقم الاتصال غير صحيح!", '', 6)
	}
	else {

		let autoLogin = true;
		if (Number($(form).find('#reset_password').val())) {
			autoLogin = false;
		}

		//ajax start
		$.ajax({
			url: $(form).attr('action'),//@culture/Customer/Account/verifyOTP
			type: 'Post',
			data: {
				__RequestVerificationToken: $(form).find('input[name="__RequestVerificationToken"]').val(),
				PhoneCode: code,
				Contact: contact,
				otp: otp,
				AutoLogin: autoLogin,
			},
			success: function (response) {
				if (response.success) {
					if (Number($(form).find('#reset_password').val())) {
						AlertMessage(form, fa_icon_success, ChangeString("OTP verified! Please create a new password.", "تم التحقق من OTP! الرجاء إنشاء كلمة مرور جديدة."), '', 6);
						showNewPasswordDiv('.otp-div', contact, otp);
					}
					else {
						AlertMessage(form, fa_icon_success, response.message, '', 6);

						setTimeout(function () {
							if (!bookingPopupAfterLogin) {
								window.location = "/" + culture + response.url;
							}
							else {
								bookingPopupAfterLogin = false;
								session = true;
								closeRegisterSection();
								changeSession();
								openBookingPopup(bookingServiceID);
							}
						}, 1500);
					}
				} else {
					AlertMessage(form, fa_icon_warning, response.message, '', 6);
				}
				disableSubmitButton(btn, false);

			},
			error: function (e) {
				AlertMessage(form, fa_icon_warning, ServerError, '', 6)
				disableSubmitButton(btn, false);
			},
			failure: function (e) {
				AlertMessage(form, fa_icon_warning, ServerError, '', 6)
				disableSubmitButton(btn, false);
			}
		});
	}

}
function resendVerificationCode(elem) {
	emptyRegisterSectionInputs();
	var form = $(elem).closest('form');
	var btn = $(form).find('button[type="submit"]');
	var code = $(form).find('.otp-contact').attr('data-code');
	var contact = $(form).find('.otp-contact').attr('data-value');

	currentCode = code;
	currentContact = contact_no;

	disableSubmitButton(btn, true);
	$(elem).slideUp();

	$.ajax({
		url: "/" + culture + '/Customer/Account/resendOTP',
		type: 'Post',
		data: {
			__RequestVerificationToken: $(form).find('input[name="__RequestVerificationToken"]').val(),
			PhoneCode: code,
			Contact: contact,
		},
		success: function (response) {
			changeRegisterSection('.login-div', '.otp-div');

			if (response.success) {
				AlertMessage(form, fa_icon_success, response.message, '', 6);
				if (Number($(form).find('#is_otp_access').val())) {
					timerOTP(2 * 60, form);
				} else {
					timerOTP(15 * 60, form);
				}
				$(form).find('.timer').removeClass('text-danger');
				$(form).find('button[type="submit"]').attr('disabled', false).slideDown();
				$(form).find('.resend-code').slideUp();
			} else {
				AlertMessage(form, fa_icon_warning, response.message, '', 6);
				$(elem).slideDown();
			}
			disableSubmitButton(btn, false);
		},
		error: function (e) {
			AlertMessage(form, fa_icon_warning, ServerError, '', 6)
			disableSubmitButton(btn, false);
		},
		failure: function (e) {
			AlertMessage(form, fa_icon_warning, ServerError, '', 6)
			disableSubmitButton(btn, false);
		}
	});
}
function emailVerify(form) {
	var btn = $(form).find('button[type="submit"]');
	disableSubmitButton(btn, true);

	currentEmail = $(form).find('input[name="Email"]').val();

	//ajax start
	$.ajax({
		url: $(form).attr('action'),//@culture/customer/email-verification
		type: 'Post',
		data: $(form).serialize(),
		success: function (response) {
			if (response.success) {
				AlertMessage(form, fa_icon_success, response.message, '', 6);
				setTimeout(function () {
					sendOTPVerification('.email-verify-div', false, "", "", false);
				}, 1500);

			} else {
				AlertMessage(form, fa_icon_warning, response.message, '', 6);
			}
			disableSubmitButton(btn, false);
		},
		error: function (e) {
			AlertMessage(form, fa_icon_warning, ServerError, '', 6)
			disableSubmitButton(btn, false);
		},
		failure: function (e) {
			AlertMessage(form, fa_icon_warning, ServerError, '', 6)
			disableSubmitButton(btn, false);
		}
	});
}
function resetPasswordSubmit(form) {
	var btn = $(form).find('button[type="submit"]');
	disableSubmitButton(btn, true);

	var contact = $(form).find('.flag-input').attr('data-code') + $(form).find('.Contact').val();
	$(form).find('input[name="Contact"]').val(contact);

	currentCode = code;
	currentContact = contact_no;

	//ajax start
	$.ajax({
		url: $(form).attr('action'),//@culture/customer/resetpassword
		type: 'Post',
		data: $(form).serialize(),
		success: function (response) {
			if (response.success) {
				AlertMessage(form, fa_icon_success, response.message, '', 6);
				if (response.isOtpAccess) {
					$(OtpForm).find('#is_otp_access').val(1);
				} else {
					$(OtpForm).find('#is_otp_access').val(0);
				}
				$(OtpForm).find('#reset_password').val(1);
				sendOTPVerification('.reset-password-div', false, code, contact_no, Number($(OtpForm).find('#is_otp_access').val()));
			} else {
				AlertMessage(form, fa_icon_warning, response.message, '', 6);
			}
			disableSubmitButton(btn, false);
		},
		error: function (e) {
			AlertMessage(form, fa_icon_warning, ServerError, '', 6)
			disableSubmitButton(btn, false);
		},
		failure: function (e) {
			AlertMessage(form, fa_icon_warning, ServerError, '', 6)
			disableSubmitButton(btn, false);
		}
	});
}
function newPasswordSubmit(form) {
	var btn = $(form).find('button[type="submit"]');
	disableSubmitButton(btn, true);
	//ajax start
	$.ajax({
		url: $(form).attr('action'),//@culture/customer/newpassword
		type: 'Post',
		data: $(form).serialize(),
		success: function (response) {
			if (response.success) {
				AlertMessage(form, fa_icon_success, response.message, '', 6);
				setTimeout(function () {
					changeRegisterSection('.new-password-div', '.login-div');
				}, 2000);
			} else {
				AlertMessage(form, fa_icon_warning, response.message, '', 6);
			}
			disableSubmitButton(btn, false);
		},
		error: function (e) {
			AlertMessage(form, fa_icon_warning, ServerError, '', 6)
			disableSubmitButton(btn, false);
		},
		failure: function (e) {
			AlertMessage(form, fa_icon_warning, ServerError, '', 6)
			disableSubmitButton(btn, false);
		}
	});
}

//#endregion Ajax functions
