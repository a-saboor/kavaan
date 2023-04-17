"use strict";

var productAttributes = [];

var ProjectAttributeSetting = [];

var productVariationAttributes = [];


// Class definition
var KTWizard2 = function () {
    // Base elements
    var _wizardEl;
    var _formEl;
    var _wizardObj;
    var _validations = [];

    // Private functions
    var _initWizard = function () {
        // Initialize form wizard
        _wizardObj = new KTWizard(_wizardEl, {
            startStep: 1, // initial active step number
            clickableSteps: true, // to make steps clickable this set value true and add data-wizard-clickable="true" in HTML for class="wizard" element
            buttons: false

        });

        // Validation before going to next page
        _wizardObj.on('change', function (wizard) {
            if (wizard.getStep() > wizard.getNewStep()) {
                return; // Skip if stepped back
            }

            // Validate form before change wizard step
            var validator = _validations[wizard.getStep() - 1]; // get validator for currnt step

            if (validator) {
                validator.validate().then(function (status) {
                    if (status == 'Valid') {
                        wizard.goTo(wizard.getNewStep());

                        //KTUtil.scrollTop();
                    } else {
                        Swal.fire({
                            text: "Sorry, looks like there are some errors detected, please try again.",
                            icon: "error",
                            buttonsStyling: false,
                            confirmButtonText: "Ok, got it!",
                            customClass: {
                                confirmButton: "btn font-weight-bold btn-light"
                            }
                        }).then(function () {
                            //KTUtil.scrollTop();
                        });
                    }
                });
            }

            return false;  // Do not change wizard step, further action will be handled by he validator
        });

        // Change event
        _wizardObj.on('changed', function (wizard) {
            //KTUtil.scrollTop();
        });

        // Submit event
        _wizardObj.on('submit', function (wizard) {
            Swal.fire({
                text: "All is good! Please confirm the form submission.",
                icon: "success",
                showCancelButton: true,
                buttonsStyling: false,
                confirmButtonText: "Yes, submit!",
                cancelButtonText: "No, cancel",
                customClass: {
                    confirmButton: "btn font-weight-bold btn-primary",
                    cancelButton: "btn font-weight-bold btn-default"
                }
            }).then(function (result) {
                if (result.value) {
                    _formEl.submit(); // Submit form
                } else if (result.dismiss === 'cancel') {
                    Swal.fire({
                        text: "Your form has not been submitted!.",
                        icon: "error",
                        buttonsStyling: false,
                        confirmButtonText: "Ok, got it!",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-primary",
                        }
                    });
                }
            });
        });
    }

    var _initValidation = function () {
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        // Step 1
        _validations.push(FormValidation.formValidation(
            //_formEl,
            //{
            //	fields: {
            //		fname: {
            //			validators: {
            //				notEmpty: {
            //					message: 'First name is required'
            //				}
            //			}
            //		},
            //		lname: {
            //			validators: {
            //				notEmpty: {
            //					message: 'Last Name is required'
            //				}
            //			}
            //		},
            //		phone: {
            //			validators: {
            //				notEmpty: {
            //					message: 'Phone is required'
            //				}
            //			}
            //		},
            //		email: {
            //			validators: {
            //				notEmpty: {
            //					message: 'Email is required'
            //				},
            //				emailAddress: {
            //					message: 'The value is not a valid email address'
            //				}
            //			}
            //		}
            //	},
            //	plugins: {
            //		trigger: new FormValidation.plugins.Trigger(),
            //		// Bootstrap Framework Integration
            //		bootstrap: new FormValidation.plugins.Bootstrap({
            //			//eleInvalidClass: '',
            //			eleValidClass: '',
            //		})
            //	}
            //}
        ));

        // Step 2
        _validations.push(FormValidation.formValidation(
            _formEl,
            {
                fields: {
                    address1: {
                        validators: {
                            notEmpty: {
                                message: 'Address is required'
                            }
                        }
                    },
                    postcode: {
                        validators: {
                            notEmpty: {
                                message: 'Postcode is required'
                            }
                        }
                    },
                    city: {
                        validators: {
                            notEmpty: {
                                message: 'City is required'
                            }
                        }
                    },
                    state: {
                        validators: {
                            notEmpty: {
                                message: 'State is required'
                            }
                        }
                    },
                    country: {
                        validators: {
                            notEmpty: {
                                message: 'Country is required'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    // Bootstrap Framework Integration
                    bootstrap: new FormValidation.plugins.Bootstrap({
                        //eleInvalidClass: '',
                        eleValidClass: '',
                    })
                }
            }
        ));

        // Step 3
        _validations.push(FormValidation.formValidation(
            _formEl,
            {
                fields: {
                    delivery: {
                        validators: {
                            notEmpty: {
                                message: 'Delivery type is required'
                            }
                        }
                    },
                    packaging: {
                        validators: {
                            notEmpty: {
                                message: 'Packaging type is required'
                            }
                        }
                    },
                    preferreddelivery: {
                        validators: {
                            notEmpty: {
                                message: 'Preferred delivery window is required'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    // Bootstrap Framework Integration
                    bootstrap: new FormValidation.plugins.Bootstrap({
                        //eleInvalidClass: '',
                        eleValidClass: '',
                    })
                }
            }
        ));

        // Step 4
        _validations.push(FormValidation.formValidation(
            _formEl,
            {
                fields: {
                    locaddress1: {
                        validators: {
                            notEmpty: {
                                message: 'Address is required'
                            }
                        }
                    },
                    locpostcode: {
                        validators: {
                            notEmpty: {
                                message: 'Postcode is required'
                            }
                        }
                    },
                    loccity: {
                        validators: {
                            notEmpty: {
                                message: 'City is required'
                            }
                        }
                    },
                    locstate: {
                        validators: {
                            notEmpty: {
                                message: 'State is required'
                            }
                        }
                    },
                    loccountry: {
                        validators: {
                            notEmpty: {
                                message: 'Country is required'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    // Bootstrap Framework Integration
                    bootstrap: new FormValidation.plugins.Bootstrap({
                        //eleInvalidClass: '',
                        eleValidClass: '',
                    })
                }
            }
        ));

        // Step 5
        _validations.push(FormValidation.formValidation(
            _formEl,
            {
                fields: {
                    ccname: {
                        validators: {
                            notEmpty: {
                                message: 'Credit card name is required'
                            }
                        }
                    },
                    ccnumber: {
                        validators: {
                            notEmpty: {
                                message: 'Credit card number is required'
                            },
                            creditCard: {
                                message: 'The credit card number is not valid'
                            }
                        }
                    },
                    ccmonth: {
                        validators: {
                            notEmpty: {
                                message: 'Credit card month is required'
                            }
                        }
                    },
                    ccyear: {
                        validators: {
                            notEmpty: {
                                message: 'Credit card year is required'
                            }
                        }
                    },
                    cccvv: {
                        validators: {
                            notEmpty: {
                                message: 'Credit card CVV is required'
                            },
                            digits: {
                                message: 'The CVV value is not valid. Only numbers is allowed'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    // Bootstrap Framework Integration
                    bootstrap: new FormValidation.plugins.Bootstrap({
                        //eleInvalidClass: '',
                        eleValidClass: '',
                    })
                }
            }
        ));
    }

    return {
        // public functions
        init: function () {
            _wizardEl = KTUtil.getById('kt_wizard');
            _formEl = KTUtil.getById('kt_form');

            _initWizard();
            _initValidation();
        }
    };
}();

var KTTagifyDemos = function (id) {
    var tagify;
    // Private functions
    var demo1 = function (id) {

        var prod_attr = productAttributes.filter(function (obj) {
            return obj.attributeId == id
        });

        var input = document.getElementById("kt_tagify_" + id);
        // init Tagify script on the above inputs
        tagify = new Tagify(input)


        tagify.addTags(prod_attr);
        // "remove all tags" button event listener


        // Chainable event listeners
        tagify.on('add', function (e) {
            $.ajax({
                url: '/Admin/ProjectAttribute/Create/',
                type: 'POST',
                data: {
                    "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
                    productAttribute: {
                        ProjectID: GetURLParameter(),
                        AttributeID: $(e.detail.tag).closest('.attribute').attr('id'),
                        Value: e.detail.data.value
                    }
                },
                success: function (response) {
                    if (response.success) {
                        //$(this).attr('productCategoryId', '');
                        productAttributes.push({
                            attributeId: $(e.detail.tag).closest('.attribute').attr('id'),
                            id: response.data,
                            value: e.detail.data.value
                        });
                        BindVariationAttributes();

                        toastr.success(response.message);
                    }
                    else {
                        toastr.error(response.message);
                        return false;
                    }
                },
                error: function (e) {
                    toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                },
                failure: function (e) {
                    toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                }
            });
        })
        tagify.on('remove', function (e) {

            if (e.detail.data.id) {

                $.ajax({
                    url: '/Admin/ProjectAttribute/Delete/' + e.detail.data.id,
                    type: 'POST',
                    data: {
                        "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
                    },
                    success: function (response) {
                        if (response.success) {
                            //$(this).attr('productCategoryId', '');

                            productAttributes = productAttributes.filter(function (obj) {
                                return obj.id != e.detail.data.id
                            });

                            BindVariationAttributes();

                            toastr.success(response.message);
                        }
                        else {
                            toastr.error(response.message);
                            return false;
                        }
                    },
                    error: function (e) {
                        toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                    },
                    failure: function (e) {
                        toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                    }
                });
            } else {
                $.ajax({
                    url: '/Admin/ProjectAttribute/DeleteValue/',
                    type: 'POST',
                    data: {
                        "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
                        productAttribute: {
                            ProjectID: GetURLParameter(),
                            AttributeID: $(e.detail.tagify.DOM.input).closest('.attribute').attr('id'),
                            Value: e.detail.data.value
                        }
                    },
                    success: function (response) {
                        if (response.success) {
                            productAttributes = productAttributes.filter(function (obj) {
                                return obj.id != response.data
                            });

                            BindVariationAttributes();
                            toastr.success(response.message);
                        }
                        else {
                            toastr.error(response.message);
                            return false;
                        }
                    },
                    error: function (e) {
                        toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                    },
                    failure: function (e) {
                        toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                    }
                });
            }
        });
    }

    return {
        // public functions
        init: function (id) {
            demo1(id);
        }
    };
}();

var KTTagifyProjectTags = function (id) {

    var tagify;
    // Private functions
    var demo5 = function (id) {
        $.ajax({
            url: '/Admin/ProjectTag/GetProjectTags/' + GetURLParameter(),
            type: 'GET',
            success: function (response) {
                if (response.success) {

                    // Init autocompletes
                    var toEl = document.getElementById(id);
                    tagify = new Tagify(toEl, {
                        delimiters: ", ", // add new tags when a comma or a space character is entered
                        maxTags: 10,
                        blacklist: ["fuck", "shit", "pussy"],
                        keepInvalidTags: false, // do not remove invalid tags (but keep them marked as invalid)
                        whitelist: response.tags,
                        enforceWhitelist: true,
                        templates: {
                            dropdownItem: function (tagData) {
                                try {
                                    var html = '';

                                    html += '<div class="tagify__dropdown__item">';
                                    html += '   <div class="d-flex align-items-center">';
                                    html += '       <div class="d-flex flex-column">';
                                    html += '           <a href="javascript:void(0);" id="' + (tagData.id ? tagData.id : '') + '" class="text-dark-75 text-hover-primary font-weight-bold">' + (tagData.value ? tagData.value : '') + '</a>';
                                    html += '       </div>';
                                    html += '   </div>';
                                    html += '</div>';

                                    return html;
                                } catch (err) { }
                            }
                        },
                        transformTag: function (tagData) {
                            tagData.class = 'tagify__tag tagify__tag--primary';
                        },
                        dropdown: {
                            classname: "color-blue",
                            enabled: 1,
                            maxItems: 5
                        }
                    })

                    tagify.addTags(response.projectTags);
                    tagify.on('dropdown:select', onSelectSuggestion)
                    tagify.on('remove', function (e) {

                        $.ajax({
                            url: '/Admin/ProjectTag/Delete/',
                            type: 'POST',
                            data: {
                                "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
                                projectTag: { ProjectID: GetURLParameter(), TagID: e.detail.data.id }
                            },
                            success: function (response) {
                                if (response.success) {
                                    toastr.success(response.message);
                                }
                                else {
                                    toastr.error(response.message);
                                    return false;
                                }
                            },
                            error: function (e) {
                                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                            },
                            failure: function (e) {
                                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                            }
                        });
                    })
                } else {
                }
            }
        });

        var addAllSuggestionsElm;

        function onSelectSuggestion(e) {

            $.ajax({
                url: '/Admin/ProjectTag/Create/',
                type: 'POST',
                data: {
                    "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
                    projectTag: {
                        ProjectID: GetURLParameter(),
                        TagID: $(e.detail.tagify.DOM.dropdown).find('.tagify__dropdown__item--active a').attr('id')
                    }
                },
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                    }
                    else {
                        toastr.error(response.message);
                        return false;
                    }
                },
                error: function (e) {
                    toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                },
                failure: function (e) {
                    toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                }
            });
        }

        // create a "add all" custom suggestion element every time the dropdown changes
        function getAddAllSuggestionsElm() {
            // suggestions items should be based on "dropdownItem" template
            return tagify.parseTemplate('dropdownItem', [{
                class: "addAll",
                name: "Add all",
                email: tagify.settings.whitelist.reduce(function (remainingSuggestions, item) {
                    return tagify.isTagDuplicate(item.value) ? remainingSuggestions : remainingSuggestions + 1
                }, 0) + " Members"
            }])
        }
    }
    return {
        // public functions
        init: function (id) {
            demo5(id);
        }
    };
}();

window.addEventListener('load', function () {

    $('#ProjectType').trigger('change');
})

jQuery(document).ready(function () {

    toastr.options = {
        "positionClass": "toast-bottom-right",
    };

    $("#Name").on('change', function () {
        var name = $(this);
        $("#Slug").val($(name).val().replace(/ /g, "-").toLocaleLowerCase());
    });
    var date = new Date();
    function truncateDate(date) {
        return new Date(date.getFullYear(), date.getMonth(), date.getDate());
    }
    date.setDate(date.getDate() - 1);
    $('input[name=startDate]').datepicker({
        startDate: truncateDate(date)
    });
    $('input[name=endDate]').datepicker({
        startDate: truncateDate(date)
    });

    KTWizard2.init();

    KTTagifyProjectTags.init("kt_tagify_product_tags");

    $('#IsManageStock').change(function () {
        if ($(this).prop('checked')) {
            $('.managed-stock').slideDown();
            $('.unmanaged-stock').slideUp();
        } else {

            $('.managed-stock').slideUp();
            $('.unmanaged-stock').slideDown();
        }

        //$('.managed-stock').animate({ opacity: 'toggle' }, 'fast');
        //$('.unmanaged-stock').animate({ opacity: 'toggle' }, 'fast');
    });

    $('.btn-add-attribute').click(function () {
        var id = $('#AttributeID').val();
        var name = $('#AttributeID :selected').text().trim();

        if ($('#AttributeID').val()) {
            $(this).find('i').hide();
            $(this).addClass('spinner spinner-center spinner-darker-primary').prop('disabled', true);
            //AJAX CALL To SERVER

            var prod_attr = productAttributes.filter(function (obj) {
                return obj.attributeId == id
            });

            var prod_attr_stg = ProjectAttributeSetting.find(function (obj) {
                return obj.attributeId == id
            });

            var template = '<div class="row attribute attribute' + id + '" id="' + id + '">';
            template += '	<div class="col-xl-4">';
            template += '		<!--begin::Select-->';
            template += '		<div class="form-group">';
            template += '			<label class="form-control form-control-solid form-control-lg name">' + name + '</label>';
            template += '		</div>';
            template += '		<!--end::Select-->';
            template += '	</div>';
            template += '	<div class="col-xl-7">';
            template += '		<div class="form-group">';
            template += '			<input id="kt_tagify_' + id + '" class="form-control tagify" name="attribute-values" placeholder="type..." value="" autofocus="" />';
            template += '		</div>';
            template += '	</div>';
            template += '	<div class="col-xl-1">';
            template += '		<button type="button" class="btn btn-icon btn-light-danger btn-circle btn-sm float-right btn-remove-attribute" onclick="DeleteAttribute(this)">';
            template += '			<i class="flaticon2-delete"></i>';
            template += '		</button>';
            template += '	</div>';
            if (prod_attr_stg) {
                template += '	<div class="col-xl-12 attribute-setting" id="' + prod_attr_stg.id + '">';
                template += '		<div class="form-group">';
                template += '			<span class="switch ml-1">';
                template += '				<label>';
                template += '					<input type="checkbox" name="ProjectPageVisiblity" onchange="UpdateProjectAttributeSetting(this)" value="" ' + (prod_attr_stg.ProjectPageVisiblity ? 'checked' : '') + ' />';
                template += '					<span></span>';
                template += '				</label> Visible on the project page';
                template += '			</span>';
                template += '		</div>'
                template += '		<div class="form-group">';
                template += '			<span class="switch ml-1">';
                template += '				<label>';
                template += '					<input type="checkbox" name="VariationUsage" onchange="UpdateProjectAttributeSetting(this)" value="" ' + (prod_attr_stg.VariationUsage ? 'checked' : '') + '/>';
                template += '					<span></span>';
                template += '				</label> Used for variations';
                template += '			</span>';
                template += '		</div>';
                template += '	</div>';
            } else {
                template += '	<div class="col-xl-12 attribute-setting">';
                template += '		<div class="form-group">';
                template += '			<span class="switch ml-1">';
                template += '				<label>';
                template += '					<input type="checkbox" name="ProjectPageVisiblity" onchange="UpdateProjectAttributeSetting(this)" value=""  />';
                template += '					<span></span>';
                template += '				</label> Visible on the project page';
                template += '			</span>';
                template += '		</div>'
                template += '		<div class="form-group">';
                template += '			<span class="switch ml-1">';
                template += '				<label>';
                template += '					<input type="checkbox" name="VariationUsage" onchange="UpdateProjectAttributeSetting(this)" value=""/>';
                template += '					<span></span>';
                template += '				</label> Used for variations';
                template += '			</span>';
                template += '		</div>';
                template += '	</div>';
            }
            template += '	<div class="separator separator-dashed my-1"></div>';
            template += '</div>';

            $(template).prependTo($('.product-attributes')).slideDown("fast");
            KTTagifyDemos.init(id);

            $(this).removeClass('spinner spinner-center spinner-darker-primary').prop('disabled', false);
            $(this).find('i').show();

            $('option:selected', $('#AttributeID')).remove();
            if (!prod_attr_stg) {
                $.ajax({
                    url: '/Admin/ProjectAttributeSetting/Create/',
                    type: 'POST',
                    data: {
                        "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
                        productAttributeSetting: {
                            ProjectID: GetURLParameter(),
                            AttributeID: id,
                            ProjectPageVisiblity: $('.attribute' + id).find('input[name=ProjectPageVisiblity]').prop('checked'),
                            VariationUsage: $('.attribute' + id).find('input[name=VariationUsage]').prop('checked'),
                        }
                    },
                    success: function (response) {
                        if (response.success) {
                            $('.attribute' + id).find('.attribute-setting').attr('id', response.data);
                            ProjectAttributeSetting.push({
                                id: response.data,
                                attributeId: id,
                                attributeName: $('.attribute' + id).find('label.name').text().trim(),
                                ProjectPageVisiblity: $('.attribute' + id).find('input[name=ProjectPageVisiblity]').prop('checked'),
                                VariationUsage: $('.attribute' + id).find('input[name=VariationUsage]').prop('checked'),
                            });

                            BindVariationAttributes();
                        }
                        else {
                            return false;
                            if ($('.attribute' + id).find('input[name=ProjectPageVisiblity]').prop('checked')) {
                                $('.attribute' + id).find('input[name=ProjectPageVisiblity]').prop('checked', false);
                            } else {
                                $('.attribute' + id).find('input[name=ProjectPageVisiblity]').prop('checked', true);
                            }

                            if ($('.attribute' + id).find('input[name=VariationUsage]').prop('checked')) {
                                $('.attribute' + id).find('input[name=VariationUsage]').prop('checked', false);
                            } else {
                                $('.attribute' + id).find('input[name=VariationUsage]').prop('checked', true);
                            }
                        }
                    }
                });
            }

        } else {
            jQuery.ajaxSetup({ cache: false });
            jQuery('#AttributeModal').modal({}, 'show');
        }
    });

    $('#ProjectType').change(function () {
        if ($(this).val() == "1") {
            $('.variable-product').slideUp();
            $('.simple-product').slideDown();
            $('#wizard-step-general').click();
        } else if ($(this).val() == "2") {
            $('.variable-product').slideDown();
            $('.simple-product').slideUp();
            $('#wizard-step-inventory').click();
        }
    });

    $('.btn-schedule').click(function () {
        if ($(this).text() == "Schedule") {
            $(this).closest('.variat').find('.variat-sale-price-dates').slideDown();
            $(this).text('Cancel');

        } else {
            $(this).closest('.variat').find('.variat-sale-price-dates').slideUp();
            $(this).text('Schedule');
        }
    });

    $('.variat-manage-stock').change(function () {
        if ($(this).prop('checked')) {
            $(this).closest('.variat').find('.managed-stock').slideDown();
            $(this).closest('.variat').find('.unmanaged-stock').slideUp();
        } else {

            $(this).closest('.variat').find('.managed-stock').slideUp();
            $(this).closest('.variat').find('.unmanaged-stock').slideDown();
        }

        //$('.managed-stock').animate({ opacity: 'toggle' }, 'fast');
        //$('.unmanaged-stock').animate({ opacity: 'toggle' }, 'fast');
    });

    BindProjectCategories();

    BindProjectImages();

    $('#kt_image_product_image #Image').change(function () {
        var data = new FormData();
        var files = $("#kt_image_product_image #Image").get(0).files;
        var elem = $(this)
        if (files.length > 0) {
            $(elem).closest('.image-input').find('label[data-action="change"] i').hide();
            $(elem).closest('.image-input').find('label[data-action="change"]').addClass('spinner spinner-dark spinner-center spinner-sm').prop('disabled', true);
            data.append("Image", files[0]);
            $.ajax({
                url: "/Admin/Project/Thumbnail/" + GetURLParameter(),
                type: "POST",
                processData: false,
                contentType: false,
                data: data,
                success: function (response) {
                    if (response.success) {
                        $('#kt_image_product_image .image-input-wrapper').css('background-image', 'url(' + response.data + ')');
                        toastr.success(response.message);
                    } else {
                        toastr.error(response.message);
                    }
                    $(elem).closest('.image-input').find('label[data-action="change"] i').show();
                    $(elem).closest('.image-input').find('label[data-action="change"]').removeClass('spinner spinner-dark spinner-center spinner-sm').prop('disabled', false);
                },
                error: function (e) {
                    $(elem).closest('.image-input').find('label[data-action="change"] i').show();
                    $(elem).closest('.image-input').find('label[data-action="change"]').removeClass('spinner spinner-dark spinner-center spinner-sm').prop('disabled', false);
                    toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                },
                failure: function (e) {
                    $(elem).closest('.image-input').find('label[data-action="change"] i').show();
                    $(elem).closest('.image-input').find('label[data-action="change"]').removeClass('spinner spinner-dark spinner-center spinner-sm').prop('disabled', false);
                    toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                }
            });
        }
    });

    $('#btn-gallery-image-upload').click(function () {
        $('input[name=GalleryImages]').trigger('click');
    });

    $('input[name=GalleryImages]').change(function () {

        var data = new FormData();
        var files = $("input[name=GalleryImages]").get(0).files;
        if (files.length > 0) {
            $('#btn-gallery-image-upload').addClass('spinner spinner-light spinner-left').prop('disabled', true);
            $('#btn-gallery-image-upload').find('span').hide();
            $.each(files, function (j, file) {
                data.append('Image[' + j + ']', file);
            })
            //data.append("Image", files);
            data.append("count", $('.product-gallery-images .symbol').length);

            $.ajax({
                url: "/Admin/ProjectImage/Create/" + GetURLParameter(),
                type: "POST",
                processData: false,
                contentType: false,
                data: data,
                success: function (response) {
                    if (response.success) {
                        $(response.data).each(function (k, v) {
                            $('.product-gallery-images').append('<div class="symbol symbol-70 flex-shrink-0 mr-5 mb-3">' +
                                '<span class="btn btn-xs btn-icon btn-circle btn-danger btn-hover-text-primary btn-shadow btn-remove-gallery-image" data-action="cancel" data-toggle="tooltip" title="remove" onclick="DeleteGalleryImage(this,' + v.Key + ')">' +
                                '<i class="icon-xs ki ki-bold-close ki-bold-trash"></i>' +
                                '</span>' +
                                '<div class="symbol-label" style="background-image: url(\'' + v.Value + '\')"></div>' +
                                '</div>');
                        });

                        $('#btn-gallery-image-upload').removeClass('spinner spinner-light spinner-left').prop('disabled', false);
                        $('#btn-gallery-image-upload').find('span').show();
                        toastr.success("Gallery images uploaded ...");
                    } else {
                        $('#btn-gallery-image-upload').removeClass('spinner spinner-light spinner-left').prop('disabled', false);
                        $('#btn-gallery-image-upload').find('span').show();
                        toastr.error(response.message);
                    }
                },
                error: function (e) {
                    $('#btn-gallery-image-upload').removeClass('spinner spinner-light spinner-left').prop('disabled', false);
                    $('#btn-gallery-image-upload').find('span').show();
                    toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                },
                failure: function (e) {
                    $('#btn-gallery-image-upload').removeClass('spinner spinner-light spinner-left').prop('disabled', false);
                    $('#btn-gallery-image-upload').find('span').show();
                    toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                }
            });
        }
    });

    BindProjectAttributeSetting();

    $('#FormVariationAttribute').submit(function () {
        var id = $('#AttributeID').val();
        var name = $('#AttributeID :selected').text().trim();
        var elem = $(this);

        $('#btnAddNewVariat').addClass('spinner spinner-left spinner-light').prop('disabled', true);
        //AJAX CALL To SERVER

        var prod_attr = productAttributes.filter(function (obj) {
            return obj.attributeId == id
        });

        var prod_attr_stg = ProjectAttributeSetting.find(function (obj) {
            return obj.attributeId == id
        });

        $.ajax({
            url: '/Admin/ProjectVariation/Create/',
            type: 'POST',
            data: {
                "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
                productVariation: {
                    ProjectID: GetURLParameter(),
                    SKU: $('input[name=NewVariantionSKU]').val(),
                },
                variationAttributes: {
                    ProjectAttributes: Array.from($('.variation-attributes-form select option:checked')).map(el => el.value)
                }
            },
            success: function (response) {
                if (response.success) {
                    $(response.productVariation.attributes).each(function (ind, item) {
                        productVariationAttributes.push({
                            productVariationID: response.data,
                            productAttributeID: item
                        });
                    });
                    BindProjectVariation(response.data, null, response.productVariation, $('input[name=NewVariantionSKU]').val());
                    $('#VariationAttributeModal').modal('hide');
                    toastr.success(response.message);
                }
                else {
                    toastr.error(response.message);
                }
                $('#btnAddNewVariat').removeClass('spinner spinner-left spinner-light').prop('disabled', false);
            },
            error: function (e) {
                $('#btnAddNewVariat').removeClass('spinner spinner-left spinner-light').prop('disabled', false);
                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
            },
            failure: function (e) {
                $('#btnAddNewVariat').removeClass('spinner spinner-left spinner-light').prop('disabled', false);
                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
            }
        });
        return false;
    });

    $('.btn-add-variation').click(function () {

        $('#ProjectVariationID').val('');
        $('#NewVariantionSKU').val('');
        $('#btnUpdateVaraitionAttribute').hide();
        $('#btnAddNewVariat').show();
        $('.new-variantion-sku').show();
        $('.variation-attributes-form select').val('');

        $('#VariationAttributeModal').modal({}, 'show');
    });

    $('#AttributeForm').submit(function () {
        $("#btnAddNewAttribute").addClass('spinner spinner-dark').prop('disabled', true);

        $.ajax({
            url: '/Admin/Attribute/Create/',
            type: 'POST',
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    $('#AttributeID').append($("<option />").val(response.data.id).text(response.data.name));
                    toastr.success(response.message);
                    $("#AttributeForm").trigger("reset");

                    jQuery('#AttributeModal').modal('hide');
                    $('#AttributeID').val(response.data.id);
                    $('.btn-add-attribute').trigger('click');
                }
                else {
                    toastr.error(response.message);
                }
                $("#btnAddNewAttribute").removeClass('spinner spinner-dark spinner-right').prop('disabled', false);
                $("#btnAddNewAttribute").find('i').show();
            }
        });
        return false;
    });

    //$('#FormVariationAttribute').submit(function () {
    //	$("#btnAddNewVariat").addClass('spinner spinner-dark').prop('disabled', true);
    //	var temp = [];
    //	$('.form-control-variation-attribute').each(function (k, v) {
    //		temp.push($(v).val());
    //	});
    //	$.ajax({
    //		url: '/Admin/Attribute/Create/',
    //		type: 'POST',
    //		data: {
    //			"__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
    //			productAttributeSetting: {
    //				ProjectID: GetURLParameter(),
    //				ProjectVariantID: $(this).find('input[name=ProjectVariantID]').val(),
    //				Attributes: temp,
    //			}
    //		},
    //		success: function (response) {
    //			if (response.success) {
    //				$('#AttributeID').append($("<option />").val(response.data.id).text(response.data.name));
    //				toastr.success(response.message);
    //				$("#AttributeForm").trigger("reset");
    //				jQuery('#AttributeModal').modal('hide');
    //				$('#AttributeID').val(response.data.id);
    //				$('.btn-add-attribute').trigger('click');
    //			}
    //			else {
    //				toastr.error(response.message);
    //			}
    //			$("#btnAddNewAttribute").removeClass('spinner spinner-dark spinner-right').prop('disabled', false);
    //			$("#btnAddNewAttribute").find('i').show();
    //		}
    //	});
    //	return false;
    //});
});

function UpdateProjectAttributeSetting(elem) {
    var record = $(elem).closest('.attribute-setting').attr('id');
    $(elem).addClass('spinner spinner-dark spinner-right').prop('disabled', true);
    $(elem).find('i').hide();
    $.ajax({
        url: '/Admin/ProjectAttributeSetting/Update/',
        type: 'POST',
        data: {
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
            productAttributeSetting: {
                ID: record,
                ProjectID: GetURLParameter(),
                AttributeID: $(elem).closest('.attribute').attr('id'),
                ProjectPageVisiblity: $('#' + record + '.attribute-setting').find('input[name=ProjectPageVisiblity]').prop('checked'),
                VariationUsage: $('#' + record + '.attribute-setting').find('input[name=VariationUsage]').prop('checked'),
            }
        },
        success: function (response) {
            if (response.success) {
                var setting = ProjectAttributeSetting.find(function (obj) {
                    return obj.attributeId == $(elem).closest('.attribute').attr('id');
                })

                setting.ProjectPageVisiblity = $('#' + record + '.attribute-setting').find('input[name=ProjectPageVisiblity]').prop('checked');
                setting.VariationUsage = $('#' + record + '.attribute-setting').find('input[name=VariationUsage]').prop('checked');
                if (setting.VariationUsage) {
                    //BindVariationAttribute(setting);
                    BindVariationAttributes();
                } else {
                    $('.variation-attribute[id=' + setting.attributeId + ']').remove();
                    if ($('.variation-attributes-form select').length == 0) {
                        $('#btnAddNewVariat').prop('disabled', true);
                    } else {
                        $('#btnAddNewVariat').prop('disabled', false);
                    }
                }
            }
            else {
                return false;
            }
            $(elem).removeClass('spinner spinner-dark spinner-right').prop('disabled', false);
            $(elem).find('i').show();
        }
    });
}

function BindProjectImages() {
    $.ajax({
        url: '/Admin/ProjectImage/GetProjectImages/' + GetURLParameter(),
        type: 'GET',
        success: function (response) {
            if (response.success) {
                $('.product-gallery-images').html('');
                $(response.projectImages).each(function (k, v) {
                    $('.product-gallery-images').append('<div class="symbol symbol-70 flex-shrink-0 mr-5 mb-3">' +
                        '<span class="btn btn-xs btn-icon btn-circle btn-danger btn-hover-text-primary btn-shadow btn-remove-gallery-image" data-action="cancel" data-toggle="tooltip" title="remove" onclick="DeleteGalleryImage(this,' + v.id + ')">' +
                        '<i class="icon-xs ki ki-bold-close ki-bold-trash"></i>' +
                        '</span>' +
                        '<div class="symbol-label" style="background-image: url(\'' + v.Image + '\')"></div>' +
                        '</div>');
                });
            } else {
            }
        }
    });
}

function DeleteGalleryImage(elem, record) {
    $(elem).addClass('spinner spinner-light spinner-right').prop('disabled', true);
    $(elem).find('i').hide();
    $.ajax({
        url: '/Admin/ProjectImage/Delete/' + record,
        type: 'POST',
        data: {
            "__RequestVerificationToken":
                $("input[name=__RequestVerificationToken]").val()
        },
        success: function (response) {
            if (response.success) {
                $(elem).closest('.symbol').remove();
                toastr.success(response.message);
            }
            else {
                return false;
                $(elem).removeClass('spinner spinner-light spinner-right').prop('disabled', false);
                $(elem).find('i').show();
                toastr.error(response.message);

            }
        },
        error: function (e) {
            $(elem).removeClass('spinner spinner-light spinner-right').prop('disabled', false);
            $(elem).find('i').show();
            toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
        },
        failure: function (e) {
            $(elem).removeClass('spinner spinner-light spinner-right').prop('disabled', false);
            $(elem).find('i').show();
            toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
        }
    });
}

function DeleteAttribute(elem) {
    $(elem).find('i').hide();
    $(elem).addClass('spinner spinner-center spinner-darker-danger').prop('disabled', true);
    //AJAX CALL To SERVER

    var Attribute = $(elem).closest('.attribute');
    var id = $(Attribute).attr('id');
    var name = $(Attribute).find('.name').text().trim();
    var settingId = $(Attribute).find('.attribute-setting').attr('id')
    $.ajax({
        url: '/Admin/ProjectAttribute/DeleteAll/',
        type: 'POST',
        data: {
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
            productAttribute: {
                ProjectID: GetURLParameter(),
                AttributeID: id,
            }
        },
        success: function (response) {
            if (response.success) {
                productAttributes = productAttributes.filter(function (obj) {
                    return obj.attributeId != id
                });

                $(Attribute).slideUp().remove();


                $('#AttributeID option[value=""]').after($("<option />").val(id).text(name));
            }
            else {
                return false;
            }
        }
    });

    $.ajax({
        url: '/Admin/ProjectAttributeSetting/Delete/' + settingId,
        type: 'POST',
        data: {
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
        },
        success: function (response) {
            if (response.success) {
                if (ProjectAttributeSetting) {
                    ProjectAttributeSetting = ProjectAttributeSetting.filter(function (obj) {
                        return obj.id != settingId
                    });
                }
            }
            else {
                return false;
            }
        }
    });
}

function BindProjectAttributeSetting() {
    $.ajax({
        url: '/Admin/ProjectAttributeSetting/GetProjectAttributeSetting/' + GetURLParameter(),
        type: 'GET',
        success: function (response) {
            if (response.success) {
                ProjectAttributeSetting = response.productAttributeSetting;
                BindProjectAttributes();
            } else {
            }
        }
    });
}

function BindProjectAttributes() {
    $.ajax({
        url: '/Admin/ProjectAttribute/GetProjectAttributes/' + GetURLParameter(),
        type: 'GET',
        success: function (response) {
            if (response.success) {
                productAttributes = response.productAttributes;

                var distinctAttributes = [...new Set(ProjectAttributeSetting.map(x => x.attributeId))];
                $(distinctAttributes).each(function (k, v) {
                    $('#AttributeID').val(v);
                    $('.btn-add-attribute').click();
                });

                GetProjectVariations();
            } else {
            }
        }
    });
}

function BindProjectVariationAttributes() {
    $.ajax({
        url: '/Admin/ProjectVariationAttribute/GetProjectVariationAttributes/' + GetURLParameter(),
        type: 'GET',
        success: function (response) {
            if (response.success) {
                productVariationAttributes = response.productVariationAttributes;

                $('.product-variation-attributes').each(function (k, v) {

                    let prod_var_attrs = productVariationAttributes.filter(function (obj) {
                        return obj.productVariationID == $(v).attr('product-variation-id');
                    });
                    $(v).html('');
                    $(prod_var_attrs).each(function (ind, item) {
                        let prod_attr = productAttributes.find(function (obj) {
                            return obj.id == item.productAttributeID
                        });

                        if (prod_attr) {
                            let prod_attr_stg = ProjectAttributeSetting.find(function (obj) {
                                return obj.attributeId == prod_attr.attributeId
                            });
                            if (prod_attr_stg.attributeName.toUpperCase() == "COLOR") {
                                $(v).append('<span class="ml-1 badge badge-secondary"  attributr-id="' + prod_attr.attributeId + '" product-attriburte-id="' + prod_attr.id + '" >' + prod_attr_stg.attributeName + ' : <span class="color-circle" style="background:' + prod_attr.value + '"></span></span>');
                            } else {
                                $(v).append('<span class="ml-1 badge badge-secondary" attributr-id="' + prod_attr.attributeId + '" product-attriburte-id="' + prod_attr.id + '" >' + prod_attr_stg.attributeName + ' : ' + prod_attr.value + '</span>');
                            }
                        }
                    });
                });
            } else {
            }
        }
    });
}

function BindProjectCategories() {

    $.ajax({
        url: '/Admin/ProjectCategory/GetProjectCategories/' + GetURLParameter(),
        type: 'GET',
        success: function (response) {
            if (response.success) {
                $('.product-categories').html('');
                $(response.categories).each(function (k, v) {
                    $('.product-categories').append('<div class="product-category product-category-plain mb-1" id="product-category' + v.id + '" parent-id="' + v.ParentId + '">' +
                        '<label class="checkbox">' +
                        '<input type="checkbox" name="chkCategory' + v.id + '" id="chkCategory' + v.id + '" data="' + v.id + '"/>' +
                        '<span></span>' +
                        v.name +
                        '</label>' +
                        (v.hasChilds ? '<div class="sub-category pl-10"></div>' : '') +
                        '</div>');
                });

                $(response.productCategories).each(function (k, v) {
                    $("#chkCategory" + v.categoryId).prop('checked', true).attr('productCategoryId', v.id);
                });

                $('.product-category-plain').each(function (k, v) {
                    if ($(this).attr("parent-id") != "null") {
                        /*$(this).appendTo("#product-category" + $(this).attr("parent-id")).find('.sub-category');*/

                        var html = $(this).get(0);
                        $("#product-category" + $(this).attr("parent-id")).find('.sub-category:first').append(html);
                        /*$(this).remove();*/
                        $(this).removeClass("product-category-plain").addClass("product-category");
                    }
                });

                $('.product-category input[type="checkbox"]').change(function () {
                    var chkcategory = $(this);
                    $(chkcategory).closest('.checkbox').addClass('spinner spinner-dark spinner-right');
                    $(chkcategory).prop('disabled', true);
                    if ($(chkcategory).prop('checked')) {
                        $.ajax({
                            url: '/Admin/ProjectCategory/Create/',
                            type: 'POST',
                            data: {
                                "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
                                productCategory: { ProjectID: GetURLParameter(), ProjectCategoryID: $(this).attr('data') }
                            },
                            success: function (response) {
                                if (response.success) {
                                    $(chkcategory).attr('productCategoryId', response.data);
                                    toastr.success(response.message);
                                } else {
                                    $(chkcategory).prop('checked', false);
                                    toastr.error(response.message);
                                    return false;
                                }
                                $(chkcategory).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
                                $(chkcategory).prop('disabled', false);
                            },
                            error: function (e) {

                                $(chkcategory).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
                                $(chkcategory).prop('disabled', false);
                                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                            },
                            failure: function (e) {

                                $(chkcategory).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
                                $(chkcategory).prop('disabled', false);
                                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                            }

                        });
                    } else {
                        $.ajax({
                            url: '/Admin/ProjectCategory/Delete/' + $(chkcategory).attr('productCategoryId'),
                            type: 'POST',
                            data: {
                                "__RequestVerificationToken":
                                    $("input[name=__RequestVerificationToken]").val()
                            },
                            success: function (response) {
                                if (response.success) {
                                    $(chkcategory).attr('productCategoryId', '');
                                    toastr.success(response.message);
                                } else {
                                    $(chkcategory).prop('checked', true);
                                    toastr.error(response.message);
                                    return false;
                                }
                                $(chkcategory).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
                                $(chkcategory).prop('disabled', false);
                            },
                            error: function (e) {
                                $(chkcategory).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
                                $(chkcategory).prop('disabled', false);
                                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                            },
                            failure: function (e) {
                                $(chkcategory).closest('.checkbox').removeClass('spinner spinner-dark spinner-right');
                                $(chkcategory).prop('disabled', false);
                                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                            }
                        });
                    }
                });

            } else {
                $('.product-categories').html('No Categories!');
            }
        }
    });
}

function SearchCategory(elem) {
    var text = $(elem).val().trim();
    if (text && text != "") {
        $('.product-category .checkbox').hide();
        //$('.product-category .checkbox:contains("' + text + '")').css('background-color', 'red');
        $('.product-category .checkbox:contains("' + text + '")').fadeIn();
    } else {
        $('.product-category .checkbox').show();
    }
}

function GetProjectVariations() {
    $.ajax({
        url: '/Admin/ProjectVariation/GetProjectVariation/' + GetURLParameter(),
        type: 'GET',
        success: function (response) {
            if (response.success) {
                $(response.productVariation).each(function (k, v) {
                    BindProjectVariation(v.ID, v);
                });

                $('.product-variations .spinner').remove();

                BindVariationAttributes();
                BindProjectVariationAttributes();
            } else {

            }
        }
    });
}

function BindVariationAttributes() {
    $('.variation-attributes-form').html('');
    var variationAttributes = ProjectAttributeSetting.filter(function (obj) {
        return obj.VariationUsage == true;
    });

    $(variationAttributes).each(function (k, v) {

        var template = '<div class="col-xl-4 variation-attribute variation-attribute' + v.attributeId + '" id="' + v.attributeId + '">';
        template += '				<label class="form-control form-control-solid form-control-lg name">' + v.attributeName + '</label>';

        template += '			<select class="form-control" id="variation-attribute-value' + v.attributeId + '" required>';
        template += '			</select>';
        template += '	</div>';


        $('.variation-attributes-form').append(template);

        var prod_attr = productAttributes.filter(function (obj) {
            return obj.attributeId == v.attributeId
        });

        // console.log(prod_attr);
        $("#variation-attribute-value" + v.attributeId).append($("<option />").val('').text("Select " + v.attributeName));
        $(prod_attr).each(function (k, v) {
            $("#variation-attribute-value" + v.attributeId).append($("<option />").val(v.id).text(v.value));
        });

        //KTTagifyProjectVariationAttribute.init(v.attributeId);

    });

    if ($('.variation-attributes-form select').length == 0) {
        $('#btnAddNewVariat').prop('disabled', true);
    } else {
        $('#btnAddNewVariat').prop('disabled', false);
    }
}

function BindVariationAttribute(variationAttribute) {

    var template = '<div class="col-xl-4 variation-attribute variation-attribute' + variationAttribute.attributeId + '" id="' + variationAttribute.attributeId + '">';
    template += '				<label class="form-control form-control-solid form-control-lg name">' + variationAttribute.attributeName + '</label>';
    template += '			<select class="form-control" id="variation-attribute-value' + variationAttribute.attributeId + '" required>';
    template += '			</select>';
    template += '	</div>';


    $('.variation-attributes-form').append(template);

    var prod_attr = productAttributes.filter(function (obj) {
        return obj.attributeId == variationAttribute.attributeId
    });

    $("#variation-attribute-value" + variationAttribute.attributeId).append($("<option />").val('').text("Select " + variationAttribute.attributeName));
    $(prod_attr).each(function (k, v) {
        $("#variation-attribute-value" + v.attributeId).append($("<option />").val(v.id).text(v.value));
    });

    if ($('.variation-attributes-form select').length == 0) {
        $('#btnAddNewVariat').prop('disabled', true);
    } else {
        $('#btnAddNewVariat').prop('disabled', false);
    }
}

function BindProjectVariation(id, data, productVariation, sku) {
    var template = '<div class="accordion accordion-light accordion-toggle-plus accordion-light-borderless accordion-svg-toggle" id="accordionVariant' + id + '">';
    template += '	<div class="card">';
    template += '		<div class="card-header" id="headingVariant' + id + '">';
    template += '			<div class="card-title collapsed" data-toggle="collapse" data-target="#collapseVariant' + id + '">';
    template += '				<div class="card-label pl-4 product-variation-attributes" product-variation-id="' + id + '">';

    if (productVariation && productVariation.attributes && productVariation.attributes.length > 0) {
        //$(productVariation.attributes).each(function (pv_k,pv_v) {
        //	let prod_var_attrs = productVariationAttributes.filter(function (obj) {
        //		return obj.productVariationID == pv_v;
        //	});
        $(productVariation.attributes).each(function (ind, item) {
            let prod_attr = productAttributes.find(function (obj) {
                return obj.id == item
            });
            if (prod_attr) {
                let prod_attr_stg = ProjectAttributeSetting.find(function (obj) {
                    return obj.attributeId == prod_attr.attributeId
                });
                if (prod_attr_stg.attributeName.toUpperCase() == "COLOR") {
                    template += '<span class="ml-1 badge badge-secondary"  attributr-id="' + prod_attr.attributeId + '" product-attriburte-id="' + prod_attr.id + '" >' + prod_attr_stg.attributeName + ' : <span class="color-circle" style="background:' + prod_attr.value + '"></span></span>';
                } else {
                    template += '<span class="ml-1 badge badge-secondary" attributr-id="' + prod_attr.attributeId + '" product-attriburte-id="' + prod_attr.id + '" >' + prod_attr_stg.attributeName + ' : ' + prod_attr.value + '</span>';
                }
            }
        });
        //});
    }
    else {
        template += (data && data.SKU ? data.SKU : 'Set New Variation');
    }
    template += '				</div>';
    template += '			</div>';
    template += '		</div>';
    template += '		<div id="collapseVariant' + id + '" class="collapse" data-parent="#accordionVariant' + id + '">';
    template += '			<div class="variat pt-3 variation-section">';
    template += '				<div class="variation-attributes"></div>';
    template += '				<div class="row justify-content-around">';
    template += '					<div class="col-md-4">';
    template += '						<button type="button" class="btn btn-sm btn-dark mb-5 btn-block font-weight-bolder text-uppercase " onclick="EditAttributes(this,' + id + ')">Edit Attributes</button>'
    template += '					</div>';
    template += '					<div class="col-md-4">';
    template += '						<button type="button" class="btn btn-sm btn-danger mb-5 btn-block  font-weight-bolder text-uppercase " onclick="DeleteVariation(this,' + id + ')">Delete</button>'
    template += '					</div>';
    template += '				</div>';
    template += '				<div class="form-group row">';
    template += '					<div class="col-lg-6 text-center">';
    template += '						<div class="image-input image-input-outline" id="kt_image_variat">';
    template += '							<div class="image-input-wrapper" style="background-image: url(' + (data && data.Thumbnail ? data.Thumbnail : '\'../../../../Assets/AppFiles/Images/default.png\'') + ')"></div>';
    template += '							<label class="btn btn-xs btn-icon btn-circle btn-white btn-hover-text-primary btn-shadow" data-action="change" data-toggle="tooltip" title="" data-original-title="Change logo">';
    template += '								<i class="fa fa-pen icon-sm text-muted"></i>';
    template += '								<input type="file" name="V_Image" id="Image" accept=".png, .jpg, .jpeg" onchange="ChangeProjectVariationThumbnail(this,' + id + ')"/>';
    template += '								<input type="hidden" name="V_profile_avatar_remove" />';
    template += '								<input type="hidden" name="V_Thumbnail" value="' + (data && data.Thumbnail ? data.Thumbnail : '') + '"/>';
    template += '							</label>';
    template += '							<span class="btn btn-xs btn-icon btn-circle btn-white btn-hover-text-primary btn-shadow cancelimage" data-action="cancel" data-toggle="tooltip" title="Cancel avatar">';
    template += '								<i class="ki ki-bold-close icon-xs text-muted"></i>';
    template += '							</span>';
    template += '						</div>';
    template += '						<span class="form-text text-muted">Allowed file types: png, jpg, jpeg.</span>';
    template += '					</div>';
    template += '					<div class="col-xl-6">';
    template += '						<!--begin::Input-->';
    template += '						<div class="form-group">';
    template += '							<label>SKU</label>';
    template += '							<input type="text" class="form-control form-control-solid form-control-lg" name="V_variantSKU" placeholder="sku" value="' + (data && data.SKU ? data.SKU : sku) + '" />';
    template += '							<span class="form-text text-muted">Please enter variat sku.</span>';
    template += '						</div>';
    template += '						<!--end::Input-->';
    template += '					</div>';
    template += '				</div>';
    template += '				<div class="row">';
    template += '					<div class="col-md-4">';
    template += '						<div class="form-group">';
    template += '							<span class="switch ml-1">';
    template += '								<label>';
    template += '									<input type="checkbox" name="V_Enabled" value="" ' + (data && data.IsActive == true ? 'checked' : '') + '/>';
    template += '									<span></span>';
    template += '								</label> Enabled';
    template += '							</span>';
    template += '						</div>';
    template += '					</div>';
    template += '					<div class="col-md-4">';
    template += '						<div class="form-group">';
    template += '							<span class="switch ml-1">';
    template += '								<label>';
    template += '									<input type="checkbox" name="V_SoldIndividually" value="" ' + (data && data.SoldIndividually == true ? 'checked' : '') + '/>';
    template += '									<span></span>';
    template += '								</label> Sold Individually';
    template += '							</span>';
    template += '						</div>';
    template += '					</div>';
    template += '					<div class="col-md-4">';
    template += '						<div class="form-group">';
    template += '							<span class="switch ml-1">';
    template += '								<label>';
    template += '									<input type="checkbox" onchange="ToggleVariatStockManagement(this)" name="V_IsManageStock" value="" ' + (data && data.IsManageStock == true ? 'checked' : '') + '/>';
    template += '									<span></span>';
    template += '								</label> Manage Stock';
    template += '							</span>';
    template += '						</div>';
    template += '					</div>';
    template += '				</div>';
    template += '				<div class="row">';
    template += '					<div class="col-xl-6">';
    template += '						<!--begin::Input-->';
    template += '						<div class="form-group">';
    template += '							<label>Regular Price</label>';
    template += '							<input type="number" step=".01" min="0" onkeydown="validatePrice(this)" onchange="CheckVSalePrice(this)" class="form-control form-control-solid form-control-lg regular-price" name="V_RegularPrice" placeholder="Regular Price" value="' + (data && data.RegularPrice ? data.RegularPrice : '') + '" />';
    template += '							<span class="form-text text-muted"></span>';
    template += '						</div>';
    template += '						<!--end::Input-->';
    template += '					</div>';
    template += '					<div class="col-xl-6">';
    template += '						<!--begin::Input-->';
    template += '						<div class="form-group">';
    template += '							<label>Sale Price</label>';
    template += '							<input type="number" step=".01" min="0" onkeydown="validatePrice(this)" onchange="CheckVSalePrice(this)" class="form-control form-control-solid form-control-lg sale-price" name="V_SalePrice" placeholder="Sale Price" value="' + (data && data.SalePrice ? data.SalePrice : '') + '" />';
    template += '							<a href="javascript:;" class="float-right form-text text-muted mt-1 toggle-variat-scheduled-sale" onclick="ToggleVariatScheduledSale(this)">' + (data && data.SalePriceFrom ? 'Schedule' : 'Cancel') + '</a>';
    template += '						</div>';
    template += '						<!--end::Input-->';
    template += '					</div>';
    template += '					<div class="col-12">';
    template += '						<div class="form-group variat-sale-price-dates" style="display:none">';
    template += '							<label>Sale Price Dates</label>';
    template += '							<div class="input-daterange input-group kt_datepicker_range">';
    template += '								<input type="text" class="form-control form-control-solid form-control-lg sdate" name="V_startDate" required="required" placeholder="FROM... MM/DD/YYYY" value="' + (data && data.SalePriceFrom ? data.SalePriceFrom : '') + '"/>';
    template += '								<div class="input-group-append">';
    template += '									<span class="input-group-text"><i class="la la-ellipsis-h"></i></span>';
    template += '								</div>';
    template += '								<input type="text" class="form-control form-control-solid form-control-lg sdate" name="V_endDate" required="required" placeholder="TO... MM/DD/YYYY" value="' + (data && data.SalePriceTo ? data.SalePriceTo : '') + '"/>';
    template += '								<div class="input-group-append">';
    template += '									<span class="input-group-text"><i class="fa fa-calendar"></i></span>';
    template += '								</div>';
    template += '							</div>';
    template += '						</div>';
    template += '					</div>';
    template += '				</div>';
    template += '				<div class="row">';
    template += '					<div class="col-12">';
    template += '						<!--begin::Select-->';
    template += '						<div class="form-group unmanaged-stock" ' + (data && data.IsManageStock == true ? 'style="display:none"' : '') + '>';
    template += '							<label>Stock Status</label>';
    template += '							<select name="V_StockStatus" class="form-control form-control-solid form-control-lg">';
    template += '								<option value="1" selected="selected">In Stock</option>';
    template += '								<option value="2">Out Of Stock</option>';
    template += '							</select>';
    template += '						</div>';
    template += '						<!--end::Select-->';
    template += '						<div class="row managed-stock" ' + (data && data.IsManageStock == false ? 'style="display:none"' : '') + '>';
    template += '							<div class="col-xl-6">';
    template += '								<!--begin::Input-->';
    template += '								<div class="form-group">';
    template += '									<label>Stock Quantity</label>';
    template += '									<input type="number" min="0" class="form-control form-control-solid form-control-lg" name="V_Stock" placeholder="Stock" value="' + (data && data.Stock ? data.Stock : '') + '" />';
    template += '									<span class="form-text text-muted"></span>';
    template += '								</div>';
    template += '								<!--end::Input-->';
    template += '							</div>';
    template += '							<div class="col-xl-6">';
    template += '								<!--begin::Input-->';
    template += '								<div class="form-group">';
    template += '									<label>Low stock threshold</label>';
    template += '									<input type="number" min="0" class="form-control form-control-solid form-control-lg" name="V_StockThreshold" placeholder="Stock Threshold" value="' + (data && data.Threshold ? data.Threshold : '') + '" />';
    template += '									<span class="form-text text-muted"></span>';
    template += '								</div>';
    template += '								<!--end::Input-->';
    template += '							</div>';
    template += '						</div>';
    template += '					</div>';
    template += '				</div>';
    template += '				<!--begin::Input-->';
    template += '				<div class="form-group">';
    template += '					<label>Weight (kg)</label>';
    template += '					<input type="number" step=".01" min="0" class="form-control form-control-solid form-control-lg" name="V_Weight" placeholder="Weight" value="' + (data && data.Weight ? data.Weight : '') + '" />';
    template += '					<span class="form-text text-muted"></span>';
    template += '				</div>';
    template += '				<!--end::Input-->';
    template += '				<!--begin::Input-->';
    template += '				<div class="form-group row">';
    template += '					<div class="col-12">';
    template += '						<label>Dimensions (cm)</label>';
    template += '					</div>';
    template += '					<div class="col-md-4">';
    template += '						<input type="number" step=".01" min="0" class="form-control form-control-solid form-control-lg" name="V_Length" placeholder="Length" value="' + (data && data.Length ? data.Length : '') + '" />';
    template += '						<span class="form-text text-muted"></span>';
    template += '					</div>';
    template += '					<div class="col-md-4">';
    template += '						<input type="number" step=".01" min="0" class="form-control form-control-solid form-control-lg" name="V_Width" placeholder="Width" value="' + (data && data.Width ? data.Width : '') + '" />';
    template += '						<span class="form-text text-muted"></span>';
    template += '					</div>';
    template += '					<div class="col-md-4">';
    template += '						<input type="number" step=".01" min="0" class="form-control form-control-solid form-control-lg" name="V_Height" placeholder="Height" value="' + (data && data.Height ? data.Height : '') + '" />';
    template += '						<span class="form-text text-muted"></span>';
    template += '					</div>';
    template += '				</div>';
    template += '				<!--end::Input-->';
    template += '				<label>Description</label>';
    template += '				<textarea class="form-control form-control-solid form-control-lg" rows="3" name="V_description">' + (data && data.Description ? data.Description : '') + '</textarea>';
    //template += '				<label>Description (ar)</label>';
    //template += '				<textarea class="form-control form-control-solid form-control-lg" rows="3" dir="rtl" name="V_descriptionar">' + (data && data.DescriptionAr ? data.DescriptionAr : '') + '</textarea>';
    template += '				<hr class="m-5"/>';
    template += '				<div class="row">';
    template += '					<div class="col-md-6 col-sm-12">';
    template += '						<h5>Gallery Images</h5>';
    template += '					</div>';
    template += '					<div class="col-md-6 col-sm-12">';
    template += '						<a href="javascript:;" class="btn btn-secondary btn-sm float-right btnUploadProjectVariationGalleryImages" onclick="BrowseProjectVariationGalleryImages(this,' + id + ')">';
    template += '							<span class="svg-icon svg-icon-lg m-0">';
    template += '								<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">';
    template += '									<g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">';
    template += '										<polygon points="0 0 24 0 24 24 0 24"></polygon>';
    template += '										<path d="M6,5 L18,5 C19.6568542,5 21,6.34314575 21,8 L21,17 C21,18.6568542 19.6568542,20 18,20 L6,20 C4.34314575,20 3,18.6568542 3,17 L3,8 C3,6.34314575 4.34314575,5 6,5 Z M5,17 L14,17 L9.5,11 L5,17 Z M16,14 C17.6568542,14 19,12.6568542 19,11 C19,9.34314575 17.6568542,8 16,8 C14.3431458,8 13,9.34314575 13,11 C13,12.6568542 14.3431458,14 16,14 Z" fill="#ffffff"></path>';
    template += '									</g>';
    template += '								</svg>';
    template += '							</span> Upload Gallery Images';
    template += '						</a>';
    template += '					</div>';
    template += '					<div class="col-md-12 mt-5">';
    template += '						<div class="product-variation-gallery-images">';
    template += '							<div class="spinner spinner-dark spinner-center"></div>'
    template += '						</div>';
    template += '						<input type="file" name="ProjectVariationGalleryImages" value="" multiple style="display:none" onchange="UploadProjectVariationGalleryImages(this,' + id + ')"/>';
    template += '					</div>';
    template += '				</div>';
    template += '				<div class="row justify-content-end">';
    template += '					<div class="col-md-4">';
    template += '						<button type="button" class="btn btn-sm btn-secondary btn-block mt-5 font-weight-bolder text-uppercase " onclick="SaveVariation(this)">Save Changes</button>'
    template += '					</div>';
    template += '				</div>';
    template += '			</div>';
    template += '		</div>';
    template += '	</div>';
    template += '</div>';

    $('.product-variations').append(template);

    BindProjectVariationImages(id);

    var defaultDate = new Date();
    // Date Range
    $('.sdate').datepicker({
        todayHighlight: true,
        templates: arrows,
        startDate: defaultDate
    });

    if (data) {
        $('#accordionVariant' + id).find('select[name=StockStatus]').val(data.StockStatus);

        $("#accordionVariant" + id).find('input[name=IsManageStock]').trigger('change');
        $("#accordionVariant" + id).find('.toggle-variat-scheduled-sale').trigger('click');
    }
}

function SaveVariation(form) {
    var record = $(form).closest('.accordion').attr('id').replace('accordionVariant', '');
    $(form).addClass('spinner spinner-light spinner-left').prop('disabled', true);

    var formElement = $(form).closest('.accordion');
    $.ajax({
        url: '/Admin/ProjectVariation/Update/',
        type: 'POST',
        data: {
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
            productVariation: {
                ID: record,
                Thumbnail: $(formElement).find('input[name=V_Thumbnail]').val(),
                SKU: $(formElement).find('input[name=V_variantSKU]').val(),
                ProjectID: GetURLParameter(),
                RegularPrice: $(formElement).find('input[name=V_RegularPrice]').val(),
                SalePrice: $(formElement).find('input[name=V_SalePrice]').val(),
                SalePriceFrom: $(formElement).find('.variat-sale-price-dates').is(":visible") ? new Date($(formElement).find('input[name=V_startDate]').val()).toISOString() : null,
                SalePriceTo: $(formElement).find('.variat-sale-price-dates').is(":visible") ? new Date($(formElement).find('input[name=V_endDate]').val()).toISOString() : null,
                Stock: $(formElement).find('input[name=V_IsManageStock]').prop('checked') ? $(formElement).find('input[name=V_Stock]').val() : null,
                Threshold: $(formElement).find('input[name=V_IsManageStock]').prop('checked') ? $(formElement).find('input[name=V_StockThreshold]').val() : null,
                StockStatus: !$(formElement).find('input[name=V_IsManageStock]').prop('checked') ? $(formElement).find('select[name=V_StockStatus]').val() : null,
                Weight: $(formElement).find('input[name=V_Weight]').val(),
                Length: $(formElement).find('input[name=V_Length]').val(),
                Width: $(formElement).find('input[name=V_Width]').val(),
                Height: $(formElement).find('input[name=V_Height]').val(),
                Description: $(formElement).find('textarea[name=V_description]').val(),
                //DescriptionAr: $(formElement).find('textarea[name=V_descriptionar]').val(),
                DescriptionAr: "-",
                IsManageStock: $(formElement).find('input[name=V_IsManageStock]').prop('checked'),
                SoldIndividually: $(formElement).find('input[name=V_SoldIndividually]').prop('checked'),
                IsActive: $(formElement).find('input[name=V_Enabled]').prop('checked'),
            }
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
            }
            else {
                toastr.error(response.message);
            }
            $(form).removeClass('spinner spinner-light spinner-left').prop('disabled', false);
        },
        error: function (e) {
            $(form).removeClass('spinner spinner-light spinner-left').prop('disabled', false);
            toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
        },
        failure: function (e) {
            $(form).removeClass('spinner spinner-light spinner-left').prop('disabled', false);
            toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
        }
    });
}

function DeleteVariation(elem, id) {
    $(elem).find('i').hide();
    $(elem).addClass('spinner spinner-left spinner-light').prop('disabled', true);
    //AJAX CALL To SERVER

    var Variant = $(elem).closest('.accordion');
    $.ajax({
        url: '/Admin/ProjectVariation/Delete/' + id,
        type: 'POST',
        data: {
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
        },
        success: function (response) {
            if (response.success) {

                toastr.success(response.message);
                $(Variant).slideUp().remove();

                productVariationAttributes = productVariationAttributes.filter(function (obj) {
                    return obj.ID != id
                });

                $.ajax({
                    url: '/Admin/ProjectVariationAttribute/DeleteAll/' + id,
                    type: 'POST',
                    data: {
                        "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
                    },
                    success: function (response) {
                        if (response.success) {
                        }
                        else {
                            return false;
                        }
                    },
                    error: function (e) {
                        toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                    },
                    failure: function (e) {
                        toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
                    }
                });
            }
            else {
                $(elem).addClass('spinner spinner-left spinner-light').prop('disabled', false);
                toastr.error(response.message);
                return false;
            }
        },
        error: function (e) {
            $(elem).addClass('spinner spinner-left spinner-light').prop('disabled', false);
            toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
        },
        failure: function (e) {
            $(elem).addClass('spinner spinner-left spinner-light').prop('disabled', false);
            toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
        }
    });

}

function EditAttributes(elem, id) {

    var Variant = $(elem).closest('.accordion');

    $(Variant).find('.product-variation-attributes span').each(function (k, v) {
        $("#variation-attribute-value" + $(v).attr('attributr-id')).val($(v).attr('product-attriburte-id'));
    });

    $('#ProjectVariationID').val(id);
    $('#btnUpdateVaraitionAttribute').show();
    $('#btnAddNewVariat').hide();
    $('.new-variantion-sku').hide();
    $('#VariationAttributeModal').modal({}, 'show');

}

function UpdateVaraitionAttributes(elem) {

    if ($('.variation-attributes-form select').length == $('.variation-attributes-form select option[value!=""]:checked').length) {
        $('#btnUpdateVaraitionAttribute').addClass('spinner spinner-left spinner-darker-primary').prop('disabled', true);

        $.ajax({
            url: '/Admin/ProjectVariationAttribute/Update/',
            type: 'POST',
            data: {
                "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
                productVariationAttributeViewModel: {
                    ProjectId: GetURLParameter(),
                    ProjectVariationId: $('#ProjectVariationID').val(),
                    ProjectAttributes: Array.from($('.variation-attributes-form select option:checked')).map(el => el.value)
                }
            },
            success: function (response) {
                if (response.success) {
                    productVariationAttributes = productVariationAttributes.filter(function (obj) {
                        return obj.productVariationID != response.data
                    });

                    $(response.productVariation.attributes).each(function (ind, item) {
                        productVariationAttributes.push({
                            productVariationID: response.data,
                            productAttributeID: item
                        });
                    });

                    var productVariationAttributesContainer = $('.product-variation-attributes[product-variation-id="' + $('#ProjectVariationID').val() + '"]');
                    productVariationAttributesContainer.html('');
                    if (response.productVariation && response.productVariation.attributes && response.productVariation.attributes.length > 0) {
                        $(response.productVariation.attributes).each(function (ind, item) {
                            let prod_attr = productAttributes.find(function (obj) {
                                return obj.id == item
                            });
                            if (prod_attr) {
                                let prod_attr_stg = ProjectAttributeSetting.find(function (obj) {
                                    return obj.attributeId == prod_attr.attributeId
                                });
                                if (prod_attr_stg.attributeName.toUpperCase() == "COLOR") {
                                    productVariationAttributesContainer.append('<span class="ml-1 badge badge-secondary"  attributr-id="' + prod_attr.attributeId + '" product-attriburte-id="' + prod_attr.id + '" >' + prod_attr_stg.attributeName + ' : <span class="color-circle" style="background:' + prod_attr.value + '"></span></span>');
                                } else {
                                    productVariationAttributesContainer.append('<span class="ml-1 badge badge-secondary" attributr-id="' + prod_attr.attributeId + '" product-attriburte-id="' + prod_attr.id + '" >' + prod_attr_stg.attributeName + ' : ' + prod_attr.value + '</span>');
                                }
                            }
                        });
                    }

                    $('#ProjectVariationID').val('');
                    $('#btnUpdateVaraitionAttribute').hide();
                    $('#btnAddNewVariat').show();
                    $('.new-variantion-sku').show();
                    $('#VariationAttributeModal').modal('hide');
                }
                else {
                    toastr.error(response.message);
                }
                $('#btnUpdateVaraitionAttribute').removeClass('spinner spinner-center spinner-darker-primary').prop('disabled', false);
            },
            error: function (e) {
                $('#btnUpdateVaraitionAttribute').removeClass('spinner spinner-center spinner-darker-primary').prop('disabled', false);
                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
            },
            failure: function (e) {
                $('#btnUpdateVaraitionAttribute').removeClass('spinner spinner-center spinner-darker-primary').prop('disabled', false);
                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
            }
        });
    } else {
        toastr.error("Please, select all attributes");
    }
}

function ChangeProjectVariationThumbnail(elem, productVariationId) {
    var data = new FormData();
    var files = $(elem).get(0).files;
    if (files.length > 0) {
        data.append("__RequestVerificationToken", $("input[name=__RequestVerificationToken]").val());
        data.append("Image", files[0]);

        $(elem).closest('.image-input').find('label[data-action="change"] i').hide();
        $(elem).closest('.image-input').find('label[data-action="change"]').addClass('spinner spinner-dark spinner-center spinner-sm').prop('disabled', true);
        $.ajax({
            url: "/Admin/ProjectVariation/Thumbnail/" + productVariationId,
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                if (response.success) {
                    $(elem).closest('.image-input').find('input[name=V_Thumbnail]').val(response.data);
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
                $(elem).closest('.image-input').find('label[data-action="change"] i').show();
                $(elem).closest('.image-input').find('label[data-action="change"]').removeClass('spinner spinner-dark spinner-center spinner-sm').prop('disabled', false);
            },
            error: function (e) {
                $(elem).closest('.image-input').find('label[data-action="change"] i').show();
                $(elem).closest('.image-input').find('label[data-action="change"]').removeClass('spinner spinner-dark spinner-center spinner-sm').prop('disabled', false);
                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
            },
            failure: function (e) {
                $(elem).closest('.image-input').find('label[data-action="change"] i').show();
                $(elem).closest('.image-input').find('label[data-action="change"]').removeClass('spinner spinner-dark spinner-center spinner-sm').prop('disabled', false);
                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
            }
        });
    }
}

function BrowseProjectVariationGalleryImages(elem, productVariationId) {
    $(elem).closest('.variat').find('input[name=ProjectVariationGalleryImages]').trigger('click');
}

function UploadProjectVariationGalleryImages(elem, productVariationId) {


    var data = new FormData();
    var files = $(elem).get(0).files;
    if (files.length > 0) {
        $('.btnUploadProjectVariationGalleryImages').addClass('spinner spinner-dark spinner-left').prop('disabled', true);
        $('.btnUploadProjectVariationGalleryImages').find('span').hide();
        $.each(files, function (j, file) {
            data.append('Image[' + j + ']', file);
        });
        //data.append("Image", files);
        data.append("count", $(elem).closest('.variat').find('.product-variation-gallery-images .symbol').length);

        $.ajax({
            url: "/Admin/ProjectVariationImages/Create/" + productVariationId,
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                if (response.success) {
                    let productVariationGalleryImages = $(elem).closest('.variat').find('.product-variation-gallery-images');
                    $(response.data).each(function (k, v) {
                        productVariationGalleryImages.append('<div class="symbol symbol-70 flex-shrink-0 mr-5 mb-3">' +
                            '<span class="btn btn-xs btn-icon btn-circle btn-danger btn-hover-text-primary btn-shadow btn-remove-gallery-image" data-action="cancel" data-toggle="tooltip" title="remove" onclick="DeleteProjectVariationGalleryImage(this,' + v.Key + ')">' +
                            '<i class="icon-xs ki ki-bold-close ki-bold-trash"></i>' +
                            '</span>' +
                            '<div class="symbol-label" style="background-image: url(\'' + v.Value + '\')"></div>' +
                            '</div>');
                    });
                    $('.btnUploadProjectVariationGalleryImages').removeClass('spinner spinner-dark spinner-left').prop('disabled', false);
                    $('.btnUploadProjectVariationGalleryImages').find('span').show();
                    toastr.success("Variation gallery images uploaded ...");
                } else {
                    $('.btnUploadProjectVariationGalleryImages').removeClass('spinner spinner-dark spinner-left').prop('disabled', false);
                    $('.btnUploadProjectVariationGalleryImages').find('span').show();
                    toastr.error(response.message);
                }
            },
            error: function (e) {
                $('.btnUploadProjectVariationGalleryImages').removeClass('spinner spinner-dark spinner-left').prop('disabled', false);
                $('.btnUploadProjectVariationGalleryImages').find('span').show();
                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
            },
            failure: function (e) {
                $('.btnUploadProjectVariationGalleryImages').removeClass('spinner spinner-dark spinner-left').prop('disabled', false);
                $('.btnUploadProjectVariationGalleryImages').find('span').show();
                toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
            }
        });
    }
}

function BindProjectVariationImages(productVariationId) {
    $.ajax({
        url: '/Admin/ProjectVariationImages/GetProjectVariationImages/' + productVariationId,
        type: 'GET',
        success: function (response) {
            if (response.success) {
                let productVariationGalleryImages = $('.collapse[data-parent="#accordionVariant' + productVariationId + '"]').find('.product-variation-gallery-images');
                productVariationGalleryImages.html('');

                $(response.productImages).each(function (k, v) {
                    productVariationGalleryImages.append('<div class="symbol symbol-70 flex-shrink-0 mr-5 mb-3">' +
                        '<span class="btn btn-xs btn-icon btn-circle btn-danger btn-hover-text-primary btn-shadow btn-remove-gallery-image" data-action="cancel" data-toggle="tooltip" title="remove" onclick="DeleteProjectVariationGalleryImage(this,' + v.id + ')">' +
                        '<i class="icon-xs ki ki-bold-close ki-bold-trash"></i>' +
                        '</span>' +
                        '<div class="symbol-label" style="background-image: url(\'' + v.Image + '\')"></div>' +
                        '</div>');
                });
            } else {
                productVariationGalleryImages.html('');
            }
        }
    });
}

function DeleteProjectVariationGalleryImage(elem, record) {
    $(elem).addClass('spinner spinner-light spinner-right').prop('disabled', true);
    $(elem).find('i').hide();
    $.ajax({
        url: '/Admin/ProjectVariationImages/Delete/' + record,
        type: 'POST',
        data: {
            "__RequestVerificationToken":
                $("input[name=__RequestVerificationToken]").val()
        },
        success: function (response) {
            if (response.success) {
                $(elem).closest('.symbol').remove();
                toastr.success("Variation gallery image deleted");
            }
            else {
                $(elem).removeClass('spinner spinner-light spinner-right').prop('disabled', false);
                $(elem).find('i').show();
                toastr.error(response.message);
                return false;
            }
        },
        error: function (e) {
            $(elem).removeClass('spinner spinner-light spinner-right').prop('disabled', false);
            $(elem).find('i').show();
            toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
        },
        failure: function (e) {
            $(elem).removeClass('spinner spinner-light spinner-right').prop('disabled', false);
            $(elem).find('i').show();
            toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
        }
    });
}

function ToggleVariatScheduledSale(e) {

    if ($(e).text() == "Schedule") {
        $(e).closest('.variat').find('.variat-sale-price-dates').slideDown();
        $(e).text('Cancel');

    } else {
        $(e).closest('.variat').find('.variat-sale-price-dates').slideUp();
        $(e).text('Schedule');
    }
}

function ToggleVariatStockManagement(e) {

    if ($(e).prop('checked')) {
        $(e).closest('.variat').find('.managed-stock').slideDown();
        $(e).closest('.variat').find('.unmanaged-stock').slideUp();
    } else {

        $(e).closest('.variat').find('.managed-stock').slideUp();
        $(e).closest('.variat').find('.unmanaged-stock').slideDown();
    }
}

function ToggleScheduledSale(e) {

    if ($(e).text().trim() == "Schedule") {
        $('#sale-price-dates').slideDown();
        $(e).text('Cancel');

    } else {
        $('#sale-price-dates').slideUp();
        $(e).text('Schedule');
    }

}

function TogglePublishSchedule(e) {

    if ($(e).text().trim() == "Schedule") {
        $('#publish-schedule').slideDown();
        $(e).text('Cancel');

    } else {
        $('#publish-schedule').slideUp();
        $(e).text('Schedule');
    }

}

function ToggleStockManagement(e) {

    if ($(e).prop('checked')) {
        $(e).closest('.variat').find('.managed-stock').slideDown();
        $(e).closest('.variat').find('.unmanaged-stock').slideUp();
    } else {

        $(e).closest('.variat').find('.managed-stock').slideUp();
        $(e).closest('.variat').find('.unmanaged-stock').slideDown();
    }
}

function SaveProject(e) {
    $(e).addClass('spinner spinner-light spinner-left').prop('disabled', true);
    $(e).find('i').hide();
    var brands = $("#BrandID option:selected").val();

    $.ajax({
        url: '/Admin/Project/Update/',
        type: 'POST',
        data: {
            "__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
            project: {
                ID: GetURLParameter(),
                SKU: $('input[name=SKU]').val(),
                Name: $('input[name=Name]').val(),
                NameAr: $('input[name=NameAr]').val(),

                Bedrooms: $('input[name=Bedrooms]').val(),
                Baths: $('input[name=Baths]').val(),
                AreaStart: $('input[name=AreaStart]').val(),
                AreaEnd: $('input[name=AreaEnd]').val(),

                Purpose: $('select[name=Purpose]').val(),
                ProjectTypeID: $('select[name=ProjectTypeID]').val(),
                PriceStart: $('input[name=PriceStart]').val(),
                PriceEnd: $('input[name=PriceEnd]').val(),
                Slug: $('input[name=Slug]').val(),
                //VendorID: $('input[name=VendorID]').val(),
                //BrandID: brands,
                ShortDescription: EditorShortDescription.getData(),
                //ShortDescriptionAr: EditorShortDescriptionAr.getData(),
                ShortDescriptionAr: "-",

                Description: EditorDescription.getData(),
                //DescriptionAr: EditorDescriptionAr.getData(),
                DescriptionAr: "-",

                MobileDescription: $('textarea[name=MobileDescription]').val(),
                //MobileDescriptionAr: $('textarea[name=MobileDescriptionAr]').val(),
                MobileDescriptionAr: "-",

                Address: $('input[name=Address]').val(),
                Latitude: $('input[name=Latitude]').val(),
                Longitude: $('input[name=Longitude]').val(),
                Country: $('input[name=Country]').val(),
                City: $('select[name=City]').val(),
                Locality: $('input[name=Locality]').val(),
                Developer: $('input[name=Developer]').val(),

                RegularPrice: $('input[name=RegularPrice]').val(),
                SalePrice: $('input[name=SalePrice]').val(),
                SalePriceFrom: $('#sale-price-dates').is(":visible") ? new Date($('input[name=startDate]').val()).toISOString() : null,
                SalePriceTo: $('#sale-price-dates').is(":visible") ? new Date($('input[name=endDate]').val()).toISOString() : null,
                Stock: $('input[name=IsManageStock]').prop('checked') ? $('input[name=Stock]').val() : null,
                Threshold: $('input[name=IsManageStock]').prop('checked') ? $('input[name=StockThreshold]').val() : null,
                StockStatus: !$('input[name=IsManageStock]').prop('checked') ? $('select[name=StockStatus]').val() : null,

                Weight: $('input[name=Weight]').val(),
                Length: $('input[name=Length]').val(),
                Width: $('input[name=Width]').val(),
                Height: $('input[name=Height]').val(),

                PurchaseNote: $('textarea[name=PurchaseNote]').val(),
                EnableReviews: $('input[name=EnableReviews]').prop('checked'),

                Type: $('select[name=ProjectType]').val(),
                IsManageStock: $('input[name=IsManageStock]').prop('checked'),
                IsPublished: $('input[name=IsPublished]').prop('checked'),
                IsRecommended: $('input[name=IsRecommended]').prop('checked'),
                IsTaxInclusive: $('input[name=IsTaxInclusive]').prop('checked'),
                IsFeatured: $('input[name=IsFeatured]').prop('checked'),
                IsSoldIndividually: $('input[name=IsSoldIndividually]').prop('checked'),
                Status: $('select[name=Status]').val(),

                PublishStartDate: $('#publish-schedule').is(":visible") ? new Date($('input[name=PublishStartDate]').val()).toISOString() : null,
                PublishEndDate: $('#publish-schedule').is(":visible") ? new Date($('input[name=PublishEndDate]').val()).toISOString() : null,
            }
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
            }
            else {
                toastr.error(response.message);
            }
            $(e).find('i').show();
            $(e).removeClass('spinner spinner-light spinner-left').prop('disabled', false);
        },
        error: function (e) {
            $(e).find('i').show();
            $(e).removeClass('spinner spinner-light spinner-left').prop('disabled', false);
            toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
        },
        failure: function (e) {
            $(e).find('i').show();
            $(e).removeClass('spinner spinner-light spinner-left').prop('disabled', false);
            toastr.error("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.");
        }
    });
}

function GetURLParameter() {
    var sPageURL = window.location.href;
    var indexOfLastSlash = sPageURL.lastIndexOf("/");

    if (indexOfLastSlash > 0 && sPageURL.length - 1 != indexOfLastSlash)
        return sPageURL.substring(indexOfLastSlash + 1);
    else
        return 0;
}

var KTTagifyProjectVariationAttribute = function (id) {
    var tagify;
    // Private functions
    var demo1 = function (id) {
        var prod_attr = productAttributes.filter(function (obj) {
            return obj.attributeId == id
        });

        var input = document.getElementById("kt_tagify_Variation_Attribute" + id),
            tagify = new Tagify(input, {
                whitelist: prod_attr,
                callbacks: {
                    "invalid": onInvalidTag
                },
                dropdown: {
                    position: 'text',
                    enabled: 1 // show suggestions dropdown after 1 typed character
                }
            }),
            button = input.nextElementSibling;  // "add new tag" action-button

        button.addEventListener("click", onAddButtonClick)

        function onAddButtonClick() {
            tagify.addEmptyTag()
        }

        function onInvalidTag(e) {
            // console.log("invalid", e.detail)
        }

        // init Tagify script on the above inputs
        //tagify = new Tagify(input)


        //tagify.addTags(prod_attr);
        // "remove all tags" button event listener

        // Chainable event listeners
        //tagify.on('add', function (e) {
        //	$.ajax({
        //		url: '/Admin/ProjectVariationAttribute/Create/',
        //		type: 'POST',
        //		data: {
        //			"__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
        //			productAttribute: {
        //				ProjectID: GetURLParameter(),
        //				AttributeID: $(e.detail.tag).closest('.variation-attribute').attr('id'),
        //				Value: e.detail.data.value
        //			}
        //		},
        //		success: function (response) {
        //			if (response.success) {
        //				//$(this).attr('productCategoryId', '');
        //			}
        //			else {
        //				return false;
        //			}
        //		}
        //	});
        //})
        //tagify.on('remove', function (e) {
        //	if (e.detail.data.id) {
        //		$.ajax({
        //			url: '/Admin/ProjectVariationAttribute/Delete/' + e.detail.data.id,
        //			type: 'POST',
        //			data: {
        //				"__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val()
        //			},
        //			success: function (response) {
        //				if (response.success) {
        //					//$(this).attr('productCategoryId', '');
        //				}
        //				else {
        //					return false;
        //				}
        //			}
        //		});
        //	} else {
        //		$.ajax({
        //			url: '/Admin/ProjectVariationAttribute/DeleteValue/',
        //			type: 'POST',
        //			data: {
        //				"__RequestVerificationToken": $("input[name=__RequestVerificationToken]").val(),
        //				productAttribute: {
        //					ProjectID: GetURLParameter(),
        //					AttributeID: $(e.detail.tagify.DOM.input).closest('.attribute').attr('id'),
        //					Value: e.detail.data.value
        //				}
        //			},
        //			success: function (response) {
        //				if (response.success) {
        //					//$(this).attr('productCategoryId', '');
        //				}
        //				else {
        //					return false;
        //				}
        //			}
        //		});
        //	}
        //});
    }

    return {
        // public functions
        init: function (id) {
            demo1(id);
        }
    };
}();

jQuery.expr[':'].contains = function (a, i, m) {
    return jQuery(a).text().toUpperCase()
        .indexOf(m[3].toUpperCase()) >= 0;
};

function validatePrice(event) {
    var $this = $(event);
    $this.val($this.val().replace(/[^\d.]/g, ''));
}

function CheckSalePrice(elem) {

    var regular = $(elem).closest('.product-section').find('.regular-price').val();
    var sale = $(elem).closest('.product-section').find('.sale-price').val();

    if (parseFloat(sale) > parseFloat(regular)) {
        $(elem).closest('.product-section').find('.sale-price').val(regular);
    }
}

function CheckVSalePrice(elem) {

    var regular = $(elem).closest('.variation-section').find('.regular-price').val();
    var sale = $(elem).closest('.variation-section').find('.sale-price').val();

    if (parseFloat(sale) > parseFloat(regular)) {
        $(elem).closest('.variation-section').find('.sale-price').val(regular);
    }


}