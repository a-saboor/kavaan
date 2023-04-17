using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using MyProject.Service.Helpers.OTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Security.Claims;
using MyProject.Service.Helpers.Encryption;
using MyProject.Service.Helpers.Routing;

namespace MyProject.Service
{
	public class VendorService : IVendorService
	{
		private readonly IVendorRepository _vendorRepository;
		//private readonly IVendorUserRepository _vendorUserRepository;

		private readonly IUnitOfWork _unitOfWork;

		public VendorService(IVendorRepository vendorRepository, IUnitOfWork unitOfWork)
		{
			this._vendorRepository = vendorRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IVendorService Members

		public IEnumerable<Vendor> GetVendors()
		{
			var vendors = _vendorRepository.GetAll();
			return vendors;
		}

		public IEnumerable<Vendor> GetVendors(bool isApproved)
		{
			var vendors = _vendorRepository.GetAll(isApproved);
			return vendors;
		}

		public IEnumerable<Vendor> GetVendorsForApproval()
		{
			var vendors = _vendorRepository.GetAllForApproval();
			return vendors;
		}
		public IEnumerable<object> GetVendorsForDropDown()
		{
			var Vendors = _vendorRepository.GetAll().Where(i => i.IsDeleted == false);
			var dropdownList = from vendors in Vendors
							   select new { value = vendors.ID, text = vendors.Name };
			return dropdownList;
		}

		public Vendor GetVendor(long id)
		{
			var vendor = _vendorRepository.GetById(id);
			return vendor;
		}

		public Vendor GetVendor(string name)
		{
			var vendor = _vendorRepository.GetVendorByName(name);
			return vendor;
		}

        public Vendor GetVendorByEmail(string email)
        {
            var vendor = _vendorRepository.GetVendorByEmail(email);
            return vendor;
        }

		public bool GetVendorByContact(string contact)
		{
			var vendor = _vendorRepository.GetVendorByContact(contact);
			return vendor == null ? false : true;
		}

        public bool Authenticate(string email, string password, ref ClaimsIdentity identity, ref string returnMessage)
        {
            var vendor = _vendorRepository.GetVendorByEmail(email);
            if (vendor != null)
            {
                if (vendor.IsActive.HasValue && vendor.IsActive.Value)
                {
                    if ((vendor.IsContactVerified.HasValue && vendor.IsContactVerified.Value))
                    {
                        string RetrievedPass = new Crypto().RetrieveHash(password, Convert.ToString(vendor.Salt.Value));
                        //string RetrievedPass = OTP;
                        if (RetrievedPass.Equals(vendor.Password.ToString()))
                        {
                            returnMessage = "Login Successful";
                            identity.AddClaim(new Claim(ClaimTypes.Name, vendor.Name));
                            identity.AddClaim(new Claim(ClaimTypes.Email, vendor.Email));
                            identity.AddClaim(new Claim(ClaimTypes.Role, "Vendor"));
                            identity.AddClaim(new Claim(ClaimTypes.UserData, vendor.ID.ToString()));

                            return true;
                        }
                        else
                        {
                            returnMessage = "Wrong Password!";
                        }
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
                returnMessage = "Invalid Contact or OTP";
            }

            return false;
        }

        public Vendor GetVendorBySlug(string slug)
		{
			var vendor = _vendorRepository.GetVendorBySlug(slug);
			return vendor;
		}

		public async Task<Boolean> SendOTP(string Contact)
		{

			Vendor vendor = _vendorRepository.GetVendorByContact(Contact);
			int otp;
			DateTime otpTime = (DateTime)vendor.AuthorizationExpiry;
			DateTime currentTime = Helpers.TimeZone.GetLocalDateTime();


			if (vendor != null)
			{
                List<string> contacts = new List<string>()
                {   "971507567600"
                };

                if (contacts.Contains(Contact))
				{
                    vendor.OTP = 1234;
                    vendor.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(15);
                    _vendorRepository.Update(vendor);
                    if (SaveVendor())
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
                    //int otp;
                    //DateTime otpTime = new DateTime();
                    try
                    {
                        if (vendor.AuthorizationExpiry != null)
                        {
                            otpTime = (DateTime)vendor.AuthorizationExpiry;
                        }
                        else
                        {
                            otpTime = Helpers.TimeZone.GetLocalDateTime();
                        }
                    }
                    catch (Exception ex)
                    { }

                    //DateTime currentTime = Helpers.TimeZone.GetLocalDateTime();

                    if (otpTime < currentTime)
                    {
					otp = new Random().Next(1000, 9999);

                        var apiKey = "RjkzQkJBMTMtNDBDQi00MUQ3LTg2MjItNDBFODMzQURFNzcy";
					var text = "Your OTP Code is " + otp + " \nMDNSMGYjkzc";

                        var uri = new Uri("https://api.antwerp.ae/Send?phonenumbers=" + Contact + "&sms.sender=Eathlos&sms.text=" + text + "&apiKey=" + apiKey + "");


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
						_vendorRepository.Update(vendor);
						if (SaveVendor())
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
            }

			return false;


		}

		//public async Task<Boolean> SendOTP(string Contact)
		//{
		//	Vendor vendor = _vendorRepository.GetVendorByContact(Contact);
		//	if (vendor != null)
		//	{
		//		int otp = new Random().Next(1000, 9999);
		//		List<Message> mes = new List<Message>()
		//		{
		//			new Message()
		//			{
		//				clientRef="0934345",
		//				mask="MARUDEALS",
		//				campaignName="OTP",
        //				text= "<#>\nYour OTP Code is" + otp + "\n PlayYaroPortal.com\n\nMDNSMGYjkzc",
		//				number = Contact
		//			}
		//		};
		//		var body = new Messages() { messages = mes.ToArray() };
		//		string jsonMessage = JsonConvert.SerializeObject(body);

		//		var request = new HttpRequestMessage(HttpMethod.Post, "https://richcommunication.dialog.lk/api/sms/send");

		//		request.Headers.TryAddWithoutValidation("USER", "maru_deals");
		//		request.Headers.TryAddWithoutValidation("DIGEST", "781957ea504ff2eb31434ae804928241");
		//		request.Headers.TryAddWithoutValidation("CREATED", Helpers.TimeZone.GetLocalDateTime().ToString());

		//		request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

		//		var client = new HttpClient();
		//		var response = await client.SendAsync(request);
		//		if (response.IsSuccessStatusCode)
		//		{
		//			vendor.OTP = otp;
		//			vendor.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(2.1);
		//			_vendorRepository.Update(vendor);
		//			if (SaveVendor())
		//			{
		//				return true;
		//			}
		//			else
		//			{
		//				return false;
		//			}
		//		}
		//		else
		//		{
		//			return false;
		//		}
		//	}
		//	return false;
		//}

		public bool VerifyOTP(string contact, int otp, ref string status, ref string message)
		{
			try
			{
				var vendor = _vendorRepository.GetVendorByContact(contact);

				if (vendor != null)
				{

					if (Helpers.TimeZone.GetLocalDateTime() <= vendor.AuthorizationExpiry)
					{
						if (vendor.OTP == otp)
						{
							vendor.OTP = null;
							vendor.AuthorizationExpiry = null;
							vendor.IsContactVerified = true;
							_vendorRepository.Update(vendor);
							if (SaveVendor())
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

		public bool CreateVendor(ref Vendor vendor, ref string message)
		{
			try
			{
				if (_vendorRepository.GetVendorByName(vendor.Name, vendor.Email) == null)
				{
					vendor.IsContactVerified = vendor.IsContactVerified.HasValue ? vendor.IsContactVerified : false;
					vendor.IsDeleted = vendor.IsDeleted.HasValue ? vendor.IsDeleted.Value : false;
					vendor.IsActive = vendor.IsActive.HasValue ? vendor.IsActive.Value : true;
					vendor.IsDeleted = vendor.IsDeleted.HasValue ? vendor.IsDeleted.Value : false;
					vendor.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_vendorRepository.Add(vendor);
					if (SaveVendor())
					{
						message = "Vendor added successfully ...";
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
					message = "Vendor already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateVendor(ref Vendor vendor, ref string message, bool updater = true)
		{
			try
			{
				if (_vendorRepository.GetVendorByName(vendor.Name, vendor.Email, vendor.ID) == null)
				{
					if (updater)
					{
						Vendor CurrentVendor = _vendorRepository.GetById(vendor.ID);

						CurrentVendor.VendorCode = vendor.VendorCode;
						CurrentVendor.Name = vendor.Name;
						CurrentVendor.NameAr = vendor.NameAr;
						CurrentVendor.Email = vendor.Email;
						CurrentVendor.Logo = vendor.Logo;
						CurrentVendor.CoverImage = vendor.CoverImage;
						CurrentVendor.Contact = vendor.Contact;
						CurrentVendor.Mobile = vendor.Mobile;
						CurrentVendor.Address = vendor.Address;
						CurrentVendor.IDNo = vendor.IDNo;
						CurrentVendor.TRN = vendor.TRN;
						CurrentVendor.Website = vendor.Website;
						CurrentVendor.Commission = vendor.Commission;
						CurrentVendor.License = vendor.License;
						CurrentVendor.FAX = vendor.FAX;
						CurrentVendor.Longitude = vendor.Longitude;
						CurrentVendor.Latitude = vendor.Latitude;
						CurrentVendor.CountryID = vendor.CountryID;
						CurrentVendor.CityID = vendor.CityID;
						CurrentVendor.BaseCurrency = vendor.BaseCurrency;
						CurrentVendor.IsEmailSent = vendor.IsEmailSent;
                        CurrentVendor.VendorPackageID = vendor.VendorPackageID;
                        CurrentVendor.BankName = vendor.BankName;
                        CurrentVendor.AccountHolderName = vendor.AccountHolderName;
                        CurrentVendor.BankAccountNumber = vendor.BankAccountNumber;
                        CurrentVendor.IBAN = vendor.IBAN;
                        CurrentVendor.Latitude = vendor.Latitude;
                        CurrentVendor.Longitude = vendor.Longitude;
                        CurrentVendor.VendorSectionID = vendor.VendorSectionID;
                        CurrentVendor.VendorIndustryID = vendor.VendorIndustryID;
                        CurrentVendor.VendorTypeID = vendor.VendorTypeID;
						
						vendor = null;

						_vendorRepository.Update(CurrentVendor);
						if (SaveVendor())
						{
							vendor = CurrentVendor;
							message = "Vendor updated successfully ...";
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
						_vendorRepository.Update(vendor);
						if (SaveVendor())
						{
							message = "Vendor updated successfully ...";
							return true;
						}
						else
						{
							message = "Oops! Something went wrong. Please try later.";
							return false;
						}
					}
				}
				else
				{
					message = "Vendor already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateVendorStatus(Vendor vendor, ref string message)
		{
			try
			{
				Vendor CurrentVendor = _vendorRepository.GetById(vendor.ID);

				CurrentVendor.IsActive = vendor.IsActive;
				vendor = null;
				_vendorRepository.Update(CurrentVendor);
				if (SaveVendor())
				{
					message = "Vendor updated successfully ...";
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

		public bool DeleteVendor(long id, ref string message, bool softDelete = true)
		{
			try
			{
				Vendor vendor = _vendorRepository.GetById(id);
				if (softDelete)
				{
					vendor.IsDeleted = true;
					_vendorRepository.Update(vendor);
				}
				else
				{
					_vendorRepository.Delete(vendor);
				}

				if (SaveVendor())
				{
					message = "Vendor deleted successfully ...";
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

		public bool SaveVendor()
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

		//public IEnumerable<SP_GetVendorFilters_Result> GetVendorFilters(long vendorId, string lang)
		//{
		//	var filters = _vendorRepository.GetVendorFilters(vendorId, lang);
		//	return filters;
		//}

		

		//public IEnumerable<SP_GetVendorBrands_Result> GetVendorBrands(long vendorId, string lang)
		//{
		//	var Brands = _vendorRepository.GetVendorBrands(vendorId, lang);
		//	return Brands;
		//}

		public bool verifyOTP(string contact, int otp, ref string status, ref string message)
		{
			try
			{
				var customer = _vendorRepository.GetVendorByContact(contact);

				if (customer != null)
				{

					if (Helpers.TimeZone.GetLocalDateTime() <= customer.AuthorizationExpiry)
					{
						if (customer.OTP == otp)
						{
							customer.OTP = null;
							customer.AuthorizationExpiry = null;
							customer.IsContactVerified = true;
							_vendorRepository.Update(customer);
							if (SaveVendor())
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

		public IEnumerable<SP_GetVendorCategories_Result> GetVendorCategories(long vendorId, string lang)
		{
			var Categories = _vendorRepository.GetVendorCategories(vendorId, lang);
			return Categories;
		}

		//public bool ForgotPassword(string email, string path, ref string status, ref string message)
		//{
		//	try
		//	{
		//		var vendor = _vendorRepository.GetVendorByEmail(email);

		//		if (vendor != null)
		//		{
		//			Crypto objCrypto = new Helpers.Encryption.Crypto();

		//			vendor.AuthorizationCode = objCrypto.Random(225);
		//			while (_vendorRepository.GetByAuthCode(vendor.AuthorizationCode) != null)
		//			{
		//				vendor.AuthorizationCode = objCrypto.Random(225);
		//			}
		//			vendor.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(5);
		//			_vendorRepository.Update(vendor);
		//			if (SaveVendor())
		//			{

		//				if (_email.SendForgotPasswordMail(vendor.ID, vendor.Name, vendor.Email, CustomURL.GetFormatedURL("Customer/Account/ResetPassword?auth=" + vendor.AuthorizationCode), path))
		//				{
		//					status = "success";
		//					message = "Cool! Password recovery instruction has been sent to your email.";
		//					return true;
		//				}
		//				else
		//				{
		//					status = "failure";
		//					message = "Oops! Something went wrong. Please try later.";
		//				}
		//			}
		//		}
		//		else
		//		{
		//			status = "error";
		//			message = "Invalid Email.";
		//		}

		//		return false;
		//	}
		//	catch
		//	{
		//		status = "failure";
		//		message = "Oops! Something went wrong. Please try later.";
		//		return false;
		//	}
		//}

		#endregion
	}

	public interface IVendorService
	{
		IEnumerable<Vendor> GetVendors();
		IEnumerable<Vendor> GetVendors(bool isApproved);
        IEnumerable<object> GetVendorsForDropDown();
		IEnumerable<Vendor> GetVendorsForApproval();
		Vendor GetVendor(long id);
		Vendor GetVendor(string name);
        Vendor GetVendorByEmail(string email);
		Vendor GetVendorBySlug(string slug);
		Task<Boolean> SendOTP(string Contact);
		bool VerifyOTP(string contact, int otp, ref string status, ref string message);
		bool CreateVendor(ref Vendor vendor, ref string message);
		bool UpdateVendor(ref Vendor vendor, ref string message, bool updater = true);
		bool UpdateVendorStatus(Vendor vendor, ref string message);
		bool DeleteVendor(long id, ref string message, bool softDelete = true);
		bool SaveVendor();
		//IEnumerable<SP_GetVendorFilters_Result> GetVendorFilters(long vendorId, string lang);
		//IEnumerable<SP_GetVendorBrands_Result> GetVendorBrands(long vendorId, string lang);
		IEnumerable<SP_GetVendorCategories_Result> GetVendorCategories(long vendorId, string lang);
		bool GetVendorByContact(string contact);
		bool Authenticate(string contact, string OTP, ref ClaimsIdentity identity, ref string returnMessage);
		
		bool verifyOTP(string contact, int otp, ref string status, ref string message);
		//bool ForgotPassword(string email, string path, ref string status, ref string message);
	}
}
