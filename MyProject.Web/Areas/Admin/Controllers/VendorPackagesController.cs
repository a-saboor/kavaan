using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using Project.Web.Helpers.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class VendorPackagesController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IVendorPackagesService _VendorPackagesService;
        public VendorPackagesController(IVendorPackagesService VendorPackagesService)
        {
            _VendorPackagesService = VendorPackagesService;
        }
        // GET: Admin/Newsfeed
        string Message = string.Empty;
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        bool status = false;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var VendorPackage = _VendorPackagesService.GetVendorPackages();
            return View(VendorPackage);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(VendorPackage data)
        {
            if (ModelState.IsValid)
            {
                VendorPackage record = new VendorPackage();
                record.Name = data.Name;
                record.NameAr = data.NameAr;
                record.Price = data.Price;
                record.Description = data.Description;
                record.DescriptionAr = data.DescriptionAr;
                record.BillingPeriod = data.BillingPeriod;
                record.IsActive = data.IsActive;
                record.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                record.IsDeleted = false;
                

                
                if (_VendorPackagesService.CreateVendorPackages(record, ref Message))
                {
                    status = true;
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created vendor packages {record.Name}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/VendorPackages/Index",
                        message = Message,
                        data = new
                        {
                            CreatedOn = record.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                            Name = record.Name,
                            Description = record.Description,
                            Price = record.Price,
                            BillingPeriod = record.BillingPeriod,
                            IsActive = record.IsActive.ToString(),
                            ID = record.ID
                        }
                    });
                }
            }
            else
            {
                Message = "Please fill the form properly ...";
            }
            return Json(new { success = status, message = Message });
        }
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VendorPackage package = _VendorPackagesService.GetVendorPackagesByID((long)id);
            if (package == null)
            {
                return HttpNotFound();
            }


            return View(package);
        }
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Edit(VendorPackage data)
        {
            string message = string.Empty;

            VendorPackage updatepackage = _VendorPackagesService.GetVendorPackagesByID(data.ID);
            
            if (ModelState.IsValid)
            {
                
                updatepackage.Name = data.Name;
                updatepackage.NameAr = data.NameAr;
                updatepackage.Price = data.Price;
                updatepackage.Description = data.Description;
                updatepackage.DescriptionAr = data.DescriptionAr;
                updatepackage.BillingPeriod = data.BillingPeriod;

                if (_VendorPackagesService.UpdateVendorPackages(ref updatepackage, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated vendor packages {updatepackage.Name}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/VendorPackages/Index",
                        message = message,
                        data = new
                        {
                            CreatedOn = updatepackage.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                            Name = updatepackage.Name,
                            Description = updatepackage.Description,
                            Price = updatepackage.Price,
                            BillingPeriod = updatepackage.BillingPeriod,
                            IsActive = updatepackage.IsActive.ToString(),
                            ID = updatepackage.ID
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                message = "Please fill the form properly ...";
            }
            return Json(new { message = message, success = false });
        }
        public ActionResult Activate(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Package = _VendorPackagesService.GetVendorPackagesByID((long)id);
            if (Package == null)
            {
                return HttpNotFound();
            }

            if (!(bool)Package.IsActive)
                Package.IsActive = true;
            else
            {
                Package.IsActive = false;
            }
            string message = string.Empty;
            if (_VendorPackagesService.UpdateVendorPackages(ref Package, ref message))
            {
                SuccessMessage = "Vendor Package " + ((bool)Package.IsActive ? "activated" : "deactivated") + "  successfully ...";
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} { ((bool)Package.IsActive ? "activated" : "deactivated")} vendor packages {Package.Name}.");
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        CreatedOn = Package.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                        Name = Package.Name,
                        Description = Package.Description,
                        Price = Package.Price,
                        BillingPeriod = Package.BillingPeriod,
                        IsActive = Package.IsActive.ToString(),
                        ID = Package.ID
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later...";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VendorPackage package = _VendorPackagesService.GetVendorPackagesByID((Int16)id);

            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VendorPackage package = _VendorPackagesService.GetVendorPackagesByID((Int16)id);
            if (package == null)
            {
                return HttpNotFound();
            }
            TempData["ID"] = id;
            return View(package);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id, bool softDelete = true)
        {
            VendorPackage package = _VendorPackagesService.GetVendorPackagesByID((Int16)id);
            string message = string.Empty;
            if (softDelete)
            {
                //soft delete of data updating delete column
                if (_VendorPackagesService.DeleteVendorPackages((Int16)id, ref message, softDelete))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted vendor packages {package.Name}.");
                    return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                //permenant delete of data
                if (_VendorPackagesService.DeleteVendorPackages((Int16)id, ref message, softDelete))
                {
                    return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}