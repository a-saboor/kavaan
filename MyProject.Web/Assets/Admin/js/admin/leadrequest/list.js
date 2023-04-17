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
					className: "dt-center",
					//width: '130px',
				},
				{
					targets: 2,
					width: '100px',
				},
				{
					targets: 3,
					//width: '75px',
					render: function (data, type, full, meta) {
						if (!data) {
							return '<span>-</span>';
						}
						var car = data.split('|');
						return '<div class="d-flex align-items-center">' +
							'<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
							'<div class="symbol-label" style="background-image: url(\'' + car[0] + '\')"></div>' +
							'</div>' +
							'<div>' +
							'<a href="javascript:void(0);" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + car[1] + '</a>' +
							'</div>' +
							'</div>';
					},
				},
				{
					targets: 4,
					width: '75px',
					render: function (data, type, full, meta) {
						var status = {
							"Pending": {
								'title': 'Pending',
								'class': ' label-light-dark'
							},
							"Processing": {
								'title': 'Processing',
								'class': ' label-light-success'
							},
							"Canceled": {
								'title': 'Canceled',
								'class': ' label-light-danger'
							},
							"Closed": {
								'title': 'Closed',
								'class': ' label-light-info'
							},
						};
						if (typeof status[data] === 'undefined') {
							return '<a  href="javascript:" class="label label-lg label-light-dark label-inline" onclick="OpenModelPopup(this,\'/Admin/LeadRequest/StatusChange/' + full[5] + '\',true)">' + data + '</a>';
						}
						return '<a href="javascript:" class="label label-lg ' + status[data].class + ' label-inline" onclick="OpenModelPopup(this,\'/Admin/LeadRequest/StatusChange/' + full[5] + '\',true)">' + data + ' </a>';

					},
				},
				{
					targets: -1,
					width: '200px',
					title: 'Actions',
					className: "dt-center",
					orderable: false,
					render: function (data, type, full, meta) {
						return '<a type="button" class="btn btn-bg-secondary btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/LeadRequest/Details/' + data + '\')" title="View">' +
							'<i class="fa fa-folder-open"></i> Details' +
							'</a> ';

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
	});

	$("#toDate").change(function () {

		if (new Date($("#fromDate").val()) > new Date($("#toDate").val())) {
			$('#fromDate').datepicker('setDate', new Date($("#toDate").val()));
			$("#fromDate").datepicker("option", "maxDate", new Date($("#toDate").val()));
		}
	});

	//$('.kt_datepicker_range').datepicker({
	//    todayHighlight: true,
	//});


	$("#btnSearch").on("click", function () {
		
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
			url: '/Admin/LeadRequest/List',
			type: 'POST',
			data: {
				fromDate: $('#fromDate').val(),
				toDate: $('#toDate').val(),
			},
			success: function (data) {
				"use strict";

				if (data != null) {
					
					$("#LeadRequest").html(data);
					KTDatatablesBasicScrollable.init();
					$('#from').val(fromDate);
					$('#to').val(toDate);

					var td = data.includes("</td>");
					if (td) {
						$('#btnSubmit').show();
					}
				}
				else {
					$('#btnSubmit').hide();
				}
			}
		});

	})//btnSearch;
});

function callback(dialog, elem, isedit, response) {

	if (response.success) {
		toastr.success(response.message);

		if (isedit) {
			table1.row($(elem).closest('tr')).remove().draw();
		}
		if (response.data.ApprovalStatus == 2) {
		    addRow(response.data);
		}
		else {
		    table1.row($(elem).closest('tr')).remove();
		}

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
		row.RequestNo,
		row.Customer,
		row.Car,
		row.Status,
		row.ID,
	]).draw(true);

}