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
            "order": [[0, "desc"]],
            columnDefs: [
                {
                    "targets": 0,
                    "visible": false,
                    "searchable": false,
                    /* "order": [[0, "desc"]]*/
                },
                {
                    targets: 1,
                    className: "dt-center",
                    width: '130px',
                },
              
                {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    width: '300px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                        data = data.split(',');
                        var actions = '';

                        actions += '<a href="/Admin/unit/Edit/' + data[0] + '" class="btn btn-bg-secondary btn-icon btn-sm mr-1" >' +
                            '<i class="fa fa-pen"></i>' +
                            '</a> ' +
                            '<a href="/Admin/unit/Details/' + data[0] + '" class="btn btn-bg-secondary btn-icon btn-sm mr-1"  title="View">' +
                            '<i class="fa fa-folder-open"></i>' +
                            '</a> ' +
                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,' + data[0] + ')"><i class="fa fa-trash"></i></button>';






                        actions += '<div class="dropdown dropdown-inline" style=""><a href="javascript:;" class="btn btn-bg-secondary btn-icon btn-sm mr-1" data-toggle="dropdown" aria-expanded="false"><i class="la la-cog"></i></a>' + '<div class="dropdown-menu dropdown-menu-sm dropdown-menu-right" style="display: none;"><ul class="nav nav-hoverable flex-column">' +
                            '<li class="nav-item"><a class="nav-link" href="/Admin/UnitImages/CreateUnitImageGallery/' + data[0] +'"><i class="nav-icon la la-upload"></i><span class="nav-text">Unit Images</span></a></li>' +
                            '<li class="nav-item"><a class="nav-link" href="/Admin/unitpaymentplan/Index?unitid=' + data[0] + '"><i class="nav-icon fas fa-coins"></i><span class="nav-text"> ' +
                            'Payment Plan</span ></a ></li ></ul > ' + '</div ></div > ';
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
                url: '/Admin/unit/Delete/' + record,
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


function ActivateFeature(element, record) {
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
                url: '/Admin/Unit/ActivateFeature/' + record,
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
        row.CreatedOn,
        row.Title,
        row.Address,
        row.IsFeatured,
        row.IsFeatured + ',' + row.ID + ',' + row.Title,
    ]).draw(true);



}


