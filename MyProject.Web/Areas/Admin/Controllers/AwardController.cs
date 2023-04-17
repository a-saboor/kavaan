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
    public class AwardController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly IAwardService _awardService;
        public AwardController(IAwardService awardService)
        {
            this._awardService = awardService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var result = _awardService.GetAwards();
            return PartialView(result);
        }

        public ActionResult Details(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Award award = _awardService.GetAward(id);
            if (award == null)
            {
                return HttpNotFound();
            }
            return View(award);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string Name , string NameAr , string Description , string DescriptionAr , string Image)
        {
            string message = string.Empty;
            Award award = new Award();
            if (ModelState.IsValid)
            {

                if (Image != null)
                {
                    string nameappend = Name.Replace(" ", "-");
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Award/Image/{0}/", nameappend);
                    Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
                }

               
                award.Title = Name;
                award.TitleAr = NameAr;
                award.Description = Description;
                award.DescriptionAr = DescriptionAr;
                award.Image = Image;
                if (_awardService.CreateAward(award, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Award/Index",
                        message = message,
                        data = new
                        {
                            CreatedOn = award.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                            Name = award.Title,
                            IsActive = award.IsActive.ToString(),
                            ID = award.ID
                        }
                    });
                }

                return RedirectToAction("Index");
            }
            return View(award);
        }

        public ActionResult Edit(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Award award = _awardService.GetAward(id);
            if (award == null)
            {
                return HttpNotFound();
            }
            return View(award);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( long ID , string Name, string NameAr, string Description, string DescriptionAr, string Image )
        {
            string message = string.Empty;
            string replacement = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                Award currentAward = _awardService.GetAward(ID);
                currentAward.Title = Name;
                currentAward.TitleAr = NameAr;
                currentAward.Description = Description;
                currentAward.DescriptionAr = DescriptionAr;

                if (Image != null)
                {
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Award/Image/{0}/", replacement.Replace(" ", "_"));
                    if (currentAward.Image != null)
                    {

                        if (System.IO.File.Exists(absolutePath + currentAward.Image))
                        {
                            System.IO.File.Delete(absolutePath + currentAward.Image);
                        }
                    }
                    currentAward.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
                }

                if (_awardService.UpdateAward(ref currentAward, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Award/Index",
                        message = message,
                        data = new
                        {
                            CreatedOn = currentAward.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                            Name = currentAward.Title,
                            IsActive = currentAward.IsActive.ToString(),
                            ID = currentAward.ID
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
            bool Award = _awardService.DeleteAward(id, ref messsage, true); ;
            if (!Award)
            {
                return HttpNotFound();
            }
            return View(Award);
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
            bool Award = _awardService.DeleteAward(id, ref message, true); ;
            if (Award)
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

            var Award = _awardService.GetAward((long)id);
            if (Award == null)
            {
                return HttpNotFound();
            }
            string message = string.Empty;
            if (!(bool)Award.IsActive)
                Award.IsActive = true;
            else
            {
                Award.IsActive = false;
            }
            if (_awardService.UpdateAward(ref Award, ref message))
            {
                SuccessMessage = "Award " + ((bool)Award.IsActive ? "activated" : "deactivated") + "  successfully ...";
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        CreatedOn = Award.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                        Name = Award.Title,
                        IsActive = Award.IsActive.ToString(),
                        ID = Award.ID
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