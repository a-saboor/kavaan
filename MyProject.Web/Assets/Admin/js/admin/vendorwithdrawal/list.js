"use strict";
var table1;
var KTDatatablesBasicScrollable = function () {

    var initTable1 = function () {
        var table = $('#kt_datatable123');

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
                $('#kt_datatable123 tbody').fadeIn();
            },
            columnDefs: [
                 {
                     targets: 0,
                     className: "dt-center",
                     width: '130px',
                 },
                {
                    targets: 1,
                    //width: '75px',
                    render: function (data, type, full, meta) {

                        if (!data) {
                            return '<span>-</span>';
                        }
                        var vendor = data.split(',');
                        return `<div class="d-flex align-items-center">
                                    <div class="symbol symbol-50 flex-shrink-0 mr-4">
                                        <div class ="symbol-label" style="background-image: url('${vendor[0]}')"></div>
                                    </div>
                                    <div>
                                        <a href="javascript:;" class="text-dark-75 font-weight-bolder text-hover-primary mb-1 font-size-lg">${vendor[1]}</a>
                                        <span class="text-muted font-weight-bold d-block">${vendor[2]}</span>
                                    </div>
                                </div>`;
                    },
                },
                {
                    targets: 2,
                    className: "dt-center",
                    width: '75px',
                },
                {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    className: "dt-center",
                    width: '230px',
                    render: function (data, type, full, meta) {
                        var actions = '';

                        actions += `<button type="button" id="appreq" class="btn btn-outline-success btn-sm mr-1 btnapprove" onclick="pendingrequestapproval(this,${data},'Processed')">
                                        <i class ="fa fa-check-circle"></i> Approve Transfer
                                    </button>
                                    <button type="button" id="appreqfalse" class ="btn btn-outline-danger btn-sm mr-1 btnapprove" onclick="pendingrequestapproval(this,${data},'Declined')">
                                        <i class ="fa fa-times-circle"></i> Decline
                                    </button>`;


                        return actions;
                    }
                }]
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

function pendingrequestapproval(elem, id, approvalstatus) {
    swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, do it!'
    }).then(function (result) {
        if (result.value) {

            $(elem).find('i').hide();
            $(elem).addClass('spinner spinner-left spinner-sm').attr('disabled', true);

            var table = $('#kt_datatable123').DataTable();
            $.ajax({
                url: '/Admin/VendorWalletShareHistory/AcceptRequest',
                type: 'Post',
                data: {
                    ID: id,
                    Status: approvalstatus,
                    __RequestVerificationToken: $('input[name=__RequestVerificationToken]').val()
                },
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);

                        $(elem).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                        $(elem).find('i').show();

                        table1.row($(elem).closest('tr')).remove().draw();

                        $('select[name=VendorID]').trigger('change');

                        $('#btnSearch').trigger('click');

                    } else {
                        $(elem).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                        $(elem).find('i').show();

                        toastr.error(response.message);
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
                        $(elem).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                        $(elem).find('i').show();
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
        row.Vendor,
        row.Amount,
        row.Status,
        row.ID,
    ]).draw(true);

}