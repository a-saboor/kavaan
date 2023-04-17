"use strict";

let defaultFileName = 'No File Chosen';
var imageUrl;
var video1stUrl;
var video2ndUrl;
var video3rdUrl;

jQuery(document).ready(function () {

    //#region validation for enable image
    
    //var videochecbox = $('input[name="VideoFirst.IsEnable"]').prop("checked");
    //var imagecheckbox = $('input[name="Image.IsEnable"]').prop("checked");
    ////#endregion



    //#region Image

    var _URL = window.URL || window.webkitURL;
    function imageremove() {
        $("#image-file").val("");
        $('.image-file-span').text(defaultFileName);
        $('.image-file-remove').hide();
        $('#image-file-label').show();
    }
    $("#image-file").change(function (e) {
        var file, img;

        if ((file = this.files[0])) {
            if (this.files[0].size >= 3000000) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Image size should be equal to 300 KB and dimension should be 1920 x 966!',
                    //footer: '<a href>Image size should be less than or equal to  100KB and dimension should be 1923x764</a>'
                })
                imageremove();
            }
            else {
                $(this).prev('span').text(file.name);
                $('.image-file-remove').show();
                $('#image-file-label').hide();
            }
            img = new Image();
            img.onload = function () {

                if (this.width != 1920) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Image size should be equal to 300 KB and dimension should be 1920 x 966!',
                        //  footer: '<a href>Image dimension should be 1923x764 and size should less than 100 KB</a>'
                    })
                    imageremove();
                }
                else if (this.height != 966) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Image size should be equal to 300 KB and dimension should be 1920 x 966!',
                        // footer: '<a href>Image dimension should be 1923x764 and size should less than 100 KB</a>'
                    })
                    imageremove();
                }
                else {
                    img.onerror = function () {
                        alert("not a valid file: " + file.type);
                    };
                }
            };

            img.src = _URL.createObjectURL(file);
        }
    });

    $('.image-file-remove').click(function () {
        var file = $('#image-file')[0].files[0];
        //byte size
        $("#image-file").val("");
        $('.image-file-span').text(defaultFileName);
        $('.image-file-remove').hide();
        $('#image-file-label').show();
    });

    $("#image-file-delete").click(function () {
        Delete($('#image-id'), $('#image-id').val(),"Image");
    })

    $('#image-file-edit').click(function () {

        var div = $(this).closest('.content-div');
        imageUrl = $('.url-image').val();
        $(div).find('.title-image').prop('disabled', false).focus();
        $(div).find('.titlear-image').prop('disabled', false);
        $(div).find('.description-image').prop('disabled', false);
        $(div).find('.descriptionar-image').prop('disabled', false);
        $(div).find('.checkbox-image').prop('disabled', false);
        $(div).find('.btn-image-url').show();
    });

    $('.btn-image-url-cancel').click(function () {
        var div = $(this).closest('.content-div');
        $(div).find('.title-image').prop('disabled', true);
        $(div).find('.subtitle-image').prop('disabled', true);
        $(div).find('.url-image').prop('disabled', true).val(imageUrl);
        $(div).find('.btn-image-url').hide();
    });

    $('.btn-image-url-save').click(function () {
        Update($('#image-id'), $('.url-image').val(), $('.title-image').val(), $('.titlear-image').val(), $('.description-image').val(), $('.descriptionar-image').val(), $('.checkbox-image').prop("checked"), $('#image-type').val());
    });

    $('#BtnImage').click(function () {

        var videochecbox = $('.isenablevideo').prop("checked");
        var imagecheckbox = $('.isenableimage').prop("checked");


        var files = $("#image-file").get(0).files;
        if (files.length > 0) {
            if (videochecbox != true && imagecheckbox != true) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Please enable at least one media file.',
                })
            }
            else {
                $("#FormImage").submit();
            }

        }
        else {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'You have not specified a file.',
            })
        }
    });
    //#endregion





    //#region First Video
    $('#video-file-first').change(function () {
        var i = $(this).prev('span').clone();
        try {
            var file = $('#video-file-first')[0].files[0];
            //byte size
            if (file.size > 50485760) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Video size should be less than 50 MB!',
                })
                $("#video-file-first").val("");
                $(this).prev('span').text(defaultFileName);
            }
            else {
                $(this).prev('span').text(file.name);
                $('.video-file-first-remove').show();
                $('#video-file-first-label').hide();
            }

        } catch (e) {
            $(this).prev('span').text(defaultFileName);
        }
    });

    //remove video in file input

    $('.video-file-first-remove').click(function () {
        var file = $('#video-file-first')[0].files[0];
        //byte size
        $("#video-file-first").val("");
        $('.video-file-first-span').text(defaultFileName);
        $('.video-file-first-remove').hide();
        $('#video-file-first-label').show();
    });

    //deleting the video
    $("#video-file-first-delete").click(function () {
        Delete($('#video-first-id'), $('#video-first-id').val(),"Video");
    });

    //edit video frame enable
    $('.video-frame-first span:eq(1)').click(function () {
        
        var div = $(this).closest('.content-div');
        video1stUrl = $(div).find('input:eq(1)').val();
        ////tt
        //$(div).find('input:eq(1)').prop('disabled', false).focus();
        ////tt ar
        //$(div).find('input:eq(2)').prop('disabled', false);
        ////desc
        //$(div).find('input:eq(3)').prop('disabled', false);
        ////descar
        //$(div).find('input:eq(4)').prop('disabled', false);
        //enable
        $(div).find('.checkbox-video').prop('disabled', false);

        $(div).find('.btn-video-url').show();
    });

    //$('.video-frame-first button:eq(0)').click(function () {
    //    
    //    var div = $(this).closest('.content-div');
    //    $(div).find('input:eq(1)').prop('disabled', true).val(video1stUrl);
    //    $(div).find('div:eq(4)').hide();
    //});

    $('.btn-video-url-save').click(function () {
       
        Update($('.video-frame-first input:eq(0)'), $('.video-frame-first input:eq(3)').val(), $('.video-frame-first input:eq(1)').val(), $('.video-frame-first input:eq(2)').val(), $('.video-frame-first input:eq(3)').val(), $('.video-frame-first input:eq(4)').val(), $('.checkbox-video').prop("checked"), $('#video-first-type').val());
    });

    $('#BtnVideoFirst').click(function () {

        var videochecbox = $('.isenablevideo').prop("checked");
        var imagecheckbox = $('.isenableimage').prop("checked");

        var files = $("#video-file-first").get(0).files;
        if (files.length > 0) {
            if (videochecbox != true && imagecheckbox != true) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Please enable at least one media file.',
                })
            } else {
                $("#FormVideoFirst").submit();
            }

        }
        else {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'You have not specified a file.',
            })
        }
    });
    //#endregion




});
//#region swal fire on delete
function swalfireonemptymedia() {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Media can not be deleted insert or enable at least one media!',
    })
   
}
//#endregion 

function Delete(element, record, type) {
    
    var videopath = $('video').find('Source:first').attr('src');
    var imagepath = $('#imagepath').attr('attr-data');

    var videochecbox = $('.isenablevideo').prop("checked");
    var imagecheckbox = $('.isenableimage').prop("checked");

    //on video delete if image is null or not enabled then 
    //(Media can not be deleted insert or enable at least one media!)
    if (type === "Image")
    {
        if (!videopath || !videochecbox) {
            swalfireonemptymedia();
            return false;
        }
    }
  
    if (type === "Video")
    {
        if (!imagepath || !imagecheckbox) {
            swalfireonemptymedia();
            return false;
        }
    }
  
    swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!'
    }).then(function (result) {
        if (result.value) {

            $.ajax({
                url: '/Admin/ContentManagement/Delete/' + record,
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

                            var div = $(element).closest('.card-body');
                            $(div).find('form').show()
                            $(div).find('.content-div').remove();

                            $(div).closest('.card').find('.card-title').text(`Upload ${$(div).closest('.card').find('.card-title').text()}`);

                            toastr.success('File Deleted Successfully');
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

function Update(element, url, title, titlear, description, descriptionar, IsEnable, Type) {
    
    var div = $(element).closest('.content-div');
    //valdation for enable
    var videochecbox = $('.isenablevideo').prop("checked");
    var imagecheckbox = $('.isenableimage').prop("checked");
    if (videochecbox != true && imagecheckbox != true) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please enable at least one media file.',
        })
    }
    else {
        if (element.val()) {
            $(div).find('button:eq(1)').prop('disabled', true).html('<i class="fa fa-sm fa-save text-white"></i> loading...');

            $.ajax({
                url: '/Admin/ContentManagement/Update',
                type: 'POST',
                data: {
                    "__RequestVerificationToken":
                        $("input[name=__RequestVerificationToken]").val(),
                    id: element.val(),
                    Title: title,
                    TitleAr: titlear,
                    description: description,
                    descriptionar: descriptionar,
                    type: Type,
                    isenable: IsEnable

                },
                success: function (result) {
                    if (result.success != undefined) {
                        if (result.success) {
                            toastr.options = {
                                "positionClass": "toast-bottom-right",
                            };
                            toastr.success(result.message);

                            ////tt
                            //$(div).find('input:eq(1)').prop('disabled', true).focus();
                            ////tt ar
                            //$(div).find('input:eq(2)').prop('disabled', true);
                            ////desc
                            //$(div).find('input:eq(3)').prop('disabled', true);
                            ////descar
                            //$(div).find('input:eq(4)').prop('disabled', true);
                            ////enable
                            $(div).find('input:eq(2)').prop('disabled', true);

                            $(div).find('div:eq(3)').hide();
                            location.reload();
                        }
                        else {
                            toastr.error(result.message);
                        }
                    } else {
                        swal.fire("Your are not authorize to perform this action", "For further details please contact administrator !", "warning").then(function () {
                        });
                    }

                    $(div).find('button:eq(1)').prop('disabled', false).html('<i class="fa fa-sm fa-save text-white"></i> Save');

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
                    $(div).find('button:eq(1)').prop('disabled', false).html('<i class="fa fa-sm fa-save text-white"></i> Save');
                }
            });
        }
    }

}

