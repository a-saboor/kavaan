'use strict'

//#region Global Variables and Arrays
var pg = 1;
var PageSize = 8;
var isPageRendered = false;
var totalPages;
//var lang = "en";

var filter = {
	search: null,
	PageSize: 8,
	pageNumber: 1,
	sortBy: 3// Ascending by Name
}

//#endregion

//#region document ready function
$(document).ready(function () {

	GetFilterValues();
	GetArticles();

	$('#ArticleSearch').change(function () {
		if ($('#ArticleSearch').val()) {
			$('#contractors_section .main-div').empty();
			pg = 1;
			GetFilterValues();
			GetArticles();
		}
	});

	$('#btnSearch').click(function () {
		$('#contractors_section .main-div').empty();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#SortBy').change(function () {
		$('#contractors_section .main-div').empty();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#contractors_section .see-more').click(function () {
		//if (pg < totalPages) {
		pg++;
		$('#contractors_section .see-more').hide();
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
		url: '/contractors/filters',
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

	$('#contractors_section .main-div').empty().show();

	$.each(data, function (k, v) {

		$('#contractors_section .main-div').append(`
			
			${ /* /consultants/${v.ID} */''}
			<a class="flex flex-row pb-4 w-full sm:w-[49%]">
				<div class="bg-black/5 w-[25%] flex justify-center items-center rounded-l-lg drop-shadow-lg">
					<img src="${v.Image}" alt="" class="w-10 h-10 xl:w-12 xl:h-12">
				</div>
				<div class="w-[75%] flex flex-col justify-center py-4 pl-4 bg-white rounded-r-lg drop-shadow-lg">
					<h1 class="font-Rubik text-xs font-medium xl:text-base">${v.Name}</h1>
					<p class="cut-text font-Rubik text-xs pr-3 text-justify xl:pr-6">${v.Description}</p>
				</div>
			</a>

		`);

		//var template = '<div class="col-12 col-sm-12 col-md-4 col-lg-4 article">';
		//template += '			<!-- Article Image -->';
		//template += '			<a class="article_featured-image" href="/' + lang + '/news/' + v.Slug + '">';
		//template += '				<img class="blur-up lazyload img-lazyload" data-src="' + v.Cover + '" src="' + v.Cover + '" alt="' + v.Title + '">';
		//template += '			</a>';
		//template += '			<h2 class="h3">';
		//template += '				<a href="/' + lang + '/news/' + v.Slug + '">' + v.Title + '</a>';
		//template += '			</h2>';
		//template += '			<ul class="publish-detail">';

		//template += '				<li>';
		//template += '					<i class="icon anm anm-clock-r"></i> <time datetime="2017-05-02">' + v.Date + '</time>';
		//template += '				</li>';
		//template += '			</ul>';
		//template += '			<div class="rte">';
		//template += '				<p>' + v.Description + '...</p>';
		//template += '			</div>';
		//template += '			<p>';
		//template += '				<a href="/' + lang + '/news/' + v.Slug + '" class="btn btn-secondary btn--small" title="Read more">';
		//template += '					Read more';
		//template += '					<i class="fa fa-caret-right" aria-hidden="true"></i>';
		//template += '				</a>';
		//template += '			</p>';
		//template += '		</div>';

	});

	//setTimeout(function () { OnErrorImage(); }, 3000);

	if (data && data.length >= PageSize) {
		$("#contractors_section .see-more").fadeIn();
	} else {
		$("#contractors_section .see-more").fadeOut();
	}

	if ($('#contractors_section .main-div').html().length == 0) {
		$("html, body").animate({ scrollTop: 0 }, 1000);
		$('#contractors_section .no-more').fadeIn();
		$("#contractors_section .see-more").fadeOut();
	}
}

function GetFilterValues() {
	filter.search = $("#ArticleSearch").val();
	filter.pageNumber = pg;
	filter.sortBy = $("#SortBy").val() ? $("#SortBy").val() : filter.sortBy;
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
	$('#contractors_section .main-div').empty();
	GetArticles();
});

//#endregion
