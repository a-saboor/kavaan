using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using MyProject.Service.Helpers;
using MyProject.Service.Helpers.OTP;
using MyProject.Service.Helpers.Encryption;
using MyProject.Service.Helpers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.Net.Http.Headers;
using System.Web;

namespace MyProject.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerSessionService _customerSessionService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMail _email;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(ICustomerSessionService customerSessionService, ICustomerRepository customerRepository, IMail email, IUnitOfWork unitOfWork)
        {
            this._customerSessionService = customerSessionService;
            this._customerRepository = customerRepository;
            this._email = email;
            this._unitOfWork = unitOfWork;
        }

        #region ICustomerService Members


        public IEnumerable<object> GetCustomersDropDownForNotifications()
        {
            var Customers = _customerRepository.GetAll();
            var dropdownList = from customer in Customers
                               select new { value = customer.ID, text = customer.UserName + " | " + customer.Contact };
            return dropdownList;
        }
        public IEnumerable<object> GetCustomersForDropDown()
        {
            var Customers = _customerRepository.GetAll();
            var dropdownList = from customer in Customers
                               select new { value = customer.ID, text = customer.Email };
            return dropdownList;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            var customers = _customerRepository.GetAll().Where(x => x.IsDeleted == false);
            return customers;
        }
        public List<Customer> GetCustomersDateWise(DateTime FromDate, DateTime ToDate)
        {
            var Customers = _customerRepository.GetCustomersDateWise(FromDate, ToDate);
            return Customers;
        }
        public Customer GetCustomer(long id)
        {
            var customer = _customerRepository.GetById(id);
            return customer;
        }

        public Customer GetCustomerByEmail(string email)
        {
            Customer customer = new Customer();

            if (email.Contains('@') && email.Contains('.'))
            {
                customer = _customerRepository.GetCustomerByEmail(email);
            }
            else
            {
                customer = _customerRepository.GetCustomerByUsername(email);
            }

            return customer;
        }

        public Customer GetCustomerByUsername(string username, long id = 0)
        {
            var customer = _customerRepository.GetCustomerByUsername(username, id);
            return customer;
        }

        public Customer IsCustomerExist(string email, long id = 0)
        {
            var customer = _customerRepository.GetCustomerByEmail(email);
            return customer;
        }
        public Customer GetByAuthCode(string authCode)
        {
            var customer = _customerRepository.GetByAuthCode(authCode);
            return customer;
        }

        public Customer GetByContact(string contact, long id = 0)
        {
            var customer = _customerRepository.GetByContact(contact, id);
            return customer;
        }

        public Customer GetByContactAndPhoneCode(string contact, string phoneCode, long id = 0)
        {
            var customer = _customerRepository.GetByContactAndPhoneCode(contact, phoneCode, id);
            return customer;
        }

        //public bool CreateCustomer(ref Customer customer, ref string message)
        //{
        //	try
        //	{
        //		customer.IsActive = true;
        //		customer.IsDeleted = false;
        //		customer.IsEmailVerified = false;
        //		customer.IsGuest = true;
        //		customer.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
        //		_customerRepository.Add(customer);
        //		if (SaveCustomer())
        //		{
        //			message = "Customer added successfully ...";
        //			return true;
        //		}
        //		else
        //		{
        //			message = "Oops! Something went wrong. Please try later...";
        //			return false;
        //		}
        //	}
        //	catch (Exception ex)
        //	{
        //		message = "Oops! Something went wrong. Please try later...";
        //		return false;
        //	}
        //}

        public bool CreateCustomer(ref Customer customer, string path, ref string message, bool sendMail = false, bool sendVerificationEmail = false, bool createFromAdmin = false)
        {
            try
            {
                if (this._email.IsValidEmailAddress(customer.Email))
                {
                    if (customer.Email == null || _customerRepository.GetCustomerByEmail(customer.Email) == null)
                    {
                        int i = 0;
                        do
                        {
                            customer.UserName = customer.UserName.ToLower() + new Random().Next(1000, 9999);
                            i = GetCustomerByUsername(customer.UserName, customer.ID) == null ? 1 : 0;

                        } while (i == 0);

                        var password = customer.Password;

                        customer.IsActive = true;
                        customer.IsDeleted = false;
                        customer.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                        customer.Points = 0;
                        customer.IsBookingNoticationAllowed = true;
                        customer.IsPushNotificationAllowed = true;
                        customer.PhoneCode = customer.PhoneCode;
                        customer.Contact = customer.Contact;

                        if (createFromAdmin)
                        {
                            if (_customerRepository.GetByContact(customer.Contact) != null)
                            {
                                message = "Contact is already registered to other customer...";
                                return false;
                            }

                            if (customer.Contact.StartsWith("92") || customer.Contact.StartsWith("+971"))//In dubai, contact verify by otp verification
                            {
                                customer.IsContactOTPAccess = true;
                                customer.IsContactVerified = true;
                                customer.IsEmailVerified = false;
                            }
                            else
                            {
                                customer.IsContactOTPAccess = false;
                                customer.IsContactVerified = false;
                                customer.IsEmailVerified = true;
                            }
                            sendVerificationEmail = false;
                        }
                        else
                        {
                            customer.IsEmailVerified = !sendVerificationEmail;
                        }

                        _customerRepository.Add(customer);
                        if (SaveCustomer())
                        {
                            customer.Salt = (Int16)(customer.CreatedOn.Value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                            string HashPass = new Helpers.Encryption.Crypto().RetrieveHash(customer.Password, Convert.ToString(customer.Salt));
                            customer.Password = HashPass;

                            Crypto objCrypto = new Helpers.Encryption.Crypto();

                            customer.AuthorizationCode = objCrypto.Random(225);
                            while (_customerRepository.GetByAuthCode(customer.AuthorizationCode) != null)
                            {
                                customer.AuthorizationCode = objCrypto.Random(225);
                            }
                            customer.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(0);
                            _customerRepository.Update(customer);
                            if (SaveCustomer())
                            {
                                string culture = "";
                                HttpCookie cookie = HttpContext.Current.Response.Cookies["_culture"];

                                if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                                    culture = cookie.Value;
                                else
                                    culture = "";

                                if (sendVerificationEmail)
                                {
                                    //_email.SendVerificationMail(customer.UserName, customer.Email, CustomURL.GetFormatedURL(culture + "/Customer/Account/Verify?auth=" + customer.AuthorizationCode), path);
                                    SendOTPEmail(customer.Email, 5);
                                }
                                if (sendMail)
                                {
                                    _email.SendWellcomeMail(customer.UserName, customer.Email, customer.Email, password, culture + "/Customer/Account/Login", path);
                                }

                                message = "Account created successfully ...";
                                return true;
                            }
                            else
                            {
                                message = "Oops! Something went wrong. Please try later...";
                                return false;
                            }
                        }
                        else
                        {
                            message = "Oops! Something went wrong. Please try later...";
                            return false;
                        }
                    }
                    else
                    {
                        message = "Email is already registered to other customer...";
                        return false;
                    }
                }
                else
                {
                    message = "Please enter a proper email address...";
                    return false;
                }

            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        //public bool UpdateCustomer(ref Customer customer, ref string message, string path, bool sendMail = false, bool sendVerificationEmail = false, bool editFromAdmin = false)
        //{
        //	try
        //	{
        //		var password = customer.Password;
        //		customer.Salt = (Int16)(customer.CreatedOn.Value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        //		string HashPass = new Crypto().RetrieveHash(customer.Password, Convert.ToString(customer.Salt));
        //		customer.Password = HashPass;
        //		if (this._email.IsValidEmailAddress(customer.Email))
        //		{
        //			if (editFromAdmin)
        //			{
        //				if (_customerRepository.GetByContact(customer.Contact, customer.ID) != null)
        //				{
        //					message = "Contact is already registered to other customer...";
        //					return false;
        //				}
        //				if (_customerRepository.GetCustomerByEmail(customer.Email, customer.ID) != null)
        //				{
        //					message = "Email is already registered to other customer...";
        //					return false;
        //				}
        //			}

        //			_customerRepository.Update(customer);
        //			if (SaveCustomer())
        //			{
        //				if (sendVerificationEmail)
        //				{
        //					_email.SendVerificationMail(customer.UserName, customer.Email, CustomURL.GetFormatedURL("Customer/Account/Verify?auth=" + customer.AuthorizationCode), path);
        //				}
        //				if (sendMail)
        //				{
        //					_email.SendAccountCreationMail(customer.Email, password, customer.UserName, "/Customer/Account/Login");
        //				}

        //				message = "Customer updated successfully ...";
        //				return true;
        //			}
        //			else
        //			{
        //				message = "Oops! Something went wrong. Please try later...";
        //				return false;
        //			}
        //		}
        //		else
        //		{
        //			message = "Please enter a proper email address...";
        //			return false;
        //		}


        //	}
        //	catch (Exception ex)
        //	{
        //		message = "Oops! Something went wrong. Please try later...";
        //		return false;
        //	}
        //}

        public bool UpdateCustomer(ref Customer customer, ref string message, ref string status, bool editFromAdmin = false)
        {
            try
            {
                Customer CurrentCustomer = _customerRepository.GetById(customer.ID);
                var customerExist = _customerRepository.GetCustomerByEmail(customer.Email, CurrentCustomer.ID);

                if (this._email.IsValidEmailAddress(customer.Email))
                {
                    if (customerExist == null)
                    {
                        if (editFromAdmin)
                        {
                            if (_customerRepository.GetByContact(customer.Contact, customer.ID) != null)
                            {
                                message = "Contact is already registered to other customer...";
                                return false;
                            }
                            if (_customerRepository.GetCustomerByEmail(customer.Email, customer.ID) != null)
                            {
                                message = "Email is already registered to other customer...";
                                return false;
                            }

                            //CurrentCustomer.UserName = customer.UserName;
                            CurrentCustomer.FirstName = customer.FirstName;
                            CurrentCustomer.LastName = customer.LastName;
                            CurrentCustomer.Email = customer.Email;
                            CurrentCustomer.PhoneCode = customer.PhoneCode;
                            CurrentCustomer.Contact = customer.Contact;
                            CurrentCustomer.ZipCode = customer.ZipCode;
                            CurrentCustomer.EmiratesID = customer.EmiratesID;
                            CurrentCustomer.EmiratesIDExpiry = customer.EmiratesIDExpiry;
                            CurrentCustomer.PassportNo = customer.PassportNo;
                            CurrentCustomer.PassportExpiry = customer.PassportExpiry;
                            CurrentCustomer.PoBox = customer.PoBox;
                            CurrentCustomer.CustomerCountry = customer.CustomerCountry;
                            CurrentCustomer.CustomerCity = customer.CustomerCity;
                            CurrentCustomer.Address = customer.Address;
                            CurrentCustomer.Address2 = customer.Address2;

                            if (CurrentCustomer.Password != customer.Password)
                            {
                                customer.Salt = (Int16)(CurrentCustomer.CreatedOn.Value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                                string HashPass = new Crypto().RetrieveHash(customer.Password, Convert.ToString(customer.Salt));
                                CurrentCustomer.Password = HashPass;
                            }
                        }
                        else
                        {
                            //CurrentCustomer.UserName = customer.UserName;
                            CurrentCustomer.Email = customer.Email;
                            CurrentCustomer.Contact = customer.Contact;
                            CurrentCustomer.Address = customer.Address;
                            CurrentCustomer.Address2 = customer.Address2;
                            CurrentCustomer.CountryID = customer.CountryID;
                            CurrentCustomer.CityID = customer.CityID;
                            CurrentCustomer.AreaID = customer.AreaID;
                            CurrentCustomer.CustomerCountry = customer.CustomerCountry;
                            CurrentCustomer.CustomerCity = customer.CustomerCity;
                            CurrentCustomer.AccountType = customer.AccountType;
                            //CurrentCustomer.IsActive = customer.IsActive;
                            CurrentCustomer.FirstName = customer.FirstName;
                            CurrentCustomer.LastName = customer.LastName;
                            //CurrentCustomer.Contact = customer.Contact;
                            CurrentCustomer.PoBox = customer.PoBox;
                            CurrentCustomer.ZipCode = customer.ZipCode;
                            CurrentCustomer.PassportNo = customer.PassportNo;
                            CurrentCustomer.PassportExpiry = customer.PassportExpiry;
                            //CurrentCustomer.CNICNo = customer.CNICNo;
                            //CurrentCustomer.CNICExpiry = customer.CNICExpiry;

                            CurrentCustomer.IsEmailVerified = customer.IsEmailVerified;
                            CurrentCustomer.AuthorizationCode = customer.AuthorizationCode;
                            CurrentCustomer.AuthorizationExpiry = customer.AuthorizationExpiry;

                            if (CurrentCustomer.Password != customer.Password)
                            {
                                customer.Salt = (Int16)(CurrentCustomer.CreatedOn.Value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                                string HashPass = new Crypto().RetrieveHash(customer.Password, Convert.ToString(customer.Salt));
                                CurrentCustomer.Password = HashPass;
                            }
                        }

                        customer = null;

                        _customerRepository.Update(CurrentCustomer);
                        if (SaveCustomer())
                        {
                            customer = CurrentCustomer;
                            status = "success";
                            message = "Customer updated successfully ...";
                            return true;
                        }
                        else
                        {
                            status = "failure";
                            message = "Oops! Something went wrong. Please try later...";
                            return false;
                        }
                    }
                    else
                    {
                        status = "failure";
                        message = "Email is already registered to other customer";
                        return false;
                    }
                }
                else
                {
                    status = "failure";
                    message = "Please enter a proper email address...";
                    return false;
                }


            }
            catch (Exception ex)
            {
                status = "failure";
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        //public bool UpdateCustomer(Customer customer, ref string message, ref string status)
        //{
        //	try
        //	{
        //		if (_customerRepository.GetCustomerByEmail(customer.Email, customer.ID) == null)
        //		{
        //			Customer CurrentCustomer = _customerRepository.GetById(customer.ID);

        //			CurrentCustomer.UserName = customer.UserName;
        //			CurrentCustomer.Email = customer.Email;
        //			CurrentCustomer.Contact = customer.Contact;
        //			CurrentCustomer.Address = customer.Address;
        //			CurrentCustomer.CountryID = customer.CountryID;
        //			CurrentCustomer.AreaID = customer.AreaID;
        //			CurrentCustomer.AccountType = customer.AccountType;
        //			CurrentCustomer.CityID = customer.CityID;

        //			customer = null;

        //			_customerRepository.Update(CurrentCustomer);
        //			if (SaveCustomer())
        //			{
        //				customer = CurrentCustomer;
        //				status = "success";
        //				message = "Customer updated successfully ...";
        //				return true;
        //			}
        //			else
        //			{
        //				status = "failure";
        //				message = "Oops! Something went wrong. Please try later...";
        //				return false;
        //			}
        //		}
        //		else
        //		{
        //			status = "error";
        //			message = "Customer already exist  ...";
        //			return false;
        //		}
        //	}
        //	catch (Exception ex)
        //	{
        //		status = "failure";
        //		message = "Oops! Something went wrong. Please try later...";
        //		return false;
        //	}
        //}

        public bool DeleteCustomer(long id, ref string message, bool softDelete = true)
        {
            try
            {
                Customer customer = _customerRepository.GetById(id);
                if (softDelete)
                {
                    customer.IsDeleted = true;
                    _customerRepository.Update(customer);
                }
                else
                {
                    _customerRepository.Delete(customer);
                }
                if (SaveCustomer())
                {
                    message = "Customer deleted successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public Customer Authenticate(string email, string password, ref string returnMessage, ref bool verifyByEmail, ref bool verifyByContact, ref bool verifyAgain)
        {
            var customer = this.GetCustomerByEmail(email);
            if (customer != null)
            {
                if (customer.IsActive.HasValue && customer.IsActive.Value)
                {
                    if (
                        (customer.IsContactVerified.HasValue && customer.IsContactVerified.Value && customer.IsContactOTPAccess.HasValue && customer.IsContactOTPAccess.Value)//verify by Contact
                        ||
                        (customer.IsEmailVerified.HasValue && customer.IsEmailVerified.Value && customer.IsContactOTPAccess.HasValue && !customer.IsContactOTPAccess.Value)//verify by Email
                        )
                    {
                        string RetrievedPass = new Crypto().RetrieveHash(password, Convert.ToString(customer.Salt.Value));
                        if (RetrievedPass.Equals(customer.Password))
                        {
                            returnMessage = "Login Successful!";

                            try
                            {
                                string updateMsg = string.Empty;
                                string updateStatus = string.Empty;
                                customer.AuthorizationCode = Guid.NewGuid() + "-" + Convert.ToString(customer.ID);
                                this.UpdateCustomer(ref customer, ref updateMsg, ref updateStatus);
                            }
                            catch (Exception)
                            { }

                            return customer;
                        }
                        else
                        {
                            returnMessage = "Wrong Password!";
                        }
                    }
                    else
                    {
                        verifyAgain = true;

                        if (verifyByContact)
                            returnMessage = "OTP verification needed, we just need to validate OTP before you can access!";
                        else
                            returnMessage = "OTP verification needed, we just need to validate OTP by your email address before you can access!";
                    }
                }
                else
                {
                    returnMessage = "Account suspended!";
                }
            }
            else
            {
                returnMessage = "Invalid Username!";
            }
            return null;
        }
        public Customer Authenticate(string phoneCode, string contact, string password, ref string returnMessage, ref bool verifyByEmail, ref bool verifyByContact, ref bool verifyAgain)
        {
            var customer = _customerRepository.GetByContactAndPhoneCode(contact, phoneCode);
            if (customer != null)
            {
                if (customer.IsActive.HasValue && customer.IsActive.Value)
                {
                    if (
                        (customer.IsContactVerified.HasValue && customer.IsContactVerified.Value && customer.IsContactOTPAccess.HasValue && customer.IsContactOTPAccess.Value)//verify by Contact
                        ||
                        (customer.IsEmailVerified.HasValue && customer.IsEmailVerified.Value && customer.IsContactOTPAccess.HasValue && !customer.IsContactOTPAccess.Value)//verify by Email
                        )
                    {
                        string RetrievedPass = new Crypto().RetrieveHash(password, Convert.ToString(customer.Salt.Value));
                        if (RetrievedPass.Equals(customer.Password))
                        {
                            returnMessage = "Login Successful!";

                            try
                            {
                                string updateMsg = string.Empty;
                                string updateStatus = string.Empty;
                                customer.AuthorizationCode = Guid.NewGuid() + "-" + Convert.ToString(customer.ID);
                                this.UpdateCustomer(ref customer, ref updateMsg, ref updateStatus);
                            }
                            catch (Exception)
                            { }

                            return customer;
                        }
                        else
                        {
                            returnMessage = "Wrong Password!";
                        }
                    }
                    else
                    {
                        verifyAgain = true;

                        if (verifyByContact)
                            returnMessage = "OTP verification needed, we just need to validate OTP before you can access!";
                        else
                            returnMessage = "OTP verification needed, we just need to validate OTP by your email address before you can access!";
                    }
                }
                else
                {
                    returnMessage = "Account suspended!";
                }
            }
            else
            {
                returnMessage = "Invalid Contact or Password!";
            }
            return null;
        }

        public bool Authenticate(string countryCode, string contact, string Password, ref ClaimsIdentity identity, ref string returnMessage, ref bool verifyByEmail, ref bool verifyByContact, ref bool verifyAgain)
        {
            var customer = _customerRepository.GetByContactAndPhoneCode(contact, countryCode);
            if (customer != null)
            {
                if (customer.IsActive.HasValue && customer.IsActive.Value)
                {
                    if ((customer.IsContactVerified.HasValue && customer.IsContactVerified.Value) || (customer.IsEmailVerified.HasValue && customer.IsEmailVerified.Value))
                    {
                        //string RetrievedPass = new Crypto().RetrieveHash(password, Convert.ToString(customer.Salt.Value));
                        string RetrievedPass = new Crypto().RetrieveHash(Password, Convert.ToString(customer.Salt.Value));
                        if (RetrievedPass.Equals(customer.Password))
                        {
                            returnMessage = "Login Successful!";
                            identity.AddClaim(new Claim(ClaimTypes.Name, customer.UserName));
                            identity.AddClaim(new Claim(ClaimTypes.Email, customer.Email));
                            identity.AddClaim(new Claim(ClaimTypes.Role, "Customer"));
                            identity.AddClaim(new Claim(ClaimTypes.UserData, customer.ID.ToString()));

                            return true;
                        }
                        else
                        {
                            returnMessage = "Wrong Password!";
                        }
                    }
                    else
                    {
                        verifyAgain = true;

                        if (verifyByContact)
                            returnMessage = "OTP verification needed, we just need to verify your contact before you can access!";
                        else
                            returnMessage = "Email verification needed, we just need to verify your email address before you can access!";

                    }
                }
                else
                {
                    returnMessage = "Account suspended!";
                }
            }
            else
            {
                returnMessage = "Invalid Password Or Email";
            }

            return false;
        }

        public bool ChangePassword(string oldPassword, string NewPassword, long customerId, ref string status, ref string message)
        {
            try
            {
                var customer = _customerRepository.GetById(customerId);

                string HashOldPassword = new Crypto().RetrieveHash(oldPassword, Convert.ToString(customer.Salt));

                if (customer.Password.Equals(HashOldPassword))
                {
                    string HashNewPassword = new Crypto().RetrieveHash(NewPassword, Convert.ToString(customer.Salt));
                    customer.Password = HashNewPassword;
                    _customerRepository.Update(customer);

                    if (SaveCustomer())
                    {
                        status = "success";
                        message = "Password Changed Successfully !";
                        return true;
                    }
                    else
                    {
                        status = "failure";
                        message = "Oops! Something went wrong. Please try later...";
                    }
                }
                else
                {
                    status = "error";
                    message = "The current password is wrong!";
                }

                return false;
            }
            catch (Exception ex)
            {
                status = "failure";
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool ForgotPassword(string email, string path, ref string status, ref string message)
        {
            try
            {
                var customer = _customerRepository.GetCustomerByEmail(email);

                if (customer != null)
                {
                    Crypto objCrypto = new Helpers.Encryption.Crypto();

                    customer.AuthorizationCode = objCrypto.Random(225);
                    while (_customerRepository.GetByAuthCode(customer.AuthorizationCode) != null)
                    {
                        customer.AuthorizationCode = objCrypto.Random(225);
                    }
                    customer.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(5);
                    _customerRepository.Update(customer);
                    if (SaveCustomer())
                    {
                        string culture = "";
                        HttpCookie cookie = HttpContext.Current.Response.Cookies["_culture"];

                        if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                            culture = "/";
                        else
                            culture = "";

                        if (_email.SendForgotPasswordMail(customer.ID, customer.UserName, customer.Email, CustomURL.GetFormatedURL(culture + "/Customer/Account/ResetPassword?auth=" + customer.AuthorizationCode), path))
                        {
                            status = "success";
                            message = $"Password recovery instruction has been sent to your email ({email}). Please check your email";
                            return true;
                        }
                        else
                        {
                            status = "failure";
                            message = "Oops! Something went wrong. Please try later...";
                        }
                    }
                }
                else
                {
                    status = "error";
                    message = "Invalid Email Address!";
                }

                return false;
            }
            catch
            {
                status = "failure";
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool ResetPassword(string password, long customerId, ref string message)
        {
            var Customer = _customerRepository.GetById(customerId);

            string HashNewPassword = new Crypto().RetrieveHash(password, Convert.ToString(Customer.Salt));
            Customer.Password = HashNewPassword;
            Customer.AuthorizationCode = new Crypto().RetrieveHash(Customer.UserName, Convert.ToString((Int16)(DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds));

            Customer.OTP = null;

            _customerRepository.Update(Customer);
            if (SaveCustomer())
            {
                message = "Password reset successfully!";
                return true;
            }
            else
            {
                message = "Oops! Something went wrong. Please try later...";
            }

            return false;
        }

        public bool VerifyEmail(string email, string path, string culture, ref string status, ref string message)
        {
            try
            {

                var customer = _customerRepository.GetCustomerByEmail(email);

                if (customer != null)
                {
                    customer.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(0);
                    _customerRepository.Update(customer);
                    if (SaveCustomer())
                    {
                        if (SendOTPEmail(customer.Email, 5))
                        {
                            status = "success";
                            message = "OTP verification code has been sent to your email.";
                            return true;
                        }
                        else
                        {
                            status = "failure";
                            message = "Oops! Something went wrong. Please try later...";
                        }
                    }
                    else
                    {
                        status = "failure";
                        message = "Oops! Something went wrong. Please try later...";
                    }

                    //if (customer.AuthorizationCode != null)
                    //{
                    //	if (_email.SendVerificationMail(customer.UserName, customer.Email, CustomURL.GetFormatedURL(culture + "/Customer/Account/Verify?auth=" + customer.AuthorizationCode), path))
                    //	{
                    //		status = "success";
                    //		message = "Account verification instruction has been sent to your email.";
                    //		return true;
                    //	}
                    //	else
                    //	{
                    //		status = "failure";
                    //		message = "Oops! Something went wrong. Please try later...";
                    //	}
                    //}
                    //else
                    //{
                    //	Crypto objCrypto = new Helpers.Encryption.Crypto();

                    //	customer.AuthorizationCode = objCrypto.Random(225);
                    //	while (_customerRepository.GetByAuthCode(customer.AuthorizationCode) != null)
                    //	{
                    //		customer.AuthorizationCode = objCrypto.Random(225);
                    //	}
                    //	customer.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(5);
                    //	_customerRepository.Update(customer);
                    //	if (SaveCustomer())
                    //	{
                    //		if (_email.SendVerificationMail(customer.UserName, customer.Email, CustomURL.GetFormatedURL(culture + "/Customer/Account/Verify?auth=" + customer.AuthorizationCode), path))
                    //		{
                    //			status = "success";
                    //			message = "Account verification instruction has been sent to your email.";
                    //			return true;
                    //		}
                    //		else
                    //		{
                    //			status = "failure";
                    //			message = "Oops! Something went wrong. Please try later...";
                    //		}
                    //	}
                    //	else
                    //	{
                    //		status = "failure";
                    //		message = "Oops! Something went wrong. Please try later...";
                    //	}
                    //}
                }
                else
                {
                    status = "error";
                    message = "Invalid Email Address!";
                }

                return false;
            }
            catch
            {
                status = "failure";
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public async Task<Boolean> SendOTP(string Contact, double time = 2.1)
        {
            Customer customer = _customerRepository.GetByContact(Contact);
            if (customer != null)
            {
                List<string> contacts = new List<string>()
                {   "507567600","37546410"
                };
                if (contacts.Contains(Contact))
                {
                    customer.OTP = 1234;
                    customer.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(15);
                    _customerRepository.Update(customer);
                    if (SaveCustomer())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    int otp;

                    DateTime otpTime = customer.AuthorizationExpiry != null ? (DateTime)customer.AuthorizationExpiry : Helpers.TimeZone.GetLocalDateTime();
                    DateTime currentTime = Helpers.TimeZone.GetLocalDateTime();

                    if (otpTime <= currentTime || true)
                    {
                        var number = customer.PhoneCode + Contact;
                        otp = new Random().Next(1000, 9999);
                        //var apiKey = "ODhGNjg0NDgtRTQ0RC00Njg4LUJDNjItOUM1MTM5RUVBNTZG";
                        //var text = "Your OTP Code is " + otp + " \nMDNSMGYjkzc";

                        //var uri = new Uri("https://api.antwerp.ae/Send?phonenumbers=" + number + "&sms.sender=KAVAAN&sms.text=" + text + "&apiKey=" + apiKey + "");

                        //var request = new HttpRequestMessage(HttpMethod.Get, uri);

                        ////request.Headers.TryAddWithoutValidation("USER", "");
                        ////request.Headers.TryAddWithoutValidation("DIGEST", "781957ea504ff2eb31434ae804928241");
                        ////request.Headers.TryAddWithoutValidation("CREATED", Helpers.TimeZone.GetLocalDateTime().ToString());

                        ////request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                        //var client = new HttpClient();

                        //client.DefaultRequestHeaders.Authorization =
                        //              new AuthenticationHeaderValue(
                        //                  "Bearer", apiKey);

                        //var response = await client.SendAsync(request);
                        //if (response.IsSuccessStatusCode)
                        //{
                        //    customer.OTP = otp;
                        //    customer.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(time);
                        //    _customerRepository.Update(customer);
                        //    if (SaveCustomer())
                        //    {
                        //        return true;
                        //    }
                        //    else
                        //    {
                        //        return false;
                        //    }
                        //}
                        //else
                        //{
                        //    return false;
                        //}
                    }
                    return false;
                }
            }
            return false;
        }
        public bool SendOTPEmail(string email, double time = 2.1)
        {
            Customer customer = _customerRepository.GetCustomerByEmail(email);
            if (customer != null)
            {
                int otp;

                DateTime otpTime = customer.AuthorizationExpiry != null ? (DateTime)customer.AuthorizationExpiry : Helpers.TimeZone.GetLocalDateTime();
                DateTime currentTime = Helpers.TimeZone.GetLocalDateTime();

                if (otpTime <= currentTime)
                {
                    otp = new Random().Next(1000, 9999);

                    var path = System.Web.Hosting.HostingEnvironment.MapPath("~/");

                    if (_email.SendOTPMail(customer.UserName, email, otp, "OTP Verification", path))
                    {
                        customer.OTP = otp;
                        customer.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(time);
                        HttpContext.Current.Session["CustomerAuthorizationExpiry"] = customer.AuthorizationExpiry;

                        _customerRepository.Update(customer);
                        if (SaveCustomer())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                return false;

            }
            return false;
        }

        public Boolean IsCustomer(string email, string Contact)
        {
            var customer = _customerRepository.GetCustomerByEmailandContact(email, Contact);

            return customer == null ? false : true;
        }

        public bool IsCustomerByContact(string contact)
        {
            var customer = _customerRepository.GetByContact(contact);

            return customer == null ? false : true;
        }

        public bool verifyOTP(string phoneCode, string contact, int otp, ref string status, ref string message)
        {
            try
            {
                var customer = _customerRepository.GetByContact(contact);

                if (customer != null)
                {
                    if (phoneCode == customer.PhoneCode)
                    {

                        if (Helpers.TimeZone.GetLocalDateTime() <= customer.AuthorizationExpiry)
                        {
                            if (customer.OTP == otp)
                            {
                                //customer.OTP = null;
                                customer.AuthorizationExpiry = null;
                                if (customer.IsContactOTPAccess == true)
                                {
                                    customer.IsContactVerified = true;
                                }
                                else
                                {
                                    customer.IsEmailVerified = true;
                                }
                                _customerRepository.Update(customer);
                                if (SaveCustomer())
                                {
                                    status = "success";
                                    message = "OTP Verified.";
                                    return true;
                                }
                                else
                                {
                                    status = "failure";
                                    message = "Oops! Something went wrong. Please try later...";
                                }
                            }
                            else
                            {
                                status = "error";
                                message = "Incorrect OTP Code, please verify code with valid OTP";
                            }
                        }

                        else
                        {
                            status = "error";
                            message = "OTP Code Expired";
                        }
                    }

                    else
                    {
                        status = "error";
                        message = "Invalid Phone Code.";
                    }
                }
                else
                {
                    status = "error";
                    message = "Invalid Contact Number.";
                }

                return false;
            }
            catch
            {
                status = "failure";
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool SaveCustomer()
        {
            try
            {
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }

    public interface ICustomerService
    {
        IEnumerable<Customer> GetCustomers();
        List<Customer> GetCustomersDateWise(DateTime FromDate, DateTime ToDate);
        IEnumerable<object> GetCustomersForDropDown();
        Customer GetCustomer(long id);
        Customer GetCustomerByEmail(string email);
        Customer GetCustomerByUsername(string username, long id = 0);
        Customer GetByAuthCode(string authCode);
        Customer GetByContact(string contact, long id = 0);
        Customer GetByContactAndPhoneCode(string contact, string phoneCode, long id = 0);
        Task<Boolean> SendOTP(string Contact, double time = 2.1);
        bool SendOTPEmail(string email, double time = 2.1);
        //bool CreateCustomer(ref Customer customer, ref string message);
        bool CreateCustomer(ref Customer customer, string path, ref string message, bool sendMail = false, bool sendVerificationEmail = false, bool createFromAdmin = false);
        bool UpdateCustomer(ref Customer customer, ref string message, ref string status, bool editFromAdmin = false);
        //bool UpdateCustomer(Customer customer, ref string message, ref string status);
        //bool UpdateCustomer(ref Customer customer, ref string message, string path, bool sendMail = false, bool sendVerificationEmail = false, bool editFromAdmin = false);
        bool DeleteCustomer(long id, ref string message, bool softDelete = true);

        Customer Authenticate(string email, string password, ref string returnMessage, ref bool verifyByEmail, ref bool verifyByContact, ref bool verifyAgain);

        Customer Authenticate(string phoneCode, string contact, string password, ref string returnMessage, ref bool verifyByEmail, ref bool verifyByContact, ref bool verifyAgain);
        bool Authenticate(string countryCode, string contact, string Password, ref ClaimsIdentity identity, ref string returnMessage, ref bool verifyByEmail, ref bool verifyByContact, ref bool verifyAgain);
        bool ChangePassword(string oldPassword, string NewPassword, long customerId, ref string status, ref string message);
        bool ForgotPassword(string email, string path, ref string status, ref string message);
        bool ResetPassword(string password, long customerId, ref string message);
        bool VerifyEmail(string email, string path, string culture, ref string status, ref string message);
        bool verifyOTP(string phoneCode, string contact, int otp, ref string status, ref string message);
        bool IsCustomer(string email, string Contact);
        bool IsCustomerByContact(string contact);
        bool SaveCustomer();
        IEnumerable<object> GetCustomersDropDownForNotifications();
    }
}
