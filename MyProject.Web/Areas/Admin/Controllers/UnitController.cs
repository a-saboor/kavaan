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
	public class UnitController : Controller
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private Project_DB_Entities db = new Project_DB_Entities();
		private readonly IUnitService unitService;
		private readonly ICountryService countryService;
		private readonly ICityService cityService;
		private readonly IAreaService areaService;
		private readonly ICustomerService customerService;
		private readonly IPropertyService propertyService;
		private readonly IUnitTypeService unitTypeService;
		private readonly INumberRangeService numberRangeService;
		string SuccessMessage = string.Empty;
		string ErrorMessage = string.Empty;
		public UnitController(IUnitService unitService, ICountryService countryService, ICityService cityService, IAreaService areaService, ICustomerService customerService, IPropertyService propertyService, IUnitTypeService unitTypeService, INumberRangeService numberRangeService)
		{
			this.unitService = unitService;
			this.countryService = countryService;
			this.cityService = cityService;
			this.areaService = areaService;
			this.customerService = customerService;
			this.propertyService = propertyService;
			this.unitTypeService = unitTypeService;
			this.numberRangeService = numberRangeService;
		}

		// GET: Admin/Units
		public ActionResult Index()
		{

			return View();
		}

		public ActionResult List()
		{

			var units = this.unitService.GetUnits();
			return View(units.ToList());
		}

		// GET: Admin/Units/Details/5
		public ActionResult Details(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Unit unit = unitService.GetUnit((long)id);
			if (unit == null)
			{
				return HttpNotFound();
			}
			ViewBag.CountryID = new SelectList(countryService.GetCountriesForDropDown(), "value", "text", unit.CountryID);
			ViewBag.CityID = new SelectList(cityService.GetCitiesForDropDown(), "value", "text", unit.CityID);
			ViewBag.AreaID = new SelectList(areaService.GetAreasForDropDown(), "value", "text", unit.AreaID);
			ViewBag.PropertyID = new SelectList(propertyService.GetPropertiesForDropDown(), "value", "text", unit.PropertyID);
			ViewBag.UnitTypeID = new SelectList(this.unitTypeService.GetUnitTypeForDropDown(), "value", "text", unit.UnitTypeID);

			return View(unit);
		}

		// GET: Admin/Units/Create
		public ActionResult Create()
		{
			ViewBag.CountryID = new SelectList(countryService.GetCountriesForDropDown(), "value", "text");
			ViewBag.CityID = new SelectList(cityService.GetCitiesForDropDown(), "value", "text");
			ViewBag.AreaID = new SelectList(areaService.GetAreasForDropDown(), "value", "text");
			ViewBag.UnitTypeID = new SelectList(this.unitTypeService.GetUnitTypeForDropDown(), "value", "text");
			ViewBag.PropertyID = new SelectList(propertyService.GetPropertiesForDropDown(), "value", "text");
			return View();
		}

		// POST: Admin/Units/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Unit data)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (data.Thumbnail != null)
				{
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Units/Thumbnail/{0}/", data.Title.Replace(" ", "_").Trim());
					data.Thumbnail = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Thumbnail", ref message, "Thumbnail");
				}
				if (data.FloorPlan != null)
				{
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Units/FloorPlan/{0}/", data.Title.Replace(" ", "_").Trim());
					string errorMsg = string.Empty;

					data.FloorPlan = Uploader.UploadDocs(Request.Files, absolutePath, relativePath, "FloorPlan", ref errorMsg, "FloorPlan");
					if (!string.IsNullOrEmpty(errorMsg))
					{
						data.FloorPlan = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "FloorPlan", ref errorMsg, "FloorPlan");
					}
				}
				if (data.Video != null)
				{
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Units/Video/{0}/", data.Title.Replace(" ", "_").Trim());
					data.Video = Uploader.UploadVideo(Request.Files, absolutePath, relativePath, "Video", ref message, "Video");
				}
				//if (data.VRTour != null)
				//{
				//	string absolutePath = Server.MapPath("~");
				//	string relativePath = string.Format("/Assets/AppFiles/Units/VRTour/{0}/", data.Title.Replace(" ", "_").Trim());
				//	data.VRTour = Uploader.UploadVideo(Request.Files, absolutePath, relativePath, "VRTour", ref message, "VRTour");
				//}
				if (data.Broucher != null)
				{
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Units/Broucher/{0}/", data.Title.Replace(" ", "_").Trim());
					data.Broucher = Uploader.UploadDocs(Request.Files, absolutePath, relativePath, "Broucher", ref message, "Broucher");
				}

				data.Slug = Helpers.Slugify.GenerateSlug(data.Slug);
				data.UnitNo = this.numberRangeService.GetNextValueFromNumberRangeByName("UNIT");
				if (unitService.CreateUnit(ref data, ref message))
				{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created unit {data.Title}.");
					TempData["SuccessMessage"] = message;
					return Json(new
					{
						success = true,
						url = "/Admin/unit/Index",
						message = message,

					});
				}
				return Json(new { success = false, message = message });
				//ViewBag.AreaID = new SelectList(areaService.GetAreasForDropDown(), "value", "text");
				//ViewBag.CityID = new SelectList(cityService.GetCitiesForDropDown(), "value", "text");
				//ViewBag.CountryID = new SelectList(countryService.GetCountriesForDropDown(), "value", "text");
				//ViewBag.UnitTypeID = new SelectList(this.unitTypeService.GetUnitTypeForDropDown(), "value", "text");
				//ViewBag.PropertyID = new SelectList(propertyService.GetPropertiesForDropDown(), "value", "text");
				//ViewBag.ErrorMessage = message;
				//return View(data);

			}

			message = "Please fill the form correctly ...";
			return Json(new { success = false, message = message });
			//ViewBag.AreaID = new SelectList(areaService.GetAreasForDropDown(), "value", "text");
			//ViewBag.CityID = new SelectList(cityService.GetCitiesForDropDown(), "value", "text");
			//ViewBag.CountryID = new SelectList(countryService.GetCountriesForDropDown(), "value", "text");
			//ViewBag.UnitTypeID = new SelectList(this.unitTypeService.GetUnitTypeForDropDown(), "value", "text");
			//ViewBag.PropertyID = new SelectList(propertyService.GetPropertiesForDropDown(), "value", "text");
			//ViewBag.ErrorMessage = "Please fill the form properly ...";
			//return View(data);


		}

		// GET: Admin/Units/Edit/5
		public ActionResult Edit(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Unit unit = this.unitService.GetUnit((long)id);
			if (unit == null)
			{
				return HttpNotFound();
			}
			ViewBag.CountryID = new SelectList(countryService.GetCountriesForDropDown(), "value", "text", unit.CountryID);
			ViewBag.CityID = new SelectList(cityService.GetCitiesForDropDown((long)unit.CountryID), "value", "text", unit.CityID);
			//ViewBag.AreaID = new SelectList(areaService.GetAreasForDropDown((long)unit.CityID), "value", "text", unit.AreaID);

			ViewBag.UnitTypeID = new SelectList(this.unitTypeService.GetUnitTypeForDropDown(), "value", "text", unit.UnitTypeID);
			ViewBag.PropertyID = new SelectList(propertyService.GetPropertiesForDropDown(), "value", "text", unit.PropertyID);

			return View(unit);
		}

		// POST: Admin/Units/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Unit data)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				Unit db = unitService.GetUnit(data.ID);
				db.Title = data.Title;
				db.TitleAr = data.TitleAr;
				db.Slug = data.Slug;
				db.Description = data.Description;
				db.DescriptionAr = data.DescriptionAr;
				db.PropertyID = data.PropertyID;
				db.Size = data.Size;
				db.SellingPrice = data.SellingPrice;
				db.BookingAmount = data.BookingAmount;
				db.EMI = data.EMI;
				db.LandRegistrationFee = data.LandRegistrationFee;
				db.OqoodAmount = data.OqoodAmount;
				db.DubaiLandDepartmentFee = data.DubaiLandDepartmentFee;
				db.PropertyUsage = data.PropertyUsage;
				db.Address = data.Address;
				db.Latitude = data.Latitude;
				db.Longitude = data.Longitude;
				db.CountryID = data.CountryID;
				db.CityID = data.CityID;
				//db.AreaID = data.AreaID;
				db.PropertyID = data.PropertyID;
				db.NoOfBedrooms = data.NoOfBedrooms;
				db.NoOfGarages = data.NoOfGarages;
				db.NoOfRooms = data.NoOfRooms;
				db.NoOfBaths = data.NoOfBaths;
				db.NoOfDinings = data.NoOfDinings;
				db.NoOfLaundry = data.NoOfLaundry;
				db.BuildYear = data.BuildYear;
				db.IsPublished = data.IsPublished;
				db.ZipCode = data.ZipCode;
				db.VRTour = data.VRTour;
				db.Status = data.Status;
				db.IsPublished = data.IsPublished;
				db.IsFeatured = data.IsFeatured;
				//db.UnitNo = data.UnitNo;
				if (data.Thumbnail != null)
				{
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Units/Thumbnail/{0}/", data.Title.Replace(" ", "_").Trim());
					db.Thumbnail = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Thumbnail", ref message, "Thumbnail");
				}
				if (data.FloorPlan != null)
				{
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Units/FloorPlan/{0}/", data.Title.Replace(" ", "_").Trim());
					string errorMsg = string.Empty;

					data.FloorPlan = Uploader.UploadDocs(Request.Files, absolutePath, relativePath, "FloorPlan", ref errorMsg, "FloorPlan");
					if (!string.IsNullOrEmpty(errorMsg))
					{
						data.FloorPlan = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "FloorPlan", ref errorMsg, "FloorPlan");
					}
				}
				if (data.Video == null)
				{
					if (Request.Files["VideoFile"] != null)

					{
						string absolutePath = Server.MapPath("~");
						string relativePath = string.Format("/Assets/AppFiles/Units/Video/{0}/", data.Title.Replace(" ", "_").Trim());
						db.Video = Uploader.UploadVideo(Request.Files, absolutePath, relativePath, "Video", ref message, "VideoFile");

					}
					else
					{
						db.Video = "";

					}
				}
				if (data.Broucher != null)
				{
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Units/Broucher/{0}/", data.Title.Replace(" ", "_").Trim());
					db.Broucher = Uploader.UploadDocs(Request.Files, absolutePath, relativePath, "Broucher", ref message, "Broucher");

				}
				if (this.unitService.UpdateUnit(ref db, ref message))
				{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated unit {data.Title}.");
					TempData["SuccessMessage"] = message;
					return Json(new
					{
						success = true,
						url = "/Admin/unit/Index",
						message = message,

					});

				}
				return Json(new { success = false, message = message });
				//ViewBag.AreaID = new SelectList(areaService.GetAreasForDropDown(), "value", "text", data.AreaID);
				//ViewBag.CityID = new SelectList(cityService.GetCitiesForDropDown(), "value", "text", data.CityID);
				//ViewBag.CountryID = new SelectList(countryService.GetCountriesForDropDown(), "value", "text", data.CountryID);
				//ViewBag.UnitTypeID = new SelectList(this.unitTypeService.GetUnitTypeForDropDown(), "value", "text", data.UnitTypeID);
				//ViewBag.PropertyID = new SelectList(propertyService.GetPropertiesForDropDown(), "value", "text", data.PropertyID);

				//TempData["ErrorMessage"] = message;
				//return View(data);
			}
			message = "Please fill the form correctly ...";
			return Json(new { success = false, message = message });
			//TempData["ErrorMessage"] = message;
			//return View(data);


		}

		// GET: Admin/Units/Delete/5
		public ActionResult Delete(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Unit unit = db.Units.Find(id);
			if (unit == null)
			{
				return HttpNotFound();
			}
			return View(unit);
		}

		// POST: Admin/Units/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id)
		{
			string message = string.Empty;
			if (unitService.DeleteUnit(id, ref message, true))
			{
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted unit ID: {id}.");
				return Json(new { success = true, message = message });
			}
			return Json(new { success = false, message = message });

		}

		public ActionResult ActivateFeature(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var Feed = this.unitService.GetUnit((long)id);
			if (Feed == null)
			{
				return HttpNotFound();
			}

			if (!(bool)Feed.IsFeatured)
				Feed.IsFeatured = true;
			else
			{
				Feed.IsFeatured = false;
			}
			string message = string.Empty;
			if (this.unitService.UpdateUnit(ref Feed, ref message))
			{
				SuccessMessage = "Unit " + ((bool)Feed.IsFeatured ? "featured" : "not featured") + "  successfully ...";
				return Json(new
				{
					success = true,
					message = SuccessMessage,
					data = new
					{
						CreatedOn = Feed.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
						Title = Feed.Title,
						Address = string.Format("{0},{1},{1}", Feed.Area.Name, Feed.City.Name, Feed.Country.Name),
						IsFeatured = Feed.IsFeatured.ToString(),
						ID = Feed.ID
					}
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				ErrorMessage = "Oops! something went wrong ...";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}


	}
}
