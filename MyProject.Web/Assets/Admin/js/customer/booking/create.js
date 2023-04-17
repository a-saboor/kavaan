'use strict'

//#region Global Variables and Arrays


//#endregion

//#region document ready function
$(document).ready(function () {
	initAllFunc();

	$.each($('i[for="Images"]'), function (idx, elem) {
		$(elem).click(function () {
			$(elem).siblings().click();
		});
	});
	
	//submit form
	$('button[form="booking_form"]').click(function () {
		const btn = $(this);
		const url = `/${culture}/customer/booking/create`;
		const showSpinner = true;
		const showAlert = true;
		const showToastr = false;
		const btnSpinner = true;
		const msg = "Please fill the form properly...";
		const form = $('#' + $(btn).attr('form'));
		if (formValidate(form, showAlert, showToastr, msg)) {
			fileFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner);
		}
		return false;
	});
});
//#endregion

//#region Other functions

function fileFormSubmit_callback(form, status, response, url) {

	if (status) {
		successPopup($(form).closest('.popup-section'));
	}
}

//#endregion
