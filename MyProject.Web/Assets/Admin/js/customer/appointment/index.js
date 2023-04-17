'use strict';
//#region Global Variables and Arrays
//#endregion

//#region document ready function
$(document).ready(function () {

	$('#appointment_form').submit(function () {
		var form = this;
		if (validateAppointmentForm(form)) {
			AlertMessage(form, icon_info, msg_loading, '', 6)
			appointmentFormSubmit(form);
		}
		return false;
	});

	var date = new Date();
	date = String(date.getMonth() + 1).padStart(2, '0') + '/' + String(date.getDate()).padStart(2, '0') + '/' + date.getFullYear();

	$('#appointment_form input[name="Date"]').datepicker({
		showOtherMonths: true,
		format: "mm/dd/yyyy",
		minDate: date,
		value: date,
		uiLibrary: 'bootstrap4'
	});

	$('#appointment_form .gj-datepicker').addClass('relative').append('<i class="absolute fa fa-calendar mt-0 mt-2 text-gray-900" style="margin: 14px -24px;font-size: 0.8em;"></i>');

	$('#appointment_form input[name="Time"]').change(function () {
		var value = this.value;

		if (value) {

			// 9:30 am to 7:30 pm
			let hour = value.split(":")[0];
			let mins = value.split(":")[1];

			if (Number(hour) >= 9 && Number(hour) <= 19) {
				if (Number(hour) == 9 && Number(mins) < 30) {
					validateAppointmentTime(false);
				}
				else if (Number(hour) == 19 && Number(mins) > 30) {
					validateAppointmentTime(false);
				}
				else {
					validateAppointmentTime(true);
				}
			}
			else {
				validateAppointmentTime(false);
			}
		}
	});

	//$('#appointment_form input[name="Time"]').timepicker({
	//	modal: false,
	//	header: false,
	//	footer: false,
	//	format: 'HH.MM',
	//	mode: '24hr',
	//	uiLibrary: 'bootstrap3',
	//	size: 'small'
	//});

});
//#endregion

//form submit function
function validateAppointmentForm(form) {

	if (!$('#appointment_form input[name="Date"]').val()) {
		$('#appointment_form input[name="Date"]').parent().next('span.text-danger').html(`${ChangeString('Appointment Date* is required.', 'تاريخ الموعد* مطلوب.')}`);
	}
	else {
		$('#appointment_form input[name="Date"]').parent().next('span.text-danger').html('');
	}
	if (!$('#appointment_form input[name="Time"]').val()) {
		$('#appointment_form input[name="Time"]').next('span.text-danger').html(`${ChangeString('Appointment Time* is required.', 'وقت الموعد* مطلوب.')}`);
	}
	else {
		$('#appointment_form input[name="Time"]').next('span.text-danger').html('');
	}
	if (!$('#appointment_form input[name="Type"]:checked').val()) {
		$('#appointment_form input[name="Type"]').parent().parent().next('span.text-danger').html(`${ChangeString('Appointment Type* is required.', 'نوع الموعد* مطلوب.')} `);
	}
	else {
		$('#appointment_form input[name="Type"]').parent().parent().next('span.text-danger').html('');
	}

	var emptyField = 0;
	$.each($('#appointment_form span.text-danger'), function (k, elem) {
		if ($(elem).text()) {
			emptyField = 1;
		}
	});
	if (!emptyField) {
		return true;
	}

	return false;
}

function appointmentFormSubmit(form) {

	var btn = $(form).find('button[type="submit"]');
	disableSubmitButton(btn, true);



	$(form).find('input[name="TypeAr"]').val($(form).find('input[name="Type"]:checked').attr('data-ar-value'));

	var date = $(form).find('input[name="Date"]').val();
	var time = $(form).find('input[name="Time"]').val();

	date = date.split('/');

	date = date[1] + "/" + date[0] + "/" + date[2]

	var AppointmentDate = date + " " + time;

	$(form).find('input[name="AppointmentDate"]').val(AppointmentDate);

	//ajax start
	$.ajax({
		url: $(form).attr('action'),///@culture/Customer/Appointments/Index
		type: 'Post',
		data: $(form).serialize(),
		success: function (response) {
			if (response.success) {
				AlertMessage(form, icon_success, response.message, '', 6)
				emptyFormInputs(form);
			} else {
				AlertMessage(form, icon_error, response.message, '', 6)
			}
			disableSubmitButton(btn, false);
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

function emptyFormInputs(form) {
	$(form).find('.input-field-hidden').val('-');
	$(form).find('input[name="Date"]').val('');
	$(form).find('input[name="Time"]').val('');
	$(form).find('input[name="Type"]').prop('checked', false);
}

function validateAppointmentTime(flag) {
	
	if (flag) {
		$('#appointment_form input[name="Time"]').next('span.text-danger').html('');
	}
	else {
		$('#appointment_form input[name="Time"]').val('');
		$('#appointment_form input[name="Time"]').next('span.text-danger').html(ChangeString('Please select Appointment Time from 09:30 AM to 07:30 PM', 'يرجى تحديد موعد الموعد من 09:30 صباحًا إلى 07:30 مساءً'));
	}
}