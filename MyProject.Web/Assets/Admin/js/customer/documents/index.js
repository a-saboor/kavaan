'use strict';

//#region global variables and arrays

//#endregion

//#region document ready function

$(document).ready(function () {

	fetchDocuments(0);

	$.each($('i[for="Path"]'), function (idx, elem) {
		$(elem).click(function () {
			$(elem).siblings().click();
		});
	});

	initAllFunc();

	//customer documents form
	$('button[form="form_documents_1"]').click(function () {
		const btn = $(this);
		const url = `/${culture}/customer/documents/create`;
		const showSpinner = true;
		const showAlert = true;
		const showToastr = false;
		const btnSpinner = true;
		const msg = "Please fill the form properly...";
		const form = $('#' + $(btn).attr('form'));
		scrollTop(75);
		if (formValidate(form, showAlert, showToastr, msg)) {
			fileFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner);
		}
		return false;
	});

	$('button[form="form_documents_2"]').click(function () {
		const btn = $(this);
		const url = `/${culture}/customer/documents/create`;
		const showSpinner = true;
		const showAlert = true;
		const showToastr = false;
		const btnSpinner = true;
		const msg = "Please fill the form properly...";
		const form = $('#' + $(btn).attr('form'));
		scrollTop(75);
		if (formValidate(form, showAlert, showToastr, msg)) {
			fileFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner);
		}
		return false;
	});
});

//#endregion

//#region Other functions
function fileFormSubmit_callback(form, status, response, url) {

	if (status) {

		$('select[name="CustomerDocumentTypeID"]').prop('selectedIndex', 0).select2();
		$('select[name="CustomerRelationID"]').prop('selectedIndex', 0).select2();

		$.each($('i[for="Path"]'), function (idx, elem) {
			$(elem).removeClass('border-2 border-ak-gold').addClass('border-black/50');
		});

		if (!response.isFamily) {
			fetchDocuments(1)
		}
		else {
			fetchDocuments(2)
		}

	}
}

function fetchDocuments(isFamily = 0) {
	$.ajax({
		type: 'Get',
		url: `/${culture}/customer/Documents/GetDocuments`,
		success: function (response) {
			if (response.success) {
				BindDocuments(response.data);
			} else {
				BindDocuments(0);
				console.log(response.message);
			}
		},
		error: function (e) {
			BindDocuments(0);
			console.log("Get Documents Error!");
		}
	});

	function BindDocuments(data) {

		if (data.length) {
			let personalDocs = data.filter(x => x.IsFamily == false);
			let otherDocs = data.filter(x => x.IsFamily == true);

			if (isFamily == 0) {
				BindDocumentsGrid('#personal_documents .document-grid tbody', personalDocs);
				BindDocumentsGrid('#family_documents .document-grid tbody', otherDocs);
			}
			else if (isFamily == 1) {
				BindDocumentsGrid('#personal_documents .document-grid tbody', personalDocs);
			}
			else if (isFamily == 2) {
				BindDocumentsGrid('#family_documents .document-grid tbody', otherDocs);
			}
		}

	}

	function BindDocumentsGrid(target, data) {

		$(target).html('');

		$.each(data, function (idx, row) {
			let relationTD = "";
			if (row.IsFamily) {
				relationTD = `<td class="text-[0.65rem] leading-3 font-medium px-6 py-2.5 text-center">${row.Relation}</td>`;
			}

			$(target).append(`																				 

					<tr class="bg-white odd:bg-ak-light uppercase text-black/90" data_id="${row.ID}">
						<td class="text-[0.65rem] leading-3 font-medium px-6 py-2.5 text-center">${row.Type}</td>
						<td class="text-[0.65rem] leading-3 font-medium px-6 py-2.5 text-center">${row.DocumentNo}</td>
						${relationTD}
						<td class="text-[0.65rem] leading-3 font-medium px-6 py-2.5 text-center">${row.ExpiryDate}</td>
						<td class="text-[0.65rem] leading-3 font-medium px-6 py-2.5 text-center">
							<div class="flex flex-row flex-wrap justify-center text-[0.65rem] leading-3 gap-2">
								<a target="_blank" href="${row.Path}" class="cursor-pointer bg-ak-gold h-6 w-6 hover:bg-white hover:text-ak-gold hover:border-[1px] hover:border-ak-gold rounded text-white transition-[0.3s_all_ease-in-out] relative" >
									<i class="fa fa-arrow-down text-stroke-ak-gold absolute top-[50%] left-[50%] translate-x-[-50%] translate-y-[-50%]"></i>
								</a>
								${/*<a class="cursor-pointer bg-ak-gold h-6 w-6 hover:bg-white hover:text-ak-gold hover:border-[1px] hover:border-ak-gold rounded text-white transition-[0.3s_all_ease-in-out] relative">
									<i class="fa fa-pen text-stroke-ak-gold absolute top-[50%] left-[50%] translate-x-[-50%] translate-y-[-50%]"></i>
								</a>*/ ''}
								<button type="button" onclick="deleteDocument(this, ${row.ID})" class="cursor-pointer bg-ak-gold h-6 w-6 hover:bg-white hover:text-ak-gold hover:border-[1px] hover:border-ak-gold rounded text-white transition-[0.3s_all_ease-in-out] relative">
									<i class="fa fa-trash text-stroke-ak-gold absolute top-[50%] left-[50%] translate-x-[-50%] translate-y-[-50%]"></i>
								</button>
							</div>
						</td>
					</tr>

			`);

		});
	}
}

function deleteDocument(btn, id) {
	btn = $(btn);
	const url = `/${culture}/customer/documents/delete`;
	const showSpinner = false;
	const showAlert = true;
	const showToastr = false;
	const btnSpinner = false;
	const msg = "Please fill the form properly...";
	const form = $(btn).closest('.document-grid').prev('form');
	scrollTop(75);
	deleteDataByIDForm(form, btn, url, id, showSpinner, showAlert, showToastr, btnSpinner);
	return false;
}

function deleteDataByIDForm_callback(form, id, status, response, url){

	if (status) {
		$('tr[data_id="' + id + '"]').remove();
	}
}
//#endregion

