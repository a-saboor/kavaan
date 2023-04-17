using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using Project.Web.Helpers.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class EventController : Controller
	{
		#region Dependecy Settings

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		string Message = string.Empty;
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;
		bool status = false;

		private readonly IEventService _EventService;
		public EventController(IEventService EventService)
		{
			_EventService = EventService;
		}
		#endregion

		public ActionResult Index()
		{
			return View();
		}
		public ActionResult List()
		{
			var Event = _EventService.GetEvent();
			return PartialView(Event);
		}
		public ActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Create(Event data)
		{
			if (ModelState.IsValid)
			{
				Event record = new Event();
				record.Title = data.Title;
				record.TitleAr = data.TitleAr;
				record.Slug = Slugify.GenerateSlug(data.Title);
				record.TitleDescription = data.TitleDescription;
				record.TitleDescriptionAr = data.TitleDescriptionAr;
				record.Host = data.Host;
				record.EventDate = data.EventDate;
				record.TwitterUrl = data.TwitterUrl;
				record.FacebookURL = data.FacebookURL;
				record.InstagramURL = data.InstagramURL;
				record.LinkedinURL = data.LinkedinURL;
				record.Email = data.Email;
				record.IsFeatured = data.IsFeatured;
				record.Position = data.Position;
				record.IsActive = true;
				record.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				record.IsDeleted = false;
				record.PostedDate = data.PostedDate;
				string replacelement = data.Title.Replace("?", "");

				if (Request.Files.Count > 0)
				{
					try
					{
						//  Get all files from Request object  
						HttpFileCollectionBase files = Request.Files;
						if (files.Count > 0)
						{
							string absolutePath = Server.MapPath("~");
							if (data.BannerImage != null)
							{
								string relativePath = string.Format("/Assets/AppFiles/Event/BannerImages/{0}/", replacelement.Replace(" ", "_"));
								record.BannerImage = Uploader.UploadImage(files, absolutePath, relativePath, "BannerImage", ref Message, "BannerImage");
								if (!string.IsNullOrEmpty(record.BannerImage))
								{
									record.BannerImage = CustomURL.GetImageServer() + record.BannerImage.Remove(0, 1);
								}
							}
							if (data.Video != null)
							{
								string relativePath = string.Format("/Assets/AppFiles/Event/Video/{0}/", replacelement.Replace(" ", "_"));
								record.Video = Uploader.UploadVideo(files, absolutePath, relativePath, "Video", ref Message, "Video");
								if (!string.IsNullOrEmpty(record.Video))
								{
									record.Video = CustomURL.GetImageServer() + record.Video.Remove(0, 1);
								}
							}
						}
					}
					catch (Exception ex)
					{
						//add logs here...
					}
				}
				if (_EventService.CreateEvent(record, ref Message))
				{
					status = true;
					return Json(new
					{
						success = true,
						url = "/Admin/Event/Index",
						message = Message,
						data = new
						{
							CreatedOn = record.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
							Title = record.Title,
							EventDate = record.EventDate.Value.ToString("dd MMMM yyyy"),
							IsActive = record.IsActive.ToString(),
							ID = record.ID
						}
					});
				}
			}
			else
			{
				Message = "Please fill the form properly ...";
			}
			return Json(new { success = status, message = Message });
		}
		public ActionResult Edit(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Event feed = _EventService.GetEventByID((long)id);
			//feed.PostedDate = DateTime.Parse(feed.PostedDate.Value.ToString("ddd/MM/yyy"));
			if (feed == null)
			{
				return HttpNotFound();
			}


			return View(feed);
		}
		[HttpPost]
		//[ValidateInput(false)]
		public ActionResult Edit(Event data)
		{
			string message = string.Empty;

			Event updatefeed = _EventService.GetEventByID(data.ID);
			string previousImagepath = updatefeed.BannerImage.Replace(CustomURL.GetImageServer(), "");
			string previousVideopath = updatefeed.Video.Replace(CustomURL.GetImageServer(), "");

			if (ModelState.IsValid)
			{
				string replacelement = data.Title.Replace("?", "");
				updatefeed.Title = data.Title;
				updatefeed.TitleAr = data.TitleAr;
				updatefeed.Slug = Slugify.GenerateSlug(data.Title);
				updatefeed.TitleDescription = data.TitleDescription;
				updatefeed.TitleDescriptionAr = data.TitleDescriptionAr;
				updatefeed.Host = data.Host;
				updatefeed.EventDate = data.EventDate.Value.Date;
				updatefeed.TwitterUrl = data.TwitterUrl;
				updatefeed.FacebookURL = data.FacebookURL;
				updatefeed.InstagramURL = data.InstagramURL;
				updatefeed.LinkedinURL = data.LinkedinURL;
				updatefeed.Email = data.Email;
				updatefeed.IsFeatured = data.IsFeatured;
				updatefeed.Position = data.Position;
				updatefeed.PostedDate = data.PostedDate;

				if (Request.Files.Count > 0)
				{
					try
					{
						//  Get all files from Request object  
						HttpFileCollectionBase files = Request.Files;
						if (files.Count > 0)
						{
							string absolutePath = Server.MapPath("~");
							if (data.BannerImage != null && !data.BannerImage.Equals(CustomURL.GetImageServer() + previousImagepath))
							{
								string relativePath = string.Format("/Assets/AppFiles/Event/BannerImages/{0}/", replacelement.Replace(" ", "_"));
								updatefeed.BannerImage = Uploader.UploadImage(files, absolutePath, relativePath, "BannerImage", ref Message, "BannerImage");
								if (!string.IsNullOrEmpty(updatefeed.BannerImage))
								{
									updatefeed.BannerImage = CustomURL.GetImageServer() + updatefeed.BannerImage.Remove(0, 1);
								}
								//delete old file
								if (System.IO.File.Exists(previousImagepath))
								{
									System.IO.File.Delete(previousImagepath);
								}
							}
							if (data.Video != null && !data.Video.Equals(CustomURL.GetImageServer() + previousVideopath))
							{
								string relativePath = string.Format("/Assets/AppFiles/Event/Video/{0}/", replacelement.Replace(" ", "_"));
								updatefeed.Video = Uploader.UploadVideo(files, absolutePath, relativePath, "Video", ref Message, "Video");
								if (!string.IsNullOrEmpty(updatefeed.Video))
								{
									updatefeed.Video = CustomURL.GetImageServer() + updatefeed.Video.Remove(0, 1);
								}
								//delete old file
								if (System.IO.File.Exists(previousVideopath))
								{
									System.IO.File.Delete(previousVideopath);
								}
							}
						}
					}
					catch (Exception ex)
					{
						//add logs here...
					}
				}
				else
				{
					if (string.IsNullOrEmpty(data.BannerImage))
					{
						updatefeed.BannerImage = "";
						//delete old file
						if (System.IO.File.Exists(previousImagepath))
						{
							System.IO.File.Delete(previousImagepath);
						}
					}
					if (string.IsNullOrEmpty(data.Video))
					{
						updatefeed.Video = "";
						//delete old file
						if (System.IO.File.Exists(previousVideopath))
						{
							System.IO.File.Delete(previousVideopath);
						}
					}
				}

				if (_EventService.UpdateEvent(ref updatefeed, ref message))
				{


					return Json(new
					{
						success = true,
						url = "/Admin/Event/Index",
						message = message,
						data = new
						{
							CreatedOn = updatefeed.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
							Title = updatefeed.Title,
							EventDate = updatefeed.EventDate.Value.ToString("dd MMMM yyyy"),
							IsActive = updatefeed.IsActive.ToString(),
							ID = updatefeed.ID
						}
					}, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				message = "Please fill the form properly ...";
			}
			return Json(new { message = message, success = false });
		}
		public ActionResult Activate(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var Feed = _EventService.GetEventByID((long)id);
			if (Feed == null)
			{
				return HttpNotFound();
			}

			if (!(bool)Feed.IsActive)
				Feed.IsActive = true;
			else
			{
				Feed.IsActive = false;
			}
			string message = string.Empty;
			if (_EventService.UpdateEvent(ref Feed, ref message))
			{
				SuccessMessage = "Event " + ((bool)Feed.IsActive ? "activated" : "deactivated") + "  successfully ...";
				return Json(new
				{
					success = true,
					message = SuccessMessage,
					data = new
					{
						CreatedOn = Feed.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
						Title = Feed.Title,
						EventDate = Feed.EventDate.Value.ToString("dd MMM yyyy, h:mm tt"),
						IsActive = Feed.IsActive.HasValue ? Feed.IsActive.Value.ToString() : bool.FalseString,
						ID = Feed.ID
					}
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				ErrorMessage = "Oops! Something went wrong. Please try later.";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}
		public ActionResult Details(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Event Feed = _EventService.GetEventByID((Int16)id);

			if (Feed == null)
			{
				return HttpNotFound();
			}
			return View(Feed);
		}
		public ActionResult Delete(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Event Newsfeed = _EventService.GetEventByID((Int16)id);
			if (Newsfeed == null)
			{
				return HttpNotFound();
			}
			TempData["ID"] = id;
			return View(Newsfeed);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id, bool softDelete = true)
		{
			Event Newsfeed = _EventService.GetEventByID((Int16)id);
			string message = string.Empty;
			if (softDelete)
			{
				//soft delete of data updating delete column
				if (_EventService.DeleteEvent((Int16)id, ref message, softDelete))
				{
					return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

				}
			}
			else
			{
				//permenant delete of data
				if (_EventService.DeleteEvent((Int16)id, ref message, softDelete))
				{
					if (Newsfeed.BannerImage != null)
					{
						string absolutePath = Server.MapPath("~");
						if (System.IO.File.Exists(absolutePath + Newsfeed.BannerImage))
						{
							System.IO.File.Delete(absolutePath + Newsfeed.BannerImage);
						}
					}
					if (Newsfeed.Video != null)
					{
						string absolutePath = Server.MapPath("~");
						if (System.IO.File.Exists(absolutePath + Newsfeed.Video))
						{
							System.IO.File.Delete(absolutePath + Newsfeed.Video);
						}
					}
					return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
				}
			}

			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}
	}
}