using MyProject.Data;
using MyProject.Service;
using MyProject.Service.Helpers;
using MyProject.Web.Helpers;
using MyProject.Web.Helpers.Routing;
using MyProject.Web.ViewModels;
using MyProject.Web.ViewModels.Blog;
using MyProject.Web.ViewModels.JobCandidate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyProject.Web.Controllers
{
    public class ContactUsController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly IMail _mail;
        private readonly IBusinessSettingService _businessSettingService;
        private readonly IEnquiryService _enquiryService;

        public ContactUsController(IBusinessSettingService businessSettingService, IEnquiryService enquiryService, IMail mail)
        {
            this._businessSettingService = businessSettingService;
            this._enquiryService = enquiryService;
            this._mail = mail;
        }

        [Route("contact-us", Name = "contact-us")]
        public ActionResult Index()
        {
            BusinessSetting businessSetting = new BusinessSetting();
            var setting = _businessSettingService.GetDefaultBusinessSetting();
            businessSetting = setting != null ? setting : businessSetting;
            return View(businessSetting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ContactUs/Create", Name = "contactus/create")]
        public ActionResult Create(Enquiry enquiry)
        {
            string message = string.Empty;
            string description = string.Empty;
            bool status = false;

            if (ModelState.IsValid)
            {
                try
                {
                    var path = Server.MapPath("~/");
                    if (_mail.SendContactUsMail(enquiry.FullName, enquiry.Email, enquiry.Subject, enquiry.PhoneCode+enquiry.Contact, enquiry.Message))
                    {
                        //Get Customer ID
                        if (Session["CustomerID"] != null)
                            enquiry.CustomerID = Convert.ToInt64(Session["CustomerID"].ToString());

                        _enquiryService.CreateContactEnquiry(ref enquiry, ref message);

                        status = true;
                        message = "Message sent successfully.";
                    }
                    else
                    {
                        status = false;
                        //message = "";
                    }
                }
                catch (Exception ex)
                {
                    status = false;
                    message = "Message not sent due to server error !";
                }
            }
            else
            {
                status = false;
                message = "Message not sent due to server error !";
                description = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }

            return Json(new { success = status, message, description }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("contact-setting", Name = "contact-setting")]
        public ActionResult DefaultSetting(string culture = "en-ae")
        {
            string lang = "en";
            if (culture.Contains('-'))
                lang = culture.Split('-')[0];

            try
            {
                var contact = _businessSettingService.GetDefaultBusinessSetting();

                Session["MapKey"] = contact.GoogleMapKey;

                return Json(new
                {
                    success = true,
                    message = "Data recieved successfully!",
                    data = new
                    {
                        contact.ID,
                        contact.Title,
                        contact.TitleAr,
                        contact.Whatsapp,
                        FirstMessage = lang == "en" ? contact.FIrstMessage : contact.FIrstMessage,/*Ar*/
                        //contact.FirstMessageAr,
                        contact.MapIframe,
                        StreetAddress = lang == "en" ? contact.StreetAddress : contact.StreetAddressAr,
                        contact.Contact,
                        contact.Contact2,
                        contact.Fax,
                        contact.Email,
                        contact.WorkingDays,
                        contact.Facebook,
                        contact.Instagram,
                        contact.Youtube,
                        contact.Twitter,
                        contact.Snapchat,
                        contact.LinkedIn,
                        Behance = contact.Behnace,
                        contact.Pinterest,
                        contact.AndroidAppUrl,
                        contact.IosAppUrl,
                        contact.GoogleMapKey,

                        //contact.WhatsappEnabled,
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Customer Enquiry

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ContactUs/RegisterCustomer", Name = "contactus/registercustomer")]
        public ActionResult RegisterCustomer(Enquiry enquiry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string message = "";
                    var path = Server.MapPath("~/");
                    if (_mail.SendCustomerEnquiryConfirmationMail(enquiry.FullName, enquiry.Email, path))
                    {
                        _mail.SendCustomerEnquiryMail(enquiry.FullName, enquiry.Contact, enquiry.Email, path);
                        //Get Customer ID
                        if (Session["CustomerID"] != null)
                            enquiry.CustomerID = Convert.ToInt64(Session["CustomerID"].ToString());

                        _enquiryService.CreateCustomerEnquiry(ref enquiry, ref message);

                        return Json(new
                        {
                            success = true,
                            message = message
                        });
                    }
                }
                else
                {
                    var description = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return Json(new
                    {
                        success = false,
                        message = "",
                        description = description
                    });
                }
                return Json(new
                {
                    success = false,
                    message = ""
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ""
                });
            }
        }

        #endregion
    }
}