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
    public class VendorSectionController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IVendorSectionService _vendorSectionService;

        public VendorSectionController(IVendorSectionService vendorSectionService)
        {
            this._vendorSectionService = vendorSectionService;

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
            var vendorSections = _vendorSectionService.GetVendorSections();
            return PartialView(vendorSections);

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(VendorSection data)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (_vendorSectionService.CreateVendorSection( ref data, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created vendor section {data.Name}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/VendorSection/Index",
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
            VendorSection vendorSection = _vendorSectionService.GetVendorSection((long)id);
            if (vendorSection == null)
            {
                return HttpNotFound();
            }

            TempData["VendorSectionID"] = id;
            return View(vendorSection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorSection vendorSection)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                long Id;
                if (TempData["VendorSectionID"] != null && Int64.TryParse(TempData["VendorSectionID"].ToString(), out Id) && vendorSection.ID == Id)
                {
                    if (_vendorSectionService.UpdateVendorSection(ref vendorSection, ref message))
                    {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated vendor section {vendorSection.Name}.");
                        return Json(new
                        {
                            success = true,
                            url = "/Admin/VendorSection/Index",
                            message = "Vendor Section updated successfully ...",
                            data = new
                            {
                                Date = vendorSection.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                                Name = vendorSection.Name,
                                IsActive = vendorSection.IsActive.HasValue ? vendorSection.IsActive.Value.ToString() : bool.FalseString,
                                ID = vendorSection.ID
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
            VendorSection vendorSection = _vendorSectionService.GetVendorSection((Int16)id);
            if (vendorSection == null)
            {
                return HttpNotFound();
            }
            TempData["VendorSectionID"] = id;
            return View(vendorSection);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (_vendorSectionService.DeleteVendorSection((Int16)id, ref message))
            {
                VendorSection vendorSection = _vendorSectionService.GetVendorSection((Int16)id);
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted vendor section {vendorSection.Name}.");
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
            var vendorSection = _vendorSectionService.GetVendorSection((long)id);
            if (vendorSection == null)
            {
                return HttpNotFound();
            }

            if (!(bool)vendorSection.IsActive)
                vendorSection.IsActive = true;
            else
            {
                vendorSection.IsActive = false;
            }
            string message = string.Empty;
            if (_vendorSectionService.UpdateVendorSection(ref vendorSection, ref message))
            {
                SuccessMessage = "Vendor Section " + ((bool)vendorSection.IsActive ? "activated" : "deactivated") + "  successfully ...";
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)vendorSection.IsActive ? "activated" : "deactivated")} vendor section {vendorSection.Name}.");
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        Date = vendorSection.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                        Name = vendorSection.Name,
                        IsActive = vendorSection.IsActive.HasValue ? vendorSection.IsActive.Value.ToString() : bool.FalseString,
                        ID = vendorSection.ID
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
            VendorSection vendorSection = _vendorSectionService.GetVendorSection((Int16)id);
            if (vendorSection == null)
            {
                return HttpNotFound();
            }
            return View(vendorSection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VendorSectionReport()
        {
            var getAllTags = _vendorSectionService.GetVendorSections().ToList();
            if (getAllTags.Count() > 0)
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("VendorSectionReport");

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
                    var worksheet = excel.Workbook.Worksheets["VendorSectionReport"];

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

                    return File(excel.GetAsByteArray(), "application/msexcel", "VendorSection Report.xlsx");
                }
            }
            return RedirectToAction("Index");
        }

    }
}