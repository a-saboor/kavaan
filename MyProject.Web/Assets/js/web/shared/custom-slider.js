'use strict';

$(document).ready(function () {

	$('.catogory-swiper .swiper-slide').click(function () {
		//remove active class from all swiper-slide
		$('.catogory-swiper .swiper-slide a').removeClass('active');
		//add active class to clicked swiper-slide
		$(this).find('a').addClass('active');
	});

	//services_swiper.init();

});
//category slider
const catogory_swiper = new Swiper(".catogory-swiper", {
	direction: window.outerWidth > 767 ? "vertical" : "horizontal",
	// updateOnWindowResize:true,
	// slidesPerView: window.outerWidth > 764 ? 6 : 1,
	spaceBetween: 10,
	mousewheel: true,
	// pagination: {
	//   el: ".swiper-pagination",
	//   clickable: true,
	// },
	navigation: {
		nextEl: '.next',
		prevEl: '.prev',
	},
	on: {
		resize: function () {
			catogory_swiper.changeDirection(window.outerWidth > 767 ? "vertical" : "horizontal");
		},
	},
	breakpoints: {
		0: {
			slidesPerView: 1,
		},
		767: {
			slidesPerView: 6
		},
		1400: {
			slidesPerView: 8
		}
	}
});

//services slider
const services_swiper = new Swiper(".services-swiper", {
	direction: "horizontal",
	speed: 500,
	centeredSlides: window.outerWidth > 767 ? false : true,
	// effect: "creative",
	// creativeEffect: {
	//   prev: {
	//     shadow: true,
	//     translate: ["-20%", 0, -1],
	//   },
	//   next: {
	//     translate: ["100%", 0, 0],
	//   },
	// },
	spaceBetween: 15,
	mousewheel: true,
	pagination: {
		el: ".swiper-pagination",
		clickable: true,
	},
	// navigation: {
	//   nextEl: '.next',
	//   prevEl: '.prev',
	// },
	// scrollbar: {
	//   el:'.swiper-scrollbar',
	// },
	on: {
		resize: function () {
			services_swiper.centeredSlides = window.outerWidth > 767 ? false : true;
		},
	},
	breakpoints: {
		0: {
			slidesPerView: 1,
			centeredSlides: true,
		},
		425: {
			slidesPerView: 3,
			centeredSlides: true,
		},
		767: {
			slidesPerView: 3.5,
			centeredSlides: false,
		},
		1024: {
			slidesPerView: 3.5,
			centeredSlides: false,
		},
		1800: {
			slidesPerView: 4.5,
			centeredSlides: false,
		},
	}
});

//swiper-ak-1 slider 
/*contact-us*/
const swiper_ak_1 = new Swiper(".swiper-ak-1", {
	direction: "horizontal",
	speed: 500,
	centeredSlides: true,
	centerInsufficientSlides: true,
	centeredSlidesBounds: true,
	spaceBetween: 1,
	mousewheel: true,
	// pagination: {
	//   el: ".swiper-pagination",
	//   clickable: true,
	// },
	// navigation: {
	//   nextEl: '.next',
	//   prevEl: '.prev',
	// },
	// scrollbar: {
	//   el:'.swiper-scrollbar',
	// },
	// on: {
	//   resize: function () {
	//     swiper_ak_1.changeDirection("horizontal");
	//     swiper_ak_1.centeredSlides = true;
	//   },
	// },
	breakpoints: {
		0: {
			slidesPerView: 1,
		},
		425: {
			slidesPerView: 2,
		},
		767: {
			slidesPerView: 3,
		},
		1024: {
			slidesPerView: 3,
		},
		1800: {
			slidesPerView: 5,
		},
	}
});

//swiper-ak-2 slider
const swiper_ak_2 = new Swiper(".swiper-ak-2", {
	direction: "horizontal",
	speed: 500,
	centeredSlides: window.outerWidth > 767 ? false : true,
	centerInsufficientSlides: window.outerWidth > 767 ? false : true,
	centeredSlidesBounds: window.outerWidth > 767 ? false : true,
	spaceBetween: 15,
	mousewheel: true,
	// pagination: {
	//   el: ".swiper-pagination",
	//   clickable: true,
	// },
	// navigation: {
	//   nextEl: '.next',
	//   prevEl: '.prev',
	// },
	// scrollbar: {
	//   el:'.swiper-scrollbar',
	// },
	on: {
		resize: function () {
			swiper_ak_2.changeDirection("horizontal");
			swiper_ak_2.centeredSlides = window.outerWidth > 767 ? false : true;
			swiper_ak_2.centerInsufficientSlides = window.outerWidth > 767 ? false : true;
			swiper_ak_2.centeredSlidesBounds = window.outerWidth > 767 ? false : true;
		},
	},
	breakpoints: {
		0: {
			slidesPerView: 2,
			centeredSlides: true,
		},
		425: {
			slidesPerView: 3,
			centeredSlides: true,
		},
		767: {
			slidesPerView: 3.5,
			centeredSlides: false,
		},
		1024: {
			slidesPerView: 4.5,
			centeredSlides: false,
		},
		1800: {
			slidesPerView: 6.5,
			centeredSlides: false,
		},
	}
});

//swiper-ak-2 slider
const swiper_ak_3 = new Swiper(".swiper-ak-3", {
	direction: "horizontal",
	speed: 500,
	centeredSlides: true,
	centerInsufficientSlides: true,
	centeredSlidesBounds: true,
	spaceBetween: 15,
	mousewheel: true,
	// pagination: {
	//   el: ".swiper-pagination",
	//   clickable: true,
	// },
	// navigation: {
	//   nextEl: '.next',
	//   prevEl: '.prev',
	// },
	// scrollbar: {
	//   el:'.swiper-scrollbar',
	// },
	on: {
		resize: function () {
			services_swiper.changeDirection("horizontal");
			services_swiper.centeredSlides = window.outerWidth > 767 ? false : true;
			services_swiper.centerInsufficientSlides = window.outerWidth > 767 ? false : true;
			services_swiper.centeredSlidesBounds = window.outerWidth > 767 ? false : true;
		},
	},
	breakpoints: {
		0: {
			slidesPerView: 1,
			centeredSlides: true,
		},
	}
});
