﻿"use strict";
var table1;
var KTDatatablesBasicScrollable = function () {

    var initTable1 = function () {
        var table = $('#kt_datatable1');
        
        // begin first table
        table1 = table.DataTable({
            //scrollY: '50vh',
            "bPaginate": false,
            "searching": false,
            "bLengthChange": false,
            "bFilter": false,
            "bSort": false,
            "bInfo": false,
            "bAutoWidth": false,
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
                    width: '130px',
                },
                {
                    targets: 1,
                    className: "dt-center",
                },
                {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    width: '250px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {

                        data = data.split(',');
                        var isActive = data[0].toUpperCase();
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

                        actions += 
                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/Car/InsuranceDetails/' + data[1] + '\')" title="View">' +
                            '<i class="fa fa-folder-open"></i>' +
                            '</button> ' 
                            //'<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,' + data[1] + ')"><i class="fa fa-trash"></i></button>';

                       

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


function BindInsurance() {

    $.ajax({
        url: '/Admin/Car/GetInsurances/' + GetURLParameter(),
        type: 'GET',
        success: function (response) {
            if (response.success) {
                $(response.insurance).each(function (k, v) {
                    table1.row.add([
                        v.name ? v.name : '-',
                        v.namear ? v.namear : '-',
                        v.car,
                        v.IsActive + ',' + v.id,

                    ]).draw(false);
                })

            } else {
                $('.car-Package').html('No Packages!');
            }
        }
    });
}
jQuery(document).ready(function () {
    KTDatatablesBasicScrollable.init();
    BindInsurance();
});



//$("#btnSubmit").click(function () {
//	
//	var name = $("#Name").val();
//	var namear = $("#NameArInc").val();
//	var price = $("#Price").val();
//	var detail = $("#Detail").val();
//	var detailar = $("#DetailAr").val();

//	$.ajax({
//		url: '/Vendor/Car/CreateInsurance/',
//		type: 'POST',
//		data: {
//			"insurance": { CarID: GetURLParameter(), Name: name, NameAr: namear, Price: price, Details: detail, DetailsAr: detailar }
//		},
//		success: function (response) {
//			if (response.success) {

//			}
//		},


//	});

//})

//function Delete(element, record) {

//    swal.fire({
//        title: 'Are you sure?',
//        text: "You won't be able to revert this!",
//        type: 'warning',
//        showCancelButton: true,
//        confirmButtonText: 'Yes, delete it!'
//    }).then(function (result) {
//        if (result.value) {

//            $.ajax({
//                url: '/Vendor/Car/DeleteInsuranceConfirmed/' + record,
//                type: 'POST',
//                data: {
//                    "__RequestVerificationToken":
//                        $("input[name=__RequestVerificationToken]").val()
//                },
//                success: function (result) {
//                    if (result.success != undefined) {
//                        if (result.success) {
//                            toastr.options = {
//                                "positionClass": "toast-bottom-right",
//                            };
//                            toastr.success('Insurance Deleted Successfully');

//                            table1.row($(element).closest('tr')).remove().draw();
//                        }
//                        else {
//                            toastr.error(result.message);
//                        }
//                    } else {
//                        swal.fire("Your are not authorize to perform this action", "For further details please contact administrator !", "warning").then(function () {
//                        });
//                    }
//                },
//                error: function (xhr, ajaxOptions, thrownError) {
//                    if (xhr.status == 403) {
//                        try {
//                            var response = $.parseJSON(xhr.responseText);
//                            swal.fire(response.Error, response.Message, "warning").then(function () {
//                                $('#myModal').modal('hide');
//                            });
//                        } catch (ex) {
//                            swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact administrator !", "warning").then(function () {
//                                $('#myModal').modal('hide');
//                            });
//                        }


//                        $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
//                        $(element).find('i').show();

//                    }
//                }
//            });
//        }
//    });
//}

//function Activate(element, record) {
//    swal.fire({
//        title: 'Are you sure?',
//        text: "You won't be able to revert this!",
//        type: 'warning',
//        showCancelButton: true,
//        confirmButtonText: 'Yes, do it!'
//    }).then(function (result) {
//        if (result.value) {
//            $(element).find('i').hide();
//            $(element).addClass('spinner spinner-left spinner-sm').attr('disabled', true);

//            $.ajax({
//                url: '/Vendor/Car/Activate/' + record,
//                type: 'Get',
//                success: function (response) {
//                    if (response.success) {
//                        toastr.success(response.message);
//                        table1.row($(element).closest('tr')).remove().draw();
//                        addRow(response.data);
//                    } else {
//                        toastr.error(response.message);
//                        $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
//                        $(element).find('i').show();
//                    }
//                },
//                error: function (xhr, ajaxOptions, thrownError) {
//                    if (xhr.status == 403) {
//                        try {
//                            var response = $.parseJSON(xhr.responseText);
//                            swal.fire(response.Error, response.Message, "warning").then(function () {
//                                $('#myModal').modal('hide');
//                            });
//                        } catch (ex) {
//                            swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact administrator !", "warning").then(function () {
//                                $('#myModal').modal('hide');
//                            });
//                        }

//                        $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
//                        $(element).find('i').show();

//                    }
//                }
//            });
//        } else {
//            //swal("Cancelled", "Your imaginary file is safe :)", "error");
//        }
//    });
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
        row.Name,
        row.NameAr,
        row.car,
        row.IsActive + ',' + row.ID,
    ]).draw(true);
}