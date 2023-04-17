using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using System.Linq;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class ProductAttributeSettingController : Controller
    {
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IProductAttributeSettingService _productAttributeSettingService;

		public ProductAttributeSettingController(IProductAttributeSettingService productAttributeSettingService)
		{
			this._productAttributeSettingService = productAttributeSettingService;
		}

		[HttpGet]
		public ActionResult GetProductAttributeSetting(long id)
		{
			var productAttributeSetting = _productAttributeSettingService.GetProductAttributesSetting(id).Select(i => new
			{
				id = i.ID.ToString(),
				attributeId = i.AttributeID,
				attributeName = i.Attribute.Name,
				i.ProductPageVisiblity,
				i.VariationUsage
			}).ToList();

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				productAttributeSetting = productAttributeSetting
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ProductAttributeSetting productAttributeSetting)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (_productAttributeSettingService.CreateProductAttributeSetting(ref productAttributeSetting, ref message))
				{
					return Json(new
					{
						success = true,
						data = productAttributeSetting.ID,
						message = "Product attribute assigned.",
					});
				}
			}
			else
			{
				message = "Please fill the form properly ...";
			}
			return Json(new { success = false, message = message });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Update(ProductAttributeSetting productAttributeSetting)
		{
			string message = string.Empty;
			if (_productAttributeSettingService.UpdateProductAttributeSetting(ref productAttributeSetting, ref message))
			{
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id)
		{
			string message = string.Empty;
			if (_productAttributeSettingService.DeleteProductAttributeSetting(id, ref message))
			{
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost, ActionName("DeleteValue")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteValue(ProductAttributeSetting productAttributeSetting)
		{
			string message = string.Empty;
			if (_productAttributeSettingService.DeleteProductAttributeSetting(productAttributeSetting, ref message))
			{
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}
	}
}