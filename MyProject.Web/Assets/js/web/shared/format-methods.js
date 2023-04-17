'use strict';

//#region Global Variables and Arrays

//Nationalities
const allCountries = `
<option value="afghan">Afghan</option>
<option value="albanian">Albanian</option>
<option value="algerian">Algerian</option>
<option value="american">American</option>
<option value="andorran">Andorran</option>
<option value="angolan">Angolan</option>
<option value="antiguans">Antiguans</option>
<option value="argentinean">Argentinean</option>
<option value="armenian">Armenian</option>
<option value="australian">Australian</option>
<option value="austrian">Austrian</option>
<option value="azerbaijani">Azerbaijani</option>
<option value="bahamian">Bahamian</option>
<option value="bahraini">Bahraini</option>
<option value="bangladeshi">Bangladeshi</option>
<option value="barbadian">Barbadian</option>
<option value="barbudans">Barbudans</option>
<option value="batswana">Batswana</option>
<option value="belarusian">Belarusian</option>
<option value="belgian">Belgian</option>
<option value="belizean">Belizean</option>
<option value="beninese">Beninese</option>
<option value="bhutanese">Bhutanese</option>
<option value="bolivian">Bolivian</option>
<option value="bosnian">Bosnian</option>
<option value="brazilian">Brazilian</option>
<option value="british">British</option>
<option value="bruneian">Bruneian</option>
<option value="bulgarian">Bulgarian</option>
<option value="burkinabe">Burkinabe</option>
<option value="burmese">Burmese</option>
<option value="burundian">Burundian</option>
<option value="cambodian">Cambodian</option>
<option value="cameroonian">Cameroonian</option>
<option value="canadian">Canadian</option>
<option value="cape verdean">Cape Verdean</option>
<option value="central african">Central African</option>
<option value="chadian">Chadian</option>
<option value="chilean">Chilean</option>
<option value="chinese">Chinese</option>
<option value="colombian">Colombian</option>
<option value="comoran">Comoran</option>
<option value="congolese">Congolese</option>
<option value="costa rican">Costa Rican</option>
<option value="croatian">Croatian</option>
<option value="cuban">Cuban</option>
<option value="cypriot">Cypriot</option>
<option value="czech">Czech</option>
<option value="danish">Danish</option>
<option value="djibouti">Djibouti</option>
<option value="dominican">Dominican</option>
<option value="dutch">Dutch</option>
<option value="east timorese">East Timorese</option>
<option value="ecuadorean">Ecuadorean</option>
<option value="egyptian">Egyptian</option>
<option value="emirati">Emirati</option>
<option value="equatorial guinean">Equatorial Guinean</option>
<option value="eritrean">Eritrean</option>
<option value="estonian">Estonian</option>
<option value="ethiopian">Ethiopian</option>
<option value="fijian">Fijian</option>
<option value="filipino">Filipino</option>
<option value="finnish">Finnish</option>
<option value="french">French</option>
<option value="gabonese">Gabonese</option>
<option value="gambian">Gambian</option>
<option value="georgian">Georgian</option>
<option value="german">German</option>
<option value="ghanaian">Ghanaian</option>
<option value="greek">Greek</option>
<option value="grenadian">Grenadian</option>
<option value="guatemalan">Guatemalan</option>
<option value="guinea-bissauan">Guinea-Bissauan</option>
<option value="guinean">Guinean</option>
<option value="guyanese">Guyanese</option>
<option value="haitian">Haitian</option>
<option value="herzegovinian">Herzegovinian</option>
<option value="honduran">Honduran</option>
<option value="hungarian">Hungarian</option>
<option value="icelander">Icelander</option>
<option value="indian">Indian</option>
<option value="indonesian">Indonesian</option>
<option value="iranian">Iranian</option>
<option value="iraqi">Iraqi</option>
<option value="irish">Irish</option>
<option value="israeli">Israeli</option>
<option value="italian">Italian</option>
<option value="ivorian">Ivorian</option>
<option value="jamaican">Jamaican</option>
<option value="japanese">Japanese</option>
<option value="jordanian">Jordanian</option>
<option value="kazakhstani">Kazakhstani</option>
<option value="kenyan">Kenyan</option>
<option value="kittian and nevisian">Kittian and Nevisian</option>
<option value="kuwaiti">Kuwaiti</option>
<option value="kyrgyz">Kyrgyz</option>
<option value="laotian">Laotian</option>
<option value="latvian">Latvian</option>
<option value="lebanese">Lebanese</option>
<option value="liberian">Liberian</option>
<option value="libyan">Libyan</option>
<option value="liechtensteiner">Liechtensteiner</option>
<option value="lithuanian">Lithuanian</option>
<option value="luxembourger">Luxembourger</option>
<option value="macedonian">Macedonian</option>
<option value="malagasy">Malagasy</option>
<option value="malawian">Malawian</option>
<option value="malaysian">Malaysian</option>
<option value="maldivan">Maldivan</option>
<option value="malian">Malian</option>
<option value="maltese">Maltese</option>
<option value="marshallese">Marshallese</option>
<option value="mauritanian">Mauritanian</option>
<option value="mauritian">Mauritian</option>
<option value="mexican">Mexican</option>
<option value="micronesian">Micronesian</option>
<option value="moldovan">Moldovan</option>
<option value="monacan">Monacan</option>
<option value="mongolian">Mongolian</option>
<option value="moroccan">Moroccan</option>
<option value="mosotho">Mosotho</option>
<option value="motswana">Motswana</option>
<option value="mozambican">Mozambican</option>
<option value="namibian">Namibian</option>
<option value="nauruan">Nauruan</option>
<option value="nepalese">Nepalese</option>
<option value="new zealander">New Zealander</option>
<option value="ni-vanuatu">Ni-Vanuatu</option>
<option value="nicaraguan">Nicaraguan</option>
<option value="nigerien">Nigerien</option>
<option value="north korean">North Korean</option>
<option value="northern irish">Northern Irish</option>
<option value="norwegian">Norwegian</option>
<option value="omani">Omani</option>
<option value="pakistani">Pakistani</option>
<option value="palauan">Palauan</option>
<option value="panamanian">Panamanian</option>
<option value="papua new guinean">Papua New Guinean</option>
<option value="paraguayan">Paraguayan</option>
<option value="peruvian">Peruvian</option>
<option value="palestinian">Palestinian</option>
<option value="polish">Polish</option>
<option value="portuguese">Portuguese</option>
<option value="qatari">Qatari</option>
<option value="romanian">Romanian</option>
<option value="russian">Russian</option>
<option value="rwandan">Rwandan</option>
<option value="saint lucian">Saint Lucian</option>
<option value="salvadoran">Salvadoran</option>
<option value="samoan">Samoan</option>
<option value="san marinese">San Marinese</option>
<option value="sao tomean">Sao Tomean</option>
<option value="saudi">Saudi</option>
<option value="scottish">Scottish</option>
<option value="senegalese">Senegalese</option>
<option value="serbian">Serbian</option>
<option value="seychellois">Seychellois</option>
<option value="sierra leonean">Sierra Leonean</option>
<option value="singaporean">Singaporean</option>
<option value="slovakian">Slovakian</option>
<option value="slovenian">Slovenian</option>
<option value="solomon islander">Solomon Islander</option>
<option value="somali">Somali</option>
<option value="south african">South African</option>
<option value="south korean">South Korean</option>
<option value="spanish">Spanish</option>
<option value="sri lankan">Sri Lankan</option>
<option value="sudanese">Sudanese</option>
<option value="surinamer">Surinamer</option>
<option value="swazi">Swazi</option>
<option value="swedish">Swedish</option>
<option value="swiss">Swiss</option>
<option value="syrian">Syrian</option>
<option value="taiwanese">Taiwanese</option>
<option value="tajik">Tajik</option>
<option value="tanzanian">Tanzanian</option>
<option value="thai">Thai</option>
<option value="togolese">Togolese</option>
<option value="tongan">Tongan</option>
<option value="trinidadian or tobagonian">Trinidadian or Tobagonian</option>
<option value="tunisian">Tunisian</option>
<option value="turkish">Turkish</option>
<option value="tuvaluan">Tuvaluan</option>
<option value="ugandan">Ugandan</option>
<option value="ukrainian">Ukrainian</option>
<option value="uruguayan">Uruguayan</option>
<option value="uzbekistani">Uzbekistani</option>
<option value="venezuelan">Venezuelan</option>
<option value="vietnamese">Vietnamese</option>
<option value="welsh">Welsh</option>
<option value="yemenite">Yemenite</option>
<option value="zambian">Zambian</option>
<option value="zimbabwean">Zimbabwean</option>
`;

//#endregion

//#region Layout Javascript Queries

//#endregion

//#region document ready function
$(document).ready(function () {

});
//#endregion

//#region Functions
function isNumber(evt) {
	evt = (evt) ? evt : window.event;
	var charCode = (evt.which) ? evt.which : evt.keyCode;
	if (charCode > 31 && (charCode < 48 || charCode > 57)) {
		return false;
	}
	return true;
}

function isString(evt) {
	evt = (evt) ? evt : window.event;
	var charCode = (evt.which) ? evt.which : evt.keyCode;
	if ((charCode < 65 || charCode > 90) && (charCode < 97 || charCode > 123) && charCode != 32) {
		return false;
	}
	return true;
}

function dateFormat() {
}

function disableSubmitButton(btn, flag = true, spanElement = "", btnSpinner = false) {

	if (btn[0].localName == 'button') {
		
		if (spanElement) {
			if (flag) {
				if (btnSpinner) {
					$(btn).prepend('<i class="fa fa-circle-notch fa-spin mr-10 text-xs mb-[auto] mt-1"></i>').attr('disabled', true);
					//$(btn).html('<i class="fa fa-circle-notch fa-spin mr-1"></i>' + $(btn).text()).attr('disabled', true);
				}
				else {
					$(btn).attr('disabled', true);
				}
			}
			else {
				//$(btn).html('<i class="fa ' + spanElement + ' mr-1 mt-1"></i>' + $(btn).text()).attr('disabled', false);
			}
		}
		else {
			if (flag) {
				if (btnSpinner) {
					$(btn).attr('disabled', true).prepend('<i class="fa fa-circle-notch fa-spin mr-10 text-xs mb-[auto] mt-1"></i>');
					//$(btn).html('<i class="fa fa-circle-notch fa-spin mr-1"></i>' + $(btn).text()).attr('disabled', true);
					//$(btn).html('<i class="fa fa-circle-notch fa-spin mr-1"></i>' + $(btn).text()).attr('disabled', true);
				}
				else {
					$(btn).attr('disabled', true);
				}
			}
			else {
				if (btnSpinner) {
					$(btn).attr('disabled', false).find('i').remove();
				}
				else {
					$(btn).attr('disabled', false);
				}
			}
		}
	}
	else {

	}
}

function return_loading_section(colspan = 0) {
	var fa_spin = "";
	if (colspan) {
		fa_spin = `<tr>
						<td colspan="${colspan}">
							<section class="flex flex-col my-auto py-2 text-center w-[100%] loading" style=""><i class="mt-1 mx-auto dot-flashing text-center w-[100%]"></i></section>
						</td>
					</tr>`;
	}
	else {
		fa_spin = `<section class="flex flex-col my-auto py-2 text-center w-[100%] loading" style=""><i class="mt-1 mx-auto dot-flashing text-center w-[100%]"></i></section>`;
	}
	return fa_spin;
}

function goBack() {
	window.location = document.referrer;
	//window.history.go(-1); //this method cannot change the state of previous page.
}

function ButtonDisabled(id, flag = false, span = false) {
	if (span) {
		if (flag) {
			$(id).html('<span class="fa fa-circle-notch fa-spin ' + margin(1) + '"> </span> ' + $(id).text()).attr('disabled', flag);
		}
		else {
			$(id).html($(id).text()).attr('disabled', flag);
		}
	}
	else {
		$(id).html($(id).text()).attr('disabled', flag);
	}
}

function margin(num) {
	return lang == "en" ? "mr-" + num : "ml-" + num;
}

function padding(num) {
	return lang == "en" ? "pr-" + num : "pl-" + num;
}

function PageReload(url, timeup = 3) {
	url = url ? url : window.location.href;
	setTimeout(function () {
		window.location.href = url;
	}, timeup * 1000);
}

function GetCurrencyAmount(amount) {
	return `${currency} ${amount}`;
};

function BorderDangerInput(id, timeup = 0) {

	$(id).closest('div').addClass('border border-danger');

	if (timeup && timeup > 0) {
		setTimeout(function () {
			$(id).closest('div').removeClass('border border-danger');
		}, (timeup * 1000));
	}
}
function BorderDangerInputRemove(id, timeup) {
	setTimeout(function () {
		$(id).closest('div').removeClass('border border-danger');
	}, (timeup * 1000));
}

function scroll_top() {
	$("#site-scroll").on("click", function () {
		//var body = $("html, body");
		//body.stop().animate({ scrollTop: 0 }, 500, 'swing');
		$("html, body").animate({ scrollTop: 0 }, 1000);
		return false;
	});
}
scroll_top();

//set video element width height using its poster image
function SetElementHeight(elem) {
	$(elem).height($(elem).height());
};

//resize height of the element
function changeElementHeight(id, height) {
	document.getElementById(id).style.height = height + "px";
}

//owl carousel
function owlCarousel_1() {
	$('.owl-carousel').owlCarousel({
		loop: true,
		navText: ["<img src='/assets/images/web-icons/left-arrow.png'>",
			"<img src='/assets/images/web-icons/right-arrow.png'>"
		],
		nav: true,
		margin: 16,
		dots: false,
		responsiveClass: true,
		responsive: {
			0: {
				items: 1,
			},
			600: {
				items: 2,
			},
			1000: {
				items: 3,
			}
		}
	})
}

//#endregion

//#region Play Functions
function PlayVideo(elem) {

	if (elem.localName == "video") {
		if ($(elem).hasClass('played')) {
			$(elem).closest('div').find('img.control').attr('src', '/assets/images/web-icons/hero-play-icon.png');
			$(elem).removeClass('played').trigger('pause');
		} else {
			$(elem).closest('div').find('img.control').attr('src', '/assets/images/web-icons/hero-pause-icon.png');
			$(elem).addClass('played').trigger('play');
		}
	} else {

		if ($(elem).prevAll('video').hasClass('played')) {
			$(elem).attr('src', '/assets/images/web-icons/hero-play-icon.png');
			$(elem).prevAll('video').removeClass('played').trigger('pause');
		} else {
			$(elem).attr('src', '/assets/images/web-icons/hero-pause-icon.png');
			$(elem).prevAll('video').addClass('played').trigger('play');
		}
	}
}
//#endregion

//set video element width height using its poster image
function SetElementHeight(elem) {
	$(elem).height($(elem).height());
};

//set video element width height
$(window).resize(function () {
	$('#banner_video').height("auto");
	$('#banner_video').height($('#banner_video').height() - $('#banner_video').height() / 4.5);
});

//swap video and image
function changeContent(elem) {
	const content = $(elem).attr('data-content');
	if (content === "ak-video") {
		$(elem).closest(".content-div").find(".ak-video").show();
		$(elem).closest(".content-div").find(".ak-image").hide();
		$(elem).attr('data-content', 'ak-image');
		$(elem).removeClass('fa-video');
		$(elem).addClass('fa-image');
	} else {
		$(elem).closest(".content-div").find(".ak-video").hide();
		$(elem).closest(".content-div").find(".ak-image").show();
		$(elem).attr('data-content', 'ak-video');
		$(elem).addClass('fa-video');
		$(elem).removeClass('fa-image');
	}
}

//#region Modal functions

//div like modal show function
function showModalDiv(id, flag) {
	if (flag) {
		$('#' + id).show();
	}
	else {
		$('#' + id).hide();
	}
}
//#endregion