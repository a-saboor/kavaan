﻿"use strict";

var table1;
var ProductSelection = [];
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
			"drawCallback": function (settings) {
				//if ($('#checkAll').prop('checked')) {
				//	$('input[name=chkProduct]').prop('checked', true);
				//}

				if (!ProductSelection) {
					ProductSelection = [];
				}

				ProductSelection.forEach(function (obj) {
					$(`input[id=chk${obj}]`).prop('checked', true);
				});

			},
			lengthMenu: [
				[10, 25, 100, 500, -1],
				['10', '25', '100', '500', 'Show all']
			],
			"proccessing": true,
			"serverSide": true,
			"ajax": {
				url: "/Vendor/Product/ApprovalList",
				type: 'POST',
			},
			"columns": [
				{
					"mData": null,
					"bSortable": false,
					className: "dt-center",
					width: '50px',
					"mRender": function (o) {
						if (o.ApprovalStatus == 1 || o.ApprovalStatus == 4) {
							return `<label class="checkbox checkbox-lg checkbox-inline">
										<input type="checkbox" value="${o.ID}" id="chk${o.ID}" name="chkProduct" onchange="ProductSelected(this,${o.ID})">
										<span></span>
									</label>`;
						} else {
							return ``;
						}
					}
				},
				{
					"data": "CreatedOn",
					className: "dt-left",
					width: '130px',
				},
				{
					"mData": null,
					"bSortable": true,
					"mRender": function (o) {
						return '<div class="d-flex align-items-center">' +
							'<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
							'<div class="symbol-label" style="background-image: url(\'' + o.Thumbnail + '\')"></div>' +
							'</div>' +
							'<div class="product-name">' +
							'<a href="javascript:;" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + o.Name + '</a>' +
							//'<span class="text-muted font-weight-bold d-block">' + o.Name + '</span>' +
							'</div>' +
							'</div>'
					}
				},
				{
					"data": "SKU",
					className: "dt-center",
					width: '100px',
				},
				{
					"mData": null,
					"bSortable": false,
					width: '150px',
					"mRender": function (o) {
						if (o.Categories) {
							var Categories = o.Categories.split(',');
							var CategoriesTemplate = '';
							for (var i = 0; i < Categories.length; i++) {
								CategoriesTemplate += '<a href="javascript:;" class="font-weight-bold text-primary" >' + Categories[i] + '</a>, ';
							}
							return CategoriesTemplate.substring(0, CategoriesTemplate.length - 2);
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

						var data = o.ApprovalStatus.toString().toUpperCase();
						var status = {
							"1": {
								'title': 'New',
								'class': ' label-light-success'
							},
							"2": {
								'title': 'Pending',
								'class': ' label-light-primary'
							},
							"3": {
								'title': 'Approved',
								'class': ' label-light-success'
							},
							"4": {
								'title': 'Rejected',
								'class': ' label-light-danger'
							},
						};

						if (typeof status[data] === 'undefined') {

							return '<span class="label label-lg font-weight-bold label-light-danger label-inline">New</span>';
						}
						return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
					}
				},
				{
					"mData": null,
					//"bSortable": true,
					orderable: false,
					width: '300px',
					className: "dt-center",
					"mRender": function (o) {
						//var IsApproved = o.IsApproved.toString().toUpperCase();
						var ApprovalStatus = o.ApprovalStatus;
						var actions = '';

						actions += `<a class="btn btn-bg-secondary btn-icon btn-sm mr-1" href="/Vendor/Product/Edit/${o.ID}" title="View">
										<i class="fa fa-pen"></i>
									</a> `;
						actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,' + o.ID + ')"><i class="fa fa-trash"></i></button>';

						if (ApprovalStatus == 1) {
							actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="IsApproved(this, ' + o.ID + ')",true)">' +
								'<i class="fa fa-check-circle"></i> Send For Approval' +
								'</button> ';
						}
						else if (ApprovalStatus == 2) {
							actions += '<button hidden type="button" class="btn btn-outline-danger btn-sm mr-1" onclick="IsApproved(this, ' + o.ID + ')",true)">' +
								'<i class="fa fa-times-circle"></i> Cancel' +
								'</button> ' +
								'<button type="button" class="btn btn-outline-danger btn-sm mr-1" onclick="IsCanceled(this, ' + o.ID + ')",true)">' +
								'<i class="fa fa-times-circle"></i> Cancel Request' +
								'</button> ';
						}
						else if (ApprovalStatus == 4) {
							actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="IsApproved(this, ' + o.ID + ')",true)">' +
								'<i class="fa fa-check-circle"></i> Send For Approval' +
								'</button> ' +
								'<button type="button" class="btn btn-outline-danger btn-sm mr-1" onclick="OpenModelPopup(this,\'/Vendor/Product/Remarks/' + o.ID + '/?approvalStatus=true\',true)">' +
								'<i class="fa fa-comment-alt"></i> Remarks' +
								'</button> ';
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
	KTDatatablesBasicScrollable.init();

	$(".btn_approveall").prop("disabled", true);

	$("#checkAll").click(function () {
		if (this.checked) {
			$('input[name=chkProduct]').prop('checked', this.checked);

			$("input[name=chkProduct]:checked").each(function (k, elem) {

				if (!ProductSelection) {
					ProductSelection = [];
				}

				var product = ProductSelection.find(function (obj) { return obj == $(elem).val() });

				if ($("#checkAll").prop('checked')) {
					if (!product) {
						ProductSelection.push($(elem).val());
					}
				} else {
					if (product) {
						ProductSelection = ProductSelection.filter(function (obj) { return obj != $(elem).val() });
					}
				}
			});

			$(".btn_approveall").prop("disabled", false);
			$(".btnapprove").toggle("disabled");

		} else {
			$('input[name=chkProduct]').prop('checked', false);
			ProductSelection = [];
		}

		var chk = ProductSelection.length;
		if (chk > 0) {
			$(".btnapprove").hide();
			$(".btn_approveall").prop("disabled", false);

			$(".btn_approveall").html(`<i class="fa fa-check-circle"></i> Send For Approval (${chk})`);
		}
		else {
			$(".btn_approveall").prop("disabled", true);
			$(".btnapprove").show();

			$(".btn_approveall").html(`<i class="fa fa-check-circle"></i> Send For Approval (0)`);
		}
	});
});

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
				url: '/Vendor/User/Activate/' + record,
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
				url: '/Vendor/Product/Delete/' + record,
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
							toastr.success('Product Deleted Successfully');

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
function IsCanceled(element, record) {
	swal.fire({
		title: 'Product Approval Cancel',
		text: "You won't be able to revert this!",
		type: 'warning',
		showCancelButton: true,
		confirmButtonText: 'Yes, do it!'
	}).then(function (result) {
		if (result.value) {
			$(element).find('i').hide();
			$(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);

			$.ajax({
				url: '/Vendor/Product/CancelApproval/' + record,
				type: 'Get',
				success: function (response) {
					if (response.success) {
						toastr.success(response.message);
						setTimeout(function () {
							location.reload();
						}, 1000);
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

function IsApproved(element, record) {
	swal.fire({
		title: 'Product Approval',
		text: "Send Your product for Approval !",
		type: 'warning',
		showCancelButton: true,
		confirmButtonText: 'Yes, do it!'
	}).then(function (result) {
		if (result.value) {
			$(element).find('i').hide();
			$(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);

			$.ajax({
				url: '/Vendor/Product/IsApproved/' + record,
				type: 'Get',
				success: function (response) {
					if (response.success) {
						toastr.success(response.message);
						setTimeout(function () {
							location.reload();
						}, 1000);

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

function ProductSelected(element, record) {

	if (!ProductSelection) {
		ProductSelection = [];
	}

	var product = ProductSelection.find(function (obj) { return obj == $(element).val() });

	if ($(element).prop('checked')) {
		if (!product) {
			ProductSelection.push($(element).val());
		}
	} else {
		if (product) {
			ProductSelection = ProductSelection.filter(function (obj) { return obj != $(element).val() });
		}
	}

	var chk = ProductSelection.length;
	if (chk > 0) {
		$(".btnapprove").hide();
		$(".btn_approveall").prop("disabled", false);

		$(".btn_approveall").html(`<i class="fa fa-check-circle"></i> Send For Approval (${chk})`);
	}
	else {
		$(".btn_approveall").prop("disabled", true);
		$(".btnapprove").show();

		$(".btn_approveall").html(`<i class="fa fa-check-circle"></i> Send For Approval (${chk})`);
	}
}

function BulkSendForApproval(element, record) {

	swal.fire({
		title: 'Are you sure?',
		text: "You won't be able to revert this!",
		type: 'warning',
		showCancelButton: true,
		confirmButtonText: 'Yes, send it!'
	}).then(function (result) {
		if (result.value) {

			$(element).addClass('spinner spinner-sm spinner-left').attr('disabled', true);
			$(element).find('i').hide();

			//var SelectedProducts = [];
			//$("input[name=chkProduct]:checked").each(function () {
			//	SelectedProducts.push($(this).val());
			//});

			if (ProductSelection.length > 0) {
				$.ajax({
					url: '/Vendor/Product/BulkSendForApproval',
					type: 'POST',
					data: { 'ids': ProductSelection },
					success: function (response) {
						if (response.success) {
							toastr.options = {
								"positionClass": "toast-bottom-right",
							};

							toastr.success(response.message);

							setTimeout(function () {
								location.reload();
							}, 1000);
						} else {
							toastr.error(response.message);

							$(element).removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
							$(element).find('i').show();
						}
					},
					error: function (xhr, ajaxOptions, thrownError)
					{
						if (xhr.status === 403) {
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
				toastr.error("Please select products first!");
			}
		}
	});
}

function callback(dialog, elem, isedit, response) {

	if (response.success) {
		toastr.success(response.message);
		console.log(addRow(response.data));
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