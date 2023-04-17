using Project.Data;
using Project.Service;
using Project.Service.Helpers;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using Project.Web.Helpers.Routing;
using Project.Web.ViewModels.Vendor;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Globalization;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class VendorController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IVendorUserRoleService _vendorUserRoleService;
        private readonly IVendorService _vendorService;
        private readonly IVendorUserService _vendorUserService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IMail _email;
        private readonly INumberRangeService _numberRangeService;
        //private readonly IVendorWalletShareService _VendorWalletShareService;
        private readonly IVendorDocumentService _vendorDocumentService;
        private readonly IVendorPackagesService _vendorPackageService;
        private readonly INotificationService _notificationService;
        private readonly INotificationReceiverService _notificationReceiverService;
        private readonly IVendorTransactionHistoryService _vendorTransactionHistoryService;
        private readonly IVendorSectionService _vendorSectionService;
        private readonly IVendorTypeService _vendorTypeService;
        private readonly IVendorIndustryService _vendorIndustryService;
        private readonly IBankService _bankService;


        public VendorController(IVendorService vendorService, ICountryService countryService, IBankService bankService, ICityService cityService, IVendorUserRoleService vendorUserRoleService, IVendorUserService vendorUserService, IMail email, INumberRangeService numberRangeService, IVendorDocumentService vendorDocumentService, IVendorPackagesService vendorPackagesService, INotificationService notificationService, INotificationReceiverService notificationReceiverService, IVendorTransactionHistoryService vendorTransactionHistoryService, IVendorSectionService vendorSectionService, IVendorTypeService vendorTypeService, IVendorIndustryService vendorIndustryService)
        {
            this._email = email;
            this._numberRangeService = numberRangeService;
            this._vendorUserService = vendorUserService;
            this._vendorUserRoleService = vendorUserRoleService;
            this._vendorService = vendorService;
            this._countryService = countryService;
            this._cityService = cityService;
            //this._VendorWalletShareService = VendorWalletShareService;
            this._vendorDocumentService = vendorDocumentService;
            this._vendorPackageService = vendorPackagesService;
            this._notificationService = notificationService;
            this._notificationReceiverService = notificationReceiverService;
            this._vendorTransactionHistoryService = vendorTransactionHistoryService;
            this._vendorIndustryService = vendorIndustryService;
            this._vendorTypeService = vendorTypeService;
            this._vendorSectionService = vendorSectionService;
            this._bankService = bankService;
        }
        //public ActionResult GetVendorPackagesByID(long id)
        //{
        //    var vendorPackage = _vendorPackageService.GetVendorPackageModules((long)id);
        //    return Json(new { success = true, message = "Data recieved successfully!", data = vendorPackage }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult VendorListReport()
        {
            var vendors = _vendorService.GetVendors(true);
            return View(vendors);
        }

        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }

        public ActionResult List()
        {
            var vendors = _vendorService.GetVendors(true);
            return PartialView(vendors);
        }

        public ActionResult Approvals()
        {
            //var vendors = _vendorService.GetVendors(false);
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            var vendors = _vendorService.GetVendorsForApproval();
            return View(vendors);
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = _vendorService.GetVendor((Int16)id);
            ViewBag.VendorPackageID = new SelectList(_vendorPackageService.GetPackageForDropDown(), "value", "text", vendor.VendorPackageID);
            ViewBag.VendorSectionID = new SelectList(_vendorPackageService.GetPackageForDropDown(), "value", "text", vendor.VendorSectionID);
            ViewBag.VendorTypeID = new SelectList(_vendorPackageService.GetPackageForDropDown(), "value", "text", vendor.VendorTypeID);
            ViewBag.VendorIndustryID = new SelectList(_vendorPackageService.GetPackageForDropDown(), "value", "text", vendor.VendorIndustryID);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(_countryService.GetCountriesForDropDown(), "value", "text");
            ViewBag.CityID = new SelectList(_cityService.GetCitiesForDropDown(), "value", "text");
            ViewBag.VendorPackageID = new SelectList(_vendorPackageService.GetPackageForDropDown(), "value", "text");
            ViewBag.VendorSectionID = new SelectList(_vendorSectionService.GetVendorSectionsForDropDown(), "value", "text");
            ViewBag.VendorTypeID = new SelectList(_vendorTypeService.GetVendorTypesForDropDown(), "value", "text");
            ViewBag.VendorIndustryID = new SelectList(_vendorIndustryService.GetVendorIndustrysForDropDown(), "value", "text");
            ViewBag.BankID = new SelectList(_bankService.GetBankForDropDown(), "value", "text");
            ViewBag.VendorCode = _numberRangeService.GetNextValueFromNumberRangeByName("VENDOR");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorFormViewModel vendorFormViewModel)
        {
            string message = string.Empty;


            if (ModelState.IsValid)
            {
                vendorFormViewModel.Contact = "971" + vendorFormViewModel.Contact;
                vendorFormViewModel.Mobile = "971" + vendorFormViewModel.Mobile;
                if (_vendorUserService.GetUserByEmail(vendorFormViewModel.UserEmail) == null)
                {
                    var vendor = new Vendor();

                    vendor.VendorCode = vendorFormViewModel.VendorCode;
                    vendor.Name = vendorFormViewModel.Name;
                    vendor.NameAr = vendorFormViewModel.NameAr;
                    vendor.Slug = vendorFormViewModel.Slug;
                    vendor.Email = vendorFormViewModel.Email;
                    vendor.Logo = vendorFormViewModel.Logo;
                    vendor.Contact = vendorFormViewModel.Contact;
                    vendor.Mobile = vendorFormViewModel.Mobile;
                    vendor.Address = vendorFormViewModel.Address;
                    vendor.IDNo = vendorFormViewModel.IDNo;
                    vendor.TRN = vendorFormViewModel.TRN;
                    vendor.Website = vendorFormViewModel.Website;
                    vendor.Commission = vendorFormViewModel.Commission;
                    vendor.License = vendorFormViewModel.License;
                    vendor.FAX = vendorFormViewModel.FAX;
                    vendor.About = vendorFormViewModel.About;
                    vendor.AboutAr = vendorFormViewModel.AboutAr;
                    vendor.CountryID = vendorFormViewModel.CountryID;
                    vendor.CityID = vendorFormViewModel.CityID;
                    vendor.BankName = vendorFormViewModel.BankName;
                    vendor.AccountHolderName = vendorFormViewModel.AccountHolderName;
                    vendor.BankAccountNumber = vendorFormViewModel.BankAccountNumber;
                    vendor.IBAN = vendorFormViewModel.IBAN;
                    vendor.Latitude = vendorFormViewModel.Latitude;
                    vendor.Longitude = vendorFormViewModel.Longitude;
                    vendor.VendorTypeID = vendorFormViewModel.VendorTypeID;
                    vendor.VendorSectionID = vendorFormViewModel.VendorSectionID;
                    vendor.VendorIndustryID = vendorFormViewModel.VendorIndustryID;
                    //vendor.TermAndConditionWebAr = vendorFormViewModel.TermsAndConditionWebAr;
                    //vendor.TermAndConditionWebEn = vendorFormViewModel.TermsAndConditionWebEn;
                    //vendor.EnableAcademyModule = vendorFormViewModel.EnableAcademyModule;
                    //vendor.EnableEcommerceModule = vendorFormViewModel.EnableEcommerceModule;
                    //vendor.EnableFacilityModule = vendorFormViewModel.EnableFacilityModule;
                    //vendor.EnableOrganizerModule = vendorFormViewModel.EnableOrganizerModule;
                    vendor.VendorPackageID = vendorFormViewModel.VendorPackageID;
                    var vendorpkg = _vendorPackageService.GetVendorPackagesByID((long)vendor.VendorPackageID);
                    if (vendorpkg.Price == 0 || vendorpkg.Price == null)
                    {
                        vendor.StartDate = Helpers.TimeZone.GetLocalDateTime();
                        vendor.IsExpiry = false;

                        if (vendorpkg.BillingPeriod == "Annual")
                        {
                            vendor.EndDate = vendor.StartDate.Value.AddMonths(12);
                        }
                        else if (vendorpkg.BillingPeriod == "BiAnnual")
                        {
                            vendor.EndDate = vendor.StartDate.Value.AddMonths(6);
                        }
                        else if (vendorpkg.BillingPeriod == "Monthly")
                        {
                            vendor.EndDate = vendor.StartDate.Value.AddMonths(1);
                        }

                    }

                    if (vendorFormViewModel.Logo != null)
                    {
                        string FilePath = string.Format("{0}{1}{2}", Server.MapPath("~/Assets/AppFiles/Images/Vendors/"), vendor.ID, "/logo");

                        string absolutePath = Server.MapPath("~");
                        string relativePath = string.Format("/Assets/AppFiles/Images/Vendors/{0}/", vendor.ID);

                        vendor.Logo = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "logo", ref message, "Logo");
                    }

                    vendor.IsApproved = true;
                    if (!string.IsNullOrEmpty(vendor.Logo))
                    {
                        if (_vendorService.CreateVendor(ref vendor, ref message))
                        {
                            if (vendor.VendorPackage.Price == null)
                            {
                                VendorTransactionHistory transactionHistory = new VendorTransactionHistory();
                                transactionHistory.VendorID = vendor.ID;
                                transactionHistory.VendorPackageID = vendor.VendorPackageID;
                                transactionHistory.Status = "Completed";
                                transactionHistory.Adjustment = 0;
                                transactionHistory.Price = 0;
                                if (_vendorTransactionHistoryService.CreateVendorTransactionHistory(ref transactionHistory, ref message)) { }
                            }
                            //long i;
                            long VendorID = vendor.ID;
                            string UserEmail = vendorFormViewModel.UserEmail;
                            string UserPassword = vendorFormViewModel.UserPassword;
                            string Role = "Administrator";
                            var UserRoleID = _vendorUserRoleService.GetVendorUserRoleByName(Role);
                            var vendoruser = new VendorUser();

                            vendoruser.VendorID = VendorID;
                            vendoruser.Name = vendor.Name;
                            vendoruser.MobileNo = vendor.Mobile;
                            vendoruser.EmailAddress = UserEmail;
                            vendoruser.Password = UserPassword;
                            vendoruser.UserRoleID = UserRoleID.ID;

                            if (_vendorUserService.CreateVendorUser(vendoruser, ref message, true))
                            {
                                var path = Server.MapPath("~/");
                                if (_email.SendVendorCreationMail(vendorFormViewModel.Name, vendorFormViewModel.Email, vendoruser.EmailAddress, UserPassword, CustomURL.GetFormatedURL("/Vendor/Account/Login"), path))
                                {
                                    vendor.IsEmailSent = true;
                                    if (_vendorService.UpdateVendor(ref vendor, ref message))
                                    {

                                    }
                                }

                                //return Json(new
                                //{
                                //    success = true,
                                //    url = "/Admin/Vendor/Index/",
                                //    isRedirect = true,
                                //    message = message,
                                //    data = new
                                //    {
                                //        CreatedOn = vendor.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                                //        Logo = vendor.Logo,
                                //        Name = vendor.Name,
                                //        VendorCode = vendor.VendorCode,
                                //        Email = vendor.Email,
                                //        IsEmailSent = vendor.IsEmailSent,
                                //        IsActive = vendor.IsActive.ToString(),
                                //        ID = vendor.ID
                                //    }
                                //});
                                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created vendor user {vendor.Name}.");
                                return RedirectToAction("Index");
                            }
                            TempData["SuccessMessage"] = message;
                            log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created vendor {vendor.Name}.");
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    message = "Vendor user account already exists ...";
                }
            }
            else
            {
                message = "Please fill the form properly ...";
            }

            ViewBag.CountryID = new SelectList(_countryService.GetCountriesForDropDown(), "value", "text", vendorFormViewModel.CountryID);
            ViewBag.CityID = new SelectList(_cityService.GetCitiesForDropDown((long)vendorFormViewModel.CountryID), "value", "text", vendorFormViewModel.CityID);
            ViewBag.VendorPackageID = new SelectList(_vendorPackageService.GetPackageForDropDown(), "value", "text", vendorFormViewModel.VendorPackageID);
            ViewBag.VendorTypeID = new SelectList(_vendorTypeService.GetVendorTypesForDropDown(), "value", "text", vendorFormViewModel.VendorTypeID);
            ViewBag.ErrorMessage = message;
            ViewBag.VendorCode = vendorFormViewModel.VendorCode;
            return View(vendorFormViewModel);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = _vendorService.GetVendor((long)id);
            vendor.Contact = vendor.Mobile != null ? vendor.Contact.Replace("971", "") : "";
            vendor.Mobile = vendor.Mobile != null ? vendor.Mobile.Replace("971", "") : "";
            if (vendor == null)
            {
                return HttpNotFound();
            }

            //ViewBag.CountryID = new SelectList(_countryService.GetCountriesForDropDown(), "value", "text", vendor.CountryID);
            ViewBag.CityID = new SelectList(_cityService.GetCitiesForDropDown(), "value", "text", vendor.CityID);
            ViewBag.CountryID = new SelectList(_countryService.GetCountriesForDropDown(), "value", "text", vendor.CountryID);
            ViewBag.VendorPackageID = new SelectList(_vendorPackageService.GetPackageForDropDown(), "value", "text", vendor.VendorPackageID);
            ViewBag.VendorSectionID = new SelectList(_vendorSectionService.GetVendorSectionsForDropDown(), "value", "text", vendor.VendorSectionID);
            ViewBag.VendorTypeID = new SelectList(_vendorTypeService.GetVendorTypesForDropDown(), "value", "text", vendor.VendorTypeID);
            ViewBag.VendorIndustryID = new SelectList(_vendorIndustryService.GetVendorIndustrysForDropDown(), "value", "text", vendor.VendorIndustryID);
            ViewBag.BankID = new SelectList(_bankService.GetBankForDropDown(), "value", "text", vendor.BankID);

            VendorEditViewModel vendorEditViewModel = new VendorEditViewModel()
            {
                ID = vendor.ID,
                VendorCode = vendor.VendorCode,
                Name = vendor.Name,
                NameAr = vendor.NameAr,
                Slug = vendor.Slug,
                Email = vendor.Email,
                Contact = vendor.Contact,
                Mobile = vendor.Mobile,
                Address = vendor.Address,
                IDNo = vendor.IDNo,
                TRN = vendor.TRN,
                Website = vendor.Website,
                Commission = vendor.Commission.HasValue ? vendor.Commission.Value : 0,
                License = vendor.License,
                FAX = vendor.FAX,
                CountryID = vendor.CountryID,
                CityID = vendor.CityID,
                VendorPackageID = vendor.VendorPackageID,
                Logo = vendor.Logo,
                CoverImage = vendor.CoverImage,
                About = vendor.About,
                AboutAr = vendor.AboutAr,
                BankName = vendor.BankName,
                BankAccountNumber = vendor.BankAccountNumber,
                AccountHolderName = vendor.AccountHolderName,
                IBAN = vendor.IBAN,
                Latitude = vendor.Latitude,
                Longitude = vendor.Longitude,
                VendorTypeID = vendor.VendorTypeID,
                VendorSectionID = vendor.VendorSectionID,
                VendorIndustryID = vendor.VendorIndustryID,
                //TermsAndConditionWebAr = vendor.TermAndConditionWebAr,
                //TermsAndConditionWebEn = vendor.TermAndConditionWebEn,
                //EnableAcademyModule = (bool)vendor.EnableAcademyModule,
                //EnableEcommerceModule = (bool)vendor.EnableEcommerceModule,
                //EnableFacilityModule = (bool)vendor.EnableFacilityModule,
                //EnableOrganizerModule = (bool)vendor.EnableOrganizerModule

            };
            return View(vendorEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorEditViewModel vendorEditViewModel)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                vendorEditViewModel.Contact = "971" + vendorEditViewModel.Contact;
                vendorEditViewModel.Mobile = "971" + vendorEditViewModel.Mobile;
                var vendor = _vendorService.GetVendor(vendorEditViewModel.ID);
                vendor.ID = vendorEditViewModel.ID;
                vendor.VendorCode = vendorEditViewModel.VendorCode;
                vendor.Name = vendorEditViewModel.Name;
                vendor.NameAr = vendorEditViewModel.NameAr;
                vendor.Email = vendorEditViewModel.Email;
                vendor.Contact = vendorEditViewModel.Contact;
                vendor.Mobile = vendorEditViewModel.Mobile;
                vendor.Address = vendorEditViewModel.Address;
                vendor.IDNo = vendorEditViewModel.IDNo;
                vendor.TRN = vendorEditViewModel.TRN;
                vendor.Website = vendorEditViewModel.Website;
                vendor.Commission = vendorEditViewModel.Commission;
                vendor.License = vendorEditViewModel.License;
                vendor.FAX = vendorEditViewModel.FAX;
                vendor.About = vendorEditViewModel.About;
                vendor.AboutAr = vendorEditViewModel.AboutAr;
                vendor.CountryID = vendorEditViewModel.CountryID;
                vendor.CityID = vendorEditViewModel.CityID;
                vendor.VendorPackageID = vendorEditViewModel.VendorPackageID;
                vendor.BankID = vendorEditViewModel.BankID;
                vendor.AccountHolderName = vendorEditViewModel.AccountHolderName;
                vendor.BankAccountNumber = vendorEditViewModel.BankAccountNumber;
                vendor.IBAN = vendorEditViewModel.IBAN;
                vendor.Latitude = vendorEditViewModel.Latitude;
                vendor.Longitude = vendorEditViewModel.Longitude;
                vendor.VendorTypeID = vendorEditViewModel.VendorTypeID;
                vendor.VendorSectionID = vendorEditViewModel.VendorSectionID;
                vendor.VendorIndustryID = vendorEditViewModel.VendorIndustryID;
                //vendor.TermAndConditionWebAr = vendorEditViewModel.TermsAndConditionWebAr;
                //vendor.TermAndConditionWebEn = vendorEditViewModel.TermsAndConditionWebEn;

                if (vendor.Logo != vendorEditViewModel.Logo && vendorEditViewModel.Logo != null)
                {
                    string FilePath = string.Format("{0}{1}{2}", Server.MapPath("~/Assets/AppFiles/Images/Vendors/"), vendor.VendorCode, "/logo");

                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Images/Vendors/{0}/", vendor.ID);

                    vendor.Logo = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "logo", ref message, "Logo");

                }
                if (vendor.CoverImage != vendorEditViewModel.CoverImage && vendorEditViewModel.CoverImage != null)
                {
                    string FilePath = string.Format("{0}{1}{2}", Server.MapPath("~/Assets/AppFiles/Images/Vendors/"), vendor.ID, "/cover");

                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Images/Vendors/{0}/", vendor.ID);

                    vendor.CoverImage = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "CoverImage", ref message, "CoverImage");

                }

                if (_vendorService.UpdateVendor(ref vendor, ref message))
                {
                    //if (data.BannerImage != null)
                    //{
                    //    string absolutePath = Server.MapPath("~");
                    //    if (System.IO.File.Exists(imagepath))
                    //    {
                    //        System.IO.File.Delete(imagepath);
                    //    }
                    //}
                    TempData["SuccessMessage"] = message;
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated vendor {vendor.Name}.");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                message = "Please fill the form properly ...";
            }
            ViewBag.CountryID = new SelectList(_countryService.GetCountriesForDropDown(), "value", "text", vendorEditViewModel.CountryID);
            ViewBag.CityID = new SelectList(_cityService.GetCitiesForDropDown(), "value", "text", vendorEditViewModel.CityID);
            ViewBag.VendorPackageID = new SelectList(_vendorPackageService.GetPackageForDropDown(), "value", "text", vendorEditViewModel.VendorPackageID);
            ViewBag.ErrorMessage = message;
            return View(vendorEditViewModel);
        }

        public ActionResult EmailSent(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var vendor = _vendorService.GetVendor((long)id);
                string VendorRole = "Administrator";
                var vendorUser = _vendorUserService.GetUserByRole((long)vendor.ID, "Administrator");
                if (vendor == null)
                {
                    return HttpNotFound();
                }

                if (vendor.IsEmailSent.HasValue)
                {
                    if (!vendor.IsEmailSent.Value)
                        vendor.IsEmailSent = true;
                    else
                    {
                        vendor.IsEmailSent = false;
                    }
                }
                else
                {
                    vendor.IsEmailSent = true;
                }
                string message = string.Empty;
                var path = Server.MapPath("~/");
                if (_email.SendVendorCreationMail(vendor.Name, vendor.Email, vendorUser.EmailAddress, string.Empty, CustomURL.GetFormatedURL("/Vendor/Account/Login"), path))
                {
                    vendor.IsEmailSent = true;
                    if (_vendorService.UpdateVendor(ref vendor, ref message))
                    {

                        log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} email send vendor {vendor.Name}.");
                        return Json(new
                        {
                            success = true,
                            message = "Email Sent Successfully",
                            data = new
                            {
                                ID = vendor.ID,
                                Date = vendor.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                                VendorCode = vendor.Logo + "|" + vendor.VendorCode + "|" + vendor.Name,
                                Email = vendor.Email,
                                IsActive = vendor.IsActive.HasValue ? vendor.IsActive.Value.ToString() : bool.FalseString,

                            }
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later.";
                }

                return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Oops! Something went wrong. Please try later." }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Activate(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vendor = _vendorService.GetVendor((long)id);
            if (vendor == null)
            {
                return HttpNotFound();
            }

            if (vendor.IsActive.HasValue)
            {
                if (!vendor.IsActive.Value)
                    vendor.IsActive = true;
                else
                {
                    vendor.IsActive = false;
                }
            }
            else
            {
                vendor.IsActive = true;
            }
            string message = string.Empty;
            if (_vendorService.UpdateVendorStatus(vendor, ref message))
            {
                SuccessMessage = "Vendor " + ((bool)vendor.IsActive ? "activated" : "deactivated") + "  successfully ...";
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)vendor.IsActive ? "activated" : "deactivated")} vendor {vendor.Name}.");
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        ID = vendor.ID,
                        Date = vendor.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                        VendorCode = vendor.VendorCode,
                        Name = vendor.Name,
                        Logo = vendor.Logo,
                        Email = vendor.Email,

                        IsActive = vendor.IsActive.HasValue ? vendor.IsActive.Value.ToString() : bool.FalseString,
                        IsEmailSent = vendor.IsEmailSent.HasValue ? vendor.IsEmailSent.Value.ToString() : bool.FalseString,

                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later.";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Approve(long? id)
        //{
        //	if (id == null)
        //	{
        //		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //	}
        //	var vendor = _vendorService.GetVendor((long)id);
        //	if (vendor == null)
        //	{
        //		return HttpNotFound();
        //	}

        //	vendor.IsApproved = true;
        //	vendor.IsActive = true;

        //	string message = string.Empty;
        //	if (_vendorService.UpdateVendor(ref vendor, ref message))
        //	{
        //		SuccessMessage = "Vendor signup request approved successfully ...";
        //		return Json(new
        //		{
        //			success = true,
        //			message = SuccessMessage,
        //		}, JsonRequestBehavior.AllowGet);
        //	}
        //	else
        //	{
        //		ErrorMessage = "Oops! Something went wrong. Please try later.";
        //	}

        //	return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public ActionResult Remarks(long id, bool approvalStatus)
        //{
        //	ViewBag.BuildingID = id;
        //	ViewBag.ApprovalStatus = approvalStatus;

        //	var vendor = _vendorService.GetVendor((long)id);
        //	vendor.IsApproved = approvalStatus;

        //	return View(vendor);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Remarks(FacilityVenueApprovalViewModel facilityVenueApprovalFormViewModel)
        //{

        //	var vendor = _vendorService.GetVendor(facilityVenueApprovalFormViewModel.ID);
        //	if (vendor == null)
        //	{
        //		return HttpNotFound();
        //	}

        //	if (facilityVenueApprovalFormViewModel.IsApproved)
        //	{
        //		vendor.IsApproved = true;
        //		vendor.IsActive = true;
        //	}
        //	else
        //	{
        //		vendor.IsApproved = false;
        //		vendor.IsActive = false;
        //	}

        //	vendor.Remarks = vendor.Remarks + "<hr />" + Helpers.TimeZone.GetLocalDateTime().ToString("dd MMM yyyy, h:mm tt") + "<br />" + facilityVenueApprovalFormViewModel.Remarks;

        //	string message = string.Empty;

        //	if (_vendorService.UpdateVendor(ref vendor, ref message))
        //	{
        //		SuccessMessage = "Vendor " + ((bool)vendor.IsActive ? "Approved" : "Rejected") + "  successfully ...";

        //		Notification not = new Notification();
        //		not.Title = "Vendor Approval";
        //		not.TitleAr = "الموافقة على المنشأة";
        //		if ((bool)vendor.IsApproved)
        //		{
        //			not.Description = "Your request have been approved ";
        //			not.Url = "/Vendor/Account/ProfileManagement";
        //		}
        //		else
        //		{
        //			not.Description = "Your request have been rejected ";
        //			not.Url = "/Vendor/Account/ProfileManagement";
        //		}
        //		not.OriginatorID = Convert.ToInt64(Session["AdminUserID"]);
        //		not.OriginatorName = Session["UserName"].ToString();
        //		not.Module = "Vendor";
        //		not.OriginatorType = "Admin";
        //		not.RecordID = vendor.ID;
        //		if (_notificationService.CreateNotification(not, ref message))
        //		{
        //			NotificationReceiver notRec = new NotificationReceiver();
        //			notRec.ReceiverID = vendor.ID;
        //			notRec.ReceiverType = "Vendor";
        //			notRec.NotificationID = not.ID;
        //			if (_notificationReceiverService.CreateNotificationReceiver(notRec, ref message))
        //			{

        //			}

        //		}


        //		return Json(new
        //		{

        //			success = true,
        //			message = SuccessMessage,
        //			//data = new
        //			//{
        //			//	ID = vendor.ID,
        //			//	Date = vendor.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
        //			//	Vendor = vendor.Logo + "|" + vendor.VendorCode + "|" + vendor.Name,
        //			//	ApprovalStatus = vendor.IsApproved,
        //			//	IsActive = vendor.IsActive.HasValue ? vendor.IsActive.Value.ToString() : bool.FalseString,
        //			//	IsApproved = vendor.IsApproved.HasValue ? vendor.IsApproved.Value.ToString() : bool.FalseString
        //			//}
        //		}, JsonRequestBehavior.AllowGet);
        //	}
        //	else
        //	{
        //		ErrorMessage = "Oops! Something went wrong. Please try later.";
        //	}

        //	return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        //}


        [HttpGet]
        public ActionResult Approve(long id, bool approvalStatus)
        {
            ViewBag.BuildingID = id;
            ViewBag.ApprovalStatus = approvalStatus;

            var vendor = _vendorService.GetVendor((long)id);
            vendor.IsApproved = approvalStatus;

            return View(vendor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(VendorApprovalFormViewModel vendorApprovalFormViewModel)
        {

            var vendor = _vendorService.GetVendor(vendorApprovalFormViewModel.ID);
            if (vendor == null)
            {
                return HttpNotFound();
            }

            if (vendorApprovalFormViewModel.IsApproved)
            {
                vendor.IsApproved = true;
                vendor.IsActive = true;
            }
            else
            {
                vendor.IsApproved = false;
                vendor.IsActive = true;
            }

            vendor.Remarks = vendor.Remarks + "<hr />" + Helpers.TimeZone.GetLocalDateTime().ToString("dd MMM yyyy, h:mm tt") + "<br />" + vendorApprovalFormViewModel.Remarks;

            string message = string.Empty;

            if (_vendorService.UpdateVendor(ref vendor, ref message, false))
            {

                var vendorUser = _vendorUserService.GetVendorUser(vendor.ID);
                if (vendorUser != null)
                {
                    vendorUser.UserType = null;
                    _vendorUserService.UpdateVendorUser(ref vendorUser, ref message);
                }

                SuccessMessage = "Vendor " + ((bool)vendor.IsApproved ? "Approved" : "Rejected") + "  successfully ...";
                //var vendor = _vendorService.GetVendor((long)vendor.VendorID);

                Notification not = new Notification();
                not.Title = "Vendor Approval";
                not.TitleAr = "موافقة البائع";
                if (vendor.IsApproved == true)
                {
                    not.Description = "Your profile approval have been approved ";
                    not.Url = "/Vendor/Dashboard/Index";
                }
                else
                {
                    not.Description = "Your profile approval have been rejected ";
                    not.Url = "/Vendor/Account/ProfileManagement";
                }
                not.OriginatorID = Convert.ToInt64(Session["AdminUserID"]);
                not.OriginatorName = Session["UserName"].ToString();
                not.Module = "Vendor";
                not.OriginatorType = "Admin";
                not.RecordID = vendor.ID;
                if (_notificationService.CreateNotification(not, ref message))
                {
                    NotificationReceiver notRec = new NotificationReceiver();
                    notRec.ReceiverID = vendor.ID;
                    notRec.ReceiverType = "Vendor";
                    notRec.NotificationID = not.ID;
                    if (_notificationReceiverService.CreateNotificationReceiver(notRec, ref message))
                    {
                    }
                }
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)vendor.IsApproved ? "Approved" : "Rejected")} vendor {vendor.Name}.");
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        ID = vendor.ID,
                        Date = vendor.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                        Vendor = vendor.Logo + "|" + vendor.Name + "|" + vendor.VendorCode,
                        Email = vendor.Email,
                        IsActive = vendor.IsActive.HasValue ? vendor.IsActive.Value.ToString() : bool.FalseString,
                        IsApproved = vendor.IsApproved.HasValue ? vendor.IsApproved.Value.ToString() : bool.FalseString
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later.";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Decline(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vendor = _vendorService.GetVendor((long)id);
            if (vendor == null)
            {
                return HttpNotFound();
            }

            vendor.IsApproved = null;
            vendor.IsActive = false;

            string message = string.Empty;
            if (_vendorService.UpdateVendor(ref vendor, ref message))
            {
                SuccessMessage = "Vendor signup request declined successfully ...";
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later.";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = _vendorService.GetVendor((Int16)id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            TempData["VendorID"] = id;
            return View(vendor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (_vendorService.DeleteVendor((Int16)id, ref message))
            {
                Vendor vendor = _vendorService.GetVendor((Int16)id);
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted vendor {vendor.Name}.");
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VendorReport()
        {
            string ImageServer = CustomURL.GetImageServer();
            var getAllVendors = _vendorService.GetVendors(true).ToList();
            if (getAllVendors.Count() > 0)
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("VendorReport");

                    var headerRow = new List<string[]>()
                    {
                    new string[] {
                        "Creation Date"
                        ,"Code"
                        ,"Name"
                        ,"Website"
                        ,"Email"
                        ,"Commission"
                        ,"Contact"
                        ,"Mobile"
                        ,"ID No"
                        ,"TRN"
                        ,"License"
                        ,"Country Name"
                        ,"City Name"
                        ,"Package Name"
                        ,"Fax"
                        ,"Address"
                        ,"About"
                        ,"Logo"
                        ,"Status"
                        }
                    };

                    // Determine the header range (e.g. A1:D1)
                    string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    // Target a worksheet
                    var worksheet = excel.Workbook.Worksheets["VendorReport"];

                    // Popular header row data
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var i in getAllVendors)
                    {
                        cellData.Add(new object[] {
                        i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
                        ,!string.IsNullOrEmpty(i.VendorCode) ? i.VendorCode : "-"
                        ,!string.IsNullOrEmpty(i.Name) ? i.Name : "-"
                        ,!string.IsNullOrEmpty(i.Website) ? i.Website : "-"
                        ,!string.IsNullOrEmpty(i.Email) ? i.Email : "-"
                        ,i.Commission != null? i.Commission : 0
                        ,!string.IsNullOrEmpty(i.Contact) ? i.Contact : "-"
                        ,!string.IsNullOrEmpty(i.Mobile) ? i.Mobile : "-"
                        ,!string.IsNullOrEmpty(i.IDNo) ? i.IDNo : "-"
                        ,!string.IsNullOrEmpty(i.TRN) ? i.TRN : "-"
                        ,!string.IsNullOrEmpty(i.License) ? i.License : "-"
                        ,!string.IsNullOrEmpty(i.CustomerCountry) ? i.CustomerCountry : "-"
                        ,!string.IsNullOrEmpty(i.CustomerCity) ? i.CustomerCity: "-"
                        ,!string.IsNullOrEmpty(i.VendorPackage.Name) ? i.VendorPackage.Name : "-"
                        ,!string.IsNullOrEmpty(i.FAX) ? i.FAX : "-"
                        ,!string.IsNullOrEmpty(i.Address) ? i.Address : "-"
                        ,!string.IsNullOrEmpty(i.About) ? i.About : "-"
                        ,!string.IsNullOrEmpty(i.Logo) ? (ImageServer + i.Logo) : "-"
                        ,i.IsActive == true ? "Active" : "InActive"
                        });
                    }

                    worksheet.Cells[2, 1].LoadFromArrays(cellData);

                    return File(excel.GetAsByteArray(), "application/msexcel", "Vendor Report.xlsx");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult CreateDocuments(long? id)
        {
            ViewBag.VendorID = id;
            Session["VendorIDForDocument"] = ViewBag.VendorID;
            return View();
        }

        [HttpPost]
        public ActionResult CreateDocuments(string Name)
        {
            if (Name != string.Empty)
            {
                long vendorId = (long)Session["VendorIDForDocument"];

                string message = string.Empty;
                VendorDocument data = new VendorDocument();
                data.VendorID = vendorId;
                string absolutePath = Server.MapPath("~");
                string relativePath = string.Format("/Assets/AppFiles/Documents/Vendors/{0}/", vendorId.ToString().Replace(" ", "_"));
                data.Name = Name;
                data.Path = Uploader.UploadDocs(Request.Files, absolutePath, relativePath, "Document", ref message, "FileUpload");

                if (Request.Files.Count == 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please fill the form correctly",


                    }, JsonRequestBehavior.AllowGet);
                }
                if (_vendorDocumentService.CreateDocument(ref data, ref message))
                {

                }

                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created vendor document {data.Name}.");
                return Json(new
                {
                    success = true,
                    message = "Document added successfully!",
                    name = data.Name,
                    path = data.Path,
                    id = data.ID

                }, JsonRequestBehavior.AllowGet); ;
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Please fill the form correctly!",


                }, JsonRequestBehavior.AllowGet); ;
            }

        }
        [HttpPost, ActionName("DeleteVendorDocument")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCarDocument(long id)
        {

            string message = string.Empty;
            _vendorDocumentService.DeleteDocument(id, ref message);
            log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} vendor document deleted.");
            return Json(new { success = true, data = id, message });
        }

        [HttpGet]
        public ActionResult GetDocuments(long id)
        {
            var document = _vendorDocumentService.GetDocumentByVendorID(id);


            return Json(new
            {
                success = true,
                message = "Data recieved successfully!",
                document = document.Select(i => new
                {
                    name = i.Name,
                    path = i.Path,
                    id = i.ID
                })

            }, JsonRequestBehavior.AllowGet);
        }
    }
}