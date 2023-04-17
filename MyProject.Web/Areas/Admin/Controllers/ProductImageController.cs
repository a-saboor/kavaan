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
	public class ProductImageController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IProductImageService _productImageService;
		private readonly IProductService _productService;

		public ProductImageController(IProductImageService productImageService, IProductService productService)
		{
			this._productImageService = productImageService;
			this._productService = productService;
		}

		[HttpGet]
		public ActionResult GetProductImages(long id)
		{
			var productImages = _productImageService.GetProductImages(id).Select(i => new
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
				Product product = _productService.GetProduct((long)id);
				if (product == null)
				{
					return HttpNotFound();
				}

				string message = string.Empty;

				string absolutePath = Server.MapPath("~");
				string relativePath = string.Format("/Assets/AppFiles/Images/Product/{0}/Gallery/", product.SKU.Replace(" ", "_"));

				List<string> Pictures = new List<string>();

				Dictionary<long, string> data = new Dictionary<long, string>();
				Uploader.UploadImages(Request.Files, absolutePath, relativePath, "g", ref Pictures, ref message, "GalleryImages");
				if (Pictures.Count() == 0)
				{
					return Json(new
					{
						success = false,
						message = "Incorrect format..!",
					});
				}
				foreach (var item in Pictures)
				{
					ProductImage productImage = new ProductImage();
					productImage.ProductID = id;
					productImage.Image = item;
					productImage.Position = ++count;
					if (_productImageService.CreateProductImage(ref productImage, ref message))
					{
						data.Add(productImage.ID, item);
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
			if (_productImageService.DeleteProductImage(id, ref message, ref filePath))
			{
				if (System.IO.File.Exists(Server.MapPath(filePath)))
					System.IO.File.Delete(Server.MapPath(filePath));
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}
	}
}