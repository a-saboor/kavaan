using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider; 
using Project.Web.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using OfficeOpenXml;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class JobOpeningController : Controller
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly ICategoryService _categoryService;
		private readonly IJobOpeningService _jobOpeningService;

		public JobOpeningController(IJobOpeningService jobOpeningService, ICategoryService categoryService)
		{
			this._jobOpeningService = jobOpeningService;
			this._categoryService = categoryService;
		}

		public ActionResult Index()
		{
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			ViewBag.ExcelUploadErrorMessage = TempData["ExcelUploadErrorMessage"];
			return View();
		}

		public ActionResult List()
		{
			var jobOpenings = _jobOpeningService.GetJobOpenings();
			return PartialView(jobOpenings);
		}

		public ActionResult ListReport()
		{
			var jobOpenings = _jobOpeningService.GetJobOpenings();
			return View(jobOpenings);
		}

		public ActionResult Details(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			JobOpening jobOpening = _jobOpeningService.GetJobOpening((Int16)id);
			if (jobOpening == null)
			{
				return HttpNotFound();
			}
			return View(jobOpening);
		}

		public ActionResult Create()
		{
			ViewBag.CategoryID = new SelectList(_categoryService.GetCategoriesForDropDown(), "value", "text");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(JobOpening jobOpening)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (_jobOpeningService.CreateJobOpening(jobOpening, ref message))
				{
					var category = _categoryService.GetCategory((long)jobOpening.CategoryID);
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created department {jobOpening.Category.CategoryName}");
					return Json(new
					{
						success = true,
						url = "/Admin/JobOpening/Index",
						message = message,
						data = new
						{
							Date = jobOpening.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
							Department = category.CategoryName,
							Title = jobOpening.Title,
							IsActive = jobOpening.IsActive.HasValue ? jobOpening.IsActive.Value.ToString() : bool.FalseString,
							ID = jobOpening.ID
						}
					});
				}
			}
			else
			{
				message = "Please fill the form properly ...";
			}

			return Json(new { success = false, message = message });
		}

		public ActionResult Edit(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			JobOpening jobOpening = _jobOpeningService.GetJobOpening((long)id);
			if (jobOpening == null)
			{
				return HttpNotFound();
			}

			ViewBag.CategoryID = new SelectList(_categoryService.GetCategoriesForDropDown(), "value", "text", jobOpening.CategoryID);
			TempData["JobOpeningID"] = id;
			return View(jobOpening);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(JobOpening jobOpening)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				long Id;
				if (TempData["JobOpeningID"] != null && Int64.TryParse(TempData["JobOpeningID"].ToString(), out Id) && jobOpening.ID == Id)
				{
					if (_jobOpeningService.UpdateJobOpening(ref jobOpening, ref message))
					{
						log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated department {jobOpening.Category.CategoryName}");
						return Json(new
						{
							success = true,
							url = "/Admin/JobOpening/Index",
							message = "JobOpening updated successfully ...",
							data = new
							{
								Date = jobOpening.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
								Department = jobOpening.Category.CategoryName,
								Title = jobOpening.Title,
								IsActive = jobOpening.IsActive.HasValue ? jobOpening.IsActive.Value.ToString() : bool.FalseString,
								ID = jobOpening.ID
							}
						});
					}

				}
				else
				{
					message = "Oops! Something went wrong. Please try later.";
				}
			}
			else
			{
				message = "Please fill the form properly ...";
			}
			return Json(new { success = false, message = message });
		}

		public ActionResult Activate(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var jobOpening = _jobOpeningService.GetJobOpening((long)id);
			if (jobOpening == null)
			{
				return HttpNotFound();
			}

			if (!(bool)jobOpening.IsActive)
			{
				jobOpening.IsActive = true;
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} activated department {jobOpening.Category.CategoryName}");
			}
			else
			{
				jobOpening.IsActive = false;
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deactivated department {jobOpening.Category.CategoryName}");
			}
			string message = string.Empty;
			if (_jobOpeningService.UpdateJobOpening(ref jobOpening, ref message))
			{
				SuccessMessage = "JobOpening " + ((bool)jobOpening.IsActive ? "activated" : "deactivated") + "  successfully ...";
				return Json(new
				{
					success = true,
					message = SuccessMessage,
					data = new
					{
						Date = jobOpening.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
						Department = jobOpening.Category.CategoryName,
						Title = jobOpening.Title,
						IsActive = jobOpening.IsActive.HasValue ? jobOpening.IsActive.Value.ToString() : bool.FalseString,
						ID = jobOpening.ID
					}
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				ErrorMessage = "Oops! Something went wrong. Please try later...";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Delete(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			JobOpening jobOpening = _jobOpeningService.GetJobOpening((Int16)id);
			if (jobOpening == null)
			{
				return HttpNotFound();
			}
			TempData["JobOpeningID"] = id;
			return View(jobOpening);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id)
		{
			string message = string.Empty;
			if (_jobOpeningService.DeleteJobOpening((Int16)id, ref message))
			{
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted department ID: {id}");
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult JobOpeningsReport()
		{
			var getAllJobOpenings = _jobOpeningService.GetJobOpenings().ToList();
			if (getAllJobOpenings.Count() > 0)
			{
				using (ExcelPackage excel = new ExcelPackage())
				{
					excel.Workbook.Worksheets.Add("JobOpeningsReport");

					var headerRow = new List<string[]>()
				{
					new string[] {
						"Creation Date"
						,"Department"
						,"Title"
						,"TitleAr"
						,"Status"
					}
				};

					// Determine the header range (e.g. A1:D1)
					string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

					// Target a worksheet
					var worksheet = excel.Workbook.Worksheets["JobOpeningsReport"];

					// Popular header row data
					worksheet.Cells[headerRange].LoadFromArrays(headerRow);

					var cellData = new List<object[]>();

					if (getAllJobOpenings.Count != 0)
						getAllJobOpenings = getAllJobOpenings.OrderByDescending(x => x.ID).ToList();

					foreach (var i in getAllJobOpenings)
					{
						cellData.Add(new object[] {
						i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
						,i.Category != null ? i.Category.CategoryName :"-"
						,!string.IsNullOrEmpty(i.Title) ? i.Title :"-"
						,!string.IsNullOrEmpty(i.TitleAr) ? i.TitleAr :"-"
						,i.IsActive == true ? "Active" :"InActive"
						});
					}

					worksheet.Cells[2, 1].LoadFromArrays(cellData);

					return File(excel.GetAsByteArray(), "application/msexcel", "JobOpenings Report.xlsx");
				}
			}
			return RedirectToAction("Index");
		}
	}
}