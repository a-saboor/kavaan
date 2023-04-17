using Project.Data;
using Project.Service;
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
	public class ProductVariationImagesController : Controller
    {
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IProductVariationImageService _productVariationImageService;
		private readonly IProductVariationService _productVariationService;

		public ProductVariationImagesController(IProductVariationImageService productVariationImageService, IProductVariationService productVariationService)
		{
			this._productVariationImageService = productVariationImageService;
			this._productVariationService = productVariationService;
		}

		[HttpGet]
		public ActionResult GetProductVariationImages(long id)
		{
			var productImages = _productVariationImageService.GetProductVariationImages(id).Select(i => new
			{
				id = i.ID,
				i.Title,
				Image = i.Image,
				position = i.Position,
			}).ToList();

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				productImages = productImages
			}, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Create(long? id, int count)
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

				string message = string.Empty;

				string absolutePath = Server.MapPath("~");
				string relativePath = string.Format("/Assets/AppFiles/Images/Product/{0}/Variations/{1}/Gallery/", productVariation.Product.SKU.Replace(" ", "_"), productVariation.SKU.Replace(" ", "_"));

				List<string> Pictures = new List<string>();

				Dictionary<long, string> data = new Dictionary<long, string>();
				Uploader.UploadImages(Request.Files, absolutePath, relativePath, "PVGI", ref Pictures, ref message, "ProductVariationGalleryImages");
				foreach (var item in Pictures)
				{
					ProductVariationImage productVariationImage = new ProductVariationImage();
					productVariationImage.ProductID = productVariation.ProductID;
					productVariationImage.ProductVariationID = productVariation.ID;
					productVariationImage.Image = item;
					productVariationImage.Position = ++count;

					if (_productVariationImageService.CreateProductVariationImage(ref productVariationImage, ref message))
					{
						data.Add(productVariationImage.ID, item);
					}
				}

				return Json(new
				{
					success = true,
					message = message,
					data = data.ToList()
				});
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
			string message = string.Empty;
			string filePath = string.Empty;
			if (_productVariationImageService.DeleteProductVariationImage(id, ref message, ref filePath))
			{
				System.IO.File.Delete(Server.MapPath(filePath));
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}
	}
}