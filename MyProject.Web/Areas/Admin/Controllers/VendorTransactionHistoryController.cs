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
    public class VendorTransactionHistoryController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        private readonly IVendorService _vendorService;
        private readonly IVendorTransactionHistoryService _vendorTransactionHistoryService;

        public VendorTransactionHistoryController(IVendorService vendorService, IVendorTransactionHistoryService vendorTransactionHistoryService)
        {
            this._vendorService = vendorService;
            this._vendorTransactionHistoryService = vendorTransactionHistoryService;
        }


        public ActionResult Index(long id)
        {
            TempData["VendorID"] = id;
            return View();
        }


        public ActionResult List()
        {
            long VendorID = (long)TempData["VendorID"];
            var vendor = _vendorService.GetVendor(VendorID);
            ViewBag.VendorName = vendor != null ? vendor.Name : "-";
            var vendorTransaction = _vendorTransactionHistoryService.GetVendorTransactionHistoriesByVendor(VendorID);
            return View(vendorTransaction);
        }
    }
}