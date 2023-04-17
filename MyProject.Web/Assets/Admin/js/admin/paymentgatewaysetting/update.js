
$(document).ready(function () {
	changeButtonMethod();
});


function EmailTest() {
	$('#btnEmailSend').addClass('kt-spinner kt-spinner--left kt-spinner--sm kt-spinner--light');
	$.ajax({
		url: '/Emails/TestEMail/',
		type: 'Post',
		data: {

			Email: $('#EmailSend').val()
		},
		success: function (response) {
			if (response.success) {
				toastr.success(response.message)
				$('#btnEmailSend').removeClass('kt-spinner kt-spinner--left kt-spinner--sm kt-spinner--light');
			}
			else {
				toastr.error(response.message)
				$('#btnEmailSend').removeClass('kt-spinner kt-spinner--left kt-spinner--sm kt-spinner--light');
			}
			$('#EmailSend').val('');
		}
	});


}