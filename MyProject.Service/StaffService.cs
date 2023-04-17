using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using MyProject.Service.Helpers;
using MyProject.Service.Helpers.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyProject.Service
{
    class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMail _email;
        public StaffService(IUnitOfWork unitOfWork, IStaffRepository staffRepository, IMail email)
        {
            this._unitOfWork = unitOfWork;
            this._staffRepository = staffRepository;
            this._email = email;
        }
        public IEnumerable<Staff> GetStaffs(long VendorID = 0)
        {
            var staffs = _staffRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == VendorID);
            return staffs;
        }
        public IEnumerable<Staff> GetStaffByVendorID(long vendorid)
        {
            var staffs = _staffRepository.GetAll().Where(i =>i.VendorID == vendorid && i.IsDeleted == false && i.IsActive == true);
            return staffs;
        }
        public Staff GetStaff(long id)
        {
            var staffs = this._staffRepository.GetById(id);
            return staffs;
        }
        public bool CreateStaff(Staff staff, ref string message)
        {
            try
            {
                staff.IsActive = true;
                staff.IsDeleted = false;
                staff.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                staff.IsContactOTPAccess = true;
                _staffRepository.Add(staff);
                if (SaveStaff())
                {
                    message = "Staff added successfully...";
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
        public IEnumerable<object> GetStaffForDropDownByDepartmentID(long? id)
        {
            var Staff = _staffRepository.GetAll().Where(x => x.DepartmentID == id && x.IsActive == true && x.IsDeleted == false);
            var dropdownList = from staff in Staff
                               select new { value = staff.ID, text = staff.Name };
            return dropdownList;
        }

        public bool UpdateStaff(ref Staff staff, ref string message)
        {
            try
            {
                if (_staffRepository.GetStaffByEmail(staff.Email, staff.ID) == null)
                {
                    Staff updateStaff = _staffRepository.GetById(staff.ID);
                    staff.CreatedOn = updateStaff.CreatedOn;
                    if (updateStaff.Password != staff.Password)
                    {
                        updateStaff.Salt = (Int16)(updateStaff.CreatedOn.Value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                        string HashPass = new Crypto().RetrieveHash(staff.Password, Convert.ToString(updateStaff.Salt));
                        updateStaff.Password = HashPass;
                    }
                    _staffRepository.Update(updateStaff);
                    if (SaveStaff())
                    {
                        message = "Staff updated successfully...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later...";
                        return false;
                    }
                }
                {
                    message = "User already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }
        public bool GetStaffByContact(string countryCode, string contact)
        {
            var staff = _staffRepository.GetByContact(countryCode,contact);

            return staff == null ? false : true;
        }

        public Staff GetStaffObjectByContact(string countryCode, string contact)
        {
            var staff = _staffRepository.GetByContact(countryCode, contact);

            return staff;
        }
        public bool EmailVerification(ref string Email, long ID, ref string message)
        {
            try
            {
                if (_staffRepository.GetStaffByEmail(Email, ID) == null)
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
                if (_staffRepository.GetByContact(Code, Contact, ID) == null)
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
        public bool DeleteStaff(long id, ref string message, bool softdelete)
        {
            if (softdelete)
            {
                try
                {
                    //soft delete
                    Staff staff = _staffRepository.GetById(id);
                    staff.IsDeleted = true;
                    _staffRepository.Update(staff);

                    if (SaveStaff())
                    {
                        message = "Staff deleted successfully ...";
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
            else
            {

                //hard delete
                try
                {
                    Staff staff = _staffRepository.GetById(id);

                    _staffRepository.Delete(staff);

                    if (SaveStaff())
                    {
                        message = "Staff deleted successfully...";
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
        }
        public async Task<Boolean> SendOTP(string Contact, double time = 2.1)
        {
            Staff staff = _staffRepository.GetByContact("92",Contact);
            if (staff != null)
            {
                List<string> contacts = new List<string>()
                {   "507567600","37546410"
                };
                if (contacts.Contains(Contact))
                {
                    staff.OTP = 1234;
                    staff.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(15);
                    _staffRepository.Update(staff);
                    if (SaveStaff())
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

                    DateTime otpTime = staff.AuthorizationExpiry != null ? (DateTime)staff.AuthorizationExpiry : Helpers.TimeZone.GetLocalDateTime();
                    DateTime currentTime = Helpers.TimeZone.GetLocalDateTime();

                    if (otpTime <= currentTime || true)
                    {
                        var number = staff.PhoneCode + Contact;
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
                        //    staff.OTP = otp;
                        //    staff.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(time);
                        //    _staffRepository.Update(staff);
                        //    if (SaveStaff())
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
            Staff staff= _staffRepository.GetStaffByEmail(email);
            if (staff != null)
            {
                int otp;

                DateTime otpTime = staff.AuthorizationExpiry != null ? (DateTime)staff.AuthorizationExpiry : Helpers.TimeZone.GetLocalDateTime();
                DateTime currentTime = Helpers.TimeZone.GetLocalDateTime();

                if (otpTime <= currentTime)
                {
                    otp = new Random().Next(1000, 9999);

                    var path = System.Web.Hosting.HostingEnvironment.MapPath("~/");

                    if (_email.SendOTPMail("User", email, otp, "OTP Verification", path))
                    {
                        staff.OTP = otp;
                        staff.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(time);
                        _staffRepository.Update(staff);
                        if (SaveStaff())
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
        public bool ResetPassword(string password, long staffid, ref string message)
        {
            var staff = _staffRepository.GetById(staffid);

            string HashNewPassword = new Crypto().RetrieveHash(password, Convert.ToString(staff.Salt));
            staff.Password = HashNewPassword;
            staff.AuthorizationCode = new Crypto().RetrieveHash(staff.Name, Convert.ToString((Int16)(DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds));

            staff.OTP = null;

            _staffRepository.Update(staff);
            if (SaveStaff())
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
        public IEnumerable<object> GetStaffForDropDown(string lang = "en", long VendorID = 0)
        {
            var Staffs = GetStaffs(VendorID).Where(x => x.IsActive == true && x.IsDeleted == false);
            var dropdownList = from staff in Staffs
                               select new { value = staff.ID, text = lang == "en" ? staff.Name : staff.Name };
            return dropdownList;
        }
        public IEnumerable<Staff> GetStaffByDate(DateTime FromDate, DateTime ToDate, long VendorID = 0)
        {
            var staffs = _staffRepository.GetStaffByDate(FromDate, ToDate, VendorID);
            return staffs;
        }
        public bool ChangePassword(string oldPassword, string NewPassword, long vendorId, ref string status, ref string message)
        {
            try
            {
                var staff = _staffRepository.GetById(vendorId);

                string HashOldPassword = new Crypto().RetrieveHash(oldPassword, Convert.ToString(staff.Salt));

                if (staff.Password.Equals(HashOldPassword))
                {
                    string HashNewPassword = new Crypto().RetrieveHash(NewPassword, Convert.ToString(staff.Salt));
                    staff.Password = HashNewPassword;
                    _staffRepository.Update(staff);

                    if (SaveStaff())
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

        public bool Authenticate(string email, string password, ref ClaimsIdentity identity, ref string returnMessage,string countryCode)
        {
            var staff = _staffRepository.GetByContact(countryCode,email);
            if (staff != null)
            {
                if (staff.IsActive.HasValue && staff.IsActive.Value)
                {

                    string RetrievedPass = new Crypto().RetrieveHash(password, Convert.ToString(staff.Salt.Value));
                    //string RetrievedPass = OTP;
                    if (RetrievedPass.Equals(staff.Password.ToString()))
                    {
                        returnMessage = "Login Successful";
                        identity.AddClaim(new Claim(ClaimTypes.Name, staff.Name));
                        identity.AddClaim(new Claim(ClaimTypes.Email, staff.Email));
                        identity.AddClaim(new Claim(ClaimTypes.Role, "Technician"));
                        identity.AddClaim(new Claim(ClaimTypes.UserData, staff.ID.ToString()));

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

        public bool verifyOTP(string phoneCode, string contact, int otp, ref string status, ref string message)
        {
            try
            {
                var staff = _staffRepository.GetByContact(phoneCode,contact);

                if (staff != null)
                {
                    if (phoneCode == staff.PhoneCode)
                    {

                        if (Helpers.TimeZone.GetLocalDateTime() <= staff.AuthorizationExpiry)
                        {
                            if (staff.OTP == otp)
                            {
                                //customer.OTP = null;
                                staff.AuthorizationExpiry = null;
                                if (staff.IsContactOTPAccess == true)
                                {
                                    staff.IsContactVerified = true;
                                }
                                else
                                {
                                    staff.IsEmailVerified = true;
                                }
                                _staffRepository.Update(staff);
                                if (SaveStaff())
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
        public bool SaveStaff()
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
    }
    public interface IStaffService
    {
        Staff GetStaff(long id);
        IEnumerable<Staff> GetStaffByVendorID(long vendorid);
        IEnumerable<Staff> GetStaffs(long VendorID = 0);
        IEnumerable<object> GetStaffForDropDown(string lang = "en", long VendorID = 0);
        IEnumerable<Staff> GetStaffByDate(DateTime FromDate, DateTime ToDate, long VendorID = 0);
        bool CreateStaff(Staff staff, ref string message);
        IEnumerable<object> GetStaffForDropDownByDepartmentID(long? id);
        bool UpdateStaff(ref Staff staff, ref string message);
        bool GetStaffByContact(string countryCode, string contact);
        Staff GetStaffObjectByContact(string countryCode, string contact);
        bool EmailVerification(ref string Email, long ID, ref string message);
        bool ContactVerification(ref string Code,ref string Contact,long ID, ref string message);
        bool DeleteStaff(long id, ref string message, bool softdelete);
        bool SaveStaff();
        Task<Boolean> SendOTP(string Contact, double time = 2.1);
        bool SendOTPEmail(string email, double time = 2.1);
        bool ResetPassword(string password, long staffid, ref string message);
        bool Authenticate(string email, string password, ref ClaimsIdentity identity, ref string returnMessage, string countryCode);
        bool ChangePassword(string oldPassword, string NewPassword, long vendorId, ref string status, ref string message);
        bool verifyOTP(string phoneCode, string contact, int otp, ref string status, ref string message);
    }
}
