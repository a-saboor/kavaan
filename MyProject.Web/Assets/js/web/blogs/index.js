'use strict'

//#region Global Variables and Arrays
var pg = 1;
var PageSize = 9;
var isPageRendered = false;
var totalPages;
//var lang = "en";

var filter = {
    search: null,
    PageSize: 9,
    pageNumber: 1,
    sortBy: 1
}

//#endregion

//#region document ready function
$(document).ready(function () {

    $('#blogs_section .main-div').empty().html();
    hideGridLoading('#blogs_section', false);

    GetFilterValues();
    GetArticles();

    $('#ArticleSearch').change(function () {
        if ($('#ArticleSearch').val()) {
            $('#blogs_section .main-div').empty();
            hideGridLoading('#blogs_section', false);
            pg = 1;
            GetFilterValues();
            GetArticles();
        }
    });

    $('#btnSearch').click(function () {
        $('#blogs_section .main-div').empty();
        hideGridLoading('#blogs_section', false);
        pg = 1;
        GetFilterValues();
        GetArticles();
    });

    $('#SortBy').change(function () {
        $('#blogs_section .main-div').empty();
        hideGridLoading('#blogs_section', false);
        pg = 1;
        GetFilterValues();
        GetArticles();
    });

    $('#blogs_section .see-more').click(function () {
        //if (pg < totalPages) {
        pg++;
        $('#blogs_section .see-more').hide();
        hideGridLoading('#blogs_section', false);
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
        url: '/blogs/filters',
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

        $('#blogs_section .main-div').append(`
			
			<div class="col-lg-4 col-sm-6 col-12">
				<div class="ltn__blog-item ltn__blog-item-3">
					<div class="ltn__blog-img">
						<a href="/blogs/${v.Slug}"><img src="${v.Image}" alt="#"></a>
					</div>
					<div class="ltn__blog-brief">
						<div class="ltn__blog-meta">
							<ul>
								<li class="ltn__blog-author">
									<a href="#"><i class="far fa-user"></i>by: ${v.Author}</a>
								</li>
								<li class="ltn__blog-tags">
									<a href="#"><i class="fas fa-tags"></i>${v.Title}</a>
								</li>
							</ul>
						</div>
						<h3 class="ltn__blog-title"><a href="/blogs/${v.Slug}">${v.Description}</a></h3>
						<div class="ltn__blog-meta-btn">
							<div class="ltn__blog-meta">
								<ul>
									<li class="ltn__blog-date"><i class="far fa-calendar-alt"></i>${v.FullDate}</li>
								</ul>
							</div>
							<div class="ltn__blog-btn">
								<a href="/blogs/${v.Slug}">Read more</a>
							</div>
						</div>
					</div>
				</div>
			</div>

		`);

    });

    //setTimeout(function () { OnErrorImage(); }, 3000);
    hideGridLoading('#blogs_section', true);

    if (data && data.length >= PageSize) {
        $("#blogs_section .see-more").fadeIn();
    } else {
        $("#blogs_section .see-more").fadeOut();
    }

    if ($('#blogs_section .main-div').html().length == 0) {
        $("html, body").animate({ scrollTop: 0 }, 1000);
        $('#blogs_section .no-more').fadeIn();
        $("#blogs_section .see-more").fadeOut();
    }
    else {
        $('#blogs_section .no-more').fadeOut();
    }
    OnErrorImage(0.5, 'blog');
}

function GetFilterValues() {
    filter.search = $("#ArticleSearch").val();
    filter.pageNumber = pg;
    filter.sortBy = $("#SortBy").val();
}

//#endregion
