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
    public class ConsultantController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        private readonly IConsultantService consultantService;
        public ConsultantController(IConsultantService consultantService)
        {
            this.consultantService = consultantService;
        }
        // GET: Admin/Consultants
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var result = this.consultantService.GetAll();
            return PartialView(result);
        }
        // GET: Admin/Consultants/Details/5
        public ActionResult Details(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultant Consultant = this.consultantService.Edit(id);
            if (Consultant == null)
            {
                return HttpNotFound();
            }
            return View(Consultant);
        }

        // GET: Admin/Consultants/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Consultants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,NameAr,Image,Description,DescriptionAr")] Consultant consultant)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (consultant.Image != null)
                {
                    string nameappend = consultant.Name.Replace(" ", "-");
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Consultant/Image/{0}/", nameappend);
                    consultant.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
                }
                if (this.consultantService.Create(consultant, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Consultants/Index",
                        message = message,
                        data = new
                        {
                            CreatedOn = consultant.CreatedOn.ToString("dd MMM yyyy, h:mm tt"),
                            Name = consultant.Name,
                            IsActive = consultant.IsActive.ToString(),
                            ID = consultant.ID
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

        // GET: Admin/Consultants/Edit/5
        public ActionResult Edit(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultant Consultant = this.consultantService.Edit(id);
            if (Consultant == null)
            {
                return HttpNotFound();
            }
            return View(Consultant);
        }

        // POST: Admin/Consultants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,NameAr,Image,Description,DescriptionAr,IsActive")] Consultant Consultant)
        {
            string message = string.Empty;

            if (ModelState.IsValid)
            {
                Consultant currentConsultant = this.consultantService.Edit(Consultant.ID);
                currentConsultant.Name = Consultant.Name;
                currentConsultant.NameAr = Consultant.NameAr;
                currentConsultant.Description = Consultant.Description;
                currentConsultant.DescriptionAr = Consultant.DescriptionAr;

                if (Consultant.Image != null)
                {
                    string nameappend = Consultant.Name.Replace(" ", "-");
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Consultant/Image/{0}/", nameappend);
                    if (currentConsultant.Image != null)
                    {

                        if (System.IO.File.Exists(absolutePath + currentConsultant.Image))
                        {
                            System.IO.File.Delete(absolutePath + currentConsultant.Image);
                        }
                    }
                    currentConsultant.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
                }

                if (this.consultantService.Edit(ref currentConsultant, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Consultant/Index",
                        message = message,
                        data = new
                        {
                            CreatedOn = currentConsultant.CreatedOn.ToString("dd MMM yyyy, h:mm tt"),
                            Name = currentConsultant.Name,
                            IsActive = currentConsultant.IsActive.ToString(),
                            ID = currentConsultant.ID
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

        // GET: Admin/Consultants/Delete/5
        public ActionResult Delete(long id)
        {
            string messsage = string.Empty;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bool Consultant = this.consultantService.Delete(id, ref messsage, true); ;
            if (!Consultant)
            {
                return HttpNotFound();
            }
            return View(Consultant);
        }

        // POST: Admin/Consultants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bool Consultant = this.consultantService.Delete(id, ref message, true); ;
            if (Consultant)
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

            var Consultant = this.consultantService.Edit((long)id);
            if (Consultant == null)
            {
                return HttpNotFound();
            }
            string message = string.Empty;
            if (!(bool)Consultant.IsActive)
                Consultant.IsActive = true;
            else
            {
                Consultant.IsActive = false;
            }
            if (this.consultantService.Edit(ref Consultant, ref message))
            {
                SuccessMessage = "Consultant " + ((bool)Consultant.IsActive ? "activated" : "deactivated") + "  successfully ...";
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        CreatedOn = Consultant.CreatedOn.ToString("dd MMM yyyy, h:mm tt"),
                        Name = Consultant.Name,
                        IsActive = Consultant.IsActive.ToString(),
                        ID = Consultant.ID
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
