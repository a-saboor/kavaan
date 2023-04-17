using MyProject.Data;
using MyProject.Service;
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
	public class CustomerDocumentController : Controller
	{
		// GET: Admin/CustomerDocument
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly ICustomerDocumentService _customerDocumentService;
		private readonly ICustomerDocumentTypeService _customerDocumentTypeService;
		private readonly ICustomerService _customerService;
		private readonly ICustomerRelationService _customerRelation;
		private readonly INumberRangeService _numberRangeService;
		public CustomerDocumentController(ICustomerDocumentService customerDocumentService, ICustomerRelationService customerRelation, ICustomerDocumentTypeService customerDocumentTypeService, ICustomerService customerService, INumberRangeService numberRangeService)
		{
			this._customerDocumentService = customerDocumentService;
			this._customerService = customerService;
			this._customerRelation = customerRelation;
			this._customerDocumentTypeService = customerDocumentTypeService;
			this._numberRangeService = numberRangeService;
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult List(long id)
		{
			ViewBag.CustomerID = id;
			var result = _customerDocumentService.GetCustomerDocumentsByCustomerID(id);
			return PartialView(result);
		}

		public ActionResult Details(long id)
		{
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			CustomerDocument customerDocument = _customerDocumentService.GetCustomerDocument(id);
			if (customerDocument == null)
			{
				return HttpNotFound();
			}
			return View(customerDocument);
		}

		public ActionResult Create(long id)
		{
			ViewBag.CustomerID = id;
			ViewBag.CustomerDocumentTypeID = new SelectList(_customerDocumentTypeService.GetDocumentTypesForDropDown(), "value", "text");
			ViewBag.CustomerRelationID = new SelectList(_customerRelation.GetCustomerRelationForDropDown(), "value", "text");
			return View();
		}

		[HttpPost]

		public ActionResult Create(CustomerDocument customerDocument)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				CustomerDocument data = new CustomerDocument();
				data.CustomerID = customerDocument.CustomerID;
				data.CustomerRelationID = customerDocument.CustomerRelationID;
				data.ExpiryDate = customerDocument.ExpiryDate;
				data.CustomerDocumentTypeID = customerDocument.CustomerDocumentTypeID;
				data.Relation = customerDocument.Relation;
				data.DocumentNo = _numberRangeService.GetNextValueFromNumberRangeByName("DOCUMENT");
				var customerdocs = _customerDocumentTypeService.GetCustomerDocumentType((long)customerDocument.CustomerDocumentTypeID);
				string absolutePath = Server.MapPath("~");
				string relativePath = string.Format("/Assets/AppFiles/CustomerDocument/{0}/", customerdocs.Title.Replace(" ", "_"));
				//data.Type = Type;
				//data.TypeAr = TypeAr;
				if (_customerDocumentService.isDocumentAlreadyExist(ref data, ref message))
				{
					data.Path = Uploader.UploadDocs(Request.Files, absolutePath, relativePath, "Document", ref message, "FileUpload");
				}
				if (!string.IsNullOrEmpty(data.Path))
				{
					if (_customerDocumentService.CreateCustomerDocument(ref data, ref message))
					{
						CustomerRelation customerRelation = _customerRelation.GetCustomerRelation((long)data.CustomerRelationID);
						log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created customer document {data.DocumentNo}.");
						return Json(new
						{
							success = true,
							url = "/Admin/CustomerDocument/Index",
							message = message,
							data = new
							{
								CreatedOn = data.CreatedOn.HasValue ? data.CreatedOn.Value.ToString(CustomHelper.GetDateFormat) : "-",
								DocumentNo = data.DocumentNo,
								Relation = customerRelation.Relation,
								Type = data.CustomerDocumentType != null ? data.CustomerDocumentType.Title : "-",
								Path = data.Path,
								ExpiryDate = data.ExpiryDate.HasValue ? data.ExpiryDate.Value.ToString("dd MMM yyyy") : "-",
								//IsActive = data.IsActive.ToString(),
								ID = data.ID
							},
						});
					}
				}
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Edit(long id)
		{
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			CustomerDocument customerDocument = _customerDocumentService.GetCustomerDocument(id);
			if (customerDocument == null)
			{
				return HttpNotFound();
			}
			return View(customerDocument);
		}

		[HttpPost]
		public ActionResult Edit(string Type, string TypeAr, long id)
		{
			string message = string.Empty;
			string replacement = Guid.NewGuid().ToString();
			if (ModelState.IsValid)
			{
				string absolutePath = Server.MapPath("~");
				string relativePath = string.Format("/Assets/AppFiles/CustomerDocument/{0}/", Type.Replace(" ", "_"));

				CustomerDocument currentCustomerDocument = _customerDocumentService.GetCustomerDocument(id);
				//currentCustomerDocument.Type = Type;
				//currentCustomerDocument.TypeAr = TypeAr;

				if (Request.Files.Count > 0)
				{
					currentCustomerDocument.Path = Uploader.UploadDocs(Request.Files, absolutePath, relativePath, "Document", ref message, "FileUpload");
				}

				if (_customerDocumentService.UpdateCustomerDocument(ref currentCustomerDocument, ref message))
				{
					log.Info($"{Session["UserName"]} | {Session["Email"]} udpated customer document {currentCustomerDocument.DocumentNo}.");
					return Json(new
					{
						success = true,
						url = "/Admin/CustomerDocument/Index",
						message = message,
						data = new
						{
							CreatedOn = currentCustomerDocument.CreatedOn.HasValue ? currentCustomerDocument.CreatedOn.Value.ToString(CustomHelper.GetDateFormat) : "-",
							DocumentNo = currentCustomerDocument.DocumentNo,
							Relation = currentCustomerDocument.Relation,
							Type = currentCustomerDocument.CustomerDocumentType != null ? currentCustomerDocument.CustomerDocumentType.Title : "-",
							Path = currentCustomerDocument.Path,
							ExpiryDate = currentCustomerDocument.ExpiryDate.HasValue ? currentCustomerDocument.ExpiryDate.Value.ToString("dd MMM yyyy") : "-",
							//IsActive = currentCustomerDocument.IsActive.HasValue ? currentCustomerDocument.IsActive.Value.ToString() : bool.FalseString,
							ID = currentCustomerDocument.ID
						}
					}, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				message = "Please fill the form correctly";
			}
			return Json(new { success = false, message = message });
		}

		//public ActionResult Delete(long id)
		//{
		//    string messsage = string.Empty;
		//    if (id == 0)
		//    {
		//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//    }
		//    bool customerDocument = _customerDocumentService.DeleteCustomerDocument(id, ref messsage, true); ;
		//    if (!customerDocument)
		//    {
		//        return HttpNotFound();
		//    }
		//    return View(customerDocument);
		//}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id)
		{
			string message = string.Empty;
			string filePath = string.Empty;
			CustomerDocument document = _customerDocumentService.GetCustomerDocument(id);
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			bool customerDocument = _customerDocumentService.DeleteCustomerDocument(id, ref message, ref filePath, true); ;
			if (customerDocument)
			{
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted customer document ID: {id}.");
				System.IO.File.Delete(Server.MapPath(filePath));
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);


		}
		//public ActionResult Activate(long? id)
		//{
		//	if (id == null || id == 0)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}

		//	var customerDocument = _customerDocumentService.GetCustomerDocument((long)id);
		//	if (customerDocument == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	string message = string.Empty;
		//	if (!(bool)customerDocument.IsActive)
		//	{
		//		customerDocument.IsActive = true;
		//		log.Info($"{Session["UserName"]} | {Session["Email"]} activated customer document {customerDocument.Type}.");
		//	}
		//	else
		//	{
		//		customerDocument.IsActive = false;
		//		log.Info($"{Session["UserName"]} | {Session["Email"]} deactivated customer document {customerDocument.Type}.");
		//	}
		//	if (_customerDocumentService.UpdateCustomerDocument(ref customerDocument, ref message))
		//	{
		//		SuccessMessage = "Document " + ((bool)customerDocument.IsActive ? "activated" : "deactivated") + "  successfully ...";
		//		return Json(new
		//		{
		//			success = true,
		//			message = SuccessMessage,
		//			data = new
		//			{
		//				CreatedOn = customerDocument.CreatedOn.HasValue ? customerDocument.CreatedOn.Value.ToString(CustomHelper.GetDateFormat) : "-",
		//				Type = customerDocument.Type,
		//				Path = customerDocument.Path,
		//				IsActive = customerDocument.IsActive.ToString(),
		//				ID = customerDocument.ID
		//			}
		//		}, JsonRequestBehavior.AllowGet); ;
		//	}
		//	else
		//	{
		//		ErrorMessage = "Oops! Something went wrong. Please try later...";
		//	}

		//	return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		//}
	}
}