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
				if (json.recordsFiltered <= 0) {
					$('.excel-btn').prop('disabled', true);
				}
				else {
					$('.excel-btn').prop('disabled', false);
				}
			},
			"drawCallback": function (settings, json) {
				if (settings.aoData.length <= 0) {
					$('.excel-btn').prop('disabled', true);
				}
				else {
					$('.excel-btn').prop('disabled', false);
				}
			},
			lengthMenu: [
				[10, 25, 100, 500],
				['10', '25', '100', '500']
			],
			"proccessing": true,
			"serverSide": true,
			"ajax": {
				url: "/Admin/JobCandidates/List",
				type: 'POST',
				data: function (d) {
					d.min = $('#fromDate').val();
					d.max = $('#toDate').val();
				},
			},
			"columns": [
				{
					"data": "CreatedOn",
					className: "dt-center",
					width: '130px',
				},
				{
					"data": "UCC",
					//className: "dt-center",
					//width: '130px',
				},
				{
					"mData": null,
					"bSortable": true,
					width: '250px',
					"mRender": function (o) {
						return '<div class="d-flex align-items-center">' +
							'<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
							'<div class="symbol-label" style="background-image: url(\'' + o.Photo + '\')"></div>' +
							'</div>' +
							'<div class="product-name" title="' + o.FullName + '" onclick="SearchSpan(this)">' +
							'<a href="javascript:;" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + o.FullName + '</a>' +
							'<span class="text-muted font-weight-bold d-block">' + o.Email + '</span>' +
							'</div>' +
							'</div>'
					}
				},
				{
					"mData": null,
					"bSortable": true,
					//width: '250px',
					"mRender": function (o) {
						return `<div class="d-flex align-items-center">
                                    <div>
                                        <a href="javascript:;" class ="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg" onclick="Search(this)">${o.DepartmentName}</a>
                                        <a href="javascript:;" class="text-muted font-weight-bold d-block text-hover-primary" onclick="Search(this)">${o.JobOpeningTitle}</a>
                                    </div>
                                </div>`;
					}
				},
				{
					"mData": null,
					"bSortable": true,
					//width: '250px',
					"mRender": function (o) {
						if ((!o.Status) || (o.Status && o.Status === "Pending")) {
							return `<a href="javascript:" class="label label-lg label-light-dark label-inline" onclick="OpenModelPopup(this,'/Admin/JobCandidates/ScheduleStatus/${o.ID}',true)">Pending</a>`;
						}
						else if (o.Status === "Approved") {
							return `<a href="javascript:" class="label label-lg label-success label-inline" onclick="OpenModelPopup(this,'/Admin/JobCandidates/ScheduleStatus/${o.ID}',true)">Approved</a>`
						}
						else if (o.Status === "Rejected") {
							return `<a href="javascript:" class="label label-lg label-danger label-inline" onclick="OpenModelPopup(this,'/Admin/JobCandidates/ScheduleStatus/${o.ID}',true)">Rejected</a>`
						}
						else if (o.Status === "Shortlisted") {
							return `<a href="javascript:" class="label label-lg label-info label-inline" onclick="OpenModelPopup(this,'/Admin/JobCandidates/ScheduleStatus/${o.ID}',true)">Shortlisted</a>`
						}
						return o.Status;
					},
				},
				{
					"mData": null,
					"bSortable": true,
					width: '120px',
					className: "dt-center",
					"mRender": function (o) {
						if ((!o.Schedule) || (o.Schedule && o.Schedule === "-")) {
							return '<span>-</span>';
						}
						return `<a href="javascript:" class="label label-lg label-light-dark label-inline">${o.Schedule}</a>`
					},
				},
				{
					"mData": null,
					orderable: false,
					width: '130px',
					className: "dt-center",
					"mRender": function (o) {
						var actions = '';
						actions += '<a  class="btn btn-bg-secondary btn-icon btn-sm mr-1" target="_blank" href="' + o.CV + '"  title="View CV">' +
							'<i class="fa fa-download"></i>' +
							'</a> '
						actions += '<a class="btn btn-bg-secondary btn-icon btn-sm mr-1" href="/Admin/JobCandidates/Details/' + o.ID + '" title="Details">' +
							'<i class="fa fa-folder-open"></i>' +
							'</a> '

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

		$('.FromDate').val($('#fromDate').val());//Fromdate for reporting

	});

	$("#toDate").change(function () {

		if (new Date($("#fromDate").val()) > new Date($("#toDate").val())) {
			$('#fromDate').datepicker('setDate', new Date($("#toDate").val()));
			$("#fromDate").datepicker("option", "maxDate", new Date($("#toDate").val()));
		}

		$('.ToDate').val($('#toDate').val());//ToDate for reporting

	});

	$('.FromDate').val($('#fromDate').val());//Fromdate for reporting
	$('.ToDate').val($('#toDate').val());//ToDate for reporting

	//$('.kt_datepicker_range').datepicker({
	//    todayHighlight: true,
	//});


	$("#btnSearch").on("click", function () {
		$(this).addClass('spinner spinner-sm spinner-left');

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

		table1.ajax.reload();

		$(this).removeClass('spinner spinner-sm spinner-left');


		//$.ajax({
		//    url: '/Admin/JobCandidates/List',
		//    type: 'POST',
		//    data: {
		//        fromDate: $('#fromDate').val(),
		//        toDate: $('#toDate').val(),
		//    },
		//    success: function (data) {
		//        "use strict";

		//        if (data != null) {

		//            $("#Candidates").html(data);
		//            KTDatatablesBasicScrollable.init();
		//        }
		//    }
		//});

	})//btnSearch;

	//$('#form-excel').submit(function () {
	//	$(this).find('.excel-btn').prop('disabled', true).addClass('spinner spinner-sm spinner-left');

	//	return true;
	//});

});

function Search(element) {
	table1.search($(element).text().trim()).draw();
}

function SearchSpan(element) {
	element = $(element).find('span');
	table1.search($(element).text().trim()).draw();
}


function callback(dialog, elem, isedit, response) {

	if (response.success) {
		if (response.message == "Email sending failed ...") {
			toastr.error(response.message);
		}
		else {
			toastr.success(response.message);
		}

		//if (isedit) {
		//    table1.row($(elem).closest('tr')).remove().draw();
		//}

		//addRow(response.data);

		table1.ajax.reload();

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
		row.UCC,
		row.Candidate,
		row.Job,
		row.Status,
		row.Schedule,
		row.Details,
	]).draw(true);

}