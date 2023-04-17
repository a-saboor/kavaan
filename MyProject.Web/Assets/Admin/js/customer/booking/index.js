'use strict'

//#region Upcoming Bookings

//#region Global Variables and Arrays
var upcomingSearch = null;
var upcomingPg = 1;
var upcomingPageSize = 8;
var upcomingStatus = "Pending";
var upcomingSortBy = 1;
var upcomingTotal = 0;

var upcomingFilter = {
    search: upcomingSearch,
    status: upcomingStatus,
    PageSize: upcomingPageSize,
    pageNumber: upcomingPg,
    sortBy: upcomingSortBy,
    Lang: lang,
}
//#endregion

//#region document ready function
$(document).ready(function () {

    $("#my-bookings .sort-by").val(upcomingSortBy);
    $('#my-bookings .upcoming .main-div').empty();
    $('#my-bookings .upcoming .loading').show();
    GetUpcomingFilterValues();
    GetUpcomingBookings();

    $('.change-status').click(function () {

        $(this).siblings().removeClass('active');
        $(this).addClass('active');

        $(`.booking-tabs`).hide();
        $(`.${$(this).attr('data-tab')}`).show();

    });

    $('#my-bookings .upcoming .load-more').click(function () {
        //if (upcomingPg < bookingTotalPages) {
        upcomingPg++;
        $('#my-bookings .upcoming .load-more').hide();
        $('#my-bookings .upcoming .loading').show();
        GetUpcomingFilterValues();
        GetUpcomingBookings();
        //}
    });
});
//#endregion

//#region fetch bookings

function GetUpcomingBookings() {

    let filter = upcomingFilter;

    $.ajax({
        type: 'POST',
        url: '/customer/booking/FilteredBookings',
        contentType: "application/json",
        data: JSON.stringify(filter),
        success: function (response) {
            BindBookings(response.data);
        }
    });

    function BindBookings(data) {
        upcomingTotal += data && data.length ? data.length : upcomingTotal;

        $.each(data, function (k, v) {
            /*${GetCurrencyAmount(v.Amount)*/
            

            var template = get_upcoming_template(v);

            $('#my-bookings .upcoming .main-div').append(template);

        });

        //setTimeout(function () { OnErrorImage(); }, 3000);
        $('#my-bookings .upcoming .loading').hide();

        if (data.FilteredRecords > upcomingTotal) {
            $("#my-bookings .upcoming .load-more").fadeIn();
        } else {
            $("#my-bookings .upcoming .load-more").fadeOut();
        }

        if ($('#my-bookings .upcoming .main-div').html().length == 0) {
            //$("html, body").animate({ scrollTop: 0 }, 1000);
            $('#my-bookings .upcoming .no-more').fadeIn();
            $("#my-bookings .upcoming .load-more").fadeOut();
        }

    }
}
//#endregion

//#region other functions

function GetUpcomingFilterValues() {
    upcomingFilter.search = upcomingSearch;
    upcomingFilter.status = upcomingStatus;
    upcomingFilter.PageSize = upcomingPageSize;
    upcomingFilter.pageNumber = upcomingPg;
    upcomingFilter.sortBy = upcomingSortBy;
}

function get_upcoming_template(v) {
    var status = {
        "Pending": {
            'title': 'Pending',
            'class': ' text-ak-gold'
        },
        "Inprocess": {
            'title': 'Inprocess',
            'class': ' text-ak-gold'
        },
        "Diagnosis": {
            'title': 'Diagnosis',
            'class': ' text-ak-gold'
        },
        "Invoiced": {
            'title': 'Invoiced',
            'class': ' text-ak-gold'
        },
    };

    var template = `
                <div class="flex flex-col flex-wrap my-4 items-center justify-between w-full upcoming-item-${v.ID}">

                    <div class="flex flex-row flex-wrap justify-between items-center w-full">
                        <p class="px-2">
                            <span class="text-black font-medium text-xs md:text-sm">Booking NO : </span>
                            <span class="text-ak-gold font-normal text-xs md:text-sm">${v.BookingNo}</span>
                        </p>
                        <p class="px-2">
                            <span class="text-black font-medium text-xs md:text-sm">Date : </span>
                            <span class="text-ak-gold font-normal text-xs md:text-sm">${v.CreationDate}</span>
                        </p>
                    </div>

                    <div class="bg-white border border-ak-gold/80 flex flex-row flex-wrap py-0 md:py-3 px-2 w-full md:w-full rounded-lg items-center justify-between mt-3">

                        <div class="flex flex-col items-center md:items-start mx-auto w-fit md:w-[70%] mt-4 md:mt-0 text-1 text-center">
                            <h6 class="text-xs md:text-lg text-ak-gray font-medium">${v.Category}</h6>
                            <hr class="w-full md:w-fit my-3">
                            <p class="py-1 rounded-[5px] flex flex-row justify-between items-center w-full md:w-fit">
                                <span class="text-ak-gray font-medium text-xs md:text-sm">Address</span>&nbsp;&nbsp;&nbsp;
                                <span class="text-ak-gold font-normal text-xs md:text-sm ">
                                    ${v.Address}
                                </span>
                            </p>
                            <hr class="w-full md:w-fit my-3">
                            <p class="py-1 rounded-[5px] flex flex-row justify-between items-center w-full md:w-fit">
                                <span class="text-ak-gray font-medium text-xs md:text-sm">Status</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <span class="text-ak-gold font-normal text-xs md:text-sm ${status[v.Status].class}">${v.Status}</span>
                            </p>
                        </div>

                        <div class="w-[80%] mx-auto mt-4 mb-2 md:hidden bg-ak-light h-[1px]"></div>

                        <div class="text-1 w-full md:w-fit flex flex-col items-center md:items-end justify-center md:justify-center md:mt-0 px-5 py-3">
                            <p class="text-ak-gray/60 font-medium text-xs md:text-sm my-2">${v.Service}</p>
                            <div class="flex flex-row flex-wrap md:flex-col">
                                <a class="cursor-pointer rounded-lg bg-ak-gold py-2 px-5 my-2 text-center text-white/90 font-light text-xs md:text-sm" onclick="AllDetailsBooking(this, ${v.ID})">
                                    All
                                    Details
                                </a>
                                <a class="cursor-pointer rounded-lg bg-ak-white border border-black/60 py-2 px-5 my-2 text-center text-ak-gray font-light text-xs md:text-sm" onclick="CancelBooking(this, ${v.ID})">Cancel</a>
                            </div>
                        </div>
                    </div>

                </div>

            `;

    return template;
}

//#endregion

//#endregion Upcoming Bookings

//#region Past Bookings

//#region Global Variables and Arrays
var pastSearch = null;
var pastPg = 1;
var pastPageSize = 8;
var pastStatus = "Completed";
var pastSortBy = 1;
var pastTotal = 0;

var pastFilter = {
    search: pastSearch,
    status: pastStatus,
    PageSize: pastPageSize,
    pageNumber: pastPg,
    sortBy: pastSortBy,
    Lang: lang,
}
//#endregion

//#region document ready function
$(document).ready(function () {

    $("#my-bookings .sort-by").val(pastSortBy);
    $('#my-bookings .past .main-div').empty();
    $('#my-bookings .past .loading').show();
    GetPastFilterValues();
    GetPastBookings();

    $('#my-bookings .past .load-more').click(function () {
        //if (pastPg < bookingTotalPages) {
        pastPg++;
        $('#my-bookings .past .load-more').hide();
        $('#my-bookings .past .loading').show();
        GetPastFilterValues();
        GetPastBookings();
        //}
    });
});
//#endregion

//#region fetch bookings

function GetPastBookings() {

    let filter = pastFilter;

    $.ajax({
        type: 'POST',
        url: '/customer/booking/FilteredBookings',
        contentType: "application/json",
        data: JSON.stringify(filter),
        success: function (response) {
            BindBookings(response.data);
        }
    });

    function BindBookings(data) {

        pastTotal += data && data.length ? data.length : pastTotal;

        $.each(data, function (k, v) {
            /*${GetCurrencyAmount(v.Amount)*/
        
            var template = get_past_template(v);

            $('#my-bookings .past .main-div').append(template);

        });

        //setTimeout(function () { OnErrorImage(); }, 3000);
        $('#my-bookings .past .loading').hide();

        if (data.FilteredRecords > pastTotal) {
            $("#my-bookings .past .load-more").fadeIn();
        } else {
            $("#my-bookings .past .load-more").fadeOut();
        }

        if ($('#my-bookings .past .main-div').html().length == 0) {
            //$("html, body").animate({ scrollTop: 0 }, 1000);
            $('#my-bookings .past .no-more').fadeIn();
            $("#my-bookings .past .load-more").fadeOut();
        }

    }
}
//#endregion

//#region other functions

function GetPastFilterValues() {
    pastFilter.search = pastSearch;
    pastFilter.status = pastStatus;
    pastFilter.PageSize = pastPageSize;
    pastFilter.pageNumber = pastPg;
    pastFilter.sortBy = pastSortBy;
}

function get_past_template(v) {

    var status = {
        "Cancelled": {
            'title': 'Cancelled',
            'class': ' text-ak-gold'
        },
        "Canceled": {
            'title': 'Canceled',
            'class': ' text-ak-gold'
        },
        "Completed": {
            'title': 'Completed',
            'class': ' text-ak-gold'
        },
    };

    var template = `
            <div class="flex flex-col flex-wrap my-4 items-center justify-between w-full past-item canceled-item">

                <div class="flex flex-row flex-wrap justify-between items-center w-full">
                    <p class="px-2">
                        <span class="text-black font-medium text-xs md:text-sm">Booking NO : </span>
                        <span class="text-ak-gold font-normal text-xs md:text-sm">${v.BookingNo}</span>
                    </p>
                    <p class="px-2">
                        <span class="text-black font-medium text-xs md:text-sm">Date : </span>
                        <span class="text-ak-gold font-normal text-xs md:text-sm">${v.CreationDate}</span>
                    </p>
                </div>

                <div class="bg-white border border-ak-gold/80 flex flex-row flex-wrap py-0 md:py-3 px-2 w-full md:w-full rounded-lg items-center justify-between mt-3">

                    <div class="flex flex-col items-center md:items-start mx-auto w-fit md:w-[70%] mt-4 md:mt-0 text-1 text-center">
                        <h6 class="text-xs md:text-lg text-ak-gray font-medium">${v.Category}</h6>
                        <hr class="w-full md:w-fit my-3">
                        <p class="py-1 rounded-[5px] flex flex-row justify-between items-center w-full md:w-fit">
                            <span class="text-ak-gray font-medium text-xs md:text-sm">Address</span>&nbsp;&nbsp;&nbsp;
                            <span class="text-ak-gold font-normal text-xs md:text-sm ">
                                ${v.Address}
                            </span>
                        </p>
                        <hr class="w-full md:w-fit my-3">
                        <p class="py-1 rounded-[5px] flex flex-row justify-between items-center w-full md:w-fit">
                            <span class="text-ak-gray font-medium text-xs md:text-sm">Status</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <span class="text-ak-gold font-normal text-xs md:text-sm ${status[v.Status].class}">${v.Status}</span>
                        </p>
                    </div>

                    <div class="w-[80%] mx-auto mt-4 mb-2 md:hidden bg-ak-light h-[1px]"></div>

                    <div class="text-1 w-full md:w-[30%] flex flex-col items-start md:items-end justify-center md:justify-end md:mt-0 px-5 py-3">
                        <p class="text-ak-gray/60 font-medium text-xs md:text-sm my-2 md:mb-auto">Deep Cleaning Service</p>
                        <p class="py-1 pt-0 rounded-[5px] flex flex-row justify-between items-center w-full md:w-fit md:mb-auto">
                            <span class="text-ak-gray font-medium text-xs md:text-sm">Total</span>&nbsp;&nbsp;&nbsp;
                            <span class="text-ak-gold font-normal text-xs md:text-sm ">AED 60.00</span>
                        </p>
                        <div class="flex flex-row flex-wrap md:flex-col ml-auto mr-auto md:ml-auto md:mr-0 md:mt-auto">
                            <a class="cursor-pointer rounded-lg bg-ak-gold py-2 px-5 my-2 text-center text-white/90 font-light text-xs md:text-sm" onclick="reviewBooking(this, ${v.ID})">
                                Write
                                a Review
                            </a>
                        </div>
                    </div>

                </div>

            </div>

        `;

    return template;
}

function appendCancelBooking(v) {

    var template = get_past_template(v);

    $('#my-bookings .past .main-div').append(template);
}

//#endregion

//#endregion Past Bookings

//#region submit functions 

/*cancel bookings*/

function CancelBooking(elem, id) {
                //modal_loader(true);

    $.ajax({
        type: 'GET',
        url: '/customer/booking/Cancel/'+ id,
        contentType: "application/json",
        data: id,
        success: function (response) {
            if (response) {
                $('body').append(response);
                $('#cancellation_form').show()/*.slideDown('fast', 'linear')*/;
                //modal_loader(false);
            } else {
                ToastrMessage(ServerErrorShort);
            }
        }
    });
}

function simpleFormSubmit_callback(form, status, response, url) {
    if (status) {
        var data = response.data;
        successPopup($(form).closest('.popup-section'));
        $(".upcoming-item-" + data.ID).remove();
        let booking = get_past_template(data);
        $('#my-bookings .past .main-div').append(booking);
    }
}


/*review bookings*/

function reviewBooking(elem, id) {
    //modal_loader(true);
    
    $.ajax({
        type: 'GET',
        url: '/customer/booking/review/' + id,
        contentType: "application/json",
        data: id,
        success: function (response) {
            if (response) {
                $('body').append(response);
                $('#review_form').show()/*.slideDown('fast', 'linear')*/;
                //modal_loader(false);
            } else {
                ToastrMessage(ServerErrorShort);
            }
        }
    });
}


/* All Details */
function AllDetailsBooking(elem, id) {
    $.ajax({
        type: 'GET',
        url: '/customer/booking/update/' + id,
        contentType: "application/json",
        data: id,
        success: function (response) {
            if (response) {
                $('body').append(response);
                $('#booking_details_section').show()/*.slideDown('fast', 'linear')*/;
            } else {
                ToastrMessage(ServerErrorShort);
            }
        }
    });
}
//#endregion submit functions 