"use strict";
var vendor;
var table1;
var Url;
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
				if (json.recordsTotal <= 0) {
					$('.excel-btn').prop('disabled', true);
				}
				else {
					$('.excel-btn').prop('disabled', false);
				}
			},
			lengthMenu: [
					[10, 25, 100, 500, -1],
					['10', '25', '100', '500', 'Show all']
			],
			"proccessing": true,
			"serverSide": true,
			"ajax": {
				url: "/Admin/Product/List",
				type: 'POST',
				data: (d) =>{ d.VendorID = vendor },
			},
			"columns": [
			{
				"data": "CreatedOn",
				className: "dt-left",
				width: '130px',
			},
			{
				"mData": null,
				"bSortable": true,
				width: '250px',
				"mRender": function (o) {
					return '<div class="d-flex align-items-center">' +
								'<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
									'<div class="symbol-label" style="background-image: url(\'' + o.Thumbnail + '\')"></div>' +
								'</div>' +
								'<div class="product-name" title="' + o.Name + '">' +
									'<a href="javascript:;" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + o.Name + '</a>' +
									'<span class="text-muted font-weight-bold d-block">' + o.SKU + '</span>' +
								'</div>' +
							'</div>'
				}
			},
			//{ "data": "SKU" },
			{
				"mData": null,
				"bSortable": true,
				width: '70px',
				className: "dt-center",

				"mRender": function (o) {

					if (o.IsManageStock && o.IsManageStock == true) {
						return '<span class="font-weight-bold font-size-sm ' + (o.Stock > 0 ? 'text-success' : 'text-danger') + '">' + (o.Stock > 0 ? 'InStock' : 'Out Of Stock') + ' (' + o.Stock + ') </span>';
					} else {
						var status = {
							"1": {
								'title': 'InStock',
								'class': ' text-success'
							},
							"2": {
								'title': 'Out Of Stock',
								'class': ' text-danger'
							},
						};
						if (typeof status[o.StockStatus] === 'undefined') {
							return '<span class="font-weight-bold font-size-sm text-primary ">-</span>';
						}
						return '<span class="font-weight-bold font-size-sm ' + status[o.StockStatus].class + '">' + status[o.StockStatus].title + ' </span>';
					}
				}
			},
			{
				"mData": null,
				"bSortable": false,
				width: '80px',
				"mRender": function (o) {
					if (o.Categories) {
						var Categories = o.Categories.split(',');
						var CategoriesTemplate = '';
						for (var i = 0; i < Categories.length; i++) {
							CategoriesTemplate += '<a href="javascript:;" class="font-weight-bold text-primary" onclick="Search(this)">' + Categories[i] + '</a>, ';
						}
						return CategoriesTemplate.substring(0, CategoriesTemplate.length - 2);
					}
					return '<span class="font-weight-bold font-size-sm text-primary ">-</span>';
				}
			},
			{
				"mData": null,
				"bSortable": false,
				width: '80px',
				className: "dt-center",
				"mRender": function (o) {
					if (o.Tags) {
						var Tags = o.Tags.split(',');
						var TagsTemplate = '';
						for (var i = 0; i < Tags.length; i++) {
							TagsTemplate += '<a href="javascript:;" class="font-weight-bold text-primary" onclick="Search(this)">' + Tags[i].trim() + '</a>, ';
						}
						return TagsTemplate.substring(0, TagsTemplate.length - 2);
					}
					return '<span class="font-weight-bold font-size-sm text-primary ">-</span>';
				}
			},
			{
				"mData": null,
				"bSortable": true,
				width: '75px',
				className: "dt-center",
				"mRender": function (o) {

					var data = o.IsActive.toString().toUpperCase();
					var status = {
						"TRUE": {
							'title': 'Active',
							'class': ' label-light-success'
						},
						"FALSE": {
							'title': 'InActive',
							'class': ' label-light-danger'
						},
					};
					if (typeof status[data] === 'undefined') {

						return '<span class="label label-lg font-weight-bold label-light-danger label-inline">Inactive</span>';
					}
					return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
				}
			},
			{
				"mData": null,
				//"bSortable": true,
				orderable: false,
				width: '230px',
				className: "dt-center",
				"mRender": function (o) {
					var isActive = o.IsActive.toString().toUpperCase();
					var status = {
						"TRUE": {
							'title': 'Deactivate',
							'icon': 'fa-times-circle',
							'class': ' btn-outline-danger'
						},
						"FALSE": {
							'title': 'Activate',
							'icon': 'fa-check-circle',
							'class': ' btn-outline-success'
						},
					};
					var actions = '';

					actions += '<a class="btn btn-bg-secondary btn-icon btn-sm mr-1 float-left" href="/Admin/Product/Edit/' + o.ID + '">' +
							'<i class="fa fa-pen"></i>' +
							'</a> ';

					actions += '<a class="btn btn-bg-secondary btn-icon btn-sm mr-1 float-left" href="/Admin/Product/Details/' + o.ID + '" title="View">' +
									'<i class="fa fa-eye"></i>' +
								'</a> ';


					if (typeof status[isActive] === 'undefined') {
						actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="Activate(this, ' + o.ID + ')">' +
										'<i class="fa fa-check-circle" aria-hidden="true"></i> Activate' +
									'</button>';
					} else {
						actions += '<button type="button" class="btn btn-sm mr-1' + status[isActive].class + '" onclick="Activate(this, ' + o.ID + ')">' +
										'<i class="fa ' + status[isActive].icon + '" aria-hidden="true"></i> ' + status[isActive].title +
									'</button>';
					}

					return actions;
				}
			}
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
	//selectVendor();
	KTDatatablesBasicScrollable.init();
	
});

$("#VendorID").change(function () {
	vendor = $("#VendorID").val();
	$("#VendorID_Report").val(vendor);
	table1.draw();
})

function selectVendor() {
	
	/*$('#VendorID').prop('disabled', true);*/
	
	const value = $('#VendorID').val();
	table1.draw();
	/*return value;*/
	//$.ajax({
	//	url: '/Admin/Product/List',
	//	type: 'POST',
	//	data: { VendorID: value},
	//	success: function (data) {
	//		"use strict";

	//		if (data != null) {
	//			table1.draw();
	//			/*KTDatatablesBasicScrollable.init();*/
	//		}
	//		$('#VendorID').val(value);
	//		$('#VendorID').prop('disabled', false);
			
	//	}
	//});
}

function Search(element) {
	table1.search($(element).text().trim()).draw();
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
				url: '/Admin/Product/Activate/' + record,
				type: 'Get',
				success: function (response) {
					if (response.success) {
						toastr.success(response.message);
						setTimeout(function () { location.reload(); }, 1000);
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

		window.location.href = response.url;

		if (isedit) {
			table1.row($(elem).closest('tr')).remove().draw();
		}

		//addRow(response.data);
		//jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
		//jQuery('#myModal').modal('hide');
	}
	else {
		jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

		toastr.error(response.message);
	}
}

function addRow(row) {
	table1.row.add([
		row.Date,
		row.Vendor,
		row.Product,
		row.ApprovalStatus,
		row.ApprovalStatus + ',' + row.ID,
	]).draw(true);

}