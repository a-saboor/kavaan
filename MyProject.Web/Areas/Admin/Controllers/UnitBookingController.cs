using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using Project.Data;
using Project.Service;
using Project.Service.Helpers;
using Project.Web.AuthorizationProvider; 
using Project.Web.Helpers;
using static Project.Web.Helpers.Enumerations.Enumeration;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class UnitBookingController : Controller
    {
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IUnitBookingService unitBookingService;
        private readonly IUnitService unitService;
        private readonly ITransactionService _transactionService;
        private readonly IMail _email;

        public UnitBookingController(IUnitBookingService unitBookingService = null, IUnitService unitService = null, ITransactionService transactionService = null)
        {
            this.unitBookingService = unitBookingService;
            this.unitService = unitService;
            this._transactionService = transactionService;
        }

        // GET: Admin/UnitBooking
        public ActionResult Index()
        {
            ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
            ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30).ToString("MM/dd/yyyy");
            return View();
        }

        public ActionResult List()
        {
            DateTime ToDate = Helpers.TimeZone.GetLocalDateTime();
            DateTime FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30);
            var unitbookings = this.unitBookingService.GetUnitBookings(FromDate, ToDate.AddMinutes(1439), "");
            List<BookingStatus> bookingStatus = Enum.GetValues(typeof(BookingStatus))
                        .Cast<BookingStatus>()
                        .ToList();
            ViewBag.BookingStatus = bookingStatus;

            return View(unitbookings.ToList());
        }

        [HttpPost]
        public ActionResult List(DateTime fromDate, DateTime ToDate, string status = "")
        {
            var unitbookings = this.unitBookingService.GetUnitBookings(fromDate, ToDate.AddMinutes(1439), status).ToList();
            List<BookingStatus> bookingStatus = Enum.GetValues(typeof(BookingStatus))
                      .Cast<BookingStatus>()
                      .ToList();
            ViewBag.BookingStatus = bookingStatus;
            return PartialView(unitbookings);
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitBooking UnitBooking = this.unitBookingService.GetUnitBooking((long)id);
            Transaction Transaction = _transactionService.GetTransactionByCode(UnitBooking.BookingNo);
            if (UnitBooking == null)
            {
                return HttpNotFound();
            }
            ViewData["Transaction"] = Transaction;
            return View(UnitBooking);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitBooking UnitBooking = this.unitBookingService.GetUnitBooking((long)id);
            if (UnitBooking == null)
            {
                return HttpNotFound();
            }
            ViewBag.UnitID = new SelectList(this.unitService.GetUnitsForDropDown(), "value", "text", UnitBooking.UnitID);
            return View(UnitBooking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UnitBooking UnitBooking)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (this.unitBookingService.UpdateUnitBooking(ref UnitBooking, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated booking # {UnitBooking.BookingNo}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/UnitBooking/Index",
                        message = message,
                        data = new
                        {
                            Date = UnitBooking.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                            reference = UnitBooking.BookingNo,
                            project = UnitBooking.Unit.Property.Title,
                            unit = UnitBooking.Unit.UnitNo,
                            customer = UnitBooking.Customer != null ? UnitBooking.Customer.Logo + "," + UnitBooking.Customer.UserName + "," + UnitBooking.Customer.Email + "," + UnitBooking.Customer.Contact : "-",
                            Status = UnitBooking.BookingStatus,
                            ID = UnitBooking.ID
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

        public ActionResult ChangeStatus(long id)
        {
            UnitBooking unitBooking = unitBookingService.GetUnitBooking(id);
            List<BookingStatus> bookingStatus = Enum.GetValues(typeof(BookingStatus))
                      .Cast<BookingStatus>()
                      .ToList();
            ViewBag.BookingStatus = bookingStatus;
            ViewBag.Status = unitBooking.BookingStatus;
            return View(unitBooking);
        }

        [HttpPost]
        public ActionResult ChangeStatus(UnitBooking unitBooking)
        {
            var path = Server.MapPath("~/");
            var subject = "Booking Status";
            string message = string.Empty;

            UnitBooking currentBooking = unitBookingService.GetUnitBooking(unitBooking.ID);
            currentBooking.BookingStatus = unitBooking.BookingStatus;
            if (unitBookingService.UpdateUnitBooking(ref currentBooking, ref message))
            {
                //var emailMessage = " Your Booking #" + unitBooking.BookingNo + " status has been updated.";
                //_email.SendDocumentStatusMail(unitBooking.Customer.Email, unitBooking.Customer.FirstName, subject, emailMessage, path);
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} change status '{currentBooking.BookingStatus}' of booking # {currentBooking.BookingNo}.");

                return Json(new
                {
                    success = true,
                    url = "/Admin/UnitBooking/Index",
					message = "Status updated successfully ...",
                    data = new
                    {
                        Date = currentBooking.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                        reference = currentBooking.BookingNo,
                        project = currentBooking.Unit.Property.Title,
                        unit = currentBooking.Unit.UnitNo,
                        customer = currentBooking.Customer != null ? currentBooking.Customer.Logo + "," + currentBooking.Customer.UserName + "," + currentBooking.Customer.Email + "," + currentBooking.Customer.Contact : "-",
                        Status = currentBooking.BookingStatus,
                        ID = currentBooking.ID
                    }
                });
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookingsReport(DateTime? FromDate, DateTime? ToDate, string Status = "")
        {
            DateTime EndDate = ToDate.Value.AddMinutes(1439);
            var getAllBookings = unitBookingService.GetUnitBookings(FromDate.Value, EndDate, Status).ToList();
            if (getAllBookings.Count() > 0)
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("BookingsReport");

                    var headerRow = new List<string[]>()
                    {
                    new string[] {
                        "Creation Date"
                        ,"Booking #"
                        ,"Project"
                        ,"Unit No"
                        ,"Customer Name"
                        ,"Customer Contact"
                        ,"Customer Email"
                        ,"Status"
                    }
                    };

                    // Determine the header range (e.g. A1:D1)
                    string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    // Target a worksheet
                    var worksheet = excel.Workbook.Worksheets["BookingsReport"];

                    // Popular header row data
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    if (getAllBookings.Count != 0)
                        getAllBookings = getAllBookings.OrderByDescending(x => x.ID).ToList();

                    foreach (var i in getAllBookings)
                    {
                        cellData.Add(new object[] {
                        i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
                        ,!string.IsNullOrEmpty(i.BookingNo) ? i.BookingNo:"-"
                        ,!string.IsNullOrEmpty(i.Unit.Property.Title) ? i.Unit.Property.Title :"-"
                        ,!string.IsNullOrEmpty(i.Unit.UnitNo) ? i.Unit.UnitNo:"-"
                        ,!string.IsNullOrEmpty(i.Customer.UserName) ? i.Customer.UserName:"-"
                        ,!string.IsNullOrEmpty(i.Customer.Contact) ? i.Customer.Contact:"-"
                        ,!string.IsNullOrEmpty(i.Customer.Email) ? i.Customer.Email:"-"
                        ,!string.IsNullOrEmpty(i.BookingStatus) ? i.BookingStatus:"-"
						//,i.IsActive == true ? "Active" :"InActive"
						});
                    }

                    worksheet.Cells[2, 1].LoadFromArrays(cellData);

                    return File(excel.GetAsByteArray(), "application/msexcel", "Bookings Report.xlsx");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
