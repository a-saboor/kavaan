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
	anyID: $('#ID').val(),
	bedrooms: null,
	bathrooms: null,
	minprice: null,
	maxprice: null,
	typeId: null,
}

let locations = [
	// ['@Model.Title', @Model.Latitude, @Model.Longitude, 1, '@Model.SellingPrice', '@Model.City.Name', @Model.ID]
];

var i = 1;
var j = 0;
let map;
let markers = [];

//#endregion

//#region document ready function
$(document).ready(function () {

	$('.units_section section .main-div').html(return_loading_section(0));
	$('.units_section section .main-map-div').html(return_loading_section(0));

	GetUnitTypes();

	$('.btn-view').click(function () {
		if ($('#list-section').is(':visible')) {
			$('#list-section').hide();
			$('#map-list-section').show();

			$(this).find('p').text(ChangeString("List View", "طريقة عرض القائمة"));
			$(this).find('i').removeClass("fa-map-o").addClass('fa-list-ul');
		}
		else {
			$('#list-section').show();
			$('#map-list-section').hide();

			$(this).find('p').text(ChangeString("Map View", "طريقة عرض الخريطة"));
			$(this).find('i').removeClass("fa-list-ul").addClass('fa-map-o');
		}
	});

	$('.units_section section .main-div').html(return_loading_section(0));
	$('.units_section section .main-map-div').html(return_loading_section(0));

	BindFilters();
	//setTimeout(function () {
	//	BindFilters();
	//	GetFilterValues();
	//	GetArticles();
	//}, 500);

	//$('#ArticleSearch').change(function () {
	//	if ($('#ArticleSearch').val()) {
	//		$('.units_section section .main-div').html(return_loading_section(0));
	//		$('.units_section section .main-map-div').html(return_loading_section(0));
	//		removeMapMarkers();
	//		pg = 1;
	//		GetFilterValues();
	//		GetArticles();
	//	}
	//});

	$('#ArticleSearch').keyup(function () {
		$('.units_section section .main-div').html(return_loading_section(0));
		$('.units_section section .main-map-div').html(return_loading_section(0));
		removeMapMarkers();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#btnSearch').click(function () {
		$('.units_section section .main-div').html(return_loading_section(0));
		$('.units_section section .main-map-div').html(return_loading_section(0));
		removeMapMarkers();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#sortBy').change(function () {
		$('.units_section section .main-div').html(return_loading_section(0));
		$('.units_section section .main-map-div').html(return_loading_section(0));
		removeMapMarkers();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#UnitTypes').change(function () {
		$('.units_section section .main-div').html(return_loading_section(0));
		$('.units_section section .main-map-div').html(return_loading_section(0));
		removeMapMarkers();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#noOfBedrooms').change(function () {
		$('.units_section section .main-div').html(return_loading_section(0));
		$('.units_section section .main-map-div').html(return_loading_section(0));
		removeMapMarkers();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('#sortBy').change(function () {
		$('.units_section section .main-div').html(return_loading_section(0));
		$('.units_section section .main-map-div').html(return_loading_section(0));
		removeMapMarkers();
		pg = 1;
		GetFilterValues();
		GetArticles();
	});

	$('.units_section section .see-more').click(function () {
		//if (pg < totalPages) {
		pg++;
		$('.units_section section .see-more').hide();
		$(".filter-loader").show();
		GetFilterValues();
		GetArticles();
		//}
	});

});
//#endregion

//#region Ajax Call

function GetUnitTypes() {
	$.ajax({
		type: 'GET',
		url: '/units/unit-types',
		success: function (response) {
			BindUnitTypes(response.data);
		},
		error: function (e) {
			ToastrMessage('error', ServerErrorShort, 6);
		}
	});
}

function GetArticles() {
	$.ajax({
		type: 'POST',
		url: '/units/filtered-units',
		contentType: "application/json",
		data: JSON.stringify(filter),
		success: function (response) {
			BindGridArticles(response.data);
		},
		error: function (e) {
			ToastrMessage('error', ServerErrorShort, 6);
		}
	});
}

//#endregion

//#region Functions for Binding Data

function BindUnitTypes(data) {

	$('#UnitTypes').empty();
	$('#UnitTypes').append($("<option />").val('').text(ChangeString("Any", "أي")));

	$.each(data, function (k, v) {
		$('#UnitTypes').append($("<option />").val(v.Value).text(v.Text).attr('selected', v.Selected).attr('disabled', v.Disabled));
	});
}

function BindGridArticles(data) {

	//$('.units_section section .main-div').empty();
	//$('.units_section section .main-map-div').empty();

	$.each(data, function (k, v) {

		$('.units_section section .main-div').append(`
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
							<a href="/units/${v.ID}"><h1 class="font-Rubik text-xl font-medium my-2">Price: ${ChangeString('AED', 'درهم إماراتي')} ${v.SellingPrice}</h1></a>
							<a href="/units/${v.ID}">
								<div class="flex justify-between my-2">
									<div class="sm:flex sm:items-center">
										<img src="/assets/images/web-icons/rooms-icon.png" alt="" class="sm:h-5 sm:mr-2">
										<p class="font-Rubik text-xs font-medium sm:text-[10px] sm:mr-2">${v.NoOfRooms} ${ChangeString('Rooms', 'غرف')}</p>
									</div>
									<div class="sm:flex sm:items-center">
										<img src="/assets/images/web-icons/baths-icon.png" alt="" class="sm:h-5 sm:mr-2">
										<p class="font-Rubik text-xs font-medium sm:text-[10px] sm:mr-2">${v.NoOfBaths} ${ChangeString('Bathrooms', 'الحمامات')}</p>
									</div>
									<div class="sm:flex sm:items-center">
										<img src="/assets/images/web-icons/size-icon.png" alt="" class="sm:h-5 sm:mr-2">
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

		$('.units_section section .main-map-div').append(`
			<div class="px-1 md:px-10 py-4 unit-box">
				<div class="bg-white hover:bg-white hover:shadow-xl transition duration-300 ease-in-out transform hover:-translate-y-1 p-4 rounded-lg sm:flex sm:flex-wrap">
					<div class="relative sm:w-[28%]">
						<div class="bg-gk-gold flex px-2 py-1 rounded-md items-center justify-center absolute top-4 -left-4">
							<svg viewBox="0 0 384 512" class="w-2 mr-1">
								<path fill="white" d="M172.268 501.67C26.97 291.031 0 269.413 0 192 0 85.961 85.961 0 192 0s192 85.961 192 192c0 77.413-26.97 99.031-172.268 309.67-9.535 13.774-29.93 13.773-39.464 0zM192 272c44.183 0 80-35.817 80-80s-35.817-80-80-80-80 35.817-80 80 35.817 80 80 80z" class=""></path>
							</svg>
							<p class="font-Rubik font-medium text-[10px] text-white unit-area">${v.CityName}</p>
						</div>
						<a href="/units/${v.ID}" class="unit-url">
							<img src="${v.Image}" alt="" class="rounded-lg sm:object-cover sm:h-40 img-unit">
						</a>
						<div style="display: none;">
							<p class="unit-latitude">${v.Latitude}</p>
							<p class="unit-longitude">${v.Longitude}</p>
						</div>
					</div>
					<div class="sm:w-[67%] sm:ml-4">
						<a href="/units/${v.ID}"><h1 class="font-Rubik text-xs font-medium my-2">${v.Title}</h1></a>
						<a href="/units/${v.ID}"><h1 class="font-Rubik text-xl font-medium my-2">Price: ${ChangeString('AED', 'درهم إماراتي')} ${v.SellingPrice}</h1></a>
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
								<button onclick="showEnquirePopup(${v.ID})" class="border-2 border-black/20 w-full py-2 rounded-md text-black/40 font-Rubik text-xs" title="${ChangeString('ENQUIRE', 'استعلام')}">
									${ChangeString('ENQUIRE', 'استعلام')}
								</button>
							</div>
							<div class="my-2 sm:w-[35%] sm:mr-2">
								<button onclick="showPaymentPlanPopup(${v.ID})" class="border-2 border-black/20 w-full py-2 rounded-md text-black/40 font-Rubik text-xs" title="${ChangeString('Payment Plans', 'خطط الدفع')}">
									${ChangeString('Payment Plans', 'خطط الدفع')}
								</button>
							</div>
							<div class="mt-2 sm:w-[28%] sm:mr-2">
								<button onclick="bookNow(${v.ID})" class="border-2 border-black/20 w-full py-2 rounded-md text-black/40 font-Rubik text-xs" title="${ChangeString('Book Now', 'احجز الآن')}">
									${ChangeString('Book Now', 'احجز الآن')}
								</button>
							</div>
						</div>
					</div>
				</div>
			</div>
		`);

		var location = [
			`${v.Title}`,
			v.Latitude,
			v.Longitude,
			i,
			`${v.SellingPrice}`,
			`${v.CityName}`,
			`${v.ID}`
		]

		i++;
		locations.push(location);

	});

	if ($('#list-section section .main-div section')) {
		$('#list-section section .main-div section').remove();
	}

	if ($('#map-list-section section .main-map-div section')) {
		$('#map-list-section section .main-map-div section').remove();
	}

	//setTimeout(function () { OnErrorImage(); }, 3000);

	if (data && data.length >= PageSize) {
		$(".units_section section .see-more").fadeIn();
	} else {
		$(".units_section section .see-more").fadeOut();
	}

	if ($('.units_section section .main-div').html().length == 0) {
		$("html, body").animate({ scrollTop: 0 }, 1000);
		$(".units_section section .see-more").hide();
		$('.units_section section .no-more').fadeIn();
		//$('.btn-view').fadeOut();
	}
	else {
		$('.units_section section .no-more').fadeOut();
	}

	addMarkersOnMap();//add markers on map function
	OnErrorImage(0.5, 'unit');
}

function GetFilterValues() {
	filter.search = $("#ArticleSearch").val();
	filter.pageNumber = pg;
	filter.sortBy = $("#sortBy").val();
	filter.bedrooms = $("#noOfBedrooms").val();
	filter.typeId = $("#UnitTypes").val();
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
	$('.units_section section .main-div').html(return_loading_section(0));
	$('.units_section section .main-map-div').html(return_loading_section(0));
	GetArticles();
});

function BindFilters() {

	if (false /*unitFilters*/) {
		//if (unitFilters.unitTypeId)
		//	$('#UnitTypes').val(Number(unitFilters.unitTypeId));
		//if (unitFilters.bedrooms)
		//	$('#noOfBedrooms').val(Number(unitFilters.bedrooms));
		////if (unitFilters.priceRange)
		////	$('#price_range').val(unitFilters.priceRange);
		////if (unitFilters.developmentTypeId)
		////	$('#development_type').val(Number(unitFilters.developmentTypeId));

		//setTimeout(function () {
		//	if ($('#btnSearch')) {
		//		$('#btnSearch').trigger('click');
		//	}
		//	else {
		//		GetFilterValues();
		//		GetArticles();
		//	}
		//}, 500);

	}
	else {
		//BindFilters();
		GetFilterValues();
		GetArticles();
	}
}

//#endregion

//#region Functions for Binding Map

function removeMapMarkers() {

	for (var k = 0; k < locations.length; k++) {

		map.setCenter(markers[k].getPosition());
		markers[k].setPosition(null);
		markers[k].setMap(null);
		markers[k] = null;
		//markers.splice(k, 1);
	}
	markers = [];
	locations = [];
	i = 1;
	j = 0;
}

function InitMap() {

	map = new google.maps.Map(document.getElementById('googleMap'), {
		zoom: 12,
		center: new google.maps.LatLng(25.0959006, 55.1944191),
		mapTypeId: google.maps.MapTypeId.ROADMAP,
	});

	addMarkersOnMap();

}

function addMarkersOnMap() {

	//var marker, j;

	var infowindow = new google.maps.InfoWindow();

	for (j = j; j < locations.length; j++) {
		var marker = new google.maps.Marker({
			position: new google.maps.LatLng(locations[j][1], locations[j][2]),
			map: map,
			icon: '/assets/images/web-icons/map-marker.png'
		});
		google.maps.event.addListener(marker, 'click', (function (marker, j) {
			return function () {
				var template = `<div class="flex flex-col font-Rubik">
											<a class="flex mb-1" target="_blank" href="/units/${locations[j][6]}">
												<p class="w-[50%]">${ChangeString('Property:', 'ملكية:')} </p> <p class="w-[100%]">${locations[j][0]} </p>
											</a>
											<div class="flex mb-1">
												<p class="w-[50%]">${ChangeString('Location:', 'موقع:')} </p> <p class="w-[100%]">${locations[j][5]} </p>
											</div>
											<div class="flex mb-1">
												<p class="w-[50%]">${ChangeString('AED:', 'درهم إماراتي:')} </p> <p class="w-[100%]">${locations[j][4]} </p>
											</div>
											<div class="flex" >
												<p class="w-[50%]"></p> <a target="_blank" href="/units/${locations[j][6]}" class="w-[100%] text-gk-blue font-medium">${ChangeString('See More', 'شاهد المزيد')}</a>
											</div>
										</div>
										`;
				infowindow.setContent(template);
				infowindow.open(map, marker);
			}
		})(marker, j));

		//Add marker to the array.
		markers.push(marker);
	}

}

//#endregion