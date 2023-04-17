'use strict'

//#region Global Variables and Arrays

//#endregion

//#region document ready function
$(document).ready(function () {
	$('#enquire_section form').submit(function () {
		var form = this;
		if (validateEnquiryForm(form)) {
			AlertMessage(form, icon_info, msg_loading, '', 6)
			enquiryFormSubmit(form);
		}
		return false;
	});
});
//#endregion


//#region Other Functions
function showEnquirePopup(unitId) {
	emptyEnquiryFormInputs($('#enquire_section form'));
	$('#enquire_section').show();
	$('#enquire_section form input[name="UnitID"]').val(unitId);
}
function showPaymentPlanPopup(unitId) {
	$('#payment_plan_section tbody').html(`<tr>
											<td colspan="3">
												<section class="text-center py-2 w-[100%]" style=""><i class="fa fa-2x fa-circle-o-notch fa-spin"></i></section>
											</td>
										</tr>`);
	$('#payment_plan_section').show();
	GetPaymentPlan(unitId);
}
function bookNow(unitId) {
	let bookUrl = `/bookings/book-now/${unitId}`;

	if (session)
		window.location = bookUrl;
	else {
		showRegisterSection(ChangeString("Please Login or Signup, for Booking a Property", "الرجاء تسجيل الدخول أو الاشتراك ، لحجز عقار"), bookUrl);
	}
}
//#endregion

//#region Ajax Call
function GetPaymentPlan(id) {
	$.ajax({
		type: 'POST',
		url: `/ModalPopups/GetPaymentPlans`,
		data: { id: id },
		success: function (response) {
			BindPaymentPlan(response.data);
		}
	});
}

//#endregion

//#region Functions for Binding Data
function BindPaymentPlan(data) {

	$('#payment_plan_section tbody').empty();

	$.each(data, function (k, v) {
		var template = `<tr>
							<td class="py-1 w-full whitespace-no-wrap border-b border-gray-200">
								<span class="text-sm leading-5 text-gray-500">${v.Milestones}</span>
							</td>
							<td class="py-1 pr-10 whitespace-no-wrap text-right border-b border-gray-200">
								<div class="text-sm leading-5 text-gray-500">${v.Percentage}</div>
							</td>
							<td class="py-1 whitespace-no-wrap text-right border-b border-gray-200">
								<span class="text-sm leading-5 text-gray-500">${v.Amount}</span>
							</td>
						</tr>`;

		$('#payment_plan_section tbody').append(template);
	});

	if ($('#payment_plan_section tbody').html().length == 0) {
		$('#payment_plan_section tbody').html(`
			<tr>
				<td colspan="3">
					<section class="text-center py-2 w-[100%]" style=""><i class="fa fa-exclamation-circle mr-1"></i> ${ChangeString('No Records Found!', 'لا توجد سجلات!')} </section>
				</td>
			</tr>
		`);
	}

}

//#endregion

//form submit functions
function validateEnquiryForm(form) {

	$.each($('#enquire_section form .input-field'), function (k, elem) {
		if (!$(elem).val() && $(elem).is(":visible")) {
			if ($(elem).next().length) {
				$(elem).next('span.text-danger').html(`${$(elem).prev('p').html()} ${ChangeString('is required.','مطلوب.')}`);
			} else {
				if ($(elem).closest('div').next('span.text-danger').length)
					$(elem).closest('div').next('span.text-danger').html(`${$(elem).closest('div').prev('p').html()} ${ChangeString('is required.','مطلوب.')}`);
				else
					$(elem).closest('bdi').next('span.text-danger').html(`${$(elem).closest('bdi').prev('p').html()} ${ChangeString('is required.','مطلوب.')}`);
			}
		}
		else {
			if ($(elem).next().length) {
				$(elem).next('span.text-danger').html(``);
			} else {
				if ($(elem).closest('div').next('span.text-danger').length)
					$(elem).closest('div').next('span.text-danger').html(``);
				else
					$(elem).closest('bdi').next('span.text-danger').html(``);
			}
		}
	});

	var emptyField = 0;
	$.each($('#enquire_section form  span.text-danger'), function (k, elem) {
		if ($(elem).text()) {
			emptyField = 1;
		}
	});
	if (!emptyField) {
		return true;
	}

	return false;
}

function enquiryFormSubmit(form) {

	var btn = $(form).find('button[type="submit"]');
	disableSubmitButton(btn, true);

	var contact = $(form).find('.flag-input').attr('data-code') + $(form).find('.Contact').val();
	$(form).find('input[name="Contact"]').val(contact);

	//ajax start
	$.ajax({
		url: $(form).attr('action'),///@culture/ModalPopups/UnitEnquiry
		type: 'Post',
		data: $(form).serialize(),
		success: function (response) {
			if (response.success) {
				AlertMessage(form, icon_success, response.message, '', 6)
				emptyEnquiryFormInputs(form);
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

function emptyEnquiryFormInputs(form) {
	//$(form).find('.input-field-hidden').val('-');
	$(form).find('.input-field').val('');
}