"use strict";

var carAttributes = [];

var CarAttributeSetting = [];
var t;

var carVariationAttributes = [];

var KTTagifyCarTags = function (id) {
	var tagify;
	// Private functions
	var demo5 = function (id) {
		$.ajax({
			url: '/Admin/CarTag/GetCarTags/' + GetURLParameter(),
			type: 'GET',
			success: function (response) {
				if (response.success) {

					// Init autocompletes
					var toEl = document.getElementById(id);
					tagify = new Tagify(toEl, {
						delimiters: ", ", // add new tags when a comma or a space character is entered
						maxTags: 10,
						blacklist: ["fuck", "shit", "pussy"],
						keepInvalidTags: false, // do not remove invalid tags (but keep them marked as invalid)
						whitelist: response.tags,
						enforceWhitelist: true,
						templates: {
							dropdownItem: function (tagData) {
								try {
									var html = '';

									html += '<div class="tagify__dropdown__item">';
									html += '   <div class="d-flex align-items-center">';
									//html += '       <span class="symbol sumbol-' + (tagData.initialsState ? tagData.initialsState : '') + ' mr-2">';
									//html += '           <span class="symbol-label" style="background-image: url(\'' + (tagData.pic ? tagData.pic : '') + '\')">' + (tagData.initials ? tagData.initials : '') + '</span>';
									//html += '       </span>';
									html += '       <div class="d-flex flex-column">';
									html += '           <a href="javascript:void(0);" id="' + (tagData.id ? tagData.id : '') + '" class="text-dark-75 text-hover-primary font-weight-bold">' + (tagData.value ? tagData.value : '') + '</a>';
									//html += '           <span class="text-muted font-weight-bold">' + (tagData.value ? tagData.value : '') + '</span>';
									html += '       </div>';
									html += '   </div>';
									html += '</div>';

									return html;
								} catch (err) { }
							}
						},
						transformTag: function (tagData) {
							tagData.class = 'tagify__tag tagify__tag--primary';
						},
						dropdown: {
							classname: "color-blue",
							enabled: 1,
							maxItems: 5
						}
					})

					tagify.addTags(response.carTags);
					tagify.on('dropdown:select', onSelectSuggestion)
					tagify.on('remove', function (e) {

						$.ajax({
							url: '/Admin/CarTag/Delete/',
							type: 'POST',
							data: {
								"__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
								carTag: { CarID: GetURLParameter(), TagID: e.detail.data.id }
							},
							success: function (response) {
								if (response.success) {
									toastr.success(response.message);
								}
								else {
									toastr.error(response.message);
									return false;
								}
							},
							error: function (e) {
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							},
							failure: function (e) {
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							}
						});
					})
				} else {
				}
			}
		});

		var addAllSuggestionsElm;

		function onSelectSuggestion(e) {

			$.ajax({
				url: '/Admin/CarTag/Create/',
				type: 'POST',
				data: {
					"__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
					carTag: { CarID: GetURLParameter(), TagID: $(e.detail.tagify.DOM.dropdown).find('.tagify__dropdown__item--active a').attr('id') }
				},
				success: function (response) {
					if (response.success) {
						toastr.success(response.message);
					}
					else {
						toastr.error(response.message);
						return false;
					}
				},
				error: function (e) {
					toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
				},
				failure: function (e) {
					toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
				}
			});
		}

		// create a "add all" custom suggestion element every time the dropdown changes
		function getAddAllSuggestionsElm() {
			// suggestions items should be based on "dropdownItem" template
			return tagify.parseTemplate('dropdownItem', [{
				class: "addAll",
				name: "Add all",
				email: tagify.settings.whitelist.reduce(function (remainingSuggestions, item) {
					return tagify.isTagDuplicate(item.value) ? remainingSuggestions : remainingSuggestions + 1
				}, 0) + " Members"
			}])
		}
	}
	return {
		// public functions
		init: function (id) {
			demo5(id);
		}
	};
}();

window.addEventListener('load', function () {

	$('#CarType').trigger('change');
})

jQuery(document).ready(function () {

	toastr.options = {
		"positionClass": "toast-bottom-left",
	};

	$("#datepicker").datepicker({
		format: "yyyy",
		viewMode: "years",
		minViewMode: "years"
	});
	//$(function () {
	//    $("#datepicker").datepicker({ dateFormat: 'yy' });
	//});
	//var startYear = 1965;
	//for (i = new Date().getFullYear(); i > startYear; i--) {
	//    $('#yearpicker').append($('<option />').val(i).html(i));
	//}


	$("#Name").on('change', function () {
		var name = $(this);
		$("#Slug").val($(name).val().replace(/ /g, "-").toLocaleLowerCase());
	});

	$("#CarMakeID").change(function () {

		var CarMakeID = $('#CarMakeID').val();
		$("#CarModelID").empty();
		getCarModelsByCarMakeID(CarMakeID);
	});

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

	$("#EnableDelivery").trigger('change');

	$("#AllowKilometer").change(function () {

		if ($("#AllowKilometer").is(':checked')) {
			$("#Kilometer").val($("#Kilometer").attr("data"));
			$("#Kilometer").attr('placeholder', 'Enter price / km').prop('disabled', false).prop('required', true);

		}
		else {
			$("#Kilometer").attr("data", $("#Kilometer").val());
			$("#Kilometer").val('').attr('placeholder', '').prop('disabled', true).prop('required', false);
		}
	})

	$("#AllowKilometer").trigger('change');

	KTTagifyCarTags.init("kt_tagify_car_tags");

	/*BindCarFeatures();*/

	/*  $("#CarMakeID").trigger("change");*/

	BindCarCategories();

	BindCarFeatures();

	BindCarImages();

	BindPackages();

	BindDocument();

	$('#kt_image_car_image #Image').change(function () {
		var data = new FormData();
		var files = $("#kt_image_car_image #Image").get(0).files;
		var elem = $(this)
		if (files.length > 0) {
			$(elem).closest('.image-input').find('label[data-action="change"] i').hide();
			$(elem).closest('.image-input').find('label[data-action="change"]').addClass('spinner spinner-dark spinner-center spinner-sm').prop('disabled', true);
			data.append("Image", files[0]);
			$.ajax({
				url: "/Admin/Car/Thumbnail/" + GetURLParameter(),
				type: "POST",
				processData: false,
				contentType: false,
				data: data,
				success: function (response) {
					if (response.success) {
						$('#kt_image_car_image .image-input-wrapper').css('background-image', 'url(' + response.data + ')');
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

			if ($('.car-gallery-images .symbol').length + files.length > 4) {
				toastr.error("You can only upload four images");
				return;
			}

			$('#btn-gallery-image-upload').addClass('spinner spinner-light spinner-left').prop('disabled', true);
			$('#btn-gallery-image-upload').find('span').hide();
			$.each(files, function (j, file) {
				data.append('Image[' + j + ']', file);
			})
			//data.append("Image", files);
			data.append("count", $('.car-gallery-images .symbol').length);

			$.ajax({
				url: "/Admin/CarImage/Create/" + GetURLParameter(),
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

						if ($('.car-gallery-images .symbol').length === 4) {
							$('#btn-gallery-image-upload').hide();
						} else {
							$('#btn-gallery-image-upload').show();
						}
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
		var e = $("#btnSaveCar");
		$(e).addClass('spinner spinner-light spinner-left').prop('disabled', true);
		$(e).find('i').hide();
		var brands = $("#BrandID option:selected").val();

		$.ajax({
			url: '/Admin/Car/Update/',
			type: 'POST',
			data: {
				"__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
				car: {

					ID: GetURLParameter(),
					SKU: $('input[name=SKU]').val(),
					Name: $('input[name=Name]').val(),
					NameAr: $('input[name=NameAr]').val(),
					Slug: $('input[name=Slug]').val(),
					CategoryID: $("#CategoryID").val(),
					CarMakeID: $("#CarMakeID").val(),
					BodyTypeID: $("#BodyTypeID").val(),
					CarModelID: $("#CarModelID").val(),
					PricePerKilometer: $("#Kilometer").val(),
					ChargesType: $("#ChargesType").val(),
					LicensePlate: $("#LicensePlate").val(),
					DeliveryChargesAmount: $("#DeliveryChargesAmount").val(),
					ChargesType: $("#ChargesType").val(),
					EnableDelivery: $('input[name=EnableDelivery]').prop('checked'),
					BrandID: brands,
					/*ShortDescription: EditorShortDescription.getData(),*/
					//ShortDescriptionAr: EditorShortDescriptionAr.getData(),
					ShortDescriptionAr: "-",
					TermsAndCondition: $("#termsAndCondition").val(),
					TermsAndConditionAr: $("#termsAndConditionAr").val(),
					CancelationPolicy: $("#CancelationPolicy").val(),
					CancelationPolicyAr: $("#CancelationPolicyar").val(),
					LongDescription: EditorLongDescription.getData(),
					//LongDescriptionAr: EditorLongDescriptionAr.getData(),
					LongDescriptionAr: "-",
					Specification: $('textarea[name=Specification]').val(),
					SpecificationAr: $('textarea[name=SpecificationAr]').val(),
					MobileDescription: $('textarea[name=MobileDescription]').val(),
					//MobileDescriptionAr: $('textarea[name=MobileDescriptionAr]').val(),
					MobileDescriptionAr: "-",
					Doors: $('select[name=Doors]').val(),
					Cylinders: $('select[name=Cylinders]').val(),
					HorsePower: $('select[name=HorsePower]').val(),
					RegionalSpecification: $('select[name=RegionalSpecification]').val(),

					FuelEconomy: $('input[name=FuelEconomy]').val(),
					Transmission: $('input[name=Transmission]').val(),
					Capacity: $('select[name=Capacity]').val(),
					Year: $('input[name=Year]').val(),


					RegularPrice: $('input[name=RegularPrice]').val(),
					SalePrice: $('input[name=SalePrice]').val(),
					SalePriceFrom: $('#sale-price-dates').is(":visible") ? $('input[name=startDate]').val() : null,
					SalePriceTo: $('#sale-price-dates').is(":visible") ? $('input[name=endDate]').val() : null,
					Stock: $('input[name=IsManageStock]').prop('checked') ? $('input[name=Stock]').val() : null,
					Threshold: $('input[name=IsManageStock]').prop('checked') ? $('input[name=StockThreshold]').val() : null,
					StockStatus: !$('input[name=IsManageStock]').prop('checked') ? $('select[name=StockStatus]').val() : null,

					Weight: $('input[name=Weight]').val(),
					Length: $('input[name=Length]').val(),
					Width: $('input[name=Width]').val(),
					Height: $('input[name=Height]').val(),

					PurchaseNote: $('textarea[name=PurchaseNote]').val(),
					EnableReviews: $('input[name=EnableReviews]').prop('checked'),

					Type: $('select[name=CarType]').val(),
					IsManageStock: $('input[name=IsManageStock]').prop('checked'),
					IsPublished: $('input[name=IsPublished]').prop('checked'),
					IsRecommended: $('input[name=IsRecommended]').prop('checked'),
					IsTaxInclusive: $('input[name=IsTaxInclusive]').prop('checked'),
					/*IsFeatured: $('input[name=IsFeatured]').prop('checked'),*/
					IsSoldIndividually: $('input[name=IsSoldIndividually]').prop('checked'),
					Status: $('select[name=Status]').val(),

					PublishStartDate: $('#publish-schedule').is(":visible") ? $('input[name=PublishStartDate]').val() : null,
					PublishEndDate: $('#publish-schedule').is(":visible") ? $('input[name=PublishEndDate]').val() : null,
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

function SaveCar(e) {


}

function getPrice(f) {

	var getval = $(f).closest('.car-packages').find('input[name="chkcarpackage"]').val();
	var getPrice = $(f).closest('.car-packages').find('input[name="Price"]').val();
	var getKilometers = $(f).closest('.car-packages').find('input[name="Kilometer"]').val();

	$.ajax({
		url: '/Admin/Car/AddCarPackage/',
		type: 'POST',
		data: {
			"data": { CarID: GetURLParameter(), PackageID: getval, Kilometer: getKilometers, Price: getPrice, IsActive: true }
		},
		success: function (response) {
			if (response.success) {

				$(chkcarPackage).closest('.car-packages').find('input[name="Kilometer"]').attr('disabled', false);
				$(chkcarPackage).closest('.car-packages').find('input[name="Price"]').attr('disabled', false);
				toastr.success(response.message);
			} else {
				$(chkcarPackage).prop('checked', false);
				toastr.error(response.message);
				return false;
			}
			$(chkcarPackage).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
			$(chkcarPackage).prop('disabled', false);
		},
		error: function (e) {

			$(chkcarPackage).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
			$(chkcarPackage).prop('disabled', false);
			toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
		},
		failure: function (e) {

			$(chkcarPackage).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
			$(chkcarPackage).prop('disabled', false);
			toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
		}

	});

};

function BindCarImages() {
	$.ajax({
		url: '/Admin/CarImage/GetCarImages/' + GetURLParameter(),
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

				if ($('.car-gallery-images .symbol').length === 4) {
					$('#btn-gallery-image-upload').hide();
				} else {
					$('#btn-gallery-image-upload').show();
				}
			} else {
			}
		}
	});
}

function DeleteGalleryImage(elem, record) {
	$(elem).addClass('spinner spinner-light spinner-right').prop('disabled', true);
	$(elem).find('i').hide();
	$.ajax({
		url: '/Admin/CarImage/Delete/' + record,
		type: 'POST',
		data: {
			"__RequestVerificationToken":
				$("input[name=__RequestVerificationToken]").val()
		},
		success: function (response) {
			if (response.success) {
				$(elem).closest('.symbol').remove();
				toastr.success(response.message);

				if ($('.car-gallery-images .symbol').length === 4) {
					$('#btn-gallery-image-upload').hide();
				} else {
					$('#btn-gallery-image-upload').show();
				}
			}
			else {
				$(elem).removeClass('spinner spinner-light spinner-right').prop('disabled', false);
				$(elem).find('i').show();
				toastr.error(response.message);
				return false;

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

function BindPackages() {

	$.ajax({
		url: '/Admin/Car/GetCarPackages/' + GetURLParameter(),
		type: 'GET',
		success: function (response) {
			if (response.success) {
				var html = "";
				$('.car-packages').html('');
				$(response.packages).each(function (k, v) {

					$('#tBodyCarPackages').append(`<tr class="car-packages  car-packages-plain " id="car-packages${v.id}">
														<td>
													        <span class="switch">
													             <label>
													                 <input type="checkbox" class="car-Package" name="chkcarpackage" id="car-package${v.id}" value="${v.id}">
													                 <span class=""></span>
													             </label> 
													         </span>
													    </td>
														<td> <span class=""><b>${v.name}</b></span> </td>
														<td>
															<div class="form-group mb-0 m-auto" >
																<div class="input-group mb-2" >
																	<input type="number" disabled class="form-control price" name="Price" onchange="getPrice(this)" id="packagePrice${v.id}" data="${v.id}"/>
																	<div class="input-group-append">
																		<span class="input-group-text">
																			<i class="fa fa-money-bill"></i>
																		</span>
																	</div>
																</div>
															</div>
														</td>
														<td>
															<div class="form-group  mb-0 m-auto" >
																<div class="input-group mb-2">
																	<input type="number" disabled class="form-control kilometer" name="Kilometer"  onchange="getPrice(this)" id="packageKilomerters${v.id}" data="${v.id}"/>
																	<div class="input-group-append">
																		<span class="input-group-text">
																			<i class="fa fa-road"></i>
																		</span>
																	</div>
																</div>
															</div>
														</td>
													</tr>`);

					//html += `<div class="car-packages row car-packages-plain mt-10" id="car-packages${v.id}">
					//            <span class="switch ml-1">
					//                 <label>
					//                     <input type="checkbox" class="car-Package" name="chkcarpackage" id="car-package${v.id}" value="${v.id}">
					//                     <span class="ml-10"></span>
					//                 </label> 
					//             </span>
					//             <div class="col-md-3 pl-18">
					//                <span class="pl-19"><b>${v.name}</b></span>                                    
					//             </div>
					//            <input type="number" disabled class="col-2 ml-17 form-control price" name="Price" onchange="getPrice(this)" id="packagePrice${v.id}" data="${v.id}"/>
					//            <input type="number" disabled class="col-2 ml-17 form-control kilometer" name="Kilometer"  onchange="getPrice(this)" id="packageKilomerters${v.id}" data="${v.id}"/>
					//         </div>`;
				});

				//$('.car-packages').append(html);
				$(response.CarPackages).each(function (k, v) {
					$("#car-package" + v.packageId).prop('checked', true).attr('carPackageId', v.id);
					$("#packagePrice" + v.packageId).val(v.price);
					$("#packageKilomerters" + v.packageId).val(v.kilometer);
					$("#packagePrice" + v.packageId).prop('disabled', false);
					$("#packageKilomerters" + v.packageId).prop('disabled', false);

				});

				//$(".price").change(function () {				//    
				//    var packageID = $(this).closest('.price').find('input[name="chkcarpackage"]').val();
				//});

				$('.car-Package').change(function () {

					var chkcarPackage = $(this);
					var price = $(this).closest('.car-packages').find('input[name="Price"]').val();
					var km = $(this).closest('.car-packages').find('input[name="Kilometer"]').val();

					$(chkcarPackage).closest('.checkbox').addClass('spinner spinner-dark spinner-right');
					$(chkcarPackage).prop('disabled', true);
					if ($(chkcarPackage).prop('checked')) {
						$.ajax({
							url: '/Admin/Car/AddCarPackage/',
							type: 'POST',
							data: {
								"data": { CarID: GetURLParameter(), PackageID: $(this).val(), Kilometer: km, Price: price, IsActive: true }
							},
							success: function (response) {
								if (response.success) {

									$(chkcarPackage).closest('.car-packages').find('input[name="Kilometer"]').attr('disabled', false);
									$(chkcarPackage).closest('.car-packages').find('input[name="Price"]').attr('disabled', false);
									toastr.success(response.message);
								} else {
									$(chkcarPackage).prop('checked', false);
									toastr.error(response.message);
									return false;
								}
								$(chkcarPackage).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkcarPackage).prop('disabled', false);
							},
							error: function (e) {

								$(chkcarPackage).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkcarPackage).prop('disabled', false);
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							},
							failure: function (e) {

								$(chkcarPackage).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkcarPackage).prop('disabled', false);
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							}

						});
					} else {
						var price = $(this).closest('.car-packages').find('input[name="Price"]').val();
						var km = $(this).closest('.car-packages').find('input[name="Kilometer"]').val();
						$.ajax({
							url: '/Admin/Car/DeleteCarPackage/',
							type: 'POST',
							data: {
								"data": { CarID: GetURLParameter(), PackageID: $(this).val(), Kilometer: km, Price: price, IsActive: false }
							},
							success: function (response) {
								if (response.success) {
									$(chkcarPackage).attr('carPackageId', '');
									toastr.success(response.message);
								} else {
									$(chkcarPackage).prop('checked', true);
									toastr.error(response.message);
									return false;
								}
								$(chkcarPackage).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkcarPackage).prop('disabled', false);
							},
							error: function (e) {
								$(chkcarPackage).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkcarPackage).prop('disabled', false);
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							},
							failure: function (e) {
								$(chkcarPackage).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkcarPackage).prop('disabled', false);
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							}
						});
					}
				});

			} else {
				$('.car-Package').html('No Packages!');
			}
		}
	});
}

function BindDocument() {

	$.ajax({
		url: '/Admin/Car/GetDocuments/' + GetURLParameter(),
		type: 'GET',
		success: function (response) {
			if (response.success) {
				var html = "";
				$('.car-documents').html('');

				$(response.document).each(function (k, v) {

					html += `<div class="row mt-1 ${v.id}">                                 
                                 <div class="col-8">
                                     <label class="mt-2"><i class="fa fa-file-contract mr-2"></i><b>${v.name}</b></label>
                                 </div>                
                                 <div class="col-2">
                                     <a href="${v.path}" class="btn btn-bg-secondary btn-icon btn-sm" target="blank"> <span class="fas fa-eye"></span></a>
                                 </div>
                                 <div class="col-2">
                                    <button id="btnDeleteDocs" class="btn btn-bg-secondary btn-icon btn-sm" onclick="DeleteDocument(this,${v.id} )">
										<span class="fas fa-trash "></span>
									</button>
                                 </div>
                             </div> `;
				});

				$('.car-documents').append(html);
			} else {
				$('.car-Package').html('No Packages!');
			}
		}
	});
}

function SaveNewCarDocs() {

	$("#btnSubmitDocument").addClass('spinner spinner-dark spinner-right');
	var data = new FormData();
	var files = $("#fileUpload").get(0).files;
	if (files.length > 0) {
		data.append("FileUpload", files[0]);
	}

	data.append("Name", $('#NameDocs').val());
	data.append("id", $("#CarID").val());

	$.ajax({
		url: '/Admin/Car/CreateDocuments/',
		type: 'POST',
		processData: false,
		contentType: false,
		data: data,
		success: function (response) {
			if (response.success) {

				$('#myModal').modal('hide');
				var html = "";

				html += `<div class="row mt-1 ${response.data.ID}">
                                 <div class="col-8">
                                     <label class="mt-2"><i class="fa fa-file-contract mr-2"></i><b>${response.data.Name}</b></label>
                                 </div>                
                                 <div class="col-2">
                                     <a href="${response.data.Path}" class="btn btn-bg-secondary btn-icon btn-sm" target="blank"> <span class="fas fa-eye"></span></a>
                                 </div>
                                 <div class="col-2">
                                    <button id="btnDeleteDocs" class="btn btn-bg-secondary btn-icon btn-sm" onclick="DeleteDocument(this,${response.data.ID} )">
										<span class="fas fa-trash "></span>
									</button>
                                 </div>
                             </div> `;

				$('.car-documents').append(html);
			}
		},
	});
}

function SaveNewCarMake() {

	$("#btnSubmitCarMake").addClass('spinner spinner-dark spinner-right');

	var data1 = $('#CarMakeName').text();
	var data = new FormData();


	data.append("Name", $('#CarMakeName').val());
	data.append("NameAR", $("#CarMakeNamear").val());


	$.ajax({
		url: '/Admin/Car/AddCarMake/',
		type: 'POST',
		processData: false,
		contentType: false,
		data: data,
		success: function (response) {
			if (response.success) {

				$('#myModal').modal('hide');

				$("#CarMakeID").append("<option selected value=" + response.id + ">" + response.value + "</option>");
				$("#CarModelID").empty();
				toastr.success(response.message);




			}
		},


	});

}

function SaveNewBodyType() {

	$("#btnSubmitBodyType").addClass('spinner spinner-dark spinner-right');

	//var data1 = $('#CarMakeName').text();
	var data = new FormData();


	data.append("Name", $('#bodytypeName').val());
	data.append("NameAR", $("#bodytypeNamear").val());


	$.ajax({
		url: '/Admin/Car/AddBodyType/',
		type: 'POST',
		processData: false,
		contentType: false,
		data: data,
		success: function (response) {
			if (response.success) {

				$('#myModal').modal('hide');

				$("#BodyTypeID").append("<option selected value=" + response.id + ">" + response.value + "</option>");
				toastr.success(response.message);

			}
		},


	});

}

function SaveNewCarModel() {

	$("#btnSubmitCarModel").addClass('spinner spinner-dark spinner-right');

	//var data1 = $('#CarMakeName').text();
	var data = new FormData();

	data.append("CarMake_ID", $('#CarMakeID').val());
	data.append("Name", $('#carmodelName').val());
	data.append("NameAR", $("#carmodelNamear").val());


	$.ajax({
		url: '/Admin/Car/AddCarModel/',
		type: 'POST',
		processData: false,
		contentType: false,
		data: data,
		success: function (response) {
			if (response.success) {

				$('#myModal').modal('hide');

				$("#CarModelID").append("<option selected value=" + response.id + ">" + response.value + "</option>");

				toastr.success(response.message);

			}
		},


	});

}

function SaveNewFeature() {

	$("#btnSubmitFeature").addClass('spinner spinner-dark spinner-right');
	var data = new FormData();
	data.append("Name", $('#FeatureName').val());
	data.append("NameAR", $("#FeatureNamear").val());
	data.append("CarID", GetURLParameter());

	$.ajax({
		url: '/Admin/Car/AddFeature/',
		type: 'POST',
		processData: false,
		contentType: false,
		data: data,
		success: function (response) {
			if (response.success) {
				$('#myModal').modal('hide');

				$('.car-features').append('<div class="car-feature car-feature-plain mb-1" id="car-feature' + response.id + '">' +
					'<label class="checkbox">' +
					'<input type="checkbox" onchange="MyNewFeature(this)" checked name="chkFeature' + response.id + '" id="chkFeature' + response.id + '" data="' + response.id + '" FeatureID=' + response.FeatureID + '/>' +
					'<span></span>' +
					response.name +
					'</label>' +
					'</div>');
				toastr.success(response.message);
			}
		},
	});
}

function MyNewFeature(e) {

	var fid = $(e).attr('data');

	if ($(e).prop('checked')) {
		$.ajax({
			url: '/Admin/CarFeature/Create/',
			type: 'POST',
			data: {
				"__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
				carFeature: { CarID: GetURLParameter(), FeatureID: fid }
			},
			success: function (response) {
				if (response.success) {
					$(e).attr('FeatureID', response.data.FeatureID);
					toastr.success(response.message);
				} else {
					$(e).prop('checked', false);
					toastr.error(response.message);
					return false;
				}
				$(e).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
				$(e).prop('disabled', false);
			},
			error: function (e) {

				$(chkfeature).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
				$(chkfeature).prop('disabled', false);
				toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
			},
			failure: function (e) {

				$(chkfeature).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
				$(chkfeature).prop('disabled', false);
				toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
			}

		});
	} else {
		$.ajax({
			url: '/Admin/CarFeature/Delete/' + $(e).attr('FeatureID'),
			type: 'POST',
			data: {
				"__RequestVerificationToken":
					$("input[name=__RequestVerificationToken]").val()
			},
			success: function (response) {
				if (response.success) {
					$(e).attr('FeatureID', '');
					toastr.success(response.message);
				} else {
					$(e).prop('checked', true);
					toastr.error(response.message);
					return false;
				}
				$(e).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
				$(e).prop('disabled', false);
			},
			error: function (ex) {
				$(e).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
				$(e).prop('disabled', false);
				toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
			},
			failure: function (fe) {
				$(e).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
				$(e).prop('disabled', false);
				toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
			}
		});
	}
}


function getCarModelsByCarMakeID(CarMakeID) {
	if (CarMakeID == "") { CarMakeID = -1; }
	var json = { "CarMakeID": CarMakeID };

	$.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: '/Admin/Car/GetCarModelsByCarMakeID',
		async: true,
		data: JSON.stringify(json),
		success: function (data) {

			if (data.data != 0) {
				GetDropdownValuesforUnits(data);
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

function GetDropdownValuesforUnits(data) {
	$.each(data, function (k, v) {
		$("#CarModelID").append("<option value=" + v.value + ">" + v.text + "</option>");
	})

}

function DeleteDocument(element, record) {



	swal.fire({
		title: 'Are you sure?',
		text: "You won't be able to revert this!",
		type: 'warning',
		showCancelButton: true,
		confirmButtonText: 'Yes, delete it!'
	}).then(function (result) {
		if (result.value) {

			$(this).addClass('spinner spinner-dark spinner-right');
			$.ajax({
				url: '/Admin/Car/DeleteCarDocument/' + record,
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
							$("." + result.data).remove();
							toastr.success('Document Deleted Successfully');
							$("#btnDeleteDocs").removeClass('spinner spinner-dark spinner-right')

						}
						else {
							toastr.error(result.message);
							$("#btnDeleteDocs").removeClass('spinner spinner-dark spinner-right')
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

/*Car Categories*/

function BindCarCategories() {

	$.ajax({
		url: '/Admin/CarCategory/GetCarCategories/' + GetURLParameter(),
		type: 'GET',
		success: function (response) {
			if (response.success) {
				$('.car-categories').html('');
				$(response.categories).each(function (k, v) {
					$('.car-categories').append('<div class="car-category car-category-plain mb-1" id="car-category' + v.id + '" parent-id="' + v.ParentId + '">' +
						'<label class="checkbox">' +
						'<input type="checkbox" name="chkCategory' + v.id + '" id="chkCategory' + v.id + '" data="' + v.id + '"/>' +
						'<span></span>' +
						v.name +
						'</label>' +
						(v.hasChilds ? '<div class="sub-category pl-10"></div>' : '') +
						'</div>');
				});

				$(response.carCategories).each(function (k, v) {
					$("#chkCategory" + v.categoryId).prop('checked', true).attr('carCategoryId', v.id);
				});

				$('.car-category-plain').each(function (k, v) {
					if ($(this).attr("parent-id") !== "null") {
						/*$(this).appendTo("#car-category" + $(this).attr("parent-id")).find('.sub-category');*/

						var html = $(this).get(0);
						$("#car-category" + $(this).attr("parent-id")).find('.sub-category:first').append(html);
						/*$(this).remove();*/
						$(this).removeClass("car-category-plain").addClass("car-category");
					}
				});

				$('.car-category input[type="checkbox"]').change(function () {
					var chkcategory = $(this);
					$(chkcategory).closest('.checkbox').addClass('spinner spinner-dark spinner-right');
					$(chkcategory).prop('disabled', true);
					if ($(chkcategory).prop('checked')) {
						$.ajax({
							url: '/Admin/CarCategory/Create/',
							type: 'POST',
							data: {
								"__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
								carCategory: { CarID: GetURLParameter(), CarCategoryID: $(this).attr('data') }
							},
							success: function (response) {
								if (response.success) {
									$(chkcategory).attr('carCategoryId', response.data);
									toastr.success(response.message);
								} else {
									$(chkcategory).prop('checked', false);
									toastr.error(response.message);
									return false;
								}
								$(chkcategory).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkcategory).prop('disabled', false);
							},
							error: function (e) {

								$(chkcategory).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkcategory).prop('disabled', false);
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							},
							failure: function (e) {

								$(chkcategory).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkcategory).prop('disabled', false);
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							}

						});
					} else {
						$.ajax({
							url: '/Admin/CarCategory/Delete/' + $(chkcategory).attr('carCategoryId'),
							type: 'POST',
							data: {
								"__RequestVerificationToken":
									$("input[name=__RequestVerificationToken]").val()
							},
							success: function (response) {
								if (response.success) {
									$(chkcategory).attr('carCategoryId', '');
									toastr.success(response.message);
								} else {
									$(chkcategory).prop('checked', true);
									toastr.error(response.message);
									return false;
								}
								$(chkcategory).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkcategory).prop('disabled', false);
							},
							error: function (e) {
								$(chkcategory).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkcategory).prop('disabled', false);
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							},
							failure: function (e) {
								$(chkcategory).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkcategory).prop('disabled', false);
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							}
						});
					}
				});

			} else {
				$('.car-categories').html('No Categories!');
			}
		}
	});
}

function SearchCategory(elem) {
	var text = $(elem).val().trim();
	if (text && text !== "") {
		$('.car-category .checkbox').hide();
		//$('.car-category .checkbox:contains("' + text + '")').css('background-color', 'red');
		$('.car-category .checkbox:contains("' + text + '")').fadeIn();
	} else {
		$('.car-category .checkbox').show();
	}
}

/*Car Categories*/

function BindCarFeatures() {

	$.ajax({
		url: '/Admin/CarFeature/GetCarFeatures/' + GetURLParameter(),
		type: 'GET',
		success: function (response) {
			if (response.success) {
				$('.car-features').html('');
				$(response.features).each(function (k, v) {
					$('.car-features').append('<div class="car-feature car-feature-plain mb-1" id="car-feature' + v.id + '">' +
						'<label class="checkbox">' +
						'<input type="checkbox" name="chkFeature' + v.id + '" id="chkFeature' + v.id + '" data="' + v.id + '"/>' +
						'<span></span>' +
						v.name +
						'</label>' +

						'</div>');
				});

				$(response.carFeatures).each(function (k, v) {
					$("#chkFeature" + v.FeatureID).prop('checked', true).attr('FeatureID', v.id);
				});

				//$('.car-feature-plain').each(function (k, v) {
				//    if ($(this).attr("parent-id") != "null") {
				//        /*$(this).appendTo("#car-feature" + $(this).attr("parent-id")).find('.sub-feature');*/

				//        var html = $(this).get(0);
				//        $("#car-feature" + $(this).attr("parent-id")).find('.sub-feature:first').append(html);
				//        /*$(this).remove();*/
				//        $(this).removeClass("car-feature-plain").addClass("car-feature");
				//    }
				//});

				$('.car-feature input[type="checkbox"]').change(function () {

					var chkfeature = $(this);
					$(chkfeature).closest('.checkbox').addClass('spinner spinner-dark spinner-right');
					$(chkfeature).prop('disabled', true);
					if ($(chkfeature).prop('checked')) {
						$.ajax({
							url: '/Admin/CarFeature/Create/',
							type: 'POST',
							data: {
								"__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
								carFeature: { CarID: GetURLParameter(), FeatureID: $(this).attr('data') }
							},
							success: function (response) {
								if (response.success) {
									$(chkfeature).attr('FeatureID', response.data);
									toastr.success(response.message);
								} else {
									$(chkfeature).prop('checked', false);
									toastr.error(response.message);
									return false;
								}
								$(chkfeature).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkfeature).prop('disabled', false);
							},
							error: function (e) {

								$(chkfeature).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkfeature).prop('disabled', false);
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							},
							failure: function (e) {

								$(chkfeature).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkfeature).prop('disabled', false);
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							}

						});
					} else {
						$.ajax({
							url: '/Admin/CarFeature/Delete/' + $(chkfeature).attr('FeatureID'),
							type: 'POST',
							data: {
								"__RequestVerificationToken":
									$("input[name=__RequestVerificationToken]").val()
							},
							success: function (response) {
								if (response.success) {
									$(chkfeature).attr('FeatureID', '');
									toastr.success(response.message);
								} else {
									$(chkfeature).prop('checked', true);
									toastr.error(response.message);
									return false;
								}
								$(chkfeature).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkfeature).prop('disabled', false);
							},
							error: function (e) {
								$(chkfeature).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkfeature).prop('disabled', false);
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							},
							failure: function (e) {
								$(chkfeature).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
								$(chkfeature).prop('disabled', false);
								toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
							}
						});
					}
				});

			} else {
				$('.car-features').html('No Features!');
			}
		}
	});
}

function SearchFeature(elem) {
	var text = $(elem).val().trim();
	if (text && text != "") {
		$('.car-feature .checkbox').hide();
		//$('.car-feature .checkbox:contains("' + text + '")').css('background-color', 'red');
		$('.car-feature .checkbox:contains("' + text + '")').fadeIn();
	} else {
		$('.car-feature .checkbox').show();
	}
}

function TogglePublishSchedule(e) {

	if ($(e).text().trim() == "Schedule") {
		$('#publish-schedule').slideDown();
		$(e).text('Cancel');

	} else {
		$('#publish-schedule').slideUp();
		$(e).text('Schedule');
	}

}

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

