using MyProject.Data;
using MyProject.Service;
using MyProject.Service.Helpers;
using MyProject.Web.Helpers;
using MyProject.Web.Helpers.Routing;
using MyProject.Web.ViewModels;
using MyProject.Web.ViewModels.JobCandidate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyProject.Web.Controllers
{
	public class ModalPopupsController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IUnitService _unitsService;
		private readonly IUnitEnquiriesService _unitEnquiriesService;
		private readonly IUnitPaymentPlanService _unitPaymentPlanService;

		public ModalPopupsController(IUnitService unitsService, IUnitEnquiriesService unitEnquiriesService, IUnitPaymentPlanService unitPaymentPlanService)
		{
			this._unitsService = unitsService;
			this._unitEnquiriesService = unitEnquiriesService;
			this._unitPaymentPlanService = unitPaymentPlanService;
		}

		#region Login Modal
		public ActionResult LoginModal()
		{
			return View();
		}
		#endregion

		[Route("confirmation-message", Name = "confirmation-messag")]
		public ActionResult ConfirmationMessage()
		{
			string url = TempData["NewBookingURL"] != null ? TempData["NewBookingURL"].ToString() : "javascript:void(0);";
			string text = TempData["NewBookingText"] != null ? TempData["NewBookingText"].ToString() : "OK";

			ViewBag.URL = url;
			ViewBag.Text = text;

			if (url.Contains("my-bookings"))
			{
				var booking = TempData["NewBooking"] as ServiceBooking;
				ViewData["Booking"] = booking;
			}

			return View();
		}

		#region Units Modal
		public ActionResult UnitModal()
		{
			return View();
		}
		#endregion

		#region Enquire

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UnitEnquiry(UnitEnquiry unitEnquiry)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				try
				{
					//Get Customer ID
					if (Session["CustomerID"] != null)
						unitEnquiry.CustomerID = Convert.ToInt64(Session["CustomerID"].ToString());
					else
						unitEnquiry.CustomerID = null;

					if (_unitEnquiriesService.CreateUnitEnquiry(ref unitEnquiry, ref message))
					{
						return Json(new
						{
							success = true,
							message = "Enquiry submitted successfully.",
						}, JsonRequestBehavior.AllowGet);
					}
				}
				catch (Exception ex)
				{
					message = ex.Message;
				}
			}
			return Json(new
			{
				success = false,
				message = ""
			}, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region Payment Plan
		[HttpPost]
		public ActionResult GetPaymentPlans(long id)
		{
			try
			{
				var units = _unitPaymentPlanService.GetPaymentPlanByUnitID(id).OrderBy(x => x.ID).ToList();

				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = units.Select(x => new
					{
						x.ID,
						x.Milestones,
						x.Percentage,
						x.Amount,
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
		#endregion
	}
}