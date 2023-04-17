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
	public class CustomerDocumentTypeController : Controller
	{
		// GET: Admin/CustomerDocument
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly ICustomerDocumentTypeService _customerDocumentTypeService;
		public CustomerDocumentTypeController(ICustomerDocumentTypeService customerDocumentTypeService)
		{
			this._customerDocumentTypeService = customerDocumentTypeService;
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult List()
		{
			var result = _customerDocumentTypeService.GetCustomerDocumentType();
			return PartialView(result);
		}



		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]

		public ActionResult Create(CustomerDocumentType data)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				CustomerDocumentType customerDocumentType = new CustomerDocumentType();
				customerDocumentType.Title = data.Title;
				customerDocumentType.TitleAr = data.TitleAr;
				if (_customerDocumentTypeService.CreateCustomerDocumentType(ref customerDocumentType, ref message))
				{
					//log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created customer document {customerDocumentType.DocumentNo}.");
					return Json(new
					{
						success = true,
						url = "/Admin/CustomerDocumentType/Index",
						message = message,
						data = new
						{
							CreatedOn = customerDocumentType.CreatedOn.HasValue ? customerDocumentType.CreatedOn.Value.ToString(CustomHelper.GetDateFormat) : "-",
                            Title = customerDocumentType.Title,
							TitleAr = customerDocumentType.TitleAr,
                            IsActive = customerDocumentType.IsActive.ToString(),
                            ID = customerDocumentType.ID
                        },
					});
				}


			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Details(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			CustomerDocumentType customerDocumentType = _customerDocumentTypeService.GetCustomerDocumentType((Int16)id);
			if (customerDocumentType == null)
			{
				return HttpNotFound();
			}
			return View(customerDocumentType);
		}
		public ActionResult Edit(long id)
		{
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			CustomerDocumentType customerDocumentType = _customerDocumentTypeService.GetCustomerDocumentType(id);
			if (customerDocumentType == null)
			{
				return HttpNotFound();
			}
			return View(customerDocumentType);
		}

		[HttpPost]
		public ActionResult Edit(CustomerDocumentType data)
		{
			string message = string.Empty;
			string replacement = Guid.NewGuid().ToString();
			if (ModelState.IsValid)
			{
				
				CustomerDocumentType currentCustomerDocumentType = _customerDocumentTypeService.GetCustomerDocumentType(data.ID);
				currentCustomerDocumentType.Title = data.Title;
				currentCustomerDocumentType.TitleAr = data.TitleAr;
				if (_customerDocumentTypeService.UpdateCustomerDocumentType(ref currentCustomerDocumentType, ref message))
				{
					//log.Info($"{Session["UserName"]} | {Session["Email"]} udpated customer document {currentCustomerDocument.DocumentNo}.");
					return Json(new
					{
						success = true,
						url = "/Admin/CustomerDocumentType/Index",
						message = message,
						data = new
						{
							CreatedOn = currentCustomerDocumentType.CreatedOn.HasValue ? currentCustomerDocumentType.CreatedOn.Value.ToString(CustomHelper.GetDateFormat) : "-",
							Title = currentCustomerDocumentType.Title,
							TitleAr = currentCustomerDocumentType.TitleAr,
							IsActive = currentCustomerDocumentType.IsActive.ToString(),
							ID = currentCustomerDocumentType.ID
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
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			bool customerDocumentType = _customerDocumentTypeService.DeleteCustomerDocumentType(id, ref message, true); ;
			if (customerDocumentType)
			{
				//log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted customer document ID: {id}.");
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

            var customerDocumentType = _customerDocumentTypeService.GetCustomerDocumentType((long)id);
            if (customerDocumentType == null)
            {
                return HttpNotFound();
            }
            string message = string.Empty;
            if (!(bool)customerDocumentType.IsActive)
            {
				customerDocumentType.IsActive = true;
                //log.Info($"{Session["UserName"]} | {Session["Email"]} activated customer document {customerDocument.Type}.");
            }
            else
            {
				customerDocumentType.IsActive = false;
                //log.Info($"{Session["UserName"]} | {Session["Email"]} deactivated customer document {customerDocument.Type}.");
            }
            if (_customerDocumentTypeService.UpdateCustomerDocumentType(ref customerDocumentType, ref message))
            {
                SuccessMessage = "Document " + ((bool)customerDocumentType.IsActive ? "activated" : "deactivated") + "  successfully ...";
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
						CreatedOn = customerDocumentType.CreatedOn.HasValue ? customerDocumentType.CreatedOn.Value.ToString(CustomHelper.GetDateFormat) : "-",
						Title = customerDocumentType.Title,
						TitleAr = customerDocumentType.TitleAr,
						IsActive = customerDocumentType.IsActive.ToString(),
						ID = customerDocumentType.ID
					}
                }, JsonRequestBehavior.AllowGet); ;
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later...";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }
    }
}