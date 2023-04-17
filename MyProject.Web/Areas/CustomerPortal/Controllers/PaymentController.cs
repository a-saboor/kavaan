using Project.Payment;
using Project.Payment.ViewModel;
using Project.Service;
using Project.Web.Controllers;
using Project.Web.Helpers.Routing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static Project.Web.Helpers.Enumerations.Enumeration;

namespace Project.Web.Areas.CustomerPortal.Controllers
{
	//[AuthorizationProvider.AuthorizeCustomer]
	public class PaymentController : Controller
	{
		private readonly IServiceBookingService _bookingService;
		private readonly IInvoiceService _invoiceService;
		private readonly ITransaction _transaction;
		private readonly IMerchant _merchant;
		private readonly IPaymentGatewaySettingService _paymentGatewaySettingService;
		private readonly ITransactionService _transactionService;

		private const string currentBookingStatus = "Invoiced";

		public PaymentController(IServiceBookingService bookingService, IInvoiceService invoiceService, ITransaction transaction, IMerchant merchant, IPaymentGatewaySettingService paymentGatewaySettingService, ITransactionService transactionService)
		{
			this._bookingService = bookingService;
			this._invoiceService = invoiceService;
			this._transaction = transaction;
			this._merchant = merchant;
			this._paymentGatewaySettingService = paymentGatewaySettingService;
			this._transactionService = transactionService;
		}

		[HttpGet]
		public ActionResult Pay(long id, string culture = "en-ae")
		{
			//string lang = "en";
			//if (culture.Contains('-'))
			//	lang = culture.Split('-')[0];

			string message = string.Empty;
			var booking = _bookingService.GetServiceBooking(id);

			if (booking.Status.Equals(currentBookingStatus) && (!booking.IsPayed.HasValue || booking.IsPayed == false))
			{
				var invoice = _invoiceService.GetInvoiceByBooking(id);
				if (invoice == null)
				{
					/*Create Booking Invoice*/
					invoice = new Data.Invoice()
					{
						RecordID = booking.ID,
						InvoiceNo = booking.BookingNo/*.Replace("BKG", "INV")*/,
						//Amount = booking.TotalAmount,
						Amount = booking.Total,
						Description = booking.BookingNo + " payment invoice",
						PaymentMethod = "Card",
						Status = InvoiceStatus.Sent.ToString(),
						IsDeleted = false,
						CreatedOn = Helpers.TimeZone.GetLocalDateTime(),
					};

					_invoiceService.CreateInvoice(invoice, ref message);
				}

				_transaction.OrderID = invoice.InvoiceNo;
				_transaction.ReturnURL = CustomURL.GetFormatedURL("/" + culture + "/Customer/Payment/Paid/" + booking.ID);

				string error = string.Empty;
				if (_transaction.ProcessRestful(ref error, invoice.Amount))
				{
					string msg = string.Empty;
					booking.SessionID = _transaction.SessionID;
					_bookingService.UpdateServiceBooking(ref booking, ref msg);

					return Json(new
					{
						success = true,
						message = "Booking placed successfully!",
						booking = new
						{
							id = booking.ID,
							bookingNo = booking.BookingNo,
							invoice = new
							{
								id = invoice.ID
							}

						}
					}, JsonRequestBehavior.AllowGet);
				}
				else
				{
					//Update booking as invalid due to payment gateway issue
					booking.Status = "Invalid";
					_bookingService.UpdateServiceBooking(ref booking, ref message);
					return Json(new
					{
						success = false,
						message = Resources.Resources.SomethingWentWrong
					}, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				////Update booking as invalid due to payment gateway issue
				//booking.Status = "Invalid";
				//_bookingService.UpdateServiceBooking(ref booking, ref message);

				return Json(new
				{
					success = false,
					message = Resources.Resources.SomethingWentWrong
				}, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult Processing(string id, string culture = "en-ae")
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			id = id.ToString();
			var invoice = _invoiceService.GetInvoice(Convert.ToInt64(id));

			if (invoice == null)
			{
				return HttpNotFound();
			}

			var booking = _bookingService.GetServiceBooking((long)invoice.RecordID);
			OrderPaymentProcessingViewModel bookingPaymentProcessing = new OrderPaymentProcessingViewModel();

			bookingPaymentProcessing.Invoice = invoice;
			bookingPaymentProcessing.PaymentGatewaySetting = _paymentGatewaySettingService.GetDefaultPaymentGatewaySetting();
			bookingPaymentProcessing.Customer = booking.Customer;
			bookingPaymentProcessing.city = !string.IsNullOrEmpty(booking.CustomerCity) ? booking.CustomerCity : "x";

			if (Session["mastercard_session_id"] != null)
			{
				bookingPaymentProcessing.mastercard_session_id = Session["mastercard_session_id"].ToString();
				bookingPaymentProcessing.mastercard_session_version = Session["mastercard_session_version"].ToString();
				bookingPaymentProcessing.mastercard_success_indicator = Session["mastercard_success_indicator"].ToString();
				bookingPaymentProcessing.serial_expires = Session["serial_expires"].ToString();
			}
			else if (!string.IsNullOrEmpty(booking.SessionID))
			{
				bookingPaymentProcessing.mastercard_session_id = booking.SessionID;
				bookingPaymentProcessing.mastercard_session_version = "";
				bookingPaymentProcessing.mastercard_success_indicator = "";
				bookingPaymentProcessing.serial_expires = "";
			}
			else
			{
				return RedirectToAction("Paid");
			}
			return View(bookingPaymentProcessing);
		}

		public ActionResult Paid(long id, string culture = "en-ae")
		{
			//check if payment is successfull:
			string resultIndicator = Convert.ToString(Request.QueryString["resultIndicator"]);
			string mastercard_success_indicator = Convert.ToString(Session["mastercard_success_indicator"]);
			string message = string.Empty;
			/*Update Booking Status*/
			var booking = _bookingService.GetServiceBooking(id);

			if (resultIndicator == mastercard_success_indicator || true)
			{
				//create token:

				_merchant.LoadSetting();

				// [Snippet] howToConfigureURL - start
				StringBuilder url = new StringBuilder();
				if (!_merchant.GatewayHost.StartsWith("http"))
					url.Append("https://");
				url.Append(_merchant.GatewayHost);
				url.Append("/api/nvp/version/");
				url.Append(_merchant.Version);
				_merchant.GatewayUrl = url.ToString();

				Connection connection = new Connection(_merchant);

				StringBuilder RequestData = new StringBuilder();
				RequestData.Append("merchant=" + _merchant.MerchantId);
				RequestData.Append("&apiUsername=" + _merchant.Username);
				RequestData.Append("&apiPassword=" + _merchant.Password);
				RequestData.Append("&apiOperation=RETRIEVE_SESSION");
				RequestData.Append("&session.id=" + booking.SessionID);

				//String result3 = null;
				//String response3 = null;
				//String gatewayCode3 = null;

				var TransactionDetails = connection.SendTransaction(RequestData.ToString());

				NameValueCollection TransactionDetailParameters = new NameValueCollection();

				if (TransactionDetails != null && TransactionDetails.Length > 0)
				{
					String[] ResponseParameters = TransactionDetails.Split('&');

					foreach (String ParameterField in ResponseParameters)
					{
						String[] RawData = ParameterField.Split('=');
						TransactionDetailParameters.Add(RawData[0], HttpUtility.UrlDecode(RawData[1]));
					}
				}

				if (TransactionDetailParameters["session.updateStatus"] == "SUCCESS")
				{
					/*Update Invoice Status*/
					string InvoiceCode = TransactionDetailParameters["order.id"]; //maybe booking.id


					var invoice = _invoiceService.GetInvoiceByInvoiceNo(InvoiceCode);

					invoice.Status = InvoiceStatus.Paid.ToString();
					if (_invoiceService.UpdateInvoice(ref invoice, ref message))
					{
						booking.Status = BookingStatus.Authorized.ToString();
						booking.IsPayed = true;

						if (_bookingService.UpdateServiceBooking(ref booking, ref message))
						{
							/*Capture Transaction Details in Db*/

							Data.Transaction Transaction = new Data.Transaction();

							Transaction.InvoiceCode = invoice.InvoiceNo;
							Transaction.TransactionID = TransactionDetailParameters["transaction.id"];
							Transaction.NameOnCard = TransactionDetailParameters["sourceOfFunds.provided.card.nameOnCard"];
							Transaction.TransactionDate = Helpers.TimeZone.GetLocalDateTime();
							Transaction.MaskCardNo = TransactionDetailParameters["sourceOfFunds.provided.card.number"];
							Transaction.TransactionStatus = TransactionDetailParameters["session.updateStatus"];
							Transaction.Amount = Decimal.Parse(TransactionDetailParameters["order.amount"]); //maybe booking.amount
							Transaction.SessionID = TransactionDetailParameters["session.id"];

							Transaction.RawResponse = TransactionDetails;
							_transactionService.CreateTransaction(Transaction, ref message);

							ViewBag.SuccessMessage = "Your Booking is Confirmed! Your Booking No is : " + booking.BookingNo;

							return View(booking);
						}
						else
						{
							ViewBag.ErrorMessage = "Oops ! something went wrong please try later ...";
						}
					}
					else
					{
						ViewBag.ErrorMessage = "Oops ! something went wrong please try later ...";
					}
				}
				else
				{
					ViewBag.ErrorMessage = "Oops ! Transaction unsuccessfull, please try later ...";
				}
			}
			else
			{
				ViewBag.ErrorMessage = "Oops ! Session Failed, please try later ...";
			}

			return View(booking);
		}

		[AllowAnonymous]
		public ActionResult Initiate(long id, string culture = "en-ae")
		{
			string message = string.Empty;
			var booking = _bookingService.GetServiceBooking(id);
			if (booking == null)
			{
				return Content("Booking Not Found");
			}

			if (!booking.IsPayed.HasValue || booking.IsPayed == false)
			{
				if (booking.Status.Equals(currentBookingStatus))
				{
					var invoice = _invoiceService.GetInvoiceByBooking(id);
					if (invoice == null)
					{
						/*Create Booking Invoice*/
						invoice = new Data.Invoice()
						{
							RecordID = booking.ID,
							InvoiceNo = booking.BookingNo/*.Replace("BKG", "INV")*/,
							//Amount = booking.TotalAmount,
							Amount = booking.Total,
							Description = booking.BookingNo + " payment invoice",
							PaymentMethod = "Card",
							Status = InvoiceStatus.Sent.ToString(),
							IsDeleted = false,
							CreatedOn = Helpers.TimeZone.GetLocalDateTime(),
						};

						_invoiceService.CreateInvoice(invoice, ref message);
					}

					_transaction.OrderID = invoice.InvoiceNo;
					_transaction.ReturnURL = CustomURL.GetFormatedURL("/" + culture + "/Customer/Payment/Complete/" + booking.ID);

					string error = string.Empty;
					//if (_transaction.Process(ref error))
					if (_transaction.ProcessRestful(ref error, invoice.Amount))
					{
						string msg = string.Empty;
						booking.SessionID = _transaction.SessionID;
						_bookingService.UpdateServiceBooking(ref booking, ref msg);
						//var bookingDeliveryAddress = booking.BookingDeliveryAddresses.FirstOrDefault();

						OrderPaymentProcessingViewModel bookingPaymentProcessing = new OrderPaymentProcessingViewModel();

						bookingPaymentProcessing.Invoice = invoice;
						bookingPaymentProcessing.Booking = _bookingService.GetServiceBooking((long)invoice.RecordID);
						bookingPaymentProcessing.PaymentGatewaySetting = _paymentGatewaySettingService.GetDefaultPaymentGatewaySetting();
						bookingPaymentProcessing.Customer = booking.Customer;

						bookingPaymentProcessing.city = !string.IsNullOrEmpty(booking.CustomerCity) ? booking.CustomerCity : "x";

						if (Session["mastercard_session_id"] != null)
						{
							bookingPaymentProcessing.mastercard_session_id = Session["mastercard_session_id"].ToString();
							bookingPaymentProcessing.mastercard_session_version = Session["mastercard_session_version"].ToString();
							bookingPaymentProcessing.mastercard_success_indicator = Session["mastercard_success_indicator"].ToString();
							bookingPaymentProcessing.serial_expires = Session["serial_expires"].ToString();

							return View(bookingPaymentProcessing);
						}
						else if (!string.IsNullOrEmpty(booking.SessionID))
						{
							bookingPaymentProcessing.mastercard_session_id = booking.SessionID;
							bookingPaymentProcessing.mastercard_session_version = "";
							bookingPaymentProcessing.mastercard_success_indicator = "";
							bookingPaymentProcessing.serial_expires = "";

							return View(bookingPaymentProcessing);
						}
						else
						{
							TempData["ErrorMessage"] = "Booking Payment can't be processed.";
						}
					}
					else
					{
						Console.WriteLine(error);
						TempData["ErrorMessage"] = Resources.Resources.SomethingWentWrong;
					}
				}
				else
				{
					TempData["ErrorMessage"] = "Booking Payment can't be processed.";
				}
			}
			else
			{
				TempData["ErrorMessage"] = "Booking Payment already received.";
			}

			return RedirectToAction("Failure");
		}

		[AllowAnonymous]
		public ActionResult Complete(long id, string culture = "en-ae")
		{
			//check if payment is successfull:
			string resultIndicator = Convert.ToString(Request.QueryString["resultIndicator"]);
			string mastercard_success_indicator = Convert.ToString(Session["mastercard_success_indicator"]);
			string message = string.Empty;
			/*Update Booking Status*/
			var booking = _bookingService.GetServiceBooking(id);
			var invoice = _invoiceService.GetInvoiceByBooking(id);

			if (resultIndicator == mastercard_success_indicator || true)
			{
				//create token:

				_merchant.LoadSetting();

				// [Snippet] howToConfigureURL - start
				StringBuilder url = new StringBuilder();
				if (!_merchant.GatewayHost.StartsWith("http"))
					url.Append("https://");
				url.Append(_merchant.GatewayHost);
				url.Append("/api/nvp/version/");
				url.Append(_merchant.Version);
				_merchant.GatewayUrl = url.ToString();

				Connection connection = new Connection(_merchant);

				StringBuilder RequestData = new StringBuilder();
				RequestData.Append("merchant=" + _merchant.MerchantId);
				RequestData.Append("&apiUsername=" + _merchant.Username);
				RequestData.Append("&apiPassword=" + _merchant.Password);
				RequestData.Append("&apiOperation=RETRIEVE_SESSION");
				RequestData.Append("&session.id=" + booking.SessionID);

				//String result3 = null;
				//String response3 = null;
				//String gatewayCode3 = null;

				var TransactionDetails = connection.SendTransaction(RequestData.ToString());

				NameValueCollection TransactionDetailParameters = new NameValueCollection();

				if (TransactionDetails != null && TransactionDetails.Length > 0)
				{
					String[] ResponseParameters = TransactionDetails.Split('&');

					foreach (String ParameterField in ResponseParameters)
					{
						String[] RawData = ParameterField.Split('=');
						TransactionDetailParameters.Add(RawData[0], HttpUtility.UrlDecode(RawData[1]));
					}
				}

				if (TransactionDetailParameters["session.updateStatus"] == "SUCCESS")
				{
					/*Update Invoice Status*/
					string InvoiceCode = TransactionDetailParameters["order.id"]; //maybe booking.id

					invoice = _invoiceService.GetInvoiceByInvoiceNo(InvoiceCode);

					invoice.Status = InvoiceStatus.Paid.ToString();
					if (_invoiceService.UpdateInvoice(ref invoice, ref message))
					{
						booking.Status = BookingStatus.Authorized.ToString();
						booking.IsPayed = true;

						if (_bookingService.UpdateServiceBooking(ref booking, ref message))
						{
							/*Capture Transaction Details in Db*/

							Data.Transaction Transaction = new Data.Transaction();

							Transaction.InvoiceCode = invoice.InvoiceNo;
							Transaction.TransactionID = TransactionDetailParameters["transaction.id"];
							Transaction.NameOnCard = TransactionDetailParameters["sourceOfFunds.provided.card.nameOnCard"];
							Transaction.TransactionDate = Helpers.TimeZone.GetLocalDateTime();
							Transaction.MaskCardNo = TransactionDetailParameters["sourceOfFunds.provided.card.number"];
							Transaction.TransactionStatus = TransactionDetailParameters["session.updateStatus"];
							Transaction.Amount = Decimal.Parse(TransactionDetailParameters["order.amount"]); //maybe booking.amount
							Transaction.SessionID = TransactionDetailParameters["session.id"];

							Transaction.RawResponse = TransactionDetails;
							_transactionService.CreateTransaction(Transaction, ref message);

							TempData["SuccessMessage"] = "Transaction Successfull.";


							return RedirectPermanent("/" + culture + "/customer/payment/Success/" + booking.ID);
						}
						else
						{
							TempData["ErrorMessage"] = Resources.Resources.SomethingWentWrong;
						}
					}
					else
					{
						TempData["ErrorMessage"] = Resources.Resources.SomethingWentWrong;
					}
				}
				else
				{
					TempData["ErrorMessage"] = "Oops! Transaction unsuccessfull. Please try later.";
				}
			}
			else
			{
				TempData["ErrorMessage"] = "Oops! Session Expired/Invalid. Please try later.";
			}

			return RedirectPermanent("/" + culture + "/customer/payment/failure/" + booking.ID);
		}

		[AllowAnonymous]
		public ActionResult Success(long id, string culture = "en-ae")
		{
			string message = string.Empty;
			ViewBag.Message = TempData["SuccessMessage"];
			/*Update Booking Status*/
			var booking = _bookingService.GetServiceBooking(id);
			var invoice = _invoiceService.GetInvoiceByBooking(id);

			OrderPaymentProcessingViewModel bookingPaymentProcessing = new OrderPaymentProcessingViewModel();
			bookingPaymentProcessing.Invoice = invoice;
			bookingPaymentProcessing.Booking = booking;
			bookingPaymentProcessing.PaymentGatewaySetting = _paymentGatewaySettingService.GetDefaultPaymentGatewaySetting();
			bookingPaymentProcessing.Customer = booking.Customer;
			bookingPaymentProcessing.city = !string.IsNullOrEmpty(booking.CustomerCity) ? booking.CustomerCity : "x";

			return View(bookingPaymentProcessing);
		}

		[AllowAnonymous]
		public ActionResult Failure(long id, string culture = "en-ae")
		{
			string message = string.Empty;
			ViewBag.Message = TempData["ErrorMessage"];
			/*Update Booking Status*/
			var booking = _bookingService.GetServiceBooking(id);
			var invoice = _invoiceService.GetInvoiceByBooking(id);

			OrderPaymentProcessingViewModel bookingPaymentProcessing = new OrderPaymentProcessingViewModel();
			bookingPaymentProcessing.Invoice = invoice;
			bookingPaymentProcessing.Booking = booking;
			bookingPaymentProcessing.PaymentGatewaySetting = _paymentGatewaySettingService.GetDefaultPaymentGatewaySetting();
			bookingPaymentProcessing.Customer = booking.Customer;
			bookingPaymentProcessing.city = !string.IsNullOrEmpty(booking.CustomerCity) ? booking.CustomerCity : "x";

			return View(bookingPaymentProcessing);
		}

	}
}