'use strict'

//#region Global Variables and Arrays

//#endregion

//#region document ready function
$(document).ready(function () {
	$('#search_button').click(function () {
		SearchQuery(this);
	});

	$('#search_query').change(function () {
		SearchQuery(this);
	});

	function SearchQuery(elem) {
		//var query = $.trim($(btn).prevAll('#search_query').val()).toLowerCase();
		var query = $('#search_query').val().toLowerCase();

		$('.search-elem').each(function () {
			var elem = $(this);
			var text = $(elem).find('.p-1st').text().toLowerCase();
			text = text + " " + $(elem).find('.p-2nd').text().toLowerCase();
	
			if (text.indexOf(query) === -1)
				elem.closest('.w-full').fadeOut();
			else elem.closest('.w-full').fadeIn();
		});
	}
});
//#endregion

//#region Ajax Call

//#endregion

//#region Functions for Binding Data

//#endregion
