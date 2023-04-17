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

            <tr>
                <td class="ltn__my-properties-img">
                    <a href="/projects/${v.Slug}"><img src="${v.Thumbnail}" alt="${v.Name}"></a>
                </td>
                <td>
                    <div class="ltn__my-properties-info">
                        <h6 class="mb-10"><a href="/projects/${v.Slug}">${v.Name}</a></h6>
                        <small><i class="icon-placeholder"></i> ${v.Address}</small>
                        <small><i class="icon-tag"></i> ${v.Purpose}</small>
                        ${/*<div class="product-ratting">
                            <ul>
                                <li><a href="#"><i class="fas fa-star"></i></a></li>
                                <li><a href="#"><i class="fas fa-star"></i></a></li>
                                <li><a href="#"><i class="fas fa-star"></i></a></li>
                                <li><a href="#"><i class="fas fa-star-half-alt"></i></a></li>
                                <li><a href="#"><i class="far fa-star"></i></a></li>
                                <li class="review-total"> <a href="#"> ( 95 Reviews )</a></li>
                            </ul>
                        </div>*/''}
                    </div>
                </td>
                <td>${v.Bedrooms}</td>
                <td>${v.Baths}</td>
                <td><a href="#"><i class="fa-solid fa-trash-can"></i></a></td>
            </tr>
	        
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
