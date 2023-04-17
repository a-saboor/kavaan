'use strict'

$(document).ready(function () {
	$('#EmailTemplate').change(function () {
		if ($(this).val()) {
			$('#iframe').attr('src', '/Assets/EmailTemplates/' + $(this).val() + '.html');
		}
		else {
			$('#iframe').attr('src', '/Assets/EmailTemplates/TestEmail.html');
		}
	});
	changeButtonMethod();

});

function showEmail(btn, flag) {
	if (flag) {
		$('#kt_page_sticky_card2').slideDown();
	}
	else {
		$('#kt_page_sticky_card2').slideUp();
	}
}
