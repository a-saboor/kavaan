using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider; 
using Project.Web.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LinqToExcel;
using Project.Web.Helpers.POCO;
using System.ComponentModel.DataAnnotations;
using OfficeOpenXml;
using System.Web.Helpers;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class UnitPaymentPlanController : Controller
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IUnitPaymentPlanService unitPaymentPlanService;
		private readonly IUnitService unitService;

		public UnitPaymentPlanController(IUnitPaymentPlanService unitPaymentPlanService, IUnitService unitService)
		{
			this.unitPaymentPlanService = unitPaymentPlanService;
			this.unitService = unitService;
		}

		public ActionResult Index(int unitid)
		{

			ViewBag.UnitId = unitid;
			return View();
		}

		public ActionResult List(int unitid)
		{
			IEnumerable<UnitPaymentPlan> unitpaymentplans = unitPaymentPlanService.GetUnitPaymentPlans(unitid);
			Unit unit = this.unitService.GetUnit(unitid);
			ViewBag.UnitName = unit.Title;
			ViewBag.UnitId = unit.ID;
			return PartialView(unitpaymentplans);
		}
		public ActionResult Details(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			UnitPaymentPlan paymentPlan = unitPaymentPlanService.GetUnitPaymentPlan((long)id);
			if (paymentPlan == null)
			{
				return HttpNotFound();
			}
			ViewBag.UnitID = new SelectList(unitService.GetUnitsForDropDown(), "value", "text", paymentPlan.UnitID);

			return View(paymentPlan);
		}

		public ActionResult Create(long unitid)
		{
			ViewBag.UnitID = unitid;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(UnitPaymentPlan paymentPlan)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (this.unitPaymentPlanService.CreateUnitPaymentPlan(ref paymentPlan, ref message))
				{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created unit payment plan.");
					return Json(new
					{
						success = true,
						url = "/Admin/PaymentPlan/Index",
						message = message,
						data = new
						{
							Date = paymentPlan.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
							MilesStone = paymentPlan.Milestones,
							Percentage = paymentPlan.Percentage,
							Amount = paymentPlan.Amount.ToString(),
							ID = paymentPlan.ID
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
			UnitPaymentPlan paymentPlan = this.unitPaymentPlanService.GetUnitPaymentPlan((long)id);
			if (paymentPlan == null)
			{
				return HttpNotFound();
			}

			TempData["ID"] = id;
			return View(paymentPlan);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(UnitPaymentPlan paymentPlan)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (this.unitPaymentPlanService.UpdateUnitPaymentPlan(ref paymentPlan, ref message))
				{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated unit payment plan.");
					return Json(new
					{
						success = true,
						url = "/Admin/PaymentPlan/Index",
						message = message,
						data = new
						{
							Date = paymentPlan.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
							MilesStone = paymentPlan.Milestones,
							Percentage = paymentPlan.Percentage,
							Amount = paymentPlan.Amount.ToString(),
							ID = paymentPlan.ID
						}
					}, JsonRequestBehavior.AllowGet);
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
			//if (id == null)
			//{
			//    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			//}
			//var country = this.unitPaymentPlanService.GetUnitPaymentPlan((long)id);
			//if (country == null)
			//{
			//    return HttpNotFound();
			//}

			//if (!(bool)country.IsActive)
			//    country.IsActive = true;
			//else
			//{
			//    country.IsActive = false;
			//}
			//string message = string.Empty;
			//if (_countryService.UpdateCountry(ref country, ref message))
			//{
			//    SuccessMessage = "Country " + ((bool)country.IsActive ? "activated" : "deactivated") + "  successfully ...";
			//    return Json(new
			//    {
			//        success = true,
			//        message = SuccessMessage,
			//        data = new
			//        {
			//            Date = country.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
			//            Name = country.Name,
			//            IsActive = country.IsActive.HasValue ? country.IsActive.Value.ToString() : bool.FalseString,
			//            ID = country.ID
			//        }
			//    }, JsonRequestBehavior.AllowGet);
			//}
			//else
			//{
			//    ErrorMessage = "Oops! Something went wrong. Please try later...";
			//}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Delete(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			UnitPaymentPlan paymentPlan = this.unitPaymentPlanService.GetUnitPaymentPlan((long)id);
			if (paymentPlan == null)
			{
				return HttpNotFound();
			}
			TempData["PaymentPlanId"] = id;
			return View(paymentPlan);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id)
		{
			string message = string.Empty;
			if (this.unitPaymentPlanService.DeleteUnitPaymentPlan((long)id, ref message))
			{
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted unit payment plan.");
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}




	}
}