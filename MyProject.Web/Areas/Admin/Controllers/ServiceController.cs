using OfficeOpenXml;
using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using Project.Web.Helpers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
	public class ServiceController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IServicesService _servicesService;
		private readonly ICategoryService _categoryService;

		public ServiceController(IServicesService servicesService, ICategoryService categoryService)
		{
			this._servicesService = servicesService;
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
			var services = this._servicesService.GetServicesByDate(fromDate, EndDate);
			return PartialView(services);
		}
		[HttpPost]
		public ActionResult List(DateTime fromDate, DateTime toDate)
		{
			toDate = AdminCustomHelper.GetToDate(toDate);
			var AdminID = 0;
			var services = this._servicesService.GetServicesByDate(fromDate, toDate, AdminID);
			return PartialView(services);
		}
		public ActionResult Create()
		{
			ViewBag.CategoryID = new SelectList(this._categoryService.GetCategoriesForDropDown(), "value", "text");
			return View();
		}
		public ActionResult Details(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Data.Service service = _servicesService.GetService((Int16)id);
			if (service == null)
			{
				return HttpNotFound();
			}
			ViewBag.CategoryID = new SelectList(this._categoryService.GetCategoriesForDropDown(), "value", "text", service.CategoryID);

			return View(service);
		}
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public ActionResult Create(Data.Service data)
		{
			try
			{
				string message = string.Empty;
				if (ModelState.IsValid)
				{
					Data.Service record = new Data.Service();
					record.Name = data.Name;
					record.NameAr = data.NameAr;
					record.CategoryID = data.CategoryID;
					record.Slug = data.Slug;
					record.Type = data.Type;
					record.Description = data.Description;
					record.DescriptionAr = data.DescriptionAr;
					record.Position = data.Position;
					string FilePath = string.Format("{0}{1}{2}", Server.MapPath("~/Assets/AppFiles/Images/Services/"), data.Name.Replace(" ", "_"), "/Image");
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Images/Services/{0}/", data.Name.Replace(" ", "_"));




					if (Request.Files.Count > 0)
					{
						try
						{
							//  Get all files from Request object  
							HttpFileCollectionBase files = Request.Files;
							if (files.Count > 0)
							{
								if (data.Thumbnail != null)
								{
									relativePath = string.Format("/Assets/AppFiles/Images/Services/{0}/", data.Name.Replace(" ", "_"));
									record.Thumbnail = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Thumbnail", ref message, "Thumbnail");
									if (!string.IsNullOrEmpty(data.Thumbnail))
									{
										data.Thumbnail = CustomURL.GetImageServer() + data.Thumbnail.Remove(0, 1);
									}
								}
								if (data.Video != null)
								{
									relativePath = string.Format("/Assets/AppFiles/NewsFeed/Video/{0}/");
									data.Video = Uploader.UploadVideo(files, absolutePath, relativePath, "Video", ref message, "Video");
									if (!string.IsNullOrEmpty(data.Video))
									{
										data.Video = CustomURL.GetImageServer() + data.Video.Remove(0, 1);
									}
								}
							}
						}
						catch (Exception ex)
						{
							//add logs here...
						}
					}
					if (_servicesService.CreateSerivce(record, ref message))
					{
						log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created service {record.Name}.");
						//var Parent = category.ParentCategoryID.HasValue ? _categoryService.GetCategory((long)category.ParentCategoryID) : null;
						var category = _categoryService.GetCategory((long)record.CategoryID);
						return Json(new
						{
							success = true,
							url = "/Admin/Category/Index",
							message = message,
							data = new
							{
								ID = record.ID,
								Date = record.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
								Name = record.Name,
								Thumbnail = record.Thumbnail,
								Type = record.Type,
								Position = record.Position,
								Category = category.CategoryName,
								IsActive = record.IsActive.HasValue ? record.IsActive.Value.ToString() : bool.FalseString
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
			Data.Service service = _servicesService.GetService((long)id);
			if (service == null)
			{
				return HttpNotFound();
			}

			TempData["ServiceID"] = id;
			ViewBag.CategoryID = new SelectList(this._categoryService.GetCategoriesForDropDown(), "value", "text", service.CategoryID);
			//ViewBag.CategoryID = new SelectList(_categoryService.GetCategoriesForDropDown(id), "value", "text", service.CategoryID);
			return View(service);
		}

		[HttpPost]
		//[ValidateAntiForgeryToken]
		public ActionResult Edit(Data.Service data)
		{
			try
			{
				string message = string.Empty;

				Data.Service updateService = _servicesService.GetService(data.ID);

				string previousImagepath = string.IsNullOrEmpty(updateService.Thumbnail) ? "" : updateService.Thumbnail.Replace(CustomURL.GetImageServer(), "");
				//string previousVideopath = updateService.Video.Replace(CustomURL.GetImageServer(), "");
				if (ModelState.IsValid)
				{
					long Id;
					if (TempData["ServiceID"] != null && Int64.TryParse(TempData["ServiceID"].ToString(), out Id) && data.ID == Id)
					{

						//Data.Service service = _servicesService.GetService(data.ID);
						updateService.Name = data.Name;
						updateService.NameAr = data.NameAr;
						updateService.Slug = data.Slug;
						updateService.Type = data.Type;
						updateService.Description = data.Description;
						updateService.DescriptionAr = data.DescriptionAr;
						updateService.Position = data.Position;
						updateService.CategoryID = data.CategoryID;

						if (Request.Files.Count > 0)
						{
							try
							{
								//  Get all files from Request object  
								HttpFileCollectionBase files = Request.Files;
								if (files.Count > 0)
								{
									string absolutePath = Server.MapPath("~");
									if (data.Thumbnail != null)
									{
										string relativePath = string.Format("/Assets/AppFiles/Images/Services/{0}/", data.Name.Replace(" ", "_"));
										updateService.Thumbnail = Uploader.UploadImage(files, absolutePath, relativePath, "Thumbnail", ref message, "Thumbnail");
										if (!string.IsNullOrEmpty(updateService.Thumbnail))
										{
											updateService.Thumbnail = CustomURL.GetImageServer() + updateService.Thumbnail.Remove(0, 1);
										}
										//delete old file
										if (System.IO.File.Exists(previousImagepath))
										{
											System.IO.File.Delete(previousImagepath);
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
							if (string.IsNullOrEmpty(data.Thumbnail))
							{
								updateService.Thumbnail = "";
								//delete old file
								if (System.IO.File.Exists(previousImagepath))
								{
									System.IO.File.Delete(previousImagepath);
								}
							}
						}

						//if (Request.Files["Image"] != null)
						//{
						//	string FilePath = string.Format("{0}{1}{2}", Server.MapPath("~/Assets/AppFiles/Images/Services/"), Name.Replace(" ", "_"), "/Image");

						//	string absolutePath = Server.MapPath("~");
						//	string relativePath = string.Format("/Assets/AppFiles/Images/Services/{0}/", Name.Replace(" ", "_"));
						//	service.Thumbnail = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "logo", ref message, "Image");
						//}

						if (_servicesService.UpdateService(ref updateService, ref message))
						{
							var category = _categoryService.GetCategory((long)updateService.CategoryID);
							log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated service {data.Name}.");
							return Json(new
							{
								success = true,
								url = "/Admin/Service/Index",
								message = "Service updated successfully ...",
								data = new
								{
									ID = updateService.ID,
									Date = updateService.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
									Name = updateService.Name,
									Thumbnail = updateService.Thumbnail,
									Type = updateService.Type,
									Position = updateService.Position,
									Category = category.CategoryName,
									IsActive = updateService.IsActive.HasValue ? updateService.IsActive.Value.ToString() : bool.FalseString
								}
							});
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
			string message = string.Empty;
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var service = _servicesService.GetService((long)id);
			if (service == null)
			{
				return HttpNotFound();
			}

			if (!(bool)service.IsActive) 
			{
				service.IsActive = true;
				
			}
			else
			{
				service.IsActive = false;
			}

			if (_servicesService.UpdateService(ref service, ref message))
			{

				SuccessMessage = "Service " + ((bool)service.IsActive ? "activated" : "deactivated") + "  successfully ...";
				var category = _categoryService.GetCategory((long)service.CategoryID);
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)service.IsActive ? "activated" : "deactivated")} {service.Name}.");
				return Json(new
				{
					success = true,
					//hadChilds = hasChilds,
					message = SuccessMessage,
					data = new
					{
						ID = service.ID,
						Date = service.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
						Name = service.Name,
						Thumbnail = service.Thumbnail,
						Type = service.Type,
						Category = category.CategoryName,
						Position = service.Position,
						IsActive = service.IsActive.HasValue ? service.IsActive.Value.ToString() : bool.FalseString
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

			if (_servicesService.DeleteService((Int16)id, ref message, true))
			{
				Data.Service service = _servicesService.GetService((Int16)id);
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted service {service.Name}.");
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
				var getAllServices = _servicesService.GetServicesByDate(FromDate.Value, EndDate, AdminID).ToList();
				if (getAllServices.Count() > 0)
				{
					using (ExcelPackage excel = new ExcelPackage())
					{
						excel.Workbook.Worksheets.Add("ServicesReport");

						var headerRow = new List<string[]>()
					{

							new string[] {
						"Creation Date"
						,"Service"
						,"Type"
						,"Category"
						,"Position"
						,"Status"
						
						}
					};

						// Determine the header range (e.g. A1:D1)
						string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

						// Target a worksheet
						var worksheet = excel.Workbook.Worksheets["ServicesReport"];

						// Popular header row data
						worksheet.Cells[headerRange].LoadFromArrays(headerRow);

						var cellData = new List<object[]>();

						if (getAllServices.Count != 0)
							getAllServices = getAllServices.OrderByDescending(x => x.ID).ToList();

						
						foreach (var i in getAllServices)
						{
							cellData.Add(new object[] {
							i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
							,!string.IsNullOrEmpty(i.Name) ? i.Name: "-"
							,!string.IsNullOrEmpty(i.Type) ? i.Type : "-"
							,i.Category != null ? (!string.IsNullOrEmpty(i.Category.CategoryName) ? i.Category.CategoryName: "-") : "-"
							,i.Position != null ? i.Position : 0
							,i.IsActive == true ? "Active" : "InActive"
							});
						}

						worksheet.Cells[2, 1].LoadFromArrays(cellData);

						return File(excel.GetAsByteArray(), "application/msexcel", "Service Report.xlsx");
					}
				}
			}
			return RedirectToAction("Index");
		}
	}
}