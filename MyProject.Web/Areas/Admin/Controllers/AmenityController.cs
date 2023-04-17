

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Helpers;

namespace MyProject.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class AmenityController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        private readonly IAmenityService amenityService;
        public AmenityController(IAmenityService amenityService)
        {
            this.amenityService = amenityService;
        }
        // GET: Admin/Amenitys
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var result = amenityService.GetAmenites();
            return PartialView(result);
        }
        // GET: Admin/Amenitys/Details/5
        public ActionResult Details(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Amenity amenity = this.amenityService.GetAmenity(id);
            if (amenity == null)
            {
                return HttpNotFound();
            }
            return View(amenity);
        }

        // GET: Admin/Amenitys/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,NameAr,Image,Description,DescriptionAr")] Amenity amenity)
        {
            string message = string.Empty;
            string nameappend = amenity.Name.Replace(" ", "-");
            if (ModelState.IsValid)
            {
                if (amenity.Image != null)
                {
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Amenity/Image/{0}/", nameappend);
                    amenity.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
                }
                if (this.amenityService.CreateAmenity(amenity, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Amenity/Index/",
                        message = message,
                        data = new
                        {
                            CreatedOn = amenity.CreatedOn.Value.ToString(CustomHelper.GetDateFormat),
                            amenity.Image,
                            Name = amenity.Name,
                            IsActive = amenity.IsActive.ToString(),
                            ID = amenity.ID
                        }
                    });
                }

            }
            else
            {
                message = "Please fill the form properly ...";
            }
            return Json(new { success = false, message = message });
        }

        // GET: Admin/Amenitys/Edit/5
        public ActionResult Edit(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Amenity amenity = this.amenityService.GetAmenity(id);
            if (amenity == null)
            {
                return HttpNotFound();
            }
            return View(amenity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,NameAr,Image,Description,DescriptionAr,IsActive")] Amenity amenity)
        {
            string message = string.Empty;
            string nameappend = amenity.Name.Replace(" ", "-");
            if (ModelState.IsValid)
            {
                Amenity currentamenity = this.amenityService.GetAmenity(amenity.ID);
                currentamenity.Name = amenity.Name;
                currentamenity.NameAr = amenity.NameAr;

                if (amenity.Image != null)
                {
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Amenity/Image/{0}/", nameappend);
                    if (currentamenity.Image != null)
                    {

                        if (System.IO.File.Exists(absolutePath + currentamenity.Image))
                        {
                            System.IO.File.Delete(absolutePath + currentamenity.Image);
                        }
                    }

                    currentamenity.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
                }
                if (this.amenityService.UpdateAmenity(ref currentamenity, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Amenity/Index/",
                        message = message,
                        data = new
                        {
                            CreatedOn = currentamenity.CreatedOn.Value.ToString(CustomHelper.GetDateFormat),
                            currentamenity.Image,
                            Name = currentamenity.Name,
                            IsActive = currentamenity.IsActive.ToString(),
                            ID = currentamenity.ID
                        }
                    }, JsonRequestBehavior.AllowGet);
                }


            }
            else
            {
                message = "Please fill the form correctly";
            }
            return Json(new { success = false, message = message });
        }

        // GET: Admin/Amenitys/Delete/5
        public ActionResult Delete(long id)
        {
            string messsage = string.Empty;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bool amenity = this.amenityService.DeleteAmenity(id, ref messsage, true); ;
            if (!amenity)
            {
                return HttpNotFound();
            }
            return View(amenity);
        }

        // POST: Admin/Amenitys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bool amenity = this.amenityService.DeleteAmenity(id, ref message, true); ;
            if (amenity)
            {
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);


        }
        public ActionResult Activate(long? id)
        {
            if (id == null || id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var amenity = this.amenityService.GetAmenity((long)id);
            if (amenity == null)
            {
                return HttpNotFound();
            }
            string message = string.Empty;
            if (!(bool)amenity.IsActive)
                amenity.IsActive = true;
            else
            {
                amenity.IsActive = false;
            }
            if (this.amenityService.UpdateAmenity(ref amenity, ref message))
            {
                SuccessMessage = "Amenity " + ((bool)amenity.IsActive ? "activated" : "deactivated") + "  successfully ...";
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        CreatedOn = amenity.CreatedOn.Value.ToString(CustomHelper.GetDateFormat),
                        amenity.Image,
                        Name = amenity.Name,
                        IsActive = amenity.IsActive.ToString(),
                        ID = amenity.ID
                    }
                }, JsonRequestBehavior.AllowGet); ;
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later...";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }
    }
}
