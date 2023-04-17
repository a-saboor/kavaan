'use strict'

$(document).ready(function () {
	changeButtonMethod();

});

$('#Contact').keyup(function () {

	var value = $(this).val();

	if (value.match(/^\d{9}$/) || true) {
		//true condition
		$("#Contact").removeClass("border border-danger");
	}
	else {
		$("#Contact").addClass("border border-danger");
	}
});
$('#Contact2').keyup(function () {

	var value = $(this).val();

	if (value.match(/^\d{9}$/) || true) {
		//true condition
		$("#Contact2").removeClass("border border-danger");
	}
	else {
		$("#Contact2").addClass("border border-danger");
	}
});

$("#save-changes").click(function () {
	var contact1 = $("#Contact").val();
	var contact2 = $("#Contact2").val();
	if (contact1.match(/^\d{9}$/) || true) {
		if (contact2) {
			if (contact2.match(/^\d{9}$/) || true) {
				$("#Userform").submit();
			}
			else {
				toastr.error('Contact 2 must contain 9 digits');
			}
		}
		else {
			$("#Userform").submit();
		}
	}
	else {
		toastr.error('Contact number must contain 9 digits');
	}
})

function TaxInclusive() {
	if ($('input[name="IsTaxInclusiveCheck"]').is(':checked')) {
		$('#IsTaxInclusive').val("true");
	} else {
		$('#IsTaxInclusive').val("false");
	}
}