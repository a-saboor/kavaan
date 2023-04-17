using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class PropertyProgressController : Controller

    {
        private readonly IPropertyProgressService propertyProgressService;
        public PropertyProgressController(IPropertyProgressService propertyProgressService)
        {
            this.propertyProgressService = propertyProgressService;
        }
        // GET: Admin/PropertyProgress
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/PropertyProgress/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/PropertyProgress/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Admin/PropertyProgress/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/PropertyProgress/Edit/5
        public ActionResult Edit(int propertyid)
        {
            IEnumerable<PropertyProgress> progress = this.propertyProgressService.GetAll(propertyid);
            return View(progress);
        }

        // POST: Admin/PropertyProgress/Edit/5
        [HttpPost]
        public ActionResult Edit(List<PropertyProgress> propertyProgresses)
        {
            string message = String.Empty;
            if (ModelState.IsValid)
            {
                bool insertflag = false;
                foreach (var propertyProgress in propertyProgresses)
                {
                    this.propertyProgressService.Edit(propertyProgress, ref message, true, propertyProgress.ID);

                    insertflag = true;


                }
                if (insertflag)
                {

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Property/Index",
                        message = message,
                    });


                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        url = "/Admin/Property/Index",
                        message = message,
                    });
                }



            }
            else
            {
                message = "Please fill the form properly ...";
            }

            return Json(new { success = false, message = message });
        }

        // GET: Admin/PropertyProgress/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/PropertyProgress/Delete/5
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
    }
}
