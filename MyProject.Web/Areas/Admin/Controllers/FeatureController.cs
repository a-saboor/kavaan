using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class FeatureController : Controller
	{
		private readonly IFeatureService featureService;


		public FeatureController(IFeatureService featureService)
		{
			this.featureService = featureService;

		}
		// GET: Admin/Newsfeed
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult List()
		{
			var feature = this.featureService.GetFeatures();

			return PartialView(feature);
		}
		public ActionResult Create()
		{
			return View();
		}
		public ActionResult Edit(long id)
		{
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Feature feature = this.featureService.GetFeatureByID(id);
			if (feature == null)
			{
				return HttpNotFound();
			}
			return View(feature);
		}
		public ActionResult Details(long id)
		{
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Feature feature = this.featureService.GetFeatureByID(id);

			if (feature == null)
			{
				return HttpNotFound();
			}
			return View(feature);
		}
		public ActionResult Delete(long id)
		{
			string message = string.Empty;
			bool success = this.featureService.DeleteFeature(id, ref message, true);

			return Json(new { success = success, message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult Create(Feature data)
		{
			string message = string.Empty;

			if (ModelState.IsValid)
			{
				string replacelement = "";
				if (data.Image != null)
				{
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Features/Images/{0}/", replacelement.Replace(" ", "_"));
					data.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
				}
				if (this.featureService.CreateFeature(data, ref message))
				{
					return Json(new
					{
						success = true,
						url = "/Admin/Feature/Index",
						message = message,
						data = new
						{
							CreatedOn = data.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
							Name = data.Name,
							Approval = data.IsApproved.HasValue ? data.IsApproved.Value.ToString() : bool.FalseString.ToString(),
							IsActive = data.IsActive.HasValue ? data.IsActive.Value.ToString() : bool.FalseString.ToString(),
							ID = data.ID,
						}
					}, JsonRequestBehavior.AllowGet);
				}
				return Json(new { success = false, message = message });
			}
			else
			{
				message = "Please fill the form properly ...";
				return Json(new { success = false, message = message });
			}

		}
		public ActionResult Activate(long id)
		{
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var feature = this.featureService.GetFeatureByID(id);
			if (feature == null)
			{
				return HttpNotFound();
			}

			if (!(bool)feature.IsActive)
				feature.IsActive = true;
			else
			{
				feature.IsActive = false;
			}
			string message = string.Empty;
			if (this.featureService.UpdateFeature(ref feature, ref message))
			{
				SuccessMessage = "Feature " + ((bool)feature.IsActive ? "activated" : "deactivated") + "  successfully ...";
				return Json(new
				{
					success = true,
					message = SuccessMessage,
					data = new
					{
						CreatedOn = feature.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
						Name = feature.Name,
						Approval = feature.IsApproved.HasValue ? feature.IsApproved.Value.ToString() : bool.FalseString.ToString(),
						IsActive = feature.IsActive.HasValue ? feature.IsActive.Value.ToString() : bool.FalseString,
						ID = feature.ID
					}
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				ErrorMessage = "Oops! Something went wrong. Please try later...";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}
		public ActionResult Update(Feature data, string Title)
		{
			string message = "";
			string replacelement = "";
			Feature updatefeed = featureService.GetFeatureByID(data.ID);

			updatefeed.Name = data.Name;
			updatefeed.Description = data.Description;
			updatefeed.DescriptionAr = data.DescriptionAr;
			updatefeed.NameAr = data.NameAr;
			updatefeed.IsApproved = data.IsApproved;



			string imagepath = Server.MapPath("~") + updatefeed.Image;

			if (data.ID == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			if (data.Image != null)
			{
				string absolutePath = Server.MapPath("~");
				if (data.Image != null)
				{
					if (System.IO.File.Exists(imagepath))
					{
						System.IO.File.Delete(imagepath);
					}
				}
				string relativePath = string.Format("/Assets/AppFiles/Features/BannerImages/{0}/", replacelement.Replace(" ", "_"));
				updatefeed.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
			}
			if (featureService.UpdateFeature(ref updatefeed, ref message))
			{

				return Json(new
				{
					success = true,
					url = "/Admin/Feature/Index",
					message = message,
					data = new
					{
						CreatedOn = updatefeed.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
						Name = updatefeed.Name,
						Approval = updatefeed.IsApproved.HasValue ? updatefeed.IsApproved.Value.ToString() : bool.FalseString.ToString(),
						IsActive = updatefeed.IsActive.HasValue ? updatefeed.IsActive.Value.ToString() : bool.FalseString.ToString(),
						ID = updatefeed.ID,
					}
				}, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message });


		}


	}
}