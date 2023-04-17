"use strict";
var table1;
var VendorServicesSelection = [];
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
            "drawCallback": function (settings) {
                //if ($('#checkAll').prop('checked')) {
                //	$('input[name=chkProduct]').prop('checked', true);
                //}

                if (!VendorServicesSelection) {
                    VendorServicesSelection = [];
                }

                VendorServicesSelection.forEach(function (obj) {
                    $(`input[id=chk${obj}]`).prop('checked', true);
                });

            },
            columnDefs: [
                {
                    targets: 0,
                    width: '140px' ,
                    className: "dt-center",
                },
                {
                    targets: 1,
                    className: "dt-center ",
                    width: '140px',
                    render: function (data, type, full, meta) {

                        if (!data) {
                            return '<span>-</span>';
                        }
                        var vendor = data.split('|');
                        console.log(vendor);
                        return '<div class="d-flex align-items-center mx-5">' +
                            '<div class="symbol symbol-50 flex-shrink-0 mr-4">' +
                            '<div class="symbol-label" style="background-image: url(\'' + vendor[0] + '\')"></div>' +
                            '</div>' +
                            '<div>' +
                            '<span class=" text-dark-75 font-weight-bolder mb-1 font-size-l">' + vendor[1] + '</span><br>' +
                            '<span class=" font-weight-bold d-block text-primary">' + vendor[2] + '</span>' +
                            '</div>' +
                            '</div>';

                    },
                },
                {
                    targets: 2,
                    width: '140px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                        data = data.split(',');
                        let IsChecked = data[1].toLowerCase();
                        let Checked = IsChecked == 'true' ? "checked" : "";

                        return `<label class="checkbox justify-content-center checkbox-lg checkbox-custom checkbox-inline">
										<input type="checkbox" value="${data[0]}" id="chk${data[0]}" name="chkProduct" onchange="VendorServiceSelected(this,${data[0]},${IsChecked})" ${Checked}>
										<span></span>
									</label>`;
                       
                    }
                },
                //{
                //    targets: -1,
                //    title: 'Actions',
                //    orderable: false,
                //    className: "dt-center",
                //    width: '230px',
                //    render: function (data, type, full, meta) {

                //        data = data.split(',');

                //        var actions = '';
                        
                //        actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/Vendor/VendorServices/Details/' + full[4] + '\')" title="View">' +
                //            '<i class="fa fa-folder-open"></i>'+
                //            '</button> ';
                //        return actions;
                       
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
});

function VendorServiceSelected(element, record, isChecked) {
    
    if (!VendorServicesSelection) {
        VendorServicesSelection = [];
    }

    var product = VendorServicesSelection.find(function (obj) { return obj == $(element).val() });

    if ($(element).prop('checked')) {
        if (!product) {
            $.ajax({
                url: '/Vendor/VendorServices/Create/',
                type: 'POST',
                data: {
                    "__RequestVerificationToken":
                        $("input[name=__RequestVerificationToken]").val(),
                    id: record,
                },
                success: function (result) {
                    if (result.success != undefined) {
                        if (result.success) {
                            toastr.options = {
                                "positionClass": "toast-bottom-right",
                            };
                            toastr.success('Service saved successfully');
                            $(element).prop('checked', true);
                        }
                        else {
                            toastr.error(result.message);
                        }
                    } else {
                        swal.fire("Your are not authorize to perform this action", "For further details please contact administrator !", "warning").then(function () {
                        });

                    }
                }
            });
        }
    } else {
        $.ajax({
            url: '/Vendor/VendorServices/Delete/',
            type: 'POST',
            data: {
                "__RequestVerificationToken":
                    $("input[name=__RequestVerificationToken]").val(),
                id: record,
            },
            success: function (result) {
                if (result.success != undefined) {
                    if (result.success) {
                        toastr.options = {
                            "positionClass": "toast-bottom-right",
                        };
                        toastr.success('Service removed successfully');
                        $(element).prop('checked', false);
                    }
                    else {
                        toastr.error(result.message);
                    }
                } else {
                    swal.fire("Your are not authorize to perform this action", "For further details please contact administrator !", "warning").then(function () {
                    });

                }
            }
        });
    }
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
        row.ID + ',' + row.IsChecked,
        row.Date,
        row.Thumbnail + '|' + row.Name + '|' + row.Type,
        row.Category,
        row.ID
    ]).draw(true);

}