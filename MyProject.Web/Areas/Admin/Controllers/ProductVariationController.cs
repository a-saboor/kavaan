using Project.Data;
using Project.Service;
using Project.Web.Areas.VendorPortal.ViewModels.Product;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class ProductVariationController : Controller
    {
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IProductVariationService _productVariationService;
		private readonly IProductVariationAttributeService _productVariationAttributeService;

		public ProductVariationController(IProductVariationService productVariationService, IProductVariationAttributeService productVariationAttributeService)
		{
			this._productVariationService = productVariationService;
			this._productVariationAttributeService = productVariationAttributeService;
		}

		[HttpGet]
		public ActionResult GetProductVariation(long id)
		{

			var productVariation = _productVariationService.GetProductVariations(id).Select(i => new
			{
				i.ID,
				i.ProductID,
				i.SKU,
				i.RegularPrice,
				i.SalePrice,
				SalePriceFrom= i.SalePriceFrom.HasValue?i.SalePriceFrom.Value.ToString("MM/dd/yyyy"):"",
				SalePriceTo= i.SalePriceTo.HasValue?i.SalePriceTo.Value.ToString("MM/dd/yyyy"):"",
				i.Stock,
				i.Threshold,
				i.StockStatus,
				i.Thumbnail,
				i.Weight,
				i.Length,
				i.Width,
				i.Height,
				i.Description,
				i.DescriptionAr,
				i.IsManageStock,
				i.SoldIndividually,
				i.IsActive
			}).ToList();

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				productVariation = productVariation
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ProductVariation productVariation, ProductVariationAttributesViewModel variationAttributes)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				var productVariationAttributes = _productVariationAttributeService.GetVariationAttributesByProduct((long)productVariation.ProductID);

				foreach (var item in productVariationAttributes.Select(i => i.ProductVariationID).Distinct().ToArray())
				{
					IEnumerable<long> var_attr = productVariationAttributes.Where(i => i.ProductVariationID == item).Select(i => i.ProductAttributeID.Value).Distinct().ToList();

					bool isEqual = var_attr.SequenceEqual(variationAttributes.ProductAttributes);
					if (isEqual)
					{
						return Json(new
						{
							success = false,
							message = "Variation with same attribute combination already exist.",
						});
					}
				}

				if (_productVariationService.CreateProductVariation(ref productVariation, ref message))
				{
					var attributes = new List<long>();
					foreach (var productAttribute in variationAttributes.ProductAttributes)
					{
						ProductVariationAttribute productVariationAttribute = new ProductVariationAttribute()
						{
							ProductID = productVariation.ProductID,
							ProductVariationID = productVariation.ID,
							ProductAttributeID = productAttribute
						};
						if (_productVariationAttributeService.CreateProductVariationAttribute(ref productVariationAttribute, ref message))
						{
							attributes.Add(productAttribute);
						}
					}
					return Json(new
					{
						success = true,
						data = productVariation.ID,
						productVariation = new
						{
							id = productVariation.ID,
							attributes = attributes
						},
						message = "Variation created successfully.",
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
		public ActionResult Update(ProductVariation productVariation)
		{
			string message = string.Empty;
			if (_productVariationService.UpdateProductVariation(ref productVariation, ref message))
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
			if (_productVariationService.DeleteProductVariation(id, ref message))
			{
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost, ActionName("DeleteValue")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteValue(ProductVariation productVariation)
		{
			string message = string.Empty;
			if (_productVariationService.DeleteProductVariation(productVariation, ref message))
			{
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Thumbnail(long? id)
		{
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				var productVariation = _productVariationService.GetProductVariation((long)id);
				if (productVariation == null)
				{
					return HttpNotFound();
				}
				string filePath = !string.IsNullOrEmpty(productVariation.Thumbnail) ? productVariation.Thumbnail : string.Empty;

				string message = string.Empty;

				string absolutePath = Server.MapPath("~");
				string relativePath = string.Format("/Assets/AppFiles/Images/Product/{0}/Variations/{1}/", productVariation.Product.SKU.Replace(" ", "_"), productVariation.SKU.Replace(" ", "_"));

				productVariation.Thumbnail = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Thumbnail", ref message, "Image");

				if (_productVariationService.UpdateProductVariation(ref productVariation, ref message, false))
				{
					if (!string.IsNullOrEmpty(filePath))
					{
						System.IO.File.Delete(Server.MapPath(filePath));
					}
					return Json(new
					{
						success = true,
						message = string.Format("{0} thumbnail image uploaded.", productVariation.SKU),
						data = productVariation.Thumbnail
					});
				}
				return Json(new { success = false, message = message });
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = "Oops! Something went wrong. Please try later."
				});
			}
		}
	}
}