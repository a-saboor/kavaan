using MyProject.Data;
using MyProject.Service;
using MyProject.Web.Areas.CustomerPortal.ViewModels.Documents;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Controllers;
using MyProject.Web.Helpers;
using MyProject.Web.Helpers.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Web.Areas.CustomerPortal.Controllers
{
	[AuthorizeCustomer]
	public class DocumentsController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly ICustomerDocumentService _customerDocumentService;
		private readonly ICustomerDocumentDetailService _customerDocumentDetailService;
		private readonly ICustomerDocumentTypeService _customerDocumentTypeService;
		private readonly ICustomerService _customerService;
		private readonly ICustomerRelationService _customerRelation;
		private readonly INumberRangeService _numberRangeService;
		public DocumentsController(ICustomerDocumentService customerDocumentService, ICustomerDocumentDetailService customerDocumentDetailService, ICustomerRelationService customerRelation, ICustomerDocumentTypeService customerDocumentTypeService, ICustomerService customerService, INumberRangeService numberRangeService)
		{
			this._customerDocumentService = customerDocumentService;
			this._customerDocumentDetailService = customerDocumentDetailService;
			this._customerService = customerService;
			this._customerRelation = customerRelation;
			this._customerDocumentTypeService = customerDocumentTypeService;
			this._numberRangeService = numberRangeService;
		}

		public ActionResult Index(string culture = "en-ae")
		{
			ViewBag.CustomerDocumentTypeID = new SelectList(_customerDocumentTypeService.GetDocumentTypesForDropDown(), "value", "text");
			ViewBag.CustomerRelationID = new SelectList(_customerRelation.GetCustomerRelationForDropDown(culture.Split('-')[0], true), "value", "text");

			ViewBag.PersonalID = _customerRelation.GetCustomerRelations().FirstOrDefault(x => x.Relation == "Personal");
			return View();
		}

		public ActionResult GetDocuments(string culture = "en-ae")
		{
			var documents = _customerDocumentService.GetCustomerDocumentsByCustomerID(CustomerSessionHelper.ID);

			string message = "";
			object data = null;
			bool status = false;

			if (documents.Count() > 0)
			{
				status = true;
				message = "Data retrieved successfully ...";
				data = documents.Select(i => new
				{
					i.ID,
					i.Path,
					i.DocumentNo,
					i.IsFamily,
					Type = culture == "en-ae" ? i.CustomerDocumentType.Title : i.CustomerDocumentType.TitleAr,
					Relation = culture == "en-ae" ? i.CustomerRelation.Relation : i.CustomerRelation.RelationAr,
					ExpiryDate = i.ExpiryDate.HasValue ? i.ExpiryDate.Value.ToString(CustomHelper.GetDateFormat2) : "",
				});
			}
			else
			{
				message = "No data found ...";
			}
			return Json(new { success = status, message, data }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CustomerDocument customerDocument)
		{
			string message = string.Empty;
			string description = string.Empty;
			object data = null;
			bool status = false;
			bool? isFamily = false;

			if (ModelState.IsValid)
			{
				long CustomerID = CustomerSessionHelper.ID;

				try
				{
					customerDocument.CustomerID = CustomerID;
					customerDocument.DocumentNo = _numberRangeService.GetNextValueFromNumberRangeByName("DOCUMENT");
					CustomerRelation customerRelation = _customerRelation.GetCustomerRelation((long)customerDocument.CustomerRelationID);
					var customerdocs = _customerDocumentTypeService.GetCustomerDocumentType((long)customerDocument.CustomerDocumentTypeID);
					customerDocument.IsFamily = customerRelation.Relation == "Personal" ? false : true;

					HttpFileCollectionBase files = Request.Files;
					if (files.Count > 0)
					{
						string absolutePath = Server.MapPath("~");
						string relativePath = string.Format("/Assets/AppFiles/CustomerDocument/{0}/", customerdocs.Title.Replace(" ", "_"));
						//data.Type = Type;
						//data.TypeAr = TypeAr;

						customerDocument.Path = Uploader.UploadDocs(files, absolutePath, relativePath, "Document", ref message, "Path");
						customerDocument.Path = CustomURL.GetImageServer() + customerDocument.Path.Remove(0, 1);
						if (_customerDocumentService.CreateCustomerDocument(ref customerDocument, ref message))
						{
							isFamily = customerDocument.IsFamily;
							status = true;
						}
					}
					else
					{
						message = "No document selected! Please upload document first ...";
					}
				}
				catch (Exception)
				{
					message = "Something went wrong! Please try later.";
				}
			}
			else
			{
				message = "Please fill the form properly!";
				description = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
			}

			return Json(new { success = status, message, description, data, isFamily }, JsonRequestBehavior.AllowGet);
		}

		//[HttpGet]
		//public ActionResult GetDocuments(string culture)
		//{
		//	string lang = "en";
		//	if (culture.Contains('-'))
		//		lang = culture.Split('-')[0];

		//	var documents = _customerDocumentService.GetCustomerDocuments().Where(x => x.IsDeleted == false);

		//	if (documents.Count() > 0)
		//	{
		//		return Json(new
		//		{
		//			success = true,
		//			message = "Data retrieved successfully !",
		//			data = documents.Select(i => new
		//			{
		//				i.ID,
		//				i.Path,
		//				//Type = lang == "en" ? i.Type : i.TypeAr,
		//			})
		//		}, JsonRequestBehavior.AllowGet);
		//	}
		//	else
		//	{
		//		return Json(new { success = false, message = "No result found !" }, JsonRequestBehavior.AllowGet);
		//	}
		//}

		[HttpGet]
		public ActionResult GetMyDocuments()
		{
			long CustomerId = CustomerSessionHelper.ID;

			var document = _customerDocumentDetailService.GetCustomerDocumentDetailsByCustomerID(CustomerId).OrderByDescending(x => x.ID).ToList();

			if (document.Count() > 0)
			{
				return Json(new
				{
					success = true,
					message = "Data retrieved successfully !",
					data = document.Select(i => new
					{
						i.ID,
						i.ServiceNo,
						//i.CustomerDocument.Type,
						i.Status,
						i.Path,
						Remarks = !string.IsNullOrEmpty(i.Reason) ? i.Reason : "No remarks yet."
						//Remarks = HTMLTagsSplitter.ToSplitList(i.Reason)
					})
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { success = false, message = "No result found !" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Upload(DocumentDetailsViewModel documentsViewModel)
		{
			string message = string.Empty;
			string status = string.Empty;
			string path = string.Empty;
			try
			{
				if (ModelState.IsValid)
				{
					Int64 CustomerId = Convert.ToInt64(Session["CustomerID"].ToString());

					if (Request.Files.Count > 0)
					{
						string absolutePath = Server.MapPath("~");
						string relativePath = string.Format("/Assets/AppFiles/CustomerDocument/{0}/", CustomerId.ToString().Replace(" ", "_"));
						path = Uploader.UploadDocs(Request.Files, absolutePath, relativePath, "Document", ref message, "Path");

						if (string.IsNullOrEmpty(message))
						{
							CustomerDocumentDetail documentDetail = new CustomerDocumentDetail();
							documentDetail.CustomerID = CustomerId;
							documentDetail.CustomerDocumentID = documentsViewModel.ID;
							documentDetail.Path = path;
							if (_customerDocumentDetailService.CreateCustomerDocumentDetail(documentDetail, ref message))
							{
								return Json(new
								{
									success = true,
									message = "Your document uploaded successfullly!",
									//data = new
									//{
									//	documentDetail.ID,
									//	documentDetail.ServiceNo,
									//	documentDetail.CustomerDocument.Type,
									//	documentDetail.Path,
									//	documentDetail.Status,
									//}
								});
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = message, error = ex.Message });
			}
			return Json(new { success = false, message = message });
		}

		[HttpPost]
		public ActionResult Cancel(long id)
		{
			try
			{
				string message = string.Empty;
				string status = string.Empty;

				var customerDocumentDetail = _customerDocumentDetailService.GetCustomerDocumentDetail(id);
				customerDocumentDetail.Status = "Cancelled";
				customerDocumentDetail.Reason = "Document is cancelled by the customer.";

				if (_customerDocumentDetailService.UpdateCustomerDocumentDetail(ref customerDocumentDetail, ref message))
				{
					return Json(new
					{
						success = true,
						message = "Document cancelled successfully."
					}, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new
					{
						success = false,
						message = "Something went wrong! Please try later.",
					}, JsonRequestBehavior.AllowGet);
				}
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = "Something went wrong! Please try later.",
				}, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public ActionResult Delete(long id)
		{
			string message = string.Empty;
			string filePath = string.Empty;
			bool status = false;
			try
			{
				CustomerDocument document = _customerDocumentService.GetCustomerDocument(id);
				if (document == null)
				{
					message = "Document not found !";
				}

				if (_customerDocumentService.DeleteCustomerDocument(id, ref message, ref filePath, true, true))
				{
					filePath = filePath.Replace(CustomURL.GetImageServer(), "");
					//delete file
					if (System.IO.File.Exists(Server.MapPath(filePath)))
					{
						System.IO.File.Delete(Server.MapPath(filePath));
					}
					status = true;
				}
				else
				{
					message = "Something went wrong! Please try later.";
				}
			}
			catch (Exception ex)
			{
				message = "Something went wrong! Please try later.";
			}
			return Json(new { success = status, message }, JsonRequestBehavior.AllowGet);
		}

	}
}