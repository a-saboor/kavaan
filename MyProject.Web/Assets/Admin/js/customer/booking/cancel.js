'use strict'

//#region Global Variables and Arrays


//#endregion

//#region document ready function
$(document).ready(function () {
	initAllFunc();
	
	//submit form
	$('button[form="cancellation_form"]').click(function () {
		debugger;
		//const id = $("#IDD").val();
		const btn = $(this);
		const url = `/${culture}/customer/booking/cancel`;
		const showSpinner = true;
		const showAlert = true;
		const showToastr = false;
		const btnSpinner = true;
		const msg = "Please fill the form properly...";
		const form = $('#' + $(btn).attr('form'));
		if (formValidate(form, showAlert, showToastr, msg)) {
			simpleFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner) ;
		}
		return false;
	});
});
//#endregion

//#region Other functions

//#endregion
