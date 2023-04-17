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

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class VendorSuggestionController : Controller
    {
        private readonly IVendorSuggestionService _vendorSuggestionService;
        public VendorSuggestionController(IVendorSuggestionService vendorSuggestionService)
        {
            this._vendorSuggestionService = vendorSuggestionService;
        }

        // GET: Admin/Suggestion
        public ActionResult Index()
        {
            ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
            ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-7).ToString("MM/dd/yyyy");
            return View();
        }
        public ActionResult List()
        {
            var subscribers = _vendorSuggestionService.GetVendorSuggestions();
            return PartialView(subscribers);
        }
        public ActionResult SuggestionList()
        {
            var suggestions = _vendorSuggestionService.GetVendorSuggestions();
            return PartialView(suggestions);
        }

        [HttpPost]
        public ActionResult List(DateTime fromDate, DateTime ToDate)
        {
            DateTime EndDate = ToDate.AddMinutes(1439);
            var Suggestions = _vendorSuggestionService.GetVendorSuggestionDateWise(fromDate, EndDate);
            return PartialView(Suggestions);
        }
    }
}