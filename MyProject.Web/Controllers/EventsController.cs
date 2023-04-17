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
	public class EventsController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IEventService _eventService;

		public EventsController(IEventService eventService)
		{
			this._eventService = eventService;
		}

		[HttpGet]
		[Route("news-and-events")]
		public ActionResult Index()
		{
			var events = _eventService.GetEvent();

			return View(events);
		}

		[HttpPost]
		[Route("news-and-events/filters", Name = "news-and-events/filters")]
		public ActionResult Index(FilterViewModel filters, string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var events = _eventService.GetFilteredEvents(filters.search, filters.pageSize, filters.pageNumber, filters.sortBy, lang, ImageServer);
				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = events.Select(x => new
					{
						x.ID,
						Date = x.Date.HasValue ? x.Date.Value.ToString("dd") : "-",
						Month = x.Date.HasValue ? x.Date.Value.ToString("MMM") : "-",
						x.PostedDate,
						x.Slug,
						x.Title,
						x.Description,
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
		[Route("news-and-events/{slug}")]
		public ActionResult Details(string slug)
		{
			var events = _eventService.GetEventBySlug(slug);//get single event

			if (events == null)
			{
				throw new HttpException(404, "File Not Found");
			}
			return View(events);
		}

		[HttpGet]
		[Route("news-and-events/get-all")]
		public ActionResult GetAll(string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var events = _eventService.GetEvent().Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.ID).Take(8);

				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = events.Select(x => new
					{
						x.ID,
						Date = x.EventDate.HasValue ? x.EventDate.Value.ToString("dd") : "-",
						Month = x.EventDate.HasValue? x.EventDate.Value.ToString("MMM") : "-",
						x.PostedDate,
						x.Slug,
						Title = lang == "en" ? x.Title : x.TitleAr,
						BannerImage = ImageServer + x.BannerImage,
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