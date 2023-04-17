using MyProject.Data;
using MyProject.Service;
using MyProject.Service.Helpers;
using MyProject.Web.Helpers;
using MyProject.Web.Helpers.Routing;
using MyProject.Web.ViewModels.Blog;
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
	public class UnitsController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IUnitService _unitsService;
		private readonly IUnitTypeService _unitTypeService;
		public UnitsController(IUnitService unitsService, IUnitTypeService unitTypeService)
		{
			this._unitsService = unitsService;
			this._unitTypeService = unitTypeService;
		}

		[HttpGet]
		[Route("units")]
		public ActionResult Index(string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			return View();
		}

		[HttpGet]
		[Route("units/{id}")]
		public ActionResult Details(long id)
		{
			var units = _unitsService.GetUnit(id);//get single units

			if (units == null)
			{
				throw new HttpException(404, "File Not Found");
			}
			return View(units);
		}

		//[HttpGet]
		//[Route("units/{slug}")]
		//public ActionResult DetailsBySlug(string slug)
		//{
		//	var unit = _unitsService.GetUnitBySlug(slug);//get single unit

		//	if (unit == null)
		//	{
		//		throw new HttpException(404, "File Not Found");
		//	}
		//	return View(unit);
		//}

		[HttpGet]
		[Route("units/get-all")]
		public ActionResult GetAll(string culture = "en-ae", int take = 8)
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var units = _unitsService.GetUnits().OrderByDescending(x => x.ID).Take(take);

				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = units.Select(x => new
					{
						x.ID,
						Title = lang == "en" ? x.Title : x.TitleAr,
						Thumbnail = ImageServer + x.Thumbnail,
						Description = lang == "en" ? x.Description : x.DescriptionAr,
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

		[HttpPost]
		[Route("units/filters", Name = "units/filters")]
		public ActionResult Index(FilterViewModel filters, string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var units = _unitsService.GetFilteredUnits(filters.search, filters.pageSize, filters.pageNumber, filters.sortBy, lang, ImageServer, filters.parentID);
				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = units.Select(x => new
					{
						x.ID,
						x.Date,
						x.Title,
						x.Description,
						x.Image,
						x.Slug,
						x.CityName,
						x.NoOfRooms,
						x.NoOfBedrooms,
						x.NoOfBaths,
						x.Size,
						x.SellingPrice,

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

		[HttpPost]
		[Route("units/filtered-units", Name = "units/filtered-units")]
		public ActionResult FilteredUnits(FilterViewModel filters, string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var units = _unitsService.GetFilteredUnitsByProject(filters.search, filters.pageSize, filters.pageNumber, filters.sortBy, lang, ImageServer, filters.parentID,null,null,filters.bedrooms,filters.bathrooms, filters.minprice, filters.maxprice, filters.typeId);
				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = units.Select(x => new
					{
						x.ID,
						x.Date,
						x.Title,
						x.Description,
						x.Image,
						x.Slug,
						x.CityName,
						x.NoOfRooms,
						x.NoOfBedrooms,
						x.NoOfBaths,
						x.Size,
						x.SellingPrice,
						x.Latitude,
						x.Longitude,
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

		[HttpGet]
		[Route("units/unit-types", Name = "units/unit-types")]
		public ActionResult UnitTypes(string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			var unitTypes = new SelectList(_unitTypeService.GetUnitTypeForDropDown(lang), "value", "text");

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				data = unitTypes.Select(x => new
				{
					x.Disabled,
					x.Selected,
					x.Group,
					x.Value,
					x.Text,
				})
			}, JsonRequestBehavior.AllowGet);
		}

	}
}