using MyProject.Data;
using MyProject.Service;
using MyProject.Service.Helpers;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace MyProject.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class CustomerDocumentDetailController : Controller
	{
		// GET: Admin/CustomerDocumentDetail
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly ICustomerDocumentDetailService _customerDocumentDetailsService;
		private readonly IMail _email;
		public CustomerDocumentDetailController(ICustomerDocumentDetailService customerDocumentDetailsService, IMail email)
		{
			this._customerDocumentDetailsService = customerDocumentDetailsService;
			this._email = email;
		}

		public ActionResult Index(int id = 0)
		{
			ViewBag.Id = id;
			return View();
		}

		public ActionResult List(int Id = 0)
		{
			var result = _customerDocumentDetailsService.GetCustomerDocumentDetails().OrderByDescending(x => x.ID).ToList();
			string filter = string.Empty;
			if (Id == 1)
				filter = "Pending";
			else if (Id == 2)
				filter = "Rejected";
			else if (Id == 3)
				filter = "Approved";

			if (Id != 0)
				result = result.Where(x => x.Status == filter).ToList();

			ViewBag.Status = filter + " ";
			return PartialView(result);
		}
		//public ActionResult Approve(long id)
		//{
		//	var path = Server.MapPath("~/");
		//	var subject = "Document Approval";
		//	CustomerDocumentDetail customerDocumentDetail = _customerDocumentDetailsService.GetCustomerDocumentDetail((long)id);
		//	customerDocumentDetail.Status = "Approved";
		//	string message = string.Empty;

		//	if (_customerDocumentDetailsService.UpdateCustomerDocumentDetail(ref customerDocumentDetail, ref message))
		//	{
		//		var emailMessage = " Your document type " + customerDocumentDetail.CustomerDocument.Type + " for ServiceNo:" + customerDocumentDetail.ServiceNo + " has been approved.";
		//		if (_email.SendDocumentStatusMail(customerDocumentDetail.Customer.Email, customerDocumentDetail.Customer.FirstName, subject, emailMessage, path))
		//		{
		//		}

		//		return Json(new
		//		{
		//			success = true,
		//			message = "Status updated successfully ...",
		//			data = new
		//			{
		//				ID = customerDocumentDetail.ID,
		//				CreatedOn = customerDocumentDetail.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
		//				Serviceno = customerDocumentDetail.ServiceNo,
		//				UserName = customerDocumentDetail.Customer != null ? customerDocumentDetail.Customer.UserName : null,
		//				Type = customerDocumentDetail.CustomerDocument != null ? customerDocumentDetail.CustomerDocument.Type : null,
		//				Path = customerDocumentDetail.Path,
		//				Status = customerDocumentDetail.Status,

		//			}
		//		}, JsonRequestBehavior.AllowGet);
		//	}
		//	return Json(new { success = false });
		//}
		public ActionResult StatusChange(long id, string status)
		{
			CustomerDocumentDetail customerDocumentDetail = _customerDocumentDetailsService.GetCustomerDocumentDetail((long)id);
			ViewBag.Status = status;
			return View(customerDocumentDetail);
		}

		[HttpPost]
		public ActionResult StatusChange(CustomerDocumentDetail csd)
		{
			var path = Server.MapPath("~/");
			var subject = "Document Request";
			string message = string.Empty;

			CustomerDocumentDetail customerDocumentDetail = _customerDocumentDetailsService.GetCustomerDocumentDetail((long)csd.ID);
			customerDocumentDetail.Status = csd.Status;
			customerDocumentDetail.Reason = "<p>" + csd.Reason + "</p><hr /><p>" + Helpers.TimeZone.GetLocalDateTime().ToString(CustomHelper.GetDateFormat) + "</p><br /><p>" + customerDocumentDetail.Reason + "</p>";

			if (_customerDocumentDetailsService.UpdateCustomerDocumentDetail(ref customerDocumentDetail, ref message))
			{
				var emailMessage = " Your document type " + customerDocumentDetail.CustomerDocumentID + " for ServiceNo:" + customerDocumentDetail.ServiceNo + " has been " + customerDocumentDetail.Status;

				if (customerDocumentDetail.Status == "Approved")
				{
					_email.SendDocumentStatusMail(customerDocumentDetail.Customer.Email, customerDocumentDetail.Customer.FirstName, subject, emailMessage, path);
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} approved customer document No: {customerDocumentDetail.ServiceNo}.");
				}
				else if (customerDocumentDetail.Status == "Cancelled" || customerDocumentDetail.Status == "Rejected")
				{
					_email.SendDocumentRejectedMail(customerDocumentDetail.Customer.Email, customerDocumentDetail.Customer.FirstName, csd.Reason, subject, emailMessage, path);
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} rejected customer document No: {customerDocumentDetail.ServiceNo}.");
				}

				return Json(new
				{
					success = true,
					url = "/Admin/CustomerDocumentDetail/Index",
					message = "Status updated successfully ...",
					data = new
					{
						ID = customerDocumentDetail.ID,
						CreatedOn = customerDocumentDetail.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
						Serviceno = customerDocumentDetail.ServiceNo,
						UserName = customerDocumentDetail.Customer != null ? customerDocumentDetail.Customer.UserName : null,
						Type = customerDocumentDetail.CustomerDocumentID != null ? customerDocumentDetail.CustomerDocumentID : null,
						Path = customerDocumentDetail.Path,
						Status = customerDocumentDetail.Status,
					}
				});
			}
			return View("Index");
		}

		public ActionResult Remarks(long id)
		{
			CustomerDocumentDetail customerDocumentDetail = this._customerDocumentDetailsService.GetCustomerDocumentDetail((long)id);
			if (customerDocumentDetail == null)
			{
				return HttpNotFound();
			}
			return View(customerDocumentDetail);
		}
	}
}