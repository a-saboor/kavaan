using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using Project.Web.Areas.Admin.ViewModels;
using Project.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class PropertyImagesController : Controller
    {
        private readonly IPropertyImageService propertyImageService;
        private readonly IPropertyService propertyService;

        public PropertyImagesController(IPropertyImageService propertyImageService, IPropertyService propertyService)
        {
            this.propertyImageService = propertyImageService;
            this.propertyService = propertyService;
        }
        // GET: Admin/Newsfeed
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;




        public ActionResult Create(int id)
        {

            var property = this.propertyService.GetProperty(id);


            TempData["PropertyIDI"] = property.ID;
            TempData["PropertyTitleI"] = property.Title;

            List<PropertyImage> propertyImages = this.propertyImageService.GetPropertyImages(id).ToList();
            return PartialView(propertyImages);

        }
        // POST: Admin/PrpertyImages/Create 
        [HttpPost]
        public ActionResult CreateImages(PropertyImage propertyImage, long propertyid, int propertypoisition, string propertytitle)
        {

            List<string> propertytitleslist = new List<string>();
            List<PropertyImage> propertyImages = new List<PropertyImage>();
            string message = string.Empty;
            try
            {

                if (Request.Files.Count > 0)
                {

                    string absolutePath = Server.MapPath("~");
                    string prefix = propertytitle.Replace("?", "");
                    string relativePath = string.Format("/Assets/AppFiles/PropertyImages/Image/{0}/", prefix);
                    propertyImage.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, propertypoisition.ToString());
                    propertyImage.Position = propertypoisition;
                    var dbpropertyimage = this.propertyImageService.GetPropertyImageByPosition(propertyid, propertypoisition);
                    if (dbpropertyimage != null)
                    {
                        if (dbpropertyimage.Image != null)
                        {

                            if (System.IO.File.Exists(absolutePath + dbpropertyimage.Image))
                            {
                                System.IO.File.Delete(absolutePath + dbpropertyimage.Image);
                            }
                        }
                        this.propertyImageService.DeletePropertyImage(propertyid, propertypoisition);
                    }
                    if (this.propertyImageService.Create(propertyImage, ref message))
                    {

                        return Json(new
                        {
                            success = true,
                            url = "/Admin/Property/Index",
                            message = message


                        }, JsonRequestBehavior.AllowGet); ;
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops Something Went Wrong!",

                }, JsonRequestBehavior.AllowGet);

            }
            return Json(new
            {
                success = false,
                message = "Oops Something Went Wrong!",

            }, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/PropertyFeatures/Edit/5
        public ActionResult Edit(long id)
        {
            return View();
        }

        // POST: Admin/PropertyFeatures/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/PropertyFeatures/Delete/5
        public ActionResult Delete(long propertyid, int imageposition)
        {
            return View();
        }

        // POST: Admin/PropertyFeatures/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreatePropertyImageGallery(long? id)
        {
            Session["id"] = id;
            ViewBag.IntroductionID = TempData["id"];
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            GalleryList objBannersViewModel = new GalleryList()
            {
                GalleryBanners = propertyImageService.GetPropertyImages((long)id).ToList(),

            };
            //Gallery=ClsShoeImages
            return View(objBannersViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePropertyImageGallery(FormCollection form, string bannertype, DateTime UploadedOn)
        {
            long PropertyID = (long)Session["id"];

            string ErrorMessage = string.Empty;
            OzoneImage objBannersViewModel = new OzoneImage();
            var Position = propertyImageService.GalleyPropertyCount(PropertyID);

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

                            PropertyImage ObjModelGalleryImage = new PropertyImage();
                            ObjModelGalleryImage.PropertyID = PropertyID;
                            ObjModelGalleryImage.Image = FilePath;
                            ObjModelGalleryImage.UploadedOn = UploadedOn;
                            ObjModelGalleryImage.Position = ++Position;
                            //ObjModelShoeImage.Thumbnail = ImagesCount++ == 0 ? true : false;
                            string msg = null;

                            if (propertyImageService.Create(ObjModelGalleryImage, ref msg))
                            {
                                try
                                {
                                    MainDirectory = Path.Combine(Server.MapPath("~" + FilePath));
                                    file.SaveAs(MainDirectory);
                                    TempData["SuccessMessage"] = "Project images added successfully ...";
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

            return RedirectToAction("CreatePropertyImageGallery");

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
                    var ItemImages = propertyImageService.SaveItemImagePosition(item.ID, item.Position, ref ErrorMessage);
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
            PropertyImage bannerData = propertyImageService.GetPropertyImage(id);
            string filepath = bannerData.Image;
            if (System.IO.File.Exists(absolutePath + bannerData.Image))
            {
                System.IO.File.Delete(absolutePath + bannerData.Image);
            }
            propertyImageService.DeletePropertyImages(id, ref message, ref filepath);
            TempData["SuccessMessage"] = "Image Deleted Successfully ...";
            long ID = (long)Session["id"];
            return RedirectToAction("CreatePropertyImageGallery", new { id = ID });
        }
    }
}
