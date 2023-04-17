using Mentor15.Helpers.Datatable;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider; 
using Project.Web.Helpers;
using Project.Web.Helpers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class JobCandidatesController : Controller
    {
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IJobCandidateService _careerService;

        public JobCandidatesController(IJobCandidateService careerService)
        {
            this._careerService = careerService;

        }
        // GET: Admin/Candidates
        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
            ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30).ToString("MM/dd/yyyy");
            return View();
        }
        public ActionResult List()
        {
            var candidates = _careerService.GetJobCandidates();
            return PartialView(candidates);
        }

        //Server Side Candidates
        [HttpPost]
        public JsonResult List(DataTableAjaxPostModel model)
        {
            var fromDate = new Nullable<DateTime>();
            var toDate = new Nullable<DateTime>();

            #region Date filter work
            //Date filter work
            try
            {
                var min = Request.Form["min"];
                var max = Request.Form["max"];

                if (min.Length > 0)
                {
                    fromDate = Convert.ToDateTime(min);
                    fromDate = fromDate.Value.AddDays(-1);
                }

                if (max.Length > 0)
                {
                    toDate = Convert.ToDateTime(max);
                    //toDate = toDate.Value.AddDays(1);
                }
            }
            catch (Exception)
            {
                fromDate = new Nullable<DateTime>();
                toDate = new Nullable<DateTime>();
            }
            #endregion

            string ImageServer = CustomURL.GetImageServer();

            var searchBy = (model.search != null) ? model.search.value : "";
            int sortBy = 0;
            string sortDir = "";

            if (model.order != null)
            {
                sortBy = model.order[0].column;
                sortDir = model.order[0].dir.ToLower();
            }

            if (toDate.HasValue)
            { toDate = toDate.Value.AddMinutes(1439); }
            var candidadtes = _careerService.GetSPCandidates(model.length, model.start, sortBy, sortDir, searchBy, ImageServer, fromDate, toDate);

            int filteredResultsCount = candidadtes != null && candidadtes.Count() > 0 ? (int)candidadtes.FirstOrDefault().FilteredResultsCount : 0;
            int totalResultsCount = candidadtes != null && candidadtes.Count() > 0 ? (int)candidadtes.FirstOrDefault().TotalResultsCount : 0;

            return Json(new
            {
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = candidadtes
            });
        }

        //[HttpPost]
        //public ActionResult List(DateTime fromDate, DateTime ToDate)
        //{
        //    DateTime EndDate = ToDate.AddMinutes(1439);
        //    var candidates = _careerService.GetJobCandidatesDateWise(fromDate, EndDate);
        //    return PartialView(candidates);
        //}

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCandidate candidate = _careerService.GetJobCandidate((long)id);
            if (candidate == null)
            {
                return HttpNotFound();
            }
            return View(candidate);
        }

        public ActionResult ScheduleStatus(long id)
        {
            JobCandidate candidate = _careerService.GetJobCandidate((long)id);
            return View(candidate);
        }

        [HttpPost]
        public ActionResult ScheduleStatus(JobCandidate candidate)
        {
            string message = string.Empty;
            bool sendMail = false;
            var path = Server.MapPath("~/");

            JobCandidate candidateCurrent = _careerService.GetJobCandidate(candidate.ID);

            candidateCurrent.Status = candidate.Status;
            if (candidateCurrent.Status == "Approved" || candidateCurrent.Status == "Rejected")
            {
                candidateCurrent.Schedule = candidate.Schedule;
                sendMail = true;
            }

            if (_careerService.UpdateJobCandidate(candidateCurrent, ref message, path, sendMail))
            {
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {candidateCurrent.Status} schedule status.");
                return Json(new
                {
                    success = true,
                    message = "Status updated successfully ...",
                    data = new
                    {
                        ID = candidateCurrent.ID,
                        Date = candidateCurrent.CreatedOn.HasValue ? candidateCurrent.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt") : "-",
                        UCC = candidateCurrent.UCC,
                        Candidate = candidateCurrent.Photo + "," + candidateCurrent.FirstName + "," + candidateCurrent.MiddleName + "," + candidateCurrent.LastName + "," + candidateCurrent.Email,
                        Job = candidateCurrent.JobOpening.Category.CategoryName + "," + candidateCurrent.JobOpening.Title,
                        Status = candidateCurrent.Status,
                        Schedule = candidateCurrent.Schedule.HasValue ? candidateCurrent.Schedule.Value.ToString("dd MMM yyyy, h: mm tt") : "-",
                        Details = candidateCurrent.Cv + "," + candidate.MarkAsRead + "," + candidate.IsFlaged + "," + candidate.ID,
                    }
                });
            }
            return Json(new
            {
                success = false,
                message = "Please fill the form properly ...",
            });
        }

        #region Reports

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplicantsReport(DateTime FromDate, DateTime ToDate)
        {
            DateTime EndDate = ToDate.AddMinutes(1439);
            var getAllCandidates = _careerService.GetJobCandidatesDateWise(FromDate, EndDate);
            if (getAllCandidates.Count() > 0)
            {
                string ImageServer = CustomURL.GetImageServer();

                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("Applicants");

                    #region Top Header

                    var headerRow = new List<string[]>();

                    // Determine the header range (e.g. A1:D1)
                    string headerRange = "A1:AF1";

                    // Target a worksheet
                    var worksheet = excel.Workbook.Worksheets["Applicants"];

                    // Popular header row data
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    worksheet.Cells[1, 1].Value = "Applicant Details";// Heading Name
                    worksheet.Cells[1, 1, 1, 24].Merge = true; //Merge columns start and end range
                    worksheet.Cells[1, 1, 1, 24].Style.Font.Bold = true; //Font should be bold
                    worksheet.Cells[1, 1, 1, 24].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Aligmnet is center

                    worksheet.Cells[1, 25].Value = "Experience Details";// Heading Name
                    worksheet.Cells[1, 25, 1, 29].Merge = true; //Merge columns start and end range
                    worksheet.Cells[1, 25, 1, 29].Style.Font.Bold = true; //Font should be bold
                    worksheet.Cells[1, 25, 1, 29].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Aligmnet is center

                    worksheet.Cells[1, 30].Value = "Education Details";// Heading Name
                    worksheet.Cells[1, 30, 1, 32].Merge = true; //Merge columns start and end range
                    worksheet.Cells[1, 30, 1, 32].Style.Font.Bold = true; //Font should be bold
                    worksheet.Cells[1, 30, 1, 32].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Aligmnet is center

                    worksheet.Cells[1, 1, 1, 32].Style.Font.Size = 14; //Font size of Top Header

                    #endregion

                    #region Header

                    headerRow = new List<string[]>()
                    {
                        new string[] {
                        "Date"
                        ,"UCC"
                        ,"Name"
                        ,"Email"
                        ,"Mobile No. 1"
                        ,"Mobile No. 2"
                        ,"Gender"
                        ,"Date of Birth"
                        ,"Marital Status"
                        ,"Nationality"
                        ,"Notice Period"
                        ,"GCC Driving License"
                        ,"Total Experience"
                        ,"UAE Experience"
                        ,"Real Estate Industry Experience"
                        ,"Visa Status"
                        ,"Country"
                        ,"City"
                        ,"Address Line 1"
                        ,"Address Line 2"
                        ,"Department"
                        ,"Position"
                        ,"Status"
                        ,"CV"

                        ,"Company Name"
                        ,"Designation"
                        ,"Start Date"
                        ,"End Date"
                        ,"Currently Working Here"

                        ,"Degree"
                        ,"Institute"
                        ,"Year of Passing"

                        }
                    };

                    // Determine the header range (e.g. A1:D1)
                    //headerRange = "A2:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "2";////Gives error.
                    headerRange = "A2:AF2";

                    // Target a worksheet
                    worksheet = excel.Workbook.Worksheets["Applicants"];

                    // Cell styling
                    worksheet.Cells[2, 1, 2, 35].Style.Font.Bold = true; //Font should be bold
                    worksheet.Cells[1, 1, 1, 35].Style.Font.Size = 12; //Font size of Header

                    // Popular header row data
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                    #endregion

                    #region Values

                    var cellData = new List<object[]>();

                    foreach (var i in getAllCandidates)
                    {
                        IList<CandidateExperience> experiences = i.CandidateExperiences.ToList();
                        var exp = experiences.FirstOrDefault();

                        IList<CandidateEducation> educationdetails = i.CandidateEducations.ToList();
                        var edu = educationdetails.FirstOrDefault();

                        cellData.Add(new object[] {
							
							//Applicant Details Work
							i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
                            ,!string.IsNullOrEmpty(i.UCC) ? i.UCC : "-"
                            ,i.FirstName + " " + i.MiddleName + " " +i.LastName
                            ,!string.IsNullOrEmpty(i.Email) ? i.Email : "-"
                            ,!string.IsNullOrEmpty(i.Mobile1) ? i.Mobile1 : "-"
                            ,!string.IsNullOrEmpty(i.Mobile2) ? i.Mobile2 : "-"
                            ,!string.IsNullOrEmpty(i.Gender) ? i.Gender : "-"
                            ,i.Dob.HasValue ? i.Dob.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
                            ,!string.IsNullOrEmpty(i.MaritalStatus) ? i.MaritalStatus : "-"
                            ,!string.IsNullOrEmpty(i.Nationality) ? i.Nationality : "-"
                            ,!string.IsNullOrEmpty(i.NoticePeriod) ? i.NoticePeriod : "-"
                            ,!string.IsNullOrEmpty(i.DrivingLicense) ? i.DrivingLicense : "-"
                            ,!string.IsNullOrEmpty(i.TotalExperience) ? i.TotalExperience : "-"
                            ,!string.IsNullOrEmpty(i.UAEExperience) ? i.UAEExperience : "-"
                            ,!string.IsNullOrEmpty(i.INTExperience) ? i.INTExperience : "-"
                            ,!string.IsNullOrEmpty(i.VISAStatus) ? i.VISAStatus : "-"
                            ,!string.IsNullOrEmpty(i.Country) ? i.Country : "-"
                            ,!string.IsNullOrEmpty(i.City) ? i.City : "-"
                            ,!string.IsNullOrEmpty(i.Address1) ? i.Address1 : "-"
                            ,!string.IsNullOrEmpty(i.Address2) ? i.Address2 : "-"
                             ,!string.IsNullOrEmpty(i.JobOpening.Category.CategoryName) ? i.JobOpening.Category.CategoryName : "-"
                            ,!string.IsNullOrEmpty(i.JobOpening.Title) ? i.JobOpening.Title : "-"
                            ,!string.IsNullOrEmpty(i.Status) ? i.Status : "-"
                            ,!string.IsNullOrEmpty(i.Cv) ? ImageServer + i.Cv : "-"

							//Experience Work
							,exp != null ? exp.CompanyName : "-"
                            ,exp != null ? exp.Designation : "-"
                            ,exp != null ? (exp.StartDate.HasValue ? exp.StartDate.Value.ToString("dd MMM yyyy, h:mm tt") : "-") : "-"
                            ,exp != null ? (exp.EndDate.HasValue ? exp.EndDate.Value.ToString("dd MMM yyyy, h:mm tt") : "-") : "-"
                            ,exp != null ? (exp.EndDate.HasValue ? "No" : "Yes") : "-"

							//Education Work
							,edu != null ? edu.Degree : "-"
                            ,edu != null ? edu.Institute : "-"
                            ,edu != null ? edu.PassingYear : "-"

                        });

                        #region Experience and Education Details Work

                        if (experiences.Count() > 0)
                            experiences.Remove(exp);

                        if (educationdetails.Count() > 0)
                            educationdetails.Remove(edu);

                        int count = experiences.Count() > educationdetails.Count() ? experiences.Count() : educationdetails.Count();

                        for (int j = 0; j < count; j++)
                        {
                            exp = experiences.Count() <= j ? null : experiences[j];
                            edu = educationdetails.Count() <= j ? null : educationdetails[j];

                            cellData.Add(new object[]
                                {
									//Applicant Details Work
									""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""

									//Experience Work
									,exp != null ? exp.CompanyName : " "
                                    ,exp != null ? exp.Designation : " "
                                    ,exp != null ? (exp.StartDate.HasValue ? exp.StartDate.Value.ToString("dd MMM yyyy, h:mm tt") : "-") : " "
                                    ,exp != null ? (exp.EndDate.HasValue ? exp.EndDate.Value.ToString("dd MMM yyyy, h:mm tt") : "-") : " "
                                    ,exp != null ? (exp.EndDate.HasValue ? "No" : "Yes") : " "

									//Education Work
									,edu != null ? edu.Degree : " "
                                    ,edu != null ? edu.Institute : " "
                                    ,edu != null ? edu.PassingYear : " "

                                });

                        }

                        //new empty row added after applicant deatils
                        cellData.Add(new object[]
                                {
									//Applicant Details Work
									""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""

									//Experience Work
									,""
                                    ,""
                                    ,""
                                    ,""
                                    ,""

									//Education Work
									,""
                                    ,""
                                    ,""

                                });

                        #endregion
                    }

                    #endregion

                    worksheet.Cells[3, 1].LoadFromArrays(cellData);

                    return File(excel.GetAsByteArray(), "application/msexcel", "Applicants Report.xlsx");
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}