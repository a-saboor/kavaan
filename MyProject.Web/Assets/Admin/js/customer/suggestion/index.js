'use strict';

//#region global variables and arrays

//#endregion

//#region document ready function

$(document).ready(function () {
	initAllFunc();

	//customer suggestions form
	$('button[form="form_suggestions"]').click(function () {
		const btn = $(this);
		const url = `/customer/suggestion/index`;
		const showSpinner = true;
		const showAlert = true;
		const showToastr = false;
		const btnSpinner = true;
		const msg = "Please fill the form properly...";
		const form = $('form[id="form_suggestions"]');
		scrollTop(75);
		if (formValidate(form, showAlert, showToastr, msg)) {
			simpleFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner);
		}
		return false;
	});
});

//#endregion

//#region Other functions

//#endregion

