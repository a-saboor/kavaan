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
	public class BlogsController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IBlogService _blogService;

		public BlogsController(IBlogService blogService)
		{
			this._blogService = blogService;
		}

		[HttpGet]
		[Route("blogs", Name = "blogs")]
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[Route("blogs/filters", Name = "blogs/filters")]
		public ActionResult Index(FilterViewModel filters, string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var blogs = _blogService.GetFilteredBlog(filters.search, filters.pageSize, filters.pageNumber, filters.sortBy,lang, ImageServer,null,null);
				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = blogs.Select(x => new
					{
						x.ID,
						x.Date,
						x.PostedDate,
						FullDate = x.PostedDate.HasValue ? x.PostedDate.Value.ToString("dd MMM, yyyy") : "-",
						x.Slug,
						x.Title,
						x.Description,
						x.Author,
						Image = x.Cover,
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
		[Route("blogs/{slug}", Name = "blogs/{slug}")]
		public ActionResult Details(string slug)
		{
			var blog = _blogService.GetBlogBySlug(slug);//get single blog

			if (blog == null)
			{
				throw new HttpException(404, "File Not Found");
			}
			return View(blog);
		}

		[HttpGet]
		[Route("blogs/get-all", Name = "blogs/get-all")]
		public ActionResult GetAll(string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = "";
				var blogs = _blogService.GetBlog().Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.ID).Take(8);

				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = blogs.Select(x => new
					{
						x.ID,
						Date = x.PostedDate.HasValue ? x.PostedDate.Value.ToString("dd") : "-",
						Month = x.PostedDate.HasValue ? x.PostedDate.Value.ToString("MMM") : "-",
						x.PostedDate,
						x.Slug,
						x.Author,
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
					message = ""
				}, JsonRequestBehavior.AllowGet);
			}
		}

	}
}