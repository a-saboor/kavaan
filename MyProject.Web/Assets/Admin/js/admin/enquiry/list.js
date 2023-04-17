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
			"order": [[4, "desc"]],

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
			url: '/Admin/Enquiry/List/',
			type: 'POST',
			data: {
				fromDate: $('#fromDate').val(),
				toDate: $('#toDate').val(),
			},
			success: function (data) {
				"use strict";
				
				if (data != null) {

					$("#customerenquiry").html(data);
					KTDatatablesBasicScrollable.init();
					$('#from').val(fromDate);
					$('#to').val(toDate);

					var td = data.includes("</td>");
					if (td) {
						$('#btnSubmit').prop('disabled', false);
					}
				}
				else {
					$('#btnSubmit').prop('disabled', true);
				}
				$("#btnSearch").removeClass('spinner spinner-left').prop('disabled', false);

			}
		});

	});
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
				url: '/Admin/Award/Delete/' + record,
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

				url: '/Admin/Award/Activate/' + record,
				type: 'Get',
				success: function (response) {
					
					if (response.success) {
						toastr.success(response.message);
						table1.row($(element).closest('tr')).remove().draw();
						addRow(response.data);
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
//function Activate(element, record) {
//	swal.fire({
//		title: 'Are you sure?',
//		text: "You won't be able to revert this!",
//		type: 'warning',
//		showCancelButton: true,
//		confirmButtonText: 'Yes, do it!'
//	}).then(function (result) {
//		if (result.value) {
//			$(element).find('i').hide();
//			$(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);

//			$.ajax({
//				url: '/Admin/Award/Activate/' + record,
//				type: 'Get',
//				success: function (response) {
//					if (response.success) {
//						toastr.success(response.message);
//						table1.row($(element).closest('tr')).remove().draw();
//						addRow(response.data);
//					} else {
//						toastr.error(response.message);
//						$(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
//						$(element).find('i').show();
//					}
//				},
//				error: function (xhr, ajaxOptions, thrownError) {
//					if (xhr.status == 403) {
//						try {
//							var response = $.parseJSON(xhr.responseText);
//							swal.fire(response.Error, response.Message, "warning").then(function () {
//								$('#myModal').modal('hide');
//							});
//						} catch (ex) {
//							swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact administrator !", "warning").then(function () {
//								$('#myModal').modal('hide');
//							});
//						}
//						$(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
//						$(element).find('i').show();

//					}
//				}
//			});
//		} else {
//			//swal("Cancelled", "Your imaginary file is safe :)", "error");
//		}
//	});
//}
function callback(dialog, elem, isedit, response) {

	if (response.success) {
		toastr.success(response.message);

		if (isedit) {
			table1.row($(elem).closest('tr')).remove().draw();
		}

		addRow(response.data);
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
		row.Name,
		row.IsActive,
		row.IsActive + ',' + row.ID,
		row.ID,
	]).draw(true);

}