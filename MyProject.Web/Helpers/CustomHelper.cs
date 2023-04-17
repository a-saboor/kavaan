using MyProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace MyProject.Web.Helpers
{

	public static class CustomHelper
	{
		public static string GetDateFormat = "yyyy-MM-dd hh:mm:ss tt";
		public static string GetDateFormat2 = "MMM dd, yyyy";
		public static string GetDateFormat3 = "dd MMM yyyy, hh:mm tt";

		public static string MapKey
		{
			get
			{
				return HttpContext.Current.Session["MapKey"] != null ? HttpContext.Current.Session["MapKey"].ToString() : "AIzaSyAGeDHQMAVN3jAAPXYDvf87HCNPFK4hPX4";
			}
		}
	}

	#region Users Helpers
	public static class CustomerCustomHelper
	{

	}

	public static class AdminCustomHelper
	{
		public static string GetDateFormat = "dd MMM yyyy, hh:mm tt";

		public static DateTime GetFromDate(int days = 365)
		{
			return Helpers.TimeZone.GetLocalDateTime().AddDays(-days);
		}
		public static string GetFromDateString(int days = 365)
		{
			return GetFromDate(days).ToString("MM/dd/yyyy");
		}

		public static DateTime GetToDate(DateTime? date = null)
		{
			DateTime newDate = Helpers.TimeZone.GetLocalDateTime();
			if (date.HasValue)
			{
				newDate = date.Value;
			}
			
			return newDate.AddMinutes(1439.99);
		}
		public static string GetToDateString(DateTime? date = null)
		{
			return GetToDate(date).ToString("MM/dd/yyyy");
		}
	}

	public static class VendorCustomHelper
	{
		public static string GetDateFormat = "dd MMM yyyy, hh:mm tt";

		public static DateTime GetFromDate(int days = 365)
		{
			return Helpers.TimeZone.GetLocalDateTime().AddDays(-days);
		}
		public static string GetFromDateString(int days = 365)
		{
			return GetFromDate(days).ToString("MM/dd/yyyy");
		}

		public static DateTime GetToDate(DateTime? date = null)
		{
			DateTime newDate = Helpers.TimeZone.GetLocalDateTime();
			if (date.HasValue)
			{
				newDate = date.Value;
			}

			return newDate.AddMinutes(1439.99);
		}
		public static string GetToDateString(DateTime? date = null)
		{
			return GetToDate(date).ToString("MM/dd/yyyy");
		}
	}

	#endregion

	#region Session Helpers

	public static class CustomerSessionHelper
	{
		public static long ID
		{
			get
			{
				return HttpContext.Current.Session["CustomerID"] != null ? Convert.ToInt64(HttpContext.Current.Session["CustomerID"]) : 0;
			}
		}
		public static string UserName
		{
			get
			{
				return HttpContext.Current.Session["CustomerName"] != null ? HttpContext.Current.Session["CustomerName"].ToString() : "";
			}
		}
		public static string PhoneCode
		{
			get
			{
				return HttpContext.Current.Session["PhoneCode"] != null ? HttpContext.Current.Session["PhoneCode"].ToString() : "";
			}
		}
		public static string Contact
		{
			get
			{
				return HttpContext.Current.Session["Contact"] != null ? HttpContext.Current.Session["Contact"].ToString() : "";
			}
		}
		public static string Email
		{
			get
			{
				return HttpContext.Current.Session["Email"] != null ? HttpContext.Current.Session["Email"].ToString() : "";
			}
		}
		public static string Photo
		{
			get
			{
				return HttpContext.Current.Session["Photo"] != null ? HttpContext.Current.Session["Photo"].ToString() : "";
			}
		}
		public static DateTime AuthorizationExpiry
		{
			get
			{
				return HttpContext.Current.Session["CustomerAuthorizationExpiry"] != null ? Convert.ToDateTime(HttpContext.Current.Session["CustomerAuthorizationExpiry"]) : DateTime.MinValue;
			}
		}
		public static bool IsAuthorized
		{
			get
			{
				return HttpContext.Current.Session["CustomerID"] != null ? true : false;
			}
		}
		public static void setAuthorizeSessions(Customer customer)
		{
			HttpContext.Current.Session["CustomerID"] = customer.ID;
			HttpContext.Current.Session["CustomerName"] = !string.IsNullOrEmpty(customer.UserName) ? customer.UserName : customer.FirstName + customer.LastName;
			HttpContext.Current.Session["PhoneCode"] = customer.PhoneCode;
			HttpContext.Current.Session["Contact"] = customer.Contact;
			HttpContext.Current.Session["Email"] = customer.Email;
			HttpContext.Current.Session["Photo"] = customer.Logo;
			HttpContext.Current.Session["Points"] = customer.Points;
			HttpContext.Current.Session["AccountType"] = customer.AccountType;
			HttpContext.Current.Session["ReceiverType"] = "Customer";

			HttpContext.Current.Response.Cookies["Customer-Details"]["Name"] = CustomerSessionHelper.UserName;
			HttpContext.Current.Response.Cookies["Customer-Details"]["Logo"] = CustomerSessionHelper.Photo;
			HttpContext.Current.Response.Cookies["Customer-Session"]["Access-Token"] = customer.AuthorizationCode;
		}

		public static void setSignupSessions(Customer customer)
		{
			HttpContext.Current.Session["CustomerName"] = !string.IsNullOrEmpty(customer.UserName) ? customer.UserName : customer.FirstName + customer.LastName;
			HttpContext.Current.Session["PhoneCode"] = customer.PhoneCode;
			HttpContext.Current.Session["Contact"] = customer.Contact;
			HttpContext.Current.Session["Email"] = customer.Email;
			HttpContext.Current.Session["CustomerAuthorizationExpiry"] = customer.AuthorizationExpiry;
		}

		public static void setAuthorizationExpiry(Customer customer)
		{
			HttpContext.Current.Session["CustomerAuthorizationExpiry"] = customer.AuthorizationExpiry;
		}
	}

	public static class AdminSessionHelper
	{
		public static long ID
		{
			get
			{
				return HttpContext.Current.Session["AdminUserID"] != null ? Convert.ToInt64(HttpContext.Current.Session["AdminUserID"]) : 0;
			}
		}
		public static string UserName
		{
			get
			{
				return HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "";
			}
		}
		public static string Role
		{
			get
			{
				return HttpContext.Current.Session["Role"] != null ? HttpContext.Current.Session["Role"].ToString() : "";
			}
		}
		public static string Email
		{
			get
			{
				return HttpContext.Current.Session["Email"] != null ? HttpContext.Current.Session["Email"].ToString() : "";
			}
		}
		public static string ReceiverType
		{
			get
			{
				return HttpContext.Current.Session["ReceiverType"] != null ? HttpContext.Current.Session["ReceiverType"].ToString() : "";
			}
		}
		public static string MakeUserNameChar
		{
			get
			{
				string Chars = "";
				try
				{
					if (!string.IsNullOrEmpty(UserName))
					{
						string[] names = UserName.Split(' ');
						for (int i = 0; i < names.Length; i++)
						{
							char Char;
							Char = names[i].ToUpper().First();
							Chars += Char;
						}
					}
					else
					{
						Chars = "AD";
					}

				}
				catch (Exception)
				{
					Chars = "AD";
				}

				return Chars;
			}
		}
		public static string UserNameChar
		{
			get
			{
				return HttpContext.Current.Session["UserNameChar"] != null ? HttpContext.Current.Session["UserNameChar"].ToString() : "";
			}
		}
	}

	public static class VendorSessionHelper
	{
		public static long VendorID
		{
			get
			{
				return HttpContext.Current.Session["VendorID"] != null ? Convert.ToInt64(HttpContext.Current.Session["VendorID"]) : 0;
			}
		}
		public static long VendorUserID
		{
			get
			{
				return HttpContext.Current.Session["VendorUserID"] != null ? Convert.ToInt64(HttpContext.Current.Session["VendorUserID"]) : 0;
			}
		}
		public static string UserName
		{
			get
			{
				return HttpContext.Current.Session["VendorUserName"] != null ? HttpContext.Current.Session["VendorUserName"].ToString() : "";
			}
		}
		public static string Role
		{
			get
			{
				return HttpContext.Current.Session["VendorRole"] != null ? HttpContext.Current.Session["VendorRole"].ToString() : "";
			}
		}
		public static string Email
		{
			get
			{
				return HttpContext.Current.Session["VendorEmail"] != null ? HttpContext.Current.Session["VendorEmail"].ToString() : "";
			}
		}
		public static string ReceiverType
		{
			get
			{
				return HttpContext.Current.Session["VendorReceiverType"] != null ? HttpContext.Current.Session["VendorReceiverType"].ToString() : "";
			}
		}
		public static string MakeUserNameChar
		{
			get
			{
				string Chars = "";
				try
				{
					if (!string.IsNullOrEmpty(UserName))
					{
						string[] names = UserName.Split(' ');
						for (int i = 0; i < names.Length; i++)
						{
							char Char;
							Char = names[i].ToUpper().First();
							Chars += Char;
						}
					}
					else
					{
						Chars = "AD";
					}

				}
				catch (Exception)
				{
					Chars = "AD";
				}

				return Chars;
			}
		}
		public static string UserNameChar
		{
			get
			{
				return HttpContext.Current.Session["VendorUserNameChar"] != null ? HttpContext.Current.Session["VendorUserNameChar"].ToString() : "";
			}
		}
	}

	#endregion

}