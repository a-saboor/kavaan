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
            columnDefs: [
                {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    render: function (data, type, full, meta) {

                        return '<button type="button" disabled class="btn btn-secondary  btn-sm mr-1 btnSave"   onclick="EditStock(this, ' + data + ')" >' +
                            '<i class="far fa-save"></i> Save' +
                            '</button> ';
                    },
                },
                {
                    targets: 1,
                    width: '15%',
                }
                //{
                //	
                //	render: function (data, type, full, meta) {
                //		data = data.toUpperCase();
                //		var status = {
                //			"TRUE": {
                //				'title': 'Active',
                //				'class': ' label-light-success'
                //			},
                //			"FALSE": {
                //				'title': 'InActive',
                //				'class': ' label-light-danger'
                //			},
                //		};
                //		if (typeof status[data] === 'undefined') {
                //			return '<span class="label label-lg font-weight-bold label-light-danger label-inline">Inactive</span>';
                //		}
                //		return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
                //	},
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

$("input").change(function () {
    $("#btnUpdate").prop('disabled', false);
    $($(this).closest('tr').addClass("updated"));
    $($(this).closest('tr').children()[5]).children().prop('disabled', false);
    var saleprice = parseFloat($($($($(this).closest('tr').children()[3])[0]).children()).val());
    var regularprice = parseFloat($($($($(this).closest('tr').children()[2])[0]).children()).val());
    if (saleprice > regularprice) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Sale Price is greater than Regular Price!',
        });
        $($($($(this).closest('tr').children()[3])[0]).children()).val(regularprice);
    }

});

$("#btnUpdate").click(function () {
    
    let record;
    const stock = [];
    $(".btnSave:enabled").each(function (i, j) {

        record = $(j).closest('tr').attr('id');
        var data = {
            ID: record,
            CreatedOn: $($("#" + record).children()[0]).text(),
            Name: $($("#" + record).children()[1]).text(),
            RegularPrice: $($($("#" + record).children()[2]).children()).val(),
            SalePrice: $($($("#" + record).children()[3]).children()).val(),
            Stock: $($($("#" + record).children()[4]).children()).val()
        }
        if (!priceValidation(data.RegularPrice, data.SalePrice, data.Stock)) {
            toastr.error("Please add value more than or equal to zero stocks not added ...");
            stock = [];
            return false;
        } else {
            stock.push(data)
        }

    });

    $("#btnUpdate").addClass("spinner spinner-left spinner-sm").prop('disabled', true);


    $.ajax({
        type: "POST",
        url: '/Vendor/StockEdit/EditStockBulk',
        async: true,
        data: { stockViewModels: stock },
        success: function (data) {
            console.log(data);
            if (data.success) {

                toastr.success("All stocks updated successfully ...");
                setTimeout(function () { location.reload(); }, 2000);
                $("#btnUpdate").removeClass("spinner spinner-left spinner-sm").prop('disabled', false);
            } else {
                toastr.error(data.message);
                $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                $(element).find('i').show();
                $("#btnUpdate").removeClass("spinner spinner-left spinner-sm").prop('disabled', false);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            if (xhr.status === 403) {
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

                $("#btnUpdate").removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                $("#btnUpdate").find('i').show();
            }
        }
    });


})


function EditStock(element, record) {

    var data = {
        ID: record,
        CreatedOn: $($("#" + record).children()[0]).text(),
        Name: $($("#" + record).children()[1]).text(),
        RegularPrice: $($($("#" + record).children()[2]).children()).val(),
        SalePrice: $($($("#" + record).children()[3]).children()).val(),
        Stock: $($($("#" + record).children()[4]).children()).val()
    }
    if (!priceValidation(data.RegularPrice, data.SalePrice, data.Stock)) {
        toastr.error("Please add value more than or equal to zero ...");
        return false;
    }
    $.ajax({
        type: "POST",
        url: '/Vendor/StockEdit/EditStock',
        async: true,
        data: data,
        success: function (data) {
            console.log(data);
            if (data.success) {

                toastr.success(data.message);
                $($("#" + data.data.ID).closest('tr').children()[5]).children().prop('disabled', true);
                /*$("#btnUpdate").prop('disabled', true);*/
                //	table1.row($(element).closest('tr')).remove().draw();
                //addRow(data.data);
            } else {
                toastr.error(data.message);
                $(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
                $(element).find('i').show();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            if (xhr.status === 403) {
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
        row.Name + '|' + row.SKU,
        row.RegularPrice.toFixed(2),
        row.SalePrice.toFixed(2),
        row.Stock,
        row.ID,
    ]).draw(true);

}

function priceValidation(regularprice, saleprice, stock) {
    if (regularprice < 0 || saleprice < 0 || stock < 0) {
        return false;
    }
    return true;
}