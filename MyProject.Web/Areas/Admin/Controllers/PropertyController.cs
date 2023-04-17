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
	public class PropertyController : Controller
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private readonly IPropertyService propertyService;
		private readonly ICountryService countryService;
		private readonly ICityService cityService;
		private readonly IAreaService areaService;
		private readonly IDevelopmentService developmentService;

		public PropertyController(IPropertyService propertyService, ICountryService countryService, ICityService cityService, IAreaService areaService, IDevelopmentService developmentService)
		{
			this.propertyService = propertyService;
			this.countryService = countryService;
			this.cityService = cityService;
			this.areaService = areaService;
			this.developmentService = developmentService;
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
			var property = this.propertyService.GetProperties();

			return PartialView(property);
		}
		public ActionResult Create()
		{
			ViewBag.DevelopmentID = new SelectList(this.developmentService.GetDevelopmentForDropDown(), "value", "text");
			//ViewBag.CountryID = new SelectList(this.countryService.GetCountriesForDropDown(), "value", "text");
			//ViewBag.citieslist = new SelectList(this.cityService.GetCitiesForDropDown(), "value", "text");
			//ViewBag.AreaID = new SelectList(this.areaService.GetAreasForDropDown(), "value", "text");

			return PartialView();
		}

		[HttpPost, ValidateInput(false)]
		public ActionResult Create(Property data)
		{
			string message = string.Empty;

			if (ModelState.IsValid)
			{
				string replacement = data.Title.Replace("?", "");
				if (data.Thumbnail != null)
				{
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Property/Thumbnail/{0}/", replacement);
					data.Thumbnail = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Thumbnail", ref message, "Thumbnail");
				}
				if (data.Video != null)
				{
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Property/Video/{0}/", replacement);
					data.Video = Uploader.UploadVideo(Request.Files, absolutePath, relativePath, "Video", ref message, "Video");

				}
				//if (data.VRTour != null)
				//{
				//    string absolutePath = Server.MapPath("~");
				//    string relativePath = string.Format("/Assets/AppFiles/Property/VRTour/{0}/", replacement);
				//    data.VRTour = Uploader.UploadVideo(Request.Files, absolutePath, relativePath, "VRTour", ref message, "VRTour");

				//}
				if (data.Broucher != null)
				{
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Property/Broucher/{0}/", replacement);
					data.Broucher = Uploader.UploadDocs(Request.Files, absolutePath, relativePath, "Broucher", ref message, "Broucher");

				}
				if (data.FloorPlan != null)
				{
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Property/FloorPlan/{0}/", replacement.Replace(" ", "_"));
					string errorMsg = string.Empty;

					data.FloorPlan = Uploader.UploadDocs(Request.Files, absolutePath, relativePath, "FloorPlan", ref errorMsg, "FloorPlan");
					if (!string.IsNullOrEmpty(errorMsg))
					{
						data.FloorPlan = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "FloorPlan", ref errorMsg, "FloorPlan");
					}
				}

				if (this.propertyService.Create(data, ref message))
				{
					Development development = this.developmentService.Edit((long)data.DevelopmentID);
					//Country country = this.countryService.GetCountry((long)data.CountryID);
					//City city = this.cityService.GetCity((long)data.CityID);
					//Area area = this.areaService.GetArea((long)data.AreaID);
					TempData["SuccessMessage"] = message;
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created property {data.Title}.");
					return Json(new
					{
						success = true,
						url = "/Admin/Property/Index",
						message = message,
						data = new
						{
							CreatedOn = data.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
							DevelopmentName = development.Name,
							Title = data.Title,
							IsActive = data.IsActive.HasValue ? data.IsActive.Value.ToString() : bool.FalseString.ToString(),
							IsFeatured = data.IsFeatured.HasValue ? data.IsFeatured.Value.ToString() : bool.FalseString,
							ID = data.ID,
						}
					});
				}
				return Json(new { success = false, message = message });
			}
			else
			{
				message = "Please fill the form properly ...";
				return Json(new { success = false, message = message });
				//return View(data);
			}

		}
		public ActionResult Edit(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Property community = this.propertyService.GetProperty((long)id);
			if (community == null)
			{
				return HttpNotFound();
			}
			ViewBag.DevelopmentID = new SelectList(this.developmentService.GetDevelopmentForDropDown(), "value", "text", community.DevelopmentID);
			//ViewBag.CountryID = new SelectList(countryService.GetCountriesForDropDown(), "value", "text", community.CountryID);
			//ViewBag.CityID = new SelectList(cityService.GetCitiesForDropDown((long)community.CountryID), "value", "text", community.CityID);
			//ViewBag.AreaID = new SelectList(areaService.GetAreasForDropDown((long)community.CityID), "value", "text", community.AreaID);
			return View(community);
		}
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult Update(Property data)
		{
			string message = string.Empty;

			if (ModelState.IsValid)
			{
				string replacement = data.Title.Replace("?", "");
				var currentProperty = this.propertyService.GetProperty(data.ID);
				string absolutePath = Server.MapPath("~");
				if (data.Thumbnail != null)
				{
					string relativePath = string.Format("/Assets/AppFiles/Property/Thumbnail/{0}/", replacement.Replace(" ", ""));

					if (System.IO.File.Exists(absolutePath + currentProperty.Thumbnail))
					{
						System.IO.File.Delete(absolutePath + currentProperty.Thumbnail);
					}
					currentProperty.Thumbnail = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Thumbnail", ref message, "Thumbnail");
				}
				if (data.Video == null)
				{
					if (Request.Files["VideoFile"] != null)

					{
						if (System.IO.File.Exists(absolutePath + currentProperty.Video))
						{
							System.IO.File.Delete(absolutePath + currentProperty.Video);
						}
						string relativePath = string.Format("/Assets/AppFiles/Property/Video/{0}/", replacement);
						currentProperty.Video = Uploader.UploadVideo(Request.Files, absolutePath, relativePath, "Video", ref message, "VideoFile");

					}
					else
					{
						currentProperty.Video = "";
					}
				}
				//if (data.VRTour == null)
				//{
				//    if (Request.Files["VRTourFile"] != null)

				//    {
				//        if (System.IO.File.Exists(absolutePath + currentProperty.VRTour))
				//        {
				//            System.IO.File.Delete(absolutePath + currentProperty.VRTour);
				//        }
				//        string relativePath = string.Format("/Assets/AppFiles/Property/VRTour/{0}/", replacement);
				//        currentProperty.VRTour = Uploader.UploadVideo(Request.Files, absolutePath, relativePath, "Video", ref message, "VRTourFile");
				//    }
				//    else
				//    {
				//        currentProperty.VRTour = "";

				//    }
				//}
				if (data.Broucher != null && data.Broucher != "undefined")
				{
					if (System.IO.File.Exists(absolutePath + currentProperty.Broucher))
					{
						System.IO.File.Delete(absolutePath + currentProperty.Broucher);
					}
					string relativePath = string.Format("/Assets/AppFiles/Property/Broucher/{0}/", replacement);
					currentProperty.Broucher = Uploader.UploadDocs(Request.Files, absolutePath, relativePath, "Broucher", ref message, "Broucher");

				}
				if (data.FloorPlan != null && data.FloorPlan != "undefined")
				{
					if (System.IO.File.Exists(absolutePath + currentProperty.FloorPlan))
					{
						System.IO.File.Delete(absolutePath + currentProperty.FloorPlan);
					}
					string relativePath = string.Format("/Assets/AppFiles/Property/FloorPlan/{0}/", replacement.Replace(" ", "_"));
					string errorMsg = string.Empty;

					currentProperty.FloorPlan = Uploader.UploadDocs(Request.Files, absolutePath, relativePath, "FloorPlan", ref errorMsg, "FloorPlan");
					if (!string.IsNullOrEmpty(errorMsg))
					{
						currentProperty.FloorPlan = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "FloorPlan", ref errorMsg, "FloorPlan");
					}
				}
				currentProperty.IsFeatured = data.IsFeatured;
				currentProperty.Title = data.Title;
				currentProperty.TitleAr = data.TitleAr;
				//currentProperty.ShortDescription = data.ShortDescription;
				//currentProperty.ShortDescriptionAr = data.ShortDescriptionAr;
				currentProperty.Description = data.Description;
				currentProperty.DescriptionAr = data.DescriptionAr;
				currentProperty.VRTour = data.VRTour;
				//currentProperty.Address = data.Address;
				currentProperty.Latitude = data.Latitude;
				currentProperty.Longitude = data.Longitude;
				//currentProperty.CountryID = data.CountryID;
				//currentProperty.CityID = data.CityID;
				//currentProperty.AreaID = data.AreaID;
				currentProperty.DevelopmentID = data.DevelopmentID;
				currentProperty.Faqs = data.Faqs;
				currentProperty.FaqsAr = data.FaqsAr;
				if (this.propertyService.UpdateProperty(ref currentProperty, ref message))
				{
					TempData["SuccessMessage"] = message;
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated property {data.Title}.");
					return Json(new
					{
						success = true,
						url = "/Admin/Property/Index",
						message = message,
						data = new
						{
							CreatedOn = currentProperty.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
							DevelopmentName = currentProperty.Development.Name,
							Title = currentProperty.Title,
							IsActive = currentProperty.IsActive.HasValue ? currentProperty.IsActive.Value.ToString() : bool.FalseString.ToString(),
							IsFeatured = currentProperty.IsFeatured.HasValue ? currentProperty.IsFeatured.Value.ToString() : bool.FalseString,
							ID = currentProperty.ID,
						}
					});

				}
				var property = this.propertyService.GetProperty(data.ID);
				data.FloorPlan = property.FloorPlan;
				data.Broucher = property.Broucher;
				data.VRTour = property.VRTour;
				data.Video = property.Video;
				data.Thumbnail = property.Thumbnail;
				ViewBag.DevelopmentID = new SelectList(this.developmentService.GetDevelopmentForDropDown(), "value", "text", data.DevelopmentID);
				ViewBag.CountryID = new SelectList(countryService.GetCountriesForDropDown(), "value", "text", data.CountryID);
				ViewBag.CityID = new SelectList(cityService.GetCitiesForDropDown(), "value", "text", data.CityID);
				ViewBag.AreaID = new SelectList(areaService.GetAreasForDropDown(), "value", "text", data.AreaID);
				return Json(new { success = false, message = message });
				//ViewBag.ErrorMessage = message;
				//return View("Edit", data);

			}
			var property2 = this.propertyService.GetProperty(data.ID);
			data.FloorPlan = property2.FloorPlan;
			data.Broucher = property2.Broucher;
			data.VRTour = property2.VRTour;
			data.Video = property2.Video;
			data.Thumbnail = property2.Thumbnail;
			ViewBag.DevelopmentID = new SelectList(this.developmentService.GetDevelopmentForDropDown(), "value", "text", data.DevelopmentID);
			ViewBag.CountryID = new SelectList(countryService.GetCountriesForDropDown(), "value", "text", data.CountryID);
			ViewBag.CityID = new SelectList(cityService.GetCitiesForDropDown(), "value", "text", data.CityID);
			ViewBag.AreaID = new SelectList(areaService.GetAreasForDropDown(), "value", "text", data.AreaID);
			return Json(new { success = false, message = message });
			//ViewBag.ErrorMessage = "Please fill the form correctly ...";
			//return View("Edit", data);
		}
		public ActionResult Details(long id)
		{
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Property community = this.propertyService.GetProperty(id);
			ViewBag.DevelopmentID = new SelectList(this.developmentService.GetDevelopmentForDropDown(), "value", "text", community.DevelopmentID);
			//ViewBag.CountryID = new SelectList(countryService.GetCountriesForDropDown(), "value", "text", community.CountryID);
			//ViewBag.CityID = new SelectList(cityService.GetCitiesForDropDown(), "value", "text", community.CityID);
			//ViewBag.AreaID = new SelectList(areaService.GetAreasForDropDown(), "value", "text", community.AreaID);

			if (community == null)
			{
				return HttpNotFound();
			}
			return View(community);
		}
		public ActionResult Delete(long id)
		{
			string message = string.Empty;
			bool hasChilds = true;
			if (this.propertyService.DeleteProperty(id, ref message, ref hasChilds))
			{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted property ID: {id}.");
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}

			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);

		}

		public ActionResult Activate(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var Feed = this.propertyService.GetProperty((long)id);
			if (Feed == null)
			{
				return HttpNotFound();
			}

			if (!(bool)Feed.IsActive)
			{ 
				Feed.IsActive = true;
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} activated property {Feed.Title}.");
			}
			else
			{
				Feed.IsActive = false;
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deactivated property {Feed.Title}.");
			}
			string message = string.Empty;
			if (this.propertyService.UpdateProperty(ref Feed, ref message))
			{
				SuccessMessage = "Project " + ((bool)Feed.IsActive ? "activated" : "deactivated") + "  successfully ...";
				return Json(new
				{
					success = true,
					message = SuccessMessage,
					data = new
					{
						CreatedOn = Feed.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
						DevelopmentName = Feed.Development.Name,
						Title = Feed.Title,
						IsActive = Feed.IsActive.HasValue ? Feed.IsActive.Value.ToString() : bool.FalseString,
						IsFeatured = Feed.IsFeatured.HasValue ? Feed.IsFeatured.Value.ToString() : bool.FalseString,
						ID = Feed.ID
					}
				}, JsonRequestBehavior.AllowGet);
			}


			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}
		public ActionResult ActivateFeature(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var Feed = this.propertyService.GetProperty((long)id);
			if (Feed == null)
			{
				return HttpNotFound();
			}

			if (!(bool)Feed.IsFeatured)
				Feed.IsFeatured = true;
			else
			{
				Feed.IsFeatured = false;
			}
			string message = string.Empty;
			if (this.propertyService.UpdateProperty(ref Feed, ref message))
			{
				SuccessMessage = "Featured " + ((bool)Feed.IsFeatured ? "activated" : "deactivated") + "  successfully ...";
				return Json(new
				{
					success = true,
					message = SuccessMessage,
					data = new
					{
						CreatedOn = Feed.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
						DevelopmentName = Feed.Development.Name,
						Title = Feed.Title,
						IsActive = Feed.IsActive.HasValue ? Feed.IsActive.Value.ToString() : bool.FalseString,
						IsFeatured = Feed.IsFeatured.HasValue ? Feed.IsFeatured.Value.ToString() : bool.FalseString,
						ID = Feed.ID
					}
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				ErrorMessage = "Oops! Something went wrong. Please try later...";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}
		[ValidateInput(false)]
		public ActionResult GetDescriptionHTML(int id)
		{
			var result = propertyService.GetProperty(id)?.Faqs;
			return Json(!string.IsNullOrEmpty(result) ? result : string.Empty, JsonRequestBehavior.AllowGet);
		}
		//      [HttpPost, ActionName("Delete")]
		//      [ValidateAntiForgeryToken]
		//      public ActionResult DeleteConfirmed(long id, bool softDelete = true)
		//      {
		//          Property Newsfeed = this.propertyService.GetCommunityByID((Int16)id);
		//          string message = string.Empty;
		//          if (softDelete)
		//          {
		//              //soft delete of data updating delete column
		//              if (this.propertyService.DeleteCommunity((Int16)id, ref message, softDelete))
		//              {
		//                  return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

		//              }
		//          }
		//          else
		//          {
		//              //permenant delete of data
		//              if (this.propertyService.DeleteCommunity((Int16)id, ref message,softDelete))
		//              {
		//                  if (Newsfeed.BannerImage != null)
		//                  {
		//                      string absolutePath = Server.MapPath("~");
		//                      if (System.IO.File.Exists(absolutePath + Newsfeed.BannerImage))
		//                      {
		//                          System.IO.File.Delete(absolutePath + Newsfeed.BannerImage);
		//                      }
		//                  }
		//                  if (Newsfeed.Video != null)
		//                  {
		//                      string absolutePath = Server.MapPath("~");
		//                      if (System.IO.File.Exists(absolutePath + Newsfeed.Video))
		//                      {
		//                          System.IO.File.Delete(absolutePath + Newsfeed.Video);
		//                      }
		//                  }
		//                  return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
		//              }
		//          }

		//          return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		//      }
	}
}