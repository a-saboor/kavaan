"use strict";
var table1;
var KTDatatablesBasicScrollable = function () {

    var initTable1 = function () {
        var table = $('#kt_datatable1');

        // begin first table
        table1 = table.DataTable({
            scrollY: '50vh',
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
                    width: '150px',
                    title: 'Actions',
                    className: "dt-center",
                    orderable: false,
                    render: function (data, type, full, meta) {
                        var ApprovalStatus;
                        data = data.toLowerCase();
                        data = data.split(',');
                        ApprovalStatus = data[0];
                       

                        var actions = '';

                        actions += '<a type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Admin/ServiceRatings/Details/' + data[1] + '\')" title="View">' +
                            '<i class="fa fa-folder-open"></i>' +
                            '</a> ';
                        if (ApprovalStatus == "") {
                            actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="Approval(this,' + data[1] + ', ' + true + ',true)">' +
                                '<i class="fa fa-check-circle"></i> Approve' +
                                '</button> ' +
                                '<button type="button" class="btn btn-outline-danger btn-sm mr-1" onclick="Approval(this,' + data[1] + ', ' + false + ',true)">' +
                                '<i class="fa fa-times-circle"></i> Reject' +
                                '</button> ';
                        } else if (ApprovalStatus == "true") {
                            actions += '<button type="button" class="btn btn-outline-danger btn-sm mr-1" onclick="Approval(this,' + data[1] + ', ' + false + ',true)">' +
                                '<i class="fa fa-times-circle"></i> Reject' +
                                '</button> ';
                        } else if (ApprovalStatus == "false") {
                            actions += '<button type="button" class="btn btn-outline-success btn-sm mr-1" onclick="Approval(this,' + data[1] + ', ' + true + ',true)">' +
                                '<i class="fa fa-check-circle"></i> Approve' +
                                '</button> ';
                        }


                        return actions;
                    },
                },
                {
                    targets: 3,
                    width: '100px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {

                        var actions = '';

                        actions +=
                            '<button class="btn btn-bg-light btn-rating p-1" data="' + data + '">' +
                            '<i class="la la-star-o"></i>' +
                            '<i class="la la-star-o"></i>' +
                            '<i class="la la-star-o"></i>' +
                            '<i class="la la-star-o"></i>' +
                            '<i class="la la-star-o"></i>' +
                            '</button>';
                        return actions;
                    },
                },
                {
                    targets: 4,
                    width: '75px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                        data = data.toUpperCase();
                        var status = {
                            "TRUE": {
                                'title': 'Completed',
                                'class': ' label-light-success'
                            },

                            "FALSE": {
                                'title': 'Rejected',
                                'class': ' label-light-danger'
                            },
                        };
                        if (typeof status[data] === 'undefined') {

                            return '<span class="label label-lg font-weight-bold label-primary-light label-inline">Pending</span>';
                        }
                        return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
                    },
                },
                {
                    targets: 5,
                    width: '75px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                        
                        data = data.toLowerCase();
                        var actions = '';
                        if (data == "true") {
                            actions +=
                                '<label class="switch">' +
                                '<input type="checkbox" checked>' +
                                '<span class="slider round"></span>' +
                                '</label> ';
                        }
                        if (data == "false") {
                            actions +=
                                '<label class="switch">' +
                                '<input type="checkbox" disabled>' +
                                '<span class="slider round"></span>' +
                                '</label> ';
                        }
                        if (data == "") {
                            actions +=
                                '<label class="switch">' +
                                '<input type="checkbox" disabled>' +
                                '<span class="slider round"></span>' +
                                '</label> ';
                        }

                        return actions;
                    },
                },
                //{
                //	targets: 4,
                //	width: '205px',

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

function Approval(element, data, status) {
    swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, do it!'
    }).then(function (result) {
        if (result.isConfirmed) {

            $.ajax({
                url: '/Admin/ServiceRatings/Approval',
                type: 'POST',
                data: { 'id': data, 'status': status },
                success: function (response) {
                    if (response.success) {
                        toastr.options = {
                            "positionClass": "toast-bottom-right",
                        };
                        //toastr.success('City Deleted Successfully');
                        const status = $("#ApprovalStatus").val();
                        if (status == "1") {
                            table1.row($(element).closest('tr')).remove().draw();
                        } else if (status == "0") {
                            table1.row($(element).closest('tr')).remove().draw();
                        } else
                        {
                            table1.row($(element).closest('tr')).remove().draw();
                            addRow(response.data);
                            initRating();
                        }

                    } else {
                        //toastr.error(response.message);
                        $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                        $(element).find('i').show();
                    }

                }
            });
        }
    });
}

jQuery(document).ready(function () {
    KTDatatablesBasicScrollable.init();

    initRating();

    $(".seemore").click(function () {
        Swal.fire($(this).text());
    });
});

function initRating() {
    $('.btn-rating').each(function (k, v) {
        var rating = parseFloat($(v).attr('data'));
        $(this).find('i:lt(' + (rating) + ')').addClass("la-star").removeClass("la-star-o");
    });
}

function Statuschange(elem) {
    
    const status = $("#ApprovalStatus").val();
    $.ajax({
        url: '/Admin/ServiceRatings/List',
        type: 'POST',
        data: { ApprovalStatus: status },
        success: function (response) {
            
            if (response.data != null) {
                $("#ServiceRatingList").html(response.data);
                //KTDatatablesBasicScrollable.init();
                initRating();
            } else {
                $("#ServiceRatingList").html(response);
                if (status == "1") {
                    $("#ApprovalStatus option[value='1']").attr("selected", true)
                }
                else if (status == "0") {
                    $("#ApprovalStatus option[value='0']").attr("selected", true)
                }
                else {
                    $("#ApprovalStatus option[value='3']").attr("selected", true)
                }
                KTDatatablesBasicScrollable.init();
                initRating();
            }

            //else {
            //	//toastr.error(response.message);
            //	$(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
            //	$(element).find('i').show();
            //}
        }
    });

};

function callback(dialog, elem, isedit, response) {

    if (response.success) {

        toastr.success(response.message);
        if (isedit) {
            table1.row($(elem).closest('tr')).remove().draw();
        }
        if (response.data.IsApproved == false) {
            addRow(response.data);
        }
        else {
            table1.row($(elem).closest('tr')).remove();
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
        //row.BookingID,
        row.Bookingno,
        row.Service,
        row.Rating,
        row.IsApproved,
        row.IsApproved,
        row.IsApproved + ',' + row.ID,
    ]).draw(true);

}