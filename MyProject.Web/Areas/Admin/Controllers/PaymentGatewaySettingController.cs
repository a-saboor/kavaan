using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider; 
using Project.Web.Helpers;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class PaymentGatewaySettingController : Controller
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IPaymentGatewaySettingService _paymentgatewaysettingService;

		public PaymentGatewaySettingController(IPaymentGatewaySettingService paymentgatewaysettingService)
		{
			this._paymentgatewaysettingService = paymentgatewaysettingService;
		}

		public ActionResult Index()
		{
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			PaymentGatewaySetting paymentGatewaySetting = _paymentgatewaysettingService.GetDefaultPaymentGatewaySetting();

			return View(paymentGatewaySetting);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Update(long? id, PaymentGatewaySetting paymentGatewaySetting)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (id.HasValue && id > 0)
				{
					if (_paymentgatewaysettingService.UpdatePaymentGatewaySetting(ref paymentGatewaySetting, ref message))
					{
						log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated payment gateway settings.");
						TempData["SuccessMessage"] = message;
						return RedirectToAction("Index");
					}
				}
				else
				{
					if (_paymentgatewaysettingService.CreatePaymentGatewaySetting(paymentGatewaySetting, ref message))
					{
						log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created payment gateway settings.");
						TempData["SuccessMessage"] = message;
						return RedirectToAction("Index");
					}
				}
			}
			else
			{
				message = "Please fill the form properly ...";
			}
			ViewBag.ErrorMessage = message;
			return View("Index", paymentGatewaySetting);
		}
	}
}