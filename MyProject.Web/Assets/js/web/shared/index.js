'use strict';

//#region Global Variables and Arrays
//var categories;
let culture = 'en-ae';
let lang = 'en';
let InfoEmail = "info@KAVAAN.com";
let CheckUrl = false;
const Is_Mobile = window.innerWidth <= 425 ? true : false;
const Is_Tablet = window.innerWidth <= 768 && window.innerWidth >= 426 ? true : false;
const Is_Laptop = window.innerWidth <= 1024 && window.innerWidth >= 769 ? true : false;
const Is_Laptop_Lg = window.innerWidth >= 1025 ? true : false;
const today = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
const current_url_path = window.location.pathname;

if (typeof checkUrlfalse != "undefined") {
	CheckUrl = checkUrlfalse;
}

//var IsLoyaltyEnabled;
//var DefaultAvatarUrl = "/Assets/images/default/default-omw-avatar.png";
//var DefaultImageUrl = "/Assets/images/default/default-omw-image.png";
//var DefaultMapMarker = "/Assets/images/web-icons/map-marker.png";
const DefaultContactImageUrl = "/Assets/images/bg/abu-dhabi-building.png";
const DefaultBlogImageUrl = "/Assets/images/service-images/Handyman.png";
const DefaultEventImageUrl = "/Assets/images/service-images/Handyman.png";
const DefaultNewsfeedImageUrl = "/Assets/images/service-images/Handyman.png";
const DefaultPropertyImageUrl = "/Assets/images/service-images/Handyman.png";
const DefaultUnitImageUrl = "/Assets/images/service-images/Handyman.png";
const DefaultPropertyFeaturesUrl = "/Assets/images/web-icons/theaters-icon.png";

if ($.cookie("_culture") != "undefined" && false) {
	culture = $.cookie("_culture");
	//if (lang == 'ar') {
	//window.location.pathname = window.location.pathname.includes('en/') ? window.location.pathname.replace('en/', 'ar/') : window.location.pathname = '/ar' + window.location.pathname;
	//}
} else {
	culture = 'en-ae'
	$.cookie('_culture', 'en-ae', { expires: 365 });
}
//#region Change Url Path if lang and region is not define

//	//Check_Url();

//#endregion

if (culture.includes('-')) {
	lang = culture.split('-')[0];
}
const ServerError = lang == 'en' ? "Ooops, something went wrong.Try to refresh this page or feel free to contact us if the problem persists." : "عفوًا ، حدث خطأ ما. حاول تحديث هذه الصفحة أو لا تتردد في الاتصال بنا إذا استمرت المشكلة.";

const ServerErrorShort = lang == 'en' ? "Ooops, something went wrong. Please Try Later !" : "عفوًا ، حدث خطأ ما. يرجى المحاولة لاحقًا!";
const currency = ChangeString("AED", "درهم إماراتي");
//#endregion

//#region Layout Javascript Queries
//layout js queries
//document.querySelector('#OpenMenu').addEventListener('click', (elem) => {
//	document.querySelector('#MobileMenu').style.display = "block";
//});

//document.querySelector('#CloseMenu').addEventListener('click', (elem) => {
//	document.querySelector('#MobileMenu').style.display = "none";
//});

//document.querySelector('.OpenLang').addEventListener('click', (elem) => {

//});

function OpenLang(elem) {

	let language = $(elem).next('.CloseLang');
	if (language && $(language).is(":visible")) {
		$(language).hide();
		$(elem).find('svg').removeClass('rotate-180');
	}
	else {
		$(language).show();
		$(elem).find('svg').addClass('rotate-180');
	}
}

//const navbar = document.getElementById('mobile_menu'); //sm-screen

//const navbar2 = document.getElementById('desktop_menu'); //lg-screen

//ScrollDown();

//window.onscroll = () => {
//	ScrollDown();
//};

////function ScrollDown() {
//	let scroll_height = window.outerWidth > 767 ? 70 : 50;
//	if (title !== "home") {
//		if (window.scrollY > scroll_height) {
//			navbar.classList.add('bg-white', 'border-b-2', 'border-b-[#c59e6e8f]', 'desktop_menu');
//			navbar2.classList.add('bg-white', 'border-b-2', 'border-b-[#c59e6e8f]', 'mobile_menu');

//			navbar.classList.remove('relative');
//			navbar2.classList.remove('relative');

//			navbar.classList.add('sticky');
//			navbar2.classList.add('sticky');
//		} else {
//			navbar.classList.remove('bg-white', 'border-b-2', 'border-b-[#c59e6e8f]', 'desktop_menu');
//			navbar2.classList.remove('bg-white', 'border-b-2', 'border-b-[#c59e6e8f]', 'mobile_menu');
//			navbar.classList.add('relative');
//			navbar2.classList.add('relative');

//			navbar.classList.remove('sticky');
//			navbar2.classList.remove('sticky');
//		}
//	} else {
//		//home page
//		navbar.classList.add('bg-white', 'border-b-2', 'border-b-[#c59e6e8f]');
//		navbar2.classList.add('bg-white', 'border-b-2', 'border-b-[#c59e6e8f]');

//		if (window.scrollY > scroll_height) {
//			navbar.classList.remove('relative');
//			navbar2.classList.remove('relative');

//			navbar.classList.add('sticky');
//			navbar2.classList.add('sticky');
//		} else {
//			navbar.classList.add('relative');
//			navbar2.classList.add('relative');

//			navbar.classList.remove('sticky');
//			navbar2.classList.remove('sticky');
//		}
//	}
//}

//#endregion

//#region document ready function
$(document).ready(function () {

	//$('#message').click(function () { $(this).html('').hide() });

	//GetAndBindBusinessSetting();

	//#region Language change submit function
	//$('#wo-lang').change(function () {
	//	$('.ReturnUrl').val(window.location.pathname);
	//	$("#SetCulture").submit();
	//});
	//#endregion

	//if (lang == "ar") {
	//	$('#GTranslate_Select').val('en|ar').trigger('change')
	//}
	//else {
	//	$('#GTranslate_Select').val('en|en').trigger('change')
	//}

	//var prComapre = localStorage.getItem("carCompareArray");
	//if (prComapre != "[0]" && prComapre != "[]" && prComapre != "[null]") {
	//	CarsCompareArray = JSON.parse(localStorage.getItem("carCompareArray"));
	//}

	//RefreshCarCompareCount(CarsCompareArray);

	//OnErrorImage(1.5);  //after 1.5 seconds, this function checks every image on-page, if the image's broken, then the default image would be shown.
	//OnErrorImage(6);    //after 6 seconds, Recheck Images

	//#region Rating Work
	//$(".rating-star-custom i").hover(
	//	function () {
	//		$(this).addClass("filled").prevAll(".fa-star").addClass("filled");
	//	}, function () {
	//		$(".rating-star-custom i").removeClass("filled");
	//	}
	//);
	//$(".rating-star-custom i").click(function () {
	//	$(".rating-star-custom i").removeClass("selected");
	//	$(this).addClass("filled selected").prevAll(".fa-star").addClass("filled selected");
	//});
	//#endregion

	//#region Feedback Work
	//$('#Feedback-Form').submit(function () {

	//	var form = $(this);
	//	ButtonDisabled('#feedback-submit-btn', true, true);
	//	ButtonDisabled('#feedback-close-btn', true, false);
	//	var Rating = $('#Feedback-Rating i.selected').length;
	//	$('#FeedbackRating').val(parseFloat(Rating));

	//	if (Rating && Rating > 0) {
	//		ShowFormAlert(form, 'dark', ChangeString('Please wait ...', 'ارجوك انتظر ...'), 3);
	//		$.ajax({
	//			url: $(form).attr('action'),
	//			type: 'Post',
	//			data: $(form).serialize(),
	//			success: function (response) {
	//				if (response.success) {
	//					ButtonDisabled('#feedback-submit-btn', false, false);
	//					ButtonDisabled('#feedback-close-btn', false, false);
	//					$('#Feedback-Modal').slideUp();
	//					$('#Feedback-Thankyou').slideDown();
	//				} else {
	//					ShowFormAlert(form, 'danger', response.message, 6);
	//					ButtonDisabled('#feedback-submit-btn', false, false);
	//					ButtonDisabled('#feedback-close-btn', false, false);
	//				}
	//			},
	//			error: function (e) {
	//				ShowFormAlert(form, 'danger', ServerErrorShort, 6);
	//				ButtonDisabled('#feedback-submit-btn', false, false);
	//				ButtonDisabled('#feedback-close-btn', false, false);
	//			},
	//			failure: function (e) {
	//				ShowFormAlert(form, 'danger', ServerErrorShort, 6);
	//				ButtonDisabled('#feedback-submit-btn', false, false);
	//				ButtonDisabled('#feedback-close-btn', false, false);
	//			}
	//		});
	//	}
	//	else {
	//		ShowFormAlert(form, 'danger', ChangeString("Please give appropriate rating first!", "يرجى إعطاء التصنيف المناسب أولا!"), 6);
	//		ButtonDisabled('#feedback-submit-btn', false, false);
	//		ButtonDisabled('#feedback-close-btn', false, false);
	//	}
	//	return false;
	//});
	//#endregion

	if (session) {
		/*GetAndBindNewNotificationsCount();*/
	}

	//if (direction && direction.length) {
	//	$('input').attr('dir', direction);
	//}

	

	/* 
	let googleTrans = "";
	if ($.cookie("googtrans") != "undefined") {
		googleTrans = $.cookie("googtrans");
	}
	if (lang == 'en' && googleTrans == '/en/ar')
		$('#GTranslate_Select').val('en|en').trigger('change');
	else if (lang == 'ar' && googleTrans == '/en/en')
		$('#GTranslate_Select').val('en|ar').trigger('change');
	else if (lang == 'ar' && googleTrans == '')
		$('#GTranslate_Select').val('en|ar').trigger('change');
	*/

	//$(window).resize(function () {
	//	$('#hero_section').height(window.innerHeight);
	//});

	//setTimeout(function () {
	//	if ($('body').height() < $(window).height()) {
	//		$('.render-body').height(($('.render-body').height()) + ($(window).height() - $('body').height()) + 5);
	//	}
	//}, 1000);

	setTimeout(function () {
		$('.body-loader').hide();
	}, 1000);

});
//#endregion doc ready function

//#region Ajax Call Submit Forms

function GetAndBindBusinessSetting() {
	/*Fecth and Bind Business Setting*/
	$.ajax({
		type: 'Get',
		url: `/contact-setting/`,
		success: function (response) {
			if (response.success) {
				BindBusiness(response.data);
			} else {
				console.log("GetAndBindBusinessSetting() Error.");
			}
		}
		,
		error: function (e) {
			console.log("Business Setting Error!");
		}
	});
}

function GetAndBindNewNotificationsCount() {
	$.ajax({
		type: 'Get',
		url: '/Customer/Notification/GetNewNotificationCount',
		success: function (data) {
			if (data.success) {
				$('.web-alerts-count').text(data.data);
			}
		}
	});
}

//#endregion

//#region Functions for Binding Data

function BindBusiness(data) {

	//#region Social Links

	//if ((data.Facebook && data.Facebook != '-')
	//	|| (data.Instagram && data.Instagram != '-')
	//	|| (data.Youtube && data.Youtube != '-')
	//	|| (data.Twitter && data.Twitter != '-')
	//	|| (data.Snapchat && data.Snapchat != '-')
	//	|| (data.LinkedIn && data.LinkedIn != '-')
	//	|| (data.Behance && data.Behance != '-')
	//	|| (data.Pinterest && data.Pinterest != '-')) {
	//	$('.web-social-icons').append(`<li><span>${lang == "en" ? "Follow Us:" : "تابعنا:"}</span></li>`);
	//}
	if (data.Facebook && data.Facebook != '-') {
		$('.web-social-icons').append(`<a target="_blank" href="${data.Facebook}" class="mr-2">
										<svg class="w-5 h-5"
										  xmlns="http://www.w3.org/2000/svg">
										  <image href="/Assets/images/social-icons/facebook.png" class="w-5 h-5"/>
										</svg>
									</a>`);
	}
	if (data.Youtube && data.Youtube != '-') {
		$('.web-social-icons').append(`<a target="_blank" href="${data.Youtube}" class="mr-2">
										<svg class="w-5 h-5"
										  xmlns="http://www.w3.org/2000/svg">
										  <image href="/Assets/images/social-icons/youtube.png" class="w-5 h-5"/>
										</svg>
									</a>`);
	}
	if (data.Twitter && data.Twitter != '-') {
		$('.web-social-icons').append(`<a target="_blank" href="${data.Twitter}" class="mr-2">
										<svg class="w-5 h-5"
										  xmlns="http://www.w3.org/2000/svg">
										  <image href="/Assets/images/social-icons/twitter.png" class="w-5 h-5"/>
										</svg>
									</a>`);
	}
	if (data.LinkedIn && data.LinkedIn != '-') {
		$('.web-social-icons').append(`<a target="_blank" href="${data.LinkedIn}" class="mr-2">
										<svg class="w-5 h-5"
										  xmlns="http://www.w3.org/2000/svg">
										  <image href="/Assets/images/social-icons/linkedin.png" class="w-5 h-5"/>
										</svg>
									</a>`);
	}
	if (data.Instagram && data.Instagram != '-') {
		$('.web-social-icons').append(`<a target="_blank" href="${data.Instagram}" class="mr-2">
										<svg class="w-5 h-5"
										  xmlns="http://www.w3.org/2000/svg">
										  <image href="/Assets/images/social-icons/instagram.png" class="w-5 h-5"/>
										</svg>
									</a>`);
	}
	if (data.Snapchat && data.Snapchat != '-') {
		$('.web-social-icons').append(`<a target="_blank" href="${data.Snapchat}" class="mr-2">
										<svg class="w-5 h-5"
										  xmlns="http://www.w3.org/2000/svg">
										  <image href="/Assets/images/social-icons/snapchat.png" class="w-5 h-5"/>
										</svg>
									</a>`);
	}
	if (data.Behance && data.Behance != '-') {
		$('.web-social-icons').append(`<a target="_blank" href="${data.Behance}" class="mr-2">
										<svg class="w-5 h-5"
										  xmlns="http://www.w3.org/2000/svg">
										  <image href="/Assets/images/social-icons/behance.png" class="w-5 h-5"/>
										</svg>
									</a>`);
	}
	if (data.Pinterest && data.Pinterest != '-') {
		$('.web-social-icons').append(`<a target="_blank" href="${data.Pinterest}" class="mr-2">
									<svg class="w-5 h-5"
										xmlns="http://www.w3.org/2000/svg">
										<image href="/Assets/images/social-icons/pinterest.png" class="w-5 h-5"/>
									</svg>
									</a>`);
	}
	//#endregion Social Links ends

	//#region Contact Us
	if (data.Contact && data.Contact != '-') {
		//$('.web-contact-info').append(`	<li class="${lang == "en" ? "" : "text-right"}">
		//									<a href="tel:+${data.Contact}"><i class="fa fa-phone-alt ${lang == "en" ? "" : "fa-flip-horizontal"} "></i>
		//										<bdi>${data.Contact}</bdi>
		//									</a>
		//								</li>`);

		$('.phone-link').attr("href", `tel:${data.Contact}`);

	}
	//if (data.StreetAddress && data.StreetAddress != '-') {
	//	$('.web-contact-info').append(`	<li class="${lang == "en" ? "" : "text-right"}">
	//										<address><i class="fa fa-map-marker-alt"></i>${lang == "en" ? data.StreetAddress : " " + data.StreetAddressAr}</address>
	//									</li>`);
	//}
	//if (data.Fax) {
	//	$('#contact_footer').append(`<li class="fax">
	//                                       <i class="icon anm anm-fax"></i><p id="contact_fax">${data.Fax}</p>
	//                                   </li>`);
	//}

	//if (data.Contact2) {
	//	$('#contact_footer').append(`<li class="phone">
	//                                       <i class="icon anm anm-phone-s"></i><p id="contact_phone2">${data.Contact2}</p>
	//                                   </li>`);
	//}
	if (data.Email && data.Email != '-') {
		InfoEmail = data.Email;
		$('.mail-link').attr("href", `mailto:${data.Email}`);
		//mailto: info % 40danubeproperties % C2 % B7ae ? subject = Danube % 20Properties % 20 -% 20Inquiry % 20!

		//$('#contact_footer').append(`<li class="email">
		//                                      <i class="icon anm anm-envelope-l"></i><p id="contact_email">${data.Email}</p>
		//                                  </li>`);


	}
	//#endregion Contact Us

	//#region Whatsapp
	if (data.Whatsapp && data.Whatsapp != '-') {
		//$('#whatsapp_link').html(`<i class="fab fa-whatsapp"></i> ${lang == "en" ? data.Title : data.TitleAr}`)
		$('.whatsapp-link').attr("href", `https://api.whatsapp.com/send?phone=${data.Whatsapp}&text=${encodeURIComponent(lang == "en" ? data.FirstMessage : data.FirstMessageAr)}`)
	} else {
		//$('#whatsapp_link').remove();
	}
	//#endregion

	//#region Others Checks
	//IsLoyaltyEnabled = data.IsLoyaltyEnabled;

	//if (!IsLoyaltyEnabled) {
	//    $('.redeem-amount-container').remove();
	//}
	//if (!data.IsMaruCompare) {
	//    $('.MaruCompareBanner').remove();
	//}
	//else {
	//    $('.MaruCompareBanner').show();
	//
	//#endregion
}

//#endregion

//#region Others Function

function BindCoupons(data) {

	$('#couponpromotion').empty()
	$.each(data, function (k, v) {
		//'<marquee behavior="scroll" direction="left" scrollamount="10"><div class=""><i class="fa fa-gift"></i> ' + v.name + '</div></marquee>'
		$('#couponpromotion').append('				<i class="fa fa-gift"></i> ' + v.name);
	});
}

function FormatPrices() {
	$('span.price,span.old-price,.money').each(function (k, v) {
		if (!$(v).hasClass('formatted')) {
			let text = $(v).text();
			if (text.includes('-')) {
				text = text.split('-');
				let min = text[0].replace('AED', '').trim();
				let max = text[1].replace('AED', '').trim();

				$(v).html('AED ' + numeral(min).format('0,0.00') + ' - AED ' + numeral(max).format('0,0.00'));
			} else {
				if (text.includes('AED')) {
					text = text.replace('AED', '').trim();
					$(v).html('AED ' + numeral(text).format('0,0.00'));
				} else {
					$(v).html(numeral(text).format('0,0.00'));
				}
			}
			$(v).addClass('formatted');
		}
	});

	$('span.points').each(function (k, v) {
		if (!$(v).hasClass('formatted')) {
			let text = $(v).text();

			$(v).html(numeral(text).format('0,0'));

			$(v).addClass('formatted');
		}
	});
}

function FormatPrice(v) {
	let text = $(v).text();
	if (text.includes('-')) {
		text = text.split('-');
		let min = text[0].replace('AED', '').trim();
		let max = text[1].replace('AED', '').trim();

		$(v).html('AED ' + numeral(min).format('0,0.00') + ' - AED ' + numeral(max).format('0,0.00'));
	} else {
		if (text.includes('AED')) {
			text = text.replace('AED', '').trim();
			$(v).html('AED ' + numeral(text).format('0,0.00'));
		} else {
			$(v).html(numeral(text).format('0,0.00'));
		}
	}
}

function FormatAmount(v, hasDoublePrice = true) {
	let text = $(v).text();
	if (hasDoublePrice && text.includes('-')) {
		text = text.split('-');
		let min = text[0].replace(currency, '').trim();
		let max = text[1].replace(currency, '').trim();

		$(v).html(`${currency} ${numeral(min).format('0,0.00')} - ${currency} ${numeral(max).format('0,0.00')}`);
	} else {
		if (text.includes(currency)) {
			text = text.replace(currency, '').trim();
			$(v).html(`${currency} ${numeral(text).format('0,0.00')}`);
		} else {
			$(v).html(numeral(text).format('0,0.00'));
		}
	}
}

function FormatPoint(v) {
	let text = $(v).text();
	$(v).html(numeral(text).format('0,0'));
}

function addScore(score, $domElement) {
	$("<span class='stars-container'>")
		.addClass("stars-" + score.toString())
		.text("★★★★★")
		.appendTo($domElement);
}

function ChangeString(en_text, ar_text) {
	return lang == 'en' ? en_text : ar_text;
};
//#region Feedback and Suggestions

function ShowFeedbackView() {
	$('#Feedback-Modal').show();
	$('#Feedback-Thankyou').hide();
	$('.feedback-fields').val('');
	$(".rating-star-custom i").removeClass("selected");
	$('#FeedbackRating').val(0);
	//DisableEmojis();

	if (session) {
		$('#FeedbackName').attr('readonly', true).val($('.customer-name').val());
		$('#FeedbackEmail').attr('readonly', true).val($('.customer-email').val());
	}
	else {
		$('#FeedbackName').attr('readonly', false).val('');
		$('#FeedbackEmail').attr('readonly', false).val('');
	}
	$('#feedbackpopup').modal('show');
}

$('.feedback-close-btn').click(function () {
	$('#feedbackpopup').modal('hide');
	$('.feedback-fields').val('');
	$(".rating-star-custom i").removeClass("selected");
	$('#FeedbackRating').val(0);

	$('#Feedback-Modal').slideDown();
	$('#Feedback-Thankyou').slideUp();
	//DisableEmojis();
});

$('#btn-become-a-vendor').click(function () {
	$('#Vendor-Rendor').html(`
            <div class="text-center">
				<span class="fa fa-circle-notch fa-spin fa-2x"></span>
			</div>
            `);
	$('#Vendor-Rendor').load(`/Vendors/Signup`);
	$('#vendorpopup').modal('show');
});

//function HoverEmoji(elem) {
//	if ($(elem).attr('src').includes('disable')) {
//		var src = $(elem).attr('src').replace('disable', 'active');
//		$(elem).attr('src', src);
//	}
//}

//function HoverEmojiOut(elem) {
//	if ($(elem).attr('src').includes('active') && $(elem).attr('data-id') == 0) {
//		var src = $(elem).attr('src').replace('active', 'disable');
//		$(elem).attr('src', src);
//	}
//}

//$('.feedback-emoji-icon').click(function () {
//	var emoji = $(this);
//	if (DisableEmojis()) {
//		var src = $(emoji).attr('src').replace('disable', 'active');
//		$(emoji).attr('src', src).attr('data-id', '1');
//		$('#FeedbackExperience').val($(emoji).attr('alt'));
//	}
//});

//var DisableEmojis = function disableEmojis() {
//	$.each($('.feedback-emoji-icon'), function (k, v) {
//		if ($(v).attr('src').includes('active')) {
//			var src = $(v).attr('src').replace('active', 'disable');
//			$(v).attr('src', src).attr('data-id', '0');
//		}
//	});
//	return true;
//}
//#endregion

//$(window).scroll(function () {
//	if ($(this).scrollTop() > 300) {
//		$("#site-scroll").fadeIn();
//	} else {
//		$("#site-scroll").fadeOut();
//	}
//});

var ShowFormAlert = function (form, type, msg, timeup = 0) {
	var alert = $(`
					<div class= "alert alert-${type} ${lang == "en" ? "text-left" : "text-right"}" role = "alert">
						<button type="button" class="close" data-dismiss="alert" aria-label="Close">
							<i class="fa fa-times small"></i>
						</button>
						<span></span>
					</div>
				`);
	form.find('.alert').remove();
	alert.prependTo(form);
	$(alert).slideDown();
	alert.find('span').html(msg);

	if (timeup && timeup > 0) {
		setTimeout(function () {
			$(alert).slideUp();
		}, (timeup * 1000));
	}
}

var HideFormAlert = function (type) {
	var alert = $(`
					<div class= "alert alert-${type} ${lang == "en" ? "text-left" : "text-right"}" role = "alert">
						<button type="button" class="close" data-dismiss="alert" aria-label="Close">
							<i class="fa fa-times small"></i>
						</button>
						<span></span>
					</div>
				`);

	$(alert).slideUp();
}

var ShowFormAlertOffset = function (form, type, msg, offsetElem, timeup = 0) {

	var alert = $(`
					<div class= "alert alert-${type}" role = "alert">
						<button type="button" class="close" data-dismiss="alert" aria-label="Close">
							<i class="fa fa-times small"></i>
						</button>
						<span></span>
					</div>
				`);

	form.find('.alert').remove();
	if (offsetElem) {
		offsetElem.after(alert);
	}
	else {
		alert.prependTo(form);
	}
	$(alert).slideDown();
	alert.find('span').html(msg);
	if (timeup && timeup > 0) {
		setTimeout(function () {
			$(alert).slideUp();
		}, (timeup * 1000));
	}
}

if ($('#SuccessMessage').val()) {
	//SlideDownToasterMessage('<span class="fa fa-check-circle ' + margin(1) + ' "></span>', $('#SuccessMessage').val(), 10);
	ShowSweetAlert('success', ChangeString("Ok!", "حسنا!"), $('#SuccessMessage').val(), '#40194e');
}
if ($('#ErrorMessage').val()) {
	//SlideDownToasterMessage('<span class="fa fa-check-circle ' + margin(1) + ' "></span>', $('#ErrorMessage').val(), 10);
	ShowSweetAlert('error', ChangeString("Oops...", "وجه الفتاة..."), $('#ErrorMessage').val(), '#40194e');
}

function ShowSweetAlert(Icon, Title, Text, cnfrm_btn_color, callbackFunction) {
	Swal.fire({
		icon: Icon,
		title: Title,
		text: Text,
		confirmButtonColor: cnfrm_btn_color,
	});
}

function ShowSweetAlertBtnText(Icon, Title, Text, cnfrm_btn_color, cnfrm_btn_text, callbackFunction) {
	Swal.fire({
		icon: Icon,
		title: Title,
		text: Text,
		confirmButtonColor: cnfrm_btn_color,
		confirmButtonText: cnfrm_btn_text
	});
}

function OnErrorImage(timeup = 3, type = 'all') {
	setTimeout(function () {

		if (type == 'all' || type == 'blog') {
			$('.img-blog').each(function () {
				var $this = $(this),
					src = $this.attr('src');

				var img = new Image();
				img.onload = function () {
					$this.attr('src', src);
				}
				img.onerror = function () {
					$this.attr('src', DefaultBlogImageUrl);
				}
				img.src = src;
			});
		}
		if (type == 'all' || type == 'service') {
			$('.img-service').each(function () {
				var $this = $(this),
					src = $this.attr('src');

				var img = new Image();
				img.onload = function () {
					$this.attr('src', src);
				}
				img.onerror = function () {
					$this.attr('src', DefaultBlogImageUrl);
				}
				img.src = src;
			});
		}
		if (type == 'all' || type == 'contact') {
			$('.img-contact').each(function () {
				var $this = $(this),
					src = $this.attr('src');

				var img = new Image();
				img.onload = function () {
					$this.attr('src', src);
				}
				img.onerror = function () {
					$this.attr('src', DefaultContactImageUrl);
				}
				img.src = src;
			});
		}
		if (type == 'all' || type == 'event') {
			$('.img-event').each(function () {
				var $this = $(this),
					src = $this.attr('src');

				var img = new Image();
				img.onload = function () {
					$this.attr('src', src);
				}
				img.onerror = function () {
					$this.attr('src', DefaultEventImageUrl);
				}
				img.src = src;
			});
		}
		if (type == 'all' || type == 'newsfeed') {
			$('.img-newsfeed').each(function () {
				var $this = $(this),
					src = $this.attr('src');

				var img = new Image();
				img.onload = function () {
					$this.attr('src', src);
				}
				img.onerror = function () {
					$this.attr('src', DefaultNewsfeedImageUrl);
				}
				img.src = src;
			});
		}
		if (type == 'all' || type == 'property') {
			$('.img-property').each(function () {
				var $this = $(this),
					src = $this.attr('src');

				var img = new Image();
				img.onload = function () {
					$this.attr('src', src);
				}
				img.onerror = function () {
					$this.attr('src', DefaultPropertyImageUrl);
				}
				img.src = src;
			});
		}
		if (type == 'all' || type == 'unit') {
			$('.img-unit').each(function () {
				var $this = $(this),
					src = $this.attr('src');

				var img = new Image();
				img.onload = function () {
					$this.attr('src', src);
				}
				img.onerror = function () {
					$this.attr('src', DefaultUnitImageUrl);
				}
				img.src = src;
			});
		}
		if (type == 'all' || type == 'feature') {
			$('.img-feature').each(function () {
				var $this = $(this),
					src = $this.attr('src');

				var img = new Image();
				img.onload = function () {
					$this.attr('src', src);
				}
				img.onerror = function () {
					$this.attr('src', DefaultPropertyFeaturesUrl);
				}
				img.src = src;
			});
		}

		//$('img:not(.img-avatar)').each(function () {
		//    var $this = $(this),
		//        src = $this.attr('src');

		//    var img = new Image();
		//    img.onload = function () {
		//        $this.attr('src', src);
		//    }
		//    img.onerror = function () {
		//        $this.attr('src', DefaultImageUrl);
		//    }
		//    img.src = src;
		//});

		//$('.img-avatar').each(function () {
		//    var $this = $(this),
		//        src = $this.attr('src');

		//    var img = new Image();
		//    img.onload = function () {
		//        $this.attr('src', src);
		//    }
		//    img.onerror = function () {
		//        $this.attr('src', DefaultAvatarUrl);
		//    }
		//    img.src = src;
		//});

	}, (timeup * 1000));
}

function Check_Url() {

	if (window.location.pathname.toLowerCase().includes('/en-us/') || window.location.pathname.toLowerCase().includes('/ar-us/')) {
		if (window.location.pathname.toLowerCase().includes('/en-us/') && CheckUrl == true) {
			CheckUrl = false;
			window.location.pathname = window.location.pathname.replace('/en-us/', '/en-ae/');
		}
		else if (window.location.pathname.toLowerCase().includes('/ar-us/') && CheckUrl == true) {
			CheckUrl = false;
			window.location.pathname = window.location.pathname.replace('/ar-us/', '/ar-ae/');
		}
	}

	if (window.location.pathname.toLowerCase().includes('/en/') || window.location.pathname.toLowerCase().includes('/ar/')) {
		if (window.location.pathname.toLowerCase().includes('/en/') && CheckUrl == true) {
			CheckUrl = false;
			window.location.pathname = window.location.pathname.replace('/en/', '/en-ae/');
		}
		else if (window.location.pathname.toLowerCase().includes('/ar/') && CheckUrl == true) {
			CheckUrl = false;
			window.location.pathname = window.location.pathname.replace('/ar/', '/ar-ae/');
		}
	}
	if (window.location.pathname.toLowerCase().includes('/en-/') || window.location.pathname.toLowerCase().includes('/ar-/')) {
		if (window.location.pathname.toLowerCase().includes('/en-/') && CheckUrl == true) {
			CheckUrl = false;
			window.location.pathname = window.location.pathname.replace('/en-/', '/en-ae/');
		}
		else if (window.location.pathname.toLowerCase().includes('/ar-/') && CheckUrl == true) {
			CheckUrl = false;
			window.location.pathname = window.location.pathname.replace('/ar-/', '/ar-ae/');
		}
	}
	else if ((window.location.pathname.toLowerCase().includes('/en-ae') && window.location.search.includes('returnUrl')) || window.location.pathname.toLowerCase().includes('/ar-ae/') && window.location.search.includes('returnUrl')) {
		if (CheckUrl == true) {
			if (window.location.pathname.toLowerCase() == "/en-ae/home" || window.location.pathname.toLowerCase() == "/ar-ae/home") {
			}
			else {
				CheckUrl = false;
				window.location.pathname = "/" + culture + "/home";
			}
		}
	}
	else if (!window.location.pathname.toLowerCase().includes('/en-ae/') && !window.location.pathname.toLowerCase().includes('/ar-ae/')) {
		if (CheckUrl == true) {
			CheckUrl = false;
			window.location.pathname = "/" + culture + window.location.pathname;
		}
	}

	if (location.hash && !session) {
		location.hash = '';
	}

	//Default_Url();
}

function Default_Url() {
	window.location.pathname = '/home/index';
}

function getParameterByName(name, url = window.location.href) {
	name = name.replace(/[\[\]]/g, '\\$&');
	var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
		results = regex.exec(url);
	if (!results) return null;
	if (!results[2]) return '';
	return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

function PopulateSchedule() {
	$('#StartDate,#EndDate').off('change');

	$('#StartTime,#EndTime').clockTimePicker('dispose');

	var currentDate = new Date();
	var startDate = new Date($("#StartDate").val());
	var endDate = new Date($("#EndDate").val());
	var packageId = $('.car-packages .package.selected').attr('data');

	if (startDate > currentDate) {

		$('#StartTime').clockTimePicker({
			duration: true,
			precision: 60
		});

		$('#EndTime').clockTimePicker({
			duration: true,
			precision: 60
		});

		$('#StartTime').val(currentDate.getHours() + ':00');
		if (Number(packageId) === 1) {
			currentDate.addHours(1);
		}
		$('#EndTime').val(currentDate.getHours() + ':00');
	} else {
		$('#StartTime').clockTimePicker({
			duration: true,
			precision: 60,
			minimum: currentDate.addHours(3).getHours() + ':00',
		});
		$('#StartTime').val(currentDate.getHours() + ':00');
		if (Number(packageId) === 1) {
			currentDate.addHours(1);
		}
		$('#EndTime').clockTimePicker({
			duration: true,
			precision: 60,
			minimum: currentDate.getHours() + ':00',
		});
		$('#EndTime').val(currentDate.getHours() + ':00');
	}

	switch (Number(packageId)) {
		case 1:
			$("#EndDate").datepicker("setStartDate", startDate);
			$('#EndDate').datepicker('setDate', startDate);

			break;
		case 2:

			var diffInMilliSeconds = Math.abs(endDate - startDate) / 1000;
			// calculate days
			var days = Math.floor(diffInMilliSeconds / 86400);
			days = days <= 0 ? 1 : days;

			startDate = startDate.addDays(days)
			$("#EndDate").datepicker("setStartDate", startDate);
			$('#EndDate').datepicker('setDate', startDate);


			break;
		case 3:

			var diffInMilliSeconds = Math.abs(endDate - startDate) / 1000;
			// calculate days
			var days = Math.floor(diffInMilliSeconds / 86400);
			days = days <= 0 ? 1 : days;

			if (days % 7 !== 0) {
				var daysToAdd = days + (7 - (days % 7))

				startDate = startDate.addDays(daysToAdd)
				$("#EndDate").datepicker("setStartDate", startDate);
				$('#EndDate').datepicker('setDate', startDate);
			}

			break;
		case 4:

			var months;
			months = (endDate.getFullYear() - startDate.getFullYear()) * 12;
			months -= startDate.getMonth();
			months += endDate.getMonth();
			months = months <= 0 ? 1 : months;

			startDate.setMonth(startDate.getMonth() + months);
			$("#EndDate").datepicker("setStartDate", startDate);
			$('#EndDate').datepicker('setDate', startDate);

			break;
		default:
	}

	$('#StartDate,#EndDate').change(function (event) {
		event.preventDefault()
		PopulateSchedule();
	});

	if (typeof CalulateTotal !== "undefined") {
		CalulateTotal();
	}
}

Date.prototype.addDays = function (days) {
	var date = new Date(this.valueOf());
	date.setDate(date.getDate() + days);
	return date;
}

Date.prototype.addHours = function (h) {
	this.setTime(this.getTime() + (h * 60 * 60 * 1000));
	return this;
}
//#endregion
