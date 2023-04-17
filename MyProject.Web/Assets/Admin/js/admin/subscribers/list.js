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
            "language": {
                processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
            },
            "initComplete": function (settings, json) {
                $('#kt_datatable1 tbody').fadeIn();
            },
            dom: 'Bfrtip',
            buttons: [
                //{
                //    extend: 'print',
                //    messageTop: function () {
                //        //if ( printCounter === 1 ) {
                //        return html;
                //        //}
                //        //else {
                //        //    return 'You have printed this document '+printCounter+' times';
                //        //}
                //    },
                //    title: '',
                //    exportOptions: {
                //        columns: [0, 1]
                //    }
                //},
                {
                    extend: 'excel',
                    messageTop: function () {
                        return 'Subscribers';
                    },
                    title: '',
                    exportOptions: {
                        columns: [0, 1]
                    }
                }
            ],

            columnDefs: [{
                targets: 0,
                width: '250px',
            }
            ],
            
           // buttons: [
           //        {
           //            extend: 'print',
           //            messageTop: function () {
           //                //if ( printCounter === 1 ) {
           //                return html;
           //                //}
           //                //else {
           //                //    return 'You have printed this document '+printCounter+' times';
           //                //}
           //            },
           //            title: '',
           //            exportOptions: {
           //                columns: [0, 1]
           //            }
           //        },
           //{
           //    extend: 'excel',
           //    messageTop: function () {
           //        return 'Subscribers';
           //    },
           //    title: '',
           //    exportOptions: {
           //        columns: [0, 1]
           //    }
           //}
           // ],
            
            
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
        //var html = "<h3 style=margin-top:208px; margin-bottom:76px; align=center >Filtered Subscribers</h3><br/>";

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
            url: '/Admin/Subscribers/List',
            type: 'POST',
            data: {
                fromDate: $('#fromDate').val(),
                toDate: $('#toDate').val(),
            },
            success: function (data) {
                "use strict";

                if (data != null) {
                    $("#subscibers").html(data);
                    KTDatatablesBasicScrollable.init();
                    $('#from').val(fromDate);
                    $('#to').val(toDate);

                    //if ($(data).find('tbody tr').length) {
                    //    $('#btnSubmit').prop('disabled', false);
                    //}
                    //else {
                    //    $('#btnSubmit').prop('disabled', true);
                    //}
                }
                $("#btnSearch").removeClass('spinner spinner-left').prop('disabled', false);
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
//                url: '/Admin/Brands/Activate/' + record,
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

        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

        //if (isedit) {
        //    table1.row($(elem).closest('tr')).remove().draw();
        //}

        //addRow(response.data);
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
		row.Date,
		row.Email,
		//row.IsActive + ',' + row.ID,
    ]).draw(true);

}