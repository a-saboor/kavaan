using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using Project.Web.ViewModels.OzoneGallery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
    public class OzoneImageController : Controller
    {
		// GET: Admin/OzoneImage
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IOzoneImageService _ozoneImageService;
		private readonly IOzoneService _ozoneService;

		public OzoneImageController(IOzoneImageService ozoneImageService, IOzoneService ozoneService)
		{
			this._ozoneImageService = ozoneImageService;
			this._ozoneService = ozoneService;
		}

		public ActionResult GetOzoneImages(long id)
		{
			ViewBag.OzoneID = id;
			return View();
		}
		[HttpGet]
		public ActionResult GetOzoneImagesbyid(long id)
		{
			var ozoneImages = _ozoneImageService.GetOzoneImagesByOzoneID(id).Select(i => new
			{
				id = i.ID,
				//i.Title,
				Image = i.Image
				//position = i.Position,
			}).ToList();
			ViewBag.OzoneID = id;

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				ozoneImages = ozoneImages
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
				Ozone introduction = _ozoneService.GetOzone((long)id);
				if (introduction == null)
				{
					return HttpNotFound();
				}

				string message = string.Empty;

				string absolutePath = Server.MapPath("~");
				string relativePath = string.Format("/Assets/AppFiles/Images/OzoneGallery/{0}/Gallery/", "OZONE".Replace(" ", "_"));

				List<string> Pictures = new List<string>();

				Dictionary<long, string> data = new Dictionary<long, string>();
				Uploader.UploadImages(Request.Files, absolutePath, relativePath, "g", ref Pictures, ref message, "GalleryImages");
				foreach (var item in Pictures)
				{
					OzoneImage introductionImage = new OzoneImage();
					introductionImage.OzoneID = id;
					introductionImage.Image = item;
					//carImage.Position = ++count;
					if (_ozoneImageService.CreateOzoneImage(ref introductionImage, ref message))
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
			if (_ozoneImageService.DeleteOzoneImage(id, ref message, ref filePath))
			{
				System.IO.File.Delete(Server.MapPath(filePath));
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult CreateOzoneGallery(long? id)
		{
			Session["id"] = id;
			ViewBag.OzoneID = TempData["id"];
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			GalleryList objBannersViewModel = new GalleryList()
			{
				GalleryBanners = _ozoneImageService.GetOzoneImages().ToList(),

			};
			//Gallery=ClsShoeImages
			return View(objBannersViewModel);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateOzoneGallery(FormCollection form, string bannertype)
		{
			long OzoneID= (long)Session["id"];
			
			string ErrorMessage = string.Empty;
			OzoneImage objBannersViewModel = new OzoneImage();
			var Position = _ozoneImageService.GalleyOzoneCount(OzoneID);

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

							OzoneImage ObjModelGalleryImage = new OzoneImage();
							ObjModelGalleryImage.OzoneID = OzoneID;
							ObjModelGalleryImage.Image = FilePath;
							ObjModelGalleryImage.Position = ++Position;
							//ObjModelShoeImage.Thumbnail = ImagesCount++ == 0 ? true : false;
							string msg = null;

							if (_ozoneImageService.CreateOzoneImage(ref ObjModelGalleryImage, ref msg))
							{
								try
								{
									MainDirectory = Path.Combine(Server.MapPath("~" + FilePath));
									file.SaveAs(MainDirectory);
									TempData["SuccessMessage"] = "Ozone images added successfully ...";
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

			return RedirectToAction("CreateOzoneGallery");

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
					var ItemImages = _ozoneImageService.SaveItemImagePosition(item.ID, item.Position, ref ErrorMessage);
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
			OzoneImage bannerData = _ozoneImageService.GetOzoneImage(id);
			string filepath = bannerData.Image;
			if (System.IO.File.Exists(absolutePath + bannerData.Image))
			{
				System.IO.File.Delete(absolutePath + bannerData.Image);
			}
			_ozoneImageService.DeleteOzoneImage(id,ref message,ref filepath );
			TempData["SuccessMessage"] = "Image Deleted Successfully ...";
			long ID = (long)Session["id"];
			return RedirectToAction("CreateOzoneGallery", new { id= ID });
		}
	}
}