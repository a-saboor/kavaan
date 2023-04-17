'use strict'

//#region Global Variables and Arrays
var pg = 1;
var PageSize = 15;
var isPageRendered = false;
var totalPages;
//var lang = "en";

var filter = {
	search: null,
	PageSize: 15,
	pageNumber: 1,
	sortBy: 1,
}

//#endregion

//#region document ready function
$(document).ready(function () {

	$('#projects_section .main-div').empty().show();

	GetFilterValues();
	GetArticles();

	$('#ArticleSearch').change(function () {
		if ($('#ArticleSearch').val()) {
			$('#projects_section .main-div').empty();
			pg = 1;
			GetFilterValues();
			GetArticles();
		}
	});

	$('#btnSearch').click(function () {
		$('#projects_section .main-div').empty();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#sortBy').change(function () {
		$('#projects_section .main-div').empty();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#projects_section .see-more').click(function () {
		//if (pg < totalPages) {
		pg++;
		$('#projects_section .see-more').hide();
		$(".filter-loader").show();
		GetFilterValues();
		GetArticles();
		//}
	});


});
//#endregion

//#region Ajax Call

function GetArticles() {
	$.ajax({
		type: 'POST',
		url: '/projects/filters',
		contentType: "application/json",
		data: JSON.stringify(filter),
		success: function (response) {
			BindGridArticles(response.data);
		}
	});
}

//#endregion

//#region Functions for Binding Data

function BindGridArticles(data) {

	$.each(data, function (k, v) {

		$('#projects_section .main-div').append(`
			<a href="/projects-progress/${v.ID}" class="mb-4 w-full sm:w-[46.5%] lg:w-[17.5%] mx-2">
				<img src="${v.Image}" alt="" class="rounded-lg mb-2 w-full h-[15rem] object-cover">
				<p class="font-Rubik text-xs uppercase">${v.Title}</p>
			</a>
		`);

	});

	//setTimeout(function () { OnErrorImage(); }, 3000);

	if (data && data.length >= PageSize) {
		$("#projects_section .see-more").fadeIn();
	} else {
		$("#projects_section .see-more").fadeOut();
	}
	
	if ($('#projects_section .main-div').html().length == 0) {
		$("html, body").animate({ scrollTop: 0 }, 1000);
		$('#projects_section .no-more').fadeIn();
		$("#projects_section .see-more").fadeOut();
	}
}

function GetFilterValues() {
	filter.search = $("#ArticleSearch").val();
	filter.pageNumber = pg;
	filter.sortBy = $("#sortBy").val();
}

function UpdateQueryString(key, value, url) {
	if (!url) url = window.location.href;
	var re = new RegExp("([?&])" + key + "=.*?(&|#|$)(.*)", "gi"),
		hash;

	if (re.test(url)) {
		if (typeof value !== 'undefined' && value !== null) {
			return url.replace(re, '$1' + key + "=" + value + '$2$3');
		}
		else {
			hash = url.split('#');
			url = hash[0].replace(re, '$1$3').replace(/(&|\?)$/, '');
			if (typeof hash[1] !== 'undefined' && hash[1] !== null) {
				url += '#' + hash[1];
			}
			return url;
		}
	}
	else {
		if (typeof value !== 'undefined' && value !== null) {
			var separator = url.indexOf('?') !== -1 ? '&' : '?';
			hash = url.split('#');
			url = hash[0] + separator + key + '=' + value;
			if (typeof hash[1] !== 'undefined' && hash[1] !== null) {
				url += '#' + hash[1];
			}
			return url;
		}
		else {
			return url;
		}
	}
}

function GetURLParameter() {
	var sPageURL = window.location.href;
	var indexOfLastSlash = sPageURL.lastIndexOf("/");

	if (indexOfLastSlash > 0 && sPageURL.length - 1 != indexOfLastSlash)
		return sPageURL.substring(indexOfLastSlash + 1).replace('#', '');
	else
		return 0;
}

$("#language-selector").change(function () {
	$("select option:selected").each(function () {
		lang = $(this).val();
	});
	$('#projects_section .main-div').empty();
	GetArticles();
});

//#endregion
