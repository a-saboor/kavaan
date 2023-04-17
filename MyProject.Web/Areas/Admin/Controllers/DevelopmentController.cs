using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider; 
using Project.Web.Helpers;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class DevelopmentController : Controller
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;
		private readonly IDevelopmentService developmentService;
		private readonly IPropertyService _propertyService;
		public DevelopmentController(IDevelopmentService developmentService, IPropertyService propertyService)
		{
			this.developmentService = developmentService;
			this._propertyService = propertyService;
		}
		// GET: Admin/Contractors
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult List()
		{
			var result = developmentService.GetAll();
			return PartialView(result);
		}
		// GET: Admin/Contractors/Details/5
		public ActionResult Details(long id)
		{
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Development development = this.developmentService.Edit(id);
			if (development == null)
			{
				return HttpNotFound();
			}
			return View(development);
		}

		// GET: Admin/Contractors/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Admin/Contractors/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ID,Name,NameAr,Image,Description,DescriptionAr,IsApproved")] Development development)
		{
			string message = string.Empty;

			if (ModelState.IsValid)
			{
				if (development.Image != null)
				{

					string nameappend = development.Name.Replace(" ", "-");
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Development/Image/{0}/", nameappend);
					development.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
				}
				if (this.developmentService.Create(development, ref message))
				{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created development {development.Name}.");
					return Json(new
					{
						success = true,
						url = "/Admin/Development/Index",
						message = message,
						data = new
						{
							CreatedOn = development.CreatedOn.ToString("dd MMM yyyy, h:mm tt"),
							Name = development.Name,
							IsApproved = development.IsApproved.ToString(),
							IsActive = development.IsActive.ToString(),
							ID = development.ID
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

		// GET: Admin/Contractors/Edit/5
		public ActionResult Edit(long id)
		{
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Development development = this.developmentService.Edit(id);
			if (development == null)
			{
				return HttpNotFound();
			}
			return View(development);
		}

		// POST: Admin/Contractors/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,Name,NameAr,Image,Description,DescriptionAr,IsApproved,IsActive")] Development development)
		{
			string message = string.Empty;

			if (ModelState.IsValid)
			{

				string nameappend = development.Name.Replace(" ", "-");
				Development currentdevelopment = this.developmentService.Edit(development.ID);
				currentdevelopment.Name = development.Name;
				currentdevelopment.NameAr = development.NameAr;
				currentdevelopment.Description = development.Description;
				currentdevelopment.DescriptionAr = development.DescriptionAr;
				currentdevelopment.IsApproved = development.IsApproved;
				if (development.Image != null)
				{

					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Development/Image/{0}/", nameappend);
					if (currentdevelopment.Image != null)
					{

						if (System.IO.File.Exists(absolutePath + currentdevelopment.Image))
						{
							System.IO.File.Delete(absolutePath + currentdevelopment.Image);
						}
					}
					currentdevelopment.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
				}

				if (this.developmentService.Edit(ref currentdevelopment, ref message))
				{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated development {currentdevelopment.Name}");
					return Json(new
					{
						success = true,
						url = "/Admin/Development/Index/",
						message = message,
						data = new
						{
							CreatedOn = currentdevelopment.CreatedOn.ToString("dd MMM yyyy, h:mm tt"),
							Name = currentdevelopment.Name,
							IsApproved = currentdevelopment.IsApproved.ToString(),
							IsActive = currentdevelopment.IsActive.ToString(),
							ID = currentdevelopment.ID
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

		// GET: Admin/Contractors/Delete/5
		public ActionResult Delete(long id)
		{
			string messsage = string.Empty;
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			bool Development = this.developmentService.Delete(id, ref messsage, true); ;
			if (!Development)
			{
				return HttpNotFound();
			}
			return View(Development);
		}

		// POST: Admin/Contractors/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id)
		{
			string message = string.Empty;
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			bool Development = this.developmentService.Delete(id, ref message, true); ;
			if (Development)
			{
				//activate or deactivate the projects of this development:
				string propertyMessage = string.Empty;
				this._propertyService.DeleteProperties(id, ref propertyMessage, true);
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted development ID: {id}");

				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);


		}
		public ActionResult Activate(long? id)
		{
			if (id == null || id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var Development = this.developmentService.Edit((long)id);
			if (Development == null)
			{
				return HttpNotFound();
			}

			string message = string.Empty;
			if (!(bool)Development.IsActive)
				Development.IsActive = true;
			else
			{
				Development.IsActive = false;
			}

			if (this.developmentService.Edit(ref Development, ref message))
			{
				//activate or deactivate the projects of this development:
				string propertyMessage = string.Empty;
				_propertyService.ActivateProperties(Development.ID, ref propertyMessage, Development.IsActive);

				SuccessMessage = "Development " + ((bool)Development.IsActive ? "activated" : "deactivated") + "  successfully ...";

				if ((bool)Development.IsActive)
				{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} activated development {Development.Name}");
				}
				else
				{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deactivated development {Development.Name}");
				}

				return Json(new
				{
					success = true,
					message = SuccessMessage,
					data = new
					{
						CreatedOn = Development.CreatedOn.ToString("dd MMM yyyy, h:mm tt"),
						Name = Development.Name,
						IsApproved = Development.IsApproved.ToString(),
						IsActive = Development.IsActive.ToString(),
						ID = Development.ID
					}
				}, JsonRequestBehavior.AllowGet); ;
			}
			else
			{
				message = "Oops! Something went wrong. Please try later.";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}

	}
}
