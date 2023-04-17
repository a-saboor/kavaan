'use strict';
//#region Global Variables and Arrays
var hash = "#ltn_tab_1_1";

let dashboardLoaded = false;
let accountSectionLoaded = false;
let documentSectionLoaded = false;
let notificationSectionLoaded = false;
let bookingSectionLoaded = false;
let couponSectionLoaded = false;
let suggestionSectionLoaded = false;
let changePasswordSectionLoaded = false;

let changeTab = true;

//var isAccountPageRendered = false;
//var countries;
//var cities;
//var areas;

//#endregion

//#region document ready function
$(document).ready(function () {

	initChangeTabFunc();

	$(window).bind('hashchange', function () {

		if (changeTab) {

			if (!location.hash)
				location.hash = '#account-info';

			hash = location.hash;
			if (!dashboardLoaded) {
				dashboardLoaded = true;
				$('.nav').find('a[href="' + hash + '"]').click();
			}

		}

	}).trigger('hashchange');

	$(window).resize(function () {
		$('#loading').height($('#dashboard_tab').height())
	}).trigger('resize');

	$('a[for="ImageFile"]').click(function () {
		$('input[name="ImageFile"]').click();
	});

});
//#endregion
//#region Other functions

function initChangeTabFunc() {
	$('.nav-link').click(function () {

		if (changeTab) {

			changeTab = false;

			$('.dashboard-sections').hide();
			//show loading icon
			$('#loading').show('fast', 'swing');

			const elem = this;
			const sectionTitle = $(this).attr('href');
			hash = $(this).attr('href');

			$(elem).siblings().removeClass('active');
			$(elem).addClass('active');

			if (sectionTitle === '#account-info') {
				$('.heading-dashboard').html('Customer Portal');
				if (accountSectionLoaded) {
					hideSections();
				}
				else {
					fetchProfile();
				}
			} else if (sectionTitle === '#documents') {
				$('.heading-dashboard').html('Documents');
				if (documentSectionLoaded) {
					hideSections();
				}
				else {
					fetchDocuments();
				}
			} else if (sectionTitle === '#notifications') {
				$('.heading-dashboard').html('Notifications');
				if (notificationSectionLoaded) {
					hideSections();
				}
				else {
					fetchNotifications();
				}
			} else if (sectionTitle === '#my-bookings') {
				$('.heading-dashboard').html('My Bookings');
				if (bookingSectionLoaded) {
					hideSections();
				}
				else {
					fetchBookings();
				}
			} else if (sectionTitle === '#coupons') {
				$('.heading-dashboard').html('Coupons');
				if (couponSectionLoaded) {
					hideSections();
				}
				else {
					fetchCoupons();
				}

			} else if (sectionTitle === '#suggestions') {
				$('.heading-dashboard').html('Suggestions');
				if (suggestionSectionLoaded) {
					hideSections();
				}
				else {
					fetchSuggestions();
				}
			} else if (sectionTitle === '#change-password') {
				$('.heading-dashboard').html('Change Password');
				if (changePasswordSectionLoaded) {
					hideSections();
				}
				else {
					fetchChangePassword();
				}
			}

		}
	});
}

function fetchProfile() {
	if (!accountSectionLoaded) {
		accountSectionLoaded = true;
		$.ajax({
			type: 'Get',
			url: `/customer/account/profile`,
			success: function (response) {
				if (response) {
					BindSection(response);
				} else {
					console.log("Profile Error!");
				}
			},
			error: function (e) {
				console.log("Profile Error!");
			}
		});

		function BindSection(view) {
			$('#dashboard_render_section').append(view);
			hideSections();
		}
	}
}

function fetchSuggestions() {
	if (!suggestionSectionLoaded) {
		suggestionSectionLoaded = true;
		$.ajax({
			type: 'Get',
			url: `/customer/suggestion/index`,
			success: function (response) {
				if (response) {
					BindSection(response);
				} else {
					console.log("Suggestion Error!");
				}
			},
			error: function (e) {
				console.log("Suggestion Error!");
			}
		});

		function BindSection(view) {
			$('#dashboard_render_section').append(view);
			hideSections();
		}
	}
}

function fetchDocuments() {
	if (!documentSectionLoaded) {
		documentSectionLoaded = true;
		$.ajax({
			type: 'Get',
			url: `/customer/Documents/index`,
			success: function (response) {
				if (response) {
					BindSection(response);
				} else {
					console.log("Documents Error!");
				}
			},
			error: function (e) {
				console.log("Documents Error!");
			}
		});

		function BindSection(view) {
			$('#dashboard_render_section').append(view);
			hideSections();
		}
	}
}

function fetchNotifications() {
	if (!notificationSectionLoaded) {
		notificationSectionLoaded = true;
		$.ajax({
			type: 'Get',
			url: `/customer/Notification/index`,
			success: function (response) {
				if (response) {
					BindSection(response);
				} else {
					console.log("Notification Error!");
				}
			},
			error: function (e) {
				console.log("Notification Error!");
			}
		});

		function BindSection(view) {
			$('#dashboard_render_section').append(view);
			hideSections();
		}
	}
}

function fetchBookings() {
	if (!bookingSectionLoaded) {
		bookingSectionLoaded = true;
		$.ajax({
			type: 'Get',
			url: `/customer/Booking/index`,
			success: function (response) {
				if (response) {
					BindSection(response);
				} else {
					console.log("Booking Error!");
				}
			},
			error: function (e) {
				console.log("Booking Error!");
			}
		});

		function BindSection(view) {
			$('#dashboard_render_section').append(view);
			hideSections();
		}
	}
}

function fetchCoupons() {
	if (!couponSectionLoaded) {
		couponSectionLoaded = true;
		$.ajax({
			type: 'Get',
			url: `/customer/Coupon/index`,
			success: function (response) {
				if (response) {
					BindSection(response);
				} else {
					console.log("Coupon Error!");
				}
			},
			error: function (e) {
				console.log("Coupon Error!");
			}
		});

		function BindSection(view) {
			$('#dashboard_render_section').append(view);
			hideSections();
		}
	}
}

function fetchChangePassword() {
	if (!changePasswordSectionLoaded) {
		changePasswordSectionLoaded = true;
		$.ajax({
			type: 'Get',
			url: `/customer/account/ChangePassword`,
			success: function (response) {
				if (response) {
					BindSection(response);
				} else {
					console.log("Change Password Error!");
				}
			},
			error: function (e) {
				console.log("Change Password Error!");
			}
		});

		function BindSection(view) {
			$('#dashboard_render_section').append(view);
			hideSections();
		}
	}
}

function hideSections() {

	//hide loading icon
	$('#loading').hide();

	if (window.innerWidth > 767) {
		$(hash).show('fast', 'swing');
		scrollTop(0);
		//$("html, body").animate({ scrollTop: 0 }, 500);
	}
	else {
		$(hash).slideDown('fast', 'swing');
		scrollTop(650);
		//$("html, body").animate({ scrollTop: 650 }, 500);
	}
	changeTab = true;
}

//#endregion

//#region Ajax Call
function LoadAccount() {
	if (!isAccountPageRendered) {
		GetProfile();
		isAccountPageRendered = true;
	}
}

function GetProfile() {
	$.ajax({
		type: 'GET',
		url: '/Customer/Account/Profile',
		contentType: "application/json",
		success: function (response) {
			BindProfile(response.data);
		}
	});
}

function GetCities(id) {
	if (id) {
		$.ajax({
			type: 'POST',
			url: '/Customer/Dashboard/GetCites',
			data: {
				countryId: id,
			},
			success: function (response) {
				BindCites(response.data);
			}
		});
	}
	else {
		BindCites(0);
	}
}

function GetAreas(id) {
	if (id) {
		$.ajax({
			type: 'POST',
			url: '/Customer/Dashboard/GetAreas',
			data: {
				cityId: id,
			},
			success: function (response) {
				BindAreas(response.data);
			}
		});
	}
	else {
		BindAreas(0);
	}
}
//#endregion

//#region Functions for Binding Data
function BindProfile(data) {
	var form = $('#account form');

	$(form).find('input[name="UserName"]').val(data.name);
	$(form).find('input[name="FirstName"]').val(data.firstName);
	$(form).find('input[name="LastName"]').val(data.lastName);
	$(form).find('input[name="Email"]').val(data.email);
	$(form).find('input[name="Contact"]').val(data.contact);
	$(form).find('input[name="Address"]').val(data.address);
	$(form).find('input[name="Address2"]').val(data.address2);
	$(form).find('input[name="CustomerCity"]').val(data.city);
	$(form).find('input[name="PoBox"]').val(data.poBox);
	$(form).find('input[name="ZipCode"]').val(data.zipCode);
	$(form).find('input[name="CNICNo"]').val(data.cnicNo);
	$(form).find('input[name="CNICExpiry"]').val(data.cnicExpiry);
	$(form).find('input[name="PassportNo"]').val(data.passportNo);
	$(form).find('input[name="PassportExpiry"]').val(data.passportExpiry);

	$('#account form input[name="CNICExpiry"]').datepicker({
		value: data.cnicExpiry,
	});
	$('#account form input[name="PassportExpiry"]').datepicker({
		value: data.passportExpiry,
	});

	if (data.country)
		$('#account form select[name="CustomerCountry"]').val(data.country);

	//BindCountries(data.countries);
	//BindCites(data.cities);
	//BindAreas(data.areas);

	$(form).find('section').hide();
}
function BindCountries(data) {
	var form = $('#account form');

	if (data && data.length) {
		$(form).find($('select[name="CountryID"]')).html($("<option />").val('').text(ChangeString("Select Country", "تحديد بلد")));
		$.each(data, function (k, v) {
			$(form).find($('select[name="CountryID"]')).append($("<option />").val(v.Value).text(v.Text).attr('selected', v.Selected));
		});
	}
	else {
		$(form).find($('select[name="CountryID"]')).html($("<option />").val('').text(ChangeString("Select Country", "تحديد بلد")));
		BindAreas(0);
	}
}
function BindCites(data) {
	var form = $('#account form');

	if (data && data.length) {
		$(form).find($('select[name="CityID"]')).html($("<option />").val('').text(ChangeString("Select City", "تحديد مدينة")));
		$.each(data, function (k, v) {
			$(form).find($('select[name="CityID"]')).append($("<option />").val(v.Value).text(v.Text).attr('selected', v.Selected));
		});
	}
	else {
		$(form).find($('select[name="CityID"]')).html($("<option />").val('').text(ChangeString("Select City", "تحديد مدينة")));
		BindAreas(0);
	}
}
function BindAreas(data) {
	var form = $('#account form');

	if (data && data.length) {
		$(form).find($('select[name="AreaID"]')).html($("<option />").val('').text(ChangeString("Select Area", "تحديد منطقة")));
		$.each(data, function (k, v) {
			$(form).find($('select[name="AreaID"]')).append($("<option />").val(v.Value).text(v.Text).attr('selected', v.Selected));
		});
	}
	else {
		$(form).find($('select[name="AreaID"]')).html($("<option />").val('').text(ChangeString("Select Area", "تحديد منطقة")));
	}
}
//#endregion

//form submit function
function validateAccountForm(form) {

	$.each($('#account form input'), function (k, elem) {
		if (!$(elem).val()) {
			$(elem).next('span.text-danger').html(`${$(elem).prev('label').html()} ${ChangeString('is required.', 'مطلوب.')}`);
		}
		else {
			$(elem).next('span.text-danger').html('');
		}
	});
	$.each($('#account form select'), function (k, elem) {
		if (!$(elem).val()) {
			$(elem).next('span.text-danger').html(`${$(elem).prev('label').html()} ${ChangeString('is required.', 'مطلوب.')}`);
		}
		else {
			$(elem).next('span.text-danger').html('');
		}
	});
	$.each($('#account form input[inputmode="numeric"]'), function (k, elem) {
		if (!$(elem).val()) {
			$(elem).closest('div').next('span.text-danger').html(`${$(elem).closest('div').prev('label').html()} ${ChangeString('is required.', 'مطلوب.')}`);
		}
		else {
			$(elem).closest('div').next('span.text-danger').html('');
		}
	});

	var emptyField = 0;
	$.each($('#account form span.text-danger'), function (k, elem) {
		if ($(elem).text()) {
			emptyField = 1;
		}
	});
	if (!emptyField) {
		return true;
	}

	return false;
}

function accountFormSubmit(form) {

	var btn = $(form).find('button[type="submit"]');
	disableSubmitButton(btn, true);

	//ajax start
	$.ajax({
		url: $(form).attr('action'),///@culture/Customer/Account/Profile
		type: 'Post',
		data: $(form).serialize(),
		success: function (response) {
			if (response.success) {
				AlertMessage(form, icon_success, response.message, '', 6);

				const bookingUrl = $.cookie("_bookingUrl");
				if (bookingUrl)
					window.location = bookingUrl;
			} else {
				AlertMessage(form, icon_error, response.message, '', 6);
			}
			disableSubmitButton(btn, false);
		},
		error: function (e) {
			AlertMessage(form, icon_error, ServerError, '', 6)
			disableSubmitButton(btn, false);
		},
		failure: function (e) {
			AlertMessage(form, icon_error, ServerError, '', 6)
			disableSubmitButton(btn, false);
		}
	});

}

function img_upload_callback(form, status) {

	var fileUpload = $(form).find('input[type="file"]').get(0);
	var files = fileUpload.files;

	// Create FormData object  
	var fileData = new FormData();

	// Looping over all files and add it to FormData object  
	for (var i = 0; i < files.length; i++) {
		fileData.append(files[i].name, files[i]);
	}

	imageSubmit(form, fileData, "/" + culture + "/Customer/Account/ProfileImage")
}

function img_upload_form_callback(form, status, response, url) {

	if (status) {
		$('.img-person').attr('src', response.image);
	}
}