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
			"order": [[0, "desc"]],
			"language": {
				processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
			},
			"initComplete": function (settings, json) {
				$('#kt_datatable1 tbody').fadeIn();
			},
			columnDefs: [
				{ 'visible': false, 'targets': [0] },
				{
					targets: 1,
					className: "dt-center",
					width: '130px',
				},
				{
					targets: 2,
					className: "dt-center",
				},
				{
					targets: 5,

					className: "dt-center",
					render: function (data, type, full, meta) {

						data = data.toUpperCase();
						var status = {
							"TRUE": {
								'title': 'Yes',
								'class': ' label-light-success'
							},
							"FALSE": {
								'title': 'No',
								'class': ' label-light-danger'
							},
						};
						if (typeof status[data] === 'undefined') {
							return '<span class="label label-lg font-weight-bold label-light-danger label-inline">Inactive</span>';
						}
						return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
					},
				},
				{
					targets: -2,
					/*title: 'Actions',*/
					orderable: true,
					className: "dt-center",
					render: function (data, type, full, meta) {

						let actions = "";
						data = data.split(',');
						let isApproved = data[0].toUpperCase();
						let isCancelled = data[1].toUpperCase();

						let status = {
							"TRUE": {
								'title': 'Yes',
								'class': ' label-light-success'
							},
							"FALSE": {
								'title': 'No',
								'class': ' label-light-danger'
							},
						};

						if (isCancelled == "TRUE") {
							actions += '<span class="label label-lg font-weight-bold label-light-danger label-inline">Cancelled</span>';
						}
						else if (isApproved == "TRUE") {
							actions += '<span class="label label-lg font-weight-bold label-light-success label-inline">Approved</span>';
						}
						else if (isCancelled == "FALSE" && isApproved == "FALSE") {
							actions += '<span class="label label-lg font-weight-bold label-light-dark label-inline">Pending</span>';
						}
						else {
							actions += '<span class="label label-lg font-weight-bold label-light-dark label-inline">Pending</span>';
						}

						return actions;
					},
				},
				{
					targets: -1,
					/*title: 'Actions',*/
					orderable: false,
					width: '300px',
					className: "dt-left",
					render: function (data, type, full, meta) {

						var actions = '';
						data = data.split(',');
						var id = data[0];
						var isApproved = data[1].toUpperCase();
						var isCancelled = data[2].toUpperCase();

						if ($('#myId').val() == 0) {
							actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/appointment/Edit/' + id + '\',true)">' +
								'<i class="fa fa-pen"></i>' +
								'</button> ' +
								'<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/appointment/Details/' + id + '\')" title="View">' +
								'<i class="fa fa-folder-open"></i>' +
								'</button> ' +
								'<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,' + id + ')"><i class="fa fa-trash"></i></button>';
						} else {
							actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/appointment/Details/' + id + '\')" title="View">' +
								'<i class="fa fa-folder-open"></i>' +
								'</button> ';
						}

						if ($('#myId').val() == 0) {
							if (isCancelled == "TRUE") {
							}
							else if (isApproved == "TRUE") {
								actions += '<button type="button" class="btn btn-outline-danger btn-sm mr-1" onclick="Cancel(this, ' + id + ')">' +
									'<i class="fa fa-times-circle" aria-hidden="true"></i> Cancel' +
									'</button>';
							}
							else if (isCancelled == "FALSE" && isApproved == "FALSE") {
								actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="Activate(this, ' + id + ')">' +
									'<i class="fa fa-check-circle" aria-hidden="true"></i> Approve' +
									'</button>';
								actions += '<button type="button" class="btn btn-outline-danger btn-sm mr-1" onclick="Cancel(this, ' + id + ')">' +
									'<i class="fa fa-times-circle" aria-hidden="true"></i> Cancel' +
									'</button>';
							}
							else {
								actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="Activate(this, ' + id + ')">' +
									'<i class="fa fa-check-circle" aria-hidden="true"></i> Approve' +
									'</button>';
								actions += '<button type="button" class="btn btn-outline-danger btn-sm mr-1" onclick="Cancel(this, ' + id + ')">' +
									'<i class="fa fa-times-circle" aria-hidden="true"></i> Cancel' +
									'</button>';
							}
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

	const statusId = $('#myId').val();

	$("#fromDate").datepicker({
		todayHighlight: true,
	});

	$("#toDate").datepicker({
		todayHighlight: true,
	});

	$("#fromDate").change(function () {

		if (new Date($("#fromDate").val()) > new Date($("#toDate").val())) {
			$('#toDate').datepicker('setDate', new Date($("#fromDate").val()));
			$("#toDate").datepicker("option", "minDate", new Date($("#fromDate").val()));

		}
		//$("input[name=fromDate]").val($("#fromDate").val());
	});

	$("#toDate").change(function () {

		if (new Date($("#fromDate").val()) > new Date($("#toDate").val())) {
			$('#fromDate').datepicker('setDate', new Date($("#toDate").val()));
			$("#fromDate").datepicker("option", "maxDate", new Date($("#toDate").val()));

		}
		//$("input[name=ToDate]").val($("#toDate").val());
	});

	//$("#fromDate").trigger('change');
	//$("#toDate").trigger('change');

	//$('.kt_datepicker_range').datepicker({
	//    todayHighlight: true,
	//});

	var fromDate = $('#fromDate').val();
	var toDate = $('#toDate').val();

	$('#from').val(fromDate);
	$('#to').val(toDate);

	$("#btnSearch").on("click", function () {
		
		$("#btnSearch").addClass('spinner spinner-left').prop('disabled', true);

		var fromDate = $('#fromDate').val();
		var toDate = $('#toDate').val();

		if (fromDate == "" && toDate == "") {
			Swal.fire({
				icon: 'error',
				title: 'Oops...',
				text: 'Please! Select Date',
			})
		}
		else if (fromDate == "") {
			Swal.fire({
				icon: 'error',
				title: 'Oops...',
				text: 'Please! Select From Date',
			})
		}
		else if (toDate == "") {
			Swal.fire({
				icon: 'error',
				title: 'Oops...',
				text: 'Please! Select To Date',
			})
		}

		$.ajax({
			url: '/Admin/Appointment/List',
			type: 'POST',
			data: {
				fromDate: $('#fromDate').val(),
				toDate: $('#toDate').val(),
				id: statusId
			},
			success: function (data) {
				"use strict";
				
				if (data != null) {

					$("#appointments").html(data);
					KTDatatablesBasicScrollable.init();
					$('#from').val(fromDate);
					$('#to').val(toDate);

					if ($(data).find('tbody tr').length) {
						$('#btnSubmit').prop('disabled', false);
					}
					else {
						$('#btnSubmit').prop('disabled', true);
					}
				}
				else {
					$('#btnSubmit').prop('disabled', true);
				}
				$("#btnSearch").removeClass('spinner spinner-left').prop('disabled', false);

			}
		});

	});

	$('#status').val(statusId);
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
				url: '/Admin/appointment/Delete/' + record,
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
							toastr.success('Appointment deleted successfully ...');

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
				url: '/Admin/Appointment/Activate/' + record,
				type: 'Get',
				success: function (response) {
					if (response.success) {
						toastr.success(response.message);
						table1.row($(element).closest('tr')).remove().draw();
						addRow(response.data);

						//show remarks popup here
						OpenModelPopup(element, '/Admin/Appointment/Remarks/' + record, true);

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

function Cancel(element, record) {
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
				url: '/Admin/Appointment/Cancel/' + record,
				type: 'Get',
				success: function (response) {
					if (response.success) {
						toastr.success(response.message);
						table1.row($(element).closest('tr')).remove().draw();
						if ($('#myId').val() == 0) {
							//addRow(response.data);
						}

						//show remarks popup here
						OpenModelPopup(element, '/Admin/Appointment/Remarks/' + record, true);

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
		row.ID,
		row.Date,
		row.UserName,
		row.AppointmentDate,
		row.Type,
		row.IsCompleted,
		row.IsApproved + ',' + row.IsCancelled,
		row.ID + ',' + row.IsApproved + ',' + row.IsCancelled,
	]).draw(true);
}