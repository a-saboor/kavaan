'use strict'

//#region Global Variables and Arrays

//#endregion

//#region document ready function
$(document).ready(function () {

	GetDevelopments();
	GetProjects();

});
//#endregion

//#region Ajax Call

function GetDevelopments() {
	$.ajax({
		type: 'Get',
		url: '/developments/get-all',
		success: function (response) {
			if (response.success) {
				BindDevelopments(response.data);
			} else {
				BindDevelopments(0);
			}
		}
	});
}

function GetProjects() {
	$.ajax({
		type: 'Get',
		url: '/projects/get-all?take=' + 8,
		success: function (response) {
			if (response.success) {
				BindProjects(response.data);
			} else {
				GetProjects(0);
			}
		}
	});
}

//#endregion

//#region Functions for Binding Data

function BindDevelopments(data) {
	//<a href="/developments/${v.ID}
	if (data && data.length) {
		$('#our_developments_section .main-div').empty().html(return_loading_section(0));
		$('#our_developments_section .main-div').append(`<img src="/assets/images/bg/yarndyde-cricle.png" alt="" class="absolute -top-20 -left-20 object-cover">`);

		$.each(data, function (k, v) {
			$('#our_developments_section .main-div').append(`
			<a href="/developments/${v.ID}" class="rounded-lg w-full h-[150px] bg-residentials-bg bg-cover flex items-center justify-center mb-4 lg:mr-4 z-[1]" style="background-image: url(${v.Image});">
				<h1 class="font-Rubik font-medium text-white text-4xl text-shadow capitalize xl:text-3xl">${v.Name}</h1>
			</a>
			`);
		});

	}
	else {
		//$('#our_developments_section').remove();
		$('#our_developments_section .no-more').show();
	}

	if ($('#our_developments_section .main-div section')) {
		$('#our_developments_section .main-div section').remove();
	}
}

function BindProjects(data) {
	//<a href="/projects/${v.ID}"
	if (data && data.length) {
		$('#latest_project_section .main-div').empty().html(return_loading_section(0)).show();
		$('#latest_project_section .main-div').append(`<div class="owl-carousel owl-theme"></div>`);

		$.each(data, function (k, v) {
			$('#latest_project_section .main-div .owl-carousel').append(`
				<div class="">
					<a href="/projects/${v.ID}" class="mb-4 w-full sm:w-[48.5%] lg:w-[18.5%]">
						<img src="${v.Thumbnail}" alt="" class="rounded-lg mb-2 w-full h-[18rem] object-cover">
						<p class="font-Rubik text-xs uppercase">${v.Title}</p>
					</a>
				</div>
			`);
		});
	}
	else {
		//$('#latest_project_section').remove();
		$('#latest_project_section .no-more').show();
	}

	if ($('#latest_project_section .main-div section')) {
		$('#latest_project_section .main-div section').remove();
	}

	OnErrorImage(0.5, 'property');

	$('.owl-carousel').owlCarousel({
		rtl: direction == "ltr" ? false : true,
		loop: false,
		navText: [/*"<img src='/assets/images/web-icons/left-arrow.png'>"*/, /*"<img src='/assets/images/web-icons/right-arrow.png'>"*/],
		nav: false,
		margin: 16,
		dots: false,
		responsiveClass: true,
		responsive: {
			0: {
				items: 1,
			},
			600: {
				items: 3,
			},
			1000: {
				items: 5,
			}
		}
	});

}

//#endregion
