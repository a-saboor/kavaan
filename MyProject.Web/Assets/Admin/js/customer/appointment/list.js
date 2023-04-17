'use strict'

//#region Global Variables and Arrays
var pg = 1;
var apptSortBy = 1;
var apptPageSize = 12;
var isAppointmentPageRendered = false;
var totalPages;
//var lang = "en";

var filter = {
	search: null,
	PageSize: apptPageSize,
	pageNumber: pg,
	sortBy: apptSortBy,
	anyID: $('#Customer_ID').val()
}
//#endregion

//#region document ready function
$(document).ready(function () {

	$('#appointments .main-div').empty().show().html(return_loading_section(0));

	//GetAppointmentsFilterValues();
	//GetAppointments();

	$('#ArticleSearch').change(function () {
		if ($('#ArticleSearch').val()) {
			$('#appointments .main-div').empty();
			pg = 1;
			GetAppointmentsFilterValues();
			GetAppointments();
		}
	});

	$('#btnSearch').click(function () {
		$('#appointments .main-div').empty();
		pg = 1;
		GetAppointmentsFilterValues();
		GetAppointments();
	});

	$('#sortBy').change(function () {
		$('#appointments .main-div').empty();
		pg = 1;
		GetAppointmentsFilterValues();
		GetAppointments();
	});

	$('#appointments .see-more').click(function () {
		//if (pg < totalPages) {
		pg++;
		$('#appointments .see-more').hide();
		$(".filter-loader").show();
		GetAppointmentsFilterValues();
		GetAppointments();
		//}
	});

});
//#endregion

//#region Ajax Call
function Loadappointments() {
	if (!isAppointmentPageRendered) {
		pg = 1;
		GetAppointmentsFilterValues();
		GetAppointments();
		isAppointmentPageRendered = true;
	}
}

function GetAppointments() {
	$.ajax({
		type: 'POST',
		url: '/Customer/Appointments/FilteredAppointments',
		contentType: "application/json",
		data: JSON.stringify(filter),
		success: function (response) {
			BindAppointments(response.data);
		}
	});
}

//#endregion

//#region Functions for Binding Data

function BindAppointments(data) {

	//$('#appointments .main-div').empty().show();

	$.each(data, function (k, v) {
		let remarks_div = "";
		let cancel_appointment_div = "";
		let status = v.status == "Pending" ? ChangeString("Pending", "ريثما") : v.status == "Approved" ? ChangeString("Approved", "وافق") : v.status;

		if (v.Remarks) {
			remarks_div = `
							<div class="flex flex-col justify-center reply-div">
								<div class="flex justify-center">
									<a href="javascript:void(0);" onclick="showRemarks(this);">
										<h1 class="font-Rubik text-sm font-semibold rounded mt-2 p-1 px-2 text-gk-purple bg-pink-200">${ChangeString("Remarks", "ملاحظات")}</h1>
									</a>
								</div>
								<div class="pt-4 remarks-div" style="display:none;">
									<p class="text-black/60 font-Rubik text-sm font-medium text-justify">
										${v.Remarks}
									</p>
								</div>
							</div>
						`;
		}
		if (!v.IsCancelled) {
			cancel_appointment_div = `
							<a href="javascript:void();" onclick="cancelAppointment(this, ${v.ID})">
								<svg class="h-8 w-8 absolute -top-4 -right-4 bg-red-400/40 rounded-full p-1.5"
									 viewBox="0 0 20 20" fill="currentColor">
									<path fill-rule="evenodd"
										  d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
										  clip-rule="evenodd" />
								</svg>
							</a>
						`;
		}

		var template = `
						<div class="border-[1px] border-gray-200 rounded-lg p-4 mb-8 w-full relative mx-auto lg:mx-0 lg:w-[48%] lg:even:ml-auto">
							${cancel_appointment_div}
							<h1 class="font-Rubik text-base font-semibold pb-4">${v.Type}</h1>
							<div class="flex items-end sm:items-start justify-between">
								<div class="sm:flex sm:flex-wrap w-full">
									<div class="font-Rubik text-xs font-semibold uppercase mb-2 sm:w-[50%] xl:flex">
										<h1 class="xl:mr-2">${ChangeString("Appointment No", "رقم الموعد")}</h1>
									</div>
									<div class="font-Rubik text-xs font-semibold uppercase mb-2 sm:w-[50%] xl:flex">
										<p class="text-black/40">${v.AppointmentNo}</p>
									</div>
									<div class="font-Rubik text-xs font-semibold uppercase mb-2 sm:w-[50%] xl:flex">
										<h1 class="xl:mr-2">${ChangeString("Date", "تاريخ")}</h1>
										<p class="text-black/40">${v.AppointmentDate}</p>
									</div>
									<div class="font-Rubik text-xs font-semibold uppercase mb-2 sm:w-[50%] xl:flex">
										<h1 class="xl:mr-2">${ChangeString("Time", "الوقت")}</h1>
										<p class="text-black/40 uppercase">${v.AppointmentTime}</p>
									</div>
								</div>
								<div class="text-right mb-2 md:mb-0">
									<h1 class="font-Rubik text-sm font-semibold">${ChangeString("Status", "مكانة")}</h1>
									<h1 class="font-Rubik text-base font-semibold text-gk-blue appt_status">${v.IsCancelled == true ? ChangeString("Canceled", "تم الإلغاء") : status}</h1>
								</div>
							</div>
							${remarks_div}
						</div>
					`;

		$('#appointments .main-div').append(template);

	});

	//setTimeout(function () { OnErrorImage(); }, 3000);

	if (data && data.length >= apptPageSize) {
		$("#appointments .see-more").fadeIn();
	} else {
		$("#appointments .see-more").fadeOut();
	}

	if ($('#appointments section'))
		$('#appointments section').remove();

	if ($('#appointments .main-div').html().length == 0) {
		$("html, body").animate({ scrollTop: 0 }, 1000);
		$('#appointments .no-more').fadeIn();
		$("#appointments .see-more").fadeOut();
	}
	else {
		$('#appointments .no-more').fadeOut();
	}
}

function GetAppointmentsFilterValues() {
	filter.search = $("#ArticleSearch").val();
	filter.pageNumber = pg;
	filter.sortBy = $("#sortBy").val() ? $("#sortBy").val() : apptSortBy;
}

//#endregion

//Other functions
function showRemarks(elem) {
	elem = $(elem).parent();
	if ($(elem).next().is(":visible")) {
		$(elem).next().slideUp();
	}
	else {
		$(elem).next().slideDown();
	}
}

function cancelAppointment(btn, id) {

	disableSubmitButton(btn, true);
	ToastrMessage(icon_info, msg_loading, 6);

	//ajax start
	$.ajax({
		url: `/Customer/Appointments/Cancel`,
		type: 'Post',
		data: { id: id },
		success: function (response) {
			if (response.success) {
				ToastrMessage(icon_success, response.message, 6);
				$(btn).parent().find('.appt_status').text("Cancelled");
				$(btn).remove();
			} else {
				ToastrMessage(icon_error, response.message, 6);
				disableSubmitButton(btn, false);
			}
		},
		error: function (e) {
			ToastrMessage(icon_error, ServerError, 6);
			disableSubmitButton(btn, false);
		},
		failure: function (e) {
			ToastrMessage(icon_error, ServerError, 6);
			disableSubmitButton(btn, false);
		}
	});

}
