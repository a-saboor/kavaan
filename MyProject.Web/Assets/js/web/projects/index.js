'use strict'

//#region Global Variables and Arrays
var pg = 1;
var PageSize = 6;
var isPageRendered = false;
var totalPages;
//var lang = "en";

var filter = {
    search: null,
    PageSize: 6,
    pageNumber: 1,
    sortBy: 1,
    parentID: $('#ParentID').val()
}

//#endregion

//#region document ready function
$(document).ready(function () {

    $('#projects_section .main-div').empty().html();
    hideGridLoading('#projects_section', false);

    GetFilterValues();
    GetArticles();

    $('#ArticleSearch').change(function () {
        //if ($('#ArticleSearch').val()) {
            $('#projects_section .main-div').empty();
            hideGridLoading('#projects_section', false);
            pg = 1;
            GetFilterValues();
            GetArticles();
        //}
    });


    $('#ArticleSearchBtn').click(function () {
        //if ($('#ArticleSearch').val()) {
            $('#projects_section .main-div').empty();
            hideGridLoading('#projects_section', false);
            pg = 1;
            GetFilterValues();
            GetArticles();
        //}
    });

    $('#btnSearch').click(function () {
        $('#projects_section .main-div').empty();
        hideGridLoading('#projects_section', false);
        pg = 1;
        GetFilterValues();
        GetArticles();
    });

    $('#SortBy').change(function () {
        $('#projects_section .main-div').empty();
        hideGridLoading('#projects_section', false);
        pg = 1;
        GetFilterValues();
        GetArticles();
    });

    $('#ParentID').change(function () {
        $('#projects_section .main-div').empty();
        hideGridLoading('#projects_section', false);
        pg = 1;
        GetFilterValues();
        GetArticles();
    });

    $('#projects_section .see-more').click(function () {
        //if (pg < totalPages) {
        pg++;
        $('#projects_section .see-more').hide();
        hideGridLoading('#projects_section', false);
        $(".filter-loader").show();
        GetFilterValues();
        GetArticles();
        //}
    });


    $('#No_Submit').submit(function (e) {
        e.preventDefault();
        return false;
    });

});
//#endregion

//#region Ajax Call

function GetArticles() {
    $.ajax({
        type: 'POST',
        url: '/projects/filters',
        contentType: "application/json",
        data: JSON.stringify(filter),
        success: function (response) {
            $('.project-count').html(`<span>Showing ${response.count} results</span>`);
            BindGridArticles(response.data);
        }
    });
}

//#endregion

//#region Functions for Binding Data

function BindGridArticles(data) {

    $.each(data, function (k, v) {

        $('#projects_section .main-div').append(`
			
	         <div class="col-lg-4 col-sm-6 col-12">
                <div class="ltn__product-item ltn__product-item-4 ltn__product-item-5 text-center---">
                    <div class="product-img">
                        <a href="/projects/${v.Slug}"><img src="${v.Thumbnail}" alt="${v.Name}"></a>
                    </div>
                    <div class="product-info">
                        <div class="product-badge">
                            <ul>
                                <li class="sale-badg">${v.Purpose}</li>
                            </ul>
                        </div>
                        <h2 class="product-title"><a href="/projects/${v.Slug}">${v.Name}</a></h2>
                        <div class="product-img-location">
                            <ul>
                                <li>
                                    <a href="/projects/${v.Slug}"><i class="flaticon-pin"></i> ${v.Address}</a>
                                </li>
                            </ul>
                        </div>
                        <ul class="ltn__list-item-2--- ltn__list-item-2-before--- ltn__plot-brief">
                            <li>
                                <span>${v.Bedrooms} </span>
                                Bedrooms
                            </li>
                            <li>
                                <span>${v.Baths} </span>
                                Bathrooms
                            </li>
                            <li>
                                <span>${v.AreaStart} - ${v.AreaEnd} </span>
                                square Ft
                            </li>
                        </ul>
                        <div class="product-hover-action">
                            <ul>
                                <li>
                                    <a href="javascript:void(0);" title="Quick View" data-bs-toggle="modal" data-bs-target="#quick_view_modal">
                                        <i class="flaticon-expand"></i>
                                    </a>
                                </li>
                                <li>
                                    <a href="javascript:void(0);" title="Wishlist" data-bs-toggle="modal" data-bs-target="#liton_wishlist_modal">
                                        <i class="flaticon-heart-1"></i>
                                    </a>
                                </li>
                                <li>
                                    <a href="/projects/${v.Slug}" title="Project Details">
                                        <i class="flaticon-add"></i>
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="product-info-bottom">
                        <div class="product-price">
                            <span>PKR ${v.PriceStart} - ${v.PriceEnd}<label></label></span>
                        </div>
                    </div>
                </div>
            </div>

		`);

    });

    //setTimeout(function () { OnErrorImage(); }, 3000);
    hideGridLoading('#projects_section', true);

    if (data && data.length >= PageSize) {
        $("#projects_section .see-more").fadeIn();
    } else {
        $("#projects_section .see-more").fadeOut();
    }

    if ($('#projects_section .main-div').html().length == 0) {
        $("html, body").animate({ scrollTop: 0 }, 1000);
        $('#projects_section .no-more').fadeIn();
        $("#projects_section .see-more").fadeOut();
    }
    else {
        $('#projects_section .no-more').fadeOut();
    }

    OnErrorImage(0.5, 'blog');
}

function GetFilterValues() {
    filter.search = $("#ArticleSearch").val();
    filter.pageNumber = pg;
    filter.sortBy = $("#SortBy").val();
    filter.parentID = $("#ParentID").val();
}

//#endregion
