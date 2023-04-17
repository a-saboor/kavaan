using MyProject.Data;
using MyProject.Service;
using MyProject.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Web.Controllers
{
	public class SubscribersController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly ISubscribersService _subscribersService;

		public SubscribersController(ISubscribersService subscribersService)
		{
			this._subscribersService = subscribersService;
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		//[Route("subscribers")]
		[Route("subscribers", Name = "subscribers")]
		public ActionResult Create(Subscriber subscriber)
		{
			string message = string.Empty;
			bool status = false;

			if (ModelState.IsValid)
			{
				try
				{
					if (_subscribersService.CreateSubscriber(ref subscriber, ref message))
					{
						status = true;
					}
					else
					{
						status = false;
					}
				}
				catch (Exception ex)
				{
					status = false;
					message = "";
				}
			}
			else
			{
				status = false;
				message = "Invalid Email Address";
			}

			return Json(new { success = status, message = message }, JsonRequestBehavior.AllowGet);
		}

	}
}