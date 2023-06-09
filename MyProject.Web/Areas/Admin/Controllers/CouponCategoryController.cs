﻿using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using System.Linq;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class CouponCategoryController : Controller
    {
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly ICouponCategoryService _couponCategoryService;
		private readonly ICouponService _couponsService;
		private readonly ICategoryService _categoryService;
		private readonly IServicesService _serviceService;

		public CouponCategoryController(ICouponCategoryService couponCategoryService, ICouponService couponsService, ICategoryService categoryService, IServicesService serviceService)
		{
			this._couponCategoryService = couponCategoryService;
			this._couponsService = couponsService;
			this._categoryService = categoryService;
			this._serviceService = serviceService;
		}

		public ActionResult Index(long id)
		{

			ViewBag.CouponID = id;
			return View();
		}

		[HttpGet]
		public ActionResult GetAll(long id)
		{
			var coupons = _serviceService.GetServices().Select(i => new
			{
				id = i.ID.ToString(),
				value = i.Name
			}).ToList();

			var couponCategories = _couponCategoryService.GetCouponCategories(id).Select(i => new
			{
				id = i.ServiceID.ToString(),
				value = i.Service.Name,
				couponCategoryId = i.ID
			}).ToList();

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				coupons = coupons,
				couponCategories = couponCategories
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CouponCategory couponCategory)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (_couponCategoryService.CreateCouponCategory(ref couponCategory, ref message))
				{
					return Json(new
					{
						success = true,
						data = couponCategory.ID,
						message = "Coupon Category assigned ...",
					});
				}
			}
			else
			{
				message = "Please fill the form properly ...";
			}
			return Json(new { success = false, message = message });
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(CouponCategory couponCategory)
		{
			string message = string.Empty;
			if (_couponCategoryService.DeleteCouponCategory(couponCategory, ref message))
			{
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}
	}
}