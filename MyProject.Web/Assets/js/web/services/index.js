'use strict'

//#region Global Variables and Arrays
var pg = 1;
var PageSize = window.innerWidth > 1440 ? 12 : 8;
var isPageRendered = false;
var totalPages;
var ParentID;	//CategoryID
var isServiceFetched = true;
var IsInitAutoCompleteRun = false;

//var lang = "en";

var filter = {
	search: null,
	PageSize: window.innerWidth > 1440 ? 12 : 8,
	pageNumber: 1,
	sortBy: 1,
	parentID: $('#ID').val()
}

const params = (new URL(document.location)).searchParams;
const search = params.get("search");
//#endregion

//#region document ready function
$(document).ready(function () {

	//description div work
	OnErrorImage(0.5, 'service');
	let description = $('#description').val();
	if (description.includes("\n")) {
		description = description.split('\n');
		$.each(description, function (k, v) {
			$('#TitleDescription').append(`<p class="p text-xs md:text-lg 2xl:text-xl my-2 leading-[1rem] md:leading-[1.8rem]">${v}</p>`)
		});
	}
	else {
		description = description;
		$('#TitleDescription').html(description);
	}

	$('#TitleDescription').show();
	//description div work end

	$('#services_section .main-div').empty();
	$('#services_section .loading').show();

	GetFilterValues();

	//$('#ArticleSearch').change(function () {
	//	if ($('#ArticleSearch').val()) {
	//		$('#services_section .main-div').empty();
	//		pg = 1;
	//		GetFilterValues();
	//		GetArticles();
	//	}
	//});

	$('#ArticleSearch').keyup(function (e) {
		if (isServiceFetched) {
			if (e.keyCode === 13)/*Enter key code*/ {
				$('#services_section .main-div').empty();
				$('#services_section .loading').show();
				pg = 1;
				GetFilterValues();
				GetArticles();
			}
			else if ($(this).val() === '') {
				$('#services_section .main-div').empty();
				$('#services_section .loading').show();
				pg = 1;
				GetFilterValues();
				GetArticles();
			}
		}
	});

	$('#btnSearch').click(function () {
		if (isServiceFetched) {
			$('#services_section .main-div').empty();
			$('#services_section .loading').show();
			pg = 1;
			GetFilterValues();
			GetArticles();
		}
	});

	$('#SortBy').change(function () {
		if (isServiceFetched) {
			$('#services_section .main-div').empty();
			$('#services_section .loading').show();
			pg = 1;
			GetFilterValues();
			GetArticles();
		}
	});

	$('#services_section .see-more').click(function () {
		if (isServiceFetched) {
			//if (pg < totalPages) {
			pg++;
			$('#services_section .see-more').hide();
			$('#services_section .loading').show();
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
	isServiceFetched = false;
	$.ajax({
		type: 'POST',
		url: '/services/filters',
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

		$('#services_section .main-div').append(`

			<div class="flex h-auto">
				<a class="relative group cursor-pointer" onclick="openBookingPopup(${v.ID});">
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
	$('#services_section .loading').hide();

	if (data && data.length >= PageSize) {
		$("#services_section .see-more").fadeIn();
	} else {
		$("#services_section .see-more").fadeOut();
	}

	if ($('#services_section .main-div').html().length == 0) {
		//$("html, body").animate({ scrollTop: 0 }, 1000);
		$('#services_section .no-more').fadeIn();
		$("#services_section .see-more").fadeOut();
	}
	else {
		$('#services_section .no-more').fadeOut();
	}

	OnErrorImage(0.5, 'blog');
	isServiceFetched = true;
}

function GetFilterValues() {
	filter.search = $("#ArticleSearch").val();
	filter.pageNumber = pg;
	filter.sortBy = $("#SortBy").val();
}

//#endregion
