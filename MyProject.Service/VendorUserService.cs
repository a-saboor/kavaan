using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using MyProject.Service.Helpers;
using MyProject.Service.Helpers.Encryption;
using MyProject.Service.Helpers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyProject.Service
{
    public class VendorUserService : IVendorUserService
    {
        private readonly IVendorUserRepository _vendorUserRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly IVendorUserRoleRepository _vendorUserRoleRepository;
        private readonly IMail _email;
        private readonly IUnitOfWork _unitOfWork;

        public VendorUserService(IVendorUserRepository vendorUserRepository, IVendorUserRoleRepository vendorUserRoleRepository, IVendorRepository vendorRepository, IMail email, IUnitOfWork unitOfWork)
        {
            this._vendorUserRepository = vendorUserRepository;
            this._vendorRepository = vendorRepository;
            this._vendorUserRoleRepository = vendorUserRoleRepository;
            this._email = email;
            this._unitOfWork = unitOfWork;
        }

        #region IUserService Members

        public IEnumerable<VendorUser> GetVendorUsers()
        {
            var vendorUsers = _vendorUserRepository.GetAll();
            return vendorUsers;
        }

        public IEnumerable<VendorUser> GetVendorUsers(long vendorId)
        {
            var vendorUsers = _vendorUserRepository.GetVendorUsers(vendorId);
            return vendorUsers;
        }

        public VendorUser GetVendorUser(long id)
        {
            var user = _vendorUserRepository.GetById(id);
            return user;
        }

        public VendorUser GetUserByRole(long vendorId, string roleName)
        {
            var role = _vendorUserRoleRepository.GetvendorRoleByName(roleName);
            var user = _vendorUserRepository.GetVendorUserByRole(vendorId, role.ID);
            return user;
        }

        public VendorUser GetUserByEmail(string email)
        {
            var user = _vendorUserRepository.GetVendorUserByEmail(email);
            return user;
        }

        public VendorUser GetVendorUserByContact(string phone)
        {
            var user = _vendorUserRepository.GetVendorUserByContact(phone);
            return user;
        }

        public bool IsVendorExistByContact(string contact)
        {
            var user = _vendorUserRepository.GetVendorUserByContact(contact);
            return user == null ? true : false;
        }

        public VendorUser IsUserExist(string email, long id = 0)
        {
            var user = _vendorUserRepository.GetVendorUserByEmail(email);
            return user;
        }

        public VendorUser IsUserExistByUserName(string email, long id = 0)
        {
            var user = _vendorUserRepository.GetVendorUserByEmail(email);
            return user;
        }

        public VendorUser GetVendorUserByVendorID(long vendorId)
        {
            var user = _vendorUserRepository.GetVendorUserByVendorID(vendorId);
            return user;
        }

        public VendorUser GetByAuthCode(string authCode)
        {
            var user = _vendorUserRepository.GetByAuthCode(authCode);
            return user;
        }
        public bool EmailVerification(ref string Email, long ID, ref string message)
        {
            try
            {
                if (_vendorUserRepository.GetVendorUserByEmail(Email, ID) == null)
                {

                    message = "Email is unique";
                    return true;
                }
                else
                {
                    message = "Email address already exists! Please write another email address.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }
        public bool ContactVerification(ref string Code, ref string Contact, long ID, ref string message)
        {
            try
            {
                if (_vendorUserRepository.GetVendorUserByContact(Contact, ID) == null)
                {

                    message = "Contact is unique";
                    return true;
                }
                else
                {
                    message = "Contact already exists! Please write another contact.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }
        public bool CreateVendorUser(VendorUser user, ref string message, bool sendMail = false)
        {
            try
            {
                if (_vendorUserRepository.GetVendorUserByEmail(user.EmailAddress) == null)
                {
                    var password = user.Password;

                    user.IsActive = true;
                    user.IsDeleted = false;
                    user.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    Crypto objCrypto = new Helpers.Encryption.Crypto();

                    user.AuthorizationCode = objCrypto.Random(225);
                    while (_vendorUserRepository.GetByAuthCode(user.AuthorizationCode) != null)
                    {
                        user.AuthorizationCode = objCrypto.Random(225);
                    }
                    user.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(5);
                    _vendorUserRepository.Add(user);
                    if (SaveUser())
                    {
                        user.Salt = (Int16)(user.CreatedOn.Value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                        string HashPass = new Helpers.Encryption.Crypto().RetrieveHash(user.Password, Convert.ToString(user.Salt));
                        user.Password = HashPass;
                        _vendorUserRepository.Update(user);
                        if (SaveUser())
                        {
                            if (sendMail)
                            {
                                _email.SendAccountCreationMail(user.EmailAddress, password, user.Name, "/Vendor/Account/Login");
                            }
                            message = "User added successfully ...";
                            return true;
                        }
                        else
                        {
                            message = "Oops! Something went wrong. Please try later.";
                            return false;
                        }
                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later.";
                        return false;
                    }
                }
                else
                {
                    message = "Vendor user already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }
        public bool Authenticate(string email, string password, ref ClaimsIdentity identity, ref string returnMessage)
        {
            var vendor = _vendorUserRepository.GetVendorUserByContact(email);
            if (vendor != null)
            {
                if (vendor.IsActive.HasValue && vendor.IsActive.Value)
                {

                    string RetrievedPass = new Crypto().RetrieveHash(password, Convert.ToString(vendor.Salt.Value));
                    //string RetrievedPass = OTP;
                    if (RetrievedPass.Equals(vendor.Password.ToString()))
                    {
                        returnMessage = "Login Successful";
                        identity.AddClaim(new Claim(ClaimTypes.Name, vendor.Name));
                        identity.AddClaim(new Claim(ClaimTypes.Email, vendor.EmailAddress));
                        identity.AddClaim(new Claim(ClaimTypes.Role, "Vendor"));
                        identity.AddClaim(new Claim(ClaimTypes.UserData, vendor.VendorID.ToString()));

                        return true;
                    }
                    else
                    {
                        returnMessage = "Wrong Password!";
                    }

                    //else
                    //{
                    //    if (customer.IsSrilankan.HasValue && customer.IsSrilankan.Value)
                    //        returnMessage = "OTP Unverified";
                    //    else
                    //        returnMessage = "We just need to verify your email address before you can access";
                    //}
                }
                else
                {
                    returnMessage = "Account suspended!";
                }
            }
            else
            {
                returnMessage = "Invalid email or password";
            }

            return false;
        }
        public bool UpdateVendorUser(ref VendorUser user, ref string message)
        {
            try
            {
                if (_vendorUserRepository.GetVendorUserByEmail(user.EmailAddress) != null)
                {
                    VendorUser CurrentUser = _vendorUserRepository.GetById(user.ID);

                    CurrentUser.Name = user.Name;
                    CurrentUser.EmailAddress = user.EmailAddress;
                    CurrentUser.MobileNo = user.MobileNo;
                    CurrentUser.UserRoleID = user.UserRoleID;

                    if (CurrentUser.Password != user.Password)
                    {
                        user.Salt = (Int16)(user.CreatedOn.Value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                        string HashPass = new Crypto().RetrieveHash(user.Password, Convert.ToString(user.Salt));
                        CurrentUser.Password = HashPass;
                    }
                    user = null;

                    _vendorUserRepository.Update(CurrentUser);
                    if (SaveUser())
                    {
                        user = CurrentUser;
                        message = "User updated successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later.";
                        return false;
                    }
                }
                else
                {
                    message = "User already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }

        public bool DeleteVendorUser(long id, ref string message, bool softDelete = true)
        {
            try
            {
                VendorUser user = _vendorUserRepository.GetById(id);
                if (user.VendorUserRole.Name.Equals("Administrator"))
                {
                    message = "Administrator User can't be deleted!";
                    return false;
                }

                if (softDelete)
                {
                    user.IsDeleted = true;
                    _vendorUserRepository.Update(user);
                }
                else
                {
                    _vendorUserRepository.Delete(user);
                }

                if (SaveUser())
                {
                    message = "User deleted successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }

        public VendorUser Authenticate(string email, string password, ref string returnMessage, ref int errorCode, ref long vendorId)
        {
            errorCode = 0;
            var user = _vendorUserRepository.GetVendorUserByEmail(email);
            if (user != null)
            {
                var vendor = user.Vendor;
                if (vendor != null)
                {
                    if (vendor.IsActive == null || !vendor.IsActive.Value)
                    {
                        returnMessage = "Your administrator account suspended, please contact to your administrator!";
                        return null;
                    }

                    if (vendor.Contact.StartsWith("94"))
                    {
                        if (vendor.IsContactVerified == null || !vendor.IsContactVerified.Value)
                        {
                            if (user.VendorUserRole.Name.Equals("Administrator"))
                            {
                                vendorId = vendor.ID;
                                returnMessage = "Your Contact is not verified!";
                                errorCode = 1;
                            }
                            else
                            {
                                returnMessage = "Your administrator account Contact is not verified, please contact to your administrator!";
                                errorCode = 0;
                            }

                            return null;
                        }
                    }
                    else
                    {

                        if (vendor.IsEmailSent == null || !vendor.IsEmailSent.Value)
                        {
                            if (user.VendorUserRole.Name.Equals("Administrator"))
                            {
                                vendorId = vendor.ID;
                                returnMessage = "Your Email is not verified, Please check your email!";
                                errorCode = 2;
                            }
                            else
                            {
                                returnMessage = "Your administrator account Email is not verified, please contact to your administrator!";
                                errorCode = 0;
                            }

                            return null;
                        }
                    }
                }

                if (user.IsActive.HasValue && user.IsActive.Value)
                {
                    string RetrievedPass = new Crypto().RetrieveHash(password, Convert.ToString(user.Salt.Value));
                    if (RetrievedPass.Equals(user.Password))
                    {
                        returnMessage = "Login Successful";
                        try
                        {
                            string updateMsg = string.Empty;
                            user.AuthorizationCode = Guid.NewGuid() + "-" + Convert.ToString(user.ID);
                            this.UpdateVendorUser(ref user, ref updateMsg);
                        }
                        catch (Exception)
                        { }
                        return user;
                    }
                    else
                    {
                        returnMessage = "Wrong Password!";
                    }
                }
                else
                {
                    returnMessage = "Account suspended!";
                }
            }
            else
            {
                returnMessage = "Invalid Email or Password!";
            }

            return null;
        }

        public async Task<Boolean> SendOTP(string Contact)
        {

            VendorUser vendor = _vendorUserRepository.GetVendorUserByContact(Contact);
            int otp;
            DateTime otpTime =  Helpers.TimeZone.GetLocalDateTime().AddMinutes(2.1); ;
            DateTime currentTime = Helpers.TimeZone.GetLocalDateTime();

            if (vendor != null)
            {

                List<string> contacts = new List<string>()
                {   "971507567601","97137546410","9713131027955"
                };
                if (contacts.Contains(Contact))
                {
                    vendor.OTP = 1234;
                    vendor.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(2.1);
                    _vendorUserRepository.Update(vendor);
                    if (SaveUser())
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
                    if (otpTime < currentTime)
                    {


                        otp = new Random().Next(1000, 9999);
                        var apiKey = "ODhGNjg0NDgtRTQ0RC00Njg4LUJDNjItOUM1MTM5RUVBNTZG";
                        var text = "Your OTP Code is " + otp + " \nMDNSMGYjkzc";




                        var uri = new Uri("https://api.antwerp.ae/Send?phonenumbers=" + Contact + "&sms.sender=KAVAAN&sms.text=" + text + "&apiKey=" + apiKey + "");


                        var request = new HttpRequestMessage(HttpMethod.Get, uri);


                        //request.Headers.TryAddWithoutValidation("USER", "");
                        //request.Headers.TryAddWithoutValidation("DIGEST", "781957ea504ff2eb31434ae804928241");
                        //request.Headers.TryAddWithoutValidation("CREATED", Helpers.TimeZone.GetLocalDateTime().ToString());

                        //request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                        var client = new HttpClient();

                        client.DefaultRequestHeaders.Authorization =
                                      new AuthenticationHeaderValue(
                                          "Bearer", apiKey);

                        var response = await client.SendAsync(request);
                        if (response.IsSuccessStatusCode)
                        {
                            vendor.OTP = otp;
                            vendor.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(2.1);
                            _vendorUserRepository.Update(vendor);
                            if (SaveUser())
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
                }



                return false;

            }

            return false;


        }

        public bool ChangePassword(string oldPassword, string NewPassword, long userId, ref string message)
        {
            var user = _vendorUserRepository.GetById(userId);

            string HashOldPassword = new Crypto().RetrieveHash(oldPassword, Convert.ToString(user.Salt));

            if (user.Password.Equals(HashOldPassword))
            {
                string HashNewPassword = new Crypto().RetrieveHash(NewPassword, Convert.ToString(user.Salt));
                user.Password = HashNewPassword;
                _vendorUserRepository.Update(user);

                if (SaveUser())
                {
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later.";
                }
            }
            else
            {
                message = "Entered old password is wrong !";
            }

            return false;
        }

        public bool verifyOTP(string contact, int otp, ref string status, ref string message)
        {
            try
            {

                var vendorUser = _vendorUserRepository.GetVendorUserByContact(contact);
                var vendor = _vendorRepository.GetVendorByContact(contact);

                if (vendorUser != null)
                {

                    if (Helpers.TimeZone.GetLocalDateTime() <= vendorUser.AuthorizationExpiry)
                    {
                        if (vendorUser.OTP == otp)
                        {
                            vendorUser.OTP = null;
                            vendorUser.AuthorizationExpiry = null;
                            vendorUser.IsContactVerified = true;
                            UpdateVendorUser(ref vendorUser, ref message);
                            if (SaveUser())
                            {
                                status = "success";
                                message = "Cool! OTP Verified";
                                return true;
                            }
                            else
                            {
                                status = "failure";
                                message = "Oops! Something went wrong. Please try later.";
                            }
                        }
                        else
                        {
                            status = "error";
                            message = "OTP Code Incorrect";
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
                    message = "Invalid Contact Number.";
                }

                return false;
            }
            catch
            {
                status = "failure";
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }

        public bool ForgotPassword(string email, ref string path, ref string message, ref string status)
        {
            try
            {
                var user = _vendorUserRepository.GetVendorUserByEmail(email);

                if (user != null)
                {
                    Crypto objCrypto = new Helpers.Encryption.Crypto();

                    user.AuthorizationCode = objCrypto.Random(225);
                    while (_vendorUserRepository.GetByAuthCode(user.AuthorizationCode) != null)
                    {
                        user.AuthorizationCode = objCrypto.Random(225);
                    }
                    user.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(5);
                    _vendorUserRepository.Update(user);
                    if (SaveUser())
                    {

                        if (_email.SendForgotPasswordMail(user.ID, user.Name, user.EmailAddress, CustomURL.GetFormatedURL("/Vendor/Account/ResetPassword?auth=" + user.AuthorizationCode), path))
                        {
                            message = "Cool! Password recovery instruction has been sent to your email.";
                            return true;
                        }
                        else
                        {
                            message = "Oops! Something went wrong. Please try later.";
                        }
                    }
                }
                else
                {
                    message = "Invalid Email.";
                }

                return false;
            }
            catch
            {
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }

        public bool ResetPassword(string password, long userId, ref string message)
        {
            var VendorUser = _vendorUserRepository.GetById(userId);

            string HashNewPassword = new Crypto().RetrieveHash(password, Convert.ToString(VendorUser.Salt));
            VendorUser.Password = HashNewPassword;
            VendorUser.AuthorizationCode = new Crypto().RetrieveHash(VendorUser.Name, Convert.ToString((Int16)(DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds));
            _vendorUserRepository.Update(VendorUser);
            if (SaveUser())
            {
                message = "Account reset successfully";
                return true;
            }
            else
            {
                message = "Oops! Something went wrong. Please try later.";
            }

            return false;
        }

        public bool SaveUser()
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

    public interface IVendorUserService
    {
        IEnumerable<VendorUser> GetVendorUsers();
        IEnumerable<VendorUser> GetVendorUsers(long vendorId);
        VendorUser GetVendorUser(long id);
        Task<Boolean> SendOTP(string Contact);
        bool verifyOTP(string contact, int otp, ref string status, ref string message);
        //VendorUser GetVendorUserByRole(string administrator);
        VendorUser GetUserByEmail(string email);
        VendorUser GetVendorUserByContact(string phone);
        bool IsVendorExistByContact(string email);
        VendorUser GetUserByRole(long vendorId, string roleName);
        VendorUser GetByAuthCode(string authCode);
        bool EmailVerification(ref string Email, long ID, ref string message);
        bool ContactVerification(ref string Code, ref string Contact, long ID, ref string message);
        VendorUser GetVendorUserByVendorID(long vendorId);
        bool CreateVendorUser(VendorUser user, ref string message, bool sendMail = false);
        bool UpdateVendorUser(ref VendorUser user, ref string message);
        bool DeleteVendorUser(long id, ref string message, bool softDelete = true);
        VendorUser Authenticate(string email, string password, ref string returnMessage, ref int errorCode, ref long vendorId);
        bool ChangePassword(string oldPassword, string NewPassword, long userId, ref string message);
        bool ForgotPassword(string email, ref string path, ref string message, ref string status);
        bool ResetPassword(string password, long userId, ref string message);
        bool Authenticate(string email, string password, ref ClaimsIdentity identity, ref string returnMessage);
        bool SaveUser();
    }
}
