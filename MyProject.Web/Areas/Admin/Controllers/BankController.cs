using OfficeOpenXml;
using Project.Data;
using Project.Data.Infrastructure;
using Project.Data.Repositories;
using Project.Service;
using Project.Service.Helpers;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class BankController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBankService _bankService;
        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }
        string message = string.Empty;
        string Errormessage = string.Empty;
        string Successmessage = string.Empty;
        bool status = false;
        // GET: VendorPortal/Bank
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var bank = _bankService.GetBanks();
            return PartialView(bank);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Bank data)
        {
            if (ModelState.IsValid)
            {
                Bank record = new Bank();


                record.Name = data.Name;

                if (_bankService.CreateBank(record, ref message))
                {
                    status = true;
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created bank {record.Name}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Bank/Index",
                        message = message,
                        data = new
                        {
                            CreatedOn = record.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                            Name = record.Name,
                            IsActive = record.IsActive.ToString(),
                            ID = record.ID
                        }
                    });
                }
            }
            else
            {
                message = "Please fill the form properly ...";
            }
            return Json(new { success = status, message = message });
        }
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = _bankService.GetBank((long)id);
            if (bank == null)
            {
                return HttpNotFound();
            }

            return View(bank);
        }
        [HttpPost]
        public ActionResult Edit(Bank data)
        {
            string message = string.Empty;
            Bank updateBank = _bankService.GetBank(data.ID);

            if (ModelState.IsValid)
            {
                updateBank.Name = data.Name;

                if (_bankService.UpdateBank(ref updateBank, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated bank {updateBank.Name}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Vendor/Bank/Index",
                        message = message,
                        data = new
                        {
                            CreatedOn = updateBank.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                            Name = updateBank.Name,
                            IsActive = updateBank.IsActive.ToString(),
                            ID = updateBank.ID
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
            var bank = _bankService.GetBank((long)id);
            if (bank == null)
            {
                return HttpNotFound();
            }

            if (!(bool)bank.IsActive)
            {
                bank.IsActive = true;
            }
            else
            {
                bank.IsActive = false;
            }

            if (_bankService.UpdateBank(ref bank, ref message))
            {
                Successmessage = "Bank " + ((bool)bank.IsActive ? "activated" : "deactivated") + "  successfully ...";
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)bank.IsActive ? "activated" : "deactivated")} bank {bank.Name}.");
                return Json(new
                {
                    success = true,
                    message = Successmessage,
                    data = new
                    {
                        CreatedOn = bank.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                        Name = bank.Name,
                        IsActive = bank.IsActive.ToString(),
                        ID = bank.ID
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Errormessage = "Oops! Something went wrong. Please try later...";
            }

            return Json(new { success = false, message = Errormessage }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = _bankService.GetBank((Int16)id);

            if (bank == null)
            {
                return HttpNotFound();
            }
            return View(bank);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = _bankService.GetBank((Int16)id);
            if (bank == null)
            {
                return HttpNotFound();
            }
            TempData["ID"] = id;
            return View(bank);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id, bool softDelete = true)
        {
            Bank bank = _bankService.GetBank((Int16)id);
            string message = string.Empty;
            bool hasChilds = false;
            if (softDelete)
            {
                //soft delete of data updating delete column
                if (_bankService.DeleteBank((Int16)id, ref message, ref hasChilds, softDelete))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted bank {bank.Name}.");
                    return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                //permenant delete of data
                if (_bankService.DeleteBank((Int16)id, ref message, ref hasChilds, softDelete))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted bank {bank.Name}.");
                    return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report()
        {
          
                var getAllBanks = _bankService.GetBanks().ToList();
                if (getAllBanks.Count() > 0)
                {
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        excel.Workbook.Worksheets.Add("BanksReport");

                        var headerRow = new List<string[]>()
                    {

                            new string[] {
                        "Creation Date"
                        ,"Bank Name"
                        ,"Status"

                        }
                    };

                        // Determine the header range (e.g. A1:D1)
                        string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        // Target a worksheet
                        var worksheet = excel.Workbook.Worksheets["BanksReport"];

                        // Popular header row data
                        worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                        var cellData = new List<object[]>();

                        if (getAllBanks.Count() != 0)
                            getAllBanks = getAllBanks.OrderByDescending(x => x.ID).ToList();


                        foreach (var i in getAllBanks)
                        {
                            cellData.Add(new object[] {
                            i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
                            ,!string.IsNullOrEmpty(i.Name) ? i.Name: "-"
                            ,i.IsActive == true ? "Active" : "InActive"
                            });
                        }

                        worksheet.Cells[2, 1].LoadFromArrays(cellData);

                        return File(excel.GetAsByteArray(), "application/msexcel", "Bank Report.xlsx");
                    }
                }
            
            return RedirectToAction("Index");
        }
    }
}