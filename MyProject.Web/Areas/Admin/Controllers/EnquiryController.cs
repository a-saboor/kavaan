using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Helpers;

namespace MyProject.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class EnquiryController : Controller
    {
        private readonly IEnquiryService _enquiryService;
        public EnquiryController(IEnquiryService enquiryService)
        {
            this._enquiryService = enquiryService;
        }

        public ActionResult Index()
        {
            ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
            ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-365).ToString("MM/dd/yyyy");
            return View();
        }

        public ActionResult List()
        {
            DateTime ToDate = Helpers.TimeZone.GetLocalDateTime();
            DateTime fromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-365);
            IEnumerable<Enquiry> enquiries = this._enquiryService.GetCustomerEnquiry(fromDate, ToDate).ToList();
            return PartialView(enquiries);
        }

        [HttpPost]
        public ActionResult List(DateTime fromDate, DateTime ToDate)
        {
            IEnumerable<Enquiry> enquiries = this._enquiryService.GetCustomerEnquiry(fromDate, ToDate.AddMinutes(1439));
            return PartialView(enquiries);
        }

        public ActionResult ContactEnquiry()
        {
            ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
            ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-365).ToString("MM/dd/yyyy");
            return View();
        }
        public ActionResult ContactEnquiryList()
        {
            DateTime ToDate = Helpers.TimeZone.GetLocalDateTime();
            DateTime fromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-365);
            IEnumerable<Enquiry> enquiries = this._enquiryService.GetContactEnquiry(fromDate, ToDate);
            return PartialView(enquiries);
        }

        [HttpPost]
        public ActionResult ContactEnquiryList(DateTime fromDate, DateTime ToDate)
        {
            IEnumerable<Enquiry> enquiries = this._enquiryService.GetContactEnquiry(fromDate, ToDate.AddMinutes(1439));
            return PartialView(enquiries);
        }

        public ActionResult Message(long id)
        {
            Enquiry enquiry = this._enquiryService.GetEnquiryById(id);
            return View(enquiry);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(DateTime? FromDate, DateTime? ToDate)
        {
            DateTime EndDate = ToDate.Value.AddMinutes(1439);
            var getAllTransactions = this._enquiryService.GetContactEnquiry(FromDate.Value, EndDate).ToList();
            if (getAllTransactions.Count() > 0)
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("ContactEnquiryReport");

                    var headerRow = new List<string[]>()
                    {
                    new string[] {
                        "Creation Date"
                        ,"Full Name"
                        ,"Contact"
                        ,"Email"
                        ,"Message"
                        }
                    };

                    // Determine the header range (e.g. A1:D1)
                    string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    // Target a worksheet
                    var worksheet = excel.Workbook.Worksheets["ContactEnquiryReport"];

                    // Popular header row data
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    if (getAllTransactions.Count != 0)
                        getAllTransactions = getAllTransactions.OrderByDescending(x => x.ID).ToList();

                    foreach (var i in getAllTransactions)
                    {
                        cellData.Add(new object[] {
                        i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString(CustomHelper.GetDateFormat) : "-"
                        ,!string.IsNullOrEmpty(i.FullName) ? i.FullName :"-"
                        ,!string.IsNullOrEmpty(i.Contact) ? i.Contact :"-"
                        ,!string.IsNullOrEmpty(i.Email) ? i.Email:"-"
                        ,!string.IsNullOrEmpty(i.Message) ? i.Message:"-"
						});
                    }

                    worksheet.Cells[2, 1].LoadFromArrays(cellData);

                    return File(excel.GetAsByteArray(), "application/msexcel", "Contact Enquiry Report.xlsx");
                }
            }
            return RedirectToAction("ContactEnquiry");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerReport(DateTime? FromDate, DateTime? ToDate)
        {
            DateTime EndDate = ToDate.Value.AddMinutes(1439);
            var getAllTransactions = this._enquiryService.GetCustomerEnquiry(FromDate.Value, EndDate).ToList();
            if (getAllTransactions.Count() > 0)
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("CustomerEnquiryReport");

                    var headerRow = new List<string[]>()
                    {
                    new string[] {
                        "Creation Date"
                        ,"Full Name"
                        ,"Contact"
                        ,"Email"
                        }
                    };

                    // Determine the header range (e.g. A1:D1)
                    string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    // Target a worksheet
                    var worksheet = excel.Workbook.Worksheets["CustomerEnquiryReport"];

                    // Popular header row data
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    if (getAllTransactions.Count != 0)
                        getAllTransactions = getAllTransactions.OrderByDescending(x => x.ID).ToList();

                    foreach (var i in getAllTransactions)
                    {
                        cellData.Add(new object[] {
                        i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString(CustomHelper.GetDateFormat) : "-"
                        ,!string.IsNullOrEmpty(i.FullName) ? i.FullName :"-"
                        ,!string.IsNullOrEmpty(i.Contact) ? i.Contact :"-"
                        ,!string.IsNullOrEmpty(i.Email) ? i.Email:"-"
                        });
                    }

                    worksheet.Cells[2, 1].LoadFromArrays(cellData);

                    return File(excel.GetAsByteArray(), "application/msexcel", "Customer Enquiry Report.xlsx");
                }
            }
            return RedirectToAction("Index");
        }
    }
}