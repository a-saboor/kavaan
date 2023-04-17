'use strict'

//#region Global Variables and Arrays
var couponPageNo = 1;
var couponMoreRecordExist = true;
var couponCurrentRecords = 0;
//var lang = "en";

//#endregion

//#region document ready function
$(document).ready(function () {

	$('#coupons .main-div').empty();
	$('#coupons .loading').show();

	GetArticles();

	$("#coupons").scroll(function () {

		if (($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) && couponMoreRecordExist) {
			$('#coupons .loading').show();
			couponPageNo++;
			GetArticles();
		}
	});

});
//#endregion

//#region Ajax Call

function GetArticles() {
	$.ajax({
		type: 'Get',
		url: '/Customer/Coupon/LoadCoupons?pageNo=' + couponPageNo + '&culture=' + culture,
		success: function (response) {
			if (response.success) {
				BindGridArticles(response);
			}
		}
	});
}

//#endregion

//#region Functions for Binding Data

function BindGridArticles(response) {

	if (response.data.length > 0) {

		couponCurrentRecords += response.data.length;
		if (response.TotalRecord == couponCurrentRecords) {
			couponMoreRecordExist = false;
		}

		$.each(response.data, function (k, v) {

			const available = v.Status.toLowerCase() == 'available' ? 1 : 0;
			const getCode = `onclick=CopyCode(this,'${v.CouponCode}');`;

			$('#coupons .main-div').append(`
		
				<div class="bg-[#EEEBE7] rounded-lg flex flex-col md:flex-row flex-wrap items-center justify-between w-full">
					<div class="flex items-center justify-center w-full md:w-[20%] rounded-t-lg border border-[#EEEBE7] md:rounded-t-none">
						<img src="/assets/images/web-icons/coupon.png" class="w-12 md:w-20 py-2">
					</div>
					<div class="bg-white border-t-0 border-l md:border-t md:border-l-[0] border-r border-b border-ak-gold/80 flex flex-row flex-wrap py-6 w-full md:w-[80%] rounded-b-lg md:rounded-l-none md:rounded-r-lg items-center justify-between">
						<div class="flex flex-col items-center md:items-start md:w-[70%] px-10 text-1 text-center w-full">
							<h6 class="text-xs md:text-lg text-black font-noraml">${v.Name}</h6>
							<p class="bg-[#EEEBE7] my-3 px-4 py-1 rounded-[5px] flex flex-row justify-between items-center md:w-[50%] w-fit">
								<span class="text-black/60 font-light text-xs md:text-sm">Value</span>&nbsp;&nbsp;&nbsp;<span class="text-black font-normal text-xs md:text-sm ">AED ${v.Value}</span>
							</p>
							<p class="text-black font-light text-xs md:text-sm">${available ? "Valid Until " + v.Expiry : ""}</p>
						</div>

						<div class="w-[80%] mx-auto mt-4 mb-2 md:hidden bg-ak-light h-[1px]"></div>

						<div class="text-1 w-full md:w-[30%] text-center mt-4 md:mt-0">
							<p class="text-black/60 font-light text-xs md:text-sm">COUPON CODE</p>
							<p class="rounded-[5px] bg-[#EEEBE7] py-0.5 px-2 my-3 text-center mx-auto text-black font-normal text-xs md:text-sm w-fit">
								${v.CouponCode}
							</p>
							<a ${available ? getCode : ""} class="${available ? "cursor-pointer bg-ak-gold" : "bg-[#00000040] cursor-default" } rounded-[5px] py-1.5 px-2 mt-0.5 my-3 text-center mx-auto text-white/90 font-light text-xs md:text-sm" title="${available ? "Copy Coupon Code" : "Coupon not Available"}">
								${available ? "Get a Code" : v.Status}
							</a>
						</div>
					</div>
				</div>

			`);

		});
	}

	//setTimeout(function () { OnErrorImage(); }, 3000);
	$('#coupons .loading').hide();

	//if (response.data && response.data.length >= PageSize) {
	//	$("#coupons .see-more").fadeIn();
	//} else {
	//	$("#coupons .see-more").fadeOut();
	//}

	//if ($('#coupons .main-div section')) {
	//	$('#coupons .main-div section').remove();
	//}

	if ($('#coupons .main-div').html().length == 0) {
		//$("html, body").animate({ scrollTop: 0 }, 1000);
		$('#coupons .no-more').fadeIn();
		//$("#coupons .see-more").fadeOut();
	}
}

//#endregion

//copy to clipboard code
const tempInput = $("<input>");
function CopyCode(elem, code) {
	$(elem).append(tempInput);
	tempInput.val(code).select();
	document.execCommand("copy");
	tempInput.remove();
	//show message popup
	ToastrMessage(fa_icon_success, ChangeString('Copied to clipboard.', 'نسخ إلى الحافظة.'), "", 6);
}