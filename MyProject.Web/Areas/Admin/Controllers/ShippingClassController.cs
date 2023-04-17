using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Project.Web.Helpers.POCO;
using System.ComponentModel.DataAnnotations;
using OfficeOpenXml;


namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class ShippingClassController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly IShippingClassService _shippingClassService;

        public ShippingClassController(IShippingClassService shippingClassService)
        {
            this._shippingClassService = shippingClassService;
        }

        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.ExcelUploadErrorMessage = TempData["ExcelUploadErrorMessage"];

            return View();
        }

        public ActionResult List()
        {
            var shippingClasses = _shippingClassService.GetShippingClasses();
            return PartialView(shippingClasses);
        }

        public ActionResult ListReport()
        {
            var shippingClasses = _shippingClassService.GetShippingClasses();
            return View(shippingClasses);
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingClass shippingClass = _shippingClassService.GetShippingClass((long)id);
            if (shippingClass == null)
            {
                return HttpNotFound();
            }
            return View(shippingClass);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShippingClass shippingClass)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (_shippingClassService.CreateShippingClass(shippingClass, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/ShippingClass/Index",
                        message = message,
                        data = new
                        {
                            ID = shippingClass.ID,
                            Date = shippingClass.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                            Name = shippingClass.Name,
                            Slug = shippingClass.Slug,
                            IsActive = shippingClass.IsActive.HasValue ? shippingClass.IsActive.Value.ToString() : bool.FalseString
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
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingClass shippingClass = _shippingClassService.GetShippingClass((long)id);
            if (shippingClass == null)
            {
                return HttpNotFound();
            }

            TempData["ShippingClassID"] = id;
            return View(shippingClass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShippingClass shippingClass)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                long Id;
                if (TempData["ShippingClassID"] != null && Int64.TryParse(TempData["ShippingClassID"].ToString(), out Id) && shippingClass.ID == Id)
                {
                    if (_shippingClassService.UpdateShippingClass(ref shippingClass, ref message))
                    {
                        TempData.Keep("ShippingClassID");

                        return Json(new
                        {
                            success = true,
                            url = "/Admin/ShippingClass/Index",
                            message = message,
                            data = new
                            {
                                ID = shippingClass.ID,
                                Date = shippingClass.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                                Name = shippingClass.Name,
                                Slug = shippingClass.Slug,
                                IsActive = shippingClass.IsActive.HasValue ? shippingClass.IsActive.Value.ToString() : bool.FalseString
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
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var shippingClass = _shippingClassService.GetShippingClass((long)id);
                if (shippingClass == null)
                {
                    return HttpNotFound();
                }

                if (!(bool)shippingClass.IsActive)
                    shippingClass.IsActive = true;
                else
                {
                    shippingClass.IsActive = false;
                }
                string message = string.Empty;
                if (_shippingClassService.UpdateShippingClass(ref shippingClass, ref message))
                {
                    SuccessMessage = "Shipping class " + ((bool)shippingClass.IsActive ? "activated" : "deactivated") + "  successfully ...";
                    return Json(new
                    {
                        success = true,
                        message = SuccessMessage,
                        data = new
                        {
                            ID = shippingClass.ID,
                            Date = shippingClass.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                            Name = shippingClass.Name,
                            Slug = shippingClass.Slug,
                            IsActive = shippingClass.IsActive.HasValue ? shippingClass.IsActive.Value.ToString() : bool.FalseString
                        }
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingClass shippingClass = _shippingClassService.GetShippingClass((Int16)id);
            if (shippingClass == null)
            {
                return HttpNotFound();
            }
            TempData["ShippingClassID"] = id;
            return View(shippingClass);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (_shippingClassService.DeleteShippingClass((Int16)id, ref message))
            {
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BulkUpload()
        {

            return View();

        }

        //[HttpPost]
        //public ActionResult BulkUpload(HttpPostedFileBase FileUpload)
        //{
        //    string data = "";
        //    List<string> ErrorItems = new List<string>();
        //    List<string> EmailFailed = new List<string>();

        //    if (FileUpload != null)
        //    {
        //        if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        //        {
        //            string filename = FileUpload.FileName;

        //            if (filename.EndsWith(".xlsx"))
        //            {
        //                string targetpath = Server.MapPath("~/assets/AppFiles/Documents/ExcelFiles");
        //                FileUpload.SaveAs(targetpath + filename);
        //                string pathToExcelFile = targetpath + filename;

        //                string sheetName = "BulkshippingClasses";
        //                var realEstateID = Convert.ToInt64(Session["RealEstateID"]);

        //                int count = 1;
        //                try
        //                {
        //                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
        //                    var tenants = from a in excelFile.Worksheet<shippingClassesWorkSheet>(sheetName) select a;
        //                    foreach (var item in tenants)
        //                    {
        //                        var results = new List<ValidationResult>();
        //                        var context = new ValidationContext(item, null, null);
        //                        if (Validator.TryValidateObject(item, context, results))
        //                        {
        //                            if (_shippingClassService.PostExcelData(item.AreaName, item.MinOrder, item.Charges))
        //                            {
        //                                //Mail ObjMail = new Mail(realEstateID);
        //                                //if (!ObjMail.SendTenantAccountCreationMail(item.Name, item.NameAR, item.Country))
        //                                //{
        //                                //    EmailFailed.Add(item.Email);
        //                                //}
        //                            }
        //                            else
        //                            {
        //                                ErrorItems.Add(string.Format("Row Number {0} Not Inserted.<br>", count));
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ErrorItems.Add(string.Format("<b>Row Number {0} Not Inserted:</b><br>{1}", count, string.Join<string>("<br>", results.Select(i => i.ErrorMessage).ToList())));
        //                        }
        //                        count++;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    TempData["ErrorMessage"] = "Error binding some fields, Please check your excel sheet for null or wrong entries";
        //                    return RedirectToAction("Index");
        //                }



        //                TempData["SuccessMessage"] = string.Format("{0} Charges inserted!", (count - 1) - ErrorItems.Count());

        //                if (ErrorItems.Count() > 0)
        //                {
        //                    TempData["ErrorMessage"] = string.Format("{0} Charges not inserted!", ErrorItems.Count());
        //                    TempData["ExcelUploadErrorMessage"] = string.Join<string>("<br>", ErrorItems);
        //                }
        //                return RedirectToAction("Index");
        //            }

        //            TempData["ErrorMessage"] = "Invalid file format, Only .xlsx format is allowed";
        //        }

        //        TempData["ErrorMessage"] = "Invalid file format, Only Excel file is allowed";
        //    }

        //    TempData["ErrorMessage"] = "Please upload Excel file first";
        //    return RedirectToAction("Index");
        //}
     
    }
}