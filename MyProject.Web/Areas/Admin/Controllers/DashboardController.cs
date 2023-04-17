using MyProject.Service;
using MyProject.Web.Areas.Admin.ViewModels;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Helpers;
using System;
using System.Web.Mvc;

namespace MyProject.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class DashboardController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private readonly IUserService _userService;
		private readonly ICategoryService _departmentService;
		private readonly IJobOpeningService _jobOpeningService;
		private readonly IJobCandidateService _jobCandidateService;
		private readonly ISPService _spService;
		public DashboardController(IUserService userService, ICategoryService departmentService, IJobOpeningService jobOpeningService, IJobCandidateService jobCandidateService, ISPService spService)
		{
			this._userService = userService;
			this._departmentService = departmentService;
			this._jobOpeningService = jobOpeningService;
			this._jobCandidateService = jobCandidateService;
			this._spService = spService;
		}

		public ActionResult Index()
		{
			ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
			ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-365).ToString("MM/dd/yyyy");
			DateTime ed= Helpers.TimeZone.GetLocalDateTime() ;
			DateTime sd= Helpers.TimeZone.GetLocalDateTime().AddDays(-365);
			DashboardStatsViewModel ObjDashboardStatsViewModel = new DashboardStatsViewModel()
			{
				Stats = _spService.GetAdmingDashboardStats(sd, ed),
				BookingGraph = _spService.GetBookingGraphByDateRange(sd, ed)
				//RevenueGraph = _spService.GetYearlyRevenueGraphChart()
			};
			return View(ObjDashboardStatsViewModel);
		}


		[HttpPost]
		public ActionResult Filter(DateTime fromDate, DateTime toDate)
		{
			toDate = AdminCustomHelper.GetToDate(toDate);

			var stats = _spService.GetAdmingDashboardStats(fromDate, toDate);
			var booking = _spService.GetBookingGraphByDateRange(fromDate, toDate);

			return Json(new { success = true, data = new { stats, booking } }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult RevenueGraph()
		{
			var Data = _spService.GetYearlyRevenueGraphChart();

			return Json(new { success = true, message = "Data received successfully!", data = Data }, JsonRequestBehavior.AllowGet);
		}

	}
}