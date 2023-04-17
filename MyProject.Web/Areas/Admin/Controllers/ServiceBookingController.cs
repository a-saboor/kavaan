using OfficeOpenXml;
using Project.Data;
using Project.Service;
using Project.Web.Areas.Admin.ViewModels;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using Project.Web.Helpers.PushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class ServiceBookingController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IServiceBookingService _ServiceBookingService;
        private readonly INotificationService _notificationService;
        private readonly INotificationReceiverService _notificationReceiverService;
        private readonly ICustomerSessionService _customerSessionService;
        private readonly ITransactionService _transactionService;
        private readonly IVendorService _vendorService;
        private readonly IQuotationService _quotationService;
        private readonly IQuotationDetailService _quotationDetailService;


        public ServiceBookingController(IVendorService vendorService, IServiceBookingService ServiceBookingService, IQuotationService quotationService, IQuotationDetailService quotationDetailService, INotificationService notificationService, INotificationReceiverService notificationReceiverService, ICustomerSessionService customerSessionService, ITransactionService transactionService)
        {
            this._ServiceBookingService = ServiceBookingService;
            this._notificationService = notificationService;
            this._notificationReceiverService = notificationReceiverService;
            this._customerSessionService = customerSessionService;
            this._transactionService = transactionService;
            this._vendorService = vendorService;
            this._quotationService = quotationService;
            this._quotationDetailService = quotationDetailService;
        }

        public ActionResult Index()
        {
            ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
            ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30).ToString("MM/dd/yyyy");
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }

        public ActionResult List()
        {
            DateTime EndDate = Helpers.TimeZone.GetLocalDateTime();
            DateTime FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30);
            var AdminID = 0;
            var booking = _ServiceBookingService.GetPendingServiceBookingsDateWise(FromDate, EndDate, AdminID).OrderByDescending(x => x.ID).ToList();
            return PartialView(booking);
        }
        [HttpPost]
        public ActionResult List(DateTime FromDate, DateTime ToDate)
        {
            DateTime EndDate = ToDate.AddMinutes(1439);
            var AdminID = 0;
            var booking = _ServiceBookingService.GetPendingServiceBookingsDateWise(FromDate, EndDate, AdminID).OrderByDescending(x => x.ID).ToList();
            return PartialView(booking);
        }
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceBooking serviceBooking = this._ServiceBookingService.GetServiceBooking((long)id);
            Transaction Transaction = _transactionService.GetTransactionByCode(serviceBooking.BookingNo);
            if (serviceBooking == null)
            {
                return HttpNotFound();
            }
            ViewData["Transaction"] = Transaction;
            return View(serviceBooking);
        }

        public ActionResult QuotationDetails(long? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            QuotationViewModel Details = new QuotationViewModel();
            
            Quotation quotation = this._quotationService.GetQuotationByServiceBookingID((long)id);
            var serviceBooking = this._ServiceBookingService.GetServiceBooking((long)id);

            if(quotation != null)
            {
                var quotationDetail = this._quotationDetailService.GetQuotationByServiceQuotationID((long)quotation.ID);

                Details.BookingNo = quotation.ServiceBooking.BookingNo;
                Details.ServiceName = quotation.ServiceBooking.Service != null ? quotation.ServiceBooking.Service.Name : "-";
                Details.ServiceCategoryName = quotation.ServiceBooking.Category != null ? quotation.ServiceBooking.Category.CategoryName : "-";
                Details.Status = quotation.ServiceBooking.Status;
                Details.CreatedOn = serviceBooking.CreatedOn;
                Details.CustomerName = serviceBooking.CustomerName;
                Details.CustomerContact = serviceBooking.CustomerContact;
                Details.IsPaid = (bool)serviceBooking.IsPayed;
                Details.MapLocation = serviceBooking.MapLocation;
                Details.CustomerLogo = serviceBooking.Customer.Logo;
                Details.CancellationReason = serviceBooking.CancellationReason;
                Details.TotalAmount = (decimal)quotation.Total;
                Details.Amount = (decimal)quotation.Subtotal;
                Details.Tax = (decimal)quotation.Tax;
                Details.quotationDetails = quotationDetail;
            }

            if (Details == null)
            {
                return HttpNotFound();
            }
            return View(Details);
        }
        public ActionResult AssignVendor(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceBooking serviceBooking = _ServiceBookingService.GetServiceBooking((long)id);
            if (serviceBooking == null)
            {
                return HttpNotFound();
            }
            var obj = new SelectList(_vendorService.GetVendorsForDropDown(), "value", "text", serviceBooking.VendorID);
            ViewBag.VendorID = obj;
            ViewBag.ID = serviceBooking.ID;

            return View(serviceBooking);
        }
        [HttpPost]
        public ActionResult AssignVendor(ServiceBooking data)
        {
            try
            {
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    ServiceBooking updateServiceBooking = _ServiceBookingService.GetServiceBooking(data.ID);
                    if (updateServiceBooking.Status != "Diagnosis" && updateServiceBooking.Status != "Invoiced")
                    {
                        updateServiceBooking.VendorID = data.VendorID;

                        if (_ServiceBookingService.UpdateServiceBooking(ref updateServiceBooking, ref message))
                        {
                            //var category = _categoryService.GetCategory((long)updateService.CategoryID);
                            log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} assign vendor booking {updateServiceBooking.BookingNo}.");
                            return Json(new
                            {
                                success = true,
                                url = "/Vendor/ServiceBookings/Index",
                                message = message,
                                data = new
                                {
                                    ID = updateServiceBooking.ID,
                                    Service = updateServiceBooking.Service.Name,
                                    Date = updateServiceBooking.CreatedOn.ToString("dd MMM yyyy, h: mm tt"),
                                    CustomerName = updateServiceBooking.CustomerName,
                                    CustomerContact = updateServiceBooking.CustomerContact,
                                    CustomerLogo = updateServiceBooking.Customer.Logo,
                                    BookingNo = updateServiceBooking.BookingNo,
                                    IsPayed = updateServiceBooking.IsPayed,
                                    Total = updateServiceBooking.Total != null ? updateServiceBooking.Total.ToString() : "0.00",
                                    VendorName = updateServiceBooking.Vendor != null ? updateServiceBooking.Vendor.Name : "-",
                                    Status = updateServiceBooking.Status,
                                    IsQuoteApproved = updateServiceBooking.IsQuoteApproved,
                                }
                            });
                        }
                    }
                    else 
                    {
                        message = "Can't assign vendor ...";
                    }
                       
                }
                else
                {
                    message = "Please fill the form properly ...";
                }
                return Json(new { success = false, message = message });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                });
            }
        }
        public ActionResult CompletedServiceBooking()
        {
            ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
            ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30).ToString("MM/dd/yyyy");
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }

        public ActionResult CompletedServiceBookingList()
        {
            DateTime EndDate = Helpers.TimeZone.GetLocalDateTime();
            DateTime FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30);
            var AdminID = 0;
            var booking = _ServiceBookingService.GetCompletedServiceBookingsDateWise(FromDate, EndDate, AdminID).OrderByDescending(x => x.ID).ToList();
            return PartialView(booking);
        }

        [HttpPost]
        public ActionResult CompletedServiceBookingList(DateTime FromDate, DateTime ToDate)
        {
            DateTime EndDate = ToDate.AddMinutes(1439);
            var AdminID = 0;
            var booking = _ServiceBookingService.GetCompletedServiceBookingsDateWise(FromDate, EndDate, AdminID).OrderByDescending(x => x.ID).ToList();
            return PartialView(booking);
        }
        public ActionResult StatusChange(long ID)
        {
            ServiceBooking bookings = _ServiceBookingService.GetServiceBooking((long)ID);
            return View(bookings);
        }

        [HttpPost]
        public ActionResult StatusChange(ServiceBooking serviceBooking, string status)
        {
            string message = string.Empty;
            ServiceBooking booking = _ServiceBookingService.GetServiceBooking((long)serviceBooking.ID);
            booking.Status = status;
           
            if (_ServiceBookingService.UpdateStatus(ref booking, ref message))
            {
                var VendorID = Convert.ToInt64(Session["VendorID"]);
                
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} status change booking {booking.Status}.");
                return Json(new
                {
                    success = true,
                    url = "/Admin/ServiceBooking/Index",
                    message = "Status updated successfully ...",
                    data = new
                    {
                        ID = booking.ID,
                        Service = booking.Service.Name,
                        Date = booking.CreatedOn.ToString("dd MMM yyyy, h: mm tt"),
                        CustomerName = booking.CustomerName,
                        CustomerContact = booking.CustomerContact,
                        CustomerLogo = booking.Customer.Logo,
                        BookingNo = booking.BookingNo,
                        IsPayed = booking.IsPayed,
                        Total = booking.Total != null ? booking.Total.ToString() : "0.00",
                        VendorName = booking.Vendor != null ? booking.Vendor.Name : "-",
                        Status = booking.Status,
                        IsQuoteApproved = booking.IsQuoteApproved,
                    }
                });
            }
            return Json(new
            {
                success = false,
                message = "Ooops! something went wrong..."
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PendingBookingReport(DateTime? FromDate, DateTime? ToDate)
        {
            if (FromDate != null && ToDate != null)
            {
                DateTime EndDate = ToDate.Value.AddMinutes(1439);
                var AdminID = 0;
                var getAllBookings = _ServiceBookingService.GetPendingServiceBookingsDateWise(FromDate.Value, EndDate, AdminID).ToList();
                if (getAllBookings.Count() > 0)
                {
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        excel.Workbook.Worksheets.Add("CustomersReport");

                        var headerRow = new List<string[]>()
                    {
                            
                            new string[] {
                        "Creation Date"
                        ,"Service"
                        ,"Name"
                        ,"Contact"
                        ,"Booking No"
                        ,"Is Payed"
                        ,"Total"
                        ,"Vendor Name"
                        ,"Status"
                        }
                    };

                        // Determine the header range (e.g. A1:D1)
                        string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        // Target a worksheet
                        var worksheet = excel.Workbook.Worksheets["CustomersReport"];

                        // Popular header row data
                        worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                        var cellData = new List<object[]>();

                        if (getAllBookings.Count != 0)
                            getAllBookings = getAllBookings.OrderByDescending(x => x.ID).ToList();

                        //Date = updateServiceBooking.CreatedOn.ToString("dd MMM yyyy, h: mm tt"),
                        //Service = updateServiceBooking.Service.Name,
                        //CustomerName = updateServiceBooking.CustomerName,
                        //CustomerContact = updateServiceBooking.CustomerContact,
                        //BookingNo = updateServiceBooking.BookingNo,
                        //IsPayed = updateServiceBooking.IsPayed,
                        //Total = updateServiceBooking.Total != null ? updateServiceBooking.Total.ToString() : "0.00",
                        //VendorName = updateServiceBooking.Vendor != null ? updateServiceBooking.Vendor.Name : "-",
                        //Status = updateServiceBooking.Status,
                        foreach (var i in getAllBookings)
                        {
                            cellData.Add(new object[] {
                            i.CreatedOn != null ? i.CreatedOn.ToString("dd MMM yyyy, h:mm tt") : "-"
                            ,i.Service != null ? (!string.IsNullOrEmpty(i.Service.Name) ? i.Service.Name : "-") : "-"
                            ,!string.IsNullOrEmpty(i.CustomerName) ? i.CustomerName: "-"
                            ,!string.IsNullOrEmpty(i.CustomerContact) ? i.CustomerContact : "-"
                            ,!string.IsNullOrEmpty(i.BookingNo) ? i.BookingNo : "-"
                            ,i.IsPayed == true ? "Paid" : "Not Paid"
                            ,i.Total != null ? i.Total : 0
                            ,i.Vendor != null ? (!string.IsNullOrEmpty(i.Vendor.Name) ? i.Vendor.Name : "-") : "-"
                            ,!string.IsNullOrEmpty(i.Status) ? i.Status : "-"
                            });
                        }

                        worksheet.Cells[2, 1].LoadFromArrays(cellData);

                        return File(excel.GetAsByteArray(), "application/msexcel", "Pending Bookings Report.xlsx");
                    }
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompletedBookingReport(DateTime? FromDate, DateTime? ToDate)
        {
            if (FromDate != null && ToDate != null)
            {
                DateTime EndDate = ToDate.Value.AddMinutes(1439);
                var AdminID = 0;
                var getAllBookings = _ServiceBookingService.GetCompletedServiceBookingsDateWise(FromDate.Value, EndDate, AdminID).ToList();
                if (getAllBookings.Count() > 0)
                {
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        excel.Workbook.Worksheets.Add("CustomersReport");

                        var headerRow = new List<string[]>()
                    {

                            new string[] {
                        "Creation Date"
                        ,"Service"
                        ,"Name"
                        ,"Contact"
                        ,"Booking No"
                        ,"Is Payed"
                        ,"Total"
                        ,"Vendor Name"
                        ,"Status"
                        }
                    };

                        // Determine the header range (e.g. A1:D1)
                        string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        // Target a worksheet
                        var worksheet = excel.Workbook.Worksheets["CustomersReport"];

                        // Popular header row data
                        worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                        var cellData = new List<object[]>();

                        if (getAllBookings.Count != 0)
                            getAllBookings = getAllBookings.OrderByDescending(x => x.ID).ToList();

                        //Date = updateServiceBooking.CreatedOn.ToString("dd MMM yyyy, h: mm tt"),
                        //Service = updateServiceBooking.Service.Name,
                        //CustomerName = updateServiceBooking.CustomerName,
                        //CustomerContact = updateServiceBooking.CustomerContact,
                        //BookingNo = updateServiceBooking.BookingNo,
                        //IsPayed = updateServiceBooking.IsPayed,
                        //Total = updateServiceBooking.Total != null ? updateServiceBooking.Total.ToString() : "0.00",
                        //VendorName = updateServiceBooking.Vendor != null ? updateServiceBooking.Vendor.Name : "-",
                        //Status = updateServiceBooking.Status,
                        foreach (var i in getAllBookings)
                        {
                            cellData.Add(new object[] {
                            i.CreatedOn != null ? i.CreatedOn.ToString("dd MMM yyyy, h:mm tt") : "-"
                            ,i.Service != null ? (!string.IsNullOrEmpty(i.Service.Name) ? i.Service.Name : "-") : "-"
                            ,!string.IsNullOrEmpty(i.CustomerName) ? i.CustomerName: "-"
                            ,!string.IsNullOrEmpty(i.CustomerContact) ? i.CustomerContact : "-"
                            ,!string.IsNullOrEmpty(i.BookingNo) ? i.BookingNo : "-"
                            ,i.IsPayed == true ? "Active" : "InActive"
                            ,i.Total != null ? i.Total : 0
                            ,i.Vendor != null ? (!string.IsNullOrEmpty(i.Vendor.Name) ? i.Vendor.Name : "-") : "-"
                            ,!string.IsNullOrEmpty(i.Status) ? i.Status : "-"
                            });
                        }

                        worksheet.Cells[2, 1].LoadFromArrays(cellData);

                        return File(excel.GetAsByteArray(), "application/msexcel", "Customers Report.xlsx");
                    }
                }
            }
            return RedirectToAction("Index");
        }

    }
}