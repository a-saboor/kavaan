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

namespace MyProject.Web.Controllers{
	public class NewsfeedsController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly INewsFeedService _newsFeedService;

		public NewsfeedsController(INewsFeedService newsFeedService)
		{
			this._newsFeedService = newsFeedService;
		}

		[HttpGet]
		[Route("newsfeeds")]
		public ActionResult Index()
		{
			var newsFeed = _newsFeedService.GetNewsFeed();

			return View(newsFeed);
		}

		[HttpPost]
		[Route("newsfeeds/filters", Name = "newsfeeds/filters")]
		public ActionResult Index(FilterViewModel filters, string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var newsFeed = _newsFeedService.GetFilteredNewsfeeds(filters.search, filters.pageSize, filters.pageNumber, filters.sortBy, lang, ImageServer);
				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = newsFeed.Select(x => new
					{
						x.ID,
						FullDate = x.Date.HasValue ? x.Date.Value.ToString("dd MMM, yyyy") : "-",
						Date = x.Date.HasValue ? x.Date.Value.ToString("dd") : "-",
						Month = x.Date.HasValue ? x.Date.Value.ToString("MMM") : "-",
						x.PostedDate,
						x.Slug,
						x.Title,
						x.Description,
						x.Host,
						Image = x.Cover,
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
		[Route("newsfeeds/{slug}")]
		public ActionResult Details(string slug)
		{
			var newsFeed = _newsFeedService.GetNewsFeedBySlug(slug);//get single event

			if (newsFeed == null)
			{
				throw new HttpException(404, "File Not Found");
			}
			return View(newsFeed);
		}

		[HttpGet]
		[Route("newsfeeds/get-all")]
		public ActionResult GetAll(string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var newsFeed = _newsFeedService.GetNewsFeed().Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.ID).Take(8);

				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = newsFeed.Select(x => new
					{
						x.ID,
						Date = x.EventDate.HasValue ? x.EventDate.Value.ToString("dd") : "-",
						Month = x.EventDate.HasValue? x.EventDate.Value.ToString("MMM") : "-",
						x.PostedDate,
						x.Slug,
						x.Host,
						Title = lang == "en" ? x.Title : x.TitleAr,
						Image = ImageServer + x.BannerImage,
						TitleDescription = lang == "en" ? x.TitleDescription : x.TitleDescriptionAr
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