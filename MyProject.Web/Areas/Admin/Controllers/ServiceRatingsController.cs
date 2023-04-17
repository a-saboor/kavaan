using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using OfficeOpenXml;
using Project.Web.Helpers.Routing;
using Project.Web.Helpers;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class ServiceRatingsController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IServiceRatingService _serviceRatingsService;

        public ServiceRatingsController(IServiceRatingService tournamantRatingsService)
        {
            this._serviceRatingsService = tournamantRatingsService;
        }
        // GET: Admin/ServiceRatings
        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }

        public ActionResult List()
        {
            var Rating = _serviceRatingsService.GetServiceRatings();
            return PartialView(Rating);
        }
        [HttpPost]
        public ActionResult List(string ApprovalStatus)
        {
            bool status;
            var Rating = _serviceRatingsService.GetServiceRatings();

            if (ApprovalStatus == "1") 
            {
                status = true;
                Rating = _serviceRatingsService.GetServiceRatingsByStatus(status);
                ViewBag.ApprovalStatus = ApprovalStatus;
            }
            else if(ApprovalStatus == "0")
            {
                status = false;
                Rating = _serviceRatingsService.GetServiceRatingsByStatus(status);
                ViewBag.ApprovalStatus = ApprovalStatus;
            }
            
            
            return PartialView(Rating);
        }

        [HttpPost]
        public ActionResult Approval(long id, bool status)
        {
            string message = string.Empty;
            var getdata = _serviceRatingsService.GetServiceRating(id);
            if (status == true)
            {
                getdata.IsApproved = true;
                if (_serviceRatingsService.UpdateServiceRating(ref getdata, ref message))
                {
                    SuccessMessage = "Rating " + ((bool)getdata.IsApproved ? "Approved" : "Rejected") + "  successfully ...";

                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)getdata.IsApproved ? "Approved" : "Rejected")} service rating {getdata.Service.Name}.");
                    return Json(new
                    {
                        success = true,
                        message = SuccessMessage,
                        data = new
                        {
                            Date = getdata.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                            BookingID = getdata.ServiceBookingID,
                            Bookingno = getdata.ServiceBooking.BookingNo,
                            customer = getdata.Customer.UserName,
                            Service = getdata.Service.Name,
                            Rating = getdata.Rating,
                            Remarks = getdata.Review,
                            IsApproved = getdata.IsApproved.HasValue ? getdata.IsApproved.Value.ToString(): false.ToString(),
                            ID = getdata.ID
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorMessage = "Oops! Something went wrong. Please try later.";
                }
            }
            else
            {
                {
                    getdata.IsApproved = false;
                    if (_serviceRatingsService.UpdateServiceRating(ref getdata, ref message))
                    {
                        SuccessMessage = "Rating " + ((bool)getdata.IsApproved ? "approved" : "rejected") + "  successfully ...";

                        log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)getdata.IsApproved ? "Approved" : "Rejected")} service rating {getdata.Service.Name}.");
                        return Json(new
                        {
                            success = true,
                            message = SuccessMessage,
                            data = new
                            {
                                Date = getdata.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                                BookingID = getdata.ServiceBookingID,
                                Bookingno = getdata.ServiceBooking.BookingNo,
                                customer = getdata.Customer.UserName,
                                Service = getdata.Service.Name,
                                Rating = getdata.Rating,
                                Remarks = getdata.Review,
                                IsApproved = getdata.IsApproved.HasValue ? getdata.IsApproved.Value.ToString() : false.ToString(),
                                ID = getdata.ID
                            }
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ErrorMessage = "Oops! Something went wrong. Please try later.";
                    }
                }
            }
            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRating rating = _serviceRatingsService.GetServiceRating((Int16)id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ServiceRatingReport()
        {
            string ImageServer = CustomURL.GetImageServer();

            var getAllCatagories = _serviceRatingsService.GetServiceRatings().ToList();
            if (getAllCatagories.Count() > 0)
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("ServiceRating");

                    var headerRow = new List<string[]>()
                    {
                    new string[] {
                        "Creation Date"
                        ,"Booking No"
                        ,"Customer"
                        ,"Service"
                        ,"Remarks"
                        ,"Status"
                        }
                    };

                    // Determine the header range (e.g. A1:D1)
                    string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    // Target a worksheet
                    var worksheet = excel.Workbook.Worksheets["ServiceRating"];

                    // Popular header row data
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    if (getAllCatagories.Count != 0)
                        getAllCatagories = getAllCatagories.OrderByDescending(x => x.ID).ToList();

                    foreach (var i in getAllCatagories)
                    {

                        cellData.Add(new object[] {
                        i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
                        ,i.ServiceBooking.BookingNo

                        ,i.Customer != null ? (!string.IsNullOrEmpty(i.Customer.UserName) ? i.Customer.UserName : "-") : "-"
                        ,i.Service != null ? (!string.IsNullOrEmpty(i.Service.Name) ? i.Service.Name : "-") : "-"
                        ,!string.IsNullOrEmpty(i.Review) ? i.Review : "-"

                        ,"Pending"
                        });
                    }

                    worksheet.Cells[2, 1].LoadFromArrays(cellData);

                    return File(excel.GetAsByteArray(), "application/msexcel", "Service Rating Report.xlsx");
                }
            }
            return RedirectToAction("Index");
        }
    }
}