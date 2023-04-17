using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class ProductCategoryController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IProductCategoryService _productCategoryService;
		private readonly ICategoryService _categoryService;

		public ProductCategoryController(IProductCategoryService productCategoryService, ICategoryService categoryService)
		{
			this._productCategoryService = productCategoryService;
			this._categoryService = categoryService;
		}

		[HttpGet]
		public ActionResult GetProductCategories(long id)
		{
			var category = _categoryService.GetCategories(string.Empty, "en").Select(i => new { id = i.ID, name = i.Name, ParentId = i.ParentID, hasChilds = i.hasChilds });
			var productCategory = _productCategoryService.GetProductCategories(id).Select(i => new { id = i.ID, categoryId = i.ProductCategoryID }).ToList();

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				categories = category,
				productCategories = productCategory
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ProductCategory productCategory)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (_productCategoryService.CreateProductCategory(ref productCategory, ref message))
				{
					return Json(new
					{
						success = true,
						data = productCategory.ID,
						message = "Product category assigned.",
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
		public ActionResult DeleteConfirmed(long id)
		{
			string message = string.Empty;
			if (_productCategoryService.DeleteProductCategory((Int16)id, ref message))
			{
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}
	}
}