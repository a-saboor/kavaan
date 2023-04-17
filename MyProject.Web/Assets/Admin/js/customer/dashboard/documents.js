'use strict'

//#region Global Variables and Arrays
var isDocPageRendered = false;
let defaultFileChosenText = $('#requests .document-upload-div form p').text();
//#endregion

//#region document ready function
$(document).ready(function () {

	/*form request submit*/
	$('#requests .document-upload-div form').submit(function () {
		var form = $(this);

		if (!$(form).find('select').val()) {
			AlertMessage(form, icon_error, ChangeString('Error! Please select request type.', 'خطأ! الرجاء تحديد نوع الطلب.') , '', 6)
		}
		else if (!$(form).find('input[type="file"]').val()) {
			AlertMessage(form, icon_error, ChangeString('Error! Please upload document.', 'خطأ! الرجاء تحميل المستند.'), '', 6)
		}
		else {
			AlertMessage(form, icon_info, msg_loading, '', 6)
			requestFormSubmit(form);
		}
		return false;
	});

	/*upload document*/
	$('#requests .document-upload-div .upload-document').click(function () {
		$(this).siblings('input[type="file"]').trigger('click');
	});

	/*upload document change function*/
	$('#requests .document-upload-div form input[type="file"]').change(function () {
		var input = this;
		var file = $(this)[0].files[0] ? $(this)[0].files[0] : "";
		var filename = defaultFileChosenText;

		if (file) {
			var ext = file.name.split(".");
			ext = ext[ext.length - 1].toLowerCase();
			var fileExtension = ['doc', 'docx', 'pdf'];

			if (fileExtension.lastIndexOf(ext) == -1) {
				AlertMessage($(input).closest('form'), icon_error, ChangeString("Wrong format! Only formats are allowed : ", 'تنسيق خاطئ! مسموح بالتنسيقات فقط :') + fileExtension.join(', '), '', 6)
				$(input).val('').prev('p').text(filename);
			}
			else {
				$(input).prev('p').text(file.name);
			}
		}
		else {
			$(input).val('').prev('p').text(filename);
		}

	});

});
//#endregion

//#region Ajax Call
function LoadDocuments() {
	if (!isDocPageRendered) {
		GetDocuments();
		GetMyDocuments();
		isDocPageRendered = true;
	}
}

function GetDocuments() {
	$.ajax({
		type: 'GET',
		url: '/Customer/Documents/GetDocuments',
		contentType: "application/json",
		success: function (response) {
			BindGridDocuments(response.data);
		}
	});
}

function GetMyDocuments() {
	$.ajax({
		type: 'GET',
		url: '/Customer/Documents/GetMyDocuments',
		contentType: "application/json",
		success: function (response) {
			BindGridMyDocuments(response.data);
		}
	});
}

//#endregion

//#region Functions for Binding Data
function BindGridDocuments(data) {

	$('#requests .documents-div .main-div').empty().show();

	$.each(data, function (k, v) {

		var template = ` <div class="border-2 border-black/20 rounded-lg p-4 flex flex-col items-center my-4 sm:w-[47.5%] md:w-[22.5%] ml-1">
							<h1 class="font-Rubik text-xs font-medium capitalize">${v.Type}</h1>
							<svg viewBox="0 0 384 512" class="fill-current h-16 my-10 sm:my-4 text-black/20 w-10 w-[80%] document-type-svg-image">
							</svg>
							<a href="${v.Path}" target="_blank">
								<h1 class="font-Rubik text-xs font-medium uppercase text-gk-blue">${ChangeString('download','تنزيل')}</h1>
							</a>
						</div>`;

		$('#requests .documents-div .main-div').append(template);

		$('#requests form select').append($("<option />").val(v.ID).text(v.Type));
	});

	if ($('#requests .documents-div .main-div').html().length == 0) {
		$("html, body").animate({ scrollTop: 0 }, 1000);
		$('#requests .documents-div .no-more').fadeIn();
		$("#requests .documents-div .see-more").fadeOut();
	}
}

function BindGridMyDocuments(data) {

	$('#requests .requests-list tbody').html(`<tr>
												<td colspan="4">
													<section class="text-center py-2 w-[100%]" style=""><i class="fa fa-2x fa-circle-o-notch fa-spin"></i></section>
												</td>
											</tr>`).show();

	$.each(data, function (k, v) {

		var fileName = v.Path.split('/').reverse()[0];

		var cancelRequest = `<button onclick="cancelRequest(this,'fa-times-circle text-red-600',${v.ID});" class="inline-flex font-Rubik text-xs font-medium leading-5 text-gray-900 label-light rounded py-1 px-2">
									<i class="fa fa-times-circle text-red-600 mr-1 mt-1"></i> ${ChangeString('Cancel Request', 'إلغاء الطلب')}
							</button>`;
		var remarksBtn = `<button onclick="SeeRemarks(this);" class="inline-flex font-Rubik text-xs font-medium leading-5 text-gray-900 label-light rounded py-1 px-2">
									<i class="fa fa-info-circle text-gk-blue mr-1 mt-1"></i> ${ChangeString('See Remarks', 'راجع الملاحظات')}
									<span class="" style="display:none">${v.Remarks}<span>
							</button>`;

		let status = v.Status == "Pending" ? ChangeString("Pending", "ريثما") : v.Status == "Approved" ? ChangeString("Approved", "وافق") : v.Status == "Cancelled" ? ChangeString("Cancelled", "تم الإلغاء") : v.Status;

		var template = `<tr>
							<td class="px-6 py-2 whitespace-no-wrap border-b border-gray-200">
								<div class="font-Rubik text-xs font-medium leading-5 text-gray-900">${v.ServiceNo}</div>
							</td>
							<td class="px-6 py-2 whitespace-no-wrap border-b border-gray-200">
								<div class="font-Rubik text-xs font-medium leading-5 text-gray-900">${v.Type}</div>
							</td>
							<td class="px-6 py-2 whitespace-no-wrap border-b border-gray-200">
								<a href="${v.Path}">
									<span class="inline-flex font-Rubik text-xs font-medium leading-5 text-gray-900">${fileName}</span>
								</a>
							</td>
							<td class="px-6 py-2 whitespace-no-wrap border-b border-gray-200 label">
								<span class="inline-flex font-Rubik text-xs font-medium leading-5 text-white status ${v.Status == "Approved" ? "label-success" : v.Status == "Cancelled" ? "label-danger" : "label-secondary"}">${status}</span>
							</td>
							<td class="px-6 py-2 whitespace-no-wrap border-b border-gray-200 action label">
								${v.Status == "Pending" ? cancelRequest : remarksBtn}
							</td>
					</tr>`;

		$('#requests .requests-list tbody').append(template);

	});

	$('#requests .requests-list tbody').find('section').closest('tr').remove();

	if ($('#requests .requests-list tbody').html().length == 0)
		$("#requests .requests-list").fadeOut();
	else
		$("#requests .requests-list").fadeIn();

}

//#endregion
//#region Submit Functions
function requestFormSubmit(form) {
	var btn = $(form).find('button[type="submit"]');

	disableSubmitButton(btn, true);

	var formData = new FormData();

	var files = $(form).find('input[type="file"]').get(0).files;
	if (files.length > 0) {
		formData.append("Path", files[0], files[0].name);
	}
	formData.append("__RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());
	formData.append("ID", $(form).find('select').val());
	formData.append("Type", $(form).find('select option:selected').text());

	//ajax start
	$.ajax({
		url: $(form).attr('action'),//@culture/Customer/Documents/Upload
		type: 'Post',
		data: formData,
		contentType: false,
		processData: false,
		success: function (response) {
			if (response.success) {
				AlertMessage(form, icon_success, response.message, '', 6)
				emptyDocumentSectionInputs();
				GetMyDocuments();
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

function emptyDocumentSectionInputs() {
	$('#requests .document-upload-div form input[type="file"]').val('').prev('p').text(defaultFileChosenText);
	$('#requests .document-upload-div form select').prop('selectedIndex', 0);
}

function cancelRequest(btn, spanElement, id) {

	disableSubmitButton(btn, true, spanElement);
	ToastrMessage(icon_info, msg_loading, 6);
	//ajax start
	$.ajax({
		url: `/Customer/Documents/Cancel`,
		type: 'Post',
		data: { id: id },
		success: function (response) {
			if (response.success) {
				ToastrMessage(icon_success, response.message, 6);
				$(btn).closest('td').prev('td').find('.status').text("Cancelled");
				$(btn).remove();
			} else {
				ToastrMessage(icon_error, response.message, 6);
				disableSubmitButton(btn, false, spanElement);
			}
		},
		error: function (e) {
			ToastrMessage(icon_error, ServerError, 6);
			disableSubmitButton(btn, false, spanElement);
		},
		failure: function (e) {
			ToastrMessage(icon_error, ServerError, 6);
			disableSubmitButton(btn, false, spanElement);
		}
	});

}

function SeeRemarks(elem) {

	let remarks = $(elem).find('span').html();
	$('.request-remarks').html(remarks ? remarks : `<p>${ChangeString("No Remarks given yet.", "لم تصدر أي ملاحظات بعد.")}</p>`);
	$('.request-remarks').closest('#Remarks_section').show();
}
//#endregion
