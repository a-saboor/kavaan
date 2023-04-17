'use strict'

//#region Global Variables and Arrays

//#endregion

//#region document ready function
$(document).ready(function () {
	let description = $('#description').val();
	if (description.includes("\n")) {
		description = description.split('\n');
		$.each(description, function (k, v) {
			$('#Description').append(`<p class="text-xs text-justify font-medium pb-1">${v}</p>`)
		});
	}
	else {
		$('#Description').html(description);
	}

});
//#endregion

//#region Ajax Call

//#endregion

//#region Functions for Binding Data

//#endregion
