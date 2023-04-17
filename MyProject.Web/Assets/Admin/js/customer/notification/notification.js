'use strict'

//#region Global Variables and Arrays
var PageNo = 1;
var MoreRecordExist = true;
var IsUnseenRecoredExist = false;
var IsUnReadRecoredExist = false;
var UnSeenMessages = 0;
var currentRecords = 0;
//var lang = "en";

//#endregion

//#region document ready function
$(document).ready(function () {

	$('#notifications .main-div').empty();
	$('#notifications .loading').show();

	GetArticles();

	$("#notifications").scroll(function () {
		
		if (($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) && MoreRecordExist) {
			$('#notifications .loading').show();
			PageNo++;
			GetArticles();
		}
	});

});
//#endregion

//#region Ajax Call


function GetArticles() {
	$.ajax({
		type: 'Get',
		url: '/Customer/Notification/LoadNotifications?pageNo=' + PageNo + '&culture=' + culture,
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
		
		currentRecords += response.data.length;
		if (response.TotalRecord == currentRecords) {
			MoreRecordExist = false;
		}

		UnSeenMessages = response.data.filter(function (obj) {
			return obj.IsSeen == false;
		}).length;

		if (UnSeenMessages > 0) {
			IsUnseenRecoredExist = true;
		}

		IsUnReadRecoredExist = response.data.filter(function (obj) {
			return obj.IsRead == false;
		}).length > 0 ? true : false;

		$.each(response.data, function (k, v) {
			$('#notifications .main-div').append(`

				<div class="bg-white border border-ak-gold py-4 px-8 rounded-lg flex flex-row flex-wrap items-center justify-between w-full">
					<div class="text">
						<h6 class="text-xs md:text-lg leading-[1rem] md:leading-[1.5rem] text-[#707070] font-medium">
							${v.Title}
						</h6>
						<p class="p text-xs md:text-base leading-[1rem] md:leading-[1.5rem]">${v.Description}</p>
					</div>
					<div class="time ml-auto">
						<p class="text-ak-gold text-xs">${v.Date}</p>
						<p class="leading-[1rem] md:leading-[1.5rem] rounded-full text-ak-blue text-center text-xs new-notification">${v.IsDelivered ? "" : "New"}</p>
					</div>
				</div>

			`);

		});
	}

	//setTimeout(function () { OnErrorImage(); }, 3000);
	$('#notifications .loading').hide();

	//if (response.data && response.data.length >= PageSize) {
	//	$("#notifications .see-more").fadeIn();
	//} else {
	//	$("#notifications .see-more").fadeOut();
	//}

	//if ($('#notifications .main-div section')) {
	//	$('#notifications .main-div section').remove();
	//}

	if ($('#notifications .main-div').html().length == 0) {
		//$("html, body").animate({ scrollTop: 0 }, 1000);
		$('#notifications .no-more').fadeIn();
		//$("#notifications .see-more").fadeOut();
	}
}

//#endregion
