using Project.Data;
using Project.Service;
using Project.Service.Helpers;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using Project.Web.Helpers.Routing;
using Project.Web.ViewModels.Vendor;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Globalization;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class VendorDocumentsController : Controller
    {
		private readonly IVendorDocumentService _vendorDocumentService;
		// GET: Admin/VendorDocuments
		string ImageServer = Helpers.Routing.CustomURL.GetImageServer();

		public VendorDocumentsController(IVendorDocumentService vendorDocumentService)
		{

			this._vendorDocumentService = vendorDocumentService;
		}
		public ActionResult Index(long? id)
		{
			ViewBag.VendorID = id;
			Session["VendorIDForDocument"] = ViewBag.VendorID;
			return View();

		}

		[HttpPost]
		public ActionResult CreateDocuments(string Name, DateTime? ExpiryDate)
		{
			if (Name != string.Empty)
			{
				long vendorId = (long)Session["VendorIDForDocument"];

				string message = string.Empty;
				VendorDocument data = new VendorDocument();
				data.VendorID = vendorId;
				string absolutePath = Server.MapPath("~");
				string relativePath = string.Format("/Assets/AppFiles/Documents/Vendors/{0}/", vendorId.ToString().Replace(" ", "_"));
				data.Name = Name;
				data.ExpiryDate = ExpiryDate;
				data.Path = Uploader.UploadDocs(Request.Files, absolutePath, relativePath, "Document", ref message, "FileUpload");

				if (Request.Files.Count == 0)
				{
					return Json(new
					{
						success = false,
						message = "Please fill the form correctly",


					}, JsonRequestBehavior.AllowGet);
				}
				if (_vendorDocumentService.CreateDocument(ref data, ref message))
				{

				}

				return Json(new
				{
					success = true,
					message = "Document added successfully!",
					name = data.Name,
					expirydate = data.ExpiryDate,
					path = ImageServer + data.Path,
					id = data.ID

				}, JsonRequestBehavior.AllowGet); ;
			}
			else
			{
				return Json(new
				{
					success = false,
					message = "Please fill the form correctly!",


				}, JsonRequestBehavior.AllowGet); ;
			}

		}

		[HttpPost, ActionName("DeleteVendorDocument")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteCarDocument(long id)
		{

			string message = string.Empty;
			_vendorDocumentService.DeleteDocument(id, ref message);
			return Json(new { success = true, data = id, message });
		}

		[HttpGet]
		public ActionResult GetDocuments(long id)
		{
			var document = _vendorDocumentService.GetDocumentByVendorID(id);


			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				document = document.Select(i => new {
					name = i.Name,
					expirydate = i.ExpiryDate.HasValue ? i.ExpiryDate.Value.ToString("dd MMM yyyy") : "-",
					path = ImageServer + i.Path,
					id = i.ID
				})

			}, JsonRequestBehavior.AllowGet);
		}
	}
}