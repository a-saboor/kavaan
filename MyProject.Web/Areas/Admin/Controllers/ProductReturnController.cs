using Project.Data;
using Project.Service;
using Project.Web.Helpers.Routing;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizationProvider.AuthorizeAdmin]
	public class ProductReturnController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IProductReturnService _productReturnService;
		private readonly INotificationService _notificationService;
		private readonly INotificationReceiverService _notificationReceiverService;
		private readonly IVendorWalletShareService _vendorWalletShareService;

		public ProductReturnController(IProductReturnService productReturnService, INotificationService notificationService, INotificationReceiverService notificationReceiverService, IVendorWalletShareService vendorWalletShareService)
		{
			this._productReturnService = productReturnService;
			this._notificationService = notificationService;
			this._notificationReceiverService = notificationReceiverService;
			this._vendorWalletShareService = vendorWalletShareService;
		}

		public ActionResult Index()
		{
			ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
			ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-7).ToString("MM/dd/yyyy");
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			return View();
		}

		public ActionResult List()
		{
			var Return = _productReturnService.GetProductReturns();
			return PartialView(Return);
		}

		[HttpPost]
		public ActionResult List(DateTime fromDate, DateTime ToDate)
		{
			DateTime EndDate = ToDate.AddMinutes(1439);
			var Return = _productReturnService.GetProductReturnsDateWise(fromDate, EndDate);
			return PartialView(Return);
		}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult ProductReturnsReport(DateTime fromDate, DateTime ToDate)
		//{
		//	DateTime EndDate = ToDate.AddMinutes(1439);
		//	var getAllReturns = _productReturnService.GetProductReturnsDateWise(fromDate, EndDate);
		//	if (getAllReturns.Count() > 0)
		//	{
		//		string ImageServer = CustomURL.GetImageServer();
		//		using (ExcelPackage excel = new ExcelPackage())
		//		{
		//			excel.Workbook.Worksheets.Add("ProductReturns");

		//			var headerRow = new List<string[]>()
		//			{
		//			new string[] {
		//				"Creation Date"
		//				,"Order No"
		//				,"Product SKU"
		//				,"Product Name"
		//				,"Product Image"
		//				,"Customer Name"
		//				,"Customer Contact"
		//				,"Customer Email"
		//				,"Return Type"
		//				,"Reason"
		//				,"Received Product Images"
		//				,"Status"
		//				}
		//			};

		//			// Determine the header range (e.g. A1:D1)
		//			string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

		//			// Target a worksheet
		//			var worksheet = excel.Workbook.Worksheets["ProductReturns"];

		//			// Popular header row data
		//			worksheet.Cells[headerRange].LoadFromArrays(headerRow);

		//			var cellData = new List<object[]>();

		//			foreach (var i in getAllReturns)
		//			{
		//				string Images = i.ProductReturnImages.Count != 0 ? string.Join(", ", i.ProductReturnImages.Select(x => ImageServer + x.Image)) : "-";

		//				cellData.Add(new object[] {
		//				i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
		//				,i.OrderDetail != null ? "" +i.OrderDetail.Order != null ? i.OrderDetail.Order.OrderNo : "-" + "" : "-"
		//				,i.Product != null ? !string.IsNullOrEmpty(i.Product.SKU) ? i.Product.SKU : "-" : "-"
		//				,i.Product != null ? !string.IsNullOrEmpty(i.Product.Name) ? i.Product.Name : "-" : "-"
		//				,i.Product != null ? !string.IsNullOrEmpty(i.Product.Thumbnail) ? (ImageServer + i.Product.Thumbnail) : "-" : "-"
		//				,i.Customer != null ? !string.IsNullOrEmpty(i.Customer.Name) ? i.Customer.Name : "-" : "-"
		//				,i.Customer != null ? !string.IsNullOrEmpty(i.Customer.Contact) ? i.Customer.Contact : "-" : "-"
		//				,i.Customer != null ? !string.IsNullOrEmpty(i.Customer.Email) ? i.Customer.Email : "-" : "-"
		//				,!string.IsNullOrEmpty(i.ReturnMethod) ? i.ReturnMethod : "-"
		//				,!string.IsNullOrEmpty(i.Reason) ? i.Reason : "-"
		//				,Images
		//				,!string.IsNullOrEmpty(i.Status) ? i.Status : "-"
		//				});
		//			}

		//			worksheet.Cells[2, 1].LoadFromArrays(cellData);

		//			return File(excel.GetAsByteArray(), "application/msexcel", "Product Returns Report.xlsx");
		//		}
		//	}
		//	return RedirectToAction("Index");
		//}

		//public ActionResult StatusChange(long id)
		//{
		//	var productReturn = _productReturnService.GetProductReturn((long)id);
		//	return View(productReturn);
		//}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult StatusChange(ProductReturn productReturn)
		//{
		//	try
		//	{
		//		string message = string.Empty;
		//		if (_productReturnService.UpdateProductReturn(ref productReturn, ref message))
		//		{
		//			/*need to add amount substraction from vendor wallet logic */
		//			//if (productReturn.Status == "Completed")
		//			//{
		//			//	_vendorWalletShareService.UpdateVendorEarning(orderID.ID);
		//			//}

		//			Notification not = new Notification();

		//			if (productReturn.Status == "Pending")
		//			{
		//				not.Title = "Return Request Placed";
		//				not.TitleAr = "Return Request  Placed";
		//				not.Description = string.Format("Your return request # {0} has been placed. You can check the return status via my returns", productReturn.ReturnCode);
		//				not.DescriptionAr = string.Format("Your return request # {0} has been placed. You can check the return status via my returns", productReturn.ReturnCode);

		//			}
		//			else if (productReturn.Status == "Completed")
		//			{
		//				not.Title = "Return Request Completed";
		//				not.TitleAr = "Return Request  Completed";
		//				not.Description = string.Format("Your return request # {0} has been completed. You can check the return status via my returns", productReturn.ReturnCode);
		//				not.DescriptionAr = string.Format("Your return request # {0} has been completed. You can check the return status via my returns", productReturn.ReturnCode);
		//			}
		//			else if (productReturn.Status == "Canceled")
		//			{
		//				not.Title = "Return Request Canceled";
		//				not.TitleAr = "Return Request  Canceled";
		//				not.Description = string.Format("Your return request # {0} has been canceled. You can check the return status via my returns", productReturn.ReturnCode);
		//				not.DescriptionAr = string.Format("Your return request # {0} has been canceled. You can check the return status via my returns", productReturn.ReturnCode);
		//			}
		//			else if (productReturn.Status == "Closed")
		//			{
		//				not.Title = "Return Request Closed";
		//				not.TitleAr = "Return Request  Closed";
		//				not.Description = string.Format("Your return request # {0} has been closed. You can check the return status via my returns", productReturn.ReturnCode);
		//				not.DescriptionAr = string.Format("Your return request # {0} has been closed. You can check the return status via my returns", productReturn.ReturnCode);
		//			}

		//			not.OriginatorID = Convert.ToInt64(Session["AdminUserID"]);
		//			not.OriginatorName = Session["UserName"].ToString();
		//			not.Module = "Order";
		//			not.OriginatorType = "Admin";
		//			not.RecordID = productReturn.ID;
		//			if (_notificationService.CreateNotification(not, ref message))
		//			{
		//				NotificationReceiver notRec = new NotificationReceiver();
		//				notRec.ReceiverID = productReturn.CustomerID;
		//				notRec.ReceiverType = "Customer";
		//				notRec.NotificationID = not.ID;
		//				if (_notificationReceiverService.CreateNotificationReceiver(notRec, ref message))
		//				{
		//				}
		//			}
		//			return Json(new
		//			{
		//				success = true,
		//				url = "/Admin/ProductReturn/Index",
		//				message = "Product return request status updated successfully ...",
		//				data = new
		//				{
		//					Date = productReturn.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
		//					OrderNo = productReturn.OrderDetail.Order.OrderNo,
		//					Customer = productReturn.Customer.Name,
		//					Product = productReturn.Product.Name,
		//					Status = productReturn.Status,
		//					ID = productReturn.ID
		//				}
		//			});
		//		}
		//		else
		//		{

		//			return Json(new
		//			{
		//				success = false,
		//				message = "Oops! Something went wrong. Please try later."
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

		public ActionResult Details(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ProductReturn rating = _productReturnService.GetProductReturn((Int16)id);
			if (rating == null)
			{
				return HttpNotFound();
			}
			return View(rating);
		}
	}
}