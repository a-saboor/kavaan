using MyProject.Data;
using MyProject.Service;
using MyProject.Web.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MyProject.Web.ViewModels;

namespace MyProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IAreaService _areaService;
        private readonly IContentManagementService _contentManagementService;
        private readonly IBannerService _bannerService;
        private readonly IContractorService _contractorService;
        private readonly IBlogService _blogService;
        private readonly INewsFeedService _newsFeedService;
        private readonly ITeamService _teamService;
        private readonly IAmenityService _amenityService;
        private readonly IProjectTypeService _projectTypeService;
        private readonly IIntroductionSettingService _introductionSettingService;

        public HomeController(
            IAreaService areaService
            , ICountryService countryService
            , IContentManagementService contentManagementService
            , ICityService cityService
            , IBannerService bannerService
            , IContractorService contractorService
            , IBlogService blogService
            , INewsFeedService newsFeedService
            , ITeamService teamService
            , IAmenityService amenityService
            , IProjectTypeService projectTypeService
            , IIntroductionSettingService introductionSettingService
            )
        {
            this._countryService = countryService;
            this._cityService = cityService;
            this._areaService = areaService;
            this._contentManagementService = contentManagementService;
            this._bannerService = bannerService;
            this._contractorService = contractorService;
            this._blogService = blogService;
            this._newsFeedService = newsFeedService;
            this._teamService = teamService;
            this._amenityService = amenityService;
            this._projectTypeService = projectTypeService;
            this._introductionSettingService = introductionSettingService;
        }

        public ActionResult SetCulture(string culture, string ReturnUrl)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
                cookie = new HttpCookie("_culture");
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
                cookie.Value = culture;
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
                cookie.Expires = DateTime.Now.AddYears(1);
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
            }
            Response.Cookies.Add(cookie);
            RouteData.Values["culture"] = culture;  // set culture

            var url = ReturnUrl.Split('/');
            string NewUrl = "~/" + cookie.Value;

            for (int i = 2; i < url.Length; i++)
            {
                NewUrl += "/" + url[i];
            }

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return RedirectPermanent(NewUrl);

            //// Validate input
            //culture = CultureHelper.GetImplementedCulture(culture);
            //RouteData.Values["culture"] = culture;  // set culture

            //return RedirectToAction("Index");
        }

        public ActionResult LanguageSelector(string culture = "en-ae")
        {
            return View();
        }

        //[Route("home/index", Name = "home/index")]
        //[Route("home", Name = "home")]
        //[Route("index", Name = "index")]
        //[Route("", Name = "")]
        public ActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                HeaderVideo = _bannerService.GetBannersByContentTypeAndDeviceAndType("Video", "Website", "Header").FirstOrDefault(),
                CenteredVideo = _bannerService.GetBannersByContentTypeAndDeviceAndType("Video", "Website", "Centered").FirstOrDefault(),
                FooterVideo = _bannerService.GetBannersByContentTypeAndDeviceAndType("Video", "Website", "Footer").FirstOrDefault(),

                HeaderBanners = _bannerService.GetBannersByContentTypeAndDeviceAndType("Image", "Website", "Header").ToList(),
                CenteredBanners = _bannerService.GetBannersByContentTypeAndDeviceAndType("Image", "Website", "Centered").FirstOrDefault(),
                FooterBanners = _bannerService.GetBannersByContentTypeAndDeviceAndType("Image", "Website", "Footer").FirstOrDefault(),

                ProjectTypes = _projectTypeService.GetProjectTypes().Take(5).OrderBy(x=>x.Name).ToList(),

                Contractors = _contractorService.GetAll().Where(x => x.IsActive == true).ToList(),

                Amenities = _amenityService.GetAmenites().Where(x => x.IsActive == true).ToList(),

                Blogs = _blogService.GetBlog().Where(x => x.IsActive == true).OrderByDescending(x => x.ID).Take(8).ToList(),
                NewsFeeds = _newsFeedService.GetNewsFeed().Where(x => x.IsActive == true).OrderByDescending(x => x.ID).Take(8).ToList(),
            };

            //var HeaderVideo = _bannerService.GetBannersByContentTypeAndDeviceAndType("Video", "Website", "Header").FirstOrDefault();
            //var FooterVideo = _bannerService.GetBannersByContentTypeAndDeviceAndType("Video", "Website", "Footer").FirstOrDefault();
            //var HeaderBanners = _bannerService.GetBannersByContentTypeAndDeviceAndType("Image", "Website", "Header");
            //var FooterBanners = _bannerService.GetBannersByContentTypeAndDeviceAndType("Image", "Website", "Footer");

            return View(homeViewModel);
        }

        [Route("about-us", Name = "about-us")]
        public ActionResult AboutUs()
        {
            AboutViewModel aboutViewModel = new AboutViewModel()
            {
                AboutImage = _bannerService.GetBannersByContentTypeAndDeviceAndType("Image", "Website", "About").FirstOrDefault(),
                AboutVideo = _bannerService.GetBannersByContentTypeAndDeviceAndType("Video", "Website", "About").FirstOrDefault(),

                Teams = _teamService.GetTeams().Where(x => x.IsActive == true).ToList(),
            };

            return View(aboutViewModel);
        }

        [Route("terms-and-conditions", Name = "terms-and-conditions")]
        public ActionResult TermsAndConditions()
        {
            return View();
        }

        [Route("privacy-policy", Name = "privacy-policy")]
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        [Route("disclaimer", Name = "disclaimer")]
        public ActionResult Disclaimer()
        {
            return View();
        }

        [Route("frequently-asked-questions", Name = "frequently-asked-questions")]
        public ActionResult FAQs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetCountries(long? countryId, string culture = "en-ae")
        {
            string lang = "en";
            if (culture.Contains('-'))
                lang = culture.Split('-')[0];

            var countries = new SelectList(_countryService.GetCountriesForDropDown(lang), "value", "text", countryId);

            return Json(new
            {
                success = true,
                message = "data retreived successfully.",
                data = countries
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCites(long countryId, long? cityId, string culture = "en-ae")
        {
            string lang = "en";
            if (culture.Contains('-'))
                lang = culture.Split('-')[0];

            var cities = new SelectList(_cityService.GetCitiesForDropDown(countryId, lang), "value", "text", cityId);

            return Json(new
            {
                success = true,
                message = "data retreived successfully.",
                data = cities
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetAreas(long cityId, long? areaId, string culture = "en-ae")
        {
            string lang = "en";
            if (culture.Contains('-'))
                lang = culture.Split('-')[0];

            var areas = new SelectList(_areaService.GetAreasForDropDown(cityId, lang), "value", "text", areaId);

            return Json(new
            {
                success = true,
                message = "data retreived successfully.",
                data = areas
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("content-setting", Name = "content-setting")]
        public ActionResult ContentSetting(string culture = "en-ae")
        {
            try
            {
                var content = _contentManagementService.GetEnableContent();
                return Json(new
                {
                    success = true,
                    message = "Data recieved successfully!",
                    data = content
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}