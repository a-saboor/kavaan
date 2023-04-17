'use strict'

//#region Global Variables and Arrays
var pg = 1;
var PageSize = 12;
var isPageRendered = false;
var totalPages;
//var lang = "en";

var filter = {
	search: null,
	PageSize: 12,
	pageNumber: 1,
	sortBy: 1,
	anyID: $('#ID').val()
}

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

	$('#units_section .main-div').empty().show();

	GetFilterValues();
	GetArticles();

	$('#ArticleSearch').change(function () {
		if ($('#ArticleSearch').val()) {
			$('#units_section .main-div').empty();
			pg = 1;
			GetFilterValues();
			GetArticles();
		}
	});

	$('#btnSearch').click(function () {
		$('#units_section .main-div').empty();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#sortBy').change(function () {
		$('#units_section .main-div').empty();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#units_section .see-more').click(function () {
		//if (pg < totalPages) {
		pg++;
		$('#units_section .see-more').hide();
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
		url: '/units/filters',
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

		$('#units_section .main-div').append(`
			<div class="xl:w-[49%] my-4">
				<div class="px-1">
					<div class="bg-white hover:bg-white hover:shadow-xl transition duration-300 ease-in-out transform hover:-translate-y-1 p-4 rounded-lg sm:flex sm:flex-wrap">
						<div class="relative sm:w-[28%]">
							<div class="bg-gk-gold flex px-2 py-1 rounded-md items-center justify-center absolute top-4 -left-4">
								<svg viewBox="0 0 384 512" class="w-2 mr-1">
									<path fill="white" d="M172.268 501.67C26.97 291.031 0 269.413 0 192 0 85.961 85.961 0 192 0s192 85.961 192 192c0 77.413-26.97 99.031-172.268 309.67-9.535 13.774-29.93 13.773-39.464 0zM192 272c44.183 0 80-35.817 80-80s-35.817-80-80-80-80 35.817-80 80 35.817 80 80 80z" class=""></path>
								</svg>
								<p class="font-Rubik font-medium text-[10px] text-white">${v.CityName}</p>
							</div>
							<a href="/units/${v.ID}">
								<img src="${v.Image}" alt="" class="rounded-lg sm:object-cover sm:h-40 img-unit">
							</a>
						</div>
						<div class="sm:w-[67%] sm:ml-4">
							<a href="/units/${v.ID}"><h1 class="font-Rubik text-xs font-medium my-2">${v.Title}</h1></a>
							<a href="/units/${v.ID}"><h1 class="font-Rubik text-xl font-medium my-2"><h1 class="font-Rubik text-xl font-medium my-2">${ChangeString('AED', 'درهم إماراتي')} ${v.SellingPrice}</h1></a>
							<a href="/units/${v.ID}">
								<div class="flex justify-between my-2">
									<div class="sm:flex sm:items-center">
										<img src="/assets/images/web-icons/rooms-icon.png" alt="" class="sm:h-5 sm:mr-2">
										<p class="font-Rubik text-xs font-medium sm:text-[10px] sm:mr-2">${v.NoOfRooms} ${ChangeString('Rooms', 'غرف')}</p>
									</div>
									<div class="sm:flex sm:items-center">
										<img src="/assets/images/web-icons/rooms-icon.png" alt="" class="sm:h-5 sm:mr-2">
										<p class="font-Rubik text-xs font-medium sm:text-[10px] sm:mr-2">${v.NoOfBaths} ${ChangeString('Bathrooms', 'الحمامات')}</p>
									</div>
									<div class="sm:flex sm:items-center">
										<img src="/assets/images/web-icons/rooms-icon.png" alt="" class="sm:h-5 sm:mr-2">
										<p class="font-Rubik text-xs font-medium sm:text-[10px] sm:mr-2">${v.Size} ${ChangeString('SQ.FT', 'قدم مربع')}</p>
									</div>
								</div>
							</a>
							<div class="sm:flex sm:flex-wrap sm:justify-between">
								<div class="my-2 sm:w-[28%] sm:mr-2">
									<button onclick="showEnquirePopup(${v.ID})" class="border-2 border-black/20 w-full py-2 rounded-md text-black/60 font-Rubik text-xs" title="${ChangeString('ENQUIRE', 'استعلام')}">
										${ChangeString('ENQUIRE', 'استعلام')}
									</button>
								</div>
								<div class="my-2 sm:w-[35%] sm:mr-2">
									<button onclick="showPaymentPlanPopup(${v.ID})" class="border-2 border-black/20 w-full py-2 rounded-md text-black/60 font-Rubik text-xs" title="${ChangeString('Payment Plans', 'خطط الدفع')}">
										${ChangeString('Payment Plans', 'خطط الدفع')}
									</button>
								</div>
								<div class="mt-2 sm:w-[28%] sm:mr-2">
									<button onclick="bookNow(${v.ID})" class="border-2 border-black/20 w-full py-2 rounded-md text-black/60 font-Rubik text-xs" title="${ChangeString('Book Now', 'احجز الآن')}">
										${ChangeString('Book Now', 'احجز الآن')}
									</button>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		`);

	});

	//setTimeout(function () { OnErrorImage(); }, 3000);

	if (data && data.length >= PageSize) {
		$("#units_section .see-more").fadeIn();
	} else {
		$("#units_section .see-more").fadeOut();
	}

	if ($('#units_section .main-div').html() && $('#units_section .main-div').html().length == 0) {
		$("html, body").animate({ scrollTop: 0 }, 1000);
		$('#units_section .no-more').fadeIn();
		$("#units_section .see-more").fadeOut();
	}

	OnErrorImage(0.5, 'unit');
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
	$('#units_section .main-div').empty();
	GetArticles();
});

//#endregion
