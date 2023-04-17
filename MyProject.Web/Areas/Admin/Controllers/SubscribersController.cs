using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider; 
using MyProject.Web.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;
using MyProject.Web.ViewModels;
using MyProject.Service.Helpers;
using OfficeOpenXml;
using MyProject.Web.Helpers.Routing;
using System.Collections.Generic;

namespace MyProject.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class SubscribersController : Controller
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private readonly ISubscribersService _subscribersService;
		private readonly IMail _email;

		public SubscribersController(ISubscribersService subscribersService, IMail email)
		{
			this._subscribersService = subscribersService;
			this._email = email;

		}

		public ActionResult Index()
		{
			ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
			ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-365).ToString("MM/dd/yyyy");
			return View();
		}

		public ActionResult List()
		{
			DateTime ToDate = Helpers.TimeZone.GetLocalDateTime();
			DateTime FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-365);
			var subscribers = _subscribersService.GetsubscribersDateWise(FromDate, ToDate).OrderByDescending(i => i.ID).ToList();

			return PartialView(subscribers);

		}

		[HttpPost]
		public ActionResult List(DateTime fromDate, DateTime ToDate)
		{
			DateTime EndDate = ToDate.AddMinutes(1439);
			var Subscribers = _subscribersService.GetsubscribersDateWise(fromDate, EndDate);
			return PartialView(Subscribers);
		}

		public ActionResult SendEmailToSubscribers()
		{
			ViewBag.Email = new SelectList(_subscribersService.GetSubscribersForDropDown(), "value", "text");
			return View();
		}

		[HttpPost]
		public ActionResult SendEmailToSubscribers(SubscriberMailViewModel subscribermail)
		{
			string message = string.Empty;
			var path = Server.MapPath("~/");
			foreach (var item in subscribermail.Email)
			{
				Subscriber Email = _subscribersService.GetSubscriberByID(item);

				if (_email.SendPromoMail(Email.Email, subscribermail.Subject, subscribermail.Message, path))
				{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} sent email to subscribers.");
					return Json(new
					{
						success = true,
						url = "/Admin/Subscribers/Index",
						message = "Email Sent",
						data = new
						{
							Date = Email.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
							Email = Email.Email,

						}
					});

				}
			}

			return RedirectToAction("Index", "Subscriber");
		}
		#region Reports

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SubscribersReport(DateTime FromDate, DateTime ToDate)
		{
			DateTime EndDate = ToDate.AddMinutes(1439);
			var getAllCandidates = _subscribersService.GetsubscribersDateWise(FromDate, EndDate);
			if (getAllCandidates.Count() > 0)
			{
				using (ExcelPackage excel = new ExcelPackage())
				{
					excel.Workbook.Worksheets.Add("SubscribersReport");

					var headerRow = new List<string[]>()
					{
					new string[] {
						"Date"
						,"Email"

						}
					};

					// Determine the header range (e.g. A1:D1)
					string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

					// Target a worksheet
					var worksheet = excel.Workbook.Worksheets["SubscribersReport"];

					// Popular header row data
					worksheet.Cells[headerRange].LoadFromArrays(headerRow);

					var cellData = new List<object[]>();

					if (getAllCandidates.Count != 0)
						getAllCandidates = getAllCandidates.OrderByDescending(x => x.ID).ToList();

					foreach (var i in getAllCandidates)
					{
						cellData.Add(new object[] {
						i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString(CustomHelper.GetDateFormat) : "-"
                        //,!string.IsNullOrEmpty(i.Country.Name) ? i.Country.Name :"-"
                        ,i.Email != null ? i.Email: "-"

						});
					}

					worksheet.Cells[2, 1].LoadFromArrays(cellData);

					return File(excel.GetAsByteArray(), "application/msexcel", "Subscribers Report.xlsx");
				}
			}
			return RedirectToAction("Index");
		}

		#endregion
	}
}