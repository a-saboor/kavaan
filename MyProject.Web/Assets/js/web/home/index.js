'use strict'

//#region Global Variables and Arrays
//#endregion

//#region document ready function
$(document).ready(function () {

});
//#endregion

//#region Ajax Call
function getServiceCategories() {

	$.ajax({
		type: 'GET',
		url: '/categories/get-all',
		contentType: "application/json",
		success: function (response) {

			//bind categories dropdown
			bindDropdown(response.data);
			//bind categories slider
			bindCategorySlider(response.data);
		}
	});

	function bindDropdown(data) {
		$.each(data, function (k, v) {
			$('#service_selectbox').append($("<option />").val(v.Slug).text(v.Title));
		});
		initSelect2('#service_selectbox');
	}
}

function getServicesByCategory(categoryID) {

	var filter = {
		search: null,
		PageSize: 12,
		pageNumber: 1,
		sortBy: 1,
		parentID: categoryID
	}

	$.ajax({
		type: 'POST',
		url: '/services/filters',
		contentType: "application/json",
		data: JSON.stringify(filter),
		success: function (response) {
			const data = response.data;
			bindServicesSlider(data.sort(random_sort));
		}
	});
}

//#endregion

//#region other functions

//#endregion

//#region slider functions

//#endregion
