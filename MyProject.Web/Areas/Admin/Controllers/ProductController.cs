using LinqToExcel;
using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using Project.Web.Helpers.POCO;
using Project.Web.Helpers.Routing;
using Project.Web.Util;
using Project.Web.ViewModels.DataTables;
using Project.Web.ViewModels.Product;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static Project.Web.Helpers.Enumerations.Enumeration;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class ProductController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IProductService _productService;
		private readonly IVendorService _vendorService;
		private readonly IAttributeService _attributeService;
		private readonly INotificationService _notificationService;
		private readonly INotificationReceiverService _notificationReceiverService;
		private readonly IBrandsService _brandService;
		private readonly IProductVariationService _productVariationService;
		private readonly IProductVariationImageService _productVariationImageService;
		private readonly IProductCategoryService _productCategoryService;
		private readonly IProductTagService _productTagService;
		private readonly IProductAttributeService _productAttributeService;
		private readonly IProductVariationAttributeService _productVariationAttributeService;
		private readonly IProductImageService _productImageService;

		public ProductController(IProductVariationAttributeService productVariationAttributeService,
			IProductAttributeService productAttributeService
			, IProductTagService productTagService, IProductCategoryService productCategoryService, IProductVariationService productVariationService, IProductVariationImageService productVariationImageService, IProductService productService, IVendorService vendorService, IAttributeService attributeService, INotificationService notificationService, INotificationReceiverService notificationReceiverService, IBrandsService brandService, IProductImageService productImageService)
		{
			this._productVariationAttributeService = productVariationAttributeService;
			this._productAttributeService = productAttributeService;
			this._productTagService = productTagService;
			this._productCategoryService = productCategoryService;
			this._productVariationService = productVariationService;
			this._productVariationImageService = productVariationImageService;
			this._productService = productService;
			this._vendorService = vendorService;
			this._attributeService = attributeService;
			this._notificationService = notificationService;
			this._notificationReceiverService = notificationReceiverService;
			this._brandService = brandService;
			this._productImageService = productImageService;

		}

		#region Product

		public ActionResult Index()
		{
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			return View();
		}

		public ActionResult List()
		{
			var obj = new SelectList(_vendorService.GetVendorsForDropDown(), "value", "text");
			ViewBag.VendorID = obj;
			return PartialView();
		}

		[HttpPost]
		public JsonResult List(DataTableAjaxPostModel model)
		{
			var searchBy = (model.search != null) ? model.search.value : "";
			int sortBy = 0;
			string sortDir = "";
			if (model.order != null)
			{
				sortBy = model.order[0].column;
				sortDir = model.order[0].dir.ToLower();
			}
			var Products = _productService.GetVendorProducts(model.length, model.start, sortBy, sortDir, searchBy, model.VendorID);

			int filteredResultsCount = Products != null && Products.Count() > 0 ? (int)Products.FirstOrDefault().FilteredResultsCount : 0;
			int totalResultsCount = Products != null && Products.Count() > 0 ? (int)Products.FirstOrDefault().TotalResultsCount : 0;

			var obj = new SelectList(_vendorService.GetVendorsForDropDown(), "value", "text");
			ViewBag.Vendor = obj;
			//ViewBag.VendorID = _vendorService.GetVendorsForDropDown().ToList();

			return Json(new
			{
				draw = model.draw,
				recordsTotal = totalResultsCount,
				recordsFiltered = filteredResultsCount,
				data = Products
			});
		}

		[HttpPost]
		public JsonResult ListVendor(DataTableAjaxPostModel model, long? VendorID)
		{
			var searchBy = (model.search != null) ? model.search.value : "";
			int sortBy = 0;
			string sortDir = "";
			if (model.order != null)
			{
				sortBy = model.order[0].column;
				sortDir = model.order[0].dir.ToLower();
			}
			var Products = _productService.GetVendorProducts(model.length, model.start, sortBy, sortDir, searchBy, VendorID);

			int filteredResultsCount = Products != null && Products.Count() > 0 ? (int)Products.FirstOrDefault().FilteredResultsCount : 0;
			int totalResultsCount = Products != null && Products.Count() > 0 ? (int)Products.FirstOrDefault().TotalResultsCount : 0;

			var obj = new SelectList(_vendorService.GetVendorsForDropDown(), "value", "text");
			ViewBag.Vendor = obj;
			//ViewBag.VendorID = _vendorService.GetVendorsForDropDown().ToList();

			return Json(new
			{
				draw = model.draw,
				recordsTotal = totalResultsCount,
				recordsFiltered = filteredResultsCount,
				data = Products
			});
		}

		public ActionResult Details(long? id)
		{
			var request = Request.UrlReferrer;

			if (request.AbsolutePath == "/Admin/Product/Index")
			{
				ViewBag.BreadCrumb = "Products";
			}

			else
			{
				ViewBag.BreadCrumb = "Product Approvals";
			}

			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Data.Product product = _productService.GetProduct((Int16)id);
			ViewBag.AttributeID = new SelectList(_attributeService.GetAttributesForDropDown(), "value", "text");
			ViewBag.BrandID = new SelectList(_brandService.GetBrandsForDropDown(), "value", "text", product.BrandID);
			if (product == null)
			{
				return HttpNotFound();
			}
			return View(product);
		}
		public ActionResult ApproveDetails(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Data.Product product = _productService.GetProduct((Int16)id);
			ViewBag.AttributeID = new SelectList(_attributeService.GetAttributesForDropDown(), "value", "text");
			ViewBag.BrandID = new SelectList(_brandService.GetBrandsForDropDown(), "value", "text", product.BrandID);
			if (product == null)
			{
				return HttpNotFound();
			}
			return View(product);
		}
		public ActionResult Create(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Data.Product product = _productService.GetProduct((long)id);
			if (product == null)
			{
				return HttpNotFound();
			}

			ViewBag.AttributeID = new SelectList(_attributeService.GetAttributesForDropDown(), "value", "text");
			ViewBag.BrandID = new SelectList(_brandService.GetBrandsForDropDown(), "value", "text");
			//TempData["ProductID"] = id;
			return View(product);
		}

		public ActionResult QuickCreate()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult QuickCreate(Data.Product product)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				product.VendorID = Convert.ToInt64(Session["VendorID"]);
				product.Status = "1";
				product.ApprovalStatus = 1;
				product.Type = "1";
				product.Slug = Slugify.GenerateSlug(product.Name + "-" + product.SKU);

				if (_productService.CreateProduct(product, ref message))
				{
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created product {product.SKU}.");
					return Json(new
					{
						success = true,
						url = "/Admin/Product/Edit/" + product.ID,
						message = message
					});
				}
			}
			else
			{
				message = "Please fill the form properly ...";
			}

			return Json(new { success = false, message = message });
		}

		public ActionResult Edit(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Data.Product product = _productService.GetProduct((long)id);
			if (product == null)
			{
				return HttpNotFound();
			}

			ViewBag.AttributeID = new SelectList(_attributeService.GetAttributesForDropDown(), "value", "text");
			ViewBag.BrandID = new SelectList(_brandService.GetBrandsForDropDown(), "value", "text", product.BrandID);
			TempData["ProductID"] = id;
			return View(product);
		}

		[HttpPost]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult Update(Data.Product product)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				product.Slug = Slugify.GenerateSlug(product.Name + "-" + product.SKU);
				if (_productService.UpdateProduct(ref product, ref message))
				{
					_productService.UpdateProductApprovalStatus(product.ID);

                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated product {product.SKU}.");
					return Json(new
					{
						success = true,
						//url = "/Admin/Product/Index",
						message = "Product updated successfully ...",
						data = new
						{
							ID = product.ID,
							Date = product.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
							SKU = product.SKU,
							Name = product.Name,
							LongDescription = product.LongDescription,
							Remark = product.Remarks,
							IsActive = product.IsActive.HasValue ? product.IsActive.Value.ToString() : bool.FalseString
						}
					});
				}
			}
			else
			{
				message = "Please fill the form properly ...";
			}
			return Json(new { success = false, message = message });
		}

		public ActionResult Thumbnail(long? id)
		{
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Data.Product product = _productService.GetProduct((long)id);
				if (product == null)
				{
					return HttpNotFound();
				}
				string filePath = !string.IsNullOrEmpty(product.Thumbnail) ? product.Thumbnail : string.Empty;


				string message = string.Empty;

				string absolutePath = Server.MapPath("~");
				string relativePath = string.Format("/Assets/AppFiles/Images/Product/{0}/", product.SKU.Replace(" ", "_"));

				product.Thumbnail = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Thumbnail", ref message, "Image");

				if (_productService.UpdateProduct(ref product, ref message, false))
				{
					_productService.UpdateProductApprovalStatus(product.ID);

					if (!string.IsNullOrEmpty(filePath))
					{
						System.IO.File.Delete(Server.MapPath(filePath));
					}
					return Json(new
					{
						success = true,
						message = message,
						data = product.Thumbnail
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

		public ActionResult Delete(long? id)
		{
			

			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Data.Product product = _productService.GetProduct((Int16)id);
			if (product == null)
			{
				return HttpNotFound();
			}
			TempData["ProductID"] = id;
			return View(product);
		}

		public ActionResult Publish(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var product = _productService.GetProduct((long)id);
			if (product == null)
			{
				return HttpNotFound();
			}
			if (product.IsPublished.HasValue)
			{
				if (product.IsPublished.Value)
					product.IsPublished = false;
				else
				{
					product.IsPublished = true;
				}
			}
			else
			{
				product.IsPublished = false;
			}

			string message = string.Empty;
			if (_productService.UpdateProduct(ref product, ref message))
			{
				SuccessMessage = "Product " + (product.IsPublished.Value ? "Published" : "Unpublished") + "  successfully ...";
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {(product.IsPublished.Value ? "Published" : "Unpublished")} product {product.SKU}.");
				return Json(new
				{
					success = true,
					message = SuccessMessage,
					data = new
					{
						IsPublished = product.IsPublished.HasValue ? product.IsPublished.Value.ToString() : bool.FalseString,
						ID = product.ID
					}
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				ErrorMessage = "Oops! Something went wrong. Please try later.";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id)
		{
			Data.Product product = _productService.GetProduct((Int16)id);
			string message = string.Empty;
			if (_productService.DeleteProduct((Int16)id, ref message))
			{
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted product {product.SKU}.");
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult BulkUpload()
		{

			return View();

		}

		//[HttpPost]
		//public ActionResult BulkUpload(HttpPostedFileBase FileUpload, string connectionId)
		//{
		//	try
		//	{

		//		if (FileUpload != null)
		//		{
		//			if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
		//			{
		//				string filename = FileUpload.FileName;

		//				if (filename.EndsWith(".xlsx"))
		//				{
		//					string targetpath = Server.MapPath("~/assets/AppFiles/Documents/ExcelFiles");
		//					FileUpload.SaveAs(targetpath + filename);
		//					string pathToExcelFile = targetpath + filename;

		//					string sheetName = "BulkProduct";
		//					long VendorID = (long)Session["VendorID"];


		//					string data = "";
		//					string message = "";
		//					List<string> ErrorItems = new List<string>();
		//					int count = 0;
		//					var total = 0;
		//					int successCount = 0;



		//					var excelFile = new ExcelQueryFactory(pathToExcelFile);
		//					IList<ProductWorkSheet> Product = (from a in excelFile.Worksheet<ProductWorkSheet>(sheetName) select a).ToList();
		//					total = Product.Count();
		//					//if (Product.Count() != Product.Select(i => i.Slug).Distinct().Count())
		//					//{
		//					//	return Json(new
		//					//	{
		//					//		success = false,
		//					//		message = "All Products Must Contain"
		//					//	}, JsonRequestBehavior.AllowGet);
		//					//}

		//					foreach (ProductWorkSheet item in Product)
		//					{
		//						try
		//						{
		//							var results = new List<ValidationResult>();
		//							var context = new ValidationContext(item, null, null);
		//							if (Validator.TryValidateObject(item, context, results))
		//							{
		//								if (string.IsNullOrEmpty(item.VariantSKU))
		//								{
		//									/* IF PRODUCT IS SIMPLE*/

		//									/*Upload Product Thumnail*/
		//									string filePath = string.Empty;
		//									if (!string.IsNullOrEmpty(item.Thumbnail))
		//									{
		//										filePath = item.Thumbnail;

		//										message = string.Empty;

		//										string absolutePath = Server.MapPath("~");
		//										string relativePath = string.Format("/Assets/AppFiles/Images/Product/{0}/", item.SKU.Replace(" ", "_"));

		//										item.Thumbnail = Uploader.SaveImage(item.Thumbnail, absolutePath, relativePath, "PTI", ImageFormat.Jpeg);

		//									}
		//									if (!string.IsNullOrEmpty(item.ProductTags))
		//									{
		//										item.ProductTags = item.ProductTags.Replace("&", "&amp;");
		//									}

		//									if (!string.IsNullOrEmpty(item.ProductCategories))
		//									{
		//										item.ProductCategories = item.ProductCategories.Replace("&", "&amp;");
		//									}

		//									if (_productService.PostExcelData(VendorID
		//																	, item.Brand
		//																	, item.SKU
		//																	, string.IsNullOrEmpty(item.Slug) ? Slugify.GenerateSlug(item.Name + "-" + item.SKU) : item.Slug
		//																	, item.Name
		//																	, item.Thumbnail
		//																	, item.ShortDescription
		//																	, item.LongDescription
		//																	, item.MobileDescription
		//																	, item.RegularPrice
		//																	, item.SalePrice
		//																	, item.SalePriceFrom
		//																	, item.SalePriceTo
		//																	, item.Stock
		//																	, item.Threshold
		//																	, item.Type == "Simple" ? "1" : "2"
		//																	, item.Weight
		//																	, item.Length
		//																	, item.Width
		//																	, item.Height
		//																	, item.IsSoldIndividually == "Yes" ? true : false
		//																	, item.AllowMultipleOrder == "Yes" ? true : false
		//																	, item.StockStatus == "In Stock" ? 1 : 2
		//																	, item.IsFeatured == "Yes" ? true : false
		//																	, item.IsRecommended == "Yes" ? true : false
		//																	, item.IsManageStock == "Yes" ? true : false
		//																	, item.ProductCategories
		//																	, item.ProductTags))
		//									{
		//										Data.Product product = _productService.GetProductbySKU(item.SKU, VendorID);
		//										if (item.Attributes != null)
		//										{

		//											string[] values = item.Attributes.Split(',');
		//											bool VariationUsage;
		//											if (item.Type == "Simple")
		//											{
		//												VariationUsage = false;
		//											}
		//											else
		//											{
		//												VariationUsage = true;
		//											}
		//											for (int i = 0; i < values.Count(); i++)
		//											{
		//												string[] val1 = values[i].Split(':');
		//												if (val1 != null)
		//												{
		//													var result = _productAttributeService.PostExcelData(product.ID, val1[0], val1[1], VariationUsage);
		//												}
		//											}

		//										}

		//										/*Uploading Product Images*/
		//										if (!string.IsNullOrEmpty(item.Images))
		//										{
		//											message = string.Empty;
		//											string absolutePath = Server.MapPath("~");
		//											string relativePath = string.Format("/Assets/AppFiles/Images/Product/{0}/Gallery/", product.SKU.Replace(" ", "_"));

		//											List<string> Pictures = new List<string>();

		//											Uploader.SaveImages(item.Images.Split(','), absolutePath, relativePath, "PGI", ImageFormat.Jpeg, ref Pictures);
		//											var imageCount = 0;
		//											foreach (var image in Pictures)
		//											{
		//												ProductImage productImage = new ProductImage();
		//												productImage.ProductID = product.ID;
		//												productImage.Image = image;
		//												productImage.Position = ++imageCount;
		//												if (!_productImageService.CreateProductImage(ref productImage, ref message))
		//												{
		//												}
		//											}
		//										}

		//										product = null;
		//										successCount++;
		//									}
		//									else
		//									{
		//										ErrorItems.Add(string.Format("Row Number {0} Not Inserted.<br>", count));

		//										if (!string.IsNullOrEmpty(item.Thumbnail))
		//										{
		//											System.IO.File.Delete(Server.MapPath(item.Thumbnail));
		//										}
		//									}
		//								}
		//								else
		//								{
		//									/* IF Product is VARIATION*/
		//									Data.Product product = _productService.GetProductbySKU(item.SKU, VendorID);
		//									if (product != null)
		//									{
		//										if (item.Type == "Variable" && item.VariantSKU != null)
		//										{
		//											/*Upload Product Variation Thumnail*/
		//											string filePath = string.Empty;
		//											if (!string.IsNullOrEmpty(item.VariantThumbnail))
		//											{
		//												filePath = item.VariantThumbnail;

		//												message = string.Empty;

		//												string absolutePath = Server.MapPath("~");
		//												string relativePath = string.Format("/Assets/AppFiles/Images/Product/{0}/Variations/{1}/", product.SKU.Replace(" ", "_"), item.VariantSKU.Replace(" ", "_"));

		//												item.VariantThumbnail = Uploader.SaveImage(item.VariantThumbnail, absolutePath, relativePath, "PVTI", ImageFormat.Jpeg);

		//											}

		//											_productVariationService.PostExcelData(product.ID, item.VariantRegularPrice, item.VariantSalePrice, item.VariantSalePriceFrom, item.VariantSalePriceTo, item.VariantStock, item.VariantThreshold, item.VariantStockStatus == "In Stock" ? 1 : 2, item.VariantSKU, item.VariantThumbnail, item.VariantWeight, item.VariantLength, item.VariantWidth, item.VariantHeight, item.VariantDescription, item.VariantSoldIndividually == "Yes" ? true : false, item.VariantIsManageStock == "Yes" ? true : false);

		//										}

		//										ProductVariation objProductVariation = _productVariationService.GetProductbySKU(item.VariantSKU);

		//										/*Creating Product Variation Attributes Images*/
		//										if (item.VariantAttributes != null)
		//										{
		//											string[] values = item.VariantAttributes.Split(',');
		//											for (int i = 0; i < values.Count(); i++)
		//											{
		//												string[] val1 = values[i].Split(':');
		//												if (val1 != null && val1.Count() > 1)
		//												{
		//													var result = _productVariationAttributeService.PostExcelData(product.ID, val1[0], val1[1], objProductVariation.ID);
		//												}
		//											}
		//										}

		//										/*Uploading Product Variation Images*/
		//										if (!string.IsNullOrEmpty(item.VariantImages))
		//										{
		//											message = string.Empty;
		//											string absolutePath = Server.MapPath("~");

		//											string relativePath = string.Format("/Assets/AppFiles/Images/Product/{0}/Variations/{1}/Gallery/", product.SKU.Replace(" ", "_"), item.VariantSKU.Replace(" ", "_"));

		//											List<string> Pictures = new List<string>();

		//											Uploader.SaveImages(item.VariantImages.Split(','), absolutePath, relativePath, "PVGI", ImageFormat.Jpeg, ref Pictures);
		//											var imageCount = 0;
		//											foreach (var image in Pictures)
		//											{
		//												ProductVariationImage productVariationImage = new ProductVariationImage();
		//												productVariationImage.ProductID = product.ID;
		//												productVariationImage.ProductVariationID = objProductVariation.ID;
		//												productVariationImage.Image = image;
		//												productVariationImage.Position = ++imageCount;
		//												if (_productVariationImageService.CreateProductVariationImage(ref productVariationImage, ref message))
		//												{
		//												}
		//											}
		//										}

		//										objProductVariation = null;
		//										successCount++;
		//									}
		//									else
		//									{
		//										ErrorItems.Add(string.Format("<b>Row Number {0} Not Inserted:</b><br>Product not found, Please add product row first without variant", count));
		//									}
		//								}
		//							}
		//							else
		//							{
		//								ErrorItems.Add(string.Format("<b>Row Number {0} Not Inserted:</b><br>{1}",
		//									count,
		//									string.Join<string>("<br>", results.Select(i => i.ErrorMessage).ToList())));
		//							}
		//							count++;
		//						}
		//						catch (Exception ex)
		//						{
		//							ErrorItems.Add(string.Format("<b>Row Number {0} Not Inserted:</b><br>Internal Server Serror", count));
		//						}

		//						//CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
		//						Functions.SendProgress(connectionId, "Upload in progress...", count, total);
		//					}

		//					return Json(new
		//					{
		//						success = true,
		//						successMessage = string.Format("{0} Products uploaded!", (successCount)),
		//						errorMessage = (ErrorItems.Count() > 0) ? string.Format("{0} Products are not uploaded!", total - successCount) : null,
		//						detailedErrorMessages = (ErrorItems.Count() > 0) ? string.Join<string>("<br>", ErrorItems) : null,
		//					}, JsonRequestBehavior.AllowGet);
		//				}
		//				else
		//				{
		//					return Json(new
		//					{
		//						success = false,
		//						message = "Invalid file format, Only .xlsx format is allowed"
		//					}, JsonRequestBehavior.AllowGet);
		//				}
		//			}
		//			else
		//			{
		//				return Json(new
		//				{
		//					success = false,
		//					message = "Invalid file format, Only Excel file is allowed"
		//				}, JsonRequestBehavior.AllowGet);
		//			}
		//		}
		//		else
		//		{

		//			return Json(new
		//			{
		//				success = false,
		//				message = "Please upload Excel file first"
		//			}, JsonRequestBehavior.AllowGet);
		//		}

		//	}
		//	catch (Exception ex)
		//	{
		//		return Json(new
		//		{
		//			success = false,
		//			message = "Oops! Something went wrong. Please try later."
		//		}, JsonRequestBehavior.AllowGet);
		//	}
		//}

		#endregion

		#region Approval

		public ActionResult Approvals()
		{
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			return View();
		}

		public ActionResult ApprovalsList()
		{
			var products = _productService.GetProducts();
			return PartialView(products);
		}

		[HttpGet]
		public ActionResult Approve(long id, bool approvalStatus)
		{
			ViewBag.BuildingID = id;
			ViewBag.ApprovalStatus = approvalStatus;

			var product = _productService.GetProduct((long)id);
			product.IsApproved = approvalStatus;

			return View(product);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Approve(ProductApprovalFormViewModel productApprovalFormViewModel)
		{

			var product = _productService.GetProduct(productApprovalFormViewModel.ID);
			if (product == null)
			{
				return HttpNotFound();
			}

			if (productApprovalFormViewModel.IsApproved)
			{
				product.IsApproved = true;
				product.IsActive = true;
				product.ApprovalStatus = (int)ProductApprovalStatus.Approved;
			}
			else
			{
				product.IsApproved = false;
				product.IsActive = false;
				product.ApprovalStatus = (int)ProductApprovalStatus.Rejected;
			}

			product.Remarks = product.Remarks + "<hr />" + Helpers.TimeZone.GetLocalDateTime().ToString("dd MMM yyyy, h:mm tt") + "<br />" + productApprovalFormViewModel.Remarks;

			string message = string.Empty;

			if (_productService.UpdateProduct(ref product, ref message, false))
			{
				SuccessMessage = "Product " + ((bool)product.IsActive ? "approved" : "rejected") + "  successfully ...";
				var vendor = _vendorService.GetVendor((long)product.VendorID);

				Notification not = new Notification();
				not.Title = "Product Approval";
				not.TitleAr = "الموافقة على المنتج";
				if (product.ApprovalStatus == 3)
				{
					not.Description = "Your product " + product.SKU + " have been approved ";
					not.Url = "/Vendor/Product/Index";
				}
				else
				{
					not.Description = "Your " + product.SKU + " have been rejected ";
					not.Url = "/Vendor/Product/ApprovalIndex";
				}
				not.OriginatorID = Convert.ToInt64(Session["AdminUserID"]);
				not.OriginatorName = Session["UserName"].ToString();
				not.Module = "Product";
				not.OriginatorType = "Admin";
				not.RecordID = product.ID;
				if (_notificationService.CreateNotification(not, ref message))
				{
					NotificationReceiver notRec = new NotificationReceiver();
					notRec.ReceiverID = product.VendorID;
					notRec.ReceiverType = "Vendor";
					notRec.NotificationID = not.ID;
					if (_notificationReceiverService.CreateNotificationReceiver(notRec, ref message))
					{

					}

				}


                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {(product.IsApproved.Value ? "Approved" : "Rejected")} product {product.SKU}.");
				return Json(new
				{

					success = true,
					message = SuccessMessage,
					data = new
					{
						ID = product.ID,
						Date = product.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
						Vendor = vendor.Logo + "|" + vendor.VendorCode + "|" + vendor.Name,
						Product = product.Name + "|" + product.SKU,
						ApprovalStatus = product.ApprovalStatus,
						IsActive = product.IsActive.HasValue ? product.IsActive.Value.ToString() : bool.FalseString,
						IsApproved = product.IsApproved.HasValue ? product.IsApproved.Value.ToString() : bool.FalseString
					}
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				ErrorMessage = message;
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]		
		public ActionResult ApproveAll(List<long> ids)
		{
			foreach (var items in ids)
			{
				var product = _productService.GetProduct(items);

				product.IsApproved = true;
				product.IsActive = true;
				product.ApprovalStatus = (int)ProductApprovalStatus.Approved;


				//product.Remarks = product.Remarks + "<hr />" + Helpers.TimeZone.GetLocalDateTime().ToString("dd MMM yyyy, h:mm tt") + "<br />" + productApprovalFormViewModel.Remarks;

				string message = string.Empty;

				if (_productService.UpdateProduct(ref product, ref message, false))
				{
					SuccessMessage = "Product " + ((bool)product.IsActive ? "Approved" : "Rejected") + "  successfully ...";
					var vendor = _vendorService.GetVendor((long)product.VendorID);
					Notification not = new Notification();
					not.Title = "Product Approval";
					not.TitleAr = "الموافقة على المنتج";
					if (product.ApprovalStatus == 3)
					{
						not.Description = "Your product " + product.SKU + " have been approved ";
						not.Url = "/Vendor/Product/Index";
					}
					else
					{
						not.Description = "Your " + product.SKU + " have been rejected ";
						not.Url = "/Vendor/Product/ApprovalIndex";
					}
					not.OriginatorID = Convert.ToInt64(Session["AdminUserID"]);
					not.OriginatorName = Session["UserName"].ToString();
					not.Module = "Product";
					not.OriginatorType = "Admin";
					not.RecordID = product.ID;
					if (_notificationService.CreateNotification(not, ref message))
					{
						NotificationReceiver notRec = new NotificationReceiver();
						notRec.ReceiverID = product.VendorID;
						notRec.ReceiverType = "Vendor";
						notRec.NotificationID = not.ID;
						if (_notificationReceiverService.CreateNotificationReceiver(notRec, ref message))
						{

						}

					}
					else
					{
						ErrorMessage = "Oops! Something went wrong. Please try later.";
					}
				}
			log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} approved all product ");
			}
			// return RedirectToAction("Index");
			return Json(new { success = true }, JsonRequestBehavior.AllowGet);

		}

		public ActionResult Activate(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var product = _productService.GetProduct((long)id);
			if (product == null)
			{
				return HttpNotFound();
			}

			if (!(bool)product.IsActive)
				product.IsActive = true;
		
			else
			{
				product.IsActive = false;
			}
			string message = string.Empty;
			if (_productService.UpdateProduct(ref product, ref message))
			{
				SuccessMessage = "Product " + ((bool)product.IsActive ? "activated" : "deactivated") + "  successfully ...";
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)product.IsActive ? "activated" : "deactivated")} product {product.SKU}.");
				return Json(new
				{
					success = true,
					message = SuccessMessage,
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				ErrorMessage = "Oops! Something went wrong. Please try later.";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region Reports

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ApprovalReport()
		{
			var getAllApprovals = _productService.GetProducts().ToList();
			if (getAllApprovals.Count() > 0)
			{
				using (ExcelPackage excel = new ExcelPackage())
				{
					excel.Workbook.Worksheets.Add("ApprovalReport");

					var headerRow = new List<string[]>()
					{
					new string[] {
						"Creation Date"
						,"Vendor Code"
						,"Vendor Name"
						,"Product Name"
						,"Status"
						}
					};

					// Determine the header range (e.g. A1:D1)
					string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

					// Target a worksheet
					var worksheet = excel.Workbook.Worksheets["ApprovalReport"];

					// Popular header row data
					worksheet.Cells[headerRange].LoadFromArrays(headerRow);

					var cellData = new List<object[]>();

					foreach (var i in getAllApprovals)
					{
						cellData.Add(new object[] {
						i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
						,!string.IsNullOrEmpty(i.Vendor.VendorCode) ? i.Vendor.VendorCode : "-"
						,!string.IsNullOrEmpty(i.Vendor.Name) ? i.Vendor.Name : "-"
						,!string.IsNullOrEmpty(i.Name) ? i.Name : "-"
						,i.ApprovalStatus != null ? ((ProductApprovalStatus)i.ApprovalStatus).ToString() : "-"
						});
					}

					worksheet.Cells[2, 1].LoadFromArrays(cellData);

					return File(excel.GetAsByteArray(), "application/msexcel", "Products Approval Report.xlsx");
				}
			}
			return RedirectToAction("Approvals");
		}

        [HttpPost]
        public ActionResult ProductsReport(long? VendorID)
        {
			if(VendorID != null)
			{

				var getAllProducts = _productService.GetVendorProductsByVendorID((long)VendorID);

				if (getAllProducts.Count() > 0)
			{
				using (ExcelPackage excel = new ExcelPackage())
				{
					excel.Workbook.Worksheets.Add("ProductsReport");

					var headerRow = new List<string[]>()
					{

							new string[] {
						"Creation Date"
						,"Name"
						,"Name Ar"
						,"ShortDescription"
						,"LongDescription"
						,"Type"
						,"Vendor Name"
						,"Shipping Name"
						,"SKU"
						,"Stock"
						,"Regular Price"
						,"Sale Price"
						,"IsApproved"
						,"Approval Status"
						,"Status"
						,"IsActive"
						}
					};

					// Determine the header range (e.g. A1:D1)
					string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

					// Target a worksheet
					var worksheet = excel.Workbook.Worksheets["ProductsReport"];

					// Popular header row data
					worksheet.Cells[headerRange].LoadFromArrays(headerRow);

					var cellData = new List<object[]>();

					if (getAllProducts.Count() != 0)
						getAllProducts = getAllProducts.OrderByDescending(x => x.ID).ToList();


					foreach (var i in getAllProducts)
					{
						cellData.Add(new object[] {
							i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
							,!string.IsNullOrEmpty(i.Name) ? i.Name: "-"
							,!string.IsNullOrEmpty(i.NameAr) ? i.NameAr: "-"
							,!string.IsNullOrEmpty(i.ShortDescription) ? i.ShortDescription: "-"
							,!string.IsNullOrEmpty(i.LongDescription) ? i.LongDescription: "-"
							,!string.IsNullOrEmpty(i.Type) ? i.Type: "-"
							,i.Vendor != null ? i.Vendor.Name: "-"
							,i.ShippingClass != null ? i.ShippingClass.Name: "-"
							,!string.IsNullOrEmpty(i.SKU) ? i.SKU: "-"
							,i.Stock != null ? i.Stock: 0
							,i.RegularPrice != null ? i.RegularPrice : 0
							,i.SalePrice != null ? i.SalePrice : 0
							,i.IsApproved == true ? "Approved" : "Un Approved"
							,i.ApprovalStatus == 1 ? "true" : "false"
							,!string.IsNullOrEmpty(i.Status) ? i.Status: "-"
							,i.IsActive == true ? "Active" : "InActive"
							});
					}

					worksheet.Cells[2, 1].LoadFromArrays(cellData);

					return File(excel.GetAsByteArray(), "application/msexcel", "Product Report.xlsx");
				}
			}   
			}
			return RedirectToAction("Index");
        }


        #endregion
    }
}