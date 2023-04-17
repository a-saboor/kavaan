using MyProject.Data;
using MyProject.Service;
using MyProject.Web.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Web.Controllers
{
	public class MobileViewController : Controller
	{

		public MobileViewController()
		{
		}

		[Route("about-us-mobile", Name = "about-us-mobile")]
		public ActionResult AboutUs()
		{
			return View();
		}

		[Route("terms-and-conditions-mobile", Name = "terms-and-conditions-mobile")]
		public ActionResult TermsAndConditions()
		{
			return View();
		}

		[Route("privacy-policy-mobile", Name = "privacy-policy-mobile")]
		public ActionResult PrivacyPolicy()
		{
			return View();
		}

		[Route("disclaimer-mobile", Name = "disclaimer-mobile")]
		public ActionResult Disclaimer()
		{
			return View();
		}

		[Route("frequently-asked-questions-mobile", Name = "frequently-asked-questions-mobile")]
		public ActionResult FAQs()
		{
			return View();
		}
	}
}