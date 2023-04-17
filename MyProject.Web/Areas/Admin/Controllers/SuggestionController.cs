using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using MyProject.Web.Helpers.POCO;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class SuggestionController : Controller
    {
        private readonly ICustomerSuggestionService _customerSuggestionService;
        public SuggestionController(ICustomerSuggestionService customerSuggestionService)
        {
            this._customerSuggestionService = customerSuggestionService;
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
            var subscribers = _customerSuggestionService.GetCustomerSuggestions();
            return PartialView(subscribers);
        }
        public ActionResult SuggestionList()
        {
            var suggestions = _customerSuggestionService.GetCustomerSuggestions();
            return PartialView(suggestions);
        }

        [HttpPost]
        public ActionResult List(DateTime fromDate, DateTime ToDate)
        {
            DateTime EndDate = ToDate.AddMinutes(1439);
            var Suggestions = _customerSuggestionService.GetCustomerSuggestionDateWise(fromDate, EndDate);
            return PartialView(Suggestions);
        }
    }
}