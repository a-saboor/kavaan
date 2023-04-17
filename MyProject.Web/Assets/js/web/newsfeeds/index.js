'use strict'

//#region Global Variables and Arrays
var pg = 1;
var PageSize = 9;
var isPageRendered = false;
var totalPages;
//var lang = "en";

var filter = {
	search: null,
	PageSize: 9,
	pageNumber: 1,
	sortBy: 1
}

//#endregion

//#region document ready function
$(document).ready(function () {

	$('#newsfeeds_section .main-div').empty();
	hideGridLoading('#newsfeeds_section', false);

	GetFilterValues();
	GetArticles();

	$('#ArticleSearch').change(function () {
		if ($('#ArticleSearch').val()) {
			$('#newsfeeds_section .main-div').empty();
			hideGridLoading('#newsfeeds_section', false);
			pg = 1;
			GetFilterValues();
			GetArticles();
		}
	});

	$('#btnSearch').click(function () {
		$('#newsfeeds_section .main-div').empty();
		hideGridLoading('#newsfeeds_section', false);
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#SortBy').change(function () {
		$('#newsfeeds_section .main-div').empty();
			hideGridLoading('#newsfeeds_section', false);
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#newsfeeds_section .see-more').click(function () {
		//if (pg < totalPages) {
		pg++;
		$('#newsfeeds_section .see-more').hide();
		hideGridLoading('#newsfeeds_section', false);
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
		url: '/newsfeeds/filters',
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
		console.log(v)
		$('#newsfeeds_section .main-div').append(`

			<div class="col-lg-4 col-sm-6 col-12">
				<div class="ltn__blog-item ltn__blog-item-3">
					<div class="ltn__blog-img">
						<a href="/newsfeeds/${v.Slug}"><img src="${v.Image}" alt="#"></a>
					</div>
					<div class="ltn__blog-brief">
						<div class="ltn__blog-meta">
							<ul>
								<li class="ltn__blog-author">
									<a href="#"><i class="far fa-user"></i>by: ${v.Host}</a>
								</li>
								<li class="ltn__blog-tags">
									<a href="#"><i class="fas fa-tags"></i>${v.Title}</a>
								</li>
							</ul>
						</div>
						<h3 class="ltn__blog-title"><a href="/newsfeeds/${v.Slug}">${v.Description}</a></h3>
						<div class="ltn__blog-meta-btn">
							<div class="ltn__blog-meta">
								<ul>
									<li class="ltn__blog-date"><i class="far fa-calendar-alt"></i>${v.FullDate}</li>
								</ul>
							</div>
							<div class="ltn__blog-btn">
								<a href="/newsfeeds/${v.Slug}">Read more</a>
							</div>
						</div>
					</div>
				</div>
			</div>

		`);
		
	});

	//setTimeout(function () { OnErrorImage(); }, 3000);
	hideGridLoading('#newsfeeds_section', true);

	if (data && data.length >= PageSize) {
		$("#newsfeeds_section .see-more").fadeIn();
	} else {
		$("#newsfeeds_section .see-more").fadeOut();
	}

	if ($('#newsfeeds_section .main-div').html().length == 0) {
		$("html, body").animate({ scrollTop: 0 }, 1000);
		$('#newsfeeds_section .no-more').fadeIn();
		$("#newsfeeds_section .see-more").fadeOut();
	}
	else {
		$('#newsfeeds_section .no-more').fadeOut();
	}

	OnErrorImage(0.5, 'newsfeed');
}

function GetFilterValues() {
	filter.search = $("#ArticleSearch").val();
	filter.pageNumber = pg;
	filter.sortBy = $("#SortBy").val();
}
//#endregion
