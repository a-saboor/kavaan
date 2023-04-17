'use strict'

//#region Global Variables and Arrays
var pg = 1;
var PageSize = window.innerWidth > 1440 ? 12 : 8;
var isPageRendered = false;
var totalPages;
var isCategoryFetched = true;
//var lang = "en";

var filter = {
	search: null,
	PageSize: window.innerWidth > 1440 ? 12 : 8,
	pageNumber: 1,
	sortBy: 1
}

const params = (new URL(document.location)).searchParams;
const search = params.get("search");
//#endregion

//#region document ready function
$(document).ready(function () {

	$('#categories_section .main-div').empty();
	$('#categories_section .loading').show();

	GetFilterValues();


	//$('#ArticleSearch').change(function () {
	//	if ($('#ArticleSearch').val()) {
	//		$('#categories_section .main-div').empty();
	//		pg = 1;
	//		GetFilterValues();
	//		GetArticles();
	//	}
	//});

	$('#ArticleSearch').keyup(function (e) {
		if (isCategoryFetched) {
			if (e.keyCode === 13) /*Enter key code*/ {
				$('#categories_section .main-div').empty();
				$('#categories_section .loading').show();
				pg = 1;
				GetFilterValues();
				GetArticles();
			}
			else if ($(this).val() === '') {
				$('#categories_section .main-div').empty();
				$('#categories_section .loading').show();
				pg = 1;
				GetFilterValues();
				GetArticles();
			}
		}
	});

	$('#btnSearch').click(function () {
		if (isCategoryFetched) {
			$('#categories_section .main-div').empty();
			$('#categories_section .loading').show();
			pg = 1;
			GetFilterValues();
			GetArticles();
		}
	});

	$('#SortBy').change(function () {
		if (isCategoryFetched) {
			$('#categories_section .main-div').empty();
			$('#categories_section .loading').show();
			pg = 1;
			GetFilterValues();
			GetArticles();
		}
	});

	$('#categories_section .see-more').click(function () {
		if (isCategoryFetched) {
			//if (pg < totalPages) {
			pg++;
			$('#categories_section .see-more').hide();
			$('#categories_section .loading').show();
			GetFilterValues();
			GetArticles();
			//}
		}
	});

	//search with url parameters
	if (search) {
		$('#ArticleSearch').val(search);
		$('#btnSearch').trigger('click');
	} else {
		GetArticles();
	}

});
//#endregion

//#region Ajax Call

function GetArticles() {
	isCategoryFetched = false;
	$.ajax({
		type: 'POST',
		url: '/categories/filters',
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

		$('#categories_section .main-div').append(`

			<div class="flex h-auto">
				<a class="relative group" href="/service/${v.Slug}">
					<img src="${v.Image}"
							class="w-full mx-auto rounded-xl filter-br-bl-1 img-blog" alt="img">
					<div class="absolute text-start bottom-[-2%] left-[50%] translate-x-[-50%] translate-y-[-50%] w-full px-4 md:px-6">
						<h2 class="text-white font-normal text-sm md:text-base leading-[0.8rem] md:leading-[1.5rem]">
							${v.Title}
						</h2>
						<p class="text-white/90 font-extralight text-xxs md:text-xs opacity-0 group-hover:opacity-100 transition ease-in-out duration-300">
							View More &nbsp;<i class="fa fa-angle-right"></i>
						</p>
					</div>
				</a>
			</div>

		`);

	});

	//setTimeout(function () { OnErrorImage(); }, 3000);
	$('#categories_section .loading').hide();

	if (data && data.length >= PageSize) {
		$("#categories_section .see-more").fadeIn();
	} else {
		$("#categories_section .see-more").fadeOut();
	}

	if ($('#categories_section .main-div section')) {
		$('#categories_section .main-div section').remove();
	}

	if ($('#categories_section .main-div').html().length == 0) {
		//$("html, body").animate({ scrollTop: 0 }, 1000);
		$('#categories_section .no-more').fadeIn();
		$("#categories_section .see-more").fadeOut();
	}
	else {
		$('#categories_section .no-more').fadeOut();
	}

	OnErrorImage(0.5, 'blog');
	isCategoryFetched = true;
}

function GetFilterValues() {
	filter.search = $("#ArticleSearch").val();
	filter.pageNumber = pg;
	filter.sortBy = $("#SortBy").val();
}

//#endregion
