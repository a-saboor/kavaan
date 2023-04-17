using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MyProject.Web.Helpers;

namespace MyProject.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class CustomerController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly ICustomerService _customerService;
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;
        private readonly IAreaService _areaService;


        public CustomerController(ICustomerService customerService, ICountryService countryService, ICityService cityService, IAreaService areaService)
        {
            this._customerService = customerService;
            this._countryService = countryService;
            this._cityService = cityService;
            this._areaService = areaService;
        }


        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.FromDate = AdminCustomHelper.GetFromDateString();
            ViewBag.ToDate = AdminCustomHelper.GetToDateString();
            return View();
        }

        public ActionResult List()
        {
            DateTime EndDate = Helpers.TimeZone.GetLocalDateTime();
            DateTime FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-365);
            var customers = _customerService.GetCustomersDateWise(FromDate, EndDate);
            return PartialView(customers);
        }

        [HttpPost]
        public ActionResult List(DateTime fromDate, DateTime toDate)
        {
            toDate = AdminCustomHelper.GetToDate(toDate);
            var customers = _customerService.GetCustomersDateWise(fromDate, toDate);
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            return PartialView(customers);
        }

        public ActionResult ListReport()
        {
            var customers = _customerService.GetCustomers();
            return View(customers);
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = _customerService.GetCustomer((Int16)id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.CountryID = new SelectList(_countryService.GetCountriesForDropDown(), "value", "text");
            ViewBag.CityID = new SelectList(_cityService.GetCitiesForDropDown(), "value", "text", customer.CityID);
            ViewBag.AreaID = new SelectList(_areaService.GetAreasForDropDown(), "value", "text", customer.AreaID);

            return View(customer);
        }
        [HttpGet]
        public ActionResult GetCitiesByCountry(long id)
        {
            var cities = _cityService.GetCitiesForDropDown(id);

            return Json(new { success = true, message = "Data recieved successfully!", data = cities }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAreasByCity(long id)
        {
            var areas = _areaService.GetAreasForDropDown(id);

            return Json(new { success = true, message = "Data recieved successfully!", data = areas }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(_countryService.GetCountriesForDropDown(), "value", "text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            string message = string.Empty;
            if (ModelState.IsValid || true)
            {
                customer.PhoneCode = !string.IsNullOrEmpty(customer.PhoneCode) ? customer.PhoneCode : "92";

                customer.FirstName = customer.UserName;
                customer.AccountType = "Basic";
                customer.Logo = "/Assets/AppFiles/Customer/default.png";
                customer.Points = 0;
                customer.PhoneCode = customer.PhoneCode.Replace("+", "");
                //customer.Contact = customer.PhoneCode + customer.Contact;
                if (_customerService.CreateCustomer(ref customer, string.Empty, ref message, true, true, true))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created customer {customer.UserName}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Customer/Index",
                        message = message,
                        data = new
                        {
                            ID = customer.ID,
                            Date = customer.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                            UserName = customer.UserName,
                            Contact = customer.Contact,
                            PhoneCode = customer.PhoneCode,
                            Email = customer.Email,
                            Address = customer.Address,
                            IsActive = customer.IsActive.HasValue ? customer.IsActive.Value.ToString() : bool.FalseString

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
            Customer customer = _customerService.GetCustomer((long)id);
            ViewBag.CountryID = new SelectList(_countryService.GetCountriesForDropDown(), "value", "text");
            ViewBag.CityID = new SelectList(_cityService.GetCitiesForDropDown(), "value", "text", customer.CityID);
            ViewBag.AreaID = new SelectList(_areaService.GetAreasForDropDown(), "value", "text", customer.AreaID);
            if (customer == null)
            {
                return HttpNotFound();
            }

            TempData["CustomerID"] = id;
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            string message = string.Empty;
            string status = string.Empty;
            if (ModelState.IsValid)
            {
                long Id;
                if (TempData["CustomerID"] != null && Int64.TryParse(TempData["CustomerID"].ToString(), out Id) && customer.ID == Id)
                {
                    customer.PhoneCode = customer.PhoneCode.Replace("+", "");
                    customer.Contact = customer.PhoneCode + customer.Contact;
                    //Customer currentCustomer = _customerService.GetCustomer(Id);
                    if (_customerService.UpdateCustomer(ref customer, ref message, ref status, true))
                    {
                        log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated customer {customer.UserName}.");
                        return Json(new
                        {
                            success = true,
                            url = "/Admin/Customer/Index",
                            message = "Customer updated successfully ...",
                            data = new
                            {
                                ID = customer.ID,
                                Date = customer.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                                UserName = customer.UserName,
                                Contact = customer.Contact,
                                PhoneCode = customer.PhoneCode,
                                Email = customer.Email,
                                Address = customer.Address,
                                IsActive = customer.IsActive.HasValue ? customer.IsActive.Value.ToString() : bool.FalseString
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

        public ActionResult Activate(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = _customerService.GetCustomer((long)id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            if (!(bool)customer.IsActive)
            {
                customer.IsActive = true;
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} activated customer {customer.UserName}.");
            }
            else
            {
                customer.IsActive = false;
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deactivated customer {customer.UserName}.");
            }
            string message = string.Empty;
            string status = string.Empty;
            if (_customerService.UpdateCustomer(ref customer, ref message, ref status))
            {
                SuccessMessage = "Customer " + ((bool)customer.IsActive ? "activated" : "deactivated") + "  successfully ...";
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        ID = customer.ID,
                        Date = customer.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                        UserName = customer.UserName,
                        Contact = customer.Contact,
                        PhoneCode = customer.PhoneCode,
                        Email = customer.Email,
                        Address = customer.Address,
                        IsActive = customer.IsActive.HasValue ? customer.IsActive.Value.ToString() : bool.FalseString

                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later...";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = _customerService.GetCustomer((Int16)id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            TempData["CustomerID"] = id;
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (_customerService.DeleteCustomer((Int16)id, ref message))
            {
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted customer ID: {id}.");
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomersReport(DateTime? FromDate, DateTime? ToDate)
        {
            if (FromDate != null && ToDate != null)
            {
                DateTime EndDate = ToDate.Value.AddMinutes(1439);
                var getAllCustomers = _customerService.GetCustomersDateWise(FromDate.Value, EndDate).ToList();
                if (getAllCustomers.Count() > 0)
                {
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        excel.Workbook.Worksheets.Add("CustomersReport");

                        var headerRow = new List<string[]>()
                    {
                    new string[] {
                        "Creation Date"
                        ,"Name"
                        ,"Contact"
                        ,"Email"
                        ,"Address"
                        ,"Country"
                        ,"City"
                        ,"Area"
                        ,"Account Type"
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

                        if (getAllCustomers.Count != 0)
                            getAllCustomers = getAllCustomers.OrderByDescending(x => x.ID).ToList();

                        foreach (var i in getAllCustomers)
                        {
                            cellData.Add(new object[] {
                            i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString(CustomHelper.GetDateFormat) : "-"
                            ,!string.IsNullOrEmpty(i.UserName) ? i.UserName : "-"
                            ,!string.IsNullOrEmpty(i.Contact) ? i.Contact: "-"
                            ,!string.IsNullOrEmpty(i.Email) ? i.Email : "-"
                            ,!string.IsNullOrEmpty(i.Address) ? i.Address : "-"
                            ,i.Country != null ? (!string.IsNullOrEmpty(i.Country.Name) ? i.Country.Name : "-") : "-"
                            ,i.City != null ? (!string.IsNullOrEmpty(i.City.Name) ? i.City.Name : "-") : "-"
                            ,i.Area != null ? (!string.IsNullOrEmpty(i.Area.Name) ? i.Area.Name : "-") : "-"
                            ,!string.IsNullOrEmpty(i.AccountType) ? i.AccountType : "-"
                            ,i.IsActive == true ? "Active" : "InActive"
                            });
                        }

                        worksheet.Cells[2, 1].LoadFromArrays(cellData);

                        return File(excel.GetAsByteArray(), "application/msexcel", "Customers Report.xlsx");
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditProfile(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = _customerService.GetCustomer((long)id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            TempData["CustomerID"] = id;
            return View(customer);
        }
        [HttpPost]
        //[ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Customer customer)
        {
            string message = string.Empty;
            string status = string.Empty;
            if (ModelState.IsValid)
            {
                //facility.IsApproved = false;
                //facility.ApprovalStatus = 1;

                var hello = customer.EmiratesID;
                if (_customerService.UpdateCustomer(ref customer, ref message, ref message, true))
                {

                    return Json(new
                    {
                        success = true,
                        //url = "/Admin/Car/Index",
                        message = "Customer updated successfully ...",
                        //data = new
                        //{
                        //	ID = facility.ID,
                        //	Date = facility.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                        //	//SKU = facility.SKU,
                        //	Name = facility.Name,
                        //	//LongDescription = facility.LongDescription,
                        //	Remark = facility.Remarks,
                        //	IsActive = facility.IsActive.HasValue ? facility.IsActive.Value.ToString() : bool.FalseString
                        //}
                    });
                }
            }
            else
            {
                message = "Please fill the form properly ...";
            }
            return Json(new { success = false, message = message });
        }
        //public ActionResult Thumbnail(long? id)
        //{
        //	try
        //	{
        //		if (id == null)
        //		{
        //			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //		}
        //		Tournament tournament = _tournamentService.GetTournament((long)id);
        //		if (tournament == null)
        //		{
        //			return HttpNotFound();
        //		}
        //		string filePath = !string.IsNullOrEmpty(tournament.Thumbnail) ? tournament.Thumbnail : string.Empty;


        //		string message = string.Empty;

        //		string absolutePath = Server.MapPath("~");
        //		string relativePath = string.Format("/Assets/AppFiles/Images/Coach/{0}/", tournament.Name.Replace(" ", "_"));

        //		tournament.Thumbnail = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Thumbnail", ref message, "Image");

        //		if (_tournamentService.UpdateTournament(ref tournament, ref message))
        //		{
        //			//_carService.UpdateCarApprovalStatus(tounament.ID);

        //			if (!string.IsNullOrEmpty(filePath))
        //			{
        //				System.IO.File.Delete(Server.MapPath(filePath));
        //			}
        //			return Json(new
        //			{
        //				success = true,
        //				message = message,
        //				data = tournament.Thumbnail
        //			});
        //		}
        //		return Json(new { success = false, message = message });
        //	}
        //	catch (Exception ex)
        //	{
        //		return Json(new
        //		{
        //			success = false,
        //			message = "Oops! Something went wrong. Please try later."
        //		});
        //	}
        //}

    }
}