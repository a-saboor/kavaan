"use strict";
var table1;

var KTDatatablesBasicScrollable = function () {

	var initTable1 = function () {
		var table = $('#kt_datatable1');

		// begin first table
		table1 = table.DataTable({
			//scrollY: '50vh',
			scrollX: true,
			scrollCollapse: true,
			"language": {
				processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
			},
			"initComplete": function (settings, json) {
				$('#kt_datatable1 tbody').fadeIn();
			},
			columnDefs: [
				{
					targets: 0,
					className: "dt-left",
					width: '130px',
				}, {
					targets: -1,
					title: 'Actions',
					orderable: false,
					width: '230px',
					className: "dt-center",
					render: function (data, type, full, meta) {
						data = data.split(',');
						var actions = '';
						actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/Vendor/Details/' + data[1] + '\')">' +
							'<i class="fa fa-folder-open"></i>' +
							'</button> ';

						actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1 btnapprove" onclick="OpenModelPopup(this,\'/Admin/Vendor/Approve/' + data[1] + '/?approvalStatus=true\',true)">' +
							'<i class="fa fa-check-circle"></i> Approve' +
							'</button> ';
						actions += '<button type="button" class="btn btn-outline-danger btn-sm mr-1 btnapprove" onclick="OpenModelPopup(this,\'/Admin/Vendor/Approve/' + data[1] + '/?approvalStatus=false\',true)">' +
							'<i class="fa fa-times-circle"></i> Reject' +
							'</button>';

						return actions;
					},
				},
			{
				targets: 1,
				//width: '75px',
				render: function (data, type, full, meta) {

					if (!data) {
						return '<span>-</span>';
					}
					var vendor = data.split('|');

					return '<div class="d-flex align-items-center">' +
								'<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
									'<div class="symbol-label" style="background-image: url(\'' + vendor[0] + '\')"></div>' +
								'</div>' +
								'<div>' +
									'<a href="javascript:void(0);" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + vendor[1] + '</a>' +
									'<span class="text-muted font-weight-bold d-block">' + vendor[2] + '</span>' +
								'</div>' +
							'</div>';
				},
			}
			],
		});
	};

	return {
		//main function to initiate the module
		init: function () {
			initTable1();

			$('.btn').tooltip()
		},
	};
}();

jQuery(document).ready(function () {
	KTDatatablesBasicScrollable.init();
});

function Approve(element, record) {

	swal.fire({
		title: 'Are you sure?',
		text: "You won't be able to revert this!",
		type: 'warning',
		showCancelButton: true,
		confirmButtonText: 'Yes, Approve it!'
	}).then(function (result) {
		if (result.value) {
			$.ajax({
				url: '/Admin/Vendor/Approve/' + record,
				type: 'POST',
				data: {
					"__RequestVerificationToken":
						$("input[name=__RequestVerificationToken]").val()
				},
				success: function (result) {
					if (result.success != undefined) {
						if (result.success) {
							toastr.options = {
								"positionClass": "toast-bottom-right",
							};
							toastr.success(result.message);

							table1.row($(element).closest('tr')).remove().draw();
						}
						else {
							toastr.error(result.message);
						}
					} else {
						swal.fire("Your are not authorize to perform this action", "For further details please contact administrator !", "warning").then(function () {
						});
					}
				},
				error: function (xhr, ajaxOptions, thrownError) {
					if (xhr.status == 403) {
						try {
							var response = $.parseJSON(xhr.responseText);
							swal.fire(response.Error, response.Message, "warning").then(function () {
								$('#myModal').modal('hide');
							});
						} catch (ex) {
							swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact administrator !", "warning").then(function () {
								$('#myModal').modal('hide');
							});
						}

						$(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
						$(element).find('i').show();

					}
				}
			});
		}
	});
}

function Decline(element, record) {

	swal.fire({
		title: 'Are you sure?',
		text: "You won't be able to revert this!",
		type: 'warning',
		showCancelButton: true,
		confirmButtonText: 'Yes, Decline it!'
	}).then(function (result) {
		if (result.value) {
			$.ajax({
				url: '/Admin/Vendor/Decline/' + record,
				type: 'POST',
				data: {
					"__RequestVerificationToken":
						$("input[name=__RequestVerificationToken]").val()
				},
				success: function (result) {
					if (result.success != undefined) {
						if (result.success) {
							toastr.options = {
								"positionClass": "toast-bottom-right",
							};
							toastr.success(result.message);

							table1.row($(element).closest('tr')).remove().draw();
						}
						else {
							toastr.error(result.message);
						}
					} else {
						swal.fire("Your are not authorize to perform this action", "For further details please contact administrator !", "warning").then(function () {
						});
					}
				},
				error: function (xhr, ajaxOptions, thrownError) {
					if (xhr.status == 403) {
						try {
							var response = $.parseJSON(xhr.responseText);
							swal.fire(response.Error, response.Message, "warning").then(function () {
								$('#myModal').modal('hide');
							});
						} catch (ex) {
							swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact administrator !", "warning").then(function () {
								$('#myModal').modal('hide');
							});
						}
						$(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
						$(element).find('i').show();

					}
				}
			});
		}
	});
}

function callback(dialog, elem, isedit, response) {

	if (response.success) {
		toastr.success(response.message);

		if (isedit) {
			table1.row($(elem).closest('tr')).remove().draw();
		}
		table1.row($(elem).closest('tr')).remove();

		//addRow(response.data);
		jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
		jQuery('#myModal').modal('hide');
	}
	else {
		jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

		toastr.error(response.message);
	}
}

function addRow(row) {
	table1.row.add([
		row.Date,
		row.Logo + '|' + row.Name + '|' + row.VendorCode  ,
		row.Email,
		row.IsApproved + ',' + row.ID,
	]).draw(true);

}