function InitiatePayment(elem, bookingId, bookingNo) {

	//$(elem).html('<i class="fa fa-circle-o-notch fa-spin" aria-hidden="true"></i> Pay').prop('disabled', true);

	//$('#message').html('<span class="fa fa-hour-watch fa-2x"></span> Processing for payment ...')
	//$('#message').slideDown();

	var result_div = $(elem).closest('.result-div');

	disableSubmitButton(elem, true);
	AlertMessage(result_div, icon_info, 'Processing for payment ...', '', 6);

	$.ajax({
		url: '/Customer/Payment/Pay/' + bookingId,
		type: 'GET',
		success: function (response) {
			if (response.success) {
				window.location = "/" + culture + "/Customer/Payment/Processing/" + response.booking.invoice.id
			}
			else {
				disableSubmitButton(elem, false);
				AlertMessage(result_div, icon_error, response.message, '', 6);

				//$(elem).html('<i class="fa fa-credit-card" aria-hidden="true"></i> Pay').prop('disabled', false);

				//$('#message').html('<span class="fa fa-warning fa-2x"></span> ' + response.message);
				//$('#message').slideDown();
				//MessageSlideUP();
			}
		}
	})
}

function getParameterByName(name, url) {
	if (!url) url = window.location.href;
	name = name.replace(/[\[\]]/g, "\\$&");
	var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
		results = regex.exec(url);
	if (!results) return null;
	if (!results[2]) return '';
	return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function MessageSlideUP() {
	setTimeout(function () {
		$('#message').slideUp();
	}, 5000);
}