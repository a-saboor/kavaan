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
			"order": [[1, "desc"]],

			"language": {
				processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
			},
			"initComplete": function (settings, json) {
				$('#kt_datatable1 tbody').fadeIn();
			},
			columnDefs: [
				{
					targets: 0,
					className: "dt-center test",
					width: '130px',

				},
				{
					targets: 1,
					className: "",
					width: '130px',

				},
				{
					targets: 4,
					width: 'auto',
					render: function (data, type, full, meta) {
						//
						var mydata = full[5].split(',');
						var status = {
							"Pending": {
								'title': 'Pending',
								'class': ' label-light-dark'
							},
							"Approved": {
								'title': 'Approved',
								'class': ' label-light-success'
							},
							"Unapproved": {
								'title': 'Unapproved',
								'class': ' label-light-danger'
							},
							"Rejected": {
								'title': 'Rejected',
								'class': ' label-light-danger'
							},
							"Cancelled": {
								'title': 'Cancelled',
								'class': ' label-light-danger'
							},

						};
						if (typeof status[data] === 'undefined') {
							return '<a  href="javascript:" class="label label-lg label-light-dark label-inline">' + data + '</a>';
						}
						return '<a href="javascript:" class="label label-lg ' + status[data].class + ' label-inline">' + data + ' </a>';

					},
				},
				{
					targets: -1,
					title: 'Actions',
					orderable: false,
					width: $('#myId').val() == 0 ? '300px' : "auto",
					className: "dt-left",
					render: function (data, type, full, meta) {
						data = data.split(',');
						var Path = data[0];
						var ID = data[1];
						var actions = '';

						var status = {
							"Pending": {
								'title': 'Pending',
								'class': ' label-light-dark'
							},
							"Approved": {
								'title': 'Approved',
								'class': ' label-light-success'
							},
							"Unapproved": {
								'title': 'Unapproved',
								'class': ' label-light-danger'
							},
							"Rejected": {
								'title': 'Rejected',
								'class': ' label-light-danger'
							},
							"Cancelled": {
								'title': 'Cancelled',
								'class': ' label-light-danger'
							},
							//"TRUE": {
							//	'title': 'Deactivate',
							//	'icon': 'fa-times-circle',
							//	'class': ' btn-outline-danger'
							//},
							//"FALSE": {
							//	'title': 'Activate',
							//	'icon': 'fa-check-circle',
							//	'class': ' btn-outline-success'
							//},
						};
						
						actions += '<a  class="btn btn-bg-secondary  btn-sm mr-1" target="_blank" href="' + Path + '">' +
							'<i class="fa fa-eye"></i> View' +
							'</a> ';
						if ($('#myId').val() == 0 && (typeof status[full[4]] === 'undefined' || status[full[4]].title === 'Pending')) {

							actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1 btnapprove" onclick="OpenModelPopup(this,\'/Admin/CustomerDocumentDetail/StatusChange?id=' + ID + '&status=' + 'Approved' + '\',true)">' +
								'<i class="fa fa-check-circle"></i> Approve' +
								'</button> ' +
								'<button type="button" class="btn btn-outline-danger btn-sm mr-1 btnapprove" onclick="OpenModelPopup(this,\'/Admin/CustomerDocumentDetail/StatusChange?id=' + ID + '&status=' + 'Cancelled' + '\',true)">' +
								'<i class="fa fa-times-circle"></i> Reject' +
								'</button> ';
						}
						else {
							actions += '<button type="button" class="btn btn-outline-primary btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/CustomerDocumentDetail/Remarks/' + ID + '\',true)">' +
								'<i class="fa fa-info-circle"></i> Remarks' +
								'</button> ' +
								'';
						}

						return actions;
					},
				},
			],
		});
	};

	return {
		//main function to initiate the module
		init: function () {
			initTable1();
		},
	};
}();

jQuery(document).ready(function () {
	KTDatatablesBasicScrollable.init();
});


function Delete(element, record) {

	swal.fire({
		title: 'Are you sure?',
		text: "You won't be able to revert this!",
		type: 'warning',
		showCancelButton: true,
		confirmButtonText: 'Yes, delete it!'
	}).then(function (result) {
		if (result.value) {

			$.ajax({
				url: '/Admin/CustomerDocument/Delete/' + record,
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
							toastr.success('Document deleted successfully ...');

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

function Activate(element, record) {
	swal.fire({
		title: 'Are you sure?',
		text: "You won't be able to revert this!",
		type: 'warning',
		showCancelButton: true,
		confirmButtonText: 'Yes, do it!'
	}).then(function (result) {
		if (result.value) {
			$(element).find('i').hide();
			$(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);

			$.ajax({
				url: '/Admin/CustomerDocument/Activate/' + record,
				type: 'Get',
				success: function (response) {

					if (response.success) {
						toastr.success(response.message);
						table1.row($(element).closest('tr')).remove().draw();
						addRow(response.data);

						//show remarks popup here
						OpenModelPopup(element, '/Admin/CustomerDocumentDetail/StatusChange/' + record, true);

					} else {
						toastr.error(response.message);
						$(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
						$(element).find('i').show();
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
		} else {
			//swal("Cancelled", "Your imaginary file is safe :)", "error");
		}
	});
}
function callback(dialog, elem, isedit, response) {

	if (response.success) {
		toastr.success(response.message);

		if (isedit) {
			table1.row($(elem).closest('tr')).remove().draw();
		}
		if ($('#myId').val() == 0) {
			addRow(response.data);
		}
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
		row.CreatedOn,
		row.Serviceno,
		row.UserName,
		row.Type,
		row.Status,
		row.Path + ',' + row.ID,
	]).draw(true);

}

function Approve(element, record) {
	swal.fire({
		title: 'Are you sure?',
		text: "Approving Document!",
		type: 'warning',
		showCancelButton: true,
		confirmButtonText: 'Yes, do it!'
	}).then(function (result) {
		if (result.value) {
			$(element).find('i').hide();
			$(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);

			$.ajax({
				url: '/Admin/CustomerDocumentDetail/Approve?id=' + record,
				type: 'Get',
				success: function (response) {
					if (response.success) {
						toastr.success(response.message);
						//window.setTimeout(function () { location.reload() }, 2000)
						table1.row($(element).closest('tr')).remove().draw();
						if ($('#myId').val() == 0) {
							addRow(response.data);
						}

					} else {
						$(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
						swal.fire({
							title: 'An error occur while approving the property. please try again later',
							html: response.message,
							icon: 'error',
							//timer: 10000,
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
							swal.fire("Access Denied", "You are not authorize to perform this action, For further details please contact administrator!", "warning").then(function () {
								$('#myModal').modal('hide');
							});
						}

						$(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
						$(element).find('i').show();

					}
				}
			});
		} else {
			//swal("Cancelled", "Your imaginary file is safe :)", "error");
		}
	});
}