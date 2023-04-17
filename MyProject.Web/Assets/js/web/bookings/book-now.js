'use strict'

//#region Global Variables and Arrays

//#endregion

//#region document ready function
$(document).ready(function () {

	//sessionStorage.removeItem("bookingUrl");//remove returnUrl for BookNow
	$.removeCookie('_bookingUrl', { path: '/' });

	$('.countries').append(allCountries);

	if ($('#booking form .country').val())
		$('#booking form select[name="CustomerCountry"]').val($('#booking form .country').val());

	//var startDate;
	//startDate = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
	var date = new Date();
	date = String(date.getMonth() + 1).padStart(2, '0') + '/' + String(date.getDate()).padStart(2, '0') + '/' + date.getFullYear();

	$('#booking form input[name="EmiratesIDExpiry"]').datepicker({
		showOtherMonths: true,
		format: "mm/dd/yyyy",
		minDate: date,
		value: date,
		uiLibrary: 'bootstrap4'
	});
	$('#booking form input[name="PassportExpiry"]').datepicker({
		showOtherMonths: true,
		format: "mm/dd/yyyy",
		minDate: date,
		value: date,
		uiLibrary: 'bootstrap4'
	});

	$('#booking form .gj-datepicker').addClass('relative').append('<i class="absolute fa fa-calendar mt-0 mt-2 text-gray-900" style="margin: 11px -30px;font-size: 1.5em;"></i>');

	//GetCountries($('#Customer_CountryID').val());

	//$('#booking form select[name="CountryID"]').change(function () {
	//	GetCities($(this).val());
	//});
	//$('#booking form select[name="CityID"]').change(function () {
	//	GetAreas($(this).val());
	//});

	$('#booking form').submit(function () {
		var form = this;
		if (validateBookingForm(form)) {
			AlertMessage(form, icon_info, msg_loading, '', 6)
			$("html, body").animate({ scrollTop: 300 }, 500);
			bookingFormSubmit(form);
		}
		return false;
	});
});
//#endregion

//#region Ajax Call

function GetCountries(id) {

	$.ajax({
		type: 'POST',
		url: '/Home/GetCountries',
		data: {
			countryId: id,
		},
		success: function (response) {
			BindCountries(response.data);
		}
	});

}
function GetCities(id) {
	if (id) {
		$.ajax({
			type: 'POST',
			url: '/Home/GetCites',
			data: {
				countryId: id,
				cityId: $('#Customer_CityID').val(),
			},
			success: function (response) {
				BindCites(response.data);
			}
		});
	}
	else {
		BindCites(0);
	}
}
function GetAreas(id) {
	if (id) {
		$.ajax({
			type: 'POST',
			url: '/Home/GetAreas',
			data: {
				cityId: id,
				areaId: $('#Customer_AreaID').val()
			},
			success: function (response) {
				BindAreas(response.data);
			}
		});
	}
	else {
		BindAreas(0);
	}
}
//#endregion

//#region Functions for Binding Data

function BindCountries(data) {
	var form = $('#booking form');

	if (data && data.length) {
		$(form).find($('select[name="CountryID"]')).html($("<option />").val('').text("Select Country"));
		$.each(data, function (k, v) {
			$(form).find($('select[name="CountryID"]')).append($("<option />").val(v.Value).text(v.Text).attr('selected', v.Selected));
		});
	}
	else {
		$(form).find($('select[name="CountryID"]')).html($("<option />").val('').text("Select Country"));
		BindAreas(0);
	}

	$(form).find($('select[name="CountryID"]')).trigger('change');
}
function BindCites(data) {
	var form = $('#booking form');

	if (data && data.length) {
		$(form).find($('select[name="CityID"]')).html($("<option />").val('').text("Select City"));
		$.each(data, function (k, v) {
			$(form).find($('select[name="CityID"]')).append($("<option />").val(v.Value).text(v.Text).attr('selected', v.Selected));
		});
	}
	else {
		$(form).find($('select[name="CityID"]')).html($("<option />").val('').text("Select City"));
		BindAreas(0);
	}

	$(form).find($('select[name="CityID"]')).trigger('change');

}
function BindAreas(data) {
	var form = $('#booking form');

	if (data && data.length) {
		$(form).find($('select[name="AreaID"]')).html($("<option />").val('').text("Select Area"));
		$.each(data, function (k, v) {
			$(form).find($('select[name="AreaID"]')).append($("<option />").val(v.Value).text(v.Text).attr('selected', v.Selected));
		});
	}
	else {
		$(form).find($('select[name="AreaID"]')).html($("<option />").val('').text("Select Area"));
	}
}
//#endregion

//#region Other Functions

function checkAgree(elem) {
	
	$('#btn_booking_form').prop('disabled', !$(elem).prop('checked'));

}

//#endregion

//form submit function
function validateBookingForm(form) {

	$.each($('#booking form input'), function (k, elem) {
		if (!$(elem).val()) {
			$(elem).next('span.text-danger').html(`${$(elem).prev('label').html()} ${ChangeString('is required.','مطلوب.')}`);
		}
		else {
			$(elem).next('span.text-danger').html('');
		}
	});
	$.each($('#booking form select'), function (k, elem) {
		if (!$(elem).val()) {
			$(elem).next('span.text-danger').html(`${$(elem).prev('label').html()} ${ChangeString('is required.','مطلوب.')}`);
		}
		else {
			$(elem).next('span.text-danger').html('');
		}
	});
	$.each($('#booking form input[inputmode="numeric"]'), function (k, elem) {
		if (!$(elem).val()) {
			$(elem).closest('div').next('span.text-danger').html(`${$(elem).closest('div').prev('label').html()} ${ChangeString('is required.','مطلوب.')}`);
		}
		else {
			$(elem).closest('div').next('span.text-danger').html('');
		}
	});

	var emptyField = 0;
	$.each($('#booking form span.text-danger'), function (k, elem) {
		if ($(elem).text()) {
			emptyField = 1;
		}
	});
	if (!emptyField) {
		return true;
	}

	return false;
}

function bookingFormSubmit(form) {

	var btn = $(form).find('button[type="submit"]');
	disableSubmitButton(btn, true);

	//ajax start
	$.ajax({
		url: $(form).attr('action'),///@culture/bookings/book
		type: 'Post',
		data: $(form).serialize(),
		success: function (response) {
			if (response.success) {
				AlertMessage(form, icon_success, response.message, '', 6)

				//write payment gateway setting code here...
				InitiatePayment(form, response);

			} else {
				AlertMessage(form, icon_error, response.message, '', 6)
				disableSubmitButton(btn, false);
			}
		},
		error: function (e) {
			AlertMessage(form, icon_error, ServerError, '', 6)
			disableSubmitButton(btn, false);
		},
		failure: function (e) {
			AlertMessage(form, icon_error, ServerError, '', 6)
			disableSubmitButton(btn, false);
		}
	});

}

function InitiatePayment(form, response) {

	$(".booking-hide").hide();
	$(form).hide();
	var result_div = $(form).prev('.result-div');
	$(result_div).prev('h1').text('Processing for Payment');
	$(result_div).find('h1').text(response.message);
	$(result_div).show();

	$.ajax({
		url: '/'+culture+'/Customer/Payment/Pay/' + response.booking.id,
		type: 'GET',
		success: function (response) {
			if (response.success) {
				window.location = "/" + culture +"/Customer/Payment/Processing/" + response.booking.invoice.id
			}
			else {
				result_div.find('img').attr('src','/assets/images/web-icons/result-icon-exclamation.svg');
				result_div.find('h1').text(response.message);//booking no should be added in message
				result_div.find('p:eq(0)').text('Booking payment processing failed, please try again.');
				result_div.find('p:eq(1)').text('If the error persists contact our support.');
			}
		}
	})
}