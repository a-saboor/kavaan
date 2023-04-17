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

	public class CouponController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly ICustomerService _customerService;
		private readonly ICustomerCouponsService _customerCouponsService;

		public CouponController(ICustomerService customerService, ICustomerCouponsService customerCouponsService)
		{
			this._customerService = customerService;
			this._customerCouponsService = customerCouponsService;
		}

		public ActionResult Index(string culture = "en-ae")
		{
			return View();
		}

		[HttpGet]
		public ActionResult LoadCoupons(int pageNo, string culture = "en-ae")
		{
			long CustomerID = Helpers.CustomerSessionHelper.ID;

			var coupons = _customerCouponsService.GetSPCustomerCoupons(CustomerID, pageNo);
			var totalcoupons = _customerCouponsService.GetCustomerCouponsCount(CustomerID);

			return Json(new
			{
				success = true,
				message = "Data retrieved successfully !",
				data = coupons,
				TotalRecord = totalcoupons,
			}, JsonRequestBehavior.AllowGet);
		}
	}
}