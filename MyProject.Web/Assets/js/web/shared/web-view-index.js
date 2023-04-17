'use strict';

//#region Global Variables and Arrays
let culture = 'en-ae';
let lang = 'en';
let InfoEmail = "info@KAVAAN.com";
let CheckUrl = true;

if ($.cookie("_culture") != "undefined") {
	culture = $.cookie("_culture");
} else {
	culture = 'en-ae'
	$.cookie('_culture', 'en-ae', { expires: 365 });
}
//#region Change Url Path if lang and region is not define
Check_Url();
//#endregion

if (culture.includes('-')) {
	lang = culture.split('-')[0];
}
const ServerError = lang == 'en' ? "Ooops, something went wrong.Try to refresh this page or feel free to contact us if the problem persists." : "عفوًا ، حدث خطأ ما. حاول تحديث هذه الصفحة أو لا تتردد في الاتصال بنا إذا استمرت المشكلة.";

const ServerErrorShort = lang == 'en' ? "Ooops, something went wrong. Please Try Later !" : "عفوًا ، حدث خطأ ما. يرجى المحاولة لاحقًا!";
const currency = ChangeString("AED", "درهم إماراتي");
//#endregion

function changeCulture(elem, lng) {
	/* 
	if (lang == 'en')
		$('#GTranslate_Select').val('en|ar').trigger('change');
	else
		$('#GTranslate_Select').val('en|en').trigger('change');
	*/
	var form = $(elem).closest('form.culture-form');
	$(form).find('input[name="ReturnUrl"]').val(window.location.pathname);
	$(form).find('input[name="culture"]').val(lng);

	$(form).submit();
	//$('body').css('opacity', '0.5');

	//setTimeout(function () {
	//}, 1500);
}
//#endregion

//#region document ready function
$(document).ready(function () {

	$('#message').click(function () { $(this).html('').hide() });

	setTimeout(function () {
		$('.body-loader').hide();
	}, title == 1000);

});
//#endregion


function ChangeString(en_text, ar_text) {
	return lang == 'en' ? en_text : ar_text;
};

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