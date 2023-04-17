"use strict";

var model = {
    BusinessSettingID: null,
    ID: null,
    BusinessSettingID: null,
    Image: null,
    Name: null,
    NameAr: null,
    Contact: null,
    Contact2: null,
    Fax: null,
    Email: null,
    MapIframe: null,
    StreetAddress: null,
    StreetAddressAr: null,
    Days: null,
};

jQuery(document).ready(function () {
    //KTFormRepeater.init();
    var _URL = window.URL || window.webkitURL;




    //var row = $(element).closest(".list-repeat");
    $(".Image").change(function (e) {
        var file, img, elem;
        elem = this;


        if ((file = this.files[0])) {
            if (this.files[0].size >= 500000) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Image size should be less than 500 KB  & dimension should be 1098 x 300 !',
                    //footer: '<a href>Image size should be less than 100 KB  & dimension should be (1713x540) </a>'
                })
                $(".Image").val("");
            }
            img = new Image();
            img.onload = function () {
                if (this.width > 400 || this.width < 400) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Image size should be less than 500 KB  & dimension should be 600 x 400 !',
                        //footer: '<a href>Image size should be less than 100 KB  & dimension should be (1713x540) </a>'
                    })
                    $(".Image").val("");
                }
                else if (this.height > 600 || this.height < 600) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Image size should be less than 500 KB  & dimension should be 600 x 400 !',
                        //footer: '<a href>Image size should be less than 100 KB  & dimension should be (1713x540) </a>'
                    })
                    $(".Image").val("");
                }
                else {
                    img.onerror = function () {
                        alert("not a valid file: " + file.type);
                    };

                    $(elem).closest('.list-repeat').find('.img-div').hide();
                };
            };
            img.src = _URL.createObjectURL(file);

        }

    });


});



// Class definition
var KTFormRepeater = function () {

    // Private functions
    var addList = function () {
        $('#kt_repeater_1').repeater({
            initEmpty: false,
            defaultValues: {
                'text-input': 'foo'
            },
            show: function () {


                //$(this).find('input').val('');
                //$(this).find('textarea').empty();


                $(this).find(".BranchName").text('Branch Name');
                $(this).find(".ID").attr("value", "").val('');
                //$(this).find(".BusinessSettingID").attr("value", "").val('');
                $(this).find(".show-image").attr("src", "");
                $(this).find(".Image").attr("value", "").val('');
                $(this).find(".Name").attr("value", "").val('');
                $(this).find(".NameAr").attr("value", "").val('');
                $(this).find(".Contact").attr("value", "").val('');
                $(this).find(".Contact2").attr("value", "").val('');
                $(this).find(".Fax").attr("value", "").val('');
                $(this).find(".Email").attr("value", "").val('');
                $(this).find(".MapIframe").val('');
                $(this).find(".StreetAddress").val('');
                $(this).find(".StreetAddressAr").val('');
                $(this).find(".Days").val('');
                $(this).find(".Delete").attr("disabled", false).attr("onclick", "Delete(this, 0)");
                $(this).find(".edit-profile").attr("disabled", false).attr("onclick", "Change(this, 0)");
                $(this).find(".edit-cancel").attr("disabled", false).attr("onclick", "Cancel(this, 0)");
                $(this).find(".save-changes").attr("disabled", false).attr("onclick", "Save(this, 0)");

                $(this).slideDown();
            },
        });
    }

    return {
        // public functions
        init: function () {
            addList();
        }
    };
}();




function Save(element) {
    var row = $(element).closest(".list-repeat");

    if (row.find(".Name").val() == "") {
        Swal.fire(
            'Oops!',
            'Branch Name required ...',
            'error'
        )
    }

    else {


        var data = new FormData();
        row.find('.edit-cancel').prop('disabled', true);
        row.find('.save-changes').prop('disabled', true);
        row.find('.edit-profile').prop('disabled', true);
        row.find('.Delete').prop('disabled', true);

        data.append("ID", Number(row.find(".ID").val()));

        if (row.find(".Image").length) {

            var files = row.find(".Image").get(0).files;
            if (files.length > 0) {
                data.append("Image", files[0]);
            }

        }

        data.append("BusinessSettingID", row.find(".BusinessSettingID").val());
        data.append("Name", row.find(".Name").val());
        data.append("NameAr", row.find(".NameAr").val());
        data.append("Contact", row.find(".Contact").val());
        data.append("Contact2", row.find(".Contact2").val());
        data.append("Fax", row.find(".Fax").val());
        data.append("Email", row.find(".Email").val());
        data.append("MapIframe", row.find(".MapIframe").val());
        data.append("StreetAddress", row.find(".StreetAddress").val());
        data.append("StreetAddressAr", row.find(".StreetAddressAr").val());
        data.append("Days", row.find(".Days").val());

        //// console.log(model);

        $.ajax({
            url: "/Admin/BusinessSetting/BranchSettings/",
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                if (response.success) {

                    row.find(".BranchName").text(response.data.Name);
                    row.find(".ID").val(response.data.ID);
                    row.find(".BusinessSettingID").val(response.data.BusinessSettingID);
                    row.find(".Delete").attr("onclick", "Delete(this," + response.data.ID + ")");
                    row.find(".edit-profile").attr("onclick", "Change(this," + response.data.ID + ")");
                    row.find(".edit-cancel").attr("onclick", "Cancel(this," + response.data.ID + ")");
                    row.find(".save-changes").attr("onclick", "Save(this," + response.data.ID + ")");
                    toastr.success(response.message);

                }
                else {
                    toastr.error(response.message);
                    // console.log(response.error);
                }

                SaveAfter(row);
            },
            error: function (er) {
                toastr.error(er);
            }
        });
        return false;
    }
}

function Delete(element, record) {

    var row = $(element).closest(".list-repeat");

    if (record == 0) {
        setTimeout(function () {
            row.remove();
        }, 2000);
        row.slideUp();
    }
    else {
        swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!'
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    url: '/Admin/BusinessSetting/BranchSettingDelete/' + record,
                    type: 'POST',
                    data: {
                        "__RequestVerificationToken":
                            $("input[name=__RequestVerificationToken]").val()
                    },
                    success: function (response) {
                        if (response.success != undefined) {
                            if (response.success) {
                                row.slideUp();
                                setTimeout(function () { row.remove(); }, 250);
                                row.find('input').empty();
                                row.find('textarea').empty();
                                toastr.success(response.message);
                            }
                            else {
                                toastr.error(response.message);
                                // console.log(response.error);
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
}

function Change(element, record) {
    var row = $(element).closest(".list-repeat");
    //var allInputElements = $("input");
    //row.find(allInputElements);
    row.find('input').prop('disabled', false);
    row.find('textarea').prop('disabled', false);
    row.find('.edit-cancel').fadeIn();
    row.find('.save-changes').fadeIn();
    row.find('.edit-profile').hide();
}

function Cancel(element, record) {
    var row = $(element).closest(".list-repeat");
    row.find('input').prop('disabled', true);
    row.find('textarea').prop('disabled', true);
    row.find('.edit-cancel').hide();
    row.find('.save-changes').hide();
    row.find('.edit-profile').fadeIn();
}

function SaveAfter(row) {
    row.find('.edit-cancel').prop('disabled', false);
    row.find('.save-changes').prop('disabled', false);
    row.find('.edit-profile').prop('disabled', false);
    row.find('.Delete').prop('disabled', false);

    row.find('input').prop('disabled', true);
    row.find('textarea').prop('disabled', true);

    row.find('.edit-cancel').hide();
    row.find('.save-changes').hide();
    row.find('.edit-profile').fadeIn();

}