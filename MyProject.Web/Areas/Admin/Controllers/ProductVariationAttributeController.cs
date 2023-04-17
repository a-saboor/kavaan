using Project.Data;
using Project.Service;
using Project.Web.Areas.VendorPortal.ViewModels;
using Project.Web.AuthorizationProvider;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
	public class ProductVariationAttributeController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IProductVariationAttributeService _productVariationAttributeService;

		public ProductVariationAttributeController(IProductVariationAttributeService productVariationAttributeService)
		{
			this._productVariationAttributeService = productVariationAttributeService;
		}

		[HttpGet]
		public ActionResult GetVariationAttributes(long id)
		{
			var productVariationAttributes = _productVariationAttributeService.GetProductVariationAttributes(id).Select(i => new
			{
				id = i.ID.ToString(),
				productVariationID = i.ProductVariationID,
				productAttributeID = i.ProductAttributeID
			}).ToList();

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				variationAttributes = productVariationAttributes
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetProductVariationAttributes(long id)
		{
			var productVariationAttributes = _productVariationAttributeService.GetVariationAttributesByProduct(id).Select(i => new
			{
				id = i.ID.ToString(),
				productVariationID = i.ProductVariationID,
				productAttributeID = i.ProductAttributeID
			}).ToList();

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				productVariationAttributes = productVariationAttributes
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Update(ProductVariationAttributeViewModel productVariationAttributeViewModel)
		{

			string message = string.Empty;
			var productVariationAttributes = _productVariationAttributeService.GetVariationAttributesByProduct((long)productVariationAttributeViewModel.ProductId);

			foreach (var item in productVariationAttributes.Select(i => i.ProductVariationID).Distinct().ToArray())
			{
				IEnumerable<long> var_attr = productVariationAttributes.Where(i => i.ProductVariationID == item).Select(i => i.ProductAttributeID.Value).Distinct().ToList();

				bool isEqual = var_attr.SequenceEqual(productVariationAttributeViewModel.ProductAttributes);
				if (isEqual && item != productVariationAttributeViewModel.ProductVariationId)
				{
					return Json(new
					{
						success = false,
						message = "Variation with same attribute combination already exist.",
					});
				}
			}
			_productVariationAttributeService.DeleteProductVariationAttributes(productVariationAttributeViewModel.ProductVariationId, ref message);
			var attributes = new List<long>();
			foreach (var productAttribute in productVariationAttributeViewModel.ProductAttributes)
			{
				ProductVariationAttribute productVariationAttribute = new ProductVariationAttribute()
				{
					ProductID = productVariationAttributeViewModel.ProductId,
					ProductVariationID = productVariationAttributeViewModel.ProductVariationId,
					ProductAttributeID = productAttribute
				};
				if (_productVariationAttributeService.CreateProductVariationAttribute(ref productVariationAttribute, ref message)) ;
				{
					attributes.Add(productAttribute);
				}
			}
			return Json(new
			{
				success = true,
				data = productVariationAttributeViewModel.ProductVariationId,
				productVariation = new
				{
					id = productVariationAttributeViewModel.ProductVariationId,
					attributes = attributes
				},
				message = "Product variation attributes updated.",
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ProductVariationAttribute productVariationAttribute)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (_productVariationAttributeService.CreateProductVariationAttribute(ref productVariationAttribute, ref message))
				{
					return Json(new
					{
						success = true,
						data = new
						{
							productVariationAttribute.ID,
							productVariationAttribute.ProductVariationID,
							productVariationAttribute.ProductAttributeID
						},
						message = "Product variation attribute assigned.",
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
			if (_productVariationAttributeService.DeleteProductVariationAttribute(id, ref message))
			{
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost, ActionName("DeleteValue")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteValue(ProductVariationAttribute productVariationAttribute)
		{
			string message = string.Empty;
			if (_productVariationAttributeService.DeleteProductVariationAttribute(productVariationAttribute, ref message))
			{
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteAll(long id)
		{
			string message = string.Empty;
			if (_productVariationAttributeService.DeleteProductVariationAttributes(id, ref message))
			{
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}
	}
}