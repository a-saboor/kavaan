using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class ProgressController : Controller
    {
        string SuccessMessage = string.Empty;
        string ErrorMessage = string.Empty;
        private readonly IProgressService _progressService;
        public ProgressController(IProgressService progressService)
        {
            this._progressService = progressService;
           
        }
        // GET: Admin/Progress
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var progress = _progressService.GetProgresses();
            return PartialView(progress);
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Progress progress = _progressService.GetProgress((Int16)id);
            if (progress == null)
            {
                return HttpNotFound();
            }
            return View(progress);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Progress progress)
        {
            string message = string.Empty;
            string FImage = string.Empty;
            if (ModelState.IsValid)
            {
                if (_progressService.CreateProgress(progress, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Progress/Index",
                        message = message,
                        data = new
                        {
                            Date = progress.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                            Name = progress.Name,
                            IsActive = progress.IsActive.HasValue ? progress.IsActive.Value.ToString() : bool.FalseString,
                            ID = progress.ID
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

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Progress progress = _progressService.GetProgress((long)id);
            if (progress == null)
            {
                return HttpNotFound();
            }

            TempData["Progress"] = id;
            return View(progress);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Progress progress)
        {
            string FImage = string.Empty;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                long Id;
                if (TempData["Progress"] != null && Int64.TryParse(TempData["Progress"].ToString(), out Id) && progress.ID == Id)
                {
                    Progress currentProgress = _progressService.GetProgress(progress.ID);

                    currentProgress.Name = progress.Name;
                    currentProgress.NameAr = progress.NameAr;
                    


                    if (_progressService.UpdateProgress(ref currentProgress, ref message))
                    {
                        return Json(new
                        {
                            success = true,
                            url = "/Admin/Progress/Index",
                            message = "Progress updated successfully ...",
                            data = new
                            {
                                Date = currentProgress.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                                Name = currentProgress.Name,
                                IsActive = currentProgress.IsActive.HasValue ? currentProgress.IsActive.Value.ToString() : bool.FalseString,
                                ID = currentProgress.ID
                            }
                        });
                    }

                }
                else
                {
                    message = "Oops! Something went wrong. Please try later.";
                }
            }
            else
            {
                message = "Please fill the form properly ...";
            }
            return Json(new { success = false, message = message });
        }

        public ActionResult Activate(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var progress = _progressService.GetProgress((long)id);
            if (progress == null)
            {
                return HttpNotFound();
            }

            if (!(bool)progress.IsActive)
                progress.IsActive = true;
            else
            {
                progress.IsActive = false;
            }
            string message = string.Empty;
            if (_progressService.UpdateProgress(ref progress, ref message))
            {
                SuccessMessage = "Progress " + ((bool)progress.IsActive ? "activated" : "deactivated") + "  successfully ...";
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        Date = progress.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                        Name = progress.Name,
                        IsActive = progress.IsActive.HasValue ? progress.IsActive.Value.ToString() : bool.FalseString,
                        ID = progress.ID
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later...";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Progress type = _progressService.GetProgress((Int16)id);
            if (type == null)
            {
                return HttpNotFound();
            }
            TempData["Progress"] = id;
            return View(type);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (_progressService.DeleteProgress((Int16)id, ref message))
            {
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}