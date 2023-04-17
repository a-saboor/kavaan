using MyProject.Data;
using MyProject.Service;
using MyProject.Web.Helpers;
using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Web.Controllers{
	public class OzoneController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IOzoneService _ozoneService;

		public OzoneController(IOzoneService ozoneService)
		{
			this._ozoneService = ozoneService;
		}

		[Route("ozone", Name = "ozone")]
		public ActionResult Index()
		{
			Ozone ozone = _ozoneService.GetOzonefirstordefault();
			if (ozone == null)
				ozone = new Ozone();

			return View(ozone);
		}

	}
}