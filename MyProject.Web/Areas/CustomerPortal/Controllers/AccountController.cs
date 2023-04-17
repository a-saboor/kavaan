using MyProject.Data;
using MyProject.Service;
using MyProject.Web.Areas.CustomerPortal.ViewModels.Account;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Controllers;
using MyProject.Web.Helpers;
using MyProject.Web.Helpers.Encryption;
using MyProject.Web.Helpers.Routing;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Web.Areas.CustomerPortal.Controllers
{
    public class AccountController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly ICustomerService _customerService;

        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IAreaService _areaService;
        private readonly IBusinessSettingService _businessSettingService;
        private readonly INotificationService _notificationService;
        private readonly INotificationReceiverService _notificationReceiverService;

        public AccountController(ICustomerService customerService, IAreaService areaService, ICountryService countryService, ICityService cityService, IBusinessSettingService businessSettingService, INotificationService notificationService, INotificationReceiverService notificationReceiverService)
        {
            this._customerService = customerService;
            this._countryService = countryService;
            this._cityService = cityService;
            this._areaService = areaService;
            this._businessSettingService = businessSettingService;
            this._notificationService = notificationService;
            this._notificationReceiverService = notificationReceiverService;
        }

        public ActionResult Login(string returnUrl)
        {
            if (TempData["ReturnUrl"] == null && !string.IsNullOrEmpty(returnUrl))
                TempData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("customer/login", Name = "customer/login")]
        public async Task<ActionResult> LoginAsync(CustomerLoginViewModel Login)
        {
            bool verifyByEmail = false;
            bool verifyByContact = false;
            bool verifyAgain = false;
            string message = string.Empty;
            string status = string.Empty;
            string url = string.Empty;

            if (ModelState.IsValid)
            {
                if (Login.PhoneCode.Equals("971") || Login.PhoneCode.Equals("+971"))//In dubai, contact verify by otp verification
                    verifyByContact = true;
                else
                    verifyByEmail = true;

                var customer = _customerService.Authenticate(Login.Email, Login.Password, ref message, ref verifyByEmail, ref verifyByContact, ref verifyAgain);
                if (customer != null)
                {
                    CustomerSessionHelper.setAuthorizeSessions(customer);

                    var cookie = Request.Cookies.Get("_culture");

                    url = string.Empty;
                    if (!string.IsNullOrEmpty(Login.ReturnUrl))
                        url = Login.ReturnUrl;
                    else
                        url = cookie != null && cookie.Value == "ar-ae" ? "/Customer/Dashboard/Index" : "/Customer/Dashboard/Index";

                    return Json(new
                    {
                        success = true,
                        url = url,
                        message = message,
                        CustomerName = customer.UserName,
                        Photo = customer.Logo,
                    });
                }
                else if (verifyAgain)
                {
                    customer = _customerService.GetCustomerByEmail(Login.Email);
                    CustomerSessionHelper.setSignupSessions(customer);
                    bool isOTPSent = false;
                    double time = 5;

                    if (customer != null)
                    {
                        if (customer.IsContactOTPAccess == true)
                        {
                            time = 2.1;
                            isOTPSent = await _customerService.SendOTP(customer.Contact, time);
                            if (isOTPSent)
                            {
                                message = "OTP sent successfully. Please verify OTP for further process.";
                                //Status = true;
                            }
                            else
                            {
                                message = "OTP not sent due to some server error.";
                                //Error = "OTP Sending Gateway Failed.";
                            }
                        }
                        else if (!string.IsNullOrEmpty(customer.Email))
                        {
                            isOTPSent = _customerService.SendOTPEmail(customer.Email, time);
                            if (isOTPSent)
                            {
                                message = "OTP Email sent successfully. Please check your email to verify OTP for further process.";
                                //Status = true;
                            }
                            else
                            {
                                message = "OTP Email not sent due to some server error.";
                                //Error = "OTP Sending Email Failed.";
                            }
                        }
                        else
                        {
                            message = "Customer don't have any access to get OTP.";
                        }
                    }
                    url = "/customer/account/verifyotp";
                }
            }
            else
            {
                message = "Please fill the form properly!";
            }

            return Json(new
            {
                success = false,
                PhoneCode = Login.PhoneCode,
                Contact = Login.Contact,
                verifyByEmail,
                verifyByContact,
                verifyAgain,
                message = message,
                url
            });
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("customer/signup", Name = "customer/signup")]
        public async Task<ActionResult> Signup(CustomerSignupViewModel signupViewModel, string culture)
        {
            try
            {
                bool verifyByEmail = false;
                bool verifyByContact = false;
                bool isOTPSent = false;
                string message = string.Empty;
                string status = string.Empty;

                if (ModelState.IsValid)
                {
                    Customer customer = new Customer();
                    if (_customerService.GetCustomerByEmail(signupViewModel.Email) == null)
                    {
                        //var customerLoyaltySetting = _customerLoyaltySettingService.GetList().Where(i => i.IsActive == true).OrderBy(i => i.CustomerTypeMaxSlab).FirstOrDefault();

                        //if (signupViewModel.PhoneCode.Equals("971") || signupViewModel.PhoneCode.Equals("+971"))
                        //{
                        //	//verify by OTP Verification
                        //	verifyByContact = true;
                        //	customer.IsContactOTPAccess = true;
                        //	customer.IsEmailVerified = null;
                        //}
                        //else
                        //{
                        //verify by Email Verification
                        verifyByEmail = true;
                        customer.IsContactOTPAccess = false;
                        //}
                        customer.UserName = signupViewModel.FirstName;
                        customer.FirstName = signupViewModel.FirstName;
                        customer.LastName = signupViewModel.LastName;
                        customer.PhoneCode = signupViewModel.PhoneCode;
                        customer.Contact = signupViewModel.Contact;
                        customer.Email = signupViewModel.Email;
                        customer.Password = signupViewModel.Password;
                        customer.AccountType = "Basic";
                        customer.Logo = CustomURL.GetImageServer() + "assets/AppFiles/Customer/default.png";

                        var path = Server.MapPath("~/");
                        if (_customerService.CreateCustomer(ref customer, path, ref message, false, verifyByEmail))
                        {
                            CustomerSessionHelper.setSignupSessions(customer);

                            if (customer.IsContactOTPAccess.HasValue && customer.IsContactOTPAccess.Value)
                            {
                                //verify by OTP Verification
                                isOTPSent = await _customerService.SendOTP(customer.Contact);
                                if (isOTPSent)
                                    message = "Account created! Please wait for OTP verification";
                                else
                                    message = "Account created! Please click login for further process.";
                            }
                            else
                            {
                                //verify by Email Verification
                                message = "Account created! Please check your email for OTP verification.";
                            }

                            //message = Resources.Resources.PleaseWaitAMoment;
                            string url = "/customer/account/verifyOTP";
                            return Json(new
                            {
                                success = true,
                                url = url,
                                contact = customer.Contact,
                                email = customer.Email,
                                message = message,
                                isOTPSent = isOTPSent,
                                verifyByEmail,
                                verifyByContact
                            });
                        }
                    }
                    else
                    {
                        message = "Eamil already exist! Please use another Email Address.";
                    }
                }
                else
                {
                    var description = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return Json(new
                    {
                        success = false,
                        message = "Please fill the form properly!",
                        description = description
                    });
                }

                return Json(new
                {
                    success = false,
                    message = message
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong! Please try later.",
                });
            }
        }

        public ActionResult VerifyOTP()
        {
            CustomerVerifyOTPViewModel verifyOTPViewModel = new CustomerVerifyOTPViewModel()
            {
                PhoneCode = CustomerSessionHelper.PhoneCode,
                Contact = CustomerSessionHelper.Contact,
                Email = CustomerSessionHelper.Email,
                UserName = CustomerSessionHelper.UserName,
                OTPExpired = CustomerSessionHelper.AuthorizationExpiry <= DateTime.Now ? false : true,
            };

            return View(verifyOTPViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult verifyOTP(CustomerVerifyOTPViewModel verifyOTPViewModel)
        {
            try
            {
                string Description = string.Empty;
                string Message = string.Empty;
                string Status = string.Empty;
                string url = string.Empty;
                if (ModelState.IsValid)
                {
                    var customer = _customerService.GetCustomerByEmail(verifyOTPViewModel.Email);
                    if (customer != null)
                    {
                        string msg = "";
                        string sts = "";

                        if (customer.IsContactVerified.HasValue && customer.IsContactVerified.Value)
                        {
                            if (customer.OTP.Equals(verifyOTPViewModel.OTP))
                            {
                                customer.IsContactVerified = true;
                                _customerService.UpdateCustomer(ref customer, ref msg, ref sts, false);

                                if ((bool)verifyOTPViewModel.AutoLogin)
                                {
                                    CustomerSessionHelper.setAuthorizeSessions(customer);

                                    url = "/Customer/Dashboard/Index";
                                }

                                return Json(new
                                {
                                    success = true,
                                    url = url,
                                    message = "Login Successfull",
                                    CustomerName = customer.UserName,
                                    CustomerEmail = customer.Email,
                                    Photo = customer.Logo
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    success = false,
                                    message = "Invalid OTP"
                                });
                            }
                        }
                        else
                        {
                            //email verification
                            if (customer.OTP.Equals(verifyOTPViewModel.OTP))
                            {
                                customer.IsEmailVerified = true;
                                _customerService.UpdateCustomer(ref customer, ref msg, ref sts, false);

                                if ((bool)verifyOTPViewModel.AutoLogin)
                                {
                                    CustomerSessionHelper.setAuthorizeSessions(customer);

                                    url = "/Customer/Dashboard/Index";
                                }

                                return Json(new
                                {
                                    success = true,
                                    url = url,
                                    message = "Login Successfull",
                                    CustomerName = customer.UserName,
                                    CustomerEmail = customer.Email,
                                    Photo = customer.Logo
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    success = false,
                                    message = "Invalid OTP"
                                });
                            }
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Something went wrong! Please try later.",
                        });
                    }
                }
                else
                {
                    string message = string.Empty;
                    Description = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                    if (verifyOTPViewModel.OTP < 1)
                    {
                        message = "OTP required !";
                    }
                    else if (string.IsNullOrEmpty(verifyOTPViewModel.Email))
                    {
                        message = "Email Adress required !";
                    }
                    else if (string.IsNullOrEmpty(verifyOTPViewModel.Contact))
                    {
                        message = "Mobile Number required !";
                    }
                    else
                    {
                        message = "Something went wrong! Please try later.";
                    }

                    return Json(new
                    {
                        success = false,
                        status = "error",
                        message = message,
                    });
                }
            }

            catch (Exception ex)

            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong! Please try later."
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> resendOTP(string email)
        {
            string Message = string.Empty;
            string Error = string.Empty;
            string Description = string.Empty;
            bool IsContactOTPAccess = false;
            bool Status = false;
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    bool isOTPSent = false;
                    double time = 5;

                    Customer customer = _customerService.GetCustomerByEmail(email);
                    if (customer != null)
                    {
                        if (customer.IsContactOTPAccess == true)
                        {
                            time = 2.1;
                            IsContactOTPAccess = true;
                            isOTPSent = await _customerService.SendOTP(customer.Contact, time);
                            if (isOTPSent)
                            {
                                Message = "OTP sent successfully. Please verify OTP for further process.";
                                Status = true;
                            }
                            else
                            {
                                Message = "OTP not sent due to some server error.";
                                Error = "OTP Sending Gateway Failed.";
                            }
                        }
                        else if (!string.IsNullOrEmpty(customer.Email))
                        {
                            isOTPSent = _customerService.SendOTPEmail(customer.Email, time);
                            if (isOTPSent)
                            {
                                Message = "OTP Email sent successfully. Please check your email to verify OTP for further process.";
                                Status = true;
                            }
                            else
                            {
                                Message = "OTP Email not sent due to some server error.";
                                Error = "OTP Sending Email Failed.";
                            }
                        }
                        else
                        {
                            Message = "Customer don't have any access to get OTP.";
                            Description = "Contact Number is not supported for OTP and also Customer doesn't have an email address.";
                        }
                    }
                    else
                    {
                        Message = "Invalid Email Address.";
                    }
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid Email Address.",
                        isOTPSent = false,
                    });
                }
            }
            catch (Exception ex)
            {
                Message = "Oops! Something went wrong. Please try later.";
                Error = ex.Message;
            }
            return Json(new
            {
                IsContactOTPAccess = IsContactOTPAccess,
                success = Status,
                message = Message,
                error = Error,
                description = Description
            });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> resendOTP(string PhoneCode, string contact)
        //{
        //    string Message = string.Empty;
        //    string Error = string.Empty;
        //    string Description = string.Empty;
        //    bool IsContactOTPAccess = false;
        //    bool Status = false;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(contact))
        //        {
        //            bool isOTPSent = false;
        //            double time = 15;

        //            Customer customer = _customerService.GetByContactAndPhoneCode(contact, PhoneCode);
        //            if (customer != null)
        //            {
        //                if (customer.IsContactOTPAccess == true)
        //                {
        //                    time = 2.1;
        //                    IsContactOTPAccess = true;
        //                    isOTPSent = await _customerService.SendOTP(customer.Contact, time);
        //                    if (isOTPSent)
        //                    {
        //                        Message = "OTP sent successfully. Please verify OTP for further process.";
        //                        Status = true;
        //                    }
        //                    else
        //                    {
        //                        Message = "OTP not sent due to some server error.";
        //                        Error = "OTP Sending Gateway Failed.";
        //                    }
        //                }
        //                else if (!string.IsNullOrEmpty(customer.Email))
        //                {
        //                    isOTPSent = _customerService.SendOTPEmail(customer.Email, time);
        //                    if (isOTPSent)
        //                    {
        //                        Message = "OTP Email sent successfully. Please check your email to verify OTP for further process.";
        //                        Status = true;
        //                    }
        //                    else
        //                    {
        //                        Message = "OTP Email not sent due to some server error.";
        //                        Error = "OTP Sending Email Failed.";
        //                    }
        //                }
        //                else
        //                {
        //                    Message = "Customer don't have any access to get OTP.";
        //                    Description = "Contact Number is not supported for OTP and also Customer doesn't have an email address.";
        //                }
        //            }
        //            else
        //            {
        //                Message = "Invalid Contact Number.";
        //            }
        //        }
        //        else
        //        {
        //            return Json(new
        //            {
        //                success = false,
        //                message = "Invalid Contact Number.",
        //                isOTPSent = false,
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message = "Oops! Something went wrong. Please try later.";
        //        Error = ex.Message;
        //    }
        //    return Json(new
        //    {
        //        IsContactOTPAccess = IsContactOTPAccess,
        //        success = Status,
        //        message = Message,
        //        error = Error,
        //        description = Description
        //    });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Referral(CustomerReferralViewModel referral)
        {
            try
            {
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    var customer = _customerService.GetCustomerByEmail(referral.Email);
                    if (customer != null)
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Data recieved successfully!",
                            ReferralID = customer.ID,
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Invalid Referral!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "The email address is required!"
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong! Please try later."
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(CustomerForgotPasswordViewModel forgotPasswordViewModel)
        {
            string Message = string.Empty;
            string Status = string.Empty;
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/");
                if (_customerService.ForgotPassword(forgotPasswordViewModel.Email, path, ref Status, ref Message))
                {
                    return Json(new { success = true, message = Message });
                }
                else
                {
                    ErrorMessage = Message;
                }
            }
            else
            {
                ErrorMessage = "Please fill the form properly ...";
            }


            return Json(new { success = false, message = ErrorMessage });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("customer/email-verification", Name = "customer/email-verification")]
        public ActionResult VerifyEmail(CustomerForgotPasswordViewModel account, string culture)
        {
            string Message = string.Empty;
            string Status = string.Empty;
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/");
                if (_customerService.VerifyEmail(account.Email, path, culture, ref Status, ref Message))
                {
                    return Json(new { success = true, message = Message });
                }
                else
                {
                    ErrorMessage = Message;
                }
            }
            else
            {
                ErrorMessage = "Please fill the form properly ...";
            }


            return Json(new { success = false, message = ErrorMessage });
        }

        public ActionResult Verify()
        {
            var AuthCode = Request.QueryString["auth"];
            if (AuthCode == "" || AuthCode == null)
            {
                ViewBag.ErrorMessage = "Authorization failed! Please verify again or contact KAVAAN Facilities support.";
            }
            else
            {
                string message = string.Empty;
                string status = string.Empty;
                var customer = _customerService.GetByAuthCode(AuthCode);
                if (customer != null)
                {
                    if (customer.AuthorizationExpiry >= Helpers.TimeZone.GetLocalDateTime())
                    {
                        Crypto objCrypto = new Helpers.Encryption.Crypto();

                        customer.IsEmailVerified = true;
                        customer.AuthorizationCode = objCrypto.Random(225);
                        customer.AuthorizationExpiry = null;
                        if (_customerService.UpdateCustomer(ref customer, ref message, ref status))
                        {
                            ViewBag.SuccessMessage = "Your Account is Verified!";

                            Session["CustomerID"] = customer.ID;
                            Session["CustomerName"] = !string.IsNullOrEmpty(customer.UserName) ? customer.UserName : customer.FirstName + customer.LastName;
                            Session["Contact"] = customer.Contact;
                            Session["Email"] = customer.Email;
                            Session["Photo"] = customer.Logo;
                            Session["Points"] = customer.Points;
                            Session["AccountType"] = customer.AccountType;
                            Session["ReceiverType"] = "Customer";

                            Response.Cookies["Customer-Details"]["Name"] = CustomerSessionHelper.UserName;
                            Response.Cookies["Customer-Details"]["Logo"] = CustomerSessionHelper.Photo;
                            Response.Cookies["Customer-Session"]["Access-Token"] = customer.AuthorizationCode;
                            //var cookie = Request.Cookies.Get("_culture");

                            //string url = cookie != null && cookie.Value == "ar-ae" ? "/ar-ae/Customer/Dashboard/Index" : "/en-ae/Customer/Dashboard/Index";

                            //return Json(new
                            //{
                            //    success = true,
                            //    url = url,
                            //    message = message,
                            //    CustomerName = customer.UserName,
                            //    Photo = customer.Logo,
                            //});
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Something went wrong! Please try later.";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Authorization failed due to Session expired! Please verify again or contact KAVAAN Facilities support.";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Authorization failed! Please verify again or contact KAVAAN Facilities support.";
                }
            }

            return View();
        }

        [AuthorizeCustomer]
        public ActionResult ChangePassword()
        {
            long customerId = Convert.ToInt64(Session["CustomerID"]);
            var customer = _customerService.GetCustomer(customerId);

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeCustomer]
        public ActionResult ChangePassword(CustomerChangePasswordViewModel changePasswordViewModel)
        {
            string Message = string.Empty;
            string description = string.Empty;
            string Status = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {

                    long CustomerId = Convert.ToInt64(Session["CustomerID"]);

                    if (_customerService.ChangePassword(changePasswordViewModel.CurrentPassword, changePasswordViewModel.NewPassword, CustomerId, ref Status, ref Message))
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Password changed succesfully",
                        });
                    }
                }
                else
                {
                    description = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return Json(new
                    {
                        success = false,
                        message = "Please fill the form properly!",
                        description = description
                    });
                }
                return Json(new { success = false, message = Message, description });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong! Please try later.",
                    description
                });
            }
        }

        //[HttpGet]
        //new public ActionResult Profile()
        //{
        //	long customerId = Convert.ToInt64(Session["CustomerID"]);
        //	Customer customer = _customerService.GetCustomer((long)customerId);

        //	ViewBag.CountryID = new SelectList(_countryService.GetCountriesForDropDown(), "value", "text");
        //	ViewBag.CityID = new SelectList(_cityService.GetCitiesForDropDown(), "value", "text");
        //	ViewBag.AreaID = new SelectList(_areaService.GetAreasForDropDown(), "value", "text");


        //	return View(customer);
        //}

        [HttpGet]
        [AuthorizeCustomer]
        new public ActionResult Profile(string culture = "en-ae")
        {
            Customer customer = _customerService.GetCustomer(CustomerSessionHelper.ID);
            return View(customer);

            //string lang = "en";
            //if (culture.Contains('-'))
            //    lang = culture.Split('-')[0];

            //var countries = new SelectList(_countryService.GetCountriesForDropDown(lang), "value", "text", customer.CountryID);
            //var cities = new SelectList(_cityService.GetCitiesForDropDown(), "value", "text", customer.CityID);
            //var areas = new SelectList(_areaService.GetAreasForDropDown(), "value", "text", customer.AreaID);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeCustomer]
        new public ActionResult Profile(Customer viewModel)
        {
            string message = string.Empty;
            string status = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    Customer customer = _customerService.GetCustomer(CustomerSessionHelper.ID);

                    //if (_customerService.GetByContact(viewModel.Contact, customerId) != null)
                    //{
                    //customer.UserName = viewModel.FirstName + " " + viewModel.LastName;
                    customer.FirstName = viewModel.FirstName;
                    customer.LastName = viewModel.LastName;
                    customer.Email = viewModel.Email;
                    customer.PhoneCode = viewModel.PhoneCode;
                    customer.Contact = viewModel.Contact;
                    customer.PoBox = viewModel.PoBox;
                    customer.ZipCode = viewModel.ZipCode;
                    customer.EmiratesID = viewModel.EmiratesID;
                    customer.EmiratesIDExpiry = viewModel.EmiratesIDExpiry;
                    customer.PassportNo = viewModel.PassportNo;
                    customer.PassportExpiry = viewModel.PassportExpiry;
                    customer.AreaID = viewModel.AreaID;
                    customer.CityID = viewModel.CityID;
                    customer.CountryID = viewModel.CountryID;
                    customer.Address = viewModel.Address;
                    customer.Address2 = viewModel.Address2;
                    customer.CustomerCountry = viewModel.CustomerCountry;
                    customer.CustomerCity = viewModel.CustomerCity;

                    if (Request.Files.Count > 0)
                    {
                        //string FilePath = string.Format("{0}{1}{2}", Server.MapPath("~/Assets/AppFiles/Customer/"), viewModel.ImagePath);

                        string absolutePath = Server.MapPath("~");
                        string relativePath = string.Format("/Assets/AppFiles/Customer/");

                        customer.Logo = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "image", ref message, "ImagePath");
                    }

                    if (_customerService.UpdateCustomer(ref customer, ref message, ref status))
                    {

                        //Session["CustomerID"] = customer.ID;
                        //Session["CustomerName"] = !string.IsNullOrEmpty(customer.UserName) ? customer.UserName : customer.FirstName + " " + customer.LastName;
                        //Session["Contact"] = customer.Contact;
                        //Session["Email"] = customer.Email;
                        //Session["Photo"] = customer.Logo;
                        //Session["Points"] = customer.Points;
                        //Session["AccountType"] = customer.AccountType;
                        //Session["ReceiverType"] = "Customer";

                        //Response.Cookies["Customer-Details"]["Name"] = CustomerSessionHelper.UserName;
                        //Response.Cookies["Customer-Details"]["Logo"] = CustomerSessionHelper.Photo;
                        //Response.Cookies["Customer-Session"]["Access-Token"] = customer.AuthorizationCode;

                        return Json(new
                        {
                            customer = new
                            {
                                customer.ID,
                                customer.UserName,
                                customer.FirstName,
                                customer.LastName,
                                customer.Email,
                                customer.Logo,
                            },
                            success = true,
                            message = "Profile updated successfully",
                        });
                    }
                    //}
                    //else
                    //{
                    //	message = "Account with same email already exist!";
                    //}
                    //}
                    //else
                    //{
                    //	//message = "Account with same contact no already exist!";
                    //	message = "Something went wrong! Please try later.";
                    //}
                }
                else
                {
                    var description = string.Join(" , ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return Json(new
                    {
                        success = false,
                        message = "Please fill the form properly!",
                        description = description
                    });
                }
            }
            catch (Exception ex)
            {
                message = "Something went wrong! Please try later.";
            }

            return Json(new { success = false, message = message });
        }

        [HttpPost]
        public ActionResult ProfileImage()
        {
            string image = string.Empty;
            string message = string.Empty;
            string msg = string.Empty;
            bool status = false;

            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    Customer customer = _customerService.GetCustomer(CustomerSessionHelper.ID);
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        string absolutePath = Server.MapPath("~");
                        if (file != null)
                        {
                            string relativePath = string.Format("/Assets/AppFiles/Customer/{0}/", customer.ID);
                            customer.Logo = Uploader.UploadImage(files, absolutePath, relativePath, "Customer-", ref message, fname);
                            if (!string.IsNullOrEmpty(customer.Logo))
                            {
                                customer.Logo = CustomURL.GetImageServer() + customer.Logo.Remove(0, 1);
                            }
                        }
                    }

                    if (_customerService.UpdateCustomer(ref customer, ref message, ref msg))
                    {
                        image = customer.Logo;
                        Session["Photo"] = customer.Logo;
                        // Returns message that successfully uploaded  
                        status = true;
                        message = "Image Updated Successfully ...";
                    }
                    else
                    {
                        message = "Something went wrong! Please try later.";
                    }
                }
                catch (Exception ex)
                {
                    message = "Error occurred. Error details: " + ex.Message;
                }
            }
            else
            {
                message = "No image selected ...";
            }

            return Json(new { success = status, message = message, image });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Route("customer/resetpassword", Name = "customer/resetpassword")]
        //public async Task<ActionResult> ResetPassword(CustomerResetPasswordViewModel customerResetPasswordViewModel)
        //{
        //    string Message = string.Empty;
        //    string Error = string.Empty;
        //    string Description = string.Empty;
        //    bool IsContactOTPAccess = false;
        //    bool Status = false;

        //    if (ModelState.IsValid)
        //    {
        //        bool isOTPSent = false;
        //        double time = 15;
        //        try
        //        {
        //            Customer customer = _customerService.GetByContact(customerResetPasswordViewModel.Contact);
        //            if (customer != null)
        //            {
        //                if (customer.IsContactOTPAccess == true)
        //                {
        //                    time = 2.1;
        //                    IsContactOTPAccess = true;
        //                    isOTPSent = await _customerService.SendOTP(customer.Contact, time);
        //                    if (isOTPSent)
        //                    {
        //                        Message = "OTP sent successfully. Please verify OTP for further process.";
        //                        Status = true;
        //                    }
        //                    else
        //                    {
        //                        Message = "OTP not sent due to some server error.";
        //                        Error = "OTP Sending Gateway Failed.";
        //                    }
        //                }
        //                else if (!string.IsNullOrEmpty(customer.Email))
        //                {
        //                    isOTPSent = _customerService.SendOTPEmail(customer.Email, time);
        //                    if (isOTPSent)
        //                    {
        //                        Message = "OTP Email sent successfully. Please check your email to verify OTP for further process.";
        //                        Status = true;
        //                    }
        //                    else
        //                    {
        //                        Message = "OTP Email not sent due to some server error.";
        //                        Error = "OTP Sending Email Failed.";
        //                    }
        //                }
        //                else
        //                {
        //                    Message = "Customer don't have any access to get OTP.";
        //                    Description = "Contact Number is not supported for OTP and also Customer doesn't have an email address.";
        //                }
        //            }
        //            else
        //            {
        //                Message = "Invalid Contact Number.";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Message = "Oops! Something went wrong. Please try later.";
        //            Error = ex.Message;
        //        }
        //    }
        //    else
        //    {
        //        Message = "Please fill the form properly ...";
        //        Error = "Invalid Model State.";
        //        Description = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }

        //    return Json(new
        //    {
        //        IsContactOTPAccess = IsContactOTPAccess,
        //        success = Status,
        //        message = Message,
        //        error = Error,
        //        description = Description
        //    });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("customer/newpassword", Name = "customer/newpassword")]
        public ActionResult NewPassword(CustomerNewPasswordViewModel customerNewPasswordViewModel)
        {
            string Message = string.Empty;
            string Error = string.Empty;
            string Description = string.Empty;
            bool Status = false;

            if (ModelState.IsValid)
            {
                try
                {
                    Customer customer = _customerService.GetByContact(customerNewPasswordViewModel.Contact);
                    if (customer != null)
                    {
                        if (customerNewPasswordViewModel.OTP == customer.OTP)
                        {
                            if (_customerService.ResetPassword(customerNewPasswordViewModel.Password, customer.ID, ref Message))
                            {
                                Status = true;
                            }
                            else
                            {
                                Error = "Password reset failed!";
                            }
                        }
                        else
                        {
                            Message = "Please verify OTP first to proceed.";
                            Error = "Invalid OTP!";
                        }
                    }
                    else
                    {
                        Message = "Invalid Contact Number.";
                        Error = "Invalid Credentials.";
                    }
                }
                catch (Exception ex)
                {
                    Message = "Oops! Something went wrong. Please try later.";
                    Error = ex.Message;
                }
            }
            else
            {
                Message = "Please fill the form properly ...";
                Error = "Invalid Model State.";
                Description = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }

            return Json(new
            {
                success = Status,
                message = Message,
                error = Error,
                description = Description
            });

        }

        public ActionResult ResetPassword()
        {
            var AuthCode = Request.QueryString["auth"];
            if (AuthCode == "" || AuthCode == null)
            {
                ViewBag.ErrorMessage = "Invalid Session!";
            }
            else
            {
                var customer = _customerService.GetByAuthCode(AuthCode);
                if (customer != null)
                {
                    if (customer.AuthorizationExpiry >= Helpers.TimeZone.GetLocalDateTime())
                    {
                        Session["CustomerResetID"] = customer.ID;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Session expired!";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Session!";
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(CustomerResetPasswordViewModel resetPasswordViewModel)
        {
            string Message = string.Empty;

            if (ModelState.IsValid)
            {
                if (Session["CustomerResetID"] != null)
                {
                    long CustomerId = Convert.ToInt64(Session["CustomerResetID"]);

                    if (_customerService.ResetPassword(resetPasswordViewModel.NewPassword, CustomerId, ref Message))
                    {
                        Session.Remove("CustomerResetID");
                        string url = "/customer/login";
                        return Json(new { success = true, url = url, message = Message });
                    }
                    else
                    {
                        ErrorMessage = Message;
                    }
                }
                else
                {
                    ErrorMessage = "Session expired!";
                }
            }
            else
            {
                ErrorMessage = "Please fill the form properly ...";
            }
            return Json(new { success = false, message = ErrorMessage });
        }

        public ActionResult Logout(string culture = "en-ae")
        {
            Session.Remove("CustomerID");
            Session.Remove("CustomerName");
            Session.Remove("Contact");
            Session.Remove("Email");
            Session.Remove("Photo");
            Session.Remove("Points");
            Session.Remove("AccountType");
            Session.Remove("ReceiverType");

            Session.Remove("Access-Token");
            Request.Cookies.Remove("Customer-Session");
            Response.Cookies.Remove("Customer-Session");
            Request.Cookies.Remove("_bookingUrl");
            Response.Cookies.Remove("_bookingUrl");
            Session.Abandon();
            Session.Clear();

            Response.Cookies["Customer-Session"]["Access-Token"] = "";
            Response.Cookies["Customer-Details"]["Name"] = "";
            Response.Cookies["Customer-Details"]["Logo"] = "";

            var returnUrl = $"/home/index";
            if (!string.IsNullOrEmpty(Request.UrlReferrer.AbsolutePath) && !Request.UrlReferrer.AbsolutePath.ToLower().Contains("customer") && !Request.UrlReferrer.AbsolutePath.ToLower().Contains("dashboard"))
                returnUrl = Request.UrlReferrer.AbsolutePath;

            return RedirectPermanent(returnUrl);
        }
    }
}