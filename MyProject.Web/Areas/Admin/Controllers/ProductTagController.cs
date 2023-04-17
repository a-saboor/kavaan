using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class ProductTagController : Controller
    {
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IProductTagService _productTagService;
		private readonly ITagService _tagService;

		public ProductTagController(IProductTagService productTagService, ITagService tagService)
		{
			this._productTagService = productTagService;
			this._tagService = tagService;
		}

		[HttpGet]
		public ActionResult GetProductTags(long id)
		{
			var tags = _tagService.GetTags().Select(i => new
			{
				id = i.ID.ToString(),
				value = i.Name
			}).ToList();
			var productTags = _productTagService.GetProductTags(id).Select(i => new
			{
				id = i.TagID.ToString(),
				value = i.Tag.Name,
				producttagId = i.ID
			}).ToList();

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				tags = tags,
				productTags = productTags
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ProductTag productTag)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (_productTagService.CreateProductTag(ref productTag, ref message))
				{
					return Json(new
					{
						success = true,
						data = productTag.ID,
						message = "Product tag assigned.",
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
		public ActionResult DeleteConfirmed(ProductTag productTag)
		{
			string message = string.Empty;
			if (_productTagService.DeleteProductTag(productTag, ref message))
			{
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}
	}
}