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
	
	public class CategoriesController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly ICategoryService _categoriesService;

		public CategoriesController(ICategoryService categoriesService)
		{
			this._categoriesService = categoriesService;
		}

		[HttpGet]
		[Route("service-categories", Name = "service-categories")]
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[Route("categories/filters", Name = "categories/filters")]
		public ActionResult Index(FilterViewModel filters, string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var categories = _categoriesService.GetFilteredCategories(filters.search, filters.pageSize, filters.pageNumber, filters.sortBy, lang, ImageServer);
				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = categories.Select(x => new
					{
						x.ID,
						Title = x.Name,
						x.Icon,
						x.Image,
						x.Thumbnail,
						x.Slug,
						x.Description,
					})
				}, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = ""
				}, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpGet]
		[Route("categories/get-all", Name = "categories/get-all")]
		public ActionResult GetAll(string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var categories = _categoriesService.GetCategories().Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.ID).ToList()/*.Take(8)*/;

				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = categories.Select(x => new
					{
						x.ID,
						Title = x.CategoryName,
						TitleAR = x.CategoryNameAr,
						x.Icon,
						x.Image,
						x.Thumbnail,
						x.Slug,
						x.Description,
					})
				}, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = ""
				}, JsonRequestBehavior.AllowGet);
			}
		}
	}
}