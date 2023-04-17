using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Service.Helpers.Routing;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;

namespace MyProject.Service.Helpers
{
	public class Mail : IMail
	{
		private string AdminEmail { get; set; }
		private string NetworkEmail { get; set; }
		private string ContactEmail { get; set; }
		private string NetworkPassword { get; set; }
		private string FromEmail { get; set; }
		private string DisplayName { get; set; }
		private string WebsiteLink { get; set; }
		private int Port { get; set; }
		private string Host { get; set; }
		private bool EnableSsl { get; set; }
		//Dynamic Email Links
		private string PlayStoreLink { get; set; }
		private string AppStoreLink { get; set; }
		private string FacebookLink { get; set; }
		private string InstagramLink { get; set; }
		private string YoutubeLink { get; set; }
		private string TwitterLink { get; set; }
		private string LinkedinLink { get; set; }

		private readonly IEmailSettingService _emailservice;
		private readonly IBusinessSettingService _businessSettingService;
		private readonly IUnitOfWork _unitOfWork;

		public Mail(IEmailSettingService emailservice, IBusinessSettingService businessSettingService, IUnitOfWork unitOfWork)
		{
			this._emailservice = emailservice;
			this._businessSettingService = businessSettingService;
			this._unitOfWork = unitOfWork;

			
			var EmailSetting = _emailservice.GetDefaultEmailSetting();
			if (EmailSetting != null)
			{
				NetworkEmail = EmailSetting.EmailAddress;
				ContactEmail = EmailSetting.ContactEmail;
				NetworkPassword = EmailSetting.Password;
				FromEmail = EmailSetting.EmailAddress;
				DisplayName = "Kavaan Real Estate";
				Port = Convert.ToInt16(EmailSetting.Port);
				Host = EmailSetting.Host;
				EnableSsl = EmailSetting.EnableSSL.Equals(true) ? true : false;
			}

			var BusinessEmailSetting = _businessSettingService.GetDefaultBusinessSetting();
			if (BusinessEmailSetting != null)
			{
				AdminEmail = BusinessEmailSetting.Email;
				PlayStoreLink = BusinessEmailSetting.AndroidAppUrl;
				AppStoreLink = BusinessEmailSetting.IosAppUrl;
				FacebookLink = BusinessEmailSetting.Facebook;
				InstagramLink = BusinessEmailSetting.Instagram;
				YoutubeLink = BusinessEmailSetting.Youtube;
				TwitterLink = BusinessEmailSetting.Twitter;
				LinkedinLink = BusinessEmailSetting.LinkedIn;
				WebsiteLink = "www.kavaan.com";
			}
		}
		public bool IsValidEmailAddress(string email)
		{
			try
			{
				var emailChecked = new System.Net.Mail.MailAddress(email);
				return true;
			}
			catch
			{
				return false;
			}
		}
		public bool SendMail(string email, string subject, string body)
		{
			try
			{
				// Credentials
				var credentials = new NetworkCredential(NetworkEmail, NetworkPassword);

				// Mail message
				var mail = new MailMessage()
				{
					From = new MailAddress(FromEmail, DisplayName),
					Subject = subject,
					Body = body,
					IsBodyHtml = true
				};

				mail.To.Add(new MailAddress(email));

				// Smtp client
				var client = new SmtpClient()
				{
					Port = Port,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					UseDefaultCredentials = false,
					Host = Host,
					EnableSsl = EnableSsl,
					Credentials = credentials
				};

				// Send it...         
				client.Send(mail);
				return true;

			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<bool> SendMailAsync(string email, string subject, string body)
		{
			try
			{
				// Credentials
				var credentials = new NetworkCredential(NetworkEmail, NetworkPassword);

				// Mail message
				var mail = new MailMessage()
				{
					From = new MailAddress(FromEmail, DisplayName),
					Subject = subject,
					Body = body,
					IsBodyHtml = true
				};

				mail.To.Add(new MailAddress(email));

				// Smtp client
				var client = new SmtpClient()
				{
					Port = Port,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					UseDefaultCredentials = false,
					Host = Host,
					EnableSsl = EnableSsl,
					Credentials = credentials
				};

				// Send it...         
				await client.SendMailAsync(mail);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SendMail(string email, string subject, string body, AlternateView alternate, bool isImage = false)
		{
			try
			{
				// Credentials
				var credentials = new NetworkCredential(NetworkEmail, NetworkPassword);

				// Mail message
				var mail = new MailMessage()
				{
					From = new MailAddress(FromEmail, DisplayName),
					Subject = subject,
					Body = body,
					IsBodyHtml = true
				};

				mail.To.Add(new MailAddress(email));
				if (alternate != null)
					mail.AlternateViews.Add(alternate);

				// Smtp client
				var client = new SmtpClient()
				{
					Port = Port,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					UseDefaultCredentials = false,
					Host = Host,
					EnableSsl = EnableSsl,
					Credentials = credentials
				};

				// Send it...         
				client.Send(mail);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SendAccountCreationMail(string email, string password, string userName, string url)
		{
			try
			{
				string subject = DisplayName + " | Registration Confirmation";
				string body = string.Empty;

				var dirPath = Assembly.GetExecutingAssembly().Location;
				dirPath = Path.GetDirectoryName(dirPath);
				var template = Path.GetFullPath(Path.Combine(dirPath, "/Assets/EmailTemplates/AccountVerification.htm"));


				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}
				body = body.Replace("{Url}", CustomURL.GetFormatedURL(url));
				body = body.Replace("{UserName}", userName);
				body = body.Replace("{Email}", email);
				body = body.Replace("{Password}", password);
				AddLinksInBody(ref body);

				if (SendMail(email, subject, body, null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SendInterviewSchedule(string email, string name, string schedule, string path)
		{
			try
			{
				string subject;
				string body = string.Empty;
				string template = string.Empty;

				//var template = Path.GetFullPath(Path.Combine(path, "/Assets/EmailTemplates/ForgotPassword.htm"));
				if (schedule == "nil")//Rejection email sent process
				{
					subject = DisplayName + " | HR";
					template = string.Format("{0}/Assets/EmailTemplates/Candidate/CandidateRejection.html", path);
				}
				else
				{
					subject = DisplayName + " | Interview Schedule";
					template = string.Format("{0}/Assets/EmailTemplates/Candidate/CandidateShortlist.html", path);
				}
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}
				body = body.Replace("{Candidate}", name);
				AddLinksInBody(ref body);
				if (schedule == "nil")//Rejection email sent process
				{
				}
				else
				{
					body = body.Replace("{Schedule}", schedule);
				}

				if (SendMail(email, subject, body, null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SendForgotPasswordMail(long userId, string username, string Email, string url, string path)
		{
			try
			{
				string subject = DisplayName + " | Account Recovery ";
				string body = string.Empty;

				string template = string.Format("{0}/Assets/EmailTemplates/ForgotPassword.htm", path);
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{Url}", url);
				body = body.Replace("{UserName}", username);
				AddLinksInBody(ref body);

				if (SendMail(Email, subject, body))
				{
					return true;
				}
				else
				{

					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SendVerificationMail(string username, string Email, string url, string path)
		{
			try
			{
				string subject = "Please verify your Kavaan Real Estate account.";
				string body = string.Empty;

				string template = string.Format("{0}/Assets/EmailTemplates/AccountVerification.html", path);
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{Url}", url);
				body = body.Replace("{UserName}", username);
				AddLinksInBody(ref body);

				if (SendMail(Email, subject, body))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<bool> SendOrderMailAsync(string Email, string subject, string body)
		{
			try
			{
				var result = await SendMailAsync(Email, subject, body);
				return result;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SendOrderMail(string Email, string subject, string body)
		{
			try
			{
				var result = SendMail(Email, subject, body);
				return result;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SendVendorCreationMail(string Name, string Email, string UserEmail, string password, string url, string path)
		{
			try
			{
				string subject = "Welcome to Kavaan Real Estate";
				string body = string.Empty;

				string template = string.Format("{0}/Assets/EmailTemplates/Vendor/Wellcome.html", path);
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{UserName}", Name);
				body = body.Replace("{Email}", UserEmail);
				AddLinksInBody(ref body);

				if (string.IsNullOrEmpty(password))
				{
					body = body.Replace("{Password}", password);
				}
				else
				{
					body = body.Replace("{Password}", "Password : " + password);
				}
				body = body.Replace("{Url}", url);

				if (SendMail(Email, subject, body))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SendWellcomeMail(string Name, string Email, string UserEmail, string password, string url, string path)
		{
			try
			{
				string subject = "Welcome to Kavaan Real Estate";
				string body = string.Empty;

				string template = string.Format("{0}/Assets/EmailTemplates/Wellcome.html", path);
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{UserName}", Name);
				body = body.Replace("{Email}", UserEmail);
				AddLinksInBody(ref body);

				if (string.IsNullOrEmpty(password))
				{
					body = body.Replace("{Password}", password);
				}
				else
				{
					body = body.Replace("{Password}", "Password : " + password);
				}
				body = body.Replace("{Url}", url);

				if (SendMail(Email, subject, body))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public bool SendAppointmentMail(Appointment appointment, string path)
		{
			try
			{
				string subject = DisplayName + " | Appointment Confirmation";
				string message = string.Empty;
				string Email = string.Empty;
				string Name = string.Empty;
				string body = string.Empty;

				//appointment email setting
				Email = appointment.Customer.Email;
				Name = appointment.Customer.FirstName + appointment.Customer.LastName;

				if (appointment.IsCancelled == true)
				{
					subject = "Appointment Details";
					message = "Your appointment # " + appointment.AppointmentNo + " for " + appointment.Type + " has been Cancelled.";
					message += "<br>";
					message += "<br>";
					message += "<b>Appintment Date:</b> " + appointment.AppointmentDate.Value.ToString("dd MMM, yyyy");
					message += "<br>";
					message += "<b>Appintment Time:</b> " + appointment.AppointmentDate.Value.ToString("hh:mm tt").ToUpper();
					message += "<br>";
					message += "Kindly check your customer portal for more information.";
				}
				else if (appointment.IsApproved)
				{
					message = "Your appointment # " + appointment.AppointmentNo + " for " + appointment.Type + " has been Approved.";
					message += "<br>";
					message += "<br>";
					message += "<b>Appintment Date:</b> " + appointment.AppointmentDate.Value.ToString("dd MMM, yyyy");
					message += "<br>";
					message += "<b>Appintment Time:</b> " + appointment.AppointmentDate.Value.ToString("hh:mm tt").ToUpper();
				}

				string template = string.Format("{0}/Assets/EmailTemplates/AppointmentConfirmation.html", path);
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{Name}", Name);
				body = body.Replace("{Message}", message);
				AddLinksInBody(ref body);

				if (SendMail(Email, subject, body))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public bool SendDocumentStatusMail(string Email, string Username, string Subject, string message, string path)
		{
			try
			{
				string subject = Subject;
				string body = string.Empty;

				string template = string.Format("{0}/Assets/EmailTemplates/DocumentStatus.html", path);
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{Message}", message);
				body = body.Replace("{Username}", Username);
				AddLinksInBody(ref body);

				if (SendMail(Email, subject, body))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public bool SendDocumentRejectedMail(string Email, string Username, string Reason, string Subject, string message, string path)
		{
			try
			{
				string subject = Subject;
				string body = string.Empty;

				string template = string.Format("{0}/Assets/EmailTemplates/DocumentRejection.html", path);
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{Message}", message);
				body = body.Replace("{Username}", Username);
				body = body.Replace("{Reason}", Reason);
				AddLinksInBody(ref body);

				if (SendMail(Email, subject, body))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SendOTPMail(string Name, string Email, int OTP, string Subject, string path)
		{
			try
			{
				string subject = Subject;
				string body = string.Empty;

				string template = string.Format("{0}/Assets/EmailTemplates/OTPVerification.html", path);
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{UserName}", Name);
				body = body.Replace("{Code}", OTP.ToString());
				AddLinksInBody(ref body);

				if (SendMail(Email, subject, body))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		
		public bool SendPromoMail(string Email, string Subject, string message, string path)
		{
			try
			{
				string subject = Subject;
				string body = string.Empty;

				string template = string.Format("{0}/Assets/EmailTemplates/Promo.html", path);
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{Message}", message);
				AddLinksInBody(ref body);

				if (SendMail(Email, subject, body))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SendContactUsMail(string name, string email, string subject, string contact, string message)
		{
			try
			{
				string body = string.Empty;

				body += string.Format("Name : {0}", name);
				body += "<br />";
				body += string.Format("Contact : {0}", contact);
				body += "<br />";
				body += string.Format("Message : {0}", message);

				if (SendMail(ContactEmail, subject, body))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SendAcknowledgementMail(string email, string ucc, string candidate, string path)
		{
			try
			{
				string subject = DisplayName + " | Job Application Received";
				string body = string.Empty;

				string template = string.Format("{0}/Assets/EmailTemplates/Candidate/CandidateAplication.html", path);
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{UCC}", ucc);
				body = body.Replace("{Candidate}", candidate);
				AddLinksInBody(ref body);

				if (SendMail(email, subject, body))
				{
					return true;
				}
				else
				{

					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}


		#region Customer Enquiry
		public bool SendCustomerEnquiryConfirmationMail(string name, string email, string path)
		{
			try
			{
				string subject = DisplayName + " | Customer Enquiry";
				string message = "We have received your details, we will call back you soon. <br /><br />Thankyou";
				string body = string.Empty;

				string template = string.Format("{0}/Assets/EmailTemplates/CustomerEnquiryConfirmation.html", path);
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{Name}", name);
				body = body.Replace("{Message}", message);
				AddLinksInBody(ref body);

				if (SendMail(email, subject, body))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		//Send to Admin of Project, ref business settings
		public bool SendCustomerEnquiryMail(string name, string contact, string email, string path)
		{
			try
			{
				string subject = DisplayName + " | Customer Enquiry";
				string message = "A new customer {name} enquiry has been submitted through the website.";
				string body = string.Empty;

				string template = string.Format("{0}/Assets/EmailTemplates/CustomerEnquiry.html", path);
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{Message}", message);
				body = body.Replace("{Name}", name);
				body = body.Replace("{Email}", email);
				body = body.Replace("{Contact}", contact);
				AddLinksInBody(ref body);

				if (SendMail(ContactEmail, subject, body))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		#endregion

		public bool SendTestEmail(string Name, string Email, string Subject, string emailTemplate, string path)
		{
			try
			{
				string subject = Subject;
				string body = string.Empty;

				string template = string.Format("{0}/Assets/EmailTemplates/{1}.html", path, emailTemplate);
				using (StreamReader reader = new StreamReader(template))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{UserName}", Name);
				AddLinksInBody(ref body);

				if (SendMail(Email, subject, body))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void AddLinksInBody(ref string body) 
		{
			//App Links
			body = body.Replace("{AppStoreLink}", !string.IsNullOrEmpty(AppStoreLink) ? AppStoreLink : "javacript: void(0)");
			body = body.Replace("{PlayStoreLink}", !string.IsNullOrEmpty(PlayStoreLink) ? PlayStoreLink : "javacript: void(0)");

			//Website Links
			body = body.Replace("{WebsiteLink}", !string.IsNullOrEmpty(WebsiteLink) ? WebsiteLink : "javacript: void(0)");

			//Social Links
			body = body.Replace("{FacebookLink}", !string.IsNullOrEmpty(FacebookLink) ? FacebookLink : "javacript: void(0)");
			body = body.Replace("{InstagramLink}", !string.IsNullOrEmpty(InstagramLink) ? InstagramLink : "javacript: void(0)");
			body = body.Replace("{YoutubeLink}", !string.IsNullOrEmpty(YoutubeLink) ? YoutubeLink : "javacript: void(0)");
			body = body.Replace("{TwitterLink}", !string.IsNullOrEmpty(TwitterLink) ? TwitterLink : "javacript: void(0)");
			body = body.Replace("{LinkedinLink}", !string.IsNullOrEmpty(LinkedinLink) ? LinkedinLink : "javacript: void(0)");
		}
	}

	public interface IMail
	{
		bool IsValidEmailAddress(string email);
		bool SendPromoMail(string Email, string Subject, string message, string path);
		bool SendMail(string email, string subject, string body);
		bool SendMail(string email, string subject, string body, AlternateView alternate, bool isImage = false);
		bool SendAccountCreationMail(string email, string password, string userName, string url);
		bool SendInterviewSchedule(string email, string name, string schedule, string path);
		bool SendForgotPasswordMail(long userId, string username, string Email, string url, string path);
		bool SendVerificationMail(string username, string Email, string url, string path);
		Task<bool> SendOrderMailAsync(string Email, string subject, string body);
		bool SendAppointmentMail(Appointment appointment, string path);
		bool SendOrderMail(string Email, string subject, string body);
		bool SendVendorCreationMail(string Name, string Email, string UserEmail, string password, string url, string path);
		bool SendWellcomeMail(string Name, string Email, string UserEmail, string password, string url, string path);
		bool SendContactUsMail(string name, string email, string subject, string contact, string message);
		bool SendAcknowledgementMail(string email, string ucc, string candidate, string path);
		bool SendDocumentStatusMail(string Email, string Username, string Subject, string message, string path);
		bool SendDocumentRejectedMail(string Email, string Username, string Reason, string Subject, string message, string path);
		bool SendOTPMail(string Name, string Email, int OTP, string Subject, string path);
		#region Customer Enquiry
		bool SendCustomerEnquiryConfirmationMail(string name, string email, string path);
		bool SendCustomerEnquiryMail(string name, string contact, string email, string path);

		#endregion

		bool SendTestEmail(string Name, string Email, string Subject, string emailTemplate, string path);


	}
}
