using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Web.Controllers
{
	public class ErrorController : Controller
	{
		public ErrorController()
		{
		}

		// GET: Error
		[Route("Error/PageNotFound", Name = "PageNotFound")]
		[Route("page-not-found", Name = "page-not-found")]
		public ActionResult PageNotFound()
		{
			return View();
		}
		
		[Route("Error/InternalServerError", Name = "InternalServerError")]
		[Route("internal-server-error", Name = "internal-server-error")]
		public ActionResult InternalServerError()
		{
			return View();
		}

	}
}