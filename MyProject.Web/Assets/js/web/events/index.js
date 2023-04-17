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
	sortBy: 1
}

//#endregion

//#region document ready function
$(document).ready(function () {

	$('#events_section .main-div').empty().html(return_loading_section(0));

	GetFilterValues();
	GetArticles();

	$('#ArticleSearch').change(function () {
		if ($('#ArticleSearch').val()) {
			$('#events_section .main-div').empty();
			pg = 1;
			GetFilterValues();
			GetArticles();
		}
	});

	$('#btnSearch').click(function () {
		$('#events_section .main-div').empty();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#SortBy').change(function () {
		$('#events_section .main-div').empty();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#events_section .see-more').click(function () {
		//if (pg < totalPages) {
		pg++;
		$('#events_section .see-more').hide();
		$('#events_section .loading').show();
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
		url: '/news-and-events/filters',
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

		$('#events_section .main-div').append(`

			<div class="bg-white rounded-lg mb-8 sm:w-[47.5%] xl:w-[22.5%] ml-1">
				<div class="relative">
					<a href="/news-and-events/${v.Slug}">
						<img src="${v.Image}" alt="" class="rounded-lg h-[230px] w-full xl:h-[180px] object-cover img-event">
							<h1 class="font-Rubik text-xs uppercase font-medium bg-gk-gold w-10 h-11 rounded-lg text-white flex items-center text-center flex-col justify-center absolute -bottom-4 right-4">
							<span class="text-lg">${v.Date}</span>
							<span class="text-[10px] -mt-2">${v.Month}</span>
						</h1>
					</a>
				</div>
				<div class="p-4">
					<h1 class="uppercase font-Rubik text-xs font-medium">${v.Title}</h1>
					<p class="text-xs font-Rubik text-justify my-2 cut-text">${ v.Description && (v.Description).length > 200 ? (v.Description).substring(0, 200) + ' ...' : v.Description}</p>
					<a href="/news-and-events/${v.Slug}" class="font-Rubik text-gk-gold text-xs font-medium cursor-pointer">Read More</a>
				</div>
			</div>

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
		$("#events_section .see-more").fadeIn();
	} else {
		$("#events_section .see-more").fadeOut();
	}

	if ($('#events_section .main-div section')) {
		$('#events_section .main-div section').remove();
	}

	if ($('#events_section .main-div').html().length == 0) {
		$("html, body").animate({ scrollTop: 0 }, 1000);
		$('#events_section .no-more').fadeIn();
		$("#events_section .see-more").fadeOut();
	}

	OnErrorImage(0.5, 'event');
}

function GetFilterValues() {
	filter.search = $("#ArticleSearch").val();
	filter.pageNumber = pg;
	filter.sortBy = $("#SortBy").val();
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
	$('#events_section .main-div').empty();
	GetArticles();
});

//#endregion
