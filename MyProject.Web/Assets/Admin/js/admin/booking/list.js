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
            "order": [[1, "desc"]],
            "language": {
                processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
            },
            "initComplete": function (settings, json) {
                $('#kt_datatable1 tbody').fadeIn();
            },
            columnDefs: [
                {
                    targets: 0,
                    className: "dt-center test",
                    width: '130px',

                },
                {
                    targets: 1,
                    className: "",
                    width: 'auto',

                },
                {
                    targets: 2,
                    className: "",
                    width: 'auto',

                },
                {
                    targets: 3,
                    className: "",
                    width: 'auto',

                },
                {
                    targets: 4,
                    width: '270px',
                    render: function (data, type, full, meta) {

                        if (!data) {
                            return '<span>-</span>';
                        }
                        var customer = data.split(',');
                        return '<div class="d-flex align-items-center">' +
                            '<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
                            '<div class="symbol-label" style="background-image: url(\'' + customer[0] + '\')"></div>' +
                            '</div>' +
                            '<div>' +
                            '<a href="javascript:void(0);" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + customer[1] + '</a><br>' +
                            '<a href="javascript:void(0);" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + customer[2] + '</a>' +
                            '<span class="text-muted font-weight-bold d-block">' + customer[3] + '</span>' +
                            '</div>' +
                            '</div>';

                    },
                },
                {
                    targets: 5,
                    width: 'auto',
                    render: function (data, type, full, meta) {
                        const td = full[6].split(',');

                        var status = {
                            "Pending": {
                                'title': 'Pending',
                                'class': ' label-light-dark'
                            },
                            "Authorized": {
                                'title': 'Authorized',
                                'class': ' label-light-warning'
                            },
                            "Captured": {
                                'title': 'Captured',
                                'class': ' label-light-success'
                            },
                            "Cancelled": {
                                'title': 'Cancelled',
                                'class': ' label-light-danger'
                            },

                        };
                        if (typeof status[data] === 'undefined') {
                            return '<a  href="javascript:" class="label label-lg label-light-dark label-inline">' + data + '</a>';
                        }
                        return '<a href="javascript:" class="label label-lg ' + status[data].class + ' label-inline" onclick="OpenModelPopup(this,\'/Admin/UnitBooking/ChangeStatus/' + td[1] + '\', true)" title="Change Status">' + data + ' </a>';

                    },
                },
                {
                    targets: -1,
                    width: '100px',
                    title: 'Actions',
                    orderable: false,
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

                        actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/UnitBooking/Details/' + data[1] + '\')" title="View">' +
                            '<i class="fa fa-folder-open"></i>' +
                            '</button>';

                        //actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/UnitBooking/Edit/' + data[1] + '\',true)">' +
                        //	'<i class="fa fa-pen"></i>' +
                        //	'</button> ' +
                        //	'<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/UnitBooking/Details/' + data[1] + '\')" title="View">' +
                        //	'<i class="fa fa-folder-open"></i>' +
                        //	'</button> ' +
                        //	'';

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

    datepickerFunction();

    //#region search filter function
    $("#btnSearch").on("click", function () {
        $("#btnSearch").addClass('spinner spinner-left').prop('disabled', true);
        const value = $('#BookingStatus').val();

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
            url: '/Admin/UnitBooking/List',
            type: 'POST',
            data: {
                fromDate: $('#fromDate').val(),
                toDate: $('#toDate').val(),
                status: value,
            },
            success: function (data) {
                "use strict";

                if (data != null) {

                    $("#bookings").html(data);
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
                $("#btnSearch").removeClass('spinner spinner-left').prop('disabled', false);
                $('#status').val(value);
                $('#BookingStatus').val(value);
                datepickerFunction();
            }
        });

    })//btnSearch;
    //#endregion

});

//#region datepicker functions
function datepickerFunction() {

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

    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();

    $('#from').val(fromDate);
    $('#to').val(toDate);
}
//#endregion

//#region status filter function
function selectStatus() {
    $('#BookingStatus').prop('disabled', true);
    const value = $('#BookingStatus').val();

    $.ajax({
        url: '/Admin/UnitBooking/List',
        type: 'POST',
        data: {
            fromDate: $('#fromDate').val(),
            toDate: $('#toDate').val(),
            status: value,
        },
        success: function (data) {
            "use strict";

            if (data != null) {
                $("#bookings").html(data);
                KTDatatablesBasicScrollable.init();

                if ($(data).find('tbody tr').length) {
                    $('#btnSubmit').prop('disabled', false);
                }
                else {
                    $('#btnSubmit').prop('disabled', true);
                }
            }

            $('#status').val(value);
            $('#BookingStatus').val(value);
            $('#BookingStatus').prop('disabled', false);
            datepickerFunction();
        }
    });
}
//#endregion

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
                url: '/Admin/City/Delete/' + record,
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
                            toastr.success('City Deleted Successfully');

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
                url: '/Admin/City/Activate/' + record,
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
        row.Date,
        row.reference,
        row.project,
        row.unit,
        row.customer,
        row.Status,
        row.Status + "," + row.ID,
    ]).draw(true);

}

Number.prototype.padLeft = function (base, chr) {
    var len = (String(base || 10).length - String(this).length) + 1;
    return len > 0 ? new Array(len).join(chr || '0') + this : this;
}