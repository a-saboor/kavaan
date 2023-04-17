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
    public class VendorTypeController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IVendorTypeService _vendorTypeService;

        public VendorTypeController(IVendorTypeService vendorTypeService)
        {
            this._vendorTypeService = vendorTypeService;

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
            var vendorTypes = _vendorTypeService.GetVendorTypes();
            return PartialView(vendorTypes);

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(VendorType data)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (_vendorTypeService.CreateVendorType( ref data, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created vendor type {data.Name}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/VendorType/Index",
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
            VendorType vendorType = _vendorTypeService.GetVendorType((long)id);
            if (vendorType == null)
            {
                return HttpNotFound();
            }

            TempData["VendorTypeID"] = id;
            return View(vendorType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorType vendorType)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                long Id;
                if (TempData["VendorTypeID"] != null && Int64.TryParse(TempData["VendorTypeID"].ToString(), out Id) && vendorType.ID == Id)
                {
                    if (_vendorTypeService.UpdateVendorType(ref vendorType, ref message))
                    {
                        log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated vendor type {vendorType.Name}.");
                        return Json(new
                        {
                            success = true,
                            url = "/Admin/VendorType/Index",
                            message = "Vendor Type updated successfully ...",
                            data = new
                            {
                                Date = vendorType.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                                Name = vendorType.Name,
                                IsActive = vendorType.IsActive.HasValue ? vendorType.IsActive.Value.ToString() : bool.FalseString,
                                ID = vendorType.ID
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
            VendorType vendorType = _vendorTypeService.GetVendorType((Int16)id);
            if (vendorType == null)
            {
                return HttpNotFound();
            }
            TempData["vendorTypeID"] = id;
            return View(vendorType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (_vendorTypeService.DeleteVendorType((Int16)id, ref message))
            {
                VendorType vendorType = _vendorTypeService.GetVendorType((Int16)id);
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted vendor type {vendorType.Name}.");
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
            var vendorType = _vendorTypeService.GetVendorType((long)id);
            if (vendorType == null)
            {
                return HttpNotFound();
            }

            if (!(bool)vendorType.IsActive)
                vendorType.IsActive = true;
            else
            {
                vendorType.IsActive = false;
            }
            string message = string.Empty;
            if (_vendorTypeService.UpdateVendorType(ref vendorType, ref message))
            {
                SuccessMessage = "Vendor Type " + ((bool)vendorType.IsActive ? "activated" : "deactivated") + "  successfully ...";
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)vendorType.IsActive ? "activated" : "deactivated")} vendor type {vendorType.Name}.");
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        Date = vendorType.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                        Name = vendorType.Name,
                        IsActive = vendorType.IsActive.HasValue ? vendorType.IsActive.Value.ToString() : bool.FalseString,
                        ID = vendorType.ID
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
            VendorType vendorType = _vendorTypeService.GetVendorType((Int16)id);
            if (vendorType == null)
            {
                return HttpNotFound();
            }
            return View(vendorType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VendorTypeReport()
        {
            var getAllTags = _vendorTypeService.GetVendorTypes().ToList();
            if (getAllTags.Count() > 0)
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("VendorTypesReport");

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
                    var worksheet = excel.Workbook.Worksheets["VendorTypesReport"];

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

                    return File(excel.GetAsByteArray(), "application/msexcel", "Vendor types Report.xlsx");
                }
            }
            return RedirectToAction("Index");
        }

    }
}