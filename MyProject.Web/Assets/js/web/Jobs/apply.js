var today = formatHtmlDate(new Date());
var jobCandidate = {
	FirstName: 'Faizan',
	MiddleName: null,
	LastName: null,
	Nationality: null,
	VISAStatus: null,
	Gender: null,
	Dob: null,
	TotalExperience: null,
	UAEExperience: null,
	INTExperience: null,
	Mobile1: null,
	Mobile2: null,
	NoticePeriod: null,
	Email: null,
	CountryID: null,
	City: null,
	Address1: null,
	Address2: null,
	Cv: null,
	MaritalStatus: null,
	Photo: null,
	DrivingLicense: null,
	JobID: jobId,
	IsFlaged: null,
	MarkAsRead: null,
	Experiences: [],
	Education: []
}



$(document).ready(function () {

	var date = new Date();

	$('.date').datepicker({
		calendarWeeks: true,
		autoclose: true,
		todayHighlight: true,
		startDate: date
	}).datepicker('update', date).datepicker("option", "minDate", date);

	var inputElements = document.querySelectorAll("input[data-format]");
	inputElements.forEach(input => {
		let m = new IMask(input, {
			mask: input.getAttribute("data-format")
		});
	});

	$('input[name=StartDate]').attr('max', today);
	$('input[name=EndDate]').attr('max', today);

	$('input[name=StartDate]').attr('min', '1980-01-31');
	$('input[name=EndDate]').attr('min', '1980-01-31');

	$('.btn-add-experience').click(function () {

		if (!$('.experience-template').find('input[name=CompanyName]').val()) {
			$('.experience-template').find('input[name=CompanyName]').focus();
			return;
		}
		if (!$('.experience-template').find('input[name=DesignationPosition]').val()) {
			$('.experience-template').find('input[name=DesignationPosition]').focus();
			return;
		}
		if (!$('.experience-template').find('input[name=StartDate]').val()) {
			$('.experience-template').find('input[name=StartDate]').focus();
			return;
		}
		if (!$('.experience-template').find('input[name=CurrentlyWorking]').prop('checked') && !$('.experience-template').find('input[name=EndDate]').val()) {
			$('.experience-template').find('input[name=EndDate]').focus();
			return;
		}

		var CompanyName = $('.experience-template').find('input[name=CompanyName]').val();
		var Designation = $('.experience-template').find('input[name=DesignationPosition]').val();
		var StartDate = $('.experience-template').find('input[name=StartDate]').val();
		var EndDate = $('.experience-template').find('input[name=EndDate]').val();
		var CurrentlyWorking = $('.experience-template').find('input[name=CurrentlyWorking]').prop('checked');

		var SNo = jobCandidate.Experiences.length + 1;

		jobCandidate.Experiences.push({
			SNo: SNo,
			CompanyName: CompanyName,
			Designation: Designation,
			StartDate: StartDate,
			EndDate: EndDate
		})

		$(this).closest('.experience-template').before(`<div class="xl:flex xl:items-center experience" row-no="${SNo}">
            <h1 class ="text-left xl:w-[5%] xl:mt-4">${SNo}.</h1>
            <div class="mt-4 xl:w-[90%]">
                <div class="flex flex-wrap justify-between">
                    <div class="font-Rubik w-[100%] sm:w-[48%] xl:w-[23%]">
                        <div class="flex flex-col mt-4">
                            <label for="CompanyName" class="text-sm mb-1 text-black/40 font-medium">Company Name</label>
                            <input type="text" name="CompanyName" id="CompanyName" placeholder="Type Here" class ="bg-black/10 text-sm h-[2.5rem] rounded-lg px-3" value="${CompanyName}"  onchange="UpdateExperience(this)">
                        </div>
                    </div>
                    <div class="font-Rubik w-[100%] sm:w-[48%] xl:w-[23%]">
                        <div class="flex flex-col mt-4">
                            <label for="DesignationPosition" class="text-sm mb-1 text-black/40 font-medium">Designation / Position</label>
                            <input type="text" name="DesignationPosition" id="DesignationPosition" placeholder="Type Here" class ="bg-black/10 text-sm h-[2.5rem] rounded-lg px-3" value="${Designation}"  onchange="UpdateExperience(this)">
                        </div>
                    </div>
                    <div class="font-Rubik w-[100%] sm:w-[48%] xl:w-[23%]">
                        <div class="flex flex-col mt-4">
                            <label for="StartDate" class="text-sm mb-1 text-black/40 font-medium ">Start Date</label>
                            <input type="date" name="StartDate" id="StartDate" max="${today}" placeholder="YY/MM" class ="bg-black/10 text-sm h-[2.5rem] rounded-lg px-3" onchange="JobDuration(this)" value="${StartDate}">
                        </div>
                    </div>
                    <div class="font-Rubik w-[100%] sm:w-[48%] xl:w-[23%]">
                        <div class="flex flex-col mt-4">
                            <label for="EndDate" class="text-sm mb-1 text-black/40 font-medium">End Date</label>
                            <input type="date" name="EndDate" id="EndDate"  max="${today}" placeholder="YY/MM" class ="bg-black/10 text-sm h-[2.5rem] rounded-lg px-3" onchange="JobDuration(this)" ${CurrentlyWorking ? `disabled` : `value="${EndDate}"`} >
                        </div>
                        <input type="checkbox" id="CurrentlyWorking" name="CurrentlyWorking"  onclick="MarkCurrentlyWorking(this)" ${CurrentlyWorking ? `checked` : ``}>
                        <label for="CurrentlyWorking">Currently Working</label>
                    </div>
                </div>
            </div>
            <div class ="flex flex-row justify-end xl:w-[5%] xl:mt-4 cursor-pointor" >
                <div class ="rounded-full w-8 h-8 flex justify-center items-center border-2 border-[#B10071] mt-2 cursor-pointer" onclick="RemoveExperience(this)">
                    Remove
                </div>
            </div>
        </div>`);

		$('.experience-template').find("h1").html((SNo + 1) + ".");
		$('.experience-template').find('input[name=CompanyName]').val('');
		$('.experience-template').find('input[name=DesignationPosition]').val('');
		$('.experience-template').find('input[name=StartDate]').val('');
		$('.experience-template').find('input[name=EndDate]').val('');

	});

	$('.btn-add-education').click(function () {

		if (!$('input[name=Degree]').val()) {
			$('.education-template').find('input[name=Degree]').focus();
			return;
		}
		if (!$('input[name=Institute]').val()) {
			$('.education-template').find('input[name=Institute]').focus();
			return;
		}
		if (!$('input[name=PassingYear]').val()) {
			$('.education-template').find('input[name=PassingYear]').focus();
			return;
		}

		var Degree = $('.education-template').find('input[name=Degree]').val();
		var Institute = $('.education-template').find('input[name=Institute]').val();
		var PassingYear = $('.education-template').find('input[name=PassingYear]').val();

		var SNo = jobCandidate.Education.length + 1;

		jobCandidate.Education.push({
			SNo: SNo,
			Degree: Degree,
			Institute: Institute,
			YearOfPassing: PassingYear
		})

		$(this).closest('.education-template').before(`<div class="mt-4 xl:flex xl:items-end education" row-no="${SNo}">
                                                            <h1 class="xl:w-[5%] xl:mb-4">${SNo}.</h1>
                                                            <div class="flex flex-wrap justify-between xl:w-[90%]">
                                                                <div class="font-Rubik w-[100%] sm:w-[48%] xl:w-[32%]">
                                                                    <div class="flex flex-col mt-4">
                                                                        <label for="Degree" class="text-sm mb-1 text-black/40 font-medium">Degree / Diploma / Certification</label>
                                                                        <input type="text" name="Degree" id="Degree" placeholder="Type Here" class ="bg-black/10 text-sm h-[2.5rem] rounded-lg px-3" value="${Degree}" onchange="UpdateEducation(this)">
                                                                    </div>
                                                                </div>
                                                                <div class="font-Rubik w-[100%] sm:w-[48%] xl:w-[32%]">
                                                                    <div class="flex flex-col mt-4">
                                                                        <label for="Institute" class="text-sm mb-1 text-black/40 font-medium">University / Board / Institute</label>
                                                                        <input type="text" name="Institute" id="Institute" placeholder="Type Here" class ="bg-black/10 text-sm h-[2.5rem] rounded-lg px-3" value="${Institute}" onchange="UpdateEducation(this)">
                                                                    </div>
                                                                </div>
                                                                <div class="font-Rubik w-[100%] sm:w-[48%] xl:w-[32%]">
                                                                    <div class="flex flex-col mt-4">
                                                                        <label for="PassingYear" class="text-sm mb-1 text-black/40 font-medium">Year Of Passing</label>
                                                                        <input type="number" min="1900" max="2099" step="1"  name="PassingYear" id="PassingYear" placeholder="YYYY" onchange="ValidatePassingYear(this)" class ="bg-black/10 text-sm h-[2.5rem] rounded-lg px-3" value="${PassingYear}">
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class ="flex flex-row justify-end xl:w-[5%] ">
                                                                <div class ="rounded-full w-8 h-8 flex justify-center items-center border-2 border-[#B10071] mt-4 cursor-pointer"  onclick="RemoveEducation(this)">
                                                                    Remove
                                                                </div>
                                                            </div>
                                                        </div>`);

		$('.education-template').find("h1").html((SNo + 1) + ".");
		$('.education-template').find('input[name=Degree]').val('');
		$('.education-template').find('input[name=Institute]').val('');
		$('.education-template').find('input[name=PassingYear]').val('');

	});

	$('#FormCareers').submit(function () {

		//$('#message').html('').hide();
		var files = $("#CV").get(0).files;
		if (files.length <= 0) {
			//$('#message').html("Please Upload Your Resume first.").addClass('error').slideDown();
			ToastrMessage('error', "Please Upload Your Resume first.", 6);
			return false;
		}

		if (jobCandidate.Experiences.length <= 0) {
			//$('#message').html("Please Add Experience.").addClass('error').slideDown();
			ToastrMessage('error', "Please Add Experience.", 6);
			return false;
		}

		if (jobCandidate.Education.length <= 0) {
			//$('#message').html("Please Add Education.").addClass('error').slideDown();
			ToastrMessage('error', "Please Add Education.", 6);
			return false;
		}

		document.getElementById('myModal').style.display = "block";
		return false;
	});

	document.getElementById("Dob").setAttribute("max", '2005-12-31');
	document.getElementById("Dob").setAttribute("min", '1960-01-01');

	//Photo Validation Begin
	$("#Photo").change(function (e) {
		var file, img;
		if ((file = this.files[0])) {
			var ext = $('#Photo').val().split('.').pop().toLowerCase();
			if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
				//$('#message').html("Invalid extension! only 'gif', 'png', 'jpg', 'jpeg' extensions are allowed.").addClass('error').slideDown();
				ToastrMessage('error', "Invalid extension! only 'gif', 'png', 'jpg', 'jpeg' extensions are allowed.", 6);
				return false;
			}
			if (file.size > 200000) {
				//$('#message').html("Image size must be less than 2 MB!").addClass('error').slideDown();
				ToastrMessage('error', "Image size must be less than 2 MB!", 6);
				return false;
			}

			$('#PhotoName').html(`${$('#Photo')[0].files[0].name} (Tab to change)`);
		}
	});
	//Photo Validation End

	//Photo Validation Begin
	$("#CV").change(function (e) {
		var file, img;
		if ((file = this.files[0])) {
			var ext = $('#CV').val().split('.').pop().toLowerCase();
			if ($.inArray(ext, ['pdf', 'doc', 'docx']) == -1) {
				//$('#message').html("Invalid extension! only 'pdf', 'doc', 'docx' extensions are allowed.").addClass('error').slideDown();
				ToastrMessage('error', "Invalid extension! only 'pdf', 'doc', 'docx' extensions are allowed.", 6);
				return false;
			}
			if (file.size > 200000) {
				//$('#message').html("Image size must be less than 2 MB!").addClass('error').slideDown();
				ToastrMessage('error', "Image size must be less than 2 MB!", 6);
				return false;
			}
			$('#FileName').html(`${$('#CV')[0].files[0].name} (Tab to change)`);
		}
	});
	//Photo Validation End

	$("#Mobile1,#Mobile2").keypress(function (e) {
		//if the letter is not digit then display error and don't type anything
		if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
			//display error message
			//$("#errmsg").html("Digits Only").show().fadeOut("slow");
			return false;
		}
	});
});

function MarkCurrentlyWorking(elem) {
	if ($(elem).prop('checked')) {
		$(elem).closest('.experience').find("input[name=EndDate]").val('').prop('disabled', true);
		$(elem).closest('.experience-template').find("input[name=EndDate]").val('').prop('disabled', true);
	} else {
		$(elem).closest('.experience').find("input[name=EndDate]").val('').prop('disabled', false);
		$(elem).closest('.experience-template').find("input[name=EndDate]").val('').prop('disabled', false);
	}
}

function JobDuration(elem) {

	if ($(elem).closest('.experience').length > 0) {
		var StartDate = new Date($(elem).closest('.experience').find("input[name=StartDate]").val());
		var EndDate = new Date($(elem).closest('.experience').find("input[name=EndDate]").val());

		if (StartDate > EndDate) {
			$(elem).closest('.experience').find("input[name=StartDate]").val(formatHtmlDate(StartDate));
			$(elem).closest('.experience').find("input[name=EndDate]").val(formatHtmlDate(StartDate))
		}
	} else {

		var StartDate = new Date($(elem).closest('.experience-template').find("input[name=StartDate]").val());
		var EndDate = new Date($(elem).closest('.experience-template').find("input[name=EndDate]").val());

		if (StartDate > EndDate) {
			$(elem).closest('.experience-template').find("input[name=StartDate]").val(formatHtmlDate(StartDate));
			$(elem).closest('.experience-template').find("input[name=EndDate]").val(formatHtmlDate(StartDate))
		}
	}

	UpdateExperience(elem);
}

function ValidatePassingYear(elem) {

	var PassingYear
	if ($(elem).val() < 1980) {
		$(elem).val(1980)
	} else if ($(elem).val() > 2021) {
		$(elem).val(2021)
	}

	UpdateEducation(elem);
}

function UpdateExperience(elem) {
	var SNo = $(elem).closest('.experience').attr("row-no");
	var Experience = jobCandidate.Experiences.find(function (obj) {
		return obj.SNo == SNo
	});

	if (Experience) {
		Experience.CompanyName = $('.experience').find('input[name=CompanyName]').val();
		Experience.Designation = $('.experience').find('input[name=DesignationPosition]').val();
		Experience.StartDate = $('.experience').find('input[name=StartDate]').val();
		Experience.EndDate = $('.experience').find('input[name=EndDate]').val();
	}
}

function RemoveExperience(elem) {

	var SNo = $(elem).closest('.experience').attr("row-no");
	jobCandidate.Experiences = jobCandidate.Experiences.filter(function (obj) {
		return obj.SNo != SNo
	});

	$(elem).closest('.experience').remove();

	jobCandidate.Experiences.forEach(function (k, v) {
		var Experience = jobCandidate.Experiences.find(function (obj) {
			return obj.SNo == k.SNo
		});

		$(`div[row-no="${k.SNo}"]`).find("h1").html((v + 1) + ".")
		$(`div[row-no="${k.SNo}"]`).attr("row-no", v + 1)
		Experience.SNo = v + 1;
	});
	SNo = jobCandidate.Experiences.length;
	$('.experience-template').find("h1").html((SNo + 1) + ".");
}

function UpdateEducation(elem) {
	var SNo = $(elem).closest('.education').attr("row-no");
	var Education = jobCandidate.Education.find(function (obj) {
		return obj.SNo == SNo
	});

	if (Education) {
		Education.Degree = $('.education').find('input[name=Degree]').val();
		Education.Institute = $('.education').find('input[name=Institute]').val();
		Education.YearOfPassing = $('.education').find('input[name=PassingYear]').val();
	}
}

function RemoveEducation(elem) {
	var SNo = $(elem).closest('.education').attr("row-no");
	jobCandidate.Education = jobCandidate.Education.filter(function (obj) {
		return obj.SNo != SNo
	});
	$(elem).closest('.education').remove();
	jobCandidate.Education.forEach(function (k, v) {
		var Education = jobCandidate.Education.find(function (obj) {
			return obj.SNo == k.SNo
		});

		$(`div[row-no="${k.SNo}"]`).find("h1").html((v + 1) + ".")
		$(`div[row-no="${k.SNo}"]`).attr("row-no", v + 1)
		Education.SNo = v + 1;
	});
	SNo = jobCandidate.Education.length;
	$('.education-template').find("h1").html((SNo + 1) + ".");
}

function Apply(elem) {

	var files = $("#CV").get(0).files;
	if (files.length <= 0) {
		//$('#message').html("Please Upload Your Resume first.").addClass('error').slideDown();
		ToastrMessage('error', "Please Upload Your Resume first.", 6);
		return false;
	}

	if (jobCandidate.Experiences.length <= 0) {
		//$('#message').html("Please Add Experience.").addClass('error').slideDown();
		ToastrMessage('error', "Please Add Experience.", 6);
		return false;
	}

	if (jobCandidate.Education.length <= 0) {
		//$('#message').html("Please Add Education.").addClass('error').slideDown();
		ToastrMessage('error', "Please Add Education.", 6);
		return false;
	}

	jobCandidate.FirstName = $('input[name=FirstName]').val();
	jobCandidate.MiddleName = $('input[name=MiddleName]').val();
	jobCandidate.LastName = $('input[name=LastName]').val();
	jobCandidate.Nationality = $('select[name=Nationality]').val();
	jobCandidate.VISAStatus = $('select[name=VISAStatus]').val();
	jobCandidate.Gender = $('select[name=Gender]').val();
	jobCandidate.Dob = $('input[name=Dob]').val();
	jobCandidate.TotalExperience = $('input[name=TotalExperience]').val();
	jobCandidate.UAEExperience = $('input[name=UAEExperience]').val();
	jobCandidate.IndustryExperience = $('input[name=IndustryExperience]').val();
	jobCandidate.Mobile1 = $('input[name=Mobile1]').val();
	jobCandidate.Mobile2 = $('input[name=Mobile2]').val();
	jobCandidate.NoticePeriod = $('select[name=NoticePeriod]').val();
	jobCandidate.Email = $('input[name=Email]').val();
	jobCandidate.Country = $('select[name=Country]').val();
	jobCandidate.City = $('input[name=City]').val();
	jobCandidate.AddressLine1 = $('textarea[name=AddressLine1]').val();
	jobCandidate.AddressLine2 = $('textarea[name=AddressLine2]').val();
	jobCandidate.MaritalStatus = $('select[name=MaritalStatus]').val();
	jobCandidate.DrivingLicense = $('select[name=DrivingLicense]').val();

	var data = getFormData(jobCandidate);

	data.append("candidateExperinceRoot", JSON.stringify({ Experiences: jobCandidate.Experiences }));
	data.append("candidateEducationRoot", JSON.stringify({ Educations: jobCandidate.Education }));

	data.append("CV", files[0]);
	var Photo = $("#Photo").get(0).files;
	if (Photo.length > 0) {
		data.append("Photo", Photo[0]);
	}

	try {

		//$('#message').html('').hide();
		ToastrMessage();
		$('#btnApply').prop(`disabled`, true);

		$.ajax({
			url: `/Jobs/Apply`,
			type: `POST`,
			processData: false,
			contentType: false,
			data: data,
			success: function (response) {
				if (response.success) {
					//$('#message').html(response.message).addClass('success').slideDown();
					ToastrMessage('success', response.message, 6);
					setTimeout(function () {
						window.location.reload();
					}, 500)
				} else {
					//$('#message').html(response.message).addClass('error').slideDown();
					ToastrMessage('error', response.message, 6);
				}

				$('#btnApply').prop(`disabled`, false);
				document.getElementById('myModal').style.display = "none";
			},
			error: function (e) {
				//$('#message').html("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.").addClass('error').slideDown();
				ToastrMessage('error', "Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.", 6);
				$('#btnApply').prop(`disabled`, false);
				document.getElementById('myModal').style.display = "none";
			},
			failure: function (e) {
				//$('#message').html("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.").addClass('error').slideDown();
				ToastrMessage('error', "Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.", 6);
				$('#btnApply').prop(`disabled`, false);
				document.getElementById('myModal').style.display = "none";
			}
		});
	} catch (e) {
		//$('#message').html("Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.").addClass('error').slideDown();
				ToastrMessage('error', "Ooops, something went wrong.Try to refresh this page or contact Administrator if the problem persists.", 6);
		$('#btnApply').prop(`disabled`, false);
		document.getElementById('myModal').style.display = "none";
	}
}

function getFormData(object) {
	const formData = new FormData();
	Object.keys(object).forEach(key => formData.append(key, object[key]));
	return formData;
}

function ActiveFunction() {
	const file = document.getElementById("CV");
	file.click();
}

function myFunction(e) {
	console.log(e.children[0])
	e.children[0].click()
}

function onPopup() {
	document.getElementById('myModal').style.display = "block";
}

function closePopup() {
	document.getElementById('myModal').style.display = "none";
}

function formatHtmlDate(currentDate) {
	var dd = currentDate.getDate();
	var mm = currentDate.getMonth() + 1; //January is 0!
	var yyyy = currentDate.getFullYear();
	if (dd < 10) {
		dd = '0' + dd
	}
	if (mm < 10) {
		mm = '0' + mm
	}

	return yyyy + '-' + mm + '-' + dd;
}

$(document).ready(function () {
	$('#message').click(function () { $(this).html('').hide() });
	if (window.location.hash == '#careers') {
		$('#btnCareer').trigger('click');
	}
});