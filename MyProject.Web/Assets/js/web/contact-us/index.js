'use strict'

$(document).ready(function () {

	//contact us form / enquiry
	$('button[form="form_contact"]').click(function () {
		const btn = $(this);
		const url = `/ContactUs/Create`;
		const showSpinner = true;
		const showAlert = true;
		const showToastr = false;
		const btnSpinner = true;
		const msg = "Please fill the form properly...";
		const form = $('form[id="form_contact"]');
		if (formValidate(form, showAlert, showToastr, msg)) {
			simpleFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner);
		}
		return false;
	});

	OnErrorImage(0.5, 'contact');

});
