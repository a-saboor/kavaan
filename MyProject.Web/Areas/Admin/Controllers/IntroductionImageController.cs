using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using Project.Web.ViewModels.IntroductionGallery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
    public class IntroductionImageController : Controller
	{
		// GET: Admin/IntroductionImage
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IIntroductionImageService _introductionImageService;
		private readonly IIntroductionService _introductionService;

		public IntroductionImageController(IIntroductionImageService introductionImageService, IIntroductionService introductionService)
		{
			this._introductionImageService = introductionImageService;
			this._introductionService = introductionService;
		}

		public ActionResult GetIntroductionImages(long id)
		{
			ViewBag.IntroductionID = id;
			return View();
		}
		[HttpGet]
		public ActionResult GetIntroductionImagesbyid(long id)
		{
			var introImages = _introductionImageService.GetIntroductionImagesByIntroID(id).Select(i => new
			{
				id = i.ID,
				//i.Title,
				Image = i.Image
				//position = i.Position,
			}).ToList();
			ViewBag.FacilityVenueID = id;

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				introImages = introImages
			}, JsonRequestBehavior.AllowGet);

		}
		[HttpPost]
		public ActionResult Create(long? id, int count)
		{
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Introduction introduction = _introductionService.GetIntroduction((long)id);
				if (introduction == null)
				{
					return HttpNotFound();
				}

				string message = string.Empty;

				string absolutePath = Server.MapPath("~");
				string relativePath = string.Format("/Assets/AppFiles/Images/IntroductionGallery/{0}/Gallery/", "Introduction".Replace(" ", "_"));

				List<string> Pictures = new List<string>();

				Dictionary<long, string> data = new Dictionary<long, string>();
				Uploader.UploadImages(Request.Files, absolutePath, relativePath, "g", ref Pictures, ref message, "GalleryImages");
				foreach (var item in Pictures)
				{
					IntroductionImage introductionImage = new IntroductionImage();
					introductionImage.IntroductionID = id;
					introductionImage.Image = item;
					//carImage.Position = ++count;
					if (_introductionImageService.CreateIntroductionImage(ref introductionImage, ref message))
					{
						data.Add(introductionImage.ID, item);
					}
				}

				return Json(new
				{
					success = true,
					message = message,
					data = data.ToList()
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = "Oops! Something went wrong. Please try later."
				});
			}
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id)
		{
			string message = string.Empty;
			string filePath = string.Empty;
			if (_introductionImageService.DeleteIntroductionImage(id, ref message, ref filePath))
			{
				System.IO.File.Delete(Server.MapPath(filePath));
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult CreateIntroductionGallery(long? id)
		{
			Session["id"] = id;
			ViewBag.IntroductionID = TempData["id"];
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			GalleryList objBannersViewModel = new GalleryList()
			{
				GalleryBanners = _introductionImageService.GetIntroductionImages().ToList(),

			};
			//Gallery=ClsShoeImages
			return View(objBannersViewModel);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateIntroductionGallery(FormCollection form, string bannertype)
		{
			long IntroductionID = (long)Session["id"];

			string ErrorMessage = string.Empty;
			OzoneImage objBannersViewModel = new OzoneImage();
			var Position = _introductionImageService.GalleyIntroductionCount(IntroductionID);

			//Int32 ImagesCount = objBannersViewModel.GetGalleryCountByID(id);
			for (int index = 0; index < Request.Files.Count; index++)
			{
				var file = Request.Files[index];
				if (file != null && Request.Files.AllKeys[index].Equals("file"))
				{
					string[] SupportedImageFormat = { ".jpeg", ".png", ".jpg" };
					String fileExtension = System.IO.Path.GetExtension(file.FileName);
					string FilePath;
					string MainDirectory = string.Empty;
					if (file.ContentType.Contains("image"))
					{
						if (SupportedImageFormat.Contains(fileExtension.ToLower()))
						{
							FilePath = string.Format("{0}{1}{2}", "/Assets/AppFiles/Gallery/", Guid.NewGuid().ToString(), fileExtension);

							IntroductionImage ObjModelGalleryImage = new IntroductionImage();
							ObjModelGalleryImage.IntroductionID = IntroductionID;
							ObjModelGalleryImage.Image = FilePath;
							ObjModelGalleryImage.Position = ++Position;
							//ObjModelShoeImage.Thumbnail = ImagesCount++ == 0 ? true : false;
							string msg = null;

							if (_introductionImageService.CreateIntroductionImage(ref ObjModelGalleryImage, ref msg))
							{
								try
								{
									MainDirectory = Path.Combine(Server.MapPath("~" + FilePath));
									file.SaveAs(MainDirectory);
									TempData["SuccessMessage"] = "Introduction images added successfully ...";
								}
								catch (Exception ex)
								{
									//objBannersViewModel.DeleteShoeImage((long)ObjModelShoeImage.ID, ref FilePath, ref id);
								}
							}
						}
						else
						{

							TempData["ErrorMessage"] += "Image Format Not Supported ...";
						}
					}
					else
					{
						TempData["ErrorMessage"] += "Wrong format for image ...";
					}
				}
				else
				{
					TempData["ErrorMessage"] += "Please Select a file first ...";
				}
			}

			ViewBag.Message = ErrorMessage;

			return RedirectToAction("CreateIntroductionGallery");

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SaveImagePositions(List<GalleryImagePositionViewModel> positions)
		{
			string ErrorMessage = string.Empty;
			string SuccessMessage = string.Empty;
			try
			{
				foreach (var item in positions)
				{
					var ItemImages = _introductionImageService.SaveItemImagePosition(item.ID, item.Position, ref ErrorMessage);
				}

				SuccessMessage = "Gallery image positions saved ...";
				return Json(new { success = true, message = SuccessMessage }, JsonRequestBehavior.AllowGet);
			}
			catch
			{
				ErrorMessage = "Oops! Something went wrong. Please try later...";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}
		public ActionResult DeleteImage(long id)
		{
			string message = string.Empty;
			string absolutePath = Server.MapPath("~");
			IntroductionImage bannerData = _introductionImageService.GetIntroductionImage(id);
			string filepath = bannerData.Image;
			if (System.IO.File.Exists(absolutePath + bannerData.Image))
			{
				System.IO.File.Delete(absolutePath + bannerData.Image);
			}
			_introductionImageService.DeleteIntroductionImage(id, ref message, ref filepath);
			TempData["SuccessMessage"] = "Image Deleted Successfully ...";
			long ID = (long)Session["id"];
			return RedirectToAction("CreateIntroductionGallery", new { id = ID });
		}
	}
}