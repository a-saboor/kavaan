'use strict'

//#region Global Variables and Arrays


//#endregion

//#region document ready function
$(document).ready(function () {
	let rating = $(".rating-input").val();
	let review = $(".Review").text();

	initAllFunc();
	ratingStarsInitiate();

	

	if (rating != 0 || review != "") {
		debugger;
		$("#Rating_heading").text("Rating & Review");
		$(".rating-input").attr("disabled", true);
		$(".Review").attr("disabled", true);
		$("#submitButton").remove();
	}

	//submit form
	$('button[form="review_form"]').click(function () {
		//const id = $("#IDD").val();
		const btn = $(this);
		const url = `/${culture}/customer/booking/review`;
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
