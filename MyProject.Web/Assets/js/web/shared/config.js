'use strict';

//#region Global Variables and Arrays

//#endregion Global Variables and Arrays

//#region document ready function
$(document).ready(function () {

    //cookie policy content setting
    //let cookie_consent = getCookie("user_cookie_consent");
    //if (cookie_consent != "") {
    //	document.getElementById("cookieNotice").style.display = "none";
    //} else {
    //	document.getElementById("cookieNotice").style.display = "block";
    //}

    ////change target of void links.
    //$('a[href="javascript:void(0);"').attr('target', '_top');

    //change active link of mobile and desktop nav.
    $('#MobileMenu .nav-items-mobile .nav-items-li').removeClass("active");
    $('#MobileMenu .nav-items-mobile .nav-items-li[href="' + current_url_path + '"').addClass("active");

    $('#desktop_menu .nav-items-li').removeClass("active");
    $('#desktop_menu .nav-items-li a[href="' + current_url_path + '"').parent().addClass("active");

    //initiate all helping functions
    initAllFunc();

    /* Fetch and Bind Functions */
    FetchBusinessSetting();

    /* Form Submit */
    //signup form for newsletter
    $('#form_subscribe').submit(function (e) {
        const btn = $(this);
        const url = `/subscribers`;
        const showSpinner = false;
        const showAlert = false;
        const showToastr = true;
        const btnSpinner = false;
        const msg = "Invalid Email Address!";
        const form = $('form[id="form_subscribe"]');
        if (formValidate(form, showAlert, showToastr, msg)) {
            simpleFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner);
        }
        return false;
    });

    //check images round corner
    setImagesBorderRadius();

});
//#endregion document ready function

//#region initiate functions

function initAllFunc() {
    //initiate hide alert message
    removeAlert();
    //initiate hide toastr message
    removeToastr();
    //initiate gijgo datepicker
    initDatePicker();
    //initiate gijgo timepicker
    initTimePicker();
    //initiate upload image function to show images after upload
    initShowImagesAfterUpload();
    //initiate email validation
    initEmailValidation();
    //initiate countries dropdown
    initCountries();	//.ak-countries
    //initiate mask inputs
    initMaskInputs();
    //initiate remove input space character
    initRemoveSpace();
    //initiate password function
    initPasswordFunc();
    //initiate select 2
    initSelect2();	//.select2-dd
    //initiate intl dropdown
    initIntlInputs();
    //initiate image upload
    initImageUpload();
    //initiate document upload
    initDocUpload();
    //hide popup/modal
    initHidePopup();
    //remove popup/modal
    initRemovePopup();
    //enable/disable button on checked
    initDisabledButtonOnChecked();
    //enable/disable button on input
    initDisabledButtonOnInput();
    //See Password click on I
    initSeePassword();
}

function initSelect2(cls) {
    if (cls) {
        $(cls).select2();
    }
    else {
        $('.select2-dd').select2();
    }
}

function initEmailValidation() {
    $('input[type="email"]').change(function (e) {
        isEmailValid(this);
    });
}

function removeToastr() {
    $('.close-toastr-section').click(function () {
        $(this).closest('.toastr-section').hide();
    });

    $('.toastr-section-no-input').click(function () {
        $(this).hide();
    });
}

function removeAlert() {
    $('.close-popup-section').click(function () {
        $(this).closest('.popup-section').hide();
    });

    $('.popup-section-no-input').click(function () {
        $(this).hide();
    });
}

function initDatePicker() {

    $.each($('input[attr-date="gijgo-date"]'), function (index, elem) {
        $(elem).datepicker({
            footer: true,
            modal: true,
            minDate: today,
            format: 'mmm dd, yyyy'
        });
    });

}

function initTimePicker() {

    $.each($('input[attr-date="gijgo-time"]'), function (index, elem) {
        $(elem).timepicker();
    });

}

function initShowImagesAfterUpload() {
    $(".img-upload-1").change(function () {

        const imgInput = $(this);

        const uploadIcon = $(this).siblings();
        $(uploadIcon).removeClass('border-2 border-ak-gold').addClass('border-black/50');

        if (typeof (FileReader) != "undefined") {
            var dvPreview = $(".issue-images-div");
            $(uploadIcon).parent().siblings().remove();
            // dvPreview.html("");
            var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;

            $($(this)[0].files).each(function (i, v) {
                var file = $(this);
                if (regex.test(file[0].name.toLowerCase())) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        let dvImage = $("<div />");
                        dvImage.addClass(
                            "issue-image-item relative w-10 h-10 object-cover border rounded-lg");
                        var img = $("<img />");
                        // img.attr("style", "height:2.5rem;width: 2.5rem");
                        img.attr("src", e.target.result);
                        img.addClass("rounded-lg w-10 h-10 object-cover border rounded-lg");
                        dvImage.html(img);
                        let spanDelete =
                            `<i class="img-${i} fas fa-times-circle top-0 right-0 absolute text-danger cursor-pointer bg-white rounded-full" onclick="removeImage_img_upload_1(this, ${i})"></i>`;
                        //dvImage.append(spanDelete);

                        dvPreview.append(dvImage);
                    }
                    reader.readAsDataURL(file[0]);

                    $(uploadIcon).addClass('border-2 border-ak-gold').removeClass('border-black/50');
                } else {
                    alert(file[0].name + " is not a valid image file.");
                    // dvPreview.html("");
                    return false;
                }
            });
        } else {
            alert("This browser does not support HTML5 FileReader.");
        }
    });
}

function removeImage_img_upload_1(elem, idx) {

    const imgInput = $(elem).closest('.issue-images-div').find('input[type="file"]');
    const dt = new DataTransfer();

    let files = Array.from(imgInput[0].files);
    files.splice(idx, 1);

    for (let i = 0; i < files.length; i++) {
        dt.items.add(files[i]);
    }

    imgInput.files = dt.items;// Assign the updates list
    $(elem).closest('.issue-image-item').remove();
}

function initCountries(cls) {
    if (cls) {
        $(cls).append(allCountries);
    }
    else {
        $('.ak-countries').append(allCountries);
        if ($('.ak-countries').attr('value')) {
            $('.ak-countries').val($('.ak-countries').attr('value'));
        }
    }
}

function initMaskInputs(cls) {
    if (cls) {
        $(cls).inputmask();
    }
    else {
        $('.mask-input').inputmask();
    }
}

function initRemoveSpace(cls) {

    setTimeout(function () {
        if (cls) {
            if ($(cls).val()) {
                var value = $(cls).val().replace(/ /g, '');
                $(cls).val(value);
            }
        }
        else {
            if ($(".Contact").val()) {
                var value = $(".Contact").val().replace(/ /g, '');
                $(".Contact").val(value);
            }
        }
    }, 500);

}

function initPasswordFunc() {

    $.each($('.current-password'), function (index, elem) {
        $(elem).keyup(function (e) {
            isCurrentPasswordValid(elem);
        });
    });

    $.each($('.password'), function (index, elem) {
        $(elem).keyup(function (e) {
            isPasswordValid(elem);
        });
    });

    $.each($('.confirm-password'), function (index, elem) {
        $(elem).keyup(function (e) {
            isPasswordMatch(elem);
        });
    });
}

function initHidePopup() {
    $('.hide-modal').click(function (e) {
        $(this).closest('.popup-section').hide();
    });
}

function initRemovePopup() {

    $('.close-modal').click(function (e) {
        $(this).closest('.popup-section').remove();
    });
}

function initDisabledButtonOnChecked() {

    $('.disabled-btn-checked').click(function (e) {
        if ($('.disabled-btn-checked').is(':checked')) {
            $(".disabled-btn").attr("disabled", false);
        } else {
            $(".disabled-btn").attr("disabled", true);
        }
    });

}
function initDisabledButtonOnInput() {

    $(".input-for-btn").keyup(function (e) {
        const value = $(".input-for-btn").val();
        if (value != "") {
            $(".form-group").find(":button").attr("disabled", false);
        } else {
            $(".form-group").find(":button").attr("disabled", true);
        }
    });

}
function initSeePassword() {
    
    $(".see-password").click(function (e) {
        const icon = this;
        const input = $(icon).closest('.form-group').find('.s-p');
        const type = $(input).attr('type');

        if (type === "text") {
            $(icon).find('i').removeClass('fa-eye-slash');
            $(icon).find('i').addClass('fa-eye');
            $(input).attr('type', 'password');
        }
        else if (type === "password") {
            $(icon).find('i').removeClass('fa-eye');
            $(icon).find('i').addClass('fa-eye-slash');
            $(input).attr('type', 'text');
        }
    });
}

var _URL_ = window.URL || window.webkitURL;

//image change event
function initImageUpload() {

    $('.img-upload').change(function () {

        const elem = this;
        const form = $(elem).closest('form');
        let msg;
        let status = false;

        if ($(elem.files[0]).length) {

            let size = Number($(elem).attr('data-size'));
            let originalWidth = Number($(elem).attr('data-width'));
            let originalHeight = Number($(elem).attr('data-height'));

            let file = elem.files[0];

            let img = new Image();

            img.onload = function () {

                let width = this.width;
                let height = this.height;
                let ratio = ((originalHeight / originalWidth) * width);
                ratio = Math.floor(ratio);

                if (ratio !== height) {
                    msg = `Image dimension should be ${originalWidth} x ${originalHeight} !`;
                    ToastrMessage(icon_warning, msg, "", 6);
                    $(elem).val('');
                }
                else if (this.size > size * 1000) {
                    msg = `Image size must be less than ${size} kb!`;
                    ToastrMessage(icon_warning, msg, "", 6);
                    $(elem).val('');
                }
                else {
                    status = true;

                    img.onerror = function () {
                        status = false;
                        msg = `not a valid file: ${file.type}`;
                        ToastrMessage(icon_warning, msg, "", 6);
                        $(elem).val('');
                    };

                    //callback function
                    setTimeout(function () {
                        if (typeof img_upload_callback === "function")
                            img_upload_callback(form, status);
                    }, 250);
                }
            };
            img.src = _URL_.createObjectURL(file);
        }

    });
}

//document change event
function initDocUpload() {

    $('.doc-upload').change(function () {

        const elem = this;
        const label = $(elem).siblings()[0];
        const form = $(elem).closest('form');
        let msg;
        let status = false;
        let size = Number($(elem).attr('data-size'));

        if ($(elem.files[0]).length) {
            let file = elem.files[0];
            let fileSize = file.size / 1000000;
            let filename = file.name;

            /* getting file extenstion eg- .jpg,.png, etc */
            let extension = filename.substr(filename.lastIndexOf("."));

            /* define allowed file types */
            let allowedExtensionsRegx = /(\.pdf|\.docx)$/i;

            /* testing extension with regular expression */
            let isAllowed = allowedExtensionsRegx.test(extension);

            if (!isAllowed) {
                msg = `Only pdf or word document is allowed to upload!`;
                ToastrMessage(icon_warning, msg, "", 6);
                $(elem).val('');
                return false;
            }
            else if (fileSize > size) {
                msg = `document size must be less than ${size} mb!`;
                ToastrMessage(icon_warning, msg, "", 6);
                $(elem).val('');
                return false;
            }
            else {
                /* file upload logic goes here... */
                $(label).removeClass('border-black/50').addClass('border-2 border-ak-gold');
            }
        }
        else {
            $(label).removeClass('border-2 border-ak-gold').addClass('border-black/50');
        }

    });
}

//#endregion initiate functions

//#region we save your cookies functions
// Create cookie
function setCookie(cname, cvalue, exdays) {
    const d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

// Delete cookie
function deleteCookie(cname) {
    const d = new Date();
    d.setTime(d.getTime() + (24 * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=;" + expires + ";path=/";
}

// Read cookie
function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function getCookie2(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

// Set cookie consent
function acceptCookieConsent() {

    deleteCookie('user_cookie_consent');
    setCookie('user_cookie_consent', 1, 30);
    document.getElementById("cookieNotice").style.display = "none";
}
//#endregion

//#region Others Function

function setImagesBorderRadius() {

    setTimeout(function () { thisFunction(); }, 1000);

    function thisFunction() {
        $.each($('img'), function (idx, img) {
            $(img).css('border-radius') == "0px" || $(img).css('border-radius') == "2px" ? $(img).css('border-radius', '2.5px') : false;
        });
    }

}

//scroll body smooth after event
function scrollTop(scroll = 0) {
    $("html, body").animate({ scrollTop: scroll }, 500);
}

//check email is valid or not
function isEmailValid(element) {
    let value = $(element).val();
    let requiredMessage = $(element).closest('.form-group').find('.form-text-validation');

    const regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    if (value.match(regex)) {
        //true condition
        $(element).removeClass('border border-danger').addClass('border border-success');
        $(requiredMessage).text('');

        //element.setCustomValidity("");
    }
    else {
        $(element).removeClass('border border-success').addClass('border border-danger');
        $(requiredMessage).text('* Invalid email address! Please write valid email address.');

        //element.setCustomValidity("Please write valid email address.");
        //element.reportValidity();
    }
}

//check email is already exist or not
function emailVerification(elem, id, url) {
    if ($(elem).hasClass('border-success')) {
        let value = $(elem).val();
        ajaxVerification(value, id, url, elem, true);
    }
}
//check contact is already exist or not
function contactVerification(elem, id, url) {
    if (!$(elem).closest('.form-group').find('form-text-validation').text()) {
        let value = $(elem).val();
        let code = $(elem).closest('.form-group').find('.code').val();
        ajaxVerification(code + '|' + value, id, url, elem, false);
    }
}
//Ajax function for email and contact verification
function ajaxVerification(value, id, url, elem, isBorderCss = false) {
    let msgElement = $(elem).closest('.form-group').find('.form-text-validation');
    $.ajax({
        type: 'POST',
        url: url,
        data: { value, id },
        success: function (response) {
            if (response.success) {
                //true condition
                if (isBorderCss)
                    $(elem).removeClass('border border-danger').addClass('border border-success');

                $(msgElement).text('');
            }
            else {
                if (isBorderCss)
                    $(elem).removeClass('border border-success').addClass('border border-danger');

                $(msgElement).text(response.message);
            }
        }
    });
}

//check password is valid or not
function isCurrentPasswordValid(elem) {

    var value = $(elem).val();
    var passRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z@@*$&#%!~()\d]{8,}$/;
    if (value) {
        if (!value.match(passRegex)) {
            $(elem).addClass('border-red-600')
                .removeClass('border-green-600')
                .closest('.form-group').find('.form-text-validation').text(ChangeString('Min. 8 characters, at least one uppercase letter, one lowercase letter, and one number', 'دقيقة. 8 أحرف ، حرف كبير واحد على الأقل ، وحرف صغير واحد ، ورقم واحد '));
        }
        else {
            $(elem).addClass('border-green-600')
                .removeClass('border-red-600')
                .closest('.form-group').find('.form-text-validation').text('');
        }
    }
    else {
        $(elem).removeClass('border-red-600 border-green-600')
            .closest('.form-group').find('.form-text-validation').text("* " + $(elem).closest('.form-group').find('label').text() + ChangeString(' is required.', 'مطلوب. '));
    }
}

//check password is valid or not
function isPasswordValid(element, e = event) {
    if (e.keyCode === 9)
        return false;

    let value = $(element).val();

    let requiredMessage = $(element).closest('.form-group').find('.form-text-validation');

    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z@@*$&#%!~()\d]{8,}$/;
    if (value.match(regex)) {
        //true condition
        $(element).removeClass('border border-danger').addClass('border border-success');
        $(requiredMessage).text('');

        element.setCustomValidity("");
    }
    else {
        $(element).removeClass('border border-success').addClass('border border-danger');
        $(requiredMessage).text('Min. 8 characters, at least one uppercase letter, one lowercase letter, and one number');

        element.setCustomValidity("Please match the password pattern.");
        element.reportValidity();
    }
    let form = $(element).closest('form');
    if ($(form).find('.confirm-password').length) {
        isPasswordMatch($('.confirm-password'));
    }
}
//check password is valid or not
function isPasswordMatch(element, e = event) {
    if (e.keyCode === 9)
        return false;

    let password = $(element).closest('form').find('.password').val();
    let value = $(element).val();
    let requiredMessage = $(element).closest('.form-group').find('.form-text-validation');

    if (value.match(password)) {
        //true condition
        $(element).removeClass('border border-danger').addClass('border border-success');
        $(requiredMessage).text('');

        //element.setCustomValidity("");
    }
    else {
        $(element).removeClass('border border-success').addClass('border border-danger');
        $(requiredMessage).text("Password doesn't match! Please write the correct password.");

        //element.setCustomValidity("Password doesn't match! Please write the correct password.");
        //element.reportValidity();
    }
}

function dateFormat(flag = 1) {
    if (flag == 1) {
        $.each($('.date-format-1'), function (k, elem) {
            let isVal = true;
            let date = $(elem).val();
            if (!date) {
                date = $(elem).text();
                isVal = false;
            }
            if (date) {
                let newDate = new Date(date);
                let value = newDate.getDate() + " " + newDate.toLocaleString('default', { month: 'long' }) + " " + newDate.getFullYear();

                if (isVal)
                    $(elem).val(value);
                else
                    $(elem).text(value);
            }
        });
    }
}

function seePassword(icon) {

    const input = $(icon).closest('.form-group').find('.s-p');
    const type = $(input).attr('type');

    if (type === "text") {
        $(icon).find('i').removeClass('fa-eye-slash');
        $(icon).find('i').addClass('fa-eye');
        $(input).attr('type', 'password');
    }
    else if (type === "password") {
        $(icon).find('i').removeClass('fa-eye');
        $(icon).find('i').addClass('fa-eye-slash');
        $(input).attr('type', 'text');
    }

}

function create_guid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function random_sort() {
    return Math.random() - 0.5;
}

function modal_loader(IsShow = true) {

    if (IsShow) {
        var loader = `

			<section class="font-Rubik w-full bg-black/50 fixed inset-0 modal fade flex flex-wrap top-0 left-0 justify-center items-center overflow-x-hidden overflow-y-auto h-auto z-[60] overflow-scroll-none modal-loader">
				<!-- Main Modal -->
				<div class="max-w-screen-4xl 4xl:mx-auto w-full relative">
					<!-- Modal Content -->
					<div class="bg-white rounded-lg w-[80%] mx-auto m-10">

						<!-- Loader Section -->
							<div class="modal-body p-4 md:py-2 px-6 md:px-2 space-y-6">

								<div
								class="flex flex-col md:flex-row flex-wrap divide-y-[1.8px] md:divide-y-0 md:divide-x-[1.8px] justify-between items-center">

									<div class="w-[100%] px-2 md:px-6 py-2 md:py-0 md:min-h-[18rem] mx-auto flex items-center ">
										<div class="container w-full">
											<div class="bg-white border-[#707070] rounded-lg">
												<div class="flex flex-col my-4 text-center justify-center">
													<div class="flex flex-row items-center">
														<div class="mt-1 mx-auto dot-flashing text-center w-[100%]"></div>
													</div>
												</div>
											</div>
										</div>
									</div>

								</div>

							</div>
						<!-- Loader Section End -->

					</div>
					<!-- Modal Content End -->
				</div>
				<!-- Main Modal End -->
			</section>

		`;

        $('body').prepend(loader);
    }
    else {
        $('body .modal-loader').remove();
    }

}

function changeSession() {
    var details = getCookie("Customer-Details");
    if (details) {

        const name = details.split('&')[0].split('=')[1];
        const logo = details.split('&')[1].split('=')[1];

        //change login links to customer links
        $('.user-avatar').removeAttr('onclick').attr('href', `/customer/dashboard/index?v=${create_guid()}`);

        $('.img-person').attr('src', logo);
        $('.hi-person').text(`Hi, ${name}`);
        $('.hello-person').text(`Hello, ${name}`);
        $('.login-link').remove();

        //convert login to logout
        $('.login-link-1').removeAttr('onclick').attr('href', `/Customer/Account/Logout?v=${create_guid()}`);
        $('.login-text').text(`Logout`);
    }
}

function hideGridLoading(id, isHide) {
    if (isHide) {
        $(`${id} .loading`).removeClass('d-flex');
        $(`${id} .loading`).hide();
    }
    else {
        $(`${id} .loading`).addClass('d-flex');
        $(`${id} .loading`).show();
    }
}

//#endregion Others Function

//#region fetch and bind functions

function FetchBusinessSetting() {
    $.ajax({
        type: 'Get',
        url: `/contact-setting/`,
        success: function (response) {
            if (response.success) {
                BindBusinessSetting(response.data);
            } else {
                // console.log("Business Setting Error!");
            }
        },
        error: function (e) {
            // console.log("Business Setting Error!");
        }
    });

    function BindBusinessSetting(data) {
        //#region Contact Us
        if ($('.web-contact-info').is(":visible")) {
            $('.web-contact-info').empty();

            if (data.StreetAddress && data.StreetAddress != '-') {
                $('.web-contact-info')
                    .append(`<li>
								<div class="footer-address-icon">
									<i class="icon-placeholder"></i>
								</div>
								<div class="footer-address-info">
									<p>${data.StreetAddress}</p>
								</div>
							</li>`);
            }
            if (data.Contact && data.Contact != '-') {
                $('.web-contact-info')
                    .append(`<li>
								<div class="footer-address-icon">
									<i class="icon-call"></i>
								</div>
								<div class="footer-address-info">
									<p><a href="tel:${data.Contact}">+${data.Contact}</a></p>
								</div>
							</li>`);

                $('.phone-link').attr("href", `tel:${data.Contact}`);
            }
            if (data.Contact2 && data.Contact2 != '-') {
                $('.web-contact-info')
                $('.web-contact-info')
                    .append(`<li>
								<div class="footer-address-icon">
									<i class="icon-call"></i>
								</div>
								<div class="footer-address-info">
									<p><a href="tel:${data.Contact2}">+${data.Contact2}</a></p>
								</div>
							</li>`);
            }
            if (data.Email && data.Email != '-') {
                $('.web-contact-info')
                $('.web-contact-info')
                    .append(`<li>
								<div class="footer-address-icon">
									<i class="icon-mail"></i>
								</div>
								<div class="footer-address-info">
									<p><a href="mailto:${data.Email}">${data.Email}</a></p>
								</div>
							</li>`);

                $('.mail-link').attr("href", `mailto:${data.Email}`);
            }
            //if (data.Fax) {
            //	$('.web-contact-info')
            //		.append(`<a class="flex flex-row items-center">
            //				<i class="fa fa-fax pr-4 text-white text-sm md:text-md"></i>
            //				<p class="text-xs lg:text-sm xl:text-md my-2 text-[#FFFFFF85] font-light hover:text-ak-gold transition-[0.3s_all_ease-in-out]">${data.Fax}</p>
            //			</a>`);
            //}
        }

        if ($('.web-contact-us-info').is(":visible")) {
            //$('.web-contact-us-info').empty();

            if (data.Contact && data.Contact != '-') {
                $('.web-contact-us-info').find('.phone-number').append("+" + data.Contact);
            }
            if (data.Contact2 && data.Contact2 != '-') {
                $('.web-contact-us-info').find('.phone-number').append("<br>" + "+" + data.Contact2);
            }
            if (data.Email && data.Email != '-') {
                $('.web-contact-us-info').find('.email-address').append(data.Email + "<br><br> ");
            }
            if (data.StreetAddress && data.StreetAddress != '-') {
                $('.web-contact-us-info').find('.street-address').append(data.StreetAddress + "<br><br> ");
            }

        }

        if ($('.web-contact-need-help').is(":visible")) {
            //$('.web-contact-need-help').empty();

            if (data.Contact && data.Contact != '-') {
                $('.web-contact-need-help').find('.phone-number').append(`<i class="fas fa-phone"></i>+` + data.Contact);
            }
            //if (data.Contact2 && data.Contact2 != '-') {
            //	$('.web-contact-need-help').find('.phone-number').append("<br>" + `<i class="fas fa-phone"></i>+` + data.Contact2);
            //}
            //if (data.Email && data.Email != '-') {
            //	$('.web-contact-need-help').find('.email-address').append(data.Email + "<br><br> ");
            //}
            //if (data.StreetAddress && data.StreetAddress != '-') {
            //	$('.web-contact-need-help').find('.street-address').append(data.StreetAddress + "<br><br> ");
            //}

        }

        if ($('.web-contact-promo').is(":visible")) {
            //$('.web-contact-promo').empty();

            if (data.Contact && data.Contact != '-') {
                $('.web-contact-promo').find('.phone-number').html(`+` + data.Contact);
                $('.web-contact-promo').find('.phone-link').attr('href', `tel:+${data.Contact}`);
            }
            //if (data.Contact2 && data.Contact2 != '-') {
            //	$('.web-contact-promo').find('.phone-number').append("<br>" + `<i class="fas fa-phone"></i>+` + data.Contact2);
            //}
            //if (data.Email && data.Email != '-') {
            //	$('.web-contact-promo').find('.email-address').append(data.Email + "<br><br> ");
            //}
            //if (data.StreetAddress && data.StreetAddress != '-') {
            //	$('.web-contact-promo').find('.street-address').append(data.StreetAddress + "<br><br> ");
            //}

        }
        //#endregion Contact Us

        //#region Whatsapp
        if (data.Whatsapp && data.Whatsapp != '-') {
            //$('#whatsapp_link').html(`<i class="fab fa-whatsapp"></i> ${lang == "en" ? data.Title : data.TitleAr}`)
            $('.whatsapp-link').attr("href", `https://api.whatsapp.com/send?phone=${data.Whatsapp}&text=${encodeURIComponent(lang == "en" ? data.FirstMessage : data.FirstMessageAr)}`);
            $('.whatsapp-link').show();
        } else {
            //$('#whatsapp_link').remove();
        }
        //#endregion

        //#region Social Links

        //if ((data.Facebook && data.Facebook != '-')
        //	|| (data.Instagram && data.Instagram != '-')
        //	|| (data.Youtube && data.Youtube != '-')
        //	|| (data.Twitter && data.Twitter != '-')
        //	|| (data.Snapchat && data.Snapchat != '-')
        //	|| (data.LinkedIn && data.LinkedIn != '-')
        //	|| (data.Behance && data.Behance != '-')
        //	|| (data.Pinterest && data.Pinterest != '-')) {
        //	$('.web-social-icons').append(`<li><span>${lang == "en" ? "Follow Us:" : "تابعنا:"}</span></li>`);
        //}

        if ($('.web-social-icons').is(":visible")) {

            $('.web-social-icons').empty();

            if (data.Facebook && data.Facebook != '-') {
                $('.web-social-icons')
                    .append(`<li><a href="${data.Facebook}" title="Facebook" target="_blank"><i class="fab fa-facebook-f"></i></a></li>`);
            }
            if (data.Twitter && data.Twitter != '-') {
                $('.web-social-icons')
                    .append(`<li><a href="${data.Twitter}" title="Twitter" target="_blank"><i class="fab fa-twitter"></i></a></li>`);
            }
            if (data.Instagram && data.Instagram != '-') {
                $('.web-social-icons')
                    .append(`<li><a href="${data.Instagram}" title="Instagram" target="_blank"><i class="fab fa-instagram"></i></a></li>`);
            }
            if (data.LinkedIn && data.LinkedIn != '-') {
                $('.web-social-icons')
                    .append(`<li><a href="${data.LinkedIn}" title="LinkedIn" target="_blank"><i class="fab fa-linkedin"></i></a></li>`);
            }
            if (data.Youtube && data.Youtube != '-') {
                $('.web-social-icons')
                    .append(`<li><a href="${data.Youtube}" title="Youtube" target="_blank"><i class="fab fa-youtube"></i></a></li>`);
            }
            if (data.Snapchat && data.Snapchat != '-') {
                $('.web-social-icons')
                    .append(`<li><a href="${data.Snapchat}" title="Snapchat" target="_blank"><i class="fab fa-snapchat"></i></a></li>`);
            }
            if (data.Behance && data.Behance != '-') {
                $('.web-social-icons')
                    .append(`<li><a href="${data.Behance}" title="Behance" target="_blank"><i class="fab fa-behance"></i></a></li>`);
            }
            if (data.Pinterest && data.Pinterest != '-') {
                $('.web-social-icons')
                    .append(`<li><a href="${data.Pinterest}" title="Pinterest" target="_blank"><i class="fab fa-pinterest"></i></a></li>`);
            }
        }

        //#endregion Social Links ends

        //#region App Links
        //if (data.AndroidAppUrl && data.AndroidAppUrl != '-') {
        //	$('#AndroidAppUrl').attr("href", data.AndroidAppUrl);
        //}
        //if (data.IosAppUrl && data.IosAppUrl != '-') {
        //	$('#IosAppUrl').attr("href", data.IosAppUrl);
        //}
        //#endregion

        //#region Others Checks
        //IsLoyaltyEnabled = data.IsLoyaltyEnabled;

        //if (!IsLoyaltyEnabled) {
        //    $('.redeem-amount-container').remove();
        //}
        //if (!data.IsMaruCompare) {
        //    $('.MaruCompareBanner').remove();
        //}
        //else {
        //    $('.MaruCompareBanner').show();
        //
        //#endregion

        //change target of void links.
        $('a[href="javascript:void(0);"').attr('target', '_top');

    }
}

//#region Open Pages and popup Functions

function openAppointmentPage() {
    let apptUrl = `/Customer/Appointments/Index`;

    if (session) {
        window.location = apptUrl;
    }
    else {
        showRegisterSection(ChangeString('Please login first, to schedule an Appointment.', 'الرجاء تسجيل الدخول أولا لتحديد موعد.'), apptUrl);
    }
}

let open_BookingPopup = true;

async function openBookingPopup(ID) {

    if (!session)
        await get_session();

    if (open_BookingPopup) {
        open_BookingPopup = false;
        modal_loader(true);

        if (session) {
            bookingPopupAfterLogin = false;
            fetchServiceBooking(ID);
        }
        else {
            bookingPopupAfterLogin = true;
            bookingServiceID = ID;
            modal_loader(false);
            showRegisterSection(ChangeString('Please login first, to book a service.', 'الرجاء تسجيل الدخول أولا لحجز الخدمة.'), "");
            open_BookingPopup = true;
        }

        //fetch view
        function fetchServiceBooking(ID) {
            $.ajax({
                type: 'Get',
                url: `/customer/booking/Create/${ID}`,
                success: function (response) {
                    if (response) {
                        $('body').append(response);
                        $('#booking_section').show()/*.slideDown('fast', 'linear')*/;
                        open_BookingPopup = true;
                        modal_loader(false);
                    } else {
                        ToastrMessage(ServerErrorShort);
                    }
                },
                error: function (e) {
                    ToastrMessage(ServerErrorShort);
                }
            });
        }
    }

}

async function get_session() {
    let result;

    try {
        result = await $.ajax({
            type: 'Get',
            url: `/Customer/Dashboard/GetSession`,
        });

    } catch (error) {
        result = false;
    }

    if (result.success) {
        session = true;
        changeSession();
    }

    return result;
}


function closePopup(modal) {
    $(modal).remove();
}


function successPopup(modal) {
    $(modal).remove();
    modal_loader(true);
    fetchConfirmationMessage();
}

function fetchConfirmationMessage() {

    $.ajax({
        type: 'Get',
        url: `/confirmation-message`,
        success: function (response) {
            if (response) {
                $('body').append(response);
                $('#success_section').show()/*.slideDown('fast', 'linear')*/;
                modal_loader(false);
            } else {
                ToastrMessage(ServerErrorShort);
            }
        },
        error: function (e) {
            ToastrMessage(ServerErrorShort);
        }
    });
}

//#endregion

//ratings star code

function ratingStarsInitiate() {
    $('.rate-1')
        .mouseout(function (e) {
            if ($(".rating-input").val()) {
                $('.rate-1 .stars').each(function (k, elem) {
                    $(elem).addClass('selected');
                    if ((k + 1) == $(".rating-input").val()) {
                        return false;
                    }
                });
            }
        });

    $('.rate-1 .stars')
        .mouseover(function (e) {

            let obj = this;
            removeHighlightStars();

            $('.rate-1 .stars').each(function (k, elem) {
                $(elem).addClass('highlight');
                if (k == $('.rate-1 .stars').index(obj)) {
                    return false;
                }
            });

        })
        .mouseout(function (e) {
            removeHighlightStars();
        })
        .click(function (e) {

            let obj = this;

            $('.rate-1 .stars').each(function (k, elem) {
                $(elem).addClass('selected');
                $('.rating-input').val((k + 1));
                if (k == $('.rate-1 .stars').index(obj)) {
                    return false;
                }
            });

        });
}

function removeHighlightStars() {
    $('.rate-1 .stars').removeClass('selected');
    $('.rate-1 .stars').removeClass('highlight');
}

//ratings star code end

//#region form submit functions

function formValidate(form, showAlert = false, showToastr = false, msg = "") {
    let validForm = false;

    //insert, remove validate message on input fields
    $.each($(form).find('.form-field'), function (idx, elem) {

        if ($(elem).attr('data-strict') === "1") {

            //has value
            if ((!$(elem).attr('multiple') && $(elem).val())
                ||
                ($(elem).attr('multiple') && $(elem).val().length)) {
                if ($(elem).hasClass('password') || $(elem).hasClass('email') || $(elem).hasClass('confirm-password')) {
                    if ($(elem).closest('.form-group').find('.form-text-validation').text() && $(elem).hasClass('border-danger')) {
                    }
                    else {
                        $(elem).closest('.form-group').find('.form-text-validation').text('');
                    }
                }
                else {
                    $(elem).closest('.form-group').find('.form-text-validation').text('');
                }
            }
            //validate
            else {
                $(elem).closest('.form-group').find('.form-text-validation').text("* " + $(elem).closest('.form-group').find('label').text() + ChangeString(' is required.', 'مطلوب. '));
            }

            //
        }

    });

    //check if there is non validate input fields
    $.each($(form).find('.form-field'), function (idx, elem) {

        if ($(elem).closest('.form-group').find('.form-text-validation').text()) {
            validForm = false;
            return false; // breaks loop;
        }
        else {
            validForm = true;
        }
    });

    //return true of false
    if (validForm) {
        return true;
    }
    else {
        if (showAlert) {
            //show alert message
            //AlertMessage(form, icon_warning, msg, "", 10);
            ToastrMessage(icon_warning, msg, "", 6);
        }
        if (showToastr) {
            //show toastr message
            ToastrMessage(icon_warning, msg, "", 6);
        }
        return false;
    }
}

function simpleFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner) {

    //waiting
    disableSubmitButton(btn, true, '', showSpinner);
    ToastrMessage(icon_info, msg_loading, "", 6);

    //if ($(form).find('input[name="Contact"]').length && $(form).find('.flag-input').length) {
    //	const full_contact = $(form).find('.flag-input').attr('data-code') + $(form).find('.Contact').val();
    //	$(form).find('input[name="Contact"]').val(full_contact);
    //}

    $.ajax({
        url: url,
        type: 'Post',
        data: $(form).serialize(),
        success: function (response) {
            //success
            if (response.success) {
                ToastrMessageHide();
                //success message
                if (response.message) {
                    showFormMessage(showAlert, showToastr, form, icon_success, response.message, (response.url ? response.url : ""), 8);
                }

                //empty form fields
                if ($(form).attr('data-notvanished') || $(form).hasClass('prevent')) {
                    $(form).find('.form-field').removeClass("border-success border-danger");
                }
                else {
                    $(form).find('.form-field').val('').removeClass("border-success border-danger");
                }

                //callback function
                if (typeof simpleFormSubmit_callback === "function")
                    simpleFormSubmit_callback(form, true, response, (response.url ? response.url : ""));

                //enable submit btn
                disableSubmitButton(btn, false, '', showSpinner);
            }
            //error
            else {
                //error message
                if (response.message) {
                    showFormMessage(showAlert, showToastr, form, icon_warning, response.message, (response.url ? response.url : ""), 8);
                }
                else if (response.error) {
                    showFormMessage(showAlert, showToastr, form, icon_warning, response.error, (response.url ? response.url : ""), 8);
                }

                //error message description
                setTimeout(function () {
                    if (response.description) {
                        showFormMessage(showAlert, showToastr, form, icon_warning, response.description, (response.url ? response.url : ""), 20);
                    }
                }, 6000);

                //callback function
                if (typeof simpleFormSubmit_callback === "function")
                    simpleFormSubmit_callback(form, false, response, (response.url ? response.url : ""));

                //enable submit btn
                disableSubmitButton(btn, false, '', showSpinner);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorFunction(xhr, ajaxOptions, thrownError, btn);
        },
        failure: function (xhr, ajaxOptions, thrownError) {
            ErrorFunction(xhr, ajaxOptions, thrownError, btn);
        }
    });
    return false;
}

function fileFormSubmit(form, btn, url, showSpinner, showAlert, showToastr, btnSpinner) {

    //waiting
    disableSubmitButton(btn, true, '', showSpinner);
    ToastrMessage(icon_info, msg_loading, "", 6);

    //if ($(form).find('input[name="Contact"]').length && $(form).find('.flag-input').length) {
    //	const full_contact = $(form).find('.flag-input').attr('data-code') + $(form).find('.Contact').val();
    //	$(form).find('input[name="Contact"]').val(full_contact);
    //}

    var data = new FormData();

    $.each($(form).find('input'), function (k, elem) {
        if ($(elem).attr('type') == "file") {

            let files = $(elem)[0].files;
            if (files.length) {
                for (var i = 0; i < files.length; i++) {
                    data.append($(elem).attr('name'), files[i]);
                }
            }
            else {
                data.append($(elem).attr('name'), $(elem).attr('value'));
            }
        }
        if ($(elem).attr('type') == "checkbox") {
            data.append($(elem).attr('name'), $(elem).prop("checked"));
        }
        else {
            data.append($(elem).attr('name'), $(elem).val());
        }
    });
    $.each($(form).find('textarea'), function (k, elem) {
        data.append($(elem).attr('name'), $(elem).val());
    });
    $.each($(form).find('select'), function (k, elem) {
        data.append($(elem).attr('name'), $(elem).val());
    });

    $.ajax({
        url: url,
        type: 'Post',
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: data,
        success: function (response) {
            //success
            if (response.success) {

                //success message
                if (response.message) {
                    showFormMessage(showAlert, showToastr, form, icon_success, response.message, (response.url ? response.url : ""), 8);
                }

                //empty form fields
                if ($(form).attr('data-notvanished') || $(form).hasClass('prevent')) {
                    $(form).find('.form-field').removeClass("border-success border-danger");
                }
                else {
                    $(form).find('.form-field').val('').removeClass("border-success border-danger");
                }

                //callback function
                if (typeof fileFormSubmit_callback === "function")
                    fileFormSubmit_callback(form, true, response, (response.url ? response.url : ""));

                //enable submit btn
                disableSubmitButton(btn, false, '', showSpinner);
            }
            //error
            else {
                //error message
                if (response.message) {
                    showFormMessage(showAlert, showToastr, form, icon_warning, response.message, (response.url ? response.url : ""), 8);
                }
                else if (response.error) {
                    showFormMessage(showAlert, showToastr, form, icon_warning, response.error, (response.url ? response.url : ""), 8);
                }

                //error message description
                setTimeout(function () {
                    if (response.description) {
                        showFormMessage(showAlert, showToastr, form, icon_warning, response.description, (response.url ? response.url : ""), 20);
                    }
                }, 6000);

                //callback function
                if (typeof fileFormSubmit_callback === "function")
                    fileFormSubmit_callback(form, false, response, (response.url ? response.url : ""));

                //enable submit btn
                disableSubmitButton(btn, false, '', showSpinner);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorFunction(xhr, ajaxOptions, thrownError, btn);
        },
        failure: function (xhr, ajaxOptions, thrownError) {
            ErrorFunction(xhr, ajaxOptions, thrownError, btn);
        }
    });
    return false;
}

function deleteDataByIDForm(form, btn, url, id, showSpinner, showAlert, showToastr, btnSpinner) {

    //waiting
    disableSubmitButton(btn, true, 'i', showSpinner);
    ToastrMessage(icon_info, msg_loading, "", 6);

    $.ajax({
        url: url,
        type: 'Post',
        data: { id: id },
        success: function (response) {
            //success
            if (response.success) {

                //success message
                if (response.message) {
                    showFormMessage(showAlert, showToastr, form, icon_success, response.message, (response.url ? response.url : ""), 8);
                }

                //callback function
                if (typeof deleteDataByIDForm_callback === "function")
                    deleteDataByIDForm_callback(form, id, true, response, (response.url ? response.url : ""));
            }
            //error
            else {
                //error message
                if (response.message) {
                    showFormMessage(showAlert, showToastr, form, icon_warning, response.message, (response.url ? response.url : ""), 8);
                }
                else if (response.error) {
                    showFormMessage(showAlert, showToastr, form, icon_warning, response.error, (response.url ? response.url : ""), 8);
                }

                //error message description
                setTimeout(function () {
                    if (response.description) {
                        showFormMessage(showAlert, showToastr, form, icon_warning, response.description, (response.url ? response.url : ""), 20);
                    }
                }, 6000);

                //callback function
                if (typeof deleteDataByIDForm_callback === "function")
                    deleteDataByIDForm_callback(form, id, false, response, (response.url ? response.url : ""));

                //enable submit btn
                disableSubmitButton(btn, false, '', showSpinner);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorFunction(xhr, ajaxOptions, thrownError, btn);
        },
        failure: function (xhr, ajaxOptions, thrownError) {
            ErrorFunction(xhr, ajaxOptions, thrownError, btn);
        }
    });
    return false;
}

function showFormMessage(showAlert, showToastr, form, icon, msg, link, timeup) {
    if (showAlert) {
        //AlertMessage(form, icon, msg, (link ? link : ""), timeup);
        ToastrMessage(icon, msg, (link ? link : ""), timeup);
    }
    if (showToastr) {
        ToastrMessage(icon, msg, (link ? link : ""), timeup);
    }
}

function changeCulture(elem, lng) {
    /* 
    if (lang == 'en')
        $('#GTranslate_Select').val('en|ar').trigger('change');
    else
        $('#GTranslate_Select').val('en|en').trigger('change');
    */
    var form = $(elem).closest('form.culture-form');
    $(form).find('input[name="ReturnUrl"]').val(window.location.pathname);
    $(form).find('input[name="culture"]').val(lng);

    $(form).submit();
    //$('body').css('opacity', '0.5');

    //setTimeout(function () {
    //}, 1500);
}

function imageSubmit(form, fileData, url) {

    // Adding one more key to FormData object  
    //fileData.append('name', 'Manas');

    $.ajax({
        url: url,
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: fileData,
        success: function (response) {

            //success
            if (response.success) {

                //success message
                if (response.message) {
                    showFormMessage(false, true, form, icon_success, response.message, (response.url ? response.url : ""), 8);
                }

                //callback function
                if (typeof img_upload_form_callback === "function")
                    img_upload_form_callback(form, true, response, (response.url ? response.url : ""));

            }
            //error
            else {
                //error message
                if (response.message) {
                    showFormMessage(false, true, form, icon_warning, response.message, (response.url ? response.url : ""), 8);
                }
                else if (response.error) {
                    showFormMessage(false, true, form, icon_warning, response.error, (response.url ? response.url : ""), 8);
                }

                //error message description
                setTimeout(function () {
                    if (response.description) {
                        showFormMessage(false, true, form, icon_warning, response.description, (response.url ? response.url : ""), 20);
                    }
                }, 6000);

                //callback function
                if (typeof img_upload_form_callback === "function")
                    img_upload_form_callback(form, false, response, (response.url ? response.url : ""));

            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorFunction(xhr, ajaxOptions, thrownError, "");
        },
        failure: function (xhr, ajaxOptions, thrownError) {
            ErrorFunction(xhr, ajaxOptions, thrownError, "");
        }
    });

}

//#endregion form submit functions
