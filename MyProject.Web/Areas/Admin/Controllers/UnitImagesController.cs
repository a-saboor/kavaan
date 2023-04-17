using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Project.Web.Areas.Admin.ViewModels.UnitImage;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using System.IO;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class UnitImagesController : Controller
    {

        private readonly IUnitImageService unitImageService;
        private readonly IUnitService unitService;

        public UnitImagesController(IUnitImageService unitImageService, IUnitService unitService)
        {
            this.unitImageService = unitImageService;
            this.unitService = unitService;
        }
        // GET: Admin/Newsfeed
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;




        public ActionResult Create(int unitid)
        {

            var unit = this.unitService.GetUnit(unitid);


            TempData["UnitID"] = unit.ID;
            TempData["UnitTitle"] = unit.Title;

            List<UnitImage> unitImages = this.unitImageService.GetUnitImages(unitid).ToList();
            return PartialView(unitImages);

        }
        // POST: Admin/PrpertyImages/Create 
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateImages(UnitImage unitImage, long unitid, int imageposition,string unittitle)
        {

          
                List<UnitImage> unitImages = new List<UnitImage>();
            string message = string.Empty;
            try
            {

                if (Request.Files.Count > 0)
                {

                    string absolutePath = Server.MapPath("~");
                    string prefix = unittitle.Replace("?", "");
                    string relativePath = string.Format("/Assets/AppFiles/UnitImages/Image/{0}/", prefix);
                    unitImage.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, imageposition.ToString());
                    unitImage.Position = imageposition;
                    unitImage.UnitD = unitid;
                   var unitimagedb = this.unitImageService.GetUnitImageByPosition(unitid, imageposition);
                    if (unitimagedb != null)
                    {
                        if (unitimagedb.Image != null)
                        {

                            if (System.IO.File.Exists(absolutePath + unitimagedb.Image))
                            {
                                System.IO.File.Delete(absolutePath + unitimagedb.Image);
                            }
                        }
                        unitimagedb.Image = unitImage.Image;
                        if (this.unitImageService.UpdateUnitImage(ref unitimagedb, ref message))
                        {
                            return Json(new
                            {
                                success = true,
                                url = "/Admin/Unit/Index",
                                message = message


                            }, JsonRequestBehavior.AllowGet); ;
                        }
                    }
                    else
                    {
                        if (this.unitImageService.Create(unitImage, ref message))
                        {

                            return Json(new
                            {
                                success = true,
                                url = "/Admin/Unit/Index",
                                message = message


                            }, JsonRequestBehavior.AllowGet); ;
                        }
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
        public ActionResult Edit(long unitid)
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

        public ActionResult CreateUnitImageGallery(long? id)
        {
            Session["id"] = id;
            ViewBag.IntroductionID = TempData["id"];
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            GalleryList objBannersViewModel = new GalleryList()
            {
                GalleryBanners = unitImageService.GetUnitImages((long)id).ToList(),

            };
            //Gallery=ClsShoeImages
            return View(objBannersViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUnitImageGallery(FormCollection form, string bannertype)
        {
            long UnitID = (long)Session["id"];

            string ErrorMessage = string.Empty;
            UnitImage objBannersViewModel = new UnitImage();
            var Position = unitImageService.GalleyUnitCount(UnitID);

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

                            UnitImage ObjModelGalleryImage = new UnitImage();
                            ObjModelGalleryImage.UnitD = UnitID;
                            ObjModelGalleryImage.Image = FilePath;
                            ObjModelGalleryImage.Position = ++Position;
                            //ObjModelShoeImage.Thumbnail = ImagesCount++ == 0 ? true : false;
                            string msg = null;

                            if (unitImageService.Create(ObjModelGalleryImage, ref msg))
                            {
                                try
                                {
                                    MainDirectory = Path.Combine(Server.MapPath("~" + FilePath));
                                    file.SaveAs(MainDirectory);
                                    TempData["SuccessMessage"] = "Unit images added successfully ...";
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

            return RedirectToAction("CreateUnitImageGallery");

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
                    var ItemImages = unitImageService.SaveItemImagePosition(item.ID, item.Position, ref ErrorMessage);
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
            UnitImage bannerData = unitImageService.GetUnitImage(id);
            string filepath = bannerData.Image;
            if (System.IO.File.Exists(absolutePath + bannerData.Image))
            {
                System.IO.File.Delete(absolutePath + bannerData.Image);
            }
            unitImageService.DeleteUnitImages(id, ref message, ref filepath);
            TempData["SuccessMessage"] = "Image deleted successfully ...";
            long ID = (long)Session["id"];
            return RedirectToAction("CreateUnitImageGallery", new { id = ID });
        }
    }
}
