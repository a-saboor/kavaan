using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyProject.Web.Areas.CustomerPortal.Controllers
{
	[AuthorizeCustomer]

	public class SuggestionController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly ICustomerSuggestionService _customerSuggestionService;
		private readonly ICustomerService _customerService;

		public SuggestionController(ICustomerSuggestionService customerSuggestionService, ICustomerService customerService)
		{
			this._customerSuggestionService = customerSuggestionService;
			this._customerService = customerService;
		}

		public ActionResult Index(string culture = "en-ae")
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AuthorizeCustomer]
		public ActionResult Index(CustomerSuggestion customerSuggestion)
		{
			string Message = string.Empty;
			string description = string.Empty;
			bool status = false;
			try
			{
				if (ModelState.IsValid)
				{
					customerSuggestion.CustomerID = Helpers.CustomerSessionHelper.ID;

					if (_customerSuggestionService.CreateCustomerSuggestion(customerSuggestion, ref Message))
					{
						Message = "Thank you! Your suggestion has been successfully sent ...";
						status = true;
					}
				}
				else
				{
					Message = "Please fill the form properly!";
					description = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
				}
			}
			catch (Exception ex)
			{
				Message = "Something went wrong! Please try later.";
			}

			return Json(new { success = status, message = Message, description }, JsonRequestBehavior.AllowGet);
		}

	}
}