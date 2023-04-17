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
			"order": [[0, "desc"]],
            //dom: 'Bfrtip',

            "language": {
                processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
            },
            "initComplete": function (settings, json) {
                $('#kt_datatable1 tbody').fadeIn();
            },
            columnDefs: [
                {
                targets: 0,
                width: '130px',
                className: "dt-left",
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
        //$("input[name=fromDate]").val($("#fromDate").val());
    });

    $("#toDate").change(function () {

        if (new Date($("#fromDate").val()) > new Date($("#toDate").val())) {
            $('#fromDate').datepicker('setDate', new Date($("#toDate").val()));
            $("#fromDate").datepicker("option", "maxDate", new Date($("#toDate").val()));

        }
        //$("input[name=ToDate]").val($("#toDate").val());
    });

    //$("#fromDate").trigger('change');
    //$("#toDate").trigger('change');

    //$('.kt_datepicker_range').datepicker({
    //    todayHighlight: true,
    //});

    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();

    $('#from').val(fromDate);
    $('#to').val(toDate);

    $("#btnSearch").on("click", function () {
        $("#btnSearch").addClass('spinner spinner-left').prop('disabled', true);

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
            url: '/Admin/Transaction/List',
            type: 'POST',
            data: {
                fromDate: $('#fromDate').val(),
                toDate: $('#toDate').val(),
            },
            success: function (data) {
                "use strict";

                if (data != null) {

                    $("#transaction").html(data);
                    KTDatatablesBasicScrollable.init();
                    $('#from').val(fromDate);
                    $('#to').val(toDate);

                    var td = data.includes("</td>");
                    if (td) {
                        $('#btnSubmit').prop('disabled', false);
                    }
                }
                else {
                    $('#btnSubmit').prop('disabled', true);
                }
                $("#btnSearch").removeClass('spinner spinner-left').prop('disabled', false);

            }
        });

    });
});



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
        row.ID,
        row.CretedOn,
        row.OrderNo,
        row.NameOnCard,
        row.MaskCard,
        row.TransactionStatus,
        row.Amount,
    ]).draw(true);

}