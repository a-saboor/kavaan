using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using OfficeOpenXml;
using Project.Web.Helpers;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class VendorIndustryController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IVendorIndustryService _vendorIndustryService;

        public VendorIndustryController(IVendorIndustryService vendorIndustryService)
        {
            this._vendorIndustryService = vendorIndustryService;

        }
      
        // GET: Admin/Tags
        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.ExcelUploadErrorMessage = TempData["ExcelUploadErrorMessage"];
            return View();
        }
        public ActionResult List()
        {
            var tags = _vendorIndustryService.GetVendorIndustrys();
            return PartialView(tags);

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(VendorIndustry data)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (_vendorIndustryService.CreateVendorIndustry( ref data, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created vendor industry {data.Name}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/VendorIndustry/Index",
                        message = message,
                        data = new
                        {
                            Date = data.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                            Name = data.Name,
                            IsActive = data.IsActive.HasValue ? data.IsActive.Value.ToString() : bool.FalseString,
                            ID = data.ID
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
            VendorIndustry vendorIndustry = _vendorIndustryService.GetVendorIndustry((long)id);
            if (vendorIndustry == null)
            {
                return HttpNotFound();
            }

            TempData["VendorIndustryID"] = id;
            return View(vendorIndustry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorIndustry vendorIndustry)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                long Id;
                if (TempData["VendorIndustryID"] != null && Int64.TryParse(TempData["VendorIndustryID"].ToString(), out Id) && vendorIndustry.ID == Id)
                {
                    if (_vendorIndustryService.UpdateVendorIndustry(ref vendorIndustry, ref message))
                    {
                        log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated vendor industry {vendorIndustry.Name}.");
                        return Json(new
                        {
                            success = true,
                            url = "/Admin/Brands/Index",
                            message = "Vendor Industry updated successfully ...",
                            data = new
                            {
                                Date = vendorIndustry.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                                Name = vendorIndustry.Name,
                                IsActive = vendorIndustry.IsActive.HasValue ? vendorIndustry.IsActive.Value.ToString() : bool.FalseString,
                                ID = vendorIndustry.ID
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
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VendorIndustry vendorIndustry = _vendorIndustryService.GetVendorIndustry((Int16)id);
            if (vendorIndustry == null)
            {
                return HttpNotFound();
            }
            TempData["tagID"] = id;
            return View(vendorIndustry);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (_vendorIndustryService.DeleteVendorIndustry((Int16)id, ref message))
            {
                VendorIndustry vendorIndustry = _vendorIndustryService.GetVendorIndustry((Int16)id);
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted vendor industry {vendorIndustry.Name}.");
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Activate(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vendorIndustry = _vendorIndustryService.GetVendorIndustry((long)id);
            if (vendorIndustry == null)
            {
                return HttpNotFound();
            }

            if (!(bool)vendorIndustry.IsActive)
                vendorIndustry.IsActive = true;
            else
            {
                vendorIndustry.IsActive = false;
            }
            string message = string.Empty;
            if (_vendorIndustryService.UpdateVendorIndustry(ref vendorIndustry, ref message))
            {
                SuccessMessage = "Vendor Industry " + ((bool)vendorIndustry.IsActive ? "activated" : "deactivated") + "  successfully ...";
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)vendorIndustry.IsActive ? "activated" : "deactivated")} vendor industry {vendorIndustry.Name}.");
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        Date = vendorIndustry.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                        Name = vendorIndustry.Name,
                        IsActive = vendorIndustry.IsActive.HasValue ? vendorIndustry.IsActive.Value.ToString() : bool.FalseString,
                        ID = vendorIndustry.ID
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later.";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VendorIndustry vendorIndustry = _vendorIndustryService.GetVendorIndustry((Int16)id);
            if (vendorIndustry == null)
            {
                return HttpNotFound();
            }
            return View(vendorIndustry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VendorIndustryReport()
        {
            var getAllTags = _vendorIndustryService.GetVendorIndustrys().ToList();
            if (getAllTags.Count() > 0)
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("VendorIndustryReport");

                    var headerRow = new List<string[]>()
                    {
                    new string[] {
                        "Creation Date"
                        ,"Name"
                        ,"Status"
                        }
                    };

                    // Determine the header range (e.g. A1:D1)
                    string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    // Target a worksheet
                    var worksheet = excel.Workbook.Worksheets["VendorIndustryReport"];

                    // Popular header row data
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var i in getAllTags)
                    {
                        cellData.Add(new object[] {
                        i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
                        ,!string.IsNullOrEmpty(i.Name) ? i.Name : "-"
                        ,i.IsActive == true ? "Active" : "InActive"
                        });
                    }

                    worksheet.Cells[2, 1].LoadFromArrays(cellData);

                    return File(excel.GetAsByteArray(), "application/msexcel", "Vendor Industry Report.xlsx");
                }
            }
            return RedirectToAction("Index");
        }

    }
}