using MyProject.Data;
using MyProject.Service;
using MyProject.Service.Helpers;
using MyProject.Web.Helpers;
using MyProject.Web.Helpers.Routing;
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
	public class DevelopmentsController : BaseController
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IDevelopmentService _developmentService;

		public DevelopmentsController(IDevelopmentService developmentService)
		{
			this._developmentService = developmentService;
		}

		[HttpGet]
		[Route("{culture}/developments")]
		public ActionResult Index()
		{
			var developments = _developmentService.GetAll();

			return View(developments);
		}

		[HttpGet]
		[Route("{culture}/developments/{id}")]
		public ActionResult Details(long id)
		{
			var development = _developmentService.Edit(id);//get single development

			if (development == null)
			{
				throw new HttpException(404, "File Not Found");
			}
			return View(development);
		}

		[HttpGet]
		[Route("{culture}/developments/get-all")]
		public ActionResult GetAll(string culture = "en-ae", int take = 8)
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var developments = _developmentService.GetAll().Where(x => x.IsActive == true).OrderBy(x => x.Position).Take(take);

				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = developments.Select(x => new
					{
						x.ID,
						Name = lang == "en" ? x.Name : x.NameAr,
						Image = ImageServer + x.Image,
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

		[HttpGet]
		[Route("{culture}/developments/development-types", Name = "developments/development-types")]
		public ActionResult DevelopmentTypes(string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			var developmentTypes = new SelectList(_developmentService.GetDevelopmentForDropDown(lang), "value", "text");

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				data = developmentTypes.Select(x => new
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