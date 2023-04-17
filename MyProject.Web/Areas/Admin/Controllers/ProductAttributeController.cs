using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class ProductAttributeController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IProductAttributeService _productAttributeService;
		private readonly IAttributeService _attributeService;

		public ProductAttributeController(IProductAttributeService productAttributeService, IAttributeService attributeService)
		{
			this._productAttributeService = productAttributeService;
			this._attributeService = attributeService;
		}

		[HttpGet]
		public ActionResult GetProductAttributes(long id)
		{
			var productAttributes = _productAttributeService.GetProductAttributes(id).Select(i => new
			{
				id = i.ID.ToString(),
				value = i.Value,
				attributeId = i.AttributeID
			}).ToList();

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				productAttributes = productAttributes
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ProductAttribute productAttribute)
		{
			try
			{
				string message = string.Empty;
				if (ModelState.IsValid)
				{
					bool isAlreadyExist = false;
					if (_productAttributeService.CreateProductAttribute(ref productAttribute, ref message, ref isAlreadyExist))
					{
						return Json(new
						{
							success = true,
							data = productAttribute.ID,
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
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = "Oops! Something went wrong. Please try later."
				});
			}
		}


		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id)
		{

			try
			{
				string message = string.Empty;
				if (_productAttributeService.DeleteProductAttribute(id, ref message))
				{
					return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
				}
				return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
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

		[HttpPost, ActionName("DeleteValue")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteValue(ProductAttribute productAttribute)
		{
			try
			{
				string message = string.Empty;
				long id = 0;
				if (_productAttributeService.DeleteProductAttribute(productAttribute, ref message, ref id))
				{
					return Json(new
					{
						success = true,
						data = id,
						message = message
					}, JsonRequestBehavior.AllowGet);
				}
				return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);

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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteAll(ProductAttribute productAttribute)
		{
			try
			{
				string message = string.Empty;
				if (_productAttributeService.DeleteProductAttributes(productAttribute, ref message))
				{
					return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
				}
				return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);

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