using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Controllers;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Web.Areas.CustomerPortal.Controllers
{
	[AuthorizeCustomer]

	public class ProjectController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly ICountryService _countryService;
		private readonly ICityService _cityService;
		private readonly IAreaService _areaService;

		public ProjectController(IAreaService areaService, ICountryService countryService, ICityService cityService)
		{
			this._countryService = countryService;
			this._cityService = cityService;
			this._areaService = areaService;
		}

		public ActionResult Index()
		{
			return View();
		}

	}
}