﻿"use strict";
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
                //{
                //	targets: 0,
                //	className: "dt-center",
                //	width: '130px',
                //},
                {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    width: '230px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                        var actions = '';
                        actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/propertyfeatures/Edit/' + data + '\',true)">' +
                            '<i class="fa fa-pen"></i>' +
                            '</button> ' +
                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/propertyfeatures/Details/' + data + '\')" title="View">' +
                            '<i class="fa fa-folder-open"></i>' +
                            '</button> ' +
                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,' + data + ')"><i class="fa fa-trash"></i></button>';
                        return actions;
                    },
                },
                {
                    targets: 0,
                    width: '250px',

                    render: function (data, type, full, meta) {
                        if (data == "-") {
                            return "-";
                        }
                        data = data.split('|');
                        return '<div class="d-flex align-items-center">' +
                            '<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
                            '<div class="symbol-label" style="background-image: url(\'' + data[0] + '\')"></div>' +
                            '</div>' +
                            '<div class="product-name" title="' + data[1] + '">' + data[1] + '' +
                            '</div>' +
                            '</div>'

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
                url: '/Admin/propertyfeatures/Delete/' + record,
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
//				url: '/Admin/propertyfeatures/Activate/' + record,
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
        row.FeatureImage + "|" + row.FeatureName,
        row.Poisition,
        row.ID
    ]).draw(true);

}