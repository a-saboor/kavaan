using Project.Data;
using Project.Service;
using Project.Service.Helpers;
using Project.Web.Helpers;
using Project.Web.Helpers.Routing;
using Project.Web.ViewModels.Blog;
using Project.Web.ViewModels.JobCandidate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Project.Web.Controllers{/*[RoutePrefix("development")]*/
	public class OldProjectsController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IPropertyService _propertyService;
		private readonly IPropertyProgressService _propertyprogressService;

		public OldProjectsController(IPropertyService propertyService, IPropertyProgressService propertyprogressService)
		{
			this._propertyService = propertyService;
			this._propertyprogressService = propertyprogressService;
		}

		#region OldProjects

		[HttpGet]
		[Route("projects")]
		public ActionResult Index()
		{
			var properties = _propertyService.GetProperties();

			return View(properties);
		}

		[HttpGet]
		[Route("projects/{id}")]
		public ActionResult Details(long id)
		{
			var property = _propertyService.GetProperty(id);//get single property

			if (property == null)
			{
				throw new HttpException(404, "File Not Found");
			}
			return View(property);
		}

		[HttpGet]
		[Route("projects/get-all")]
		public ActionResult GetAll(string culture = "en-ae", int take = 8)
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = CustomURL.GetImageServer();
				var properties = _propertyService.GetProperties().Where(x => x.IsActive == true).OrderByDescending(x => x.ID).Take(take);

				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = properties.Select(x => new
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
		[Route("projects/filters", Name = "projects/filters")]
		public ActionResult Index(FilterViewModel filters, string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = CustomURL.GetImageServer();
				var projects = _propertyService.GetFilteredProperties(filters.search, filters.pageSize, filters.pageNumber, filters.sortBy, lang, ImageServer, filters.parentID,null,filters.vrTour);
				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = projects.Select(x => new
					{
						x.ID,
						x.Date,
						x.Title,
						x.VRTour,
						x.Description,
						x.Image,
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

		#endregion OldProjects

		#region Construction Updates

		[HttpGet]
		[Route("construction-updates")]
		public ActionResult Construction()
		{
			return View();
		}

		[HttpGet]
		[Route("projects-progress/{id}")]
		public ActionResult ProjectProgress(long id)
		{
			var project = _propertyService.GetProperty(id);
			if (project != null)
				if (project.PropertyProgresses.Count() > 0)
					ViewBag.LastUpdatedDate = project.PropertyProgresses.FirstOrDefault().UpdatedOn.HasValue ? project.PropertyProgresses.FirstOrDefault().UpdatedOn.Value.ToString("dd-MMM-yyyy"): "";
			
			return View(project);
		}

		#endregion Construction Updates

		#region VR-Tours

		[HttpGet]
		[Route("vr-tours")]
		public ActionResult VRTours()
		{
			return View();
		}

		[HttpGet]
		[Route("vr-tours-details")]
		public ActionResult VRToursDetails()
		{
			return PartialView();
		}

		#endregion
	}
}