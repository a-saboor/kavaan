"use strict";

//#region Global variables and settings
toastr.options = {
    "positionClass": "toast-bottom-right",
};

//all countries for dropdown
const allPakistanCities = `
<option value="Karachi">Karachi</option>
<option value="Lahore">Lahore</option>
<option value="Faisalabad">Faisalabad</option>
<option value="Rawalpindi">Rawalpindi</option>
<option value="Gujranwala">Gujranwala</option>
<option value="Peshawar">Peshawar</option>
<option value="Multan">Multan</option>
<option value="Saidu Sharif">Saidu Sharif</option>
<option value="Hyderabad City">Hyderabad City</option>
<option value="Islamabad">Islamabad</option>
<option value="Quetta">Quetta</option>
<option value="Bahawalpur">Bahawalpur</option>
<option value="Sargodha">Sargodha</option>
<option value="Sialkot City">Sialkot City</option>
<option value="Sukkur">Sukkur</option>
<option value="Larkana">Larkana</option>
<option value="Chiniot">Chiniot</option>
<option value="Shekhupura">Shekhupura</option>
<option value="Jhang City">Jhang City</option>
<option value="Dera Ghazi Khan">Dera Ghazi Khan</option>
<option value="Gujrat">Gujrat</option>
<option value="Rahimyar Khan">Rahimyar Khan</option>
<option value="Kasur">Kasur</option>
<option value="Mardan">Mardan</option>
<option value="Nawabshah">Nawabshah</option>
<option value="Sahiwal">Sahiwal</option>
<option value="Mirpur Khas">Mirpur Khas</option>
<option value="Okara">Okara</option>
<option value="Mandi Burewala">Mandi Burewala</option>
<option value="Jacobabad">Jacobabad</option>
<option value="Saddiqabad">Saddiqabad</option>
<option value="Kohat">Kohat</option>
<option value="Muridke">Muridke</option>
<option value="Muzaffargarh">Muzaffargarh</option>
<option value="Khanpur">Khanpur</option>
<option value="Gojra">Gojra</option>
<option value="Mandi Bahauddin">Mandi Bahauddin</option>
<option value="Abbottabad">Abbottabad</option>
<option value="Turbat">Turbat</option>
<option value="Dadu">Dadu</option>
<option value="Khairpur Mirâ€™s">Khairpur Mirâ€™s</option>
<option value="Bahawalnagar">Bahawalnagar</option>
<option value="Khuzdar">Khuzdar</option>
<option value="Pakpattan">Pakpattan</option>
<option value="Zafarwal">Zafarwal</option>
<option value="Tando Allahyar">Tando Allahyar</option>
<option value="Jaranwala">Jaranwala</option>
<option value="Ahmadpur East">Ahmadpur East</option>
<option value="Vihari">Vihari</option>
<option value="New Mirpur">New Mirpur</option>
<option value="Kamalia">Kamalia</option>
<option value="Kot Addu">Kot Addu</option>
<option value="Nowshera">Nowshera</option>
<option value="Swabi">Swabi</option>
<option value="Khushab">Khushab</option>
<option value="Dera Ismail Khan">Dera Ismail Khan</option>
<option value="Chaman">Chaman</option>
<option value="Charsadda">Charsadda</option>
<option value="Kandhkot">Kandhkot</option>
<option value="Chishtian">Chishtian</option>
<option value="Hasilpur">Hasilpur</option>
<option value="Attock Khurd">Attock Khurd</option>
<option value="Kambar">Kambar</option>
<option value="Arifwala">Arifwala</option>
<option value="Muzaffarabad">Muzaffarabad</option>
<option value="Mianwali">Mianwali</option>
<option value="Jalalpur Jattan">Jalalpur Jattan</option>
<option value="Bhakkar">Bhakkar</option>
<option value="Zhob">Zhob</option>
<option value="Dipalpur">Dipalpur</option>
<option value="Kharian">Kharian</option>
<option value="Mian Channun">Mian Channun</option>
<option value="Bhalwal">Bhalwal</option>
<option value="Jamshoro">Jamshoro</option>
<option value="Pattoki">Pattoki</option>
<option value="Harunabad">Harunabad</option>
<option value="Kahror Pakka">Kahror Pakka</option>
<option value="Toba Tek Singh">Toba Tek Singh</option>
<option value="Samundri">Samundri</option>
<option value="Shakargarh">Shakargarh</option>
<option value="Sambrial">Sambrial</option>
<option value="Shujaabad">Shujaabad</option>
<option value="Hujra Shah Muqim">Hujra Shah Muqim</option>
<option value="Kabirwala">Kabirwala</option>
<option value="Mansehra">Mansehra</option>
<option value="Lala Musa">Lala Musa</option>
<option value="Chunian">Chunian</option>
<option value="Nankana Sahib">Nankana Sahib</option>
<option value="Bannu">Bannu</option>
<option value="Pasrur">Pasrur</option>
<option value="Timargara">Timargara</option>
<option value="Parachinar">Parachinar</option>
<option value="Chenab Nagar">Chenab Nagar</option>
<option value="Abdul Hakim">Abdul Hakim</option>
<option value="Gwadar">Gwadar</option>
<option value="Hassan Abdal">Hassan Abdal</option>
<option value="Tank">Tank</option>
<option value="Hangu">Hangu</option>
<option value="Risalpur Cantonment">Risalpur Cantonment</option>
<option value="Karak">Karak</option>
<option value="Kundian">Kundian</option>
<option value="Umarkot">Umarkot</option>
<option value="Chitral">Chitral</option>
<option value="Dainyor">Dainyor</option>
<option value="Kulachi">Kulachi</option>
<option value="Kalat">Kalat</option>
<option value="Kotli">Kotli</option>
<option value="Murree">Murree</option>
<option value="Mithi">Mithi</option>
<option value="Mian Sahib">Mian Sahib</option>
<option value="Gilgit">Gilgit</option>
<option value="Gilwala">Gilwala</option>
`;

//all countries for dropdown
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

var _URL_ = window.URL || window.webkitURL;
//#endregion

//#region document ready function
$(document).ready(function () {

    //#region ajax error function
    $(function () {
        $(document).ajaxError(function (e, xhr) {
            if (xhr.status == 401) {
                var response = $.parseJSON(xhr.responseText);
                window.location = response.LogOnUrl;
            }
            else if (xhr.status == 403) {
                try {
                    var response = $.parseJSON(xhr.responseText);
                    if (!swal.isVisible()) {
                        $('#myModal').modal('hide');

                        swal.fire(response.Error, response.Message, "warning").then(function () {
                            $('#myModal').modal('hide');
                        });
                    }
                } catch (ex) {
                    if (!swal.isVisible()) {
                        $('#myModal').modal('hide');

                        swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact administrator !", "warning").then(function () {
                            $('#myModal').modal('hide');
                        });
                    }
                }
            }
        });
    });

    //#endregion

    datepickerFunction();

    //changeButtonMethod();

    //select searchable (in popup view it should be reinitialized)
    SelectSearchable.init();

    //datepicker initialization .bstrp-datepicker
    datePicker();

    //append countries data in dropdown
    appendCountries();

    $('.form').submit(function () {
        const form = $(this);
        const btn = $("button[form='"+$(form).attr('id')+"']");

        if (validateForm(form)) {
            disableSubmitButton(btn, true);
            return true;
        }

        return false;
    });

    /*
     *	if you want to do sum action after form submit then, write (custom_form_view_callback) function in your view
     * */
    $('.custom-form-view').submit(function () {
        var form = $(this);
        if (validateForm(form)) {
            custom_form_view_submit(form);
        }
        return false;
    });

    setTimeout(function () { dateFormat(1); }, 1000);

    //initiate intl dropdown
    initIntlInputs();
});
//#endregion

//#region search filter functions
/**
 * SearchFilter()
 * @param {any} btn (this, search button)
 * @param {any} url (url of the search filter)
 */
function SearchFilter(btn, url) {
    
    disableSubmitButton(btn, true);

    let row = $(btn).closest('.search_filter_row');
    const fromDate = $(row).find('#fromDate').val();
    const toDate = $(row).find('#toDate').val();
    const id = $(row).find('#ID').val();
    const status = $(row).find('#Status').val();

    if (searchFilterValidation(fromDate, toDate)) {
        $.ajax({
            url: url,
            type: 'Post',
            data: {
                fromDate: fromDate,
                toDate: toDate,
                id: id,
                status: status,
            },
            success: function (response) {
                "use strict";

                if (response.data != null) {
                    if (typeof DateFilterCallBack === "function")
                        DateFilterCallBack(response.data, fromDate, toDate, "" , "" , "");
                }
                else {
                    if (typeof DateFilterCallBack === "function")
                        DateFilterCallBack(response, fromDate, toDate, "", "", "");
                }

                disableSubmitButton(btn, false);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                ErrorFunction(xhr, ajaxOptions, thrownError, btn);
            },
            failure: function (xhr, ajaxOptions, thrownError) {
                ErrorFunction(xhr, ajaxOptions, thrownError, btn);
            }
        });
    }
}

function searchFilterValidation(fromDate, toDate) {
    if (fromDate == "" && toDate == "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please! Select Date',
        })
        return false;
    }
    else if (fromDate == "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please! Select From Date',
        })
        return false;
    }
    else if (toDate == "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please! Select To Date',
        })
        return false;
    }
    return true;
}

// datepicker function
function datepickerFunction() {
    $("#fromDate").datepicker({
        todayHighlight: true,
    });

    $("#toDate").datepicker({
        todayHighlight: true,
    });

    $("#fromDate").change(function () {

        if (new Date($("#fromDate").val()) > new Date($("#toDate").val())) {
            $('#toDate').datepicker('setDate', new Date($("#fromDate").val()));
            $("#toDate").datepicker("option", "minDate", new Date($("#fromDate").val()));
        }
    });

    $("#toDate").change(function () {

        if (new Date($("#fromDate").val()) > new Date($("#toDate").val())) {
            $('#fromDate').datepicker('setDate', new Date($("#toDate").val()));
            $("#fromDate").datepicker("option", "maxDate", new Date($("#toDate").val()));
        }
    });

    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();

    $('#from').val(fromDate);
    $('#to').val(toDate);
}
//#endregion

//#region errors
function ErrorFunction(xhr, ajaxOptions, thrownError, btn) {

    if (xhr.status == 403) {
        try {
            var response = $.parseJSON(xhr.responseText);
            swal.fire(response.Error, response.Message, "warning").then(function () {
                $('#myModal').modal('hide');
            });
        } catch (ex) {
            swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact administrator !", "warning").then(function () {
                $('#myModal').modal('hide');
            });
        }

        if (btn) {
            disableSubmitButton(btn, false);
        }
    }
}
//#endregion

//#region methods
function FormatPrices() {
    $('td.price,span.price').each(function (k, v) {
        if (!$(v).hasClass('formatted')) {
            let text = $(v).text();
            //if (text.includes('-')) {
            //	text = text.split('-');
            //	let min = text[0].replace('AED', '').trim();
            //	let max = text[1].replace('AED', '').trim();

            //	$(v).html('AED ' + numeral(min).format('0,0.00') + ' - AED ' + numeral(max).format('0,0.00'));
            //} else {
            if (text.includes('AED')) {
                text = text.replace('AED', '').trim();
                $(v).html('AED ' + numeral(text).format('0,0.00'));
            } else {
                $(v).html(numeral(text).format('0,0.00'));
            }
            //}
            $(v).addClass('formatted');
        }
    });

    $('div.price').each(function (k, v) {
        if (!$(v).hasClass('formatted')) {
            let text = $(v).text();
            //if (text.includes('-')) {
            //	text = text.split('-');
            //	let min = text[0].replace('AED', '').trim();
            //	let max = text[1].replace('AED', '').trim();

            //	$(v).html('AED ' + numeral(min).format('0,0.00') + ' - AED ' + numeral(max).format('0,0.00'));
            //} else {
            if (text.includes('AED')) {
                text = text.replace('AED', '').trim();
                $(v).html('AED ' + numeral(text).format('0,0.00'));
            } else {
                $(v).html('AED ' + numeral(text).format('0,0.00'));
            }
            //}
            $(v).addClass('formatted');
        }
    });
}

//check input is string or not
function isString(evt) {
    evt = (evt) ? evt : window.event;
    const element = evt.target;

    const charCode = (evt.which) ? evt.which : evt.keyCode;
    if ((charCode < 65 || charCode > 90) && (charCode < 97 || charCode > 123) && charCode != 32) {
        element.setCustomValidity("Only alphabets can be written in the text field.");
        element.reportValidity();
        //$(element).removeClass('border border-success').addClass('border border-danger');

        return false;
    }
    element.setCustomValidity("");
    //$(element).removeClass('border border-danger').addClass('border border-success');;

    return true;
}

//check input is number or not
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    const element = evt.target;

    const charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        element.setCustomValidity("Only digits can be written in the contact field.");
        element.reportValidity();
        //$(element).removeClass('border border-success').addClass('border border-danger');

        return false;
    }
    element.setCustomValidity("");
    //$(element).removeClass('border border-danger').addClass('border border-success');;

    return true;
}

//remove space from element
function removeSpace() {
    setTimeout(function () {
        var value = $(".remove-space").val().replace(/ /g, '');
        $(".remove-space").val(value);
    }, 1000);
}

//make readonly element after some time
function readonly() {
    setTimeout(function () {
        $(".readonly").attr('readonly',true);
    }, 500);
}

//check email is valid or not
function isEmailValid(element) {
    let value = $(element).val();
    let requiredMessage = $(element).closest('.form-group').find('.field-validation-valid');

    const regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    if (value.match(regex)) {
        //true condition
        $(element).removeClass('border border-danger').addClass('border border-success');;
        $(requiredMessage).text('');

        element.setCustomValidity("");
    }
    else {
        $(element).removeClass('border border-success').addClass('border border-danger');
        $(requiredMessage).text('Invalid email address! Please write valid email address.');

        element.setCustomValidity("Please write valid email address.");
        element.reportValidity();
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
    let value = $(elem).val();
    let code = $(elem).closest('.form-group').find('.code').val();

    if (code) {
        ajaxVerification(code + '|' + value, id, url, elem, false);
    }
    else {

    }

    //if (!$(elem).closest('.form-group').find('span.field-validation-valid').text()) {
        
    //}
    //else {

    //}
}
//Ajax function for email and contact verification
function ajaxVerification(value, id, url, elem, isBorderCss = false) {
    let msgElement = $(elem).closest('.form-group').find('.field-validation-valid');
    $.ajax({
        type: 'POST',
        url: url,
        data: { value, id },
        success: function (response) {
            if (response.success) {
                //true condition
                if (isBorderCss)
                    $(elem).removeClass('border border-danger').addClass('border border-success');;

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
function isPasswordValid(element, e = event) {
    if (e.keyCode === 9)
        return false;

    let value = $(element).val();

    let requiredMessage = $(element).closest('.form-group').find('.field-validation-valid');

    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z@@*$&#%!~()\d]{8,}$/;
    if (value.match(regex)) {
        //true condition
        $(element).removeClass('border border-danger').addClass('border border-success');;
        $(requiredMessage).text('');

        element.setCustomValidity("");
    }
    else {
        $(element).removeClass('border border-success').addClass('border border-danger');
        $(requiredMessage).text('Min. 8 characters, at least one uppercase letter, one lowercase letter, and one number');

        element.setCustomValidity("Please match the password pattern.");
        element.reportValidity();
    }

    isPasswordMatch($('.confirm-password'));
}

//check password is valid or not
function isPasswordMatch(element, e = event) {
    if (e.keyCode === 9)
        return false;

    let password = $(element).closest('form').find('.password').val();
    let value = $(element).val();
    let requiredMessage = $(element).closest('.form-group').find('.field-validation-valid');

    if (value.match(password)) {
        //true condition
        $(element).removeClass('border border-danger').addClass('border border-success');;
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

    if (type === "text")
    {
        $(icon).find('i').removeClass('fa-eye-slash');
        $(icon).find('i').addClass('fa-eye');
        $(input).attr('type', 'password');
    }
    else if (type === "password")
    {
        $(icon).find('i').removeClass('fa-eye');
        $(icon).find('i').addClass('fa-eye-slash');
        $(input).attr('type', 'text');
    }
    
}

function goBack() {
    window.location = document.referrer;
    //window.history.go(-1); //this method cannot change the state of previous page.
}

function GetCurrencyAmount(amount) {
    return `AED ${amount}`;
};

//change string numbers into digits
function convertStringIntoDigits(value, toFixed = 2) {
    if (toFixed > 0) {
        value = Number(value.replace(/[^0-9.]/g, '')).toFixed(toFixed);
    }
    else {
        value = Number(value.replace(/[^0-9]/g, ''));
    }
    return value;
}

function disableSubmitButton(btn, flag = true, spanElement = "") {

    if (!spanElement) {
        if (flag) {
            $(btn).addClass('spinner spinner-left spinner-sm').attr('disabled', true);
        }
        else {
            $(btn).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
        }
    }
    else {
        if (flag) {
        }
        else {
        }
    }
}

function changeButtonMethod() {

    $('#Userform input').prop('disabled', true);
    $('#Userform textarea').prop('disabled', true);
    $('#edit-cancel').hide();
    $('#save-changes').hide();
    $('#edit-profile').fadeIn();

    $('#edit-profile').click(function () {

        $('#Userform input').prop('disabled', false);
        $('#Userform textarea').prop('disabled', false);

        $('#edit-profile').hide();
        $('#edit-cancel').fadeIn();
        $('#save-changes').fadeIn();
    });

    $('#edit-cancel').click(function () {
        $('#Userform input').prop('disabled', true);
        $('#Userform textarea').prop('disabled', true);
        $('#edit-cancel').hide();
        $('#save-changes').hide();
        $('#edit-profile').fadeIn();
    });

}

function appendCountries() {
    $('.countries').append(allCountries);

    $.each($('.countries'), function (k, val) {
        if ($(this).attr('data-value')) {
            $(this).val($(this).attr('data-value'));
        }
    });
}


function appendPakCities() {
    $('.pak-cities').append(allPakistanCities);

    $.each($('.pak-cities'), function (k, val) {
        if ($(this).attr('data-value')) {
            $(this).val($(this).attr('data-value'));
        }
    });
}

//image change event
function imgUpload(elem) {
    
    let btnCancel, file, img, size, originalWidth, originalHeight, width, height, ratio;
    btnCancel = $(elem).closest('.form-group').find('.cancelimage');

    if ($(elem.files[0]).length) {

        size = Number($(elem).attr('data-size'));
        originalWidth = Number($(elem).attr('data-width'));
        originalHeight = Number($(elem).attr('data-height'));

        file = elem.files[0];

        img = new Image();
        img.onload = function () {

            width = this.width;
            height = this.height;
            ratio = ((originalHeight / originalWidth) * width);
            ratio = Math.floor(ratio);

            if (ratio !== height) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: `Image dimension should be ${originalWidth} x ${originalHeight} !`,
                }).then(function (result) {
                    $(elem).attr("src", "/Assets/AppFiles/Images/default.png");
                    $(btnCancel).trigger('click');
                });
            }
            else if (this.size > size * 1000) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: `Image size must be less than ${size} kb!`,
                }).then(function (result) {
                    $(elem).attr("src", "/Assets/AppFiles/Images/default.png");
                    $(btnCancel).trigger('click');
                });
            }
            else {
                img.onerror = function () {
                    alert("not a valid file: " + file.type);
                };
            }
        };
        img.src = _URL_.createObjectURL(file);
    }
}
//video change event
function videoUpload(elem) {


    let file, size;
    size = Number($(elem).attr('data-size'));
    if ((file = elem.files[0])) {

        if (!file.type.match('video.*')) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Please upload valid video file !',

            }).then(function (result) {
                $(elem).val("");
            });
        }
        else if (file.size >= size * 1000000) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: `Video size must be less than ${size} mb !`,
            }).then(function (result) {
                $(elem).val("");
            });
        }
    }
}

//Delete video
function videoDelete(elem) {
    let videoDivClass = "." + $(elem).attr('for');
    $(elem).closest(".video-frame").hide();
    $(elem).closest(".video-frame").find("source").attr('src', '');
    $(videoDivClass).show();
    
    $(videoDivClass).find('input').attr('value', '');
}
function imageDelete(elem) {
    let imageDivClass = "." + $(elem).attr('for');
    $(elem).closest(".image-frame").hide();
    $(elem).closest(".image-frame").find("input").attr('value', '');
    $(imageDivClass).show();
    
    $(imageDivClass).find('input').attr('value', '');
}

//select searchable initialization
$('.kt-selectpicker').attr("data-live-search", "true").attr("data-size", "8");
var SelectSearchable = function () {

    // Private functions
    var demos = function () {
        // minimum setup
        $('.kt-selectpicker').selectpicker();
    }

    return {
        // public functions
        init: function () {
            demos();
        }
    };
}();

//#endregion

//#region validation form submission on simple page
function validateForm(form) {
    let formValidate = true;

    //insert, remove validate message on input fields
    $.each($(form).find('.required-custom'), function (k, elem) {
        if (
            (!$(elem).attr('multiple') && $(elem).val())
            ||
            ($(elem).attr('multiple') && $(elem).val().length)
        ) {
            if ($(elem).hasClass('password') || $(elem).hasClass('email') || $(elem).hasClass('confirm-password')) {
                if ($(elem).closest('.form-group').find('span.field-validation-valid').text() && $(elem).hasClass('border-danger')) {
                }
                else {
                    $(elem).closest('.form-group').find('span.field-validation-valid').text('');
                }
            }
            else {
                $(elem).closest('.form-group').find('span.field-validation-valid').text('');
            }
        }
        else {
            $(elem).closest('.form-group').find('span.field-validation-valid').text($(elem).closest('.form-group').find('label').text() + " is required.");
        }
    });

    //check if there is non validate input fields
    $.each($(form).find('.required-custom'), function (k, elem) {
        if ($(elem).closest('.form-group').find('span.field-validation-valid').text()) {
            formValidate = false;

            return false; // breaks
        }
        else {
            formValidate = true;
        }
    });

    //return true of false
    if (formValidate) {
        return true;
    }
    else {
        return false;
    }
}

function custom_form_view_submit(form) {
    

    let btn = $(`button[form="${$(form).attr('id')}"]`);
    if (!btn) {
        btn = $(form).find('button[type="submit"]');
    }
    disableSubmitButton(btn, true);

    $.ajax({
        url: $(form).attr('action'),
        type: 'Post',
        data: $(form).serialize(),
        success: function (response) {
            if (response.success) {
                toastr.success(response.message)
                disableSubmitButton(btn, false);
                $(form).find('.input-fields').val('').removeClass("border border-success border-danger");;
                $(form).find('.input').removeClass("border border-success border-danger");;

                if (typeof custom_form_view_callback === "function")
                    custom_form_view_callback(form, response);
            }
            else {
                toastr.error(response.message)
                if (typeof custom_form_view_callback === "function")
                    custom_form_view_callback(form, response);
                disableSubmitButton(btn, false);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorFunction(xhr, ajaxOptions, thrownError, btn);
        },
        failure: function (xhr, ajaxOptions, thrownError) {
            ErrorFunction(xhr, ajaxOptions, thrownError, btn);
        }
    });

}

function form_file_custom(form) {
    /*
    *	this function is use for form which have some files, in edit form  we need to write attribute (data-value = db.file.path) for delete and update proecess
    * */
    if (validateForm(form)) {
        let btn = $(`button[form="${$(form).attr('id')}"]`);
        if (!btn) {
            btn = $(form).find('button[type="submit"]');
        }
        disableSubmitButton(btn, true);

        var data = new FormData();

        $.each($(form).find('input'), function (k, elem) {
            if ($(elem).attr('type') == "file") {

                let files = $(elem)[0].files;
                if (files.length) {
                    data.append($(elem).attr('name'), files[0]);
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
            url: $(form).attr('action'),
            type: 'Post',
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: data,
            success: function (response) {
                if (response.success) {
                    if (typeof callback === "function") {
                        callback($('#myModalContent'), element, true, response);
                    }
                }
                else {

                    toastr.error(response.message)
                    if (typeof form_file_custom_callback === "function")
                        form_file_custom_callback(form, response);
                    disableSubmitButton(btn, false);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                ErrorFunction(xhr, ajaxOptions, thrownError, btn);
            },
            failure: function (xhr, ajaxOptions, thrownError) {
                ErrorFunction(xhr, ajaxOptions, thrownError, btn);
            }
        });
    }
    return false;
}

function datePicker() {
    $(".bstrp-datepicker").datepicker({
        todayHighlight: true,
        format: 'dd MM yyyy',
    });
}

//#endregion

//#region Play Functions
function PlayVideo(elem) {

    if (elem.localName == "video") {
        if ($(elem).hasClass('played')) {
            $(elem).closest('div').find('i').removeClass('fa-pause-circle').addClass('fa-play-circle');
            $(elem).removeClass('played').trigger('pause');
        }
        else {
            $(elem).closest('div').find('i').removeClass('fa-pause-circle').addClass('fa-pause-circle');
            $(elem).addClass('played').trigger('play');
        }
    }
    else {

        if ($(elem).prevAll('video').hasClass('played')) {
            $(elem).find('i').removeClass('fa-pause-circle').addClass('fa-play-circle');
            $(elem).prevAll('video').removeClass('played').trigger('pause');
        }
        else {
            $(elem).find('i').removeClass('fa-play-circle').addClass('fa-pause-circle');
            $(elem).prevAll('video').addClass('played').trigger('play');
        }
    }
}
//#endregion
