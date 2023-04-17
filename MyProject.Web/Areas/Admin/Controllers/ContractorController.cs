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
    public class ContractorController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        private readonly IContractorService contractorService;
        public ContractorController(IContractorService contractorService)
        {
            this.contractorService = contractorService;
        }
        // GET: Admin/Contractors
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var result = contractorService.GetAll();
            return PartialView(result);
        }
        // GET: Admin/Contractors/Details/5
        public ActionResult Details(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = this.contractorService.Edit(id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            return View(contractor);
        }

        // GET: Admin/Contractors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Contractors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,NameAr,Image,Description,DescriptionAr")] Contractor contractor)
        {
            string message = string.Empty;
            string nameappend = contractor.Name.Replace(" ", "-");
            if (ModelState.IsValid)
            {
                if (contractor.Image != null)
                {
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Contractor/Image/{0}/", nameappend);
                    contractor.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
                }
                if (this.contractorService.Create(contractor, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Contractor/Index/",
                        message = message,
                        data = new
                        {
                            CreatedOn = contractor.CreatedOn.ToString(CustomHelper.GetDateFormat),
                            contractor.Image,
                            Name = contractor.Name,
                            IsActive = contractor.IsActive.ToString(),
                            ID = contractor.ID
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

        // GET: Admin/Contractors/Edit/5
        public ActionResult Edit(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = this.contractorService.Edit(id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            return View(contractor);
        }

        // POST: Admin/Contractors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,NameAr,Image,Description,DescriptionAr,IsActive")] Contractor contractor)
        {
            string message = string.Empty;
            string nameappend = contractor.Name.Replace(" ", "-");
            if (ModelState.IsValid)
            {
                Contractor currentcontractor = this.contractorService.Edit(contractor.ID);
                currentcontractor.Name = contractor.Name;
                currentcontractor.NameAr = contractor.NameAr;
                currentcontractor.Description = contractor.Description;
                currentcontractor.DescriptionAr = contractor.DescriptionAr;

                if (contractor.Image != null)
                {
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Contractor/Image/{0}/", nameappend);
                    if (currentcontractor.Image != null)
                    {

                        if (System.IO.File.Exists(absolutePath + currentcontractor.Image))
                        {
                            System.IO.File.Delete(absolutePath + currentcontractor.Image);
                        }
                    }

                    currentcontractor.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
                }
                if (this.contractorService.Edit(ref currentcontractor, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Contractor/Index/",
                        message = message,
                        data = new
                        {
                            CreatedOn = currentcontractor.CreatedOn.ToString(CustomHelper.GetDateFormat),
                            contractor.Image,
                            Name = currentcontractor.Name,
                            IsActive = currentcontractor.IsActive.ToString(),
                            ID = currentcontractor.ID
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

        // GET: Admin/Contractors/Delete/5
        public ActionResult Delete(long id)
        {
            string messsage = string.Empty;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bool contractor = this.contractorService.Delete(id, ref messsage, true); ;
            if (!contractor)
            {
                return HttpNotFound();
            }
            return View(contractor);
        }

        // POST: Admin/Contractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bool contractor = this.contractorService.Delete(id, ref message, true); ;
            if (contractor)
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

            var contractor = this.contractorService.Edit((long)id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            string message = string.Empty;
            if (!(bool)contractor.IsActive)
                contractor.IsActive = true;
            else
            {
                contractor.IsActive = false;
            }
            if (this.contractorService.Edit(ref contractor, ref message))
            {
                SuccessMessage = "Contractor " + ((bool)contractor.IsActive ? "activated" : "deactivated") + "  successfully ...";
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        CreatedOn = contractor.CreatedOn.ToString(CustomHelper.GetDateFormat),
                        contractor.Image,
                        Name = contractor.Name,
                        IsActive = contractor.IsActive.ToString(),
                        ID = contractor.ID
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
