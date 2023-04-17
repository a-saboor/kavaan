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
                FormatPrices();
            },
            columnDefs: [
                {
                    targets: 0,
                    className: "dt-left",
                    width: '130px',
                },
                {
                targets: -1,
                width: '50px',
                title: 'Actions',
                orderable: false,
                className: "dt-center",
                render: function (data, type, full, meta) {
                    var actions = '';
                    var splitdata = full[7].split('|');
                    if (splitdata[1]) {
                        actions +=
                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/ServiceBooking/Details/' + splitdata[0] + '\')" title="View">' +
                            '<i class="fa fa-folder-open"></i>' +
                            '</button> ' +

                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/ServiceBooking/QuotationDetails/' + splitdata[0] + '\')" title="View">' +
                            '<i class="fa fa-eye"></i>' +
                            '</button> ';
                    }
                    else {
                        actions +=
                            '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/ServiceBooking/Details/' + splitdata[0] + '\')" title="View">' +
                            '<i class="fa fa-folder-open"></i>' +
                            '</button> ';
                    }
                    return actions
                },
            },



            {
                targets: 1,
                width: '160px',
                className: "dt-left",
                render: function (data, type, full, meta) {
                    
                    // console.log(data);
                    if (!data) {
                        return '<span>-</span>';
                    }
                    var customer = data.split('|');
                    return '<div class="d-flex align-items-center">' +
                        '<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
                        '<div class="symbol-label" style="background-image: url(\'' + customer[0] + '\')"></div>' +
                        '</div>' +
                        '<div>' +
                        '<a href="javascript:void(0);" class="text-dark-75 text-hover-primary mb-1 ">' + customer[1] + '</a><br>' +
                        // '<a href="javascript:void(0);" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">' + Admin[3] + '</a>' +
                        '<a href="javascript:void(0);" class="label label-inline font-weight-bolder mb-2 text-left opacity-70" >' + customer[2] + '</a>' +
                        '</div>' +
                        '</div>';

                },
                },
            {
                targets: 5,
                width: '50px',
                className: "dt-center",
                render: function (data, type, full, meta) {

                    //data = data.toUpperCase();
                    //var status = {
                    //    "TRUE": {
                    //        'title': 'Active',
                    //        'class': ' label-light-success'
                    //    },
                    //    "FALSE": {
                    //        'title': 'InActive',
                    //        'class': ' label-light-danger'
                    //    },
                    //};
                    var splitdata = full[7].split('|');
                    if (full[6] === 'Diagnosis' || full[6] === 'Invoiced' )
                    {
                        return '<a href="javascript:" class="label label-lg label-light-success label-inline" disabled>' + data + ' </a>';
                    }
                    else if (typeof data === 'undefined') {
                        return '<a href="javascript:" class="label label-lg label-light-danger label-inline" onclick="OpenModelPopup(this,\'/Admin/ServiceBooking/AssignVendor/' + splitdata[0] + '\',true)">' + data + ' </a>';
                    }
                    else if (data === "-") {
                        return '<a href="javascript:" class="label label-lg label-light-danger label-inline" onclick="OpenModelPopup(this,\'/Admin/ServiceBooking/AssignVendor/' + splitdata[0] + '\',true)">' + 'Not Assigned' + ' </a>';
                    }
                    return '<a href="javascript:" class="label label-lg label-light-success label-inline" onclick="OpenModelPopup(this,\'/Admin/ServiceBooking/AssignVendor/' + splitdata[0] + '\',true)">' + data + ' </a>';
                },
                },
                {
                    targets: 4,
                    width: '60px',
                    className: "dt-center",
                },
                {
                    targets: 2,
                    width: '100px',
                    className: "dt-left",
                },
                {
                    targets: 3,
                    width: '90px',
                    className: "dt-left",
                },
            {

                targets: 6,
                width: '75px',
                render: function (data, type, full, meta) {
                    var status = {
                        "Pending": {
                            'title': 'Pending',
                            'class': ' label-light-dark'
                        },
                        "Confirmed": {
                            'title': 'Confirmed',
                            'class': ' label-light-success'
                        },
                    };
                    if (typeof status[data] === 'undefined') {
                        return '<a  href="javascript:" class="label label-lg label-light-dark label-inline">' + data + '</a>';
                    }
                    return '<a href="javascript:" class="label label-lg ' + status[data].class + ' label-inline">' + data + ' </a>';
                },

            },
                //{
                //    targets: 7,
                //    width: '75px',
                //    render: function (data, type, full, meta) {

                //        var status = {
                //            "Pending": {
                //                'title': 'Pending',
                //                'class': ' label-light-dark'
                //            },
                //            "Processing": {
                //                'title': 'Processing',
                //                'class': ' label-light-success'
                //            },
                //            "Fulfilled": {
                //                'title': 'Fulfilled',
                //                'class': ' label-light-primary'
                //            },
                //            "Not Fulfilled": {
                //                'title': 'Not Fulfilled',
                //                'class': ' label-light-danger'
                //            },

                //        };
                //        if (typeof status[data] === 'undefined') {

                //            //return '<a href="javascript:" class="label label-lg label-light-dark label-inline" onclick="OpenModelPopup(this,\'/Admin/Order/ShipmentChange/' + full[6] + '\',true)">' + data + '</a>';
                //            return '<a hidden href="javascript:" class="label label-lg label-light-dark label-inline" >' + data + '</a>';
                //        }
                //        //return '<a href="javascript:" class="label label-lg ' + status[data].class + ' label-inline" onclick="OpenModelPopup(this,\'/Admin/Order/ShipmentChange/' + full[6] + '\',true)">' + data + ' </a>';
                //        return '<a hidden href="javascript:" class="label label-lg ' + status[data].class + ' label-inline" >' + data + ' </a>';
                //    },
                //},
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


    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();

    $('#from').val(fromDate);
    $('#to').val(toDate);

    //$('.kt_datepicker_range').datepicker({
    //    todayHighlight: true,
    //});


    $("#btnSearchfilter").on("click", function () {

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
            url: '/Admin/ServiceBooking/List',
            type: 'POST',
            data: {
                fromDate: $('#fromDate').val(),
                toDate: $('#toDate').val(),
            },
            success: function (data) {
                "use strict";

                if (data != null) {

                    $("#Orders").html(data);
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

function DateFilterCallBack(data, fromDate, toDate, id = "", status = "", url = "") {

    if (data != null) {
        $("#Orders").html(data);
        KTDatatablesBasicScrollable.init();
    }

    //list excel form date inputs
    $('#from').val(fromDate);
    $('#to').val(toDate);
}

function ChangeStatus(element, record) {
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
                url: '/Admin/ServiceBooking/Status/' + record,
                type: 'POST',
                data: JSON.stringify({ status: $(element).text().trim() }),
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
        if (response.data.Status == "Pending") {
            addRow(response.data);
        }
        else if (response.data.Status == "Inprocess") {
            addRow(response.data);
        }
        else if (response.data.Status == "Diagnosis") {
            addRow(response.data);
        }
        else if (response.data.Status == "Invoiced") {
            addRow(response.data);
        }
        else if (response.data.Status == "Completed") {
            table1.row($(elem).closest('tr')).remove();
        }
        else if (response.data.Status == "Canceled") {
            table1.row($(elem).closest('tr')).remove();
        }
        else {
            table1.row($(elem).closest('tr')).remove();
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
        row.Date,
        row.CustomerLogo + '|' + row.CustomerName + '|' + row.CustomerContact,
        row.Service,
        row.BookingNo,
        row.Total,
        row.VendorName,
        row.Status,
        row.ID + "|" + row.IsQuoteApproved,
    ]).draw(true);
}