using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Controllers;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Web.Areas.CustomerPortal.Controllers
{
	[AuthorizeCustomer]

	public class DashboardController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly ICountryService _countryService;
		private readonly ICityService _cityService;
		private readonly IAreaService _areaService;

		public DashboardController(IAreaService areaService, ICountryService countryService, ICityService cityService)
		{
			this._countryService = countryService;
			this._cityService = cityService;
			this._areaService = areaService;
		}

		public ActionResult Index()
		{
			//ViewBag.CountryID = new SelectList(_countryService.GetCountriesForDropDown(), "value", "text");
			//ViewBag.CityID = new SelectList(_cityService.GetCitiesForDropDown(), "value", "text");
			//ViewBag.AreaID = new SelectList(_areaService.GetAreasForDropDown(), "value", "text");

			//HttpCookie cookie = Request.Cookies["_bookingUrl"];
			//if (cookie != null)
			//{
			//	var url = cookie.Value.Replace("%2F", "/");
			//	return RedirectPermanent(url);
			//}

			return View();
		}

		[HttpPost]
		public ActionResult GetCites(long countryId, string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			var cities = new SelectList(_cityService.GetCitiesForDropDown(countryId, lang), "value", "text");

			return Json(new
			{
				success = true,
				message = "data retreived successfully.",
				data = cities
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult GetAreas(long cityId, string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			var areas = new SelectList(_areaService.GetAreasForDropDown(cityId, lang), "value", "text");

			return Json(new
			{
				success = true,
				message = "data retreived successfully.",
				data = areas
			}, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetSession()
		{
			return Json(new { success = true }, JsonRequestBehavior.AllowGet);
		}

	}
}