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
using Project.Web.AuthorizationProvider; 
using Project.Web.Helpers;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class UnitEnquiriesController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IUnitEnquiriesService unitEnquiriesService;
        private readonly IUnitService unitService;
        string SuccessMessage=string.Empty;
        string ErrorMessage=string.Empty;
        public UnitEnquiriesController(IUnitEnquiriesService unitEnquiriesService = null, IUnitService unitService = null)
        {
            this.unitEnquiriesService = unitEnquiriesService;
            this.unitService = unitService;
        }

        // GET: Admin/UnitEnquiries
        public ActionResult Index(int id = 0)
        {
            ViewBag.Id = id;
            ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
            ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30).ToString("MM/dd/yyyy");
            return View();
        }
        public ActionResult List(int id = 0)
        {
            DateTime ToDate = Helpers.TimeZone.GetLocalDateTime();
            DateTime fromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30);
            string status = string.Empty;
            IEnumerable<UnitEnquiry> unitEnquiries = new List<UnitEnquiry>();
            if (id == 0)
                unitEnquiries = this.unitEnquiriesService.GetUnitEnquiry(fromDate, ToDate);
            if (id == 1)
            {
                status = "Pending";
                unitEnquiries = this.unitEnquiriesService.GetUnitEnquiry(fromDate, ToDate).Where(m => m.Status == null );
            }
            else if (id == 2)
			{
                status = "Completed";
                unitEnquiries = this.unitEnquiriesService.GetUnitEnquiry(fromDate, ToDate).Where(m => m.Status == true);
            }
            else if (id == 3)
            {
                status = "Cancelled";
                unitEnquiries = this.unitEnquiriesService.GetUnitEnquiry(fromDate, ToDate).Where(m => m.Status == false);
            }
            //var unitEnquiries = this.unitEnquiriesService.GetUnitEnquiry();

            ViewBag.Status = status + " ";
            return View(unitEnquiries.ToList());

        }

        [HttpPost]
        public ActionResult List(DateTime fromDate, DateTime ToDate ,int id = 0 )
        {
            ToDate = ToDate.AddMinutes(1439);
            string status = string.Empty;
            IEnumerable<UnitEnquiry> unitEnquiries = new List<UnitEnquiry>();
            if (id == 0)
                unitEnquiries = this.unitEnquiriesService.GetUnitEnquiry(fromDate, ToDate);
            if (id == 1)
            {
                status = "Pending";
                unitEnquiries = this.unitEnquiriesService.GetUnitEnquiry(fromDate, ToDate).Where(m => m.Status == null);
            }
            else if (id == 2)
            {
                status = "Completed";
                unitEnquiries = this.unitEnquiriesService.GetUnitEnquiry(fromDate, ToDate).Where(m => m.Status == true);
            }
            else if (id == 3)
            {
                status = "Cancelled";
                unitEnquiries = this.unitEnquiriesService.GetUnitEnquiry(fromDate, ToDate).Where(m => m.Status == false);
            }
            //var unitEnquiries = this.unitEnquiriesService.GetUnitEnquiry();

            ViewBag.Status = status + " ";
            return View(unitEnquiries.ToList());

        }

        // GET: Admin/UnitEnquiries/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitEnquiry unitEnquiry = this.unitEnquiriesService.GetUnitEnquiry((long)id);
            if (unitEnquiry == null)
            {
                return HttpNotFound();
            }
            return View(unitEnquiry);
        }

        // GET: Admin/UnitEnquiries/Create
        //public ActionResult Create()
        //{
        //    ViewBag.UnitD = new SelectList(db.Units, "ID", "Title");
        //    return View();
        //}

        //// POST: Admin/UnitEnquiries/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,UnitD,FullName,Contact,Email,Status,CreatedOn")] UnitEnquiry unitEnquiry)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.UnitEnquiries.Add(unitEnquiry);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.UnitD = new SelectList(db.Units, "ID", "Title", unitEnquiry.UnitD);
        //    return View(unitEnquiry);
        //}

        // GET: Admin/UnitEnquiries/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitEnquiry unitEnquiry = this.unitEnquiriesService.GetUnitEnquiry((long)id);
            if (unitEnquiry == null)
            {
                return HttpNotFound();
            }
            ViewBag.UnitID = new SelectList(this.unitService.GetUnitsForDropDown(), "value", "text", unitEnquiry.UnitID);
            return View(unitEnquiry);
        }

        // POST: Admin/UnitEnquiries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UnitD,FullName,Contact,Email,Status,CreatedOn")] UnitEnquiry unitEnquiry)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {

                if (this.unitEnquiriesService.UpdateUnitEnquiry(ref unitEnquiry, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated unit enquiry.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/UnitEnquiries/Index",
                        message = message,
                        data = new
                        {
                            Date = unitEnquiry.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                            UnitName = unitEnquiry.Unit.Title,
                            FullName = unitEnquiry.FullName,
                            Contact = unitEnquiry.Contact,
                            Email = unitEnquiry.Email,
                            Status = unitEnquiry.Status,
                            ID = unitEnquiry.ID
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
        public ActionResult Activate(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var enquiry = this.unitEnquiriesService.GetUnitEnquiry((long)id);
            if (enquiry == null)
            {
                return HttpNotFound();
            }

            if (enquiry.Status == null || !(bool)enquiry.Status)
			{
                enquiry.Status = true;
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} activated unit enquiry.");
			}
            else
            {
                enquiry.Status = false;
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deactivated unit enquiry.");
            }
            string message = string.Empty;
            if (this.unitEnquiriesService.UpdateUnitEnquiry(ref enquiry, ref message))
            {
                SuccessMessage = "Unit Inquiry " + ((bool)enquiry.Status ? "Completed" : "Cancelled") + "  successfully ...";
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        Date = enquiry.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                        Title = enquiry.Unit.Title,
                        FullName=enquiry.FullName,
                        Contact=enquiry.Contact,
                        Email=enquiry.Email,
                        Status=enquiry.Status,
                        ID = enquiry.ID
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later...";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Reply(long id)
        {
            var model = unitEnquiriesService.GetUnitEnquiry(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Reply(UnitEnquiry enq)
        {
            string message = string.Empty;
            UnitEnquiry unitEnquiry = unitEnquiriesService.GetUnitEnquiry((long)enq.ID);
            unitEnquiry.Status = enq.Status;
            unitEnquiry.Reply = "<p>" + enq.Reply + "</p><hr /><p>" + Helpers.TimeZone.GetLocalDateTime().ToString("dd MMM yyyy, h:mm tt") + "</p><br /><p>" + unitEnquiry.Reply + "</p>";
            if (unitEnquiriesService.UpdateUnitEnquiry(ref unitEnquiry, ref message))
            {
                var Feed = unitEnquiry;
                return Json(new
                {
                    success = true,
                    url = "/Admin/CustomerDocumentDetail/Index",
                    message = "Status updated successfully ...",
                    
                    data = new
                    {
                        Date = Feed.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                        Title = Feed.Unit.Title,
                        FullName = Feed.FullName,
                        Contact = Feed.Contact,
                        Email = Feed.Email,
                        Status = Feed.Status,
                        ID = Feed.ID

                    }
                }); ;
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnitEnquiriesReport(DateTime? FromDate, DateTime? ToDate)
        {
            DateTime EndDate = ToDate.Value.AddMinutes(1439);
            var getAllTransactions = this.unitEnquiriesService.GetUnitEnquiry(FromDate.Value, EndDate).ToList();
            if (getAllTransactions.Count() > 0)
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("UnitEnquiriesReport");

                    var headerRow = new List<string[]>()
                    {
                    new string[] {
                        "Creation Date"
                        ,"Unit Name"
                        ,"Full Name"
                        ,"Contact"
                        ,"Email"
                        }
                    };

                    // Determine the header range (e.g. A1:D1)
                    string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    // Target a worksheet
                    var worksheet = excel.Workbook.Worksheets["UnitEnquiriesReport"];

                    // Popular header row data
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    if (getAllTransactions.Count != 0)
                        getAllTransactions = getAllTransactions.OrderByDescending(x => x.ID).ToList();

                    foreach (var i in getAllTransactions)
                    {
                        cellData.Add(new object[] {
                        i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
                        ,i.Unit != null ? i.Unit.Title : "-"
                        ,!string.IsNullOrEmpty(i.FullName) ? i.FullName:"-"
                        ,!string.IsNullOrEmpty(i.Contact) ? i.Contact :"-"
                        ,!string.IsNullOrEmpty(i.Email) ? i.Email:"-"
                        });
                    }

                    worksheet.Cells[2, 1].LoadFromArrays(cellData);

                    return File(excel.GetAsByteArray(), "application/msexcel", "Unit Enquiry Report.xlsx");
                }
            }
            return RedirectToAction("Index");
        }


    }
}
