"use strict";

var carAttributes = [];

var CarAttributeSetting = [];
var t;

var carVariationAttributes = [];



window.addEventListener('load', function () {

	$('#CarType').trigger('change');
})

jQuery(document).ready(function () {

	toastr.options = {
		"positionClass": "toast-bottom-left",
	};
	$("#ratepercentage").hide();
	$("#usefullife").hide();
	$("#residualvalue").hide();

	//$("#datepicker").datepicker({

	//	changeYear: true,
	//	changeMonth: true,
	//	minDate: new Date(),
	//	dateFormat: "yy-m-dd",
	//	yearRange: "-100:+20",

	//});


	$("#Name").on('change', function () {
		var name = $(this);
		$("#Slug").val($(name).val().replace(/ /g, "-").toLocaleLowerCase());
	});
	BindAssetImages();

	$('#kt_image_car_image_2 #Image').change(function () {
		var data = new FormData();
		var files = $("#kt_image_car_image_2 #Image").get(0).files;
		var elem = $(this)
		if (files.length > 0) {
			$(elem).closest('.image-input').find('label[data-action="change"] i').hide();
			$(elem).closest('.image-input').find('label[data-action="change"]').addClass('spinner spinner-dark spinner-center spinner-sm').prop('disabled', true);
			data.append("Image", files[0]);
			$.ajax({
				url: "/Vendor/Asset/Thumbnail/" + GetURLParameter(),
				type: "POST",
				processData: false,
				contentType: false,
				data: data,
				success: function (response) {
					if (response.success) {
						$('#kt_image_car_image_2 .image-input-wrapper').css('background-image', 'url(' + response.data + ')');
						toastr.success(response.message);
					} else {
						toastr.error(response.message);
					}
					$(elem).closest('.image-input').find('label[data-action="change"] i').show();
					$(elem).closest('.image-input').find('label[data-action="change"]').removeClass('spinner spinner-dark spinner-center spinner-sm').prop('disabled', false);
				},
				error: function (e) {
					$(elem).closest('.image-input').find('label[data-action="change"] i').show();
					$(elem).closest('.image-input').find('label[data-action="change"]').removeClass('spinner spinner-dark spinner-center spinner-sm').prop('disabled', false);
					toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
				},
				failure: function (e) {
					$(elem).closest('.image-input').find('label[data-action="change"] i').show();
					$(elem).closest('.image-input').find('label[data-action="change"]').removeClass('spinner spinner-dark spinner-center spinner-sm').prop('disabled', false);
					toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
				}
			});
		}
	});

	$('#btn-gallery-image-upload').click(function () {
		$('input[name=GalleryImages]').trigger('click');
	});

	$('input[name=GalleryImages]').change(function () {

		var data = new FormData();
		var files = $("input[name=GalleryImages]").get(0).files;
		if (files.length > 0) {
			$('#btn-gallery-image-upload').addClass('spinner spinner-light spinner-left').prop('disabled', true);
			$('#btn-gallery-image-upload').find('span').hide();
			$.each(files, function (j, file) {
				data.append('Image[' + j + ']', file);
			})
			//data.append("Image", files);
			data.append("count", $('.car-gallery-images .symbol').length);

			$.ajax({
				url: "/Vendor/AssetImage/Create/" + GetURLParameter(),
				type: "POST",
				processData: false,
				contentType: false,
				data: data,
				success: function (response) {
					if (response.success) {
						$(response.data).each(function (k, v) {
							$('.car-gallery-images').append('<div class="symbol symbol-70 flex-shrink-0 mr-5 mb-3">' +
								'<span class="btn btn-xs btn-icon btn-circle btn-danger btn-hover-text-primary btn-shadow btn-remove-gallery-image" data-action="cancel" data-toggle="tooltip" title="remove" onclick="DeleteGalleryImage(this,' + v.Key + ')">' +
								'<i class="icon-xs ki ki-bold-close ki-bold-trash"></i>' +
								'</span>' +
								'<div class="symbol-label" style="background-image: url(\'' + v.Value + '\')"></div>' +
								'</div>');
						});

						$('#btn-gallery-image-upload').removeClass('spinner spinner-light spinner-left').prop('disabled', false);
						$('#btn-gallery-image-upload').find('span').show();
						toastr.success("Gallery images uploaded ...");
					} else {
						$('#btn-gallery-image-upload').removeClass('spinner spinner-light spinner-left').prop('disabled', false);
						$('#btn-gallery-image-upload').find('span').show();
						toastr.error(response.message);
					}
				},
				error: function (e) {
					$('#btn-gallery-image-upload').removeClass('spinner spinner-light spinner-left').prop('disabled', false);
					$('#btn-gallery-image-upload').find('span').show();
					toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
				},
				failure: function (e) {
					$('#btn-gallery-image-upload').removeClass('spinner spinner-light spinner-left').prop('disabled', false);
					$('#btn-gallery-image-upload').find('span').show();
					toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
				}
			});
		}
	});


	$('#btnFormSubmit').submit(function () {
		var e = $("#btnSaveFacility");
		$(e).addClass('spinner spinner-light spinner-left').prop('disabled', true);
		$(e).find('i').hide();
		
		$.ajax({

			url: '/Vendor/Asset/Update/',
			type: 'POST',
			data: {
				"__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
				asset: {

					ID: GetURLParameter(),
					Name: $('input[name=Name]').val(),
					SerialNumber: $('input[name=SerialNumber]').val(),
					AssetsParentCategoryID: $("#AssetsParentCategoryID").val(),
					AssetsCategoryID: $("#AssetsCategoryID").val(),
					AssetsProductID: $("#AssetsProductID").val(),
					LocationID: $("#LocationID").val(),
					DepartmentID: $("#DepartmentID").val(),
					StaffID: $("#StaffID").val(),
					AssetsContractorID: $('#AssetContractorID').val(),
					Price: $('input[name=Price]').val(),
					PurchaseDate: $('input[name=PurchaseDate]').val(),
					WarrantyExpiryDate: $('input[name=WarrantyExpiryDate]').val(),
					PurchaseType: $('select[name=PurchaseType]').val(),
					DepreciationType: $('select[name=DepreciationType]').val(),
					Position: $('input[name=Position]').val(),
					RatePercentage: $('input[name=RatePercentage]').val(),
					ResidualValue: $('input[name=ResidualValue]').val(),
					UsefulLife: $('input[name=UsefulLife]').val(),
				}
			},
			success: function (response) {

				if (response.success) {
					toastr.success(response.message);
				}
				else {
					toastr.error(response.message);
				}
				$(e).find('i').show();
				$(e).removeClass('spinner spinner-light spinner-left').prop('disabled', false);
			},
			error: function (e) {
				$(e).find('i').show();
				$(e).removeClass('spinner spinner-light spinner-left').prop('disabled', false);
				toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
			},
			failure: function (e) {
				$(e).find('i').show();
				$(e).removeClass('spinner spinner-light spinner-left').prop('disabled', false);
				toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
			}
		});

		return false;
	});
});



$("#DepreciationType").change(function () {

	if ($("#DepreciationType").val() == "Declining Balance") {
		$("#residualvalue").show();
		$("#usefullife").show();
		$("#ratepercentage").show();
		
	}
	if ($("#DepreciationType").val() == "Double Declining Balance") {
		$("#residualvalue").show();
		$("#usefullife").show();
		$("#ratepercentage").show();
	}
	if ($("#DepreciationType").val() == "Straight Line") {
		$("#residualvalue").show();
		$("#usefullife").show();
		$("#ratepercentage").hide();
	}
	if ($("#DepreciationType").val() == "Sum of the Years Digit") {
		$("#residualvalue").show();
		$("#usefullife").show();
		$("#ratepercentage").hide();
	}


})
function DeleteGalleryImage(elem, record) {
	$(elem).addClass('spinner spinner-light spinner-right').prop('disabled', true);
	$(elem).find('i').hide();
	$.ajax({
		url: '/Vendor/AssetImage/Delete/' + record,
		type: 'POST',
		data: {
			"__RequestVerificationToken":
				$("input[name=__RequestVerificationToken]").val()
		},
		success: function (response) {
			if (response.success) {
				$(elem).closest('.symbol').remove();
				toastr.success(response.message);
			}
			else {
				return false;
				$(elem).removeClass('spinner spinner-light spinner-right').prop('disabled', false);
				$(elem).find('i').show();
				toastr.error(response.message);

			}
		},
		error: function (e) {
			$(elem).removeClass('spinner spinner-light spinner-right').prop('disabled', false);
			$(elem).find('i').show();
			toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
		},
		failure: function (e) {
			$(elem).removeClass('spinner spinner-light spinner-right').prop('disabled', false);
			$(elem).find('i').show();
			toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
		}
	});
}

function BindAssetImages() {
	$.ajax({
		url: '/Vendor/AssetImage/GetAssetImages/' + GetURLParameter(),
		type: 'GET',
		success: function (response) {
			if (response.success) {
				$('.car-gallery-images').html('');
				$(response.carImages).each(function (k, v) {
					$('.car-gallery-images').append('<div class="symbol symbol-70 flex-shrink-0 mr-5 mb-3">' +
						'<span class="btn btn-xs btn-icon btn-circle btn-danger btn-hover-text-primary btn-shadow btn-remove-gallery-image" data-action="cancel" data-toggle="tooltip" title="remove" onclick="DeleteGalleryImage(this,' + v.id + ')">' +
						'<i class="icon-xs ki ki-bold-close ki-bold-trash"></i>' +
						'</span>' +
						'<div class="symbol-label" style="background-image: url(\'' + v.Image + '\')"></div>' +
						'</div>');
				});
			} else {
			}
		}
	});
}

$("#TeamRegistration").change(function () {
	
	var type = $('#TeamRegistration').val();
	if (type == 'Individual') {
		$('#PlayersPerTeam').val('');
		$('#PlayersPerTeam').prop('disabled', true)
	}
	else if (type == 'Team') {
		$('#PlayersPerTeam').prop('disabled', false)
	}
	else {
		$('#PlayersPerTeam').prop('disabled', true)
	}

});



$("#FacilityID").change(function () {

	var FacilityID = $('#FacilityID').val();
	if (FacilityID == null) {
		$('textarea[name=Address]').val("");
		$("#Longitude").val("");
		$("#Latitude").val("");
	}
	getFacilityAddress(FacilityID);
});


function getFacilityAddress(FacilityID) {
	if (FacilityID == "") { FacilityID = -1; }
	var json = { "id": FacilityID };
	if (FacilityID == -1) {


		$("#Longitude").prop('disabled', false);
		$("#Latitude").prop('disabled', false);
		$("#FacilityName").prop('disabled', false);
		$('textarea[name=Address]').prop('disabled', false);
		$("#btnMapAddress").show();
	}
	else {
		$.ajax({
			type: "GET",
			contentType: "application/json; charset=utf-8",
			url: '/Vendor/Tournament/GetAddressByFacility/' + FacilityID,
			async: true,
			success: function (data) {

				if (data.data != 0) {
					/* GetDropdownValues(data);*/
					$("#Longitude").val(data.data.Longitude);
					$("#Latitude").val(data.data.Latitude);
					$('textarea[name=Address]').val(data.data.Address);
					$("#Longitude").prop('disabled', true);
					$("#Latitude").prop('disabled', true);
					$("#FacilityName").prop('disabled', true);
					$('textarea[name=Address]').prop('disabled', true);

					$("#btnMapAddress").hide();
				}
				else {
					toastr.options = {
						"positionClass": "toast-bottom-right",
					};

					toastr.success(data.message);

					//setTimeout(function () { location.reload(); }, 2000);
				}
			},
			error: function (err) { console.log(err); }
		});
	}
}
//function GetDropdownValues(data) {
//    $.each(data, function (k, v) {
//        $("#CustomerName").val(v.CustomerName);
//        $("#OrderStyle").append("<option value=" + v.Value + ">" + v.OrderStyle + "</option>");
//        $("#OrderSheet").append("<option value=" + v.Value + ">" + v.OrderSheet + "</option>");
//        $("#OrderNo").val(v.OrderNo);
//        $("#Qty").val(v.Qty);
//    })

//}
$("#EnableDelivery").change(function () {

	if ($("#EnableDelivery").is(':checked')) {
		$("#ChargesType").val("");
		$("#ChargesType").prop('disabled', false);
		//$("#DeliveryChargesAmount").val("");
		$("#DeliveryChargesAmount").prop('disabled', false);
	}
	else {
		$("#ChargesType").val("");
		$("#DeliveryChargesAmount").val("");
		$("#ChargesType").prop('disabled', true);
		$("#DeliveryChargesAmount").prop('disabled', true);
	}
})


function GetURLParameter() {
	var sPageURL = window.location.href;
	var indexOfLastSlash = sPageURL.lastIndexOf("/");

	if (indexOfLastSlash > 0 && sPageURL.length - 1 != indexOfLastSlash)
		return sPageURL.substring(indexOfLastSlash + 1);
	else
		return 0;
}

jQuery.expr[':'].contains = function (a, i, m) {
	return jQuery(a).text().toUpperCase()
		.indexOf(m[3].toUpperCase()) >= 0;
};

function validatePrice(event) {
	var $this = $(event);
	$this.val($this.val().replace(/[^\d.]/g, ''));
}

