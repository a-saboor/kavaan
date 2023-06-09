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
                {
                    targets: 0,
                    className: "dt-left",
                    width: '130px',
                },
                {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    width: '230px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                        data = data.split(',');
                        var isActive = data[0].toUpperCase();
                        var actions = '';
                        if (full[2] === "admin@KAVAAN.com") {
                            actions += '<button type="button" class="btn btn-secondary btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/blog/Details/' + data[1] + '\')" title="View">' +
                                '<i class="fa fa-folder-open"></i> Details' +
                                '</button> ';
                        } else {

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

                            actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/teams/Edit/' + data[1] + '\',true)">' +
                                '<i class="fa fa-pen"></i>' +
                                '</button> ' +
                                '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/teams/Details/' + data[1] + '\')" title="View">' +
                                '<i class="fa fa-folder-open"></i>' +
                                '</button> ' +
                                '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,' + data[1] + ')"><i class="fa fa-trash"></i></button>';

                            if (typeof status[isActive] === 'undefined') {
                                actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="Activate(this, ' + data[1] + ')">' +
                                    '<i class="fa fa-check-circle" aria-hidden="true"></i> Activate' +
                                    '</button>';
                            } else {
                                actions += '<button type="button" class="btn btn-sm mr-1' + status[isActive].class + '" onclick="Activate(this, ' + data[1] + ')">' +
                                    '<i class="fa ' + status[isActive].icon + '" aria-hidden="true"></i> ' + status[isActive].title +
                                    '</button>';
                            }
                        }
                        return actions;
                    },
                },
                {
                    targets: 1,
                    width: '280px',
                    render: function (data, type, full, meta) {
                        if (!data) {
                            return '<span>-</span>';
                        }
                        var teams = data.split('|');
                        return '<div class="d-flex align-items-center">' +
                            '<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
                            '<div class="symbol-label" style="background-image: url(\'' + teams[0] + '\')"></div>' +
                            '</div>' +
                            '<div>' +
                            '<a href="javascript:void(0);" class="text-dark-75 font-weight-bolder mb-1 font-size-lg">' + teams[1] + '</a><br>' +
                            '<a class="text-dark-75 text-hover-primary mb-1 font-size-lg">' + teams[2] + '</a><br>' +
                            '<a href="javascript:void(0);" class="label label-inline font-weight-bolder mb-2 text-left opacity-70" >' + '+' + teams[3] + '</a>' +
                            '</div>';
                    },
                },
                {
                    targets: 3,
                    width: '75px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {

                        data = data.toUpperCase();
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
                url: '/Admin/teams/Delete/' + record,
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
                            toastr.success('WorkForce deleted successfully ...');

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
                url: '/Admin/teams/Activate/' + record,
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
        row.Image + '|' + row.Name + '|' + row.Email + '|' + row.Contact1,
        row.Designation,
        row.IsActive,
        row.IsActive + ',' + row.ID,
    ]).draw(true);

}