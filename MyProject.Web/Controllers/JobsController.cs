using MyProject.Data;
using MyProject.Service;
using MyProject.Service.Helpers;
using MyProject.Web.Helpers;
using MyProject.Web.Helpers.Routing;
using MyProject.Web.ViewModels.JobCandidate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyProject.Web.Controllers{
    public class JobsController : Controller
    {

        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly ICategoryService _categoryService;
        private readonly IJobOpeningService _jobOpeningService;
        private readonly IJobCandidateService _jobCandidateService;
        private readonly INumberRangeService _numberRangeService;
        private readonly IMail _email;

        public JobsController(IJobOpeningService jobOpeningService, ICategoryService categoryService, IJobCandidateService jobCandidateService, INumberRangeService numberRangeService, IMail email)
        {
            this._jobOpeningService = jobOpeningService;
            this._categoryService = categoryService;
            this._jobCandidateService = jobCandidateService;
            this._numberRangeService = numberRangeService;
            this._email = email;
        }

		[Route("departments/{departmentSlug}/Jobs", Name = "departments/{departmentSlug}/Jobs")]
		//[Route("departments/{departmentSlug}/Jobs", Name = "departments/{departmentSlug}/Jobs")]
        public ActionResult Index(string departmentSlug)
        {
            var category = _categoryService.GetCategoryBySlug(departmentSlug);
            if (category == null)
            {
                throw new HttpException(404, "File Not Found");
            }

            return View(category);
        }

        [HttpGet]
		[Route("departments/{departmentId}/JobOpenings", Name = "departments/{departmentId}/JobOpenings")]
		//[Route("departments/{departmentId}/JobOpenings", Name = "departments/{departmentId}/JobOpenings")]
		public ActionResult GetAll(long departmentId, string lang = "en")
        {
            try
            {
                string ImageServer = "";
                var jobs = _jobOpeningService.GetJobOpeningByCategory(departmentId);

                return Json(new
                {
                    success = true,
                    message = "Data recieved successfully!",
                    data = jobs.Select(i => new
                    {
                        i.ID,
                        Title = lang == "en" ? i.Title : i.TitleAr,
                        Requirements = lang == "en" ? i.Requirements : i.RequirementsAr,
                        i.LastDate
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                }, JsonRequestBehavior.AllowGet);
            }
        }

		[Route("Jobs/{JobId}/Apply", Name = "Jobs/{JobId}/Apply")]
		//[Route("Jobs/{JobId}/Apply", Name = "Jobs/{JobId}/Apply")]
		public ActionResult Apply(long jobId)
        {
            var Job = _jobOpeningService.GetJobOpening(jobId);
            if (Job == null)
            {
                throw new HttpException(404, "File Not Found");
            }

            return View(Job);
        }

        [HttpPost]
        public ActionResult Apply(JobCandidateViewModel jobCandidateViewModel, CandidateExperinceRootViewModel candidateExperinceRoot)
        {
            try
            {
                string message = string.Empty;
                if (Request.Files.Count > 0)
                {
                    string path = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Jobs/{0}/", jobCandidateViewModel.JobID);
                    string FilePath = Uploader.UploadDocs(Request.Files, path, relativePath, jobCandidateViewModel.FirstName + "_CV", ref message, "CV");
                    string PhotoPath = Uploader.UploadImage(Request.Files, path, relativePath, jobCandidateViewModel.FirstName + "_Profile", ref message, "Photo");
                    JobCandidate jobCandidate = new JobCandidate()
                    {
                        UCC = _numberRangeService.GetNextValueFromNumberRangeByName("CANDIDATE"),
                        FirstName = jobCandidateViewModel.FirstName,
                        MiddleName = jobCandidateViewModel.MiddleName,
                        LastName = jobCandidateViewModel.LastName,
                        Nationality = jobCandidateViewModel.Nationality,
                        VISAStatus = jobCandidateViewModel.VISAStatus,
                        Gender = jobCandidateViewModel.Gender,
                        Dob = jobCandidateViewModel.Dob,
                        TotalExperience = jobCandidateViewModel.TotalExperience,
                        UAEExperience = jobCandidateViewModel.UAEExperience,
                        INTExperience = jobCandidateViewModel.IndustryExperience,
                        Mobile1 = jobCandidateViewModel.Mobile1,
                        Mobile2 = jobCandidateViewModel.Mobile2,
                        NoticePeriod = jobCandidateViewModel.NoticePeriod,
                        Email = jobCandidateViewModel.Email,
                        Country = jobCandidateViewModel.Country,
                        City = jobCandidateViewModel.City,
                        Address1 = jobCandidateViewModel.AddressLine1,
                        Address2 = jobCandidateViewModel.AddressLine2,
                        Cv = FilePath,
                        MaritalStatus = jobCandidateViewModel.MaritalStatus,
                        Photo = PhotoPath,
                        DrivingLicense = jobCandidateViewModel.DrivingLicense,
                        JobID = jobCandidateViewModel.JobID,
                        IsFlaged = false,
                        MarkAsRead = false
                    };

                    var json_serializer = new JavaScriptSerializer();
                    var ParsedExperiences = Newtonsoft.Json.JsonConvert.DeserializeObject<ViewModels.JobCandidate.CandidateExperinceRootViewModel>(Request.Form["candidateExperinceRoot"].ToString());

                    foreach (var experience in ParsedExperiences.Experiences)
                    {
                        if (experience != null)
                        {
                            CandidateExperience CandidateExperience = new CandidateExperience()
                            {
                                CompanyName = experience.CompanyName,
                                Designation = experience.Designation,
                                StartDate = experience.StartDate,
                                EndDate = experience.EndDate
                            };

                            jobCandidate.CandidateExperiences.Add(CandidateExperience);
                        }
                    }

                    var ParsedEducations = Newtonsoft.Json.JsonConvert.DeserializeObject<ViewModels.JobCandidate.CandidateEducationRootViewModel>(Request.Form["candidateEducationRoot"].ToString());

                    foreach (var education in ParsedEducations.Educations)
                    {
                        if (education != null)
                        {
                            CandidateEducation candidateEducation = new CandidateEducation()
                            {
                                Degree = education.Degree,
                                Institute = education.Institute,
                                PassingYear = education.YearOfPassing
                            };

                            jobCandidate.CandidateEducations.Add(candidateEducation);
                        }
                    }

                    if (_jobCandidateService.CreateJobCandidate(jobCandidate, ref message))
                    {
                        var candidate = string.Format("{0} {1} {2}", jobCandidate.FirstName, jobCandidate.MiddleName, jobCandidate.LastName);
                        _email.SendAcknowledgementMail(jobCandidate.Email, jobCandidate.UCC, candidate, path);

                        return Json(new
                        {
                            success = true,
                            message = "Thank you for your response."
                        }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new
                    {
                        success = false,
                        message = "Oops! Something went wrong. Please try later."
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please Upload Your Resume first!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}