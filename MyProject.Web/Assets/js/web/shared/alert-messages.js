'use strict';

//#region Global Variables and Arrays
const icon_info = 'info';
const icon_success = 'success';
const icon_error = 'error';
const icon_warning = 'warning';
const icon_question = 'question';

const fa_icon_info = 'fa fa-info';
const fa_icon_warning = 'fa fa-exclamation';
const fa_icon_success = 'fa fa-check';
const fa_icon_error = 'fa fa-times';

const fa_icon_info_circle = 'fa fa-info-circle';
const fa_icon_warning_circle = 'fa fa-exclamation-circle';
const fa_icon_success_circle = 'fa fa-check-circle';
const fa_icon_error_circle = 'fa fa-times-circle';

const result_icon_info = 'info';
const result_icon_success = 'check';
const result_icon_error = 'times';
const result_icon_warning = 'exclamation';

const msg_loading = ChangeString('Loading, please wait ...', 'جاري التحميل الرجاءالانتظار ...');

let toastrTimeOut;
let alertMessageTimeOut;
//#endregion

//#region Layout Javascript Queries

//#endregion

//#region document ready function
$(document).ready(function () {
    //$('.btn-close-toastr').click(function (e) {
    //    ToastrMessageHide(0);
    //});
});
//#endregion

//#region Alerts Function

//Toastr settings

function ToastrMessage(icon = fa_icon_info, msg = msg_loading, link = "", timeup = 6)//6 seconds
{
    timeup = timeup * 1000;
    Swal.close();
    if (msg == msg_loading) {
        showLoadingToast();
    }
    else {
        showToast();
    }

    //if ($('#toastr_bottom').is(":visible")) {
    //	//$('#toastr_bottom').slideUp();
    //	clearTimeout(toastrTimeOut);
    //}

    //setTimeout(function () {

    //	//$('#toastr_bottom .toastr-icon').attr('src', `/assets/images/web-icons/toastr-${icon}.png`);
    //	$('#toastr_bottom .toastr-icon span').html(`<i class="${icon} text-ak-gold rounded-full m-auto"></i>`);
    //	$('#toastr_bottom .toastr-message').html(msg);
    //	if (link) {
    //		$('#toastr_bottom .toastr-link').html(link).show();
    //	}
    //	$('#toastr_bottom').slideDown();

    //	if (timeup && timeup > 0) {
    //		toastrTimeOut = setTimeout(function () {
    //			$('#toastr_bottom').slideUp();
    //		}, (timeup * 1000));
    //	}

    //}, 300);

    function showToast() {
        title = icon[0].toUpperCase() + icon.substr(1);

        Swal.fire({
            icon: icon,
            title: title,
            text: msg,
            showCloseButton: true,
            showDenyButton: false,
            denyButtonText: `Close`,
            cancelButtonColor: '#071c1f',
            showConfirmButton: true,
            confirmButtonText: `Okay`,
            confirmButtonColor: '#ff5a3c',
            allowOutsideClick: false,
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                //Swal.fire('Saved!', '', 'success')
            } else if (result.isDenied) {
                //Swal.fire('Changes are not saved', '', 'info')
            }
        })
    }

    function showLoadingToast() {
        //let timerInterval
        Swal.fire({
            title: "Loading",
            html: msg,
            timer: 300000, //5min
            timerProgressBar: true,
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
                //const b = Swal.getHtmlContainer().querySelector('b')
                //timerInterval = setInterval(() => {
                //	b.textContent = Swal.getTimerLeft()
                //}, 100)
            },
            willClose: () => {
                //clearInterval(timerInterval)
            }
        }).then((result) => {
            if (result.dismiss === Swal.DismissReason.timer) {
                showErrorToast();
            }
        })
    }

    function showErrorToast() {

        Swal.fire({
            icon: icon_error,
            title: "Error",
            text: ServerError,
            showCloseButton: true,
            showDenyButton: true,
            denyButtonText: `Reload Page`,
            denyButtonColor: '#071c1f',
            showConfirmButton: true,
            confirmButtonText: `Okay`,
            confirmButtonColor: '#ff5a3c',
            allowOutsideClick: false,
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                //Swal.fire('Saved!', '', 'success')
            } else if (result.isDenied) {
                PageReload(null, 1);
                //Swal.fire('Changes are not saved', '', 'info')
            }
        })
    }

}
function ToastrMessageHide(timeup = 0)//0 second
{
    Swal.close();
    //setTimeout(function () {
    //	$('#toastr_bottom').slideUp();
    //	$('#toastr_bottom .toastr-link').hide();
    //}, (timeup * 1000));
}

//Alert settings
function AlertMessage(form, icon = fa_icon_info, msg = msg_loading, link = '', timeup = 10) {

    /*
    <div class="alert-div">
        <div class="flex items-center border border-ak-gold/90 text-black/90 text-sm font-light px-3 py-2 my-3 filter drop-shadow-lg rounded-md" role="alert">
            <div class="">
                <span class="bg-white w-8 h-8 border border-ak-gold/90 rounded-full flex text-center items-center mr-2">
                    <i class="fa fa-info text-ak-gold rounded-full m-auto"></i>
                </span>
            </div>
            <p class="text-justify text-sm alert-message leading-[1rem]">Something happened that you should know about... </p>
            <a href="javascript:void(0);" class="w-8 h-8 group rounded-full flex text-center items-center ml-auto pl-3 shadow-none">
                <i class="fa fa-times text-ak-gold rounded-full m-auto group-hover:scale-125 transition-all shadow-none"></i>
            </a>
        </div>
    </div>
    */

    let a_link = '';
    if (link && link.includes('/')) {
        a_link = `<a href="${link}" class="text-ak-blue font-medium font-Rubik text-center text-sm flex justify-center mb-2 otp-contact" data-value="">click here</a>`;
    }
    else if (link) {
        a_link = `<a href="javascript:void(0);" onclick="${link}" class="text-ak-blue font-medium font-Rubik text-center text-sm flex justify-center mb-2 otp-contact" data-value="">${ChangeString("click here", "انقر هنا")}</a>`;
    }

    var alert_box = `<div class="flex items-center border border-ak-gold/90 bg-white text-black/90 text-sm font-light px-3 py-2 my-3 filter drop-shadow-lg rounded-md" role="alert">
						<div class="">
							<span class="bg-white w-8 h-8 border border-ak-gold/90 rounded-full flex text-center items-center mr-2">
								<i class="${icon} text-ak-gold rounded-full m-auto"></i>
							</span>
						</div>
						<p class="text-justify text-sm alert-message leading-[1rem] flex flex-col">
							<span>${msg}</span>
							${a_link}
						</p>
						<a onclick="AlertMessageHide(0);" class="btn-close-alert cursor-pointer w-8 h-8 group rounded-full flex text-center items-center ml-auto pl-3 shadow-none">
							<i class="fa fa-times text-ak-gold rounded-full m-auto group-hover:scale-125 transition-all shadow-none"></i>
						</a>
					</div>
					`;

    if ($(form).find('.alert-div').is(":visible")) {
        //$(form).find('.alert-div').html('').hide(); 
        clearTimeout(alertMessageTimeOut);
    }

    setTimeout(function () {

        $(form).find('.alert-div').html(alert_box).slideDown();

        if (timeup && timeup > 0) {
            alertMessageTimeOut = setTimeout(function () {
                $(form).find('.alert-div').html(alert_box).slideUp();
            }, (timeup * 1000));
        }

    }, 300);

}
function AlertMessageHide(timeup = 0)//0 second
{
    setTimeout(function () {
        $('.alert-div').slideUp();
    }, (timeup * 1000));
}
//#endregion

//#region errors
function ErrorFunction(xhr, ajaxOptions, thrownError, btn) {
    if (xhr.status == 403) {
        ToastrMessage(icon_error, ServerError, "", 10);
        if (btn) {
            disableSubmitButton(btn, false);
        }
    }
    else {
        ToastrMessage(icon_error, `(${thrownError}) ${ServerErrorShort}`, "", 10);
        if (btn) {
            disableSubmitButton(btn, false);
        }
    }
}
//#endregion