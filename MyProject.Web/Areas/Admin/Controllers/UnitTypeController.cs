using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider; 
using Project.Web.Helpers;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class UnitTypeController : Controller
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IUnitTypeService unitTypeService;
		public UnitTypeController(IUnitTypeService unitTypeService)
		{
			this.unitTypeService = unitTypeService;
		}

		// GET: Admin/UnitTypes
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult List()
		{
			var unitTypes = this.unitTypeService.GetUnits();
			return PartialView(unitTypes);
		}

		// GET: Admin/UnitTypes/Details/5
		public ActionResult Details(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			UnitType unitType = this.unitTypeService.GetUnitType((long)id);
			if (unitType == null)
			{
				return HttpNotFound();
			}
			return View(unitType);
		}

		// GET: Admin/UnitTypes/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Admin/UnitTypes/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ID,Name,NameAr,IsActive,CreatedOn,IsDeleted")] UnitType unitType)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (this.unitTypeService.CreateUnitType(ref unitType, ref message))
				{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created unit type {unitType.Name}.");
					return Json(new
					{
						success = true,
						url = "/Admin/unittype/Index",
						message = message,
						data = new
						{
							Date = unitType.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
							Name = unitType.Name,
							IsActive = unitType.IsActive.HasValue ? unitType.IsActive.Value.ToString() : bool.FalseString,
							ID = unitType.ID
						}
					});
				}
				return Json(new { success = false, message = message });
			}
			message = "Please fill the form correctly";

			return Json(new { success = false, message = message });
		}

		// GET: Admin/UnitTypes/Edit/5
		public ActionResult Edit(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			UnitType unitType = this.unitTypeService.GetUnitType((long)id);
			if (unitType == null)
			{
				return HttpNotFound();
			}
			return View(unitType);
		}

		// POST: Admin/UnitTypes/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,Name,NameAr,IsActive,CreatedOn,IsDeleted")] UnitType unitType)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (this.unitTypeService.UpdateUnitType(ref unitType, ref message))
				{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated unit type {unitType.Name}.");
					return Json(new
					{
						success = true,
						url = "/Admin/unittype/Index",
						message = message,
						data = new
						{
							Date = unitType.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
							Name = unitType.Name,
							IsActive = unitType.IsActive.HasValue ? unitType.IsActive.Value.ToString() : bool.FalseString,
							ID = unitType.ID
						}
					});
				}
				return Json(new { success = false, message = message });
			}
			return Json(new { success = false, message = message });
		}

		// GET: Admin/UnitTypes/Delete/5
		public ActionResult Delete(long? id)
		{
			//    string message = string.Empty;
			//    if (id == null)
			//    {
			//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			//    }
			//    this.unitTypeService.DeleteUnitType(id, ref message, true);
			//    if (unitType == null)
			//    {
			//        return HttpNotFound();
			//    }
			return View();
		}

		// POST: Admin/UnitTypes/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id)
		{
			string message = string.Empty;
			if (this.unitTypeService.DeleteUnitType(id, ref message, true))
			{
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted unit type ID: {id}.");
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);

		}
		public ActionResult Activate(long? id)
		{
			string ErrorMessage = string.Empty;
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var unit = this.unitTypeService.GetUnitType((long)id);
			if (unit == null)
			{
				return HttpNotFound();
			}

			if (!(bool)unit.IsActive)
			{ 
				unit.IsActive = true;
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} activated unit type {unit.Name}.");
			}
			else
			{
				unit.IsActive = false;
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deactivated unit type {unit.Name}.");
			}
			string message = string.Empty;
			if (this.unitTypeService.UpdateUnitType(ref unit, ref message))
			{
				string SuccessMessage = "UnitType " + ((bool)unit.IsActive ? "activated" : "deactivated") + "  successfully ...";
				return Json(new
				{
					success = true,
					message = SuccessMessage,
					data = new
					{
						Date = unit.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
						Name = unit.Name,
						IsActive = unit.IsActive.HasValue ? unit.IsActive.Value.ToString() : bool.FalseString,
						ID = unit.ID
					}
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				ErrorMessage = "Oops! Something went wrong. Please try later...";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}
	}
}
