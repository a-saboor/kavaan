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
	public class ContractorsController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IContractorService _contractorService;

		public ContractorsController(IContractorService contractorService)
		{
			this._contractorService = contractorService;
		}

		[HttpGet]
		[Route("contractors", Name = "contractors")]
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[Route("contractors/filters", Name = "contractors/filters")]
		public ActionResult Index(FilterViewModel filters, string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var contractors = _contractorService.GetFilteredContractors(filters.search, filters.pageSize, filters.pageNumber, filters.sortBy, lang, ImageServer);
				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = contractors.Select(x => new
					{
						x.ID,
						x.Date,
						x.Name,
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

		[HttpGet]
		[Route("contractors/{id}", Name = "contractors/{id}")]
		public ActionResult Details(long id)
		{
			var contractor = _contractorService.Edit(id);//get single contractor

			if (contractor == null)
			{
				throw new HttpException(404, "File Not Found");
			}
			return View(contractor);
		}

		[HttpGet]
		[Route("contractors/get-all", Name = "contractors/get-all")]
		public ActionResult GetAll(string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var contractors = _contractorService.GetAll().Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.ID).Take(8);

				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = contractors.Select(x => new
					{
						x.ID,
						Date = x.CreatedOn.ToString(MyProject.Web.Helpers.CustomHelper.GetDateFormat),
						Name = lang == "en" ? x.Name : x.Name,
						Image = ImageServer + x.Image,
						Description = lang == "en" ? x.Description : x.DescriptionAr
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

	}
}