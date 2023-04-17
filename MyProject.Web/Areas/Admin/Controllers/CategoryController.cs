using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using Project.Web.Helpers.Routing;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class CategoryController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private readonly ICategoryService _categoryService;


		public CategoryController(ICategoryService categoryService)
		{
			this._categoryService = categoryService;
		}

		public ActionResult Index()
		{
			ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
			ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30).ToString("MM/dd/yyyy");
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			return View();
		}

		public ActionResult List()
		{
			DateTime EndDate = Helpers.TimeZone.GetLocalDateTime();
			DateTime fromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30);
			var services = this._categoryService.GetCategoriesByDate(fromDate, EndDate);
			return PartialView(services);
		}
		[HttpPost]
		public ActionResult List(DateTime fromDate, DateTime toDate)
		{
			toDate = AdminCustomHelper.GetToDate(toDate);
			var AdminID = 0;
			var services = this._categoryService.GetCategoriesByDate(fromDate, toDate, AdminID);
			return PartialView(services);
		}

		public ActionResult Details(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Category category = _categoryService.GetCategory((Int16)id);
			if (category == null)
			{
				return HttpNotFound();
			}
			return View(category);
		}

		public ActionResult Create()
		{
			ViewBag.ParentCategoryID = new SelectList(_categoryService.GetCategoriesForDropDown(0), "value", "text");
			return View();
		}

		[HttpPost]
		//[ValidateAntiForgeryToken]
		public ActionResult Create(Category data)
		{
			try
			{
				string message = string.Empty;
				if (ModelState.IsValid)
				{
                    string absolutePath = Server.MapPath("~");
					string relativePath = "";
                    Category category = new Category();
					category.CategoryName = data.CategoryName;
					category.CategoryNameAr = data.CategoryNameAr;
					//category.ParentCategoryID = ParentCategoryID;
					category.Slug = data.Slug;
					category.Description = data.Description;
					category.DescriptionAR = data.DescriptionAR;
					//category.IsDefault = IsDefault;
					category.Position = data.Position;
					//category.Icon = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "logo", ref message, "Image");
					if (Request.Files.Count > 0)
					{
						try
						{
							//  Get all files from Request object  
							HttpFileCollectionBase files = Request.Files;
							if (files.Count > 0)
							{
								absolutePath = Server.MapPath("~");
								if (data.Icon != null)
								{
									relativePath = string.Format("/Assets/AppFiles/Category/Icon/{0}/", data.CategoryName.Replace(" ", "_"));
									category.Icon = Uploader.UploadImage(files, absolutePath, relativePath, "Icon", ref message, "Icon");
									if (!string.IsNullOrEmpty(data.Icon))
									{
										data.Icon = CustomURL.GetImageServer() + data.Icon.Remove(0, 1);
									}
								}
								if (data.Image != null)
								{
									relativePath = string.Format("/Assets/AppFiles/Category/Images/{0}/", data.CategoryName.Replace(" ", "_"));
									category.Image = Uploader.UploadImage(files, absolutePath, relativePath, "Image", ref message, "Image");
									if (!string.IsNullOrEmpty(data.Image))
									{
										data.Image = CustomURL.GetImageServer() + data.Image.Remove(0, 1);
									}
								}
								if (data.Thumbnail != null)
								{
									relativePath = string.Format("/Assets/AppFiles/Category/Images/{0}/", data.CategoryName.Replace(" ", "_"));
									category.Thumbnail = Uploader.UploadImage(files, absolutePath, relativePath, "Thumbnail", ref message, "Thumbnail");
									if (!string.IsNullOrEmpty(data.Thumbnail))
									{
										data.Image = CustomURL.GetImageServer() + data.Thumbnail.Remove(0, 1);
									}
								}
							}
						}
						catch (Exception ex)
						{
							//add logs here...
						}
					}

					if (_categoryService.CreateCategory(category, ref message))
					{
						log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created category {category.CategoryName}.");
						//var Parent = category.ParentCategoryID.HasValue ? _categoryService.GetCategory((long)category.ParentCategoryID) : null;
						return Json(new
						{
							success = true,
							url = "/Admin/Category/Index",
							message = message,
							data = new
							{
								ID = category.ID,
								Date = category.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
								Image = category.Image,
								Category = category.CategoryName,
								Position = category.Position,
								IsActive = category.IsActive.HasValue ? category.IsActive.Value.ToString() : bool.FalseString
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
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = "Oops! Something went wrong. Please try later."
				});
			}
		}

		public ActionResult Edit(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Category category = _categoryService.GetCategory((long)id);
			if (category == null)
			{
				return HttpNotFound();
			}

			TempData["CategoryID"] = id;

			ViewBag.ParentCategoryID = new SelectList(_categoryService.GetCategoriesForDropDown(id), "value", "text", category.ParentCategoryID);
			return View(category);
		}

		[HttpPost]
		//[ValidateAntiForgeryToken]
		public ActionResult Edit(Category data)
		{
			try
			{
				string message = string.Empty;
				Category category = _categoryService.GetCategory(data.ID);
				string previousImagepath = !string.IsNullOrEmpty(category.Icon) ? category.Icon.Replace(CustomURL.GetImageServer(), "") : category.Icon;
				string previousImagepath1 = !string.IsNullOrEmpty(category.Image) ? category.Image.Replace(CustomURL.GetImageServer(), "") : category.Image;
				string previousImagepath2 = !string.IsNullOrEmpty(category.Thumbnail) ? category.Thumbnail.Replace(CustomURL.GetImageServer(), "") : category.Thumbnail;
				if (ModelState.IsValid)
				{
					long Id;
					if (TempData["CategoryID"] != null && Int64.TryParse(TempData["CategoryID"].ToString(), out Id) && data.ID == Id)
					{
						category.CategoryName = data.CategoryName;
						category.CategoryNameAr = data.CategoryNameAr;
						//category.ParentCategoryID = ParentCategoryID;
						category.Slug = data.Slug;
						category.Description = data.Description;
						category.DescriptionAR = data.DescriptionAR;
						//category.IsDefault = IsDefault;
						category.IsParentCategoryDeleted = false;
						category.Position = data.Position;

						if (Request.Files.Count > 0)
						{
							try
							{
								//  Get all files from Request object  
								HttpFileCollectionBase files = Request.Files;
								if (files.Count > 0)
								{
									string absolutePath = Server.MapPath("~");
									if (data.Icon != null && !data.Icon.Equals(CustomURL.GetImageServer() + previousImagepath))
									{
										string relativePath = string.Format("/Assets/AppFiles/Category/Icon/{0}/", category.CategoryName.Replace(" ", "_"));
										category.Icon = Uploader.UploadImage(files, absolutePath, relativePath, "Icon", ref message, "Icon");
										if (!string.IsNullOrEmpty(category.Icon))
										{
											category.Icon = CustomURL.GetImageServer() + category.Icon.Remove(0, 1);
										}
										//delete old file
										if (System.IO.File.Exists(previousImagepath))
										{
											System.IO.File.Delete(previousImagepath);
										}
									}
									if (data.Image != null && !data.Image.Equals(CustomURL.GetImageServer() + previousImagepath1))
									{
										string relativePath1 = string.Format("/Assets/AppFiles/Category/Images/{0}/", category.CategoryName.Replace(" ", "_"));
										category.Image = Uploader.UploadImage(files, absolutePath, relativePath1, "Image", ref message, "Image");
										if (!string.IsNullOrEmpty(category.Image))
										{
											category.Image = CustomURL.GetImageServer() + category.Image.Remove(0, 1);
										}
										//delete old file
										if (System.IO.File.Exists(previousImagepath1))
										{
											System.IO.File.Delete(previousImagepath1);
										}
									}
									if (data.Thumbnail != null && !data.Thumbnail.Equals(CustomURL.GetImageServer() + previousImagepath2))
									{
										string relativePath = string.Format("/Assets/AppFiles/Category/Images/{0}/", category.CategoryName.Replace(" ", "_"));
										category.Thumbnail = Uploader.UploadImage(files, absolutePath, relativePath, "Thumbnail", ref message, "Thumbnail");
										if (!string.IsNullOrEmpty(category.Image))
										{
											category.Thumbnail = CustomURL.GetImageServer() + category.Thumbnail.Remove(0, 1);
										}
										//delete old file
										if (System.IO.File.Exists(previousImagepath2))
										{
											System.IO.File.Delete(previousImagepath2);
										}
									}
								}
							}
							catch (Exception ex)
							{
								//add logs here...
							}
						}
						else
						{
							if (string.IsNullOrEmpty(data.Icon))
							{
								category.Icon = "";
								//delete old file
								if (System.IO.File.Exists(previousImagepath))
								{
									System.IO.File.Delete(previousImagepath);
								}
							}
							if (string.IsNullOrEmpty(data.Image))
							{
								category.Image = "";
								//delete old file
								if (System.IO.File.Exists(previousImagepath1))
								{
									System.IO.File.Delete(previousImagepath1);
								}
							}
							if (string.IsNullOrEmpty(data.Thumbnail))
							{
								category.Thumbnail = "";
								//delete old file
								if (System.IO.File.Exists(previousImagepath2))
								{
									System.IO.File.Delete(previousImagepath2);
								}
							}

						}

						if (_categoryService.UpdateCategory(ref category, ref message, false))
						{
							log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated category {category.CategoryName}.");
							//var Parent = ParentCategoryID.HasValue ? _categoryService.GetCategory((long)category.ParentCategoryID) : null;
							return Json(new
							{
								success = true,
								url = "/Admin/Category/Index",
								message = "Category updated successfully...",
								data = new
								{
									ID = category.ID,
									Date = category.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
									//Parent = Parent != null ? (Parent.CategoryName) : "",
									Image = category.Image,
									Category = category.CategoryName,
									Position = category.Position,
									//IsParentCategoryDeleted = category.IsParentCategoryDeleted.HasValue ? category.IsParentCategoryDeleted.Value.ToString() : bool.FalseString,
									IsActive = category.IsActive.HasValue ? category.IsActive.Value.ToString() : bool.FalseString
								}
							}, JsonRequestBehavior.AllowGet);
						}
					}
					else
					{
						message = "Oops! Something went wrong. Please try later.";
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

		public ActionResult Activate(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var category = _categoryService.GetCategory((long)id);
			if (category == null)
			{
				return HttpNotFound();
			}

			if (!(bool)category.IsActive) 
			{
				category.IsActive = true;
			}

			else
			{
				category.IsActive = false;
			}
			string message = string.Empty;
			if (_categoryService.UpdateCategory(ref category, ref message))
			{
				//bool hasChilds = false;
				//if (category.Category1.Count() > 0)
				//{
				//	hasChilds = true;
				//	_categoryService.UpdateDeletedCategoryChilds(category.ID);
				//}
				//else
				//{
				//	hasChilds = false;
				//}
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} country {((bool)category.IsActive ? "activated" : "deactivated")} {category.CategoryName}.");
				SuccessMessage = "Category " + ((bool)category.IsActive ? "activated" : "deactivated") + "  successfully...";
				//var Parent = category.ParentCategoryID.HasValue ? _categoryService.GetCategory((long)category.ParentCategoryID) : null;
				return Json(new
				{
					success = true,
					//hadChilds = hasChilds,
					message = SuccessMessage,
					data = new
					{
						ID = category.ID,
						Date = category.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
						Image = category.Image,
						Category = category.CategoryName,
						Position = category.Position,
						IsActive = category.IsActive.HasValue ? category.IsActive.Value.ToString() : bool.FalseString

					}
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				ErrorMessage = "Oops! Something went wrong. Please try later...";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Delete(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Category category = _categoryService.GetCategory((Int16)id);
			if (category == null)
			{
				return HttpNotFound();
			}
			TempData["CategoryID"] = id;
			return View(category);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id)
		{
			string message = string.Empty;
			bool hasChilds = false;

			if (_categoryService.DeleteCategory((Int16)id, ref message, ref hasChilds))
			{
				Category category = _categoryService.GetCategory((Int16)id);
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted category {category.CategoryName}.");
				return Json(new
				{
					success = true,
					message = message,
					hadChilds = hasChilds
				}, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Report(DateTime? FromDate, DateTime? ToDate)
		{
			if (FromDate != null && ToDate != null)
			{
				DateTime EndDate = ToDate.Value.AddMinutes(1439);
				var AdminID = 0;
				var getAllCatagories = _categoryService.GetCategoriesByDate(FromDate.Value, EndDate, AdminID).ToList();
				if (getAllCatagories.Count() > 0)
				{
					using (ExcelPackage excel = new ExcelPackage())
					{
						excel.Workbook.Worksheets.Add("CategoriesReport");

						var headerRow = new List<string[]>()
						{
						new string[] {
							"Creation Date"
							,"Category Name"
							,"Sort Order"
							,"Status"
							}
						};

						// Determine the header range (e.g. A1:D1)
						string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

						// Target a worksheet
						var worksheet = excel.Workbook.Worksheets["CategoriesReport"];

						// Popular header row data
						worksheet.Cells[headerRange].LoadFromArrays(headerRow);

						var cellData = new List<object[]>();

						if (getAllCatagories.Count != 0)
							getAllCatagories = getAllCatagories.OrderByDescending(x => x.ID).ToList();

						foreach (var i in getAllCatagories)
						{
							//string parentCategory = "-";
							//if (i.ParentCategoryID != null)
							//	parentCategory = getAllCatagories.SingleOrDefault(x => x.ID == i.ParentCategoryID)?.CategoryName;

							cellData.Add(new object[] {
							i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
							,!string.IsNullOrEmpty(i.CategoryName) ? i.CategoryName : "-"
							,i.Position != null ? i.Position : 0
							,i.IsActive == true ? "Active" : "InActive"
							});
						}

						worksheet.Cells[2, 1].LoadFromArrays(cellData);

						return File(excel.GetAsByteArray(), "application/msexcel", "Service Category Report.xlsx");
					}
				}
			}
				return RedirectToAction("Index");
		}

	}
}