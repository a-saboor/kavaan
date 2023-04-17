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
    public class PartnerController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly IPartnerService _partnerService;
        public PartnerController(IPartnerService partnerService)
        {
            this._partnerService = partnerService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var result = _partnerService.GetPartners();
            return PartialView(result);
        }

        public ActionResult Details(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner partner = _partnerService.GetPartner(id);
            if (partner == null)
            {
                return HttpNotFound();
            }
            return View(partner);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,NameAr,Image,Description,DescriptionAr")] Partner partner)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (partner.Image != null)
                {
                    string nameappend = partner.Name.Replace(" ", "-");
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Partner/Image/{0}/", nameappend);
                    partner.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
                }
                if (_partnerService.CreatePartner(partner, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Partner/Index",
                        message = message,
                        data = new
                        {
                            CreatedOn = partner.CreatedOn.HasValue ? partner.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-",
                            Name = partner.Name,
                            IsActive = partner.IsActive.ToString(),
                            ID = partner.ID
                        }
                    });
                }
                return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);

            }
            return View(partner);
        }

        public ActionResult Edit(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner partner = _partnerService.GetPartner(id);
            if (partner == null)
            {
                return HttpNotFound();
            }
            return View(partner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(long ID , string Name , string NameAr , string Description , string DescriptionAr , string Image)
        {
            string message = string.Empty;
            string replacement = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                Partner currentPartner = _partnerService.GetPartner(ID);
                currentPartner.Name = Name;
                currentPartner.NameAr = NameAr;
                currentPartner.Description =Description;
                currentPartner.DescriptionAr = DescriptionAr;

                if (Image != null)
                {
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Partner/Image/{0}/", replacement.Replace(" ", "_"));
                    if (currentPartner.Image != null)
                    {

                        if (System.IO.File.Exists(absolutePath + currentPartner.Image))
                        {
                            System.IO.File.Delete(absolutePath + currentPartner.Image);
                        }
                    }
                    currentPartner.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
                }

                if (_partnerService.UpdatePartner(ref currentPartner, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Partner/Index",
                        message = message,
                        data = new
                        {
                            CreatedOn = currentPartner.CreatedOn.HasValue ? currentPartner.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-",
                            Name = currentPartner.Name,
                            IsActive = currentPartner.IsActive.ToString(),
                            ID = currentPartner.ID
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

        public ActionResult Delete(long id)
        {
            string messsage = string.Empty;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bool partner = _partnerService.DeletePartner(id, ref messsage, true); ;
            if (!partner)
            {
                return HttpNotFound();
            }
            return View(partner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bool partner = _partnerService.DeletePartner(id, ref message, true); ;
            if (partner)
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

            var partner = _partnerService.GetPartner((long)id);
            if (partner == null)
            {
                return HttpNotFound();
            }
            string message = string.Empty;
            if (!(bool)partner.IsActive)
                partner.IsActive = true;
            else
            {
                partner.IsActive = false;
            }
            if (_partnerService.UpdatePartner(ref partner, ref message))
            {
                SuccessMessage = "Partner " + ((bool)partner.IsActive ? "activated" : "deactivated") + "  successfully ...";
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        CreatedOn = partner.CreatedOn.HasValue ? partner.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-",
                        Name = partner.Name,
                        IsActive = partner.IsActive.ToString(),
                        ID = partner.ID
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