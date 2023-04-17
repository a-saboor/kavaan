"use strict";

//var _URL = window.URL || window.webkitURL;

jQuery(document).ready(function () {

	
});

function videoValidation(elem) {
	
	let file;
	let size;
	let videoWidth;
	let videoHeight;
	let ratio;
	let originalWidth;
	let originalHeight;
	file = elem.files[0];
	size = elem.files[0].size;
	var media = URL.createObjectURL(elem.files[0]);
	var video = document.getElementById("video");
	video.addEventListener("loadedmetadata", function (e) {

		var Type = $('#Type').val();
		let dimension_message = "";
		if (Type == "Header") {
			originalWidth = 1080;
			originalHeight = 1920;
			dimension_message = "Video ratio should be 9:16 !"
		}
		else if (Type == "Footer") {
			originalWidth = 1920;
			originalHeight = 1080;
			dimension_message = "Video ratio should be 16:9 !"
		}
		if (size >= 30000000) {
			dimension_message = "video size less then 30 mb !"
		}

		

		videoWidth = this.videoWidth;
		videoHeight = this.videoHeight;

		
		ratio = ((originalHeight / originalWidth) * videoWidth);
		ratio = Math.floor(ratio);

		if (ratio != videoHeight || size >= 30000000) {
			Swal.fire({
				icon: 'error',
				title: 'Oops...',
				text: dimension_message,
			}).then(function (result) {
				$(video).attr("src", "");
				$(elem).val("");
				file = "";
				video.style.display = "none";
				$(btnCancel).trigger('click');
			});
		}
		else {
			video.onerror = function () {
				$(elem).val("");
				file = "";
			};
		}

	}, false);


	video.src = media;
};

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
				url: '/Admin/Banner/Delete/' + record,
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
							toastr.success('Banner deleted successfully ...');
							$(element).closest('.banner').remove();
							//location.reload(true);
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