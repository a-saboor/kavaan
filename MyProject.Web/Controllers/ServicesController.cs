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
	
	public class ServicesController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly ICategoryService _categoriesService;
		private readonly IServicesService _servicesService;

		public ServicesController(ICategoryService categoriesService,IServicesService servicesService)
		{
			this._categoriesService = categoriesService;
			this._servicesService = servicesService;
		}

		[HttpGet]
		[Route("service/{slug}", Name = "service/{slug}")]
		public ActionResult Index(string slug)
		{
			Category category = _categoriesService.GetCategoryBySlug(slug);
			if (category == null)
			{
				throw new HttpException(404, "File Not Found");
			}
			return View(category);
		}

		[HttpPost]
		[Route("services/filters", Name = "services/filters")]
		public ActionResult Index(FilterViewModel filters, string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var services = _servicesService.GetFilteredServices(filters.search, filters.pageSize, filters.pageNumber, filters.sortBy, lang, ImageServer, filters.parentID);
				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = services.Select(x => new
					{
						x.ID,
						Title = x.Name,
						x.Thumbnail,
						x.Slug,
						x.Description,
						Image = x.Thumbnail,
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
		[Route("services/get-all", Name = "services/get-all")]
		public ActionResult GetAll(string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var services = _servicesService.GetServices().Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.ID).Take(8);

				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = services.Select(x => new
					{
						x.ID,
						Title = x.Name,
						TitleAr = x.NameAr,
						x.Thumbnail,
						x.Slug,
						x.Description,
						Image = x.Thumbnail,
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